using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Mandate_TerminalRegForm : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    // =====================================================================
    // Save (register or update; both use Sproc_ATMAddTerminalReg, as before)
    // =====================================================================
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        LoadCurrentUser();
        bool isEdit = hfTerminalSeq.Value != "";
        string terminalId = txtTerminalID.Text.Trim();
        string terminalType = drpTerminalType.SelectedValue;
        string location = drpTerminalLocation.SelectedValue;

        if (location == "") { ShowError("Location can not be empty. Choose a branch."); return; }

        try
        {
            // The terminal type must have a ledger mapping
            DataSet maps = Classes.GetDataset(string.Format("Sproc_GetTerminalAcctMaps '{0}'", Sql(terminalType)));
            if (maps.Tables.Count == 0 || maps.Tables[0].Rows.Count == 0)
            {
                ShowError(string.Format("There is no ledger mapping for {0} terminals.", terminalType));
                return;
            }

            // Use the account number given, or generate one (2071 for ATM, 2081 for POS)
            string accountNo = txtTerminalAcctNo.Text.Trim();
            if (accountNo == "")
                accountNo = GenerateAccountNumber(terminalType == "ATM" ? 2071 : 2081);

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("Sproc_ATMAddTerminalReg", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TerminalID", terminalId));
                cmd.Parameters.Add(new SqlParameter("@TerminalType", terminalType));
                cmd.Parameters.Add(new SqlParameter("@TerminalDesc", txtTerminalDesc.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@TerminalAcctNo", accountNo));
                cmd.Parameters.Add(new SqlParameter("@TerminalLocation", location));
                cmd.Parameters.Add(new SqlParameter("@TerminalIP", txtTerminalIP.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@TerminalEmail", txtTerminalEmail.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@TerminalAddress", txtTerminalAddress.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@TerminalPhone", txtTerminalPhone.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@TerminalRep", txtTerminalRep.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@TerminalAddedBy", guser ?? ""));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ClearForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format(isEdit ? "Terminal {0} updated." : "Terminal {0} registered (account {1}).", terminalId, accountNo));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Terminal {0} was not saved: {1}", terminalId, ex.Message));
        }
    }

    private static string GenerateAccountNumber(int numberId)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnectSource")))
        using (SqlCommand cmd = new SqlCommand("SpGenerateNumberSequence", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@NumberID", numberId));
            SqlParameter output = cmd.Parameters.Add(new SqlParameter("@GeneratedNumber", SqlDbType.VarChar, 100));
            output.Direction = ParameterDirection.Output;
            conn.Open();
            cmd.ExecuteNonQuery();
            return Convert.ToString(output.Value);
        }
    }

    // =====================================================================
    // Edit: load a row into the form
    // =====================================================================
    protected void ASPxGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditTerminal") return;

        int index = Convert.ToInt32(e.CommandArgument) - ASPxGridView1.PageIndex * ASPxGridView1.PageSize;
        DataKey k = ASPxGridView1.DataKeys[index];

        hfTerminalSeq.Value = Convert.ToString(k["TerminalSeq"]);
        txtTerminalID.Text = Convert.ToString(k["TerminalID"]);
        SelectValue(drpTerminalType, Convert.ToString(k["TerminalType"]).Trim());
        txtTerminalAcctNo.Text = Convert.ToString(k["TerminalAcctNo"]);
        txtTerminalDesc.Text = Convert.ToString(k["TerminalDesc"]);
        SelectValue(drpTerminalLocation, Convert.ToString(k["TerminalLocation"]).Trim());
        txtTerminalAddress.Text = Convert.ToString(k["TerminalAddress"]);
        txtTerminalPhone.Text = Convert.ToString(k["TerminalPhone"]);
        txtTerminalIP.Text = Convert.ToString(k["TerminalIP"]);
        txtTerminalEmail.Text = Convert.ToString(k["TerminalEmail"]);
        txtTerminalRep.Text = Convert.ToString(k["TerminalRep"]);

        litFormTitle.Text = HttpUtility.HtmlEncode("Edit terminal " + txtTerminalID.Text);
        btnSave.Text = "Save changes";
        ShowSuccess("");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ShowSuccess("");
    }

    private void ClearForm()
    {
        hfTerminalSeq.Value = "";
        txtTerminalID.Text = "";
        drpTerminalType.SelectedIndex = 0;
        txtTerminalAcctNo.Text = "";
        txtTerminalDesc.Text = "";
        drpTerminalLocation.SelectedIndex = 0;
        txtTerminalAddress.Text = "";
        txtTerminalPhone.Text = "";
        txtTerminalIP.Text = "";
        txtTerminalEmail.Text = "";
        txtTerminalRep.Text = "";
        litFormTitle.Text = "Register a terminal";
        btnSave.Text = "Register terminal";
    }

    private static void SelectValue(DropDownList list, string value)
    {
        list.ClearSelection();
        ListItem item = list.Items.FindByValue(value);
        if (item != null) item.Selected = true;
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private void LoadCurrentUser()
    {
        luser = User.Identity.Name ?? "";
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        guser = struser;
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
    }

    private void ShowError(string message)
    {
        litResult.Text = "";
        cus_val.IsValid = false;
        cus_val.ErrorMessage = message;
        cus_val.Text = HttpUtility.HtmlEncode(message);
    }

    private static string Cfg(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    private static string Sql(string value)
    {
        return (value ?? "").Replace("'", "''");
    }
}