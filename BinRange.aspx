<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BinRange.aspx.vb" Inherits="BinRange" title="Untitled Page" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Text Editor</title>
    <link rel="stylesheet" type="text/css" href="./css/main.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="./css/main_print.css" 
        media="print" />
    <link rel="stylesheet" type="text/css" href="./css/DatePicker.css" 
        media="screen" />  
    
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

    <%--<link rel="stylesheet" type="text/css" href="">--%>
    <script type="text/javascript">
    $('document').ready(function() {
        checkChanged();
    })
        function SelectAll(source) {
            if (source.checked) {
                $("#gvBinRange input[name$='chkRowSel']").each(function (index) {
                    $(this).attr('checked', true);
                });
            }
            else {
                $("#gvBinRange input[name$='chkRowSel']").each(function (index) {
                       $(this).attr('checked', false);
                });
            }
        }
        
        function checkChanged() {  
            var ckIsManual = document.getElementById('<%= chkXML_IsManual.ClientID %>');
            var trBinrange = document.getElementById('<%= trTPL_Bin.ClientID %>');
            var trPanLow = document.getElementById('<%= trPL_PanLow.ClientID %>');
            var trPanHigh = document.getElementById('<%= trTPL_PanHigh.ClientID %>');
            var trPanLength = document.getElementById('<%= trTPL_PanLength.ClientID %>');
            var trIssuer = document.getElementById('<%= tr_Issuer.ClientID %>');
            var trCardBrand = document.getElementById('<%= tr_CardBrand.ClientID %>');
            if(ckIsManual.checked)
            {
                trBinrange.style.display = 'block';
                trPanLow.style.display = 'block';
                trPanHigh.style.display = 'block';
                trPanLength.style.display = 'block';
                
                trIssuer.style.display = 'none';
                trCardBrand.style.display = 'none';
                
            }
            else
            {
                trBinrange.style.display = 'none';
                trPanLow.style.display = 'none';
                trPanHigh.style.display = 'none';
                trPanLength.style.display = 'none';
                
                trIssuer.style.display = 'block';
                trCardBrand.style.display = 'block';
            }
        
        
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 173px;
            height: 20px;
        }
        .style2
        {
            height: 20px;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="transactionType" runat="server" />
        <div  style="width: 600px; height: 509px;border-right: steelblue 1px solid; padding-right: 4px; border-top: steelblue 1px solid; padding-left: 4px; padding-bottom: 4px; border-left: steelblue 1px solid; padding-top: 4px; border-bottom: steelblue 1px solid; background-color: whitesmoke; clear: both; clip: rect(auto auto auto auto); text-align: left;">
        
        <table style="width: 100%; border-top-style: none; border-right-style: none; border-left-style: none;
            border-bottom-style: none;" id="doc-table">
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                     <tr id="trXML_IsManual" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">
                            Manual:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:CheckBox ID="chkXML_IsManual" runat="server" Text="" onclick="checkChanged()" />
                        </td>
                    </tr>
                    <tr id="trXML_RangeType" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">
                            Range Type:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:DropDownList ID="cboXML_RangeType" runat="server" Width="260px" 
                                AppendDataBoundItems="True" DataTextField="ElementName" 
                                DataValueField="ElementValue" AutoPostBack="False">
                                <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="tr_CardBrand" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">
                            Card Brand:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:DropDownList ID="cbo_CardBrand" runat="server" Width="260px" 
                                AppendDataBoundItems="True" DataTextField="ElementName" 
                                DataValueField="ElementValue" AutoPostBack="False">
                                <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="tr_Issuer" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">
                            Issuer:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:DropDownList ID="cbo_Issuer" runat="server" Width="260px" 
                                AppendDataBoundItems="True" DataTextField="ElementName" 
                                DataValueField="ElementValue" AutoPostBack="False">
                                <asp:ListItem Value="-1">- - Select Value - -</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
     
                    <tr id="trTPL_Bin" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">
                            BIN:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:TextBox ID="txtBin" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                    </tr>
                    <tr id="trPL_PanLow" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">
                            Pan Low:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:TextBox ID="txtPanLow" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                    </tr>
                    <tr id="trTPL_PanHigh" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">Pan High:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:TextBox ID="txtPanHigh" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                    </tr>
                    <tr id="trTPL_PanLength" runat="server">
                        <td style="width: 173px; height: 20px; text-align: left; padding-left: 60px;" valign="middle">Pan Length:</td>
                        <td style="height: 20px; text-align: left">
                            <asp:TextBox ID="txtPanLength" runat="server" MaxLength="50" Width="256px"></asp:TextBox></td>
                    </tr>
                    <tr id="trTPL_linkSeeding" runat="server">
                        <td> &nbsp; </td>
                        <td style="padding-right: 10px;">
                            <asp:LinkButton style="padding-right: 10px;" ID="LinkButton1" runat="server" CssClass="action-link" Enabled="True">Remove Bin Range</asp:LinkButton>
                            <asp:LinkButton style="padding-right: 10px;" ID="linkBinRange" runat="server" CssClass="action-link" Enabled="True">Add Bin Range</asp:LinkButton>
                        </td>
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
                                    
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRowSel" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="45px" HorizontalAlign="Center" />
                                        <HeaderTemplate >
                                            <asp:CheckBox ID="chkALL" runat="server" onclick="SelectAll(this)" />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
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
                        <td>
                            <tr>
                                <td>&nbsp; &nbsp;</td>
                                <td style="text-align: right">
                                     <asp:Button ID="cmdPopOK" runat="server" Text="OK" Width="83px" />
                                </td>
                            </tr>
                        </td>
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
