<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    CodeFile="rvReport.aspx.vb" Inherits="rvReport" Title="Untitled Page" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="width: 912px">
        <tr>
            <td align="center" colspan="3" rowspan="3">
                <rsweb:ReportViewer ID="rv" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="700px"
                    Width="800px" SizeToReportContent="True">
                </rsweb:ReportViewer>                
            </td>
        </tr>
    </table>
</asp:Content>
