<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="CompBranchesMaintenance.aspx.vb" Inherits="CompBranchesMaintenance" title="Company Branch Maintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<br />
<br />
    <div id="menu">
        <br />
        <br />
        <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" /><br />
        <br /><asp:Button ID="btnSave" runat="server" Text="Save" Width="119px" />
        <br />
        <br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="119px" /><br />
        <br /><asp:Button ID="btnDelete" runat="server" Text="Delete" Width="119px" Visible="False" />
        <br />
    </div>
        <div id="contents">
            <span style="font-size: 12pt">
            <strong>&nbsp;Store Maintenance</strong></span><br /><hr />
        </div>
                
<asp:Panel ID="panPopUp" runat="server" CssClass="MainFrame" Visible="False">
   <div id='popupMask' class='PageMask' style="left: 0px; top: 0px"></div>
   
   <div id='sample' class="popupWindow" style="left: 150px; width: 300px; top: 200px; height: 134px;">
        <table cellpadding='0' cellspacing='0' border='0' style="width:  473px;">
            <tr>
                <td style="background: url('./images/x11_title.gif'); height: 23px; width: 23px;"></td>
                <td style="background: url('./images/x11_bar.gif') repeat top left; height: 23px; width: 321px; text-align:left;" valign="middle">
                    &nbsp;<asp:Label ID="lblPopTitle" runat="server" Font-Bold="False" Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="White" Text="Warning" Width="384px"></asp:Label>
                </td>
                <td style="width: 15px; height: 23px;"><asp:ImageButton id="imgClose" runat="server" style="border: 0" ImageUrl="./images/x11_close.gif" Height="23px" Width="26px" /></td>
            </tr>
            <tr>
                <td colspan='3' style="background-color: White; border: solid 1px blue; width: 473px; height: 114px;" valign="top">
                    <div style="text-align: left; padding: 10px 4px 10px 4px; width: 97%; height: 80%;">
                        <br />
                        &nbsp;<table>
                            <tr>
                                <td align="center" style="width: 451px; height: 20px">
                        <asp:Literal ID="litPopMessage" runat="server"></asp:Literal></td>
                                <td style="height: 20px">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="text-align: center; padding: 4px 4px 4px 4px; width: 97%";>
                       <asp:Button ID="cmdPopUpOK" runat="server" Text="OK" Width="62px" Height="24px" /> <asp:Button ID="cmdCancel" runat="server" Text="Cancel" Width="62px" Height="24px" /></div>
                </td>
            </tr>
        </table>
   </div>     
 </asp:Panel>        
  <table id="tblMainTable" runat="server" style="width: 694px">
            <tr>
                <td colspan="3">
                <table id="tblSearchCompBranch" runat="server">
                    <tr>
                        <td><asp:TextBox ID="txtSearchInput" runat="server" Width="300px" MaxLength="8000"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblValidateMessage" runat="server" BackColor="White" Font-Bold="True" ForeColor="Green" Font-Size="Larger"></asp:Label></td>
                    </tr>
                    <tr style="color: #222222">
                        <td>
                        </td>
                    </tr>
     </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top">
                    <table id="tblGridviewCompBranch" runat="server" style="width: 735px; height: 322px;">
                        <tr>
                            <td align="right" rowspan="1" style="width: 731px" valign="top">
                                <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green"></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                        </tr>
                <tr>
                    <td rowspan="3" valign="top" align="center">
                        <asp:GridView ID="gvBranches" runat="server" CellPadding="4" Width="96%" ForeColor="#333333" GridLines="None" Font-Size="8pt" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="CompCode">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CompanyName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCompname" CommandName = "select"  runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("CompCode") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BranchCode">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("CompShortName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCShortName" CommandName = "select"  runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("BranchCode") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("BranchName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBranchName" CommandName = "select"  runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("CompanyName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch Name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("BranchShortName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBShortName" CommandName = "select"  runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("BranchName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Active?">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("NoDiscFile") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsActive" runat="server" Checked='<%# eval("InActive") %>'
                                            Enabled="False" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                                            </td>
                </tr>
     </table>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                <table id="tblCompBranchInput"  runat="server" style="width: 495px; height: 48px;">
                    <tr>
                        <td align="center" colspan="3">
                                        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="330px">
                                        </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Store Code:</td>
                        <td style="height: 11px">
                        </td>
                        <td align="left" style="width: 359px">
                            <asp:Label ID="lblCompBranch" runat="server" Text="" Width="127px"></asp:Label></td>
                    </tr>
                 
                               <tr>
                                   <td align="right">
                                       Company Name:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left" style="width: 359px">
                                       <asp:DropDownList ID="ddlCompCode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCompCode_SelectedIndexChanged" DataSourceID="ObjectDataSource2"
                                           DataTextField="CompanyName" DataValueField="CompCode" Width="340px">
                                       </asp:DropDownList>
                                       <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}"
                                           SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectCompanyTableAdapter">
                                       </asp:ObjectDataSource>
                                   </td>
                               </tr>
                                <tr>
                                    <td align="right">
                                        Branch Name:</td>
                                    <td>
                                    </td>
                                    <td align="left" style="width: 359px" >
                                        <asp:DropDownList ID="ddlBrancode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBrancode_SelectedIndexChanged" DataSourceID="ObjectDataSource1"
                                            DataTextField="BranchName" DataValueField="BranchCode" Width="338px">
                                        </asp:DropDownList>
                                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectBranchTableAdapter">
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 20px">
                                        Other Information:</td>
                                    <td style="height: 20px">
                                    </td>
                                    <td align="left" style="width: 359px">
                                        <asp:TextBox ID="txtOtherInfo" runat="server" Width="329px" MaxLength="50"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Store Group:</td>
                                    <td>
                                    </td>
                                    <td align="left" style="width: 359px" >
                                        <asp:DropDownList ID="ddlStoreGroup" runat="server"
                                            DataTextField="ElementName" DataValueField="ElementValue" Width="338px">
                                            <asp:ListItem Text="-- Select Value --" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                   <td align="right">
                                       Store Company:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left" style="width: 359px">
                                       <asp:DropDownList ID="ddlVSCompCode" runat="server"
                                           DataTextField="StoreCompanyName" DataValueField="StoreCompCode" Width="340px">
                                       </asp:DropDownList>
                                   </td>
                               </tr>
                               <tr>
                                   <td align="right" >
                                       Disc File:</td>
                                   <td style="height: 20px">
                                   </td>
                                   <td align="left" style="width: 359px; height: 20px">
                                       <asp:CheckBox ID="chkDiscFile" runat="server" /></td>
                               </tr>
                                <tr>
                                    <td align="right">
                                        Visual Store:</td>
                                    <td style="height: 20px">
                                    </td>
                                    <td align="left" style="width: 359px; height: 20px">
                                        <asp:CheckBox ID="chkIsVisual" runat="server" /></td>
                                </tr>
                                                            <tr>
                                    <td align="right">
                                        Is Active:</td>
                                    <td style="height: 20px">
                                    </td>
                                    <td align="left" style="width: 359px; height: 20px">
                                        <asp:CheckBox ID="chkIsActive" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 22px" >
                                        <asp:HiddenField ID="hfRowID" runat="server" />
                                        </td>
                                    <td style="height: 22px" >
                                    </td>
                                    <td align="left" style="height: 22px; width: 359px;" >
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td  >
                                    </td>
                                    <td style="width: 359px"  >
                                    </td>
                                </tr>
                            </table>
                </td>
            </tr>
        </table>
</asp:Content>

