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
/// eRequest booking: requests from the remote channels (SpCardRemoteRequestAllApprType, status 5) are booked
/// into a new card request batch (Sproc_AddCardRequestMobTrack, then SpCardRemoteReqStatusInfoUpd status 9)
/// or dropped (SpCardRemoteReqStatusInfoUpd status 8). Plain ASP.NET controls (no DevExpress).
/// </summary>
public partial class Request_eChannelsRequest : System.Web.UI.Page
{
    private const int StatusWaiting = 5;
    private const int StatusDropped = 8;
    private const int StatusBooked = 9;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadRequestTypes();
            BindGrid();
        }
    }

    // =====================================================================
    // Officer, connection and small helpers
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

    private static string MainDb
    {
        get { return ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString; }
    }

    private static DataTable Query(string sql, params SqlParameter[] parameters)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(MainDb))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.Parameters.AddRange(parameters);
            adapter.Fill(table);
        }
        return table;
    }

    private static void Exec(string sql, CommandType type, params SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(MainDb))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = type;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private static SqlParameter P(string name, object value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
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

    private void ShowSuccess(string message)
    {
        litResult.Text = HttpUtility.HtmlEncode(message);
        Literal1.Text = "";
    }

    private void ShowError(string message)
    {
        Literal1.Text = HttpUtility.HtmlEncode(message);
        litResult.Text = "";
    }

    // =====================================================================
    // Request types and the request list
    // =====================================================================

    private void LoadRequestTypes()
    {
        drpBatchRefNo.Items.Clear();
        drpBatchRefNo.Items.Add(new ListItem("Choose a request type", ""));
        try
        {
            foreach (DataRow r in Query("EXEC SpCardRemoteReqSel @status", P("@status", StatusWaiting)).Rows)
            {
                string code = Col(r, "CardTypeCode");
                if (code == "" || drpBatchRefNo.Items.FindByValue(code) != null) continue;
                string desc = Col(r, "CardTypeDesc");
                drpBatchRefNo.Items.Add(new ListItem(desc == "" ? code : code + " - " + desc, code));
            }
        }
        catch (Exception ex)
        {
            ShowError("The request types could not be loaded: " + ex.Message);
        }
    }

    private DataTable LoadRequests()
    {
        if (drpBatchRefNo.SelectedValue == "") return new DataTable();
        DataTable t = Query("EXEC SpCardRemoteRequestAllApprType @userId = @userId, @locationID = @locationID, @status = @status, @requestType = @requestType",
            P("@userId", CurrentUser()), P("@locationID", CurrentBranch()), P("@status", StatusWaiting), P("@requestType", drpBatchRefNo.SelectedValue));
        // grouped by branch, as the old grid was
        if (t.Columns.Contains("BranchName"))
        {
            DataView view = t.DefaultView;
            view.Sort = t.Columns.Contains("accountNo") ? "BranchName, accountNo" : "BranchName";
            t = view.ToTable();
        }
        return t;
    }

    private string _lastBranch;

    private void BindGrid()
    {
        _lastBranch = null;
        try
        {
            DataTable t = LoadRequests();
            ctrlGrid.DataSource = t;
            ctrlGrid.DataBind();
            litTotal.Text = drpBatchRefNo.SelectedValue == "" ? "" :
                string.Format("{0:N0} request{1}", t.Rows.Count, t.Rows.Count == 1 ? "" : "s");
        }
        catch (Exception ex)
        {
            ctrlGrid.DataSource = null;
            ctrlGrid.DataBind();
            litTotal.Text = "";
            ShowError("The requests could not be loaded: " + ex.Message);
        }
    }

    /// <summary>Shows the branch name only on the first request of each branch.</summary>
    protected void ctrlGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        Label lbl = e.Row.FindControl("lblBranch") as Label;
        if (lbl == null) return;
        if (lbl.Text == _lastBranch) lbl.CssClass = "same";
        else if (_lastBranch != null) e.Row.CssClass = "group-start";
        _lastBranch = lbl.Text;
    }

    protected void drpBatchRefNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        litResult.Text = "";
        Literal1.Text = "";
        BindGrid();
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("eChannelsRequest.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // =====================================================================
    // Book / drop
    // =====================================================================

    private List<GridViewRow> TickedRows()
    {
        List<GridViewRow> rows = new List<GridViewRow>();
        foreach (GridViewRow row in ctrlGrid.Rows)
        {
            if (row.RowType != DataControlRowType.DataRow) continue;
            CheckBox chk = row.FindControl("chkSelect") as CheckBox;
            if (chk != null && chk.Checked) rows.Add(row);
        }
        return rows;
    }

    private static string Text(GridViewRow row, string id)
    {
        TextBox tb = row.FindControl(id) as TextBox;
        return tb == null ? "" : tb.Text.Trim();
    }

    private string Key(GridViewRow row, string name)
    {
        return Convert.ToString(ctrlGrid.DataKeys[row.RowIndex].Values[name]);
    }

    private void SetRemoteStatus(string trackId, int status)
    {
        Exec("EXEC SpCardRemoteReqStatusInfoUpd @trackId, @status, @user", CommandType.Text,
            P("@trackId", trackId), P("@status", status), P("@user", CurrentUser()));
    }

    protected void BtnApprove_Click(object sender, EventArgs e)
    {
        Page.Validate("book");
        if (!Page.IsValid) return;

        List<GridViewRow> rows = TickedRows();
        if (rows.Count == 0) { ShowError("Tick at least one request first."); return; }

        DateTime issue, expiry;
        if (!TryDate(dateIssue.Text, out issue) || !TryDate(dateexpiry.Text, out expiry)) { ShowError("Enter valid issue and expiry dates."); return; }
        if (expiry <= issue) { ShowError("The expiry date must be after the issue date."); return; }

        string refNo = Guid.NewGuid().ToString().Substring(0, 12).ToUpperInvariant();
        string user = CurrentUser();
        int done = 0;
        List<string> failed = new List<string>();

        foreach (GridViewRow row in rows)
        {
            string trackId = Key(row, "remSeqNo");
            string accountNo = Key(row, "accountNo");
            try
            {
                string name = Text(row, "tatAccountDesc");
                if (name == "") throw new InvalidOperationException("the display name is empty");
                DateTime dob;
                object dobValue = TryDate(Text(row, "ASPxDateEdit1"), out dob) ? (object)dob : DBNull.Value;

                Exec("Sproc_AddCardRequestMobTrack", CommandType.StoredProcedure,
                    P("@RefNo", refNo),
                    P("@AccountNo", accountNo),
                    P("@Description", name),
                    P("@PanNo", ""),
                    P("@Address", Text(row, "tatAddress")),
                    P("@City", Text(row, "TxtCity")),
                    P("@CountryCode", "NGA"),
                    P("@CardBrand", drpBatchRefNo.SelectedValue),
                    P("@CardIssueDate", issue),
                    P("@CardExpiryDate", expiry),
                    P("@CardCounter", 0L),
                    P("@Seq", 2L),
                    P("@RequestTitle", txtTitle.Text.Trim()),
                    P("@DOB", dobValue),
                    P("@AddedBy", user),
                    P("@ApprovedBy", "System"),
                    P("@CardPhoneNo", Text(row, "ASPxTextBox4")),
                    P("@TrackID", trackId));

                SetRemoteStatus(trackId, StatusBooked);
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(string.Format("{0} ({1}): {2}", accountNo, trackId, ex.Message));
            }
        }

        Report(done == 0 ? "" : string.Format("Booked {0} request{1} into batch {2}.", done, done == 1 ? "" : "s", refNo), failed);
    }

    protected void btnDisApprove_Click(object sender, EventArgs e)
    {
        List<GridViewRow> rows = TickedRows();
        if (rows.Count == 0) { ShowError("Tick at least one request first."); return; }
        int done = 0;
        List<string> failed = new List<string>();
        foreach (GridViewRow row in rows)
        {
            string trackId = Key(row, "remSeqNo");
            try
            {
                SetRemoteStatus(trackId, StatusDropped);
                done++;
            }
            catch (Exception ex)
            {
                failed.Add(trackId + ": " + ex.Message);
            }
        }
        Report(done == 0 ? "" : string.Format("Dropped {0} request{1}.", done, done == 1 ? "" : "s"), failed);
    }

    private void Report(string success, List<string> failed)
    {
        ShowSuccess(success);
        if (failed.Count > 0)
            Literal1.Text = HttpUtility.HtmlEncode(string.Format("{0} request(s) not processed: {1}", failed.Count, string.Join("; ", failed.ToArray())));
        BindGrid();
    }
}
