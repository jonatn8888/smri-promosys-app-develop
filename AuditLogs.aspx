<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AuditLogs.aspx.vb" Inherits="AuditLogs" %>

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
        <asp:GridView ID="gridBranches" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlAuditTrail" Font-Size="Small"
            ForeColor="#333333" GridLines="None" Height="10px" Width="100%">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="EntryDate" HeaderText="Transaction Date " SortExpression="EntryDate" />
                <asp:BoundField DataField="EntryAction" HeaderText="Action" SortExpression="EntryAction" />
                <asp:BoundField DataField="UserId" HeaderText="User" SortExpression="UserId" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                &nbsp;
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" Height="10px" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlAuditTrail" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="USP_ViewAuditTrail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter Name="EntryID" QueryStringField="EntryID" Type="Int32" />
                <asp:QueryStringParameter Name="EntryType" QueryStringField="EntryType" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
