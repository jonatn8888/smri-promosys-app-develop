<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProjectTestPortal.aspx.vb" Inherits="ProjectTestPortal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ePMS Access Page</title>

    <link rel="stylesheet" href=".\css\main.css" media="screen" />
    
</head>

<body bgcolor="#3366cc">
    
    <form id="form1" runat="server">
    
    <div id="contents">
    <p>
        <span style="font-size: 14pt; font-family: Trebuchet MS"><strong><span style="color: #ffffff">
        </span></strong></span>&nbsp;</p>
            <span style="font-size: 14pt; font-family: Trebuchet MS"><strong><span style="color: #ffffff">
                &nbsp;Promotion Monitoring System</span></strong></span>
            <hr />
            <span style="font-size: 14pt; font-family: Trebuchet MS"><strong><span style="color: #ffffff">
                </span></strong></span>
        <br />
                                <asp:TextBox ID="txtInputUsername" runat="server" Width="176px"></asp:TextBox>
        <asp:Button ID="btnLogin" runat="server" Text="Enter" /><br />
        <table cellpadding="4" cellspacing="0" >
            <tr>
                <td style="width: 189px; height: 5px" valign="top">
                    &nbsp;</td>
                <td style="width: 424px; height: 5px" valign="top">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 189px; height: 49px" valign="top">
                    <span style="color: #ccff00; font-family: Trebuchet MS; font-size: 14pt;">Merchandising</span></td>
                <td style="width: 424px; height: 49px" valign="top">
                    <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                    <ul>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen" runat="server" Width="410px" ForeColor="White">Promo Requester 01</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen2" runat="server" ForeColor="White" Width="410px">Promo Requester 02</asp:LinkButton><br>
                            &nbsp; </li>
                        <li>
                            <asp:LinkButton ID="lnkMdsgHeadscreen" runat="server" Width="410px" ForeColor="White">Request Reviewer 01</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkMdsgHeadscreen2" runat="server" ForeColor="White" Width="410px">Request Reviewer 02</asp:LinkButton><br>
                            &nbsp; </li>
                        <li>
                            <asp:LinkButton ID="lnkMBUscreen" runat="server" Width="408px" ForeColor="White">Request Approver 01</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkMBUscreen2" runat="server" ForeColor="White" Width="408px">Request Approver 02</asp:LinkButton><br />
                            &nbsp;</li><li>
                            <asp:LinkButton ID="lnkExecScreen" runat="server" ForeColor="White" Width="408px">Executive Approver</asp:LinkButton></li>
                    </ul>
                    </span>
                </td>
            </tr>
            <tr>
                <td style="width: 189px; height: 42px" valign="top">
                    <span style="font-size: 14pt; color: #ccff00">MCI</span></td>
                <td style="width: 424px; height: 42px" valign="top">
                    <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                    <ul>
                        <li>
                            <asp:LinkButton ID="lnkSMACscreen" runat="server" ForeColor="White" Width="408px">SMAC Deals Approver</asp:LinkButton></li></ul>
                        <p>
                            &nbsp;</p>
                    </span>
                </td>
            </tr>
            <tr>
                <td style="width: 189px; height: 87px" valign="top">
                    <span style="color: #ccff00; font-family: Trebuchet MS; font-size: 14pt;">MPD</span></td>
                <td style="width: 424px; height: 87px" valign="top">
                    <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                    <ul>
                        <li><a href="MPApage.aspx">
                            <asp:LinkButton ID="lnkMPAscreen" runat="server" Width="408px" ForeColor="White">MPD Analyst 01 (Memo Draft Creation)</asp:LinkButton></a></li>
                        <li>
                            <asp:LinkButton ID="lnkMPAscreen2" runat="server" ForeColor="White" Width="408px">MPD Analyst 02</asp:LinkButton><br />
                            &nbsp; </li>
                        <li><a href="SrMgrPage.aspx"></a>
                            <asp:LinkButton ID="lnkMemoPreApp" runat="server" Width="407px" ForeColor="White">Memo Reviewer 01 (Fashion)</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkMemoPreApp2" runat="server" ForeColor="White" Width="407px">Memo Reviewer 02 (Non-Fashion)</asp:LinkButton><br />
                            &nbsp; </li>
                        <li>
                            <asp:LinkButton ID="lnkMemoApprove" runat="server" Width="408px" ForeColor="White">Approver User Screen</asp:LinkButton><a href="CSYpage.aspx"></a></li>
                    </ul>
                        <p>
                            &nbsp;</p>
                    </span>                
                </td>
            </tr>
            <tr>
                <td style="width: 189px; height: 14px" valign="top">
                    <span style="color: #ccff00; font-family: Trebuchet MS; font-size: 14pt;">Branches<br />
                    </span>
                </td>
                <td style="width: 424px; height: 14px" valign="top">
                     <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                       <ul>
                            <li><span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                                <asp:LinkButton ID="lnkBranch" runat="server" ForeColor="White" Width="183px">Branch User Screen</asp:LinkButton>
                                </span></li></ul>
                     </span>   
                </td>
            </tr>
            <tr>
                <td style="width: 189px; height: 30px" valign="top">
                    <span style="color: #ccff00; font-family: Trebuchet MS; font-size: 14pt;">IT-POS<br />
                    </span>
                </td>
                <td style="width: 424px; height: 30px" valign="top">
                    <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                    <ul>
                            <li><span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                                <asp:LinkButton ID="lnkPOSscreen" runat="server" ForeColor="White" Width="217px">IT-POS User Screen</asp:LinkButton></span></li>
                    </ul>
                    </span>
                </td>
            </tr>
            <tr>
                <td style="width: 189px;" valign="top">
                    <span style="color: #ffffff; font-family: Trebuchet MS; font-size: 14pt;"><span style="color: #ccff00">
                        SCO</span><br />
                    </span>
                </td>
                <td style="width: 424px;" valign="top">
                        <ul>
                            <li><span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">SCO User Screen</span></li>
                        </ul>
                </td>
            </tr>
            <tr>
                <td style="width: 189px" valign="top">
                    <span style="font-size: 14pt; color: #ccff00">SCO</span><br />
                </td>
                <td style="width: 424px" valign="top">
                    <ul>
                        <li>
                            <asp:LinkButton ID="lnkDocumentation" runat="server" Font-Bold="True" Font-Size="Large"
                                ForeColor="White" Width="217px">Documentation Page</asp:LinkButton></li>
                    </ul>
                </td>
            </tr>
        </table>
        
        <p>
            <span style="font-size: 14pt; font-family: Trebuchet MS"><strong><span style="color: #ffffff">
            </span></strong></span></p>
        <p>
            <span style="font-size: 14pt; font-family: Trebuchet MS"><strong><span style="color: #ffffff">
                <asp:LinkButton ID="lnkProjMan" runat="server" Font-Size="12px" ForeColor="Khaki" Font-Bold="False"><< Back to Project Management Page</asp:LinkButton></span></strong></span>&nbsp;</p>
    
    </div>
    </form>
    
</body>
</html>
