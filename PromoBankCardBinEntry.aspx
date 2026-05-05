<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PromoBankCardBinEntry.aspx.vb" Inherits="PromoBankCardBinEntry" validateRequest="false"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>BankCard BIN Entry</title>
        <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
</head>

<body  style="background: white;">
    <form id="form1" runat="server">
    
    <div>
    <asp:Panel ID="panBankBins" runat="server" HorizontalAlign="Left" Visible="True" Width="80%" Wrap="False">
                
                        <table id= "tbl_PromoDetails" runat="server" width="580px">
                            <tr>
                                <td style="width: 2px; height: 6px;"></td>
                                <td style="width: 300px; height: 6px; text-align: center;">
                                   <span style="font-size: 9pt"><strong>BankName</strong></span></td>
                                <td style="width: 37px; height: 6px; text-align: center">
                                   <span style="font-size: 9pt"><strong>BIN</strong></span></td>
                                <td id="tdHeaderPanLow" runat="server" visible="false" style="width: 40px; height: 6px; text-align: center;">
                                   <span style="font-size: 9pt"><strong>PanLow</strong></span></td>
                                <td id="tdHeaderPanHigh" runat="server" visible="false" style="width: 85px; height: 6px; text-align: center;">
                                   <span style="font-size: 9pt"><strong>PanHigh</strong></span></td>
                                <td style="width: 58px; height: 6px; text-align: center">
                                   <span style="font-size: 9pt"></span></td>
                                <td style="width: 71px; height: 6px; text-align: center" colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px; height: 13px"></td>
                                <td style="width: 300px; height: 13px">
                                    <asp:DropDownList ID="cboBankNames" runat="server" Width="100%" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                    </asp:DropDownList></td>
                                <td style="width: 37px; height: 13px; text-align: center;">
                                    <asp:TextBox ID="txtBankBIN" runat="server" Width="100px" MaxLength="8"></asp:TextBox></td>
                                <td id="tdEntryPanLow" runat="server" visible="false" style="width: 40px; height: 13px; text-align: center;">
                                    <asp:TextBox ID="txtBankPanLow" runat="server" Width="120px" MaxLength="17"></asp:TextBox></td>
                                <td id="tdEntryPanHigh" runat="server" visible="false" style="width: 85px; height: 13px; text-align: center">
                                    <asp:TextBox ID="txtBankPanHigh" runat="server" MaxLength="17" Width="120px"></asp:TextBox></td>
                                <td style="width: 58px; height: 13px; text-align: center"></td>
                                <td style="width: 71px; height: 13px; text-align: left" colspan="2">
                                    <asp:Button ID="cmdAddToBIN" runat="server" Text="Add BIN" /></td>
                            </tr>
                        </table>
        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="494px">
        </asp:BulletedList>

                        <br />
                        <asp:GridView ID="gridPromoBankBins" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Height="14px"
                        Width="500px" GridLines="None">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" /> <!-- OnCheckedChanged="chkRowSel_CheckedChanged" /-->
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_CheckedChanged" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" Visible="False" />
                            <asp:BoundField DataField="CardName" HeaderText="Card Name" SortExpression="CardName">
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CardBIN" HeaderText="BIN" SortExpression="CardBIN" >
                                <ItemStyle Width="10px" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EmptyDataTemplate>
                            <div align="center">No BIN Selected for this Promo</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                        <table style="width: 500px">
                        <tr>
                            <td style="width: 22px; text-align: left; height: 20px;">
                                <asp:LinkButton ID="lnkDeleteSelected" runat="server" CssClass="action-link" Font-Size="Small"
                                    Width="100px">Delete Selected</asp:LinkButton></td>
                            <td style="height: 20px">
                            </td>
                            <td style="height: 20px">
                                </td>
                        </tr>
                    </table>
                        <br />
                        <br />
                    </asp:Panel>    
    
        <table style="width:98%">
             <tr>
                   <td style="text-align: right; height: 26px;">
                         <asp:Button ID="cmdPopOK" runat="server" Text="Done" Width="83px" />&nbsp;
                   </td>
             </tr>
        </table>
	  
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="lblPopTitle" runat="server" />
    </form>
    
</body>
</html>
