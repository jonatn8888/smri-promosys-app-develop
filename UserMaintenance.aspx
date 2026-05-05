<%@ Page Language="VB" Debug="true" MasterPageFile="~/PromoPage.master" MaintainScrollPositionOnPostback="true"
    AutoEventWireup="false" CodeFile="UserMaintenance.aspx.vb" Inherits="UserMaintenance"
    Title="User Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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

function toggleDropdown(el) {
        if (hasClass(el, "expanded")) {
            el.className = el.className.replace(" expanded", "");
        } else {
            el.className += " expanded";
        }
 }

function collapseDropdown(el) {
        el.className = el.className.replace(" expanded", "");
}

function hasClass(el, className) {
        return (" " + el.className + " ").indexOf(" " + className + " ") > -1;
}

function openBranches(Environment,height,width)
{
    if (height == "") height= "400";
    
    if (width == "") width= "680";
    
    var url
    var title
    url = "SelectUserBranches.aspx?Params=" + Environment;
    title = "Select Branches"; 
    Branchwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    Branchwindow.onclose=function()
    { 

    document.getElementById("<%=cmdLoadForm.ClientID%>").click(); 
    var theform = this.contentDoc.forms[0] 
	return true 
	}
}

function openModalDept(myParams,height,width)
{
    if (height == "") height= "400";
    
    if (width == "") width= "680";
    
    var url
    var title
    url = "SelectUserDept.aspx?myParams=" + myParams;
    title = "Select Department(s)"; 
    modalWindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    modalWindow.onclose=function()
    { 

    document.getElementById("<%=cmdLoadForm.ClientID%>").click(); 
    var theform = this.contentDoc.forms[0] 
	return true 
	}
}
   
</script>

<script type="text/javascript">
    function OnTreeClick(evt) {
        var src = window.event ? event.srcElement : evt.target;

        if (src.tagName == "INPUT" && src.type == "checkbox") {
            var treeNode = src;
            var parentTable = GetParentByTagName("table", treeNode);
            var nextSibling = parentTable.nextSibling;

            if (nextSibling && nextSibling.nodeName == "DIV") {
                var childDiv = nextSibling;
                var checkBoxes = childDiv.getElementsByTagName("input");

                for (var i = 0; i < checkBoxes.length; i++) {
                    if (checkBoxes[i].type == "checkbox") {
                        checkBoxes[i].checked = treeNode.checked;
                    }
                }
            }

            UpdateParentCheckStatus(treeNode);
        }
    }

    function GetParentByTagName(tagName, element) {
        var parent = element.parentNode;
        while (parent) {
            if (parent.tagName && parent.tagName.toLowerCase() == tagName.toLowerCase()) {
                return parent;
            }
            parent = parent.parentNode;
        }
        return null;
    }

    function UpdateParentCheckStatus(childCheckbox) {
        var parent = GetParentByTagName("table", childCheckbox);
        if (parent) {
            var parentDiv = parent.parentNode;
            var siblingInputs = parentDiv.getElementsByTagName("input");

            var allChecked = true;
            for (var i = 0; i < siblingInputs.length; i++) {
                if (siblingInputs[i].type == "checkbox" && !siblingInputs[i].checked) {
                    allChecked = false;
                    break;
                }
            }

            var grandParentTable = GetParentByTagName("table", parentDiv);
            if (grandParentTable) {
                var parentCheckbox = grandParentTable.getElementsByTagName("input")[0];
                if (parentCheckbox) {
                    parentCheckbox.checked = allChecked;
                }
            }
        }
    }

    window.onload = function () {
        var TvBUDept = document.getElementById("<%= TvBUDept.ClientID %>");
        if (TvBUDept) {
            TvBUDept.onclick = OnTreeClick;
        }
        
        
        var TvCompanies = document.getElementById("<%= TvCompanies.ClientID %>");
        if (TvCompanies) {
            TvCompanies.onclick = OnTreeClick;
        }
    };
</script>



   


    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:Button ID="cmdNew" runat="server" Text="Create New" Width="119px" TabIndex="10" /><br />
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="119px" TabIndex="20" UseSubmitBehavior="False" />
        <br />
        <br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="119px" TabIndex="30" /><br />
        <br />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="119px" Visible="False"
            TabIndex="40" />
        <br />
        <br />
    </div>
    <div id="contents">
        <span style="font-size: 12pt"><strong>&nbsp;User Maintenance</strong></span><br />
        <hr />
    </div>
    <table>
        <tr>
            <td>
                <table id="tblSearchUsers" runat="server">
                    <tr>
                        <td style="margin-left: 80px">
                            <asp:TextBox ID="txtSearch" runat="server" Width="300px" MaxLength="150"></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblValidateMessage" runat="server" BackColor="White" Font-Bold="True"
                                ForeColor="Green" Font-Size="Larger"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #222222">
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="color: #222222">
            </td>
            <td style="color: #222222">
            </td>
        </tr>
        <tr style="color: #222222">
            <td>
                <table id="tblGridviewUsers" runat="server">
                    <tr>
                        <td style="width: 5px">
                        </td>
                        <td align="right" rowspan="1" style="width: 693px">
                            <asp:Label ID="lblRecordCount" runat="server" Font-Size="Larger" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 5px">
                        </td>
                        <td rowspan="3" style="width: 693px">
                            <asp:GridView ID="gvUsers" runat="server" Width="699px" AutoGenerateColumns="False"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Font-Size="8pt" 
                                AllowPaging="True">
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <Columns>
                                    <asp:TemplateField HeaderText="User Name">
                                        <ItemTemplate>
                                            &nbsp;<asp:LinkButton ID="lnkUserName" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("UserName") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Department">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UserDept") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("UserDept") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sign Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SignName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("SignName") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sign Position">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("SignPosition") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("SignPosition") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Category") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton5" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("Category") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Business Unit">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("BizUnit") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton6" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("BizUnit") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department" Visible="False">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Department") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton7" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("Department") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="lblCompCode" runat="server" Text='<%# Bind("CompCode") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkCompCode" runat="server" CommandArgument='<%# eval("UserID") %>'
                                                Text='<%# eval("Department") %>' CommandName="view"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 5px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 5px">
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;<table id="tblUserInput" runat="server" class="doc-table">
                    <tr>
                        <td colspan="3">
                            <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" 
                                Width="591px" Height="16px">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px; height: 11px">
                        </td>
                        <td style="width: 6px; height: 11px">
                        </td>
                        <td align="left" class="field-cell" style="width: 386px; height: 20px">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px; height: 11px;">
                            User Name:
                        </td>
                        <td style="width: 6px; height: 11px;">
                        </td>
                        <td align="left" style="height: 20px; width: 386px;" class="field-cell">
                            <asp:TextBox ID="txtUserName" runat="server" Width="305px" MaxLength="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px; width: 146px;">
                        </td>
                        <td style="height: 21px">
                        </td>
                        <td style="height: 20px; width: 386px;" class="field-cell">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 20px; width: 146px;">
                            Sign Name:
                        </td>
                        <td style="height: 20px">
                        </td>
                        <td align="left" style="height: 20px; width: 386px;" class="field-cell">
                            <asp:TextBox ID="txtSignName" runat="server" Width="305px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="height: 20px; width: 146px;">
                            Position:
                        </td>
                        <td style="height: 20px">
                        </td>
                        <td style="height: 20px; width: 386px;" align="left" class="field-cell">
                            <asp:TextBox ID="txtPosition" runat="server" Width="305px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 146px">
                        </td>
                        <td>
                        </td>
                        <td style="height: 20px; width: 386px;" class="field-cell">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px">
                            Division:
                        </td>
                        <td>
                        </td>
                        <td align="left" style="height: 20px; width: 386px;" class="field-cell">
                            <asp:DropDownList ID="ddDepartment" runat="server" Width="310px">
                                <asp:ListItem Value="0">-- Select Division --</asp:ListItem>
                                <asp:ListItem>ITS</asp:ListItem>
                                <asp:ListItem>MPD</asp:ListItem>
                                <asp:ListItem>MERCH</asp:ListItem>
                                <asp:ListItem>POS</asp:ListItem>
                                <asp:ListItem>BRANCH</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 146px">
                        </td>
                        <td>
                        </td>
                        <td style="height: 20px; width: 386px;" class="field-cell">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px">
                            Email Address:
                        </td>
                        <td>
                        </td>
                        <td style="width: 386px; height: 20px" class="field-cell">
                            <asp:TextBox ID="txtLN" runat="server" MaxLength="50" Width="305px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 146px">
                        </td>
                        <td>
                        </td>
                        <td style="width: 386px; height: 20px" class="field-cell">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px">
                            User Level:
                        </td>
                        <td>
                        </td>
                        <td align="left" style="height: 20px; width: 386px;" class="field-cell">
                            <asp:DropDownList ID="ddUserLevel" runat="server" Width="310px" AutoPostBack="True">
                                <asp:ListItem Value="0">-- Select User Level --</asp:ListItem>
                                <asp:ListItem Value="80">Promo Requester</asp:ListItem>
                                <asp:ListItem Value="75">Request Reviewer</asp:ListItem>
                                <asp:ListItem Value="70">Request Approver</asp:ListItem>
                                <asp:ListItem Value="60">Executive Approver</asp:ListItem>
                                <asp:ListItem Value="50">MCI Approver</asp:ListItem>
                                <asp:ListItem Value="40">MPD Analyst</asp:ListItem>
                                <asp:ListItem Value="30">Memo Reviewer</asp:ListItem>
                                <asp:ListItem Value="20">Memo Approver</asp:ListItem>
                                <asp:ListItem Value="100">Announcement Viewer</asp:ListItem>
                                <asp:ListItem Value="90">POS Personnel</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px">
                            <asp:Label ID="lblCategory" runat="server" Text="Category:"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td align="left" style="width: 386px" class="field-cell">
                            <asp:Panel ID="panCategory" runat="server">
                                <asp:DropDownList ID="ddlCategory" runat="server" Width="310px" AutoPostBack="True">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px; height: 38px;" valign="top">
                            <asp:Label ID="lblBizUnit" runat="server" Text="Business Unit:"></asp:Label>
                        </td>
                        <td style="height: 38px">
                        </td>
                        <td align="left" style="width: 386px; height: 38px;" class="field-cell">
                            <asp:Panel ID="panBizUnit" runat="server" Width="477px" Height="35px">
                                <asp:DropDownList ID="ddlBizUnit" runat="server" Width="310px" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:Button ID="cmdSelectDept" runat="server" Height="21px" Text="Department(s)" 
                                    Width="152px" />&nbsp;&nbsp;
                                <asp:ObjectDataSource ID="objDSBizUnit" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectUserGroupsBizUnitTableAdapter">
                                </asp:ObjectDataSource>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                                        
                    <tr>
                        <td align="right" class="style1">
                                        &nbsp;</td>
                                    <td class="style2">
                                    </td>
                                    <td align="left" class="style3" style="width: 386px">
                                    <asp:Panel ID="panTreeBUDept" runat="server" Height="240px">
                                       <div style="height: 230px; overflow-y: auto; border: 1px solid #ccc; padding: 5px; width: 462px;">
                                             <asp:TreeView ID="tvBUDept" runat="server" 
                                                             CssClass="tv-BUDept"                                                        
                                                            DataTextField="Department"
                                                            DataValueField="GroupID"
                                                            PopulateNodesFromClient="true" ExpandDepth="1"
                                                            SelectAction="SelectExpand"
                                                 Height="228px" NodeIndent="25" Width="465px" ShowCheckBoxes="All" />
                                        <br />
                                             <br />
                                        </div>
                                    </asp:Panel>
                                    </td>
                                </tr>
                        
                    </tr>
                                        
                    <tr>
                        <td align="right">
                                        </td>
                                    <td>
                                    </td>
                                    <td align="left" class="style3" style="width: 386px">
                            <asp:Panel ID="panDelBUDept" runat="server" Height="18px" Width="472px">  
                                <asp:LinkButton ID="lnkdelTvBUDept" runat="server" 
                                    CssClass="action-link"> Delete Selected</asp:LinkButton>
                                <asp:LinkButton ID="lnkselAllTvBUDept" runat="server" 
                                    CssClass="action-link">Select All /</asp:LinkButton>  
                            </asp:Panel>
                                    </td>
                                </tr>
                        
                    <tr>
                        <td align="right" style="width: 146px">
                            <asp:Label ID="lblDept" runat="server" Text="Department:"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td align="left" style="width: 386px" class="field-cell">
                            <asp:Panel ID="panDept" runat="server">
                                <asp:DropDownList ID="ddlDepartment" runat="server" Width="310px" AutoPostBack="True"
                                    ForeColor="Transparent">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:ObjectDataSource ID="objDSDept" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectUserGroupsDptCMMMTableAdapter">
                                </asp:ObjectDataSource>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px" valign="top">
                            <asp:Label ID="lblCompany" runat="server" Text="Company:"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td align="left" class="field-cell" style="width: 386px">
                            <asp:Panel ID="panCompany" runat="server" Width="477px">
                                <asp:DropDownList ID="ddlCompany" runat="server" Width="310px" AutoPostBack="True"
                                    ForeColor="Transparent" AppendDataBoundItems="True" DataTextField="CompName"
                                    DataValueField="CompCode">
                                    <asp:ListItem Value="0">-- Select Company --</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="cmdSelectBranch" runat="server" Height="21px" Text="Branch(es)" Width="152px" />
                                &nbsp;&nbsp;<asp:ObjectDataSource ID="objCompany" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectCompanyTableAdapter">
                                </asp:ObjectDataSource>
                                <br />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                    <td align="right" class="style1">
                                        &nbsp;</td>
                                    <td class="style2">
                                    </td>
                                    <td align="left" class="style3" style="width: 386px">
                                    <asp:Panel ID="panTreeCompanies" runat="server">
                                       <div style="height: 197px; overflow-y: auto; border: 1px solid #ccc; padding: 5px; width: 462px;">
                                             <asp:TreeView ID="tvCompanies" runat="server" 
                                                             CssClass="tv-companies"                                                        
                                                            DataTextField="BranchName"
                                                            DataValueField="rowID"
                                                            SelectAction="Select"
                                                 Height="198px" NodeIndent="25" Width="249px" ShowCheckBoxes="All" />
                                        <br />
                                        </div>
                                    </asp:Panel>
                                    </td>
                                </tr>
                    <tr>
                    <td align="right" class="style1">
                                        &nbsp;</td>
                                    <td class="style2">
                                        &nbsp;</td>
                                    <td align="left" class="style3" style="width: 386px">
                            <asp:Panel ID="panDelCompanies" runat="server" Height="18px" Width="472px">
                                <asp:LinkButton ID="lnkdelTvCompanies" runat="server" 
                                    CssClass="action-link">Delete Selected</asp:LinkButton>
                                <asp:LinkButton ID="lnkselAllTvCompanies_Click" runat="server" 
                                    CssClass="action-link">Select All /</asp:LinkButton>
                            </asp:Panel>
                                    </td>
                                </tr>
                    <tr>
                        <td align="right" style="width: 146px">
                            <asp:Label ID="lblBizUnitViewer" runat="server" Text="Business Unit:"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td align="left" class="field-cell" style="width: 386px">
                            <asp:Panel ID="panBizUnitViewer" runat="server">
                                <asp:DropDownList ID="ddlBizUnitViewer" runat="server" Width="310px" AutoPostBack="True">
                                    <asp:ListItem Value="-1">-- Select Business Unit --</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;
                                <asp:ObjectDataSource ID="objDSBizUnitViewer" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectUserGroupsBizUnitTableAdapter">
                                </asp:ObjectDataSource>
                            </asp:Panel>
                        </td>
                    </tr>
                    <!-- cjg2243 SR#4406088: start -->
                    <tr style="display: none;">
                        <td align="right" style="width: 146px">
                            <asp:Label ID="lblDepartmentViewer" runat="server" Text="Department:"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td align="left" class="field-cell" style="width: 386px">
                            <asp:Panel ID="panDepartmentViewer" runat="server">
                                <asp:DropDownList ID="ddlDepartmentViewer" runat="server" Width="310px" AutoPostBack="True"
                                    ForeColor="Transparent">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="right" style="width: 146px">
                            <asp:Label ID="lblGA" runat="server" Text="Business Unit:"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td align="left" valign="top" style="width: 386px" class="field-cell">
                            <asp:Panel ID="panGroupAssignment" runat="server" Width="340px">
                                <asp:DropDownList ID="ddGroupAssignment" runat="server" Width="310px">
                                </asp:DropDownList>
                                <asp:Button ID="btnAddGroupAssignment" runat="server" Text="+" Width="23px" TabIndex="2" />
                                <br />
                                <asp:GridView ID="gvGroupAssign" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" Font-Size="8pt" Width="310px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="•" ShowHeader="False">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkBizUnitAll" runat="server" AutoPostBack="True" EnableTheming="True"
                                                Font-Bold="True" ForeColor="White" OnCheckedChanged="chkBizUnitAll_CheckedChanged" Text="All" />
                                        </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkBizUnitSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBizUnits" runat="server" Text='' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField  DataField="Description" HeaderText="Business Unit">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <EmptyDataTemplate>
                                        &nbsp;
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <asp:LinkButton ID="lnkRemoveBizUnit" runat="server" CssClass="action-link">Delete 
                                Selected</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:ObjectDataSource ID="objDSGroupAssignment1" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectUserGroupsBizUnitTableAdapter">
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="objDSGroupAssignment2" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetData" TypeName="dsFillDropDownTableAdapters.USP_SelectGroupAssignments1TableAdapter">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="UserID" SessionField="GID" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </asp:Panel>
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px" valign="top">
                        <asp:Label ID="lblAccessSetting" runat="server" Text="Access Setting:"></asp:Label>
                            &nbsp;</td>
                        <td>
                        </td>
                        <td align="left" class="field-cell" style="width: 386px" valign="top">
                            <asp:Panel ID="panAccessSetting" runat="server">
                                       <asp:CheckBox ID="chkAccessSetting" runat="server" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 146px" valign="top">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td align="left" class="field-cell" style="width: 386px" valign="top">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
            <td style="height: 22px">
            </td>
            <td style="height: 22px">
            </td>
        </tr>
    </table>
        <div style="display: none">
    <asp:HiddenField ID="lblPopTitle" runat="server" />
    
    <asp:HiddenField ID="RowContainer_" runat="server" />
    <asp:HiddenField ID="GroupContainer_" runat="server" />
    <asp:HiddenField ID="DelRowContainer_" runat="server" />
    <asp:HiddenField ID="HiddenUserIDField" runat="server" />
    <asp:HiddenField ID="InitialUserLevel" runat="server" />
    <asp:HiddenField ID="InitialBizUnit" runat="server" />
    <asp:HiddenField ID="SelectedCompany" runat="server" />
    <asp:HiddenField ID="BUDeptLastAction" runat="server" />
    <asp:HiddenField ID="CompaniesLastAction" runat="server" />

        <asp:Button ID="cmdPopUpOK" runat="server" Text="Button" />
        <asp:Button ID="cmdLoadForm" runat="server" Text="Button" />
        <asp:Button ID="cmdCancel" runat="server" Text="Button" />
    </div>
    
    <asp:SqlDataSource ID="sqldsBranches" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"                      
            SelectCommand="USP_GetSelectedBranch @userID, @CompCode, @Mode, @ROWID" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
       <SelectParameters>
            <%--<asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="CurrUserID" />--%>
            <asp:Parameter Name="userID" />
            <asp:Parameter Name="CompCode" />
            <asp:Parameter Name="Mode" />
            <asp:Parameter Name="ROWID" />
        </SelectParameters>
    </asp:SqlDataSource>
    
        <asp:SqlDataSource ID="sqldsBUDept" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"                      
            SelectCommand="USP_GetSelectedBUDept @userID, @BizUnit, @Mode, @GROUPID" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
       <SelectParameters>
            <%--<asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="CurrUserID" />--%>
            <asp:Parameter Name="userID" />
            <asp:Parameter Name="BizUnit" />
            <asp:Parameter Name="Mode" />
            <asp:Parameter Name="GROUPID" />
        </SelectParameters>
    </asp:SqlDataSource>
        
    
    <asp:SqlDataSource ID="sqldsUserBranches" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="USP_GetBranchByUserID @UserID, @SelectedBranches, @AfterModal" InsertCommand="USP_SaveMultipleBranches @UserID @CompCode @BranchCode">
        <SelectParameters>
            <%--<asp:SessionParameter DefaultValue="0" Name="UserID" SessionField="CurrUserID" />--%>
            <asp:Parameter Name="UserID" />
            <asp:Parameter Name="SelectedBranches" />
            <asp:Parameter Name="AfterModal" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="UserID" />
            <asp:Parameter Name="CompCode" />
            <asp:Parameter Name="BranchCode" />
        </InsertParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sqldsDept" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
         SelectCommand="USP_SelectUserGroupsDptViewerBySelection @BizUnit, @Selected, @IncludeAll, @UserID, @UnChecked" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>" SelectCommandType="Text">
        <SelectParameters>
            <asp:Parameter Name="BizUnit" />
            <asp:Parameter Name="Selected" />
            <asp:Parameter Name="IncludeAll" DefaultValue="0" />
            <asp:Parameter Name="UserID" />
            <asp:Parameter Name="UnChecked" DefaultValue=" " />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT 1" ProviderName="<%$ ConnectionStrings:PromoConnectionString.ProviderName %>">
    </asp:SqlDataSource>
</asp:Content>
