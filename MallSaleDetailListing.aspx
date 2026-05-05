<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"  CodeFile="MallSaleDetailListing.aspx.vb" Inherits="MallSaleDetailListing" title="Storewide Sale Listing" ValidateRequest="False" EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkDetailsEntry" runat="server">Participation Details</asp:LinkButton><br />
        <br />
    </div>
    
    <div id="contents">
        <asp:GridView id="gridBrandPromos" runat="server" ForeColor="#333333" Width="694px" BorderWidth="1px" AllowSorting="false" GridLines="Vertical" CellPadding="4" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gridBrandPromos_PageIndexChanging" PageSize="50">

                <EmptyDataTemplate>
                    <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 690px; height: 82px">
                        <tr>
                            <td style="height: 82px; text-align: center" valign="middle">
                                <em><span style="font-size: 11pt"><strong>no record found for this category.</strong></span></em></td>
                        </tr>
                    </table>
                </EmptyDataTemplate>

                <RowStyle BackColor="#F7F6F3" ForeColor="#333333"  />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                        </ItemTemplate>
                        <ControlStyle CssClass="HiddenObject" />
                        <HeaderStyle CssClass="HiddenObject" />
                        <ItemStyle CssClass="HiddenObject" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ItemCode" HeaderText="Dept/SDept/Class" SortExpression="ItemCode" >
                        <HeaderStyle HorizontalAlign="Center"  />
                        <ItemStyle HorizontalAlign="Center" Width="100px"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="ItemDesc" HeaderText="Brand" SortExpression="ItemDesc">
                        <ItemStyle Width="120px"  />
                        <HeaderStyle HorizontalAlign="Center"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoDesc" HeaderText="Promotion" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" ReadOnly="True" >
                        <HeaderStyle HorizontalAlign="Center"  />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center"  />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"  />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"  />
                <EditRowStyle BackColor="#999999"  />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
            </asp:GridView>
    </div>  
    
    <div style="display:none">
        &nbsp;&nbsp;
    </div>

</asp:Content>

