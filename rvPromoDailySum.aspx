<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    CodeFile="rvPromoDailySum.aspx.vb" Inherits="rvPromoDailySum" Title="Promo Daily Summary" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    &nbsp;
    <table style="width: 912px">
        <tr>
            <td align="center" colspan="3" rowspan="3">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                    Height="679px" Width="697px">
                    <LocalReport ReportPath="repDailyPromoSum.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource Name="dsRep_USP_SelectDailyPromoSum" DataSourceId="ObjectDataSource1">
                            </rsweb:ReportDataSource>
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetData" TypeName="dsRepTableAdapters.USP_SelectDailyPromoSumTableAdapter">
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
        </tr>
    </table>
</asp:Content>
