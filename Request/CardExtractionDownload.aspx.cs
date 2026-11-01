using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Browse and download the card extraction files (replaces the DevExpress ASPxFileManager).
/// Root folder: appSetting "CardExtractionPath" when set, otherwise C:\CardExtractionPath (as before).
/// Read-only: folders can be opened and files downloaded; nothing can be created, copied or uploaded.
/// </summary>
public partial class Processing_CardExtractionDownload : System.Web.UI.Page
{
    private const string DefaultRoot = @"C:\CardExtractionPath";

    private sealed class Entry
    {
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public string Kind { get; set; }
        public long Size { get; set; }
        public string SizeText { get; set; }
        public DateTime Modified { get; set; }
    }

    private static string Root
    {
        get
        {
            string root = ConfigurationManager.AppSettings["CardExtractionPath"];
            if (string.IsNullOrWhiteSpace(root)) root = DefaultRoot;
            return Path.GetFullPath(root.Trim()).TrimEnd(Path.DirectorySeparatorChar);
        }
    }

    /// <summary>Folder being shown, relative to the root ("" = the root itself).</summary>
    private string CurrentRelative
    {
        get { return hidPath.Value ?? ""; }
        set { hidPath.Value = value ?? ""; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        litRoot.Text = "<strong>" + HttpUtility.HtmlEncode(Root) + "</strong>";
        if (!IsPostBack)
        {
            string start = Request.QueryString["path"];
            CurrentRelative = Resolve(start) != null ? Normalise(start) : "";
            BindGrid();
        }
        BuildCrumbs();
    }

    // =====================================================================
    // Paths (everything stays inside the root folder)
    // =====================================================================

    private static string Normalise(string relative)
    {
        relative = (relative ?? "").Replace('/', '\\').Trim('\\', ' ');
        return relative;
    }

    /// <summary>Full path for a path relative to the root, or null if it would leave the root.</summary>
    private static string Resolve(string relative)
    {
        string root = Root;
        relative = Normalise(relative);
        if (relative.IndexOf(':') >= 0) return null;
        string full;
        try { full = Path.GetFullPath(relative == "" ? root : Path.Combine(root, relative)).TrimEnd(Path.DirectorySeparatorChar); }
        catch (Exception) { return null; }
        if (string.Equals(full, root, StringComparison.OrdinalIgnoreCase)) return full;
        return full.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ? full : null;
    }

    private static string Join(string relative, string name)
    {
        relative = Normalise(relative);
        return relative == "" ? name : relative + "\\" + name;
    }

    // =====================================================================
    // Listing
    // =====================================================================

    private void BindGrid()
    {
        litError.Text = "";
        List<Entry> items = new List<Entry>();
        string folder = Resolve(CurrentRelative);
        try
        {
            if (folder == null) throw new InvalidOperationException("That folder is outside the extraction folder.");
            if (!Directory.Exists(folder))
            {
                if (CurrentRelative == "") throw new DirectoryNotFoundException("The extraction folder " + Root + " does not exist on the server.");
                CurrentRelative = "";
                folder = Resolve("");
            }
            DirectoryInfo dir = new DirectoryInfo(folder);
            Regex filter = FilterPattern(txtFilter.Text);
            foreach (DirectoryInfo d in dir.GetDirectories().OrderBy(x => x.Name))
            {
                if ((d.Attributes & FileAttributes.Hidden) != 0) continue;
                if (filter != null && !filter.IsMatch(d.Name)) continue;
                items.Add(new Entry { Name = d.Name, IsFolder = true, Kind = "Folder", SizeText = "", Modified = d.LastWriteTime });
            }
            foreach (FileInfo f in dir.GetFiles().OrderByDescending(x => x.LastWriteTime))
            {
                if ((f.Attributes & FileAttributes.Hidden) != 0) continue;
                if (filter != null && !filter.IsMatch(f.Name)) continue;
                string ext = f.Extension.TrimStart('.').ToUpperInvariant();
                items.Add(new Entry { Name = f.Name, IsFolder = false, Kind = ext == "" ? "File" : ext + " file", Size = f.Length, SizeText = SizeText(f.Length), Modified = f.LastWriteTime });
            }
        }
        catch (Exception ex)
        {
            litError.Text = HttpUtility.HtmlEncode("The folder could not be read: " + ex.Message);
        }
        int folders = items.Count(i => i.IsFolder), files = items.Count - folders;
        litTotal.Text = string.Format("{0:N0} folder{1}, {2:N0} file{3}", folders, folders == 1 ? "" : "s", files, files == 1 ? "" : "s");
        grid.DataSource = items;
        grid.DataBind();
    }

    /// <summary>"*.txt" style wildcards, otherwise a plain "contains" match. Null when there is no filter.</summary>
    private static Regex FilterPattern(string text)
    {
        text = (text ?? "").Trim();
        if (text == "") return null;
        string pattern = text.IndexOfAny(new[] { '*', '?' }) >= 0
            ? "^" + Regex.Escape(text).Replace("\\*", ".*").Replace("\\?", ".") + "$"
            : Regex.Escape(text);
        return new Regex(pattern, RegexOptions.IgnoreCase);
    }

    private static string SizeText(long bytes)
    {
        if (bytes < 1024) return bytes + " B";
        if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("N1") + " KB";
        return (bytes / (1024.0 * 1024.0)).ToString("N1") + " MB";
    }

    private void BuildCrumbs()
    {
        phCrumbs.Controls.Clear();
        AddCrumb("Extraction files", "");
        string path = "";
        foreach (string part in Normalise(CurrentRelative).Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
        {
            phCrumbs.Controls.Add(new LiteralControl("<span class=\"sep\">/</span>"));
            path = Join(path, part);
            AddCrumb(part, path);
        }
    }

    private void AddCrumb(string text, string path)
    {
        LinkButton link = new LinkButton { Text = HttpUtility.HtmlEncode(text), CommandArgument = path, CausesValidation = false, ID = "crumb" + phCrumbs.Controls.Count };
        link.Command += Crumb_Command;
        phCrumbs.Controls.Add(link);
    }

    // =====================================================================
    // Events
    // =====================================================================

    private void Crumb_Command(object sender, CommandEventArgs e)
    {
        OpenFolder(Convert.ToString(e.CommandArgument));
    }

    private void OpenFolder(string relative)
    {
        if (Resolve(relative) == null) { litError.Text = "That folder is outside the extraction folder."; return; }
        CurrentRelative = Normalise(relative);
        txtFilter.Text = "";
        grid.PageIndex = 0;
        BindGrid();
        BuildCrumbs();
    }

    protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string name = Path.GetFileName(Convert.ToString(e.CommandArgument));
        if (string.IsNullOrEmpty(name)) return;
        if (e.CommandName == "Open") OpenFolder(Join(CurrentRelative, name));
        else if (e.CommandName == "Download") Download(Join(CurrentRelative, name));
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        grid.PageIndex = 0;
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtFilter.Text = "";
        grid.PageIndex = 0;
        BindGrid();
    }

    private void Download(string relative)
    {
        string full = Resolve(relative);
        if (full == null || !File.Exists(full))
        {
            litError.Text = "That file no longer exists.";
            BindGrid();
            return;
        }
        string user = Request.Cookies["LogonUserID"] != null ? Request.Cookies["LogonUserID"].Value : "";
        try
        {
            NETClass.Classes.RecordOp(string.Format("Sproc_AddAuditLog 'EXTDNLD','{0}','{1}',''",
                ("Downloaded extraction file " + Normalise(relative)).Replace("'", "''"), user.Replace("'", "''")));
        }
        catch (Exception) { /* auditing must not stop the download */ }

        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(full).Replace("\"", "") + "\"");
        Response.AddHeader("Content-Length", new FileInfo(full).Length.ToString());
        Response.TransmitFile(full);
        Response.Flush();
        Response.End();
    }
}
