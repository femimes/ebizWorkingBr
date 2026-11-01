using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

public partial class Approvals_UserKYCFrame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;

        string remseq = (Request.QueryString["p"] ?? "").Trim();
        if (remseq == "")
        {
            ShowError("No document was chosen.");
            return;
        }

        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpGetAppUserKYCDet", conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@remseq", remseq));
                adapter.Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                ShowError("This document was not found.");
                return;
            }

            DataRow row = dt.Rows[0];
            List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
            foreach (DataColumn col in dt.Columns)
                fields.Add(new KeyValuePair<string, string>(col.ColumnName, Convert.ToString(row[col])));
            rptFields.DataSource = fields;
            rptFields.DataBind();

            // Link to the stored file the same way the KYC approval page does
            string path = dt.Columns.Contains("urlVals") ? Convert.ToString(row["urlVals"]).Trim() : "";
            if (path != "")
            {
                string url = DocUrl(path);
                lnkDoc.NavigateUrl = url;
                lnkDoc.Visible = true;
                string ext = Path.GetExtension(path).ToLowerInvariant();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".webp")
                {
                    Image1.ImageUrl = url;
                    Image1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowError("The document could not be loaded: " + ex.Message);
        }
    }

    // Set appSettings "KycDocBaseUrl" to move the store; defaults to the old address
    private static string DocUrl(string path)
    {
        string baseUrl = ConfigurationManager.AppSettings["KycDocBaseUrl"];
        if (string.IsNullOrEmpty(baseUrl)) baseUrl = "http://192.168.1.100/radiuspoints/";
        return baseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
    }

    private void ShowError(string message)
    {
        litError.Text = HttpUtility.HtmlEncode(message);
        pnlDoc.Visible = false;
    }

    private static string Cfg(string name)
    {
        ConnectionStringSettings s = ConfigurationManager.ConnectionStrings[name];
        return s == null ? "" : s.ConnectionString;
    }
}