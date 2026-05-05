
Partial Class PromoReqListExec
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

    Private Sub FillBizUnitList()

        'Dim dvPromo As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        'If dvPromo.Table.Rows.Count = 0 Then
        '    NewPromoEventCode = Format(nSeriesStart, "0###")
        'Else
        '    ' create new event number
        '    Dim dr As DataRow = dvPromo.Table.Rows(0)
        '    NewPromoEventCode = Format(Val(dr("PromoEventCode")) + 1, "0###")
        'End If

        'sqldsData.SelectCommand = ""

        With cboBizUnit
            '.Items.Add(New ListItem("All Units", "%"))
            .Items.Add(New ListItem("AMC", "AMC"))
            .Items.Add(New ListItem("Baby Company", "Baby Company"))
            .Items.Add(New ListItem("CFMC", "CFMC"))
            .Items.Add(New ListItem("CM", "CM")) ' Added dowcarpio10292012@smretailinc: Additional group  for CM
            .Items.Add(New ListItem("Hardware – ACE", "Hardware – ACE"))
            .Items.Add(New ListItem("HomeWorld", "HomeWorld"))
            .Items.Add(New ListItem("KSI", "KSI"))
            .Items.Add(New ListItem("LFMC", "LFMC"))
            .Items.Add(New ListItem("LTBG", "LTBG"))
            .Items.Add(New ListItem("MFMC", "MFMC"))
            .Items.Add(New ListItem("MISC", "MISC"))
            .Items.Add(New ListItem("SACI", "SACI"))
            .Items.Add(New ListItem("SBU", "SBU"))
            .Items.Add(New ListItem("SLI", "SLI"))
            .Items.Add(New ListItem("Sports Central", "Sports Central"))
            .Items.Add(New ListItem("Supplies Station", "Supplies Station"))
            .Items.Add(New ListItem("Surplus Shop", "Surplus Shop"))
            .Items.Add(New ListItem("Toys", "Toys"))
            .Items.Add(New ListItem("Watsons", "Watsons"))
        End With

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '::ToDo:: authenticate User
        If (SystemUser.UserID = 0) Or (SystemUser.UserLevel <> SystemUser.UserRoles.ExecutiveApprover) Then Response.Redirect("InvalidAccess.aspx")

        ' clear selection
        clsSession.CurrRequestID = 0
        clsSession.CurrPromoID = 0

        If Not IsPostBack() Then

            With cboFilterStatus
                .Items.Add(New ListItem("Requests Awaiting Your Approval", "Status = 'For HTS Approval'"))
                .Items.Add(New ListItem("Active Promo Requests", "PromoPeriodTo >= GETDATE() AND Status <> 'Draft' AND Status <> 'For HTS Approval'"))
                .Items.Add(New ListItem("Ended Promo Requests", "PromoPeriodTo < GETDATE() AND Status <> 'Draft' AND Status <> 'For HTS Approval'"))
            End With

            If clsSession.CurrFilterSelection > cboFilterStatus.Items.Count() Then
                cboFilterStatus.SelectedIndex = 0
            Else
                cboFilterStatus.SelectedIndex = clsSession.CurrFilterSelection
            End If

            FillBizUnitList()

        End If

        With sqldsRequests
            .SelectCommand = "SELECT * FROM [PromoRequests] WHERE " & cboFilterStatus.SelectedValue & " AND Status <> 'Archived'" & _
                             " AND OwnerGroup IN (SELECT GroupID FROM UserGroups WHERE BizUnit LIKE '" & cboBizUnit.SelectedValue & "')" & _
                             " ORDER BY [PromoPeriodFrom]"

            ' " (SELECT BizUnit FROM UserGroups WHERE GroupID = " & SystemUser.UserGroupID & "))" & _
            ' " ORDER BY [PromoPeriodFrom]"

            .Select(DataSourceSelectArguments.Empty)
        End With

        If cboFilterStatus.SelectedIndex = 0 Then
            gridRequests.Columns(5).Visible = True
            gridRequests.Columns(6).Visible = False
        Else
            gridRequests.Columns(5).Visible = False
            gridRequests.Columns(6).Visible = True
        End If

        gridRequests.DataBind()
    End Sub


    Protected Sub cboFilterStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilterStatus.SelectedIndexChanged

        clsSession.CurrFilterSelection = cboFilterStatus.SelectedIndex

    End Sub
End Class
