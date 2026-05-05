
Partial Class texteditor
    Inherits System.Web.UI.Page

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0) Then Response.Redirect("InvalidAccess.aspx")

    End Sub

    Protected Sub cmdPopOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopOK.Click

        HiddenField1.Value = ckEditor.Text  'Request("txtEditor")
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.texteditorwindow.hide();</script>")

    End Sub

    Protected Sub cmdPopCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopCancel.Click

        HiddenField1.Value = clsSession.Mechanics
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.texteditorwindow.hide();</script>")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ckEditor.ResizeEnabled = False
            ckEditor.Toolbar = ckEditor.ToolbarBasic
            ckEditor.Text = clsSession.Mechanics

        End If

    End Sub

End Class
