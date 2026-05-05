

Imports System.Data
Imports System.Web.UI.ControlCollection

Partial Class PromoRequestSwipestakesReseed
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
            sqlDSPromoSeed.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
            sqlDSPromoSeed.SelectParameters("CRID").DefaultValue = sCRID
        End If
    End Sub
End Class
