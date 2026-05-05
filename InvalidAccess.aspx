<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InvalidAccess.aspx.vb" Inherits="InvalidAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Invalid Request</title>
</head>
<body style="font-family: Trebuchet MS; background: url('') no-repeat;">

    <form id="form1" runat="server">
    <div>
        <strong><span style="color: red; font-size: 14pt;">INVALID REQUEST</span></strong><br />
    </div>

    <div>
        <hr />
    </div>

    <div>
        <br />
            You have requested a page where you are not allowed to access.
        <br />
    </div>
        
        <div>
          <ul>
           <li>
            It is possible that you have called the page using a link or history log.<br />
            Kindly use the home page of ePromo to correct this.
           </li> 
           <li>
            If you are certain that you should have access to this page, please contact your
            IT administrator.
           </li>
          </ul>
        </div>
        
        <div>
        Proceed to <a href="Default.aspx"> ePromo Home Page</a>
        </div>
        
    </form>
</body>
</html>
