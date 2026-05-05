
Partial Class PromoMemoListMain
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("UserLevel") = Nothing Then Response.Redirect("InvalidAccess.aspx")

        Select Case SystemUser.UserLevel
            Case SystemUser.UserRoles.Analyst
                Server.Transfer("PromoMemoListMPA.aspx")

            Case SystemUser.UserRoles.Reviewer
                Server.Transfer("PromoMemoList.aspx")

            Case SystemUser.UserRoles.MemoApprover
                Server.Transfer("PromoMemoList.aspx")

            Case Else
                Response.Redirect("InvalidAccess.aspx")

        End Select

    End Sub
End Class
