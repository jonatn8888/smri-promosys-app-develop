<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoBranches.aspx.vb" Inherits="PromoBranches" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
<script type="text/javascript">
function openBranches(Environment,height,width)
{
    if (height == "") height= "400";
    
    if (width == "") width= "680";
    
    var url
    var title
    url = "SelectBranches.aspx?EnvCode=" + Environment;
    title = "Select Branches"; 
    Branchwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    Branchwindow.onclose=function()
    { 

    document.getElementById("<%=cmdLoadForm.ClientID%>").click(); 
    var theform = this.contentDoc.forms[0] 
	return true 
	}
}
</script>

<script type="text/javascript">
function opentexteditor(height,width)
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

       function Filter(Obj) {
 
            var grid = document.getElementById('ctl00_ContentPlaceHolder1_gridBranches');
            var terms = Obj.value.toUpperCase();
            //var cellNr = 3; //your grid cellindex like name
            var ele;
            if (grid.rows.length > 1) {
            for (var r = 0; r < grid.rows.length; r++) {
                ele = grid.rows[r].cells[1].innerHTML.replace(/<[^>]+>/g, "");
                if (ele.toUpperCase().indexOf(terms) >= 0)
                    grid.rows[r].style.display = '';
                else grid.rows[r].style.display = 'none';
            
            }
            }
        }
        
        
    function openMissingSeeding(height,width)
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
        document.getElementById("<%=cmdMissingSeeding.ClientID%>").click(); 
	    return true 
	}
}
        
</script>
    <br />
    <br />   
     
    <div id="menu">
        <br />
        <br />
        <br />
        <asp:LinkButton ID="lnkSaveMemo" runat="server">View Request Preview</asp:LinkButton>
        <br />
        <br />
        <br />
    </div>
    
   <div id="contents">
        <table id="doc-table">
            <tr>
                <td class="DocTabHeadOff" style="height: 15px; text-align: center; width: 696px;">
                    <span style="font-size: 11pt; color: lightslategray; font-family: Trebuchet MS"><strong>
                        <asp:LinkButton ID="lnkRequest" runat="server" Width="230px">Sales Promotion Request</asp:LinkButton></strong></span></td>
            </tr>
            <tr style="font-size: 9pt">
                <td align="center" style="height: 21px; width: 696px;" class="DocTabHeadOff">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkPromoInfo" runat="server" Width="228px">Promotion Information</asp:LinkButton></span></strong></td>
            </tr>
            <tr>
                <td id="tdPromoDetails" runat="server" style="height: 21px; text-align: center; width: 696px;" valign="top" class="DocTabHeadOff">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkPromoDetails" runat="server" Width="230px">Promo Details</asp:LinkButton></span></strong></td>
            </tr>
            <tr>
                <td style="height: 22px; text-align: center; width: 696px;" class="DocTabHeadOn">
                    <span style="font-size: 11pt; color: #ffffff; font-family: Trebuchet MS"><strong>Branches</strong></span></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 696px;" valign="top" class="field-cell">
                    &nbsp;&nbsp;
        
        <table cellspacing="0">
            <tr>
                <td style="width: 20px;">
                    &nbsp;</td>
                <td colspan="2" >
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="height: 14px; width: 20px; text-align: right;">
                    <span style="font-size: 10pt; font-family: Trebuchet MS">Group:</span></td>
                <td colspan="2" style="height: 14px; text-align: left;">
                    <asp:DropDownList ID="cboCompany" runat="server" AppendDataBoundItems="True" DataTextField="EnvName" DataValueField="EnvCode"
                        Width="298px">
                        <asp:ListItem Selected="True" Value="-1">- Select Company -</asp:ListItem>
                    </asp:DropDownList>
                    
                    <asp:Button ID="cmdSelectBranch" runat="server" Text="Select From List" Width="131px" Height="21px" /></td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
                    <asp:Button ID="cmdAllBranches" runat="server" Text="All Branches" Width="122px" Height="21px" Visible="False" /></td>
            </tr>
            <tr id="trFindBranch" runat="server">
                <td style="width: 20px; height: 24px; text-align: left;">
                    <span style="font-size: 10pt; font-family: Trebuchet MS">Find:</span></td>
                <td align="left" colspan="2" style="height: 24px" >
                    <asp:TextBox ID="txtSearchValue" runat="server" onkeyup="Filter(this);" Width="289px"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" style="height: 14px" background="#ffffff" colspan="3">
                    &nbsp; &nbsp;
<%--                    <table style="width: 100%">
                        <tr>
                            <td bgcolor="#5d7b9d" style="width: 11%; height: 25px; padding-left: 13px;">
                                <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" EnableTheming="True"
                                    OnCheckedChanged="chkALL_CheckedChanged" Font-Bold="True" ForeColor="White" Text="All" /></td>
                            <td bgcolor="#5d7b9d" nowrap="nowrap" style="width: 95%; height: 25px; vertical-align: middle; text-align: center;">
                                <strong><span style="color: #ffffff">Selected Branches</span></strong></td>
                        </tr>
                    </table>--%>
                    <strong><span style="color: #ffffff"></span></strong>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 103px; text-align: center;">
                <div style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px; overflow: auto;
                        width: 534px; padding-top: 10px; height: 172px; background-color: whitesmoke" align="left">
                    <asp:GridView ID="gridBranches" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                        BorderWidth="1px" CellPadding="4" ForeColor="#333333" Width="101%" 
                        AllowSorting="True">
                        
                        <EmptyDataTemplate>
                            <b>No Selected Branch</b>
                        </EmptyDataTemplate>

                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="45px" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkALL" runat="server" OnCheckedChanged="chkALL_CheckedChanged" AutoPostBack="True" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblCompCode" runat="server" Width="91px" Text='<%# Eval("CompCode") %>'></asp:Label><asp:Label ID="lblBranchCode"
                                        runat="server" Text='<%# Eval("BranchCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CompCode" HeaderText="CmpCode" SortExpression="CompCode" Visible="False">
                                <ItemStyle Width="10px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BranchCode" HeaderText="BrCode" SortExpression="BranchCode" Visible="False">
                                <ItemStyle Width="100px" HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ShortName" HeaderText="Selected Branches" SortExpression="ShortName" >
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StoreGroupName" HeaderText="Group" SortExpression="StoreGroupName" >
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
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
            <tr>
                <td align="left" colspan="3" style="height: 37px">
                    &nbsp;
                    <asp:LinkButton ID="lnkRemoveBranch" runat="server" CssClass="action-link">Delete Selected</asp:LinkButton></td>
            </tr>
        </table>
                    &nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td style="height: 21px; text-align: center; width: 696px;"
                    valign="top" class="DocTabHeadOff">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkPreview" runat="server" Width="230px">Preview Document</asp:LinkButton></span></strong></td>
            </tr>
            <tr>
                <td style="text-align: center; height: 10px; width: 696px;" valign="top">
                    &nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
                    <br />
    
    </div>
    &nbsp;
        <asp:SqlDataSource ID="sqldsPromoBranches" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT pb.*,rlv.ElementName as StoreGroupName &#13;&#10;
                            FROM PromoBranch pb&#13;&#10;
                            left join CompBranches cb &#13;&#10;
                            on cb.CompCode = pb.CompCode &#13;&#10;
                            and cb.BranchCode = pb.BranchCode &#13;&#10;
                            left join ResListValues rlv &#13;&#10;
                            on cb.StoreGroupId = rlv.ElementValue &#13;&#10;
                            and rlv.GroupName = 'StoreGroup'&#13;&#10;
                            WHERE PromoID = @PromoID" 
             InsertCommand="INSERT INTO PromoBranch(PromoID, CompCode, BranchCode, ShortName)&#13;&#10;VALUES (@PromoID, @CompCode, @BranchCode, @ShortName)" DeleteCommand="DELETE FROM PromoBranch&#13;&#10;WHERE CompCode = @CompCode AND BranchCode = @BranchCode&#13;&#10;AND PromoID IN (SELECT PromoID &#13;&#10;                             FROM Promotions&#13;&#10;&#9;             WHERE RequestID = @RequestID )&#13;&#10;">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="PromoID" SessionField="CurrPromoID" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="PromoID" />
                <asp:Parameter Name="CompCode" />
                <asp:Parameter Name="BranchCode" />
                <asp:Parameter Name="ShortName" />
            </InsertParameters>
            <DeleteParameters>
                <asp:Parameter Name="CompCode" />
                <asp:Parameter Name="BranchCode" />
                <asp:Parameter Name="RequestID" />
            </DeleteParameters>
        </asp:SqlDataSource><asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT 1" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
        </asp:SqlDataSource>
        <div style="display:none">
            <asp:Button ID="cmdPopUpOK" runat="server" Text="Button" />
            <asp:Button ID="cmdLoadForm" runat="server" Text="Button" />
            <asp:Button ID="cmdMissingSeeding" runat="server" Text="Button" />
        </div>
        
</asp:Content>
