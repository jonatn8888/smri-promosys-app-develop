Imports System.Data.SqlClient
Imports System.Data


Partial Class MemoBranches
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadBranches()
    End Sub

    Private Sub LoadBranches()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT RIGHT('0000'+CAST(PB.BranchCode AS varchar(4)),4) AS BranchCode, PB.ShortName AS BranchName, V.ElementName AS VSstatusDesc " & _
                    "FROM PromoBranch AS PB " & _
                    "INNER JOIN Promotions AS P ON P.PromoID = PB.PromoID " & _
                    "LEFT JOIN ResListValues AS V ON V.ElementValue = PB.VSstatus " & _
                    "WHERE V.GroupName = 'VSfeedbackStatus' " & _
                    "AND P.MemoID = 0" & clsSession.CurrMemoID

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
            gridMemoBranches.DataSource = dtTable
            gridMemoBranches.DataBind()
        Else
            ' error
        End If

        ' cleanup
        dtTable = Nothing

    End Sub

End Class
