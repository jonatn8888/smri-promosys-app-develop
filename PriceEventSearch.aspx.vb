
Imports dsPromotionsTableAdapters

Partial Class PriceEventSearch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' ------- testing variables -------------
        'Session("UserID") = 2790
        'Session("SignName") = "Testing"
        'Session("UserLevel") = SystemUser.UserRoles.AnnouncementViewer
        'Session("UserSignPosition") = "Tester"
        ''Session("UserGroupID") = 23
        'Session("UserAccessSettings") = 0
        ' ---------------------------------------

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0) Then Response.Redirect("InvalidAccess.aspx")

        ' setup default codes for viewers
        If Not IsPostBack Then

            Me.Title = "ProMoSys - Price Event Report"
            optChainBranches.Checked = True

        Else

            sqldsPriceEvents.SelectCommand = ViewState("sFilterQuery")

        End If

    End Sub

    Protected Sub cmdFilterList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFilterList.Click

        Dim sFilter As String = ""

        ' validate entries
        blistErrorMsg.Items.Clear()

        If cboReportType.SelectedValue = "0" Then
            blistErrorMsg.Items.Add("Report type not specified.")
        Else
            sFilter = "TranType = '" & cboReportType.SelectedValue & "'"
        End If

        ' require user to select a branch
        If optChainBranches.Checked Then

            If cboEnvironments.SelectedValue = "0" Then
                blistErrorMsg.Items.Add("No environment selected for chain branches.")
            Else
                sFilter &= IIf(sFilter = "", "", " AND ") & "Environment = 0" & cboEnvironments.SelectedValue

                Dim sBranchCodeList As String = ""
                Dim i As Integer

                For i = 1 To cboBranch.Items.Count - 1
                    sBranchCodeList &= IIf(sBranchCodeList = "", "", ", ") & cboBranch.Items(i).Value
                Next

                sFilter &= IIf(sFilter = "", "", " AND ") & "BranchCode IN ( 0" & sBranchCodeList & ")"

            End If

        Else
            If cboBranch.SelectedValue = "0" Then
                blistErrorMsg.Items.Add("No branch selected.")
            Else
                sFilter &= IIf(sFilter = "", "", " AND ") & "BranchCode = '" & cboBranch.SelectedValue & "'"
            End If
        End If

            If txtEventNumber.Text.Trim() <> "" Then
                sFilter &= IIf(sFilter = "", "", " AND ") & "EventNumber = '" & Right("000000" + txtEventNumber.Text.Trim(), 6) & "'"
            End If

            ' limit departments from list only
            If cboDepartment.SelectedValue = "0" Then
                Dim sDeptCodeList As String = ""
                Dim i As Integer

                For i = 1 To cboDepartment.Items.Count - 1
                    sDeptCodeList &= IIf(sDeptCodeList = "", "", ", ") & cboDepartment.Items(i).Value
                Next

                sFilter &= IIf(sFilter = "", "", " AND ") & "DeptCode IN ( 0" & sDeptCodeList & ")"

            Else
                sFilter &= IIf(sFilter = "", "", " AND ") & "DeptCode = '" & cboDepartment.SelectedValue & "'"
            End If


            ' effectivity dates
            If txtPeriodFrom.Text.Trim() <> "" Then
                sFilter &= IIf(sFilter = "", "", " AND ") & "EventStartDate = '" & txtPeriodFrom.Text.Trim() & "'"
            End If

            If txtPeriodTo.Text.Trim() <> "" Then
                sFilter &= IIf(sFilter = "", "", " AND ") & "EventEndDate = '" & txtPeriodTo.Text.Trim() & "'"
            End If

            ' complete filter condition
            If sFilter <> "" Then sFilter = "WHERE " & sFilter & " "

            If blistErrorMsg.Items.Count = 0 Then
                With sqldsPriceEvents

                    ' show only first 500 results to avoid operation time-out
                ViewState("sFilterQuery") = "SELECT TranType, BranchCode, DeptCode, MAX(EventStartDate) AS EventStartDate, MAX(EventEndDate) AS EventEndDate, EventNumber, MAX(EventDesc) AS EventDesc " & _
                                                "FROM PriceEvents " & sFilter & _
                                                "GROUP BY EventNumber, BranchCode, TranType, DeptCode " & _
                                                "ORDER BY EventNumber"
                    .SelectCommand = ViewState("sFilterQuery")
                    .Select(DataSourceSelectArguments.Empty)

                    gridPriceEvents.DataBind()
                    gridPriceEvents.Visible = True
                End With
            End If

    End Sub

    Protected Sub CheckEntries()

        '    Dim taPriceEventAdapter As New dsPromotionsTableAdapters.PriceEventsTableAdapter()
        '    Dim dtblPriceEvent As dsPromotions.PriceEventsDataTable

        '    Dim sEventNumber As String = ""

        '    ' validate entries
        '    blistErrorMsg.Items.Clear()

        '    If cboReportType.SelectedValue = "0" Then
        '        blistErrorMsg.Items.Add("Select type of report to generate.")
        '        Exit Sub
        '    End If

        '    If cboBranch.SelectedValue = "0" Then
        '        blistErrorMsg.Items.Add("No Location/Branch selected.")
        '    End If

        '    If txtEventNumber.Text.Trim() = "" Then
        '        blistErrorMsg.Items.Add("Event number not specified.")
        '    Else

        '        'verify if event-branch combination exists
        '        sEventNumber = Right("000000" + txtEventNumber.Text.Trim(), 6)

        '        dtblPriceEvent = taPriceEventAdapter.GetPriceEventByEventNo(sEventNumber, cboReportType.SelectedValue, cboBranch.SelectedValue)
        '        If dtblPriceEvent.Rows.Count() = 0 Then
        '            blistErrorMsg.Items.Add("Invalid Event-Location combination.")
        '        End If

        '    End If

        '    If blistErrorMsg.Items.Count > 0 Then Exit Sub

        '    'clsSession.rptEventNumber = sEventNumber
        '    'clsSession.rptTranType = cboReportType.SelectedValue
        '    'clsSession.rptBranchCode = cboBranch.SelectedValue

        '    Response.Redirect("rvPriceEventPreview.aspx")

    End Sub

    Protected Sub gridPriceEvents_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPriceEvents.RowDataBound

        Dim lblBrCode As Label = e.Row.FindControl("lblBranchCode")

        If e.Row.RowType = DataControlRowType.DataRow Then

            ' hyperlinks - following lines must be in order
            e.Row.Cells(2).Text = "<a href='rvPriceEventPreview.aspx?TranType=" & e.Row.Cells(3).Text & "&EventNumber=" & e.Row.Cells(1).Text & "&BranchCode=" & lblBrCode.Text & "&DeptCode=" & e.Row.Cells(4).Text & "'>" & e.Row.Cells(2).Text & "</a>"
            e.Row.Cells(1).Text = "<a href='rvPriceEventPreview.aspx?TranType=" & e.Row.Cells(3).Text & "&EventNumber=" & e.Row.Cells(1).Text & "&BranchCode=" & lblBrCode.Text & "&DeptCode=" & e.Row.Cells(4).Text & "'>" & e.Row.Cells(1).Text & "</a>"

        End If

    End Sub

    Protected Sub UpdateLocationSelection()

        If optChainBranches.Checked Then
            lblEnvironment.Enabled = True
            cboEnvironments.Enabled = True

            lblBranch.Enabled = False
            cboBranch.Enabled = False
        Else
            lblEnvironment.Enabled = False
            cboEnvironments.Enabled = False

            lblBranch.Enabled = True
            cboBranch.Enabled = True
        End If

    End Sub

    Protected Sub optChainBranches_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optChainBranches.CheckedChanged
        UpdateLocationSelection()
    End Sub

    Protected Sub optSingleBranch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optSingleBranch.CheckedChanged
        UpdateLocationSelection()
    End Sub

End Class
