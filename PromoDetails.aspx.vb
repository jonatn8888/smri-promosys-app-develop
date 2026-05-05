
Partial Class PromoDetails
    Inherits System.Web.UI.Page


    Protected Sub Button1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.ServerClick
        ' Added dowcarpio08082013@smretailinc: to validate subclass and class discount promotion Per MPD c/o Ms Jessica
        Session("PromoType") = "0"
        Me.hidFlag.Value = 1
        ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>closeWin();</script>")
    End Sub


    Protected Sub Button2_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.ServerClick
        Me.hidFlag.Value = 0
        ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>closeWin();</script>")
    End Sub
End Class
