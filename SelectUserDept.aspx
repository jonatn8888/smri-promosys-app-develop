
<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="SelectUserDept.aspx.vb" Inherits="SelectUserDept" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select Department(s)</title>
    <link rel="stylesheet" type="text/css" href="./css/main.css" media="screen" />
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
    <style type="text/css">
        .style1
        {
            height: 22px;
            width: 49px;
        }
        .style2
        {
            height: 22px;
        }
        .style3
        {
            width: 29px;
        }
    </style>
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
                    <span style="color: black; font-family: Trebuchet MS"><strong>Business Unit:</strong></span></td>
                <td style="width: 8px; height: 11px" bgcolor="#bdb76b">
                </td>
                <td style="height: 11px" bgcolor="#bdb76b" colspan="2">
                    &nbsp;<strong><span style="font-family: Trebuchet MS; font-size: 11pt;"><asp:Label
                        ID="lblDeptName" runat="server" Width="413px"></asp:Label></span></strong></td>
            </tr>
            <tr>
                <td style="width: 13px; height: 21px">
                </td>
                <td bgcolor="royalblue" style="width: 76px; height: 21px">
                </td>
                <td style="width: 8px; height: 21px">
                </td>
                <td style="width: 264px; height: 21px">
                </td>
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
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td bgcolor="#5d7b9d" nowrap="nowrap" style="padding-left: 20px;" 
                                class="style3">
                                    <asp:CheckBox ID="chkALL" runat="server" AutoPostBack="True" OnCheckedChanged="chkALL_CheckedChanged" Text="All" Font-Bold="True" ForeColor="White" /></td>

                            <td align="center" bgcolor="#5d7b9d" class="style1">
                                <strong><span style="color: #ffffff">Dept Code</span></strong></td>
                            <td align="center" bgcolor="#5d7b9d" class="style2">
                                <strong><span style="color: #ffffff">Department</span></strong></td>

                        </tr>
                    </table>
                    <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 5px; overflow: auto;
                        width: 534px; padding-top: 5px; height: 207px; background-color: white; margin: 0px;">
                    <asp:GridView ID="grdDepts" runat="server" AutoGenerateColumns="False"
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
                        ForeColor="#333333" Width="106%" AllowSorting="True" Height="147px" 
                            ShowHeader="False" RowHeaderColumn="DeptCode">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" Checked='<%#Convert.ToBoolean(Eval("isSelected")) %>' />
                                </ItemTemplate>
                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderTemplate>
                                    &nbsp;
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Department" SortExpression="Department" Visible="False">
                                <EditItemTemplate>
                                    &nbsp;
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupIDX" runat="server" Text='<%# Bind("GroupID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="DeptCode" HeaderText="DeptCode" SortExpression="DeptCode">
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" >
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblGroupID" runat="server" Text='<%# Eval("GroupID") %>' Width="139px"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("Department") %>' Width="140px"></asp:Label>
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
        <asp:LinkButton ID="lnkDone" runat="server" CssClass="standard-link">Add Selected Department(s)</asp:LinkButton><br />
                </td>
            </tr>
        </table>
        &nbsp;&nbsp;
        
         <div style="display: none">
             <asp:HiddenField ID="SelectedBU" runat="server" />
             <asp:HiddenField ID="NonInitiallySelected" runat="server" />
             <asp:HiddenField ID="SelectedDepartments" runat="server" />
             
             <asp:HiddenField ID="hdnSelectedBU" runat="server" />
         </div>
                 <asp:SqlDataSource 
                        ID="SqlDataBUDesc" 
                        runat="server" 
                        ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>" 
                        ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>"
                        SelectCommand="SELECT ug.Description Description FROM UserGroups ug where ug.Department is null and ug.BizUnit in (Select ug2.BizUnit from UserGroups ug2 where ug2.Groupid = @GroupID )">
                        <SelectParameters>
                            <asp:Parameter Name="GroupID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    
                     <asp:SqlDataSource ID="SqlDSGroupID" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"                      
                        SelectCommand="usp_GetFilteredRowIDsbyBUDept @BizUnit, @SelectedRow, @ReAccumulatedRow" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
                               <SelectParameters>
                                    <%--<asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="CurrUserID" />--%>
                                    <asp:Parameter Name="BizUnit" />
                                    <asp:Parameter Name="SelectedRow" />
                                    <asp:Parameter Name="ReAccumulatedRow" />
                                </SelectParameters>
                     </asp:SqlDataSource>
            
                    <asp:SqlDataSource ID="sqldsDept" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                        SelectCommand="USP_SelectUserGroupsDptViewerBySelection @BizUnit, @Selected, @IncludeAll, @UserID, @UnChecked" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>" SelectCommandType="Text">
                        <SelectParameters>
                            <asp:Parameter Name="BizUnit" />
                            <asp:Parameter Name="Selected" />
                            <asp:Parameter Name="IncludeAll" />
                            <asp:Parameter Name="UserID" />
                            <asp:Parameter Name="UnChecked" DefaultValue=" " />
                        </SelectParameters>
                    </asp:SqlDataSource>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <div style="display:none"><asp:Button ID="cmdProcess" runat="server" /></div>
</form>
</body>
</html>