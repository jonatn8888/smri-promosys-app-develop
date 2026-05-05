<%@ Page Language="VB" AutoEventWireup="false" CodeFile="texteditor.aspx.vb" Inherits="texteditor" validateRequest="false"  %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Text Editor</title>
    <link rel="stylesheet" type="text/css" href=".\css\main.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
      <asp:HiddenField ID="HiddenField1" runat="server" />
    <div>
    <CKEditor:CKEditorControl ID="ckEditor" runat="server" Height="250" BasePath="~/ckeditor" ToolbarBasic="NewPage|Preview|-|Templates&#13;&#10;Cut|Copy|Paste|PasteText|PasteFromWord&#13;&#10;Undo|Redo|-|Find|Replace|-|SelectAll|RemoveFormat&#13;&#10;Bold|Italic|Underline|Strike|-|Subscript|Superscript&#13;&#10;TextColor|BGColorTextColor|BGColor&#13;&#10;/&#13;&#10;NumberedList|BulletedList|-|Outdent|Indent&#13;&#10;JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock&#13;&#10;Table|HorizontalRule|SpecialChar&#13;&#10;Styles|Format|Font|FontSize">
	sample	              
	</CKEditor:CKEditorControl>
	    <table style="width:100%">
             <tr>
                   <td style="text-align: right">
                         <asp:Button ID="cmdPopOK" runat="server" Text="Accept" Width="83px" />
                         <asp:Button ID="cmdPopCancel" runat="server" Text="Cancel" Width="83px" />
                   </td>
             </tr>
     </table>
	  
    </div>
    
    </form>
    
</body>
</html>
