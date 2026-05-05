Imports System.Data

Partial Class ViewUPCdetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ShowPromoUPCInfo(Request("RequestID"))
    End Sub

    Private Sub ShowPromoUPCInfo(ByVal nRequestID As Long)

        Dim dtTable As New DataTable
        Dim drRow As DataRow
        Dim strQuery As String

        strQuery = "SELECT dbo.fn_IsExclusionUPC(0" & nRequestID & ") AS IsExclusionUPC"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

            If drRow("IsExclusionUPC") = 0 Then
                lblListTitle.Text = "List of Eligible Items"
            Else
                lblListTitle.Text = "List of Exempted Items"
            End If

        End If

        ' load data to grid
        strQuery = "SELECT UPC.* FROM PromoUPCs AS UPC " & _
                    "INNER JOIN Promotions AS P ON P.PromoID = UPC.PromoID " & _
                    "WHERE P.RequestID = 0" & nRequestID.ToString

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridPromoUPC.DataSource = dtTable
        gridPromoUPC.DataBind()

        Dim nUPCcount As Long

        nUPCcount = gridPromoUPC.Rows.Count()

        If nUPCcount > 0 Then
            lblUPCcount.Text = "Total UPC Count: " & Format(nUPCcount, "#,##0")
            lblUPCcount.Visible = True
        Else
            lblUPCcount.Visible = False
        End If

    End Sub
End Class
