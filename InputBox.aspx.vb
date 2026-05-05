
Partial Class InputBox
    Inherits System.Web.UI.Page

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If txtInputbox.Text = "" Then
            errormessage.Visible = True
            blistErrorMsg.Visible = True
            blistErrorMsg.Text = "•Please indicate reason why this document is being returned to sender."
            Exit Sub
        End If
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.InputBoxwindow.hide();</script>")
    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0) Then Response.Redirect("InvalidAccess.aspx")

        litPopMessage.Text = clsSession.Message
        errormessage.Visible = False
        clsSession.DeleteStatus = "yes"
        imgIcon.ImageUrl = imgQuestion.ImageUrl
        'cmdPopUpOK.Text = "Yes"
        'cmdCancel.Text = "No"

        cmdPopUpOK.Text = "Accept"
        cmdCancel.Text = "Cancel"

    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        clsSession.DeleteStatus = "cancel"
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.InputBoxwindow.hide();</script>")
    End Sub
End Class
