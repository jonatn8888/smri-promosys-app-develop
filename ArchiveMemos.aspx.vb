' Object Name	    :       ArchiveMemosView.aspx
' Purpose		    :       Requested by MPD dated November 16, 2011. Due to limited disk space in the PromoSys server and the continues
'                           posting of PromoSys transactions since its launch in July 2010 system
'                           resouces are being used up and slowdown is expected if no housekeeping
'                           is done on expired MPD announcement memos.
' Date Created	    :       06/11/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0               06/28/2012          Dow T. Carpio       Created this module.
Partial Class ArchiveMemos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        clsSession.FlagForPOSDisp = False

        If Not Page.IsPostBack Then

            txtFrom.Text = Format(Now.AddDays(-1 * 360), "MM/dd/yyyy")
            txtTo.Text = Format(Now, "MM/dd/yyyy")

        End If


    End Sub

    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblMemoID")

        If e.Row.RowType = DataControlRowType.DataRow Then

            If Len(e.Row.Cells(2).Text) > 50 Then e.Row.Cells(2).Text = Left(e.Row.Cells(2).Text, 50) & "..."

            e.Row.Cells(2).Text = "<a href='ArchiveMemosView.aspx?MemoID=" & lb.Text & "'>" & e.Row.Cells(2).Text & "</a>"

            If Len(e.Row.Cells(5).Text) > 40 Then e.Row.Cells(5).Text = Left(e.Row.Cells(5).Text, 37) & "..."
        End If

    End Sub



    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    'txtSearchValue.Text = "Dow"
    'gridRequests.DataSourceID = "sqldsData"
    'gridRequests.DataBind()

    'End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        blistErrorMsg.Items.Clear()

        If Not IsDate(txtFrom.Text) Then

            blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format

        End If

        If Not IsDate(txtTo.Text) Then

            blistErrorMsg.Items.Add("Blank or invalid promo end date format.") ' invalid date format

        Else
            Try

                If CDate(txtTo.Text) < CDate(txtFrom.Text) Then
                    blistErrorMsg.Items.Add("End of promo date must not be earlier than the start date.")
                End If

            Catch ex As Exception

                blistErrorMsg.Items.Add("Blank or invalid promo start/end date format.")

            End Try
        
        End If

        If blistErrorMsg.Items.Count > 0 Then

            gridRequests.DataSourceID = Nothing
            gridRequests.DataBind()

        Else

            gridRequests.DataSourceID = sqldsData.ID

        End If

    End Sub
End Class
