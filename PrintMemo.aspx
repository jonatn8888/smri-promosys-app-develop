<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PrintMemo.aspx.vb" Inherits="PrintMemo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
    
    <div style="margin-top: 10px; margin-left: 20px">
        <rsweb:ReportViewer ID="rvMemo" runat="server" Width="95%" Font-Names="Verdana" Font-Size="8pt" Height="416px">
            <LocalReport ReportPath="repMemoDoc.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="dsPromotions_MemoDocInfo" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetMemoDocInfo"
            TypeName="dsPromotionsTableAdapters.MemoDocInfoTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMemoDocInfoByID"
            TypeName="dsPromotionsTableAdapters.MemoDocInfoTableAdapter" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="MemoID" QueryStringField="MemoID" Type="Int16" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
    </div>
    
</asp:Content>
