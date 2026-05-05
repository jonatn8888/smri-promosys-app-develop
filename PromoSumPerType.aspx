<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoSumPerType.aspx.vb" Inherits="PromoSumPerType" title="Promo per Type" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>

<br />
<br />
    <table style="width: 606px">
        <tr>
            <td style="width: 583px">
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 583px">
                <asp:Label ID="Label1" runat="server" Text="Promo Type:"></asp:Label></td>
            <td>
            </td>
            <td>
                <asp:DropDownList ID="ddlPromoType" runat="server" Width="427px" DataSourceID="sqldsPromoType" DataTextField="TypeDesc" DataValueField="PromoTypeID">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" style="width: 583px">
                <asp:Label ID="lblPromoP" runat="server" Text="Promo Period:"></asp:Label></td>
            <td>
            </td>
          <td style="width: 565px; height: 11px">
            <asp:TextBox ID="txtPeriodFrom" runat="server" Width="177px" MaxLength="50"></asp:TextBox>
            <input name="calPeriodFrom" type="button" id="calPeriodFrom" class="btnCal" OnClick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodFrom');" style="left: 322px; top: 345px" />&nbsp;
            <asp:TextBox ID="txtPeriodTo" runat="server" Width="177px" MaxLength="50"></asp:TextBox>
            <input name="calPeriodTo" type="button" id="calPeriodTo" class="btnCal" OnClick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodTo');" style="left: 547px; top: 345px"/></td>
        </tr>
        <tr>
            <td align="right" style="width: 583px">
            </td>
            <td>
            </td>
            <td style="width: 565px; height: 11px">
                <asp:Label ID="lblDisplay" runat="server" ForeColor="Navy"></asp:Label></td>
        </tr>
        <tr>
            <td align="right" style="width: 583px">
            </td>
            <td>
            </td>
            <td style="width: 565px; height: 11px" align="right">
                <asp:Button ID="btnView" runat="server" Text="View Report" /></td>
        </tr>
    </table>
    <asp:SqlDataSource ID="sqldsPromoType" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="USP_SelectPromoType" SelectCommandType="StoredProcedure">
    </asp:SqlDataSource>
    
</asp:Content>

