using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using QRCoder;

/// <summary>Printable QR code for a sub-merchant (?p=sub-merchant number).</summary>
public partial class Administration_QRCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadQr(Request.QueryString["p"] ?? "");
    }

    private void LoadQr(string key)
    {
        if (key.Trim() == "")
        {
            ShowError("No QR code was requested.");
            return;
        }

        try
        {
            string emvco = "", merchantName = "";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SpQRSubMerchantGetDet", conn))
            {
                // Same procedure as before, now passed as a parameter value instead of pasted into SQL
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "EXEC SpQRSubMerchantGetDet @p";
                cmd.Parameters.Add(new SqlParameter("@p", key));
                DataTable dt = new DataTable();
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
                foreach (DataRow row in dt.Rows)
                {
                    emvco = Convert.ToString(row["Emvco_code"]);
                    merchantName = Convert.ToString(row["merchantName"]);
                }
            }

            if (emvco == "")
            {
                ShowError("No QR code was found for this sub-merchant.");
                return;
            }

            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData data = generator.CreateQrCode(emvco, QRCodeGenerator.ECCLevel.Q);
            imgqrcode.ImageUrl = "data:image/png;base64," + new Base64QRCode(data).GetGraphic(20);

            litMerchant.Text = HttpUtility.HtmlEncode(merchantName);
            litRef.Text = HttpUtility.HtmlEncode(key);
            Title = merchantName == "" ? "QR code" : merchantName + " QR code";
        }
        catch (Exception ex)
        {
            ShowError("The QR code could not be loaded: " + ex.Message);
        }
    }

    private void ShowError(string message)
    {
        pnlQr.Visible = false;
        litError.Text = HttpUtility.HtmlEncode(message);
    }
}