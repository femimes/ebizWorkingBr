using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NETClass;

public partial class Processing_FrmAppKYCTiers :  System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            //luser = User.Identity.Name;// User.Identity.Name;
            //StrLocation = this.luser.Substring(this.luser.LastIndexOf("?") + 1);
            //struser = this.luser.Substring(0, this.luser.LastIndexOf("?"));
            //guser = this.struser;
            // txtUserid.Text = HttpContext.Current.User.Identity.Name;
            loadboxes();


        }
    }
    
    public void loadboxes()
    {
        DataSet ds = new DataSet();

        ds = NETClass.Classes.GetDataset(string.Format("Sproc_GetUnselectedKYCTiers '{0}'",  drpTiers.SelectedItem.Value));
        ListBox1.DataSource = ds.Tables[0];
        ListBox1.DataTextField = "Keydesc";
        ListBox1.DataValueField = "Keyid";
        ListBox1.DataBind();



        ds = NETClass.Classes.GetDataset(string.Format("Sproc_GetselectedKYCTiers '{0}'", drpTiers.SelectedItem.Value));

        ListBox2.DataSource = ds.Tables[0];
        ListBox2.DataTextField = "Keydesc";
        ListBox2.DataValueField = "Keyid";
        ListBox2.DataBind();

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCons);
        DataSet ds = new DataSet();
        if (ListBox1.SelectedIndex != -1)
        {

      SqlCommand cmd = new SqlCommand();
        Conn.Open();
        //SqlParameter result;
        cmd.CommandText = "Sproc_AddselectedKYCTiers";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@UserTierstatus", drpTiers.SelectedItem.Value));
        cmd.Parameters.Add(new SqlParameter("@keyid", ListBox1.SelectedItem.Value));
        

        cmd.ExecuteNonQuery();
        Conn.Close();
        loadboxes();

        }
  
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        string strCons = System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString;
        SqlConnection Conn = new SqlConnection(strCons);
        DataSet ds = new DataSet();
        if (ListBox2.SelectedIndex != -1)
        {

               SqlCommand cmd = new SqlCommand();
        Conn.Open();
        //SqlParameter result;
        cmd.CommandText = "Sproc_DeleteselectedKYCTiers";
        cmd.Connection = Conn;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@UserTierstatus", drpTiers.SelectedItem.Value));
        cmd.Parameters.Add(new SqlParameter("@keyid", ListBox2.SelectedItem.Value));


        cmd.ExecuteNonQuery();
        Conn.Close();
        loadboxes();}
     
    }

    protected void drpTiers_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadboxes();
    }
}