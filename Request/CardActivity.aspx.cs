using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Card operation maintenance (bulk): cards from GetAllAccountCardMaintReq (product, branch, optional account list)
/// are put into a new maintenance batch "MNT..." with Sproc_AddCardMaint, one row per ticked card.
/// Plain ASP.NET controls (no DevExpress).
/// </summary>
public partial class Request_CardActivity : System.Web.UI.Page
{
    private const string DefaultBranch = "00";
    private const string DefaultProduct = "103";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadFilters();
            BindGrid();
        }
    }

    // =====================================================================
    // Filters and card list
    // =====================================================================

    private void LoadFilters()
    {
        try
        {
            foreach (DataRow r in QueryOn(MainDb, "Sproc_GetAllProductsCards", CommandType.StoredProcedure).Rows)
            {
                string code = Col(r, "Product Code"), name = Col(r, "product"), type = Col(r, "Type");
                if (code == "" || drp_products.Items.FindByValue(code) != null) continue;
                drp_products.Items.Add(new ListItem((name == "" ? code : name) + (type == "" ? "" : " (" + type + ")"), code));
            }
            foreach (DataRow r in QueryOn(MainDb, "Sproc_GetAllBranchesCards", CommandType.StoredProcedure).Rows)
            {
                string code = Col(r, "Headsub_code"), name = Col(r, "sub_name");
                if (code == "" || drpbranches.Items.FindByValue(code) != null) continue;
                drpbranches.Items.Add(new ListItem(name == "" ? code : name + " (" + code + ")", code));
            }
        }
        catch (Exception ex)
        {
            ShowError("The products or branches could not be loaded: " + ex.Message);
        }
        Pick(drp_products, DefaultProduct);
        Pick(drpbranches, DefaultBranch);
    }

    private static void Pick(DropDownList list, string value)
    {
        if (list.Items.FindByValue(value) == null) list.Items.Insert(0, new ListItem(value, value));
        list.ClearSelection();
        list.Items.FindByValue(value).Selected = true;
    }

    private DataTable LoadCards()
    {
        DataTable t = Query("EXEC GetAllAccountCardMaintReq @product, @branch",
            P("@product", drp_products.SelectedValue), P("@branch", drpbranches.SelectedValue));
        List<string> accounts = AccountList(txtSpecificAcct.Text);
        if (accounts.Count > 0 && t.Columns.Contains("AccountNo"))
        {
            HashSet<string> wanted = new HashSet<string>(accounts, StringComparer.OrdinalIgnoreCase);
            DataTable filtered = t.Clone();
            foreach (DataRow r in t.Rows) if (wanted.Contains(Col(r, "AccountNo"))) filtered.ImportRow(r);
            t = filtered;
        }
        // columns the list shows; missing ones are added empty so the page never breaks on them
        foreach (string column in new[] { "SeqCardNo", "AccountNo", "depCardpanno", "depCardpannoMask", "AccountName", "Product", "CardExipryDate" })
            if (!t.Columns.Contains(column)) t.Columns.Add(column, typeof(string));
        return t;
    }

    private void BindGrid()
    {
        try
        {
            DataTable t = LoadCards();
            ctrlGrid.DataSource = t;
            ctrlGrid.DataBind();
            litTotal.Text = string.Format("{0:N0} card{1}", t.Rows.Count, t.Rows.Count == 1 ? "" : "s");
        }
        catch (Exception ex)
        {
            ctrlGrid.DataSource = null;
            ctrlGrid.DataBind();
            litTotal.Text = "";
            ShowError("The cards could not be loaded: " + ex.Message);
        }
    }

    protected void Filter_Changed(object sender, EventArgs e)
    {
        ctrlGrid.PageIndex = 0;
        litResult.Text = "";
        litError.Text = "";
        BindGrid();
    }

    protected void ctrlGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ctrlGrid.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Path, false);
        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Card expiry for a month box (yyyy-MM), from a date or a yyMM value.</summary>
    protected static string MonthInput(object value)
    {
        if (value == null || value == DBNull.Value) return "";
        if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM", CultureInfo.InvariantCulture);
        string s = Convert.ToString(value).Trim();
        if (s.Length == 4 && s.All(char.IsDigit)) return "20" + s.Substring(0, 2) + "-" + s.Substring(2, 2);
        DateTime d;
        return DateTime.TryParse(s, out d) ? d.ToString("yyyy-MM", CultureInfo.InvariantCulture) : "";
    }

    /// <summary>yyyy-MM from the month box as yyMM, the card expiry format used by the card switch.</summary>
    private static string CardMonth(string monthInput)
    {
        monthInput = (monthInput ?? "").Trim();
        return monthInput.Length >= 7 && monthInput[4] == '-' ? monthInput.Substring(2, 2) + monthInput.Substring(5, 2) : monthInput.Replace("-", "");
    }

    // =====================================================================
    // Submit
    // =====================================================================

    private static string RowText(GridViewRow row, string id)
    {
        TextBox tb = row.FindControl(id) as TextBox;
        return tb == null ? "" : tb.Text.Trim();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Page.Validate("submit");
        if (!Page.IsValid) return;

        List<GridViewRow> rows = new List<GridViewRow>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (row.RowType == DataControlRowType.DataRow && chk != null && chk.Checked) rows.Add(row);
        }
        if (rows.Count == 0) { ShowError("Tick at least one card first."); return; }

        // full card numbers stay on the server: look them up again by card sequence number
        Dictionary<string, string> panBySeq = new Dictionary<string, string>();
        foreach (DataRow r in LoadCards().Rows)
        {
            string seq = Col(r, "SeqCardNo");
            if (seq != "" && !panBySeq.ContainsKey(seq)) panBySeq[seq] = Col(r, "depCardpanno");
        }

        string refNo = string.Format("MNT{0}", Guid.NewGuid().ToString().Substring(0, 12).ToUpper().Replace("-", ""));
        string user = CurrentUser();
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in rows)
        {
            string seq = Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Values["SeqCardNo"]);
            string accountNo = Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Values["AccountNo"]);
            string reason = RowText(row, "tatReason");
            if (reason == "") { failed.Add(accountNo + ": reason missing"); continue; }
            string pan;
            if (!panBySeq.TryGetValue(seq, out pan)) { failed.Add(accountNo + ": card no longer listed"); continue; }
            try
            {
                string response = Exec("Sproc_AddCardMaint",
                    P("@eBatRefNo", refNo),
                    P("@requestTitle", txtTitle.Text.Trim()),
                    P("@accountNo", accountNo),
                    P("@eIssueDate", DateTime.Today),
                    P("@eExipryDate", DateTime.Today),
                    P("@reason", reason),
                    P("@UserID", user),
                    P("@SeqNum", seq),
                    P("@maintType", DrpMaithType.SelectedValue),
                    P("@CardInstno", pan),
                    P("@Cardcvv", RowText(row, "txtCvv")),
                    P("@Cardpin", RowText(row, "txtPin")),
                    P("@Cardmonth", CardMonth(RowText(row, "txtMonth"))),
                    Out("@ResponseDesc"));
                if (response == "Success") done++;
                else failed.Add(accountNo + ": " + (response == "" ? "not saved" : response));
            }
            catch (Exception ex)
            {
                failed.Add(accountNo + ": " + ex.Message);
            }
        }

        ShowSuccess(done == 0 ? "" : string.Format("Maintenance batch {0} raised for {1} card{2} ({3}). It now needs approval.",
            refNo, done, done == 1 ? "" : "s", DrpMaithType.SelectedItem.Text));
        if (failed.Count > 0)
            litError.Text = HttpUtility.HtmlEncode(string.Format("{0} card(s) not added: {1}", failed.Count, string.Join("; ", failed.ToArray())));
        BindGrid();
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
