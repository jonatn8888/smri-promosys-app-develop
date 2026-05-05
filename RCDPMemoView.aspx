<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="RCDPMemoView.aspx.vb" Inherits="RCDPMemoView" Title ="Promotional Memo" %>

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
    MsgBoxwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    document.getElementById("<%=cmdPopUpOK.ClientID%>").click(); 
	return true 
	}
}
</script>

<script type="text/javascript">
function opentexteditor2(sActionButton, sMsgTitle)
{
    var url
    url = "texteditor.aspx";
	texteditorwindow=dhtmlmodal.open('texteditor', 'iframe', url, sMsgTitle, 'width=709px,height=500px,center=1,resize=0,scrolling=0','recall')
    texteditorwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    var Mechanics = this.contentDoc.getElementById('HiddenField1') 
    document.getElementById('<%= hidBox.ClientID %>').value = Mechanics.value;
    document.getElementById(sActionButton).click(); 
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

<script type="text/javascript">
function openinputbox2(height,width)
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


<script language="javascript" type="text/javascript">
    function openSwipestakesSeed()
    {
        var url
        url = "PromoRequestSwipestakesSeed.aspx";
	    seedWindow=dhtmlmodal.open('PromoRequestSwipestakesSeed', 'iframe', url, 'Seeding', 'width=620px,height=560px,center=1,resize=0,scrolling=0',"recall")
        seedWindow.onclose=function()
        { 
	        return true 
	    }
    }
    
    function openSwipestakesReseed(queryString)
    {
        var url
        url = "PromoRequestSwipestakesReseed.aspx"+queryString;
	    seedWindow=dhtmlmodal.open('PromoRequestSwipestakesReseed', 'iframe', url, 'Reseeding', 'width=620px,height=325px,center=1,resize=0,scrolling=0',"recall")
        seedWindow.onclose=function()
        { 
	        return true 
	    }
    }

</script>

<table>
    <tr>
        <td rowspan="1" style="width: 20px">
        </td>
        <td>
              <table id="doc-table" cellpadding="3px" cellspacing="0px" style="width: 694px">
                        <tr>
                            <td colspan="2" class="DocTabHeadOn">
                                <strong><span style="font-size: 11pt; color: white; text-align: left;">Memorandum Information
                                    (<asp:Label ID="lblRequstTypeDesc" runat="server"></asp:Label>)</span></strong></td>
                        </tr>
                        <tr>
                            <td style="width: 103px">
                            </td>
                            <td>
                                <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="456px">
                                </asp:BulletedList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 103px">
                            </td>
                            <td style="text-align: right">
                                <asp:Label ID="lblReqStatus" runat="server" Font-Bold="True" Font-Names="Tahoma" ForeColor="Green"
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
                                <asp:Label ID="lblMemoID" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 103px">
                                <span style="font-size: 10pt">Reference Memo:</span></td>
                            <td class="field-cell">
                            <asp:Label ID="lblMemoNumber"
                                                runat="server" Font-Bold="False" Font-Italic="False"></asp:Label>
                                <asp:Label ID="lblPromoTitle" runat="server"></asp:Label></td>
                        </tr>
                        <tr id="trEffectDate" runat="server">
                            <td style="width: 103px">
                                <span style="font-size: 10pt">
                                Effective Date:</span></td>
                            <td class="field-cell">
                                <asp:Label ID="lblCancelEffectDate" runat="server" Width="232px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 103px">
                                <span style="font-size: 10pt">Title:</span></td>
                            <td class="field-cell">
                                <asp:Label ID="lblCancelTitle" runat="server" Width="528px"></asp:Label></td>
                        </tr>
                        <tr id="trDatePeriod" runat="server">
                            <td style="width: 103px; height: 13px">
                                Period Promo:</td>
                            <td class="field-cell" style="height: 13px">
                                <asp:Label ID="lblOldPromoPeriod" runat="server" Width="528px"></asp:Label></td>
                        </tr>
                        <tr id="trExtendedUntil" runat="server">
                            <td style="width: 103px; height: 13px">
                                Extended Until</td>
                            <td class="field-cell" style="height: 13px">
                                <asp:Label ID="lblExtendedUntil" runat="server" Width="232px"></asp:Label></td>
                        </tr>


                        <tr>
                            <td style="width: 103px">
                                <span style="font-size: 10pt">Branches:</span></td>
                            <td class="field-cell">
                                                <asp:Label ID="lblBranches" runat="server"
                                                    Height="49px" Width="583px"></asp:Label><br />
                                </td>
                        </tr>
                        <tr id="trCurrPromotions" runat="server">
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
                                                        <asp:BoundField DataField="PromoDesc" HeaderText="Promo Description" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
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
                                                    
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                </asp:GridView>
                            </td>
                        </tr>
                        <tr  id="trCRPromotions" runat="server">
                            <td id="tdCRPromotions" runat="server" style="width: 103px">
                                <asp:Label ID="lblCRPromoLabel" runat="server"></asp:Label>
                                Promotions:</td>
                            <td class="field-cell">
                                <asp:GridView ID="gridPromotionsAdd" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                    BorderWidth="2px" CellPadding="4" DataKeyNames="PromoID" DataSourceID="sqldsPromosAdd"
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
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PromoDesc" HeaderText="Promo Description" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
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
                                    </EmptyDataTemplate>
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr id="trMechanicsADD" runat="server">
                            <td>
                                Clarification of Promotion:</td>
                            <td class="field-cell">
                                <table cellpadding="0" cellspacing="0" border = "0" style="width: 582px">
                                    <tr>
                                        <td style="width: 50%; height: 19px; text-align: center">
                                            <strong>Instead of...</strong></td>
                                        <td style="width: 50%; height: 19px; text-align: center">
                                            <strong>Should be..</strong></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%">
                                            <DIV style="BORDER-RIGHT: steelblue 1px solid; PADDING-RIGHT: 4px; BORDER-TOP: steelblue 1px solid; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; OVERFLOW: auto; BORDER-LEFT: steelblue 1px solid; PADDING-TOP: 4px; BORDER-BOTTOM: steelblue 1px solid; HEIGHT: 170px; BACKGROUND-COLOR: whitesmoke; width: 300px;">
                                                <asp:Literal id="litMechanicsOld" runat="server"></asp:Literal>
                                            </DIV>
                                            <asp:LinkButton ID="lnkEditMechanicsOld" runat="server" CssClass="action-link">Edit Mechanics (Old)</asp:LinkButton></td>
                                        <td style="width: 50%">
                                            <DIV style="BORDER-RIGHT: steelblue 1px solid; PADDING-RIGHT: 4px; BORDER-TOP: steelblue 1px solid; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; OVERFLOW: auto; BORDER-LEFT: steelblue 1px solid; PADDING-TOP: 4px; BORDER-BOTTOM: steelblue 1px solid; HEIGHT: 170px; BACKGROUND-COLOR: whitesmoke; width: 300px;">
                                                <asp:Literal ID="litMechanicsNew" runat="server"></asp:Literal>
                                            </div>
                                            <asp:LinkButton ID="lnkEditMechanicsNew" runat="server" CssClass="action-link">Edit Mechanics (New)</asp:LinkButton></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trGuidelinesADD" runat="server">
                            <td>
                                Clarification of Guidelines:<br />
                                (Optional)</td>
                            <td class="field-cell">
                                <table cellpadding="0" cellspacing="0" border = "0" style="width: 582px">
                                    <tr>
                                        <td style="width: 50%; height: 19px; text-align: center">
                                            <strong>Instead of...</strong></td>
                                        <td style="width: 50%; height: 19px; text-align: center">
                                            <strong>Should be..</strong></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%">
                                            <DIV style="BORDER-RIGHT: steelblue 1px solid; PADDING-RIGHT: 4px; BORDER-TOP: steelblue 1px solid; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; OVERFLOW: auto; BORDER-LEFT: steelblue 1px solid; PADDING-TOP: 4px; BORDER-BOTTOM: steelblue 1px solid; HEIGHT: 170px; BACKGROUND-COLOR: whitesmoke; width: 300px;">
                                                <asp:Literal id="LitGuidelinesOld" runat="server"></asp:Literal>
                                            </DIV>
                                            <asp:LinkButton ID="lnkEditGuidelinesOld" runat="server" CssClass="action-link">Edit Guidelines (Old)</asp:LinkButton></td>
                                        <td style="width: 50%">
                                            <DIV style="BORDER-RIGHT: steelblue 1px solid; PADDING-RIGHT: 4px; BORDER-TOP: steelblue 1px solid; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; OVERFLOW: auto; BORDER-LEFT: steelblue 1px solid; PADDING-TOP: 4px; BORDER-BOTTOM: steelblue 1px solid; HEIGHT: 170px; BACKGROUND-COLOR: whitesmoke; width: 300px;">
                                                <asp:Literal ID="LitGuidelinesNew" runat="server"></asp:Literal>
                                            </div>
                                            <asp:LinkButton ID="lnkEditGuidelinesNew" runat="server" CssClass="action-link">Edit Guidelines (New)</asp:LinkButton></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trGuidelines" runat="server">
                            <td style="width: 103px">
                                <span style="font-size: 10pt">Guidelines:</span></td>
                            <td colspan="2" class="field-cell">
                                <div  style="width: 570px; height: 170px; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke;">
                                <asp:Literal ID="litGuidelines" runat="server"></asp:Literal></div>
                            
                                <asp:DropDownList ID="cboPromoType" runat="server" DataSourceID="sqldsPromoType"
                                    DataTextField="TypeDesc" DataValueField="PromoTypeID" Width="238px" AppendDataBoundItems="True">
                                    <asp:ListItem Selected="True" Value="-1">- Select Promo Type -</asp:ListItem>
                                </asp:DropDownList>
                          
                                <asp:LinkButton ID="lnkEditGuidelines" runat="server" CssClass="action-link" Width="78px">Edit Guidelines</asp:LinkButton>
                                <asp:LinkButton ID="lnkInsertGuidelines" runat="server" CssClass="action-link" Width="126px" OnClientClick="return check_selection()">Insert Guideline</asp:LinkButton>
                            </td>
                        </tr>
                        <tr id="trReason" runat="server">
                            <td style="width: 103px">
                                Reason:</td>
                            <td class="field-cell" colspan="2">
                                <asp:Label ID="lblCancelReason" runat="server" BorderWidth="0px" Width="464px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 103px">
                                <span style="font-size: 10pt">Prepared By:</span></td>
                            <td class="field-cell">
                                            <asp:Label ID="lblPreparedBy" runat="server" BorderWidth="0px"
                                                Width="464px"></asp:Label></td>
                        </tr>

                        <tr id="tbrowReviewedLine" runat="server">
                            <td style="width: 103px">
                                <span style="font-size: 10pt">Reviewed By:</span></td>
                            <td class="field-cell">
                                            <asp:Label ID="lblReviewedBy" runat="server" BorderWidth="0px"
                                                Width="464px"></asp:Label></td>
                        </tr>
                        <tr id="tbrowApprovedLine" runat="server">
                            <td style="width: 103px">
                                <span style="font-size: 10pt">Approved By:</span></td>
                            <td class="field-cell">
                                            <asp:Label ID="lblApprovedBy" runat="server" BorderWidth="0px"
                                                Width="464px"></asp:Label></td>
                        </tr>

                        <tr>
                            <td style="width: 103px; height: 3px">
                                <span style="font-size: 10pt">Remarks:</span></td>
                            <td class="field-cell" style="height: 3px">
                                <asp:Literal ID="lblRemarks" runat="server"></asp:Literal></td>
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
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
        </td>
        <td rowspan="1">
           <div id="menu" style="width: 173px">
               <table>
                    <tr id="trEditRequest" runat="server" style="height:30px">
                        <td style="height: 30px">
                             <asp:LinkButton ID="lnkEditRequest" runat="server" Height="5px">Make Changes</asp:LinkButton></td>
                    </tr>
                   <tr  id="trSaveMemo" runat="server"  style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkSaveMemo" runat="server" CssClass="nav-link">Submit Memo Draft</asp:LinkButton></td>
                   </tr>
                   <tr   id="trForApproval" runat="server"  style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkForApproval" runat="server" Height="5px">Submit for Approval</asp:LinkButton></td>
                   </tr>
                   <tr   id="trApprove" runat="server" style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkApprove" runat="server" Height="5px">Approve</asp:LinkButton></td>
                   </tr>
                   <tr   id="trCreateMemoDraft" runat="server" style="height:30px">
                       <td>
                         <asp:LinkButton ID="lnkCreateMemoDraft" runat="server" Height="5px">Create Memo Draft</asp:LinkButton></td>
                   </tr>
                   <tr   id="trReturn" runat="server" style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkReturn" runat="server" Height="5px">Return for Revision</asp:LinkButton></td>
                   </tr>
                   <tr   id="trViewRequest" runat="server" style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkViewRequest" runat="server" Height="5px">View Request</asp:LinkButton></td>
                   </tr>
                   <tr  id="trPrintMemo" runat="server" style="height:30px">
                       <td>
                         <asp:LinkButton ID="lnkPrintMemo" runat="server" Height="5px">Print this Document</asp:LinkButton></td>
                   </tr>
                   <tr  id="trSwipestakesSeed" runat="server" style="height:30px" visible="false">
                       <td>
                            <asp:LinkButton ID="linkSwipestakesSeed" runat="server">View Seeding</asp:LinkButton></td>
                   </tr>
                   <tr  id="trSwipestakesReseed" runat="server" style="height:30px" visible="false">
                       <td>
                            <asp:LinkButton ID="linkSwipestakesReseed" runat="server">View Reseeding</asp:LinkButton></td>
                   </tr>
                   <tr  id="trViewBranches" runat="server" style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkViewBranches" runat="server">View Branches</asp:LinkButton></td>
                   </tr>
                   <tr   id="trHistory" runat="server" style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkHistory" runat="server">Transaction History</asp:LinkButton></td>
                   </tr>
                   <tr   id="trDone" runat="server" style="height:30px">
                       <td>
                        <asp:LinkButton ID="lnkDone" runat="server">Back to Summary List</asp:LinkButton></td>
                   </tr>
                </table>
           </div>
        </td>
    </tr>
</table>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <asp:HiddenField ID="hidinputbox" runat="server" />
        <asp:HiddenField ID="hidRadioButton" runat="server" />
       <asp:SqlDataSource ID="sqldsPromos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, dbo.Fn_FormatPromoDesc(P.PromoID, NULL) PromoDesc, P.PromoTypeID&#13;&#10;FROM Promotions AS P Left Outer Join  PromoDetails AS D ON P.PromoID = D.PromoID&#13;&#10;WHERE P.MemoID = @MemoID &#13;&#10;ORDER BY ISNULL(P.SEQNO,P.PromoID)" UpdateCommand="UPDATE Promotions SET MemoID = @MemoID WHERE (RequestID = @RequestID)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hidMemoID" DefaultValue="0" Name="MemoID"  PropertyName="Value" Type="Int32"  />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter DefaultValue="0" Name="MemoID" />
                                           <asp:ControlParameter ControlID="hidRequestID" DefaultValue="0" Name="RequestID" PropertyName="Value" Type="Int32"  />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="sqldsPromosAdd" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, dbo.Fn_FormatPromoDesc_CR( P.PromoID) PromoDesc, P.PromoTypeID&#13;&#10;FROM CRPromotions AS P Left Outer Join  CRPromoDetails AS D ON P.PromoID = D.PromoID&#13;&#10;WHERE CRID = @CRID &#13;&#10;ORDER BY ISNULL(P.SEQNO,P.PromoID)" UpdateCommand="UPDATE CRPromotions SET MemoID = @MemoID WHERE (CRID = @CRID)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hidCRID" DefaultValue="0" Name="CRID" PropertyName="Value" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter DefaultValue="0" Name="MemoID" />
                                            <asp:SessionParameter Name="CRID" SessionField="CRID" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT 1"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sqldsPromoType" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT * FROM PromoTypes ORDER BY TypeDesc"></asp:SqlDataSource>
        <asp:HiddenField ID="hidBox" runat="server" /><asp:HiddenField ID="hidCRID" runat="server" /><asp:HiddenField id="hidMemoID" runat="server"></asp:HiddenField><asp:HiddenField id="hidRequestID" runat="server"></asp:HiddenField>
        <asp:HiddenField id="hidGuidelinesActive" runat="server"></asp:HiddenField>
    <div style="display:none"><asp:Button ID="cmdPopUpOK" runat="server" Text="" />
        <asp:Button ID="cmdRedirect" runat="server" />
        <asp:Button ID="cmdGuidelines" runat="server" Text="Update Guidelines" /></div>
    
</asp:Content>