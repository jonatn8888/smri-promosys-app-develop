' Object Name	    :       CreateRCPDRequest.aspx
' Purpose		    :       Cancellation/addedum/extention/clarification search parameter module
' Date Created	    :       11/09/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0              11/09/2012     Dow T. Carpio     Created this control.

Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.Web
Imports System.Web.UI
Imports System.Math
Imports System.IO

Partial Class ViewMemo_Extension
    Inherits System.Web.UI.Page

    Private Sub LoadMemoInformation()

        Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter()
        Dim dtMemos As dsPromotions.MemosDataTable
        Dim rowMemos As dsPromotions.MemosRow

        dtMemos = taMemos.GetMemoByID(clsSession.CurrMemoID)

        If dtMemos.Rows.Count = 0 Then
            Response.Redirect("InvalidAccess.aspx")
        Else

            rowMemos = dtMemos.Rows(0)

            With rowMemos

                lblRefMemo.Text = .MemoNumber & " " & .Title

                'lblPromoTitle.Text = .Title.ToString()
                lblPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() '& " to " & .OldPromoPeriodTo.ToLongDateString()

                litBranches.Text = .Branches.ToString()
                'litGuidelines.Text = Server.HtmlDecode(.Guidelines.ToString())

                ' include credit in the condition :new group type
                ' hide promo item for SBU Marketing Requestor
                If .GroupType = "SBU" Or _
                    .GroupType = "BCR" Or _
                    .GroupType = "CREDIT" Then

                    gridPromotions.Columns(1).Visible = False

                End If

                'Dim dv As DataView
                'dv = Me.SqlDSPromotype.Select(DataSourceSelectArguments.Empty)
                'Dim dr As DataRow
                'dr = dv.Table.Rows(0)
                'lblRefMemo.Text = dr("PromoType")

                ViewState("RequestID") = CInt(.RequestID)

            End With
        End If

    End Sub

    Private Sub LoadRequestInformation()



        Dim taChangeRequests As New dsPromotionsTableAdapters.ChangeRequestsTableAdapter
        Dim dtChangeRequests As dsPromotions.ChangeRequestsDataTable
        Dim rowChangeRequests As dsPromotions.ChangeRequestsRow

        dtChangeRequests = taChangeRequests.GetRequestByID(clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString))

        If dtChangeRequests.Rows.Count = 0 Then

            Response.Redirect("InvalidAccess.aspx")

        Else

            rowChangeRequests = dtChangeRequests.Rows(0)

            With rowChangeRequests

                If .Status.Trim() = "Approved" Then

                    lblReportTitle.Text = "PROMOTION ANNOUNCEMENT"
                    lblMemoNumber.Text = .MemoNumber
                    lblMemoDate.Text = Format(.FinalApproveDate, "MMMM dd, yyyy")

                Else

                    lblReportTitle.Text = "MEMO DRAFT"
                    lblMemoNumber.Text = ""
                    lblMemoDate.Text = Format(.RequestDate, "MMMM dd, yyyy")

                End If

                ' Revised dowcarpio20140225@smretailinc: due to erroneous display of dates in extension. change of saving of oldpromoperiodto from memos to changerequests(effectdate)
                lblPromoPeriod.Text &= " to " & .EffectDate.ToLongDateString()

                lblPromoTitle.Text = .Title
                'lblEffectDate.Text = .EffectDate.ToLongDateString
                lblextpromoperiod.text = .PromoPeriodTo.ToLongDateString()

                'lblReason.Text = .Reason
                'litGuidelines.Text = Server.HtmlDecode(.Guidelines.ToString())


                lblPrepBy.Text = .PreparedBy.ToUpper()
                lblPreparePos.Text = .PreparePOS.ToString()

                lblReviewedBy.Text = .ReviewedBy.ToUpper()
                lblReviewerPos.Text = .ReviewerPOS.ToString()

                lblApprovedBy.Text = .ApprovedBy.ToUpper()
                lblApproverPos.Text = .ApproverPOS.ToString()

                calCancelEffectDate.Text = .EffectDate.ToLongDateString()


            End With

        End If


    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MyBase.OnPreRender(e)
        Dim strDisAbleBackButton As String
        strDisAbleBackButton = "<script language='javascript'>"
        strDisAbleBackButton += "window.history.forward(1);"
        strDisAbleBackButton += vbLf & "</script>"
        ClientScript.RegisterClientScriptBlock(Me.Page.[GetType](), "clientScript", strDisAbleBackButton)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Revised dowcarpio08222012@smretailinc: revised sql script of sqldsPromos to sort promotion using seqno(additional field) 
        If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If (Request("MemoID") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString
        Dim btnAddName As String = Nothing
        If strType = "CCL" Then


            btnAddName = "Cancellation"

        ElseIf strType = "ADD" Then

            btnAddName = "Addendum"

        ElseIf strType = "EXT" Then

            btnAddName = "Extension"

        ElseIf strType = "CRF" Then

            btnAddName = "Clarification"

        ElseIf strType = "SWP" Then

            btnAddName = "Swipestakes Reseed"

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If

        'Dim dv As DataView
        'Dim dr As DataRow
        'dv = sqlDSFilter.Select(DataSourceSelectArguments.Empty)
        'dr = dv.Table.Rows(0)

        'If CInt(dr("Flag")) = 1 Then
        '    If Not (Request("xmode") Is Nothing) Then
        '        Response.Redirect("ViewMemoMultiplePromo.aspx?xmode=" & Request("xmode") & "&MemoID=" & Request("MemoID"))
        '    Else

        '        Try

        '            Response.Redirect("ViewMemoMultiplePromo.aspx?MemoID=" & Request("MemoID") & "&Report=False&v1=" & Request("v1").ToString)

        '        Catch ex As Exception

        '            'If ex.Message <> "{System.Threading.ThreadAbortException}" Then

        '            '    Response.Redirect("ViewMemoMultiplePromo.aspx?MemoID=" & Request("MemoID"))

        '            'End If

        '        End Try

        '    End If

        'ElseIf CInt(dr("Flag")) = 0 Then
        '    If Not (Request("xmode") Is Nothing) Then
        '        Response.Redirect("ViewMemoSinglePromo.aspx?xmode=" & Request("xmode") & "&MemoID=" & Request("MemoID"))
        '    Else

        '        Try

        '            Response.Redirect("ViewMemoSinglePromo.aspx?MemoID=" & Request("MemoID") & "&Report=False&v1=" & Request("v1").ToString)

        '        Catch ex As Exception

        '            'If ex.Message <> "{System.Threading.ThreadAbortException}" Then

        '            '    Response.Redirect("ViewMemoSinglePromo.aspx?MemoID=" & Request("MemoID"))

        '            'End If

        '        End Try

        '    End If
        'Else
        If clsSession.FlagForPOSDisp = False Then

            If Request("xmode") = 1 Then

                lnkDone.Text = "Back to Memo View"
                lnkDone.PostBackUrl = "~/RCDPMemoView.aspx?CRID=" & Request("CRID").ToString & "&MemoID=" & clsEncryptDecrypt.EncryptText(Request("MemoID").ToString, SystemUser.EncryptKey.ToString) & "&RequestID=" & Request("RequestID") & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString)

            Else

                ' Revised dowcarpio11132012@smretailinc: to view memo for RCDP request
                If Request("Report") = "False" And SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Then

                    lnkDone.Text = "Back to RCPD Request"
                    lnkDone.PostBackUrl = "~/CreateRCPDRequest.aspx?v1=" & Request("v1").ToString

                Else

                    lnkDone.Text = "Back to Announcements"
                    lnkDone.PostBackUrl = "~/PromoAnnouncement.aspx"

                End If

            End If

        Else

            lnkDone.Text = "Back to POS"
            lnkDone.PostBackUrl = "~/HomePagePOS.aspx"

        End If

        clsSession.CurrMemoID = Request("MemoID")

        'sqldsPromos.SelectParameters("RequestID").DefaultValue = Session("CurrRequestID")
        'lnkPrintMemo.Attributes.Add("OnClick", "return printSpecial()")

        ' Revised dowcarpio11132012@smretailinc: to view memo for RCDP request
        If Request("Report") = "False" And SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Then


            lnkPrintMemo.Text = "Create " & btnAddName.ToString & " Request"
            'lnkDone.Text = "Close Template Preview"
        Else

            lnkPrintMemo.Attributes.Add("OnClick", "return printSpecial()")
            lnkPrintMemo.Text = "Print this Report"
            'lnkDone.Text = "Close Print Preview"

        End If


        If Request("Filename") <> Nothing Then
            Dim fname As String
            fname = Request("Filename")
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

        ' Added dowcarpio0304/2013@smretailinc: control fields to be viewed.
        'trPromoPeriod.Visible = False
        'trPromotions.Visible = False

        If strType = "SWP" Then
            trExtensionOfPromo.Visible = False
            trEffectDate.Visible = True
        End If

        LoadMemoInformation()
        LoadRequestInformation()
        'End If
        ShowAttachments()
        GC.Collect()

    End Sub

    Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

        Const nCol As Integer = 2
        Static rowPrevious As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(nCol).Text = Server.HtmlDecode(e.Row.Cells(nCol).Text)
        End If

        If e.Row.RowIndex = 0 Then rowPrevious = e.Row

        If e.Row.RowIndex >= 0 Then
            Dim lbCurr As Label = e.Row.FindControl("lblPromoID")
            Dim gridview As GridView = e.Row.FindControl("gvBranches")
            Dim dt As DataTable
            dt = getbranches(lbCurr.Text)
            gridview.DataSource = dt
            gridview.DataBind()
        End If

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

    Private Function getbranches(ByVal promoid As Int32) As DataTable

        Dim StoredProc As String
        StoredProc = "SELECT ShortName as shortdesc FROM PromoBranch WHERE PromoID = '" & promoid & "'"

        Dim sqlConn As SqlClient.SqlConnection
        sqlConn = New SqlClient.SqlConnection(clsPromo.SQLConnString())

        Dim sqlCmd As New SqlClient.SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.Text

        Dim da As New SqlClient.SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_branches")

        Dim dt As DataTable = ds.Tables("tbl_branches")

        Return dt

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing
        da = Nothing
        ds = Nothing

        GC.Collect()


    End Function

    Protected Sub lnkBtnViewBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnViewBranches.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>OpenViewBranches('','');</script>")
    End Sub

    Private Sub ShowAttachments()
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & "CR-" & clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""RCDPMemo.aspx?FileName=" & Server.UrlEncode(file.Name.ToString()) & "&MemoID=" & Request("MemoID").ToString & "&v1=" & Request("v1").ToString & "&stat=" & Request("stat").ToString & """>" & file.Name.ToString() & "</a> "
            Next

            If Len(strFiles) = 0 Then lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr
    End Sub

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & ViewState("RequestID").ToString, 1)
        Response.Redirect("ViewMemo.aspx?DownLoad=" & ViewState("RequestID") & ".zip" & "&Path=" & clsPromo.pathAttachment & ViewState("RequestID").ToString & "&MemoID=" & clsSession.CurrMemoID.ToString)
    End Sub

    ' Revised dowcarpio11202012@smretailinc: Cancellation/Addendum/Extension/Clarification
    Protected Sub lnkPrintMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintMemo.Click

        If Mid(lnkPrintMemo.Text, 1, 6) = "Create" Then

            Response.Redirect("RCDPMemo.aspx?MemoID=" & clsEncryptDecrypt.EncryptText(Request("MemoID").ToString, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("ADDNEW", SystemUser.EncryptKey.ToString))

        End If

    End Sub



End Class
