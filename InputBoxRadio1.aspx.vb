
Partial Class InputBoxRadio1
    Inherits System.Web.UI.Page

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If txtInputbox.Text = "" Then
            errormessage.Visible = True
            blistErrorMsg.Visible = True
            blistErrorMsg.Text = "•Please indicate reason why this document is being returned to " & rblReciever.SelectedValue & "."
            Exit Sub
        End If

        hidchoice.Value = rblReciever.SelectedValue
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.InputBoxwindow.hide();</script>")

    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

        litPopMessage.Text = clsSession.Message
        errormessage.Visible = False
        clsSession.DeleteStatus = "yes"

        imgIcon.ImageUrl = imgQuestion.ImageUrl

        'cmdPopUpOK.Text = "Yes"
        'cmdCancel.Text = "No"

    End Sub


    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        clsSession.DeleteStatus = "cancel"
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.InputBoxwindow.hide();</script>")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            rblReciever.SelectedValue = "MPD Analyst"
        End If
    End Sub
End Class
