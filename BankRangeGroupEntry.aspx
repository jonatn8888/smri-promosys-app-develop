<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    CodeFile="BankRangeGroupEntry.aspx.vb" Inherits="BankRangeGroupEntry" Title="Bank Range Group Maintenance" %>

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
        <span style="font-size: 12pt"><strong>&nbsp;Bank Range Group Maintenance</strong></span><br />
        <hr />
    </div>
    
    <table runat="server" style="width: 80%; margin-left: 10px;">
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 0px;">
                    Bin Range :
                </label>
                <asp:TextBox ID="txtBinRange" runat="server" Width="260px" MaxLength="50" Enabled="false"></asp:TextBox>
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
                <label style="text-align: right; padding-left: 24px;">
                    Issuer :
                </label>
                <asp:DropDownList ID="cboIssuer" runat="server" AppendDataBoundItems="True" DataTextField="ElementName"
                    DataValueField="ResourceID" AutoPostBack="false" Width="263">
                    <asp:ListItem Selected="True" Value="-1">- Select Issuer -</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 20px;">
                    Brand :
                </label>
                <asp:DropDownList ID="cboBrand" runat="server" AppendDataBoundItems="True" DataTextField="ElementName"
                    DataValueField="ResourceID" AutoPostBack="false" Width="283">
                    <asp:ListItem Selected="True" Value="-1">- Select Card Brand -</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 30px;">
                    Type :
                </label>
                <asp:DropDownList ID="cboType" runat="server" AppendDataBoundItems="True" DataTextField="ElementName"
                    DataValueField="ResourceID" AutoPostBack="false" Width="263">
                    <asp:ListItem Selected="True" Value="-1">- Select Card Type -</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 40%; padding-left:62px;">
                <asp:CheckBox ID="chkActive" runat="server" />
                <asp:Label ID="lblActive" runat="server" Text="Set As Active" ></asp:Label>
            </td>
        </tr>
    </table>
    
    <div style="padding-left:82px;">
        <asp:HiddenField ID="lastCtrl" runat="server" />
        <asp:Button ID="btnSaveInfo" runat="server" Text="Save Info" Height="25px" OnClientClick="SaveOnClick();" />
    </div>
    
    <hr />
    
    <table id="Table1" runat="server" style="width: 80%; margin-left: 10px;">
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 0px;">
                    Loyalty 1 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup1" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="268">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 1 -</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 5px;">
                    Loyalty 2 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup2" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="282">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 2 -</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 0px;">
                    Loyalty 3 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup3" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="268">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 3 -</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 5px;">
                    Loyalty 4 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup4" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="282">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 4 -</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 0px;">
                    Loyalty 5 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup5" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="268">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 5 -</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 5px;">
                    Loyalty 6 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup6" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="282">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 6 -</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 0px;">
                    Loyalty 7 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup7" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="268">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 7 -</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 6px;">
                    Loyalty 8 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup8" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="282">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 8 -</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 0px;">
                    Loyalty 9 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup9" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="268">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 9 -</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 40%;">
                <label style="text-align: right; padding-left: 0px;">
                    Loyalty 10 :
                </label>
                <asp:DropDownList ID="cboLoyalGroup10" runat="server" AppendDataBoundItems="True" DataTextField="Combined"
                    DataValueField="LoyaltyGroupId" AutoPostBack="true" OnSelectedIndexChanged="ComboUpdateBySelection" Width="282">
                    <asp:ListItem Selected="True" Value="-1">- Select Loyalty 10 -</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    
    <script type="text/javascript">
        function SaveOnClick() {
            var hidFld = document.getElementById("<%=lastCtrl.ClientID %>");
            hidFld.value = "btnSaveInfo";
        }
        
        function RemoveOnClick() {
            var hidFld = document.getElementById("<%=lastCtrl.ClientID %>");
            hidFld.value = "btnRemove";
        }
    </script>
</asp:Content>
