Imports DataSet1TableAdapters

Partial Class PrintRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim taMemos As New DataSet1TableAdapters.MemosTableAdapter()
        Dim dtMemos As DataSet1.MemosDataTable
        'Dim rowMemos As DataSet1.MemosRow

        dtMemos = taMemos.GetMemoByID(Request("CurrMemoID"))

        'rvMemo
    End Sub
End Class
