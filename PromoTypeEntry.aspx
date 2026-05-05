<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="PromoTypeEntry.aspx.vb" Inherits="PromoTypeEntry" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

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
        <asp:Button ID="cmdDelete" runat="server" Text="Delete" Width="100px" /><br />
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
                    Promo Category:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cboPromoCategory" runat="server"
                            DataTextField="TypeCategory" DataValueField="TypeCategory" Width="270px" AppendDataBoundItems="True" AutoPostBack="True">
                    </asp:DropDownList> 
                    <asp:Button ID="btnAddTypeCategory" runat="server" Text="+" Width="23px" />                    
                    <asp:TextBox ID="txtTypeCategory" runat="server" MaxLength="50" Width="181px"></asp:TextBox>
                    <asp:Button ID="btnCancelAddTypeCat" runat="server" Text="Cancel" Width="52px" /></td>
            </tr>
            <tr style="font-size: 12pt">
                <td style="width: 112px; height: 8px">
                    <span style="font-size: 10pt">Promo Type:</span></td>
                <td class="field-cell" style="height: 8px; width: 560px;">
                    <asp:DropDownList ID="cboPromoSubCategory" runat="server"
                            DataTextField="TypeSubCategory" DataValueField="TypeSubCategory" Width="270px" AppendDataBoundItems="True" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Button ID="btnAddTypeSubCategory" runat="server" Text="+" Width="23px" />
                    <asp:TextBox ID="txtTypeSubCategory" runat="server" MaxLength="50" Width="181px"></asp:TextBox>
                    <asp:Button ID="btnCancelAddTypeSub" runat="server" Text="Cancel" Width="52px" /> 
                </td>
            </tr>
            <tr>
                <td style="width: 135px;">
                    Sub Type:</td>
                <td style="width: 560px; height: 3px" class="field-cell">
                    <asp:TextBox ID="txtPromoType" runat="server" MaxLength="30" Width="463px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="middle">
                    Short Name:</td>
                <td style="width: 560px; height: 6px" class="field-cell">
                    <asp:TextBox ID="txtShortDesc" runat="server" MaxLength="10" Width="181px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 135px; height: 30px;">
                    &nbsp;</td>
                <td style="width: 560px; height: 30px" class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Default Mechanics:</td>
                <td class="field-cell" style="width: 560px">
                    <div  style="width: 543px; height: 112px; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid;"><asp:Literal ID="litDefMechanics" runat="server"></asp:Literal></div>
                    <asp:LinkButton ID="lnkEditMechanics" runat="server" CssClass="action-link" Width="80px">Edit Mechanics</asp:LinkButton>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    &nbsp;</td>
                <td style="width: 560px; height: 2px" class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Default Guidelines:</td>
                <td class="field-cell" style="width: 560px">
                    <div  style="width: 543px; height: 112px; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid;"><asp:Literal ID="litDefGuidelines" runat="server"></asp:Literal></div>
                    <asp:LinkButton ID="lnkEditGuidelines" runat="server" CssClass="action-link" Width="80px">Edit Guidelines</asp:LinkButton>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Group Type:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cmbGroupType" runat="server" Width="231px" AutoPostBack="True">
                        <asp:ListItem Value="-1">[Select Group Type]</asp:ListItem>
                        <asp:ListItem>REGULAR</asp:ListItem>
                        <asp:ListItem>CM</asp:ListItem>
                        <asp:ListItem>BCR</asp:ListItem>
                        <asp:ListItem>SACI</asp:ListItem>
                        <asp:ListItem>SBU</asp:ListItem>
                    </asp:DropDownList><asp:CheckBox ID="chkIsExclusive" runat="server" Text="Is Exclusive" />&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    &nbsp;</td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkMpdUseOnly" runat="server" Width="326px" Text="For MPD Use Only" /></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkAllow" runat="server" Width="326px" Text="Allow Attachment" /></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkRequired" runat="server" Width="326px" Text="Required Attachment" /></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkIsClassDiscount" runat="server" Width="326px" Text="Class Discount Promotion" /></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkForSMACdeals" runat="server" Width="326px" Text="Available in SMAC Deals" /></td>
            </tr>
             <tr>
                <td style="width: 135px;" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                    <asp:CheckBox ID="chkUPClevel" runat="server" Width="326px" Text="UPC Level Promotion" /></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;</td>
                <td class="field-cell" style="width: 560px" >
                    &nbsp;</td>
            </tr>
            <tr id="trVSProcessType" runat="server">
                <td style="width: 135px" valign="top">VLSP Interface Type:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:DropDownList ID="cboVSProcessType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="181px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;</td>
                <td class="field-cell" style="width: 560px" >
                    &nbsp;</td>
            </tr>
            
            <tr>
                <td style="width: 135px" valign="top">
                    Priority:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:DropDownList ID="cboXML_Priority" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="181px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="cboXML_Priority_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Compress ID:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_CompressID" runat="server" MaxLength="25" Width="50px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_CompressID_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">Active Days:</td>
                <td class="field-cell" style="width: 560px" >
                    <table><tr><td>
                            <asp:CheckBox ID="chkMonday" runat="server" Text="Monday" /><br/>
                            <asp:CheckBox ID="chkTuesday" runat="server" Text="Tuesday" /><br/>
                            <asp:CheckBox ID="chkWednesday" runat="server" Text="Wednesday" /><br/>
                            <asp:CheckBox ID="chkThursday" runat="server" Text="Thursday" /><br/>
                            <asp:CheckBox ID="chkFriday" runat="server" Text="Friday" /><br/>
                            <asp:CheckBox ID="chkSaturday" runat="server" Text="Saturday" /><br/>
                            <asp:CheckBox ID="chkSunday" runat="server" Text="Sunday" />
                        </td><td>
                            <asp:DropDownList ID="cboXML_ActiveDays_State" runat="server" Width="127px" DataTextField="ElementName" DataValueField="ElementValue">
                            </asp:DropDownList></td>
                    </tr></table>
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Activation Time:</td>
                <td class="field-cell" style="width: 560px" >
<%--                    <asp:TextBox ID="txtXML_AcTime" runat="server" MaxLength="25" Width="80px"></asp:TextBox> to
                    <asp:TextBox ID="txtXML_Deactime" runat="server" MaxLength="25" Width="80px"></asp:TextBox>--%>
                    <asp:DropDownList ID="cboXML_StartTime" runat="server" Width="90px">
                    </asp:DropDownList>
                    to &nbsp;<asp:DropDownList ID="cboXML_EndTime" runat="server" Width="90px">
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboXML_ActDeacTime_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;</td>
                <td class="field-cell" style="width: 560px" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Description:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_Descr1prm" runat="server" MaxLength="25" Width="256px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_Descr1prm_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Description 2:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_Descr2prm" runat="server" MaxLength="25" Width="256px"></asp:TextBox>&nbsp;<asp:DropDownList ID="cboXML_Descr2prm_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Receipt Description:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:TextBox ID="txtXML_ReceiptDesc1" runat="server" MaxLength="25" Width="256px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_ReceiptDesc1_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Receipt Description 2:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:TextBox ID="txtXML_ReceiptDesc2" runat="server" MaxLength="25" Width="256px"></asp:TextBox>&nbsp;<asp:DropDownList ID="cboXML_ReceiptDesc2_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;</td>
                <td class="field-cell" style="width: 560px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Action Type:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cboXML_ActionType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="262px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>&nbsp;<asp:DropDownList ID="cboXML_ActionType_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px; height: 29px;" valign="top">
                    Exclusion Type:</td>
                <td class="field-cell" style="height: 29px; width: 560px;">
                    <asp:DropDownList ID="cboXML_ExclusionType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="330px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>&nbsp;<asp:DropDownList ID="cboXML_ExclusionType_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;</td>
                <td class="field-cell" style="width: 560px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Maximum Amount:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_MaxAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                        onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                        Width="130px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_MaxAmount_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Maximum Number:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:TextBox ID="txtXML_MaxQty" runat="server" CssClass="RightAligned" MaxLength="15"
                        onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                        Width="130px"></asp:TextBox>&nbsp;<asp:DropDownList ID="cboXML_MaxQty_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                        </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    &nbsp;</td>
                <td class="field-cell" style="width: 560px" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Process Type:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:DropDownList ID="cboXML_ProcessType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="330px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>&nbsp;<asp:DropDownList ID="cboXML_ProcessType_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Discount Type:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:DropDownList ID="cboXML_DiscountType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="330px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>&nbsp;<asp:DropDownList ID="cboXML_DiscountType_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                </td>
                <td class="field-cell" style="width: 560px" >
                </td>
            </tr>
            <tr>
                <td style="width: 135px; height: 29px;" valign="top">
                    Qualified Items:</td>
                <td class="field-cell" style="height: 29px; width: 560px;">
                    <asp:DropDownList ID="cboXML_QualifiedItems" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="330px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboXML_QualifiedItems_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Shouldering Entity:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cboXML_Sponsorship" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="330px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboXML_Sponsorship_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Message ID:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_MessageID" runat="server" CssClass="RightAligned" MaxLength="15"
                        Width="44px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_MessageID_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    SMAC Kit Price</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_SMACKitPrice" runat="server" CssClass="RightAligned" MaxLength="15"
                        onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                        Width="130px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_SMACKitPrice_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Qualified Customers:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_QualifiedCust" runat="server" MaxLength="50" Width="256px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_QualifiedCust_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Name of Partners:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_NameOfPartners" runat="server" MaxLength="50" Width="256px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_NameOfPartners_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Proof of Membership:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_ProofOfMemb" runat="server" MaxLength="50" Width="256px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_ProofOfMemb_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Name of Partner Establishment/s (GWP):</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtXML_PartnerEstablishmentGWP" runat="server" MaxLength="50" Width="256px"></asp:TextBox>
                    <asp:DropDownList ID="cboXML_PartnerEstablishmentGWP_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Included Item:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:DropDownList ID="cboXLM_UPCPromoPremium" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                </td>
                <td class="field-cell" style="width: 560px">
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Item Elements:</td>
                <td class="field-cell" style="width: 560px">
                    <table>
                        <tr><td><asp:CheckBox ID="chkXML_DeptLevel" runat="server" Text="Department Codes" /></td></tr>
                        <tr><td><asp:CheckBox ID="chkXML_BarcodeLevel" runat="server" Text="Barcodes" /></td></tr>
                        <tr><td><asp:CheckBox ID="chkXML_CouponLevel" runat="server" Text="Coupons" /></td>
                        <td>Maximum Coupons:<asp:TextBox ID="txtXML_MaxCoupons" runat="server" CssClass="RightAligned" MaxLength="15"
                        Width="44px" onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"></asp:TextBox></td></tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    No. Of Months:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_NumMonths" runat="server" CssClass="RightAligned" MaxLength="3"
                        Width="44px" onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_NumMonths_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Participating Banks:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_BankList" runat="server" CssClass="RightAligned" MaxLength="200"
                        Width="256px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_BankList_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px; height: 43px;" valign="top">
                    Name of Brand/Product:</td>
                <td class="field-cell" style="height: 43px; width: 560px;" >
                    <asp:TextBox ID="txtTPL_BrandNames" runat="server" CssClass="RightAligned" MaxLength="100"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_BrandNames_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>     
            <tr>
                <td style="width: 135px; height: 43px;" valign="top">
                    Promotion Application:</td>
                <td class="field-cell" style="height: 43px; width: 560px;" >
                    <asp:DropDownList ID="cboTPL_ProcType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="330px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboTPL_ProcType_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Purchase Requirement:</td>
                <td class="field-cell" style="height: 43px; width: 560px;" >
                    <asp:DropDownList ID="cboTPL_PurchReq" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="330px">
                        <asp:ListItem>- - Select Value - -</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="cboTPL_PurchReq_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Required Amount:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_RequiredAmt" runat="server" CssClass="RightAligned" MaxLength="15"
                        onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                        Width="130px"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_RequiredAmt_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr> 
            <tr>
                <td style="width: 135px;" valign="top">
                    Free Item/s:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:TextBox ID="txtTPL_FreeItems" runat="server" CssClass="RightAligned" MaxLength="100"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_FreeItems_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr> 
            <tr>
                <td style="width: 135px;" valign="top">
                    Prize/s:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:TextBox ID="txtTPL_Prizes" runat="server" CssClass="RightAligned" MaxLength="100"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_Prizes_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Department Name:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:TextBox ID="txtTPL_DepName" runat="server" CssClass="RightAligned" MaxLength="50"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_DepName_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr> 
            <tr>
                <td style="width: 135px;" valign="top">
                    Area/Location:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:TextBox ID="txtTPL_SellingArea" runat="server" CssClass="RightAligned" MaxLength="50"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_SellingArea_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Item Name:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_ItemName" runat="server" CssClass="RightAligned" MaxLength="50"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_ItemName_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Bonus Points:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_BonusPoints" runat="server" CssClass="RightAligned" MaxLength="20"
                        onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                        Width="130px"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_BonusPoints_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px; height: 26px;" valign="top">
                    Celebrity/Mascot:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_CelebName" runat="server" CssClass="RightAligned" MaxLength="100"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_CelebName_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>    
            <tr>
                <td style="width: 135px;" valign="top">
                    Event Time:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_EventTime" runat="server" CssClass="RightAligned" MaxLength="50"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_EventTime_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Buy Qty:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_BuyQty" runat="server" CssClass="RightAligned" MaxLength="10"
                        onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                        Width="130px"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_BuyQty_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px" valign="top">
                    Take Qty:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_TakeQty" runat="server" CssClass="RightAligned" MaxLength="10"
                        onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                        Width="130px"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_TakeQty_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr> 
            <tr>
                <td style="width: 135px;" valign="top">
                    Activity Name:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtTPL_ActivityName" runat="server" CssClass="RightAligned" MaxLength="100"
                        Width="350px" Wrap="False"></asp:TextBox>
                    <asp:DropDownList ID="cboTPL_ActivityName_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Promo Notes:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:DropDownList ID="cboTPL_PromoNotes_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 135px;" valign="top">
                    Discount Amount:</td>
                <td class="field-cell" style="width: 560px">
                    <asp:DropDownList ID="cboTPL_DiscountAmt_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue" AutoPostBack="True" Visible="False" >
                    </asp:DropDownList></td>
            </tr>  
            <tr>
                <td style="width: 135px; " valign="top">
                    Percent Discount:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:DropDownList ID="cboTPL_PercentDisc_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue" AutoPostBack="True" Visible="False" >
                    </asp:DropDownList></td>
            </tr>   
                         
           <tr id="trXML_EligibleCards" runat="server">
                <td style="width: 173px; height: 22px; text-align: left;" valign="middle">
                    Eligible Cards:
                </td>
                <td style="height: 22px; text-align: left; width: 560px;">
                     <asp:DropDownList ID="cboXML_EligibleCards_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue" AutoPostBack="True">
                       </asp:DropDownList><asp:CheckBox ID="chk_IsEligibleRequired" runat="server" Text="Is Required" />
                    <table style="width: 95%; height: 20px; text-align: left;" valign="top">
                        <tr>
                            <td><asp:CheckBox ID="chkXML_eCardAll" runat="server" Text="Select All" AutoPostBack="True" /></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkXML_eCard1" runat="server" Text="SM Prestige Card" /></td>
                            <td><asp:CheckBox ID="chkXML_eCard2" runat="server" Text="BDO Rewards" /></td>
                        </tr>
                        <tr id="trSMACextra" runat="server" visible="false">
                            <td>
                                <asp:CheckBox ID="chkXML_eCard10" runat="server" Text="China Bank Card" /></td>
                            <td>
                                <asp:CheckBox ID="chkXML_eCard11" runat="server" Text="BDO Master Card" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkXML_eCard3" runat="server" Text="MOM Card" /></td>
                            <td><asp:CheckBox ID="chkXML_eCard4" runat="server" Text="Primo Card" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkXML_eCard5" runat="server" Text="SM Advantage Card" /></td>
                            <td><asp:CheckBox ID="chkXML_eCard6" runat="server" Text="TK Amazing Card" /></td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkXML_eCard7" runat="server" Text="Supplies Station Card" /></td>
                            <td><asp:CheckBox ID="chkXML_eCard8" runat="server" Text="Love Your Body Card" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkXML_eCard9" runat="server" Text="ACE Rewards Card" /></td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 135px; height: 139px;" valign="top">PromoID Range:</td>
                <td class="field-cell" style="height: 139px; width: 560px;" >
                    <asp:TextBox ID="txtPromoIDfrom" runat="server" MaxLength="25" Width="50px"></asp:TextBox>&nbsp;to&nbsp;
                    <asp:TextBox ID="txtPromoIDto" runat="server" MaxLength="25" Width="50px"></asp:TextBox>&nbsp;
                    <asp:DropDownList ID="cboPromoIDCompSponsorship" runat="server" Width="139px" DataTextField="ElementName" DataValueField="ElementValue" AppendDataBoundItems="True">
                    </asp:DropDownList>
                    <asp:Button ID="btnAddPromoRange" runat="server" Text="+" Width="23px" /> 
                    <br />
                    <br />
                    <asp:GridView ID="gvPromoIDRange" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Size="8pt" Width="454px">
                                <EmptyDataTemplate>
                                <table width="100%">
                                  <tr>
                                 <td>
                                        </td>
                                        <td>PromoID From
                                        </td>
                                        <td>PromoID To
                                        </td>
                                        <td>CompSponsorship
                                        </td>
                                    </tr>
                                </table>
  
                                   
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkDelPromoRange" runat="server" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDelPromoRange" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="POS_PromoIDfrom" HeaderText="PromoID From" >
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="POS_PromoIDto" HeaderText="PromoID To" >
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ElementValue" HeaderText="ElementValue" >
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ElementName" HeaderText="CompSponsorship"  >
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EmptyDataTemplate>
                                    &nbsp;
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <table>
                                <tr>
                                    <td style="height: 21px; width: 113px;">
                                        <asp:LinkButton ID="lnkDeletePromoRange" runat="server" CssClass="action-link" Font-Size="Small"
                                            Width="100px">Delete Selected</asp:LinkButton></td>
                                    <td style="height: 21px; width: 7px;">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="height: 21px; width: 113px;">
                                        <asp:BulletedList ID="blistErrPromoRange" runat="server" CssClass="error-list" Width="377px">
                                            </asp:BulletedList>
                                    </td>
                                </tr>
                            </table>                            

                        </td>  
                        <td>
                            &nbsp;</td>                   
                        
                            <td style="height: 139px">                            
                            </td>
                            
                </tr>
             <tr>
                <td style="width: 135px;" valign="top">
                    Process Type:</td>
                <td class="field-cell" style="width: 560px" >
                    <asp:TextBox ID="txtProcessType" runat="server" CssClass="RightAligned" MaxLength="20"
                        Width="350px" Wrap="False"></asp:TextBox></td>
            </tr>
            <tr id="trConditions" runat="server">
                <td style="width: 135px;" valign="top">
                    Conditions:</td>
                <td class="field-cell" style="width: 560px">
                    <table>
                        <tr>
                            <td>
                                Level</td>
                            <td>Condition</td>
                            <td>
                                State</td>
                       </tr>
                        <tr>
                           <td><asp:TextBox ID="txtXML_CondSeqNo" runat="server" CssClass="RightAligned" MaxLength="3" Width="25px" Enabled="False"></asp:TextBox></td>
                           <td>
                                <asp:DropDownList ID="cboXML_DiscCondition" runat="server" AppendDataBoundItems="True"
                                    DataTextField="ElementName" DataValueField="ElementValue" Width="250px">
                                    <asp:ListItem>- - Select Value - -</asp:ListItem>
                                </asp:DropDownList></td>
                            <td><asp:DropDownList ID="cboXML_DiscCondition_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                            </asp:DropDownList></td>
                        </tr>
                        </table>
                        <table>
                            <tr>
                                <td style="width: 32px">&nbsp;</td>
                                <td>
                                    Value</td>
                                <td>
                                    Cond. State</td>
                                <td>&nbsp;</td>
                           </tr>
                            <tr>
                                <td style="width: 32px">&nbsp;</td>
                                <td>
                                <asp:TextBox ID="txtXML_DiscAmount" runat="server" CssClass="RightAligned" MaxLength="20" Width="50px"></asp:TextBox></td>
                                <td><asp:DropDownList ID="cboXML_Condition_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue">
                                </asp:DropDownList></td>
                                <td>
                                <asp:Button ID="cmdAddCondition" runat="server" Text="Add" Width="50px" /></td>
                            </tr>
                        </table>
                                <asp:GridView ID="gridConditions" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
                                    PageSize="5" Width="98%">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkALL_Condition" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_Condition_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRowSelCondition" runat="server" EnableTheming="True" OnCheckedChanged="chkRowSelCondition_CheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SequenceNo" HeaderText="Level">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DiscAmnt1" HeaderText="Disc Value" DataFormatString="{0:F2}">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DiscCond_STATE" HeaderText="Disc. Condition State">
                                            <ItemStyle Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ElementName" HeaderText="Condition">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Condition_STATE" HeaderText="Value State">
                                            <ItemStyle Width="70px" />
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
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lnkDeleteSelectCondition" runat="server" CssClass="action-link" Font-Size="Small"
                                                Width="100px">Delete Selected</asp:LinkButton></td>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                </td>
            </tr>
            <tr id="trRules" runat="server">
                <td style="width: 135px" valign="top">
                    Rules:</td>
                <td class="field-cell" style="width: 560px"><table>
                    <tr>
                        <td>
                            Level</td>
                        <td>Type</td>
                        <td>Value Type</td>
                        <td>Condition</td>
                        <td>
                            Value</td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="txtXML_RuleSeqNo" runat="server" CssClass="RightAligned" MaxLength="3" Width="25px" Enabled="False"></asp:TextBox></td>
                        <td><asp:DropDownList ID="cboXML_RuleType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="170px">
                            <asp:ListItem>- - Select Value - -</asp:ListItem>
                        </asp:DropDownList></td>
                        <td><asp:DropDownList ID="cboXML_RuleValueType" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="90px">
                            <asp:ListItem>- - Select Value - -</asp:ListItem>
                        </asp:DropDownList></td>
                        <td>
                            <asp:DropDownList ID="cboXML_RuleCondition" runat="server" AppendDataBoundItems="True"
                        DataTextField="ElementName" DataValueField="ElementValue" Width="130px">
                                <asp:ListItem>- - Select Value - -</asp:ListItem>
                            </asp:DropDownList></td>
                        <td>
                            <asp:TextBox ID="txtXML_RuleValue" runat="server" CssClass="RightAligned" MaxLength="20"
                                Width="50px"></asp:TextBox></td>
                   </tr>
                </table>
                <table>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="width: 103px">State</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="width: 103px"><asp:DropDownList ID="cboXML_Rule_State" runat="server" Width="100px" DataTextField="ElementName" DataValueField="ElementValue"></asp:DropDownList></td>
                        <td><asp:Button ID="cmdAddPosRules" runat="server" Text="Add" Width="50px" /></td>
                    </tr>
                </table>
                    <asp:GridView ID="gridPosRules" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    CellPadding="4" Font-Bold="True" Font-Size="8pt" ForeColor="#333333" GridLines="None"
                                    PageSize="5" Width="98%">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL_PosRules" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_PosRules_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSelPosRules" runat="server" EnableTheming="True" OnCheckedChanged="chkRowSelPosRules_CheckedChanged" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="SequenceNo" HeaderText="Level">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RuleTypeDesc" HeaderText="Type">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RuleValueTypeDesc" HeaderText="Value Type">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RuleConditionDesc" HeaderText="Condition">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RuleValue" HeaderText="Value" DataFormatString="{0:F2}">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            No rule defined for this promotion type.
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    <table>
                        <tr>
                            <td style="height: 21px">
                                <asp:LinkButton ID="lnkDeleteSelectRule" runat="server" CssClass="action-link" Font-Size="Small"
                                    Width="100px">Delete Selected</asp:LinkButton></td>
                            <td style="height: 21px">
                            </td>
                            <td style="height: 21px">
                                &nbsp;</td>
                        </tr>
                    </table>
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
    <div style="display:none">
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <asp:Button ID="Button2" runat="server" Text="Button" />
        <asp:Button ID="cmdPopUpOK" runat="server" Text="" />
    </div>
</asp:Content>

