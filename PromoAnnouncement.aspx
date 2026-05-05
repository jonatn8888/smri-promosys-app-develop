<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="PromoAnnouncement.aspx.vb" Inherits="PromoAnnouncement" title="Announcements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
    <div id="contents" style="width: 97%; margin-right: 10px;">
        <strong><span style="font-size: 11pt; color: #556b2f">
            Promotion Announcements<br />
            <br />
        </span></strong>
            <asp:DropDownList ID="ddlFilter" runat="server" Width="406px" AutoPostBack="True">
            <asp:ListItem Value="1">Current Promotions</asp:ListItem>
            <asp:ListItem Value="2">Ended Promotions</asp:ListItem>
            <asp:ListItem Value="3">Upcoming Promotions</asp:ListItem>
            <asp:ListItem Value="4">Cancelled Promotions</asp:ListItem>
        </asp:DropDownList><br />
        <br />
         <table id="tableSearchParam"  runat="server" visible="false"  style="width: 881px">
            <tr>
                <td colspan="7">
                    <strong><span style="text-decoration: underline">
                Search Parameters</span></strong></td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="500px">
                    </asp:BulletedList>
                </td>
            </tr>
            <tr>
                <td>
        <asp:DropDownList ID="cboSearchfield" runat="server" Width="141px">
                        <asp:ListItem Selected="True" Value="M.MEMONUMBER">MEMO NO.</asp:ListItem>
                        <asp:ListItem Value="M.TITLE">TITLE</asp:ListItem>
        </asp:DropDownList></td>
                <td>
        <asp:TextBox ID="txtSearchValue" runat="server" Width="219px"></asp:TextBox></td>
                <td align="right">
                    Memo Date:</td>
                <td align="right">
                    From</td>
                <td align="right">
        <asp:TextBox ID="txtFrom" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    <input id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtFrom');"
            style="left: 322px; top: 345px" type="button" /></td>
                <td align="right">
        To</td>
                <td>
        <asp:TextBox ID="txtTo" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
        <input id="calPeriodTo" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtTo');"
            style="left: 322px; top: 345px" type="button" />&nbsp;
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" /></td>
            </tr>
            <tr>
                <td align="right">
                    Promotion
        Type:</td>
                <td>
        <asp:DropDownList ID="cboPromoType" runat="server" Width="227px" AutoPostBack="True">
        </asp:DropDownList></td>
                <td align="right">
        <asp:Label ID="lblBizUnit" runat="server" Text="Business Unit:"></asp:Label></td>
                <td colspan="4">
        <asp:DropDownList ID="cboBizUnit" runat="server" Width="245px" AutoPostBack="True">
        </asp:DropDownList></td>
            </tr>
        </table> <br />
        <asp:GridView ID="gridRequests" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            BorderWidth="1px" CellPadding="4" DataKeyNames="MemoID" DataSourceID="sqldsData"
            ForeColor="#333333" GridLines="Vertical" Width="880px">
            <EmptyDataTemplate>
                <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 700px;
                    height: 82px">
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
                        <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>' Width="107px"></asp:Label>&nbsp;
                        <asp:HiddenField ID="lblCRID" runat="server" Value='<%# Eval("CRID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MEMONUMBER" HeaderText="Memo No." SortExpression="MEMONUMBER">
                    <ItemStyle Width="15%" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ApproveDate" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Memo Date"
                    SortExpression="ApproveDate">
                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="55%" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                    SortExpression="PromoPeriodFrom">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                    SortExpression="PromoPeriodTo">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches" Visible="False">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle CssClass="ClippedRegion" Wrap="False" />
                    <ControlStyle CssClass="ClippedRegion" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
 
        <div>
            
           <hr />
        </div>
               
        <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="USP_ViewPromoAnnouncements" SelectCommandType="StoredProcedure" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
            <SelectParameters>
                <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int16" />
                <asp:ControlParameter ControlID="ddlFilter" Name="Filter" PropertyName="SelectedValue"
                    Type="Int16" />
                 <asp:ControlParameter ControlID="cboBizUnit" Name="paramBizUnit" PropertyName="SelectedValue"
                    Type="String" DefaultValue="%" />
                 <asp:ControlParameter ControlID="cboPromoType" Name="paramPromoType" PropertyName="SelectedValue"
                    Type="String" DefaultValue=" " />
                 <asp:ControlParameter ControlID="txtSearchValue" Name="paramSearchValue" PropertyName="Text"
                    Type="String" DefaultValue=" " />
                 <asp:ControlParameter ControlID="cboSearchfield" Name="paramSearchField" PropertyName="SelectedValue"
                    Type="String" DefaultValue=" " />
                  <asp:ControlParameter ControlID="txtFrom" Name="paramDateFrom" PropertyName="Text"
                    Type="String" DefaultValue=" " />
                  <asp:ControlParameter ControlID="txtTo" Name="paramDateTo" PropertyName="Text"
                    Type="String" DefaultValue=" " />
            </SelectParameters>
        </asp:SqlDataSource>

    </div>

</asp:Content>

