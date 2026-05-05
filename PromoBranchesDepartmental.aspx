<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="PromoBranchesDepartmental.aspx.vb" Inherits="PromoBranchesDepartmental" %>

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

    document.getElementById("<%=formloaad.ClientID%>").click(); 
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
</script>

<script type="text/javascript" language="JavaScript">
<!--

function check_company()
{   
    if(document.getElementById('ctl00$ContentPlaceHolder1$cboCompany').selectedIndex == 0)
    {
        alert("Cannot add branches. No company selected.");
        document.getElementById("ctl00$ContentPlaceHolder1$cboCompany").focus();
        return false;
    }
    else
    {
        return confirm('Select all branches for this company?');
    }
}

//function cmdCompBranches_onclick() {
 
//  var WinSettings = "center:yes;resizable:no;dialogHeight:300px;status:no";

//  var MyArgs = window.showModalDialog("CompBranches.aspx", 0, WinSettings);

//}


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
        <br />
        <asp:LinkButton ID="lnkSaveMemo" runat="server">View Request Preview</asp:LinkButton><br />
        <br />
        <br />
    </div>
    
    <div id="contents">
        <table style="width: 644px">
<tr>
                <td bgcolor="gainsboro" style="height: 22px; text-align: center">
                    <span style="font-size: 11pt; color: lightslategray; font-family: Trebuchet MS"><strong>
                        <asp:LinkButton ID="lnkRequest" runat="server" Width="230px">Sales Promotion Request</asp:LinkButton></strong></span></td>
            </tr>
            <tr style="font-size: 9pt">
                <td align="center" bgcolor="#dcdcdc" style="height: 21px">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkPromoInfo" runat="server" Width="228px">Promotion Information</asp:LinkButton></span></strong></td>
            </tr>
            <tr>
                <td  id="tdPromoDetails" runat="server" bgcolor="gainsboro" style="height: 21px; text-align: center"
                    valign="top">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkPromoDetails" runat="server" Width="230px">Promo Details</asp:LinkButton></span></strong></td>
            </tr>
            <tr>
                <td bgcolor="steelblue" style="height: 22px; text-align: center">
                    <span style="font-size: 11pt; color: #ffffff; font-family: Trebuchet MS"><strong>Branches</strong></span></td>
            </tr>
            <tr>
                <td style="text-align: center" valign="top" bgcolor="aliceblue">
        
        <table cellspacing="0">
            <tr>
                <td style="width: 16px; height: 20px;">
                    </td>
                <td style="width: 6px; height: 20px;">
                    </td>
                <td style="width: 163px; height: 20px;" >
                    </td>
            </tr>
            <tr>
                <td style="height: 14px; width: 16px; text-align: right;">
                    <span style="font-size: 10pt; font-family: Trebuchet MS">Company:</span></td>
                <td style="width: 6px; height: 14px">
                </td>
                <td colspan="1" style="height: 14px; text-align: left;">
                    <asp:DropDownList ID="cboCompany" runat="server" AppendDataBoundItems="True"
                        DataSourceID="sqldsEnvironments" DataTextField="EnvName" DataValueField="EnvCode"
                        Width="298px">
                        <asp:ListItem Selected="True" Value="-1">- Select Company -</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="cmdSelectBranch" runat="server" Text="Select From List" Width="126px" Height="21px" /></td>
            </tr>
            <tr>
                <td>
                    </td>
                <td>
                </td>
                <td>
                    <asp:Button ID="cmdAllBranches" runat="server" Height="21px" Text="All Branches"
                        Visible="False" Width="124px" /></td>
            </tr>
            <tr id="trFindBranch" runat="server">
                <td style="width: 16px; height: 14px">
                    Find:</td>
                <td style="width: 6px; height: 14px">
                </td>
                <td style="width: 163px; height: 14px">
                    <asp:TextBox ID="txtSearchValue" runat="server" onkeyup="Filter(this);" Width="289px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 102px; text-align: center;">
                    <table style="width: 100%">
                        <tr>
                            <td align="center" bgcolor="#5d7b9d" style="width: 12%; height: 25px">
                                <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" EnableTheming="True" OnCheckedChanged="chkALL_CheckedChanged" Width="51px" Font-Bold="True" ForeColor="White" Text="All" /></td>
                            <td align="center" bgcolor="#5d7b9d" nowrap="nowrap" style="width: 95%; height: 25px">
                                <strong><span style="color: #ffffff">Description</span></strong>
                            </td>
                        </tr>
                    </table>
                    <div align="left" style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px;
                        overflow: auto; width: 534px; padding-top: 10px; height: 172px; background-color: whitesmoke">
                        <asp:GridView ID="gridBranches" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                            BorderStyle="Solid" BorderWidth="1px" CellPadding="4" ForeColor="#333333" ShowHeader="False"
                            Width="97%">
                            <EmptyDataTemplate>
                                <b>No Selected Branch</b>
                            </EmptyDataTemplate>
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRowSel" runat="server" />
                                        <asp:HiddenField ID="hfBranchCode" runat="server" Value='<%# eval("BranchCode") %>' />
                                        <asp:HiddenField ID="hfCompCode" runat="server" Value='<%# eval("CompCode") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="5px" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_CheckedChanged" />
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompCode" runat="server" Text='<%# Eval("CompCode") %>' Width="91px"></asp:Label><asp:Label
                                            ID="lblBranchCode" runat="server" Text='<%# Eval("BranchCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CompCode" HeaderText="CmpCode" SortExpression="CompCode"
                                    Visible="False">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10px" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BranchCode" HeaderText="BrCode" SortExpression="BranchCode"
                                    Visible="False">
                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" Wrap="False" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ShortName" HeaderText="Description" SortExpression="ShortName">
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
                <td colspan="4" style="height: 4px; text-align: left">
                    <asp:LinkButton ID="lnkDeleteSelected" runat="server" Font-Size="Small" CssClass="action-link" Width="148px">Delete Selected</asp:LinkButton>&nbsp;
                </td>
            </tr>
        </table>
                    &nbsp;</td>
            </tr>
            <tr>
                <td bgcolor="gainsboro" style="height: 21px; text-align: center"
                    valign="top">
                    <strong><span style="font-size: 11pt; color: #778899; font-family: Trebuchet MS">
                        <asp:LinkButton ID="lnkPreview" runat="server" Width="230px">Preview Document</asp:LinkButton></span></strong></td>
            </tr>
            <tr>
                <td style="text-align: center; height: 8px;" valign="top">
                    &nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
                    <br />
    
    </div>
        <asp:SqlDataSource ID="sqldsEnvironments" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT *&#13;&#10;FROM Environments&#13;&#10;WHERE IsHidden = 0 AND EnvCode IN&#13;&#10;( SELECT Distinct EnvCode &#13;&#10;  FROM DepSdepClass&#13;&#10;  WHERE STR(DeptCode)+' '+STR(SubDepCode)+' '+STR(ClassCode) IN&#13;&#10;  ( SELECT STR(A.DepCode)+' '+STR(A.SubDepCode)+' '+STR(A.ClassCode)&#13;&#10;    FROM PromoDetails A inner join Promotions B &#13;&#10;&#9;ON A.PromoID = B.PromoID&#13;&#10;&#9;WHERE B.RequestID = @RequestID))&#13;&#10;ORDER BY EnvName&#13;&#10;">
            <SelectParameters>
                <asp:SessionParameter Name="RequestID" SessionField="CurrRequestID" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqldsPromoBranches" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT  distinct CompCode,branchcode,shortname&#13;&#10;FROM PromoBranch A inner join Promotions B &#13;&#10;ON A.PromoID = B.PromoID&#13;&#10;WHERE B.RequestID =  @RequestID" InsertCommand="INSERT INTO PromoBranch(PromoID, CompCode, BranchCode, ShortName)&#13;&#10;VALUES (@PromoID, @CompCode, @BranchCode, @ShortName)" DeleteCommand="DELETE FROM PromoBranch&#13;&#10;WHERE CompCode = @CompCode AND BranchCode = @BranchCode&#13;&#10;AND PromoID IN (SELECT PromoID &#13;&#10;                             FROM Promotions&#13;&#10;&#9;             WHERE RequestID = @RequestID )&#13;&#10;">
            <SelectParameters>
                <asp:SessionParameter Name="RequestID" SessionField="CurrRequestID" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="PromoID" />
                <asp:Parameter Name="CompCode" />
                <asp:Parameter Name="BranchCode" />
                <asp:Parameter Name="ShortName" />
            </InsertParameters>
            <DeleteParameters>
                <asp:Parameter Name="RequestID" />
                <asp:Parameter Name="CompCode" />
                <asp:Parameter Name="BranchCode" />
            </DeleteParameters>
        </asp:SqlDataSource><asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT 1" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
        </asp:SqlDataSource><asp:SqlDataSource ID="sqldsTmpBranches" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT 1" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
        </asp:SqlDataSource>
    <div style="display: none">
        <asp:Button ID="cmdPopUpOK" runat="server" Text="Button" />
        <asp:Button ID="formloaad" runat="server" Text="Button" /></div>

</asp:Content>
