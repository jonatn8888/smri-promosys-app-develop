<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="PriceEventSearch.aspx.vb" Inherits="PriceEventSearch" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">

<script src="./rte/richtext.js" type="text/javascript" language="javascript"></script>
<script src="./rte/config.js" type="text/javascript" language="javascript"></script>
<script src="js/DatePicker.js" type="text/javascript" language="javascript"></script>
   
<script language="javascript" type="text/javascript">

function confirm_delete()
{
    return confirm("Delete this record?");
}

</script>    

    <br />
    <br />    
    
    <div id="contents">
        <table cellpadding="3" cellspacing="0" style="width: 605px">
            <tr>
                <td colspan="3" style="width: 612px; height: 27px; text-align: left">
                    <strong><span style="font-size: 16px; color: darkolivegreen; font-family: 'Trebuchet MS'">
                        Price Event Reports</span></strong></td>
            </tr>
        </table>
        <hr style="width: 700px" />
        <br />
        <table cellpadding="3" cellspacing="0" id="doc-table">
            <tr>
                <td style="width: 110px; height: 10px;">
                    Report Type :</td>
                <td class="field-cell" style="height: 10px" colspan="2">
                    <asp:DropDownList ID="cboReportType" runat="server" Width="175px">
                        <asp:ListItem Value="0">-- Select Report Type --</asp:ListItem>
                        <asp:ListItem Value="MKU">Markup Authorization</asp:ListItem>
                        <asp:ListItem Value="MKD">Markdown Authorization</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 110px; height: 6px" valign="middle">
                    Event Number :</td>
                <td class="field-cell" style="height: 6px" colspan="2">
                    <asp:TextBox ID="txtEventNumber" runat="server" MaxLength="6" Width="168px"></asp:TextBox></td>

            </tr>
            <tr>
                <td style="width: 110px; height: 6px" valign="middle">
                    Department :</td>
                <td class="field-cell" style="height: 6px" colspan="2">
                <asp:DropDownList ID="cboDepartment" runat="server" Width="356px" DataSourceID="sqldsDepartments" DataTextField="DisplayName" DataValueField="strDeptCode" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True" Value="0">-- All Available Departments  --</asp:ListItem>
                </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 110px; height: 6px" valign="middle">
                    Location :</td>
                <td class="field-cell" colspan="2" style="height: 6px">
                    <asp:RadioButton ID="optChainBranches" runat="server" GroupName="grpLocation"
                        Text="Chain Branches" AutoPostBack="True" /></td>
            </tr>
            <tr>
                <td style="width: 110px; height: 6px" valign="middle">
                </td>
                <td class="field-cell" style="width: 22px; height: 6px">
                </td>
                <td class="field-cell" style="width: 26px; height: 6px">
                    <asp:Label ID="lblEnvironment" runat="server" Text="Environment:"></asp:Label><br />
                    <asp:DropDownList ID="cboEnvironments" runat="server" Width="229px" DataTextField="strDisplayName" DataValueField="EnvCode" AppendDataBoundItems="True" DataSourceID="sqldsEnvironments">
                        <asp:ListItem Selected="True" Value="0">-- Select Environment  --</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 110px; height: 6px" valign="middle">
                </td>
                <td class="field-cell" colspan="2" style="height: 6px">
                    <asp:RadioButton ID="optSingleBranch" runat="server" GroupName="grpLocation"
                        Text="Specific Branch" AutoPostBack="True" /></td>
            </tr>
            <tr>
                <td style="width: 110px; height: 6px" valign="middle">
                </td>
                <td class="field-cell" style="width: 22px; height: 6px">
                </td>
                <td class="field-cell" style="width: 26px; height: 6px">
                    <asp:Label ID="lblBranch" runat="server" Enabled="False" Text="Branch:"></asp:Label><br />
                    <asp:DropDownList ID="cboBranch" runat="server" Width="353px" DataSourceID="sqldsBranches" DataTextField="strDisplayName" DataValueField="strBranchCode" AppendDataBoundItems="True" Enabled="False">
                        <asp:ListItem Selected="True" Value="0">-- Select Branch / Location  --</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 110px; height: 3px;" valign="middle">
                    </td>
                <td class="field-cell" style="width: 22px; height: 3px">
                </td>
                <td style="width: 26px; height: 3px" class="field-cell"></td>
            </tr>
            <tr>
                <td style="width: 110px; height: 11px">
                    Effectivity Date :</td>
                <td class="field-cell" colspan="2" style="height: 11px">
                    <asp:TextBox ID="txtPeriodFrom" runat="server" MaxLength="50" Width="177px"></asp:TextBox><input
                        id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodFrom');"
                        style="left: 322px; top: 345px" type="button" />
                    &nbsp;
                    <asp:TextBox ID="txtPeriodTo" runat="server" MaxLength="50" Width="177px"></asp:TextBox><input
                        id="calPeriodTo" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodTo');"
                        style="left: 547px; top: 345px" type="button" /></td>

            </tr>
            <tr>
                <td style="width: 110px;" valign="top">
                    &nbsp;</td>
                <td colspan="1" style="height: 2px; text-align: right">
                </td>
                <td style="height: 2px; text-align: right;">
                    <asp:Button ID="cmdFilterList" runat="server" Text="Search" Width="106px" /></td>

            </tr>
        </table>
        <br />
        <asp:GridView ID="gridPriceEvents" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" BorderStyle="Solid" BorderWidth="2px" CellPadding="4"
            DataSourceID="sqldsPriceEvents" ForeColor="#333333" Width="700px" Visible="False">
            <RowStyle BackColor="WhiteSmoke" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblBranchCode" runat="server" Text='<%# Eval("BranchCode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EventNumber" HeaderText="Event No" SortExpression="EventNumber">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="EventDesc" HeaderText="EventDesc" SortExpression="EventDesc" NullDisplayText="- -">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="TranType" HeaderText="Type" SortExpression="TranType">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DeptCode" HeaderText="Dept" SortExpression="DeptCode">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="30px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="BranchCode" HeaderText="Branch" SortExpression="BranchCode">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="EventStartDate" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Start Date"
                    SortExpression="EventStartDate">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="EventEndDate" DataFormatString="{0:MM-dd-yyyy}" HeaderText="End Date"
                    SortExpression="EventEndDate">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" CssClass="gridPagerRow" />
            <EmptyDataTemplate>
                <strong>No matching data for the specified search category.</strong>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
        </asp:GridView>
        <br />
        <asp:SqlDataSource ID="sqldsPriceEvents" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>">
        </asp:SqlDataSource>
        <br />
        <br />
        
        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="630px">
        </asp:BulletedList>
        
        </div>
    <div style="display:none">
        <asp:SqlDataSource ID="sqldsBranches" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT BranchCode, BranchName, &#13;&#10;    RIGHT('00000'+CAST(BranchCode AS varchar(5)),5) AS strBranchCode,&#13;&#10;    RIGHT('00000'+CAST(BranchCode AS varchar(5)),5) + ' - ' + BranchName AS strDisplayName&#13;&#10;FROM Branches&#13;&#10;WHERE IsHidden = 0&#13;&#10;AND (@UserLevel <> 100 OR BranchCode IN (&#13;&#10;&#9;&#9;&#9;SELECT BranchCode&#13;&#10;&#9;&#9;&#9;FROM BranchAssignment&#13;&#10;&#9;&#9;&#9;WHERE UserID = @UserID&#13;&#10;&#9;&#9;&#9;)&#13;&#10;&#9;)&#13;&#10;ORDER BY BranchCode&#13;&#10;">
            <SelectParameters>
                <asp:SessionParameter Name="UserLevel" SessionField="UserLevel" />
                <asp:SessionParameter Name="UserID" SessionField="UserID" />
            </SelectParameters>
        </asp:SqlDataSource><asp:SqlDataSource ID="sqldsDepartments" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT RIGHT('000'+CAST(DeptCode AS varchar(3)),3) AS strDeptCode,&#13;&#10;&#9; MAX(RIGHT('000'+CAST(DeptCode AS varchar(3)),3) + ' - ' + Description) AS DisplayName&#13;&#10;FROM DepSdepClass&#13;&#10;WHERE SubDepCode = 0 AND ClassCode = 0 AND SubClassCode = 0&#13;&#10;AND ( @UserLevel < 50 OR DeptCode IN (&#13;&#10;&#9;&#9;&#9;&#9;&#9;SELECT DeptCode&#13;&#10;&#9;&#9;&#9;&#9;&#9;FROM UserGroups&#13;&#10;&#9;&#9;&#9;&#9;&#9;WHERE GroupID IN (&#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;SELECT GroupID&#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;FROM GroupAssignment&#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;WHERE UserID = @UserID&#13;&#10;&#9;&#9;&#9;&#9;&#9;)&#13;&#10;&#9;&#9;&#9;&#9;))&#13;&#10;GROUP BY RIGHT('000'+CAST(DeptCode AS varchar(3)),3)&#13;&#10;&#13;&#10;">
            <SelectParameters>
                <asp:SessionParameter Name="UserLevel" SessionField="UserLevel" />
                <asp:SessionParameter Name="UserID" SessionField="UserID" />
            </SelectParameters>
        </asp:SqlDataSource><asp:SqlDataSource ID="sqldsEnvironments" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT EnvCode, EnvName,&#13;&#10;     ShortDesc + ' - ' +  EnvName AS strDisplayName&#13;&#10;FROM Environments&#13;&#10;WHERE IsHidden = 0&#13;&#10;">
            <SelectParameters>
                <asp:SessionParameter Name="UserLevel" SessionField="UserLevel" />
                <asp:SessionParameter Name="UserID" SessionField="UserID" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        &nbsp;</div>
</asp:Content>

