<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="POSscreen.aspx.vb" Inherits="POSscreen" title="POS File Generation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="menu">
    </div>
    
    <div id="contents">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Trebuchet MS"
            Font-Size="Large" Text="POS File Generation for Class Discount Promotions" Width="524px"></asp:Label>
        <br />
        <br />
        <div class="buttonwrapper" style="width: 46%; height: 39px">
            <a class="boldbuttons" runat="server" href="#" id="A1" style="left: 0px; top: 0px"><span>Generate POS Files</span></a> 
        </div>
        <br />
        <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="True" Width="353px">
            <asp:ListItem Value="0">Newly Generated</asp:ListItem>
            <asp:ListItem Value="1">Already Uploaded</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" style="width: 89%">
            <tr>
                <td align="center" bgcolor="#5d7b9d" style="width: 120px; height: 22px">
                    <strong><span style="color: #ffffff">Promo Start Date</span></strong></td>
                <td align="center" bgcolor="#5d7b9d" style="width: 121px; height: 22px">
                    <strong><span style="color: #ffffff">Memo Number</span></strong></td>
                <td align="center" bgcolor="#5d7b9d" style="width: 158px; height: 22px">
                    <strong><span style="color: #ffffff">POS Files</span></strong></td>
            </tr>
        </table>
        <div style="padding-right: 10px; padding-left: 10px; padding-bottom: 10px; overflow: auto;
            width: 607px; padding-top: 10px; height: 229px; background-color: whitesmoke">
              <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Blue" GridLines="None" Font-Size="9pt" Height="70px" Width="575px" ShowHeader="False">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkAdd" CommandArgument="<%# Container.DataItemIndex %>" runat="server" CausesValidation="False" CommandName="Preview"
                         Font-Size="10pt" Font-Bold="True" ForeColor="Navy">+</asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                    <HeaderStyle Height="10px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Promo Start Date">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UploadDate") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lblUploadDate" CommandArgument="<%# Container.DataItemIndex %>" runat="server"  CommandName = "Download" Text='<%# eval("UploadDate") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                    <HeaderStyle Height="10px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Memo Number">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:GridView ID="gvMemo" runat="server" AutoGenerateColumns="False"
                            ForeColor="Transparent" OnRowCommand="gvMemo_RowCommand" ShowHeader="False" Width="219px" PageSize="3" BorderColor="Transparent" BorderStyle="None">
                            <Columns>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MemoNumber") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Bind("MemoID") %>'
                                            Text='<%# Bind("MemoNumber") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="POS Files">
                    <ItemTemplate>

                        <asp:GridView ID="GVChild" runat="server" AutoGenerateColumns="False" Visible="False" OnRowCommand="GVChild_RowCommand" Font-Size="9pt" ShowHeader="False" Width="180px"  PageSize="3" Height="96px" BorderColor="Transparent" BorderStyle="None">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UploadDate") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="LinkButton1" runat="server" Text='<%# eval("Row") + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle BorderStyle="None" />
                                    <ItemStyle BorderWidth="0px" Width="0px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" HeaderText="Files Uploaded">
                                    <ItemTemplate>
                                        <asp:LinkButton CommandName="Print" ID="LinkButton3" runat="server" Text='<%# eval("FilesUploaded") %>' CommandArgument='<%# bind("MainTableRow") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle BorderStyle="None" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                
                            </EmptyDataTemplate>
                        </asp:GridView>

                    </ItemTemplate>
                    <ItemStyle Width="150px" Wrap="True" />
                    <HeaderStyle Height="10px" />
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
            
        </div>
      
        <asp:Label ID="lblValidateMessage" runat="server" BackColor="White" Font-Bold="True"
            Font-Size="Larger" ForeColor="Green"></asp:Label><p>
            </p>
  
    </div>
            
        

    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT 1"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqldsDBF" runat="server" ConnectionString="" SelectCommand=""></asp:SqlDataSource>
    <div style="display:none;"><asp:Button ID="btnUpdate" runat="server" /></div>

    
</asp:Content>

