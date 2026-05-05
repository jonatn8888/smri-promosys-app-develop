<%@ Page Language="VB"  AutoEventWireup="false" CodeFile="SearchDepSdep.aspx.vb" Inherits="SearchDepSdep" title="Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <title>Search</title>
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)" /><meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)" /> 
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <script src="js/jquery-1.2.3.min.js" type="text/javascript"></script>
    <script src="js/numbers.js" type="text/javascript"></script>
</head>

<body class="search"  style="background: white;"> <!--  background="Images/arr_dbl_blu.gif"-->
<form id="SearchDepSdep" runat="server">
 <div id="subcontainer" align="center" >
    <table style="width: 586px">
        <tr>
            <td>
<table id="Table1">
                       <tr><td style="width: 2px; height: 6px;"></td>
                           <td style="width: 28px; height: 6px; text-align: center;">
                               <span style="font-size: 9pt">
                                   <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Black" Text="Dp"></asp:Label></span></td>
                           <td style="width: 37px; height: 6px; text-align: center">
                               <span style="font-size: 9pt">
                                   <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Black" Text="SDp"></asp:Label></span></td>
                           <td style="width: 40px; height: 6px; text-align: center;">
                               <span style="font-size: 9pt">
                                   <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" Text="Cl"></asp:Label></span></td>
                           <td style="width: 40px; height: 6px; text-align: center; font-size: 9pt;">
                               <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Black" Text="SCl"></asp:Label></td>
                           <td style="width: 251px; height: 6px; text-align: center">
                               <span style="font-size: 9pt"><strong>
                                   <asp:Label ID="Label1" runat="server" ForeColor="Black" Text="Description"></asp:Label></strong></span></td>
                           <td style="width: 58px; height: 6px; text-align: center">
                               <span style="font-size: 9pt"></span></td>
                       </tr>
                        <tr>
                            <td style="width: 2px; height: 13px">
                            </td>
                            <td style="width: 28px; height: 13px">
                                <asp:TextBox ID="txtDepCode" runat="server" Height="20px" Width="25px" MaxLength="3"></asp:TextBox></td>
                            <td style="width: 37px; height: 13px; text-align: center;">
                                <asp:TextBox ID="txtSdepCode" runat="server" Height="20px" Width="25px" MaxLength="3"></asp:TextBox></td>
                            <td style="width: 40px; height: 13px; text-align: center;">
                                <asp:TextBox ID="txtClassCode" runat="server" Height="20px" Width="25px" MaxLength="3"></asp:TextBox></td>
                            <td style="width: 40px; height: 13px; text-align: center">
                                <asp:TextBox ID="txtSubClassCode" runat="server" Height="20px" MaxLength="3" Width="25px"></asp:TextBox></td>
                            <td style="width: 251px; height: 13px; text-align: center">
                                <asp:TextBox ID="txtDescription" runat="server" Height="20px" MaxLength="50" Width="242px"></asp:TextBox></td>
                            <td style="width: 58px; height: 13px; text-align: center">
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
                <asp:GridView ID="gvDeptSdeptClass" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="685px" Font-Size="8pt" AllowPaging="True" PageSize="5" DataSourceID="sqldsData" AllowSorting="True" Font-Bold="True">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# eval("cDeptCode") & ";" & eval("cSubDepCode")  & ";" & eval("cClassCode") & ";" & eval("cSubClassCode") & ";" & eval("Description") & ";" & eval("ShortDesc") & ";" & eval("EnvCode") %>'
                                    CommandName="Select" Text="Select"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="cDeptCode" HeaderText="Dp" SortExpression="cDeptCode" >
                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cSubDepCode" HeaderText="SDp" SortExpression="cSubDepCode" >
                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cClassCode" HeaderText="Cl" SortExpression="cClassCode" >
                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cSubClassCode" HeaderText="SCl">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" >
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ShortDesc" HeaderText="Short Description" SortExpression="ShortDesc" >
                            <ItemStyle Width="150px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EnvCode" HeaderText="EnvCode">
                            <ControlStyle CssClass="HiddenObject" />
                            <HeaderStyle CssClass="HiddenObject" />
                            <ItemStyle CssClass="HiddenObject" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataTemplate>
                        <asp:Label ID="Label5" runat="server" Text="* No record found matching this criteria *" Font-Size="10pt" Width="264px"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>  
        </tr>
    </table>
            </td>
        </tr>
    </table>

    <asp:HiddenField ID="hidShortDesc" runat="server" /><asp:HiddenField ID="hidEnvCode" runat="server" />
    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>"></asp:SqlDataSource>

 </div>
</form>
</body>
</html>
