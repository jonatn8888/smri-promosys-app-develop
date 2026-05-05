<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="rvPromoMonitoring.aspx.vb" Inherits="rvPromoMonitoring" title="Promotion Type" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
        Height="400px" Width="100%">
        
    <LocalReport ReportPath="repPromoMonitoring.rdlc"><DataSources>
    <rsweb:ReportDataSource Name="dsRep_USP_SelectPromoMonitoring" DataSourceId="ObjectDataSource1"></rsweb:ReportDataSource>
    </DataSources>
    </LocalReport>
    </rsweb:ReportViewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetData" TypeName="dsRepTableAdapters.USP_SelectPromoMonitoringTableAdapter">
                </asp:ObjectDataSource>
</div>

</asp:Content>

