<%@ Page Language="VB" AutoEventWireup="false" CodeFile="insert_image.aspx.vb" Inherits="rte_insert_image" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Insert Image</title>
    <script language="javascript" type="text/javascript">
	function AddImage() {
		if (document.getElementById("url").value != "") {
				var html = "";
						html += '<img src="' + document.getElementById("url").value + '"';
					if (document.getElementById("border").value != "") {
						html += ' border="' + document.getElementById("border").value + '"';
					}
					if (document.getElementById("hspace").value != "") {
						html += ' hspace="' + document.getElementById("hspace").value + '"';
					}
					if (document.getElementById("vspace").value != "") {
						html += ' vspace="' + document.getElementById("vspace").value + '"';
					}
					if (document.getElementById("align").value != "" && document.getElementById("align").value != "Default" ) {
						html += ' align="' + document.getElementById("align").value + '"';
					}
					if (document.getElementById("alt").value != "") {
						html += ' alt="' + document.getElementById("alt").value + '"';
						html += ' title="' + document.getElementById("alt").value + '"';
					}
						html += ' />';
			window.opener.rteInsertHTML(html);
			window.close();
		} else {
			alert('You must select an image!');
		}
	}
</script>
    <style type="text/css">
body, td {
background-color:#ECE9D8;
font-family:arial;
font-size:11px;
}
input {
font-family:arial;
font-size:11px;
}
select {
font-family:arial;
font-size:11px;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	<fieldset style="width: 462px">
	<legend><strong>Insert Image</strong></legend>
	  <table border="0" cellpadding="0" cellspacing="2" style="width: 462px; height: 149px">
		  <tr>
			<td style="width:88px;">Image Path</td>
			<td style="width:138px;">
                &nbsp;<asp:FileUpload ID="fileUpEx" runat="server" Font-Size="Smaller" Width="345px" /></td>
		  </tr>
		  <tr>
			<td style="width: 88px">Image Description </td>
			<td style="width: 138px"><input id="alt" type="text" style="background-color:#FFFFFF; border:1px solid #828177; font-family:arial; font-size:11px; color: #003399; width: 273px;"></td>
		  </tr>
		  <tr>
			<td style="width: 88px; height: 20px">Alignment</td>
			<td style="width: 138px; height: 20px"><select id="align" style="width: 276px">
			  <option></option>
			  <option value="baseline">Baseline</option>
			  <option value="top">Top</option>
			  <option value="middle">Middle</option>
			  <option value="bottom">Bottom</option>
			  <option value="texttop">TextTop</option>
			  <option value="absmiddle">Absolute Middle</option>
			  <option value="absbottom">Absolute Bottom</option>
			  <option value="left">Left</option>
			  <option value="right">Right</option>
			</select>        </td>
		  </tr>
		  <tr>
			<td style="width: 88px; height: 20px">Border</td>
			<td style="width: 138px; height: 20px"><input name="border" type="text" id="border" value="0" size="3" maxlength="3" style="background-color:#FFFFFF; border:1px solid #828177; font-family:arial; font-size:11px; color: #003399;"/></td>
		  </tr>
		  <tr>
			<td style="width: 88px">HSpace</td>
			<td style="width: 138px"><input name="hspace" type="text" id="hspace" size="3" maxlength="3" style="background-color:#FFFFFF; border:1px solid #828177; font-family:arial; font-size:11px; color: #003399;" /></td>
		  </tr>
		  <tr>
			<td style="width: 88px">VSpace</td>
			<td style="width: 138px"><input name="vspace" type="text" id="vspace" size="3" maxlength="3" style="background-color:#FFFFFF; border:1px solid #828177; font-family:arial; font-size:11px; color: #003399;"/></td>
		  </tr>
		  <tr>
			<td colspan="2">&nbsp; &nbsp;<asp:Label ID="Label1" runat="server" Font-Size="Larger" ForeColor="Red"></asp:Label></td>
		  </tr>
		  <tr>
			<td colspan="2" align="center" style="height: 21px"><input type="submit" name="Submit" value="Insert Image" runat="server" id="Submit1" /></td>
		  </tr>
	  </table>
	  <div style="display:none">
        <input id="url" style="border-right: #828177 1px solid; border-top: #828177 1px solid;
            font-size: 11px; border-left: #828177 1px solid; color: #003399; border-bottom: #828177 1px solid;
            font-family: arial; background-color: #ffffff" type="text" runat="server" />
      </div>
      </fieldset>
        </div>
    </form>
</body>
</html>
