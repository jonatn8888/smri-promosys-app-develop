<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="HomePageCMM.aspx.vb" enableEventValidation="false" Inherits="HomePageCMM" Title="PromoSys :: Home Page" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

    <br />
    
    <div id="divHomeContent">
       
       <table style="width: 890px; height: 21px">
            <tr>
                <td style="width: 1093px; height: 140px;" valign="top">
        <div id="divHomeMain" style="width: 96%; left: 0px; top: 0px;" class="HomeBlock">
        <asp:Label ID="lblHeader" runat="server" Text="Welcome, " Font-Bold="False" Font-Names="Tahoma" Font-Size="18px"></asp:Label>
            <br />
            <br />
            <asp:Literal ID="litStatusMsg" runat="server"></asp:Literal><br />
            <br />
            &nbsp;</div>
                </td>
                <td style="width: 505px; height: 140px;">
                </td>
                <td style="height: 140px" valign="bottom">
        <div id="divHomeDownloads" style="width:150px; height: 102px;" class="HomeBlock">
            <br />
            Browser Upgrade<br />
            <br />
            Adobe Acrobat Reader<br />
            &nbsp;</div>
                </td>
            </tr>
            <tr>
                <td style="width: 1093px; height: 20px">
        <div id="divHomeCalendar" style="width:96%; height: 203px;" class="HomeBlock">
            <strong><span style="font-size: 11pt">CALENDAR OF STORE-WIDE PROMOS:</span></strong><br />
            <asp:GridView ID="gridStoreWidePromos" runat="server" AutoGenerateColumns="False"
                BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="670px" AllowPaging="True" PageSize="5" CssClass="selectable" OnPageIndexChanging="gridStoreWidePromos_PageIndexChanging" OnSorting="gridStoreWidePromos_Sorting" AllowSorting="True">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="gridTableStyleSmall" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>' Width="107px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="HiddenObject" />
                        <ControlStyle CssClass="HiddenObject" />
                        <FooterStyle CssClass="HiddenObject" />
                        <HeaderStyle CssClass="HiddenObject" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="MemoNumber" HeaderText="Memo No." SortExpression="MemoNumber">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="150px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ApproveDate" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Memo Date"
                        SortExpression="ApproveDate">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                        SortExpression="PromoPeriodFrom">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                        SortExpression="PromoPeriodTo">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches">
                        <ControlStyle CssClass="HiddenObject" />
                        <HeaderStyle CssClass="HiddenObject" HorizontalAlign="Center" />
                        <ItemStyle CssClass="HiddenObject" Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#284775" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" CssClass="gridPagerRowSmall" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#284775" Font-Bold="True" ForeColor="White" CssClass="gridTableStyleSmall" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EmptyDataTemplate>
                    <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 100%;
                        height: 55px">
                        <tr>
                            <td style="height: 82px; text-align: center" valign="middle">
                                <em><span style="font-size: 11pt"><strong>* no current or up-coming store-wide promo *</strong></span></em></td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
            </div>
                </td>
                <td style="width: 505px; height: 20px">
                </td>
                <td style="height: 20px" valign="top">

        <div id="divHomeHelp" style="width:150px; left: 158px;" class="HomeBlock">
            <br />
            <a href="SystemHelp.aspx">Getting Started</a><br />
            <br />
            <a href="SystemAbout.aspx">About PromoSys</a><br />
            <br />
            <a href="SystemSupport.aspx">Contact Support</a><br />
        </div>
                </td>
            </tr>
            <tr>
                <td style="width: 1093px">
                </td>
                <td style="width: 505px">
                </td>
                <td>
                </td>
            </tr>
        </table>
    
    <br />
        <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>">
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
        <br />
                
    </div>
</asp:Content>