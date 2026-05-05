Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.Web
Imports System.Web.UI
Imports System.Math
Imports System.IO

Partial Class ViewMemo
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

                If .Status.Trim() = "Approved" Then
                    lblReportTitle.Text = "PROMOTION ANNOUNCEMENT"
                    lblMemoDate.Text = Format(.ApproveDate, "MMMM dd, yyyy")
                    lblMemoNumber.Text = .MemoNumber.ToString()

                    'ElseIf .Status.Trim() = "Cancelled" Then

                    '    lblReportTitle.Text = "PROMOTION ANNOUNCEMENT"
                    '    lblMemoDate.Text = Format(.ApproveDate, "MMMM dd, yyyy")
                    '    lblMemoNumber.Text = .MemoNumber.ToString()

                    ' revised dowcarpio20131218@smretailinc: promo
                    If .CRFLAG = "C" Then
                        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>changeBGImage();</script>")
                    End If

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

                ' hide promo item for SBU Marketing Requestor
                'If .GroupType = "SBU" Or .GroupType = "BCR" Then
                '    gridPromotions.Columns(1).Visible = False
                'End If

                'show column if Dept/SDept/Class data is available
                If gridPromotions.Rows.Count > 0 Then
                    gridPromotions.Columns(1).Visible = (gridPromotions.Rows(0).Cells(1).Text <> "&nbsp;")
                End If

                Dim dv As DataView
                dv = Me.SqlDSPromotype.Select(DataSourceSelectArguments.Empty)
                Dim dr As DataRow
                dr = dv.Table.Rows(0)
                lblPromoType.Text = dr("PromoType")

                ViewState("RequestID") = CInt(.RequestID)
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

        If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If (Request("MemoID") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

        Dim dv As DataView
        dv = sqlDSFilter.Select(DataSourceSelectArguments.Empty)
        Dim dr As DataRow
        dr = dv.Table.Rows(0)

        If CInt(dr("Flag")) = 1 Then

            If Not (Request("xmode") Is Nothing) Then
                Response.Redirect("ViewMemoMultiplePromo.aspx?xmode=" & Request("xmode") & "&MemoID=" & Request("MemoID"))
            Else

                Try

                    Response.Redirect("ViewMemoMultiplePromo.aspx?MemoID=" & Request("MemoID") & "&Report=False&v1=" & Request("v1").ToString)

                Catch ex As Exception

                    If ex.Message = "Object reference not set to an instance of an object." Then

                        Response.Redirect("ViewMemoMultiplePromo.aspx?MemoID=" & Request("MemoID"))

                    End If

                End Try

            End If

        ElseIf CInt(dr("Flag")) = 0 Then

            If Not (Request("xmode") Is Nothing) Then
                Response.Redirect("ViewMemoSinglePromo.aspx?xmode=" & Request("xmode") & "&MemoID=" & Request("MemoID"))
            Else

                Try

                    Response.Redirect("ViewMemoSinglePromo.aspx?MemoID=" & Request("MemoID") & "&Report=False&v1=" & Request("v1").ToString)

                Catch ex As Exception

                    If ex.Message = "Object reference not set to an instance of an object." Then

                        Response.Redirect("ViewMemoSinglePromo.aspx?MemoID=" & Request("MemoID"))

                    End If

                End Try

            End If

        Else

            If clsSession.FlagForPOSDisp = False Then
                If Request("xmode") = 1 Then
                    lnkDone.Text = "Back to Memo View"
                    lnkDone.PostBackUrl = "~/PromoMemo.aspx?MemoID=" & clsSession.CurrMemoID
                Else

                    ' Revised dowcarpio11132012@smretailinc: to view memo for RCDP request
                    If Request("Report") = "False" And SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Then

                        lnkDone.Text = "Back to Previous Page"
                        lnkDone.PostBackUrl = "~/CreateRCPDRequest.aspx?v1=" & Request("v1").ToString

                    Else

                        '11-Sept2018 - RPR1001 check Target Pages
                        If (Request("Target") = Nothing) Then
                            lnkDone.Text = "Back to Announcements"
                            lnkDone.PostBackUrl = "~/PromoAnnouncement.aspx"
                        Else
                            lnkDone.Text = "Back to Reports"
                            lnkDone.PostBackUrl = "~/PromoReport.aspx"
                        End If
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
                lnkPrintMemo.Text = "Create " & btnAddName.ToString & " Request"
                'lnkDone.Text = "Close Template Preview"
            Else

                lnkPrintMemo.Attributes.Add("OnClick", "return printSpecial_CCL()")
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

            LoadMemoInformation()
        End If
        ShowAttachments()


        ' check if SMAC Deals Promotion
        '-------------------------------------------------------------
        Dim drPromo As DataRow
        Dim sqlCmd As String

        sqlCmd = "SELECT OnlineSellingStart, OnlineSellingEnd " & _
                 "FROM Promotions AS P " & _
                 "INNER JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                 "WHERE T.ProcessType = 'SMACdeals' " & _
                 "AND P.MemoID = 0" & Request("MemoID")

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

        If gridRefMemos.Rows.Count = 0 Then

            trRefMemos.Visible = False

        End If

        GC.Collect()
        ShowMenu()
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

    ' Revised dowcarpio11202012@smretailinc: Cancellation/Addendum/Extension/Clarification
    Protected Sub lnkPrintMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintMemo.Click

        If Mid(lnkPrintMemo.Text, 1, 6) = "Create" Then

            Response.Redirect("RCDPMemo.aspx?RequestID=" & clsEncryptDecrypt.EncryptText(ViewState("RequestID").ToString, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(Request("MemoID").ToString, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("ADDNEW", SystemUser.EncryptKey.ToString))

        End If

    End Sub

    ' Revised dowcarpio11202012@smretailinc: Cancellation/Addendum/Extension/Clarification
    Protected Sub gridRefMemos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRefMemos.RowDataBound

        Dim hidCRID As HiddenField = e.Row.Cells(0).FindControl("hidCRID")
        Dim hidMemoID As HiddenField = e.Row.Cells(0).FindControl("hidMemoID")
        Dim hidReqType As HiddenField = e.Row.Cells(0).FindControl("hidReqType")
        Dim sRedTag As String = ""


        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(2).ToolTip = e.Row.Cells(2).Text

            If Len(e.Row.Cells(1).Text) > 50 Then
                e.Row.Cells(2).Text = Left(e.Row.Cells(2).Text, 45) & "..."
            End If

            If hidReqType.Value = "CCL" Then

                e.Row.Cells(1).Text = "<a href='ViewMemo_Cancellation.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("CCL", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(1).Text & "</span></a>"
                e.Row.Cells(2).Text = "<a href='ViewMemo_Cancellation.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("CCL", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"

            ElseIf hidReqType.Value = "ADD" Then

                e.Row.Cells(1).Text = "<a href='ViewMemo_Addendum.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("ADD", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(1).Text & "</span></a>"
                e.Row.Cells(2).Text = "<a href='ViewMemo_Addendum.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("ADD", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"


            ElseIf hidReqType.Value = "EXT" Then

                e.Row.Cells(1).Text = "<a href='ViewMemo_Extension.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("EXT", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(1).Text & "</span></a>"
                e.Row.Cells(2).Text = "<a href='ViewMemo_Extension.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("EXT", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"

            ElseIf hidReqType.Value = "CRF" Then

                e.Row.Cells(1).Text = "<a href='ViewMemo_Clarification.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("CRF", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(1).Text & "</span></a>"
                e.Row.Cells(2).Text = "<a href='ViewMemo_Clarification.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("CRF", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"

            ElseIf hidReqType.Value = "SWP" Then
                e.Row.Cells(1).Text = "<a href='ViewMemo_Extension.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("SWP", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(1).Text & "</span></a>"
                e.Row.Cells(2).Text = "<a href='ViewMemo_Extension.aspx?MemoID=" & hidMemoID.Value & "&CRID=" & clsEncryptDecrypt.EncryptText(hidCRID.Value, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("SWP", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"

            Else

                Response.Redirect("InvalidAccess.aspx")

            End If

        End If

    End Sub

    Protected Sub lnkDetailsUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDetailsUPC.Click
        ClientScript.RegisterStartupScript(Me.GetType, "UPCdetails", "<script>OpenDetailsUPC('324','800','" & ViewState("RequestID").ToString & "');</script>")
    End Sub

    Protected Sub linkBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkBinRange.Click
        clsSession.CurrRequestID = ViewState("RequestID").ToString
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
    End Sub

    Protected Sub linkSwipestakesMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSwipestakesMessage.Click
        clsSession.CurrRequestID = ViewState("RequestID").ToString
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

    Protected Sub linkRebateBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkRebateBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRange();</script>")
    End Sub
End Class
