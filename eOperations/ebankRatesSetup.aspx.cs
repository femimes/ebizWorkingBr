using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class eOperations_ebankRatesSetup : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    // =====================================================================
    // Add
    // =====================================================================
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        RateBand band = new RateBand();
        band.Code = txtIntRateCode.Text.Trim();
        band.Desc = txtIntRateDesc.Text.Trim();
        band.MinBal = ToDecimal(txtMinBalAmt.Text);
        band.MaxBal = ToDecimal(txtMaxBalAmt.Text);
        band.Rate = ToDecimal(txtRate.Text);
        band.Fee = ToDecimal(txtFeeAmount.Text);
        band.Apply = chkApplyRate.Checked;

        if (!CheckBand(band)) return;

        try
        {
            LoadCurrentUser();
            ExecBand("SpNapsTransAddRateBandSetups", "0", band);

            Audit(string.Format("New Application Charges Modification on - {0}", band.Code));
            ClearForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Range {0} added.", band.Code));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Range {0} was not added: {1}", band.Code, ex.Message));
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ShowSuccess("");
    }

    private void ClearForm()
    {
        txtIntRateCode.Text = "";
        txtIntRateDesc.Text = "";
        txtMinBalAmt.Text = "0.00";
        txtMaxBalAmt.Text = "0.00";
        txtRate.Text = "0.00";
        txtFeeAmount.Text = "0.00";
        chkApplyRate.Checked = false;
    }

    // =====================================================================
    // Edit / delete in the grid
    // =====================================================================
    protected void ASPxGridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ShowSuccess("");
        ASPxGridView1.EditIndex = e.NewEditIndex;
    }

    protected void ASPxGridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ASPxGridView1.EditIndex = -1;
    }

    protected void ASPxGridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        e.Cancel = true;

        GridViewRow row = ASPxGridView1.Rows[e.RowIndex];
        string seqNo = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["SeqNo"]);

        RateBand band = new RateBand();
        band.Code = TextOf(row, "txtEditIntRateCode");
        band.Desc = TextOf(row, "txtEditIntRateDesc");
        band.MinBal = ToDecimal(TextOf(row, "txtEditMinBalAmt"));
        band.MaxBal = ToDecimal(TextOf(row, "txtEditMaxBalAmt"));
        band.Rate = ToDecimal(TextOf(row, "txtEditRate"));
        band.Fee = ToDecimal(TextOf(row, "txtEditFeeAmount"));
        CheckBox chk = row.FindControl("chkEditApplyRate") as CheckBox;
        band.Apply = chk != null && chk.Checked;

        if (!CheckBand(band)) return;

        try
        {
            LoadCurrentUser();
            ExecBand("SpNapsTransUpdateRateBandSetups", seqNo, band);

            Audit(string.Format("Application Charges Modification on - {0} Code:{1};Desc:{2}; MinAmt:{3}; MaxAmt:{4} Rate:{5}; Fee Amount:{6}; Apply Rate:{7};",
                seqNo, band.Code, band.Desc, band.MinBal, band.MaxBal, band.Rate, band.Fee, band.Apply));

            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Range {0} updated.", band.Code));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Range {0} was not updated: {1}", band.Code, ex.Message));
        }
    }

    protected void ASPxGridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        string seqNo = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["SeqNo"]);

        try
        {
            LoadCurrentUser();

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpNapseTransDeleteRateBandSetups", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SeqNo", seqNo));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Audit(string.Format("Application Charges Deleting on - {0}", seqNo));
            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Range {0} deleted.", seqNo));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Range {0} was not deleted: {1}", seqNo, ex.Message));
        }
    }

    // =====================================================================
    // Shared
    // =====================================================================
    private sealed class RateBand
    {
        public string Code = "";
        public string Desc = "";
        public decimal MinBal;
        public decimal MaxBal;
        public decimal Rate;
        public decimal Fee;
        public bool Apply;
    }

    private bool CheckBand(RateBand band)
    {
        if (band.Code == "") { ShowError("Setup code cannot be empty."); return false; }
        if (band.Desc == "") { ShowError("Setup description cannot be empty."); return false; }
        if (band.MaxBal < band.MinBal) { ShowError("The maximum amount must not be less than the minimum amount."); return false; }
        return true;
    }

    private static void ExecBand(string procedure, string seqNo, RateBand band)
    {
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand(procedure, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@SeqNo", seqNo));
            cmd.Parameters.Add(new SqlParameter("@IntRateCode", band.Code));
            cmd.Parameters.Add(new SqlParameter("@IntRateDesc", band.Desc));
            cmd.Parameters.Add(new SqlParameter("@MinBalAmt", (double)band.MinBal));
            cmd.Parameters.Add(new SqlParameter("@MaxBalAmt", (double)band.MaxBal));
            cmd.Parameters.Add(new SqlParameter("@Rate", band.Rate));
            cmd.Parameters.Add(new SqlParameter("@FeeAmount", band.Fee));
            // Sent as text, as before ("True"/"False")
            cmd.Parameters.Add(new SqlParameter("@ApplyRate", band.Apply ? "True" : "False"));
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    private void Audit(string message)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '900','{0}','{1}',''", Sql(message), Sql(guser)));
    }

    private void LoadCurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        guser = struser;
    }

    protected bool AsBool(object value)
    {
        if (value == null || value == DBNull.Value) return false;
        if (value is bool) return (bool)value;
        string s = Convert.ToString(value).Trim();
        if (s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
        long n;
        return long.TryParse(s, out n) && n != 0;
    }

    private static decimal ToDecimal(string value)
    {
        decimal d;
        string s = (value ?? "").Replace(",", "").Trim();
        return decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0m;
    }

    private static string TextOf(GridViewRow row, string id)
    {
        TextBox box = row.FindControl(id) as TextBox;
        return box == null ? "" : (box.Text ?? "").Trim();
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