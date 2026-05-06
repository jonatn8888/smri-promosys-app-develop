
Imports System

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub cmdLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLogin.Click
        'test commit

        If txtUserName.Text = "" Or txtPassword.Text = "" Then

            lblMessage.ForeColor = Drawing.Color.Crimson
            lblMessage.Text = "Please enter a valid user name and password."
            txtUserName.Focus()

        Else
            Console.WriteLine(txtUserName.Text)

            If SystemUser.InitializeUser(txtUserName.Text, txtPassword.Text) Then
                If SystemUser.UserLevel = SystemUser.UserRoles.Administrator Then
                    Response.Redirect("ProjectTestPortal.aspx")
                Else
                    If (SystemUser.UserAccessSettings And 8) = 8 Then
                        Response.Redirect("Project_Portal_UAT.aspx")
                    Else
                        Response.Redirect("HomePage.aspx")
                    End If
                End If
            Else
                lblMessage.ForeColor = Drawing.Color.Crimson
                lblMessage.Text = "Invalid user name or password."
                txtPassword.Focus()
            End If

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' check if user is already logged-on
        If SystemUser.UserID <> 0 Then
            ' transfer user to the redirection page
            Server.Transfer("HomePage.aspx")
        Else
            lblTestEnvMsg.Visible = False 
		'(Request.ApplicationPath() <> "/Promotion")
        End If

    End Sub

End Class
