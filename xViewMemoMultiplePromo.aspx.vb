Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.Web
Imports System.Web.UI
Imports System.Math
Imports System.IO
Partial Class ViewMemoMultiplePromo
    Inherits System.Web.UI.Page

    Private Sub LoadMemoInformation()

        Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter
        Dim dtMemos As dsPromotions.MemosDataTable
        Dim rowMemos As dsPromotions.MemosRow

        dtMemos = taMemos.GetMemoByID(clsSession.CurrMemoID)

        If dtMemos.Rows.Count = 0 Then
            Response.Redirect("InvalidAccess.aspx")
        Else

            rowMemos = dtMemos.Rows(0)

            With rowMemos

                If .Status.Trim() = "Approved" Then
                    lblReportTitle.Text = "PROMOTION ANNOUNCEMENT"
                    lblMemoDate.Text = Format(.ApproveDate, "MMMM dd, yyyy")
                    lblMemoNumber.Text = .MemoNumber.ToString()
                Else
                    lblReportTitle.Text = "MEMO DRAFT"
                    lblMemoDate.Text = Format(.MemoDate, "MMMM dd, yyyy")
                    lblMemoNumber.Text = ""
                End If

                lblPromoType.Text = ""

                lblPromoTitle.Text = .Title.ToString()
                lblPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .PromoPeriodTo.ToLongDateString()

                litBranches.Text = .Branches.ToString()

                litGuidelines.Text = Server.HtmlDecode(.Guidelines.ToString())

                lblPrepBy.Text = .PreparedBy.ToUpper()
                lblPreparePos.Text = .PreparePos.ToString()

                lblReviewedBy.Text = .ReviewedBy.ToUpper()
                lblReviewerPos.Text = .ReviewerPos.ToString()

                lblApprovedBy.Text = .ApprovedBy.ToUpper()
                lblApproverPos.Text = .ApproverPos.ToString()

                Dim dv As DataView
                dv = Me.SqlDSPromotype.Select(DataSourceSelectArguments.Empty)
                Dim dr As DataRow
                dr = dv.Table.Rows(0)
                lblPromoType.Text = dr("PromoType")

                ViewState("RequestID") = CInt(.RequestID)
            End With
        End If

        GC.Collect()

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

        'If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If (Request("MemoID") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

        If clsSession.FlagForPOSDisp = False Then
            If Request("xmode") = 1 Then
                lnkDone.Text = "Back to Memo View"
                lnkDone.PostBackUrl = "~/PromoMemo.aspx?MemoID=" & clsSession.CurrMemoID
            Else
                lnkDone.Text = "Back to Announcements"
                lnkDone.PostBackUrl = "~/PromoAnnouncement.aspx"
            End If
        Else
            lnkDone.Text = "Back to POS"
            lnkDone.PostBackUrl = "~/HomePagePOS.aspx"
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

        clsSession.CurrMemoID = Request("MemoID")

        If Not IsPostBack() Then
            LoadMemoInformation()
            'sqldsPromos.SelectParameters("RequestID").DefaultValue = Session("CurrRequestID")
            lnkPrintMemo.Attributes.Add("OnClick", "return printSpecial()")
            ShowAttachments()
        End If

        GC.Collect()
    End Sub

    Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

        Const nCol As Integer = 2
        Static rowPrevious As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(3).Text = Server.HtmlDecode(e.Row.Cells(3).Text)
        End If

        If e.Row.RowIndex = 0 Then rowPrevious = e.Row


        If e.Row.RowIndex > 0 Then

            ' merge Description cells with same PromoID
            Dim lbPrev As Label = rowPrevious.FindControl("lblDeptSdeptClass")
            Dim lbCurr As Label = e.Row.FindControl("lblDeptSdeptClass")

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

    End Function

    Protected Sub lnkBtnViewBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnViewBranches.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>OpenViewBranches('','');</script>")
    End Sub


    Private Sub ShowAttachments()
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String = ""

        strDir = clsPromo.pathAttachment & ViewState("RequestID")
        lblFiles.Text = ""

        'Dim di As New System.IO.DirectoryInfo(strDir)

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""ViewMemo.aspx?FileName=" & Server.UrlEncode(file.Name.ToString) & "&MemoID=" & clsSession.CurrMemoID.ToString & """>" & file.Name.ToString & "</a> "
            Next
        End If

        If Len(strFiles) = 0 Then lblFiles.Text = ""

        imgbtnDownload.Visible = tr
    End Sub

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & ViewState("RequestID").ToString, 1)
        Response.Redirect("ViewMemo.aspx?DownLoad=" & ViewState("RequestID") & ".zip" & "&Path=" & clsPromo.pathAttachment & ViewState("RequestID").ToString & "&MemoID=" & clsSession.CurrMemoID.ToString)
    End Sub
End Class
