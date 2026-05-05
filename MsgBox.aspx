<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MsgBox.aspx.vb" Inherits="MsgBox"  MaintainScrollPositionOnPostback="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
<link rel="stylesheet" href=".\css\DatePicker.css" media="screen" />
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
</head>
<body style="background: white;">

    <form id="form1" runat="server">          
        <table style="background-color: white;width:100%;height:100%">
            <tr>
                <td align="right" colspan="3">
                 </td>
            </tr>
                            <tr>
                                <td align="center" colspan="1" valign="middle"><asp:Image ID="imgIcon" runat="server" ImageUrl="~/Images/Symbols Tips 48x48.png" /></td>
                                <td align="left" colspan="2" valign="middle">
                                    <asp:Literal ID="litPopMessage" runat="server"></asp:Literal></td>
                            </tr>

                            <tr>
                 <td align="center" colspan="1" valign="top">
                 </td>
                 <td align="left" colspan="2">
                                     <div id="divTextDatePeriod" runat ="server" visible="false" style="background-color: white;" >
                     <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list">
                     </asp:BulletedList><br />
                                         <strong>
                                         Title:</strong>
                                         <asp:Label ID="lblPromoTitle" runat="server" BorderColor="SteelBlue"></asp:Label><br />
                                         <strong>
                                         Promo Period:</strong>
                                        <asp:TextBox id="txtPeriodFrom" runat="server" Width="140px" MaxLength="50"></asp:TextBox>
                                        <INPUT style="LEFT: 322px; TOP: 345px" id="calPeriodFrom" class="btnCal" onclick="displayDatePicker('txtPeriodFrom');" type=button name="calPeriodFrom" />
                                        <asp:TextBox id="txtPeriodTo" runat="server" Width="140px" MaxLength="50"></asp:TextBox>
                                        <INPUT style="LEFT: 547px; TOP: 345px" id="calPeriodTo" class="btnCal" onclick="displayDatePicker('txtPeriodTo');" type=button name="calPeriodTo" />
                                    </div>
                 </td>
             </tr>
                            <tr>
                                <td align="center" colspan="3">
                                <asp:Button ID="cmdPopUpOK" runat="server" Text="Ok" Width="62px" Height="24px" /> <asp:Button ID="cmdCancel" runat="server" Text="Cancel" Width="62px" Height="25px" /></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="1" style="width: 100px; height: 21px">
                                </td>
                                <td align="center" colspan="2" style="height: 21px; width: 382px;">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        
        <asp:Image ID="imgError" Visible="False" runat="server" ImageUrl="~/Images/Symbols-Error 48.png" />
        <asp:Image ID="imgQuestion" Visible="false" runat="server" ImageUrl="~/Images/symbols-help 48.png" />
        <asp:Image ID="imgOK" Visible="false" runat="server" ImageUrl="~/Images/Symbols Tips 48x48.png" />&nbsp;
        <asp:Image ID="imgFYI" Visible="false" runat="server" ImageUrl="~/Images/Symbols-Info 48.png" />
        <asp:Image ID="imgWarning" Visible="false" runat="server" ImageUrl="~/Images/symbols-warning 48.png" />
<asp:HiddenField ID="hidCurrPeriodFrom" runat="server" />
        <asp:HiddenField ID="hidCurrPeriodTo" runat="server" />

    </form>
</body>
</html>
