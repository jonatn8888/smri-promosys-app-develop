<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoRequest.aspx.vb" Inherits="PromoRequest" Title ="Sales Promotions Request Entry" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
<script type="text/javascript">

    function OpenViewBranches(height,width)
    {
        if (height == "") height= "250";
       
        if (width == "") width= "650";
        
        var title
        var page
        page = "PromoRequestBranches.aspx";
        title = "Branches";
        Auditwindow=dhtmlmodal.open('Branches', 'iframe', page, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
        Auditwindow.onclose=function()
        { 
	    return true 
	    }
    }


    function openAttachment()
    {
        var url
        url = "UploadedFiles.aspx";
        attachmentwindow = dhtmlmodal.open('UploadedFiles', 'iframe', url, 'File Attached', 'width=490px,height=305px,center=1,resize=0,scrolling=0','recall')
        
        attachmentwindow.onclose=function()
        { 
        var theform = this.contentDoc.forms[0] 
        document.getElementById("<%=btnDownload.ClientID%>").click();
        return true 
        }
    }
    
    /************************************************    
    function AllowNumericOnly(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

        return true;
    }
    **************************************************/
    
</script>

<script type="text/javascript">
function OpenAuditLogs(height,width,entryid,entrytype)
{
    if (height == "") height= "250";
   
    if (width == "") width= "650";
    
    var title
    var page
    page = "AuditLogs.aspx?EntryID=" + entryid + "&EntryType=" + entrytype;
    title = "Audit Logs";
    Auditwindow=dhtmlmodal.open('Audit', 'iframe', page, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=1,scrolling=0',"recall")
    Auditwindow.onclose=function()
    { 
	    return true 
	}
}

function OpenDetailsUPC(height,width,requestid)
{
    if (height == "") height= "250";
   
    if (width == "") width= "650";
    
    var title
    var page
    page = "ViewUPCdetails.aspx?RequestID=" + requestid;
    title = "Promo Details: UPC";
    UPCwindow=dhtmlmodal.open('UPCdetails', 'iframe', page, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=1,scrolling=0',"recall")
    UPCwindow.onclose=function()
    { 
	    return true 
	}
}
</script>

<script type="text/javascript">

function openChoice()
{
    var url
    url = "PromoDetails.aspx";
	emailwindow=dhtmlmodal.open('InputFlow', 'iframe', url, 'Entry Type', 'width=350px,height=140px,center=1,resize=0,scrolling=0',"recall")
    emailwindow.onclose=function(){var theform = this.contentDoc.forms[0] 
	                            var theChoice = this.contentDoc.getElementById("hidFlag")   
	                            if (theChoice.value == 1){document.getElementById("<%=bclick.ClientID%>").click();}
	                            else{document.getElementById("<%=standard.ClientID%>").click();}
                                return true}
}
</script>

<script type="text/javascript">
function opentexteditor(height,width,urladd)
{
    if (height == "") height= "163";
    
    if (width == "") width= "510";
    
    var url
    var title
    url = "MsgBox.aspx"+ urladd;
    title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
    MsgBoxwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    MsgBoxwindow.onclose = function()
                            { 
                                var theform = this.contentDoc.forms[0] 
                                document.getElementById("<%=cmdDelete.ClientID%>").click(); 
                                return true 
                            }
}
</script>

<script type="text/javascript">
function openinputbox(height,width)
{
    if (height == "") height= "200";
    
    if (width == "") width= "510";
    
    var url
    var title
    url = "InputBox.aspx";
    title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
    InputBoxwindow=dhtmlmodal.open('InputBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    InputBoxwindow.onclose=function()
                            { 
                                var theform = this.contentDoc.forms[0] 
                                var Inputbox = this.contentDoc.getElementById("txtInputbox") 
                                document.getElementById("<%=hidinputbox.ClientID%>").value = Inputbox.value;
                                document.getElementById("<%=cmdPopUpOK.ClientID%>").click(); 
	                            return true 
	                        }
}
</script>
<script language="javascript" type="text/javascript">
<!--

function cmdAddPromo_onclick() {
  var WinSettings = "center:yes;resizable:no;dialogHeight:500px;dialogwidth:700px;status:no";

  var MyArgs = window.showModalDialog("PromoEntry.aspx", 0, WinSettings);
}

function cmdSave_onclick() {
    window.alert("Promo Request saved and forwarded for approval.");
}

function confirm_action(msg)
{
    return confirm(msg);
}

// -->
</script>

<script type="text/javascript">
    function openSwipestakesMessage()
    {
        var url
        url = "PromoRequestSwipestakesMessage.aspx";
        title = "Swipestakes Message";
	    seededitorwindow=dhtmlmodal.open('PromoRequestSwipestakesMessage', 'iframe', url, 'Swipestakes Message', 'width=620px,height=412px,center=1,resize=0,scrolling=0',"recall")
        seededitorwindow.onclose=function()
        { 

	        return true 
	    }
    }
</script>

<script type="text/javascript">
    function openSwipestakesSeed()
    {
        var url
        url = "PromoRequestSwipestakesSeed.aspx";
	    seedWindow=dhtmlmodal.open('PromoRequestSwipestakesSeed', 'iframe', url, 'Seeding', 'width=618px,height=560px,center=1,resize=0,scrolling=0',"recall")
        seedWindow.onclose=function()
        { 
	        return true 
	    }
    }
    function openBinRange()
    {
        var url
        url = "PromoRequestBinRange.aspx";
	    seedWindow=dhtmlmodal.open('PromoRequestBinRange', 'iframe', url, 'Bin Range', 'width=620px,height=325px,center=1,resize=0,scrolling=0',"recall")
        seedWindow.onclose=function()
        { 
	        return true 
	    }
    }
</script>


        <table id="doc-table" cellpadding="3px" cellspacing="0px">
            <tr>
                <td colspan="2" class="DocTabHeadOn">
                    <strong><span style="font-size: 11pt; color: #ffffff">Sales Promotion Request</span></strong></td>
            </tr>
            <tr>
                <td style="width: 132px; height: 14px;">&nbsp;
                </td>
                <td style="width: 573px; text-align: right; height: 14px;">
                    <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Names="Tahoma" ForeColor="Green"
                        Width="207px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px; height: 10px;">
                    <span style="font-size: 10pt"><strong>Request ID:</strong></span></td>
                <td  class="field-cell" style="width: 573px">
                    <span style="font-size: 10pt">PR-</span><asp:Label ID="lblRequestID"
                                    runat="server" Width="177px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px; height: 10px;">
                    <span style="font-size: 10pt"><strong>Request Date:</strong></span></td>
                <td  class="field-cell" style="width: 573px">
                <asp:Label ID="lblRequestDate" runat="server" Width="217px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px">
                    &nbsp;</td>
                <td class="field-cell" style="width: 573px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 132px; height: 11px">
                    <span style="font-size: 10pt;"><strong>Title:</strong></span></td>
                <td style="width: 573px; height: 11px" class="field-cell" >
                    <asp:Label ID="lblPromoTitle" runat="server" BorderColor="SteelBlue" Width="564px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px">
                    <span style="font-size: 10pt"><strong>Promo Period:</strong></span></td>
                <td style="width: 573px"  class="field-cell">
                    <asp:Label ID="lblPeriodFrom" runat="server" BorderColor="SteelBlue" Width="192px"></asp:Label>
                                <asp:Label ID="lblPeriodTo" runat="server" BorderColor="SteelBlue"
                                    Width="192px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px; height: 47px;" valign="top">
                    <span style="font-size: 10pt"><strong>Branches:</strong></span></td>
                <td style="width: 573px; height: 47px;"  class="field-cell">
                                    <asp:Label ID="lblBranches" runat="server" BorderColor="SteelBlue"
                                        Height="57px" Width="565px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px; height: 182px" valign="top">
                    <span style="font-size: 10pt"><strong>Promotions:</strong></span></td>
                <td style="width: 573px; height: 182px"  class="field-cell" valign="top">
                                    <asp:GridView ID="gridPromotions" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                        BorderWidth="2px" CellPadding="4" DataKeyNames="PromoID" DataSourceID="sqldsPromos"
                                        ForeColor="#333333" Width="566px">
                                        <RowStyle BackColor="WhiteSmoke" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowSel" runat="server" Width="10px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPromoID" runat="server" Text='<%# Eval("PromoID") %>' Width="10px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl" ReadOnly="True" SortExpression="ItemCode" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FormatPromoDesc" HeaderText="Description" SortExpression="FormatPromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle Wrap="True" HorizontalAlign="Center" />
                                                <ItemStyle Width="100%" Wrap="True" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            <asp:CheckBox ID="chkRowSel" runat="server" />
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                    </asp:GridView>
                                    
                                    <asp:SqlDataSource ID="sqldsPromos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, dbo.Fn_FormatPromoDesc(P.PromoID,@UserID) AS FormatPromoDesc, P.PromoTypeID&#13;&#10;FROM Promotions AS P LEFT JOIN PromoDetails AS D&#13;&#10;ON P.PromoID = D.PromoID&#13;&#10;WHERE P.RequestID = @RequestID&#13;&#10;ORDER BY P.PromoID">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="RequestID" SessionField="CurrRequestID" />
                                            <asp:SessionParameter Name="UserID" SessionField="UserID" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                </td>
            </tr>
            <tr id="trGuidelines" runat="server">
                <td style="width: 132px" valign="top">
                    <span style="font-size: 10pt"><strong>Guidelines:</strong></span></td>
                <td style="width: 573px" class="field-cell">
                    <asp:Literal ID="litGuidelines" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td style="width: 132px" valign="top">
                    <span style="font-size: 10pt"><strong>Requested By:</strong></span></td>
                <td style="width: 573px"  class="field-cell">
                                <asp:Label ID="lblRequestedBy" runat="server" BorderColor="SteelBlue"
                                    Width="543px"></asp:Label></td>
            </tr>
            <tr id="tbrowReviewer" runat="server">
                <td style="width: 132px" valign="top">
                    <span style="font-size: 10pt"><strong>Reviewed By:</strong></span></td>
                <td style="width: 573px"  class="field-cell">
                                <asp:Label ID="lblReviewedBy" runat="server" BorderColor="SteelBlue"
                                    Width="543px"></asp:Label></td>
            </tr>
            <tr id="tbrowApprover" runat="server">
                <td style="width: 132px" valign="top">
                    <span style="font-size: 10pt"><strong>Approved By:</strong></span></td>
                <td style="width: 573px"  class="field-cell">
                                <asp:Label ID="lblApprovedBy" runat="server" BorderColor="SteelBlue"
                                    Width="543px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px" valign="top">
                    <strong><span style="font-size: 10pt">Remarks:</span></strong></td>
                <td  class="field-cell" style="width: 573px">
                    <asp:Label ID="lblRemarks" runat="server" BorderColor="SteelBlue" Width="543px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 132px" valign="top">
                    <strong><span style="font-size: 10pt">Attached Files:</span></strong></td>
                <td class="field-cell" style="width: 573px">
                    <asp:ImageButton ID="imgbtnDownload" runat="server" ImageUrl="~/Images/down.gif"
                        Visible="False" BorderColor="White" BorderStyle="Solid" BackColor="White" ToolTip="Download attached files" />
                    <asp:LinkButton ID="lnkattachment" runat="server" OnClientClick="return openAttachment();"
                        Visible="False">Add Attachment(s)</asp:LinkButton><br />
                    <asp:Literal ID="lblFiles" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 132px" valign="top">
                    <strong><span style="font-size: 10pt">Attached Seed Files:</span></strong></td>
                <td class="field-cell" style="width: 573px">
                    <asp:ImageButton ID="imgbtnSeedDownload" runat="server" ImageUrl="~/Images/down.gif"
                        Visible="False" BorderColor="White" BorderStyle="Solid" BackColor="White" ToolTip="Download attached files" />
                    <asp:LinkButton ID="linkSeedAttachment" runat="server"
                        Visible="False">Add Attachment(s)</asp:LinkButton><br />
                    <asp:Literal ID="lblSeedFile" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td style="width: 132px" valign="top">
                </td>
                <td style="width: 573px">
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="432px">
                    </asp:BulletedList>
                </td>
            </tr>
        </table>

    <br />
    <br /> 
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkApprove" runat="server" Visible="False">Approve Request</asp:LinkButton><br />
        <asp:LinkButton ID="lnkEditDoc" runat="server">Make Changes</asp:LinkButton><br />
        <asp:LinkButton ID="lnkAllowRush" runat="server" Visible="False">Allow Late Request</asp:LinkButton><br /><br />
		<asp:LinkButton ID="lnkAllowRushWCD" runat="server" Visible="False">Allow Late Request with Change Date</asp:LinkButton><br /><br />
        <asp:LinkButton ID="lnkCancelRequest" runat="server">Return to Sender</asp:LinkButton><br />
        <asp:LinkButton ID="lnkDelete" runat="server" Visible="False">Delete Request Draft</asp:LinkButton><br />
        <br />
        <asp:LinkButton ID="lnkPrintReq" runat="server">Print this Document</asp:LinkButton>
        <br />
        <br />
        <asp:LinkButton ID="lnkSubmitRequest" runat="server" Visible="False">Submit Request</asp:LinkButton><br />
        <asp:LinkButton ID="lnkCreateMemoDraft" runat="server" Visible="False">Create Memo Draft</asp:LinkButton><br />
        <br />
            <asp:Panel ID="panSwipestakesMenu" runat="server" Visible=false>
                <asp:LinkButton ID="linkBinRange" runat="server">View Bin Range</asp:LinkButton><br />
                <br />
                <asp:LinkButton ID="linkSwipestakesMessage" runat="server">View Messages</asp:LinkButton><br />
                <br />
                <asp:LinkButton ID="linkSwipestakesSeed" runat="server">View Seeding</asp:LinkButton><br />
            </asp:Panel>
            <asp:Panel ID="panRebateMenu" runat="server" Visible=false>
                <asp:LinkButton ID="linkRebateBinRange" runat="server">View Bin Range</asp:LinkButton><br />
                <br />
                <br />
            </asp:Panel>
        <br />
        <asp:LinkButton ID="lnkDetailsUPC" runat="server">View UPC Details</asp:LinkButton><br />
        <br />
        <asp:LinkButton ID="lnkViewBranches" runat="server">View Branches</asp:LinkButton><br />
        <br />
        <asp:LinkButton ID="lnkHistory" runat="server">Transaction History</asp:LinkButton><br />
        <br />
        <br />
        <asp:LinkButton ID="lnkClose" runat="server">Close and Go Back</asp:LinkButton><br />
        <br /> <br />
    </div>
    <div id="contents">
                    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT * FROM Users"></asp:SqlDataSource>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <asp:HiddenField ID="hidinputbox" runat="server" />
        <br />
        <div style="display: none">
            <asp:Button ID="cmdPopUpOK" runat="server" Text="cmdPopUpOK" />
            <asp:Button ID="cmdDelete" runat="server" Text="cmdDelete" />
            <asp:Button ID="bclick" runat="server" Text="bclick" />
            <asp:Button ID="standard" runat="server" Text="Standard" />
            <asp:Button ID="btnDownload" runat="server" Text="btnDownload" />
            <asp:Button ID="btnSeedDownload" runat="server" Text="Seed Download" />
        </div>
            
        </div>

</asp:Content>