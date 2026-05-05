<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PromoRequestSwipestakesMessage.aspx.vb" Inherits="PromoRequestSwipestakesMessage" title="Untitled Page" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Text Editor</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <link rel="stylesheet" type="text/css" href=".\css\main_print.css" media="print" />
    <link rel="stylesheet" type="text/css" href=".\css\DatePicker.css" media="screen" />  
    
    
    
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
    <script src="rte/richtext.js" type="text/javascript"></script>
    <script src="rte/config.js" type="text/javascript"></script>
    <script src="js/jquery-1.2.3.min.js" type="text/javascript"></script>
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
    <script src="js/numbers.js" type="text/javascript" ></script> 
    
    <link rel="stylesheet" type="text/css" href="CSS/TabbedWidget.css">
    <script type="text/javascript" src="jQuery/jquery-1.4.3.min.js"></script>
    <script type="text/javascript" src="jQuery/tabbedwidget.js"></script>


    <script type="text/javascript">
        function SelectAll(source) {
            if (source.checked) {
                $("#gvBinRange input[name$='chkRowSel']").each(function (index) {
                    $(this).attr('checked', true);
                });
            }
            else {
                $("#gvBinRange input[name$='chkRowSel']").each(function (index) {
                       $(this).attr('checked', false);
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="transactionType" runat="server" />
        <div  style="width: 600px; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke; clear: both; clip: rect(auto auto auto auto); text-align: left;">
        
        
        <div class="tabs" style="float:left;">        
            <ul class="tabNavigation" style="background-color: whitesmoke;">
                <a href="#winningMessage"><li class="tab">Winning Message</li> </a>  
                <a href="#nonwinningMessage"><li class="tab">Nonwinning Message</li> </a>   
                <a href="#posMessage"><li class="tab">POS Message</li>  </a>   
            </ul> 

            <div class="sub-div" style="border-color: transparent !imporant; margin-top: 0px !important;">
                <div id="winningMessage">
                    <table style="width: 578px; border-top-style: none; border-right-style: none; border-left-style: none;
                        border-bottom-style: none;">
                        <tr id="trTPL_WinningMsg" runat="server" visible="true">
                            <td style="text-align: left;">
                                <asp:Label ID="txtXML_WinningMsg01" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg02" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg03" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg04" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg05" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg06" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg07" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg08" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg09" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg10" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg11" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg12" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg13" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg14" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg15" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg16" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg17" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg18" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg19" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_WinningMsg20" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nonwinningMessage">
                    <table style="width: 578px; border-top-style: none; border-right-style: none; border-left-style: none;
                        border-bottom-style: none">
                        <tr id="tr2" runat="server" visible="true">
                            <td style="text-align: left">
                                <asp:Label ID="txtXML_NonwinningMsg01" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg02" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg03" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg04" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg05" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg06" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg07" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg08" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg09" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg10" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg11" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg12" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg13" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg14" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg15" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg16" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg17" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg18" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg19" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_NonwinningMsg20" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div> 
                <div id="posMessage">
                    <table style="width: 578px; border-top-style: none; border-right-style: none; border-left-style: none;
                        border-bottom-style: none">
                        <tr id="tr3" runat="server" visible="true">
                            <td colspan=2 style="text-align: left">
                                <asp:Label ID="txtXML_POSMsg01" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg02" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg03" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg04" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg05" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg06" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg07" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg08" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg09" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg10" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg11" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg12" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg13" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg14" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg15" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg16" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg17" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg18" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg19" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label><br />
                                <asp:Label ID="txtXML_POSMsg20" runat="server" MaxLength="43" CssClass="Tb_Message" Width="100%"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div> 
            </div>
        </div> 
    </div>
    
    </form>
</body>
</html>
