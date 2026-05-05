<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="CreateTransactionTemplate.aspx.vb" Inherits="CreateTransactionTemplate" title="Search Transaction Template" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents">
    <div id="SectionHeader" style="padding-right: 10px; padding-left: 10px; font-size: 16px;
        padding-bottom: 5px; width: 80%; color: white; padding-top: 5px; font-family: Tahoma;
        background-color: royalblue">
        <strong>
        Search Transaction for Template </strong>
    </div>
    <br />
    <table style="width: 546px; border-left-color: white; border-bottom-color: white; border-top-style: none; border-top-color: white; border-right-style: none; border-left-style: none; border-right-color: white; border-bottom-style: none;">
        <tr>
            <td style="width: 38px">
            </td>
            <td style="color: darkblue ; font-style: italic; font-variant: small-caps">
                <strong>
                Search Text</strong></td>
            <td style="color: darkblue; font-style: italic; font-variant: small-caps">
                <strong>
                Criteria</strong></td>
            <td style="width: 3px">
            </td>
        </tr>
        <tr>
            <td style="width: 38px">
                &nbsp; &nbsp; &nbsp; &nbsp;</td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" Width="292px"></asp:TextBox></td>
            <td style="width: 177px">
                <asp:DropDownList ID="ddlCriteria" runat="server" Width="173px">
                    <asp:ListItem>Title</asp:ListItem>
                    <asp:ListItem>Request ID</asp:ListItem>
                </asp:DropDownList></td>
            <td style="width: 3px">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/search_btn.jpg" /></td>
        </tr>
        <tr>
            <td colspan="1" style="width: 38px">
            </td>
            <td colspan="3">
                <asp:Label ID="lblValidateMessage" runat="server" BackColor="White" Font-Bold="True"
                    Font-Size="Larger" ForeColor="Green"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="1" style="width: 38px; height: 16px">
            </td>
            <td colspan="3" style="height: 16px">
            </td>
        </tr>
    </table>
    &nbsp;
    <table style="width: 458px">
        <tr>
            <td style="width: 3px; color: white">
                mau&nbsp;
            </td>
            <td>
    <asp:GridView ID="gridRequests" runat="server" AutoGenerateColumns="False"
        BorderWidth="1px" CellPadding="4" DataKeyNames="RequestID" ForeColor="#333333"
        GridLines="Vertical" Width="694px">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label ID="lblRequestID" runat="server" Text='<%# Eval("RequestID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:BoundField DataField="RequestID"  HeaderText="PR ID"
                SortExpression="RequestID">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" Width="70px" />
            </asp:BoundField>
      
            <asp:BoundField DataField="RequestDate" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Request Date"
                ReadOnly="True" SortExpression="RequestDate">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" Width="80px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Promo Title" SortExpression="Title">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# eval("RequestID") %>'
                        Text='<%# eval("Title") %>'></asp:LinkButton>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="PromoPeriodFrom" DataFormatString="{0:MM-dd-yyyy}" HeaderText="From"
                SortExpression="PromoPeriodFrom">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" Width="70px" />
            </asp:BoundField>
            <asp:BoundField DataField="PromoPeriodTo" DataFormatString="{0:MM-dd-yyyy}" HeaderText="To"
                SortExpression="PromoPeriodTo">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" Width="70px" />
            </asp:BoundField>
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle Width="120px" />
            </asp:BoundField>
            <asp:TemplateField Visible="False">
                <ItemTemplate>
                    <asp:Label ID="PromoID" runat="server" Text='<%# Eval("PromoID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <EmptyDataTemplate>
            <table border="1" bordercolor="tan" cellpadding="3" cellspacing="0" style="width: 690px;
                height: 82px">
                <tr>
                    <td style="height: 82px; text-align: center" valign="middle">
                        <em><span style="font-size: 11pt"><strong>no record found for this category.</strong></span></em></td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 3px">
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
    <br />
    <div style="display:none">
        &nbsp;</div>
    </div>
</asp:Content>

