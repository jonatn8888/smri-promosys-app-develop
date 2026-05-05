<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MemoBranches.aspx.vb" Inherits="MemoBranches" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>View</title>
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)" /><meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)" /> 
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
</head>
<body class="search" style="background: white;">
    <form id="form1" runat="server">
        <div id="subcontainer" align="center" style="width: 515px;">
            <asp:GridView ID="gridMemoBranches" runat="server"
                AutoGenerateColumns="False" CellPadding="5" Font-Bold="True" Font-Size="9pt"
                ForeColor="#333333" GridLines="None" PageSize="20" Width="510px">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField>
                        <ControlStyle CssClass="HiddenObject" />
                        <FooterStyle CssClass="HiddenObject" />
                        <HeaderStyle CssClass="HiddenObject" />
                        <ItemStyle CssClass="HiddenObject" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="BranchCode" HeaderText="Code" SortExpression="BranchCode">
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BranchName" HeaderText="Branch Name" SortExpression="BranchName">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="VSstatusDesc" HeaderText="Promo Status" SortExpression="VSstatusDesc">
                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyGrid" runat="server" Font-Size="10pt" Text="* No record found matching this criteria *"
                        Width="264px"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
