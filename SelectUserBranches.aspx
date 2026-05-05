<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="SelectUserBranches.aspx.vb" Inherits="SelectUserBranches" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select Branches</title>
    <link rel="stylesheet" type="text/css" href="./css/main.css" media="screen" />
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
</head>
<body style="background: white;">

    <form id="form1" runat="server">    
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
     document.getElementById("<%=cmdProcess.ClientID%>").click(); 
    var theform = this.contentDoc.forms[0] 
	return true 
	}
}


</script>

<script language="javascript" type="text/javascript">
<!--

function confirm_action(msg)
{
    return confirm(msg);
}

// -->
</script>
           <table cellspacing="0" style="width: 427px">
            <tr>
                <td style="width: 13px; height: 14px;">
                </td>
                <td style="width: 76px; height: 14px;" bgcolor="royalblue">
                    &nbsp;</td>
                <td style="width: 8px; height: 14px;">
                    </td>
                <td style="width: 264px; height: 14px;">
                    &nbsp;</td>
                <td style="height: 14px; width: 11px;">
                    </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 11px" bgcolor="#bdb76b">
                </td>
                <td bgcolor="#bdb76b" style="width: 76px; height: 11px; text-align: center">
                    <span style="color: black; font-family: Trebuchet MS"><strong>COMPANY:</strong></span></td>
                <td style="width: 8px; height: 11px" bgcolor="#bdb76b">
                </td>
                <td style="height: 11px" bgcolor="#bdb76b" colspan="2">
                    &nbsp;<strong><span style="font-family: Trebuchet MS; font-size: 11pt;"><asp:Label
                        ID="lblCompName" runat="server" Width="413px"></asp:Label></span></strong></td>
            </tr>
            <tr>
                <td style="width: 13px; height: 21px">
                </td>
                <td bgcolor="royalblue" style="width: 76px; height: 21px">
                </td>
                <td style="width: 8px; height: 21px">
                </td>
                <td style="width: 264px; height: 21px">
                    &nbsp;</td>
                <td style="height: 21px; width: 11px;">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 25px; text-align: center">
                </td>
                <td style="width: 76px; height: 25px; text-align: center;" bgcolor="royalblue">
                </td>
                <td style="width: 8px; height: 25px;">
                </td>
                <td colspan="2" rowspan="8" valign="top">
                    &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp;
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td bgcolor="#5d7b9d" nowrap="nowrap" style="width: 34px; padding-left: 20px;">
                                    <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_CheckedChanged" Text="All" Font-Bold="True" ForeColor="White" /></td>

                            <td align="center" bgcolor="#5d7b9d" style="width: 29px; height: 22px;">
                                <strong><span style="color: #ffffff">Branch Code</span></strong></td>
                            <td align="center" bgcolor="#5d7b9d" style="width: 37px; height: 22px;">
                                <strong><span style="color: #ffffff">Branch Name</span></strong></td>

                            <td align="center" bgcolor="#5d7b9d" style="width: 162px; height: 22px;">
                                <strong><span style="color: #ffffff"></span></strong></td>
                            <td align="center" bgcolor="#5d7b9d" style="width: 148px; height: 22px;">
                                <strong><span style="color: #ffffff"></span></strong></td>
                        </tr>
                    </table>
                    <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 5px; overflow: auto;
                        width: 534px; padding-top: 5px; height: 207px; background-color: whitesmoke; margin: 0px;">
                    <asp:GridView ID="gridBranches" runat="server" AutoGenerateColumns="False"
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="4"
                        ForeColor="#333333" Width="97%" AllowSorting="True" Height="147px" ShowHeader="False">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" Checked='<%#Convert.ToBoolean(Eval("isSelected")) %>' runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderTemplate>
                                    &nbsp;
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code" SortExpression="BranchCode" Visible="False">
                                <EditItemTemplate>
                                    &nbsp;
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchCode" runat="server" Text='<%# Bind("BranchCode") %>'></asp:Label>
                                    <asp:Label ID="lblRowCode" runat="server" Text='<%# Bind("RowID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="BranchCodeFormatted" HeaderText="BranchCode" SortExpression="BranchCodeFormatted">
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" >
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblShortName" runat="server" Text='<%# Eval("BranchName") %>' Width="139px"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblCompCode" runat="server" Text='<%# Eval("CompCode") %>' Width="140px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EmptyDataTemplate>
                            <asp:CheckBox ID="chkRowSel" runat="server" />
                        </EmptyDataTemplate>
                    </asp:GridView>
                        &nbsp;
                    </div>
                    <span style="font-size: 11pt; font-family: Trebuchet MS"></span></td>
            </tr>
            <tr>
                <td style="width: 13px; text-align: center; height: 26px;">
                </td>
                <td style="width: 76px; text-align: center; height: 26px;" bgcolor="royalblue">
                    </td>
                <td style="width: 8px; height: 26px;">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 26px">
                </td>
                <td style="width: 76px; height: 26px; text-align: center;" bgcolor="royalblue">
                    </td>
                <td style="width: 8px; height: 26px;">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 22px;">
                </td>
                <td style="width: 76px; height: 22px;" bgcolor="royalblue">
                </td>
                <td style="width: 8px; height: 22px;">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 21px;">
                </td>
                <td style="width: 76px; height: 21px;" bgcolor="royalblue">
                </td>
                <td style="width: 8px; height: 21px;">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 23px;">
                </td>
                <td bgcolor="royalblue" style="width: 76px; height: 23px;" align="center"></td>
                <td style="width: 8px; height: 23px;">
                </td>
            </tr>
            <tr>
                <td style="width: 13px">
                </td>
                <td bgcolor="royalblue" style="width: 76px">
                </td>
                <td style="width: 8px">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 24px;">
                </td>
                <td bgcolor="royalblue" style="width: 76px; height: 24px;">
                </td>
                <td style="width: 8px; height: 24px;">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 24px">
                </td>
                <td bgcolor="royalblue" style="width: 76px; height: 24px">
                </td>
                <td style="width: 8px; height: 24px">
                </td>
                <td align="left" colspan="2" rowspan="1" valign="top">
        <asp:LinkButton ID="lnkDone" runat="server" CssClass="standard-link">Add Selected Branches</asp:LinkButton><br />
                </td>
            </tr>
        </table>
        &nbsp;&nbsp;
        
        <asp:SqlDataSource ID="SqlDSRowID" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"                      
            SelectCommand="usp_GetFilteredRowIDs @CompCode, @SelectedRow, @ReAccumulatedRow" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
       <SelectParameters>
            <%--<asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="CurrUserID" />--%>
            <asp:Parameter Name="CompCode" />
            <asp:Parameter Name="SelectedRow" />
            <asp:Parameter Name="ReAccumulatedRow" />
        </SelectParameters>
    </asp:SqlDataSource>

                    
        <asp:SqlDataSource ID="SqlDataCompanyDesc" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"                      
            SelectCommand="Select top 1 CompanyName from Companies where CompCode = @CompCode" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
            <SelectParameters>
                <asp:Parameter Name="CompCode" />
            </SelectParameters>
        </asp:SqlDataSource>
                    
        <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                        DeleteCommand="DELETE FROM PromoBranch WHERE PromoID = @PromoID AND CompCode = @CompCode AND BranchCode = @BranchCode"
                        SelectCommand="SELECT 1" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
                    </asp:SqlDataSource>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <asp:HiddenField ID="hdnCompCode" runat="server" />
        <asp:HiddenField ID="BranchPreviousSelection" runat="server" />
        <div style="display:none"><asp:Button ID="cmdProcess" runat="server" /></div>
</form>
</body>
</html>