Imports Microsoft.Reporting.WebForms
Imports System.Data
Partial Class rvReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Dim dtTable As New DataTable
            Dim dsReport As New dsReport
            Dim strQuery As String = ""

            Dim p(3) As ReportParameter
            Dim sReportTitle As String = ""
            Dim sReportPath As String = ""
            Dim sDSetName As String = ""

            p(1) = New ReportParameter("ReportTargetDate", Request("TargetDate").Replace("'", ""))
            p(2) = New ReportParameter("ReportPromoTypes", Request("PromoDesc").Replace("'", ""))
            p(3) = New ReportParameter("ReportBranches", Request("StoreDesc").Replace("'", ""))

            If (Request("SelectedValue") < 5) Then
                strQuery = "exec USP_GetPromoReport {0},{1},{2},{3}"
                strQuery = String.Format(strQuery, Request("SelectedValue"), Request("TargetDate"), Request("StoreCodes"), Request("PromoTypes"))
            ElseIf (Request("SelectedValue") = 5) Then
                ' 29-01-2019 Additional Reports
                strQuery = "exec usp_GetPromoForManualProcess {0}"
                strQuery = String.Format(strQuery, Request("StoreCodes"))
            ElseIf (Request("SelectedValue") = 6) Then
                ' 30-01-2019 Additional Reports
                strQuery = "exec usp_GetPromoExclusionList {0},{1}"
                strQuery = String.Format(strQuery, Request("StoreCodes"), Request("PromoTypes"))
            End If

            Select Case Request("SelectedValue")
                Case 1 'Branch Comparison of Ongoing Promotions
                    sReportTitle = "BRANCH COMPARISON OF ONGOING PROMOTIONS"
                    sReportPath = "~/repOption1A.rdlc"
                    sDSetName = "dsReport_dtOption1_Summary"
                Case 2 'Ongoing & Future Dated Promotions
                    sReportTitle = "ONGOING & FUTURE DATED PROMOTIONS"
                    sReportPath = "~/repOption2.rdlc"
                    sDSetName = "dsReport_dtOption2"
                Case 3 'Approved Promotions on a Given Date
                    sReportTitle = "APPROVED PROMOTIONS ON A GIVEN DATE"
                    sReportPath = "~/repOption3.rdlc"
                    sDSetName = "dsReport_dtOption3"
                Case 4 'Count of Active Promo IDs for Regular Price Items only
                    sReportTitle = "COUNT OF ACTIVE PROMO IDS FOR REGULAR PRICE ITEMS ONLY"
                    sReportPath = "~/repOption4.rdlc"
                    sDSetName = "dsReport_dtOption4"
                Case 5 'Promotions for Manual Processing 
                    ' 29-01-2019 Additional Reports
                    sReportTitle = "PROMOTIONS FOR MANUAL PROCESSING"
                    sReportPath = "~/repOption5.rdlc"
                    sDSetName = "dsReport_dtOption5"
                Case 6 'Promo Exclusion List for Automated Loyalty & Coupon Promos 
                    ' 30-01-2019 Additional Reports
                    sReportTitle = "PROMO EXCLUSION LIST FOR AUTOMATED LOYALTY & COUPON PROMOS"
                    sReportPath = "~/repOption6.rdlc"
                    sDSetName = "dsReport_dtOption6"
            End Select

            p(0) = New ReportParameter("ReportTitle", sReportTitle)
            If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable) Then
                rv.ProcessingMode = ProcessingMode.Local
                rv.LocalReport.ReportPath = Server.MapPath(sReportPath)
                Dim datasource As New ReportDataSource(sDSetName, dtTable)
                rv.LocalReport.DataSources.Clear()
                rv.LocalReport.DataSources.Add(datasource)
                rv.LocalReport.SetParameters(p)
                rv.LocalReport.Refresh()

                'rv.AsyncRendering = True
                'rv.SizeToReportContent = True
                'rv.ZoomMode = ZoomMode.FullPage
            End If

            dtTable = Nothing
            GC.Collect()

        End If

    End Sub
End Class
