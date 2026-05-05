
Partial Class PromoTypeList
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblPromoTypeID")

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Text = "<a href='PromoTypeEntry.aspx?xmode=1&promotypeid=" & lb.Text & "'>" & e.Row.Cells(1).Text & "</a>"
            e.Row.Cells(3).Text = IIf(e.Row.Cells(3).Text = "True", "<img src='Images/orange-blob.gif' />", "") & "</b>"

        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        'Response.Redirect("" & GridView1.SelectedValue.ToString())

    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click

        Response.Redirect("PromoTypeEntry.aspx?xmode=0&promotypeid=0")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")

        If (Session("UserLevel") Is Nothing) Or (Session("UserLevel") > SystemUser.UserRoles.Analyst) Then
            Response.Redirect("InvalidAccess.aspx")
        End If

    End Sub
End Class
