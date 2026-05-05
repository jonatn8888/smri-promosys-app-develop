
Imports Microsoft.Reporting.WebForms

Partial Class rvPriceEventPreview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0) Then Response.Redirect("InvalidAccess.aspx")

        'If Request("TranType") = "" Or Request("BranchCode") = "" Or Request("EventNumber") = "" Then Response.Redirect("InvalidAccess.aspx")


        Dim p(0) As ReportParameter
        Dim sReportTitle As String

        If Request("TranType") = "MKD" Then
            sReportTitle = "MARKDOWN AUTHORIZATION FORM"
        Else
            sReportTitle = "MARKUP AUTHORIZATION FORM"
        End If

        p(0) = New ReportParameter("ReportTitle", sReportTitle)

        rvPriceEvent.LocalReport.SetParameters(p)

        rvPriceEvent.LocalReport.Refresh()

    End Sub

End Class
