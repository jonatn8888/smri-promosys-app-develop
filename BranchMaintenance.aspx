<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="BranchMaintenance.aspx.vb" Inherits="BranchMaintenance" title="Branch Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    (<script type="text/javascript">
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
</script><br />
    <br />
    <div id="menu">
        <br />
        <br />
            <div style="display:none">
                <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" />
            </div>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="120px" /><br />
        <br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="120px" /><br />
        <br /><asp:Button ID="btnDelete" runat="server" Text="Delete" Width="121px" Visible="False" />
        <br />
        <br />
    </div>
    <div id="contents">
            <span style="font-size: 12pt">
            <strong>&nbsp;Branch Maintenance</strong></span><br /><hr style="width: 701px" />
        </div>
    


        <table id="tblMainTable" runat="server" style="width: 744px">
            <tr>
                <td colspan="3">
   <table id="tblSearchBranches" runat="server">
                    <tr>
                        <td><asp:TextBox ID="txtSearchInput" runat="server" Width="300px" MaxLength="8000"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="Search" Height="23px" /></td>
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
                        <asp:GridView ID="gvBranches" runat="server" CellPadding="4" Width="98%" ForeColor="#333333" GridLines="None" Font-Size="8pt" OnPageIndexChanging="gvBranches_PageIndexChanging" AutoGenerateColumns="False" AllowPaging="True" PageSize="10">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" /> 
                            <PagerSettings 
                                            Mode="Numeric"
                                            Position="Bottom"
                                            PageButtonCount="10" />
                            <Columns>
                                <asp:BoundField Visible="False" />
                                <asp:TemplateField HeaderText="Branch Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("BranchName") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Short Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("ShortName") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MMS Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# eval("RowID") %>'
                                            Text='<%# eval("MMSname") %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Dept. Store">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/check.gif" Visible='<%# eval("IsDeptStore") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Is Active?">
                                    <EditItemTemplate>
                                       <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Checkbox ID="chkIsActive" runat="server" Checked='<%# eval("IsHidden") %>'
                                            Enabled="False" />
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                                   <td align="right">
                                       Branch Code:       Branch Code:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left" style="width: 389px">
                                       <asp:TextBox ID="txtBranchCode" runat="server" MaxLength="3" Width="39px"></asp:TextBox></td>
                               </tr>
                                <tr>
                                    <td align="right">
                                        Branch Name:</td>
                                    <td style="height: 25px;">
                                    </td>
                                    <td align="left" style="width: 389px" >
                                        <asp:TextBox ID="txtLongName" runat="server" Width="378px" MaxLength="100"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 20px">
                                       MMS &nbsp;Name:</td>
                                    <td style="height: 20px">
                                    </td>
                                    <td align="left" style="width: 389px">
                                       <asp:TextBox ID="txtMMSName" runat="server" MaxLength="30" Width="380px"></asp:TextBox></td>
                                </tr>
                               <tr>
                                   <td align="right" >
                                        Short Name:</td>
                                   <td style="height: 20px">
                                   </td>
                                   <td align="left" style="width: 389px; height: 20px">
                                        <asp:TextBox ID="txtShortName" runat="server" Width="380px" MaxLength="15"></asp:TextBox></td>
                               </tr>
                                <tr style="display:none;">
                                    <td align="right" >
                                        Department Store:</td>
                                    <td>
                                    </td>
                                    <td  align="left" style="width: 389px" >
                                        <asp:CheckBox ID="chkDeptStore" runat="server" /></td>
                                </tr>
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

