<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"  CodeFile="MallSaleDetailsEntry.aspx.vb" Inherits="MallSaleDetailsEntry" title="Storewide Sale Brand Promo Entry" ValidateRequest="False" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
function openSearch()
{
    var url
    url = "SearchDepSdep.aspx";
	itemsearchwindow=dhtmlmodal.open('EmailBox', 'iframe', url, 'Search ITEM Code', 'width=725px,height=280px,center=1,resize=0,scrolling=0',"recall")
    itemsearchwindow.onclose=function()
    { 
        var theform = this.contentDoc.forms[0] 
	    var thedepcode = this.contentDoc.getElementById("txtDepCode") 
        var theSubDep = this.contentDoc.getElementById("txtSdepCode") 
        var theClass = this.contentDoc.getElementById("txtClassCode") 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtDeptCode").value = thedepcode.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtSubDeptCode").value = theSubDep.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtClassCode").value = theClass.value; 
	    document.getElementById("<%=cmdReturnSearchItem.ClientID%>").click();
	    return true 
	}
}
</script>

<script type="text/javascript">
function opentexteditor()
{
    var url
    url = "texteditor.aspx";
	texteditorwindow=dhtmlmodal.open('texteditor', 'iframe', url, 'Default Mechanics', 'width=709px,height=500px,center=1,resize=0,scrolling=0',"recall")
    texteditorwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    var Mechanics = this.contentDoc.getElementById("HiddenField1") 
    document.getElementById("<%=hidBox.ClientID%>").value = Mechanics.value;
    document.getElementById("<%=cmdReturnMechanics.ClientID%>").click(); 
	return true 
	}
}

function openmessagebox(height,width)
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

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkViewList" runat="server">View Participation List</asp:LinkButton><br />
        <br />
    </div>
    
    <div id="contents">
        &nbsp;<table cellpadding="3" cellspacing="0" id="doc-table">
                        <tr>
                            <td colspan="2" class="DocTabHeadOn">
                                Promo Event Participation Details</td>
                        </tr>
                        <tr>
                            <td style="width: 112px">
                            </td>
                            <td class="field-cell">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 112px">
                                Dept/Sdep/Class</td>
                            <td class="field-cell" style="width: 587px;">
                                <asp:TextBox ID="txtDeptCode" runat="server" MaxLength="3" Width="36px"></asp:TextBox>
                                <asp:TextBox ID="txtSubDeptCode" runat="server" MaxLength="3" Width="36px"></asp:TextBox>
                                <asp:TextBox ID="txtClassCode" runat="server" MaxLength="3" Width="36px"></asp:TextBox>
                                <input id="cmdSearch" runat="server" style="width: 68px; height: 24px" type="button"
                                    value="Search" /></td>
                        </tr>
                        <tr id="trBrand" runat="server">
                            <td style="width: 112px">
                                Brand</td>
                            <td class="field-cell">
                                <asp:Label ID="lblItemDesc" runat="server" Font-Bold="True" Width="517px"></asp:Label></td>
                        </tr>
            <tr>
                <td style="width: 112px">
                    &nbsp;</td>
                <td class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr id="trPromotionRow" runat="server">
                <td style="width: 112px">
                    Promotions</td>
                <td class="field-cell" style="width: 587px;">
                    <asp:GridView ID="gridPromoList" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="None" Height="14px" Width="99%">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAllRows" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAllRows_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" OnCheckedChanged="chkRowSel_CheckedChanged" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="RowID">
                                <ControlStyle CssClass="HiddenObject" />
                                <HeaderStyle CssClass="HiddenObject" />
                                <ItemStyle CssClass="HiddenObject" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TypeDesc" HeaderText="Promo Type">
                                <ItemStyle Width="100px" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PromoDescription" HeaderText="Description" HtmlEncode="False" HtmlEncodeFormatString="False" />
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No promotion for the given brand.
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    <asp:LinkButton ID="lnkAddPromo" runat="server" CssClass="action-link" Visible="False"
                        Width="95px">Add New Promo</asp:LinkButton><asp:LinkButton ID="lnkDeletePromo" runat="server"
                            CssClass="action-link" Visible="False" Width="112px">Delete Promotion</asp:LinkButton></td>
            </tr>
            <tr>
                <td style="width: 112px">
                    &nbsp;</td>
                <td class="field-cell" style="width: 587px">
                    &nbsp;</td>
            </tr>
                        <tr id="trErrorRow" runat="server">
                            <td style="width: 112px;">
                                &nbsp;</td>
                            <td class="field-cell" style="width: 587px">
                                <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="80%">
                                </asp:BulletedList>
                            </td>
                        </tr>
                        <tr id="trNewPromoRow" runat="server">
                            <td style="width: 112px; height: 8px;">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Promo Type:</span></td>
                            <td class="field-cell" style="height: 8px; width: 587px">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">
                                    <asp:DropDownList ID="cboPromoType" runat="server"
                                        DataTextField="TypeDesc" DataValueField="PromoTypeID" Width="294px" AppendDataBoundItems="True" AutoPostBack="True">
                                        <asp:ListItem Selected="True" Value="-1">- Select Promo Type -</asp:ListItem>
                                    </asp:DropDownList>&nbsp;</span><span style="font-size: 10pt; font-family: Trebuchet MS"></span></td>
                        </tr>
                        <tr id="trPromoMechanicsRow" runat="server">
                            <td style="width: 112px;" valign="top">
                                <span style="font-size: 10pt; font-family: Trebuchet MS" class="field-row"> 
                                    <asp:Label ID="lblMechanics" runat="server" Text="Mechanics:"></asp:Label>&nbsp;</span></td>
                            <td class="field-cell" align="center" style="width: 587px; text-align: left;">
                            
                                <asp:Panel ID="panMechanics" runat="server" Width="99%">
                                <div  style="height: 147px; width: 99%; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke; clear: both; clip: rect(auto auto auto auto); text-align: left;">
                                <asp:Literal ID="litMechanics" runat="server"></asp:Literal></div>
                                    <table style="border-top-style: none; border-right-style: none; border-left-style: none;
                                        border-bottom-style: none" width="98%">
                                        <tr>
                                            <td>
                                <asp:LinkButton ID="lnkEditMechanics" runat="server" CssClass="action-link">Edit Mechanics</asp:LinkButton></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                                                
                                <asp:Panel ID="panClassDiscount" runat="server" HorizontalAlign="Left" Width="99%" Wrap="False" Visible="False">
                                    <table style="width: 98%">
                                        <tr>
                                            <td>
                                               <asp:TextBox ID="txtDiscount" runat="server" MaxLength="3" Width="25px"></asp:TextBox>
                                               <asp:Label ID="lblDiscount" runat="server" Text="% Discount on all regular-priced items."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                 </asp:Panel>
                                
                                <asp:Panel ID="panMarkdown" runat="server" HorizontalAlign="Left" Width="99%" Visible="False">
                                    <table style="width: 98%">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMarkdown" runat="server" MaxLength="20" Width="94px"></asp:TextBox>
                                                <asp:Label ID="lblMarkdownDesc" runat="server" Text="% Markdown on selected items."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel><input id="cmdSavePromo" runat="server" style="width: 78px; height: 24px" type="button"
                                    value="Save" /></td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 29px">
                                </td>
                            <td class="field-cell" rowspan="1" style="width: 462px; height: 29px" valign="top">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 22px">
                                &nbsp;</td>
                            <td rowspan="1" valign="top" class="field-cell">
                                &nbsp;</td>
                        </tr>
                    </table>
    </div>
    
    <asp:HiddenField id="hidBox" runat="server"></asp:HiddenField>
    <asp:HiddenField id="lblPopTitle" runat="server"></asp:HiddenField>
    
    <div style="display:none">
        <asp:Button ID="cmdReturnSearchItem" runat="server" Text="ReturnSearchItem" />
        <asp:Button ID="cmdReturnMechanics" runat="server" Text="ReturnMechanics" />
        <asp:Button ID="cmdPopUpOK" runat="server" Text="PopUpOK" />
    </div>

</asp:Content>

