<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InputBoxRadio.aspx.vb" Inherits="InputBoxRadio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
    
    <script type='text/javascript' src='js/jquery-1.2.3.min.js'></script>
    
    <style type="text/css">
        .exceeded{
            background-color:red;
        }
        
        #spn{
            font-size: 9px;
            text-align: right;
        }
        
    </style>
    
    <script type="text/javascript">
        $(function() {
            var limit = 500;
            var tb = $('textarea[id$=txtInputbox]');
            $(tb).keyup(function() {
                var len = $(this).val().length;
                if (len > limit) {
                    //this.value = this.value.substring(0, 50);
                    $(this).addClass('exceeded');
                    $('#spn').text("Exceeded " + (len - limit) + " characters");
                }
                else {
                    $(this).removeClass('exceeded');
                    $('#spn').text((limit - len) + " characters remaining");
                }
            });
 
            $('input[id$=cmdPopUpOK]').click(function(e) {
                var len = $(tb).val().length;
                if (len > limit) {
                    e.preventDefault();
                }
            });
        });
    </script>

</head>
<body style="background: white;">

    <form id="form1" runat="server">          
        <table style="width: 476px; height: 137px; background-color: white;">
            <tr>
                <td align="right" colspan="3" style="height: 21px">
                    &nbsp;</td>
            </tr>
                            <tr>
                                <td align="center" colspan="1" style="height: 21px; width: 100px;" valign="middle"><asp:Image ID="imgIcon" runat="server" ImageUrl="~/Images/Symbols Tips 48x48.png" /></td>
                                <td align="left" style="height: 21px; width: 382px;" colspan="2" valign="middle">
                                    <asp:Literal ID="litPopMessage" runat="server"></asp:Literal><br />
                                    
                                    <asp:TextBox ID="txtInputbox" runat="server" Width="367px" MaxLength="200" Height="55px" TextMode="MultiLine" ></asp:TextBox><br />
                                    <span id="spn">500 characters remaining</span>
                                    <span id="error"></span>
                                    
                                    <asp:RadioButtonList ID="rblReciever" runat="server" Height="27px" Width="372px" TabIndex="1">
                                        <asp:ListItem>Promo Requestor</asp:ListItem>
                                        <asp:ListItem Selected="True">MPD Analyst</asp:ListItem>
                                    </asp:RadioButtonList><div id="errormessage" runat="server" style="width:352px; height:28px; overflow:auto; background-color: WhiteSmoke; padding: 10px 10px 10px 10px;">
                                        <asp:Label ID="blistErrorMsg" runat="server" ForeColor="Red" Height="20px" Width="334px"></asp:Label>
                                    </div>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3" style="height: 21px" width="480">
                                <asp:Button ID="cmdPopUpOK" runat="server" Text="OK" Width="62px" Height="24px" TabIndex="2" /> <asp:Button ID="cmdCancel" runat="server" Text="Cancel" Width="62px" Height="25px" TabIndex="3" /></td>
                            </tr>
                        </table>
                        
        <asp:Image ID="imgError" Visible="False" runat="server" ImageUrl="~/Images/Symbols-Error 48.png" />
        <asp:Image ID="imgQuestion" Visible="false" runat="server" ImageUrl="~/Images/symbols-help 48.png" />
        <asp:Image ID="imgOK" Visible="false" runat="server" ImageUrl="~/Images/Symbols Tips 48x48.png" />
        <asp:HiddenField ID="hidchoice" runat="server" />
        &nbsp;

    </form>
</body>
</html>
