<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="rvPriceEventPreview.aspx.vb" Inherits="rvPriceEventPreview" title="Untitled Page" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <br /><br />
    <div id="menu" style="top:200px;">
        <br />
        <br />
        <asp:LinkButton ID="lnkEditDoc" runat="server" PostBackUrl="~/PriceEventSearch.aspx">Select another Report</asp:LinkButton>
        <br />
        <br />
    </div>
    
    <div id="contents">
        <table cellpadding="3" cellspacing="0" style="width: 605px">
            <tr>
                <td colspan="3" style="width: 612px; height: 27px; text-align: left">
                    <strong><span style="font-size: 16px; color: darkolivegreen; font-family: 'Trebuchet MS'">
                        Price Event Reports</span></strong></td>
            </tr>
        </table>
        <strong><span style="font-size: 12pt">
    </span></strong>
        <rsweb:reportviewer id="rvPriceEvent" runat="server" font-names="Verdana" font-size="8pt"
            height="389px" width="98%" ShowDocumentMapButton="False" ShowRefreshButton="False" ZoomMode="PageWidth" InternalBorderStyle="None" ShowParameterPrompts="False">
<LocalReport ReportPath="repPriceEvent.rdlc"><DataSources>
    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="dsPromotions_PriceEvents" />
</DataSources>
</LocalReport>
</rsweb:reportviewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetPriceEventByEventNo" TypeName="dsPromotionsTableAdapters.PriceEventsTableAdapter">
            <SelectParameters>
                <asp:QueryStringParameter Name="EventNumber" QueryStringField="EventNumber" Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="TranType" QueryStringField="TranType"
                    Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="BranchCode" QueryStringField="BranchCode"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        &nbsp;
        <br />
        <br />

    
    </div>
</asp:Content>

