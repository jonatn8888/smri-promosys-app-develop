<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UploadSeedFiles.aspx.vb" Inherits="UploadSeedFiles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
    <script type="text/javascript" language="javascript">
    
    function DeleteConfirmation()
    {
    if (confirm("Are you sure,you want to delete selected records ?")==true)
       return true;
    else
       return false;
    }
    </script>
    
    
</head>
<body  style="background: white;">
    <form id="form1" runat="server">
    
    <div id="subvbAttachment">
        <table style="width: 139px; height: 212px">
            <tr>
                <td style="height: 24px">
        <asp:FileUpload ID="fileUpEx" runat="server" Height="23px" Width="363px" AllowMutiple="True" /></td>
                <td style="width: 72px; height: 24px;">
        <asp:Button ID="btnSubmit" runat="server" Height="23px" Text="Add" Width="74px" /></td>
                <td style="width: 35px; height: 24px;">
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 190px" valign="top">
        <asp:GridView ID="GridView1"  runat="server" AutoGenerateColumns="False"
            Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Size="8pt" AllowPaging="True" PageSize="5" Font-Strikeout="False">
             
            <Columns>
   
                <asp:TemplateField>
                 <HeaderTemplate>
                  <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack ="true" OnCheckedChanged="chkSelectAll_CheckedChanged" Text="Select All" />
              </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                     <FooterTemplate>
                         &nbsp;
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="AttachedFiles" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
               
             </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
       
        </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2" rowspan="1" class="field-cell">
                    <asp:BulletedList ID="blistErrorMsg" runat="server" CssClass="error-list" Width="99%" style="margin-bottom: 0px !important;  margin-left: 0px !important; height: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="height: 25px">
                    <asp:LinkButton ID="Button1" OnClientClick="return DeleteConfirmation();" runat="server" Height="22px" Width="235px">Delete Selected File</asp:LinkButton></td>
                <td style="width: 72px; height: 25px">
                    <asp:LinkButton ID="LinkButton1" runat="server" Width="50px">Close</asp:LinkButton></td>
                <td style="width: 35px; height: 25px">
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>
