Imports System.Data
Imports dsPromotionsTableAdapters

Imports System.IO
Partial Class PromoMemo
    Inherits System.Web.UI.Page

    Private Sub LoadMemoInformation(ByVal nMemoID As Integer)

        Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter()
        Dim dtMemos As dsPromotions.MemosDataTable
        Dim rowMemos As dsPromotions.MemosRow

        dtMemos = taMemos.GetMemoByID(nMemoID)

        If dtMemos.Rows.Count = 0 Then
            Response.Redirect("InvalidAccess.aspx")
        Else

            rowMemos = dtMemos.Rows(0)

            With rowMemos

                lblStatus.Text = .Status.ToString()

                If .MemoNumber.ToString() = "" Then
                    lblMemoNumber.Text = "Draft #" & Format(.MemoID, "0####")
                Else
                    lblMemoNumber.Text = .MemoNumber.ToString()
                End If

                If .Status = "Approved" Then
                    lblMemoDate.Text = .ApproveDate.ToLongDateString()
                    lblRemarks.Text = ""
                Else
                    lblMemoDate.Text = .MemoDate.ToLongDateString()
                    lblRemarks.Text = Replace(Server.HtmlDecode(.Remarks.ToString()), vbCrLf.ToString(), "<br/>")
                End If

                lblPromoTitle.Text = .Title.ToString()
                lblPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .PromoPeriodTo.ToLongDateString()

                'initialize promo duration
                txtPeriodFrom.Text = Format(.PromoPeriodFrom, "MM/dd/yyyy")
                txtPeriodTo.Text = Format(.PromoPeriodTo, "MM/dd/yyyy")

                ' 20180724 - option for change of promo duration 
                If SystemUser.UserLevel = SystemUser.UserRoles.Reviewer And lblStatus.Text = "For Review" Then
                    lnkEditPromoPeriod.Visible = True
                Else
                    lnkEditPromoPeriod.Visible = False
                End If

                trChangePromoPeriod.Visible = (ViewState("IsChangeDate") = "ShowRow")

                ViewState("PromoPeriodFrom") = .PromoPeriodFrom.Date()
                ViewState("PromoPeriodTo") = .PromoPeriodTo.Date()

                lblBranches.Text = .Branches.ToString()
                litGuidelines.Text = Server.HtmlDecode(.Guidelines.ToString())

                lblPreparedBy.Text = .PreparedBy.ToString() & " / " & .PreparePos.ToString()
                lblReviewedBy.Text = .ReviewedBy.ToString() & " / " & .ReviewerPos.ToString()
                lblApprovedBy.Text = .ApprovedBy.ToString() & " / " & .ApproverPos.ToString()

                tbrowReviewedLine.Visible = (lblReviewedBy.Text <> String.Empty)
                tbrowApprovedLine.Visible = (lblApprovedBy.Text <> String.Empty)

                ViewState("RequestID") = .RequestID
                ViewState("OwnerGroup") = .OwnerGroup
                ViewState("GroupType") = .GroupType

                ' check if SMAC Deals Promotion
                '-------------------------------------------------------------
                Dim drPromo As DataRow
                Dim sqlCmd As String

                sqlCmd = "SELECT OnlineSellingStart, OnlineSellingEnd " & _
                         "FROM Promotions AS P " & _
                         "INNER JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                         "WHERE T.ProcessType = 'SMACdeals' " & _
                         "AND P.MemoID = 0" & .MemoID

                If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sqlCmd, drPromo) Then
                    trRedeemPeriod.Visible = True

                    If Not drPromo.IsNull("OnlineSellingStart") And _
                       Not drPromo.IsNull("OnlineSellingEnd") Then
                        lblRedeemPeriod.Text = CDate(drPromo("OnlineSellingStart").ToString).ToLongDateString() & _
                                                " to " & CDate(drPromo("OnlineSellingEnd").ToString).ToLongDateString()
                    End If
                Else
                    trRedeemPeriod.Visible = False
                End If
                '-------------------------------------------------------------

                ' hide promo item for SBU Marketing and BCR
                'If ViewState("GroupType") = "SBU" Or ViewState("GroupType") = "BCR" Then
                '    gridPromotions.Columns(1).Visible = False
                'End If

                'show column if Dept/SDept/Class data is available
                If gridPromotions.Rows.Count > 0 Then
                    gridPromotions.Columns(1).Visible = (gridPromotions.Rows(0).Cells(1).Text <> "&nbsp;")
                End If

                If IsTemplatedPromo(.MemoID) Then
                    lblPreparedBy_Label.Text = "Endorsed By:"
                    lblReviewedBy_Label.Text = "Reviewed By:"
                    lblApprovedBy_Label.Text = "Approved By:"
                Else
                    lblPreparedBy_Label.Text = "Prepared By:"
                    lblReviewedBy_Label.Text = "Reviewed By:"
                    lblApprovedBy_Label.Text = "Approved By:"
                End If


                ' Memo Approval Cutoff and Lead days Validator
                '-------------------------------------------------------------
                Dim drApprovalCutoff As DataRow
                Dim sqlCmd2 As String

                sqlCmd2 = "SELECT " & _
                          "  Coalesce(T.NumLeadDaysApprovalMPD, T.NumLeadDays) AS sNumLeadDays, " & _
                          "  Coalesce(T.ApprovalCutoffMPD, '11:00') AS sApprovalCutoffMPD, " & _
                          "  CAST(LEFT(Coalesce(T.ApprovalCutoffMPD, '11:00'), 2) AS INT) * 60 + " & _
                          "  CAST(RIGHT(Coalesce(T.ApprovalCutoffMPD, '11:00'), 2) AS INT) AS ApprovalCutoffMinutes " & _
                          "FROM Promotions AS P " & _
                          "INNER JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                          "WHERE P.MemoID = " & .MemoID


                If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sqlCmd2, drApprovalCutoff) Then

                    ViewState("NumLeadDaysApprovalMPD") = drApprovalCutoff("sNumLeadDays")
                    ViewState("ApprovalCutoffMPD") = drApprovalCutoff("sApprovalCutoffMPD")
                    ViewState("ApprovalCutoffMinutes") = drApprovalCutoff("ApprovalCutoffMinutes")

                Else
                    'do nothing
                End If
                '-------------------------------------------------------------

            End With
        End If

        clsSession.CurrRequestID = ViewState("RequestID")
        ShowMenu()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '::ToDo:: parameter and security checking

        If SystemUser.UserID = 0 Or SystemUser.UserLevel > SystemUser.UserRoles.Analyst Then Response.Redirect("InvalidAccess.aspx")

        If (Request("MemoID") = Nothing) And (clsSession.CurrMemoID = 0) Then Response.Redirect("InvalidAccess.aspx")

        If Request("MemoID") <> Nothing Then
            clsSession.CurrMemoID = Request("MemoID")
        End If

        If Not IsPostBack() Then

            LoadMemoInformation(CInt(clsSession.CurrMemoID.ToString))

            lnkEditMemo.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.Analyst And InStr("Approved; For Final Approval; For Review", lblStatus.Text) = 0)


            lnkApproveMemo.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover And lblStatus.Text = "For Final Approval")
            lnkForApproval.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.Reviewer And lblStatus.Text = "For Review")

            If SystemUser.UserLevel = SystemUser.UserRoles.Reviewer Then

                'CHECK_HERE!!!
                'lnkApproveMemo.Visible = True
                'lnkForApproval.Visible = False

                'ORIGINAL CODE
                ' SBU and Templated promotions will be approved by Madam RCL
                'If ViewState("GroupType") = "SBU" Or IsTemplatedPromo(CInt(clsSession.CurrMemoID.ToString)) Then
                If IsTemplatedPromo(CInt(clsSession.CurrMemoID.ToString)) And InStr("Approved", lblStatus.Text) = 0 Then
                    lnkApproveMemo.Visible = True
                    lnkForApproval.Visible = False
                ElseIf Not IsTemplatedPromo(CInt(clsSession.CurrMemoID.ToString)) And InStr("For Final Approval", lblStatus.Text) = 0 Then
                    lnkForApproval.Visible = True
                ElseIf InStr("Approved", lblStatus.Text) = 1 Then
                    lnkApproveMemo.Visible = False
                    lnkForApproval.Visible = False
                End If

            Else
                lnkForApproval.Visible = False
            End If

            lnkReturn.Visible = (lnkForApproval.Visible Or lnkApproveMemo.Visible)

            ' override properties if start of promo is pass due
            'If (DateDiff(DateInterval.Day, Today(), ViewState("PromoPeriodFrom")) < 0) And (lblStatus.Text <> "Approved") Then
            '    lnkApproveMemo.Visible = False
            '    lnkForApproval.Visible = False

            '    lblStatus.Text &= " - Expired"
            '    lblStatus.ForeColor = Drawing.Color.Red
            'End If

        End If

        If Request("Filename") <> Nothing Then
            Dim fname As String
            fname = clsPromo.pathAttachment & ViewState("RequestID") & "\" & Request("Filename")
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

        ShowAttachments()
    End Sub

    ' consolidate business rule functions in a class
    Private Function IsTemplatedPromo(ByVal nMemoID As Long) As Boolean

        Dim strQuery As String
        Dim drPromoType As DataRow = Nothing
        Dim bResult As Boolean = False

        'strQuery = "SELECT TOP 1 P.PromoID, T.PromoTypeID, T.IsTemplated " & _
        '    "FROM Promotions AS P " & _
        '    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
        '    "WHERE P.MemoID = 0" & nMemoID & " " & _
        '    "ORDER BY P.PromoID"

        'CHECK_HERE! Remove hardcoding for Owner GroupID 99 (3-Day Sale)
        'temp fix: bypass promos requested prior 08/01/2018
        strQuery = "SELECT TOP 1 P.MemoID, R.RequestDate, P.PromoID, T.PromoTypeID, T.IsTemplated " & _
                    "FROM PromoRequests AS R " & _
                    "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                    "WHERE P.MemoID = 0" & nMemoID & " " & _
                    "AND DATEDIFF(d, '2018-08-01', R.RequestDate) >= 0 " & _
                    "AND R.OwnerGroup <> 99 " & _
                    "ORDER BY P.PromoID"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drPromoType) Then
            bResult = (drPromoType("IsTemplated") = 1)
        Else
            ' error
        End If

        Return bResult

    End Function

    Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

        Const nDescCol As Integer = 2
        Const nItemCol As Integer = 1
        Static rowPrevious As GridViewRow
        Static rowPrevItem As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(nDescCol).Text = Server.HtmlDecode(e.Row.Cells(nDescCol).Text)
        End If

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

    Protected Sub lnkDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDone.Click

        Response.Redirect("PromoMemoListMain.aspx")

    End Sub

    Protected Sub lnkEditMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditMemo.Click

        'flag to return to this module on close

        ' Added 07272012@smretailinc for create memo request string
        ' v1 = memoid, v2=requestid

        Response.Redirect("PromoMemoEntry.aspx?xmode=1&v1=" & clsEncryptDecrypt.EncryptText(clsSession.CurrMemoID.ToString, SystemUser.UserName) & "&v2=" & clsEncryptDecrypt.EncryptText(ViewState("RequestID").ToString, SystemUser.UserName))

    End Sub

    Protected Sub lnkPrintMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintMemo.Click

        ' ::ToDo:: flag to return to this module on close
        clsSession.FlagForPOSDisp = False

        Response.Redirect("ViewMemo.aspx?xmode=1&MemoID=" & clsSession.CurrMemoID)

        Dim s As New StringBuilder()

    End Sub

    Protected Sub lnkReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReturn.Click

        lblPopTitle.Value = "Return for Revision"
        clsSession.Message = "<b>Reason why this promotions is being returned:</b><br>"
        clsSession.Icon = "inputinquiry"

        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openinputbox('','');</script>")

    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If clsSession.DeleteStatus = "yes" Then

            ' replace single quotes to avoid error
            Dim sRemarks As String = Server.HtmlEncode(Left(Replace(hidinputbox.Value, "'", "''") & "<br/><br/>- " & SystemUser.UserLogName, 500))

            ' determine to whom this doc will be returned
            If hidRadioButton.Value = "MPD Analyst" Then

                ' mark Memo document as Returned
                sqldsData.UpdateCommand = "UPDATE Memos SET Status = 'Returned', Remarks = '" & sRemarks & "', ReviewedBy = '" & SystemUser.UserSignName & "', ReviewerPos = '" & SystemUser.UserSignPosition & "', ReviewDate = GETDATE() WHERE MemoID = " & clsSession.CurrMemoID
                sqldsData.Update()

                ' update promo request counterpart's status
                sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'For MPD Processing' WHERE RequestID = " & CInt(ViewState("RequestID"))
                sqldsData.Update()

                ' ===== audit trail =====
                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("RequestID"), "Promotional Memo returned for revision to MPD Analysts.", SystemUser.UserName, "Promotion Transaction")

            Else

                ' mark Request document as Returned
                sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'Returned', Remarks = '" & sRemarks & "' WHERE RequestID = " & CInt(ViewState("RequestID"))
                sqldsData.Update()

                sqldsData.UpdateCommand = "UPDATE Promotions SET MemoID = NULL WHERE MemoID = 0" & clsSession.CurrMemoID
                sqldsData.Update()

                ' mark Memo draft for this Request as deleted
                sqldsData.UpdateCommand = "UPDATE Memos SET RequestID = NULL, Status = 'Deleted' WHERE MemoID = " & clsSession.CurrMemoID
                sqldsData.Update()

                ' ===== audit trail =====
                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("RequestID"), "Promotional Memo returned for revision to Promo Requestor.", SystemUser.UserName, "Promotion Transaction")
            End If

            Response.Redirect("PromoMemoList.aspx")
            
        End If

        GC.Collect()

    End Sub

    Protected Sub lnkForApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkForApproval.Click


        If trChangePromoPeriod.Visible Then
            If ValidateChangeDate() Then
                SubmitForFinalApproval()
            End If
        Else
            SubmitForFinalApproval()
        End If


    End Sub

    Private Sub SubmitForFinalApproval()

        ' prompt for confirmation
        clsSession.Message = "Forward this memo for final approval?"
        clsSession.Icon = "inquiry"
        lblPopTitle.Value = "Confirm Action"
        ViewState("process") = "reviewed_memo"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Private Sub MessageBox(ByVal sMessage As String, ByVal sProcess As String, ByVal sTitle As String, ByVal sIcon As String)

        ' prompt for confirmation
        clsSession.Message = sMessage
        clsSession.Icon = sIcon
        lblPopTitle.Value = sTitle
        ViewState("process") = sProcess
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Private Function ValidateChangeDate() As Boolean

        Dim bResult As Boolean = False

        ' check dates
        If Not IsDate(txtPeriodFrom.Text) Or _
            Not IsDate(txtPeriodTo.Text) Then

            MessageBox("Invalid promo start/end date format.", "do_nothing", "Invalid Date", "error")

        ElseIf CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then
            MessageBox("Invalid promo start/end date range.", "do_nothing", "Invalid Date", "error")

        ElseIf DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < CInt(ViewState("NumLeadDaysApprovalMPD")) Then
            MessageBox("Start of promotion must be set at least a day in advance.", "do_nothing", "Invalid Date", "error")


        ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = CInt(ViewState("NumLeadDaysApprovalMPD"))) And (Now.TimeOfDay.TotalMinutes > CInt(ViewState("ApprovalCutoffMinutes"))) Then
            ' NBS:20200616
            'ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (14 * 60)) Then

            MessageBox("Start of promotion must be set at least " & ViewState("NumLeadDaysApprovalMPD") & " days in advance since it is already beyond the " & ViewState("ApprovalCutoffMPD") & " cut-off time", "do_nothing", "Invalid Date", "error")

        Else

            'MessageBox("It is already beyond the 2PM cut-off.<br />" & _
            '            "Promotion will be implemented within two days.", _
            '            "reviewed_with_change_date", "Beyond Cut-off", "inquiry")

            'SubmitForFinalApproval()

            bResult = True
        End If


        'Dim obj As Object = Nothing
        'Dim length As Integer = obj.ToString().Length ' NullReferenceException


        Return bResult

    End Function

    Protected Sub lnkApproveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkApproveMemo.Click

        If trChangePromoPeriod.Visible Then
            If ValidateChangeDate() Then
                ApproveMemoDraft()
            End If
        Else
            ApproveMemoDraft()
        End If

    End Sub

    Private Function GetChangeDateQueryString() As String

        Dim strChangeDate As String = ""

        ' do change date if option is visible
        If trChangePromoPeriod.Visible Then

            Dim strAuditDesc As String

            strAuditDesc = Format(ViewState("PromoPeriodFrom"), "MM/dd/yyyy") & "-" & _
                            Format(ViewState("PromoPeriodTo"), "MM/dd/yyyy") & " to " & _
                            txtPeriodFrom.Text & "-" & txtPeriodTo.Text

            'audit trail - change of date
            clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                      ViewState("RequestID"), _
                                      "Override promo date from " & strAuditDesc, _
                                      SystemUser.UserName, _
                                      "Promotion Transaction")

            strChangeDate = ", PromoPeriodFrom = '" & txtPeriodFrom.Text & "' " & _
               ", PromoPeriodTo = '" & txtPeriodTo.Text & "' "

        End If

        Return strChangeDate

    End Function


    Protected Sub cmdRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRedirect.Click


        If clsSession.DeleteStatus = "yes" Then

            Dim Error_ As Boolean = False

            Error_ = ValidateChangeDateEnhanced()


            If Error_ = True Then

            Select ViewState("process").ToString()

                    'Case "reviewed_with_change_date"

                    '    SubmitForFinalApproval()

                    Case "reviewed_memo"
                        Dim strAddQuery As String = GetChangeDateQueryString()

                        ' mark request as reviewed and update status
                        sqldsData.UpdateCommand = "UPDATE Memos SET Status = 'For Final Approval', ReviewedBy = '" & _
                                                    SystemUser.UserSignName & "', ReviewerPos = '" & _
                                                    SystemUser.UserSignPosition & "', ReviewDate = GETDATE() " & _
                                                    strAddQuery & _
                                                    "WHERE MemoID = " & clsSession.CurrMemoID & _
                                                    " AND Status <> 'Approved'"
                        sqldsData.Update()

                        ' update promo request counterpart's status
                        sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'For Final Approval' " & _
                                                    strAddQuery & _
                                                    "WHERE RequestID = " & CInt(ViewState("RequestID")) & _
                                                     " AND Status <> 'Approved'"
                        sqldsData.Update()

                        'audit trail
                        clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                                  ViewState("RequestID"), _
                                                  "Reviewed and forwarded Memo draft for final approval.", _
                                                  SystemUser.UserName, _
                                                  "Promotion Transaction")
                    Case "approve_memo"


                        '****************************************
                        ' create new memo number upon approval
                        '****************************************

                        Dim sNewMemoNum As String = CreateNewMemoNumber()

                        '****************************************
                        ' update tables
                        '****************************************
                        Dim strAddQuery As String = GetChangeDateQueryString()

                        ' mark memo as approved and post necessary info
                        sqldsData.UpdateCommand = "UPDATE Memos SET MemoNumber = '" & sNewMemoNum & "', " & _
                                                  "Status = 'Approved', ApprovedBy = '" & SystemUser.UserSignName & "', " & _
                                                  "ApproverPos = '" & SystemUser.UserSignPosition & "', " & _
                                                  "ApproveDate = GETDATE() " & _
                                                  strAddQuery & _
                                                  "WHERE MemoID = " & clsSession.CurrMemoID
                        sqldsData.Update()


                        ' -- EventCode generation moved to SMAC deals approver process
                        ' assign promo event code if promotion is "SMAC Deals"
                        ' CreateNewPromoEventCode(8000, 9999)

                        ' display info on guidelines
                        AppendExtraDetailsToGuidelines()

                        ' update promo request counterpart's status
                        sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'Approved' " & _
                                                    strAddQuery & _
                                                    "WHERE RequestID = " & CInt(ViewState("RequestID"))
                        sqldsData.Update()

                        ' update audit trail
                        clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                                  ViewState("RequestID"), "Promotional Memo approved.", _
                                                  SystemUser.UserName, "Promotion Transaction")
                    Case Else

                        ' do nothing -- INVALID PROCESS ENTRY

                End Select


                Response.Redirect("PromoMemoList.aspx")
            End If
        End If

    End Sub

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

    Private Sub CreateNewPromoEventCode(ByRef nSeriesStart As Integer, ByRef nSeriesEnd As Integer)

        ' check if promotion is SMAC Deals
        sqldsData.SelectCommand = "SELECT P.PromoID FROM Promotions AS P LEFT JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                                  "WHERE T.WithEventCode = 1 AND P.MemoID = 0" & clsSession.CurrMemoID

        Dim dvPromoTypes As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dvPromoTypes.Table.Rows.Count > 0 Then

            ' generate new event code
            Dim NewPromoEventCode As String

            ' get last event code for the year (check only those with memo)
            sqldsData.SelectCommand = "SELECT TOP 1 PromoEventCode, PromoID " & _
                                      "FROM Promotions AS P LEFT JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                                      "WHERE(T.WithEventCode = 1) AND P.PromoEventCode BETWEEN 0" & nSeriesStart & " AND 0" & nSeriesEnd & _
                                      "ORDER BY PromoEventCode DESC"
            '                                  " AND MemoID IN (SELECT MemoID FROM Memos WHERE Year(PromoPeriodFrom) = Year(GetDate()))" & _

            Dim dvPromo As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

            If dvPromo.Table.Rows.Count = 0 Then
                NewPromoEventCode = Format(nSeriesStart, "0###")
            Else
                ' create new event number
                Dim dr As DataRow = dvPromo.Table.Rows(0)
                NewPromoEventCode = Format(Val(dr("PromoEventCode")) + 1, "0###")
            End If

            ' stamp event number to this promotion
            sqldsData.UpdateCommand = "UPDATE Promotions SET PromoEventCode = '" & NewPromoEventCode & "' " & _
                                      "WHERE MemoID = " & clsSession.CurrMemoID
            sqldsData.Update()

        End If

    End Sub

    Protected Sub lnkHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHistory.Click
        ClientScript.RegisterStartupScript(Me.GetType, "keykey", "<script>OpenAuditLogs('324','800','" & ViewState("RequestID").ToString & "','02');</script>")
    End Sub

    Private Sub ShowAttachments()
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & ViewState("RequestID")
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""PromoMemo.aspx?FileName=" & Server.UrlEncode(file.Name.ToString()) & """>" & file.Name.ToString() & "</a> "
            Next

            If Len(strFiles) = 0 Then lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr
    End Sub
	
    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & ViewState("RequestID").ToString, 1)
        Response.Redirect("PromoRequest.aspx?DownLoad=" & ViewState("RequestID") & ".zip" & "&Path=" & clsPromo.pathAttachment & ViewState("RequestID").ToString)
    End Sub

    Protected Sub lnkViewBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewBranches.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>OpenViewBranches('','');</script>")
    End Sub

    Private Sub AppendExtraDetailsToGuidelines()

        Dim NewGuidelines As String

        sqldsData.SelectCommand = "SELECT Guidelines FROM Memos WHERE MemoID = 0" & clsSession.CurrMemoID

        Dim dvMemos As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dvMemos.Table.Rows.Count > 0 Then
            Dim drMemo As DataRow = dvMemos.Table.Rows(0)

            ' get guidelines
            NewGuidelines = Server.HtmlDecode(drMemo("Guidelines"))

            ' delete details if its already there (redundancy check)
            If InStr(NewGuidelines, "<ExtraDetails>") > 0 Then
                NewGuidelines = Left(NewGuidelines, InStr(NewGuidelines, "<ExtraDetails>") - 1)
            End If

            sqldsData.SelectCommand = "SELECT P.* FROM Promotions AS P LEFT JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                                      "WHERE T.WithEventCode = 1 AND P.MemoID = 0" & clsSession.CurrMemoID

            Dim dvPromos As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

            If dvPromos.Table.Rows.Count > 0 Then

                Dim drPromo As DataRow = dvPromos.Table.Rows(0)

                ' append details
                ' convert this into a table format!
                NewGuidelines &= "<ExtraDetails><pre style=""font-family: Verdana, Arial, sans serif;"">" & _
                                 "<b><u>SPECIFIC DETAILS</u></b> <br/>" & _
                                 "  Event Code" & StrDup(2, vbTab) & ": " & drPromo("PromoEventCode").ToString & "<br/>" & _
                                 "  Discount" & StrDup(2, vbTab) & ": " & "Php " & Format(drPromo("DiscAmount"), "#,##0.00") & "<br/>" & _
                                 "  Merchandise Group" & StrDup(1, vbTab) & ": " & drPromo("MerchGroup") & "<br/>" & _
                                 "  Business Unit" & StrDup(2, vbTab) & ": " & drPromo("BusinessUnit") & "<br/>" & _
                                 "  Company to Shoulder" & StrDup(1, vbTab) & ": " & drPromo("CompSponsorship") & "<br/>" & _
                                 "  Vendor Code" & StrDup(2, vbTab) & ": " & drPromo("VendorCode").ToString & "<br/>" & _
                                 "</pre></ExtraDetails>"
                ' -- "  Barcode" & StrDup(3, vbTab) & ": " & txtBarcode.Text & "<br/>" & _

                ' save new guidelines
                With sqldsData
                    .UpdateCommand = "UPDATE Memos SET Guidelines = @Guidelines " & _
                                     "WHERE MemoID = 0" & clsSession.CurrMemoID
                    .UpdateParameters.Add("Guidelines", Server.HtmlEncode(NewGuidelines))
                    .Update()
                End With
            End If

        End If

    End Sub

    Protected Sub lnkDetailsUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDetailsUPC.Click
        ClientScript.RegisterStartupScript(Me.GetType, "UPCdetails", "<script>OpenDetailsUPC('324','800','" & ViewState("RequestID").ToString & "');</script>")
    End Sub

    Protected Sub lnkEditPromoPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditPromoPeriod.Click

        ' show change promo period entry
        trChangePromoPeriod.Visible = True
        trPromoPeriod.Visible = False

        ViewState("IsChangeDate") = "ShowRow"

    End Sub

    Private Sub ApproveMemoDraft()

        ' prompt for confirmation
        clsSession.Message = "Do you wish to approve this memo?"
        clsSession.Icon = "inquiry"
        lblPopTitle.Value = "Confirm Approval"
        ViewState("process") = "approve_memo"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Protected Sub linkBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
    End Sub

    Protected Sub linkSwipestakesMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesMessage.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesMessage();</script>")
    End Sub

    Protected Sub ShowMenu()

        Dim drPromo As DataRow = Nothing
        Dim strSQLcmd As String

        strSQLcmd = "SELECT * FROM Promotions " & _
                    "WHERE RequestID = " & ViewState("RequestID").ToString

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromo) Then
            clsSession.PromoTypeID = drPromo("PromoTypeID").ToString()
        End If


        If clsSession.PromoTypeID = 290 Then
            panSwipestakesMenu.Visible = True
        ElseIf clsSession.PromoTypeID = 59 Then
            panRebateMenu.Visible = True
        End If

    End Sub

    Private Sub Error_()

        MessageBox("Invalid promo start/end date format.", "do_nothing", "Invalid Date", "error")

    End Sub

    Private Function ValidateChangeDateEnhanced() As Boolean

        Dim bResult As Boolean = False

        ' check dates
        If Not IsDate(txtPeriodFrom.Text) Or _
            Not IsDate(txtPeriodTo.Text) Then

            MessageBox("Invalid promo start/end date format.", "do_nothing", "Invalid Date", "error")

        ElseIf CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then
            MessageBox("Invalid promo start/end date range.", "do_nothing", "Invalid Date", "error")

        ElseIf DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < CInt(ViewState("NumLeadDaysApprovalMPD")) Then
            MessageBox("Start of promotion must be set at least a day in advance.", "do_nothing", "Invalid Date", "error")


        ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = CInt(ViewState("NumLeadDaysApprovalMPD"))) And (Now.TimeOfDay.TotalMinutes > CInt(ViewState("ApprovalCutoffMinutes"))) Then
            ' NBS:20200616
            'ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (14 * 60)) Then

            MessageBox("Start of promotion must be set at least " & ViewState("NumLeadDaysApprovalMPD") & " days in advance since it is already beyond the " & ViewState("ApprovalCutoffMPD") & " cut-off time", "do_nothing", "Invalid Date", "error")

        Else

            'MessageBox("It is already beyond the 2PM cut-off.<br />" & _
            '            "Promotion will be implemented within two days.", _
            '            "reviewed_with_change_date", "Beyond Cut-off", "inquiry")

            'SubmitForFinalApproval()

            bResult = True
        End If

        Return bResult

    End Function

    Protected Sub linkRebateBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkRebateBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
    End Sub
End Class
