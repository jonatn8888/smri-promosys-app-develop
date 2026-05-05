
Partial Class SearchPromotion
    Inherits System.Web.UI.Page



    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Select" Then
            clsSession.CurrRequestID = e.CommandArgument
            ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.searchwindow.hide();</script>")
        End If
    End Sub
End Class
