<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchPromotion.aspx.vb" Inherits="SearchPromotion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
          <div id="subcontainer" align="center" >
                    <table style="width: 695px; height: 17px">
                        <tr>
                            <td align="center" bgcolor="background" width="700">
                                &nbsp;<asp:TextBox ID="txtMemoNumber" runat="server" Width="294px" ForeColor="Transparent" MaxLength="100" ></asp:TextBox>
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="167px">
                                    <asp:ListItem>Memo Number</asp:ListItem>
                                    <asp:ListItem>Promo Title</asp:ListItem>
                                </asp:DropDownList>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
                        </tr>
                    </table>
        <table style="width: 699px; height: 199px">
            <tr>
                <td colspan="2" style="width: 700px; height: 10px;" valign="top">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="RequestID" DataSourceID="sqldsRequests" ForeColor="#333333" GridLines="None"
                        Width="100%" AllowPaging="True" AllowSorting="True" Font-Size="8pt" PageSize="5">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField HeaderText="Memo Number">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("MemoNumber") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" CommandArgument='<%# Bind("RequestID") %>'
                                        Text='<%# Bind("MemoNumber") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                            <asp:BoundField DataField="PromoPeriodFrom" HeaderText="Period From" SortExpression="PromoPeriodFrom" />
                            <asp:BoundField DataField="PromoPeriodTo" HeaderText="Period To" SortExpression="PromoPeriodTo" />
                            <asp:TemplateField HeaderText="Branches" SortExpression="Branches">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Branches") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    &nbsp;<asp:Literal ID="Literal1" runat="server" Text='<%# Bind("Branches") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
        </div>
        <asp:SqlDataSource ID="sqldsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            DeleteCommand="DELETE FROM [PromoRequests] WHERE [RequestID] = @RequestID" InsertCommand="INSERT INTO [PromoRequests] ([RequestDate], [Title], [PromoPeriodFrom], [PromoPeriodTo], [PreAppBy], [ApprovedBy], [DateApproved], [RequestBy], [Remarks]) VALUES (@RequestDate, @Title, @PromoPeriodFrom, @PromoPeriodTo, @PreAppBy, @ApprovedBy, @DateApproved, @RequestBy, @Remarks)"
            SelectCommand="USP_SearchPromotions" SelectCommandType="StoredProcedure" UpdateCommand="UPDATE [PromoRequests] SET [RequestDate] = @RequestDate, [Title] = @Title, [PromoPeriodFrom] = @PromoPeriodFrom, [PromoPeriodTo] = @PromoPeriodTo, [PreAppBy] = @PreAppBy, [ApprovedBy] = @ApprovedBy, [DateApproved] = @DateApproved, [RequestBy] = @RequestBy, [Remarks] = @Remarks WHERE [RequestID] = @RequestID">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtMemoNumber" DbType="String" DefaultValue=" " Name="MemoNumber"
                    PropertyName="Text" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="DropDownList1" Name="WhereCriteria" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="RequestID" Type="Int16" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter DbType="Date" Name="RequestDate" />
                <asp:Parameter Name="Title" Type="String" />
                <asp:Parameter DbType="Date" Name="PromoPeriodFrom" />
                <asp:Parameter DbType="Date" Name="PromoPeriodTo" />
                <asp:Parameter Name="PreAppBy" Type="String" />
                <asp:Parameter Name="ApprovedBy" Type="String" />
                <asp:Parameter DbType="Date" Name="DateApproved" />
                <asp:Parameter Name="RequestBy" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:Parameter Name="RequestID" Type="Int16" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter DbType="Date" Name="RequestDate" />
                <asp:Parameter Name="Title" Type="String" />
                <asp:Parameter DbType="Date" Name="PromoPeriodFrom" />
                <asp:Parameter DbType="Date" Name="PromoPeriodTo" />
                <asp:Parameter Name="PreAppBy" Type="String" />
                <asp:Parameter Name="ApprovedBy" Type="String" />
                <asp:Parameter DbType="Date" Name="DateApproved" />
                <asp:Parameter Name="RequestBy" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
