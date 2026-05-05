<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoReqListSMAC.aspx.vb" Inherits="PromoReqListSMAC" Title="Sales Promotion Request" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

    <br />
    <br />
    
    <div id="contents">
        <table cellpadding="3" cellspacing="0" style="width: 605px">
            <tr>
                <td colspan="3" style="width: 612px; height: 27px; text-align: left">
                    <strong><span style="color: darkolivegreen; font-size: 16px; font-family: 'Trebuchet MS';">
                        SMAC Deals Promotional Requests</span></strong></td>
            </tr>
        </table>
        <br />
        <asp:DropDownList ID="cboFilterStatus" runat="server" AutoPostBack="True" Width="386px">
        </asp:DropDownList><br /><br />

        <asp:GridView ID="gridRequests" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="RequestID" DataSourceID="sqldsRequests" ForeColor="#333333" GridLines="Vertical"
            Width="694px" AllowSorting="True" BorderWidth="1px">

            <EmptyDataTemplate>
                <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 690px; height: 82px">
                    <tr>
                        <td style="height: 82px; text-align: center" valign="middle">
                            <em><span style="font-size: 11pt"><strong>no record found for this category.</strong></span></em></td>
                    </tr>
                </table>
            </EmptyDataTemplate>

            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestID" runat="server" Text='<%# Eval("RequestID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" DataFormatString="{0:MM-dd-yyyy}" ReadOnly="True" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title" >
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodFrom" HeaderText="From" SortExpression="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodTo" HeaderText="To" SortExpression="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy">
                    <ItemStyle Width="200px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="False">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="200px" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="sqldsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>">
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
                
    </div>
        <br />
</asp:Content>