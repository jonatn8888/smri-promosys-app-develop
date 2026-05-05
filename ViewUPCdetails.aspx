<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewUPCdetails.aspx.vb" Inherits="ViewUPCdetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Audit Logs</title>
        <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
</head>
<body  style="background: white;">
    <form id="form1" runat="server">
    <div>
        <table width="98%">
            <tr>
                <td width="70%"><asp:Label ID="lblListTitle" runat="server" Font-Bold="True" Font-Size="12pt" Text="Promotion Details: UPC"></asp:Label></td>
                <td width="30%"><asp:Label id="lblUPCcount" runat="server" Text="Total UPC Count:" Font-Size="9pt" Font-Bold="True" Visible="False"></asp:Label></td>
            </tr>
        </table>
        <asp:GridView ID="gridPromoUPC" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
            PageSize="5" Width="98%">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="UPCno" HeaderText="UPC/Barcode">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="Description" HeaderText="Description">
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="UnitPrice" DataFormatString="{0:F2}" HeaderText="Price">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                </asp:BoundField>
                <asp:BoundField DataField="Remark" HeaderText="X/Y">
                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <table style="width: 500px">
                    <tr>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                <asp:Label ID="lblEmptyListMessage" runat="server" Font-Italic="True" Font-Size="12pt"
                    Height="21px" Text="No UPC/Barcode specified for this promotion " Width="400px" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        &nbsp;
        </div>
    </form>
</body>
</html>
