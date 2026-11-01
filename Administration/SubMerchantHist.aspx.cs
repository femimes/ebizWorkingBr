using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using NETClass;

public partial class Administration_SubMerchantHist : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtmch_no.Text = Request.QueryString["p"] ?? "";
            txtsub_no.Text = Request.QueryString["q"] ?? "";
        }
        litSub.Text = HttpUtility.HtmlEncode(txtsub_no.Text == "" ? "(none chosen)" : txtsub_no.Text);
        btnAdd.Enabled = txtsub_no.Text != "";
    }

    // =====================================================================
    // Create a dynamic QR code: NIBSS first, then save locally
    // =====================================================================
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string subMchNo = txtsub_no.Text.Trim();
        if (subMchNo == "") { ShowError("Open this page from a sub-merchant first."); return; }

        string uniqueId = txtUniqueId.Text.Trim();
        string amount = ToDecimal(txtAmount.Text).ToString("0.00", CultureInfo.InvariantCulture);

        try
        {
            LoadCurrentUser();

            NqrResult r = CallNqr("NQR/SubMerchantHist", new
            {
                sub_mch_no = subMchNo,
                sub_amount = amount,
                unique_id = uniqueId
            });

            string orderSn = DataValue(r, "OrderSn");
            Audit("044", string.Format("New Dynamic subMerchant NQRCode Profile Modification on MerchantName- ordersn/Submchno:- {0}/{1}; Message: {2}; AddedBy: {3}",
                orderSn, subMchNo, r.Raw, guser));

            if (r.Code != "00")
            {
                ShowError(string.Format("NIBSS did not create the QR code: {0} ({1})", r.Text == "" ? "no reason given" : r.Text, r.Code));
                return;
            }

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpQRSubMerchantHistAdd", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@sub_mch_no", subMchNo));
                cmd.Parameters.Add(new SqlParameter("@order_no", orderSn));
                cmd.Parameters.Add(new SqlParameter("@sub_amount", amount));
                cmd.Parameters.Add(new SqlParameter("@unique_id", uniqueId));
                cmd.Parameters.Add(new SqlParameter("@Emvco_code", DataValue(r, "CodeUrl")));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            txtUniqueId.Text = "";
            txtAmount.Text = "0.00";
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("QR code created (order {0}).", orderSn));
        }
        catch (Exception ex)
        {
            ShowError("The QR code was not created: " + ex.Message);
        }
    }

    // =====================================================================
    // Delete
    // =====================================================================
    protected void ASPxGridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;
        string seqNo = Convert.ToString(ASPxGridView1.DataKeys[e.RowIndex]["SeqNo"]);

        try
        {
            LoadCurrentUser();
            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpQRSubMerchantHistDelete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SeqNo", seqNo));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            Audit("004", string.Format("Application QR Dynamic subMerchant Deleting on - {0};", seqNo));
            ASPxGridView1.DataBind();
            ShowSuccess("QR code deleted.");
        }
        catch (Exception ex)
        {
            ShowError("The QR code was not deleted: " + ex.Message);
        }
    }

    // =====================================================================
    // NIBSS NQR API
    // =====================================================================
    private sealed class NqrResult
    {
        public string Code = "";
        public string Text = "";
        public Dictionary<string, object> Data;
        public string Raw = "";
    }

    /// <summary>Posts JSON to the NQR host (ORCodeHostUrl) with the AuthToken header, as before.</summary>
    private static NqrResult CallNqr(string path, object body)
    {
        string url = string.Format("{0}/{1}", Cfg("ORCodeHostUrl").TrimEnd('/'), path);
        string user = Classes.DecryptString(Cfg("QRCodeKeyID"));
        string pwd = Classes.DecryptString(Cfg("QRCodeKey"));
        string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user, pwd)));

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers.Add("AuthToken", token);

        JavaScriptSerializer json = new JavaScriptSerializer();
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(json.Serialize(body));
        }

        NqrResult result = new NqrResult();
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            result.Raw = reader.ReadToEnd();
        }

        Dictionary<string, object> parsed = json.Deserialize<Dictionary<string, object>>(result.Raw) ?? new Dictionary<string, object>();
        result.Code = parsed.ContainsKey("responseCode") ? Convert.ToString(parsed["responseCode"]) : "";
        result.Text = parsed.ContainsKey("responseText") ? Convert.ToString(parsed["responseText"]) : "";
        result.Data = parsed.ContainsKey("data") ? parsed["data"] as Dictionary<string, object> : null;
        return result;
    }

    private static string DataValue(NqrResult r, string key)
    {
        if (r.Data == null) return "";
        foreach (KeyValuePair<string, object> kv in r.Data)
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)) return Convert.ToString(kv.Value);
        return "";
    }

    // =====================================================================
    // Helpers
    // =====================================================================
    private void Audit(string code, string message)
    {
        Classes.RecordOp(string.Format("Sproc_AddAuditLog '{0}','{1}','{2}',''", code, Sql(message), Sql(guser)));
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
        return decimal.TryParse((value ?? "").Replace(",", "").Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0m;
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