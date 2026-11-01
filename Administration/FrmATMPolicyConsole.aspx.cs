using System;
using System.Web;
using System.Web.UI.WebControls;
using NETClass;

public partial class Mandate_FrmATMPolicyConsole : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ASPxGridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        litResult.Text = "";
        litError.Text = "";
        ASPxGridView1.EditIndex = e.NewEditIndex;
    }

    protected void ASPxGridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ASPxGridView1.EditIndex = -1;
    }

    /// <summary>Saves the edited policy, writes the audit log, then leaves edit mode.</summary>
    protected void ASPxGridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // We save it ourselves, so stop the data source from trying to
        e.Cancel = true;

        GridViewRow row = ASPxGridView1.Rows[e.RowIndex];
        TextBox txtValue = row.FindControl("txtPolicyValue") as TextBox;
        CheckBox chkApply = row.FindControl("chkApplyPolicy") as CheckBox;

        string policyId = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["PolicyID"]);
        string policyValue = txtValue == null ? "" : (txtValue.Text ?? "").Trim();
        bool apply = chkApply != null && chkApply.Checked;

        try
        {
            LoadCurrentUser();

            string msg = string.Format("New Application Policies Modification on - Policy:{0}; PolicyValue: {1}; EnablePolicy: {2}",
                policyId, policyValue, apply);
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '900','{0}','{1}',''", Sql(msg), Sql(guser)));

            Classes.RecordOp(string.Format("SpATMUpdateGENPolicy '{0}','{1}',{2}",
                Sql(policyId), Sql(policyValue), apply ? 1 : 0));

            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            litResult.Text = HttpUtility.HtmlEncode(string.Format("Policy {0} saved.", policyId));
            litError.Text = "";
        }
        catch (Exception ex)
        {
            litResult.Text = "";
            litError.Text = HttpUtility.HtmlEncode(string.Format("Policy {0} was not saved: {1}", policyId, ex.Message));
        }
    }

    /// <summary>Staff user from the LogonUserID cookie ("username?branch"), falling back to the sign-in name.</summary>
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
        if (s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase) || s.Equals("yes", StringComparison.OrdinalIgnoreCase)) return true;

        long n;
        return long.TryParse(s, out n) && n != 0;
    }

    private static string Sql(string value)
    {
        return (value ?? "").Replace("'", "''");
    }
}