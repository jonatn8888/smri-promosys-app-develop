<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoMemo.aspx.vb" Inherits="PromoMemo" Title ="Promotional Memo" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>

<script type="text/javascript">
function OpenViewBranches(height,width)
{
    if (height == "") height= "250";
   
    if (width == "") width= "550";
    
    var title
    var page
    page = "MemoBranches.aspx";
    title = "Branches";
    Auditwindow=dhtmlmodal.open('Branches', 'iframe', page, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
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
</script>
<script type="text/javascript">
function opentexteditor(height,width)
{
    if (height == "") height= "163";
    
    if (width == "") width= "510";
    
    var url
    var title
    url = "MsgBox.aspx";
    title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
    MsgBoxwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    MsgBoxwindow.onclose =  function()
                            { 
                            var theform = this.contentDoc.forms[0] 
                            document.getElementById("<%=cmdRedirect.ClientID%>").click(); 
	                        return true 
	                        }
}
</script>

<script type="text/javascript">
function openinputbox(height,width)
{
    if (height == "") height= "280";
    
    if (width == "") width= "510";
    
    var url
    var title
    url = "InputBoxRadio.aspx";
    title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
    InputBoxwindow=dhtmlmodal.open('InputBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    InputBoxwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    var Inputbox = this.contentDoc.getElementById("txtInputbox") 
    var RadioBtn = this.contentDoc.getElementById("hidchoice") 
    
    document.getElementById("<%=hidinputbox.ClientID%>").value = Inputbox.value;
    document.getElementById("<%=hidRadioButton.ClientID%>").value = RadioBtn.value;
    document.getElementById("<%=cmdPopUpOK.ClientID%>").click(); 
	return true 
	}
}
</script>

<script language="javascript" type="text/javascript">
<!--

function cmdBranches_onclick() {
  
  return confirm("This will override the branch settings of promos under this request. Continue?");
  
}

function cmdAddPromo_onclick() {
  var WinSettings = "center:yes;resizable:no;dialogHeight:500px;dialogwidth:700px;status:no";

  var MyArgs = window.showModalDialog("PromoEntry.aspx", 0, WinSettings);
}

function cmdSave_onclick() {
    window.alert("Promo Request saved and forwarded for approval.");
}

// -->
</script>


<script type="text/javascript">
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
    <br />
    <br /> 
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkEditMemo" runat="server" Height="5px" Visible="False">Make Changes</asp:LinkButton><br />
        &nbsp;<br />
        <asp:LinkButton ID="lnkForApproval" runat="server" Height="5px" Visible="False">Submit for Approval</asp:LinkButton><br />
        <asp:LinkButton ID="lnkApproveMemo" runat="server" Height="5px" Visible="False">Approve Memo</asp:LinkButton><br />
        <br />
        <asp:LinkButton ID="lnkReturn" runat="server" Height="5px" Visible="False">Return for Revision</asp:LinkButton><br />
        <br />
        <asp:LinkButton ID="lnkPrintMemo" runat="server" Height="5px">Print this Document</asp:LinkButton><br />
        &nbsp;
        <br />
            <asp:Panel ID="panSwipestakesMenu" runat="server" Visible=false>
                <asp:LinkButton ID="linkBinRange" runat="server">View Bin Range</asp:LinkButton><br />
                <br />
                <asp:LinkButton ID="linkSwipestakesMessage" runat="server">View Messages</asp:LinkButton><br />
            </asp:Panel>
            <asp:Panel ID="panRebateMenu" runat="server" Visible=false>
                <asp:LinkButton ID="linkRebateBinRange" runat="server">View Bin Range</asp:LinkButton><br />
                <br />
                <br />
            </asp:Panel>
        <br />
        <asp:LinkButton ID="lnkDetailsUPC" runat="server">View UPC Details</asp:LinkButton><br />
        <asp:LinkButton ID="lnkViewBranches" runat="server">View Branches</asp:LinkButton><br />
        &nbsp;<br />
        <asp:LinkButton ID="lnkHistory" runat="server">Transaction History</asp:LinkButton><br />
        &nbsp;<br />
        <asp:LinkButton ID="lnkDone" runat="server">Back to Memo List</asp:LinkButton><br />
        <br />
        <br />
    </div>
    <div id="contents">
        <table id="doc-table" cellpadding="3px" cellspacing="0px" style="width: 694px">
            <tr>
                <td colspan="2" class="DocTabHeadOn">
                    <strong><span style="font-size: 11pt; color: white; text-align: left;">Promotional Memo</span></strong></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    &nbsp;</td>
                <td style="text-align: right">
                    <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Names="Tahoma" ForeColor="Green"
                        Width="207px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Memo Date:</span></td>
                <td class="field-cell">
                                    <asp:Label ID="lblMemoDate" runat="server" Width="232px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Memo ID:</span></td>
                <td class="field-cell">
                <asp:Label ID="lblMemoNumber"
                                    runat="server" Width="232px" Font-Bold="True" Font-Italic="True"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    &nbsp;</td>
                <td class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Title:</span></td>
                <td class="field-cell">
                    <asp:Label ID="lblPromoTitle" runat="server" Width="528px"></asp:Label></td>
            </tr>
            <tr id="trPromoPeriod" runat="server">
                <td style="width: 103px; height: 22px">
                    <span style="font-size: 10pt">Promo Period:</span></td>
                <td class="field-cell" style="height: 22px">
                    <asp:Label ID="lblPromoPeriod" runat="server" Width="78%"></asp:Label>
                    <asp:LinkButton ID="lnkEditPromoPeriod" runat="server" CssClass="action-link" Visible="False"
                        Width="120px">Change Promo Period</asp:LinkButton></td>
            </tr>

            <tr id="trChangePromoPeriod" runat="server">
                <td style="width: 206px; height: 7px">
                    <span style="font-size: 10pt">Promo Period:</span></td>
                <td class="field-cell">
                    <asp:TextBox ID="txtPeriodFrom" runat="server" MaxLength="30"></asp:TextBox>
                    <input id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodFrom');"
                        style="left: 322px; top: 345px" type="button" />
                    <asp:TextBox ID="txtPeriodTo" runat="server" MaxLength="30"></asp:TextBox>
                    <input id="calPeriodTo" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodTo');"
                        style="left: 322px; top: 345px" type="button" /></td>
            </tr>
            
            <tr id="trRedeemPeriod" runat="server">
                <td style="width: 206px;">
                    <span style="font-size: 10pt">Redemption:</span>
                </td>
                <td class="field-cell">
                     <asp:Label ID="lblRedeemPeriod" runat="server" Width="80%"></asp:Label>
                </td>
            </tr>           
            <tr>
                <td style="width: 103px; height: 13px">
                    &nbsp;</td>
                <td class="field-cell" style="height: 13px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Branches:</span></td>
                <td class="field-cell">
                                    <asp:Label ID="lblBranches" runat="server"
                                        Height="49px" Width="583px"></asp:Label><br />
                    </td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Promotions:</span></td>
                <td class="field-cell">
                                    <asp:GridView ID="gridPromotions" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                        BorderWidth="2px" CellPadding="4" DataKeyNames="PromoID" DataSourceID="sqldsPromos"
                                        ForeColor="#333333" Width="580px">
                                        <RowStyle BackColor="WhiteSmoke" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPromoID" runat="server" Text='<%# Eval("PromoID") %>' Width="10px"></asp:Label>
                                                    <asp:Label ID="lblPromoTypeID" runat="server" Text='<%# Eval("PromoTypeID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl" ReadOnly="True" SortExpression="ItemCode" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FormatPromoDesc" HeaderText="Promo Description" SortExpression="FormatPromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Branches" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="80px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            &nbsp;
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                    </asp:GridView>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Guidelines:</span></td>
                <td class="field-cell">
                    <asp:Literal ID="litGuidelines" runat="server"></asp:Literal><br />
                                </td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt"><asp:Label ID="lblPreparedBy_Label" runat="server" BorderWidth="0px" Width="95%">Prepared By:</asp:Label></span>
                </td>
                <td class="field-cell">
                    <asp:Label ID="lblPreparedBy" runat="server" BorderWidth="0px" Width="464px"></asp:Label>
                </td>
            </tr>

            <tr id="tbrowReviewedLine" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt"><asp:Label id="lblReviewedBy_Label" runat="server" Width="95%" BorderWidth="0px">Reviewed By:</asp:Label></span>
                </td>
                <td class="field-cell">
                    <asp:Label ID="lblReviewedBy" runat="server" BorderWidth="0px" Width="464px"></asp:Label>
                </td>
            </tr>
            <tr id="tbrowApprovedLine" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt"><asp:Label id="lblApprovedBy_Label" runat="server" Width="95%" BorderWidth="0px">Approved By:</asp:Label></span>
                </td>
                <td class="field-cell">
                    <asp:Label ID="lblApprovedBy" runat="server" BorderWidth="0px" Width="464px"></asp:Label>
                </td>
            </tr>

            <tr>
                <td style="width: 103px; height: 3px">
                    <span style="font-size: 10pt">Remarks:</span></td>
                <td class="field-cell" style="height: 3px">
                    &nbsp;<asp:Literal ID="lblRemarks" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 3px">
                    <span style="font-size: 10pt">Attached Files:</span></td>
                <td class="field-cell" style="height: 3px">
                    <asp:ImageButton ID="imgbtnDownload" runat="server" BackColor="White" BorderColor="White"
                        BorderStyle="Solid" ImageUrl="~/Images/down.gif" ToolTip="Download attached files"
                        Visible="False" /><asp:Literal ID="lblFiles" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
        <br />
               
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <asp:HiddenField ID="hidinputbox" runat="server" />
        <asp:HiddenField ID="hidRadioButton" runat="server" />
        <br />
                                    <asp:SqlDataSource ID="sqldsPromos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, P.PromoDesc, P.PromoTypeID,  dbo.Fn_FormatPromoDesc(P.PromoID,@UserID) AS FormatPromoDesc&#13;&#10;FROM Promotions AS P Left Outer Join  PromoDetails AS D ON P.PromoID = D.PromoID&#13;&#10;WHERE P.MemoID = @MemoID&#13;&#10;ORDER BY P.PromoID" UpdateCommand="UPDATE Promotions SET MemoID = @MemoID WHERE (RequestID = @RequestID)">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="MemoID" SessionField="CurrMemoID" />
                                            <asp:SessionParameter Name="UserID" SessionField="UserID" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter DefaultValue="0" Name="MemoID" />
                                            <asp:SessionParameter Name="RequestID" SessionField="RequestID" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource><asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT 1"></asp:SqlDataSource>
    </div>
    <div style="display:none"><asp:Button ID="cmdPopUpOK" runat="server" Text="" />
        <asp:Button ID="cmdRedirect" runat="server" /></div>
    
</asp:Content>