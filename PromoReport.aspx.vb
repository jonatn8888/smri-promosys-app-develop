Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class PromoReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If Not Page.IsPostBack Then
            txtTargetDate.Text = Format(Now(), "MM/dd/yyyy")
            hfStorecodes.Value = Nothing

        End If
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        ' Validate Selected Branch/Stores
        blistErrorMsg.Items.Clear()
        If txtSearchStore.Text = "" And ddlReportType.SelectedValue = 1 Then
            blistErrorMsg.Items.Add("No selected Branch.")
        End If
        If blistErrorMsg.Items.Count > 0 Then
            Exit Sub
        End If

        btnPromoTypes_Click(sender, e)
        btnBranches_Click(sender, e)

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        Dim sRepParams As String = ""
        hfRepParams.Value = ""

        sRepParams = "rvReport.aspx?SelectedValue={0}&TargetDate='{1}'&StoreCodes='{2}'&PromoTypes='{3}'&StoreDesc='{4}'&PromoDesc='{5}'"
        Select Case ddlReportType.SelectedValue
            Case 1 'Branch Comparison of Ongoing Promotions
                Dim strStorecodes() As String = hfStorecodes.Value.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                Dim sBranchCode() As String = Split(strStorecodes(0).ToString, "-")
                strQuery = "exec USP_GetPromoReport {0},'{1}',{2}"
                strQuery = String.Format(strQuery, ddlReportType.SelectedValue, txtTargetDate.Text, sBranchCode(0).Trim)
                If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
                    gridOption1.DataSource = dtTable
                    gridOption1.DataBind()
                End If

                hfRepParams.Value = String.Format(sRepParams _
                    , ddlReportType.SelectedValue _
                    , txtTargetDate.Text _
                    , sBranchCode(0).Trim _
                    , "" _
                    , txtSearchStore.Text _
                    , "")

                panOption1.Visible = True

            Case 2 'Ongoing & Future Dated Promotions

                strQuery = "exec USP_GetPromoReport {0},'{1}','{2}','{3}'"
                'strQuery = String.Format(strQuery, ddlReportType.SelectedValue, txtTargetDate.Text, hfStorecodes.Value, hfPromoTypeId.Value)
                strQuery = String.Format(strQuery, ddlReportType.SelectedValue, txtTargetDate.Text, hfStoreNo.Value, hfPromoTypeId.Value)
                If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
                    gridOption2.DataSource = dtTable
                    gridOption2.DataBind()
                End If

                ', hfStorecodes.Value 
                hfRepParams.Value = String.Format(sRepParams _
                    , ddlReportType.SelectedValue _
                    , txtTargetDate.Text _
                    , hfStoreNo.Value _
                    , hfPromoTypeId.Value _
                    , txtSearchStore.Text _
                    , txtPromoType.Text)

                panOption2.Visible = True

            Case 3 'Approved Promotions on a Given Date

                strQuery = "exec USP_GetPromoReport {0},'{1}','{2}','{3}'"
                'strQuery = String.Format(strQuery, ddlReportType.SelectedValue, txtTargetDate.Text, hfStorecodes.Value, hfPromoTypeId.Value)
                strQuery = String.Format(strQuery, ddlReportType.SelectedValue, txtTargetDate.Text, hfStoreNo.Value, hfPromoTypeId.Value)
                If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
                    gridOption3.DataSource = dtTable
                    gridOption3.DataBind()
                End If

                ', hfStorecodes.Value 
                hfRepParams.Value = String.Format(sRepParams _
                    , ddlReportType.SelectedValue _
                    , txtTargetDate.Text _
                    , hfStoreNo.Value _
                    , hfPromoTypeId.Value _
                    , txtSearchStore.Text _
                    , txtPromoType.Text)

                panOption3.Visible = True

            Case 4 'Count of Active Promo IDs for Regular Price Items only

                strQuery = "exec USP_GetPromoReport {0},'{1}','{2}'"
                'strQuery = String.Format(strQuery, ddlReportType.SelectedValue, txtTargetDate.Text, hfStorecodes.Value)
                strQuery = String.Format(strQuery, ddlReportType.SelectedValue, txtTargetDate.Text, hfStoreNo.Value)
                If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
                    gridOption4.DataSource = dtTable
                    gridOption4.DataBind()
                End If

                ', hfStorecodes.Value 
                hfRepParams.Value = String.Format(sRepParams _
                    , ddlReportType.SelectedValue _
                    , txtTargetDate.Text _
                    , hfStoreNo.Value _
                    , "" _
                    , txtSearchStore.Text _
                    , "")

                panOption4.Visible = True


            Case 5 'Promotions for Manual Processing
                ' 29-01-2019 Additional Reports
                Dim sDays As String = DateDiff(DateInterval.Day, Now.Date, DateTime.Parse(txtTargetDate.Text)).ToString()
                strQuery = "exec usp_GetPromoForManualProcess_Test {0}"
                strQuery = String.Format(strQuery, sDays)
                If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
                    gridOption5.DataSource = dtTable
                    gridOption5.DataBind()
                End If

                ', hfStorecodes.Value 
                ' Use StoreCodes as sDays
                hfRepParams.Value = String.Format(sRepParams _
                    , ddlReportType.SelectedValue _
                    , txtTargetDate.Text _
                    , sDays _
                    , "" _
                    , "" _
                    , "")

                panOption5.Visible = True

            Case 6 'Promo Exclusion List for Automated Loyalty & Coupon Promos
                ' 30-01-2019 Additional Reports
                Dim sDays As String = DateDiff(DateInterval.Day, Now.Date, DateTime.Parse(txtTargetDate.Text)).ToString()
                strQuery = "exec usp_GetPromoExclusionList {0},{1}"
                strQuery = String.Format(strQuery, sDays, ddlType.SelectedValue)
                If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
                    gridOption6.DataSource = dtTable
                    gridOption6.DataBind()
                End If

                ', hfStorecodes.Value 
                ' Use StoreCodes for sDays
                ' Use PromoTypes for Type ID
                ' Use PromoDesc for Type Description
                hfRepParams.Value = String.Format(sRepParams _
                    , ddlReportType.SelectedValue _
                    , txtTargetDate.Text _
                    , sDays _
                    , ddlType.SelectedValue _
                    , "" _
                    , ddlType.SelectedItem)

                panOption6.Visible = True

        End Select

        dtTable = Nothing
        GC.Collect()
        btnPrint.Enabled = True
    End Sub

    Protected Sub gridOption2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridOption2.RowDataBound
        Dim lb As Label = e.Row.FindControl("lblMemoID")
        'Dim sRedTag As String = ""

        If e.Row.RowType = DataControlRowType.DataRow Then
            'If DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4 Then sRedTag = "style='color: red'"

            e.Row.Cells(2).ToolTip = e.Row.Cells(2).Text
            If Len(e.Row.Cells(2).Text) > 40 Then
                e.Row.Cells(2).Text = Left(e.Row.Cells(2).Text, 40) & "..."
            End If
            'e.Row.Cells(2).Text = "<a href='ViewMemo.aspx?MemoID=" & lb.Text & "'>" & e.Row.Cells(2).Text & "</a>"
            e.Row.Cells(2).Text = "<a href='ViewMemo.aspx?MemoID=" & lb.Text & "&Target=Report'>" & e.Row.Cells(2).Text & "</a>"

            e.Row.Cells(3).ToolTip = e.Row.Cells(3).Text
            If Len(e.Row.Cells(3).Text) > 20 Then
                e.Row.Cells(3).Text = Left(e.Row.Cells(3).Text, 20) & "..."
            End If

            e.Row.Cells(4).ToolTip = e.Row.Cells(4).Text
            If Len(e.Row.Cells(4).Text) > 20 Then
                e.Row.Cells(4).Text = Left(e.Row.Cells(4).Text, 20) & "..."
            End If

        End If
    End Sub

    Protected Sub gridOption3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridOption3.RowDataBound
        Dim lb As Label = e.Row.FindControl("lblMemoID")

        If e.Row.RowType = DataControlRowType.DataRow Then
            'If DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4 Then sRedTag = "style='color: red'"

            e.Row.Cells(2).ToolTip = e.Row.Cells(2).Text
            If Len(e.Row.Cells(2).Text) > 30 Then
                e.Row.Cells(2).Text = Left(e.Row.Cells(2).Text, 30) & "..."
            End If
            'e.Row.Cells(2).Text = "<a href='ViewMemo.aspx?MemoID=" & lb.Text & "'>" & e.Row.Cells(2).Text & "</a>"
            e.Row.Cells(2).Text = "<a href='ViewMemo.aspx?MemoID=" & lb.Text & "&Target=Report'>" & e.Row.Cells(2).Text & "</a>"

            e.Row.Cells(3).ToolTip = e.Row.Cells(3).Text
            If Len(e.Row.Cells(3).Text) > 10 Then
                e.Row.Cells(3).Text = Left(e.Row.Cells(3).Text, 10) & "..."
            End If

            e.Row.Cells(4).ToolTip = e.Row.Cells(4).Text
            If Len(e.Row.Cells(4).Text) > 20 Then
                e.Row.Cells(4).Text = Left(e.Row.Cells(4).Text, 20) & "..."
            End If

        End If
    End Sub

    Protected Sub gridOption5_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridOption5.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblMemoID")

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(2).Text = "<a href='ViewMemo.aspx?MemoID=" & lb.Text & "&Target=Report'>" & e.Row.Cells(2).Text & "</a>"

            'If Len(e.Row.Cells(6).Text) > 40 Then e.Row.Cells(6).Text = Left(e.Row.Cells(6).Text, 37) & "..."
            'If Len(e.Row.Cells(3).Text) > 50 Then e.Row.Cells(3).Text = Left(e.Row.Cells(3).Text, 50) & "..."

        End If

    End Sub

    Protected Sub gridOption1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridOption1.RowCommand
        If e.CommandName.ToUpper = "SELECT" Then

            Dim sTargetBranch() As String = Split(txtSearchStore.Text.ToString, "-")
            Dim sSourceBranch() As String = Split(e.CommandArgument.ToString, "-")
            Dim sParam As String
            sParam = "<script>openViewComparison('{0}','{1}','{2}','Promotions not in {3}');</script>"
            sParam = String.Format(sParam, txtTargetDate.Text, sTargetBranch(0).Trim, sSourceBranch(0).Trim, e.CommandArgument.ToString)

            'MsgBox(sParam)
            ClientScript.RegisterStartupScript(Me.GetType, "key", sParam)

        End If

    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Response.Redirect(hfRepParams.Value)
    End Sub

    Protected Sub lnkBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBranch.Click
        If ddlReportType.SelectedValue = 1 Then
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearchBranch();</script>")
        Else
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearchStore();</script>")
        End If
    End Sub

    Protected Sub lnkPromoType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoType.Click
        If ddlReportType.SelectedValue <> 1 Then
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearchPromoType();</script>")
        End If
    End Sub

    Protected Sub btnPromoTypes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPromoTypes.Click
        ' Validate Selected PromoType
        Dim strPromoTypes() As String = hfPromotypes.Value.Split(New Char() {"|"}, StringSplitOptions.RemoveEmptyEntries)
        hfPromoTypeId.Value = ""
        'Dim sPromoTypeID As String = ""
        Dim sPromoTypeDesc As String = ""
        If (strPromoTypes.Length() > 0) Then
            For Each strPromoType As String In strPromoTypes
                Dim sPromoType() As String = strPromoType.ToString.Split(New Char() {"="}, StringSplitOptions.RemoveEmptyEntries)
                Dim sDelimeter As String = IIf(hfPromoTypeId.Value = "", "", ",")
                hfPromoTypeId.Value = hfPromoTypeId.Value + sDelimeter + Right("00000" + sPromoType(0).ToString.Trim, 5)
                sPromoTypeDesc = sPromoTypeDesc + sDelimeter + sPromoType(1).ToString
            Next

            'txtPromoType.Text = sPromoTypeDesc
            If hfPromoDesc.Value <> "" Then
                txtPromoType.Text = hfPromoDesc.Value
            Else
                txtPromoType.Text = sPromoTypeDesc
            End If

        Else
            txtPromoType.Text = "All PromoTypes"
        End If
    End Sub

    Protected Sub btnBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBranches.Click
        Dim strStorecodes() As String = hfStorecodes.Value.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
        blistErrorMsg.Items.Clear()
        If strStorecodes.Length() = 0 And ddlReportType.SelectedValue = 1 Then
            blistErrorMsg.Items.Add("No selected Branch.")
        End If
        If blistErrorMsg.Items.Count > 0 Then
            btnRefresh.Enabled = False
            Exit Sub
        Else
            btnRefresh.Enabled = True
        End If
        hfStoreNo.Value = ""
        If (strStorecodes.Length() > 0) Then
            If ddlReportType.SelectedValue = 1 Then
                txtSearchStore.Text = strStorecodes(0).ToString
            Else
                Dim sStoreDesc As String = ""
                For Each strStorecode As String In strStorecodes
                    Dim sStorecode() As String = strStorecode.ToString.Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)
                    Dim sDelimeter As String = IIf(sStoreDesc = "", "", ",")
                    hfStoreNo.Value = hfStoreNo.Value + sDelimeter + sStorecode(0).ToString
                    sStoreDesc = sStoreDesc + sDelimeter + sStorecode(1).ToString
                Next

                txtSearchStore.Text = sStoreDesc
                If hfStoreDesc.Value <> "" Then
                    txtSearchStore.Text = hfStoreDesc.Value
                Else
                    txtSearchStore.Text = sStoreDesc
                End If
            End If
        Else
            txtSearchStore.Text = "All Stores"
        End If
    End Sub

    Protected Sub ddlReportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles ddlReportType.SelectedIndexChanged 'Handles ddlReportType.SelectedIndexChanged
        If ddlReportType.SelectedValue > 0 Then
            tableFilterParam.Visible = True
        Else
            tableFilterParam.Visible = False
        End If

        blistErrorMsg.Items.Clear()

        btnPrint.Enabled = False
        txtSearchStore.Text = ""
        txtPromoType.Text = ""

        hfStorecodes.Value = ""
        hfStoreNo.Value = ""
        hfPromotypes.Value = ""
        hfPromoTypeId.Value = ""
        hfRepParams.Value = ""
        hfPromoDesc.Value = ""
        hfStoreDesc.Value = ""

        gridOption1.DataSource() = Nothing
        gridOption1.DataBind()
        gridOption2.DataSource() = Nothing
        gridOption2.DataBind()
        gridOption3.DataSource() = Nothing
        gridOption3.DataBind()
        gridOption4.DataSource() = Nothing
        gridOption4.DataBind()
        gridOption5.DataSource() = Nothing
        gridOption5.DataBind()
        gridOption6.DataSource() = Nothing
        gridOption6.DataBind()

        panOption1.Visible = False
        panOption2.Visible = False
        panOption3.Visible = False
        panOption4.Visible = False
        panOption5.Visible = False
        panOption6.Visible = False

        tdlnkBranch.Visible = True ' 29-01-2019 Additional Reports
        tdtxtBranch.Visible = True ' 29-01-2019 Additional Reports

        tdlblType.Visible = False ' 30-01-2019 Additional Reports
        tdddlType.Visible = False ' 30-01-2019 Additional Reports

        If (ddlReportType.SelectedValue = 2 Or ddlReportType.SelectedValue = 3) Then
            trPromoType.Visible = True
        Else
            trPromoType.Visible = False

            If (ddlReportType.SelectedValue = 5 Or ddlReportType.SelectedValue = 6) Then
                tdlnkBranch.Visible = False
                tdtxtBranch.Visible = False

                If ddlReportType.SelectedValue = 6 Then
                    tdlblType.Visible = True ' 30-01-2019 Additional Reports
                    tdddlType.Visible = True ' 30-01-2019 Additional Reports
                End If
            End If
        End If

    End Sub

End Class
