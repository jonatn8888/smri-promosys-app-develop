' Object Name	    :       CreateRCPDRequest.aspx
' Purpose		    :       Cancellation/addedum/extention/clarification search parameter module
' Date Created	    :       11/09/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0              11/09/2012     Dow T. Carpio     Created this control.
' 2.0              10/24/2013     Dow T. Carpio     Revised session string to query string

Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.IO
Imports System.Data.SqlClient

Partial Class RCDPMemoView
    Inherits System.Web.UI.Page


    Private Sub PageAlert_Init()

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

        Try
            hidRequestID.Value = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)
        Catch ex As Exception
            'Do nothing
        End Try

        Try
            hidMemoID.Value = clsEncryptDecrypt.DecryptText(Request("MemoID").ToString, SystemUser.EncryptKey.ToString)
        Catch ex As Exception
            'Do nothing
        End Try

        Dim _v3 As String = ""
        Try
            _v3 = clsEncryptDecrypt.DecryptText(Request("v3").ToString, SystemUser.EncryptKey.ToString)
        Catch ex As Exception
            'Do nothing
        End Try

        Dim _v4 As String = ""
        Try
            _v4 = clsEncryptDecrypt.DecryptText(Request("v4").ToString, SystemUser.EncryptKey.ToString)
        Catch ex As Exception
            'Do nothing
        End Try

        Dim _v5 As String = ""
        Try
            _v5 = clsEncryptDecrypt.DecryptText(Request("v5").ToString, SystemUser.EncryptKey.ToString)
        Catch ex As Exception
            'Do nothing
        End Try

        If _Status = "VIEW" Then


            If _v4 = "I" Or _
                 _v4 = "U" Then


            End If

        ElseIf _Status = "ADDNEW" Or _
                     _Status = "EDIT" Then

            'If _Status = StatusModes.ADDNEW.ToString Then

            'MessageButton(btnEditSave, "Proceed saving?")

            If _v5 <> "" Then

                'DeleteFile()

            End If

            'End If

        End If

        'hfrbutton.Value = _v3.ToString
        'Page.ClientScript.RegisterStartupScript(Page.GetType(), "onload", "SetVisibility('" & hfrbutton.Value & "');", True)

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Not Page.IsPostBack Then

            Try


                Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

                'Revised dowcarpio08222012@smretailinc: revised sql script of sqldsPromos to sort promotion using seqno(additional field) 
                '::ToDo:: parameter and security checking
                If SystemUser.UserID = 0 Or (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Or _
                                            SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover Or _
                                            SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover Or _
                                            SystemUser.UserLevel = SystemUser.UserRoles.Analyst Or _
                                            SystemUser.UserLevel = SystemUser.UserRoles.Reviewer Or _
                                            SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover) Then

                    ' do nothing
                Else

                    Response.Redirect("InvalidAccess.aspx")

                End If

                hidCRID.Value = clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString)

                If (Request("MemoID") = Nothing) And (Request("RequestID") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

                trEditRequest.Visible = False
                trSaveMemo.Visible = False
                trForApproval.Visible = False
                trApprove.Visible = False
                trCreateMemoDraft.Visible = False
                trReturn.Visible = False
                trViewRequest.Visible = False

                clsSession.CurrMemoID = clsEncryptDecrypt.DecryptText(Request("MemoID").ToString, SystemUser.EncryptKey.ToString)

                If _Status = "ADDNEW" Or _
                        _Status = "EDIT" Then

                    'imgRequestDate.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtRequestDate.ClientID + "', '" + txtRequestDate.ClientID + "');event.cancelBubble=true;")
                    'imgPromoFrom.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtPromoFrom.ClientID + "', '" + txtPromoFrom.ClientID + "');event.cancelBubble=true;")
                    'imgPromoTo.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtPromoTo.ClientID + "', '" + txtPromoTo.ClientID + "');event.cancelBubble=true;")
                    'imgPromoApprovedDate.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtPromoApprovedDate.ClientID + "', '" + txtPromoApprovedDate.ClientID + "');event.cancelBubble=true;")
                    'imgMemoDate.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtMemoDate.ClientID + "', '" + txtMemoDate.ClientID + "');event.cancelBubble=true;")
                    'imgMemoFrom.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtMemoFrom.ClientID + "', '" + txtMemoFrom.ClientID + "');event.cancelBubble=true;")
                    'imgMemoTo.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtMemoTo.ClientID + "', '" + txtMemoTo.ClientID + "');event.cancelBubble=true;")
                    'imgMemoApprovedDate.Attributes.Add("onclick", "javascript:cshowcalendar('form1', '" + txtMemoApprovedDate.ClientID + "', '" + txtMemoApprovedDate.ClientID + "');event.cancelBubble=true;")

                    'txtSearchBranch.Attributes.Add("onkeyup", "javascript:Filter(this,'" + grdTransaction_PromoBranches.ClientID + "');")

                End If

            Catch ex As Exception

                Response.Redirect("InvalidAccess.aspx")

            End Try

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PageAlert_Init()

        If Not Page.IsPostBack Then

            StartupProcedures()

        End If
    End Sub

    Private Sub StartupProcedures()

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

        Try

            LoadMemoInformation()
            LoadRequestInformation()
            LoadDefaultGuidelines()
            ShowAttachments()
            ChangeInterface()

        Catch ex As Exception

            Dim a As String = ex.Message
            If Not ex.Message = "Thread was being aborted." Then

                Response.Redirect("InvalidAccess.aspx")

            End If

        End Try

    End Sub


    Private Sub ChangeInterface()

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

        If _Status = "VIEW" Then

            Label_Properties(True)
            TextBox_Properties(False)
            Calendar_Properties(False)

            'trEditRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor And InStr("Draft", lblStatus.Text) = 0)
            'lnkSubmitRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor And InStr("Draft", lblStatus.Text) = 0)

            If SystemUser.UserLevel = SystemUser.UserRoles.Reviewer And lblReqStatus.Text = "For Review" Then

                trForApproval.Visible = True
                trReturn.Visible = True

            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover And lblReqStatus.Text = "For Final Approval" Then

                trApprove.Visible = True
                trReturn.Visible = True

            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.Analyst And lblReqStatus.Text = "Returned to Analyst" Then

                trEditRequest.Visible = True
                trReturn.Visible = True

            End If

            '    lnkDraftSave.Visible = False
            '    trEditRequest.Visible = True
            '    lnkSubmitRequest.Visible = True
            '    lnkApprove.Visible = False
            '    trCreateMemoDraft.Visible = False
            '    trReturn.Visible = False

            'ElseIf SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover And lblReqStatus.Text = "For MBU Approval" Then

            '    lnkDraftSave.Visible = False
            '    trEditRequest.Visible = False
            '    lnkSubmitRequest.Visible = False
            '    lnkApprove.Visible = True
            '    trCreateMemoDraft.Visible = False
            '    trReturn.Visible = True

            'ElseIf SystemUser.UserLevel = SystemUser.UserRoles.Analyst And lblReqStatus.Text = "For MPD Processing" Then

            '    lnkDraftSave.Visible = False
            '    trEditRequest.Visible = False
            '    lnkSubmitRequest.Visible = False
            '    lnkApprove.Visible = False
            '    trCreateMemoDraft.Visible = True
            '    trReturn.Visible = True

            'Else

            '    lnkDraftSave.Visible = False
            '    trEditRequest.Visible = False
            '    lnkSubmitRequest.Visible = False
            '    lnkApprove.Visible = False
            '    trCreateMemoDraft.Visible = False
            '    trReturn.Visible = False

            'End If

            trViewRequest.Visible = True

            lnkInsertGuidelines.Visible = False
            lnkEditGuidelines.Visible = False

            lnkEditMechanicsOld.Visible = False
            lnkEditMechanicsNew.Visible = False

            lnkEditGuidelinesOld.Visible = False
            lnkEditGuidelinesNew.Visible = False

        ElseIf _Status = "ADDNEW" Or _
                     _Status = "EDIT" Then

            Label_Properties(False)
            TextBox_Properties(True)
            Calendar_Properties(True)

            'button properties
            trSaveMemo.Visible = True
            'lnkSubmitRequest.Visible = False
            lnkApprove.Visible = False
            trReturn.Visible = False

            lnkInsertGuidelines.Visible = True
            lnkEditGuidelines.Visible = True

            lnkEditGuidelinesOld.Visible = True
            lnkEditGuidelinesNew.Visible = True

            trPrintMemo.Visible = False

        End If

        ' RCPD Type are as follows:
        '   CCL - Cancellation
        '   ADD - Addendum
        '   EXT - Extension
        '   CRF - Clarification

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

        If strType = "CCL" Then

            trEffectDate.Visible = True
            trExtendedUntil.Visible = False
            trDatePeriod.Visible = True
            trReason.Visible = True
            trCurrPromotions.Visible = True
            trCRPromotions.Visible = False
            trGuidelinesADD.Visible = False
            trMechanicsADD.Visible = False
            trGuidelines.Visible = False
            lblCRPromoLabel.Text = Nothing
            lblRequstTypeDesc.Text = "Cancellation"


            cboPromoType.Visible = False
            lnkInsertGuidelines.Visible = False
            lnkEditGuidelines.Visible = False


        ElseIf strType = "ADD" Then

            trEffectDate.Visible = False
            trExtendedUntil.Visible = False
            trDatePeriod.Visible = True
            trReason.Visible = False
            trCurrPromotions.Visible = False
            trCRPromotions.Visible = False
            trGuidelinesADD.Visible = True
            trMechanicsADD.Visible = True
            trGuidelines.Visible = False

            If _Status = "VIEW" Then

                gridPromotionsAdd.Columns(0).Visible = False


            ElseIf _Status = "ADDNEW" Or _
                               _Status = "EDIT" Then

                If gridPromotionsAdd.Rows.Count = 0 Then

                End If

            End If
            lblCRPromoLabel.Text = "Additional"
            lblRequstTypeDesc.Text = "Addendum"

        ElseIf strType = "EXT" Then

            trEffectDate.Visible = False
            trExtendedUntil.Visible = True
            trDatePeriod.Visible = True
            trReason.Visible = False
            trCurrPromotions.Visible = True
            trGuidelinesADD.Visible = False
            trMechanicsADD.Visible = False
            trGuidelines.Visible = False
            trCRPromotions.Visible = False
            lblCRPromoLabel.Text = Nothing
            lblRequstTypeDesc.Text = "Extension"

            cboPromoType.Visible = False
            lnkInsertGuidelines.Visible = False
            lnkEditGuidelines.Visible = False

        ElseIf strType = "CRF" Then

            trEffectDate.Visible = False
            trExtendedUntil.Visible = False
            trDatePeriod.Visible = True
            trReason.Visible = False
            trCurrPromotions.Visible = False
            trCRPromotions.Visible = False
            trGuidelinesADD.Visible = False
            trMechanicsADD.Visible = False
            trGuidelines.Visible = False

            If _Status = "VIEW" Then

                gridPromotionsAdd.Columns(0).Visible = False


            ElseIf _Status = "ADDNEW" Or _
                               _Status = "EDIT" Then

                If gridPromotionsAdd.Rows.Count = 0 Then

                End If

            End If
            lblCRPromoLabel.Text = Nothing
            lblRequstTypeDesc.Text = "Clarification"

        ElseIf strType = "SWP" Then
            trEffectDate.Visible = True
            trExtendedUntil.Visible = False
            trDatePeriod.Visible = True
            trReason.Visible = False
            trCurrPromotions.Visible = False
            trCRPromotions.Visible = False
            trGuidelinesADD.Visible = False
            trMechanicsADD.Visible = False
            trGuidelines.Visible = False


            trSwipestakesSeed.Visible = True
            trSwipestakesReseed.Visible = True

            If _Status = "VIEW" Then

                gridPromotionsAdd.Columns(0).Visible = False


            ElseIf _Status = "ADDNEW" Or _
                               _Status = "EDIT" Then

                If gridPromotionsAdd.Rows.Count = 0 Then

                End If

            End If
            lblCRPromoLabel.Text = Nothing
            lblRequstTypeDesc.Text = "Swipestakes Reseeding"

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If

        If Request("Filename") <> Nothing Then
            Dim fname As String
            Dim strDir As String

            strDir = clsPromo.pathAttachment & "CR-" & Right("00000" + clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString.Trim, 5)
            fname = strDir & "\" & Request("Filename")

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

    End Sub

    Private Sub TextBox_Properties(ByVal boolValue As Boolean)

        'txtCancelEffectDate.Visible = boolValue
        'txtCancelTitle.Visible = boolValue
        'txtCancelReason.Visible = boolValue
        cboPromoType.Visible = boolValue

    End Sub

    Private Sub Label_Properties(ByVal boolValue As Boolean)

        'lblCancelEffectDate.Visible = boolValue
        'lblCancelTitle.Visible = boolValue
        'lblCancelReason.Visible = boolValue

    End Sub


    Private Sub Calendar_Properties(ByVal boolValue As Boolean)

        'calCancelEffectDate.Visible = boolValue

    End Sub
    Private Sub LoadRequestInformation()

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString
        If _Status = "ADDNEW" Then

            'txtCancelEffectDate.Text = ""
            'txtCancelTitle.Text = "CANCELLATION: " & lblPromoTitle.Text
            'txtCancelReason.Text = ""

        ElseIf _Status = "VIEW" Or _
                          _Status = "EDIT" Then
            Dim taChangeRequests As New dsPromotionsTableAdapters.ChangeRequestsTableAdapter
            Dim dtChangeRequests As dsPromotions.ChangeRequestsDataTable
            Dim rowChangeRequests As dsPromotions.ChangeRequestsRow

            dtChangeRequests = taChangeRequests.GetRequestByID(clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString))

            If dtChangeRequests.Rows.Count = 0 Then

                Response.Redirect("InvalidAccess.aspx")

            Else

                rowChangeRequests = dtChangeRequests.Rows(0)

                With rowChangeRequests

                    'lblRequestID.Text = .RequestID
                    'lblRequestDate.Text = .RequestDate.ToLongDateString()

                    If _Status = "VIEW" Or _
                    _Status = "EDIT" Then


                        lblMemoID.Text = .MemoNumber

                        lblReqStatus.Text = .Status.ToString
                        lblExtendedUntil.Text = .PromoPeriodTo.ToLongDateString()
                        lblCancelEffectDate.Text = .EffectDate.ToLongDateString()
                        'lblCRFPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .PromoPeriodTo.ToLongDateString()
                        lblCancelTitle.Text = .Title.ToString

                        If strType = "EXT" Then
                            lblOldPromoPeriod.Text &= " to " & .EffectDate.ToLongDateString()
                        End If



                        Try
                            litGuidelines.Text = Server.HtmlDecode(.Guidelines.ToString())
                        Catch ex As Exception

                            'do nothing

                        End Try

                        lblRemarks.Text = Replace(Server.HtmlDecode(.Remarks.ToString()), vbCrLf.ToString(), "<br/>")
                        lblCancelReason.Text = .Reason

                        Try

                            litMechanicsOld.Text = Server.HtmlDecode(.MechanicsOld.ToString())

                        Catch ex As Exception
                            'do nothing
                        End Try

                        Try

                            litMechanicsNew.Text = Server.HtmlDecode(.MechanicsNew.ToString())

                        Catch ex As Exception
                            'do nothing
                        End Try

                        Try

                            LitGuidelinesOld.Text = Server.HtmlDecode(.GuidelinesOld.ToString())

                        Catch ex As Exception
                            'do nothing
                        End Try

                        Try

                            LitGuidelinesNew.Text = Server.HtmlDecode(.GuidelinesNew.ToString())

                        Catch ex As Exception
                            'do nothing
                        End Try

                        lblPreparedBy.Text = clsPromo.formatParamValue(IIf(.PreparedBy.ToString() = "", .RequestedBy.ToString(), .PreparedBy.ToString())).ToString
                        lblReviewedBy.Text = clsPromo.formatParamValue(.ReviewedBy.ToString()).ToString
                        lblApprovedBy.Text = clsPromo.formatParamValue(.ApprovedBy.ToString()).ToString
                        tbrowReviewedLine.Visible = (lblReviewedBy.Text <> String.Empty)
                        tbrowApprovedLine.Visible = (lblApprovedBy.Text <> String.Empty)

                    End If

                End With


            End If



        End If

    End Sub
    Private Sub LoadMemoInformation()
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString
        Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter()
        Dim dtMemos As dsPromotions.MemosDataTable
        Dim rowMemos As dsPromotions.MemosRow

        dtMemos = taMemos.GetMemoByID(hidMemoID.Value)

        If dtMemos.Rows.Count = 0 Then

            Response.Redirect("InvalidAccess.aspx")

        Else

            rowMemos = dtMemos.Rows(0)

            With rowMemos

                'lblStatus.Text = .Status.ToString()

                If .MemoNumber.ToString() = "" Then
                    lblMemoNumber.Text = "Draft #" & Format(.MemoID, "0####")
                Else
                    lblMemoNumber.Text = .MemoNumber.ToString()

                End If

                'ViewState("MemoID") = .MemoID

                lblMemoDate.Text = Now.Date.ToLongDateString()

                If .Status = "Approved" Then
                    'lblMemoDate.Text = .ApproveDate.ToLongDateString()
                    'lblRemarks.Text = ""
                Else
                    'lblMemoDate.Text = .MemoDate.ToLongDateString()
                    'lblRemarks.Text = Replace(Server.HtmlDecode(.Remarks.ToString()), vbCrLf.ToString(), "<br/>")
                End If

                lblPromoTitle.Text = .Title.ToString()

                If strType = "EXT" Then

                    'If lblReqStatus.Text = "Approved" Then

                    '    lblOldPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .OldPromoPeriodTo.ToLongDateString()

                    'Else

                    lblOldPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() '& IIf(.OldPromoPeriodTo.ToString <> "1/1/1900 12:00:00 AM", .OldPromoPeriodTo.ToLongDateString(), .PromoPeriodTo.ToLongDateString()).ToString

                    'End If

                Else

                    lblOldPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .PromoPeriodTo.ToLongDateString()

                End If

                ViewState("PromoPeriodFrom") = .PromoPeriodFrom.Date()
                ViewState("PromoPeriodTo") = .PromoPeriodTo.Date()

                lblBranches.Text = .Branches.ToString()

                litGuidelines.Text = Server.HtmlDecode(.Guidelines.ToString())
                'lblPreparedBy.Text = .PreparedBy.ToString()

                'lblReviewedBy.Text = .ReviewedBy.ToString()
                'lblApprovedBy.Text = .ApprovedBy.ToString()

                'tbrowReviewedLine.Visible = (lblReviewedBy.Text <> String.Empty)
                'tbrowApprovedLine.Visible = (lblApprovedBy.Text <> String.Empty)

                'ViewState("RequestID") = .RequestID
                ViewState("OwnerGroup") = .OwnerGroup

                'Added dowcarpio08162012@smretailinc: hide promo item for SBU Marketing Requestor
                If .GroupType = "SBU" Or _
                .GroupType = "CREDIT" Then

                    gridPromotions.Columns(1).Visible = False

                End If

            End With

        End If

    End Sub
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

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & hidRequestID.Value, 1)
        Response.Redirect("PromoRequest.aspx?DownLoad=" & hidRequestID.Value & ".zip" & "&Path=" & clsPromo.pathAttachment & hidRequestID.Value)
    End Sub

    Protected Sub lnkViewBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewBranches.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>OpenViewBranches('','');</script>")
    End Sub

    Private Sub ShowAttachments()
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & "CR-" & Right("00000" + clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString.Trim, 5)
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""RCDPMemoView.aspx?FileName=" & Server.UrlEncode(file.Name.ToString()) & "&CRID=" & Request("CRID").ToString & "&MemoID=" & Request("MemoID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString & """>" & file.Name.ToString() & "</a> "
                'clsEncryptDecrypt.EncryptText(lblCRID.Text.ToString, SystemUser.EncryptKey.ToString)
                'lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""RCDPMemoView.aspx?FileName=" & Server.UrlEncode(file.Name.ToString()) & "&CRID=" & clsEncryptDecrypt.EncryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString) & "&MemoID=" & Request("MemoID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString & """>" & file.Name.ToString() & "</a> "
                'Response.Redirect("PromoRequest.aspx?DownLoad=" & "CR-" &  hidRequestID.Value & ".zip" & "&Path=" & clsPromo.pathAttachment & "CR-" &  hidRequestID.Value)

            Next

            If Len(strFiles) = 0 Then lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr
    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    '    If Not IsPostBack() Then



    '        LoadMemoInformation()

    '        trEditRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.Analyst And InStr("Approved; For Final Approval; For Review", lblStatus.Text) = 0)

    '        trForApproval.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.Reviewer And lblStatus.Text = "For Review")
    '        trApproveMemo.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover And lblStatus.Text = "For Final Approval")

    '        trReturn.Visible = (trForApproval.Visible Or trApproveMemo.Visible)

    '        ' override properties if start of promo is pass due
    '        'If (DateDiff(DateInterval.Day, Today(), ViewState("PromoPeriodFrom")) < 0) And (lblStatus.Text <> "Approved") Then
    '        '    trApproveMemo.Visible = False
    '        '    trForApproval.Visible = False

    '        '    lblStatus.Text &= " - Expired"
    '        '    lblStatus.ForeColor = Drawing.Color.Red
    '        'End If

    '    End If


    'End Sub



    Protected Sub lnkDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDone.Click

        If SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover Or SystemUser.UserLevel = SystemUser.UserRoles.Reviewer Then

            Response.Redirect("PromoMemoList.aspx")
        Else

            Response.Redirect("RCDPReqList.aspx?v1=" & Request("v1").ToString)
        End If

    End Sub

    Protected Sub lnkEditMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditRequest.Click

        'flag to return to this module on close

        ' Added dowcarpio07272012@smretailinc for create memo request string
        ' v1 = memoid, v2=requestid

        Response.Redirect("RCDPMemoView.aspx?MemoID=" & Request("MemoID").ToString & "&CRID=" & Request("CRID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("EDIT", SystemUser.EncryptKey.ToString))

    End Sub

    Protected Sub lnkPrintMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintMemo.Click

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

        ' ::ToDo:: flag to return to this module on close


        clsSession.FlagForPOSDisp = False


        If strType = "CCL" Then

            Response.Redirect("ViewMemo_Cancellation.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString))

        ElseIf strType = "ADD" Then

            Response.Redirect("ViewMemo_Addendum.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString))

        ElseIf strType = "EXT" Then

            Response.Redirect("ViewMemo_Extension.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString))

        ElseIf strType = "CRF" Then

            Response.Redirect("ViewMemo_Clarification.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString))

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If



    End Sub

    Protected Sub lnkReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReturn.Click

        'Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

        ' Revised dowcarpio20131216@smretailinc: application for all RCPD requests.
        'If lblReqStatus.Text = "Returned to Analyst" Then ' Or strType = "CCL"

        'prompt user for reason
        lblPopTitle.Value = "Disapprove Request"
        clsSession.Message = "<b>Reason why this document is being returned:</b><br>"
        clsSession.Icon = "inputinquiry"
        ViewState("process") = "cancelrequest"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openinputbox2('','');</script>")  ' calls cmdPopUpOK code

        'Else

        '    lblPopTitle.Value = "Return for Revision"
        '    clsSession.Message = "<b>Reason why this promotions is being returned:</b><br>"
        '    clsSession.Icon = "inputinquiry"
        '    ViewState("process") = "cancelrequest"
        '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openinputbox('','');</script>")



        ' End If

    End Sub

    ' Revised dowcarpio02012013@smretailinc: Tagging of status have been revised to eliminate double posting. Already considered the bugs/errors encountered using inline script from previous codes
    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString
        Dim AuditRem As String = Nothing

        If strType = "CCL" Then

            AuditRem = "Cancellation"

        ElseIf strType = "ADD" Then

            AuditRem = "Addendum"

        ElseIf strType = "EXT" Then

            AuditRem = "Extension"

        ElseIf strType = "CRF" Then

            AuditRem = "Clarification"

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If

        If clsSession.DeleteStatus = "yes" Then

            Select Case ViewState("process").ToString

                Case "reviewed_memo"

                    ' Submit Memo to Reviewer
                    clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                       hidMemoID.Value, _
                                       hidRequestID.Value, _
                                       Nothing, _
                                       clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                       "For Final Approval", _
                                       Nothing, _
                                       SystemUser.UserID, _
                                       Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                       AuditRem & " memorandum was reviewed.", _
                                       "Promotion Transaction")

                Case "approved_memo"

                    ' Approve Memo
                    clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                      hidMemoID.Value, _
                                        hidRequestID.Value, _
                                       Nothing, _
                                       clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                       "Approved", _
                                       Nothing, _
                                       SystemUser.UserID, _
                                       Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                       AuditRem & " memorandum was approved.", _
                                       "Promotion Transaction")

                Case "cancelrequest"
                    'If lblReqStatus.Text = "Returned to Analyst" Then
                    If hidRadioButton.Value = "MPD Analyst" Then

                        ' Return to MPD Analyst
                        clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                           hidMemoID.Value, _
                                            hidRequestID.Value, _
                                           Nothing, _
                                           clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                           "Returned to Analyst", _
                                           Server.HtmlEncode(hidinputbox.Value.Replace("'", "''")), _
                                           SystemUser.UserID, _
                                           Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                           AuditRem & " memorandum was returned to MPD Analyst.", _
                                           "Promotion Transaction")

                    Else

                        ' Return to Requestor
                        clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                             hidMemoID.Value, _
                             hidRequestID.Value, _
                             Nothing, _
                             clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                             "Returned", _
                             Server.HtmlEncode(hidinputbox.Value.Replace("'", "''")), _
                             SystemUser.UserID, _
                             Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                             AuditRem & " memorandum was returned to requestor.", _
                             "Promotion Transaction")

                    End If

                Case Else

                    ' do nothing

            End Select


            If SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover Or SystemUser.UserLevel = SystemUser.UserRoles.Reviewer Then

                Response.Redirect("PromoMemoList.aspx")

            Else

                Response.Redirect("RCDPReqList.aspx?v1=" & Request("v1").ToString)

            End If
        End If

            GC.Collect()

    End Sub

    'Protected Sub lnkForApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSubmitRequest.Click

    '    ' prompt for confirmation
    '    clsSession.Message = "Are you sure you want submit this request?"
    '    clsSession.Icon = "inquiry"
    '    lblPopTitle.Value = "Confirm Action"
    '    ViewState("process") = "submit"
    '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    'End Sub

    Protected Sub lnkApproveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkApprove.Click

        ' prompt for confirmation
        clsSession.Message = "Do you wish to approve this memo?"
        clsSession.Icon = "inquiry"
        lblPopTitle.Value = "Confirm Approval"
        ViewState("process") = "approved_memo"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    'Protected Sub cmdRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRedirect.Click

    '    If clsSession.DeleteStatus = "yes" Then

    '        Select Case ViewState("process").ToString()

    '            Case "reviewed_memo"
    '                ' mark request as reviewed and update status
    '                sqldsData.UpdateCommand = "UPDATE Memos SET Status = 'For Final Approval', ReviewedBy = '" & SystemUser.UserSignName & "', ReviewerPos = '" & SystemUser.UserSignPosition & "', ReviewDate = GETDATE() WHERE MemoID = " & hidMemoID.Value
    '                sqldsData.Update()

    '                ' update promo request counterpart's status
    '                sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'For Final Approval' WHERE RequestID = " & CInt( hidRequestID.Value)
    '                sqldsData.Update()

    '                'audit trail
    '                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), _
    '                                          hidRequestID.Value, _
    '                                          "Reviewed and forwarded Memo draft for final approval.", _
    '                                          SystemUser.UserName, _
    '                                          "Promotion Transaction")
    '            Case "approved_memo"

    '                '****************************************
    '                ' create new memo number upon approval
    '                '****************************************

    '                Dim sNewMemoNum As String = CreateNewMemoNumber()

    '                '****************************************
    '                ' update tables
    '                '****************************************

    '                ' mark memo as approved and post necessary info
    '                sqldsData.UpdateCommand = "UPDATE Memos SET MemoNumber = '" & sNewMemoNum & "', " & _
    '                                          "Status = 'Approved', ApprovedBy = '" & SystemUser.UserSignName & "', " & _
    '                                          "ApproverPos = '" & SystemUser.UserSignPosition & "', " & _
    '                                          "ApproveDate = GETDATE() WHERE MemoID = " & hidMemoID.Value
    '                sqldsData.Update()

    '                ' assign promo event code if promotion is "SMAC Deals"
    '                CreateNewPromoEventCode(8000, 9999)
    '                ' display info on guidelines
    '                AppendExtraDetailsToGuidelines()

    '                ' update promo request counterpart's status
    '                sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'Approved' WHERE RequestID = " & CInt( hidRequestID.Value)
    '                sqldsData.Update()

    '                ' update audit trail
    '                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), _
    '                                           hidRequestID.Value, "Promotional Memo approved.", _
    '                                          SystemUser.UserName, "Promotion Transaction")

    '            Case Else

    '                ' do nothing -- INVALID PROCESS ENTRY

    '        End Select

    '        Response.Redirect("PromoMemoList.aspx")
    '    End If

    'End Sub

    Protected Function CreateNewMemoNumber() As String

        ' new format: MPD-uuuu-9999-yy
        ' uuuu = business unit where request originated
        ' 9999 = sequential number
        ' yy   = year when promo was approved

        Dim sNewMemoNum As String

        ' get last memo series for this BizUnit

        sqldsData.SelectCommand = "SELECT Count(*) AS xMemoCount, G.BizUnit FROM Memos " & _
                                  "LEFT JOIN UserGroups AS G ON Memos.OwnerGroup = G.GroupID " & _
                                  "WHERE Memos.Status = 'Approved' " & _
                                  "AND G.BizUnit IN (SELECT BizUnit FROM UserGroups WHERE GroupID = 0" & ViewState("OwnerGroup") & ") " & _
                                  "GROUP BY G.BizUnit"

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
            sNewMemoNum = "MPD-" & UCase(Left(drMemo("BizUnit"), 4)) & Format(drMemo("xMemoCount") + 1, "-0###") & Format(Now, "-yy")

        End If

        CreateNewMemoNumber = sNewMemoNum
    End Function

    Private Sub CreateNewPromoEventCode(ByRef nSeriesStart As Integer, ByRef nSeriesEnd As Integer)

        ' check if promotion is SMAC Deals
        sqldsData.SelectCommand = "SELECT P.PromoID FROM Promotions AS P LEFT JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                                  "WHERE T.WithEventCode = 1 AND P.MemoID = 0" & hidMemoID.Value

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
                                      "WHERE MemoID = " & hidMemoID.Value
            sqldsData.Update()

        End If

    End Sub

    Protected Sub lnkHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkHistory.Click
        ClientScript.RegisterStartupScript(Me.GetType, "keykey", "<script>OpenAuditLogs('324','800','" & hidRequestID.Value & "','02');</script>")
    End Sub





    Private Sub AppendExtraDetailsToGuidelines()

        Dim NewGuidelines As String

        sqldsData.SelectCommand = "SELECT Guidelines FROM Memos WHERE MemoID = 0" & hidMemoID.Value

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
                                      "WHERE T.WithEventCode = 1 AND P.MemoID = 0" & hidMemoID.Value

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
                                     "WHERE MemoID = 0" & hidMemoID.Value
                    .UpdateParameters.Add("Guidelines", Server.HtmlEncode(NewGuidelines))
                    .Update()
                End With
            End If

        End If

    End Sub





    Private Sub SaveInfo(ByVal strStatus As String)

        blistErrorMsg.Items.Clear()

        If blistErrorMsg.Items.Count > 0 Then

            Exit Sub

        End If

        If Len(LitGuidelinesOld.Text + LitGuidelinesNew.Text) > 0 Then

            If LitGuidelinesOld.Text = "" Then

                blistErrorMsg.Items.Add("Guidelines (Old) is required.")
                Exit Sub

            End If

            If LitGuidelinesNew.Text = "" Then

                blistErrorMsg.Items.Add("Guidelines (New) is required.")
                Exit Sub

            End If

        End If

        If SaveRecord(strStatus) Then

            Server.Transfer("RCDPReqList.aspx?v1=" & Request("v1").ToString)

        End If

    End Sub

    Private Function SaveRecord(ByVal strStatus) As Boolean

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand

        Try

            sqlConn.Open()
            sqlCmd = New SqlCommand

           If _Status = "EDIT" Then

                sqlCmd.CommandText = "PMS_P_ChangeRequests_U2"

            End If


            sqlCmd.Connection = sqlConn
            sqlCmd.CommandTimeout = 0
            sqlCmd.CommandType = CommandType.StoredProcedure


            sqlCmd.Parameters.Add("@MemoID", SqlDbType.SmallInt)
            sqlCmd.Parameters("@MemoID").Value = hidMemoID.Value

            sqlCmd.Parameters.Add("@RequestID", SqlDbType.SmallInt)
            sqlCmd.Parameters("@RequestID").Value = hidRequestID.Value

            sqlCmd.Parameters.Add("@RequestType", SqlDbType.VarChar)
            sqlCmd.Parameters("@RequestType").Value = strType

            sqlCmd.Parameters.Add("@CRID", SqlDbType.Int)
            sqlCmd.Parameters("@CRID").Value = clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString)

            sqlCmd.Parameters.Add("@GuidelinesOld", SqlDbType.Text)
            sqlCmd.Parameters("@GuidelinesOld").Value = Server.HtmlEncode(litGuidelinesOld.Text)

            sqlCmd.Parameters.Add("@GuidelinesNew", SqlDbType.Text)
            sqlCmd.Parameters("@GuidelinesNew").Value = Server.HtmlEncode(LitGuidelinesNew.Text)

            sqlCmd.Parameters.Add("@Guidelines", SqlDbType.Text)
            sqlCmd.Parameters("@Guidelines").Value = Server.HtmlEncode(litGuidelines.Text)

            sqlCmd.Parameters.Add("@PreparedBy", SqlDbType.VarChar)
            sqlCmd.Parameters("@PreparedBy").Value = SystemUser.UserSignName.ToString

            sqlCmd.Parameters.Add("@PreparePOS", SqlDbType.VarChar)
            sqlCmd.Parameters("@PreparePOS").Value = SystemUser.UserSignPosition.ToString

            sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar)
            sqlCmd.Parameters("@Status").Value = strStatus.ToString

            sqlCmd.Parameters.Add("@UserID", SqlDbType.SmallInt)
            sqlCmd.Parameters("@UserID").Value = SystemUser.UserID.ToString

            sqlCmd.ExecuteNonQuery()

            SaveRecord = True

        Catch ex As Exception

            blistErrorMsg.Items.Add(ex.Message)
            SaveRecord = False

        End Try

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()

    End Function

    Protected Sub lnkInsertGuidelines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkInsertGuidelines.Click

        If cboPromoType.SelectedValue < 0 Then Exit Sub

        sqldsData.SelectCommand = "SELECT * FROM PromoTypes WHERE PromoTypeID = " & cboPromoType.SelectedValue

        Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        Dim dr As DataRow = dv.Table.Rows(0)

        litGuidelines.Text &= "<p><b>" & dr("TypeDesc") & "</b><br />" & dr("DefCCLGuideline") & "</p>"

    End Sub

    Protected Sub lnkEditGuidelines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditGuidelines.Click

        hidBox.Value = litGuidelines.Text
        hidGuidelinesActive.Value = lnkEditGuidelines.ClientID
        clsSession.Mechanics = litGuidelines.Text.ToString()
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor2('" & cmdGuidelines.ClientID & "', 'Promotion Guidelines');</script>")

    End Sub

    Protected Sub cmdGuidelines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGuidelines.Click

        If hidGuidelinesActive.Value = lnkEditGuidelines.ClientID Then
            litGuidelines.Text = hidBox.Value
        ElseIf hidGuidelinesActive.Value = lnkEditGuidelinesOld.ClientID Then
            LitGuidelinesOld.Text = hidBox.Value
        ElseIf hidGuidelinesActive.Value = lnkEditGuidelinesNew.ClientID Then
            LitGuidelinesNew.Text = hidBox.Value
        End If

        GC.Collect()
    End Sub

    Private Sub LoadDefaultGuidelines()

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

        If litGuidelines.Text = "" And (_Status = "ADDNEW" Or _Status = "EDIT") Then

            Dim taPromoTypes As New dsPromotionsTableAdapters.PromoTypesTableAdapter()
            Dim dtPromoTypes As dsPromotions.PromoTypesDataTable
            Dim rowPromoType As dsPromotions.PromoTypesRow

            ' Revised by dowcarpio07272012@smretailinc: new request string for requestid
            ' get promo types from requests and display corresponding guidelines
            sqldsData.SelectCommand = "SELECT DISTINCT PromoTypeID FROM Promotions WHERE RequestID = " & hidRequestID.Value

            Dim dvPromo As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
            Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

            For Each dr As DataRow In dvPromo.Table.Rows
                dtPromoTypes = taPromoTypes.GetPromoTypeByID(dr("PromoTypeID"))
                rowPromoType = dtPromoTypes.Rows(0)

                If strType = "CCL" Then

                    litGuidelines.Text &= "<p><b>" & rowPromoType.TypeDesc & "</b><br />" & rowPromoType.DefCCLGuideline & "</p>"

                ElseIf strType = "ADD" Then

                    litGuidelines.Text &= "<p><b>" & rowPromoType.TypeDesc & "</b><br />" & rowPromoType.DefADDGuideline & "</p>"

                ElseIf strType = "EXT" Then

                    litGuidelines.Text &= "<p><b>" & rowPromoType.TypeDesc & "</b><br />" & rowPromoType.DefEXTGuideline & "</p>"

                ElseIf strType = "CRF" Then

                    litGuidelines.Text &= "<p><b>" & rowPromoType.TypeDesc & "</b><br />" & rowPromoType.DefCRFGuideline & "</p>"

                End If

            Next

            'dtPromoTypes = taPromoTypes.GetPromoTypeByID(nTypeID)

        End If
       
    End Sub

    Protected Sub lnkSaveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click
        SaveInfo("For Review")
    End Sub

    Protected Sub lnkForApproval_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkForApproval.Click
        ' prompt for confirmation
        clsSession.Message = "Forward this memo for final approval?"
        clsSession.Icon = "inquiry"
        lblPopTitle.Value = "Confirm Action"
        ViewState("process") = "reviewed_memo"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
    End Sub


    Protected Sub lnkViewRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewRequest.Click

        Response.Redirect("RCDPMemo.aspx?MemoID=" & Request("MemoID").ToString & "&CRID=" & Request("CRID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString))

    End Sub

    Protected Sub gridPromotions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridPromotions.SelectedIndexChanged

    End Sub

    Protected Sub gridPromotionsAdd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotionsAdd.RowDataBound
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

    Protected Sub lnkEditGuidelinesOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditGuidelinesOld.Click
        hidBox.Value = LitGuidelinesOld.Text
        hidGuidelinesActive.Value = lnkEditGuidelinesOld.ClientID
        clsSession.Mechanics = LitGuidelinesOld.Text.ToString()
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor2('" & cmdGuidelines.ClientID & "', 'Promotion Guidelines (Old)');</script>")
    End Sub

    Protected Sub lnkEditGuidelinesNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditGuidelinesNew.Click
        hidBox.Value = LitGuidelinesNew.Text
        hidGuidelinesActive.Value = lnkEditGuidelinesNew.ClientID
        clsSession.Mechanics = LitGuidelinesNew.Text.ToString()
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor2('" & cmdGuidelines.ClientID & "', 'Promotion Guidelines (New)');</script>")
    End Sub

   
    Protected Sub linkSwipestakesSeed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesSeed.Click
        clsSession.CurrRequestID = hidRequestID.Value
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesSeed();</script>")
    End Sub

    Protected Sub linkSwipestakesReseed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesReseed.Click
        Dim strRequestString As String = "?CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString)
        clsSession.CurrRequestID = hidRequestID.Value
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesReseed('" & strRequestString & "');</script>")
    End Sub
End Class
