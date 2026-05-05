<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="ArchiveMemos.aspx.vb" Inherits="ArchiveMemos" title="Archive Memos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
    <div id="contents" style="width: 97%; margin-right: 10px;">
        <strong><span style="font-size: 11pt; color: #556b2f">
            Archive Memos<br />
            <br />
         </span></strong>
        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="500px">
        </asp:BulletedList>
        <br />
        <table>
            <tr>
                <td>
                    <strong><span style="text-decoration: underline">
                Search Parameters</span></strong></td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlSearchField" runat="server" Width="148px">
                        <asp:ListItem Selected="True" Value="M.MEMONUMBER">MEMO NO.</asp:ListItem>
                        <asp:ListItem Value="M.TITLE">TITLE</asp:ListItem>
                    </asp:DropDownList></td>
                <td>
                    <asp:TextBox ID="txtSearchValue" runat="server" ClientIDMode="Static" Width="373px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    Memo Period :</td>
                <td>
                    From
                    <asp:TextBox ID="txtFrom" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    <input id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtFrom');"
                        style="left: 322px; top: 345px" type="button" />
                    To
                    <asp:TextBox ID="txtTo" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    <input id="calPeriodTo" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtTo');"
                        style="left: 322px; top: 345px" type="button" />&nbsp;
                    <asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
            </tr>
        </table>
        <br />
     
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
                <asp:TemplateField HeaderText="Memo Number">
                    <ItemStyle Width="10px" />
                    <ItemTemplate>
                        <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>' Width="107px" Visible="False"></asp:Label>
                        <asp:Label ID="lblMemoNo" runat="server" Text='<%# Eval("MemoNumber") %>'
                            Width="107px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ApproveDate" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Memo Date"
                    SortExpression="ApproveDate">
                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
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
                <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="250px" CssClass="ClippedRegion" Wrap="False" />
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
            SelectCommand="PMS_P_ARCHIVEMEMOS_S" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlSearchField" Name="SearchField" PropertyName="SelectedValue"
                    Type="String" />
                 <asp:ControlParameter ControlID="txtSearchValue" Name="SearchValue"  PropertyName="Text"  DefaultValue="null"
                    Type="String" />
                 <asp:ControlParameter ControlID="txtFrom" Name="DateFrom"  PropertyName="Text"
                    Type="DateTime"  />
                 <asp:ControlParameter ControlID="txtTo" Name="DateTo"  PropertyName="Text"
                    Type="DateTime"  />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>

</asp:Content>

