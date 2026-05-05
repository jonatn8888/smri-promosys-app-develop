
Partial Class Project_Portal_UAT
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
                Session("UserGroupType") = .GroupType ' Added dowcarpio09182012@smretailinc: Addition session for grouptype

            End With

            AdminLoginAsUser = Session("UserID")
        Else

            AdminLoginAsUser = 0
        End If

    End Function

    Protected Sub lnkCMMscreen1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen1.Click

        If AdminLoginAsUser("CMM01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkCMMscreen2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen2.Click

        If AdminLoginAsUser("CMM02") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkCMMscreen3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen3.Click

        If AdminLoginAsUser("CMM03") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkCMMscreen4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen4.Click

        If AdminLoginAsUser("CMM04") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkCMMscreen5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen5.Click

        If AdminLoginAsUser("CMM05") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMBUscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMBUscreen.Click

        If AdminLoginAsUser("MBU01") > 0 Then
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

    Protected Sub lnkBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBranch.Click

        If AdminLoginAsUser("Viewer01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If (SystemUser.UserAccessSettings And 8) = 0 Then Response.Redirect("InvalidAccess.aspx")

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

    Protected Sub lnkBranch02_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBranch02.Click

        If AdminLoginAsUser("Viewer02") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkSameUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSameUser.Click

        Response.Redirect("HomePage.aspx")

    End Sub

    Protected Sub lnkSMACscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSMACscreen.Click

        If AdminLoginAsUser("SMAC01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    'Added dowcarpio09192012@smretailinc: SACI UAT
    Protected Sub lnkCMMscreen8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen8.Click
        If AdminLoginAsUser("CMM08") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If
    End Sub

    Protected Sub lnkCMMscreen6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen6.Click
        If AdminLoginAsUser("CMM06") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If
    End Sub

    Protected Sub lnkCMMscreen7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCMMscreen7.Click
        If AdminLoginAsUser("CMM07") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If
    End Sub

    Protected Sub lnkDBAdmin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDBAdmin.Click
        If AdminLoginAsUser("nbs6962") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If
    End Sub

    Protected Sub lnkMdsgHeadscreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMdsgHeadscreen.Click

        If AdminLoginAsUser("MdsgHead01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMdsgHeadscreen2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMdsgHeadscreen2.Click

        If AdminLoginAsUser("MdsgHead02") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkSBUApprover_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSBUApprover.Click

        If AdminLoginAsUser("SBUHEAD01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkMdsgHeadscreen3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMdsgHeadscreen3.Click

        If AdminLoginAsUser("SBUREV01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkBCRscreen1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBCRscreen1.Click

        If AdminLoginAsUser("BCRtest01") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub

    Protected Sub lnkBCRApprover1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBCRApprover1.Click

        If AdminLoginAsUser("BCRapprover1") > 0 Then
            Response.Redirect("HomePage.aspx")
        End If

    End Sub
End Class
