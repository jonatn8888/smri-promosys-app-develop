<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="PromoMemoEntry.aspx.vb" Inherits="PromoMemoEntry" Title ="Promotions Memo Entry" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>

<script src="./rte/richtext.js" type="text/javascript" language="javascript"></script>
<script src="./rte/config.js" type="text/javascript" language="javascript"></script>
   
<script type="text/javascript">

function AllowDecimalOnly(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode;
    
    if (charCode != 46 && charCode > 31 
        && (charCode < 48 || charCode > 57))
         return false;
    
    return true;
}

function msgbox(height,width)
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
    document.getElementById("<%=btnProcess.ClientID%>").click(); 
	return true 
	}
}

    //Added dowcarpio08282012@smretailinc: To display selected branches.
    function OpenViewBranches(height,width)
    {
        if (height == "") height= "250";
       
        if (width == "") width= "650";
        
        var title
        var page
        page = "PromoRequestBranches.aspx";
        title = "Branches";
        Auditwindow=dhtmlmodal.open('Branches', 'iframe', page, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
        Auditwindow.onclose=function()
        { 
	    return true 
	    }
	}
	
</script>

   
<script type="text/javascript">
function opentexteditor(sActionButton, sMsgTitle)
{
    var url
    url = "texteditor.aspx";
	texteditorwindow=dhtmlmodal.open('texteditor', 'iframe', url, sMsgTitle, 'width=709px,height=500px,center=1,resize=0,scrolling=0','recall')
    texteditorwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    var Mechanics = this.contentDoc.getElementById('HiddenField1') 
    document.getElementById('<%= hidBox.ClientID %>').value = Mechanics.value;
    document.getElementById(sActionButton).click(); 
	return true
	}
}
</script>   
   
<script language="javascript" type="text/javascript">
<!--

function cmdAddPromo_onclick() {
  var WinSettings = "center:yes;resizable:no;dialogHeight:500px;dialogwidth:700px;status:no";

  var MyArgs = window.showModalDialog("PromoEntry.aspx", 0, WinSettings);
}

function cmdSave_onclick() {
    window.alert("Promo Request saved and forwarded for approval.");
}

function check_selection()
{   
    if(document.getElementById('ctl00_ContentPlaceHolder1_cboPromoType').selectedIndex == 0)
    {
        alert("Please select a guideline to add.");
        document.getElementById('ctl00_ContentPlaceHolder1_cboPromoType').focus();
        return false;
    }
    else
    {
        return confirm('Insert selected guideline?');
    }
}

function confirm_action(msg)
{
    return confirm(msg);
}

// -->
</script>
    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton
            ID="lnkSaveMemo" runat="server" CssClass="nav-link">Submit Memo Draft</asp:LinkButton><br />
        <br />
        <br />
        <asp:LinkButton ID="lnkDone" runat="server" CssClass="nav-link">Back to Preview</asp:LinkButton><br />
    </div>
    
    <div id="contents">
       <table id="doc-table" cellpadding="3px" cellspacing="0px">
            <tr>
                <td colspan="3" class="DocTabHeadOn">
                    <strong><span style="font-size: 11pt; color: white">Promotion Memo Draft</span></strong></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    &nbsp;</td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Memo ID:</span></td>
                <td colspan="2" class="field-cell" >
                <asp:Label ID="lblMemoID"
                                    runat="server" Width="182px" Font-Bold="True" Font-Italic="True">.: New Entry :.</asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Memo Date:</span></td>
                <td  colspan="2" class="field-cell">
                                    <asp:Label ID="lblMemoDate" runat="server" Width="185px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Request ID:</span></td>
                <td  colspan="2" class="field-cell">
                                    <asp:Label ID="lblRequestID" runat="server" Width="87px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 51px;">&nbsp;</td>
                <td  colspan="2" class="field-cell" style="height: 51px; padding-top: 5px; padding-bottom: 0px">
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="432px">
                    </asp:BulletedList>
                </td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Title:</span></td>
                <td  colspan="2" class="field-cell">
                                    <asp:TextBox ID="txtPromoTitle" runat="server" Width="555px" CssClass="Tb_ToUpper"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 22px">
                    <span style="font-size: 10pt">Promo Period:</span></td>
                <td  colspan="2" style="height: 22px" class="field-cell">
                    <asp:TextBox ID="txtPeriodFrom" runat="server" Width="178px" ReadOnly="True"></asp:TextBox>
                    <input id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodFrom');"
                        style="left: 322px; top: 345px" type="button" disabled="disabled" runat="server" />
                    &nbsp;&nbsp;
                    <asp:TextBox ID="txtPeriodTo" runat="server" Width="178px" ReadOnly="True"></asp:TextBox>
                    <input id="calPeriodTo" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodTo');"
                        style="left: 322px; top: 345px" type="button" disabled="disabled" runat="server"/></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 13px">
                    &nbsp;</td>
                <td  colspan="2" style="height: 13px" class="field-cell">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Branches:</span></td>
                <td  colspan="2" class="field-cell">
                                    <asp:Label ID="lblBranches" runat="server"
                                        Height="55px" Width="583px"></asp:Label><br />
                    <asp:LinkButton ID="lnkEditBranch" runat="server" CssClass="action-link">view complete list</asp:LinkButton></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Promotions:</span></td>
                <td  colspan="2" class="field-cell">
                                    <asp:GridView ID="gridPromotions" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                        BorderWidth="2px" CellPadding="4" DataKeyNames="PromoID" DataSourceID="sqldsPromos"
                                        ForeColor="#333333" Width="580px">
                                        <RowStyle BackColor="WhiteSmoke" ForeColor="#333333" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRowSel" runat="server" Width="10px"/>
                                                </ItemTemplate>
                                                <ItemStyle Width="5px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl" ReadOnly="True" SortExpression="ItemCode" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PromoDesc" HeaderText="Promo Description" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPromoID" runat="server" Text='<%# Eval("PromoID") %>' Width="10px"></asp:Label>
                                                    <asp:Label ID="lblPromoTypeID" runat="server" Text='<%# Eval("PromoTypeID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            &nbsp;
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#999999" />
                                    </asp:GridView>
                                    
                    <asp:LinkButton ID="lnkAddPromo" runat="server" CssClass="action-link" Width="60px" Visible="False">Add Promo</asp:LinkButton>
                    <asp:LinkButton ID="lnkEditPromo" runat="server" CssClass="action-link" Width="109px">Edit Promo Detail</asp:LinkButton></td>
            </tr>
            
           <tr runat="server" id="tblrowSMACDetails">
               <td style="width: 103px; height: 213px;">
                   Specific Details:</td>
               <td class="field-cell" colspan="2" style="height: 200px">
                   <asp:Panel ID="panSMACdetails" runat="server" Visible="False" Width="500px">
                       <table>
                           <tr>
                               <td style="width: 142px" valign="middle">
                                   Merchandise Group:</td>
                               <td colspan="2">
                                   <asp:DropDownList ID="cboMerchGroup" runat="server" Width="316px">
                                       <asp:ListItem>- - Select Value - -</asp:ListItem>
                                       <asp:ListItem Value="DSRA">DSRA - Department Store and Retail Affiliates</asp:ListItem>
                                       <asp:ListItem Value="DSDS">DSDS - Department Store Only</asp:ListItem>
                                       <asp:ListItem Value="RARA">RARA - Retail Affiliates Only</asp:ListItem>
                                   </asp:DropDownList>&nbsp;</td>
                           </tr>
                           <tr>
                               <td style="width: 142px" valign="middle">
                                   Business Unit:</td>
                               <td colspan="2">
                                   <asp:DropDownList ID="cboBusinessUnit" runat="server" Width="136px">
                                       <asp:ListItem>- - Select Value - -</asp:ListItem>
                                       <asp:ListItem Value="AMC">AMC</asp:ListItem>
                                       <asp:ListItem Value="CFMC">CFMC</asp:ListItem>
                                       <asp:ListItem Value="LFMC">LFMC</asp:ListItem>
                                       <asp:ListItem Value="LTBG">LTBG</asp:ListItem>
                                       <asp:ListItem Value="MFMC">MFMC</asp:ListItem>
                                       <asp:ListItem Value="MISC">MISC</asp:ListItem>
                                       <asp:ListItem Value="SCP">SCP</asp:ListItem>
                                       <asp:ListItem Value="SLI">SLI</asp:ListItem>
                                       <asp:ListItem Value="0HWP">0HWP</asp:ListItem>
                                       <asp:ListItem Value="0KSP">0KSP</asp:ListItem>
                                       <asp:ListItem Value="0NCP">0NCP</asp:ListItem>
                                       <asp:ListItem Value="0SAP">0SAP</asp:ListItem>
                                       <asp:ListItem Value="0SMP">0SMP</asp:ListItem>
                                       <asp:ListItem Value="0SSP">0SSP</asp:ListItem>
                                       <asp:ListItem Value="0TWP">0TWP</asp:ListItem>
                                       <asp:ListItem>0WAP</asp:ListItem>
                                       <asp:ListItem Value="0WAT">0WAT</asp:ListItem>
                                   </asp:DropDownList></td>
                           </tr>
                           <tr>
                               <td style="width: 142px" valign="middle">
                                   Company to Shoulder:</td>
                               <td colspan="2">
                                   <asp:DropDownList ID="cboCompSponsorship" runat="server" Width="273px">
                                       <asp:ListItem Value="0">- - Select Value - -</asp:ListItem>
                                       <asp:ListItem Value="1">1 - Corporate</asp:ListItem>
                                       <asp:ListItem Value="2">2 - For Billing to Sponsor</asp:ListItem>
                                       <asp:ListItem Value="3">3 - Shared with Sponsor</asp:ListItem>
                                   </asp:DropDownList></td>
                           </tr>
                           <tr>
                               <td style="width: 142px; height: 24px" valign="middle">
                                   Vendor Code:</td>
                               <td colspan="2" style="height: 24px">
                                   <asp:TextBox ID="txtVendorCode" runat="server" MaxLength="6" Width="108px"></asp:TextBox></td>
                           </tr>
                           <tr runat="server" visible="false">
                               <td style="width: 142px" valign="middle">
                                   Barcode:</td>
                               <td colspan="2">
                                   <asp:TextBox ID="txtBarcode" runat="server" MaxLength="13" Width="181px"></asp:TextBox></td>
                           </tr>
                           <tr runat="server" visible="false">
                               <td style="width: 142px; height: 20px" valign="middle">
                                   Discount:</td>
                               <td style="width: 75px; height: 20px">
                                   <asp:TextBox ID="txtPercentDisc" runat="server" MaxLength="3" Width="43px"></asp:TextBox>
                                   <strong>
                                   %</strong></td>
                               <td style="height: 20px">
                                   <strong>Php</strong>
                                   <asp:TextBox ID="txtDiscAmount" runat="server" MaxLength="8" Width="84px"></asp:TextBox></td>
                           </tr>
                           <tr runat="server" visible="false">
                               <td style="width: 142px; height: 20px" valign="middle">
                               </td>
                               <td style="width: 75px; height: 20px">
                                   <asp:Label ID="lblPromoEventCode" runat="server" Font-Bold="True" Text="0000" Width="54px" Visible="False"></asp:Label></td>
                               <td style="height: 20px">
                       <asp:Label ID="lblDiscountAmount" runat="server" Font-Bold="True" Text="0000" Width="54px" Visible="False"></asp:Label></td>
                           </tr>
                       </table>
                       </asp:Panel>
               </td>
           </tr>

            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Guidelines:</span></td>
                <td colspan="2" class="field-cell">
                    <div  style="width: 570px; height: 170px; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke;">
                    <asp:Literal ID="litGuidelines" runat="server"></asp:Literal></div>
                
                    <asp:DropDownList ID="cboPromoType" runat="server" DataSourceID="sqldsPromoType"
                        DataTextField="TypeDesc" DataValueField="PromoTypeID" Width="238px" AppendDataBoundItems="True">
                        <asp:ListItem Selected="True" Value="-1">- Select Promo Type -</asp:ListItem>
                    </asp:DropDownList>
              
                    <asp:LinkButton ID="lnkEditGuidelines" runat="server" CssClass="action-link" Width="78px">Edit Guidelines</asp:LinkButton>
                    <asp:LinkButton ID="lnkInsertGuidelines" runat="server" CssClass="action-link" Width="126px" OnClientClick="return check_selection()">Insert Guideline</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <span style="font-size: 10pt">Remarks:</span></td>
                <td  colspan="2" class="field-cell">
                                <asp:Label ID="lblRemarks" runat="server" BorderWidth="0px"
                                    Width="563px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    &nbsp;</td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
        </table>
                    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                        ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="sqldsPromos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, P.PromoDesc, P.PromoTypeID&#13;&#10;FROM Promotions AS P left outer join  PromoDetails AS D ON P.PromoID = D.PromoID&#13;&#10;WHERE P.RequestID = @RequestID&#13;&#10;ORDER BY P.PromoID" UpdateCommand="UPDATE Promotions SET MemoID = @MemoID WHERE (RequestID = @RequestID)">
                                        <SelectParameters>
                                            <asp:Parameter Name="RequestID" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter DefaultValue="0" Name="MemoID" />
                                            <asp:SessionParameter Name="RequestID" SessionField="CurrRequestID" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>

                    <asp:SqlDataSource ID="sqldsPromoType" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                        SelectCommand="SELECT * FROM PromoTypes ORDER BY TypeDesc"></asp:SqlDataSource>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
                             
        <br />
        <asp:HiddenField ID="hidIsPTFilter" runat="server" />
    <br />
        <asp:HiddenField ID="hidBox" runat="server" />
    </div>

    <div style="display:none;">
        <asp:Button ID="cmdGuidelines" runat="server" Text="Update Guidelines" />
        <asp:Button ID="cmdMechanics" runat="server" Text="Update Mechanics" />
        <asp:Button ID="btnProcess" runat="server" Text="Process Button" />
    </div>
    
</asp:Content>