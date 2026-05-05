
Partial Class PromoReqList
    Inherits System.Web.UI.Page

    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblRequestID")
        Dim sRedTag As String = ""

        'If CDate(txtPeriodFrom.Text) <= DateAdd(DateInterval.Day, 10, Today()) Then
        '    blistErrorMsg.Items.Add("Request must be at least 10 days before the Promo Period.")
        'End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            If DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4 Then sRedTag = "style='color: red'"

            e.Row.Cells(2).ToolTip = e.Row.Cells(2).Text

            If Len(e.Row.Cells(2).Text) > 45 Then
                e.Row.Cells(2).Text = Left(e.Row.Cells(2).Text, 45) & "..."
            End If

            e.Row.Cells(2).Text = "<a href='PromoRequest.aspx?RequestID=" & lb.Text & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '::ToDo:: authenticate User

        If SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover Or _
             SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer Then

            ' clear selection
            clsSession.CurrRequestID = 0
            clsSession.CurrPromoID = 0

            Dim sDocumentStatus As String

            If SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer Then
                ' Reviewer sees only reviewers' pending approvals
                sDocumentStatus = "'For Mdsg/Group Head Approval'"
            ElseIf SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover Then
                'Mantis#67891 Approver sees BOTH SBU and non-SBU approvals
                sDocumentStatus = "'For MBU Approval','For BU Head Approval'"
            End If


            If Not IsPostBack() Then

                With cboFilterStatus
                    .Items.Add(New ListItem("Requests Awaiting Your Approval", "Status IN (" & sDocumentStatus & ")"))
                    .Items.Add(New ListItem("Active Promo Requests", "PromoPeriodTo >= GETDATE() AND Status <> 'Draft' AND Status NOT IN (" & sDocumentStatus & ")"))
                    .Items.Add(New ListItem("Ended Promo Requests", "PromoPeriodTo < GETDATE() AND Status <> 'Draft' AND Status NOT IN (" & sDocumentStatus & ")"))
                End With


                If clsSession.CurrFilterSelection > cboFilterStatus.Items.Count() Then
                    cboFilterStatus.SelectedIndex = 0
                Else
                    cboFilterStatus.SelectedIndex = clsSession.CurrFilterSelection
                End If

            End If

            With sqldsRequests
                .SelectCommand = "USP_GetPromoRequestsByStatusAndUser"
                .SelectCommandType = SqlDataSourceCommandType.StoredProcedure
                .SelectParameters.Clear()
                .SelectParameters.Add("ApprovalStatus", cboFilterStatus.SelectedValue)
                .SelectParameters.Add("UserID", SystemUser.UserID)
            End With

            If cboFilterStatus.SelectedIndex = 0 Then
                gridRequests.Columns(5).Visible = True
                gridRequests.Columns(6).Visible = False
            Else
                gridRequests.Columns(5).Visible = False
                gridRequests.Columns(6).Visible = True
            End If

            gridRequests.DataBind()

        Else

            Response.Redirect("InvalidAccess.aspx")

        End If

    End Sub

    
    Protected Sub cboFilterStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilterStatus.SelectedIndexChanged

        clsSession.CurrFilterSelection = cboFilterStatus.SelectedIndex

    End Sub
End Class
