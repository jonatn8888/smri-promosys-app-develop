<%@ Page Language="VB" AutoEventWireup="false" EnableEventValidation="false" CodeFile="ViewDetailsComparison.aspx.vb"
    Inherits="ViewDetailsComparison" Title="Search Branch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Details Comparison</title>
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)" />
    <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)" />
    <link rel="stylesheet" type="text/css" href="./css/main.css" media="screen" />

    <script src="js/jquery-1.2.3.min.js" type="text/javascript"></script>

    <script src="js/numbers.js" type="text/javascript"></script>

</head>
<body class="search" style="background: white;">
    <form id="SearchUPC" runat="server">
        <div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 688px; height: 176px" >
                        <asp:GridView ID="gridviewresult" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                            BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="1000px"
                            Font-Size="Small" AllowPaging="false">
                            <EmptyDataTemplate>
                                <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 700px;
                                    height: 82px">
                                    <tr>
                                        <td style="height: 82px; text-align: center" valign="middle">
                                            <em><span style="font-size: 11pt"><strong>no record found for this category.</strong></span></em></td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                                <asp:BoundField DataField="MemoNumber" HeaderText="Memo No." SortExpression="MemoNumber">
                                    <ItemStyle Width="120px" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Width="330px" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                                    SortExpression="PromoPeriodFrom">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PromoPeriodEnd" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                                    SortExpression="PromoPeriodTo">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle CssClass="ClippedRegion" Wrap="False" Width="180px" />
                                    <ControlStyle CssClass="ClippedRegion" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
