<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DBFTest.aspx.vb" Inherits="DBFTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<br />
        <br />
        <asp:TextBox ID="txtFolderName" runat="server" Width="215px"></asp:TextBox><br />
        <asp:Button ID="cmdMakeDir" runat="server" Text="Create Folder" Width="141px" TabIndex="1" /><br />
        <br />
        <br />
        <asp:Button ID="cmdCopy" runat="server" Text="Copy GIF File" Width="141px" TabIndex="2" /></div>
    </form>
</body>
</html>
