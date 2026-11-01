<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsWig.ascx.cs" Inherits="Widgets_NewsWig" %>
<%@ Register assembly="DevExpress.Web.v19.1, Version=19.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<script runat="server">
    protected void Page_Init(object sender, EventArgs e) {
        ASPxNewsControl.ItemSettings.DateVerticalPosition = DevExpress.Web.DateVerticalPosition.BelowHeader;
        ASPxNewsControl.ItemSettings.DateHorizontalPosition = DevExpress.Web.DateHorizontalPosition.Right;
        ASPxNewsControl.Paddings.Padding = 10;
        ASPxNewsControl.ContentStyle.Paddings.Padding = 0;
    }
</script>
<dx:aspxnewscontrol ID="ASPxNewsControl" runat="server" NavigateUrlFormatString="javascript:void('{0}');"
    Width="100%" RowPerPage="5">
    <ItemSettings ImagePosition="Left" MaxLength="120" TailText="Details">
    </ItemSettings>
    <Items>
        <dx:newsitem   Image-Url="../Images/ROYALM.jpg" Image-Height="80px" Image-Width="80px"
            HeaderText="Third Party Only"
            Text="This policy covers the third party against bodily injury and death resulting from a car accident caused by the insured .">
            
<Image Height="80px" Width="80px" Url="../Images/ROYALM.jpg"></Image>
            
        </dx:NewsItem>
        <dx:newsitem    Image-Url="../Images/Royal Motor Guard-3_.jpg" Image-Height="80px" Image-Width="80px"
            HeaderText="Third Party Damage"
            Text="This covers the third party against bodily injury and death resulting from a car accident caused by the insured and it also covers the insured’s car against damage to his/her car resulting from an accident.">
<%--<Image Url="../Images/The Royal Motor Guard-3_.jpg"></Image>--%>
<Image Height="80px" Width="80px" Url="../Images/Royal Motor Guard-3_.jpg"></Image>
        </dx:NewsItem>
    <dx:newsitem   Image-Url="../Images/The Royal Motor Guard-3_.jpg" Image-Height="80px" Image-Width="80px"
            HeaderText="Third Party Theft Only"
            Text="This covers the third party against bodily injury and death resulting from a car accident caused by the insured and it also covers the insured against theft to his/her car.  ">
 
<Image Height="80px" Width="80px" Url="../Images/The Royal Motor Guard-3_.jpg"></Image>
 
        </dx:NewsItem>
        <dx:newsitem   Image-Url="../Images/The Royal Heritage Plan-5_.jpg" 
            Image-Height="80px" Image-Width="80px"
            HeaderText="The Royal Heritage plan "
            
            Text="This is a unique welfare package that ensures financial security for you and your family ...&nbsp;&nbsp;" >
 
<Image Height="80px" Width="80px" Url="~/Images/The Royal Heritage Plan-5_.jpg"></Image>
 
        </dx:NewsItem>
    </Items>
    <PagerSettings>
        <AllButton Text="">
        </AllButton>
        <FirstPageButton Text="">
        </FirstPageButton>
        <LastPageButton Text="">
        </LastPageButton>
        <NextPageButton Text="">
        </NextPageButton>
        <PrevPageButton Text="">
        </PrevPageButton>
    </PagerSettings>

<Border BorderStyle="None"></Border>
</dx:aspxnewscontrol>

