using System;
using System.Diagnostics;
using System.Web;
using NETClass;

public partial class eOperations_FrmTerminalConsole : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    /// <summary>Restarts the server (shutdown /r /t 0), as before, but only after typing RESTART. The request is audited.</summary>
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        if (!string.Equals((ASPxTextBox1.Text ?? "").Trim(), "RESTART", StringComparison.Ordinal))
        {
            litError.Text = "Type RESTART (in capitals) to confirm.";
            return;
        }

        string user = User.Identity.Name ?? "";
        int split = user.LastIndexOf('?');
        if (split >= 0) user = user.Substring(0, split);

        try
        {
            Classes.RecordOp(string.Format("Sproc_AddAuditLog '900','{0}','{1}',''",
                ("Server restart requested from the terminal console").Replace("'", "''"), user.Replace("'", "''")));
        }
        catch
        {
            // The restart still goes ahead if the audit log is unavailable
        }

        try
        {
            Process.Start("shutdown", "/r /t 0");
            litResult.Text = "Restart requested. The server will go offline shortly.";
            ASPxTextBox1.Text = "";
        }
        catch (Exception ex)
        {
            litError.Text = HttpUtility.HtmlEncode("The restart could not be started: " + ex.Message);
        }
    }
}