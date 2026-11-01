using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using NETClass;
using WebRefVanzo;

public partial class Approvals_ApprPinDetails : System.Web.UI.Page
{
    public string gUser;
    public string luser;
    public string StrLocation = "";
    public string struser = "";

    private string RefNo
    {
        get { return (Request.QueryString["RefNo"] ?? "").Trim(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        litRef.Text = HttpUtility.HtmlEncode(RefNo);
        if (!IsPostBack)
        {
            LoadCurrentUser();
            lblUser.Text = gUser;
            LoadDetails();
        }
    }

    private void LoadDetails()
    {
        if (RefNo == "")
        {
            litEmmorMsg.Text = "No batch was chosen. Go back to the list and open a batch.";
            pnlConfirm.Visible = false;
            return;
        }

        DataTable dt = QueryPositional("SpGetAllBatRecPINDetails", RefNo);
        gridDetails.DataSource = dt;
        gridDetails.DataBind();
        litCount.Text = dt.Rows.Count.ToString("N0") + (dt.Rows.Count == 1 ? " record" : " records");
        pnlConfirm.Visible = dt.Rows.Count > 0;
    }

    // Show only the last 4 digits of a card number
    protected string MaskCard(object value)
    {
        string s = Convert.ToString(value).Trim();
        return s.Length <= 4 ? s : new string('\u2022', 4) + " " + s.Substring(s.Length - 4);
    }

    // =====================================================================
    // Approve: check the hardware token, then set status 3
    // =====================================================================
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!LoadCurrentUser()) { litEmmorMsg.Text = "Your session has expired. Sign in again."; return; }
        if (RefNo == "") { litEmmorMsg.Text = "No batch was chosen."; return; }

        try
        {
            string clientId = "";
            foreach (DataRow item in Classes.SqlDbGetDataset("Sproc_GetPref").Tables[0].Rows)
                clientId = item["Token2FAClientId"].ToString();

            TwoFactorService hardToken = new TwoFactorService();
            var respToken = hardToken.doResponseAuthentication(gUser, clientId, psword.Text.Trim());
            psword.Text = "";

            if (respToken == null || Convert.ToString(respToken.status) != "1")
            {
                litEmmorMsg.Text = "Token check failed. Check the code and try again.";
                LoadDetails();
                return;
            }

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpUpdatePINRequest", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BatchRefNo", RefNo));
                cmd.Parameters.Add(new SqlParameter("@status", 3));
                cmd.Parameters.Add(new SqlParameter("@approveUserId", gUser));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("ApprovePinRequest.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            litEmmorMsg.Text = HttpUtility.HtmlEncode("The batch was not approved: " + ex.Message);
            LoadDetails();
        }
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private bool LoadCurrentUser()
    {
        HttpCookie cookie = Request.Cookies["LogonUserID"];
        luser = cookie != null && !string.IsNullOrEmpty(cookie.Value) ? cookie.Value : (User.Identity.Name ?? "");
        int split = luser.LastIndexOf('?');
        struser = split >= 0 ? luser.Substring(0, split) : luser;
        StrLocation = split >= 0 ? luser.Substring(split + 1) : "";
        gUser = struser;
        return gUser != "";
    }

    private static string Cfg(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }

    // Runs a procedure whose parameters are passed by position (names not known)
    private static DataTable QueryPositional(string proc, params object[] values)
    {
        DataTable table = new DataTable();
        using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
        using (SqlCommand cmd = new SqlCommand())
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            List<string> names = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                names.Add("@p" + i);
                cmd.Parameters.Add(new SqlParameter("@p" + i, values[i] ?? DBNull.Value));
            }
            cmd.Connection = conn;
            cmd.CommandText = "EXEC " + proc + (names.Count > 0 ? " " + string.Join(",", names.ToArray()) : "");
            adapter.Fill(table);
        }
        return table;
    }
}