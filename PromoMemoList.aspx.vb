
Partial Class PromoMemoList
    Inherits System.Web.UI.Page

    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound

        Dim sRedTag As String = ""
        Dim lb As Label = e.Row.FindControl("lblMemoID")
        Dim lbReqID As Label = e.Row.FindControl("lblRequestID")
        Dim lbCRID As Label = e.Row.FindControl("lblCRID")
        Dim lbStatus As Label = e.Row.FindControl("lblStatus")
        Dim lbReqType As Label = e.Row.FindControl("lblReqType")

        If e.Row.RowType = DataControlRowType.DataRow Then

            If (lbStatus.Text <> "Approved") And (DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4) Then sRedTag = "style='color: red'"

            If lbReqType.Text = "CCL" Then
                e.Row.Cells(2).Text = "<a href='" & "RCDPMemoView.aspx" & "?CRID=" & clsEncryptDecrypt.EncryptText(lbCRID.Text, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(lbReqID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("CCL", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"
            ElseIf lbReqType.Text = "EXT" Then
                e.Row.Cells(2).Text = "<a href='" & "RCDPMemoView.aspx" & "?CRID=" & clsEncryptDecrypt.EncryptText(lbCRID.Text, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(lbReqID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & clsEncryptDecrypt.EncryptText("EXT", SystemUser.EncryptKey.ToString) & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"
            Else
                e.Row.Cells(2).Text = "<a href='PromoMemo.aspx?MemoID=" & lb.Text & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"
            End If

        End If

    End Sub

    Protected Sub gridSpecial_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridSpecial.RowDataBound

        Dim sRedTag As String = ""
        Dim lb As Label = e.Row.FindControl("lblRequestID")

        If e.Row.RowType = DataControlRowType.DataRow Then
            If DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4 Then sRedTag = "style='color: red'"

            e.Row.Cells(2).Text = "<a href='PromoRequest.aspx?RequestID=" & lb.Text & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"
        End If

    End Sub

    Private Sub InitializeDropDownList()

        'set up dropdown menu

        cboFilterStatus.Items.Clear()

        Select Case SystemUser.UserLevel

            Case SystemUser.UserRoles.Reviewer

                With cboFilterStatus
                    .Items.Add(New ListItem("Memos for Review", "Status = 'For Review'"))
                    .Items.Add(New ListItem("Other Requests for Approval", "Status = 'For Initial Approval'")) ' Added dowcarpio11062012@smretailinc: for initial approval status 
                    .Items.Add(New ListItem("Late Requests for Approval", "Status = 'Special Request'"))
                    .Items.Add(New ListItem("Memos for Approval", "Status = 'For Final Approval'"))
                    .Items.Add(New ListItem("Approved Memos", "Status = 'Approved'"))
                    ' requested by Madam CSY (promo requests under MPD Analyst view)
                    .Items.Add(New ListItem("Pending Requests", "Status = 'For MPD Processing'"))
                End With

            Case SystemUser.UserRoles.MemoApprover

                With cboFilterStatus
                    .Items.Add(New ListItem("Memos Awaiting Your Approval", "Status = 'For Final Approval'"))
                    .Items.Add(New ListItem("Other Requests for Approval", "Status = 'For Initial Approval'")) ' Added dowcarpio11062012@smretailinc: for initial approval status 
                    .Items.Add(New ListItem("Late Requests for Approval", "Status = 'Special Request'"))        ' from PromoRequests
                    .Items.Add(New ListItem("Cancellation Memos for Approval", " RequestType='CCL' AND Status = 'For Final Approval'")) ' Added dowcarpio20131204@smretailinc: to include cancellation request approval UAT findings By Ms. Jess (MPD)
                    .Items.Add(New ListItem("Extension Memos for Approval", " RequestType='EXT' AND Status = 'For Final Approval'")) ' Added dowcarpio20131204@smretailinc: to include extension request approval UAT findings By Ms. Jess (MPD)
                    .Items.Add(New ListItem("SMAC Deals Promotions", "MemoID IN ( SELECT MemoID	FROM Promotions	WHERE PromoTypeID IN (48, 93) ) AND Status = 'For Final Approval'"))
                    .Items.Add(New ListItem("SBU Marketing Promotions", " MemoID IN ( SELECT MemoID FROM Memos M INNER JOIN UserGroups UG ON UG.GroupID = M.OwnerGroup AND UG.BizUnit = 'SBU' ) AND Status = 'For Final Approval'"))
                    .Items.Add(New ListItem("Memos for Review", "Status = 'For Review'"))
                    .Items.Add(New ListItem("Approved Memos", "Status = 'Approved'"))
                    ' requested by Madam CSY (promo requests under MPD Analyst view)
                    .Items.Add(New ListItem("Pending Requests", "Status = 'For MPD Processing'"))
                End With

        End Select

        If clsSession.CurrFilterSelection > cboFilterStatus.Items.Count() Then
            cboFilterStatus.SelectedIndex = 0
        Else
            cboFilterStatus.SelectedIndex = clsSession.CurrFilterSelection
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel > SystemUser.UserRoles.Reviewer Then Response.Redirect("InvalidAccess.aspx")

        clsSession.CurrMemoID = 0

        If Not IsPostBack() Then
            InitializeDropDownList()
        End If

        Select Case SystemUser.UserLevel

            Case SystemUser.UserRoles.Reviewer

                gridSpecial.Visible = False
                gridRequests.Visible = False

                Dim HandledGroupSQL = " AND OwnerGroup IN (SELECT GroupID FROM UserGroups WHERE Category IN " & _
                                      "(SELECT Category FROM UserGroups WHERE GroupID IN " & _
                                      " (SELECT GroupID FROM GroupAssignment WHERE UserID = 0" & SystemUser.UserID & ")))" & _
                                      " ORDER BY PromoPeriodFrom"

                If cboFilterStatus.SelectedIndex = 1 Or _
                        cboFilterStatus.SelectedIndex = 2 Or _
                        cboFilterStatus.SelectedIndex = 5 Then


                    With sqldsRequests
                        .SelectCommand = "SELECT *, NULL AS CRID, NULL AS MEMOID, 'REG' AS REQUESTTYPE " & _
                                            "FROM PromoRequests " & _
                                            "WHERE " & cboFilterStatus.SelectedValue & HandledGroupSQL

                        .Select(DataSourceSelectArguments.Empty)
                        gridSpecial.DataBind()

                        gridSpecial.Visible = True
                    End With

                Else

                    With sqldsRequests
                        .SelectCommand = "SELECT * FROM " & _
                                            "(SELECT MEMOID,MEMODATE,TITLE,PROMOPERIODFROM,PROMOPERIODTO,PREPAREDBY,STATUS,NULL AS CRID, REQUESTID, 'REG' AS REQUESTTYPE,OWNERGROUP FROM Memos " & _
                                            "UNION ALL " & _
                                            "SELECT MEMOID,REQUESTDATE AS MEMODATE,TITLE,PROMOPERIODFROM,PROMOPERIODTO, REQUESTEDBY AS PREPAREDBY,STATUS,CRID AS CRID, REQUESTID, REQUESTTYPE,OWNERGROUP FROM dbo.ChangeRequests) A " & _
                                            "WHERE " & cboFilterStatus.SelectedValue & HandledGroupSQL '& _
                        '" ORDER BY PromoPeriodFrom"

                        .Select(DataSourceSelectArguments.Empty)
                        gridRequests.DataBind()

                        gridRequests.Visible = True

                    End With
                End If

            Case SystemUser.UserRoles.MemoApprover

                gridSpecial.Visible = False
                gridRequests.Visible = False

                ' Revised dowcarpio11062012@smretailinc: To include Other Promotions in the condition.
                If cboFilterStatus.SelectedIndex = 1 Or _
                     cboFilterStatus.SelectedIndex = 2 Or _
                     cboFilterStatus.SelectedIndex = 9 Then

                    With sqldsRequests
                        .SelectCommand = "SELECT *, NULL AS CRID, NULL AS MEMOID, 'REG' AS REQUESTTYPE " & _
                                            "FROM PromoRequests " & _
                                            "WHERE " & cboFilterStatus.SelectedValue & " " & _
                                            "ORDER BY PromoPeriodFrom"

                        .Select(DataSourceSelectArguments.Empty)
                        gridSpecial.DataBind()

                        gridSpecial.Visible = True
                    End With

                ElseIf cboFilterStatus.SelectedIndex = 3 Or _
                            cboFilterStatus.SelectedIndex = 4 Then


                    With sqldsRequests
                        .SelectCommand = "SELECT CRID, REQUESTID, REQUESTDATE AS MEMODATE, MEMOID, TITLE, PROMOPERIODFROM, PROMOPERIODTO, REQUESTEDBY AS PREPAREDBY, REQUESTTYPE,STATUS from dbo.ChangeRequests WHERE " & cboFilterStatus.SelectedValue

                        .Select(DataSourceSelectArguments.Empty)
                        gridRequests.DataBind()

                        gridRequests.Visible = True
                    End With

                Else

                    With sqldsRequests
                        .SelectCommand = "SELECT * FROM " & _
                                            "(SELECT MEMOID,MEMODATE,TITLE,PROMOPERIODFROM,PROMOPERIODTO,PREPAREDBY,STATUS,NULL AS CRID, REQUESTID, 'REG' AS REQUESTTYPE FROM Memos " & _
                                            "UNION ALL " & _
                                            "SELECT MEMOID,REQUESTDATE AS MEMODATE,TITLE,PROMOPERIODFROM,PROMOPERIODTO, REQUESTEDBY AS PREPAREDBY,STATUS,CRID AS CRID, REQUESTID, REQUESTTYPE FROM dbo.ChangeRequests) A " & _
                                            "WHERE " & cboFilterStatus.SelectedValue & " " & _
                                            "ORDER BY PromoPeriodFrom"

                        .Select(DataSourceSelectArguments.Empty)
                        gridRequests.DataBind()

                        gridRequests.Visible = True

                    End With

                End If
        End Select

    End Sub

    Protected Sub cboFilterStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilterStatus.SelectedIndexChanged

        clsSession.CurrFilterSelection = cboFilterStatus.SelectedIndex

    End Sub


End Class
