

Imports System.Data
Imports System.Web.UI.ControlCollection


Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports System.Collections.Generic

Imports System.Web
Imports System.Xml
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services


Partial Class PromoRequestSwipestakesSeed
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            sqlDSPromoSeed.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
            sqlDSPromoSeed.SelectParameters("FilterText").DefaultValue = IIf(String.IsNullOrEmpty(txtFilter.Text), " ", txtFilter.Text)

            'Dim dummy As DataTable = New DataTable()
            'dummy.Columns.Add("Branch")
            'dummy.Rows.Add()
            'gvSeeding.DataSource = dummy
            'gvSeeding.DataBind()


            'Required for jQuery DataTables to work.
            'gvSeeding.UseAccessibleHeader = True
            'gvSeeding.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub

    Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        sqlDSPromoSeed.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
        sqlDSPromoSeed.SelectParameters("FilterText").DefaultValue = IIf(String.IsNullOrEmpty(txtFilter.Text), " ", txtFilter.Text)
    End Sub

End Class
