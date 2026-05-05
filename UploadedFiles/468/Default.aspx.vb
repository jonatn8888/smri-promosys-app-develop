
Partial Class _Default
    Inherits System.Web.UI.Page


Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

       Dim h As HiddenField
       h = CType(Master.FindControl("UserGroup"), HiddenField)
       Dim a = Trim(Request.QueryString("a"))
        Dim usergroup As String = String.Empty

        '=== May 01, 2010 ====
        Dim username As String = String.Empty
        Dim uname() As String
        '=== May 01, 2010 ====

       Try
           Dim c As New Reports
            usergroup = c.GetUserGroup(Context.User.Identity.Name)

            '=== May 01, 2010 =====
            username = Context.User.Identity.Name
            uname = Split(username, "\")
            If Len(uname(1)) = 6 Then
                If (Left(Right(username, 4), 1) = "0" Or Left(Right(username, 4), 1) = "4") Then
                    Response.Redirect("InvalidAccess.aspx")
                End If
            End If
            '=== May 01, 2010 =====

            Session("UG") = usergroup
        Catch ex As Exception
            Response.Write("<script>Alert('" & ex.Message & "');</script>")
        End Try

       If a = "" Then
           Select Case usergroup.ToUpper.Trim
               Case "CREDIT"
                   l.Text = "<iframe src='header_info.aspx?download_url=&a=true&report_title=Bulletin Inquiry&url=/BULLETIN/Credit Bulletin Query' id='Viewer' width='100%' height='100%' frameborder='1' name='Viewer' scrolling='no'></iframe>"
               Case "MPC"
                   l.Text = "<iframe src='header_info.aspx?a=true&url=/BULLETIN/SM Bulletin Query&download_url=&report_title=Bulletin Inquiry' id='Viewer' width='100%' height='100%' frameborder='1' name='Viewer' scrolling='no'></iframe>"
               Case Else
                   l.Text = "<iframe src='header_info.aspx?download_url=&a=true&report_title=BULLETIN&url=/BULLETIN/bulletin' id='Viewer' width='100%' height='100%' frameborder='1' name='Viewer' scrolling='no'></iframe>"
           End Select
       Else
           l.Text = "<iframe src='' id='Viewer' width='100%' height='100%' frameborder='1' name='Viewer' scrolling='no'></iframe>"
       End If

   End Sub
    
End Class
