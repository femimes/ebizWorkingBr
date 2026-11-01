using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class eOperations_FTTransTypeSetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    // =====================================================================
    // Add (SpNapsAddExtAcctTransProfile, as the old data source did)
    // =====================================================================
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string accountNo = txtAccountNo.Text.Trim();
        try
        {
            Exec("SpNapsAddExtAcctTransProfile",
                new SqlParameter("@SeqRef", " "),
                new SqlParameter("@AccountNo", accountNo),
                new SqlParameter("@TransRefDesc", txtTransRefDesc.Text.Trim()),
                new SqlParameter("@TransType", drpTransType.SelectedValue));

            ClearForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Profile added for account {0}.", accountNo));
        }
        catch (Exception ex)
        {
            ShowError("The profile was not added: " + ex.Message);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ShowSuccess("");
    }

    private void ClearForm()
    {
        txtAccountNo.Text = "";
        txtTransRefDesc.Text = "";
        drpTransType.SelectedIndex = 0;
    }

    // =====================================================================
    // Edit / delete
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
        string seqRef = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["SeqRef"]);
        TextBox acct = row.FindControl("txtEditAccountNo") as TextBox;
        TextBox desc = row.FindControl("txtEditTransRefDesc") as TextBox;
        DropDownList type = row.FindControl("drpEditTransType") as DropDownList;

        try
        {
            Exec("SpNapsUpdateExtAcctTransProfile",
                new SqlParameter("@SeqRef", seqRef),
                new SqlParameter("@AccountNo", acct == null ? "" : acct.Text.Trim()),
                new SqlParameter("@TransRefDesc", desc == null ? "" : desc.Text.Trim()),
                new SqlParameter("@TransType", type == null ? "" : type.SelectedValue));

            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Profile {0} updated.", seqRef));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Profile {0} was not updated: {1}", seqRef, ex.Message));
        }
    }

    protected void ASPxGridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        string seqRef = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["SeqRef"]);

        try
        {
            // The delete procedure names its key @SeqNo
            Exec("SpNapsDeleteExtAcctTransProfile", new SqlParameter("@SeqNo", seqRef));

            ASPxGridView1.EditIndex = -1;
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Profile {0} deleted.", seqRef));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Profile {0} was not deleted: {1}", seqRef, ex.Message));
        }
    }

    protected void ASPxGridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow || (e.Row.RowState & DataControlRowState.Edit) == 0) return;

        // Pick the current type in the edit row (ignores unexpected values instead of failing)
        DropDownList type = e.Row.FindControl("drpEditTransType") as DropDownList;
        if (type == null) return;
        ListItem item = type.Items.FindByValue(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TransType")).Trim());
        if (item != null) { type.ClearSelection(); item.Selected = true; }
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    protected string TypeName(object value)
    {
        string v = Convert.ToString(value).Trim();
        if (v == "D") return "Debit";
        if (v == "C") return "Credit";
        return HttpUtility.HtmlEncode(v);
    }

    private static void Exec(string procedure, params SqlParameter[] parameters)
    {
        ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["EazyConnect"];
        using (SqlConnection conn = new SqlConnection(cs.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(procedure, conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
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