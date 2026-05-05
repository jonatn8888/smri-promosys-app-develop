<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    CodeFile="BinRangeGroupEntry.aspx.vb" Inherits="BinRangeGroupEntry" Title="Bin Range Group Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <br />
    <div id="menu" style="display: none;">
        <br />
        <br />
        <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" /><br />
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="119px" />
        <br />
        <br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="119px" /><br />
        <br />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="119px" Visible="False" />
        <br />
    </div>
    <div id="contents">
        <span style="font-size: 12pt"><strong>&nbsp;Bin Range Group Maintenance</strong></span><br />
        <hr />
    </div>
    <table runat="server" style="width: 80%; margin-left: 10px;">
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 15px;">
                    Group ID :
                </label>
                <asp:TextBox ID="txtLoyaltyGroupId" runat="server" Width="260px" MaxLength="50" Enabled="false"></asp:TextBox>
            </td>
            <td style="width: 40%;">
                <label>
                    Company :
                </label>
                <asp:TextBox ID="txtCompany" runat="server" Width="280px" MaxLength="50" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                <label>
                    Description :
                </label>
                <asp:TextBox ID="txtDescription" runat="server" Width="260px" MaxLength="50"></asp:TextBox>
            </td>
            <td style="width: 40%;">
                &nbsp;
            </td>
        </tr>
    </table>
    
    <div style="padding-left:90px;">
        <asp:HiddenField ID="lastCtrl" runat="server" />
        <asp:Button ID="btnSaveDesc" runat="server" Text="Save Description" Height="25px" OnClientClick="SaveOnClick();" />
    </div>
    
    <hr />
    
    <div style="width: 100%; padding: 10px;">
        <label>
            Bank BIN :
        </label>
        <asp:DropDownList ID="cboBankBin" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
            DataValueField="BankBIN" AutoPostBack="false" Width="425">
            <asp:ListItem Selected="True" Value="-1">- Select Bank BIN -</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
        
        <asp:Button ID="btnAddBin" runat="server" Text="Add BIN" Height="25px" />
        <asp:Button ID="btnRemove" runat="server" Text="Remove Selected" Height="25px" OnClientClick="RemoveOnClick();" />
        <br />
        
        <asp:Label ID="lblRecordCount" runat="server" ForeColor="Green" ></asp:Label>
        
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="EnvCode,BankBin" DataSourceID="sqlds" Font-Bold="False" Font-Names="Trebuchet MS"
            Font-Overline="False" Font-Size="8pt" ForeColor="#333333" GridLines="None" Width="691px"
            AllowPaging="True" PageSize="20" ShowHeaderWhenEmpty="True" EmptyDataText="No records to show">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="Bank Bin">
                    <ItemTemplate>
                        <asp:CheckBox ID="lnkChk" runat="server" />
                        &nbsp;<asp:Label ID="lnkBankBin" runat="server" Text='<%# eval("BankBin") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Issuer">
                    <ItemTemplate>
                        &nbsp;<asp:Label ID="lnkBankName" runat="server" Text='<%# eval("BankName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Card Brand">
                    <ItemTemplate>
                        &nbsp;<asp:Label ID="lnkCardBrand" runat="server" Text='<%# eval("CardBrand") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Card Type">
                    <ItemTemplate>
                        &nbsp;<asp:Label ID="lnkCardType" runat="server" Text='<%# eval("CardType") %>'></asp:Label>
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
    
    <script type="text/javascript">
        function SaveOnClick() {
            var hidFld = document.getElementById("<%=lastCtrl.ClientID %>");
            hidFld.value = "btnSaveDesc";
        }
        
        function RemoveOnClick() {
            var hidFld = document.getElementById("<%=lastCtrl.ClientID %>");
            hidFld.value = "btnRemove";
        }
    </script>
</asp:Content>