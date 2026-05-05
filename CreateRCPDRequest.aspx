<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="CreateRCPDRequest.aspx.vb" Inherits="CreateRCPDRequest" title="RCPD Request" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td rowspan="3" style="width: 20px">
        </td>
        <td>
        <div id="SectionHeader" style="padding-right: 10px; padding-left: 10px; font-size: 16px;
        padding-bottom: 5px; width: 600px; color: white; padding-top: 5px; font-family: Tahoma;
        background-color: royalblue">
        <strong>
        Search Promotion for
            <asp:Label ID="lblRequstTypeDesc" runat="server"></asp:Label></strong></div>
        </td>
        <td rowspan="3">
           <div id="menu" style="width: 173px">
               <table>
                   <tr>
                       <td style="height: 40px">
                       </td>
                   </tr>
                    <tr>
                        <td style="height: 20px">
                    <asp:LinkButton ID="lnkBackToPreviousPage" runat="server">Back to Previous Page</asp:LinkButton>&nbsp;</td>
                    </tr>
                   <tr>
                       <td style="height: 20px">
                       </td>
                   </tr>
                </table>
           </div>
        </td>
    </tr>
    <tr>
        <td style="height: 40px">
            <table style="border-left-color: white; border-bottom-color: white;
                border-top-style: none; border-top-color: white; border-right-style: none; border-left-style: none;
                border-right-color: white; border-bottom-style: none; width: 638px;">
                <tr>
                    <td>
                    </td>
                    <td style="color: darkblue; font-style: italic; font-variant: small-caps">
                        <strong>Search Text</strong></td>
                    <td style="color: darkblue; font-style: italic; font-variant: small-caps">
                        <strong>Criteria</strong></td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearch" runat="server" Width="397px"></asp:TextBox></td>
                    <td>
                        <asp:DropDownList ID="ddlCriteria" runat="server" Width="173px">
                            <asp:ListItem Value="MemoNumber">Memo Number</asp:ListItem>
                            <asp:ListItem>Title</asp:ListItem>
                        </asp:DropDownList></td>
                    <td>
                        <asp:ImageButton ID="ImageButton1" runat="server" 
                            ImageUrl="~/Images/search_btn.jpg" Height="25px" /></td>
                </tr>
                <tr>
                    <td colspan="1">
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblValidateMessage" runat="server" BackColor="White" Font-Bold="True"
                            Font-Size="Larger" ForeColor="Green"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="1" style="height: 16px">
                    </td>
                    <td colspan="3" style="height: 16px">
                    </td>
                </tr>
            </table>
     </td>
    </tr>
    <tr>
        <td>
    <asp:GridView ID="gridRequests" runat="server" AutoGenerateColumns="False"
        BorderWidth="1px" CellPadding="4" DataKeyNames="RequestID" ForeColor="#333333"
        GridLines="Vertical" Width="694px">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MEMONUMBER" HeaderText="Memo #">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle Width="120px" />
            </asp:BoundField>
            <asp:BoundField DataField="MemoDate" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Memo Date"
                ReadOnly="True" SortExpression="RequestDate">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" Width="80px" />
            </asp:BoundField>
            <asp:BoundField DataField="Title" HeaderText="Promo Title">
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
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="False">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle Width="120px" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <EmptyDataTemplate>
            <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 694px;
                height: 82px">
                <tr>
                    <td style="height: 82px; text-align: center" valign="middle">
                        <em><span style="font-size: 11pt"><strong>no record found for this category.</strong></span></em></td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
        </td>
    </tr>
</table>
</asp:Content>

