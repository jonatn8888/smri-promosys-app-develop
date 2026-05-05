' Object Name	    :       ArchiveMemosView.aspx
' Purpose		    :       Requested by MPD dated November 16, 2011. Due to limited disk space in the PromoSys server and the continues
'                           posting of PromoSys transactions since its launch in July 2010 system
'                           resouces are being used up and slowdown is expected if no housekeeping
'                           is done on expired MPD announcement memos.
' Date Created	    :       06/11/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0               06/28/2012          Dow T. Carpio       Created this module.

Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.Web
Imports System.Web.UI
Imports System.Math
Imports System.IO
Imports System.Data.SqlClient

Partial Class ArchiveMemosView
    Inherits System.Web.UI.Page

    Private Function dtMemoInfo() As DataTable

        Dim sqlAdatpter As SqlDataAdapter
        Dim sqlConn As SqlConnection
        Dim dtbl As New DataTable

        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "PMS_P_ARCHIVEMEMO_INFO"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@MemoID", SqlDbType.Int)
        sqlCmd.Parameters("@MemoID").Value = clsSession.CurrMemoID

        sqlAdatpter = New SqlDataAdapter(sqlCmd)
        sqlAdatpter.Fill(dtbl)

        dtMemoInfo = dtbl

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        sqlAdatpter = Nothing
        dtbl = Nothing

        GC.Collect()

    End Function

    Private Sub LoadMemoInformation()

        'Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter()
        Dim dtMemos As New DataTable
        'Dim rowMemos As dsPromotions.MemosRow

        dtMemos = dtMemoInfo() 'taMemos.GetMemoByID(clsSession.CurrMemoID)

        If dtMemos.Rows.Count = 0 Then
            Response.Redirect("InvalidAccess.aspx")
        Else

            'rowMemos = dtMemos.Rows(0)

            With dtMemos

                If clsPromo.FetchDataItem(dtMemos, 0, "STATUS").ToString.Trim() = "Approved" Then
                    lblReportTitle.Text = "PROMOTION MEMORANDUM (ARCHIVED)"
                    lblMemoDate.Text = Format(CDate(clsPromo.FetchDataItem(dtMemos, 0, "APPROVEDATE").ToString), "MMMM dd, yyyy")
                    lblMemoNumber.Text = clsPromo.FetchDataItem(dtMemos, 0, "MEMONUMBER").ToString
                Else
                    lblReportTitle.Text = "MEMO DRAFT"
                    lblMemoDate.Text = Format(CDate(clsPromo.FetchDataItem(dtMemos, 0, "MEMODATE").ToString), "MMMM dd, yyyy")
                    lblMemoNumber.Text = ""
                End If

                lblPromoType.Text = ""

                lblPromoTitle.Text = clsPromo.FetchDataItem(dtMemos, 0, "TITLE").ToString
                lblPromoPeriod.Text = Format(CDate(clsPromo.FetchDataItem(dtMemos, 0, "PROMOPERIODFROM").ToString), "MMMM dd, yyyy") & " to " & Format(CDate(clsPromo.FetchDataItem(dtMemos, 0, "PROMOPERIODTO").ToString), "MMMM dd, yyyy")

                litBranches.Text = clsPromo.FetchDataItem(dtMemos, 0, "BRANCHES").ToString
                litGuidelines.Text = Server.HtmlDecode(clsPromo.FetchDataItem(dtMemos, 0, "GUIDELINES").ToString)

                lblPrepBy.Text = clsPromo.FetchDataItem(dtMemos, 0, "PREPAREDBY").ToString.ToUpper
                lblPreparePos.Text = clsPromo.FetchDataItem(dtMemos, 0, "PREPAREPOS").ToString

                lblReviewedBy.Text = clsPromo.FetchDataItem(dtMemos, 0, "REVIEWEDBY").ToString.ToUpper
                lblReviewerPos.Text = clsPromo.FetchDataItem(dtMemos, 0, "REVIEWERPOS").ToString

                lblApprovedBy.Text = clsPromo.FetchDataItem(dtMemos, 0, "APPROVEDBY").ToString.ToUpper
                lblApproverPos.Text = clsPromo.FetchDataItem(dtMemos, 0, "APPROVERPOS").ToString


                Dim dv As DataView
                dv = Me.SqlDSPromotype.Select(DataSourceSelectArguments.Empty)
                Dim dr As DataRow
                dr = dv.Table.Rows(0)
                lblPromoType.Text = dr("PromoType")

                ViewState("RequestID") = CInt(clsPromo.FetchDataItem(dtMemos, 0, "REQUESTID").ToString)
            End With
        End If

        dtMemos = Nothing

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

        If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If (Request("MemoID") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

        'Dim dv As DataView
        'dv = sqlDSFilter.Select(DataSourceSelectArguments.Empty)
        'Dim dr As DataRow
        'dr = dv.Table.Rows(0)

        'If CInt(dr("Flag")) = 1 Then
        'If Not (Request("xmode") Is Nothing) Then
        '    Response.Redirect("ArchiveMemosView.aspx?xmode=" & Request("xmode") & "&MemoID=" & Request("MemoID"))
        'Else
        '    Response.Redirect("ArchiveMemosView.aspx?MemoID=" & Request("MemoID"))
        'End If

        'ElseIf CInt(dr("Flag")) = 0 Then
        'If Not (Request("xmode") Is Nothing) Then
        '    Response.Redirect("ArchiveMemosView.aspx?xmode=" & Request("xmode") & "&MemoID=" & Request("MemoID"))
        'Else
        '    Response.Redirect("ArchiveMemosView.aspx?MemoID=" & Request("MemoID"))
        'End If
        'Else
        If clsSession.FlagForPOSDisp = False Then
            If Request("xmode") = 1 Then
                lnkDone.Text = "Back to Memo View"
                lnkDone.PostBackUrl = "~/ArchiveMemos.aspx?MemoID=" & clsSession.CurrMemoID
            Else
                lnkDone.Text = "Back to Archive Memos"
                lnkDone.PostBackUrl = "~/ArchiveMemos.aspx"
            End If
        Else
            lnkDone.Text = "Back to POS"
            lnkDone.PostBackUrl = "~/HomePagePOS.aspx"
        End If

        clsSession.CurrMemoID = Request("MemoID")


        'sqldsPromos.SelectParameters("RequestID").DefaultValue = Session("CurrRequestID")
        'lnkPrintMemo.Attributes.Add("OnClick", "return printSpecial()")

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

        LoadMemoInformation()
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
        StoredProc = "SELECT ShortName as shortdesc FROM PromoArchive.dbo.PromoBranch WHERE PromoID = '" & promoid & "'"

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

        strDir = clsPromo.pathAttachment & ViewState("RequestID")
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href=""ViewMemo.aspx?FileName=" & Server.UrlEncode(clsPromo.pathAttachment & ViewState("RequestID") & "\" & file.Name.ToString) & "&MemoID=" & clsSession.CurrMemoID.ToString & """>" & file.Name.ToString & "</a> "
            Next

            If Len(strFiles) = 0 Then lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr
    End Sub

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & ViewState("RequestID").ToString, 1)
        Response.Redirect("ViewMemo.aspx?DownLoad=" & ViewState("RequestID") & ".zip" & "&Path=" & clsPromo.pathAttachment & ViewState("RequestID").ToString & "&MemoID=" & clsSession.CurrMemoID.ToString)
    End Sub
 
End Class
