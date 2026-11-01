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

public partial class Administration_SubMerchantSetup : System.Web.UI.Page
{
    public string luser;
    public string StrLocation;
    public string struser;
    public string guser;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // The add form only makes sense once a merchant is chosen
        pnlAdd.Visible = drpMchno.SelectedValue != "";
    }

    protected void drpMchno_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSuccess("");
        ASPxGridView1.PageIndex = 0;
        ASPxGridView1.DataBind();
    }

    // =====================================================================
    // Register a sub-merchant: NIBSS first, then save locally
    // =====================================================================
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        string mchNo = drpMchno.SelectedValue;
        if (mchNo == "") { ShowError("Choose a merchant first."); return; }

        string name = txtName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string email = txtEmail.Text.Trim();
        string amount = ToDecimal(txtAmount.Text).ToString("0.00", CultureInfo.InvariantCulture);
        string subFixed = chkFixed.Checked ? "1" : "0";

        try
        {
            LoadCurrentUser();

            NqrResult r = CallNqr("NQR/SubMerchant", new
            {
                name = name,
                phone = phone,
                email = email,
                sub_fixed = subFixed,
                sub_amount = amount,
                mch_no = mchNo
            });

            Audit("044", string.Format("New QRCode subMerchant Profile Modification on MerchantName- {0}; Message: {1}; AddedBy: {2}", name, r.Raw, guser));

            if (r.Code != "00")
            {
                ShowError(string.Format("NIBSS did not register {0}: {1} ({2})", name, r.Text == "" ? "no reason given" : r.Text, r.Code));
                return;
            }

            string subMchNo = DataValue(r, "Sub_mch_no");
            string emvco = DataValue(r, "Emvco_code");

            using (SqlConnection conn = new SqlConnection(Cfg("EazyConnect")))
            using (SqlCommand cmd = new SqlCommand("SpQRSubMerchantAdd", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@name", name));
                cmd.Parameters.Add(new SqlParameter("@phone", phone));
                cmd.Parameters.Add(new SqlParameter("@Mch_no", mchNo));
                cmd.Parameters.Add(new SqlParameter("@email", email));
                cmd.Parameters.Add(new SqlParameter("@sub_amount", amount));
                cmd.Parameters.Add(new SqlParameter("@sub_fixed", subFixed));
                cmd.Parameters.Add(new SqlParameter("@sub_mch_no", subMchNo));
                cmd.Parameters.Add(new SqlParameter("@Emvco_code", emvco));
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ClearForm();
            ASPxGridView1.DataBind();
            ShowSuccess(string.Format("Sub-merchant {0} registered ({1}).", name, subMchNo));
        }
        catch (Exception ex)
        {
            ShowError(string.Format("Sub-merchant {0} was not registered: {1}", name, ex.Message));
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
        ShowSuccess("");
    }

    private void ClearForm()
    {
        txtName.Text = "";
        txtPhone.Text = "";
        txtEmail.Text = "";
        txtAmount.Text = "0.00";
        chkFixed.Checked = false;
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