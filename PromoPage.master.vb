
Partial Class PromoPage
    Inherits System.Web.UI.MasterPage

    Protected Sub lnkLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLogout.Click

        SystemUser.LogOut()

        Session.Abandon()

        Response.Redirect("Default.aspx")

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Dim sMenuItems As String
        Dim sDBsource As String

        If Not IsPostBack() Then

            ' check connection string if we're using the Data

            sDBsource = clsPromo.SQLConnString.Split(";")(0).Split("=")(1).ToString()

            '::restore::
            'If (sDBsource = "pssqlp01.smretail.com") Or (sDBsource = "PSSQLP01") Then
            '    lblTestEnvMsg.Text &= " LIVE DATA"
            'Else
            '    lblTestEnvMsg.Text &= " " & sDBsource
            'End If

            lblTestEnvMsg.Visible = (Request.ApplicationPath() <> "/Promotion")

            ' display user name and position
            lblUserName.Text = SystemUser.UserName
            lblUserGroup.Text = SystemUser.UserSignPosition

            '.:.remove.:.

            'sMenuItems = "<div id='nav_bar'><ul id='nav'>" & _
            '                    "<li><a href='#'>Event Participation</a></li>" & _
            '                        "<ul><li><a href='MallSaleCreatePromo.aspx'>Create Event Promotion Memo</a></li>" & _
            '                        "<li><a href='MallSaleDetailsEntry.aspx'>Event Participation Master File</a></li></ul>" & _
            '                    "</li></ul></div>"


            '.:.restore.:.

            If (SystemUser.UserLevel = SystemUser.UserRoles.AnnouncementViewer) Then

                sMenuItems = "<ul id='nav'><li><a href='HomePage.aspx'>Home</a></li>"

            Else

                sMenuItems = "<div id='nav_bar'><ul id='nav'><li><a href='HomePage.aspx'>Home</a></li>"

            End If

            If (SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover) Or _
                (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor) Or _
                (SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer) Then
                sMenuItems &= "<li><a href='PromoRequestList.aspx'>Promotional Requests</a></li>"
            End If

            Select Case SystemUser.UserLevel

                Case SystemUser.UserRoles.Analyst
                    'sMenuItems &= "<li><a href='PromoMemoListMain.aspx'>Requests and Memos</a></li>"

                Case SystemUser.UserRoles.Reviewer Or SystemUser.UserRoles.MemoApprover
                    sMenuItems &= "<li><a href='PromoMemoListMain.aspx'>Promotional Memos</a></li>"

            End Select

            ' Revised dowcarpio03062013@smretailinc: change label from Change Requests to Promotional Change based on meeting@03042013
            ' Added dowcarpio11122012@smretailinc: Cancellation/Addendum/Extension/Clarification
            If (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor) Or _
                (SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover) Or _
                (SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer) Or _
                (SystemUser.UserLevel = SystemUser.UserRoles.Analyst) Or _
                (SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover) Then '(SystemUser.UserLevel = SystemUser.UserRoles.Reviewer)(SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover)

                ' revised dowcarpio20131204@smretailinc: Promo change module not applicable for MPD-VP module already transferred/added on regular module of MPD-VP
                'If (SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover) Or (SystemUser.UserLevel = SystemUser.UserRoles.Reviewer) Then
                sMenuItems &= "<li><a href='#'>Promotional Change</a></li>" & _
                                                                "<ul><li><a href='RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("CCL", SystemUser.EncryptKey.ToString) & "'>Cancellation</a></li>" & _
                                                                "<li><a href='RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("EXT", SystemUser.EncryptKey.ToString) & "'>Extension</a></li>" & _
                                                                "<li><a href='RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("SWP", SystemUser.EncryptKey.ToString) & "'>Swipestakes Reseeding</a></li></ul></li>"
                ' Deleted dowcarpio20131204@smretailinc: deleted menu Addendum module not applicable as per MPD c/o Ms. Jess 
                '"<li><li><a href='RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("ADD", SystemUser.EncryptKey.ToString) & "'>Addendum</a></li>" 

                '"<li><a href='RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("CRF", SystemUser.EncryptKey.ToString) & "'>Clarification</a></li></ul></li>"
                'End If

            End If

            ' NBSantos20150120:: Promo Event Participation
            If SystemUser.UserLevel = SystemUser.UserRoles.Analyst Then
                sMenuItems &= "<li><a href='#'>Event Participation</a></li>" & _
                                    "<ul><li><a href='MallSaleCreatePromo.aspx'>Create Event Promotion Memo</a></li></ul></li>"

            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Then
                sMenuItems &= "<li><a href='#'>Event Participation</a></li>" & _
                                    "<ul><li><a href='MallSaleDetailsEntry.aspx'>Event Participation Master File</a></li></ul></li>"
                ' "<li><a href='MallSaleCreatePromo.aspx'>Create Event Promotion Memo</a></li>"

            End If

            ' maintenance menu
            If (SystemUser.UserAccessSettings And 1) = 1 Then

                'Revised dowcarpio11072012@smretailinc: deleted access to archiving settings.
                'Added dowcarpio06282012@smretailinc
                sMenuItems &= "<li><a href='#'>Utilities</a></li>" & _
                                "<ul>" & _
                                "<li><a href='ArchiveMemos.aspx'>Archive Memos</a></li></ul></li>"

                sMenuItems &= "<li><a href='#'>Maintenance</a></li>" & _
                                "<ul><li><a href='BranchMaintenance.aspx'>Branch Maintenance</a></li>" & _
                                "<li><a href='CompanyMaintenance.aspx'>Company Maintenance</a></li>" & _
                                "<li><a href='CompBranchesMaintenance.aspx'>Store Maintenance</a></li>" & _
                                "<li><a href='DepartmentMaintenance.aspx'>Department</a></li>" & _
                                "<li><a href='EmailMaintenance.aspx'>Emails</a></li>" & _
                                "<li><a href='PromoTypesList.aspx'>Promotion Types</a></li>" & _
                                "<li><a href='UserMaintenance.aspx'>Users</a></li>" & _
                                "<li><a href='BinRangeGroup.aspx'>Bin Range Group</a></li>" & _
                                "<li><a href='BankRangeGroup.aspx'>Bank Range Group</a></li>" & _
                                "<li><a href='eWalletPartners.aspx'>eWallet Partners</a></li>" & _
                                "<li><a href='ConsumerFinancing.aspx'>Consumer Financing</a></li>" & _
                                "<li><a href='ValueListMaintenance.aspx'>Shouldering Entity Maintenance</a></li>" & _
                                "</ul></li>"

            End If

            ' reports menu
            If SystemUser.UserLevel <= SystemUser.UserRoles.Analyst Then

                ' Added dowcarpio11082012@smretailinc: no access for admin
                If SystemUser.UserLevel <> SystemUser.UserRoles.Administrator Then
                    sMenuItems &= "<li><a href='#'>Reports</a></li><ul>" & _
                                  "<li><a href='rvPromoMonitoring.aspx'>Late Submissions</a></li>" & _
                                  "<li><a href='PromoSumPerType.aspx'>Promo per Type</a></li></ul>"

                End If

            End If

            'Added dowcarpio11072012@smretailinc: Archiving settings will be available to DB Admin/IT only.
            If SystemUser.UserLevel = SystemUser.UserRoles.Administrator Then
                sMenuItems &= "<li><a href='#'>Utilities</a></li>" & _
                                "<ul><li><a href='ArchiveSettings.aspx'>Archive Settings</a></li>" & _
                                "<li><a href='ArchiveMemos.aspx'>Archive Memos</a></li></ul></li>"
            End If

            If SystemUser.UserLevel <> SystemUser.UserRoles.SMACapprover And _
               SystemUser.UserLevel <> SystemUser.UserRoles.POSpersonnel Then
                sMenuItems &= "<li><a href='PriceEventSearch.aspx'>Price Events</a></li>"
            End If

            If SystemUser.UserLevel = SystemUser.UserRoles.POSpersonnel Then
                ' 03-Sept-2018 RPR1001
                sMenuItems &= "<li><a href='PromoReport.aspx'>Reports</a></li>"
            End If

            If (SystemUser.UserLevel = SystemUser.UserRoles.AnnouncementViewer) Then
                sMenuItems &= "<li><a href='PromoAnnouncement.aspx'>Announcements</a></li></ul>"
            Else
                sMenuItems &= "<li><a href='PromoAnnouncement.aspx'>Announcements</a></li></ul></div>"
            End If

            litMenuList.Text = sMenuItems
        End If

    End Sub

End Class

