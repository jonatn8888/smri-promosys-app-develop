<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="RCDPReqList.aspx.vb" ValidateRequest="false" Inherits="RCDPReqList" Title="RCDP Request List" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
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
</script>
<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>

<table>
    <tr>
        <td rowspan="3" style="width: 20px">
        </td>
        <td style="FONT-SIZE: 16px; COLOR: darkolivegreen; FONT-FAMILY: 'Trebuchet MS'; font-weight: bold; height: 40px;">
			Requests for <asp:Label ID="lblRequstTypeDesc" runat="server"></asp:Label>
        </td>
        <td rowspan="3">
           <div id="menu" style="width: 173px">
               <table>
                   <tr>
                       <td style="height: 40px">
                       </td>
                   </tr>
                    <tr>
                        <td style="height: 20px">
                    <asp:LinkButton ID="lnkCreateNewRequest" runat="server" Enabled="False">Create New Request</asp:LinkButton>&nbsp;</td>
                    </tr>
                   <tr>
                       <td style="height: 20px">
                <hr />
                    &nbsp;&nbsp;</td>
                   </tr>
                    <tr>
                        <td style="height: 30px">
                            <asp:LinkButton ID="lnkCancellation" runat="server" Width="152px">Cancellation</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px">
                            <asp:LinkButton ID="lnkExtension" runat="server" Width="152px">Extension</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px">
                            <asp:LinkButton ID="lnkSwipestakesReseeding" runat="server" Width="152px">Swipestakes Reseeding</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px"> </td>
                    </tr>
                </table>
           </div>
        </td>
    </tr>
    <tr>
        <td style="height: 40px">
        <table>
            <tr>
                <td colspan="6">
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="500px">
                    </asp:BulletedList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="cboFilterStatus" runat="server" AutoPostBack="True" Width="239px">
                    </asp:DropDownList>
                </td>
                <td style="width: 112px; text-align: right">
                    From</td>
                <td>
                    <asp:TextBox ID="txtFrom" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    <input
                        id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtFrom');"
                        style="left: 322px; top: 345px" type="button" /></td>
                <td style="text-align: right">
                    To</td>
                <td>
                    <asp:TextBox ID="txtTo" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    <input
                        id="calPeriodTo" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtTo');"
                        style="left: 322px; top: 345px" type="button" /></td>
                <td>
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" /></td>
            </tr>
        </table>
	    </td>
    </tr>
    <tr>
        <td>
        <asp:GridView ID="gridRequests" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataKeyNames="RequestID" DataSourceID="sqldsRequests" ForeColor="#333333" GridLines="Vertical"
            Width="694px" AllowSorting="True" BorderWidth="1px">

            <EmptyDataTemplate>
                <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 600px; height: 82px">
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
                        <asp:Label ID="lblCRID" runat="server" Text='<%# Eval("CRID") %>'></asp:Label>
                        <asp:Label ID="lblMemoID" runat="server" Text='<%# Eval("MemoID") %>'></asp:Label>
                        <asp:Label ID="lblRequestID" runat="server" Text='<%# Eval("RequestID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="True" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="60px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="MemoNumber" HeaderText="Memo No">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="120px" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="Title" HeaderText="Promo Title" SortExpression="Title" >
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="EffectDate" HeaderText="Effectivity Date" SortExpression="EffectDate" DataFormatString="{0:MM/dd/yyyy}" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                    <ItemStyle Width="120px" />
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
                <asp:SqlDataSource ID="sqldsRequests" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            DeleteCommand="DELETE FROM [PromoRequests] WHERE [RequestID] = @RequestID" InsertCommand="INSERT INTO [PromoRequests] ([RequestDate], [Title], [PromoPeriodFrom], [PromoPeriodTo], [PreAppBy], [ApprovedBy], [DateApproved], [RequestBy], [Remarks]) VALUES (@RequestDate, @Title, @PromoPeriodFrom, @PromoPeriodTo, @PreAppBy, @ApprovedBy, @DateApproved, @RequestBy, @Remarks)" UpdateCommand="UPDATE [PromoRequests] SET [RequestDate] = @RequestDate, [Title] = @Title, [PromoPeriodFrom] = @PromoPeriodFrom, [PromoPeriodTo] = @PromoPeriodTo, [PreAppBy] = @PreAppBy, [ApprovedBy] = @ApprovedBy, [DateApproved] = @DateApproved, [RequestBy] = @RequestBy, [Remarks] = @Remarks WHERE [RequestID] = @RequestID">
            <DeleteParameters>
                <asp:Parameter Name="RequestID" Type="Int16" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter DbType="Date" Name="RequestDate" />
                <asp:Parameter Name="Title" Type="String" />
                <asp:Parameter DbType="Date" Name="PromoPeriodFrom" />
                <asp:Parameter DbType="Date" Name="PromoPeriodTo" />
                <asp:Parameter Name="PreAppBy" Type="String" />
                <asp:Parameter Name="ApprovedBy" Type="String" />
                <asp:Parameter DbType="Date" Name="DateApproved" />
                <asp:Parameter Name="RequestBy" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
                <asp:Parameter Name="RequestID" Type="Int16" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter DbType="Date" Name="RequestDate" />
                <asp:Parameter Name="Title" Type="String" />
                <asp:Parameter DbType="Date" Name="PromoPeriodFrom" />
                <asp:Parameter DbType="Date" Name="PromoPeriodTo" />
                <asp:Parameter Name="PreAppBy" Type="String" />
                <asp:Parameter Name="ApprovedBy" Type="String" />
                <asp:Parameter DbType="Date" Name="DateApproved" />
                <asp:Parameter Name="RequestBy" Type="String" />
                <asp:Parameter Name="Remarks" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:HiddenField ID="lblPopTitle" runat="server" />
         <div style="display:none">
        <asp:Button id="cmdPopUpOK" runat="server" Text="Button"></asp:Button>\
        </div>
        </td>
    </tr>
</table>
</asp:Content>