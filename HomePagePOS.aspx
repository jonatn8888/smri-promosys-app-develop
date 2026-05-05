<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="HomePagePOS.aspx.vb" Inherits="HomePagePOS" Title="PromoSys :: Home Page" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

    <br />
    
    <div id="divHomeContent">
        <br />
        <div id="divHomeMain" style="width: 70%;position:relative; left: 0px; top: 0px;" class="HomeBlock">
        <asp:Label ID="lblHeader" runat="server" Text="Welcome, " Font-Bold="False" Font-Names="Tahoma" Font-Size="18px"></asp:Label>&nbsp;<br />
            <br />
            <asp:LinkButton ID="lnkGenPOSFile" runat="server" Width="336px">POS Files for Class Discount Promotions</asp:LinkButton><br />
            <br />
            <asp:LinkButton ID="lnkGenCCLPOSFile" runat="server" Width="336px">POS Cancellation Files for Class Discount Promotions</asp:LinkButton><br />
            <br />
            <asp:LinkButton ID="lnkPromoAttachment" runat="server" Width="338px" Enabled="True">Files from Other Types of Promotions</asp:LinkButton><br />
            &nbsp;</div>
           
        <br />
        <div id="divHomeCalendar" style="width:628px;position:static; left: 43px; top: 393px;" class="HomeBlock">
            <strong><span style="font-size: 11pt">
            Calendar of Events<br />
            </span></strong>
            <br />
            <br />
            <br />
            &nbsp;</div>
        
        <br />
        <div id="divHomeDownloads" style="width:150px;position:relative; left: -13px; top: -290px; float: right;" class="HomeBlock">
            <br />
            Browser Upgrade<br />
            <br />
            Adobe Acrobat Reader<br />
            &nbsp;</div>

        <div id="divHomeHelp" style="width:150px; position:relative; left: 158px; top: -148px; float: right;" class="HomeBlock">
            <br />
            <a href="SystemHelp.aspx">Getting Started</a><br />
            <br />
            <a href="SystemAbout.aspx">About PromoSys</a><br />
            <br />
            <a href="SystemSupport.aspx">Contact Support</a><br />
        </div>
        
        <br />
        <br />
        <br />
                
    </div>
    
    <br />
    &nbsp;<br />
</asp:Content>