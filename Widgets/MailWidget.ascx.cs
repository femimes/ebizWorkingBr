using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxSpellChecker;
using NETClass;
public partial class Widgets_MailWidget : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }

    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        if (ASPxMemo1.Text.Trim() != "")
        {
            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
            SqlConnection Conn = new SqlConnection(strCon);
            SqlCommand cmd = new SqlCommand();
            Conn.Open();
            SqlParameter result;
            cmd.CommandText = "Sproc_AddComments";
            cmd.Connection = Conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Subject", "General Comments"));
            cmd.Parameters.Add(new SqlParameter("@Body", string.Format("{0}", ASPxMemo1.Text)));
            cmd.Parameters.Add(new SqlParameter("@CommentDate", DateTime.Today.Date));
            cmd.Parameters.Add(new SqlParameter("@UserId", string.Format("{0}", HttpContext.Current.User.Identity.Name)));

            cmd.ExecuteNonQuery();
            Conn.Close();

            string strmsg = string.Format("Your Request/Comment Successfully Sent");

            Classes.alertwindow(this.Page, strmsg, "Halt");
            ASPxMemo1.Text = null;
        }
        else
        {
            string strmsg = string.Format("Your Request not Sent, Pls enter your comment");

            Classes.alertwindow(this.Page, strmsg, "Halt");
        }
    }
}