<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    CodeFile="BankRangeGroup.aspx.vb" Inherits="BankRangeGroup" Title="Bank Range Group Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <!--<asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" /><br />-->
        <br />
    </div>
    <div id="contents">
        <span style="font-size: 11pt; font-family: Trebuchet MS"><span style="font-size: 12pt">
            <strong>Bank Range Group Maintenance</strong></span><br />
            <hr />
            <br />
        </span>
        <table>
            <tr>
                <td>
                    <asp:DropDownList ID="cboEnvCode" runat="server" AppendDataBoundItems="True" DataTextField="EnvName"
                        DataValueField="EnvCode" AutoPostBack="true">
                        <asp:ListItem Selected="True" Value="-1">- Select Environment -</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtSearchInput" runat="server" MaxLength="8000"></asp:TextBox><asp:Button
                        ID="btnSearch" runat="server" Text="Search" Height="23px" />
                </td>
            </tr>
        </table>
        <br />
        
        <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="EnvCode,BankBin" DataSourceID="sqlds" Font-Bold="False" Font-Names="Trebuchet MS"
            Font-Overline="False" Font-Size="8pt" ForeColor="#333333" GridLines="None" Width="691px"
            AllowPaging="True" PageSize="20">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Bank Bin">
                    <ItemTemplate>
                        &nbsp;<asp:LinkButton ID="lnkBankBin" runat="server" CommandArgument='<%# String.Format("{0}|{1}", eval("EnvCode"), eval("BankBin")) %>'
                            Text='<%# eval("BankBin") %>' CommandName="View"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Issuer">
                    <ItemTemplate>
                        &nbsp;<asp:LinkButton ID="lnkBankName" runat="server" CommandArgument='<%# String.Format("{0}|{1}", eval("EnvCode"), eval("BankBin")) %>'
                            Text='<%# eval("BankName") %>' CommandName="View"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Card Brand">
                    <ItemTemplate>
                        &nbsp;<asp:LinkButton ID="lnkCardBrand" runat="server" CommandArgument='<%# String.Format("{0}|{1}", eval("EnvCode"), eval("BankBin")) %>'
                            Text='<%# eval("CardBrand") %>' CommandName="View"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Card Type">
                    <ItemTemplate>
                        &nbsp;<asp:LinkButton ID="lnkCardType" runat="server" CommandArgument='<%# String.Format("{0}|{1}", eval("EnvCode"), eval("BankBin")) %>'
                            Text='<%# eval("CardType") %>' CommandName="View"></asp:LinkButton>
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
        <asp:SqlDataSource ID="sqlds" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="EXEC USP_BankBinGetByEnvLoyaltyGrp @EnvCode, @LoyaltyGroupId, @ViewType, @Search">
            <SelectParameters>
                <asp:Parameter Name="EnvCode" DefaultValue="" />
                <asp:Parameter Name="LoyaltyGroupId" DefaultValue="0" />
                <asp:Parameter Name="ViewType" DefaultValue="0" />
                <asp:Parameter Name="Search" DefaultValue=" " />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
