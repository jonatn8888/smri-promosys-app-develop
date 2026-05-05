<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Project_Portal_UAT.aspx.vb" Inherits="Project_Portal_UAT" %>

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
                &nbsp;Promotion Monitoring System UAT Project Portal</span></strong></span>
            <hr />
            <span style="font-size: 14pt; font-family: Trebuchet MS"><strong><span style="color: #ffffff">
                </span></strong></span>
        <br />
        &nbsp;<br />
        <table cellpadding="4" cellspacing="0" border="1" >
            <tr>
                <td style="width: 189px; height: 5px" valign="top">
                    &nbsp;<span style="font-size: 14pt; color: #ccff00">Normal Login</span></td>
                <td style="width: 424px; height: 5px" valign="top">
                    <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                    <ul>
                        <li><a href="CMMpage.aspx"></a>
                            <asp:LinkButton ID="lnkSameUser" runat="server" Width="410px" ForeColor="White">Login using same User Account</asp:LinkButton></li>
                    </ul>
                    </span>
                </td>
            </tr>
            <tr>
                <td style="width: 189px; height: 49px" valign="top">
                    <span style="color: #ccff00; font-family: Trebuchet MS; font-size: 14pt;">Merchandising</span></td>
                <td style="width: 424px; height: 49px" valign="top">
                    <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                    <ul>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen1" runat="server" Width="410px" ForeColor="White">Promo Requester 01 (LTBG)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen2" runat="server" Width="410px" ForeColor="White">Promo Requester 02 (AMC)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen3" runat="server" Width="410px" ForeColor="White">Promo Requester 03 (MISC)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen4" runat="server" Width="410px" ForeColor="White">Promo Requester 04 (WAP)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen5" runat="server" ForeColor="White" Width="410px">Promo Requester 05 (TWP)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen6" runat="server" ForeColor="White" Width="410px">Promo Requester 06 (SBU Marketing)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen7" runat="server" ForeColor="White" Width="410px">Promo Requester 07 (Character Merchandising)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCMMscreen8" runat="server" ForeColor="White" Width="410px">Promo Requester 08 (SACI)</asp:LinkButton> 
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkBCRscreen1" runat="server" ForeColor="White" Width="410px">Promo Requester 09 (BCR)</asp:LinkButton><br />&nbsp; 
                        </li>

                        <li>
                            <asp:LinkButton ID="lnkMdsgHeadscreen" runat="server" Width="408px" ForeColor="White">Request Reviewer 01 (All Business Units)</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkMdsgHeadscreen2" runat="server" ForeColor="White" Width="408px">Request Reviewer 02</asp:LinkButton> 
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkMdsgHeadscreen3" runat="server" ForeColor="White" Width="408px">Request Reviewer (SBU)</asp:LinkButton><br />&nbsp; 
                        </li>

                        <li>
                            <asp:LinkButton ID="lnkMBUscreen" runat="server" Width="408px" ForeColor="White">Request Approver 01 (All Business Units)</asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkMBUscreen2" runat="server" ForeColor="White" Width="408px">Request Approver 02</asp:LinkButton> 
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkBCRApprover1" runat="server" ForeColor="White" Width="408px">Request Approver (BCR)</asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkSBUApprover" runat="server" ForeColor="White" Width="408px">SBU Approver</asp:LinkButton>
                        </li>
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
                            <asp:LinkButton ID="lnkMPAscreen" runat="server" Width="408px" ForeColor="White">MPD Analyst 01</asp:LinkButton></a></li>
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
                                <asp:LinkButton ID="lnkBranch" runat="server" ForeColor="White" Width="407px">Branch User 1 Screen (LW, GTW - Cubao)</asp:LinkButton>
                                </span></li>
                           <li><span style="font-size: 14pt; color: #ffffff; font-family: Tahoma">
                               <asp:LinkButton ID="lnkBranch02" runat="server" ForeColor="White" Width="407px">Branch User 2 Screen (OurHome - Makati)</asp:LinkButton></span></li>
                       </ul>
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
                <td style="width: 189px; height: 30px" valign="top">
                    <span style="color: #ccff00; font-family: Trebuchet MS; font-size: 14pt;">Database Administrator<br />
                    </span>
                </td>
                <td style="width: 424px; height: 30px" valign="top">
                    <span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                    <ul>
                            <li><span style="font-size: 14pt; font-family: Tahoma; color: #ffffff">
                                <asp:LinkButton ID="lnkDBAdmin" runat="server" ForeColor="White" Width="217px">IT - Database Admin</asp:LinkButton></span></li>
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
                    <span style="font-size: 14pt; color: #ccff00">Home Page</span><br />
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
                <asp:LinkButton ID="lnkProjMan" runat="server" Font-Size="12px" ForeColor="Khaki" Font-Bold="False" Visible="False"><< Back to Project Management Page</asp:LinkButton></span></strong></span>&nbsp;</p>
    
    </div>
    </form>
    
</body>
</html>
