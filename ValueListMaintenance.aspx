<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false"
    CodeFile="ValueListMaintenance.aspx.vb" Inherits="ValueListMaintenance" Title="List of Values Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
    MsgBoxwindow.onclose =  function()
                            { 
                            var theform = this.contentDoc.forms[0] 
                            document.getElementById("<%=cmdRedirect.ClientID%>").click(); 
	                        return true 
	                        }
        }

</script>



    <br /><br />

    <div id="menu">
        <br /><br />
        <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" /><br /><br />
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="119px" /><br /><br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="119px" /><br /><br />
         <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="119px" /><br /><br />
    </div>

    <div id="contents">
        <span style="font-size: 12pt"><strong>&nbsp;Shouldering Entity Maintenance</strong></span><br />
        <hr />
    </div>

    <table id="tblMainTable" runat="server" style="width: 744px">
        <tr>
            <td colspan="3">
                <table id="tblSearchCompanies" runat="server">
                    <tr>
                        <td>
                            Group Type:
                            <asp:DropDownList ID="ddlSearchGroupType" runat="server" Width="150px">                              
                                <asp:ListItem Value="MBU">MBU</asp:ListItem>
                                <asp:ListItem Value="SBU">SBU</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;

                            Element Name:
                            <asp:TextBox ID="txtSearchInput" runat="server" Width="200px"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblValidateMessage" runat="server" BackColor="White" Font-Bold="True"
                                ForeColor="Green" Font-Size="Larger"></asp:Label>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>

        <tr>
            <td valign="top">
                <table id="tblGridview" runat="server" style="width: 700px;">
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green"></asp:Label>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:GridView ID="gvBranches" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" Width="98%" ForeColor="#333333" GridLines="None"
                                Font-Size="8pt" AllowPaging="True">

                                <Columns>

                                    <asp:TemplateField HeaderText="Group Type">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server"
                                                CommandArgument='<%# Eval("ResourceID") %>'
                                                CommandName="select"
                                                Text='<%# Eval("SubGroupName") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButtonName" runat="server"
                                                CommandArgument='<%# Eval("ResourceID") %>'
                                                CommandName="select"
                                                Text='<%# Eval("ElementName") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Nick">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButtonNick" runat="server"
                                                CommandArgument='<%# Eval("ResourceID") %>'
                                                CommandName="select"
                                                Text='<%# Eval("ElementNick") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

<%--                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButtonValue" runat="server"
                                                CommandArgument='<%# Eval("ResourceID") %>'
                                                CommandName="select"
                                                Text='<%# Eval("ElementValue") %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                </Columns>

                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />

                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td colspan="3" align="left">
                <table id="tblCompanyInput" runat="server" style="width: 453px;">
                    <tr><td colspan="3"><asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="330px" /></td></tr>

                    <tr><td align="right">Group:</td><td></td><td><asp:TextBox ID="txtGroup" Enabled="False" runat="server" Width="288px"></asp:TextBox></td></tr>
                    <tr><td align="right">Name:</td><td></td><td><asp:TextBox ID="txtName" runat="server" Width="288px"></asp:TextBox></td></tr>
                    <tr><td align="right">Nickname:</td><td></td><td><asp:TextBox ID="txtNick" runat="server" Width="288px"></asp:TextBox></td></tr>
                    <%--<tr><td align="right">Value:</td><td></td><td><asp:TextBox ID="txtValue" runat="server" Width="288px" MaxLength="20" ></asp:TextBox></td></tr>--%>

                    <tr><td align="right">Group Type:</td><td></td><td>
                    <asp:DropDownList ID="cmbGroupType" runat="server" Width="231px" AutoPostBack="True">
                        <asp:ListItem>MBU</asp:ListItem>
                        <asp:ListItem>SBU</asp:ListItem>
                    </asp:DropDownList>
                    </td></tr>

                    <tr><td align="right">Active:</td><td></td><td><asp:CheckBox ID="chkActive" runat="server" /></td></tr>

                    <tr><td><asp:HiddenField ID="hfRowID" runat="server" /></td></tr>
                </table>

                <asp:HiddenField ID="lblPopTitle" runat="server" />
            </td>
        </tr>
    </table>
    


    <div style="display:none"><asp:Button ID="cmdPopUpOK" runat="server" Text="" />
        <asp:Button ID="cmdRedirect" runat="server" /></div>

</asp:Content>
