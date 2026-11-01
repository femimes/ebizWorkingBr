using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using NETClass;

public partial class Processing_FrmSecQuestion : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string code = txtQuizSNo.Text.Trim();
        string question = txtQuizquestion.Text.Trim();

        if (code == "") { ShowError("Question code cannot be empty."); return; }
        if (question == "") { ShowError("Question cannot be empty."); return; }

        try
        {
            LoadCurrentUser();

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("Sproc_AddAppQuiz", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@QuizSNo", code));
                cmd.Parameters.Add(new SqlParameter("@Quizquestion", question));
                cmd.Parameters.Add(new SqlParameter("@Status", chkStatus.Checked ? 1 : 0));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string msg = string.Format("New Application Security Questions Modification on Code- {0}; Question: {1}", code, question);
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '044','{0}','{1}',''", Sql(msg), Sql(guser)));

            ClearForm();
            ASPxGridView1.DataBind();
            litResult.Text = HttpUtility.HtmlEncode(string.Format("Question {0} added.", code));
            litError.Text = "";
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Question {0} was not added: {1}", code, ex.Message));
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        litResult.Text = "";
        litError.Text = "";
    }

    private void ClearForm()
    {
        txtQuizSNo.Text = "";
        txtQuizquestion.Text = "";
        chkStatus.Checked = true;
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

    private void LoadCurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");

        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        guser = struser;
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