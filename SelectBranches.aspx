<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" CodeFile="SelectBranches.aspx.vb" Inherits="SelectBranches" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Select Branches</title>
    <link rel="stylesheet" type="text/css" href="./css/main.css" media="screen" />
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
    <style type="text/css">
        .style1
        {
            height: 21px;
            width: 116px;
        }
        .style2
        {
            width: 13px;
            height: 21px;
        }
        .style3
        {
            width: 76px;
            height: 21px;
        }
        .style4
        {
            width: 8px;
            height: 21px;
        }
        .style5
        {
            width: 264px;
            height: 21px;
        }
        .style6
        {
            width: 11px;
            height: 21px;
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
                <td style="width: 264px; height: 14px;" colspan="2">
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
                <td style="height: 11px" bgcolor="#bdb76b" colspan="3">
                    &nbsp;<strong><span style="font-family: Trebuchet MS; font-size: 11pt;"><asp:Label
                        ID="lblCompName" runat="server" Width="413px"></asp:Label></span></strong></td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td bgcolor="royalblue" class="style3">
                </td>
                <td class="style4">
                </td>
                <td class="style5" colspan="2">
                </td>
                <td class="style6">
                </td>
            </tr>
            <tr>
                <td class="style2">
                </td>
                <td bgcolor="royalblue" class="style3">
                </td>
                <td class="style4">
                </td>
                <td style="font-weight: bold;" class="style1">
                    Store Group:</td>
                <td class="style5">
                 <asp:DropDownList 
                    ID="ddlStoreGroup"
                    runat="server"
                    AutoPostBack="true"
                    DataTextField="ElementName" DataValueField="ElementValue"
                    OnSelectedIndexChanged="ddlStoreGroup_SelectedIndexChanged"
                    Width="200px" >
                <asp:ListItem Text="-- Select All --" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Physical Store" Value="4"></asp:ListItem>
                </asp:DropDownList>
                </td>
                <td class="style6">
                </td>
            </tr>
            <tr>
                <td style="width: 13px; height: 25px; text-align: center">
                </td>
                <td style="width: 76px; height: 25px; text-align: center;" bgcolor="royalblue">
                </td>
                <td style="width: 8px; height: 25px;">
                </td>
                <td colspan="3" rowspan="8" valign="top">
                    &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp;
                    <div style="padding: 5px 0px; overflow: auto;
                        width: 577px; height: 207px; background-color: whitesmoke; margin: 0px;">
                    <asp:GridView ID="gridBranches" runat="server" AutoGenerateColumns="False"
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="4" DataSourceID="sqldsBranches"
                        ForeColor="#333333" Width="97%" AllowSorting="True" Height="147px">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox 
                                        ID="chkALL"
                                        runat="server"
                                        AutoPostBack="true"
                                        OnCheckedChanged="chkALL_CheckedChanged"
                                        Text="All"
                                        ForeColor="White"
                                        Font-Bold="True" />
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRowSel" runat="server" />
                                </ItemTemplate>

                                <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Code" SortExpression="BranchCode" Visible="False">
                                <EditItemTemplate>
                                    &nbsp;
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchCode" runat="server" Text='<%# Bind("BranchCode") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CompCodeFormatted" HeaderText="CompCode" SortExpression="CompCodeFormatted">
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BranchCodeFormatted" HeaderText="BranchCode" SortExpression="BranchCodeFormatted">
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BranchName" HeaderText="Branch" SortExpression="BranchName" >
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MMSname" HeaderText="Short Desc" SortExpression="MMSname" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StoreGroup" HeaderText="Group" SortExpression="StoreGroup" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="150px" />
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
                <td align="left" colspan="3" rowspan="1" valign="top">
        <asp:LinkButton ID="lnkDone" runat="server" CssClass="standard-link">Add Selected Branches</asp:LinkButton><br />
                </td>
            </tr>
        </table>
        &nbsp;&nbsp;
                    <asp:SqlDataSource ID="sqldsBranches" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                        DeleteCommand="DELETE FROM PromoBranch WHERE PromoID = @PromoID AND CompCode = @CompCode AND BranchCode = @BranchCode"
                        InsertCommand="INSERT INTO PromoBranch(PromoID, CompCode, BranchCode, ShortName)&#13;&#10;VALUES (@PromoID, @CompCode, @BranchCode, @ShortName)"
                        SelectCommand="SELECT E.EnvName, C.RowID, C.CompCode, C.CompanyName, &#13;&#10;             
                                        C.ShortName + ' - ' + B.ShortName AS CompBranchNick,&#13;&#10;             
                                        C.EnvCode, C.IsHidden, B.RowID, B.BranchCode, B.BranchName,&#13;&#10;             
                                        B.MMSname, B.IsDeptStore, B.IsHidden,&#13;&#10;             
                                        RIGHT('000' + CONVERT(varchar(3),C.CompCode),3) AS CompCodeFormatted,&#13;&#10;             
                                        RIGHT('000' + CONVERT(varchar(4),B.BranchCode),4) AS BranchCodeFormatted,&#13;&#10;
                                        rlv.ElementName AS StoreGroup&#13;&#10;
                                        FROM CompBranches AS CB&#13;&#10;
                                        INNER JOIN Companies AS C ON CB.CompCode = C.CompCode&#13;&#10;
                                        INNER JOIN Branches AS B ON CB.BranchCode = B.BranchCode &#13;&#10;
                                        INNER JOIN Environments AS E ON C.EnvCode = E.EnvCode&#13;&#10;
                                        LEFT JOIN ResListValues AS rlv ON rlv.ElementValue = cb.StoreGroupId&#13;&#10; 
                                        AND rlv.GroupName = 'StoreGroup'&#13;&#10; WHERE (C.IsHidden = 0)                                        
                                        AND (B.IsHidden = 0) AND (CB.IsVisualStore = @IsVisualStore OR @IsVisualStore = 0)&#13;&#10;
                                        AND (C.EnvCode = @EnvCode)&#13;&#10; 
                                        AND (@StoreGroupID = -1
                                        OR (@StoreGroupID = 4 AND CB.StoreGroupID IN (1,2))
                                        OR (@StoreGroupID <> 4 AND CB.StoreGroupID = @StoreGroupID))&#13;&#10;
                                        ORDER BY B.BranchName" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
                        <SelectParameters>
                            <asp:Parameter Name="EnvCode" />
                            <asp:Parameter Name="IsVisualStore" />
                            <asp:Parameter Name="StoreGroupID"/>
                        </SelectParameters>
                        <InsertParameters>
                            <asp:Parameter Name="PromoID" />
                            <asp:Parameter Name="CompCode" />
                            <asp:Parameter Name="BranchCode" />
                            <asp:Parameter Name="ShortName" />
                        </InsertParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="PromoID" />
                            <asp:Parameter Name="CompCode" />
                            <asp:Parameter Name="BranchCode" />
                            <asp:Parameter Name="PromoID" />
                            <asp:Parameter Name="CompCode" />
                            <asp:Parameter Name="BranchCode" />
                        </DeleteParameters>
                    </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                        DeleteCommand="DELETE FROM PromoBranch WHERE PromoID = @PromoID AND CompCode = @CompCode AND BranchCode = @BranchCode"
                        SelectCommand="SELECT 1" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
                    </asp:SqlDataSource>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
        <div style="display:none"><asp:Button ID="cmdProcess" runat="server" /></div>
</form>
</body>
</html>