
Imports System.Data

Partial Class HomePagePOS
    Inherits System.Web.UI.Page


    ' Revised dowcarpio01182013@smretailinc: RCDP updates:
    'CDREG - class discount for regular   
    Protected Sub lnkGenPOSFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkGenPOSFile.Click
        Response.Redirect("POSscreen.aspx?v1=" & clsEncryptDecrypt.EncryptText("REG", SystemUser.EncryptKey.ToString))
    End Sub

    Protected Sub lnkPromoAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoAttachment.Click
        Response.Redirect("POSMemoAttachment.aspx")
    End Sub

    ' Added dowcarpio01182013@smretailinc: RCDP updates:
    'CDREG - class discount for cancellation
    Protected Sub lnkGenCCLPOSFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkGenCCLPOSFile.Click
        Response.Redirect("POSscreen.aspx?v1=" & clsEncryptDecrypt.EncryptText("CCL", SystemUser.EncryptKey.ToString))
    End Sub
End Class
