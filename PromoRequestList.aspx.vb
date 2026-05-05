
Partial Class PromoRequestList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        Select Case SystemUser.UserLevel

            Case SystemUser.UserRoles.PromoRequestor
                Server.Transfer("PromoReqListCMM.aspx")

            Case SystemUser.UserRoles.RequestApprover
                Server.Transfer("PromoReqList.aspx")

            Case SystemUser.UserRoles.ExecutiveApprover
                Server.Transfer("PromoReqListExec.aspx")

            Case SystemUser.UserRoles.SMACapprover
                Server.Transfer("PromoReqListSMAC.aspx")

            Case SystemUser.UserRoles.Analyst
                Server.Transfer("PromoMemoListMPA.aspx")

            Case Else
                Response.Redirect("HomePage.aspx")

        End Select

    End Sub
End Class
