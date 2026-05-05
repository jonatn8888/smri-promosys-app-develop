Imports System.Data
Imports System.Collections.Generic

Partial Class SearchBranch
    Inherits System.Web.UI.Page

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        LoadGridData()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            txtBranchCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            LoadGridData()
        End If

    End Sub

    Private Sub LoadGridData()
        Dim dtTable As New DataTable
        Dim strQuery As String
        Dim strBranchCode As String = txtBranchCode.Text
        Dim strDescription As String = txtBranchName.Text

        strQuery = "EXEC USP_SEARCHBRANCH '{0}','{1}'"
        strQuery = String.Format(strQuery, strBranchCode, strDescription)
        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
            gridSearchResult.DataSource = dtTable
            gridSearchResult.DataBind()
        End If

    End Sub


    'Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
    '    Dim strOutput As String
    '    Dim List As List(Of String) = New List(Of String)
    '    If Not Session("SelectedItemList") Is Nothing Then
    '        List = CType(Session("SelectedItemList"), List(Of String))
    '        strOutput = String.Join(",", List.ToArray())
    '        hidOutputValue.Value = strOutput
    '    End If
    '    ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.searchWindow.hide();</script>")
    'End Sub

    Protected Sub gridSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridSearchResult.RowCommand
        If e.CommandName.ToLower = "select" Then

            'Dim sTargetBranch() As String = Split(txtSearchStore.Text.ToString, "-")
            'Dim sParam As String
            'sParam = "<script>openViewComparison('{0}','{1}','{2}','Promotions not in {3}');</script>"
            'sParam = String.Format(sParam, txtTargetDate.Text, sTargetBranch(0).Trim, e.CommandArgument.ToString, sTargetBranch(1).Trim)

            Dim strDetails() As String

            strDetails = Split(e.CommandArgument, "-")

            txtBranchCode.Text = strDetails(0).Trim
            txtBranchName.Text = strDetails(1).Trim

            hidOutputValue.Value = e.CommandArgument.ToString
            ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.searchWindow.hide();</script>")
        End If

    End Sub
End Class
