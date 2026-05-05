#Region " Imports "
Imports System.Data.SqlClient
Imports System.Data
#End Region
Partial Class PromoSumPerType
    Inherits System.Web.UI.Page
#Region " Connection String "


    Private connString As System.Configuration.ConnectionStringSettings
    Private _ConnStr As String = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString '"Data Source=SMportal-DEV;Initial Catalog=Promo;Persist Security Info=True;User ID=admin;Password=password;"
    Public Property ConnStr() As String
        Get
            Return _ConnStr
        End Get
        Set(ByVal value As String)
            value = _ConnStr
        End Set
    End Property

#End Region
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim dt As DataTable
        dt = BindGridView()
        If dt.Rows.Count <> 0 Then
            Me.lblDisplay.Text = ""
            Server.Transfer("rvPromoSumPerPromoType.aspx?PromoType=" & CStr(ddlPromoType.SelectedValue) & "&PromoPeriodTo=" & txtPeriodTo.Text.Trim & "&PromoPeriodFrom=" & txtPeriodFrom.Text.Trim)
        Else
            Me.lblDisplay.Text = "Record does not exists."
        End If
    End Sub

    Private Function BindGridView() As DataTable
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(ConnStr)
        Dim StoredProc As String = "USP_SelectPromoSumPerType"

        Dim sqlCmd As New Data.SqlClient.SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@PromoType", SqlDbType.VarChar)
        sqlCmd.Parameters("@PromoType").Value = ddlPromoType.SelectedValue.Trim

        sqlCmd.Parameters.Add("@PromoPeriodTo", SqlDbType.VarChar)
        sqlCmd.Parameters("@PromoPeriodTo").Value = txtPeriodTo.Text.Trim

        sqlCmd.Parameters.Add("@PromoPeriodFrom", SqlDbType.VarChar)
        sqlCmd.Parameters("@PromoPeriodFrom").Value = txtPeriodFrom.Text.Trim
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_ViewReports")
        Dim dt As DataTable = ds.Tables("tbl_ViewReports")
        Return dt
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtPeriodFrom.Text = Now.ToShortDateString
            txtPeriodTo.Text = Now.ToShortDateString
        End If
        
    End Sub
End Class
