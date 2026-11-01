using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraPrinting.BarCode;
using iTextSharp.text.pdf;
using StandardizedQR;
using QRCoder;
using System.Data;
using System.Data.SqlClient;


public partial class Administration_GenQRCode :  System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        subMerchload(txtTitle.Text);
    }

    void subMerchload(string strsubmch_no)
    {

        DataSet ds = new DataSet();
        SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["EazyConnect"].ConnectionString);


        string Emvco_code = txtUrl.Text; string merchantName = txtTitle.Text;


        //string strinfo = string.Format("SpQRSubMerchantGetDet '{0}'", strsubmch_no);
        //SqlDataAdapter cmd = new SqlDataAdapter(strinfo, connection);
        //cmd.Fill(ds, "TblAccountDet");
        //foreach (DataRow item in ds.Tables[0].Rows)
        //{
        //    Emvco_code = item["Emvco_code"].ToString();
        //    merchantName = item["merchantName"].ToString();

        //}

        EMVQR.Services.Encoding.MerchantEncoder ks = new EMVQR.Services.Encoding.MerchantEncoder();
        emv_qrcps.QrCode.Merchant.MerchantEMVQR jp = new emv_qrcps.QrCode.Merchant.MerchantEMVQR();

        var globalUniqueIdentifier = Guid.NewGuid().ToString().Replace("-", string.Empty);
        var merchantPayload = MerchantPayload.CreateStatic(
             merchantGlobalUniqueIdentifier: globalUniqueIdentifier,
    merchantCategoryCode: 4111,
    transactionCurrencyNumericCode: 1,
    //transactionCurrencyNumericCode: Iso4217Currency.Nigeria,
    countryCode: Iso3166Countries.Nigeria,
    merchantName: merchantName,
    merchantCity: "Lagos City");

        var payload = merchantPayload.GeneratePayload();


        Bitmap bitmaps;
        string value = Emvco_code;// "0002010102111531**999166**999166****M000002758526370018NG.COM.NIBSSPLC.QR0111S000001595252040000530356654035875802NG5916JLMB MCH250420246007Nigeria630483FE";



        QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCoder.QRCodeGenerator.ECCLevel.Q);


        Base64QRCode qrCode = new Base64QRCode(qrCodeData);

        string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
        imgqrcode.ImageUrl = "data:image/png;base64," + qrCodeImageAsBase64;


    }
}