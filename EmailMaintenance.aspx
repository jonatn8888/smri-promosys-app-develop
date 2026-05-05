<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="EmailMaintenance.aspx.vb" Inherits="EmailMaintenance" title="Email" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function opentexteditor()
{
    var url
    var title
    url = "MsgBox.aspx";
    title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
    MsgBoxwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width=510px,height=163px,center=1,resize=0,scrolling=0',"recall")
    MsgBoxwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    document.getElementById("<%=cmdPopUpOK.ClientID%>").click(); 
	return true 
	}
}
</script>
<div style="display:none"><asp:Button runat="server" id="cmdPopUpOK" Text="Button"></asp:Button></div>
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
    </div>
    <div id="contents">
            <span style="font-size: 12pt">
            <strong>&nbsp;Email Address</strong></span><br /><hr style="width: 701px" />
    </div>
        
    <table id="tblMainTable" runat="server">
            <tr>
                <td colspan="3">
   <table id="tblSearchEmail" runat="server">
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
    <table id="tblGridviewEmail" runat="server" style="width: 731px">
        <tr>
            <td align="right" rowspan="1" valign="top">
                <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green"></asp:Label>
                &nbsp; &nbsp;&nbsp;
            </td>
        </tr>
                <tr>
                    <td rowspan="3" valign="top">
                        <asp:GridView ID="gvBranches" runat="server" CellPadding="4" Width="97%" ForeColor="#333333" GridLines="None" Font-Size="8pt" AutoGenerateColumns="False" AllowPaging="True" PageSize="5">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Email Address">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("EmailAddress") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("EmailAddress") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("Description") %>'  CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CompanyName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("CompanyName") %>'  CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("BranchName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("BranchName") %>'  CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("BizUnit") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("BizUnit") %>'  CommandName="select"></asp:LinkButton>
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
    
                           <table id="tblEmailInput"  runat="server" style="width: 574px">
                                <tr style="color: #222222">
                                    <td colspan="3">
                                        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="468px">
                                        </asp:BulletedList>
                                        &nbsp;
                                    </td>
                                </tr>
                               <tr>
                                   <td align="right">
                                       Email Address:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left" style="width: 389px">
                                       <asp:TextBox ID="txtEmail" runat="server" Width="388px" MaxLength="100"></asp:TextBox></td>
                               </tr>
                               <tr>
                                   <td align="right">
                                       Description:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left" style="width: 389px">
                                       <asp:TextBox ID="txtDescription" runat="server" Width="386px" MaxLength="50"></asp:TextBox></td>
                               </tr>
                               <tr>
                                   <td align="right">
                                       Company:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left" style="width: 389px">
                                       <asp:DropDownList ID="ddlCompany" runat="server" DataSourceID="ObjectDataSource1"
                                           DataTextField="CompanyName" DataValueField="CompCode" Width="398px">
                                       </asp:DropDownList></td>
                               </tr>
                                <tr>
                                    <td align="right">
                                        Branch:</td>
                                    <td style="height: 25px;">
                                    </td>
                                    <td align="left" style="width: 389px" >
                                        <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="ObjectDataSource2"
                                            DataTextField="BranchName" DataValueField="BranchCode" Width="397px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 20px">
                                        Group:</td>
                                    <td style="height: 20px">
                                    </td>
                                    <td align="left" style="width: 389px">
                                        <asp:DropDownList ID="ddlGroup" runat="server" Width="397px" DataSourceID="ObjectDataSource3" DataTextField="BizUnit" DataValueField="GroupID">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td  >
                                    </td>
                                    <td style="width: 389px"  >
                                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectCompanyTableAdapter">
                                        </asp:ObjectDataSource>
                                        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectBranchTableAdapter">
                                        </asp:ObjectDataSource>
                                        <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectUserGroupsBizUnitxTableAdapter">
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                </td>
            </tr>
        </table>
    <asp:HiddenField ID="lblPopTitle" runat="server" />
        
</asp:Content>

