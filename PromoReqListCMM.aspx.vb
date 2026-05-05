Imports System.Data

Partial Class PromoReqListCMM
    Inherits System.Web.UI.Page

    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblRequestID")
        Dim sRedTag As String = ""

        'If CDate(txtPeriodFrom.Text) <= DateAdd(DateInterval.Day, 10, Today()) Then
        '    blistErrorMsg.Items.Add("Request must be at least 10 days before the Promo Period.")
        'End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            If (DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4 _
               And e.Row.Cells(5).Text <> "Approved") Or (e.Row.Cells(5).Text = "Returned") Then sRedTag = "style='color: red'"

            e.Row.Cells(2).ToolTip = e.Row.Cells(2).Text

            If Len(e.Row.Cells(2).Text) > 50 Then
                e.Row.Cells(2).Text = Left(e.Row.Cells(2).Text, 45) & "..."
            End If

            e.Row.Cells(2).Text = "<a href='PromoRequest.aspx?RequestID=" & lb.Text & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0) Or (SystemUser.UserLevel > SystemUser.UserRoles.PromoRequestor) Then Response.Redirect("InvalidAccess.aspx")

        ' clear selection
        clsSession.CurrRequestID = 0
        clsSession.CurrPromoID = 0

        ' ** 20161124 **
        ' initialize action buttons
        lnkAddRequest.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Or SystemUser.UserLevel = SystemUser.UserRoles.Administrator)
        If lnkAddRequest.Visible Then lnkAddRequest.Enabled = clsPromo.GetAddRequestButtonState()
        If lnkTemplate.Visible Then lnkTemplate.Enabled = clsPromo.GetAddRequestButtonState()

        If Not IsPostBack() Then

            With cboFilterStatus
                .Items.Add(New ListItem("Active Promo Requests", "DATEDIFF(d, GETDATE(), PromoPeriodTo) >= 0 AND Status <> 'Draft'"))
                .Items.Add(New ListItem("Ended Promo Requests", "DATEDIFF(d, GETDATE(), PromoPeriodTo) < 0 AND Status <> 'Draft'"))
                .Items.Add(New ListItem("Drafts", "Status = 'Draft'"))

                .SelectedIndex = clsSession.CurrFilterSelection
                clsSession.CurrFilterSelection = 0

            End With

        End If

        With sqldsRequests
            .SelectCommand = "SELECT * FROM [PromoRequests] WHERE UserID = " & SystemUser.UserID & _
                             " AND " & cboFilterStatus.SelectedValue & " ORDER BY [PromoPeriodFrom]"
            .Select(DataSourceSelectArguments.Empty)
        End With

        gridRequests.DataBind()
    End Sub

    Protected Sub lnkAddRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddRequest.Click

        clsSession.MsgTransFlowFlag = 1
        clsSession.CurrRequestID = 0
        Response.Redirect("PromoRequestEntry.aspx")

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        clsSession.CurrFilterSelection = cboFilterStatus.SelectedIndex

    End Sub

    Protected Sub lnkTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkTemplate.Click
        Server.Transfer("CreateTransactionTemplate.aspx")
    End Sub
End Class
