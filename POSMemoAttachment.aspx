<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="POSMemoAttachment.aspx.vb" Inherits="POSMemoAttachment" title="POS Memo Attachment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script language="javascript" type="text/javascript" src="js/DatePicker.js"></script>
    <div id="menu">
    </div>
    
    <div id="contents">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Trebuchet MS"
            Font-Size="Large" Text="Other Promotions" Width="524px"></asp:Label><br />
        <br />
        <div style="width: 750px; height: 21px">
            <table style="width: 947px">
                <tr>
                    <td>
                        &nbsp;<asp:Label ID="Label2" runat="server" Text="Date From: "></asp:Label>
                        <asp:TextBox ID="txtPeriodFrom" runat="server" MaxLength="50" Width="177px"></asp:TextBox>
                        <input id="calPeriodFrom" class="btnCal" name="calPeriodFrom" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodFrom');"
                            style="left: 322px; top: 345px" type="button" />
                        &nbsp; &nbsp;
                        <asp:Label ID="Label3" runat="server" Text="Date To: "></asp:Label>
                        <asp:TextBox ID="txtPeriodTo" runat="server" MaxLength="50" Width="177px"></asp:TextBox>
                        <input id="calPeriodTo" class="btnCal" name="calPeriodTo" onclick="displayDatePicker('ctl00_ContentPlaceHolder1_txtPeriodTo');"
                            style="left: 547px; top: 345px" type="button" /></td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 578px">
                            <tr>
                                <td align="left" colspan="3" style="height: 20px">
                                    <asp:Label ID="lblValidate" runat="server" BackColor="Transparent" BorderColor="Transparent"
                                        ForeColor="Red" Text="Label"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                        <asp:Button ID="btnView" runat="server" Text="View" /></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="3">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <br />
        <br />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 122%">
            <tr>
                <td align="left" bgcolor="#5d7b9d" style="width: 62px; height: 22px">
                    <strong><span style="color: #ffffff">Promo Start Date</span></strong></td>
                <td align="left" bgcolor="#5d7b9d" style="width: 64px; height: 22px">
                    <strong><span style="color: #ffffff">Memo Number</span></strong></td>
                <td align="left" bgcolor="#5d7b9d" style="width: 158px; height: 22px">
                    <strong><span style="color: #ffffff">Attached Files</span></strong></td>
            </tr>
        </table>
        <div style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px; overflow: auto;
            width: 842px; padding-top: 10px; height: 229px; background-color: whitesmoke">
              <asp:GridView ID="gvPOSMemoWithAttachments" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Blue" GridLines="None" Font-Size="9pt" Height="70px" Width="818px" ShowHeader="False">
            <Columns>
                <asp:TemplateField HeaderText="Promo Period">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UploadDate") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;<asp:LinkButton ID="lnkPromoPeriodFrom" runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                            Text='<%# Eval("PromoPeriodFrom","{0:dd-MMM-yyyy}") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="250px" />
                    <HeaderStyle Height="10px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Memo Number">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:GridView ID="gvMemoNumbers" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                            Width="701px" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="RequestID" Visible="False" >
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MemoNumber") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblMemoNumber" runat="server" Text='<%# Bind("MemoNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                            Width="572px" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("AttachedFiles") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lnkAttachedFile" runat="server" Text='<%# Bind("AttachedFiles") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Top" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle BackColor="#E3EAEB" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </ItemTemplate>
                    <ItemStyle VerticalAlign="Top" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle BorderStyle="None" />
            <EditRowStyle BorderStyle="None" BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                  <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                  <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                  <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                  <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                  <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
            &nbsp;
        </div>
        <asp:Label ID="lblValidateMessage" runat="server" BackColor="White" Font-Bold="True"
            Font-Size="Larger" ForeColor="Green"></asp:Label><p>
            </p>
  
    </div>
            
        

    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT 1"></asp:SqlDataSource>

    <asp:SqlDataSource ID="sqldsDBF" runat="server" ConnectionString="" SelectCommand=""></asp:SqlDataSource>

    
</asp:Content>

