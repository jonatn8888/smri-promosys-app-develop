<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="rvPromoSumPerPromoType.aspx.vb" Inherits="rvPromoSumPerPromoType" title="Promo per Type" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<rsweb:reportviewer id="ReportViewer1" runat="server" font-names="Verdana" font-size="8pt"
                    height="500px" width="100%">
                <LocalReport ReportPath="repPromosumPerType.rdlc"><DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="dsRep_USP_SelectPromoSumPerType" />
                    </DataSources>
                </LocalReport>
                    </rsweb:reportviewer>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetData" TypeName="dsRepTableAdapters.USP_SelectPromoSumPerTypeTableAdapter">
                    <SelectParameters>
                        <asp:QueryStringParameter DefaultValue=" " Name="PromoType" QueryStringField="PromoType"
                            Type="String" />
                        <asp:QueryStringParameter DefaultValue=" " Name="PromoPeriodTo" QueryStringField="PromoPeriodTo"
                            Type="String" />
                        <asp:QueryStringParameter DefaultValue=" " Name="PromoPeriodFrom" QueryStringField="PromoPeriodFrom"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
</asp:Content>

