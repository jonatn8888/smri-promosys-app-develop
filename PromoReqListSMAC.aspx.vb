
Partial Class PromoReqListSMAC
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
        If (SystemUser.UserID = 0) Or (SystemUser.UserLevel <> SystemUser.UserRoles.SMACapprover) Then Response.Redirect("InvalidAccess.aspx")

        ' clear selection
        clsSession.CurrRequestID = 0
        clsSession.CurrPromoID = 0

        If Not IsPostBack() Then

            With cboFilterStatus
                .Items.Add(New ListItem("SMAC Deals Promo Awaiting Your Approval", "Status = 'For MCI Approval'"))
                .Items.Add(New ListItem("Promo Requests for Submission to MPD", "Status = 'For DTI Application'"))
                .Items.Add(New ListItem("Active SMAC Deals Promo Requests", _
                                        "RequestID IN (SELECT RequestID FROM Promotions WHERE PromoTypeID IN (48, 93)) AND PromoPeriodTo >= GETDATE() AND Status NOT IN ('For DTI Application','For MCI Approval','Draft')"))
            End With

            If clsSession.CurrFilterSelection > cboFilterStatus.Items.Count() Then
                cboFilterStatus.SelectedIndex = 0
            Else
                cboFilterStatus.SelectedIndex = clsSession.CurrFilterSelection
            End If

        End If

        With sqldsRequests
            .SelectCommand = " SELECT * FROM [PromoRequests] WHERE " & cboFilterStatus.SelectedValue & _
                             " ORDER BY [PromoPeriodFrom]"
            .Select(DataSourceSelectArguments.Empty)
        End With

        If cboFilterStatus.SelectedIndex = 2 Then
            gridRequests.Columns(5).Visible = False
            gridRequests.Columns(6).Visible = True
        Else
            gridRequests.Columns(5).Visible = True
            gridRequests.Columns(6).Visible = False
        End If

        gridRequests.DataBind()
    End Sub


    Protected Sub cboFilterStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilterStatus.SelectedIndexChanged

        clsSession.CurrFilterSelection = cboFilterStatus.SelectedIndex

    End Sub
End Class
