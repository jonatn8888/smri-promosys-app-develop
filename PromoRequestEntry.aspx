<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoRequestEntry.aspx.vb" Inherits="PromoRequestEntry" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>

<script type="text/javascript">
    function msgbox(height,width)
    {
        if (height == "") height= "163";
        
        if (width == "") width= "510";
        
        var url
        var title
        url = "MsgBox.aspx";
        title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
        MsgBoxwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
        MsgBoxwindow.onclose=function()
        { 
        var theform = this.contentDoc.forms[0] 
        document.getElementById("<%=btnProcess.ClientID%>").click(); 
	    return true 
	    }
    }
</script>

<%--<script type="text/javascript">
    function BClick()
    {
        document.getElementById("<%=Button1.ClientID%>").click();
    }   
</script>
    
<script type="text/javascript">
    function BClick2()
    {
        document.getElementById("<%=Button2.ClientID%>").click();
    }
</script>
--%>

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkSaveMemo" runat="server" OnClientClick="return confirm_action('Submit this memo for review?')">Back to Request List</asp:LinkButton><br />
        <br />
        <br />
        <br />
    </div>

    <div id="contents">
                    <table id="doc-table" cellpadding="3px" cellspacing="0px">
                        <tr>
                            <td colspan="2" class="DocTabHeadOn">Sales Promotion Request</td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                &nbsp;</td>
                            <td style="height: 1px; width: 565px;" class="field-cell">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 3px;">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Request ID:</span></td>
                            <td style="height: 3px; text-align: left; width: 565px;"  class="field-cell">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">
                                    <asp:Label ID="lblRequestID" runat="server" Width="203px"></asp:Label></span><span style="font-size: 10pt; font-family: Trebuchet MS"></span><span style="font-size: 10pt; font-family: Trebuchet MS"></span></td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Request Date:</span></td>
                            <td style="width: 565px; height: 1px;"  class="field-cell">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">
                                    <asp:Label ID="lblReqDate" runat="server" Width="204px"></asp:Label></span><span style="font-size: 10pt; font-family: Trebuchet MS"></span></td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                &nbsp;</td>
                            <td style="height: 1px; width: 565px;"  class="field-cell">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 112px;">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Promo Title:</span>
                            </td>
                            <td style="width: 565px;" class="field-cell">
                                <asp:TextBox ID="txtReqTitle" runat="server"
                                    Width="484px"
                                    CssClass="Tb_ToUpper"                            
                                    onkeydown="countWords(this, 'lblWordCount', 75)"
                                    onpaste="var el=this; setTimeout(function(){ countWords(el, 'lblWordCount', 75); }, 0);">
                                </asp:TextBox>
                                <br />
                                <span id="lblWordCount" >0 / 75</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 11px">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Promo Period:</span></td>
                            <td style="width: 565px; height: 11px"  class="field-cell">
                                <asp:TextBox ID="txtPeriodFrom" runat="server" Width="177px" MaxLength="50"></asp:TextBox>
                                <input name="calPeriodFrom" type="button" id="calPeriodFrom" class="btnCal" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodFrom');" style="left: 322px; top: 345px" />&nbsp;
                                <asp:TextBox ID="txtPeriodTo" runat="server" Width="177px" MaxLength="50"></asp:TextBox>
                                <input name="calPeriodTo" type="button" id="calPeriodTo" class="btnCal" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodTo');" style="left: 547px; top: 345px"/></td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                &nbsp;</td>
                            <td style="height: 1px; width: 565px; text-align: left;"  class="field-cell">
                                &nbsp;<asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="456px">
                                </asp:BulletedList>
                                </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 21px; text-align: center" class="DocTabHeadOff">
                                <asp:LinkButton ID="lnkPromoInfo" runat="server" Width="230px">Promotion Information</asp:LinkButton>
                            </td>
                        </tr>
                        <!--Revised dowcarpio08162012@smretailinc: Rename trPromoDetails-->
                        <tr id='trPromoDetails' runat="server"> 
                            <td colspan="2" style="height: 17px; text-align: center" class="DocTabHeadOff">
                                <asp:LinkButton ID="lnkPromoDetails" runat="server" Enabled="False" Width="230px">Promo Details</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 21px; text-align: center" class="DocTabHeadOff">
                                <asp:LinkButton ID="lnkBranches" runat="server" Enabled="False" Width="230px">Branches</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
        </div>
    <asp:HiddenField ID="lblPopTitle" runat="server" />

    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>"></asp:SqlDataSource>
    
    
    <div style="display:none">
        <input id="btnProcess" type="button" runat="server"  value="button" />

        <%-- <input id="Button1" type="button" runat="server" value="button" /> --%>
        <%-- <input id="Button2" type="button" runat="server"  value="button" /> --%>    
    </div>
    
</asp:Content>
