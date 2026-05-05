<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="BinRangeGroup.aspx.vb" Inherits="BinRangeGroup" Title="Bin Range Group Maintenance" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
    
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
            <strong>Bin Range Group Maintenance</strong></span><br />
            <hr />
            <br />
        </span>
        
        <table>
            <tr>
                <td>
                    <label>Environment : </label>
                    <asp:DropDownList ID="cboEnvCode" runat="server" AppendDataBoundItems="True" DataTextField="EnvName" DataValueField="EnvCode" AutoPostBack="true">
                        <asp:ListItem Selected="True" Value="-1">- Select Environment -</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtSearchInput" runat="server" MaxLength="8000"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="Search" Height="23px" />
                </td>
            </tr>
        </table><br />
        
        <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green" ></asp:Label>
        
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="LoyaltyGroupId" DataSourceID="sqlds" Font-Bold="False" Font-Names="Trebuchet MS"
            Font-Overline="False" Font-Size="8pt" ForeColor="#333333" GridLines="None"
            Width="691px" AllowPaging="True" PageSize="20">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Loyalty Group ID">
                    <ItemTemplate>
                        &nbsp;<asp:LinkButton ID="lnkLoyaltyGroupId" runat="server" CommandArgument='<%# String.Format("{0}|{1}", eval("EnvCode"), eval("LoyaltyGroupId")) %>'
                            Text='<%# eval("LoyaltyGroupId") %>' CommandName="View"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        &nbsp;<asp:LinkButton ID="lnkDescription" runat="server" CommandArgument='<%# String.Format("{0}|{1}", eval("EnvCode"), eval("LoyaltyGroupId")) %>'
                            Text='<%# eval("Description") %>' CommandName="View"></asp:LinkButton>
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
            SelectCommand="SELECT * FROM [LoyaltyGroup] WHERE EnvCode = @envCode AND 1 = CASE WHEN @searchParam = '' THEN 1 WHEN @searchParam <> '' AND [Description] LIKE '%'+@searchParam+'%' THEN 1 ELSE 0 END ORDER BY [Description];">
            <SelectParameters>
                <asp:Parameter Name="envCode" DefaultValue="-1" />
                <asp:Parameter Name="searchParam" DefaultValue="" />
            </SelectParameters>
        </asp:SqlDataSource>
        </div>

</asp:Content>
