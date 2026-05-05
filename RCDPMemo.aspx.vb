' Object Name	    :       CreateRCPDRequest.aspx
' Purpose		    :       Cancellation/addedum/extention/clarification search parameter module
' Date Created	    :       11/09/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0              11/09/2012     Dow T. Carpio     Created this control.
' 2.0              10/24/2013     Dow T. Carpio     Revised session string to query string
' 3.0              12/04/2013     Dow T. Carpio     Revised added promoperiodfrom in saving requests extension

Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.IO
Imports System.Data.SqlClient
Imports System



Partial Class RCDPMemo
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

                '::ToDo:: parameter and security checking

                If SystemUser.UserID = 0 Or (SystemUser.UserLevel <> SystemUser.UserRoles.PromoRequestor And _
                                                SystemUser.UserLevel <> SystemUser.UserRoles.RequestReviewer And _
                                                SystemUser.UserLevel <> SystemUser.UserRoles.RequestApprover And _
                                                SystemUser.UserLevel <> SystemUser.UserRoles.Analyst And _
                                                SystemUser.UserLevel <> SystemUser.UserRoles.Reviewer And _
                                                SystemUser.UserLevel <> SystemUser.UserRoles.SMACapprover And _
                                                SystemUser.UserLevel <> SystemUser.UserRoles.MemoApprover) Then
                    Response.Redirect("InvalidAccess.aspx")
                End If


                If (Request("MemoID") = Nothing) And (Request("RequestID") = Nothing) Then Response.Redirect("InvalidAccess.aspx")


                trEditRequest.Visible = False
                trSubmitRequest.Visible = False
                trDelete.Visible = False
                trDraftSave.Visible = False
                trPrintMemo.Visible = False
                trApprove.Visible = False
                trCreateMemoDraft.Visible = False
                trReturn.Visible = False

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

                    ' Initialization for current attachment(s)
                    clsSession.AttachmentPath = clsPromo.pathAttachment & hidRequestID.Value



                End If


            Catch ex As Exception

                Response.Redirect("InvalidAccess.aspx")

            End Try

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        PageAlert_Init()

        If Not Page.IsPostBack Then

            StartupProcedures(sender, e)

        End If
    End Sub

    Private Sub StartupProcedures(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

        Try

            LoadMemoInformation()
            LoadRequestInformation()
            ShowAttachments()
            ShowAttachments_SWP()
            ShowAttachments_CR()
            ChangeInterface(sender, e)

        Catch ex As Exception

            Dim a As String = ex.Message
            If Not ex.Message = "Thread was being aborted." Then

                Response.Redirect("InvalidAccess.aspx")

            End If


        End Try

    End Sub


    Private Sub ChangeInterface(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)
        Dim _proc As String = ""
        Try
            _proc = clsEncryptDecrypt.DecryptText(Request("proc").ToString, SystemUser.EncryptKey.ToString)
        Catch ex As Exception
            _proc = ""
        End Try
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

        ' get value to determine current page editing promotions
        ' _proc [yes/no]

        If _Status = "VIEW" Then

            Label_Properties(True)
            TextBox_Properties(False)
            Calendar_Properties(False)
            OtherControls_Properties(False)

            trEditRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor And InStr("Draft", lblStatus.Text) = 0)
            trSubmitRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor And InStr("Draft", lblStatus.Text) = 0)

            If (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor And (lblReqStatus.Text = "Draft" Or lblReqStatus.Text = "Returned")) Then

                trDraftSave.Visible = False

                'check requestor
                If ViewState("UserCreated").ToString = SystemUser.UserID Then
                    trDelete.Visible = True
                    trEditRequest.Visible = True
                    trSubmitRequest.Visible = True
                Else

                    trDelete.Visible = False
                    trEditRequest.Visible = False
                    trSubmitRequest.Visible = False

                End If

                trApprove.Visible = False
                trCreateMemoDraft.Visible = False
                trReturn.Visible = False

            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer And lblReqStatus.Text = "For Mdsg/Group Head Approval" Then

                trDelete.Visible = False
                trDraftSave.Visible = False
                trEditRequest.Visible = False
                trSubmitRequest.Visible = False
                trApprove.Visible = True
                trCreateMemoDraft.Visible = False
                trReturn.Visible = True


            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover And lblReqStatus.Text = "For MBU Approval" Then

                trDelete.Visible = False
                trDraftSave.Visible = False
                trEditRequest.Visible = False
                trSubmitRequest.Visible = False
                trApprove.Visible = True
                trCreateMemoDraft.Visible = False
                trReturn.Visible = True

                'MALAgasino 20180920 - Approve button should be visible for SBU
            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover And lblReqStatus.Text = "For BU Head Approval" Then

                trDelete.Visible = False
                trDraftSave.Visible = False
                trEditRequest.Visible = False
                trSubmitRequest.Visible = False
                trApprove.Visible = True
                trCreateMemoDraft.Visible = False
                trReturn.Visible = True


            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover And lblReqStatus.Text = "For MCI Approval" Then

                trDelete.Visible = False
                trDraftSave.Visible = False
                trEditRequest.Visible = False
                trSubmitRequest.Visible = False
                trApprove.Visible = True
                trCreateMemoDraft.Visible = False
                trReturn.Visible = True


            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.Analyst And lblReqStatus.Text = "For MPD Processing" Then

                trDelete.Visible = False
                trDraftSave.Visible = False
                trEditRequest.Visible = False
                trSubmitRequest.Visible = False
                trApprove.Visible = False
                trCreateMemoDraft.Visible = True
                trReturn.Visible = True

            Else

                trDelete.Visible = False
                trDraftSave.Visible = False
                trEditRequest.Visible = False
                trSubmitRequest.Visible = False
                trApprove.Visible = False
                trCreateMemoDraft.Visible = False
                trReturn.Visible = False

            End If

            'imgbtnDownload.Visible = True

        ElseIf _Status = "ADDNEW" Or _
                     _Status = "EDIT" Then

            Label_Properties(False)
            TextBox_Properties(True)
            Calendar_Properties(True)
            OtherControls_Properties(True)
            If (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor And (lblReqStatus.Text = "" Or lblReqStatus.Text = "Draft" Or lblReqStatus.Text = "Returned")) Then

                lnkattachment.Visible = True

            End If

            'button properties
            trDraftSave.Visible = True
            trSubmitRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor And InStr("Draft", lblStatus.Text) = 0)
            trApprove.Visible = False
            trReturn.Visible = False

            If (lblReqStatus.Text = "" Or lblReqStatus.Text = "Temp") And _proc = "" And strType = "ADD" Then

                ViewState("proc") = "delete"
                clsSession.DeleteStatus = "yes"
                For Each row As GridViewRow In gridPromotionsAdd.Rows

                    Dim cb As CheckBox = row.FindControl("chkRowSel")
                    cb.Checked = True

                Next

                cmdPopUpOK2_Click(sender, e)

            End If

        End If

        ' RCPD Type are as follows:
        '   CCL - Cancellation
        '   ADD - Addendum
        '   EXT - Extension
        '   CRF - Clarification


        If strType = "CCL" Then

            trEffectDate.Visible = True
            trExtendedUntil.Visible = False
            trCRFDatePeriod.Visible = False
            trCurrPromotions.Visible = True
            trPromotions.Visible = False
            trPromotionsVSLP.Visible = False 'temp
            trMechanicsADD.Visible = False
            trReason.Visible = True
            trAttachments.Visible = True

            lblRequstTypeDesc.Text = "Cancellation"

        ElseIf strType = "ADD" Then

            trEffectDate.Visible = False
            trExtendedUntil.Visible = False
            trCRFDatePeriod.Visible = False
            trCurrPromotions.Visible = False
            trPromotions.Visible = False
            trPromotionsVSLP.Visible = False
            trMechanicsADD.Visible = True
            trReason.Visible = False
            trAttachments.Visible = False

            lblRequstTypeDesc.Text = "Addendum"
            gridPromotionsAdd.EmptyDataText = "Add Promotions"

            If _Status = "VIEW" Then

                gridPromotionsAdd.Columns(0).Visible = False

                lnkDeletePromo.Visible = False
                lnkEditPromo.Visible = False
                lnkAddPromo.Visible = False

                'lnkEditMechanicsOld.Visible = False
                lnkEditMechanicsNew.Visible = False

            ElseIf _Status = "ADDNEW" Or _
                               _Status = "EDIT" Then

                If gridPromotionsAdd.Rows.Count = 0 Then

                    lnkDeletePromo.Visible = False
                    lnkEditPromo.Visible = False

                End If

            End If

        ElseIf strType = "EXT" Then

            trEffectDate.Visible = False
            trExtendedUntil.Visible = True
            trCRFDatePeriod.Visible = False
            trCurrPromotions.Visible = True
            trPromotions.Visible = False
            trPromotionsVSLP.Visible = False
            trMechanicsADD.Visible = False
            trReason.Visible = False
            trAttachments.Visible = True

            lblRequstTypeDesc.Text = "Extension"

            txtCRFPeriodFrom.Enabled = False
            calCRFPeriodFrom.Disabled = True

        ElseIf strType = "CRF" Then

            trEffectDate.Visible = False
            trExtendedUntil.Visible = False
            trCRFDatePeriod.Visible = True
            trCurrPromotions.Visible = False
            trPromotions.Visible = True
            trPromotionsVSLP.Visible = False
            trReason.Visible = False
            trMechanicsADD.Visible = False

            trAttachments.Visible = False

            lblRequstTypeDesc.Text = "Clarification"

            gridPromotionsAdd.EmptyDataText = "Edit Promotions"

            If _Status = "VIEW" Then

                gridPromotionsAdd.Columns(0).Visible = False

                lnkDeletePromo.Visible = False
                lnkEditPromo.Visible = False
                lnkAddPromo.Visible = False

            ElseIf _Status = "ADDNEW" Or _
                               _Status = "EDIT" Then

                If gridPromotionsAdd.Rows.Count = 0 Then

                    lnkDeletePromo.Visible = False
                    lnkEditPromo.Visible = False

                End If

            End If

        ElseIf strType = "SWP" Then

            'trEffectDate.Visible = False
            trExtendedUntil.Visible = False
            trCRFDatePeriod.Visible = True
            trCurrPromotions.Visible = False
            trPromotions.Visible = False
            trPromotionsVSLP.Visible = False
            trReason.Visible = False
            trMechanicsADD.Visible = False

            trAttachments.Visible = False

            trCRFDatePeriod.Visible = False
            txtRCDPTitle.Visible = False
            trPromoSeedAttachment.Visible = True
            linkSeedAttachment.Visible = True
            lblRCDPTitle.Visible = True

            trSwipestakesSeed.Visible = True
            trSwipestakesReseed.Visible = True

            lblRequstTypeDesc.Text = "Swipestakes Reseed"

            gridPromotionsAdd.EmptyDataText = "Edit Promotions"

            If _Status = "VIEW" Then

                gridPromotionsAdd.Columns(0).Visible = False

                lnkDeletePromo.Visible = False
                lnkEditPromo.Visible = False
                lnkAddPromo.Visible = False

            ElseIf _Status = "ADDNEW" Or _
                               _Status = "EDIT" Then

                If gridPromotionsAdd.Rows.Count = 0 Then

                    lnkDeletePromo.Visible = False
                    lnkEditPromo.Visible = False

                End If

            End If

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If

        trMemo1.Visible = False
        trMemo2.Visible = False
        trMemo3.Visible = False
        trMemo4.Visible = False
        trMemo5.Visible = False
        trMemo6.Visible = False
        trMemo7.Visible = False
        trMemo8.Visible = False
        trMemo9.Visible = False
        trMemo10.Visible = False
        trMemo11.Visible = False
        trMemo12.Visible = False
        trMemo13.Visible = False

        tbrowReviewedLine.Visible = False
        tbrowApprovedLine.Visible = False


        If Request("Filename") <> Nothing Then
            Dim fname As String
            fname = clsPromo.pathAttachment & "CR-" & clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString).ToString & "\" & Request("Filename")
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

        txtCancelEffectDate.Visible = boolValue
        txtExtendedUntil.Visible = boolValue
        txtCRFPeriodFrom.Visible = boolValue
        txtCRFPeriodTo.Visible = boolValue
        txtRCDPTitle.Visible = boolValue
        txtCancelReason.Visible = boolValue

    End Sub

    Private Sub Label_Properties(ByVal boolValue As Boolean)


        lblCancelEffectDate.Visible = boolValue
        lblExtendedUntil.Visible = boolValue
        lblCRFPeriodFrom.Visible = boolValue
        lblLabelCRFPeriod.Visible = boolValue
        lblCRFPeriodTo.Visible = boolValue
        lblRCDPTitle.Visible = boolValue
        lblCancelReason.Visible = boolValue

        'lblAttachments.Visible = boolValue

    End Sub

    Private Sub OtherControls_Properties(ByVal boolValue As Boolean)

        tblPromoVSLPControls.Visible = boolValue

    End Sub
    Private Sub Calendar_Properties(ByVal boolValue As Boolean)

        calCancelEffectDate.Visible = boolValue
        calExtendedUntil.Visible = boolValue
        calCRFPeriodFrom.Visible = boolValue
        calCRFPeriodTo.Visible = boolValue

    End Sub
    Private Sub LoadRequestInformation()

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

        If _Status = "ADDNEW" Then

            ' Generate temp request number
            lblCRID.Text = CreateTempCRID() 'clsPromo.LPAD(CreateTempCRID(), 5, "0").ToString
            lblRequestDate.Text = Today.ToLongDateString()

            txtCRFPeriodFrom.Text = ViewState("PromoPeriodFrom")
            txtCRFPeriodTo.Text = ViewState("PromoPeriodTo")


            If strType = "CCL" Then

                txtCancelEffectDate.Text = ""
                txtRCDPTitle.Text = "CANCELLATION: " & lblPromoTitle.Text
                txtCancelReason.Text = ""

            ElseIf strType = "ADD" Then

                txtRCDPTitle.Text = "ADDENDUM: " & lblPromoTitle.Text

            ElseIf strType = "EXT" Then

                txtRCDPTitle.Text = "EXTENSION: " & lblPromoTitle.Text

            ElseIf strType = "CRF" Then

                txtRCDPTitle.Text = "CLARIFICATION: " & lblPromoTitle.Text

            ElseIf strType = "SWP" Then

                txtRCDPTitle.Text = "RESEED: " & lblPromoTitle.Text
                lblRCDPTitle.Text = "RESEED: " & lblPromoTitle.Text

            Else

                Response.Redirect("InvalidAccess.aspx")

            End If



        ElseIf _Status = "VIEW" Or _Status = "EDIT" Then
            Dim taChangeRequests As New dsPromotionsTableAdapters.ChangeRequestsTableAdapter
            Dim dtChangeRequests As dsPromotions.ChangeRequestsDataTable
            Dim rowChangeRequests As dsPromotions.ChangeRequestsRow

            Dim xCRID = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)

            dtChangeRequests = taChangeRequests.GetRequestByID(xCRID)

            If dtChangeRequests.Rows.Count = 0 Then

                Response.Redirect("InvalidAccess.aspx")

            Else

                rowChangeRequests = dtChangeRequests.Rows(0)

                With rowChangeRequests

                    lblCRID.Text = .CRID ' clsPromo.LPAD(.CRID, 5, "0").ToString
                    lblRequestDate.Text = .RequestDate.ToLongDateString()
                    lblReqStatus.Text = .Status.ToString

                    If _Status = "VIEW" Then

                        lblExtendedUntil.Text = .PromoPeriodTo.ToLongDateString()

                        lblCRFPeriodFrom.Text = .PromoPeriodFrom.ToLongDateString()
                        lblCRFPeriodTo.Text = .PromoPeriodTo.ToLongDateString()

                        lblCancelEffectDate.Text = .EffectDate.ToLongDateString()
                        lblRCDPTitle.Text = .Title.ToString
                        lblCancelReason.Text = .Reason.ToString

                    ElseIf _Status = "EDIT" Then

                        txtExtendedUntil.Text = .PromoPeriodTo

                        txtCRFPeriodFrom.Text = .PromoPeriodFrom
                        txtCRFPeriodTo.Text = .PromoPeriodTo

                        txtCancelEffectDate.Text = .EffectDate
                        txtRCDPTitle.Text = .Title.ToString
                        txtCancelReason.Text = .Reason.ToString

                    End If


                    If strType = "EXT" Then
                        ' Revised dowcarpio20140225@smretailinc: due to erroneous display of dates in extension. change of saving of oldpromoperiodto from memos to changerequests(effectdate)
                        lblPromoPeriod.Text &= " to " & .EffectDate.ToLongDateString()
                    End If


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

                    lblCRRequestedBy.Text = clsPromo.formatParamValue(.RequestedBy.ToString()).ToString
                    lblCRApprovedBy.Text = clsPromo.formatParamValue(.InitApprovedBy.ToString()).ToString
                    lblCRMCIApprovedBy.Text = clsPromo.formatParamValue(.MCIApprovedBy.ToString()).ToString
                    lblCRRemarks.Text = clsPromo.formatParamValue(.Remarks.ToString).ToString
                    ViewState("UserCreated") = .UserCreated

                End With


            End If



        End If

    End Sub
    Private Sub LoadMemoInformation()
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString
        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)
        Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter()
        Dim dtMemos As dsPromotions.MemosDataTable
        Dim rowMemos As dsPromotions.MemosRow

        dtMemos = taMemos.GetMemoByID(hidMemoID.Value)

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

                If strType = "EXT" Then

                    'If lblReqStatus.Text = "Approved" Then

                    '    lblPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .OldPromoPeriodTo.ToLongDateString()

                    'Else

                    lblPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() '& " to " & IIf(.OldPromoPeriodTo.ToString <> "1/1/1900 12:00:00 AM", .OldPromoPeriodTo.ToLongDateString(), .PromoPeriodTo.ToLongDateString()).ToString

                    'End If

                Else

                    lblPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .PromoPeriodTo.ToLongDateString()


                End If

                ViewState("PromoPeriodFrom") = .PromoPeriodFrom.Date()
                ViewState("PromoPeriodTo") = .PromoPeriodTo.Date()

                lblBranches.Text = .Branches.ToString()
                litGuidelines.Text = Server.HtmlDecode(.Guidelines.ToString())
                lblPreparedBy.Text = .PreparedBy.ToString()

                lblReviewedBy.Text = .ReviewedBy.ToString()
                lblApprovedBy.Text = .ApprovedBy.ToString()

                tbrowReviewedLine.Visible = (lblReviewedBy.Text <> String.Empty)
                tbrowApprovedLine.Visible = (lblApprovedBy.Text <> String.Empty)

                'ViewState("RequestID") = .RequestID
                ViewState("OwnerGroup") = .OwnerGroup

                ' hide promo item for SBU Marketing, Credit and BCR
                If .GroupType = "SBU" Or _
                    .GroupType = "BCR" Or _
                    .GroupType = "CREDIT" Then

                    gridPromotions.Columns(1).Visible = False

                End If

            End With

        End If

    End Sub
    'Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

    '    Const nDescCol As Integer = 2
    '    Const nItemCol As Integer = 1
    '    Static rowPrevious As GridViewRow
    '    Static rowPrevItem As GridViewRow

    '    ' html decode promo description field in order to display properly
    '    If e.Row.RowIndex > -1 Then
    '        e.Row.Cells(nDescCol).Text = Server.HtmlDecode(e.Row.Cells(nDescCol).Text)
    '    End If

    '    If e.Row.RowIndex = 0 Then
    '        rowPrevious = e.Row
    '        rowPrevItem = e.Row
    '    End If

    '    If e.Row.RowIndex > 0 Then

    '        ' merge Description cells with same PromoID
    '        Dim lbPrev As Label = rowPrevious.FindControl("lblPromoID")
    '        Dim lbCurr As Label = e.Row.FindControl("lblPromoID")

    '        If lbCurr.Text = lbPrev.Text Then

    '            If rowPrevious.Cells(nDescCol).RowSpan < 2 Then
    '                'rowPrevious.Cells(0).RowSpan = 2
    '                rowPrevious.Cells(nDescCol).RowSpan = 2
    '            Else
    '                'rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
    '                rowPrevious.Cells(nDescCol).RowSpan = rowPrevious.Cells(nDescCol).RowSpan + 1
    '            End If

    '            'e.Row.Cells(0).Visible = False
    '            e.Row.Cells(nDescCol).Visible = False
    '        Else
    '            rowPrevious = e.Row
    '        End If

    '        If e.Row.Cells(nItemCol).Text = rowPrevItem.Cells(nItemCol).Text Then

    '            If rowPrevItem.Cells(nItemCol).RowSpan < 2 Then
    '                rowPrevItem.Cells(nItemCol).RowSpan = 2
    '            Else
    '                rowPrevItem.Cells(nItemCol).RowSpan = rowPrevItem.Cells(nItemCol).RowSpan + 1
    '            End If

    '            e.Row.Cells(nItemCol).Visible = False

    '        Else
    '            rowPrevItem = e.Row
    '        End If
    '    End If

    'End Sub

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

        strDir = clsPromo.pathAttachment & hidRequestID.Value
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""PromoRequest.aspx?FileName=" & Server.UrlEncode(file.Name.ToString) & """>" & file.Name.ToString & "</a> "
            Next

            If Len(strFiles) = 0 Then lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr


    End Sub

    Private Sub ShowAttachments_SWP()
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & hidRequestID.Value

        Dim pathTemp As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = hidRequestID.Value.ToString & "-Swipestakes"
        Dim reseedFolder As String = "\CR" & lblCRID.Text
        pathTemp = swipestakeSeedFolder & subFolderName & reseedFolder
        Directory.CreateDirectory(pathTemp)

        lblSeedFile.Text = ""

        If System.IO.Directory.Exists(pathTemp) Then
            Dim dir As New System.IO.DirectoryInfo(pathTemp)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblSeedFile.Text &= "<img src='Images/bullet green.gif' /><a href=""PromoRequest.aspx?FileName=" & Server.UrlEncode(file.Name.ToString) & """>" & file.Name.ToString & "</a> "
            Next

            If Len(strFiles) = 0 Then lblSeedFile.Text = ""
        End If

        imgbtnSeedDownload.Visible = tr
    End Sub

    Private Sub ShowAttachments_CR()
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & "CR-" & lblCRID.Text
        lblAttachments.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblAttachments.Text &= "<img src='Images/bullet green.gif' /><a href=""RCDPMemo.aspx?FileName=" & Server.UrlEncode(file.Name.ToString()) & "&CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text.ToString, SystemUser.EncryptKey.ToString) & "&MemoID=" & Request("MemoID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString & """>" & file.Name.ToString() & "</a> "
                'lblAttachments.Text &= "<img src='Images/bullet green.gif' /><a href=""RCDPMemo.aspx?FileName=" & Server.UrlEncode(file.Name.ToString()) & "&CRID=" & lblCRID.Text.ToString & "&MemoID=" & Request("MemoID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString & """>" & file.Name.ToString() & "</a> "
                'Response.Redirect("PromoRequest.aspx?DownLoad=" & "CR-" & hidRequestID.Value & ".zip" & "&Path=" & clsPromo.pathAttachment & "CR-" & hidRequestID.Value)

            Next

            If Len(strFiles) = 0 Then lblFiles.Text = ""
        End If

        imgbtnDownload2.Visible = tr


    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    '    If Not IsPostBack() Then



    '        LoadMemoInformation()

    '        trEditRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.Analyst And InStr("Approved; For Final Approval; For Review", lblStatus.Text) = 0)

    '        lnkForApproval.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.Reviewer And lblStatus.Text = "For Review")
    '        lnkApproveMemo.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover And lblStatus.Text = "For Final Approval")

    '        trReturn.Visible = (lnkForApproval.Visible Or lnkApproveMemo.Visible)

    '        ' override properties if start of promo is pass due
    '        'If (DateDiff(DateInterval.Day, Today(), ViewState("PromoPeriodFrom")) < 0) And (lblStatus.Text <> "Approved") Then
    '        '    lnkApproveMemo.Visible = False
    '        '    lnkForApproval.Visible = False

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

        Response.Redirect("RCDPMemo.aspx?MemoID=" & Request("MemoID").ToString & "&RequestID=" & Request("RequestID").ToString & "&CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("EDIT", SystemUser.EncryptKey.ToString))

    End Sub

    Protected Sub lnkPrintMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintMemo.Click

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

        ' ::ToDo:: flag to return to this module on close


        clsSession.FlagForPOSDisp = False


        If strType = "CCL" Then

            Response.Redirect("ViewMemo_Cancellation.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("EDIT", SystemUser.EncryptKey.ToString))

        ElseIf strType = "ADD" Then

            Response.Redirect("ViewMemo_Addendum.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("EDIT", SystemUser.EncryptKey.ToString))

        ElseIf strType = "EXT" Then

            Response.Redirect("ViewMemo_Cancellation.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("EDIT", SystemUser.EncryptKey.ToString))

        ElseIf strType = "CRF" Then

            Response.Redirect("ViewMemo_Cancellation.aspx?xmode=1&MemoID=" & hidMemoID.Value & "&CRID=" & Request("CRID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("EDIT", SystemUser.EncryptKey.ToString))

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If


    End Sub

    Protected Sub lnkReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReturn.Click

        'prompt user for reason
        lblPopTitle.Value = "Disapprove Request"
        clsSession.Message = "<b>Reason why this document is being returned:</b><br>"
        clsSession.Icon = "inputinquiry"
        ViewState("process") = "cancelrequest"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openinputbox('','');</script>")  ' calls cmdPopUpOK code

    End Sub

    Private Sub MergeSeedingData()
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As Data.SqlClient.SqlCommand
        sqlConn.Open()
        sqlCmd = New Data.SqlClient.SqlCommand
        sqlCmd.CommandText = "sp_MergeSeedingData"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4


        sqlCmd.Parameters.Add("@CRID", SqlDbType.Int)
        sqlCmd.Parameters("@CRID").Value = lblCRID.Text


        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = hidRequestID.Value


        sqlCmd.ExecuteNonQuery()

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()

        sqlConn = Nothing
        sqlCmd = Nothing
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

        ElseIf strType = "SWP" Then

            AuditRem = "Swipestakes Reseeding"

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If

        '-------------------------------------------------------

        If clsSession.DeleteStatus = "yes" Then

            Select Case ViewState("process").ToString

                Case "submit"   ' -- submit by requestor

                    Dim _CRID As String

                    Try

                        _CRID = clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString
                    Catch ex As Exception

                        _CRID = lblCRID.Text
                    End Try

                    Dim nNumLeadDays As Integer = 1 'ViewState("NumLeadDays")

                    ' less one day for extension requests
                    If strType = "EXT" Then
                        nNumLeadDays -= 1
                    End If

                    If strType = "SWP" Then
                    '    txtCancelEffectDate.Text = Today()
                        nNumLeadDays = 0
                    End If

                    If txtCancelEffectDate.Text = "" And lblCancelEffectDate.Text <> "" Then
                        txtCancelEffectDate.Text = lblCancelEffectDate.Text
                    End If

                    ' add a day if it is past cut-off time already
                    ' NBS::20200615 - adjust cut-off from 2pm to 11am
                    ' If (DateDiff(DateInterval.Day, Today(), CDate(lblCancelEffectDate.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (14 * 60)) Then
                    If txtCancelEffectDate.Text <> "" Then
                        If (DateDiff(DateInterval.Day, Today(), CDate(txtCancelEffectDate.Text)) = 1) Then
                            If Now.TimeOfDay.TotalMinutes > (11 * 60) Then
                                nNumLeadDays += 1
                            End If
                        End If

                        If DateDiff(DateInterval.Day, Today(), CDate(txtCancelEffectDate.Text)) < nNumLeadDays Then

                            ViewState("process") = "do_nothing"
                            lblPopTitle.Value = "Error"
                            clsSession.Icon = "error"
                            clsSession.Message = "Unable to approve and submit " & AuditRem & " request.<br /><br />" & _
                                                 "Date of effectivity does not comply with the minimum " & nNumLeadDays & "-day processing period.<br /><br />"

                            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','');</script>")     ' calls cmdDelete code

                            Exit Sub

                        End If

                    End If

                    

                    If SystemUser.UserGroupType = "CM" Then

                        ' skip MBU Approver if Character Merchandise group
                        clsPromo.CRStatus(_CRID, _
                                          hidMemoID.Value, _
                                         hidRequestID.Value, _
                                          Nothing, _
                                          clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                          "For Review", _
                                          Nothing, _
                                          SystemUser.UserID, _
                                          Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                          AuditRem & " request was submitted for review.", _
                                          "Promotion Transaction")

                    Else

                        'GetPromoRequestInfo(hidRequestID.Value)

                        'If ViewState("WorkFlowCode") = "" Then
                        '    ' Submit Request directly to MBU Approver
                        '    clsPromo.CRStatus(_CRID, _
                        '                      hidMemoID.Value, _
                        '                      hidRequestID.Value, _
                        '                      Nothing, _
                        '                      clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                        '                      "For MBU Approval", _
                        '                      Nothing, _
                        '                      SystemUser.UserID, _
                        '                      Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                        '                      AuditRem & " request was submitted to MBU approver.", _
                        '                      "Promotion Transaction")
                        'End If

                        'submit first to merchandising group head for approval

                        clsPromo.CRStatus(_CRID, _
                                          hidMemoID.Value, _
                                          hidRequestID.Value, _
                                          Nothing, _
                                          clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                          "For Mdsg/Group Head Approval", _
                                          Nothing, _
                                          SystemUser.UserID, _
                                          Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                          AuditRem & " request was submitted to Msdg Head approver.", _
                                          "Promotion Transaction")

                    End If

                Case "approved request"     'approval of Request Approver and/or Request Reviewer

                    ' get promotion request info
                    GetPromoRequestInfo(hidRequestID.Value)

                    ' check lead time for cancellation requests
                    If strType = "CCL" Then

                        Dim nNumLeadDays As Integer = 1 'ViewState("NumLeadDays")

                        ' add a day if it is past cut-off time already
                        ' NBS::20200615 - adjust cut-off from 2pm to 11am
                        ' If (DateDiff(DateInterval.Day, Today(), CDate(lblCancelEffectDate.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (14 * 60)) Then
                        If (DateDiff(DateInterval.Day, Today(), CDate(lblCancelEffectDate.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (11 * 60)) Then
                            nNumLeadDays += 1
                        End If

                        If DateDiff(DateInterval.Day, Today(), CDate(lblCancelEffectDate.Text)) < nNumLeadDays Then

                            ViewState("process") = "do_nothing"
                            lblPopTitle.Value = "Error"
                            clsSession.Icon = "error"
                            clsSession.Message = "Unable to approve and submit " & AuditRem & " request.<br /><br />" & _
                                                 "Date of effectivity does not comply with the minimum " & nNumLeadDays & "-day processing period.<br /><br />"

                            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','');</script>")     ' calls cmdDelete code

                            Exit Sub

                        End If

                    End If

                    If strType = "SWP" Then

                        Dim nNumLeadDays As Integer = 1 'ViewState("NumLeadDays")
                        If (DateDiff(DateInterval.Day, Today(), CDate(lblCancelEffectDate.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (11 * 60)) Then
                            nNumLeadDays += 1
                        End If


                        If DateDiff(DateInterval.Day, Today(), CDate(lblCancelEffectDate.Text)) < nNumLeadDays Then

                            ViewState("process") = "do_nothing"
                            lblPopTitle.Value = "Error"
                            clsSession.Icon = "error"
                            clsSession.Message = "Unable to approve and submit " & AuditRem & " request.<br /><br />" & _
                                                 "Date of effectivity does not comply with the minimum " & nNumLeadDays & "-day processing period.<br /><br />"

                            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','');</script>")     ' calls cmdDelete code

                            Exit Sub

                        End If


                    End If
                    ' check if promotype is SMACdeals
                    If IsSmacDealsPromo() Then

                        If lblReqStatus.Text = "For MBU Approval" Or lblReqStatus.Text = "For Mdsg/Group Head Approval" Then

                            ' Submit Request to MCI Approver
                            clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                           hidMemoID.Value, _
                                            hidRequestID.Value, _
                                            Nothing, _
                                            clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                            "For MCI Approval", _
                                            Nothing, _
                                            SystemUser.UserID, _
                                            Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                            AuditRem & " request was approved. (MBU approver)", _
                                            "Promotion Transaction")
                        End If

                    Else

                        If SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover Then

                            'If ViewState("WorkFlowCode") = "" Then
                            '    ' normal approval flow - MPD Review
                            '    clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                            '                      hidMemoID.Value, _
                            '                      hidRequestID.Value, _
                            '                      Nothing, _
                            '                      clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                            '                      "For Review", _
                            '                      Nothing, _
                            '                      SystemUser.UserID, _
                            '                      Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                            '                      AuditRem & " request was approved.", _
                            '                      "Promotion Transaction")
                            'End If

                            ' all cancellation/extension of promo types
                            ' automatically approve request
                            clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                              hidMemoID.Value, _
                                              hidRequestID.Value, _
                                              Nothing, _
                                              clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                              "Approved", _
                                              Nothing, _
                                              SystemUser.UserID, _
                                              Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                              AuditRem & " request was approved.", _
                                              "Promotion Transaction")

                            MergeSeedingData()



                        Else

                            'If SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer Then

                            If (SystemUser.UserGroupType = "SBU") Then
                                clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                                  hidMemoID.Value, _
                                                  hidRequestID.Value, _
                                                  Nothing, _
                                                  clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                                  "For BU Head Approval", _
                                                  Nothing, _
                                                  SystemUser.UserID, _
                                                  Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                                  AuditRem & " request was submitted to BU Head approver.", _
                                                  "Promotion Transaction")
                            Else
                                ' user is request reviewer - submit for final approval
                                clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                                  hidMemoID.Value, _
                                                  hidRequestID.Value, _
                                                  Nothing, _
                                                  clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                                  "For MBU Approval", _
                                                  Nothing, _
                                                  SystemUser.UserID, _
                                                  Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                                  AuditRem & " request was approved.", _
                                                  "Promotion Transaction")
                            End If

                        End If

                    End If


                Case "cancelrequest"

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
                                      AuditRem & " request was returned to requestor.", _
                                      "Promotion Transaction")

                Case "Delete"

                    ' Delete request
                    clsPromo.CRStatus(clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString).ToString, _
                                        hidMemoID.Value, _
                                       hidRequestID.Value, _
                                        Nothing, _
                                        clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.EncryptKey.ToString).ToString, _
                                        "Deleted", _
                                        Server.HtmlEncode(hidinputbox.Value.Replace("'", "''")), _
                                        SystemUser.UserID, _
                                        Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                        AuditRem & " request was deleted.", _
                                        "Promotion Transaction")

                Case "delete_promo" ' grid event

                    If clsSession.DeleteStatus = "yes" Then

                        For Each row As GridViewRow In gridPromotions.Rows

                            Dim cb As CheckBox = row.FindControl("chkRowSel")

                            If cb IsNot Nothing AndAlso cb.Checked Then

                                Dim lb As Label = row.FindControl("lblPromoID")

                                ' delete promotion header, promo branches, and details
                                sqldsData.DeleteCommand = "DELETE FROM CRPromoDetails WHERE PromoID = " & lb.Text
                                sqldsData.Delete()
                                sqldsData.DeleteCommand = "DELETE FROM CRPromoBranch WHERE PromoID = " & lb.Text
                                sqldsData.Delete()
                                sqldsData.DeleteCommand = "DELETE FROM CRPromotions WHERE PromoID = " & lb.Text
                                sqldsData.Delete()


                                gridPromotions.DataBind()

                            End If

                        Next

                        ' Revised dowcarpio11102012@smretailinc: no branch description indicated in promo request. Delete branch description upon deletion of all promotions.
                        If gridPromotions.Rows.Count = 0 Then

                            sqldsData.UpdateCommand = "UPDATE ChangeRequests SET  Branches = NULL WHERE CRID = 0" & clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString)
                            sqldsData.Update()

                        End If

                    End If

                Case Else

                    ' do nothing

            End Select

            Response.Redirect("RCDPReqList.aspx?v1=" & Request("v1").ToString)

        Else

            Server.Transfer("RCDPMemo.aspx?CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString).ToString & "&MemoID=" & Request("MemoID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString))

        End If

        GC.Collect()

    End Sub

    Protected Function GetPromoRequestInfo(ByVal nRequestID As Long) As Boolean

        Dim drRow As DataRow = Nothing
        Dim strQuery As String

        Dim lResult As Boolean = False

        strQuery = "SELECT COALESCE(WorkFlowCode, '') AS sWorkFlowCode, OwnerGroup " & _
                    "FROM PromoRequests " & _
                    "WHERE RequestID = 0" & nRequestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
            ViewState("WorkFlowCode") = drRow("sWorkFlowCode")
            ' ViewState("") = drRow("NumLeadDays")
            'ViewState("OwnerGroup") = drRow("OwnerGroup")
            lResult = True
        End If

        Return lResult

    End Function

    Protected Sub tdSubmitRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSubmitRequest.Click

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString

        'temp disabled for dev
        'If strType = "CCL" And (Today().ToString("ddd").ToString = "Thu" Or _
        '        Today().ToString("ddd").ToString = "Fri" Or _
        '        Today().ToString("ddd").ToString = "Sat" Or _
        '        Today().ToString("ddd").ToString = "Sun") Then


        '    ViewState("proc") = "do_nothing"
        '    lblPopTitle.Value = "Error"
        '    clsSession.Icon = "error"
        '    clsSession.Message = "Unable to create cancellation request.<br /><br />" & _
        '                         "Processing Days: Monday, Tuesday and Wednesday only.<br /><br />"

        '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','');</script>")

        '    Exit Sub

        'End If

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

        If _Status = "ADDNEW" Or _Status = "EDIT" Then

            If SaveInfo("Draft", lnkSubmitRequest) = False Then Exit Sub

        End If


        'If strType = "ADD" Then

        '    'If gridPromotionsAdd.Rows.Count = 0 Then

        '    '    blistErrorMsg.Items.Add("Please create new promotion(s).")
        '    '    Exit Sub

        '    'End If

        '    If litMechanicsOld.Text = "" Then

        '        blistErrorMsg.Items.Add("Mechanics (Old) is required.")
        '        Exit Sub

        '    End If

        '    If litMechanicsNew.Text = "" Then

        '        blistErrorMsg.Items.Add("Mechanics (New) is required.")
        '        Exit Sub

        '    End If

        'End If

        ' prompt for confirmation
        clsSession.Message = "Are you sure you want submit this request?"
        clsSession.Icon = "inquiry"
        lblPopTitle.Value = "Confirm Action"
        ViewState("process") = "submit"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Protected Sub lnkApproveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkApprove.Click

        ' prompt for confirmation
        clsSession.Message = "Do you wish to approve this request?"
        clsSession.Icon = "inquiry"
        lblPopTitle.Value = "Confirm Approval"
        ViewState("process") = "approved request"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    'Protected Sub cmdRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRedirect.Click

    '    If clsSession.DeleteStatus = "yes" Then

    '        Select Case ViewState("process").ToString()

    '            Case "reviewed_memo"
    '                ' mark request as reviewed and update status
    '                sqldsData.UpdateCommand = "UPDATE Memos SET Status = 'For Final Approval', ReviewedBy = '" & SystemUser.UserSignName & "', ReviewerPos = '" & SystemUser.UserSignPosition & "', ReviewDate = GETDATE() WHERE MemoID = " & hidmemoid.Value
    '                sqldsData.Update()

    '                ' update promo request counterpart's status
    '                sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'For Final Approval' WHERE RequestID = " & CInt(hidRequestID.Value)
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
    '                                          "ApproveDate = GETDATE() WHERE MemoID = " & hidmemoid.Value
    '                sqldsData.Update()

    '                ' assign promo event code if promotion is "SMAC Deals"
    '                CreateNewPromoEventCode(8000, 9999)
    '                ' display info on guidelines
    '                AppendExtraDetailsToGuidelines()

    '                ' update promo request counterpart's status
    '                sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'Approved' WHERE RequestID = " & CInt(hidRequestID.Value)
    '                sqldsData.Update()

    '                ' update audit trail
    '                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), _
    '                                          hidRequestID.Value, "Promotional Memo approved.", _
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



    Protected Sub lnkDraftSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDraftSave.Click

        SaveInfo("Draft", lnkDraftSave)

    End Sub

    Private Function SaveInfo(ByVal strStatus As String, ByVal ActionButton As LinkButton) As Boolean


        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey).ToString
        Dim nRushLeadDays As Integer = 5
        Dim ctr As Integer = 0

        blistErrorMsg.Items.Clear()

        If strType = "CCL" Then


            If Trim(txtRCDPTitle.Text) = "" Then

                blistErrorMsg.Items.Add("Cancellation title is empty.")

            End If

            If Not IsDate(txtCancelEffectDate.Text) Then

                blistErrorMsg.Items.Add("Blank or invalid effectivity date format.") ' invalid date format

            Else

                Try

                    If CDate(ViewState("PromoPeriodFrom").ToString) = CDate(ViewState("PromoPeriodTo").ToString) Then

                        'If (CDate(txtCancelEffectDate.Text) = CDate(ViewState("PromoPeriodFrom").ToString)) Or _
                        '(CDate(txtCancelEffectDate.Text) = CDate(ViewState("PromoPeriodTo").ToString)) Then
                        If CDate(txtCancelEffectDate.Text) < CDate(ViewState("PromoPeriodFrom").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be earlier than " & Format(CDate(ViewState("PromoPeriodFrom")), "MMMM dd, yyyy"))

                        ElseIf CDate(txtCancelEffectDate.Text) > CDate(ViewState("PromoPeriodTo").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be later than " & Format(CDate(ViewState("PromoPeriodTo").ToString), "MMMM dd, yyyy"))

                        End If

                    Else

                        If CDate(txtCancelEffectDate.Text) < CDate(ViewState("PromoPeriodFrom").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be earlier than " & Format(CDate(ViewState("PromoPeriodFrom")), "MMMM dd, yyyy"))

                        ElseIf CDate(txtCancelEffectDate.Text) > CDate(ViewState("PromoPeriodTo").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be later than " & Format(CDate(ViewState("PromoPeriodTo").ToString), "MMMM dd, yyyy")) '.AddDays(-2)

                        ElseIf CDate(txtCancelEffectDate.Text) < CDate(Today()) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be earlier than the date today.")

                        ElseIf CDate(txtCancelEffectDate.Text) < CDate(Today().AddDays(1)) Then ' Revised dowcarpio20140128@smretailinc: temp allow 1 day processing

                            blistErrorMsg.Items.Add("Date of effectivity does not comply with the minimum 2-day processing period.")

                        End If
                    End If
                Catch ex As Exception

                    blistErrorMsg.Items.Add("Blank or invalid promo start/end date format.")

                End Try

            End If

            If Trim(txtCancelReason.Text) = "" Then

                blistErrorMsg.Items.Add("Please provide a cancellation reason.")

            End If


        ElseIf strType = "EXT" Then

            If Not IsDate(txtExtendedUntil.Text) Then

                blistErrorMsg.Items.Add("Blank or invalid extension date format.") ' invalid date format
                ctr += 1

            Else

                If CDate(txtExtendedUntil.Text) < CDate(ViewState("PromoPeriodFrom").ToString) Then

                    blistErrorMsg.Items.Add("Extension date must not be earlier than the start date.")
                    ctr += 1
                ElseIf CDate(txtExtendedUntil.Text) <= CDate(ViewState("PromoPeriodTo").ToString) Then

                    blistErrorMsg.Items.Add("Extension date must not be earlier than or equal to the current end date.")
                    ctr += 1

                End If

            End If


        ElseIf strType = "ADD" Then

            If litMechanicsNew.Text = "" Then

                blistErrorMsg.Items.Add("Mechanics (New) is required.")
                Exit Function

            End If

        ElseIf strType = "SWP" Then

            If lblSeedFile.Text = "" Then

                blistErrorMsg.Items.Add("Reseeding attachment is required.")
                Exit Function

            End If


            If Not IsDate(txtCancelEffectDate.Text) Then

                blistErrorMsg.Items.Add("Blank or invalid effectivity date format.") ' invalid date format

            Else

                Try

                    If CDate(ViewState("PromoPeriodFrom").ToString) = CDate(ViewState("PromoPeriodTo").ToString) Then

                        'If (CDate(txtCancelEffectDate.Text) = CDate(ViewState("PromoPeriodFrom").ToString)) Or _
                        '(CDate(txtCancelEffectDate.Text) = CDate(ViewState("PromoPeriodTo").ToString)) Then
                        If CDate(txtCancelEffectDate.Text) < CDate(ViewState("PromoPeriodFrom").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be earlier than " & Format(CDate(ViewState("PromoPeriodFrom")), "MMMM dd, yyyy"))

                        ElseIf CDate(txtCancelEffectDate.Text) > CDate(ViewState("PromoPeriodTo").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be later than " & Format(CDate(ViewState("PromoPeriodTo").ToString), "MMMM dd, yyyy"))

                        End If

                    Else

                        If CDate(txtCancelEffectDate.Text) < CDate(ViewState("PromoPeriodFrom").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be earlier than " & Format(CDate(ViewState("PromoPeriodFrom")), "MMMM dd, yyyy"))

                        ElseIf CDate(txtCancelEffectDate.Text) > CDate(ViewState("PromoPeriodTo").ToString) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be later than " & Format(CDate(ViewState("PromoPeriodTo").ToString), "MMMM dd, yyyy")) '.AddDays(-2)

                        ElseIf CDate(txtCancelEffectDate.Text) < CDate(Today()) Then

                            blistErrorMsg.Items.Add("Effectivity date must not be earlier than the date today.")

                        ElseIf CDate(txtCancelEffectDate.Text) < CDate(Today().AddDays(1)) Then ' Revised dowcarpio20140128@smretailinc: temp allow 1 day processing

                            blistErrorMsg.Items.Add("Date of effectivity does not comply with the minimum 2-day processing period.")

                        End If
                    End If
                Catch ex As Exception

                    blistErrorMsg.Items.Add("Blank or invalid promo start/end date format.")

                End Try

            End If


        End If

        If blistErrorMsg.Items.Count > 0 Then

            Exit Function

        End If

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)

        If SaveRecord(strStatus) Then

            If ActionButton.ClientID = lnkDraftSave.ClientID And (_Status = "ADDNEW" Or _Status = "EDIT") Then

                'Server.Transfer("RCDPReqList.aspx?v1=" & Request("v1").ToString)
                Server.Transfer("RCDPMemo.aspx?CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString).ToString & "&MemoID=" & Request("MemoID").ToString & "&RequestID=" & Request("RequestID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString))

            Else

                ' do nothing

            End If

            SaveInfo = True

        Else

            SaveInfo = False

        End If

    End Function

    Private Function CreateTempCRID() As String

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString
        Dim _proc As String
        Try
            _proc = clsEncryptDecrypt.DecryptText(Request("proc").ToString, SystemUser.EncryptKey.ToString)
        Catch ex As Exception
            _proc = "No"
        End Try

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand

        Try

            sqlConn.Open()
            sqlCmd = New SqlCommand

            If _Status = "ADDNEW" Then

                sqlCmd.CommandText = "PMS_P_ChangeRequests_CreateID"

            End If


            sqlCmd.Connection = sqlConn
            sqlCmd.CommandTimeout = 0
            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@MemoID", SqlDbType.Int)
            sqlCmd.Parameters("@MemoID").Value = hidMemoID.Value

            sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
            sqlCmd.Parameters("@RequestID").Value = hidRequestID.Value


            sqlCmd.Parameters.Add("@CRID", SqlDbType.Int)
            sqlCmd.Parameters("@CRID").Value = clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString)

            sqlCmd.Parameters.Add("@RequestDate", SqlDbType.DateTime)
            sqlCmd.Parameters("@RequestDate").Value = Now.Date

            sqlCmd.Parameters.Add("@RequestType", SqlDbType.VarChar)
            sqlCmd.Parameters("@RequestType").Value = strType

            sqlCmd.Parameters.Add("@RequestedBy", SqlDbType.VarChar)
            sqlCmd.Parameters("@RequestedBy").Value = SystemUser.UserSignName.ToString

            sqlCmd.Parameters.Add("@RequesterPOS", SqlDbType.VarChar)
            sqlCmd.Parameters("@RequesterPOS").Value = SystemUser.UserSignPosition.ToString

            sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar)
            sqlCmd.Parameters("@Status").Value = "Temp"

            sqlCmd.Parameters.Add("@Proc", SqlDbType.VarChar)
            sqlCmd.Parameters("@Proc").Value = _proc

            sqlCmd.Parameters.Add("@OwnerGroup", SqlDbType.SmallInt)
            sqlCmd.Parameters("@OwnerGroup").Value = SystemUser.UserGroupID.ToString

            sqlCmd.Parameters.Add("@UserID", SqlDbType.SmallInt)
            sqlCmd.Parameters("@UserID").Value = SystemUser.UserID.ToString

            'sqlCmd.ExecuteNonQuery()
            Return sqlCmd.ExecuteScalar

        Catch ex As Exception

            blistErrorMsg.Items.Add(ex.Message)
            Return ""

        End Try

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()

    End Function

    Private Function SaveRecord(ByVal strStatus) As Boolean

        Dim _Status As String = clsEncryptDecrypt.DecryptText(Request("stat").ToString, SystemUser.EncryptKey.ToString)
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand

        Dim _RCDPTitle As String = txtRCDPTitle.Text.ToUpper
        Dim _CancelEffectDate As String = IIf(txtCancelEffectDate.Text = "", Nothing, txtCancelEffectDate.Text)
        Dim _CRFPeriodFrom As String = txtCRFPeriodFrom.Text
        Dim _CRFPeriodTo As String = txtCRFPeriodTo.Text
        Dim _CancelReason As String = txtCancelReason.Text

        Try

            sqlConn.Open()
            sqlCmd = New SqlCommand

            If _Status = "ADDNEW" Then

                sqlCmd.CommandText = "PMS_P_ChangeRequests_I"

            ElseIf _Status = "EDIT" Then

                sqlCmd.CommandText = "PMS_P_ChangeRequests_U"

                'ElseIf _Status = "VIEW" Then

                'sqlCmd.CommandText = "PMS_P_ChangeRequests_U"

                '_RCDPTitle = lblRCDPTitle.Text.ToUpper
                '_CancelEffectDate = IIf(lblCancelEffectDate.Text = "", Nothing, lblCancelEffectDate.Text)
                '_CRFPeriodFrom = lblCRFPeriodFrom.Text
                '_CRFPeriodTo = lblCRFPeriodTo.Text
                '_CancelReason = lblCancelReason.Text

            End If


            _RCDPTitle = txtRCDPTitle.Text.ToUpper
            _CancelEffectDate = IIf(txtCancelEffectDate.Text = "", Nothing, txtCancelEffectDate.Text)
            _CRFPeriodFrom = txtCRFPeriodFrom.Text

            If strType = "EXT" Then

                'Added dowcarpio20131204@smretail: to include date from due to error encountered on Memo MPD-VP
                _CRFPeriodFrom = ViewState("PromoPeriodFrom").ToString
                _CRFPeriodTo = txtExtendedUntil.Text

            ElseIf strType = "ADD" Then

                _CRFPeriodFrom = txtCRFPeriodFrom.Text
                _CRFPeriodTo = txtCRFPeriodTo.Text

            End If

            _CancelReason = txtCancelReason.Text

            sqlCmd.Connection = sqlConn
            sqlCmd.CommandTimeout = 0
            sqlCmd.CommandType = CommandType.StoredProcedure


            sqlCmd.Parameters.Add("@MemoID", SqlDbType.Int)
            sqlCmd.Parameters("@MemoID").Value = hidMemoID.Value

            sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
            sqlCmd.Parameters("@RequestID").Value = hidRequestID.Value

            sqlCmd.Parameters.Add("@CRID", SqlDbType.Int)
            sqlCmd.Parameters("@CRID").Value = lblCRID.Text

            sqlCmd.Parameters.Add("@RequestDate", SqlDbType.DateTime)
            sqlCmd.Parameters("@RequestDate").Value = Now.Date

            sqlCmd.Parameters.Add("@RequestType", SqlDbType.VarChar)
            sqlCmd.Parameters("@RequestType").Value = strType

            sqlCmd.Parameters.Add("@Title", SqlDbType.VarChar)
            sqlCmd.Parameters("@Title").Value = _RCDPTitle

            sqlCmd.Parameters.Add("@EffectDate", SqlDbType.SmallDateTime)
            sqlCmd.Parameters("@EffectDate").Value = _CancelEffectDate

            sqlCmd.Parameters.Add("@PromoPeriodFrom", SqlDbType.SmallDateTime)
            sqlCmd.Parameters("@PromoPeriodFrom").Value = _CRFPeriodFrom

            sqlCmd.Parameters.Add("@PromoPeriodTo", SqlDbType.SmallDateTime)
            sqlCmd.Parameters("@PromoPeriodTo").Value = _CRFPeriodTo

            sqlCmd.Parameters.Add("@Reason", SqlDbType.VarChar)
            sqlCmd.Parameters("@Reason").Value = _CancelReason

            sqlCmd.Parameters.Add("@MechanicsOld", SqlDbType.Text)
            sqlCmd.Parameters("@MechanicsOld").Value = Server.HtmlEncode(litMechanicsOld.Text)

            sqlCmd.Parameters.Add("@MechanicsNew", SqlDbType.Text)
            sqlCmd.Parameters("@MechanicsNew").Value = Server.HtmlEncode(litMechanicsNew.Text)

            sqlCmd.Parameters.Add("@RequestedBy", SqlDbType.VarChar)
            sqlCmd.Parameters("@RequestedBy").Value = SystemUser.UserSignName.ToString

            sqlCmd.Parameters.Add("@RequesterPOS", SqlDbType.VarChar)
            sqlCmd.Parameters("@RequesterPOS").Value = SystemUser.UserSignPosition.ToString

            sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar)
            sqlCmd.Parameters("@Status").Value = strStatus.ToString

            sqlCmd.Parameters.Add("@OwnerGroup", SqlDbType.SmallInt)
            sqlCmd.Parameters("@OwnerGroup").Value = SystemUser.UserGroupID.ToString

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

    Protected Sub lnkCreateMemoDraft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCreateMemoDraft.Click
        Response.Redirect("RCDPMemoView.aspx?MemoID=" & Request("MemoID") & "&RequestID=" & Request("RequestID") & "&CRID=" & Request("CRID") & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("EDIT", SystemUser.EncryptKey.ToString))
    End Sub
    Private Function IsSmacDealsPromo() As Boolean

        sqldsData.SelectCommand = "SELECT TOP 1 PromoID FROM Promotions WHERE RequestID = 0" & hidRequestID.Value & " AND PromoTypeID = 48"

        Dim dvPromos As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        IsSmacDealsPromo = (dvPromos.Table.Rows.Count() > 0)

        'dvPromos.Dispose()

    End Function

    Protected Sub lnkattachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkattachment.Click

        ValidateAttachmentRequired(sender, e)

        ' Added dowcarpio01102013@smretailinc: added new parameter to determine the filetype of the attachments :cr :regular request/change request
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openAttachment('cr');</script>")
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Try
            Dim tr As Boolean = False
            Dim strDir As String
            Dim strFiles As String
            strFiles = ""
            'strDir = clsPromo.pathAttachment &  hidRequestID.Value
            strDir = clsPromo.pathAttachment & "CR-" & lblCRID.Text

            'Dim di As New System.IO.DirectoryInfo(strDir)
            lblAttachments.Text = ""
            If (System.IO.Directory.Exists(strDir)) Then
                Dim dir As New System.IO.DirectoryInfo(strDir)
                Dim files As System.IO.FileInfo() = dir.GetFiles()
                For Each file As System.IO.FileInfo In files
                    tr = True
                    strFiles = strFiles & file.Name.ToString & ","
                    lblAttachments.Text &= "<img src='Images/bullet green.gif' /><a href='RCDPMemo.aspx?FileName=" & file.Name.ToString & "'>" & file.Name.ToString & "</a> "
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
                'litResult.Text &= "<a href='POSscreen.aspx?DbName=" & NewDBFname & "'>" & NewDBFname.ToUpper() & ".DBF</a><br>"
                'lblFiles.Text &= "<a href='PromoDetailsEntryDepartmental.aspx?FileName=" & Left(strFiles, Len(strFiles) - 1) & "'>" & Left(strFiles, Len(strFiles) - 1) & "</a><br>"
                'Left(strFiles, Len(strFiles) - 1)
            Else
                lblFiles.Text = ""
            End If
            imgbtnDownload2.Visible = tr
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub

    'Private Function validateattachment() As Int32
    '    Dim sqlConn As SqlConnection
    '    sqlConn = New SqlConnection(clsPromo.SQLConnString())
    '    Dim sqlCmd As SqlCommand
    '    sqlConn.Open()
    '    sqlCmd = New SqlCommand
    '    sqlCmd.CommandText = "USP_ValidateAttachment"
    '    sqlCmd.Connection = sqlConn
    '    sqlCmd.CommandTimeout = 0
    '    sqlCmd.CommandType = 4

    '    sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
    '    sqlCmd.Parameters("@RequestID").Value = hidRequestID.Value
    '    validateattachment = sqlCmd.ExecuteScalar()

    '    ' Added dowcarpio08232012@smretailinc: close and dispose connection
    '    If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
    '    sqlConn = Nothing
    '    sqlCmd = Nothing

    '    GC.Collect()
    'End Function

    Private Sub ValidateAttachmentRequired(ByVal sender As Object, ByVal e As System.EventArgs)

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")
        lnkattachment.Visible = True
        'If cboPromoType.SelectedValue > -1 Then

        'lnkattachment.Visible = True

        If lnkattachment.Visible Then

            Dim f As New IO.FileInfo(clsPromo.pathAttachment & "CR-" & lblCRID.Text)

            If Not f.Exists Then

                clsSession.AttachmentPath = clsPromo.pathAttachment & "CR-" & lblCRID.Text
                Directory.CreateDirectory(clsSession.AttachmentPath)

            End If

            btnDownload_Click(sender, e)

        End If
        'End If

    End Sub



    Protected Sub imgbtnDownload2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload2.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & "CR-" & lblCRID.Text, 1)
        Response.Redirect("PromoRequest.aspx?DownLoad=" & "CR-" & lblCRID.Text & ".zip" & "&Path=" & clsPromo.pathAttachment & "CR-" & lblCRID.Text)
    End Sub

    Protected Sub trDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDelete.Click
        'clsSession.Icon = "inquiry"
        'clsSession.Message = "Do you wish to delete this request draft?"
        'lblPopTitle.Value = "Delete"
        'ViewState("process") = "Delete"
        'ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")    ' calls cmdDelete code

        'prompt user for reason
        lblPopTitle.Value = "Delete Request"
        clsSession.Message = "<b>Do you wish to delete this request draft?</b><br>"
        clsSession.Icon = "inputinquiry"
        ViewState("process") = "Delete"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openinputbox('','');</script>")  ' calls cmdPopUpOK code

    End Sub

    Protected Sub lnkAddPromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddPromo.Click

        ' include credit in the condition :new group type
        If SystemUser.UserGroupType = "SACI" Or _
            SystemUser.UserGroupType = "SBU" Or _
            SystemUser.UserGroupType = "CREDIT" Then

            Server.Transfer("PromoRequestPreview.aspx?DocAttrib=Active")

        Else

            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openChoice();</script>")

        End If
    End Sub

    Protected Sub bclick_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bclick.Click
        Server.Transfer("RCDPMemoDepartmental.aspx?MemoID=" & Request("MemoID") & "&RequestID=" & Request("RequestID") & "&CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString)
    End Sub

    Protected Sub standard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles standard.Click
        clsSession.CurrPromoID = 0
        Server.Transfer("RCDPStandardPromoEntry.aspx?MemoID=" & Request("MemoID") & "&RequestID=" & Request("RequestID") & "&CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString)
    End Sub

    Protected Sub gridPromotionsAdd_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotionsAdd.RowDataBound
        Const nCol As Integer = 4
        Static rowPrevious As GridViewRow

        ' decode promo description field in order to display properly


        If e.Row.RowIndex > -1 Then
            'rowPrevious.Cells(nCol).Text = "<a href='PromoEntry.aspx?PromoID=" & lb.Text & "'>" & rowPrevious.Cells(nCol).Text & "</a>"

            Dim b As String = e.Row.Cells(nCol).Text
            e.Row.Cells(nCol).Text = Server.HtmlDecode(e.Row.Cells(nCol).Text)

            Dim chkRowSel As CheckBox = e.Row.Cells(0).FindControl("chkRowSel")
            chkRowSel.Attributes("OnClick") = "javascript:return checkUncheckOther('" & chkRowSel.ClientID & "',this.form);"


        End If

        If e.Row.RowIndex = 0 Then rowPrevious = e.Row

        If e.Row.RowIndex > 0 Then

            ' merge Description cells with same PromoID
            Dim lbPrev As Label = rowPrevious.FindControl("lblPromoID")
            Dim lbCurr As Label = e.Row.FindControl("lblPromoID")

            If lbCurr.Text = lbPrev.Text Then

                If rowPrevious.Cells(nCol).RowSpan < 2 Then
                    rowPrevious.Cells(0).RowSpan = 2
                    rowPrevious.Cells(nCol).RowSpan = 2
                Else
                    rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
                    rowPrevious.Cells(nCol).RowSpan = rowPrevious.Cells(nCol).RowSpan + 1
                End If

                e.Row.Cells(0).Visible = False
                e.Row.Cells(nCol).Visible = False
            Else
                rowPrevious = e.Row
            End If

        End If

    End Sub


    'Private Function formatgridPromotionsAdd() As DataTable

    '    If gridPromotionsAdd.Rows.Count > 0 Then

    '        For x As Int16 = 0 To CType(gridPromotionsAdd.Rows.Count - 1, Short)

    '            Dim chkItem As CheckBox = CType(gridPromotionsAdd.Rows.Item(x).Cells(0).FindControl("chkItem"), CheckBox)
    '            chkItem.Attributes("OnClick") = "javascript:return changeCheckAllAndProceedButton('" & chkAll.ClientID & "','chkItem','" & btnEditSave.ClientID & "',this.form);"

    '        Next

    '    End If

    'End Function

    Protected Sub lnkDeletePromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeletePromo.Click
        lblPopTitle.Value = "Promotion"
        clsSession.Message = "Delete selected promotions?"
        clsSession.Icon = "inquiry"
        ViewState("proc") = "delete"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor2('','');</script>")
    End Sub

    Protected Sub cmdPopUpOK2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK2.Click
        Select Case ViewState("proc").ToString
            Case "delete"

                If clsSession.DeleteStatus = "yes" Then

                    For Each row As GridViewRow In gridPromotionsAdd.Rows

                        Dim cb As CheckBox = row.FindControl("chkRowSel")

                        If cb IsNot Nothing AndAlso cb.Checked Then

                            Dim lb As Label = row.FindControl("lblPromoID")

                            ' delete promotion header, promo branches, and details
                            sqldsPromosAdd.DeleteCommand = "DELETE FROM CRPromoDetails WHERE PromoID = " & lb.Text
                            sqldsPromosAdd.Delete()
                            sqldsPromosAdd.DeleteCommand = "DELETE FROM CRPromoBranch WHERE PromoID = " & lb.Text
                            sqldsPromosAdd.Delete()
                            sqldsPromosAdd.DeleteCommand = "DELETE FROM CRPromotions WHERE PromoID = " & lb.Text
                            sqldsPromosAdd.Delete()


                            gridPromotionsAdd.DataBind()

                        End If

                    Next

                    ' Revised dowcarpio11102012@smretailinc: no branch description indicated in promo request. Delete branch description upon deletion of all promotions.
                    If gridPromotionsAdd.Rows.Count = 0 Then

                        sqldsPromosAdd.UpdateCommand = "UPDATE ChangeRequests SET  Branches = NULL WHERE CRID = 0" & clsEncryptDecrypt.DecryptText(Request("CRID"), SystemUser.EncryptKey.ToString)
                        sqldsPromosAdd.Update()

                        lnkDeletePromo.Visible = False
                        lnkEditPromo.Visible = False

                    End If

                End If

            Case Else

                'do nothing
        End Select

        ViewState("proc") = ""
        clsSession.DeleteStatus = ""

    End Sub

    Protected Sub lnkEditPromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditPromo.Click
        'clsSession.IsDepartmental = 0
        For Each row As GridViewRow In gridPromotionsAdd.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")

            If cb IsNot Nothing AndAlso cb.Checked Then

                Dim lb As Label = row.FindControl("lblPromoID")
                Dim lbFmt As Label = row.FindControl("lblPromoFormat")
                Dim lbTID As Label = row.FindControl("lblPromoTypeID")
                Dim lbPD As Label = row.FindControl("lblPD")

                If lbFmt.Text = "D" Then

                    Dim a As String = lbPD.Text
                    Server.Transfer("RCDPMemoDepartmental.aspx?MemoID=" & Request("MemoID") & "&RequestID=" & Request("RequestID") & "&CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString & "&pid=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&tid=" & clsEncryptDecrypt.EncryptText(lbTID.Text, SystemUser.EncryptKey.ToString) & "&pd=" & clsEncryptDecrypt.EncryptText(lbPD.Text, SystemUser.EncryptKey.ToString))

                ElseIf lbFmt.Text = "S" Then

                    clsSession.CurrPromoID = 0
                    Server.Transfer("RCDPStandardPromoEntry.aspx?MemoID=" & Request("MemoID") & "&RequestID=" & Request("RequestID") & "&CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString & "&pid=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString))

                End If
                'clsSession.CurrPromoID = lb.Text

                'clsSession.MsgTransFlowFlag = 2

                'Response.Redirect("PromoEntry.aspx")

            End If

        Next

    End Sub

    'Protected Sub lnkEditMechanicsOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditMechanicsOld.Click
    '    hidBox.Value = litMechanicsOld.Text
    '    'hidMechanicsActive.Value = lnkEditMechanicsOld.ClientID
    '    clsSession.Mechanics = litMechanicsOld.Text.ToString()
    '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor3('" & cmdMechanics.ClientID & "', 'Promotion Mechanics (Old)');</script>")
    'End Sub

    Protected Sub lnkEditMechanicsNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditMechanicsNew.Click
        hidBox.Value = litMechanicsNew.Text
        hidMechanicsActive.Value = lnkEditMechanicsNew.ClientID
        clsSession.Mechanics = litMechanicsNew.Text.ToString()
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor3('" & cmdMechanics.ClientID & "', 'Promotion Mechanics (New)');</script>")
    End Sub

    Protected Sub cmdMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMechanics.Click
        'If hidMechanicsActive.Value = lnkEditMechanicsOld.ClientID Then

        '    litMechanicsOld.Text = hidBox.Value

        'ElseIf hidMechanicsActive.Value = lnkEditMechanicsNew.ClientID Then

        litMechanicsNew.Text = hidBox.Value

        ' End If

        GC.Collect()
    End Sub


    Protected Sub gridPromotionsAdd_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridPromotionsAdd.SelectedIndexChanged

    End Sub

    Protected Sub gridInsteadOf_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridInsteadOf.RowDataBound
        Const nCol As Integer = 0
        If e.Row.RowIndex > -1 Then

            Dim b As String = e.Row.Cells(nCol).Text
            e.Row.Cells(nCol).Text = Server.HtmlDecode(e.Row.Cells(nCol).Text)

        End If
    End Sub

    Protected Sub gridInsteadOf_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridInsteadOf.SelectedIndexChanged

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



    Protected Sub lnkAddNewVSLPFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddNewFile.Click

        Dim strRequestString As String = "?RequestID=" & Request("RequestID") & "&MemoID=" & Request("MemoID") & "&v1=" & clsEncryptDecrypt.EncryptText(cboPromoType.SelectedValue, SystemUser.EncryptKey.ToString) & "&v2=" & clsEncryptDecrypt.EncryptText(cboPromoType.SelectedItem.Text, SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("ADDNEW", SystemUser.EncryptKey.ToString)

        lblRFPromoType.Text = ""

        If cboPromoType.SelectedIndex = 0 Then

            lblRFPromoType.Text = "Please select promotion type."

        Else

            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openUploadVSLP('" & strRequestString & "');</script>")

        End If

    End Sub

    Protected Sub linkSeedAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSeedAttachment.Click
        Dim strRequestString As String = "?CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(hidRequestID.Value, SystemUser.EncryptKey.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openReseedAttachment('" & strRequestString & "');</script>")
    End Sub

    Protected Sub linkSwipestakesSeed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesSeed.Click
        clsSession.CurrRequestID = hidRequestID.Value
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesSeed();</script>")
    End Sub

    Protected Sub linkSwipestakesReseed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesReseed.Click
        clsSession.CurrRequestID = hidRequestID.Value
        Dim strRequestString As String = "?CRID=" & clsEncryptDecrypt.EncryptText(lblCRID.Text, SystemUser.EncryptKey.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesReseed('" & strRequestString & "');</script>")
    End Sub

    Protected Sub btnSeedDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeedDownload.Click
        ShowAttachments_SWP()
    End Sub
End Class
