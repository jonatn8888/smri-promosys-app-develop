<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"  CodeFile="MallSaleCreatePromo.aspx.vb" Inherits="MallSaleCreatePromo" title="Storewide Sale Entry" ValidateRequest="False" EnableEventValidation="false" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>

<script type="text/javascript">

    function java_msgbox(height,width)
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
  
</script>

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkSaveMemo" runat="server" OnClientClick="return confirm_action('Create memo draft?')">Create Memo Draft</asp:LinkButton><br />
        <br />
        <br />
        <br />
    </div>

    <div id="contents">
                    <table id="doc-table" cellpadding="3px" cellspacing="0px">
                        <tr>
                            <td colspan="2" class="DocTabHeadOn">
                                Event Participation Memo Draft</td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                &nbsp;</td>
                            <td style="height: 1px; width: 565px;" class="field-cell">
                                &nbsp;<asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="456px">
                                </asp:BulletedList>
                                </td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Request Date:</span></td>
                            <td style="width: 565px; height: 1px;"  class="field-cell">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">
                                    <asp:Label ID="lblReqDate" runat="server" Width="204px"></asp:Label></span><span style="font-size: 10pt; font-family: Trebuchet MS"></span></td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                &nbsp;</td>
                            <td style="height: 1px; width: 565px;" class="field-cell">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 4px;">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Promo Title:</span></td>
                            <td style="height: 4px; width: 565px;" class="field-cell">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">
                                    <asp:TextBox ID="txtReqTitle" runat="server" Width="484px" MaxLength="100" CssClass="Tb_ToUpper"></asp:TextBox></span></td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 11px">
                                <span style="font-size: 10pt; font-family: Trebuchet MS">Promo Period:</span></td>
                            <td style="width: 565px; height: 11px"  class="field-cell">
                                <asp:TextBox ID="txtPeriodFrom" runat="server" Width="177px" MaxLength="50"></asp:TextBox>
                                <input name="calPeriodFrom" type="button" id="calPeriodFrom" class="btnCal" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodFrom');" style="left: 322px; top: 345px" />&nbsp;
                                <asp:TextBox ID="txtPeriodTo" runat="server" Width="177px" MaxLength="50"></asp:TextBox>
                                <input name="calPeriodTo" type="button" id="calPeriodTo" class="btnCal" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodTo');" style="left: 547px; top: 345px"/></td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px">
                            </td>
                            <td class="field-cell" style="width: 565px; height: 1px; text-align: left">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px;">
                                Business Unit:</td>
                            <td style="height: 1px; width: 565px; text-align: left;"  class="field-cell">
                                <asp:DropDownList ID="cboUserGroups" runat="server" AppendDataBoundItems="True" DataTextField="FullDesc" DataValueField="GroupID" Width="294px">
                                    <asp:ListItem Selected="True" Value="-1">- Select Business Unit -</asp:ListItem>
                                </asp:DropDownList>&nbsp;
                                <input id="cmdAddBizUnit" runat="server" style="width: 68px; height: 24px" type="button"
                                    value="Add" /><br />
                                &nbsp;&nbsp;<br />
                                <asp:GridView ID="gridBusinessUnit" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" Height="14px" Width="99%">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllRows" runat="server" AutoPostBack="True" MyTag="OnCheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRowSel" runat="server" MyTag="OnCheckedChanged" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5px" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Code" DataField="GroupID">
                                            <ControlStyle CssClass="HiddenObject" />
                                            <HeaderStyle CssClass="HiddenObject" />
                                            <ItemStyle CssClass="HiddenObject" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Business Unit" DataField="Description">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        *** No Business Unit Selected ***
                                    </EmptyDataTemplate>
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                                <asp:LinkButton ID="lnkDeleteBizUnit" runat="server" CssClass="action-link" Visible="False"
                                    Width="112px">Delete Selected</asp:LinkButton></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 21px; text-align: center" class="DocTabHeadOn">
                                Branches</td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px">
                                Branches:</td>
                            <td class="field-cell" style="width: 565px; height: 1px; text-align: left">
                                <table cellspacing="0">
                                    <tr>
                                        <td style="width: 20px">
                                            &nbsp;</td>
                                        <td colspan="2">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20px; height: 14px; text-align: right">
                                            <span style="font-size: 10pt; font-family: Trebuchet MS">Company:</span></td>
                                        <td colspan="2" style="height: 14px; text-align: left">
                                            <asp:DropDownList ID="cboEnvironment" runat="server" AppendDataBoundItems="True"
                                                DataTextField="EnvDescription" DataValueField="EnvCode" Width="298px">
                                                <asp:ListItem Selected="True" Value="-1">- Select Company -</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="cmdSelectBranch" runat="server" Height="21px" Text="Display List"
                                                Width="131px" /></td>
                                    </tr>
                                    <tr>
                                        <td align="left" background="#ffffff" colspan="3" style="height: 14px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"><asp:GridView ID="gridBranches" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" Height="14px" Width="99%">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAllBranchRows" runat="server" AutoPostBack="True" MyTag="OnCheckedChanged" OnCheckedChanged="chkSelectAllBranchRows_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRowSel" runat="server" MyTag="OnCheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5px" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Comp" DataField="CompCode">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BranchCode" HeaderText="Branch">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Store Name" DataField="BranchName">
                                                    <ItemStyle Wrap="False" />
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                            </Columns>
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <EmptyDataTemplate>
                                                No participating store selected.
                                            </EmptyDataTemplate>
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3" style="height: 37px">
                                            &nbsp;
                                            <asp:LinkButton ID="lnkRemoveBranch" runat="server" CssClass="action-link" Visible="False">Delete Selected</asp:LinkButton></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 112px; height: 1px">
                            </td>
                            <td class="field-cell" style="width: 565px; height: 1px; text-align: left">
                                </td>
                        </tr>
                    </table>
        </div>
    <asp:HiddenField ID="lblPopTitle" runat="server" />

    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>" SelectCommand="SELECT GroupID, BizUnit+' - '+Description AS FullDesc&#13;&#10;FROM UserGroups&#13;&#10;WHERE BizUnit IS NOT NULL AND Department IS NULL AND GroupType = 'REGULAR'&#13;&#10;ORDER BY BizUnit"></asp:SqlDataSource>

    <div style="display:none">
        <input id="btnProcess" type="button" runat="server"  value="button" />
    </div>
    
</asp:Content>

