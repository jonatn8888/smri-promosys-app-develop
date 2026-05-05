<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="CompanyMaintenance.aspx.vb" Inherits="CompanyMaintenance" title="Company Maintenance" %>

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
        <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" /><br />
        <br /><asp:Button ID="btnSave" runat="server" Text="Save" Width="119px" />
        <br />
        <br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="119px" /><br />
        <br /><asp:Button ID="btnDelete" runat="server" Text="Delete" Width="119px" Visible="False" />
    </div>
        <div id="contents">
            <span style="font-size: 12pt">
            <strong>&nbsp;Company Maintenance</strong></span><br /><hr />
        </div>
        
   
 
    <table id="tblMainTable" runat="server" style="width: 744px">
            <tr>
                <td colspan="3">
                <table id="tblSearchCompanies" runat="server">
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
                    <table id="tblGridviewCompanies" runat="server" style="width: 733px; height: 322px;">
                        <tr>
                            <td align="right" rowspan="1" valign="top">
                                <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green"></asp:Label>
                                &nbsp; &nbsp;
                            </td>
                        </tr>
                <tr>
                    <td rowspan="3" valign="top">
                        <asp:GridView ID="gvBranches" runat="server" CellPadding="4" Width="98%" ForeColor="#333333" GridLines="None" Font-Size="8pt" AutoGenerateColumns="False" AllowPaging="True">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Company Code">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("CompCode") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# eval("RowID") %>'
                                          CommandName="select"  Text='<%# eval("CompCode") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("CompanyName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# eval("RowID") %>'
                                          CommandName="select"  Text='<%# eval("CompanyName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Short Name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ShortName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# eval("RowID") %>'
                                           CommandName="select" Text='<%# eval("ShortName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Environment Name">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument='<%# eval("RowID") %>'
                                         CommandName="select"   Text='<%# eval("EnvName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Wrap="False" />
                                </asp:TemplateField>
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
                        </asp:GridView>
                      </td>
                </tr>
     </table>
                </td>
            </tr>
            <tr>
            
                <td align="left" colspan="3">
                    &nbsp;<table id="tblCompanyInput"  runat="server" style="width: 453px; height: 174px;">
                                            <tr style="color: #222222">
                                                <td colspan="3">
                                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="330px">
                </asp:BulletedList>
                                    </td>
                                </tr>
                               <tr>
                                   <td align="right">
                                       Company Code:</td>
                                   <td style="height: 11px">
                                   </td>
                                   <td align="left">
                                       <asp:TextBox ID="txtCompCode" runat="server" MaxLength="3" Width="39px"></asp:TextBox></td>
                               </tr>
                                <tr>
                                    <td align="right">
                                        Company Name:</td>
                                    <td>
                                    </td>
                                    <td align="left" >
                                        <asp:TextBox ID="txtCompanyName" runat="server" Width="288px" MaxLength="100"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Short &nbsp;Name:</td>
                                    <td style="height: 20px">
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtShortName" runat="server" Width="287px" MaxLength="20"></asp:TextBox></td>
                                </tr>
                               <tr>
                                   <td align="right" >
                                       Environment Name:</td>
                                   <td style="height: 20px">
                                   </td>
                                   <td align="left">
                                       <asp:DropDownList ID="ddlEnvCode" runat="server" 
                                           DataTextField="EnvName" DataValueField="EnvCode" Width="291px">
                                       </asp:DropDownList></td>
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
                                    <td align="right" >
                                        <asp:HiddenField ID="hfRowID" runat="server" />
                                        </td>
                                    <td style="height: 22px" >
                                    </td>
                                    <td align="left" >
                                        <asp:ObjectDataSource ID="objDSEnvironment" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectEnvironmentTableAdapter">
                                        </asp:ObjectDataSource>
                                        </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td  >
                                    </td>
                                    <td  >
                                    </td>
                                </tr>
                            </table>
                    <asp:HiddenField ID="lblPopTitle" runat="server" />

                </td>
            </tr>
        </table>  
     <div style="display:none">
    <asp:Button ID="cmdPopUpOK" runat="server" Text="Button" /></div>

</asp:Content>


