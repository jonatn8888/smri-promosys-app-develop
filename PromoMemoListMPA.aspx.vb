
Partial Class PromoMemoListMPA
    Inherits System.Web.UI.Page

    Protected Sub gridMemos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridMemos.RowDataBound

        Dim sRedTag As String = ""
        Dim lb As Label = e.Row.FindControl("lblMemoID")
        Dim lbStatus As Label = e.Row.FindControl("lblStatus")

        If e.Row.RowType = DataControlRowType.DataRow Then

            ' highlight memos with promo period nearing implementation
            If (lbStatus.Text <> "Approved") And (DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4) Then sRedTag = "style='color: red'"

            e.Row.Cells(2).Text = "<a href='PromoMemo.aspx?MemoID=" & lb.Text & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"
        End If

    End Sub

    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound

        Dim sRedTag As String = ""
        Dim lb As Label = e.Row.FindControl("lblRequestID")

        If e.Row.RowType = DataControlRowType.DataRow Then
            If DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4 Then sRedTag = "style='color: red'"

            e.Row.Cells(2).Text = "<a href='PromoRequest.aspx?RequestID=" & lb.Text & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel <> SystemUser.UserRoles.Analyst) Then Response.Redirect("InvalidAccess.aspx")

        clsSession.CurrMemoID = 0

        gridRequests.Visible = False
        gridMemos.Visible = False


        If Not IsPostBack() Then
            If clsSession.CurrFilterSelection > cboFilterStatus.Items.Count() Then
                cboFilterStatus.SelectedIndex = 0
            Else
                cboFilterStatus.SelectedIndex = clsSession.CurrFilterSelection
            End If
        End If

        If cboFilterStatus.SelectedIndex = 0 Then

            ' show promo requests for memo creation (status = 'For MPD Processing')
            ' exclude requests with corresponding memo
            sqldsDocs.SelectCommand = "SELECT * FROM [PromoRequests] WHERE Status = 'For MPD Processing' " & _
                                      "AND RequestID NOT IN (SELECT RequestID FROM Memos WHERE Status = 'Returned')" & _
                                      "AND OwnerGroup IN (SELECT GroupID FROM UserGroups WHERE BizUnit IN " & _
                                                        "(SELECT BizUnit FROM UserGroups WHERE GroupID IN " & _
                                                        "(SELECT GroupID FROM dbo.GroupAssignment WHERE UserID = " & SystemUser.UserID & "))) " & _
                                      "ORDER BY [PromoPeriodFrom]"

            sqldsDocs.Select(DataSourceSelectArguments.Empty)
            gridRequests.DataBind()

            gridRequests.Visible = True

        Else

            ' show memos filtered by status and user
            sqldsDocs.SelectCommand = "SELECT * FROM Memos WHERE " & cboFilterStatus.SelectedValue & _
                                      " AND UserID = " & SystemUser.UserID & _
                                      " ORDER BY MemoDate"

            sqldsDocs.Select(DataSourceSelectArguments.Empty)
            gridMemos.DataBind()

            gridMemos.Visible = True

        End If

    End Sub

    Protected Sub cboFilterStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilterStatus.SelectedIndexChanged

        clsSession.CurrFilterSelection = cboFilterStatus.SelectedIndex

    End Sub

End Class
