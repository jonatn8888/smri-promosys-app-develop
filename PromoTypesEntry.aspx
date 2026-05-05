<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    ValidateRequest="false" CodeFile="PromoTypesEntry.aspx.vb" Inherits="PromoTypesEntry"
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="./rte/richtext.js" type="text/javascript" language="javascript"></script>

    <script src="./rte/config.js" type="text/javascript" language="javascript"></script>

    <script language="javascript" type="text/javascript">

function confirm_delete()
{
    return confirm("Delete this record?");
}

function opentexteditor()
{
    var url
    url = "texteditor.aspx";
	texteditorwindow=dhtmlmodal.open('texteditor', 'iframe', url, 'Default Mechanics', 'width=709px,height=500px,center=1,resize=0,scrolling=0',"recall")
    texteditorwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    var Mechanics = this.contentDoc.getElementById("HiddenField1") 
    document.getElementById("<%=hidBox.ClientID%>").value = Mechanics.value;
    document.getElementById("<%=Button2.ClientID%>").click(); 
	return true 
	}
}

function ShowMsgBox(height,width)
{
    if (height == "") height= "163";
    if (width == "") width= "510";
    
    var url
    var title
    url = "MsgBox.aspx";
    title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
    MsgBoxwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    MsgBoxwindow.onclose=function()
        { 
            var theform = this.contentDoc.forms[0] 
            document.getElementById("<%=cmdPopUpOK.ClientID%>").click(); 
	        return true 
	    }
}

function chkMpdApprovalClick(checkbox) {
    if (checkbox.checked) {
        var trNumLeadDaysApprovalMPD = document.getElementById('trNumLeadDaysApprovalMPD');
        var trApprovalCutoffMPD = document.getElementById('trApprovalCutoffMPD');
        trNumLeadDaysApprovalMPD.style.display = "block";
        trApprovalCutoffMPD.style.display = "block";
        var txtNumLeadDaysApprovalMPD = document.getElementById('<%=txtNumLeadDaysApprovalMPD.ClientID%>');
        txtNumLeadDaysApprovalMPD.value = "1";
    } else {
        var trNumLeadDaysApprovalMPD = document.getElementById('trNumLeadDaysApprovalMPD');
        var trApprovalCutoffMPD = document.getElementById('trApprovalCutoffMPD');
        trNumLeadDaysApprovalMPD.style.display = "none";
        trApprovalCutoffMPD.style.display = "none";
        var txtNumLeadDaysApprovalMPD = document.getElementById('<%=txtNumLeadDaysApprovalMPD.ClientID%>');
        txtNumLeadDaysApprovalMPD.value = "0";
        var cboApprovalCutoffMPD = document.getElementById('<%=cboApprovalCutoffMPD.ClientID%>');
        cboApprovalCutoffMPD.value = "00:00";
    }
}
    </script>

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:Button ID="cmdSave" runat="server" Text="Save" Width="100px" /><br />
        <br />
        <asp:Button ID="cmdCancel" runat="server" Text="Cancel" Width="100px" /><br />
        <br />
        <br />
        <br />
    </div>
    <div id="contents">
        <strong><span style="font-size: 12pt; font-family: Trebuchet MS">Promotion Type Maintenance</span></strong><br />
        <hr style="width: 700px" />
        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="630px">
        </asp:BulletedList>
        <asp:HiddenField ID="hidBox" runat="server" />
        <br />
        <table cellpadding="3" cellspacing="0" id="doc-table">
            <tr>
                <td style="width: 112px">
                    Promo Category:
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cboPromoCategory" runat="server" DataTextField="TypeCategory"
                        DataValueField="TypeCategory" Width="270px" AppendDataBoundItems="True" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Button ID="btnAddTypeCategory" runat="server" Text="+" Width="23px" />
                    <asp:TextBox ID="txtTypeCategory" runat="server" MaxLength="50" Width="181px"></asp:TextBox>
                    <asp:Button ID="btnCancelAddTypeCat" runat="server" Text="Cancel" Width="52px" />
                </td>
            </tr>
            <tr style="font-size: 12pt">
                <td style="width: 112px; height: 8px">
                    <span style="font-size: 10pt">Promo Type:</span>
                </td>
                <td class="field-cell" style="height: 8px; width: 560px;">
                    <asp:DropDownList ID="cboPromoSubCategory" runat="server" DataTextField="TypeSubCategory"
                        DataValueField="TypeSubCategory" Width="270px" AppendDataBoundItems="True" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Button ID="btnAddTypeSubCategory" runat="server" Text="+" Width="23px" />
                    <asp:TextBox ID="txtTypeSubCategory" runat="server" MaxLength="50" Width="181px"></asp:TextBox>
                    <asp:Button ID="btnCancelAddTypeSub" runat="server" Text="Cancel" Width="52px" />
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                    Sub Type:
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:TextBox ID="txtPromoType" runat="server" MaxLength="30" Width="463px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="middle">
                    Short Name:
                </td>
                <td style="width: 560px; height: 6px" class="field-cell">
                    <asp:TextBox ID="txtShortDesc" runat="server" MaxLength="10" Width="181px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 135px; height: 30px;">
                    &nbsp;
                </td>
                <td style="width: 560px; height: 30px" class="field-cell">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Group Type:
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cmbGroupType" runat="server" Width="231px" AutoPostBack="True">
                        <asp:ListItem Value="-1">[Select Group Type]</asp:ListItem>
                        <asp:ListItem>ALL</asp:ListItem>
                        <asp:ListItem>REGULAR</asp:ListItem>
                        <%--<asp:ListItem>CM</asp:ListItem>--%>
                        <%--<asp:ListItem>BCR</asp:ListItem>
                        <asp:ListItem>SACI</asp:ListItem>--%>
                        <asp:ListItem>SBU</asp:ListItem>
                    </asp:DropDownList>
                    <%--<asp:CheckBox ID="chkIsExclusive" runat="server" Text="Is Exclusive" />&nbsp;--%>
                </td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    &nbsp;
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkMpdUseOnly" runat="server" Width="326px" Text="Is Deactivated?" />
                </td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkAllow" runat="server" Width="326px" Text="Allow Attachment" />
                </td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkRequired" runat="server" Width="326px" Text="Required Attachment" />
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;
                </td>
                <td class="field-cell" style="width: 560px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Priority(MBU):
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cboXML_Priority" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="181px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <%--<asp:DropDownList ID="cboXML_Priority_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue"></asp:DropDownList>--%>
                </td>
            </tr>
                        <tr>
                <td style="width: 135px" valign="top">
                    Priority(SBU):
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cboXML_Priority_SBU" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="181px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                    <%--<asp:DropDownList ID="cboXML_Priority_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue"></asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;
                </td>
                <td class="field-cell" style="width: 560px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                </td>
            </tr>
            <tr id="trProcessTypeMBU" runat="server">
                <td style="width: 135px;" valign="top">
                    Process Type (MBU)
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:GridView ID="gridProcessTypeMBU" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
                        PageSize="5" Width="98%">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:BoundField DataField="ElementName" HeaderText="Promo Application">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" Width="50%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ElementValue" HeaderText="Behavior">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No condition defined for this promotion type.
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="trProcessTypeSBU" runat="server">
                <td style="width: 135px;" valign="top">
                    Process Type (SBU)
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:GridView ID="gridProcessTypeSBU" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
                        PageSize="5" Width="98%">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:BoundField DataField="ElementName" HeaderText="Promo Application">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" Width="50%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ElementValue" HeaderText="Behavior">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No condition defined for this promotion type.
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                   <b> Requestor </b>
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                    Cut Off Day(s):
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:TextBox ID="txtNumLeadDaysRequestor" runat="server" MaxLength="3" Width="55px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                    Cut Off Time:
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:DropDownList ID="cboRequestorCutoff" AutoPostBack="False" runat="server">
                        <asp:ListItem Value="00:00">00:00</asp:ListItem>
                        <asp:ListItem Value="01:00">01:00</asp:ListItem>
                        <asp:ListItem Value="02:00">02:00</asp:ListItem>
                        <asp:ListItem Value="03:00">03:00</asp:ListItem>
                        <asp:ListItem Value="04:00">04:00</asp:ListItem>
                        <asp:ListItem Value="05:00">05:00</asp:ListItem>
                        <asp:ListItem Value="06:00">06:00</asp:ListItem>
                        <asp:ListItem Value="07:00">07:00</asp:ListItem>
                        <asp:ListItem Value="08:00">08:00</asp:ListItem>
                        <asp:ListItem Value="09:00">09:00</asp:ListItem>
                        <asp:ListItem Value="10:00">10:00</asp:ListItem>
                        <asp:ListItem Value="11:00">11:00</asp:ListItem>
                        <asp:ListItem Value="12:00">12:00</asp:ListItem>
                        <asp:ListItem Value="13:00">13:00</asp:ListItem>
                        <asp:ListItem Value="14:00">14:00</asp:ListItem>
                        <asp:ListItem Value="15:00">15:00</asp:ListItem>
                        <asp:ListItem Value="16:00">16:00</asp:ListItem>
                        <asp:ListItem Value="17:00">17:00</asp:ListItem>
                        <asp:ListItem Value="18:00">18:00</asp:ListItem>
                        <asp:ListItem Value="19:00">19:00</asp:ListItem>
                        <asp:ListItem Value="20:00">20:00</asp:ListItem>
                        <asp:ListItem Value="21:00">21:00</asp:ListItem>
                        <asp:ListItem Value="22:00">22:00</asp:ListItem>
                        <asp:ListItem Value="23:00">23:00</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                    <b>Approver</b>
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                    Cut Off Day(s):
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:TextBox ID="txtNumLeadDaysApprovalBU" runat="server" MaxLength="3" Width="55px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                    Cut Off Time:
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:DropDownList ID="cboApprovalCutoffBU" AutoPostBack="False" runat="server">
                        <asp:ListItem Value="00:00">00:00</asp:ListItem>
                        <asp:ListItem Value="01:00">01:00</asp:ListItem>
                        <asp:ListItem Value="02:00">02:00</asp:ListItem>
                        <asp:ListItem Value="03:00">03:00</asp:ListItem>
                        <asp:ListItem Value="04:00">04:00</asp:ListItem>
                        <asp:ListItem Value="05:00">05:00</asp:ListItem>
                        <asp:ListItem Value="06:00">06:00</asp:ListItem>
                        <asp:ListItem Value="07:00">07:00</asp:ListItem>
                        <asp:ListItem Value="08:00">08:00</asp:ListItem>
                        <asp:ListItem Value="09:00">09:00</asp:ListItem>
                        <asp:ListItem Value="10:00">10:00</asp:ListItem>
                        <asp:ListItem Value="11:00">11:00</asp:ListItem>
                        <asp:ListItem Value="12:00">12:00</asp:ListItem>
                        <asp:ListItem Value="13:00">13:00</asp:ListItem>
                        <asp:ListItem Value="14:00">14:00</asp:ListItem>
                        <asp:ListItem Value="15:00">15:00</asp:ListItem>
                        <asp:ListItem Value="16:00">16:00</asp:ListItem>
                        <asp:ListItem Value="17:00">17:00</asp:ListItem>
                        <asp:ListItem Value="18:00">18:00</asp:ListItem>
                        <asp:ListItem Value="19:00">19:00</asp:ListItem>
                        <asp:ListItem Value="20:00">20:00</asp:ListItem>
                        <asp:ListItem Value="21:00">21:00</asp:ListItem>
                        <asp:ListItem Value="22:00">22:00</asp:ListItem>
                        <asp:ListItem Value="23:00">23:00</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    With MPD Approval:
                </td>
                <td class="field-cell" style="width: 560px">
                    <input type="checkbox" id="chkMpdApproval" onclick="chkMpdApprovalClick(this);" />
                </td>
            </tr>
            <tr id="trNumLeadDaysApprovalMPD" style="display: none;">
                <td style="width: 135px;">
                    &emsp;Cut Off Day(s):
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:TextBox ID="txtNumLeadDaysApprovalMPD" runat="server" MaxLength="3" Width="55px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trApprovalCutoffMPD" style="display: none;">
                <td style="width: 135px;">
                    &emsp;Cut Off Time:
                </td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:DropDownList ID="cboApprovalCutoffMPD" AutoPostBack="False" runat="server">
                        <asp:ListItem Value="00:00">00:00</asp:ListItem>
                        <asp:ListItem Value="01:00">01:00</asp:ListItem>
                        <asp:ListItem Value="02:00">02:00</asp:ListItem>
                        <asp:ListItem Value="03:00">03:00</asp:ListItem>
                        <asp:ListItem Value="04:00">04:00</asp:ListItem>
                        <asp:ListItem Value="05:00">05:00</asp:ListItem>
                        <asp:ListItem Value="06:00">06:00</asp:ListItem>
                        <asp:ListItem Value="07:00">07:00</asp:ListItem>
                        <asp:ListItem Value="08:00">08:00</asp:ListItem>
                        <asp:ListItem Value="09:00">09:00</asp:ListItem>
                        <asp:ListItem Value="10:00">10:00</asp:ListItem>
                        <asp:ListItem Value="11:00">11:00</asp:ListItem>
                        <asp:ListItem Value="12:00">12:00</asp:ListItem>
                        <asp:ListItem Value="13:00">13:00</asp:ListItem>
                        <asp:ListItem Value="14:00">14:00</asp:ListItem>
                        <asp:ListItem Value="15:00">15:00</asp:ListItem>
                        <asp:ListItem Value="16:00">16:00</asp:ListItem>
                        <asp:ListItem Value="17:00">17:00</asp:ListItem>
                        <asp:ListItem Value="18:00">18:00</asp:ListItem>
                        <asp:ListItem Value="19:00">19:00</asp:ListItem>
                        <asp:ListItem Value="20:00">20:00</asp:ListItem>
                        <asp:ListItem Value="21:00">21:00</asp:ListItem>
                        <asp:ListItem Value="22:00">22:00</asp:ListItem>
                        <asp:ListItem Value="23:00">23:00</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none">
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <asp:Button ID="Button2" runat="server" Text="Button" />
        <asp:Button ID="cmdPopUpOK" runat="server" Text="" />
    </div>

    <script type="text/javascript" language="javascript">
    var txtNumLeadDaysApprovalMPD = document.getElementById('<%=txtNumLeadDaysApprovalMPD.ClientID%>');
    var cboApprovalCutoffMPD = document.getElementById('<%=cboApprovalCutoffMPD.ClientID%>');
    if(txtNumLeadDaysApprovalMPD.value != "" && txtNumLeadDaysApprovalMPD.value != "0")
    {
        var chk1 = document.getElementById('chkMpdApproval');
        chk1.checked = true;
        chkMpdApprovalClick(chk1);
    }
    </script>

</asp:Content>
