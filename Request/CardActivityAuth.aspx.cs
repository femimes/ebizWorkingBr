using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using iBankPoints;
using WebRefVanzo;

/// <summary>
/// Verify card maintenance (?RefNo=MNT...): the approving officer confirms a maintenance batch with a hardware-token code
/// (WebRefVanzo two-factor service). Each card in the batch is then sent to the card switch (ICardProcess):
/// 100004 block (status only), 100005 unblock, 100006 unlink, 100007 PIN re-issue, 100008 link.
/// When every card succeeds the batch is set to status 3 (Sproc_UpdateCardMaintenance) and the officer goes back
/// to the approvals list. Plain ASP.NET controls (no DevExpress).
/// </summary>
public partial class Request_CardActivityAuth : System.Web.UI.Page
{
    private const string ApprovalsPage = "../Approvals/ApproveCardMaint.aspx";

    private string RefNo { get { return (Request.QueryString["RefNo"] ?? "").Trim(); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadDetails();
    }

    // =====================================================================
    // Officer and data helpers
    // =====================================================================

    private string Identity()
    {
        string id = User != null && User.Identity != null ? User.Identity.Name : "";
        if (string.IsNullOrEmpty(id) && Request.Cookies["LogonUserID"] != null) id = Request.Cookies["LogonUserID"].Value;
        return id ?? "";
    }

    private string CurrentUser()
    {
        string id = Identity();
        int split = id.LastIndexOf('?');
        return split >= 0 ? id.Substring(0, split) : id;
    }

    private string CurrentBranch()
    {
        string id = Identity();
        int split = id.LastIndexOf('?');
        return split >= 0 ? id.Substring(split + 1) : "";
    }

    private static string ConnectionString(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    private static DataTable Query(string connectionName, string sql, CommandType type, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(ConnectionString(connectionName)))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandType = type;
            cmd.Parameters.AddRange(parameters);
            adapter.Fill(table);
        }
        return table;
    }

    private static string Col(DataRow row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column) || row.IsNull(column)) return "";
        return Convert.ToString(row[column]);
    }

    private void ShowError(string message)
    {
        litEmmorMsg.Text = HttpUtility.HtmlEncode(message);
    }

    // =====================================================================
    // Request details
    // =====================================================================

    private static bool IsSecret(string column)
    {
        string c = column.ToLowerInvariant();
        return c.Contains("cvv") || c.EndsWith("pin") || c.Contains("cardpin");
    }

    private void LoadDetails()
    {
        lblwebRefNo.Text = HttpUtility.HtmlEncode(RefNo);
        List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
        try
        {
            if (RefNo != "")
            {
                DataTable t = Query("EazyConnect", "EXEC SpGetBatRecCardMaintenance @RefNo", CommandType.Text, new SqlParameter("@RefNo", RefNo));
                if (t.Rows.Count > 0)
                {
                    foreach (DataColumn c in t.Columns)
                    {
                        if (IsSecret(c.ColumnName)) continue;
                        items.Add(new KeyValuePair<string, string>(c.ColumnName, Col(t.Rows[0], c.ColumnName)));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("The request details could not be loaded: " + ex.Message);
        }
        rptDetails.DataSource = items;
        rptDetails.DataBind();
        rptDetails.Visible = items.Count > 0;
        phNoDetails.Visible = items.Count == 0;
        btnConfirm.Enabled = items.Count > 0;
    }

    // =====================================================================
    // Confirm
    // =====================================================================

    /// <summary>Card account type for the switch, from the account's product category in core banking.</summary>
    private static string AccountType(string accountNo)
    {
        string prodType = "1";
        foreach (DataRow r in Query("EazyConnectIBank", "SpAcctMasterSelectOne", CommandType.StoredProcedure, new SqlParameter("@AccountNo", accountNo)).Rows)
            prodType = Col(r, "ProdCatCode");
        switch (prodType)
        {
            case "1": return "20";
            case "2": return "10";
            case "3":
            case "4": return "50";
            case "6": return "30";
            default: return "10";
        }
    }

    /// <summary>True when the switch reply has "success": true.</summary>
    private static bool SwitchSucceeded(string reply, out string message)
    {
        message = "";
        try
        {
            Dictionary<string, object> json = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(reply);
            foreach (string k in new[] { "message", "responseMessage", "description", "error" })
                if (json != null && json.ContainsKey(k) && json[k] != null) { message = Convert.ToString(json[k]); break; }
            bool ok;
            return json != null && json.ContainsKey("success") && bool.TryParse(Convert.ToString(json["success"]), out ok) && ok;
        }
        catch (Exception)
        {
            message = "unreadable reply from the card switch";
            return false;
        }
    }

    private void SetBatchStatus(int status)
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand("Sproc_UpdateCardMaintenance", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@eBatRefNo", RefNo));
            cmd.Parameters.Add(new SqlParameter("@status", status));
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid) return;
        if (RefNo == "") { ShowError("No request reference was given."); return; }

        string user = CurrentUser();
        bool allDone = false;
        try
        {
            // switch credentials from the preferences
            string clientId2FA = "", iswClientId = "", iswClientKey = "", iswBankCode = "";
            foreach (DataRow r in NETClass.Classes.SqlDbGetDataset("Sproc_GetPref").Tables[0].Rows)
            {
                clientId2FA = Col(r, "Token2FAClientId");
                iswClientId = Col(r, "CardISWClientId");
                iswClientKey = Col(r, "CardISWClientKey");
                iswBankCode = Col(r, "CardISWBankCode");
            }

            TwoFactorService hardToken = new TwoFactorService();
            var respToken = hardToken.doResponseAuthentication(user, clientId2FA, psword.Text);
            if (Convert.ToString(respToken.status) != "1")
            {
                ShowError("The token code was not accepted. Check the code and try again.");
                return;
            }

            DataTable cards = Query("EazyConnect", "EXEC SpGetBatRecCardMaintInfo @RefNo", CommandType.Text, new SqlParameter("@RefNo", RefNo));
            if (cards.Rows.Count == 0) { ShowError("No cards were found in this request."); return; }

            List<string> failed = new List<string>();
            foreach (DataRow item in cards.Rows)
            {
                string accountNo = Col(item, "accountNo");
                string cardPin = Col(item, "depCardpin");
                string cardCVV = Col(item, "depcardcvv");
                string cardPanNo = Col(item, "CardInstno");
                string cardMonth = Col(item, "depcardmonth");
                string linkAccountNo = Col(item, "LinkedAccountNo");
                string maintType = Col(item, "maintType");

                string mobileNo = "";
                foreach (DataRow dr in Query("EazyConnect", "EXEC SpAXMobileNo @acct", CommandType.Text, new SqlParameter("@acct", accountNo)).Rows)
                    mobileNo = Col(dr, "SMSNo");

                string reply = null, label = null;
                switch (maintType)
                {
                    case "100004":   // block: recorded only, no call to the switch
                        continue;
                    case "100005":
                        label = "Unblock Details";
                        reply = ICardProcess.UnBlockcards(iswBankCode, user, iswClientId, iswClientKey, AccountType(accountNo), cardPanNo, cardPin, cardMonth, cardCVV);
                        break;
                    case "100006":
                        label = "Unlink Details";
                        reply = ICardProcess.UnLinkcards(iswBankCode, user, iswClientId, iswClientKey, "1", cardPanNo, cardPin, cardMonth, cardCVV, accountNo);
                        break;
                    case "100007":
                        label = "Pin ReIssueance Details";
                        reply = ICardProcess.PinReIssuance(iswBankCode, user, iswClientId, iswClientKey, AccountType(accountNo), cardPanNo, cardPin, cardMonth, cardCVV, "72", accountNo, mobileNo, user);
                        break;
                    case "100008":
                        string check = CheckLink();
                        if (check != "") { failed.Add(accountNo + ": " + check); continue; }
                        label = "Linking Details";
                        reply = ICardProcess.Linkcards(iswBankCode, user, iswClientId, iswClientKey, "1", cardPanNo, cardPin, cardMonth, cardCVV, linkAccountNo);
                        break;
                    default:
                        failed.Add(accountNo + ": unknown maintenance type " + maintType);
                        continue;
                }

                ICardProcess.AddtoLogFile(reply, label);
                string message;
                if (!SwitchSucceeded(reply, out message))
                    failed.Add(accountNo + ": declined by the card switch" + (message == "" ? "" : " (" + message + ")"));
            }

            if (failed.Count > 0)
            {
                ShowError(string.Format("{0} card(s) were not processed, so the request is still open: {1}", failed.Count, string.Join("; ", failed.ToArray())));
                return;
            }

            SetBatchStatus(3);
            allDone = true;
        }
        catch (Exception ex)
        {
            ShowError("The request could not be completed: " + ex.Message);
        }

        if (allDone)
        {
            Response.Redirect(ApprovalsPage, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }

    /// <summary>Runs SpCheckCardLink for a link request; "" when linking may go ahead, otherwise the reason.</summary>
    private string CheckLink()
    {
        using (SqlConnection conn = new SqlConnection(ConnectionString("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand("SpCheckCardLink", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@eBatRefNo", RefNo));
            cmd.Parameters.Add(new SqlParameter("@status", 3));
            cmd.Parameters.Add(new SqlParameter("@MaintLocation", CurrentBranch()));
            SqlParameter code = cmd.Parameters.Add(new SqlParameter("@ResponseCode", SqlDbType.VarChar, 5000));
            code.Direction = ParameterDirection.Output;
            SqlParameter desc = cmd.Parameters.Add(new SqlParameter("@ResponseDesc", SqlDbType.VarChar, 5000));
            desc.Direction = ParameterDirection.Output;
            conn.Open();
            cmd.ExecuteNonQuery();
            if (Convert.ToString(code.Value) == "0") return "";
            string reason = Convert.ToString(desc.Value);
            return reason == "" ? "link check failed" : reason;
        }
    }
}
