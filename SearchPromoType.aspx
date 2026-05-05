<%@ Page Language="VB" AutoEventWireup="false" EnableEventValidation="false" CodeFile="SearchPromoType.aspx.vb"
    Inherits="SearchPromoType" Title="Search PromoType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search</title>
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)" />
    <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)" />
    <link rel="stylesheet" type="text/css" href="./css/main.css" media="screen" />

    <script src="js/jquery-1.2.3.min.js" type="text/javascript"></script>

    <script src="js/numbers.js" type="text/javascript"></script>

    <style type="text/css">
        .pagination 
        {
        	background:#284775;
        	padding:0;
        	margin:0;
        	color:White;
        	font-size:11px;
        	font-weight:bold;
        	height:30px;
        	width:100%
        }
        .pagination td 
        {
        	padding-left:10px;
        	padding-right:10px;
        }
        
        .pager 
        {
        	color:White;
        	font-size:11px;
        	font-weight:bold;
        }
        
        .search-input {font-size:11px}
        .align-center {text-align:center}
        .align-right {text-align:right}
        
        #gridSearchResult tbody tr td  {padding:3px}
    </style>
</head>
<body class="search" style="background: white;">
    <!--  background="Images/arr_dbl_blu.gif"-->
    <form id="SearchPromoType" runat="server">
        <div id="subcontainer" align="center">
            <table style="width: 586px">
                <tr>
                    <td>
                        <table id="Table1">
                            <tr>
                                <td style="width: 2px;">
                                </td>
                                <td style="text-align: left">
                                    <span style="font-size: 9pt">Type Desc</span></td>
                                <td style="text-align: left">
                                    <span style="font-size: 9pt">Process Group</span></td>
                                <td style="text-align: left">
                                    <span style="font-size: 9pt"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2px;">
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtTypeDesc" runat="server" Height="20px" Width="225px" CssClass="search-input"></asp:TextBox></td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtProcessGroup" runat="server" Height="20px" Width="225px" CssClass="search-input"></asp:TextBox></td>
                                <td style="text-align: left">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" Height="27px" /></td>
                                <td style="text-align: left">
                                    <asp:Button ID="cmdOk" runat="server" Text="Ok" Height="27px" Width="60px" /></td>
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
                                        ForeColor="#333333" GridLines="None" Width="685px" Font-Size="8pt" AllowPaging="false"
                                        AllowSorting="True" Font-Bold="True">
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowSel" runat="server" OnCheckedChanged="chkRowSel_CheckedChanged"
                                                        EnableTheming="True" AutoPostBack="true" />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkALL" runat="server" OnCheckedChanged="chkALL_CheckedChanged"
                                                        AutoPostBack="True" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PromoTypeID" HeaderText="Type ID">
                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="HiddenObject" />
                                                <ItemStyle CssClass="HiddenObject" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="TypeDesc" HeaderText="Type Desc">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ProcessGroup" HeaderText="Process Group">
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblGridMessage" runat="server" Text="* No record found matching this criteria *"
                                                Font-Size="10pt" Width="264px"></asp:Label>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hidShortDesc" runat="server" />
            <asp:HiddenField ID="hidOutputValue" runat="server" />
            <asp:HiddenField ID="hidOutputAll" runat="server" />            
        </div>
    </form>
</body>
</html>
