<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoTypeList.aspx.vb" Inherits="PromoTypeList" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
    
    <br />
    <br />
    <div id="menu">
        <br />
        <br />
            <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" /><br />
        <br />
    </div>
    
    <div id="contents">        
        <span style="font-size: 11pt; font-family: Trebuchet MS"><span style="font-size: 12pt">
            <strong>Promotion Type Maintenance</strong></span><br />
            <hr />
            <br />
        </span>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="PromoTypeID" DataSourceID="sqldsPromoTypes" Font-Bold="False" Font-Names="Trebuchet MS"
            Font-Overline="False" Font-Size="8pt" ForeColor="#333333" GridLines="None"
            Width="691px" AllowPaging="True" PageSize="20">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblPromoTypeID" runat="server" Text='<%# Eval("PromoTypeID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TypeDesc" HeaderText="Promo Type" SortExpression="TypeDesc">
                    <ItemStyle Width="250px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ShortDesc" HeaderText="Short Desc" SortExpression="ShortDesc">
                    <ItemStyle Width="80px" Wrap="False" HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ForMPDuseOnly" HeaderText="MPD Use" SortExpression="ForMPDuseOnly">
                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="GROUPTYPE" HeaderText="Group Type">
                    <ItemStyle Width="20px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="sqldsPromoTypes" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT * FROM [PromoTypes] ORDER BY [TypeDesc]">
        </asp:SqlDataSource>
        </div>

</asp:Content>
