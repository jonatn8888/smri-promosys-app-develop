Imports System.Data
Imports System.Collections.Generic

Partial Class ViewDetailsComparison
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            LoadGridData(Request("PromoDate"), Request("TargetBranch"), Request("ToBranch"))
        End If
    End Sub

    Private Sub LoadGridData(ByVal PromoDate As String, ByVal TargetBranch As String, ByVal ToBranch As String)
        'MsgBox(Request("ToBranch"))
        Dim dtTable As New DataTable
        Dim strQuery As String

        strQuery = "EXEC [USP_BranchComparisonDetails] '{0}',{1},{2}"
        strQuery = String.Format(strQuery, PromoDate, TargetBranch, ToBranch)
        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
            gridviewresult.DataSource = dtTable
            gridviewresult.DataBind()
        End If

    End Sub

    'Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
    '    ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.searchWindow.hide();</script>")
    'End Sub

    Protected Sub gridviewresult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridviewresult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).ToolTip = e.Row.Cells(1).Text
            If Len(e.Row.Cells(1).Text) > 45 Then
                e.Row.Cells(1).Text = Left(e.Row.Cells(1).Text, 45) & "..."
            End If

            e.Row.Cells(4).ToolTip = e.Row.Cells(4).Text
            If Len(e.Row.Cells(4).Text) > 20 Then
                e.Row.Cells(4).Text = Left(e.Row.Cells(4).Text, 20) & "..."
            End If

        End If
    End Sub
End Class
