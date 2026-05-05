<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
<script language="javascript" type="text/javascript">
<!--

function container_onclick() {

}

// -->
</script>
</head>

<body>
    <form id="form1" runat="server">
    
    <div id="container" style="height: 450px">
        <div style="margin: 20px 10px 30px 10px;">
        
        <table style="width: 101%; height: 423px">
            <tr>
                <td align="center" style="height: 91%; background: url(Images/lockdark.jpg) no-repeat left bottom;" valign="top">
                    &nbsp;<table style="width: 854px; height: 200px">
                        <tr>
                            <td align="center" colspan="3" rowspan="1" style="height: 163px; background-position: center top; background-image: url(Images/header02.jpg); background-repeat: no-repeat;" valign="top">
                                <br />
                                <span style="font-size: 18pt"><strong><span style="color: #ffffff">
                                    <br />
                                   <span style="font-size: 20pt"></span></span></strong></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="3" rowspan="3" style="height: 132px" valign="bottom">
                                <table style="width: 424px; height: 56px">
                                    <tr>
                                        <td align="left" colspan="4">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="DarkBlue" Text="Please enter your username and password to continue."
                                                Width="329px"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 23px">
                                        </td>
                                        <td align="right" style="width: 92px">
                                            <asp:Label ID="Label1" runat="server" Text="User Name:" Width="82px" ForeColor="DarkBlue" Font-Bold="True"></asp:Label></td>
                                        <td align="left" style="width: 234px">
                                            <asp:TextBox ID="txtUserName" runat="server" Width="224px"></asp:TextBox></td>
                                        <td align="left" style="width: 117px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="width: 23px">
                                        </td>
                                        <td align="right" style="width: 92px">
                                            <asp:Label ID="Label2" runat="server" Text="Password:" ForeColor="DarkBlue" Font-Bold="True"></asp:Label></td>
                                        <td align="left" style="width: 234px">
                                            <asp:TextBox ID="txtPassword" runat="server" Width="225px" TextMode="Password"></asp:TextBox></td>
                                        <td align="left" style="width: 117px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 23px; height: 10px;">
                                        </td>
                                        <td style="width: 92px; height: 10px;">
                                        </td>
                                        <td align="right" style="width: 234px; height: 10px;">
                                            </td>
                                        <td align="right" style="width: 117px; height: 10px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 23px">
                                        </td>
                                        <td style="width: 92px">
                                        </td>
                                        <td align="right" style="width: 234px">
                                            <asp:Button ID="cmdLogin" runat="server" Text="Log in" Width="74px" /></td>
                                        <td align="right" style="width: 117px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div>
                    <asp:Label ID="lblTestEnvMsg" runat="server" Text="TEST ENVIRONMENT" Font-Size="X-Large" Font-Bold="True" Font-Names="Lydian" ForeColor="#C00000" Visible="False"></asp:Label>
                    </div>
                    
                </td>
            </tr>
            
            <tr>
                <td>
                <hr style="width:100%" />
                    &nbsp;</td>
            </tr>
            
        </table>
        
        <br />
        
        </div>
    </div>
    </form>
</body>
</html>
