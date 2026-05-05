<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PromoRequestSwipestakesSeed.aspx.vb" Inherits="PromoRequestSwipestakesSeed" title="Untitled Page"  %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Text Editor</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <link rel="stylesheet" type="text/css" href=".\css\main_print.css" media="print" />
    <link rel="stylesheet" type="text/css" href=".\css\DatePicker.css" media="screen" />  
    
    
    
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
    <script src="rte/richtext.js" type="text/javascript"></script>
    <script src="rte/config.js" type="text/javascript"></script>
    <script src="js/jquery-1.2.3.min.js" type="text/javascript"></script>
    <script src="js/dhtmlwindow.js" type="text/javascript"></script>
    <script src="js/modal.js" type="text/javascript" ></script>
    <script src="js/numbers.js" type="text/javascript" ></script> 
    
    <script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
    <script type="text/javascript" src="jQuery/jquery-1.4.3.min.js"></script>
    
    <script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>


    <script type="text/javascript">
        function SelectAll(source) {
            if (source.checked) {
                $("#gvSeeding input[name$='chkRowSel']").each(function (index) {
                    $(this).attr('checked', true);
                });
            }
            else {
                $("#gvSeeding input[name$='chkRowSel']").each(function (index) {
                       $(this).attr('checked', false);
                });
            }
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="transactionType" runat="server" />
        <div  style="width: 600px; border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke; clear: both; clip: rect(auto auto auto auto); text-align: left;">
        
        <table style="width: 100%; border-top-style: none; border-right-style: none; border-left-style: none;
            border-bottom-style: none;" id="doc-table">
                    <tr id="trTPL_PanLength" runat="server">
                        <td colspan="2" style="height: 20px; text-align: left; padding-left: 10px;">
                            <asp:TextBox ID="txtFilter" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                            <asp:Button ID="btnFilter" runat="server" Text="FILTER" Width="83px" Height="24px" />
                        </td>
                        <td colspan="2">&nbsp;</td>
                    </tr>
       
                    <tr>
                        <td colspan="4" style="height: 103px; text-align: center;">
                        <div style="padding-right: 2px; padding-left: 2px; padding-bottom: 2px; overflow: auto;
                                width: 100%; padding-top: 2px; height: 478px; background-color: whitesmoke" align="left">
                            <asp:GridView ID="gvSeeding" runat="server" AutoGenerateColumns="False" 
                                BorderStyle="Solid" DataSourceID="sqlDSPromoSeed"
                                BorderWidth="1px" CellPadding="4" ForeColor="#333333" Width="97%" 
                                AllowSorting="True" ShowHeader="True" AllowPaging="True" PageSize="15">
                                
                                <EmptyDataTemplate>
                                    <b>No Seed Added.</b>
                                </EmptyDataTemplate>

                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:BoundField DataField="Branch" HeaderText="Branch" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Branch" >
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="StartDate" DataFormatString="{0:d}" >
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EndDate" HeaderText="End Date" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="EndDate" DataFormatString="{0:d}" >
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MaxNumber" HeaderText="Max Number" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="MaxNumber" >
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Counter" HeaderText="Counter" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Counter" >
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Prize" HeaderText="Prize" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Prize" >
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

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
                    <tr>
                        <td colspan="4" rowspan="1" class="field-cell">
                            <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="100%" style="margin-bottom: 0px !important;  margin-left: 0px !important; height: 100%;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;</td>
                    </tr>
                </table>  
    </div>
    
    </form>
        <asp:SqlDataSource ID="sqlDSPromoSeed" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT SUBSTRING(PS.CompBranch,4,4) +' - '+ B.BranchName AS Branch, PS.StartDate, PS.EndDate, PS.MaxNumber, PS.Counter, PS.Prize FROM PromoSeed PS 
                            LEFT JOIN Branches B
                            ON SUBSTRING(PS.CompBranch,4,4) = RIGHT('0000'+ISNULL(CAST(B.BranchCode AS nvarchar), ''),4)
                            WHERE RequestID = @RequestID 
                                AND (LTRIM(RTRIM(@FilterText)) = '' OR  SUBSTRING(PS.CompBranch,4,4) +' - '+ B.BranchName LIKE '%'+@FilterText+'%')
                            ORDER BY B.BranchName, PS.StartDate" 
        InsertCommand="INSERT INTO PromoSeed(RequestID, StartDate, EndDate, MaxNumber, Counter, Prize) 
                            VALUES (@RequestID, @StartDate, @Enddate, @MaxNumber,@Counter, @Prize)"
        DeleteCommand="DELETE FROM [PromoSeed] WHERE RequestID = @RequestID 
                                                    AND StartDate = @StartDate 
                                                    AND Enddate = @Enddate 
                                                    AND MaxNumber = @MaxNumber 
                                                    AND Counter = @Counter
                                                    AND Prize = @Prize">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="RequestID" />
            <asp:Parameter DefaultValue="" Name="FilterText" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="RequestID" />
            <asp:Parameter Name="StartDate" />
            <asp:Parameter Name="Enddate" />
            <asp:Parameter Name="MaxNumber" />
            <asp:Parameter Name="Counter" />
            <asp:Parameter Name="Prize" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="RequestID" />
            <asp:Parameter Name="StartDate" />
            <asp:Parameter Name="Enddate" />
            <asp:Parameter Name="MaxNumber" />
            <asp:Parameter Name="Counter" />
            <asp:Parameter Name="Prize" />
        </DeleteParameters>
    </asp:SqlDataSource>
</body>


 
</html>
