Imports dsPromotionsTableAdapters
Imports System.Data
Imports System.IO
Imports System.Data.SqlClient

Partial Class PromoRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0) Or _
            (SystemUser.UserLevel = SystemUser.UserRoles.POSpersonnel) Or _
            (SystemUser.UserLevel = SystemUser.UserRoles.AnnouncementViewer) Or _
            (SystemUser.UserLevel = SystemUser.UserRoles.BatchSaleRequestor) Then Response.Redirect("InvalidAccess.aspx")

        ' -- #TODO: Upgrade to ViewState variable
        If Request("RequestID") <> Nothing Then
            ViewState("CurrRequestID") = Request("RequestID")
            clsSession.CurrRequestID = Request("RequestID")
        Else
            ' invalid call
        End If

        If Not IsPostBack() Then

            If Request("Filename") <> Nothing Then
                Dim fname As String
                fname = clsPromo.pathAttachment & ViewState("CurrRequestID").ToString & "\" & Request("Filename")
                Response.ContentType = "application/x-msdownload"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" & Request("Filename"))
                Response.TransmitFile(fname)
                Response.End()
            End If

            If Request("DownLoad") <> Nothing And Request("Path") <> Nothing Then
                Dim fname As String

                'fname = Server.MapPath("~/posfiles" & Session("FolderParent").ToString & "/" & Request("DbName"))
                fname = Request("Path") & ".zip"

                Response.ContentType = "application/x-msdownload"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" & Request("DownLoad"))
                Response.TransmitFile(fname)
                Response.End()
            End If

            Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()
            Dim dtPromoRequests As New dsPromotions.PromoRequestsDataTable
            Dim rowPromoRequest As dsPromotions.PromoRequestsRow

            ' store PromoRequest information to ViewState
            If Not GetPromoRequestInfo(ViewState("CurrRequestID")) Then
                ' error accessing PromoRequest information
                blistErrorMsg.Items.Add("Error accessing Promo Request information.")
                Exit Sub
            End If

            ' get PromoRequest information
            dtPromoRequests = taPromoRequests.GetPromoRequestByID(ViewState("CurrRequestID"))

            If dtPromoRequests.Rows.Count = 0 Then
                ' error accessing PromoRequest information
                blistErrorMsg.Items.Add("Error accessing Promo Request information.")
                Exit Sub
            Else
                rowPromoRequest = dtPromoRequests.Rows(0)

                With rowPromoRequest
                    'ViewState("PeriodFrom") = .PromoPeriodFrom.Date()
                    'ViewState("PeriodTo") = .PromoPeriodTo.Date()

                    lblRequestID.Text = Format(.RequestID, "0####") '& Format(Now.Year, "-0#")
                    lblRequestDate.Text = .RequestDate.ToLongDateString()

                    lblBranches.Text = .Branches

                    lblPromoTitle.Text = .Title.ToString()

                    lblPeriodFrom.Text = ViewState("PeriodFrom") ' .PromoPeriodFrom.ToLongDateString()
                    lblPeriodTo.Text = ViewState("PeriodTo")     '.PromoPeriodTo.ToLongDateString()

                    lblStatus.ForeColor = Drawing.Color.Green
                    lblStatus.Text = .Status

                    lblRequestedBy.Text = .RequestedBy.ToString() & " / " & .RequesterPos.ToString()
                    lblReviewedBy.Text = .ReviewedBy.ToString() & " / " & .ReviewerPos.ToString()
                    lblApprovedBy.Text = .ApprovedBy.ToString() & " / " & .ApproverPos.ToString()

                    tbrowApprover.Visible = (lblApprovedBy.Text <> String.Empty)

                    lblRemarks.Text = .Remarks.ToString()



                    If ViewState("IsTemplatedPromo") Then
                        trGuidelines.Visible = True
                        litGuidelines.Text = Server.HtmlDecode(.TemplatedGuideline.ToString())
                    Else
                        If ViewState("PromotypeID").ToString() = "59" Then
                            trGuidelines.Visible = True
                            litGuidelines.Text = Server.HtmlDecode(GetPromoGuidelines(clsSession.CurrRequestID))
                        Else
                            trGuidelines.Visible = False
                        End If
                    End If

                    ' for MPD analyst only
                    If SystemUser.UserLevel = SystemUser.UserRoles.Analyst And lblStatus.Text = "For MPD Processing" Then
                        lnkCreateMemoDraft.Visible = True

                        'CHECK_HERE! Remove hardcoding for Owner GroupID 99 (3-Day Sale)
                        If ViewState("IsTemplatedPromo") And _
                            ViewState("PromotypeID").ToString() <> "59" And _
                            DateDiff(DateInterval.Day, CDate("2018/08/01"), CDate(lblRequestDate.Text)) >= 0 _
                            And ViewState("OwnerGroup") <> 99 Then
                            '  Or .GroupType = "SBU"
                            lnkCreateMemoDraft.Text = "Submit Memo Draft"
                        ElseIf ViewState("PromotypeID").ToString() = "59" And ViewState("IsTemplatedPromo") = True Then
                            lnkCreateMemoDraft.Text = "Create Memo Draft"
                        Else
                            lnkCreateMemoDraft.Text = "Create Memo Draft"
                        End If

                    Else
                        lnkCreateMemoDraft.Visible = False
                    End If

                    'hide promo item for SBU Marketing Requestor
                    'If (.GroupType = "SBU") Or (.GroupType = "BCR") Then

                    'show column if Dept/SDept/Class data is available
                    If gridPromotions.Rows.Count > 0 Then
                        gridPromotions.Columns(2).Visible = (gridPromotions.Rows(0).Cells(2).Text <> "&nbsp;")
                    End If

                End With
            End If

            ' for MPD analyst only
            'lnkCreateMemoDraft.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.Analyst And lblStatus.Text = "For MPD Processing")
            lnkAllowRush.Visible = False
            lnkAllowRushWCD.Visible = False

            'lnkAllowRush.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover)

            lnkEditDoc.Visible = ((lblStatus.Text = "Draft" Or lblStatus.Text = "Returned") And SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor)

            ' disable control based on event calendar
            If lnkEditDoc.Visible Then lnkEditDoc.Enabled = (lblStatus.Text = "Returned" Or clsPromo.GetAddRequestButtonState())

            lnkDelete.Visible = ((lblStatus.Text = "Draft") And SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor)

            ' Revised 20170112: Include Mdsg/Group Head Approver
            lnkCancelRequest.Visible = ((lblStatus.Text = "For MBU Approval" And SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover) Or _
                                        (lblStatus.Text = "For Mdsg/Group Head Approval" And SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer) Or _
                                        (lblStatus.Text = "For Initial Approval" And SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover) Or _
                                        (lblStatus.Text = "Special Request" And SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover) Or _
                                        (lblStatus.Text = "For DTI Application" And SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover) Or _
                                        (lblStatus.Text = "For MPD Processing" And SystemUser.UserLevel = SystemUser.UserRoles.Analyst) Or _
                                        (lblStatus.Text = "For HTS Approval" And SystemUser.UserLevel = SystemUser.UserRoles.ExecutiveApprover) Or _
                                        (lblStatus.Text = "For MCI Approval" And SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover) Or _
                                        (lblStatus.Text = "For BU Head Approval" And SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover))

            ' Revised 20170112: Include Mdsg/Group Head Approver
            ' for MBU and request not yet submitted
            lnkApprove.Visible = ((SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover And lblStatus.Text = "For MBU Approval") Or _
                                (SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover And lblStatus.Text = "For BU Head Approval") Or _
                                 (SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer And lblStatus.Text = "For Mdsg/Group Head Approval") Or _
                                 (SystemUser.UserLevel = SystemUser.UserRoles.ExecutiveApprover And lblStatus.Text = "For HTS Approval") Or _
                                 (SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover And lblStatus.Text = "For MCI Approval") Or _
                                 (SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover And lblStatus.Text = "For Initial Approval"))

            ' for Memo Approver
            If SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover Then
                If lblStatus.Text = "Special Request" Then
                    lnkAllowRush.Visible = True
                    lnkAllowRushWCD.Visible = True
                End If
            End If

            lnkSubmitRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover And lblStatus.Text = "For DTI Application")
            lnkattachment.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover And lblStatus.Text = "For DTI Application")

            'If (DateDiff(DateInterval.Day, Today(), ViewState("PeriodFrom")) < 0) And (lblStatus.Text <> "Approved") Then
            '    lblStatus.ForeColor = Drawing.Color.Red
            '    lblStatus.Text &= " - Expired"
            'End If

        End If

        ' attachments management
        If IsSmacDealsPromo() Then
            Dim f As New IO.FileInfo(clsPromo.pathAttachment & ViewState("CurrRequestID").ToString())

            If Not f.Exists Then
                clsSession.AttachmentPath = clsPromo.pathAttachment & ViewState("CurrRequestID").ToString()
                Directory.CreateDirectory(clsSession.AttachmentPath)
            End If
        End If

        ShowAttachments()
        ShowSeedAttachments()
        ShowMenu()
    End Sub

    Private Sub ShowAttachments()

        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & ViewState("CurrRequestID").ToString
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""PromoRequest.aspx?RequestID=" & _
                                    ViewState("CurrRequestID").ToString & "&FileName=" & _
                                    Server.UrlEncode(file.Name.ToString) & """>" & file.Name.ToString & "</a> "
            Next

            If Len(strFiles) = 0 Then lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr
    End Sub

    Private Sub ShowSeedAttachments()

        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & ViewState("CurrRequestID").ToString

        Dim pathTemp As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = clsSession.CurrRequestID.ToString & "-Swipestakes"
        pathTemp = swipestakeSeedFolder & subFolderName

        lblSeedFile.Text = ""

        If System.IO.Directory.Exists(pathTemp) Then
            Dim dir As New System.IO.DirectoryInfo(pathTemp)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblSeedFile.Text &= "<img src='Images/bullet green.gif' /><a href=""PromoRequest.aspx?RequestID=" & _
                                    ViewState("CurrRequestID").ToString & "&FileName=" & _
                                    Server.UrlEncode(file.Name.ToString) & """>" & file.Name.ToString & "</a> "
            Next

            If Len(strFiles) = 0 Then lblSeedFile.Text = ""
        End If

        imgbtnSeedDownload.Visible = tr
    End Sub

    Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

        Const nDescCol As Integer = 3
        Const nItemCol As Integer = 2
        Static rowPrevious As GridViewRow
        Static rowPrevItem As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(nDescCol).Text = Server.HtmlDecode(e.Row.Cells(nDescCol).Text)
        End If

        Dim sql As String = sqldsPromos.SelectCommand

        ' Replace the parameters manually
        sql = sql.Replace("@RequestID", "'" & Session("CurrRequestID") & "'")
        sql = sql.Replace("@UserID", "'" & Session("UserID") & "'")



        If e.Row.RowIndex = 0 Then
            rowPrevious = e.Row
            rowPrevItem = e.Row
        End If

        If e.Row.RowIndex > 0 Then

            ' merge Description cells with same PromoID
            Dim lbPrev As Label = rowPrevious.FindControl("lblPromoID")
            Dim lbCurr As Label = e.Row.FindControl("lblPromoID")

            If lbCurr.Text = lbPrev.Text Then

                If rowPrevious.Cells(nDescCol).RowSpan < 2 Then
                    'rowPrevious.Cells(0).RowSpan = 2
                    rowPrevious.Cells(nDescCol).RowSpan = 2
                Else
                    'rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
                    rowPrevious.Cells(nDescCol).RowSpan = rowPrevious.Cells(nDescCol).RowSpan + 1
                End If

                'e.Row.Cells(0).Visible = False
                e.Row.Cells(nDescCol).Visible = False
            Else
                rowPrevious = e.Row
            End If

            If e.Row.Cells(nItemCol).Text = rowPrevItem.Cells(nItemCol).Text Then

                If rowPrevItem.Cells(nItemCol).RowSpan < 2 Then
                    rowPrevItem.Cells(nItemCol).RowSpan = 2
                Else
                    rowPrevItem.Cells(nItemCol).RowSpan = rowPrevItem.Cells(nItemCol).RowSpan + 1
                End If

                e.Row.Cells(nItemCol).Visible = False

            Else
                rowPrevItem = e.Row
            End If
        End If

    End Sub

    Protected Function GetPromoRequestInfo(ByVal nRequestID As Long) As Boolean

        Dim drRow As DataRow = Nothing
        Dim strQuery As String

        Dim lResult As Boolean = False

        strQuery = "SELECT COALESCE(R.WorkFlowCode, '') AS sWorkFlowCode, " & _
                        "  Coalesce(T.NumLeadDaysApprovalBU, T.NumLeadDays) AS nNumLeadDays, " & _
                        "COALESCE(T.NumLeadDaysApprovalMPD, 0) AS nNumLeadDaysApprovalMPD, " & _
                        "  Coalesce(FORMAT(CAST(T.ApprovalCutoffBU AS TIME), 'hh:mm tt'), '11:00 AM') AS sApprovalCutoffBU, " & _
                        "CAST(LEFT(Coalesce(T.ApprovalCutoffBU, '11:00'), 2) AS INT) * 60 + " & _
                        "CAST(RIGHT(Coalesce(T.ApprovalCutoffBU, '11:00'), 2) AS INT) AS nApprovalCutoffMinutes " & _
                        ",R.PromoPeriodFrom, R.PromoPeriodTo " & _
                        ",R.OwnerGroup, R.RequestDate " & _
                        ",T.PromotypeID " & _
                        ",T.isMPDEscalate " & _
                        ",COALESCE(T.IsTemplated, 0, T.IsTemplated) AS TemplatedPromo " & _
                    "FROM PromoRequests AS R " & _
                    "LEFT JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                    "LEFT JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                    "WHERE R.RequestID = 0" & nRequestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
            ViewState("PeriodFrom") = drRow("PromoPeriodFrom")
            ViewState("PeriodTo") = drRow("PromoPeriodTo")
            ViewState("RequestDate") = drRow("RequestDate")
            ViewState("isMPDEscalate") = drRow("isMPDEscalate")

            ViewState("PromotypeID") = drRow("PromotypeID")
            ViewState("WorkFlowCode") = drRow("sWorkFlowCode")
            ViewState("NumLeadDays") = drRow("nNumLeadDays")
            ViewState("NumLeadDaysApprovalMPD") = drRow("nNumLeadDaysApprovalMPD")
            ViewState("OwnerGroup") = drRow("OwnerGroup")
            ViewState("IsTemplatedPromo") = (drRow("TemplatedPromo") = 1)

            ViewState("sApprovalCutoffBU") = IIf(drRow.IsNull("sApprovalCutoffBU"), "", drRow("sApprovalCutoffBU"))
            ViewState("nApprovalCutoffMinutes") = IIf(drRow.IsNull("nApprovalCutoffMinutes"), "", drRow("nApprovalCutoffMinutes"))


            lResult = True
        End If

        Return lResult

    End Function

    Protected Sub lnkApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkApprove.Click

        ' SBU promotions will initially approved by VP-MPD
        If lblStatus.Text = "For Initial Approval" Then

            clsSession.Message = "Are you sure you want to approve this promotional request?"
            clsSession.Icon = "inquiry"
            lblPopTitle.Value = "Promotions"
            ViewState("process") = "allow_rush"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','','');</script>")    ' calls cmdDelete code

            Exit Sub

        End If

        Dim isMPDEscalation As Boolean = False

        Dim sMessageString As String = ""
        isMPDEscalation = ViewState("isMPDEscalate")

        If DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodFrom"))) < 1 Then

            sMessageString = "Unable to approve and submit promo request.<br />" & _
                             "Date of promo effectivity is already ongoing.<br /><br />"

        ElseIf ViewState("WorkFlowCode") <> "10" And (DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodFrom"))) < ViewState("NumLeadDays")) Then

            sMessageString = "Request must be " & ViewState("NumLeadDays") & " calendar day(s) before the start of promo."
        ElseIf (DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodFrom"))) <= ViewState("NumLeadDays")) And (Now.TimeOfDay.TotalMinutes > ViewState("nApprovalCutoffMinutes") And ViewState("nApprovalCutoffMinutes") <> "0") Then  ' past 2pm *11AM
            'NBS:20200616
            'ElseIf (DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodFrom"))) = 1) And (Now.TimeOfDay.TotalMinutes > (14 * 60)) Then  ' past 2pm          
            sMessageString = "Request past the " & ViewState("sApprovalCutoffBU") & " cut-off time must be at least " & ViewState("NumLeadDays") & " calendar days before the start of promo."
            isMPDEscalation = ViewState("isMPDEscalate")
        ElseIf DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodTo"))) <= 0 Then

            sMessageString = "Unable to approve and submit promo request.<br />" & _
                                "Promotion is already expired.<br /><br />"
            isMPDEscalation = ViewState("isMPDEscalate")
        End If



        If sMessageString = "" Then

            clsSession.Message = "Are you sure you want to approve this promotional request?"
            clsSession.Icon = "inquiry"
            lblPopTitle.Value = "Promotions"
            ViewState("process") = "approve"

            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('150','','');</script>")    ' calls cmdDelete code
        Else

            If isMPDEscalation = True Then
                ViewState("process") = "MPDEscalation"
                lblPopTitle.Value = "Error"
                clsSession.Icon = "error"
                clsSession.Message = sMessageString
            Else

                ViewState("process") = "do_nothing"
                lblPopTitle.Value = "Error"
                clsSession.Icon = "error"
                clsSession.Message = sMessageString
            End If
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('150','','');</script>")
        End If


        'If DateDiff(DateInterval.Day, Today(), ViewState("PeriodFrom")) < 10 Then

        '    ' ***********************************************************************************************************
        '    '   ---- disallow late requests (below nRushLeadDays days lead time) ----
        '    ' ***********************************************************************************************************
        '    Dim nRushLeadDays As Integer = 3
        '    Dim nExtraDay As Integer = 0

        '    If Now.Hour > 17 Then       ' add a day if it is pass 5pm already
        '        nExtraDay = 1
        '    End If

        '    If DateDiff(DateInterval.Day, Today(), ViewState("PeriodFrom")) < (nRushLeadDays + nExtraDay) Then

        '        ViewState("process") = "do_nothing"
        '        lblPopTitle.Value = "Error"
        '        clsSession.Icon = "error"
        '        clsSession.Message = "Unable to approve and submit promo request.<br /><br />" & _
        '                             "Date of promo effectivity does not comply with the minimum " & nRushLeadDays & "-day processing period.<br /><br />"

        '        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','','');</script>")     ' calls cmdDelete code
        '    Else

        '        ' ***********************************************************************************************************
        '        '   ---- code for special requests ----
        '        ' ***********************************************************************************************************

        '        lblPopTitle.Value = "Warning"
        '        clsSession.Icon = "warning"

        '        Select Case SystemUser.UserLevel

        '            Case SystemUser.UserRoles.RequestApprover

        '                If IsSmacDealsPromo() Then
        '                    ViewState("process") = "approve"

        '                    clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br /><br />" & _
        '                                         "<b>Request will be forwarded to the VP-MPD for pre-approval.</b><br /><br />" & _
        '                                         "Do you still wish to continue?<br />"
        '                Else
        '                    ViewState("process") = "approverequest"

        '                    clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br /><br />" & _
        '                                         "<b>Request will be forwarded to the VP-MPD for pre-approval.</b><br /><br />" & _
        '                                         "Do you still wish to continue?<br />"
        '                End If



        '            Case SystemUser.UserRoles.SMACapprover

        '                ViewState("process") = "approve"

        '                clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br /><br />" & _
        '                                     "<b>This will be forwarded to the VP-MPD upon submission of request with DTI permit.</b><br /><br />" & _
        '                                     "Do you still wish to continue?<br />"
        '            Case Else

        '                ViewState("process") = "approverequest"

        '                clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br /><br />" & _
        '                                     "<b>Request will be forwarded to the VP-MPD for pre-approval.</b><br /><br />" & _
        '                                     "Do you still wish to continue?<br />"

        '        End Select

        '        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','','');</script>")     ' calls cmdDelete code

        '    End If

        'Else
        'End If

    End Sub

    Protected Sub lnkSubmitRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSubmitRequest.Click

        ' check if attachment has been provided
        If lblFiles.Text = "" Then
            ViewState("process") = "do_nothing"
            lblPopTitle.Value = "Error"
            clsSession.Icon = "error"
            clsSession.Message = "DTI permit attachment is required for this promotion."

            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('180','','');</script>")     ' calls cmdDelete code
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, Today(), ViewState("PeriodFrom")) < 10 Then

            ' ***********************************************************************************************************
            '   ---- disallow late requests (below nRushLeadDays days lead time) ----
            ' ***********************************************************************************************************
            Dim nRushLeadDays As Integer = 3

            If Now.Hour > 17 Then       ' add a day if it is pass 5pm already
                nRushLeadDays += 1
            End If

            If DateDiff(DateInterval.Day, Today(), ViewState("PeriodFrom")) < nRushLeadDays Then

                ViewState("process") = "do_nothing"
                lblPopTitle.Value = "Error"
                clsSession.Icon = "error"
                clsSession.Message = "Unable to submit promo request.<br /><br />" & _
                                     "Date of promo effectivity does not comply with the minimum " & nRushLeadDays & "-day processing period.<br /><br />"

                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','','');</script>")     ' calls cmdDelete code
            Else

                ' ***********************************************************************************************************
                '   ---- code for special requests ----
                ' ***********************************************************************************************************

                lblPopTitle.Value = "Warning"
                clsSession.Icon = "warning"
                ViewState("process") = "approverequest"

                clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br /><br />" & _
                                     "<b>This will be forwarded to the VP-MPD for pre-approval.</b><br /><br />" & _
                                     "Do you still wish to continue?<br />"

                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','','');</script>")     ' calls cmdDelete code

            End If
        Else

            ' confirm submission
            clsSession.Message = "Submit this promotional request to MPD?"
            clsSession.Icon = "inquiry"
            lblPopTitle.Value = "Promotions"
            ViewState("process") = "submit_request"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','','');</script>")    ' calls cmdDelete code

        End If

    End Sub

    Private Sub ApprovePromoRequest(ByVal DocStatus As String, Optional ByVal isChangeDate As Boolean = False)

        ' get user information to be posted in the document
        sqldsData.SelectCommand = "SELECT SignName, SignPosition FROM Users WHERE UserID = " & SystemUser.UserID

        Dim dvUsers As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
        Dim drUser As DataRow = dvUsers.Table.Rows(0)

        ' mark request as approved and update status
        Select Case SystemUser.UserLevel
            Case SystemUser.UserRoles.RequestReviewer
                sqldsData.UpdateCommand = "UPDATE PromoRequests SET ReviewedBy = '" & drUser("SignName") & "', ReviewerPos = '" & drUser("SignPosition") & "', ReviewDate = GETDATE(), Status = '" & DocStatus & "' WHERE RequestID = 0" & ViewState("CurrRequestID")

            Case SystemUser.UserRoles.Analyst
                sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = '" & DocStatus & "' WHERE RequestID = 0" & ViewState("CurrRequestID")

            Case Else
                sqldsData.UpdateCommand = "UPDATE PromoRequests SET ApprovedBy = '" & drUser("SignName") & "', ApproverPos = '" & drUser("SignPosition") & "', ApproveDate = GETDATE(), Status = '" & DocStatus & "' WHERE RequestID = 0" & ViewState("CurrRequestID")

        End Select

        sqldsData.Update()

        'audit trail
        If DocStatus = "Special Request" Then
            clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("CurrRequestID"), "Allowed promotional request that does not comply with the required lead time.", SystemUser.UserName, "Promotion Transaction")
        Else
            clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("CurrRequestID"), "Approved promotional request.", SystemUser.UserName, "Promotion Transaction")
            If isChangeDate = True Then

                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("CurrRequestID"), "Promo period has been changed.", SystemUser.UserName, "Promotion Transaction")
                clsSession.Notification = Nothing 'allow approver to edit date period if late requests

            End If
        End If

    End Sub


    'Private Sub SubmitPromoRequest()

    '    ' get user information to be posted in the document
    '    sqldsData.SelectCommand = "SELECT SignName, SignPosition FROM Users WHERE UserID = " & SystemUser.UserID

    '    Dim dvUsers As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
    '    Dim drUser As DataRow = dvUsers.Table.Rows(0)

    '    ' mark request as submitted
    '    sqldsData.UpdateCommand = "UPDATE PromoRequests SET ApprovedBy = '" & drUser("SignName") & "', ApproverPos = '" & drUser("SignPosition") & "', ApproveDate = GETDATE(), Status = 'For MPD Processing' WHERE RequestID = " & clsSession.CurrRequestID
    '    sqldsData.Update()

    '    clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), clsSession.CurrRequestID, "Submitted promo request to MPD for processing.", SystemUser.UserName, "Promotion Transaction")

    'End Sub

    Protected Sub lnkCreateMemoDraft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCreateMemoDraft.Click

        If lnkCreateMemoDraft.Text = "Create Memo Draft" Then
            Response.Redirect("PromoMemoEntry.aspx?v1=" & clsEncryptDecrypt.EncryptText(clsSession.CurrMemoID.ToString, SystemUser.UserName) & "&v2=" & clsEncryptDecrypt.EncryptText(ViewState("CurrRequestID"), SystemUser.UserName))
        Else
            ' prompt submit for approval (lnkCreateMemoDraft.Text = "Submit Memo")
            SysMessageBox("Submit this memo draft for approval?", "Confirm Action", "auto_submit", "inquiry")
        End If

    End Sub

    Private Sub SysMessageBox(ByVal sMessageStr As String, ByVal sMsgBoxTitle As String, ByVal sProcessTag As String, ByVal sIconTag As String)

        clsSession.Icon = sIconTag
        clsSession.Message = sMessageStr
        lblPopTitle.Value = sMsgBoxTitle
        ViewState("process") = sProcessTag

        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','','');</script>")  ' calls cmdDelete code

    End Sub

    Protected Sub lnkEditDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditDoc.Click

        Server.Transfer("PromoRequestPreview.aspx?DocAttrib=Active")

        ' allow user to edit
        'If SystemUser.UserGroupType = "SACI" Or _
        '        SystemUser.UserGroupType = "SBU" Then
        '    Server.Transfer("PromoRequestPreview.aspx?DocAttrib=Active")
        'End If

    End Sub

    Protected Sub lnkClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkClose.Click

        RedirectToListPage()

    End Sub

    Private Sub RedirectToListPage()

        If SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover Then
            Response.Redirect("PromoMemoList.aspx")
        Else
            Response.Redirect("PromoRequestList.aspx")
        End If

    End Sub

    Protected Sub lnkCancelRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCancelRequest.Click

        'prompt user for reason
        lblPopTitle.Value = "Disapprove Request"
        clsSession.Message = "<b>Reason why this document is being returned:</b><br>"
        clsSession.Icon = "inputinquiry"
        ViewState("process") = "cancelrequest"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openinputbox('','');</script>")  ' calls cmdPopUpOK code

    End Sub


    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        ' button function for Cancel/Return and for Approval of late request

        If clsSession.DeleteStatus = "yes" Then

            Select Case ViewState("process")
                Case "cancelrequest"

                    'error while trying to input single quotation. reported by Ms. Eda(MPD)
                    'cancel request
                    sqldsData.UpdateCommand = "UPDATE PromoRequests SET Remarks = '" & Server.HtmlEncode(hidinputbox.Value.Replace("'", "''")) & "', Status = 'Returned' WHERE RequestID = " & ViewState("CurrRequestID")
                    sqldsData.Update()

                    'erroneous return to sender reported by MPD(MPD Analyst)
                    'cancel memo
                    sqldsData.UpdateCommand = "UPDATE Memos SET Status = 'Deleted' WHERE Status = 'Returned' AND RequestID = " & ViewState("CurrRequestID")
                    sqldsData.Update()

                    'audit trail
                    clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("CurrRequestID"), "Promo request returned to promo requestor. " & hidinputbox.Value.Trim, SystemUser.UserName, "Promotion Transaction")

                    Server.Transfer("PromoRequestList.aspx")


                Case "approve"

                    'approve request by Request Approver
                    ApprovePromoRequest("Special Request")

                    Server.Transfer("PromoRequestList.aspx")

                    'Case "submit_request"
                    'SubmitPromoRequest()

                Case Else

                    ' do nothing

            End Select

        End If
    End Sub

    Protected Sub lnkPrintReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintReq.Click
        Server.Transfer("ViewRequest.aspx?Report=True")
    End Sub

    Protected Sub lnkDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDelete.Click

        clsSession.Icon = "inquiry"
        clsSession.Message = "Do you wish to delete this request draft?"
        lblPopTitle.Value = "Delete"
        ViewState("process") = "Delete"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','','');</script>")    ' calls cmdDelete code

    End Sub

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click

        clsPromo.CreateZipFile(clsPromo.pathAttachment & ViewState("CurrRequestID").ToString, 1)

        ' what if we call Server.Transfer instead of Response.Redirect?
        Response.Redirect("PromoRequest.aspx?DownLoad=" & ViewState("CurrRequestID").ToString & ".zip" & "&Path=" & clsPromo.pathAttachment & ViewState("CurrRequestID").ToString)

    End Sub

    Private Function IsSmacDealsPromo() As Boolean

        sqldsData.SelectCommand = "SELECT P.PromoTypeID " & _
                                  "FROM Promotions AS P " & _
                                  "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                                  "WHERE T.LayoutID = 30 " & _
                                  "AND P.RequestID = 0" & ViewState("CurrRequestID")

        Dim dvPromos As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        IsSmacDealsPromo = (dvPromos.Table.Rows.Count() > 0)

        'dvPromos.Dispose()

    End Function

    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        If clsSession.DeleteStatus = "yes" Then


            Select Case ViewState("process").ToString()
                Case "approverequest"
                    'approve request    
                    ApprovePromoRequest("Special Request")
                    Response.Redirect("PromoRequestList.aspx")

                Case "approve"              ' standard approval of request

                    Select Case SystemUser.UserLevel

                        Case SystemUser.UserRoles.ExecutiveApprover

                            ' check if promotype is SMACdeals
                            If IsSmacDealsPromo() Then
                                ApprovePromoRequest("For MCI Approval")
                            Else
                                ApprovePromoRequest("For MPD Processing")
                            End If

                        Case SystemUser.UserRoles.SMACapprover

                            ApprovePromoRequest("For DTI Application")

                        Case SystemUser.UserRoles.RequestReviewer

                            ' check if promotype is SMACdeals
                            If IsSmacDealsPromo() Then
                                ApprovePromoRequest("For MCI Approval")
                            Else

                                ' store PromoRequest information to viewstate
                                ' GetPromoRequestInfo(clsSession.CurrRequestID)

                                'If ViewState("WorkFlowCode") = "10" Then
                                ' shortened approval flow - request redirected to BU head
                                ' *create enum for workflow code
                                '    ApprovePromoRequest("For MBU Approval")

                                If GetUserGroupType() = "SBU" Then  'SystemUser.UserGroupType

                                    If SystemUser.UserBizUnit = "BCR" Then
                                        ' direct to MPD
                                        ApprovePromoRequest("For MPD Processing")
                                    Else
                                        ' redirect to VP-MPD
                                        ApprovePromoRequest("For BU Head Approval")
                                    End If

                                Else
                                    ' standard promotions
                                    'ApprovePromoRequest("For MPD Processing")
                                    ApprovePromoRequest("For MBU Approval")

                                End If
                            End If

                        Case SystemUser.UserRoles.RequestApprover

                            ' store PromoRequest information to viewstate
                            ' GetPromoRequestInfo(clsSession.CurrRequestID)

                            Select Case ViewState("WorkFlowCode")
                                Case "10"

                                    ' shortened approval flow

                                    ' checking for lead days

                                    If ViewState("NumLeadDays") > 1 And _
                                        (DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodFrom"))) < ViewState("NumLeadDays")) Then


                                        ' follow current proocess flow if less than lead days
                                        ApprovePromoRequest("For MPD Processing")

                                    Else


                                        ApprovePromoRequest("Approved")

                                        ' create and approve memo -- shortened approval workflow
                                        If CreateDefaultMemo(ViewState("CurrRequestID"), "Approved") Then
                                            'auto-approve: create approved memo

                                            'TODO:NOELS: revert PromoRequest status if an error was encountered
                                            'TODO:NOELS: *create enum for workflow code

                                        Else
                                            ' TODO: post error message
                                        End If



                                    End If



                                Case "20" 're-enable for P3 request 
                                    ' shortened approval flow with lead-days checking
                                    If (DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodFrom"))) >= ViewState("NumLeadDays")) Then

                                        'auto-approve: create approved memo
                                        If CreateDefaultMemo(ViewState("CurrRequestID"), "Approved") Then

                                            ApprovePromoRequest("Approved")

                                            'TODO:NOELS: revert PromoRequest status if an error was encountered
                                            'TODO:NOELS: *create enum for workflow code

                                        Else
                                            ' TODO: post error message
                                        End If

                                    Else
                                        'send to MPD for usual flow
                                        ApprovePromoRequest("For MPD Processing")
                                    End If
                                Case "30"
                                    If (DateDiff(DateInterval.Day, Today(), CDate(ViewState("PeriodFrom"))) >= ViewState("NumLeadDays")) Then

                                        '10/04/2025 Auto Assign to MPD for non P3 Templates
                                        ApprovePromoRequest("For MPD Processing")

                                    Else

                                    End If
                                Case "0" 'WorkflowCode 0 is for MBU Approval Only
                                    ApprovePromoRequest("Approved")

                                    ' create and approve memo -- shortened approval workflow
                                    If CreateDefaultMemo(ViewState("CurrRequestID"), "Approved") Then
                                        'auto-approve: create approved memo

                                        'TODO:NOELS: revert PromoRequest status if an error was encountered
                                        'TODO:NOELS: *create enum for workflow code

                                    Else
                                        ' TODO: post error message
                                    End If
                                Case Else

                                    ' standard promotions
                                    ApprovePromoRequest("For MPD Processing")

                            End Select

                        Case Else

                            ' do nothing / alert for error

                    End Select

                    Response.Redirect("PromoRequestList.aspx")

                Case "submit_request"

                    If SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover Then
                        ApprovePromoRequest("For MPD Processing")
                        Response.Redirect("PromoRequestList.aspx")
                    End If

                Case "Delete"       ' delete promo request transaction

                    With sqldsData
                        ' delete promotions details
                        .DeleteCommand = "DELETE FROM PromoDetails WHERE PromoID IN (SELECT PromoID FROM Promotions WHERE RequestID = 0" & Session("CurrRequestID") & ")"
                        .Delete()
                        ' delete branches for promotions
                        .DeleteCommand = "DELETE FROM PromoBranch WHERE PromoID IN (SELECT PromoID FROM Promotions WHERE RequestID = 0" & Session("CurrRequestID") & ")"
                        .Delete()
                        ' delete promotions UPCs if any
                        .DeleteCommand = "DELETE FROM PromoUPCs WHERE PromoID IN (SELECT PromoID FROM Promotions WHERE RequestID = 0" & Session("CurrRequestID") & ")"
                        .Delete()
                        'delete premium UPC
                        .DeleteCommand = "DELETE FROM PromoPremiumUPCs WHERE PromoID IN (SELECT PromoID FROM Promotions WHERE RequestID = 0" & Session("CurrRequestID") & ")"
                        .Delete()
                        ' delete promotions record
                        .DeleteCommand = "DELETE FROM Promotions WHERE RequestID = 0" & ViewState("CurrRequestID")
                        .Delete()
                        ' delete promo request
                        .DeleteCommand = "DELETE FROM PromoRequests WHERE RequestID = 0" & ViewState("CurrRequestID")
                        .Delete()
                        Response.Redirect("PromoRequestList.aspx")
                    End With

                Case "allow_rush"           ' for VP-MPD use, allow non-compliant promo request

                    ' normal flow: templated promotions
                    ApprovePromoRequest("For MPD Processing")
                    Response.Redirect("PromoMemoList.aspx")

                Case "allow_rushWCD"       ' for VP-MPD use, allow non-compliant promo request
                    'allow approver to edit date period if late requests
                    ApprovePromoRequest("For MPD Processing", True)
                    Response.Redirect("PromoMemoList.aspx")

                Case "auto_submit"          ' auto create memo draft

                    ApprovePromoRequest("For Review")

                    If Not CreateDefaultMemo(ViewState("CurrRequestID"), "For Review") Then
                        'TODO:: revert PromoRequest status if an error was encountered
                        'ApprovePromoRequest("For MBU Approval")
                    End If

                    ' back to list
                    RedirectToListPage()
            End Select

        End If

        '20250905 rbs7281
        If clsSession.DeleteStatus = "cancel" Then
            Select Case ViewState("process").ToString()
                Case "MPDEscalation"
                    '20250905: Malae D. Vergara (MPD) with confirmation of the pass cutofftime, to proceed to MPD Processing / dev rbs7281
                    sqldsData.UpdateCommand = "UPDATE PromoRequests SET Remarks = '" & Server.HtmlEncode(hidinputbox.Value.Replace("'", "''")) & "', Status = 'For MPD Processing' WHERE RequestID = " & ViewState("CurrRequestID")
                    sqldsData.Update()
                    clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("CurrRequestID"), "Since the BU reviewer/approver has exceeded the cutoff time/lead days, the approval will now be escalated to MPD." & hidinputbox.Value.Trim, SystemUser.UserName, "Promotion Transaction")
                    Server.Transfer("PromoRequestList.aspx")
            End Select
        End If

    End Sub

    Protected Sub lnkAllowRush_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAllowRush.Click

        clsSession.Icon = "inquiry"
        clsSession.Message = "Allow this late promo request?"
        lblPopTitle.Value = "Confirm Action"
        ViewState("process") = "allow_rush"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','','');</script>")    ' calls cmdDelete code

    End Sub

    Protected Sub lnkHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHistory.Click
        ClientScript.RegisterStartupScript(Me.GetType, "keykey", "<script>OpenAuditLogs('324','800','" & ViewState("CurrRequestID").ToString & "','02');</script>")
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String

        strFiles = ""
        strDir = clsPromo.pathAttachment & ViewState("CurrRequestID").ToString
        'Dim di As New System.IO.DirectoryInfo(strDir)
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href='PromoEntry.aspx?FileName=" & file.Name.ToString & "'>" & file.Name.ToString & "</a> "
            Next
        Else
            If Directory.Exists(strDir) Then
                Dim s As String
                For Each s In System.IO.Directory.GetFiles(strDir)
                    System.IO.File.Delete(s)
                Next s
                Directory.Delete(strDir)
                'Kill(strDir)
                'di.Delete(True)
            End If
        End If

        If Len(strFiles) <> 0 Then
            'lblFiles.Text = Left(strFiles, Len(strFiles) - 1)
        Else
            lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr

    End Sub

    Protected Sub lnkattachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkattachment.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openAttachment();</script>")     ' calls btnDownload
    End Sub

    Protected Sub lnkViewBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewBranches.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>OpenViewBranches('','');</script>")
    End Sub

    'allow approver to edit date period if late requests
    Protected Sub lnkAllowRushWCD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAllowRushWCD.Click

        Dim strURLAdd As String = "?id=" & clsEncryptDecrypt.EncryptText(lblRequestID.Text, SystemUser.EncryptKey.ToString)
        blistErrorMsg.Items.Clear()

        ' display error message
        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        clsSession.Icon = "inquiry"
        clsSession.Message = "Allow this late promo request?"
        'clsSession.Notification = "Please edit promotion start/end date:"
        lblPopTitle.Value = "Confirm Action"
        ViewState("process") = "allow_rushWCD"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('350','800','" & strURLAdd.ToString & "');</script>")    ' calls cmdDelete code

    End Sub

    Protected Sub lnkDetailsUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDetailsUPC.Click
        ClientScript.RegisterStartupScript(Me.GetType, "UPCdetails", "<script>OpenDetailsUPC('324','800','" & ViewState("CurrRequestID").ToString & "');</script>")
    End Sub

    Protected Function CreateDefaultMemo(ByVal nRequestID As Long, ByVal sNewStatus As String) As Boolean

        Dim lReturnValue As Boolean
        Dim sErrMess As String = ""
        Dim strQuery As String
        Dim sGuidelineQuery As String = ""

        If ViewState("IsTemplatedPromo") Then
            sGuidelineQuery = "R.TemplatedGuideline"
        Else
            sGuidelineQuery = "T.DefaultGuideline"
        End If

        If sNewStatus = "Approved" Then

            Dim sNewMemoNumber As String = CreateNewMemoNumber()

            strQuery = "INSERT INTO Memos " & _
                        "(MemoNumber, MemoDate, Title, Branches, PromoPeriodFrom, PromoPeriodTo, Guidelines, RequestID, PreparedBy, PreparePos, ReviewedBy, ReviewerPos, ReviewDate, ApprovedBy, ApproverPos, ApproveDate, OwnerGroup, Remarks, Status, UserID, POSFile, EmailFg, CRFLAG) " & _
                        "SELECT '" & sNewMemoNumber & "', GETDATE(), R.Title, R.Branches, R.PromoPeriodFrom, R.PromoPeriodTo, " & sGuidelineQuery & ", R.RequestID, R.RequestedBy, R.RequesterPos, R.ReviewedBy, R.ReviewerPos, R.ReviewDate, R.ApprovedBy, R.ApproverPos, GETDATE(), R.OwnerGroup, '', '" & sNewStatus & "', R.UserID, 0, 0, 'R' " & _
                        "FROM PromoRequests AS R " & _
                        "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                        "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                        "WHERE R.RequestID = 0" & nRequestID & "; " & _
                        "UPDATE Promotions SET MemoID = SCOPE_IDENTITY() " & _
                        "WHERE RequestID = 0" & nRequestID & "; "
        Else

            ' get user information
            sqldsData.SelectCommand = "SELECT SignName, SignPosition FROM Users WHERE UserID = " & SystemUser.UserID

            Dim dvUsers As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
            Dim drUser As DataRow = dvUsers.Table.Rows(0)

            strQuery = "INSERT INTO Memos " & _
                            "(MemoDate, Title, Branches, PromoPeriodFrom, PromoPeriodTo, Guidelines, RequestID, PreparedBy, PreparePos, ReviewedBy, ReviewerPos, ReviewDate, ApprovedBy, ApproverPos, OwnerGroup, Remarks, Status, UserID, POSFile, EmailFg, CRFLAG) " & _
                            "SELECT GETDATE(), R.Title, R.Branches, R.PromoPeriodFrom, R.PromoPeriodTo, " & sGuidelineQuery & ", R.RequestID, R.ReviewedBy, R.ReviewerPos, '" & drUser("SignName") & "', '" & drUser("SignPosition") & "', GETDATE(),  '', '', R.OwnerGroup, '', '" & sNewStatus & "', " & SystemUser.UserID & ", 0, 0, 'R' " & _
                            "FROM PromoRequests AS R " & _
                            "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                            "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                            "WHERE R.RequestID = 0" & nRequestID & "; " & _
                            "UPDATE Promotions SET MemoID = SCOPE_IDENTITY() " & _
                            "WHERE RequestID = 0" & nRequestID & "; "

            'strQuery = "INSERT INTO Memos " & _
            '            "(MemoDate, Title, Branches, PromoPeriodFrom, PromoPeriodTo, Guidelines, RequestID, PreparedBy, PreparePos, ReviewedBy, ReviewerPos, ReviewDate, ApprovedBy, ApproverPos, ApproveDate, OwnerGroup, Remarks, Status, UserID, POSFile, EmailFg, CRFLAG) " & _
            '            "SELECT GETDATE(), R.Title, R.Branches, R.PromoPeriodFrom, R.PromoPeriodTo, " & sGuidelineQuery & ", R.RequestID, R.RequestedBy, R.RequesterPos, R.ReviewedBy, R.ReviewerPos, R.ReviewDate, R.ApprovedBy, R.ApproverPos, GETDATE(), R.OwnerGroup, '', '" & sNewStatus & "', " & SystemUser.UserID & ", 0, 0, 'R' " & _
            '            "FROM PromoRequests AS R " & _
            '            "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
            '            "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
            '            "WHERE R.RequestID = 0" & nRequestID & "; " & _
            '            "UPDATE Promotions SET MemoID = SCOPE_IDENTITY() " & _
            '            "WHERE RequestID = 0" & nRequestID & "; "

        End If

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to generate an automatic memo entry." & sErrMess)
            lReturnValue = False
        Else
            lReturnValue = True
        End If

    End Function

    Protected Function CreateNewMemoNumber() As String

        ' new format: MPD-uuuu-9999-yy
        ' uuuu = business unit where request originated
        ' 9999 = sequential number
        ' yy   = year when promo was approved

        Dim sNewMemoNum As String

        ' get last memo series for this BizUnit

        'sqldsData.SelectCommand = "SELECT Count(*) AS xMemoCount, G.BizUnit FROM Memos " & _
        '                          "LEFT JOIN UserGroups AS G ON Memos.OwnerGroup = G.GroupID " & _
        '                          "WHERE Memos.Status IN ('Approved','Archived','Cancelled') " & _
        '                          "AND G.BizUnit IN (SELECT BizUnit FROM UserGroups WHERE GroupID = 0" & ViewState("OwnerGroup") & ") " & _
        '                          "GROUP BY G.BizUnit"

        sqldsData.SelectCommand = "SELECT TOP 1 MemoNumber, LEFT(RIGHT(MemoNumber,7),4) AS LastMemoCode, G.BizUnit " & _
                                    "FROM Memos " & _
                                    "LEFT JOIN UserGroups AS G ON Memos.OwnerGroup = G.GroupID " & _
                                    "WHERE Memos.MemoNumber IS NOT NULL " & _
                                    "AND RIGHT(Memos.MemoNumber,2) = RIGHT(CAST(Year(GetDate()) AS varchar(4)),2) " & _
                                    "AND G.BizUnit IN (SELECT BizUnit FROM UserGroups WHERE GroupID = 0" & ViewState("OwnerGroup") & ") " & _
                                    "ORDER BY LastMemoCode DESC"

        Dim dvMemos As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dvMemos.Table.Rows.Count() = 0 Then

            sqldsData.SelectCommand = "SELECT BizUnit FROM UserGroups WHERE GroupID = " & ViewState("OwnerGroup")
            dvMemos = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

            If dvMemos.Table.Rows.Count > 0 Then

                Dim drMemo As DataRow = dvMemos.Table.Rows(0)
                sNewMemoNum = "MPD-" & UCase(Left(drMemo("BizUnit"), 4)) & "-0001" & Format(Now, "-yy")

            Else
                ' ToDo: Data Integrity Error... GroupID doesn't exists in UserGroups table
                ' THIS SHOULD NOT HAPPEN!!!
                sNewMemoNum = "ERROR"
            End If

        Else

            Dim drMemo As DataRow = dvMemos.Table.Rows(0)
            sNewMemoNum = "MPD-" & UCase(Left(drMemo("BizUnit"), 4)) & Format(drMemo("LastMemoCode") + 1, "-0###") & Format(Now, "-yy")

        End If

        CreateNewMemoNumber = sNewMemoNum
    End Function


    'Protected Sub linkSwipestakeMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakeMessage.Click
    '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesMessage();</script>")
    'End Sub

    Protected Sub linkSwipestakesMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesMessage.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesMessage();</script>")
    End Sub

    Protected Sub linkSwipestakesSeed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesSeed.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesSeed();</script>")
    End Sub

    Protected Sub linkBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
    End Sub

    Protected Sub btnSeedDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeedDownload.Click
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String

        strFiles = ""
        strDir = clsPromo.pathAttachment & ViewState("CurrRequestID").ToString
        'Dim di As New System.IO.DirectoryInfo(strDir)
        lblSeedFile.Text = ""


        Dim pathTemp As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = clsSession.CurrRequestID.ToString & "-Swipestakes"
        pathTemp = swipestakeSeedFolder & subFolderName

        If System.IO.Directory.Exists(pathTemp) Then
            Dim dir As New System.IO.DirectoryInfo(pathTemp)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblSeedFile.Text &= "<img src='Images/bullet green.gif' /><a href='PromoEntry.aspx?FileName=" & file.Name.ToString & "'>" & file.Name.ToString & "</a> "
            Next
        Else
            If Directory.Exists(pathTemp) Then
                Dim s As String
                For Each s In System.IO.Directory.GetFiles(pathTemp)
                    System.IO.File.Delete(s)
                Next s
                Directory.Delete(pathTemp)
                'Kill(strDir)
                'di.Delete(True)
            End If
        End If

        If Len(pathTemp) <> 0 Then
            'lblSeedFile.Text = Left(strFiles, Len(strFiles) - 1)
        Else
            lblSeedFile.Text = ""
        End If

        imgbtnSeedDownload.Visible = tr
    End Sub

    Protected Sub imgbtnSeedDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnSeedDownload.Click
        Dim pathTemp As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = clsSession.CurrRequestID.ToString & "-Swipestakes"
        pathTemp = swipestakeSeedFolder & subFolderName

        clsPromo.CreateZipFile(pathTemp, 1)

        ' what if we call Server.Transfer instead of Response.Redirect?
        Response.Redirect("PromoRequest.aspx?DownLoad=" & ViewState("CurrRequestID").ToString & ".zip" & "&Path=" & pathTemp)

    End Sub

    Protected Sub ShowMenu()

        Dim drPromo As DataRow = Nothing
        Dim strSQLcmd As String

        strSQLcmd = "SELECT * FROM Promotions " & _
                    "WHERE RequestID = " & clsSession.CurrRequestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromo) Then
            clsSession.PromoTypeID = drPromo("PromoTypeID").ToString()
        End If


        If clsSession.PromoTypeID = 290 Then
            panSwipestakesMenu.Visible = True
        ElseIf clsSession.PromoTypeID = 59 Then
            panRebateMenu.Visible = True
        End If

    End Sub

    Protected Function GetUserGroupType() As String
        Dim result As String
        Dim _UserID As Integer = SystemUser.UserID ' Default if not found
        Dim currRequestId As Integer = Convert.ToInt32(clsSession.CurrRequestID)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("USP_GetUserGroupType", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@RequestID", currRequestId)
                cmd.Parameters.AddWithValue("@UserID", _UserID)

                conn.Open()
                result = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then

                End If
            End Using
        End Using

        Return result
    End Function

    Protected Function GetPromoGuidelines(ByVal nRequestID As Long) As String

        Dim result As String
        Dim _UserID As Integer = SystemUser.UserID ' Default if not found
        Dim currRequestId As Integer = Convert.ToInt32(clsSession.CurrRequestID)
        Dim connStr As String = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("USP_GetPromoGuidelineByRequest", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@RequestID", currRequestId)

                conn.Open()
                result = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then

                End If
            End Using
        End Using

        Return result
    End Function



    Protected Sub linkRebateBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkRebateBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
    End Sub
End Class
