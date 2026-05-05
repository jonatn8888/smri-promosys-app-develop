
Partial Class ProjectTestPortal
    Inherits System.Web.UI.Page

    Dim taUsers As New dsPromotionsTableAdapters.UsersTableAdapter()
    Dim dtUsers As dsPromotions.UsersDataTable
    Dim rowUser As dsPromotions.UsersRow

    Protected Function AdminLoginAsUser(ByVal sUserName As String) As Integer

        dtUsers = taUsers.GetUserByUserName(sUserName)

        If dtUsers.Rows.Count > 0 Then

            rowUser = dtUsers.Rows(0)

            With rowUser
                Session("UserID") = .UserID
                Session("UserLogName") = .UserName
                Session("UserName") = .SignName
                Session("UserLevel") = .UserLevel
                Session("UserSignName") = .SignName
                Session("UserSignPosition") = .SignPosition
                Session("UserAccessSettings") = .AccessSettings
                Session("UserGroupID") = .GroupID
                Session("UserGroupType") = .GroupType   ' Addition session for grouptype
                Session("UserBizUnit") = .BizUnit       ' Addition session for business unit
            End With

            AdminLoginAsUser = Session("UserID")
        Else

            AdminLoginAsUser = 0
        End If

    End Function

    Protected Sub lnkCMMscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen.Click

        If AdminLoginAsUser("rd9406") > 0 Then ' CMM01
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkCMMscreen2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen2.Click

        If AdminLoginAsUser("CMM02") > 0 Then ' CMM01
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMPAscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMPAscreen.Click
        If AdminLoginAsUser("Analyst01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If
    End Sub

    Protected Sub lnkMemoApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMemoApprove.Click

        If AdminLoginAsUser("Approver01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMemoPreApp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMemoPreApp.Click

        If AdminLoginAsUser("Reviewer01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkProjMan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkProjMan.Click

        SystemUser.LogOut()

        Response.Redirect("Default.aspx")

    End Sub

    Protected Sub lnkPOSscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPOSscreen.Click

        If AdminLoginAsUser("POS01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserLevel <> SystemUser.UserRoles.Administrator Then Response.Redirect("InvalidAccess.aspx")

        If SystemUser.UserLogName.ToUpper = "NBS6962" OR SystemUser.UserLogName.ToUpper = "LTE8048" Then
            txtInputUsername.Visible = True
            btnLogin.Visible = True
        Else
            txtInputUsername.Visible = False
            btnLogin.Visible = False
        End If

    End Sub

    Protected Sub lnkMBUscreen2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMBUscreen2.Click

        If AdminLoginAsUser("MBU02") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMPAscreen2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMPAscreen2.Click

        If AdminLoginAsUser("Analyst02") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMemoPreApp2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMemoPreApp2.Click

        If AdminLoginAsUser("Reviewer02") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkDocumentation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDocumentation.Click

        Response.Redirect("Documentation.htm")

    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If txtInputUsername.Text <> "" Then
            If AdminLoginAsUser(txtInputUsername.Text) > 0 Then
                Response.Redirect("HomePage.aspx")
            End If
        End If
    End Sub

    Protected Sub lnkExecScreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkExecScreen.Click

        If AdminLoginAsUser("Exec01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkSMACscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSMACscreen.Click

        If AdminLoginAsUser("SMAC01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub


    Protected Sub lnkMBUscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMBUscreen.Click

        If AdminLoginAsUser("NTC8514") > 0 Then ' MBU01
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBranch.Click

        If AdminLoginAsUser("Viewer01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMdsgHeadscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMdsgHeadscreen.Click

        If AdminLoginAsUser("MdsgHead01") > 0 Then ' CMM01
            Response.Redirect("HomePage.aspx")
        End If

    End Sub
End Class
