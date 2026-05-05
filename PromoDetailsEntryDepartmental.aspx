<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"  CodeFile="PromoDetailsEntryDepartmental.aspx.vb" Inherits="PromoDetailsEntryDepartmental" title="Promotion Information" validateRequest=false  %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
function openSearch()
{
    var url
    url = "SearchDepSdep.aspx";
	itemsearchwindow=dhtmlmodal.open('EmailBox', 'iframe', url, 'Search ITEM Code', 'width=725px,height=280px,center=1,resize=0,scrolling=0',"recall")
    itemsearchwindow.onclose=function()
    { 
        var theform = this.contentDoc.forms[0] 
	    var thedepcode = this.contentDoc.getElementById("txtDepCode") 
        var theSubDep = this.contentDoc.getElementById("txtSdepCode") 
        var theClass = this.contentDoc.getElementById("txtClassCode") 
        var theSubClass = this.contentDoc.getElementById("txtSubClassCode")
        var theDescription = this.contentDoc.getElementById("txtDescription") 
        var theShortDesc = this.contentDoc.getElementById("hidShortDesc")   		
	    
	    document.getElementById("ctl00$ContentPlaceHolder1$txtDep").value = thedepcode.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtSubDp").value = theSubDep.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtClass").value = theClass.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$txtSubClass").value = theSubClass.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$lblItemDesc").value = theDescription.value; 
	    document.getElementById("ctl00$ContentPlaceHolder1$hfShortDesc").value = theShortDesc.value; 
	    
	    document.getElementById("<%=Button1.ClientID%>").click();
	    
	    return true 
	}
}
</script>

<script type="text/javascript">
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
</script>
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
</script>

   <div id="contents">
 
        <script src="rte/richtext.js" type="text/javascript"></script>
        <script src="rte/config.js" type="text/javascript"></script>
        <asp:HiddenField ID="hidBox" runat="server" />   
        <br /> 

        <table cellpadding="0" cellspacing="0" id="doc-table" style="width: 641px">
            <tr>
                <td style="height: 22px; text-align: center; width: 633px;" class="DocTabHeadOff">
                    <span style="font-size: 11pt; color: lightslategray; font-family: Trebuchet MS"><strong>
                        &nbsp;<asp:LinkButton ID="lnkRequest" runat="server" Width="230px">Sales Promotion Request</asp:LinkButton></strong></span></td>
            </tr>
            <tr>
                <td style="height: 22px; text-align: center; width: 633px;" class="DocTabHeadOn">
                    <span style="font-size: 11pt; color: #ffffff; font-family: Trebuchet MS"><strong>&nbsp;Promotion
                        Information</strong></span></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 633px;" valign="top" align="center">

                    <table style="width: 716px; height: 313px;" id="Table1">
                       <tr><td style="width: 700px; height: 6px;"></td>
                           <td  class="field-cell" style="width: 19px; height: 6px; text-align: center;">
                               <span style="font-size: 9pt"><strong>Dp</strong></span></td>
                           <td  class="field-cell" style="height: 6px; text-align: center; width: 27px;" >
                               <span style="font-size: 9pt"><strong>SDp</strong></span></td>
                           <td  class="field-cell" style="width: 10px; height: 6px; text-align: center;" >
                               <span style="font-size: 9pt"><strong>Cl</strong></span></td>
                           <td class="field-cell" style="width: 10px; height: 6px; text-align: center">
                               <span style="font-size: 9pt"><strong>SCl</strong></span></td>
                           <td  class="field-cell" style="width: 107px; height: 6px; text-align: center" >
                               <span style="font-size: 9pt"><strong>Item Description</strong></span></td>
                           <td  class="field-cell" style="height: 6px; text-align: center"  colspan="2">
                               <span style="font-size: 9pt"></span></td>
                       </tr>
                        <tr>
                            <td  class="field-row" style="height: 14px; width: 700px;" align="left">
                                <strong>DP-SD-CL:</strong></td>
                            <td  class="field-cell" style="width: 19px; height: 14px" >
                                <asp:TextBox ID="txtDep" runat="server" Height="20px" Width="25px" MaxLength="3" ToolTip="Click Search Button" ForeColor="Black" BackColor="White"></asp:TextBox></td>
                            <td  class="field-cell" style="height: 14px; text-align: center; width: 27px;"  align="center">
                                <asp:TextBox ID="txtSubDp" runat="server" Height="20px" Width="25px" MaxLength="3" ToolTip="Click Search Button" ForeColor="Black" BackColor="White"></asp:TextBox></td>
                            <td  class="field-cell" style="width: 10px; height: 14px; text-align: center;" >
                                <asp:TextBox ID="txtClass" runat="server" Height="20px" Width="25px" MaxLength="3" ToolTip="Click Search Button" ForeColor="Black" BackColor="White"></asp:TextBox></td>
                            <td class="field-cell" style="width: 10px; height: 14px; text-align: center">
                                <asp:TextBox ID="txtSubClass" runat="server" BackColor="White" ForeColor="Black" Height="20px"
                                    MaxLength="3" ToolTip="Click Search Button" Width="25px"></asp:TextBox></td>
                            <td  class="field-cell" style="width: 107px; height: 14px; text-align: center" >
                                <asp:TextBox ID="lblItemDesc" runat="server" Height="20px" Width="263px" ToolTip="Click Search Button" ForeColor="Black" BackColor="AliceBlue" BorderColor="White"></asp:TextBox></td>
                            <td  class="field-cell" style="width: 58px; height: 14px; text-align: center" >
                                <input runat="server" id="btnSeacrh" type="button"  value="Search" style="width: 68px; height: 24px" /></td>
                            <td  class="field-row" style="width: 59px; height: 14px; text-align: center" >
                                <asp:Button ID="cmAdd" runat="server" Text="Clear" Height="25px" Width="65px" /></td>
                        </tr>
                        <tr>
                            <td align="left" style="height: 8px; width: 700px;">
                                <strong>Discount:</strong></td>
                            <td  class="field-cell"  style="width: 19px; height: 8px">
                                <asp:TextBox ID="txtDiscount" runat="server" Height="20px" MaxLength="3"
                                    Width="25px"></asp:TextBox></td>
                            <td  class="field-cell" align="left"  colspan="6" style="height: 8px; text-align: left">
                                % Discount on all regular-priced items.&nbsp;
                                <asp:Label ID="Label4" runat="server" Font-Size="8pt" ForeColor="Blue"></asp:Label></td>
                        </tr>
                        <tr>
                            <td  class="field-row" align="left" style="height: 6px; width: 700px;">
                                <strong>Markdown:</strong></td>
                            <td align="left" class="field-cell" colspan="7" valign="top">
                                <asp:TextBox ID="txtMDFrom" runat="server" Height="20px" MaxLength="20"
                                    Width="94px"></asp:TextBox>% Markdown on selected items.
                                <asp:Label ID="Label5" runat="server" Font-Size="8pt" ForeColor="Blue"></asp:Label></td>
                        </tr>
                        <tr>
                            <td  class="field-row" align="left" style="height: 13px; width: 700px;">
                                <strong>Others Promo Type:</strong></td>
                            <td  class="field-cell" align="left"  colspan="7" style="height: 13px" valign="middle">
                                <asp:DropDownList ID="cboPromoType" runat="server" Height="22px" Width="370px" AutoPostBack="True">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td  class="field-row" align="left" style="height: 13px; width: 700px;" valign="top">
                                <strong>Mechanics:</strong></td>
                            <td  class="field-cell" align="left"  colspan="7" style="height: 13px">
                            <div  style="width: 514px; height: 107px; overflow: auto; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke;">
                                <asp:Literal ID="litMechanics" runat="server"></asp:Literal>
                            </div>
                                <asp:LinkButton ID="lnkEditMechanics" runat="server" CssClass="action-link" Enabled="False">Edit Mechanics</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="field-row" style="width: 700px; height: 13px" valign="top">
                                <strong>Attachment:</strong></td>
                            <td align="left" class="field-cell" colspan="7" style="height: 13px">
                    <asp:ImageButton ID="imgbtnDownload" runat="server" ImageUrl="~/Images/down.gif"
                        Visible="False" BorderColor="White" BorderStyle="Solid" BackColor="White" ToolTip="Download attached files" />
                    <asp:LinkButton OnClientClick="return openAttachment();" ID="lnkattachment" runat="server" Visible="False">Add Attachment(s)</asp:LinkButton><br />
                                <asp:Literal ID="lblFiles" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td align="left" class="field-row" style="width: 700px; height: 13px" valign="top">
                                &nbsp;</td>
                            <td align="right" class="field-cell" colspan="7" style="height: 13px">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="60px" /></td>
                        </tr>
                    </table>
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="494px">
                    </asp:BulletedList>
                    <asp:GridView ID="gvPromoDetails" runat="server" AutoGenerateColumns="False" Width="714px" CellPadding="4" Font-Size="8pt" ForeColor="#333333" GridLines="None" AllowPaging="True" PageSize="5">
               
                        <Columns>

                            <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack ="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                            </HeaderTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    &nbsp;
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandArgument='<%# eval("PromoID") %>'
                                        CommandName="Edit" Text="Edit"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="10px" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("PromoID") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    &nbsp;<asp:Label ID="Label3" runat="server" Text='<%# eval("PromoID") %>'></asp:Label>
                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# eval("ItemCode") %>'></asp:Label>
                                    <asp:Label ID="lblItemDesc" runat="server" Text='<%# eval("PromoDesc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dp/SDp/Cl/SCl">
                                <ItemTemplate>
                                    <asp:Literal ID="LitItemCode" runat="server" Text='<%# eval("ItemCode") %>'></asp:Literal>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item Description">
                                <ItemTemplate>
                                    <asp:Literal ID="Literal2" runat="server" Text='<%# eval("PromoDesc") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From" Visible="False">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("PeriodFrom","{0:d}") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("PeriodFrom","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To" Visible="False">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server"   Text='<%# Bind("PeriodTo","{0:d}") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server"   Text='<%# Bind("PeriodTo","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    <br />
                    <table style="width: 709px" id="tblDelete" runat="server">
                        <tr>
                            <td style="width: 22px; text-align: left">
                                &nbsp;<asp:LinkButton ID="cmdDelete" runat="server" Width="142px">Delete Selected</asp:LinkButton></td>
                            <td style="width: 19px"></td>
                            <td style="width: 43px"></td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
                <td style="height: 21px; text-align: center; width: 633px;"
                    valign="top" class="DocTabHeadOff">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">&nbsp;<asp:LinkButton ID="lnkBranches" runat="server" Width="230px">Branches</asp:LinkButton></span></strong></td>
            </tr>
        </table>
       <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
           SelectCommand="select 1"></asp:SqlDataSource>
        <asp:HiddenField ID="hidPromoTypeID" runat="server" />
        <asp:HiddenField ID="hfShortDesc" runat="server" />
                    <asp:SqlDataSource ID="sqldsPromoType" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                        SelectCommand="USP_SelectAllPromoType" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:SessionParameter DefaultValue="0" Name="RequestID" SessionField="CurrRequestID"
                                Type="Int32" />
                            <asp:ControlParameter ControlID="txtDep" DefaultValue="0" Name="DepCode" PropertyName="Text"
                                Type="Int16" />
                            <asp:ControlParameter ControlID="txtSubDp" DefaultValue="0" Name="SubDepCode" PropertyName="Text"
                                Type="Int16" />
                            <asp:ControlParameter ControlID="txtClass" DefaultValue="0" Name="ClassCode" PropertyName="Text"
                                Type="Int16" />
                        </SelectParameters>
                    </asp:SqlDataSource>
    </div>

        

    <div style="display:none;">
      <asp:Button ID="Button1" runat="server" Text="Button"></asp:Button>
     <asp:Button ID="btnDownload" runat="server" Text="Button" />
        <asp:Button ID="Button2" runat="server" Text="Button" /></div>
    

</asp:Content>

