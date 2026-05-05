<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PromoDetails.aspx.vb" Inherits="PromoDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
     <script type="text/javascript">
     function closeWin()
     {
        parent.emailwindow.hide();
     }
    </script>
    
</head>

<body  style="background: white;">  
        <form id="form1" runat="server">
        <div id="subvbChoice">
            <br />
            <table style="width: 1px; height: 42px">
                <tr>
                    <td rowspan="3" style="width: 15px" valign="top">
                        <img src="Images/question-mark.jpg" style="height: 61px" /></td>
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Choose Promo Details entry type" Font-Bold="True" Height="31px" Width="229px"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="height: 17px">
                <input runat="server" id="Button1" type="button" value="Departmental" style="width: 105px" /></td>
                    <td style="height: 17px">
                <input runat="server" id="Button2" type="button" value="Standard" style="width: 105px" /></td>
                </tr>
            </table>
            <div align="justify">
                &nbsp;</div>
        </div>
        <asp:HiddenField ID="hidFlag" runat="server" />
        
        </form>
</body>
</html>
