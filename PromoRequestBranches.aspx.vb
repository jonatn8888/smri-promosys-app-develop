Imports System.Data.SqlClient
Imports System.Data


Partial Class PromoRequestBranches
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadBranches()
    End Sub

    Private Sub LoadBranches()
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New Data.SqlClient.SqlCommand("USP_SelectPromoBranches", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlCmd.Parameters.Add("@RequestID", SqlDbType.VarChar)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID
        Dim da As New Data.SqlClient.SqlDataAdapter(sqlCmd)
        Dim ds As New Data.DataSet
        sqlConn.Open()
        da.Fill(ds, "tblBranches")
        Dim dt As Data.DataTable = ds.Tables("tblBranches")
        gvBranches.DataSource = dt
        gvBranches.DataBind()
        GC.Collect()
    End Sub

    Protected Sub gvBranches_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBranches.RowDataBound
        Const nCol As Integer = 1
        Static rowPrevious As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(nCol).Text = e.Row.Cells(nCol).Text
        End If

        If e.Row.RowIndex = 0 Then rowPrevious = e.Row

        If e.Row.RowIndex > 0 Then

            ' merge Description cells with same PromoID
            Dim lbPrev As Label = rowPrevious.FindControl("label2")
            Dim lbCurr As Label = e.Row.FindControl("label2")

            If lbCurr.Text = lbPrev.Text Then

                If rowPrevious.Cells(nCol).RowSpan < 2 Then
                    rowPrevious.Cells(0).RowSpan = 2
                    rowPrevious.Cells(nCol).RowSpan = 2
                Else
                    rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
                    rowPrevious.Cells(nCol).RowSpan = rowPrevious.Cells(nCol).RowSpan + 1
                End If

                e.Row.Cells(0).Visible = False
                e.Row.Cells(nCol).Visible = False
            Else
                rowPrevious = e.Row
            End If

        End If
    End Sub
End Class
