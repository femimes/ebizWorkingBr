<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardExtractionDownload.aspx.cs" Inherits="Processing_CardExtractionDownload" %>

<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 80%;
        }
    </style>
    <link href="../Styles/SiteAll2.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="style1">
            <tr>
                <td>
                    <strong>Extraction Files Download</strong></td>
            </tr>
            <tr>
                <td>
    
        <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" 
                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"  >
            <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" >
            </Styles>
            <settings rootfolder="C:\CardExtractionPath" thumbnailfolder="~\Thumb\"   />
      
            <SettingsEditing AllowCopy="true" AllowCreate="true"   AllowDownload="true"   />
            <SettingsToolbar   ShowFilterBox="true"  ShowDownloadButton ="True" />
            <SettingsUpload Enabled="False" >
            </SettingsUpload>
    
            
            <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                <FolderContainerNodeLoadingPanel Url="~/App_Themes/Aqua/Web/tvNodeLoading.gif">
                </FolderContainerNodeLoadingPanel>
                <LoadingPanel Url="~/App_Themes/Aqua/Web/Loading.gif">
                </LoadingPanel>
            </Images>
        </dx:ASPxFileManager>
    
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
