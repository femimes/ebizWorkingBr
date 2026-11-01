using System;
using System.Web;
using QRCoder;

/// <summary>Test page: turns an EMVCo QR string into a QR image.</summary>
public partial class Administration_Merchant : System.Web.UI.Page
{
    // Sample NIBSS NQR string the old page always used
    private const string SamplePayload =
        "0002010102111531**999166**999166****M000002758526370018NG.COM.NIBSSPLC.QR0111S000001595252040000530356654035875802NG5916JLMB MCH250420246007Nigeria630483FE";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            txtPayload.Text = SamplePayload;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string payload = (txtPayload.Text ?? "").Trim();
        if (payload == "")
        {
            litError.Text = "Enter a QR string first.";
            pnlQr.Visible = false;
            return;
        }

        try
        {
            imgqrcode.ImageUrl = QrImage(payload);
            ApplySize();
            pnlQr.Visible = true;
            litError.Text = "";
        }
        catch (Exception ex)
        {
            pnlQr.Visible = false;
            litError.Text = HttpUtility.HtmlEncode("The QR code could not be generated: " + ex.Message);
        }
    }

    /// <summary>Resizes the QR code already shown.</summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(imgqrcode.ImageUrl))
        {
            Button1_Click(sender, e);
            return;
        }
        ApplySize();
        pnlQr.Visible = true;
    }

    private void ApplySize()
    {
        int px;
        if (!int.TryParse(drpSize.SelectedValue, out px)) px = 300;
        imgqrcode.Width = px;
        imgqrcode.Height = px;
    }

    private static string QrImage(string payload)
    {
        QRCodeGenerator generator = new QRCodeGenerator();
        QRCodeData data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        Base64QRCode code = new Base64QRCode(data);
        return "data:image/png;base64," + code.GetGraphic(20);
    }
}