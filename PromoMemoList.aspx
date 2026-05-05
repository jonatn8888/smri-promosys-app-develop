<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoMemoList.aspx.vb" Inherits="PromoMemoList" Title="Promotion Memo" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

    <br />
    <div id="contents">
        <table cellpadding="3" cellspacing="0" style="width: 700px">
            <tr>
                <td colspan="3" style="width: 612px; height: 27px; text-align: left">
                    <strong><span style="color: darkolivegreen; font-size: 16px; font-family: 'Trebuchet MS';">Promotion
                        Memos</span></strong></td>
            </tr>
        </table>
        <br />
        <asp:DropDownList ID="cboFilterStatus" runat="server" AutoPostBack="True" Width="299px">
        </asp:DropDownList><br />
        <br />
        <asp:GridView ID="gridRequests" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="MemoID" DataSourceID="sqldsRequests" ForeColor="#333333" GridLines="Vertical"
            Width="701px" AllowSorting="True" BorderWidth="1px">
            <EmptyDataTemplate>
                <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 700px; height: 82px">
                    <tr>
                        <td style="height: 82px; text-align: center" valign="middle">
                            <em><span style="font-size: 11pt"><strong>no record found for this category.</strong></span></em></td>
                    </tr>
                </table>
            </EmptyDataTemplate>

            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField Visible="False">
                    <ItemStyle Width="10px" />
                    <ItemTemplate>
                        <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>' Width="107px"></asp:Label>
                        <asp:Label ID="lblStatus" runat="server" Width="82px"></asp:Label>
                        <asp:Label ID="lblCRID" runat="server" Text='<%# Eval("CRID") %>' Width="107px"></asp:Label>
                        <asp:Label ID="lblRequestID" runat="server" Text='<%# Eval("RequestID") %>' Width="107px"></asp:Label>
                        <asp:Label ID="lblReqType" runat="server" Text='<%# Eval("RequestType") %>' Width="82px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MemoDate" HeaderText="Memo Date" SortExpression="MemoDate" DataFormatString="{0:MM-dd-yyyy}">
                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title" >
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                    SortExpression="PromoPeriodFrom">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                    SortExpression="PromoPeriodTo">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="PreparedBy" HeaderText="Prepared By" SortExpression="PreparedBy">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="120px" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:GridView ID="gridSpecial" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="RequestID" DataSourceID="sqldsRequests" ForeColor="#333333" GridLines="Vertical"
            Width="701px" AllowSorting="True" BorderWidth="1px" Visible="False">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestID" runat="server" Text='<%# Eval("RequestID") %>' Width="107px"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10px" />
                </asp:TemplateField>
                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" DataFormatString="{0:MM-dd-yyyy}">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title" >
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                    SortExpression="PromoPeriodFrom">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                    SortExpression="PromoPeriodTo">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="120px" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 700px; height: 82px">
                    <tr>
                        <td style="height: 82px; text-align: center" valign="middle">
                            <em><span style="font-size: 11pt"><strong>no record found for this category.</strong></span>
                            </em>
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        &nbsp;&nbsp;<table cellspacing="0" style="width: 700px">
            <tr>
                <td style="width: 213px">
                    </td>
                <td style="width: 391px">
                </td>
                <td align="right" style="width: 173px">
                    <asp:LinkButton ID="lnkRefresh" runat="server" Width="75px" Font-Underline="False" CssClass="action-link" Visible="False">Refresh List</asp:LinkButton></td>
            </tr>
        </table>
        <br />
        <br />
        <br />       
        <asp:SqlDataSource ID="sqldsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>">
        </asp:SqlDataSource>
    </div>

</asp:Content>