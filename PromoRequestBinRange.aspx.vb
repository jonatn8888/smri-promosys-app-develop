

Imports System.Data
Imports System.Web.UI.ControlCollection

Partial Class PromoRequestBinRange
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            sqlDSPromoBinRange.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
            sqlDSPromoBinRange.SelectCommand.ToString()
        End If
    End Sub
End Class
