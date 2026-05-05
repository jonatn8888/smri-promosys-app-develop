<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PromoRequestBinRange.aspx.vb" Inherits="PromoRequestBinRange" title="Untitled Page" %>


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
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
       
                    <tr>
                        <td colspan="2" style="height: 103px; text-align: center;">
                        <div style="padding-right: 2px; padding-left: 2px; padding-bottom: 2px; overflow: auto;
                                width: 100%; padding-top: 2px; height: 250px; background-color: whitesmoke" align="left">
                            <asp:GridView ID="gvBinRange" runat="server" AutoGenerateColumns="False" BorderStyle="Solid" DataSourceID="sqlDSPromoBinRange"
                                BorderWidth="1px" CellPadding="4" ForeColor="#333333" Width="97%" AllowSorting="True" ShowHeader="True">
                                <EmptyDataTemplate>
                                    <b>No Bin Range Added.</b>
                                </EmptyDataTemplate>

                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:BoundField DataField="ElementName" HeaderText="Range Type"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="ElementName" >
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BIN" HeaderText="Bin" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="Bin" >
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PanLow" HeaderText="Pan Low" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLow">
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PanHigh" HeaderText="Pan High" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanHigh" >
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PanLength" HeaderText="Pan Length" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" SortExpression="PanLength" >
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
                        <td colspan="2" rowspan="1" class="field-cell">
                            <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="100%" style="margin-bottom: 0px !important;  margin-left: 0px !important; height: 100%;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                </table>  
    </div>
    
    </form>
        <asp:SqlDataSource ID="sqlDSPromoBinRange" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT RLV.ElementName, PBR.BIN, PBR.PanLow, PBR.PanHigh, PBR.PanLength 
            FROM PromoBinRange PBR
            LEFT JOIN ResListValues RLV
            ON PBR.RangeType = RLV.ElementID
                AND RLV.GroupName = 'RangeType'  
            WHERE RequestID = @RequestID" 
        InsertCommand="INSERT INTO PromoBinRange(RequestID, RangeType, BIN, PanLow, PanHigh, PanLength) 
                            VALUES (@RequestID, @RangeType, @BIN, @PanLow, @PanHigh,@PanLength)"
        DeleteCommand="DELETE PBR
                        FROM PromoBinRange PBR
                        LEFT JOIN  ResListValues RLV
                        ON PBR.RangeType = RLV.ElementID
                        AND RLV.GroupName = 'RangeType'   
                        WHERE RequestID = @RequestID 
                            AND RLV.ElementName = @RangeType 
                            AND BIN = @BIN 
                            AND PanLow = @PanLow 
                            AND PanHigh = @PanHigh 
                            AND ISNULL(PanLength,'0') = @PanLength">
        <SelectParameters>
            <asp:Parameter DefaultValue="0" Name="RequestID" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="RequestID" />
            <asp:Parameter Name="RangeType" />
            <asp:Parameter Name="BIN" />
            <asp:Parameter Name="PanLow" />
            <asp:Parameter Name="PanHigh" />
            <asp:Parameter Name="PanLength" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="RequestID" />
            <asp:Parameter Name="RangeType" />
            <asp:Parameter Name="BIN" />
            <asp:Parameter Name="PanLow" />
            <asp:Parameter Name="PanHigh" />
            <asp:Parameter Name="PanLength" />
        </DeleteParameters>
    </asp:SqlDataSource>

</body>
</html>
