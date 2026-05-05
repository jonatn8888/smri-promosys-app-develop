<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoRequestPreview.aspx.vb" Inherits="PromoRequestPreview" Title ="Sales Promotions Request Entry" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

    <script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>



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
    document.getElementById("<%=Button1.ClientID%>").click(); 
	return true 
	}
}

function updateCharCount(textbox) {
        var count = textbox.value.length;
        document.getElementById("charCountTd").innerText = count + " / 75";
    }

window.onload = function () {
        var textbox = document.getElementById('<%= txtRequestTitle.ClientID %>');
        updateCharCount(textbox); // Show initial count
    };
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
	    seedWindow=dhtmlmodal.open('PromoRequestSwipestakesSeed', 'iframe', url, 'Seed', 'width=620px,height=560px,center=1,resize=0,scrolling=0',"recall")
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

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
            <asp:Panel ID="panSwipestakesMenu" runat="server" Visible=false>
                <asp:LinkButton ID="linkBinRange" runat="server">View Bin Range</asp:LinkButton><br />
                <br />
                <br />
                <asp:LinkButton ID="linkSwipestakesMessage" runat="server">View Messages</asp:LinkButton><br />
                <br />
                <br />
                <asp:LinkButton ID="linkSwipestakesSeed" runat="server">View Seeding</asp:LinkButton><br />
            </asp:Panel>
            <asp:Panel ID="panRebateMenu" runat="server" Visible=false>
                <asp:LinkButton ID="linkRebateBinRange" runat="server">View Bin Range</asp:LinkButton><br />
                <br />
                <br />
            </asp:Panel>
        <br />
        <br />
        <asp:LinkButton ID="lnkPrintReq" runat="server" Width="123px">Print this Document</asp:LinkButton><br />
        <br />
        <br />
        <asp:LinkButton ID="lnkDraftSave" runat="server" Width="123px">Save As Draft</asp:LinkButton><br />
        <br />
        <br />
        <asp:LinkButton ID="lnkSaveRequest" runat="server" Width="122px">Submit Request</asp:LinkButton><br />
        <br />
        <br />
        <asp:LinkButton ID="lnkClose" runat="server" Width="122px">Close and Go Back</asp:LinkButton><br />
    </div>
    
    <div id="contents">
        <table id="doc-table" cellpadding="3px" cellspacing="0px">
            <tr>
                <td style="text-align: center;" colspan="2" class="DocTabHeadOn">
                    Sales Promotion Request</td>
            </tr>
            <tr>
                <td style="width: 206px; height: 45px;">
                    &nbsp;</td>
                <td style="height: 45px;" valign="middle">
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="80%">
                    </asp:BulletedList>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 206px">
                    <span style="font-size: 10pt">Request ID:</span></td>
                <td style="width: 453px" class="field-cell">
                    <span style="font-size: 10pt">PR-<asp:Label ID="lblRequestID"
                                    runat="server" Width="152px"></asp:Label></span></td>
            </tr>
            <tr>
                <td style="width: 206px">
                    <span style="font-size: 10pt">Request Date:</span></td>
                <td class="field-cell">
                                    <asp:Label ID="lblRequestDate" runat="server" Width="204px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 206px">
                    &nbsp;</td>
                <td class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 206px;">
                    <span style="font-size: 10pt">Title:</span>
                </td>
                <td class="field-cell">
                    <asp:TextBox ID="txtRequestTitle" runat="server"
                        Width="484px"
                        CssClass="Tb_ToUpper"
                        maxlength="75"
                        onkeyup="updateCharCount(this)">
                    </asp:TextBox>
                    <br />
                    <span id="charCountTd"></span>
                </td>
            </tr>
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
            <tr>
                <td style="width: 206px; height: 11px">
                    &nbsp;</td>
                <td class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 206px; height: 11px" valign="top">
                    <span style="font-size: 10pt">Branches:</span></td>
                <td style="height: 11px" class="field-cell">
                    <asp:Label ID="lblBranches" runat="server" Height="48px" Width="99%"></asp:Label>
                    <table width="99%">
                        <tr>
                            <td style="height: 15px"><asp:LinkButton ID="lnkEditBranch" runat="server" CssClass="action-link" Width="85px" Visible="False">Edit Branch List</asp:LinkButton></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 206px; height: 11px" valign="top">
                    &nbsp;</td>
                <td style="height: 11px" class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 206px; height: 9px" valign="top">
                    <span style="font-size: 10pt">Promotions:</span></td>
                <td style="height: 9px" class="field-cell">
                                    <asp:GridView ID="gridPromotions" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                        BorderWidth="2px" CellPadding="4" DataKeyNames="PromoID" DataSourceID="sqldsPromos"
                                        ForeColor="#333333" Width="99%">
                                        
                                        <RowStyle BackColor="WhiteSmoke" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowSel" runat="server" Width="10px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5px" CssClass="HiddenObject" />
                                                <ControlStyle CssClass="HiddenObject" />
                                                <HeaderStyle CssClass="HiddenObject" />
                                            </asp:TemplateField>
                                           <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPromoID" runat="server" Text='<%# Eval("PromoID") %>' Width="10px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/roomedit.png" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl" ReadOnly="True" SortExpression="ItemCode" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <ItemStyle HorizontalAlign="Center" Width="110px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FormatPromoDesc" HeaderText="Promo Description" SortExpression="FormatPromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            &nbsp;<strong>No Promotion Details Specified</strong>
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                    </asp:GridView>
                    <asp:LinkButton ID="lnkAddPromo" runat="server" CssClass="action-link" Width="95px" Visible="False">Add New Promo</asp:LinkButton><asp:LinkButton ID="lnkEditPromo" runat="server" CssClass="action-link" Width="97px" Visible="False">Edit Promotion</asp:LinkButton><asp:LinkButton ID="lnkDeletePromo" runat="server" CssClass="action-link" Width="112px" Visible="False">Delete Promotion</asp:LinkButton>&nbsp;</td>
            </tr>
            <tr id="trGuidelines" runat="server">
                <td style="width: 206px" valign="top">
                    <span style="font-size: 10pt">Guidelines:</span></td>
                <td style="width: 552px" class="field-cell">
                    <asp:Literal ID="litGuidelines" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td style="width: 206px; height: 11px" valign="top">
                    <span style="font-size: 10pt">Remarks:</span></td>
                <td class="field-cell">
                    &nbsp;<asp:Label ID="lblRemarks" runat="server"
                                    Width="552px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 206px; height: 4px" valign="top">
                    <span style="font-size: 10pt">
                    <asp:Label ID="AttachedFiles" runat="server" Text="Attachments:"></asp:Label></span></td>
                <td style="height: 4px" class="field-cell" valign="middle">
                    <asp:ImageButton ID="imgbtnDownload" runat="server" BackColor="White" BorderColor="White"
                        BorderStyle="Solid" ImageUrl="~/Images/down.gif" Visible="False" ImageAlign="Middle" ToolTip="Download attached files" />
                    <br />
                    <asp:Literal ID="lblFiles" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 11px" valign="top">
                    &nbsp;&nbsp;</td>
            </tr>
        </table>
    </div>
    
    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>" SelectCommand="SELECT 1"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqldsPromos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode&#13;&#10;,  P.PromoID&#13;&#10;,  dbo.Fn_FormatPromoDesc(P.PromoID,@UserID) AS FormatPromoDesc&#13;&#10;,  P.PromoTypeID&#13;&#10;,  D.DepCode&#13;&#10;,  D.SubDepCode&#13;&#10;,  D.ClassCode&#13;&#10;FROM Promotions AS P LEFT JOIN PromoDetails AS D&#13;&#10;ON P.PromoID = D.PromoID&#13;&#10;WHERE P.RequestID = @RequestID">
                                        
        <SelectParameters>
            <asp:Parameter DefaultValue="" Name="RequestID" />
            <asp:SessionParameter Name="UserID" SessionField="UserID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:HiddenField ID="lblPopTitle" runat="server" />
    <br />
    &nbsp;<div style="display:none"><asp:Button ID="cmdPopUpOK" runat="server" Text="Button" />
        <asp:Button ID="Button1" runat="server" Text="Button" /></div>
    
</asp:Content>