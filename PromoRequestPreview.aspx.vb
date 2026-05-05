Imports dsPromotionsTableAdapters

Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Imports System.IO
Imports System.Linq

Partial Class PromoRequestPreview
    Inherits System.Web.UI.Page

    Protected Function GetPromoRequestInfo(ByVal nRequestID As Long) As Boolean

        Dim drRow As DataRow = Nothing
        Dim strQuery As String

        Dim lResult As Boolean = False

        strQuery = "SELECT COALESCE(R.WorkFlowCode, '') AS sWorkFlowCode, " & _
                    "COALESCE(T.IsTemplated, 0, T.IsTemplated) AS TemplatedPromo, " & _
                    "R.OwnerGroup, T.IsUPCLevel " & _
                    "FROM PromoRequests AS R " & _
                    "LEFT JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                    "LEFT JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                    "WHERE R.RequestID = 0" & nRequestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
            ViewState("WorkFlowCode") = drRow("sWorkFlowCode")
            ViewState("OwnerGroup") = drRow("OwnerGroup")
            ViewState("IsTemplatedPromo") = (drRow("TemplatedPromo") = 1)
            Session("IsUPCLevel") = drRow("IsUPCLevel")

            lResult = True
        End If

        Return lResult

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '::ToDo:: parameter and security checking
        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserLevel > SystemUser.UserRoles.PromoRequestor Then Response.Redirect("InvalidAccess.aspx")
        If Not IsPostBack() Then
            ' check if uploading file
            If Request("Filename") <> Nothing Then
                Dim fname As String
                fname = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString & "\" & Request("Filename")
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

            ' save RequestID parameter to session variable if one was passed
            If Request("RequestID") <> 0 Then clsSession.CurrRequestID = Request("RequestID")

            sqldsPromos.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID

            Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()
            Dim dtPromoRequests As dsPromotions.PromoRequestsDataTable
            Dim rowPromoRequest As dsPromotions.PromoRequestsRow

            dtPromoRequests = taPromoRequests.GetPromoRequestByID(clsSession.CurrRequestID)

            If Not GetPromoRequestInfo(clsSession.CurrRequestID) Then
                blistErrorMsg.Items.Add("Error accessing Promo Request information.")
                Exit Sub
            End If

            If dtPromoRequests.Rows.Count = 0 Then
                ' ::ToDo:: error
                ViewState("TranStatus") = ""

            Else
                rowPromoRequest = dtPromoRequests.Rows(0)

                With rowPromoRequest
                    lblRequestID.Text = Format(.RequestID, "0####") '& Format(Now.Year, "-0#")
                    lblRequestDate.Text = .RequestDate.ToLongDateString()
                    lblBranches.Text = .Branches

                    txtRequestTitle.Text = .Title.ToString()
                    txtPeriodFrom.Text = .PromoPeriodFrom
                    txtPeriodTo.Text = .PromoPeriodTo

                    If ViewState("IsTemplatedPromo") Then
                        litGuidelines.Text = Server.HtmlDecode(.TemplatedGuideline.ToString())
                        trGuidelines.Visible = True
                    Else
                        trGuidelines.Visible = False
                    End If

                    'show column if Dept/SDept/Class data is available
                    If gridPromotions.Rows.Count > 0 Then
                        gridPromotions.Columns(3).Visible = (gridPromotions.Rows(0).Cells(3).Text <> "&nbsp;")
                    End If

                    ViewState("TranStatus") = rowPromoRequest.Status.ToString()
                End With

                ' check if there's a promotion
                If gridPromotions.Rows.Count > 0 Then
                    lnkAddPromo.Visible = False
                    lnkEditPromo.Visible = True
                    lnkDeletePromo.Visible = False
                Else
                    lnkAddPromo.Visible = True
                    lnkEditPromo.Visible = False
                    lnkDeletePromo.Visible = False
                End If

            End If

            Dim tr As Boolean = False
            Dim strDir As String = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString()
            Dim strFiles As String = ""

            lblFiles.Text = ""

            If System.IO.Directory.Exists(strDir) Then

                Dim dir As New System.IO.DirectoryInfo(strDir)
                Dim files As System.IO.FileInfo() = dir.GetFiles()

                For Each file As System.IO.FileInfo In files
                    tr = True
                    strFiles = strFiles & file.Name.ToString & ","
                    lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href='PromoRequestPreview.aspx?FileName=" & file.Name.ToString & "'>" & file.Name.ToString & "</a> "
                Next

                If Len(strFiles) = 0 Then lblFiles.Text = ""

            End If

            imgbtnDownload.Visible = tr

        End If

        ' set command links visibility
        lnkClose.Visible = (Request("DocAttrib") = "Active")
        lnkDraftSave.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor)

        lnkSaveRequest.Visible = (clsPromo.GetAddRequestButtonState() Or ViewState("TranStatus").ToString = "Returned")

        lnkEditBranch.Visible = (lblBranches.Text <> "")
        ShowMenu()

    End Sub

    Protected Sub lnkAddPromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddPromo.Click

        'show promo entry screen
        clsSession.CurrPromoID = 0
        Response.Redirect("PromoEntry.aspx")

    End Sub

    Protected Sub lnkEditPromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditPromo.Click

        '::TODO:: convert promotions list to clickable row for EDIT

        If gridPromotions.Rows.Count > 0 Then
            Dim lb As Label = gridPromotions.Rows(0).FindControl("lblPromoID")

            clsSession.IsDepartmental = 0

            clsSession.CurrPromoID = lb.Text

            clsSession.MsgTransFlowFlag = 2

            Response.Redirect("PromoEntry.aspx")
        End If

    End Sub

    Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

        Const nCol As Integer = 4
        Static rowPrevious As GridViewRow

        ' decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            'rowPrevious.Cells(nCol).Text = "<a href='PromoEntry.aspx?PromoID=" & lb.Text & "'>" & rowPrevious.Cells(nCol).Text & "</a>"
            e.Row.Cells(nCol).Text = Server.HtmlDecode(e.Row.Cells(nCol).Text)
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

    Protected Sub lnkEditBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditBranch.Click
        clsSession.IsDepartmental = 0
        clsSession.CurrPromoID = 0
        Response.Redirect("PromoBranches.aspx")
    End Sub

    Protected Sub lnkSaveRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveRequest.Click

        ' check for completeness of entry
        blistErrorMsg.Items.Clear()

        ' check if there's atleast a single promo with details
        If gridPromotions.Rows.Count() < 1 Then
            blistErrorMsg.Items.Add("Cannot submit an incomplete request. Missing promotion info.")
        Else

            ' no validation of items for SBU Marketing and BCR
            If SystemUser.UserGroupType <> "SBU" And SystemUser.UserGroupType <> "BCR" Then

                ' check if there's a promo detail
                For Each rowPromo As GridViewRow In gridPromotions.Rows
                    If rowPromo.Cells(3).Text = "&nbsp;" Then
                        blistErrorMsg.Items.Add("Cannot submit an incomplete request. Missing promotion items.")
                        Exit For
                    End If
                Next

            End If

        End If

        If (lblBranches.Text = "") Or (Not ValidatePromoBranch()) Then

            blistErrorMsg.Items.Add("There must be at least a single branch to apply promo.")

        End If

        If txtRequestTitle.Text.Trim() = "" Then 'rbs7281 6/5/2025 Mantis#62995 Validation upon Submitting Request
            blistErrorMsg.Items.Clear()
            blistErrorMsg.Items.Add("Promo title not indicated.")
        End If


        '----------------------------------------
        ' get promotion information
        '----------------------------------------
        'rbs7281
        Dim drPromo As DataRow
        Dim sqlCmd As String

        sqlCmd = "SELECT * from vwGetPromoInformation " & _
                 "WHERE RequestID = 0" & clsSession.CurrRequestID

        If Not clsSystemApp.GetDataRow(clsPromo.SQLConnString, sqlCmd, drPromo) Then
            blistErrorMsg.Items.Add("Error accessing promo type configuration database.")
            Exit Sub
        End If

        Dim nNumLeadDays As Integer
        nNumLeadDays = IIf(drPromo.IsNull("NumLeadDays"), 0, drPromo("NumLeadDays"))

        Dim nApprovalCutoffMinutes As Integer
        nApprovalCutoffMinutes = IIf(drPromo.IsNull("nApprovalCutoffMinutes"), "", drPromo("nApprovalCutoffMinutes"))

        Dim sApprovalCutoff As String
        sApprovalCutoff = IIf(drPromo.IsNull("sApprovalCutoff"), "", drPromo("sApprovalCutoff"))

        Dim sProcessType As String
        sProcessType = IIf(drPromo.IsNull("ProcessType"), "", drPromo("ProcessType"))
        '----------------------------------------
        ' Check dates
        '----------------------------------------

        ' -- promo period from

        If Not IsDate(txtPeriodFrom.Text) Then

            blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format

        ElseIf DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < 1 Then

            blistErrorMsg.Items.Add("Request must be at least a calendar day before the start of promo.")

        ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < nNumLeadDays) Then

            blistErrorMsg.Items.Add("Request must be " & nNumLeadDays & " calendar day(s) before the start of promo.")
            'Mantis#67113 removal of Cutoff Time in requestor side
        ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = nNumLeadDays) And (Now.TimeOfDay.TotalMinutes > nApprovalCutoffMinutes And nApprovalCutoffMinutes <> "0") Then  ' past 2pm
            'NBS:20200616
            'ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (14 * 60)) Then  ' past 2pm

            blistErrorMsg.Items.Add("Request past the " & sApprovalCutoff & "  cut-off time must be at least " & nNumLeadDays & " calendar days before the start of promo.")

        End If

        '-- promo periodto
        If Not IsDate(txtPeriodTo.Text) Then
            blistErrorMsg.Items.Add("Blank or invalid promo end date format.") ' invalid date format

        ElseIf CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then
            blistErrorMsg.Items.Add("End of promo must not be earlier than the start date.")

        ElseIf sProcessType = "SMACdeals" Then

            '-------------------------------------------------------------
            ' check if SMAC Deals Promotion
            '-------------------------------------------------------------

            If Not drPromo.IsNull("OnlineSellingStart") And _
               Not drPromo.IsNull("OnlineSellingEnd") Then

                If CDate(drPromo("OnlineSellingStart")) <= CDate(txtPeriodTo.Text) Then
                    blistErrorMsg.Items.Add("Promo period should occur before the redemption period.")
                End If

            Else

                blistErrorMsg.Items.Add("Redemption period not properly specified.")

            End If
        End If

        ' disallow requests that are more than 3 months ahead .: 8/25/2010 :.
        If CDate(txtPeriodFrom.Text) > DateAdd(DateInterval.Month, 3, Today()) Then
            blistErrorMsg.Items.Add("Requests that are more than 3 months ahead are not allowed.")
        End If

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        ' warn user if date of request is less than the 10-day lead time
        'If DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < 10 Then

        '    If DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < 3 Then

        '        ViewState("Proc") = "do_nothing"
        '        lblPopTitle.Value = "Error"
        '        clsSession.Icon = "error"
        '        clsSession.Message = "Unable to submit promo request.<br /><br />" & _
        '                             "Date of promo effectivity does not comply with the minimum 3-day processing period.<br /><br />"

        '        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','');</script>")

        '    Else

        '        ' Revised to include character merchandising in the condition change display message for CM 
        '        lblPopTitle.Value = "Warning"
        '        clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br />" & _
        '                             IIf(SystemUser.UserGroupType = "CM", "<b>Request will be forwarded to the VP-MPD for pre-approval.</b><br><br>", "<b>Request will be forwarded to VP-MPD upon approval of MBU head.</b><br><br>").ToString & _
        '                             "Do you still wish to continue?"
        '        clsSession.Icon = "warning"
        '        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>msgbox('218','');</script>")

        '    End If

        'Else

        clsSession.Message = ""
        Dim i As Integer
        Dim DCode As String = ""
        Dim SDCode As String = ""
        Dim CCode As String = ""
        Dim dt As New DataTable
        Dim msg As String = ""
        dt = FillConflict()

        If dt.Rows.Count <> 0 Then
            DCode = dt.Rows(0)("DepCode")
            SDCode = dt.Rows(0)("SubDepCode")
            CCode = dt.Rows(0)("ClassCode")
            msg &= dt.Rows(0)("DepCode") & "-" & dt.Rows(0)("SubDepCode") & "-" & dt.Rows(0)("ClassCode") & ":" & dt.Rows(0)("ShortDesc") & "<ul>"
            msg &= "Ref# MPR-" & Format(dt.Rows(0)("RequestID"), "0####") & " " & dt.Rows(i)("TypeDesc") & "<br />"

            For i = 1 To dt.Rows.Count - 1
                If DCode = dt.Rows(i)("DepCode") And SDCode = dt.Rows(i)("SubDepCode") And CCode = dt.Rows(i)("ClassCode") Then
                    msg &= "Ref# MPR-" & Format(dt.Rows(i)("RequestID"), "0####") & " " & dt.Rows(i)("TypeDesc") & "<br />"
                Else
                    msg &= "</ul>"
                    DCode = dt.Rows(i)("DepCode")
                    SDCode = dt.Rows(i)("SubDepCode")
                    CCode = dt.Rows(i)("ClassCode")
                    msg &= dt.Rows(i)("DepCode") & "-" & dt.Rows(i)("SubDepCode") & "-" & dt.Rows(i)("ClassCode") & ":" & dt.Rows(i)("ShortDesc") & "<ul>"
                    msg &= "Ref# MPR-" & Format(dt.Rows(i)("RequestID"), "0####") & " " & dt.Rows(i)("TypeDesc") & "<br />"
                End If
            Next
        End If

        clsSession.Message = msg

        If clsSession.Message <> "" Then

            clsSession.Message &= "</ul>"

            clsSession.Message = "One or more promotional conflicts detected for this item: <br>" & _
                                 "<div style='width:350px; height:130px; overflow:auto; background-color: WhiteSmoke; padding: 10px 10px 10px 10px;'>" & _
                                 clsSession.Message & "</div><br>Do you still wish to continue?"
            clsSession.Icon = "inquiry"
            lblPopTitle.Value = "Promotions"

            ViewState("Proc") = "submit"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('313','508');</script>")

        Else

            clsSession.Message = "Are you sure you want submit this request?"
            clsSession.Icon = "inquiry"
            lblPopTitle.Value = "Promotions"
            ViewState("Proc") = "submit"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

        End If

        'End If
    End Sub

    Private Function FillConflict() As DataTable

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())

        Dim sqlCmd As New SqlCommand("USP_ValidateConflict", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlCmd.Parameters.Add("@RequestID", SqlDbType.VarChar)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID

        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet

        sqlConn.Open()
        da.Fill(ds, "tblConflict")

        Dim dt As DataTable = ds.Tables("tblConflict")
        FillConflict = dt

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing
        da = Nothing
        ds = Nothing

        GC.Collect()


    End Function

    Private Function ValidatePromoBranch() As Boolean

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())

        Dim sqlCmd As New SqlCommand("Select * from PromoBranch Where PromoID In (Select PromoID From Promotions Where RequestID = " & clsSession.CurrRequestID & ")", sqlConn)
        sqlCmd.CommandType = CommandType.Text


        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet

        sqlConn.Open()
        da.Fill(ds, "tblPromoBranch")

        Dim dt As DataTable = ds.Tables("tblPromoBranch")

        If dt.Rows.Count > 0 Then

            ValidatePromoBranch = True

        Else

            ValidatePromoBranch = False

        End If

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing
        da = Nothing
        ds = Nothing

        GC.Collect()

    End Function
                                                 
    Protected Sub lnkDraftSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDraftSave.Click

        SavePromoRequest("Draft")


        If txtRequestTitle.Text.Trim() = "" Then
            blistErrorMsg.Items.Clear()
            blistErrorMsg.Items.Add("Promo title not indicated.")
        Else
            Response.Redirect("PromoRequestList.aspx")
        End If


    End Sub

    Private Sub SavePromoRequest(ByVal DocStatus As String)

        Dim taPromoRequests As New PromoRequestsTableAdapter
        Dim dtPromoRequests As dsPromotions.PromoRequestsDataTable
        Dim rowPromoRequest As dsPromotions.PromoRequestsRow

        ' update header details
        dtPromoRequests = taPromoRequests.GetPromoRequestByID(clsSession.CurrRequestID)

        If dtPromoRequests.Rows.Count > 0 Then
            rowPromoRequest = dtPromoRequests.Rows(0)

            With rowPromoRequest

                ' save changes in header

                .RequestDate = Now()
                .Title = txtRequestTitle.Text.ToUpper()

                If IsDate(txtPeriodFrom.Text) And IsDate(txtPeriodTo.Text) Then
                    .PromoPeriodFrom = CDate(txtPeriodFrom.Text)
                    .PromoPeriodTo = CDate(txtPeriodTo.Text)

                    .Status = DocStatus

                    If DocStatus.ToUpper() <> "DRAFT" Then .Remarks = ""

                    taPromoRequests.Update(dtPromoRequests)
                    taPromoRequests.UpdatePromoRequestDetails(CDate(txtPeriodFrom.Text), CDate(txtPeriodTo.Text), clsSession.CurrRequestID)

                Else
                    blistErrorMsg.Items.Add("Invalid promo duration")
                End If

            End With

        End If

        If DocStatus.ToUpper() <> "DRAFT" Then

            If DocStatus = "For MPD Processing" Then

                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), clsSession.CurrRequestID, "Approved promotional request.", SystemUser.UserName, "Promotion Transaction")

            Else

                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), _
                                   clsSession.CurrRequestID, "Submit promotional request to request approver.", _
                                   SystemUser.UserName, "Promotion Transaction")

            End If

        End If


    End Sub

    Private Sub ApprovePromoRequest(ByVal DocStatus As String)

        ' get user information to be posted in the document
        sqldsData.SelectCommand = "SELECT SignName, SignPosition FROM Users WHERE UserID = " & SystemUser.UserID

        Dim dvUsers As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
        Dim drUser As DataRow = dvUsers.Table.Rows(0)

        ' mark request as approved and update status
        sqldsData.UpdateCommand = "UPDATE PromoRequests SET  Status = '" & DocStatus & "' WHERE RequestID = " & clsSession.CurrRequestID
        sqldsData.Update()

        'audit trail
        If SystemUser.UserGroupType = "CM" Then
            clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), clsSession.CurrRequestID, "Approved promotional request. (Special Request for Character Merchandising)", SystemUser.UserName, "Promotion Transaction")
        Else
            clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), clsSession.CurrRequestID, "Approved promotional request.", SystemUser.UserName, "Promotion Transaction")
        End If

    End Sub


    Protected Sub lnkClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkClose.Click

        Response.Redirect("PromoRequest.aspx?RequestID=" & clsSession.CurrRequestID.ToString())

    End Sub


    Protected Sub lnkDeletePromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeletePromo.Click

        lblPopTitle.Value = "Promotion"
        clsSession.Message = "Delete selected promotions?"
        clsSession.Icon = "inquiry"
        ViewState("Proc") = "delete"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Protected Sub lnkPrintReq_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintReq.Click

        Server.Transfer("ViewRequest.aspx?Report=True")

    End Sub

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & clsSession.CurrRequestID.ToString, 1)
        Response.Redirect("PromoRequestPreview.aspx?DownLoad=" & clsSession.CurrRequestID.ToString & ".zip" & "&Path=" & clsPromo.pathAttachment & clsSession.CurrRequestID.ToString)
    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Select Case ViewState("Proc").ToString
            Case "submit"
                If clsSession.DeleteStatus = "yes" Then
                    'clsSession.Message = "Promotional Request was successfully submitted for review."
                    'clsSession.Icon = "success"
                    'lblPopTitle.Value = "Promotions"
                    'ViewState("Proc") = "success"

                    Select Case SystemUser.UserGroupType
                        Case "CM"
                            ApprovePromoRequest("For MPD Processing")

                            'Case "SBU"
                            '    SavePromoRequest("For MBU Approval")

                        Case Else

                            ' 20170224: new workflow - promo should pass to Msgd/Group head
                            ' *** previous users as BU approver will now be assigned as Msgd/Group head
                            SavePromoRequest("For Mdsg/Group Head Approval")

                    End Select

                    Response.Redirect("PromoRequestList.aspx")

                    'ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
                End If

            Case "delete"

                If clsSession.DeleteStatus = "yes" Then
                    For Each row As GridViewRow In gridPromotions.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSel")
                        If cb IsNot Nothing AndAlso cb.Checked Then
                            Dim lb As Label = row.FindControl("lblPromoID")
                            ' delete promotion header, promo branches, and details
                            sqldsData.DeleteCommand = "DELETE FROM PromoDetails WHERE PromoID = " & lb.Text
                            sqldsData.Delete()
                            sqldsData.DeleteCommand = "DELETE FROM PromoBranch WHERE PromoID = " & lb.Text
                            sqldsData.Delete()
                            sqldsData.DeleteCommand = "DELETE FROM Promotions WHERE PromoID = " & lb.Text
                            sqldsData.Delete()
                            gridPromotions.DataBind()
                        End If
                    Next

                    If gridPromotions.Rows.Count = 0 Then

                        sqldsData.UpdateCommand = "UPDATE PromoRequests SET  Branches = NULL WHERE RequestID = 0" & clsSession.CurrRequestID
                        sqldsData.Update()

                    End If

                    Response.Redirect("PromoRequestList.aspx")

                End If

            Case Else

                ' do nothing

        End Select

    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        If clsSession.DeleteStatus.ToLower = "yes" Then

            clsSession.Message = ""
            Dim i As Integer
            Dim DCode As String = ""
            Dim SDCode As String = ""
            Dim CCode As String = ""
            Dim dt As New DataTable
            Dim msg As String = ""
            dt = FillConflict()

            If dt.Rows.Count <> 0 Then
                DCode = dt.Rows(0)("DepCode")
                SDCode = dt.Rows(0)("SubDepCode")
                CCode = dt.Rows(0)("ClassCode")
                msg &= dt.Rows(0)("DepCode") & "-" & dt.Rows(0)("SubDepCode") & "-" & dt.Rows(0)("ClassCode") & ":" & dt.Rows(0)("ShortDesc") & "<ul>"
                msg &= "Ref# MPR-" & Format(dt.Rows(0)("RequestID"), "0####") & " " & dt.Rows(i)("TypeDesc") & "<br />"

                For i = 1 To dt.Rows.Count - 1
                    If DCode = dt.Rows(i)("DepCode") And SDCode = dt.Rows(i)("SubDepCode") And CCode = dt.Rows(i)("ClassCode") Then
                        msg &= "Ref# MPR-" & Format(dt.Rows(i)("RequestID"), "0####") & " " & dt.Rows(i)("TypeDesc") & "<br />"
                    Else
                        msg &= "</ul>"
                        DCode = dt.Rows(i)("DepCode")
                        SDCode = dt.Rows(i)("SubDepCode")
                        CCode = dt.Rows(i)("ClassCode")
                        msg &= dt.Rows(i)("DepCode") & "-" & dt.Rows(i)("SubDepCode") & "-" & dt.Rows(i)("ClassCode") & ":" & dt.Rows(i)("ShortDesc") & "<ul>"
                        msg &= "Ref# MPR-" & Format(dt.Rows(i)("RequestID"), "0####") & " " & dt.Rows(i)("TypeDesc") & "<br />"
                    End If
                Next
            End If
            clsSession.Message = msg

            If clsSession.Message <> "" Then

                clsSession.Message &= "</ul>"

                clsSession.Message = "One or more promotional conflicts detected for this item: <br>" & _
                                     "<div style='width:350px; height:130px; overflow:auto; background-color: WhiteSmoke; padding: 10px 10px 10px 10px;'>" & _
                                     clsSession.Message & "</div><br>Do you still wish to continue?"
                clsSession.Icon = "inquiry"
                lblPopTitle.Value = "Promotions"

                ViewState("Proc") = "submit"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('313','508');</script>")
            Else

                Select Case SystemUser.UserGroupType
                    Case "CM"
                        ApprovePromoRequest("For MPD Processing")

                    Case "SBU"

                        SavePromoRequest("For MBU Approval")

                    Case Else

                        ' 20170224: new workflow - promo should pass to Msgd/Group head
                        ' *** previous users as BU approver will now be assigned as Msgd/Group head
                        SavePromoRequest("For Mdsg/Group Head Approval")

                End Select

                Response.Redirect("PromoRequestList.aspx")

            End If
        End If
    End Sub

    Protected Sub linkSwipestakesMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesMessage.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesMessage();</script>")
    End Sub

    Protected Sub linkSwipestakesSeed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesSeed.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSwipestakesSeed();</script>")
    End Sub

    Protected Sub linkBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
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

    Protected Sub linkRebateBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkRebateBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
    End Sub

End Class
