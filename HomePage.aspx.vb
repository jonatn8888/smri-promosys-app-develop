
Partial Class HomePage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        Select Case SystemUser.UserLevel

            Case SystemUser.UserRoles.PromoRequestor
                Server.Transfer("HomePageCMM.aspx")

            Case SystemUser.UserRoles.RequestReviewer
                Server.Transfer("PromoReqList.aspx")

            Case SystemUser.UserRoles.RequestApprover
                Server.Transfer("PromoReqList.aspx")

            Case SystemUser.UserRoles.ExecutiveApprover
                Server.Transfer("PromoReqListExec.aspx")

            Case SystemUser.UserRoles.SMACapprover
                Server.Transfer("PromoReqListSMAC.aspx")

            Case SystemUser.UserRoles.Analyst
                'Server.Transfer("MallSaleDetailsEntry.aspx")
                Server.Transfer("PromoMemoListMPA.aspx")

            Case SystemUser.UserRoles.Reviewer
                Server.Transfer("PromoMemoListMain.aspx")

            Case SystemUser.UserRoles.MemoApprover
                Response.Redirect("PromoMemoListMain.aspx")

            Case SystemUser.UserRoles.POSpersonnel
                Response.Redirect("HomePagePOS.aspx")

            Case SystemUser.UserRoles.AnnouncementViewer
                Server.Transfer("PromoAnnouncement.aspx")

            Case SystemUser.UserRoles.Administrator         ' Archiving Settings for IT/DB admin account only
                Server.Transfer("ArchiveSettings.aspx")

        End Select



    End Sub



End Class
