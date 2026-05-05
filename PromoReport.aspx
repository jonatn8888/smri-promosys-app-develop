<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    CodeFile="PromoReport.aspx.vb" Inherits="PromoReport" Title="Reports" enableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
    <script type="text/javascript">
    function openViewComparison(PromoDate,TargetBranch,ToBranch,WinHeader)
    {
        var url
        url = "ViewDetailsComparison.aspx?PromoDate=" + PromoDate + "&TargetBranch=" +TargetBranch+ "&ToBranch="+ToBranch;
        viewWindow = dhtmlmodal.open('EmailBox', 'iframe', url, WinHeader, 'width=1000px,height=500px,center=1,resize=0,scrolling=0', "recall")
        viewWindow.onclose=function()
        {
	        return true
	    }
    }
    </script>

    <script type="text/javascript">
    function openSearchStore()
    {
        var url
        url = "SearchStore.aspx";
        searchWindow = dhtmlmodal.open('EmailBox', 'iframe', url, 'Search Store', 'width=725px,height=400px,center=1,resize=0,scrolling=0', "recall")
        searchWindow.onclose=function()
        {
            var theform = this.contentDoc.forms[0]
            var theStoreNos = this.contentDoc.getElementById("hidOutputValue")
            var theStoreDesc = this.contentDoc.getElementById("hidOutputAll")
            document.getElementById("<%=hfStorecodes.ClientID%>").value = theStoreNos.value;
            document.getElementById("<%=hfStoreDesc.ClientID%>").value = theStoreDesc.value;     
            //document.getElementById("<%=btnRefresh.ClientID%>").click();
            document.getElementById("<%=btnBranches.ClientID%>").click();       

	        return true
	    }
    }
    </script>

    <script type="text/javascript">
    function openSearchBranch()
    {
        var url
        url = "SearchBranch.aspx";
        searchWindow = dhtmlmodal.open('EmailBox', 'iframe', url, 'Search Branch', 'width=725px,height=400px,center=1,resize=0,scrolling=0', "recall")
        searchWindow.onclose=function()
        {
            var theform = this.contentDoc.forms[0]
            var theStoreNos = this.contentDoc.getElementById("hidOutputValue")
            document.getElementById("<%=hfStorecodes.ClientID%>").value = theStoreNos.value;
            //document.getElementById("<%=btnRefresh.ClientID%>").click();
            document.getElementById("<%=btnBranches.ClientID%>").click();                        
	        return true
	    }
    }
    </script>

    <script type="text/javascript">
    function openSearchPromoType()
    {
        var url
        url = "SearchPromoType.aspx";
        searchWindow = dhtmlmodal.open('EmailBox', 'iframe', url, 'Search Promo Type', 'width=725px,height=400px,center=1,resize=0,scrolling=0', "recall")
        searchWindow.onclose=function()
        {
            var theform = this.contentDoc.forms[0]
            var thePromoTypeID = this.contentDoc.getElementById("hidOutputValue")
            var thePromoDesc = this.contentDoc.getElementById("hidOutputAll")
            document.getElementById("<%=hfPromotypes.ClientID%>").value = thePromoTypeID.value;
            document.getElementById("<%=hfPromoDesc.ClientID%>").value = thePromoDesc.value;
            //document.getElementById("<%=btnRefresh.ClientID%>").click();
            document.getElementById("<%=btnPromoTypes.ClientID%>").click();            
	        return true
	    }
    }
    </script>

    <div id="contents" style="width: 97%; margin-right: 10px;">
        <strong><span style="font-size: 11pt; color: #556b2f">Report Type<br />
            <br />
        </span></strong>
        <asp:DropDownList ID="ddlReportType" runat="server" Width="406px" AutoPostBack="True" Font-Names="Trebuchet MS" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
            <asp:ListItem Value="0">-- Select Value --</asp:ListItem>
            <asp:ListItem Value="1">Branch Comparison of Ongoing Promotions</asp:ListItem>
            <asp:ListItem Value="2">Ongoing &amp; Future Dated Promotions</asp:ListItem>
            <asp:ListItem Value="3">Approved Promotions on a Given Date</asp:ListItem>
            <asp:ListItem Value="4">Count of Active Promo IDs for Regular Price Items only</asp:ListItem>
            <asp:ListItem Value="5">Promotions for Manual Processing</asp:ListItem>
            <asp:ListItem Value="6">Promo Exclusion List for Automated Loyalty & Coupon Promos</asp:ListItem>
        </asp:DropDownList><br />
        <br />
        <table id="tableFilterParam" runat="server" visible="false" style="width: 881px">
            <tr id="trPromoType" runat="server">
                <td align="right" style="height: 27px;width:60px">
                    <asp:LinkButton ID="lnkPromoType" runat="server" CssClass="action-link" Font-Size="Small" Font-Names="Trebuchet MS">PromoType:</asp:LinkButton>
                </td>
                <td style="height: 27px;width:350px">
                    <asp:TextBox ID="txtPromoType" runat="server" Width="340px" ReadOnly="True" TextMode="MultiLine" Font-Size="Small" Font-Names="Trebuchet MS"></asp:TextBox>&nbsp;
                 </td>
            </tr>
            <tr>
                <td id="tdlnkBranch" runat="server" align="right" style="height: 27px; width: 60px">
                    <asp:LinkButton ID="lnkBranch" runat="server" CssClass="action-link" Font-Size="Small"
                        Font-Names="Trebuchet MS">Branch:</asp:LinkButton></td>
                <td id="tdtxtBranch" runat="server" style="height: 27px; width: 350px">
                    <asp:TextBox ID="txtSearchStore" runat="server" Width="340px" ReadOnly="True" TextMode="MultiLine"
                        Font-Size="Small" Font-Names="Trebuchet MS"></asp:TextBox>
                </td>
                <td id="tdlblType" runat="server" align="right" style="height: 27px; width: 60px">
                    <asp:Label ID="lblType" runat="server" Text="Type:" Font-Names="Trebuchet MS"></asp:Label>                        
                </td>
                <td id="tdddlType" runat="server" style="height: 27px; width: 350px">
                    <asp:DropDownList ID="ddlType" runat="server" Width="335px" Font-Names="Trebuchet MS">
                        <asp:ListItem Value="0">For Inclusion to List</asp:ListItem>
                        <asp:ListItem Value="1">For Removal from Exclusion</asp:ListItem>
                    </asp:DropDownList>
                </td>               
                <td align="left" style="height: 27px; width: 200px">
                    <asp:Label ID="lblTargetDate" runat="server" Text="Target Date:" Font-Names="Trebuchet MS"></asp:Label>
                    <asp:TextBox ID="txtTargetDate" runat="server" MaxLength="10" Width="80px" Font-Names="Trebuchet MS"></asp:TextBox>
                    <input id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtTargetDate');"
                        style="left: 322px; top: 345px" type="button" />
                </td>
                <td align="left" style="height: 27px; width: 200px">
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" Font-Names="Trebuchet MS" Height="25px" Width="64px" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print" Width="64px" Enabled="False" Height="25px"/></td>
            </tr>
        </table>
        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="500px">
        </asp:BulletedList>
        <br />
        <asp:Panel ID="panOption1" runat="server" Visible="False">
            <asp:GridView ID="gridOption1" runat="server" AutoGenerateColumns="False"
                BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="500px"
                Font-Size="Small" Font-Names="Trebuchet MS">
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
                    <asp:BoundField DataField="BranchCode" HeaderText="Code" SortExpression="BranchCode">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="25%" />                       
                    </asp:BoundField>
                    <asp:TemplateField  HeaderText="Source Branch" SortExpression="BranchName">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# eval("BranchCode") & "-" & eval("BranchName") %>'
                                CommandName="Select" Text='<%# Bind("BranchName") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass="ClippedRegion" Width="50%" HorizontalAlign="Center" />
                    </asp:TemplateField>           
                    <asp:BoundField DataField="BranchPercent" HeaderText="Similarity" SortExpression="BranchPercent"
                        DataFormatString="{0:P}">
                        <ItemStyle HorizontalAlign="Center" Width="25%" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        <asp:Panel ID="panOption2" runat="server" Visible="False">
            <asp:GridView ID="gridOption2" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="880px"
                Font-Size="Smaller" AllowPaging="false" Font-Names="Trebuchet MS">
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
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MemoNumber" HeaderText="Memo No." SortExpression="MemoNumber">
                        <ItemStyle Width="105px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="260px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoType" HeaderText="Promo Type" SortExpression="PromoType">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="120px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass="ClippedRegion" Wrap="False" Width="125px" />
                        <ControlStyle CssClass="ClippedRegion" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                        SortExpression="PromoPeriodFrom">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                        SortExpression="PromoPeriodTo">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProcessType" HeaderText="Process" SortExpression="ProcessType">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="120px" Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        <asp:Panel ID="panOption3" runat="server" Visible="False">
            <asp:GridView ID="gridOption3" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="880px"
                Font-Size="Smaller" AllowPaging="false" Font-Names="Trebuchet MS">
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
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="MemoNumber" HeaderText="Memo No." SortExpression="MemoNumber">
                        <ItemStyle Width="105px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="220px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoType" HeaderText="Promo Type" SortExpression="PromoType">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="80px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass="ClippedRegion" Wrap="False" Width="132px" />
                        <ControlStyle CssClass="ClippedRegion" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From / Effective"
                        SortExpression="PromoPeriodFrom">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="End Date"
                        SortExpression="PromoPeriodTo">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BrCount" HeaderText="Branch Count" SortExpression="BrCount"
                        DataFormatString="{0:###,###.##}">
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UPCCount" HeaderText="Record Count" SortExpression="UPCCount"
                        DataFormatString="{0:###,###.##}">
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TotCount" HeaderText="Total Count" SortExpression="TotCount"
                        DataFormatString="{0:###,###.##}">
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        <asp:Panel ID="panOption4" runat="server" Visible="False">
            <asp:GridView ID="gridOption4" runat="server" AutoGenerateColumns="False"
                BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="500px"
                Font-Size="Small" Font-Names="Trebuchet MS">
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
                    <asp:TemplateField HeaderText="Code" SortExpression="BranchCode">
                        <ItemTemplate>
                            <asp:Label ID="lblBranchCode" runat="server" Text='<%# Bind("BranchCode") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="25%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="BranchName" HeaderText="Branch Name" SortExpression="BranchName">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ControlStyle CssClass="ClippedRegion" />
                        <ItemStyle CssClass="ClippedRegion" HorizontalAlign="Center" Width="50%" Wrap="False" />                        
                    </asp:BoundField>
                    <asp:BoundField DataField="MemoCount" HeaderText="Memos" SortExpression="MemoCount">
                        <ItemStyle HorizontalAlign="Center" Width="25%" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>                
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        <asp:Panel ID="panOption5" runat="server" Visible="False">
            <asp:GridView ID="gridOption5" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="880px"
                Font-Size="Smaller" AllowPaging="false" Font-Names="Trebuchet MS">
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
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                
                    <asp:BoundField DataField="Seqno" HeaderText="SeqNo" SortExpression="Seqno">
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MemoNumber" HeaderText="Memo No." SortExpression="MemoNumber">
                        <ItemStyle Width="105px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="260px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Promo Type" HeaderText="Promo Type" SortExpression="Promo Type">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="120px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Period From" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                        SortExpression="Period From">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Period To" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                        SortExpression="Period To">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass="ClippedRegion" Wrap="False" Width="125px" />
                        <ControlStyle CssClass="ClippedRegion" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Date Approved" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Date Approved"
                        SortExpression="Date Approved">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        <asp:Panel ID="panOption6" runat="server" Visible="False">
            <asp:GridView ID="gridOption6" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                BorderWidth="1px" CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="880px"
                Font-Size="Smaller" AllowPaging="false" Font-Names="Trebuchet MS">
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
                    <asp:BoundField DataField="PosPromoID" HeaderText="PromoID" SortExpression="PosPromoID">
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MemoNumber" HeaderText="Memo No." SortExpression="MemoNumber">
                        <ItemStyle HorizontalAlign="Center" Width="105px" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Width="260px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                        SortExpression="PeriodFrom">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                        SortExpression="PeriodTo">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Branches" HeaderText="Branches" SortExpression="Branches">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle CssClass="ClippedRegion" Wrap="False" Width="125px" />
                        <ControlStyle CssClass="ClippedRegion" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateApproved" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Date Approved"
                        SortExpression="DateApproved">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="65px" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </asp:Panel>
        <br />
        <div>
            <hr />
        </div>
        <asp:HiddenField ID="hfStorecodes" runat="server" />
        <%--<asp:HiddenField ID="hfStorename" runat="server" />--%>
        <asp:HiddenField ID="hfStoreNo" runat="server" />
        <asp:HiddenField ID="hfRepParams" runat="server" />
        <asp:HiddenField ID="hfPromotypes" runat="server" />
        <asp:HiddenField ID="hfPromoTypeId" runat="server" />
        
        <asp:HiddenField ID="hfPromoDesc" runat="server" />
        <asp:HiddenField ID="hfStoreDesc" runat="server" />
        
        <div style="display: none">
            <asp:Button ID="btnPromoTypes" runat="server"/>
            <asp:Button ID="btnBranches" runat="server"/>
        </div>
    </div>
</asp:Content>
