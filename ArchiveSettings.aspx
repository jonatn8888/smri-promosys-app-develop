<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="ArchiveSettings.aspx.vb" Inherits="ArchiveSettings" title="Archive Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function opentexteditor()
{
    var url
    var title
    url = "MsgBox.aspx";
    title = "Utilities"; 
    MsgBoxwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width=510px,height=163px,center=1,resize=0,scrolling=0',"recall")
    MsgBoxwindow.onclose=function()
    { 

	return true 
	}
}

 function SetVisibility(rbutton) { 

        if (rbutton == 'D') {  
            document.getElementById('tblDaily').style.display = 'block';
            document.getElementById('tblWeekly').style.display = 'none'; 
            document.getElementById('tblMonthly').style.display = 'none'; 
            document.getElementById('tblQuarterly').style.display = 'none'; 
            document.getElementById('tblAnnually').style.display = 'none'; 
        }
        else if (rbutton == 'W') {  
            document.getElementById('tblDaily').style.display = 'none';
            document.getElementById('tblWeekly').style.display = 'block'; 
            document.getElementById('tblMonthly').style.display = 'none'; 
            document.getElementById('tblQuarterly').style.display = 'none'; 
            document.getElementById('tblAnnually').style.display = 'none'; 
        }
        else if (rbutton == 'M') {  
            document.getElementById('tblDaily').style.display = 'none';
            document.getElementById('tblWeekly').style.display = 'none'; 
            document.getElementById('tblMonthly').style.display = 'block'; 
            document.getElementById('tblQuarterly').style.display = 'none'; 
            document.getElementById('tblAnnually').style.display = 'none'; 
        }
        else if (rbutton == 'Q') {  
            document.getElementById('tblDaily').style.display = 'none';
            document.getElementById('tblWeekly').style.display = 'none'; 
            document.getElementById('tblMonthly').style.display = 'none'; 
            document.getElementById('tblQuarterly').style.display = 'block'; 
            document.getElementById('tblAnnually').style.display = 'none'; 
        } 
        else if (rbutton == 'A') {  
            document.getElementById('tblDaily').style.display = 'none';
            document.getElementById('tblWeekly').style.display = 'none'; 
            document.getElementById('tblMonthly').style.display = 'none'; 
            document.getElementById('tblQuarterly').style.display = 'none'; 
            document.getElementById('tblAnnually').style.display = 'block'; 
        } 
        else {  
            document.getElementById('tblDaily').style.display = 'none';
            document.getElementById('tblWeekly').style.display = 'none'; 
            document.getElementById('tblMonthly').style.display = 'none'; 
            document.getElementById('tblQuarterly').style.display = 'none'; 
            document.getElementById('tblAnnually').style.display = 'none'; 
        }  
    }  
</script>

    <br />
 <br />
    <div id="menu">
        <br />
        <br />
        <br /><asp:Button ID="btnSave" runat="server" Text="Save" Width="119px" TabIndex="20" />&nbsp;<br />
        <br />
        <br />
    </div>
    
<div id="contents">
    <table>
        <tr>
            <td>
                <asp:Image ID="imgStatus" runat="server" ImageUrl="~/Images/Symbols Tips 32x32.png" Visible="False" />
            </td>
            <td>
                <span style="font-size: 12pt">
                    <strong>Archive Maintenance</strong>
                </span>
            </td>
            <td>
                <asp:Label ID="lblDateLastUpdate" runat="server"></asp:Label></td>
        </tr>
    </table>
    <hr />
</div>
<table>
    <tr>
        <td rowspan="3" style="width: 15px">
        </td>
        <td>
                    <asp:BulletedList id="blistErrorMsg" runat="server" CssClass="error-list" Width="500px">
                    </asp:BulletedList>
        </td>
    </tr>
    <tr>
        <td style="height: 40px">
            &nbsp;<asp:CheckBox ID="chkEnable" runat="server"  Text="Enable Schedule Task" />
        </td>
    </tr>
    <tr>
        <td>
            <table id="tblSchedTaskMain" style="width:797px">
                <tr>
                    <td align="right">
                        Archive Ended Promos :
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtMonthsOld" runat="server" MaxLength="5" Width="50px"></asp:TextBox>
                        <strong>months old from current date.</strong></td>
                </tr>
                <tr>
                    <td align="right" valign="top" style="width:20%">
                        Frequency of Archiving :</td>
                    <td valign="top" style="width:10%">
                        <asp:RadioButtonList ID="radFrequency" runat="server">
                        <asp:ListItem Value="D" onclick="SetVisibility('D')" Selected="True">Daily</asp:ListItem>  
                        <asp:ListItem Value="W" onclick="SetVisibility('W')">Weekly</asp:ListItem>  
                        <asp:ListItem Value="M" onclick="SetVisibility('M')">Monthly</asp:ListItem>  
                        <asp:ListItem Value="Q" onclick="SetVisibility('Q')">Quarterly</asp:ListItem>  
                        <asp:ListItem Value="A" onclick="SetVisibility('A')">Annually</asp:ListItem>  
                        </asp:RadioButtonList>
                        </td>
                    <td valign="top" style="width:70%">
                        <table class="tableSchedTask">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="2">
                                                <strong><span style="text-decoration: underline">Schedule Task:</span></strong></td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Start Time:</td>
                                            <td align="left">
                                                <asp:TextBox ID="txtStartTime" runat="server" MaxLength="8" Width="80px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                Schedule Option :
                                            </td>
                                            <td>
                                                <table id="tblDaily">
                                                    <tr>
                                                        <td>
                                                            (Not Applicable)
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="tblWeekly">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkWeekMon" runat="server" Text="Monday" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkWeekSat" runat="server" Text="Saturday" Checked="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkWeekTue" runat="server" Text="Tuesday" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkWeekSun" runat="server" Text="Sunday" Checked="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkWeekWed" runat="server" Text="Wednesday" Checked="True" /></td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkWeekThu" runat="server" Text="Thursday" Checked="True" /></td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkWeekFri" runat="server" Text="Friday" Checked="True" /></td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="tblMonthly">
                                                    <tr>
                                                        <td colspan="2">
                                                            Desired Day
                                                             <asp:DropDownList ID="ddlMonthDay" runat="server" Width="50px">
                                                                <asp:ListItem Selected="True">1</asp:ListItem>
                                                                <asp:ListItem>2</asp:ListItem>
                                                                <asp:ListItem>3</asp:ListItem>
                                                                <asp:ListItem>4</asp:ListItem>
                                                                <asp:ListItem>5</asp:ListItem>
                                                                <asp:ListItem>6</asp:ListItem>
                                                                <asp:ListItem>7</asp:ListItem>
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>9</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                                <asp:ListItem>11</asp:ListItem>
                                                                <asp:ListItem>12</asp:ListItem>
                                                                <asp:ListItem>13</asp:ListItem>
                                                                <asp:ListItem>14</asp:ListItem>
                                                                <asp:ListItem>15</asp:ListItem>
                                                                <asp:ListItem>16</asp:ListItem>
                                                                <asp:ListItem>17</asp:ListItem>
                                                                <asp:ListItem>18</asp:ListItem>
                                                                <asp:ListItem>19</asp:ListItem>
                                                                <asp:ListItem>20</asp:ListItem>
                                                                <asp:ListItem>21</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>28</asp:ListItem>
                                                                <asp:ListItem>29</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthJan" runat="server" Text="January" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthJul" runat="server" Text="July" Checked="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthFeb" runat="server" Text="February" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthAug" runat="server" Text="August" Checked="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthMar" runat="server" Text="March" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthSep" runat="server" Text="September" Checked="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthApr" runat="server" Text="April" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthOct" runat="server" Text="October" Checked="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthMay" runat="server" Text="May" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthNov" runat="server" Text="November" Checked="True" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthJun" runat="server" Text="June" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkMonthDec" runat="server" Text="December" Checked="True" /></td>
                                                    </tr>
                                                </table>
                                                
                                                <table id="tblQuarterly">
                                                    <tr>
                                                        <td colspan="2">
                                                            Desired Day
                                                            <asp:DropDownList ID="ddlQuarterDay" runat="server" Width="50px">
                                                                <asp:ListItem Selected="True">1</asp:ListItem>
                                                                <asp:ListItem>2</asp:ListItem>
                                                                <asp:ListItem>3</asp:ListItem>
                                                                <asp:ListItem>4</asp:ListItem>
                                                                <asp:ListItem>5</asp:ListItem>
                                                                <asp:ListItem>6</asp:ListItem>
                                                                <asp:ListItem>7</asp:ListItem>
                                                                <asp:ListItem>8</asp:ListItem>
                                                                <asp:ListItem>9</asp:ListItem>
                                                                <asp:ListItem>10</asp:ListItem>
                                                                <asp:ListItem>11</asp:ListItem>
                                                                <asp:ListItem>12</asp:ListItem>
                                                                <asp:ListItem>13</asp:ListItem>
                                                                <asp:ListItem>14</asp:ListItem>
                                                                <asp:ListItem>15</asp:ListItem>
                                                                <asp:ListItem>16</asp:ListItem>
                                                                <asp:ListItem>17</asp:ListItem>
                                                                <asp:ListItem>18</asp:ListItem>
                                                                <asp:ListItem>19</asp:ListItem>
                                                                <asp:ListItem>20</asp:ListItem>
                                                                <asp:ListItem>21</asp:ListItem>
                                                                <asp:ListItem>22</asp:ListItem>
                                                                <asp:ListItem>23</asp:ListItem>
                                                                <asp:ListItem>24</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>26</asp:ListItem>
                                                                <asp:ListItem>27</asp:ListItem>
                                                                <asp:ListItem>28</asp:ListItem>
                                                                <asp:ListItem>29</asp:ListItem>
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>31</asp:ListItem>
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkQuarter1" runat="server" Text="1st Quarter" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkQuarter2" runat="server" Text="3rd Quarter" Checked="True" /></td>
                                                 </tr>
                                                 <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkQuarter3" runat="server" Text="2nd Quarter" Checked="True" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="chkQuarter4" runat="server" Text="4th Quarter" Checked="True" /></td>
                                                </tr>
                                                </table>
                                                <table id="tblAnnually">
                                                    <tr>
                                                        <td>
                                                            Desired Month
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAnnualMonth" runat="server" Width="153px">
                                                            <asp:ListItem Selected="True" Value="1">January</asp:ListItem>
                                                            <asp:ListItem Value="2">February</asp:ListItem>
                                                            <asp:ListItem Value="3">March</asp:ListItem>
                                                            <asp:ListItem Value="4">April</asp:ListItem>
                                                            <asp:ListItem Value="5">May</asp:ListItem>
                                                            <asp:ListItem Value="6">June</asp:ListItem>
                                                            <asp:ListItem Value="7">July</asp:ListItem>
                                                            <asp:ListItem Value="8">August</asp:ListItem>
                                                            <asp:ListItem Value="9">September</asp:ListItem>
                                                            <asp:ListItem Value="10">October</asp:ListItem>
                                                            <asp:ListItem Value="11">November</asp:ListItem>
                                                            <asp:ListItem Value="12">December</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Desired Day
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAnnualDay" runat="server" Width="80px">
                                                            <asp:ListItem Selected="True">1</asp:ListItem>
                                                            <asp:ListItem>2</asp:ListItem>
                                                            <asp:ListItem>3</asp:ListItem>
                                                            <asp:ListItem>4</asp:ListItem>
                                                            <asp:ListItem>5</asp:ListItem>
                                                            <asp:ListItem>6</asp:ListItem>
                                                            <asp:ListItem>7</asp:ListItem>
                                                            <asp:ListItem>8</asp:ListItem>
                                                            <asp:ListItem>9</asp:ListItem>
                                                            <asp:ListItem>10</asp:ListItem>
                                                            <asp:ListItem>11</asp:ListItem>
                                                            <asp:ListItem>12</asp:ListItem>
                                                            <asp:ListItem>13</asp:ListItem>
                                                            <asp:ListItem>14</asp:ListItem>
                                                            <asp:ListItem>15</asp:ListItem>
                                                            <asp:ListItem>16</asp:ListItem>
                                                            <asp:ListItem>17</asp:ListItem>
                                                            <asp:ListItem>18</asp:ListItem>
                                                            <asp:ListItem>19</asp:ListItem>
                                                            <asp:ListItem>20</asp:ListItem>
                                                            <asp:ListItem>21</asp:ListItem>
                                                            <asp:ListItem>22</asp:ListItem>
                                                            <asp:ListItem>23</asp:ListItem>
                                                            <asp:ListItem>24</asp:ListItem>
                                                            <asp:ListItem>25</asp:ListItem>
                                                            <asp:ListItem>26</asp:ListItem>
                                                            <asp:ListItem>27</asp:ListItem>
                                                            <asp:ListItem>28</asp:ListItem>
                                                            <asp:ListItem>29</asp:ListItem>
                                                            <asp:ListItem>30</asp:ListItem>
                                                            <asp:ListItem>31</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hfID" runat="server" />

</asp:Content>

