using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Single card maintenance: the card is found with Sproc_GetAcctCardNoInfo and a one-card maintenance batch
/// "MNT..." is raised with Sproc_AddCardMaint. Plain ASP.NET controls (no DevExpress).
/// </summary>
public partial class Request_CardActivitySingle : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Path, false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid) return;

        string cardNo = txtCardNo.Text.Trim();
        string month = txtMonth.Text.Trim();   // yyyy-MM from the month box
        if (month.Length < 7) { ShowError("Enter the expiry month."); return; }

        try
        {
            string accountNo = "", seqNo = "";
            foreach (DataRow r in Query("EXEC Sproc_GetAcctCardNoInfo @cardNo", P("@cardNo", cardNo)).Rows)
            {
                accountNo = Col(r, "depCardAccountNo");
                seqNo = Col(r, "SeqCardNo");
            }
            if (accountNo == "") { ShowError("Invalid card information, or the card is not in use."); return; }

            string refNo = string.Format("MNT{0}", Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", ""));
            string response = Exec("Sproc_AddCardMaint",
                P("@eBatRefNo", refNo),
                P("@requestTitle", txtTitle.Text.Trim()),
                P("@accountNo", accountNo),
                P("@eIssueDate", DateTime.Today),
                P("@eExipryDate", DateTime.Today),
                P("@reason", txtRemarks.Text.Trim()),
                P("@UserID", CurrentUser()),
                P("@SeqNum", seqNo),
                P("@maintType", DrpMaithType.SelectedValue),
                P("@CardInstno", cardNo),
                P("@Cardcvv", txtCVV.Text),
                P("@Cardpin", txtPin.Text),
                P("@Cardmonth", month.Substring(2).Replace("-", "")),   // yyMM
                Out("@ResponseDesc"));

            if (response != "" && response != "Success") { ShowError("The request was not saved: " + response); return; }

            ShowSuccess(string.Format("Maintenance batch {0} raised for account {1} ({2}). It now needs approval.", refNo, accountNo, DrpMaithType.SelectedItem.Text));
            txtCardNo.Text = "";
            txtMonth.Text = "";
            txtRemarks.Text = "";
        }
        catch (Exception ex)
        {
            ShowError("The request could not be saved: " + ex.Message);
        }
    }

    // =====================================================================
    // Shared Request-page helpers (same code in every rewritten C# Request page)
    // =====================================================================

    /// <summary>Signed-in identity "user?branch" (forms identity, else the LogonUserID cookie).</summary>
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

    private static string MainDb { get { return ConnectionString("EazyConnect"); } }

    /// <summary>Runs a query with parameters (never string concatenation) and returns the first table.</summary>
    private static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        return QueryOn(MainDb, sql, CommandType.Text, parameters);
    }

    private static DataTable QueryOn(string connection, string sql, CommandType type, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandType = type;
            cmd.Parameters.AddRange(parameters);
            adapter.Fill(table);
        }
        return table;
    }

    /// <summary>Runs a stored procedure on the main database; returns the value of its first output parameter (or "").</summary>
    private static string Exec(string procedure, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(MainDb))
        using (SqlCommand cmd = new SqlCommand(procedure, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        foreach (SqlParameter p in parameters)
        {
            if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                return p.Value == null || p.Value == DBNull.Value ? "" : Convert.ToString(p.Value);
        }
        return "";
    }

    private static SqlParameter P(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }

    private static SqlParameter Out(string name)
    {
        SqlParameter p = new SqlParameter(name, SqlDbType.VarChar, 5000);
        p.Direction = ParameterDirection.Output;
        return p;
    }

    private static string Col(DataRow row, string column)
    {
        if (row == null || !row.Table.Columns.Contains(column) || row.IsNull(column)) return "";
        return Convert.ToString(row[column]);
    }

    /// <summary>yyyy-MM-dd for an HTML date box.</summary>
    protected static string DateInput(object value)
    {
        DateTime d;
        if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (value != null && value != DBNull.Value && DateTime.TryParse(Convert.ToString(value), out d) && d.Year > 1900)
            return d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return "";
    }

    private static bool TryDate(string text, out DateTime value)
    {
        return DateTime.TryParseExact((text ?? "").Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out value);
    }

    /// <summary>Splits "123, 456 789" into a clean list of account numbers.</summary>
    private static List<string> AccountList(string text)
    {
        List<string> result = new List<string>();
        foreach (string part in (text ?? "").Split(new[] { ',', ';', ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries))
        {
            string acct = part.Trim().Trim('\'');
            if (acct != "" && !result.Contains(acct)) result.Add(acct);
        }
        return result;
    }

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        litError.Text = "";
    }

    private void ShowError(string message)
    {
        litError.Text = HttpUtility.HtmlEncode(message);
        litResult.Text = "";
    }
}
