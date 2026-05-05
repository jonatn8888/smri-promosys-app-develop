<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoDetailsEntry.aspx.vb" Inherits="PromoDetailsEntry" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

    <script type="text/javascript">

function openSearch()
{
    var url
    url = "SearchDepSdep.aspx";
	itemsearchwindow=dhtmlmodal.open('EmailBox', 'iframe', url, 'Search Dept/SDept/Class Code', 'width=725px,height=280px,center=1,resize=0,scrolling=0',"recall")
    itemsearchwindow.onclose=function()
    { 
        var theform = this.contentDoc.forms[0] 
	    var thedepcode = this.contentDoc.getElementById("txtDepCode") 
        var theSubDep = this.contentDoc.getElementById("txtSdepCode") 
        var theClass = this.contentDoc.getElementById("txtClassCode") 
        var theSubClass = this.contentDoc.getElementById("txtSubClassCode") 
        var theDescription = this.contentDoc.getElementById("txtDescription") 
        var theShortDesc = this.contentDoc.getElementById("hidShortDesc") 
        // var theEnvCode = this.contentDoc.getElementById("hidEnvCode")
        
	    document.getElementById("<%= txtDepCode.ClientID%>").value = thedepcode.value; 
	    // document.getElementById("ctl00$ContentPlaceHolder1$txtDepCode").value = thedepcode.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtSdepCode").value = theSubDep.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtClassCode").value = theClass.value;
	    document.getElementById("ctl00$ContentPlaceHolder1$txtSClassCode").value = theSubClass.value;
	    document.getElementById("ctl00$ContentPlaceHolder1$hfDesc").value = theDescription.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$hfShortDesc").value = theShortDesc.value; 
	    document.getElementById("<%=Button1.ClientID%>").click();
	    return true 
	}
}

function openSearchUPC()
{
    var url
    url = "SearchUPC.aspx";
	searchWindow=dhtmlmodal.open('EmailBox', 'iframe', url, 'Search UPC Code', 'width=725px,height=280px,center=1,resize=0,scrolling=0',"recall")
    searchWindow.onclose=function()
    { 
        var theform = this.contentDoc.forms[0] 
	    var theSKUno = this.contentDoc.getElementById("txtSKUno") 
        var theUPCno = this.contentDoc.getElementById("txtUPCnumber") 
        var theDescription = this.contentDoc.getElementById("txtDescription") 
        //var theShortDesc = this.contentDoc.getElementById("hidShortDesc") 
      		
	    document.getElementById("<%= txtUPCno.ClientID%>").value = theUPCno.value; 
	    //document.getElementById("ctl00$ContentPlaceHolder1$txtSdepCode").value = theSKUno.value; 
	    document.getElementById("<%= hfDesc.ClientID%>").value = theDescription.value; 
	    //document.getElementById("ctl00$ContentPlaceHolder1$hfShortDesc").value = theShortDesc.value; 
	    document.getElementById("<%=cmdProcessUPC.ClientID%>").click();
	    
	    return true 
	}
}

function openSearchPromoPremiumUPC()
{
    var url
    url = "SearchUPC.aspx";
	searchWindow=dhtmlmodal.open('EmailBox', 'iframe', url, 'Search UPC Code', 'width=725px,height=280px,center=1,resize=0,scrolling=0',"recall")
    searchWindow.onclose=function()
    { 
        var theform = this.contentDoc.forms[0] 
	    var theSKUno = this.contentDoc.getElementById("txtSKUno") 
        var theUPCno = this.contentDoc.getElementById("txtUPCnumber") 
        var theDescription = this.contentDoc.getElementById("txtDescription") 
        //var theShortDesc = this.contentDoc.getElementById("hidShortDesc") 
      		
	    document.getElementById("<%= hfPromoPremUPCno.ClientID%>").value = theUPCno.value; 
	    //document.getElementById("ctl00$ContentPlaceHolder1$txtSdepCode").value = theSKUno.value; 
	    document.getElementById("<%= hfPromoPremDesc.ClientID%>").value = theDescription.value; 
	    //document.getElementById("ctl00$ContentPlaceHolder1$hfShortDesc").value = theShortDesc.value; 
	    document.getElementById("<%=cmdProcessPromoPremUPC.ClientID%>").click();
	    
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

    <br />
    <asp:HiddenField ID="hfShortDesc" runat="server" />
    <asp:HiddenField ID="hfDesc" runat="server" />
    
      <asp:HiddenField ID="hfPromoPremDesc" runat="server" />
    <asp:HiddenField ID="hfPromoPremUPCno" runat="server" />
    
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkSaveMemo" runat="server" OnClientClick="return confirm_action('Submit this memo for review?')">View Request Preview</asp:LinkButton><br />
        <br />
    </div>
    
    <div id="contents">
        <table cellpadding="3" cellspacing="0" id="doc-table">
            <tr>
                <td class="DocTabHeadOff">
                    <span style="font-size: 11pt; color: lightslategray; font-family: Trebuchet MS"><strong>
                        <asp:LinkButton ID="lnkRequest" runat="server" Width="230px">Sales Promotion Request</asp:LinkButton></strong></span></td>
            </tr>
            <tr>
                <td align="center" class="DocTabHeadOff">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkPromoInfo" runat="server" Width="228px">Promotion Information</asp:LinkButton></span></strong></td>
            </tr>
            <tr>
                <td class="DocTabHeadOn">
                    <span style="font-size: 11pt; color: #ffffff; font-family: Trebuchet MS"><strong>Promo
                        Details</strong></span></td>
            </tr>
                                            
            <tr>
                <td style="text-align: center; width: 625px;" valign="top" class="field-cell">
                    
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="494px"></asp:BulletedList>
                    
                    <asp:Panel ID="panItemHierarchy" runat="server" HorizontalAlign="Left" Visible="False" Width="670px" Wrap="False">
                
                        <table id= "tbl_PromoDetails" runat="server">
                            <tr>
                                <td style="width: 2px; height: 6px;"></td>
                                <td style="width: 28px; height: 6px; text-align: center;">
                                   <span style="font-size: 9pt"><strong>Dept</strong></span></td>
                                <td style="width: 37px; height: 6px; text-align: center">
                                   <span style="font-size: 9pt"><strong>SDept</strong></span></td>
                                <td style="width: 40px; height: 6px; text-align: center;">
                                   <span style="font-size: 9pt"><strong>Class</strong></span></td>
                                <td style="width: 40px; height: 6px; text-align: center;">
                                   <span style="font-size: 9pt"><strong>SClass</strong></span></td>
                                <td style="width: 251px; height: 6px; text-align: center">
                                   <span style="font-size: 9pt"><strong>Item Description</strong></span></td>
                                <td style="width: 58px; height: 6px; text-align: center">
                                   <span style="font-size: 9pt"></span></td>
                                <td style="width: 71px; height: 6px; text-align: center">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px; height: 13px"></td>
                                <td style="width: 28px; height: 13px">
                                    <asp:TextBox ID="txtDepCode" runat="server" Width="25px" MaxLength="3"></asp:TextBox></td>
                                <td style="width: 37px; height: 13px; text-align: center;">
                                    <asp:TextBox ID="txtSdepCode" runat="server" Width="25px" MaxLength="3"></asp:TextBox></td>
                                <td style="width: 40px; height: 13px; text-align: center;">
                                    <asp:TextBox ID="txtClassCode" runat="server" Width="25px" MaxLength="3"></asp:TextBox></td>
                                <td style="width: 40px; height: 13px; text-align: center">
                                    <asp:TextBox ID="txtSClassCode" runat="server" MaxLength="3" Width="25px"></asp:TextBox></td>
                                <td style="width: 251px; height: 13px; text-align: left; padding-top: 2px;">
                                    <asp:Label ID="lblItemDesc" runat="server" Width="243px" BackColor="White" BorderColor="LightSlateGray" BorderStyle="Solid" BorderWidth="1px" Height="20px"></asp:Label></td>
                                <td style="width: 58px; height: 13px; text-align: center"><asp:Button ID="cmdSearch" runat="server" Text="Search" /></td>
                                <td style="width: 71px; height: 13px; text-align: left">
                                    <asp:Button ID="cmdAdd" runat="server" Text="Add Item" /></td>
                            </tr>
                        </table>

                        <br />
                        <asp:GridView ID="gridItems" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataSourceID="sqldsPromoDetails" ForeColor="#333333" Height="14px"
                        Width="530px" GridLines="None">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" OnCheckedChanged="chkRowSel_CheckedChanged" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_CheckedChanged" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SeqNo" HeaderText="SeqNo" SortExpression="SeqNo" Visible="False" />
                            <asp:BoundField DataField="DepCode" HeaderText="Dept" SortExpression="DepCode">
                                <ItemStyle Width="10px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubDepCode" HeaderText="SDept" SortExpression="SubDepCode" >
                                <ItemStyle Width="10px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ClassCode" HeaderText="Class" SortExpression="ClassCode" >
                                <ItemStyle Width="10px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubClassCode" HeaderText="SClass" >
                                <ItemStyle Width="10px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ShortDesc" HeaderText="Item Description" SortExpression="ShortDesc" />
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EmptyDataTemplate>
                            No Dp/SDp/Cl Selected for this Promo
                        </EmptyDataTemplate>
                    </asp:GridView>
                        <table style="width: 524px">
                        <tr>
                            <td style="width: 22px; text-align: left">
                                <asp:LinkButton ID="lnkDeleteSelected" runat="server" CssClass="action-link" Font-Size="Small"
                                    Width="100px">Delete Selected</asp:LinkButton></td>
                            <td>
                            </td>
                            <td>
                                </td>
                        </tr>
                    </table>
                        <br />
                        <br />
                    </asp:Panel>
                    
                    <asp:Panel ID="panUPCdetails" runat="server" HorizontalAlign="Left" Visible="False"
                        Width="670px" Wrap="False">
                    <table width="100%">
                        <tr>
                            <td colspan="6" style="text-align: center" class="DocTabHeadOn">
                                <asp:Label ID="lblUPCpanelHeader" runat="server" Text="Eligible Items" Font-Bold="True" Font-Size="14px" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 6px;">
                            </td>
                            <td style="width: 102px; height: 6px; text-align: center;">
                                <span style="font-size: 9pt"><strong>UPC/Barcode</strong></span></td>
                            <td style="height: 6px; text-align: center">
                                <span style="font-size: 9pt"><strong>Description</strong></span></td>
                            <td style="width: 42px; height: 6px; text-align: center">
                                <span style="font-size: 9pt"><strong>X/Y</strong></span></td>
                            <td style="width: 58px; height: 6px; text-align: center">
                                <span style="font-size: 9pt"></span>
                            </td>
                            <td style="width: 71px; height: 6px; text-align: center">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 13px">
                            </td>
                            <td style="width: 102px; height: 13px">
                                <asp:TextBox ID="txtUPCno" runat="server" Width="100px" MaxLength="15"></asp:TextBox></td>
                            <td style="height: 13px; padding-top: 2px;">
                                <asp:Label ID="lblUPCdesc" runat="server" BackColor="White" BorderColor="LightSlateGray"
                                    BorderStyle="Solid" BorderWidth="1px" Height="20px" Width="98%"></asp:Label></td>
                            <td style="width: 42px; height: 6px; text-align: center">
                                <asp:DropDownList id="cboUPCrem" runat="server" Width="40px" Enabled="False">
                                    <asp:ListItem Selected="True" Value="1">X</asp:ListItem>
                                    <asp:ListItem Value="2">Y</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 58px; height: 13px; text-align: center">
                                <asp:Button ID="cmdSearchUPC" runat="server" Text="Search" /></td>
                            <td style="width: 71px; height: 13px; text-align: left;">
                                <asp:Button ID="cmdAddUPC" runat="server" Text="Add Item" /></td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridPromoUPC" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
                        PageSize="5" Width="100%">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSelUPC" runat="server" OnCheckedChanged="chkRowSelUPC_CheckedChanged" EnableTheming="True" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL_UPC" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_UPC_CheckedChanged" />
                                </HeaderTemplate>
                                 <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="UPC/Barcode" DataField="UPCno">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Description" DataField="Description">
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Price" DataField="UnitPrice" DataFormatString="{0:F2}">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remark" HeaderText="X/Y">
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Dp/SDp/Cl/SCl" DataField="ItemCode">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No UPC/Barcode Selected for this Promo
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    <table width="100%">
                        <tr>
                            <td style="width: 81px">
                                <asp:LinkButton ID="lnkDeleteSelectedUPC" runat="server" CssClass="action-link" Font-Size="Small"
                                    Width="100px">Delete Selected</asp:LinkButton></td>
                            <td>
                                </td>
                        </tr>
                    </table>
                    </asp:Panel>
                    &nbsp;&nbsp;<br />
                    <asp:Panel ID="panUPCPromoPremium" runat="server" HorizontalAlign="Left" Visible="False"
                        Width="670px" Wrap="False">
                    <table width="100%">
                        <tr>
                            <td colspan="6" style="text-align: center; height: 23px;" class="DocTabHeadOn">
                                <asp:Label ID="lbl_UPCPanPrem" runat="server" Text="" Font-Bold="True" Font-Size="14px" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 6px;">
                            </td>
                            <td style="width: 208px; height: 6px; text-align: center;">
                                <span style="font-size: 9pt"><strong>UPC/Barcode</strong></span></td>
                            <td style="height: 6px; text-align: center; width: 2768px;">
                                <span style="font-size: 9pt"><strong>Description</strong></span></td>
                                <td style="height: 6px; text-align: center; width: 452px;">
                                <span style="font-size: 9pt"><strong id="lblUPCPremDiscountedPrice" runat="server" >Discounted Price</strong></span></td>
                        </tr>
                        <tr>
                            <td style="width: 2px; height: 13px">
                            </td>
                            <td style="width: 208px; height: 13px">
                                <asp:TextBox ID="txtPromoPremiumUPCno" runat="server" Width="121px" MaxLength="15"></asp:TextBox></td>
                            <td style="height: 13px; padding-top: 2px; width: 2768px;">
                                <asp:Label ID="lblPromoPremiumDesc" runat="server" BackColor="White" BorderColor="LightSlateGray"
                                    BorderStyle="Solid" BorderWidth="1px" Height="20px" Width="99%"></asp:Label></td>
                            <td style="width: 56px; height: 6px; text-align: center">
                                <asp:TextBox ID="txtDiscountedPrice" runat="server" MaxLength="15" Width="121px" Enabled="False"></asp:TextBox>&nbsp;</td>
                            <td style="width: 58px; height: 13px; text-align: center">
                                <asp:Button ID="btnSearchUPCPromoPrem" runat="server" Text="Search" /></td>
                            <td style="width: 71px; height: 13px; text-align: left;">
                                <asp:Button ID="btnAddPromoPrem" runat="server" Text="Add Item" /></td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridUPCPromoPrem" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
                        PageSize="5" Width="100%">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSelUPCPrem" runat="server" OnCheckedChanged="chkRowSelUPCPrem_CheckedChanged" EnableTheming="True" Checked="false" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL_UPCPrem" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_UPCPrem_CheckedChanged" />
                                </HeaderTemplate>
                                 <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="SKU" DataField="SKUno">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Description" DataField="Description">
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="UPC" DataField="UPCno">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Dp/SDp/Cl/SCl" DataField="ItemCode">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="120px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="RetailPrice" DataField="UnitPrice" DataFormatString="{0:F2}">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                            </asp:BoundField>
                             <asp:BoundField HeaderText="DiscountedPrice" DataField="DiscountedPrice" DataFormatString="{0:F2}">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="90px" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No UPC/Barcode Selected for this Promo
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    <table width="100%">
                        <tr>
                            <td style="width: 81px">
                                <asp:LinkButton ID="lnkDeleteUPCPromoPrem" runat="server" CssClass="action-link" Font-Size="Small"
                                    Width="100px">Delete Selected</asp:LinkButton></td>
                            <td>
                                </td>
                        </tr>
                    </table>
                    </asp:Panel>
                    &nbsp;&nbsp;<br />
                    <asp:Panel ID="panPromoCoupons" runat="server" HorizontalAlign="Left" Visible="False"
                        Width="670px" Wrap="False">
                    <table width="100%">
                        <tr>
                            <td colspan="3" style="text-align: center" class="DocTabHeadOn">
                                <asp:Label ID="lblCouponHeader" runat="server" Text="Valid Coupons" Font-Bold="True" Font-Size="14px" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="width: 1041px">
                                <span style="font-size: 9pt"><strong>Coupon Code / Number</strong></span></td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="height: 233px">
                            </td>
                            <td style="height: 233px; width: 1041px;">                            
    
                                <asp:Image ID="imgBarcode" runat="server" style="                                                         
                                                            background-color: white;
                                                            padding: 20px;
                                                            display: inline-block;"/>
                            </td>
                            <td style="text-align: left; height: 233px;">
                                <table id="Tbl_Barcode" style="height: 128px; width: 293px; margin-left: 0px;">
                                    <tr>
                                        <td style="width: 175px" align="right">
                                            <span style="font-size: 9pt"><strong>Type</strong></span>:</td>
                                        <td style="width: 290px">
                                            <asp:DropDownList ID="cboDrop_BarcodeManual" runat="server" 
                                                AppendDataBoundItems="True" AutoPostBack="true" DataTextField="ElementName" 
                                                DataValueField="ElementValue" 
                                                OnSelectedIndexChanged="cboDrop_BarcodeManual_SelectedIndexChanged" 
                                                style="margin-left: 4px" Width="176px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="tr_ValueType" runat="server">
                                        <td style="width: 175px" align="right">
                                            <span style="font-size: 9pt"><strong>Prefix Value</strong></span>:</td>
                                        <td style="width: 290px">
                                            <asp:DropDownList ID="cboDrop_ValueType" runat="server" 
                                                AppendDataBoundItems="True" AutoPostBack="true" DataTextField="ElementName" 
                                                DataValueField="ElementValue" 
                                                style="margin-left: 4px" Width="176px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="tr_CouponManual" runat="server">
                                        <td style="width: 175px; height: 3px" align="right">
                                            <span style="font-size: 9pt"><strong>Coupon Code</strong></span>:</td>
                                        <td style="height: 3px; width: 290px;">
                                            <asp:TextBox ID="txtInputBarcode" runat="server" Height="31px" MaxLength="13" 
                                                Width="182px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 175px">
                                            &nbsp;</td>
                                        <td style="width: 290px">
                                            <asp:Button ID="cmdGenerate" runat="server" OnClick="btnGenerate_Click" 
                                                Text="Generate Barcode" Width="182px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 175px">
                                            &nbsp;</td>
                                        <td style="width: 290px">
                                            <asp:Button ID="cmdAddCoupon" runat="server" Text="Add Coupon" Width="182px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridPromoCoupons" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
                        PageSize="5" Width="100%" style="margin-top: 0px">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSelCoupon" runat="server" OnCheckedChanged="chkRowSelCoupon_CheckedChanged" EnableTheming="True" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL_Coupon" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_Coupon_CheckedChanged" />
                                </HeaderTemplate>
                                 <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Coupon Code / Number" DataField="CouponCode">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No UPC/Barcode Selected for this Promo
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    <table width="100%">
                        <tr>
                            <td style="width: 81px">
                                <asp:LinkButton ID="lnkDeleteSelectedCoupon" runat="server" CssClass="action-link" Font-Size="Small"
                                    Width="100px">Delete Selected</asp:LinkButton></td>
                            <td>
                                </td>
                        </tr>
                    </table>
                    </asp:Panel>
                    <br />
                </td>
            </tr>
            <tr id="trPromoBranchTabLine" runat="server">
                <td class="DocTabHeadOff"
                    valign="top">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkBranches" runat="server" Width="230px">Branches</asp:LinkButton></span></strong></td>
            </tr>
        </table>
        <br />
        &nbsp;<asp:HiddenField ID="lblPopTitle" runat="server" />
        <asp:HiddenField ID="lblCouponCode" runat="server" />
    </div>
    
       <asp:SqlDataSource ID="sqldsPromoDetails" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT PromoID, dbo.Fn_FormatPromoCode(DepCode,'Dp') AS DepCode, dbo.Fn_FormatPromoCode(SubDepCode,'SDp') AS SubDepCode, dbo.Fn_FormatPromoCode(ClassCode,'Cl') AS ClassCode, dbo.Fn_FormatPromoCode(SubClassCode,'SCl') AS SubClassCode , ShortDesc, PercentDisc, DiscAmount, PeriodFrom, PeriodTo, SeqNo  FROM [PromoDetails] WHERE PromoID = @PromoID" InsertCommand="INSERT INTO PromoDetails(PromoID, DepCode, SubDepCode, ClassCode,SubClassCode, ShortDesc, PercentDisc, PeriodFrom, PeriodTo) VALUES (@PromoID, @DepCode, @SubDepCode, @ClassCode,@SubClassCode, @ShortDesc, @PercentDisc, @PeriodFrom, @PeriodTo)" DeleteCommand="DELETE FROM [PromoDetails] WHERE PromoID = @PromoID AND DepCode = @DepCode AND SubDepCode = @SubDepCode AND ClassCode = @ClassCode AND SubClassCode = @SubClassCode">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="PromoID" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="PromoID" />
            <asp:Parameter Name="DepCode" />
            <asp:Parameter Name="SubDepCode" />
            <asp:Parameter Name="ClassCode" />
            <asp:Parameter Name="SubClassCode" />
            <asp:Parameter Name="ShortDesc" />
            <asp:Parameter Name="PercentDisc" />
            <asp:Parameter Name="PeriodFrom" />
            <asp:Parameter Name="PeriodTo" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="PromoID" />
            <asp:Parameter Name="DepCode" />
            <asp:Parameter Name="SubDepCode" />
            <asp:Parameter Name="ClassCode" />
            <asp:Parameter Name="SubClassCode" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>"></asp:SqlDataSource> 
  
      
    <div style="display: none">
        <asp:Button ID="Button1" runat="server" Text="" />
        <asp:Button ID="cmdProcessUPC" runat="server" Text="" />
        <asp:Button ID="cmdPopUpOK" runat="server" Text="" />
        <asp:Button ID="cmdProcessPromoPremUPC" runat="server" Text="" />
    </div>
    
</asp:Content>