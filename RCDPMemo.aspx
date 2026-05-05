<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="RCDPMemo.aspx.vb" ValidateRequest="false" Inherits="RCDPMemo" Title ="Promotional Memo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
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
function openUploadVSLP(QueryString)
{
    var url
    url = "UploadPromoVSLP.aspx"+QueryString;
	UploadPromoVSLP=dhtmlmodal.open('EmailBox', 'iframe', url, 'Upload VSLP', 'width=770px,height=400px,center=1,resize=0,scrolling=0',"recall")
    UploadPromoVSLP.onclose=function()
    { 
    /*var theform = this.contentDoc.forms[0] 
	var thedepcode = this.contentDoc.getElementById("txtDepCode") 
    var theSubDep = this.contentDoc.getElementById("txtSdepCode") 
    var theClass = this.contentDoc.getElementById("txtClassCode") 
    var theSubClass = this.contentDoc.getElementById("txtSubClassCode") 
    var theDescription = this.contentDoc.getElementById("txtDescription") 
    var theShortDesc = this.contentDoc.getElementById("hidShortDesc")   		
	document.getElementById("ctl00$ContentPlaceHolder1$txtDep").value = thedepcode.value; 
	document.getElementById("ctl00$ContentPlaceHolder1$txtSubDp").value = theSubDep.value; 
	document.getElementById("ctl00$ContentPlaceHolder1$txtClass").value = theClass.value; 
	document.getElementById("ctl00$ContentPlaceHolder1$txtSubClass").value = theSubClass.value; 
	document.getElementById("ctl00$ContentPlaceHolder1$lblItemDesc").value = theDescription.value; 
	document.getElementById("ctl00$ContentPlaceHolder1$hfShortDesc").value = theShortDesc.value; */
	document.getElementById("<%=btnVSLPRefresh.ClientID%>").click();
	return true 
	}
}
</script>
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
 function openAttachment(type)
        {
            var url
            url = "UploadedFiles.aspx?type=" + type;
	        attachmentwindow = dhtmlmodal.open('UploadedFiles', 'iframe', url, 'File Attached', 'width=490px,height=305px,center=1,resize=0,scrolling=0','recall')
	        
            attachmentwindow.onclose=function()
            { 
            var theform = this.contentDoc.forms[0] 
            document.getElementById("<%=btnDownload.ClientID%>").click();
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
function opentexteditor2(height,width)
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
    document.getElementById("<%=cmdPopUpOK2.ClientID%>").click(); 
	return true 
	}
}
</script>
<script type="text/javascript">
function opentexteditor3(sActionButton, sMsgTitle)
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

function resetGridRadioButton(rdoMe,rdoName,frm){
	for(var i=0;i<frm.length;i++){
		e=frm.elements[i];
		if(e.type=='radio' && e.name.indexOf(rdoName)!=-1 && e!=rdoMe){
			e.checked=false;
		}
	}
}

function checkUncheckOther(chkItemId,frm){
	for(var i=0;i<frm.length;i++){
		e=frm.elements[i];
		if(e.type=='checkbox' && e.id!=chkItemId){
			if (e.checked){
				e.checked=false;
				//document.getElementById(chkAllId).checked=true;
				//return;
			}
		}
	}
}	

</script>
<script language="javascript" type="text/javascript">
    function openReseedAttachment(queryString)
    {
        var url
        url = "UploadReseedFiles.aspx"+queryString;
        attachmentwindow = dhtmlmodal.open('UploadReseedFiles', 'iframe', url, 'Reseed File Attachment', 'width=490px,height=305px,center=1,resize=0,scrolling=0','recall')
        
        attachmentwindow.onclose=function()
        { 
        var theform = this.contentDoc.forms[0] 
        document.getElementById("<%=btnSeedDownload.ClientID%>").click();
        return true 
        }
    }
    
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
                    <strong><span style="font-size: 11pt; color: white; text-align: left;">Request for
                        <asp:Label ID="lblRequstTypeDesc" runat="server"></asp:Label></span></strong></td>
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
                    <span style="font-size: 10pt">Request Date:</span></td>
                <td class="field-cell">
                                    <asp:Label ID="lblRequestDate" runat="server" Width="232px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Change Req. ID:</span></td>
                <td class="field-cell">
                    CR -
                <asp:Label ID="lblCRID"
                                    runat="server" Width="232px" Font-Bold="False" Font-Italic="False"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Reference Memo:</td>
                <td class="field-cell">
                <asp:Label ID="lblMemoNumber" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label> <asp:Label ID="lblPromoTitle" runat="server"></asp:Label><br />
                    (<asp:Label ID="lblPromoPeriod" runat="server"></asp:Label>)</td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Branches</td>
                <td class="field-cell">
                                    <asp:Label ID="lblBranches" runat="server"
                                        Height="49px" Width="583px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                </td>
                <td class="field-cell">
                </td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Title:</td>
                <td class="field-cell">
                    <asp:Label ID="lblRCDPTitle" runat="server" Width="528px"></asp:Label>
                    <asp:TextBox ID="txtRCDPTitle" runat="server" CssClass="Tb_ToUpper" MaxLength="100"
                        Width="484px"></asp:TextBox></td>
            </tr>
            <tr id="trEffectDate" runat ="server">
                <td style="width: 103px">
                    Effective Date:
                </td>
                <td class="field-cell">
                    <asp:Label ID="lblCancelEffectDate" runat="server" Width="232px"></asp:Label><asp:TextBox ID="txtCancelEffectDate" runat="server" MaxLength="50" Width="177px"></asp:TextBox>
                    <input id="calCancelEffectDate" runat="server" class="btnCal" name="calCancelEffectDate" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtCancelEffectDate');"
                        style="left: 322px; top: 345px" type="button" /></td>
            </tr>
            <tr id="trExtendedUntil" runat="server">
                <td style="width: 103px">
                    Extended Until:</td>
                <td class="field-cell">
                    <asp:Label ID="lblExtendedUntil" runat="server"></asp:Label><asp:TextBox ID="txtExtendedUntil"
                        runat="server" MaxLength="50" Width="177px"></asp:TextBox>
                    <input id="calExtendedUntil" runat="server" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtExtendedUntil');"
                        style="left: 547px; top: 345px" type="button" /></td>
            </tr>
            <tr id="trCRFDatePeriod" runat="server">
                <td style="width: 103px">
                    Promo Period:</td>
                <td class="field-cell">
                    <asp:Label ID="lblCRFPeriodFrom" runat="server"></asp:Label> <asp:Label ID="lblLabelCRFPeriod" runat="server">To</asp:Label> <asp:Label ID="lblCRFPeriodTo" runat="server"></asp:Label>
                    <asp:TextBox ID="txtCRFPeriodFrom" runat="server" MaxLength="50" Width="177px"></asp:TextBox>
                    <input id="calCRFPeriodFrom" runat="server" class="btnCal" name="calCRFPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtCRFPeriodFrom');"
                        style="left: 322px; top: 345px" type="button" />
                    <asp:TextBox ID="txtCRFPeriodTo" runat="server" MaxLength="50" Width="177px"></asp:TextBox>
                    <input id="calCRFPeriodTo" runat="server" class="btnCal" name="calCRFPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtCRFPeriodTo');"
                        style="left: 547px; top: 345px" type="button" /></td>
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
            <tr id="trPromotions" runat="server">
                <td style="width: 103px">
                    Promotions:</td>
                <td class="field-cell">
                    <asp:GridView id="gridPromotionsAdd" runat="server" ForeColor="#333333" Width="566px" DataSourceID="sqldsPromosAdd" DataKeyNames="PromoID" CellPadding="4" BorderWidth="2px" BorderStyle="Solid" AutoGenerateColumns="False">
                                        
                                        <RowStyle BackColor="WhiteSmoke" ForeColor="#333333"  />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowSel" runat="server" Width="10px"  />
                                                </ItemTemplate>
                                                <ItemStyle Width="5px"  />
                                            </asp:TemplateField>
                                           <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPromoID" runat="server" Text='<%# Eval("PromoID") %>' Width="10px"></asp:Label>
                                                    <asp:Label ID="lblPromoFormat" runat="server" Text='<%# Eval("PromoFormat") %>' Width="10px"></asp:Label>
                                                    <asp:Label ID="lblPromoTypeID" runat="server" Text='<%# Eval("PromoTypeID") %>' Width="10px"></asp:Label>
                                                    <asp:Label ID="lblPD" runat="server" Text='<%# Eval("PercentDisc") %>' Width="10px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/roomedit.png"  />
                                                </ItemTemplate>
                                                <ItemStyle Width="5px"  />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl" ReadOnly="True" SortExpression="ItemCode" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <ItemStyle HorizontalAlign="Center" Width="110px"  />
                                                <HeaderStyle HorizontalAlign="Center"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PromoDesc" HeaderText="Promo Description" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle HorizontalAlign="Center"  />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"  />
                                        <EmptyDataTemplate>
                                            <strong>Promotion(s).</strong>
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"  />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
                                        <EditRowStyle BackColor="#999999"  />
                                    </asp:GridView>
                    <asp:LinkButton ID="lnkAddPromo" runat="server" CssClass="action-link" Width="84px">Add New Promo</asp:LinkButton><asp:LinkButton ID="lnkEditPromo" runat="server" CssClass="action-link" Width="84px">Edit Selected</asp:LinkButton><asp:LinkButton ID="lnkDeletePromo" runat="server" CssClass="action-link" Width="84px">Delete Selected</asp:LinkButton>
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
                                <DIV style="BORDER-RIGHT: steelblue 1px solid; PADDING-RIGHT: 4px; BORDER-TOP: steelblue 1px solid; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; OVERFLOW: auto; BORDER-LEFT: steelblue 1px solid; PADDING-TOP: 4px; BORDER-BOTTOM: steelblue 1px solid; HEIGHT: 170px; BACKGROUND-COLOR: #f5f5f5; width: 300px;">
                                    <asp:Literal id="litMechanicsOld" runat="server" Visible="False"></asp:Literal>
                                    <!--<asp:GridView ID="gridInsteadOf1" runat="server" AutoGenerateColumns="False" DataSourceID="sqldsPromos"
                                        Font-Bold="False" ShowHeader="False">
                                        <Columns>
                                            <asp:BoundField DataField="PromoDesc" HeaderText="Promo Description" HtmlEncode="False"
                                                HtmlEncodeFormatString="False" SortExpression="PromoDesc" />
                                        </Columns>
                                    </asp:GridView>-->
                                    <asp:GridView  ID="gridInsteadOf" runat="server" CssClass="mGridTrans" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="PromoID"
                                        ForeColor="#333333"   BorderWidth="1px" DataSourceID="sqldsPromos" ShowHeader="False">
                                        <RowStyle  ForeColor="Black" />
                                        <Columns>
                                            <asp:BoundField DataField="PromoDesc" HeaderText="Promo Description" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle />                                                
                                                <ItemStyle VerticalAlign="Top" Font-Size="9pt" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            <strong>Promotion(s).</strong>
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle Font-Bold="True" Font-Size="9pt" Height="5px" Font-Names="Trebuchet MS" />
                                        <EditRowStyle BackColor="#999999" />
                                    </asp:GridView>
                                </DIV>
                                </td>
                            <td style="width: 50%">
                                <DIV style="BORDER-RIGHT: steelblue 1px solid; PADDING-RIGHT: 4px; BORDER-TOP: steelblue 1px solid; PADDING-LEFT: 4px; PADDING-BOTTOM: 4px; OVERFLOW: auto; BORDER-LEFT: steelblue 1px solid; PADDING-TOP: 4px; BORDER-BOTTOM: steelblue 1px solid; HEIGHT: 170px; BACKGROUND-COLOR: #f5f5f5; width: 300px;">
                                    <asp:Literal ID="litMechanicsNew" runat="server"></asp:Literal>
                                </div>
                                <asp:LinkButton ID="lnkEditMechanicsNew" runat="server" CssClass="action-link">Edit Mechanics (New)</asp:LinkButton></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trPromotionsVSLP" runat="server">
                <td style="width: 103px">
                    VSLP:</td>
                <td class="field-cell">
                    <asp:GridView id="gridVSLPBatch" runat="server" ForeColor="#333333" Width="580px" DataSourceID="sqldsPromoVSLPBatch" DataKeyNames="PromoID" CellPadding="4" BorderWidth="2px" BorderStyle="Solid" AutoGenerateColumns="False">
                        <RowStyle BackColor="WhiteSmoke" ForeColor="#333333"  />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" Width="10px"  />
                                </ItemTemplate>
                                <ItemStyle Width="5px"  />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblPromoTypeID" runat="server" Text='<%# Eval("PromoTypeID") %>' Width="10px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/roomedit.png"  />
                                </ItemTemplate>
                                <ItemStyle Width="5px"  />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ItemCode" HeaderText="PromoType" ReadOnly="True" SortExpression="ItemCode" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                <ItemStyle HorizontalAlign="Center"  />
                                <HeaderStyle HorizontalAlign="Center"  />
                            </asp:BoundField>
                            <asp:BoundField DataField="DocumentType" HeaderText="Document Type" />
                            <asp:BoundField DataField="PromoDesc" HeaderText="File Name" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                <HeaderStyle HorizontalAlign="Center"  />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"  />
                        <EmptyDataTemplate>
                            <strong>No VSLP File(s).</strong>
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"  />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
                        <EditRowStyle BackColor="#999999"  />
                    </asp:GridView>
                    <table id="tblPromoVSLPControls" runat="server" style="width: 598px">
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblRFPromoType" runat="server" ForeColor="Red"></asp:Label></td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Select Promotion Type:</td>
                            <td>
                    <asp:DropDownList ID="cboPromoType" runat="server" AppendDataBoundItems="True" DataSourceID="sqldsPromoType"
                        DataTextField="TypeDesc" DataValueField="PromoTypeID" Width="283px">
                        <asp:ListItem Selected="True" Value="-1">- Select Promo Type -</asp:ListItem>
                    </asp:DropDownList></td>
                            <td>
                    <asp:LinkButton ID="lnkAddNewFile" runat="server" CssClass="action-link" Width="132px">Add New File</asp:LinkButton></td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr  id="trReason" runat ="server">
                <td style="width: 103px">
                    Reason:</td>
                <td class="field-cell">
                    <asp:Label ID="lblCancelReason" runat="server" Width="528px"></asp:Label>
                    <asp:TextBox ID="txtCancelReason" runat="server" MaxLength="100"
                        Width="484px" CssClass="Tb_Normal"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 27px;">
                    Requested By:</td>
                <td class="field-cell" style="height: 27px">
                    <asp:Label ID="lblCRRequestedBy" runat="server" BorderWidth="0px" Width="464px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Approved By:</td>
                <td class="field-cell">
                    <asp:Label ID="lblCRApprovedBy" runat="server" BorderWidth="0px" Width="464px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Approved By(MCI):
                </td>
                <td class="field-cell">
                    <asp:Label ID="lblCRMCIApprovedBy" runat="server" BorderWidth="0px" Width="464px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    Remarks:</td>
                <td class="field-cell">
                    <asp:Label ID="lblCRRemarks" runat="server" BorderWidth="0px" Width="464px"></asp:Label></td>
            </tr>

            <tr  id="trAttachments" runat ="server"> 
                <td style="width: 103px">
                    Attachment(s):</td>
                <td class="field-cell">
                    <asp:ImageButton ID="imgbtnDownload2" runat="server" BackColor="White" BorderColor="White"
                        BorderStyle="Solid" ImageUrl="~/Images/down.gif" ToolTip="Download attached files"
                        Visible="False" />
                            <asp:Literal ID="lblAttachments" runat="server"></asp:Literal>
                    <br />
                    <asp:LinkButton ID="lnkattachment" runat="server" OnClientClick="return openAttachment();"
                            Visible="False">Add Attachment(s)</asp:LinkButton></td>
            </tr>
            <tr id="trPromoSeedAttachment" runat="server" visible="False">
                <td style="width: 112px; height: 29px">
                    <span style="font-size: 10pt">
                        <asp:Label ID="lblSeedAttachement" runat="server" Text="Reseed Attachment(s):"></asp:Label></span></td>
                <td class="field-cell" rowspan="1" style="width: 462px; height: 29px" valign="top">
                    <asp:ImageButton ID="imgbtnSeedDownload" runat="server" BackColor="White" BorderColor="White"
                        BorderStyle="Solid" ImageUrl="~/Images/down.gif" Visible="False" ToolTip="Download attached files " />
                    <asp:LinkButton ID="linkSeedAttachment" runat="server"
                        Visible="False">Add Reseed Attachment(s)</asp:LinkButton><br />
                    <asp:Literal ID="lblSeedFile" runat="server"></asp:Literal>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 103px">
                </td>
                <td>
                    </td>
            </tr>
            <tr id="trMemo1" runat="server">
                <td colspan="2" class="DocTabHeadOn">
                    <strong><span style="font-size: 11pt; color: white; text-align: left;">Memorandum Information
                    </span></strong></td>
            </tr>
            <tr  id="trMemo2" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt">Memo Date:</span></td>
                <td class="field-cell">
                                    <asp:Label ID="lblMemoDate" runat="server" Width="232px"></asp:Label></td>
            </tr>
            <tr  id="trMemo3" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt">Memo ID:</span></td>
                <td class="field-cell">
                </td>
            </tr>
            <tr  id="trMemo4" runat="server">
                <td style="width: 103px">
                    Status:</td>
                <td class="field-cell">
                    <asp:Label ID="lblStatus" runat="server" Width="232px"></asp:Label></td>
            </tr>
            <tr  id="trMemo5" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt">Title:</span></td>
                <td class="field-cell">
                    </td>
            </tr>
            <tr  id="trMemo6" runat="server">
                <td style="width: 103px; height: 22px">
                    <span style="font-size: 10pt">Promo Period:</span></td>
                <td class="field-cell" style="height: 22px">
                    </td>
            </tr>
            <tr  id="trMemo7" runat="server">
                <td style="width: 103px; height: 13px">
                </td>
                <td class="field-cell" style="height: 13px">
                </td>
            </tr>
            <tr  id="trMemo8" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt">Branches:</span></td>
                <td class="field-cell">
                                    <br />
                    </td>
            </tr>
            <tr  id="trMemo9" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt">Promotions:</span></td>
                <td class="field-cell">
                </td>
            </tr>
            <tr  id="trMemo10" runat="server">
                <td style="width: 103px">
                    <span style="font-size: 10pt">Guidelines:</span></td>
                <td class="field-cell">
                    <asp:Literal ID="litGuidelines" runat="server"></asp:Literal><br />
                                </td>
            </tr>
            <tr  id="trMemo11" runat="server">
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

            <tr  id="trMemo12" runat="server">
                <td style="width: 103px; height: 3px">
                    <span style="font-size: 10pt">Remarks:</span></td>
                <td class="field-cell" style="height: 3px">
                <asp:Literal ID="lblRemarks" runat="server"></asp:Literal></td>
            </tr>
            <tr  id="trMemo13" runat="server">
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
                    <tr id="trSubmitRequest" runat="server" style="height:30px">
                        <td>
                            <asp:LinkButton ID="lnkSubmitRequest" runat="server" Height="5px">Submit Request</asp:LinkButton></td>
                    </tr>
                   <tr  id="trEditRequest" runat="server"  style="height:30px">
                       <td>
                            <asp:LinkButton ID="lnkEditRequest" runat="server" Height="5px">Make Changes</asp:LinkButton></td>
                   </tr>
                   <tr>
                       <td id="trDelete" runat="server" style="height:30px">
                            <asp:LinkButton ID="lnkDelete" runat="server" Height="5px">Delete Request</asp:LinkButton></td>
                   </tr>
                   <tr id="trDraftSave" runat="server" style="height:30px">
                       <td>
                            <asp:LinkButton ID="lnkDraftSave" runat="server" CssClass="nav-link">Save As Draft</asp:LinkButton></td>
                   </tr>
                   <tr  id="trApprove" runat="server" style="height:30px">
                       <td>
                            <asp:LinkButton ID="lnkApprove" runat="server" Height="5px">Approve</asp:LinkButton></td>
                   </tr>
                   <tr  id="trCreateMemoDraft" runat="server" style="height:30px">
                       <td>
                            <asp:LinkButton ID="lnkCreateMemoDraft" runat="server" Height="5px">Create Memo Draft</asp:LinkButton></td>
                   </tr>
                   <tr  id="trReturn" runat="server" style="height:30px">
                       <td>
                             <asp:LinkButton ID="lnkReturn" runat="server" Height="5px">Return for Revision</asp:LinkButton></td>
                   </tr>
                   <tr id="trPrintMemo" runat="server" style="height:30px">
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
                   <tr id="trHistory" runat="server" style="height:30px">
                       <td>
                            <asp:LinkButton ID="lnkHistory" runat="server">Transaction History</asp:LinkButton></td>
                   </tr>
                   <tr   id="trDone" runat="server" style="height:30px">
                       <td>
                            <asp:LinkButton ID="lnkDone" runat="server">Back to Summary List</asp:LinkButton></td>
                   </tr>
                   <tr>
                       <td style="height: 30px">
                       </td>
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
                                        SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, dbo.Fn_FormatPromoDesc( P.PromoID, @UserID) PromoDesc, P.PromoTypeID&#13;&#10;FROM Promotions AS P Left Outer Join  PromoDetails AS D ON P.PromoID = D.PromoID&#13;&#10;WHERE P.MemoID = @MemoID &#13;&#10;ORDER BY ISNULL(P.SEQNO,P.PromoID)" UpdateCommand="UPDATE Promotions SET MemoID = @MemoID WHERE (RequestID = @RequestID)">
                                        <SelectParameters>
                                             <asp:ControlParameter ControlID="hidMemoID" DefaultValue="0" Name="MemoID"  PropertyName="Value" Type="Int32"  />
                                            <asp:SessionParameter Name="UserID" SessionField="UserID" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter DefaultValue="0" Name="MemoID" />
                                             <asp:ControlParameter ControlID="hidRequestID" DefaultValue="0" Name="RequestID" PropertyName="Value" Type="Int32"  />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                    <asp:SqlDataSource ID="sqldsPromoVSLPBatch" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT * FROM PromoVSLPBatch" >
                                        <SelectParameters>
                                             <asp:ControlParameter ControlID="hidMemoID" DefaultValue="0" Name="MemoID" PropertyName="Value" Type="Int32"  />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter DefaultValue="0" Name="MemoID" />
                                             <asp:ControlParameter ControlID="hidRequestID" DefaultValue="0" Name="RequestID" PropertyName="Value" Type="Int32"  />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqldsPromosAdd" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode&#13;&#10;,  P.PromoID&#13;&#10;,  dbo.Fn_FormatPromoDesc_CR( P.PromoID) PromoDesc,  P.PromoTypeID&#13;&#10;,  D.DepCode&#13;&#10;,  D.SubDepCode&#13;&#10;,  D.ClassCode, P.PromoFormat, P.PercentDisc &#13;&#10;FROM CRPromotions AS P LEFT JOIN CRPromoDetails AS D&#13;&#10;ON P.PromoID = D.PromoID&#13;&#10;WHERE P.CRID = @CRID ORDER BY ISNULL(P.SEQNO,P.PromoID)">
            <SelectParameters>
                 <asp:ControlParameter ControlID="lblCRID" DefaultValue="0" Name="CRID" PropertyName="Text"
                                Type="Int16" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT 1"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sqldsPromoType" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT DISTINCT PT.PromoTypeID,PT.TypeDesc FROM PromoTypes PT 
                            INNER JOIN Promotions P ON P.PromoTypeID = PT.PromoTYPEID 
                                WHERE P.RequestID = @RequestID ORDER BY PT.TypeDesc">
             <SelectParameters>
                   <asp:ControlParameter ControlID="hidRequestID" DefaultValue="0" Name="RequestID" PropertyName="Value" Type="Int32"  />
             </SelectParameters>
            </asp:SqlDataSource>
    <div style="display:none">
        <asp:Button ID="cmdPopUpOK" runat="server" Text="" />
        <asp:Button ID="cmdPopUpOK2" runat="server" Text="" />
        <asp:Button id="bclick" runat="server" Text="bclick"></asp:Button>
        <asp:Button ID="standard" runat="server" Text="Standard" />
        <asp:Button ID="cmdRedirect" runat="server" />
        <asp:Button ID="btnDownload" runat="server" Text="Download" />
        <asp:HiddenField ID="hidBox" runat="server" /><asp:HiddenField ID="hidMechanicsActive" runat="server" />
         <asp:HiddenField ID="hidMemoID" runat="server" />
          <asp:HiddenField ID="hidRequestID" runat="server" />
        <asp:Button id="cmdMechanics" runat="server" Text="Update Mechanics"></asp:Button>
        <asp:Button ID="btnVSLPRefresh" runat="server" Text="Button" />
        <asp:Button ID="btnSeedDownload" runat="server" Text="Seed Download" />
    </div>
    
</asp:Content>