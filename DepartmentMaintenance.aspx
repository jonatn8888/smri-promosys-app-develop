<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="DepartmentMaintenance.aspx.vb" Inherits="DepartmentMaintenance" title="Department Maintenance" %>

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

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
           <%--<div style="display:none">--%>
                <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" />
            <%--</div>--%>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="120px" /><br />
        <br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="120px" /><br />
        <br /><asp:Button ID="btnDelete" runat="server" Text="Delete" Width="121px" Visible="True" />
        <br />
        <br />
    </div>
    <div id="contents">
            <span style="font-size: 12pt">
            <strong>&nbsp;Department Maintenance</strong></span><br />
        </div>
    


        <table id="tblMainTable" runat="server" style="width: 744px">
            <tr>
                <td colspan="3">
   <table id="tblSearchDepartment" runat="server">
                    <tr>
                        <td><asp:TextBox ID="txtSearchInput" runat="server" Width="300px" MaxLength="8000"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="Search" Height="23px" /><hr style="width: 701px" />
                        </td>
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
    <table id="tblGridviewBranches" runat="server" style="width: 731px">
        <tr>
            <td align="right" rowspan="1" valign="top">
                <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green"></asp:Label>
                &nbsp; &nbsp;&nbsp;
            </td>
        </tr>
                <tr>
                    <td rowspan="3" valign="top">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="122px" PageSize="5"
                            Width="98%" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True">
                            <Columns>
                                <asp:BoundField Visible="False" />
                                <asp:TemplateField HeaderText="Dept Code">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# eval("GroupId") %>'
                                            Text='<%# eval("DeptCode") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Short Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# eval("GroupId") %>'
                                            Text='<%# eval("Department") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# eval("GroupId") %>'
                                            Text='<%# eval("Description") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Business Unit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument='<%# eval("GroupId") %>'
                                            Text='<%# eval("BizUnit") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Environment">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument='<%# eval("GroupId") %>'
                                            Text='<%# eval("EnvShortName") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Dept. Store">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/check.gif" Visible='<%# eval("IsDeptStore") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
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
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
    
                           <table id="tblBranchInput"  runat="server" style="width: 574px">
                                <tr style="color: #222222">
                                    <td colspan="3">
                                        <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="468px">
                                        </asp:BulletedList>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 146px" >
                                        Category:</td>
                                    <td >
                                    </td>
                                    <td align="left" style="height: 20px; width: 386px;" class="field-cell" >
                                        <asp:DropDownList ID="ddCategory" runat="server" Width="310px">
                                            <asp:ListItem Value="0">-- Select Division --</asp:ListItem>
                                            <asp:ListItem>FASHION</asp:ListItem>
                                            <asp:ListItem>NON-FASHION</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                
                                <tr>
                                    <td align="right" style="width: 146px">
                                        <asp:Label ID="lblBizUnit" runat="server" Text="Business Unit:"></asp:Label></td>
                                    <td>
                                    </td>
                                    <td align="left" style="width: 386px" class="field-cell">
                                        <asp:Panel ID="panBizUnit" runat="server">
                                        <asp:DropDownList ID="ddlBizUnit" runat="server" Width="310px" >
                                        </asp:DropDownList>&nbsp;
                                      
                                        </asp:Panel>
                                    </td>
                                </tr>
                                

                               <tr>
                                   <td align="right">
                                       Department Code:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left" style="width: 389px">
                                       <asp:TextBox ID="txtDeptCode" runat="server" MaxLength="3" Width="39px"></asp:TextBox></td>
                               </tr>
                                <tr>
                                    <td align="right">
                                        Department:</td>
                                    <td style="height: 25px;">
                                    </td>
                                    <td align="left" style="width: 389px" >
                                      <asp:TextBox ID="txtDeptName" runat="server" MaxLength="5" Width="380px"></asp:TextBox></td>  
                                </tr>
                                <tr>
                                    <td align="right" style="height: 20px">Description:
                                       </td>
                                     <td style="height: 20px"> </td>
                                    <td align="left" style="width: 389px">
                                     
                                       <asp:TextBox ID="txtDeptShortName" runat="server" Width="378px" MaxLength="100" ></asp:TextBox></td>
                                       
                                </tr>
                                <tr>
                                    <td align="right" style="height: 22px" >
                                        Environments :
                                        </td>
                                    <td style="height: 22px" >
                                    </td>
                                    <td align="left" style="height: 22px; width: 389px;" >
                                    <asp:DropDownList ID="ddEnvironments" runat="server" Width="310px" />
                                        </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 22px" >
                                        <asp:HiddenField ID="hfRowID" runat="server" />
                                        </td>
                                    <td style="height: 22px" >
                                    </td>
                                    <td align="left" style="height: 22px; width: 389px;" >
                                    
                                        </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td  >
                                    </td>
                                    <td style="width: 389px"  >
                                    </td>
                                </tr>
                            </table>
                    <div style="display: none">
                        <asp:Button ID="cmdPopUpOK" runat="server" Height="24px" Text="OK" Width="62px" />&nbsp;
                        <asp:HiddenField ID="lblPopTitle" runat="server" />
                    </div>
                </td>
            </tr>
        </table>

</asp:Content>
