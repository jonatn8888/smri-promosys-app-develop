<%@ Page Language="VB"  AutoEventWireup="false" EnableEventValidation="false" CodeFile="SearchUPC.aspx.vb" Inherits="SearchUPC" title="Search UPC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Search</title>
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)" /><meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)" /> 
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <script src="js/jquery-1.2.3.min.js" type="text/javascript"></script>
    <script src="js/numbers.js" type="text/javascript" ></script>
</head>

<body class="search"  style="background: white;"> <!--  background="Images/arr_dbl_blu.gif"-->
<form id="SearchUPC" runat="server">
 <div id="subcontainer" align="center" >
    <table style="width: 586px">
        <tr>
            <td>
                <table id="Table1">
                       <tr><td style="width: 2px;"></td>
                           <td style="text-align: left">
                               <span style="font-size: 9pt">UPC</span></td>
                           <td style="text-align: left">
                               <span style="font-size: 9pt">Description</span></td>
                           <td style="text-align: left">
                               <span style="font-size: 9pt">Unit Price</span></td>
                           <td style="text-align: left">
                               <span style="font-size: 9pt"></span></td>
                       </tr>
                        <tr>
                            <td style="width: 2px;">
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtUPCnumber" runat="server" Height="20px" Width="137px" MaxLength="13"></asp:TextBox></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtDescription" runat="server" Height="20px" Width="255px"></asp:TextBox></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtUnitPrice" runat="server" Height="20px" MaxLength="10" Width="100px"></asp:TextBox></td>
                            <td style="text-align: left">
                                <asp:Button ID="cmdSearch" runat="server" Text="Search" Height="27px" /></td>
                        </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 692px">
        <tr>
            <td style="width: 688px; height: 176px" valign="top">
                <asp:GridView ID="gridSearchResult" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="685px" Font-Size="8pt" AllowPaging="True" PageSize="5" AllowSorting="True" Font-Bold="True">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="UPCno" HeaderText="UPC" >
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description" >
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:N2}" >
                            <ItemStyle Width="90px" HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl" >
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataTemplate>
                        <asp:Label ID="lblGridMessage" runat="server" Text="* No record found matching this criteria *" Font-Size="10pt" Width="264px"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>  
        </tr>
    </table>
            </td>
        </tr>
    </table>
                <asp:HiddenField ID="hidShortDesc" runat="server" />
                                &nbsp;
 </div>
</form>
</body>
</html>
