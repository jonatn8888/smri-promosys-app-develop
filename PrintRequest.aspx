<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrintRequest.aspx.vb" Inherits="PrintRequest" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <br />
        <rsweb:ReportViewer ID="rvMemo" runat="server" Width="817px">
        </rsweb:ReportViewer>
        &nbsp;</div>
    </form>
</body>
</html>
