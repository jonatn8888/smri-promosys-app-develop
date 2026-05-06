<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="PromoEntry.aspx.vb" Inherits="PromoEntry" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

<link rel="stylesheet" type="text/css" href="CSS/TabbedWidget.css"/>
<script type="text/javascript" src="jQuery/jquery-1.4.3.min.js"></script>
<script type="text/javascript" src="jQuery/tabbedwidget.js"></script>

<script type="text/javascript">
    function openAttachment()
    {
        var url
        url = "UploadedFiles.aspx";
        attachmentwindow = dhtmlmodal.open('UploadedFiles', 'iframe', url, 'File Attached', 'width=490px,height=305px,center=1,resize=0,scrolling=0','recall')
        
        attachmentwindow.onclose=function()
        { 
        var theform = this.contentDoc.forms[0] 
        document.getElementById("<%=btnDownload.ClientID%>").click();
        return true 
        }
    }
    function openSeedAttachment()
    {
        var url
        url = "UploadSeedFiles.aspx";
        attachmentwindow = dhtmlmodal.open('UploadSeedFiles', 'iframe', url, 'Seed File Attachment', 'width=490px,height=305px,center=1,resize=0,scrolling=0','recall')
        
        attachmentwindow.onclose=function()
        { 
        var theform = this.contentDoc.forms[0] 
        document.getElementById("<%=btnSeedDownload.ClientID%>").click();
        return true 
        }
    }
        
</script>

<script type="text/javascript">
    // 2021-04-07 - add call to BIN entry
    function openPromoBankCardBinEntry()
    {
        var url
        url = "PromoBankCardBinEntry.aspx";
	    texteditorwindow=dhtmlmodal.open('texteditor', 'iframe', url, 'BIN Entry', 'width=600px,height=400px,center=1,resize=0,scrolling=0',"recall")
        texteditorwindow.onclose=function()
        { 
            var theform = this.contentDoc.forms[0] 
            var sBankList = this.contentDoc.getElementById("HiddenField1") 
            document.getElementById("<%=hidBox.ClientID%>").value = sBankList.value;
            document.getElementById("<%=cmdCopyValueToBankList.ClientID%>").click(); 
	        return true 
	    }
    }
</script>

<script type="text/javascript">
    function opentexteditor()
    {
        var url
        url = "texteditor.aspx";
	    texteditorwindow=dhtmlmodal.open('texteditor', 'iframe', url, 'Promotion Mechanics', 'width=709px,height=500px,center=1,resize=0,scrolling=0',"recall")
        texteditorwindow.onclose=function()
        { 
            var theform = this.contentDoc.forms[0] 
            var Mechanics = this.contentDoc.getElementById("HiddenField1") 
            document.getElementById("<%=hidBox.ClientID%>").value = Mechanics.value;
            document.getElementById("<%=cmdCopyValueToMechanics.ClientID%>").click(); 
	        return true 
	    }
    }
</script>

<script type="text/javascript">
    function openPromoNotesEditor()
    {
        var url
        url = "texteditor.aspx";
	    texteditorwindow=dhtmlmodal.open('texteditor', 'iframe', url, 'Promotion Notes', 'width=709px,height=500px,center=1,resize=0,scrolling=0',"recall")
        texteditorwindow.onclose=function()
        { 
            var theform = this.contentDoc.forms[0] 
            var PromoNotes = this.contentDoc.getElementById("HiddenField1") 
            document.getElementById("<%=hidBox.ClientID%>").value = PromoNotes.value;
            document.getElementById("<%=cmdCopyValueToPromoNotes.ClientID%>").click(); 
	        return true 
	    }
    }
</script>

<script type="text/javascript">
function openConfirmationBox(height,width)
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

<script type="text/javascript">
    
    // NBSantos 20140521 - format PlanVsCost textboxes
    function FormatPromoAnalyticsTextboxes()
    {
        var nPlanPromoSales = Number(stripNonNumeric(document.getElementById('<%= txtPlanPromoSales.ClientID %>').value));
        var nPlanPromoCost = Number(stripNonNumeric(document.getElementById('<%= txtPlanPromoCost.ClientID %>').value));
        var nPlanMargin = Number(stripNonNumeric(document.getElementById('<%= txtPlanMargin.ClientID %>').value));
        
        // NBSantos 20150411 - removed lines
        //if (nPlanPromoSales != 0) nPlanMargin = 100 - ((nPlanPromoCost / nPlanPromoSales) * 100);
        //else nPlanMargin = 0;
        
        // format displayed values
        document.getElementById('<%= txtPlanPromoSales.ClientID %>').value = addCommas(nPlanPromoSales.toFixed(2));
        document.getElementById('<%= txtPlanPromoCost.ClientID %>').value = addCommas(nPlanPromoCost.toFixed(2));
        document.getElementById('<%= txtPlanMargin.ClientID %>').value = addCommas(nPlanMargin.toFixed(2));
    }

    function ComputeSMACdealsTotals()
    {   
        var nOrigValue = Number(stripNonNumeric(document.getElementById('<%= txtOrigValue.ClientID %>').value));
        var nPromoValue = Math.round(Number(stripNonNumeric(document.getElementById('<%= txtPromoValue.ClientID %>').value)));
        var nAllocation = Number(stripNonNumeric(document.getElementById('<%= txtAllocation.ClientID %>').value));
        var ddVtype = document.getElementById('<%= cboVendorType.ClientID %>'); 
               
        // compute values 
        var nDiscAmount = nOrigValue - nPromoValue;

        // var nWAPrate = ddVtype.options[ddVtype.selectedIndex].value;
        var nWAPrate = ddVtype.options[ddVtype.selectedIndex].value == 20 ? 10 : ddVtype.options[ddVtype.selectedIndex].value; 
		
        var nPromoBudget = nDiscAmount * nAllocation;
        var nWebAdPlacement = nPromoValue * nAllocation * (nWAPrate / 100);
        var nTotalBudget = nWebAdPlacement + nPromoBudget;
               
        // format displayed values
        document.getElementById('<%= txtOrigValue.ClientID %>').value = addCommas(nOrigValue.toFixed(2));
        document.getElementById('<%= txtPromoValue.ClientID %>').value = addCommas(nPromoValue.toFixed(2));
        document.getElementById('<%= lblDiscAmount.ClientID %>').innerText = addCommas(nDiscAmount.toFixed(2));
        document.getElementById('<%= txtAllocation.ClientID %>').value = addCommas(nAllocation);

        document.getElementById('<%= lblWebAdRate.ClientID %>').innerText = nWAPrate + '%';
        document.getElementById('<%= lblPromoBudget.ClientID %>').innerText = addCommas(nPromoBudget.toFixed(2));
        document.getElementById('<%= lblWebAdPlacement.ClientID %>').innerText = addCommas(nWebAdPlacement.toFixed(2));
        document.getElementById('<%= lblTotalBudget.ClientID %>').innerText = addCommas(nTotalBudget.toFixed(2));
    } 
    
</script>

<script type="text/javascript">
    function openSeedEditor()
    {
        var url
        url = "Seed.aspx";
	    seededitorwindow=dhtmlmodal.open('Seed', 'iframe', url, 'Seed Editor', 'width=620px,height=502px,center=1,resize=0,scrolling=0',"recall")
        seededitorwindow.onclose=function()
        { 
            var transactionType = this.contentDoc.getElementById("transactionType") 
            document.getElementById("<%=transactionType.ClientID%>").value = transactionType.value;
            var PromoNotes = this.contentDoc.getElementById("transactionType") 
            document.getElementById("<%=cmdCopyDatasource.ClientID%>").click(); 
	        return true 
	    }
    }
    function openBinRangeEditor()
    {
        var url
        url = "BinRange.aspx";
	    binrangeditorwindow=dhtmlmodal.open('BinRange', 'iframe', url, 'Bin Range Editor', 'width=619px,height=525px,center=1,resize=0,scrolling=0',"recall")
        binrangeditorwindow.onclose=function()
        { 
            document.getElementById("<%=cmdCopyDatasource.ClientID%>").click(); 
	        return true 
	    }
    }
    
    function openRebateBinRangeEditor()
    {
        var url
        url = "RebateBinRange.aspx";
	    binrangeditorwindow=dhtmlmodal.open('RebateBinRange', 'iframe', url, 'Bin Range Editor', 'width=619px,height=525px,center=1,resize=0,scrolling=0',"recall")
        binrangeditorwindow.onclose=function()
        { 
            document.getElementById("<%=cmdCopyDatasource.ClientID%>").click(); 
	        return true 
	    }
    }
        function openShoulderingEntity(RequestId) {
        // Build URL with query string
        var url
        url = "ShoulderingEntity.aspx?RequestId=" + encodeURIComponent(RequestId);

        // Open modal
        shoulderentityeditorwindow = dhtmlmodal.open(
            'ShoulderingEntity',
            'iframe',
            url,
            'Shouldering Entity',
            'width=800px,height=525px,center=1,resize=0,scrolling=0',
            "recall"
        );

        shoulderentityeditorwindow.onclose = function() { 
            document.getElementById("<%=cmdCopyDatasource.ClientID%>").click(); 
            return true;
        };
    }
</script>


<script type = "text/javascript">
    $('document').ready(function(){
        var cboTenderType = $('#<%=cboXML_TenderType.ClientId %>').val();
        var trLinkBinRange = document.getElementById('<%= trTPL_linkBinRange.ClientID %>');
        var trLinkBinRangeGrid = document.getElementById('<%= trTPL_linkBinRangeGrid.ClientID %>');
        if(cboTenderType != null)
        {
//           showHide(cboQualifiedItem);
            if (cboTenderType == 2){
                trLinkBinRange.style.display = 'block';
                trLinkBinRangeGrid.style.display = 'block';
            }
            else {
                trLinkBinRange.style.display = 'none';
                trLinkBinRangeGrid.style.display = 'none';
            }
        }
    });

    function tenderTypeChanged(source) {
        var value = source.options[source.selectedIndex].value;
        showHide(value);
    }
    
    function showHide(selectedIndex) {
        var trLinkBinRange = document.getElementById('<%= trTPL_linkBinRange.ClientID %>');
        var trLinkBinRangeGrid = document.getElementById('<%= trTPL_linkBinRangeGrid.ClientID %>');
        if (selectedIndex == 2){
            trLinkBinRange.style.display = 'block';
            trLinkBinRangeGrid.style.display = 'block';
        }
        else {
            trLinkBinRange.style.display = 'none';
            trLinkBinRangeGrid.style.display = 'none';
        }
    }
    
</script>

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkSaveMemo" runat="server">View Request Preview</asp:LinkButton><br />
        <br />
    </div>
    
    <div id="contents">
    
            <asp:HiddenField ID="hidBox" runat="server" />
            <asp:HiddenField ID="transactionType" runat="server" />

                    <table cellpadding="3" cellspacing="0" id="doc-table">
                        <tr>
                            <td colspan="2" class="DocTabHeadOff">
                                <asp:LinkButton ID="lnkRequest" runat="server" Width="230px">Sales Promotion Request</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="DocTabHeadOn">
                                Promotion Information</td>
                        </tr>
                        <tr>
                            <td style="width: 112px;">
                                &nbsp;</td>
                            <td class="field-cell">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 112px">
                                Promo Category:</td>
                            <td class="field-cell">
                                <asp:DropDownList ID="cboPromoCategory" runat="server"
                                        DataTextField="TypeCategory" DataValueField="TypeCategory" Width="294px" AppendDataBoundItems="True" AutoPostBack="True">
                                </asp:DropDownList></td>
                        </tr>
                        <tr style="font-size: 12pt">
                            <td style="width: 112px; height: 8px">
                                <span style="font-size: 10pt">Promo Type:</span></td>
                            <td class="field-cell" style="height: 8px">
                                <asp:DropDownList ID="cboPromoSubCategory" runat="server"
                                        DataTextField="TypeSubCategory" DataValueField="TypeSubCategory" Width="294px" AppendDataBoundItems="True" AutoPostBack="True">
                                </asp:DropDownList></td>
                        </tr>
                        <tr style="font-size: 12pt">
                            <td style="width: 112px; height: 8px;">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Sub Type:</span></td>
                            <td class="field-cell" style="height: 8px">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">
                                    <asp:Label ID="lblPromoType" runat="server"></asp:Label><asp:DropDownList ID="cboPromoType" runat="server"
                                        DataTextField="TypeDesc" DataValueField="PromoTypeID" Width="294px" AppendDataBoundItems="True" AutoPostBack="True">
                                    </asp:DropDownList></span><span style="font-size: 10pt; font-family: Trebuchet MS"></span></td>
                        </tr>
                        <tr>
                            <td style="width: 112px;" valign="top">
                                <span style="font-size: 10pt; font-family: Trebuchet MS" class="field-row"> 
                                    <asp:Label ID="lblMechanics" runat="server" Text="Mechanics:"></asp:Label>&nbsp;</span></td>
                            <td class="field-cell" align="center" style="text-align: left;">
                            
                                <asp:Panel ID="panMechanics" runat="server">
                                <div  style="width: 570px; height: 147px; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke; clear: both; clip: rect(auto auto auto auto); text-align: left;">
                                <asp:Literal ID="litMechanics" runat="server"></asp:Literal></div>
                                    <table style="width: 578px; border-top-style: none; border-right-style: none; border-left-style: none;
                                        border-bottom-style: none">
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="lnkEditMechanics" runat="server" CssClass="action-link" Enabled="False">Edit Mechanics</asp:LinkButton></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr><td></td></tr>
                                    </table>
                                </asp:Panel>
                                
                                <asp:Panel ID="panBarcode" runat="server" HorizontalAlign="Left" Width="585px" Wrap="False" Visible="False">
                                    <table style="width: 580px">
                                        <tr id="trXML_Barcode" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                               <asp:Label ID="lblBarcode" runat="server" Text="Barcode:"></asp:Label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_Barcode" runat="server" MaxLength="13" Width="130px"></asp:TextBox>
                                                <!-- Add validator for minimum length requirement -->
                                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtXML_Barcode" ID="txtXML_Barcode_MinLengthValidator" ValidationExpression="^[\s\S]{13,}$" runat="server" ErrorMessage="Barcode must be 13 characters long."></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                 </asp:Panel>
                                                                 
                                <asp:Panel ID="panClassDiscount" runat="server" HorizontalAlign="Left" Width="585px" Wrap="False" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td>
                                               <asp:TextBox ID="txtDiscount" runat="server" MaxLength="3" Width="25px"></asp:TextBox>
                                               <asp:Label ID="lblDiscount" runat="server" Text="% Discount on all regular-priced items."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                 </asp:Panel>
                                
                                <asp:Panel ID="panMarkdown" runat="server" HorizontalAlign="Left" Width="585px" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtMarkdown" runat="server" MaxLength="20" Width="94px"></asp:TextBox>
                                                <asp:Label ID="lblMarkdownDesc" runat="server" Text="% Markdown on selected items."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel id="panSpecialPromo" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Promo Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtPurchaseQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="52px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; height: 17px; text-align: left;" valign="middle">
                                                                                    Free Quantity:</td>
                                            <td style="height: 17px; text-align: left">
                                                <asp:TextBox ID="txtFreeQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="52px"></asp:TextBox><strong></strong></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Promo Amount:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtPromoPrice" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                    
                                <asp:Panel id="panBuy1Take1" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Buy Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtB1T1_BuyQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="20px" BackColor="#E0E0E0" ReadOnly="True">1</asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; height: 17px; text-align: left;" valign="middle">
                                                Take Quantity:</td>
                                            <td style="height: 17px; text-align: left">
                                                <asp:TextBox ID="txtB1T1_TakeQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="20px" BackColor="#E0E0E0" ReadOnly="True">1</asp:TextBox><strong></strong></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                % Discount:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtB1T1_PercentDisc" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="20px" BackColor="#E0E0E0" ReadOnly="True">50</asp:TextBox>
                                                <strong>%</strong></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel id="panAnyXForP" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Buy Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtAnyX4P_BuyQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Promo Price:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtAnyX4P_PromoPrice" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="82px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                    
                                </asp:Panel>

                                <asp:Panel id="panBuyGetSameForP" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Buy Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBG4Ps_BuyQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Take Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBG4Ps_TakeQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Discount Amount:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtBG4Ps_DiscAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="82px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel id="panBuyGetDiffForP" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Buy Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBG4Pd_BuyQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Take Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBG4Pd_TakeQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Discount Amount:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtBG4Pd_DiscAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="82px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel id="panBuyGetSamePercent" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Buy Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBGPerOffs_BuyQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Take Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBGPerOffs_TakeQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                % Discount:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtBGPerOffs_PercentDisc" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="82px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel id="panBuyGetDiffPercent" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Buy Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBGPerOffd_BuyQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Take Quantity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtBGPerOffd_TakeQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="46px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left; height: 27px;" valign="middle">
                                                % Discount:</td>
                                            <td style="text-align: left; height: 27px;">
                                                <asp:TextBox ID="txtBGPerOffd_PercentDisc" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="82px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel id="panAnyXYforP" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr>
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Buy Quantity (Diff. Price):</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtAnyXY_BuyQty" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="20px" BackColor="#E0E0E0" ReadOnly="True">2</asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Promo Price:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtAnyXY_PromoPrice" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="82px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel ID="panSMACdeals" runat="server" Visible="False" Width="585px">
                                   <table style="width: 580px">
                                       <tr>
                                           <td style="width: 436px; height: 20px; text-align: left;" valign="middle">
                                               Original Value:</td>
                                           <td colspan="2" style="height: 20px; text-align: left">
                                               <asp:TextBox ID="txtOrigValue" runat="server" onfocus="return PrepNumericTextbox(this);" onblur="return ComputeSMACdealsTotals();" MaxLength="15" Width="130px" CssClass="RightAligned"></asp:TextBox></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; height: 17px; text-align: left;" valign="middle">
                                               Deal Price:</td>
                                           <td colspan="2" style="height: 17px; text-align: left">
                                               <!--Revised 07232012@smretailinc: to format promo value input (integers only)-->
                                               <asp:TextBox ID="txtPromoValue" runat="server" onfocus="return PrepNumericTextbox(this, 1);" onblur="return ComputeSMACdealsTotals();" MaxLength="15" Width="130px" CssClass="RightAligned"></asp:TextBox><strong></strong></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left; height: 21px;" valign="middle">
                                               Customer Savings:</td>
                                           <td colspan="2" style="height: 21px">
                                               <asp:Label ID="lblDiscAmount" runat="server" Font-Bold="False" Width="134px" CssClass="RightAligned"></asp:Label></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left;" valign="middle">
                                               Deal Allocation:</td>
                                           <td colspan="2" style="text-align: left">
                                               <asp:TextBox ID="txtAllocation" runat="server" onfocus="return PrepNumericTextbox(this, 1);" onblur="return ComputeSMACdealsTotals();" MaxLength="15" Width="130px" CssClass="RightAligned"></asp:TextBox></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left;" valign="middle">
                                               Redemption Period:</td>
                                           <td colspan="1" style="width: 306px; text-align: left">
                                               <asp:TextBox ID="txtOnlineSellingStart" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                               <input name="calSellingStart" type="button" id="calSellingStart" class="btnCal" onclick="displayDatePicker('<%=txtOnlineSellingStart.ClientID %>');" style="left: 322px; top: 345px" />&nbsp;
                                           </td>
                                           
                                           <td style="text-align: left; width: 377px;">
                                               <asp:TextBox ID="txtOnlineSellingEnd" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                               <input name="calSellingEnd" type="button" id="calSellingEnd" class="btnCal" onclick="displayDatePicker('<%=txtOnlineSellingEnd.ClientID %>');" style="left: 322px; top: 345px" />&nbsp;
                                           </td>
                                       </tr>
                                       <tr>
                                           <td style="text-align: left;" valign="middle" colspan="3">
                                               &nbsp;</td>
                                       </tr>
                                       <tr>
                                           <td style="text-align: left; border-bottom: solid thin GrayText;" 
                                               valign="middle" colspan="3">
                                               <strong>Billing Details</strong> &nbsp;</td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left;" valign="middle">
                                               Outright or Store Consignor:</td>
                                           <td colspan="2" style="text-align: left">
                                               <asp:DropDownList ID="cboVendorType" runat="server" onchange="return ComputeSMACdealsTotals();" Width="187px">
                                                   <asp:ListItem>- - Select Value - -</asp:ListItem>
                                                   <asp:ListItem Value="10">Outright</asp:ListItem>
                                                   <asp:ListItem Value="20">Store Consignor</asp:ListItem>
                                               </asp:DropDownList></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left; height: 21px;" valign="middle">
                                               Web Ad Placement (%):</td>
                                           <td colspan="1" style="width: 306px; text-align: left; height: 21px;">
                                               <asp:Label ID="lblWebAdRate" runat="server" Font-Bold="False" Width="130px" CssClass="RightAligned">0 %</asp:Label></td>
                                           <td style="width: 377px; text-align: left; height: 21px;">
                                           </td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left" valign="middle">
                                               Promo Budget:</td>
                                           <td colspan="1" style="width: 306px; text-align: left">
                                               <asp:Label ID="lblPromoBudget" runat="server" Font-Bold="False" Width="130px" CssClass="RightAligned">0.00</asp:Label></td>
                                           <td style="width: 377px; text-align: left">
                                           </td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left" valign="middle">
                                               Web Ad Placement (WAP):</td>
                                           <td colspan="1" style="width: 306px; text-align: left">
                                               <asp:Label ID="lblWebAdPlacement" runat="server" Font-Bold="False" Width="130px" CssClass="RightAligned">0.00</asp:Label></td>
                                           <td style="width: 377px; text-align: left">
                                           </td>
                                       </tr>
                                       <tr>
                                           <td style="width: 436px; text-align: left" valign="middle">
                                               Total Market Budget:</td>
                                           <td colspan="1" style="width: 306px; text-align: left">
                                               <asp:Label ID="lblTotalBudget" runat="server" Font-Bold="False" Width="130px" CssClass="RightAligned">0.00</asp:Label></td>
                                           <td style="width: 377px; text-align: left">
                                           </td>
                                       </tr>
                                       <tr>
                                           <td colspan="3" style="text-align: left" valign="middle">
                                               &nbsp;</td>
                                       </tr>
                                   </table>
                                </asp:Panel>
                                
                                <asp:Panel id="panComboOffer" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="false">                                    
                                    <table style="width: 580px">
                                      <tr id="trXML_QualifiedCust" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">Qualified Customers:</td>
                                            <td style="text-align: left; height: 23px;">
                                                <asp:TextBox ID="txtQualifiedCust" runat="server" Width="368px" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trXML_NameOfPartner" runat="server">
                                            <td style="width: 173px; text-align: left; height: 23px;" valign="middle">Name of Partner/s:</td>
                                            <td style="text-align: left; height: 23px;">
                                                <asp:TextBox ID="txtNameOfPartner" runat="server" Width="368px" Wrap="False" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trXML_ProofOfMembership" runat="server">
                                            <td style="width: 173px; text-align: left; height: 23px;" valign="middle">Proof Of Membership (e.g valid ID):</td>
                                            <td style="text-align: left; height: 23px;">
                                                <asp:TextBox ID="txtProofOfMembership" runat="server" Width="368px" Wrap="False" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trXML_PartnerEstabGWP" runat="server">
                                            <td style="width: 173px; text-align: left; height: 23px;" valign="middle">Name of Partner Establishment/s (GWP):</td>
                                            <td style="text-align: left; height: 23px;">
                                                <asp:TextBox ID="txtPartnerEstabGWP" runat="server" Width="368px" Wrap="False" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
 
                                <asp:Panel id="panTemplated" runat="server" Visible="False" Width="585px" Wrap="False" HorizontalAlign="Left">
                                    <table style="width: 580px">
                                        <tr id="trTPL_PurchaseReq" runat="server">
                                            <td style="width: 173px; text-align: left" valign="middle">Purchase Requirement:</td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="cboTPL_PurchaseReq" runat="server" Width="350px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                
                                        <tr id="trTPL_BuyQty" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Buy Qty:</td>
                                            
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_BuyQty" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                                <asp:CheckBox ID="chkTPL_BuyQtyOrMore"  runat="server" Text="Or more" style="margin-left:10px;" />
                                            </td>
                                        </tr>
                                        
                                        
                                        <tr id="trTPL_TakeQty" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Take Qty:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_TakeQty" runat="server" MaxLength="10" Width="100px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_DiscAmount" runat="server">
                                            <td id="tdDiscAmount"  runat="server" style="width: 173px; text-align: left;" valign="middle" visible="false">
                                                Amount Discount:</td>
                                            <td id="tdPromoPrice"  runat="server" style="width: 173px; text-align: left;" valign="middle" visible="false">
                                                Promo Price:</td>                                                
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTPL_DiscAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_PercentDisc" runat="server">
                                            <td id="tdPercentMarkDown"  runat="server" style="width: 173px; text-align: left;" valign="middle" visible="false">
                                                Percent Markdown:</td>
                                            <td id="tdPercentDisc"  runat="server" style="width: 173px; text-align: left;" valign="middle" visible="false">
                                                % Discount:</td>                                                
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTPL_PercentDisc" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="40px"></asp:TextBox>
                                                <strong>%</strong></td>
                                        </tr>
                                        <tr id="trTPL_ReqAmount" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Required Amount:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_ReqAmount" runat="server" MaxLength="10" Width="100px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_NumMonths" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">No. of Months:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_NumMonths" runat="server" MaxLength="2" Width="48px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_BonusPoints" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Bonus Points:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_BonusPoints" runat="server" MaxLength="10" Width="48px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_BankList" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Participating Banks:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_BankList" runat="server" MaxLength="200" Width="200px"></asp:TextBox>
                                                <!--Bank List Link -->
                                                <asp:LinkButton id="lnkShowBankBinEntry" runat="server" Width="100px" Font-Size="Small" CssClass="action-link">Edit Bank List</asp:LinkButton>
                                                </td>
                                        </tr>
                                        <tr id="trTpl_SubsidyRate" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Subsidy Rate (%):</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTpl_SubsidyRate" runat="server" MaxLength="5" Width="90px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_BrandNames" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Name of Brand/Product:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_BrandNames" runat="server" MaxLength="100" Width="350px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_CelebName" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Celebrity/Mascot:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_CelebName" runat="server" MaxLength="100" Width="350px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_ItemName" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Name of Item/Collaterals:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_ItemName" runat="server" MaxLength="50" Width="350px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_DeptName" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Department:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_DeptName" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_ActivityName" runat="server">
                                        <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                             <asp:Label ID="lblActivityName" runat="server" Text="Name of Activity/Event:"></asp:Label>
                                        </td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_ActivityName" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_SellingArea" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Area/Location:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_SellingArea" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_EventTime" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Time:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_EventTime" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_TransDesc" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Transaction Description:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_TransDesc" runat="server" MaxLength="100" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_FreeItems" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Free Item/s:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_FreeItems" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_Prizes" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Prize/s:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_Prizes" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_RefMemo" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Reference Memo:</td>
                                            <td colspan="2" style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtTPL_RefMemo" runat="server" MaxLength="25" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trTPL_ProcessType" runat="server">
                                            <td style="width: 173px; text-align: left" valign="middle">Promotion Application:</td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="cboTPL_ProcessType" runat="server" Width="350px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trTPL_MinAmount" runat="server" Visible="False"> <%--mad7740--%>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblMinAmount_Templated" runat="server" Text="Minimum Amount:"></asp:Label></td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTPL_MinAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                          <tr id="trTPL_discountcapping" runat="server" Visible="False"> <%--mad7740--%>
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lbldiscountcapping_templated" runat="server" Text="Discount Capping:"></asp:Label></td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTPL_discountcapping" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        
                                         <tr id="trTPL_percentage" runat="server" Visible="False">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblpercentage_templated" runat="server" Text="Percent Discount:"></asp:Label>
                                                </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTPL_percentage" runat="server" CssClass="RightAligned" MaxLength="3" 
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="40px"></asp:TextBox>
                                                <strong>%</strong></td>
                                        </tr>
                                        <tr id="trXML_Sponsorship" runat="server" Visible="False">
                                                <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                    Shouldering Entity:</td>
                                                <td style="height: 20px; text-align: left">
                                               <asp:DropDownList ID="cboXML_Sponsorship" runat="server" Width="273px" AppendDataBoundItems="True"
                                                        DataTextField="ElementName" DataValueField="ElementValue">
                                                   <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                               </asp:DropDownList></td>
                                            </tr>
                                        <%--
                                        mad7740end--%>
<%--                                        <tr id="trXML_Sponsorship2" runat="server" Visible="True">
                                                <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                    Shouldering Entity:</td>
                                                <td style="height: 20px; text-align: left">
                                               <asp:LinkButton ID="linkShoulderingEntity" runat="server"  Enabled="True" 
                                                        style="padding-right: 10px;" CssClass="action-link">Edit Shouldering Entity</asp:LinkButton></td>
                                        </tr>--%>
                                        <tr id="trPOS_StdExclusion" runat="server">
                                            <td colspan="3" style="height: 20px; text-align: left">
                                                <asp:CheckBox ID="chkPOS_StdExclusion" runat="server" Text="Standard Discount Exclusions Apply" /></td>
                                        </tr> 
                                        <tr id="trPOS_PermExclusion" runat="server">
                                            <td colspan="3" style="height: 20px; text-align: left">
                                                <asp:CheckBox ID="chkPOS_PermExclusion" runat="server" Text="Permanent Exclusions Apply" /></td>
                                        </tr> 
                                    </table>
                                </asp:Panel>
                                  
                                <asp:Panel id="panGenericHostXML" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr id="trXML_Descr1prm" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Description:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_Descr1prm" runat="server" MaxLength="25" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_Descr2prm" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Description 2:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_Descr2prm" runat="server" MaxLength="25" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_ReceiptDesc1" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Receipt Description:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_ReceiptDesc1" runat="server" MaxLength="25" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_ReceiptDesc2" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">Receipt Description 2:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_ReceiptDesc2" runat="server" MaxLength="25" Width="256px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_ActionType" runat="server">
                                            <td style="width: 173px; text-align: left" valign="middle">
                                                Action Type:</td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="cboXML_ActionType" runat="server" Width="262px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trXML_DiscountType" runat="server">
                                            <td style="width: 173px; text-align: left" valign="middle">
                                                Discount Type:</td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="cboXML_DiscountType" runat="server" Width="350px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trXML_ProcessType" runat="server">
                                            <td style="width: 173px; text-align: left" valign="middle">
                                                Promotion Application:</td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="cboXML_ProcessType" runat="server" Width="350px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trXML_MaxQty" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Maximum Quantity:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_MaxQty" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_MaxAmount" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Maximum Amount:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_MaxAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_MinAmount" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblMinAmount" runat="server" Text="Minimum Amount:"></asp:Label></td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_MinAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_CompessID" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Compression ID:</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_CompessID" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_ExclusionType" runat="server">
                                            <td style="width: 173px; text-align: left" valign="middle">
                                                Exclusion Type:</td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="cboXML_ExclusionType" runat="server" Width="350px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trXML_ActDeacTime" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Time:</td>
                                            <td style="text-align: left">
<%--                                                <asp:TextBox ID="txtXML_AcTime" runat="server" MaxLength="25" Width="80px"></asp:TextBox>
                                                to
                                                <asp:TextBox ID="txtXML_Deactime" runat="server" MaxLength="25" Width="80px"></asp:TextBox>--%>
                                                <asp:DropDownList ID="cboXML_StartTime" runat="server" Width="90px" Visible="False">
                                                </asp:DropDownList>
                                                to
                                                <asp:DropDownList ID="cboXML_EndTime" runat="server" Width="90px" Visible="False">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trXML_ActiveDays" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Active Days:</td>
                                            <td style="text-align: left">
                                                <asp:CheckBox ID="chkMonday" runat="server" Text="Mon" />
                                                <asp:CheckBox ID="chkTuesday" runat="server" Text="Tue" />
                                                <asp:CheckBox ID="chkWednesday" runat="server" Text="Wed" />
                                                <asp:CheckBox ID="chkThursday" runat="server" Text="Thu" />
                                                <asp:CheckBox ID="chkFriday" runat="server" Text="Fri" />
                                                <asp:CheckBox ID="chkSaturday" runat="server" Text="Sat" />
                                                <asp:CheckBox ID="chkSunday" runat="server" Text="Sun" /></td>
                                        </tr>
                                        <tr id="trXML_Priority" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Priority:</td>
                                            <td style="text-align: left"><asp:DropDownList AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue" ID="cboXML_Priority" runat="server" Width="181px">
                                                <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                            </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                        
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                        
                                        <tr id="trXML_RuleValue1" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_RuleValue1" runat="server" Text="Rule 1 Value:"></asp:Label></td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_RuleValue1" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="85px" ToolTip="Rule 1"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_RuleValue2" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_RuleValue2" runat="server" Text="Rule 2 Value:"></asp:Label></td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_RuleValue2" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="85px" ToolTip="Rule 2"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_RuleValue3" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_RuleValue3" runat="server" Text="Rule 3 Value:"></asp:Label></td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_RuleValue3" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="85px" ToolTip="Rule 3"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_RuleValue4" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_RuleValue4" runat="server" Text="Rule 4 Value:"></asp:Label></td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:TextBox ID="txtXML_RuleValue4" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="85px" ToolTip="Rule 4"></asp:TextBox></td>
                                        </tr>

                                        <tr id="trXML_Condition1" runat="server" visible="false">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_Condition1" runat="server" Text="Condition 1:"></asp:Label></td>
                                            <td style="height: 20px; text-align: left"><asp:DropDownList ID="cboXML_Condition1" runat="server" Width="369px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue" ToolTip="Condition 1">
                                                <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                            </asp:DropDownList></td>
                                        </tr>
                                        
                                        <tr id="trXML_EligibleCards" runat="server" visible="false">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Eligible Cards:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                <br /><br />
                                                <asp:CheckBoxList ID="cblEligibleCards" runat="server" RepeatColumns="2"></asp:CheckBoxList>
                                            </td>
                                        </tr>
                                        
                                         <tr id="trXML_SpecialDiscType" runat="server">
                                            <td style="width: 173px; text-align: left; height: 25px;" valign="middle">Special/ Discount:</td>
                                            <td style="text-align: left; height: 25px; margin-left: 40px;">
                                                <asp:DropDownList ID="cboXML_SpecialDiscType" runat="server" Width="368px" AutoPostBack="True" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>

                                        <tr id="trXML_RebateDiscType" runat="server">
                                            <td style="width: 173px; text-align: left; height: 25px;" valign="middle">Rebate/ Discount:</td>
                                            <td style="text-align: left; height: 25px; margin-left: 40px;">
                                                <asp:DropDownList ID="cboXML_RebateDiscType" runat="server" Width="368px" AutoPostBack="True" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>


                                        <tr id="trXML_DiscAmount" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_DiscAmount" runat="server" Text="Discount Amount:"></asp:Label></td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_DiscAmount" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr id="trXML_PercentDisc" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_PercentDisc" runat="server" Text="Percent Discount:"></asp:Label>
                                                </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_PercentDisc" runat="server" CssClass="RightAligned" MaxLength="3"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="40px"></asp:TextBox>
                                                <strong>%</strong></td>
                                        </tr>
                                        <tr id="trXML_MaxFreeQty" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblXML_MaxFreeQty" runat="server" Text="Max Qty for Free Gift Items:"></asp:Label>
                                                </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_MaxFreeQty" runat="server" CssClass="RightAligned" MaxLength="2"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        
                                        <tr id="trXML_DiscCapAmount" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:CheckBox ID="chk_DiscCapTickBox" AutoPostBack="True"  runat="server" />
                                                <asp:Label ID="lblXML_DiscCapTickBox"  runat="server" Text="Capped Amount:"></asp:Label>
                                             </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_DiscCap" runat="server" CssClass="RightAligned" 
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="40px"></asp:TextBox>
                                             </td>
                                        </tr>
                                        <tr id="trXML_SMACKitPrice" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                <asp:Label ID="lblSMACKitPrice" runat="server" Text="SMAC Kit Price:"></asp:Label></td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_SMACKitPrice" runat="server" CssClass="RightAligned" MaxLength="15"
                                                    onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="130px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: left; height: 21px;" valign="middle">
                                                &nbsp;</td>
                                        </tr>
                                        <tr id="Tr1" runat="server">
                                            <td style="width: 173px; text-align: left" valign="middle">
                                            </td>
                                            <td style="text-align: left">
                                            </td>
                                        </tr>
                                        <tr id="trXML_CouponMsg" runat="server" visible="false">
                                            <td style="width: 173px; text-align: left" valign="middle">
                                                <asp:Label ID="lblXML_RaffleMessage" runat="server" Text="Message:"></asp:Label>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtXML_CouponMsg01" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg02" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg03" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg04" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg05" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg06" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg07" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg08" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg09" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg10" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg11" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg12" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg13" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg14" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg15" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg16" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg17" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg18" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg19" runat="server" MaxLength="43"  Width="410px"></asp:TextBox><br />
                                                <asp:TextBox ID="txtXML_CouponMsg20" runat="server" MaxLength="43"  Width="410px"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                
                                <asp:Panel id="panCommonFields" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="True">                                    
                                    <table style="width: 580px">
                                        <tr id="trXML_QualifiedItems" runat="server">
                                            <td style="width: 173px; text-align: left; height: 25px;" valign="middle">Qualified Items:</td>
                                            <td style="text-align: left; height: 25px;">
                                                <asp:DropDownList ID="cboXML_QualifiedItems" runat="server" Width="368px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trXML_PosPromoSubType" runat="server">
                                            <td style="width: 173px; text-align: left; height: 25px;" valign="middle">Subtype:</td>
                                            <td style="text-align: left; height: 25px;">
                                                <asp:DropDownList ID="cboXML_PosPromoSubType" runat="server" Width="368px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue">
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trTPL_PromoNotes" runat="server">
                                            <td style="width: 173px; text-align: left;" valign="middle">Promo Notes:</td>
                                            <td style="text-align: left">
                                                <div style="width: 410px; height: 147px; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke; clear: both; clip: rect(auto auto auto auto); text-align: left;">
                                                    <asp:Literal ID="litTPLPromoNotes" runat="server"></asp:Literal>
                                                </div>
                                                <table style="width: 410px; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none">
                                                    <tr>
                                                        <td><asp:LinkButton ID="lnkEditPromoNotes" runat="server" CssClass="action-link" Enabled="True">Edit Notes</asp:LinkButton></td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                                               
                                <asp:Panel id="panDiscCharging" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr id="trXML_CompSponsorship" runat="server" visible="false">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Shouldering Entity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:LinkButton ID="linkShoulderingEntity" runat="server"  Enabled="True" 
                                                        style="padding-right: 10px;" CssClass="action-link">Edit Shouldering Entity</asp:LinkButton>
                                            </td>
                                        </tr>      
                                        <tr id="trXML_CompSponsorship2" runat="server" visible="false">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Shouldering Entity:</td>
                                            <td style="height: 20px; text-align: left">
                                                <asp:DropDownList ID="cboCompSponsorship" runat="server" Width="273px" 
                                                    DataTextField="ElementName" DataValueField="ElementValue">
                                               <asp:ListItem Value="0">- - Select Value - -</asp:ListItem>
                                           </asp:DropDownList>
                                           </td>
                                        </tr>                                                                   
                                    </table>
                                    <div id="divCompSponsorship" runat="server" style="width:100%; height:1px; overflow:auto">
                                              <asp:GridView ID="gridCompSponsorship" runat="server" AutoGenerateColumns="False" 
                                                  BorderStyle="Solid" ShowHeaderWhenEmpty="True"
                                                BorderWidth="1px" CellPadding="4" ForeColor="#333333" Width="100%" 
                                                  AllowSorting="True" ShowHeader="True">
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:BoundField DataField="Entity" HeaderText="Entity"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="ElementName" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Financing Partner" HeaderText="Financing Partner" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Bin" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Selection 1" HeaderText="Selection 1" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLow">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Selection 2" HeaderText="Selection 2" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanHigh" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Percentage" HeaderText="Percentage" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLength" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                            </div>    
                                </asp:Panel>

                                <!-- NBS:20210408 -->
                                <asp:Panel id="panChargeableEntity" runat="server" Width="585px" Wrap="False" HorizontalAlign="Left" Visible="False">
                                    <table style="width: 580px">
                                        <tr id="trXML_ChargeableEntity" runat="server">
                                            <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                                Chargeable Entity:</td>
                                            <td style="height: 20px; text-align: left">
                                           <asp:DropDownList ID="cboChargeableEntity" runat="server" Width="273px" 
                                                    DataTextField="ElementName" DataValueField="ElementValue" AutoPostBack="true">
                                               <asp:ListItem Value="0">- - Select Value - -</asp:ListItem>
                                           </asp:DropDownList></td>
                                        </tr>
                                        <tr id="trTplChargeEnt_VendorCode" runat="server" visible="false">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                Vendor Code:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTplChargeEnt_VendorCode" runat="server" CssClass="RightAligned" MaxLength="6" onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trTplChargeEnt_DSsub" runat="server" visible="false">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                DS Contrib:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTplChargeEnt_DSsub" runat="server" CssClass="RightAligned" MaxLength="5" onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="50px"></asp:TextBox>%
                                            </td>
                                        </tr>
                                        <tr id="trTplChargeEnt_BUsub" runat="server" visible="false">
                                            <td style="width: 173px; text-align: left;" valign="middle">
                                                MBU Contrib:
                                            </td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtTplChargeEnt_BUsub" runat="server" CssClass="RightAligned" MaxLength="5" onblur="return FormatNumericTextbox(this);" onfocus="return PrepNumericTextbox(this);"
                                                    Width="50px"></asp:TextBox>%
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel ID="panBankBins" runat="server" Visible="True">
                                
                                    <div style="width:100%; height:250px; overflow:auto">
                                        <asp:CheckBoxList ID="lstBankBins" runat="server"></asp:CheckBoxList>
                                    </div>
                                    
                                    <asp:Button ID="btnShowBankBins" runat="server" Text="Update Bank Bins" OnClick="ShowBankBins" Visible="False" />
                                
                                    <div style="width:100%; height:250px; overflow:auto">
                                    <asp:GridView ID="gridPromoBankBins" runat="server">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_CheckedChanged" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CardName" HeaderText="CardName" SortExpression="CardName" Visible="False" />
                            <asp:BoundField DataField="CardName" HeaderText="Card Name" SortExpression="CardName">
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EmptyDataTemplate>
                            <div align="center">No BIN Selected for this Promo</div>
                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="panSwipestakes" runat="server">
                                <div  style="width: 570px; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke; clear: both; clip: rect(auto auto auto auto); text-align: left;">
                               <%-- <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>--%>
                                    
                                    <div class="tabs" style="float:left">        
                                        <ul class="tabNavigation">
                                            <a href="#winningMessage"><li class="tab">Winning Message</li> </a>  
                                            <a href="#nonwinningMessage"><li class="tab">Nonwinning Message</li> </a>   
                                            <a href="#posMessage"><li class="tab">POS Message</li>  </a>   
                                        </ul> 

                                        <div class="sub-div" style="border-color: transparent !imporant; 
                                            margin-top: 0px !important;">
                                            <div id="winningMessage">
                                                <table style="width: 578px; border-top-style: none; border-right-style: none; border-left-style: none;
                                                    border-bottom-style: none">
                                                    <tr id="trTPL_WinningMsg" runat="server" visible="true">
                                                        <td style="width: 173px; text-align: left" valign="middle">
                                                            <asp:Label ID="lblXML_WinningMsg" runat="server" Text="Message:"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtXML_WinningMsg01" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg02" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg03" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg04" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg05" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg06" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg07" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg08" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg09" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg10" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg11" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg12" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg13" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg14" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg15" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg16" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg17" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg18" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg19" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_WinningMsg20" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox>
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
                                                        <td style="width: 173px; text-align: left" valign="middle">
                                                            <asp:Label ID="Label1" runat="server" Text="Message:"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtXML_NonwinningMsg01" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg02" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg03" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg04" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg05" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg06" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg07" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg08" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg09" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg10" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg11" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg12" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg13" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg14" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg15" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg16" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg17" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg18" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg19" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_NonwinningMsg20" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox>
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
                                                        <td style="width: 173px; text-align: left" valign="middle">
                                                            <asp:Label ID="Label2" runat="server" Text="Message:"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:TextBox ID="txtXML_POSMsg01" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg02" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg03" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg04" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg05" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg06" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg07" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg08" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg09" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg10" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg11" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg12" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg13" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg14" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg15" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg16" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg17" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg18" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg19" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox><br />
                                                            <asp:TextBox ID="txtXML_POSMsg20" runat="server" MaxLength="38" CssClass="Tb_Message" Width="410px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div> 
                                        </div>
                                    </div> 
                                    <table style="width: 578px; border-top-style: none; border-right-style: none; border-left-style: none;
                                        border-bottom-style: none">
                                        <tr>
                                            <td> &nbsp; </td>
                                            <td> &nbsp; </td>
                                        </tr>
                                         <tr id="trXML_TenderType" runat="server">
                                            <td style="width: 173px; text-align: left; height: 25px;" valign="middle">Tender Type:</td>
                                            <td style="text-align: left; height: 25px;">
                                                <asp:DropDownList ID="cboXML_TenderType" runat="server" Width="368px" AppendDataBoundItems="True" DataTextField="ElementName" DataValueField="ElementValue" OnChange="tenderTypeChanged(this)" >
                                                    <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td> &nbsp; </td>
                                            <td> &nbsp; </td>
                                        </tr>
                                        <tr id="trTPL_linkBinRange" runat="server">
                                            <td> &nbsp; </td>
                                            <td><asp:LinkButton ID="linkBinRange" runat="server" CssClass="action-link" Enabled="True" style="padding-right: 10px;">Edit Bin Range</asp:LinkButton></td>
                                        </tr>
                                        <tr id="trTPL_linkBinRangeGrid" runat="server">
                                            <td colspan="2" style="height: 103px; text-align: center; background-color: transparent;">
                                            
                                            <div id="divBinRange" runat="server" style="padding-right: 2px; padding-left: 2px; padding-bottom: 2px; overflow: auto;
                                                    width: 100%; padding-top: 2px; height: 250px; background-color: transparent" align="center">
                                                    
                                            <asp:GridView ID="gvBinRange" runat="server" AutoGenerateColumns="False" BorderStyle="Solid" ShowHeaderWhenEmpty="True"
                                                BorderWidth="1px" CellPadding="4" ForeColor="#333333" Width="98%" AllowSorting="True" ShowHeader="True">
                                        

                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:BoundField DataField="ElementName" HeaderText="Range Type"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="ElementName" >
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BIN" HeaderText="Bin" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Bin" >
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PanLow" HeaderText="Pan Low" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLow">
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PanHigh" HeaderText="Pan High" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanHigh" >
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PanLength" HeaderText="Pan Length" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLength" >
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                                </div>
                              
                                            </td>
                                        </tr>
                                        <tr id="trTPL_linkSeeding" runat="server" visible=false>
                                            <td> &nbsp; </td>
                                            <td><asp:LinkButton ID="linkSeeding" runat="server" CssClass="action-link" Enabled="True" style="padding-right: 10px;">Edit Seeding</asp:LinkButton></td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="2" style="height: 103px; text-align: center;">
                                            <div id="divSeeding" runat="server"  style="padding-right: 2px; padding-left: 2px; padding-bottom: 2px; overflow: auto;
                                                    width: 100%; padding-top: 2px; height: 150px; background-color: transparent" align="center">
                                                <asp:GridView ID="gvSeeding" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                    BorderWidth="1px" CellPadding="4" ForeColor="#333333" Width="97%" AllowSorting="True" ShowHeader="True">
                                                    
                                                    <EmptyDataTemplate>
                                                        <b>No Seed Added.</b>
                                                    </EmptyDataTemplate>

                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Branch" HeaderText="Branch" 
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Branch" >
                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" 
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="StartDate" DataFormatString="{0:d}" >
                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="EndDate" HeaderText="End Date" 
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="EndDate" DataFormatString="{0:d}" >
                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="MaxNumber" HeaderText="Max Number" 
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="MaxNumber" >
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Counter" HeaderText="Counter" 
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Counter" >
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Prize" HeaderText="Prize" 
                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Prize" >
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr><td></td></tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel id="PanRebate" runat="server" Width="578px">
                            <table style="width: 578px">
                            <tr id="trPromoRebate" runat="server" >
                                                
                                                <td><asp:LinkButton ID="linkRebateBinRange" runat="server" CssClass="action-link" 
                                                        Enabled="True" style="padding-right: 10px; margin-top: 0px;">Edit Bin Range</asp:LinkButton></td>
                            </tr>  
                            <tr id="trTPL_linkRebateBinRangeGrid" runat="server">
                                            <td style="height: 103px; text-align: center; background-color: transparent;">
                                            
                                            <div id="divRebateBinRange" runat="server" style="padding-right: 2px; padding-left: 2px; padding-bottom: 2px; overflow: auto;
                                                    width: 100%; padding-top: 2px; height: 250px; background-color: transparent" align="center">
                                                    
                                            <asp:GridView ID="gvRebateBinRange" runat="server" AutoGenerateColumns="False" 
                                                    BorderStyle="Solid" ShowHeaderWhenEmpty="True"
                                                BorderWidth="1px" CellPadding="4" ForeColor="#333333" Width="99%" 
                                                    AllowSorting="True" ShowHeader="True">
                                        

                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:BoundField DataField="ElementName" HeaderText="Transaction Type"
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="ElementName" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BIN" HeaderText="Bin" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Bin" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PanLow" HeaderText="Pan Low" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLow">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PanHigh" HeaderText="Pan High" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanHigh" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PanLength" HeaderText="Pan Length" 
                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLength" >
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#999999" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                                </div>
                              
                                            </td>                                           
                            </tr>
                            </table>
                            </asp:Panel>
                            </td>
                            
                        </tr>  
                        
                        <tr id="trPromoAnalytics" runat="server" visible="false">
                            <td style="width: 112px; height: 29px">
                                Analytics:</td>
                            <td class="field-cell" rowspan="1" style="width: 462px; height: 29px" valign="top">
                                
                                <asp:Panel ID="panPromoPlanCost" runat="server" HorizontalAlign="Left" Width="585px" Wrap="False" Visible="False">
                                   <table style="width: 580px">
                                       <tr>
                                           <td style="text-align: left; border-bottom: solid thin GrayText;" 
                                               valign="middle" colspan="2">
                                               <strong>Promo Plan and Cost</strong></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 173px; height: 20px; text-align: left;" valign="middle">
                                               Planned Promo Sales:</td>
                                           <td style="text-align: left">
                                               <asp:TextBox ID="txtPlanPromoSales" runat="server" onfocus="return PrepNumericTextbox(this);" onblur="return FormatPromoAnalyticsTextboxes();" MaxLength="15" Width="130px" CssClass="RightAligned"></asp:TextBox></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 173px; text-align: left; vertical-align: middle;" valign="middle">
                                               Planned Promo Cost:</td>
                                           <td style="text-align: left">
                                               <asp:TextBox ID="txtPlanPromoCost" runat="server" onfocus="return PrepNumericTextbox(this);" onblur="return FormatPromoAnalyticsTextboxes();" MaxLength="15" Width="130px" CssClass="RightAligned"></asp:TextBox><strong></strong></td>
                                       </tr>
                                       <tr>
                                           <td style="width: 173px; text-align: left; vertical-align: middle;" valign="middle">
                                               Planned Margin %:</td>
                                           <td style="text-align: left">
                                               <asp:TextBox ID="txtPlanMargin" runat="server" onfocus="return PrepNumericTextbox(this);" onblur="return FormatPromoAnalyticsTextboxes();" MaxLength="15" Width="106px" CssClass="RightAligned"></asp:TextBox>
                                               <strong>%</strong></td>
                                       </tr>
                                       <tr>
                                           <td colspan="2" style="text-align: left" valign="middle">
                                               &nbsp;</td>
                                       </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr id="trPromoAttachment" runat="server" visible="false">
                            <td style="width: 112px; height: 29px">
                                <span style="font-size: 10pt">
                                    <asp:Label ID="lblAttachment" runat="server" Text="Attachment:"></asp:Label></span></td>
                            <td class="field-cell" rowspan="1" style="width: 462px; height: 29px" valign="top">
                                <asp:ImageButton ID="imgbtnDownload" runat="server" BackColor="White" BorderColor="White"
                                    BorderStyle="Solid" ImageUrl="~/Images/down.gif" Visible="False" ToolTip="Download attached files " />
                                <asp:LinkButton ID="lnkattachment" runat="server" OnClientClick="return openAttachment();"
                                    Visible="False">Add Attachment(s)</asp:LinkButton><br />
                                <asp:Literal ID="lblFiles" runat="server"></asp:Literal>&nbsp;</td>
                        </tr>
                        <tr id="trPromoSeedAttachment" runat="server" visible="False">
                            <td style="width: 112px; height: 29px">
                                <span style="font-size: 10pt">
                                    <asp:Label ID="lblSeedAttachement" runat="server" Text="Seed Attachment:"></asp:Label></span></td>
                            <td class="field-cell" rowspan="1" style="width: 462px; height: 29px" valign="top">
                                <asp:ImageButton ID="imgbtnSeedDownload" runat="server" BackColor="White" BorderColor="White"
                                    BorderStyle="Solid" ImageUrl="~/Images/down.gif" Visible="False" ToolTip="Download attached files " />
                                <asp:LinkButton ID="linkSeedAttachment" runat="server"
                                    Visible="False">Add Seed Attachment(s)</asp:LinkButton><br />
                                <asp:Literal ID="lblSeedFile" runat="server"></asp:Literal>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 22px">
                                &nbsp;</td>
                            <td rowspan="1" valign="top" class="field-cell">
                                <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="432px">
                                </asp:BulletedList>
                                </td>
                        </tr>

                        <tr id='trPromoDetails' runat="server"> 
                            <td style="height: 4px" align="center" colspan="5" class="DocTabHeadOff">
                                <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                                    <asp:LinkButton ID="lnkPromoDetails" runat="server" Width="228px">Promo Details</asp:LinkButton></span></strong></td>
                        </tr>
                        <tr id='trBranches' runat="server">
                            <td colspan="5" rowspan="1" style="height: 4px; text-align: center"
                                valign="top" class="DocTabHeadOff">
                                <asp:LinkButton ID="lnkBranches" runat="server" Enabled="False" Width="230px">Branches</asp:LinkButton></td>
                        </tr>
                        
                    </table>
    </div>

    <asp:SqlDataSource ID="sqldsPromoType" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT * FROM PromoTypes WHERE (ForMPDuseOnly = 0) ORDER BY TypeDesc">
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sqldsPromoTypePerGroup" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT * FROM PromoTypes WHERE (ForMPDuseOnly = 0) AND GroupType = @GroupType ORDER BY TypeDesc">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidIsPTFilter" DefaultValue="0" Name="GroupType"
                PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>"></asp:SqlDataSource><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>"></asp:SqlDataSource>
    <asp:HiddenField ID="hidIsPTFilter" runat="server" />
    
    <div style="display:none">
        <asp:Button ID="btnDownload" runat="server" Text="Download" />
        <asp:Button ID="btnSeedDownload" runat="server" Text="Seed Download" />
        <asp:Button ID="cmdPopUpOK" runat="server" Text="" />
        <asp:Button ID="cmdCopyValueToMechanics" runat="server" Text="" />
        <asp:Button ID="cmdCopyValueToPromoNotes" runat="server" Text="" />
        <asp:Button ID="cmdCopyValueToBankList" runat="server" Text="" />
        <asp:Button ID="cmdCopyDatasource" runat="server" Text="" />    
        <asp:HiddenField ID="lblPopTitle" runat="server" />
    </div>
                                   
</asp:Content>
