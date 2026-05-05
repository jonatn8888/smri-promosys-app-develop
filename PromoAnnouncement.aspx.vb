Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Partial Class PromoAnnouncement
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If Not Page.IsPostBack Then

            If Session("UserLevel") = "20" Or _
                    Session("UserLevel") = "30" Or _
                    Session("UserLevel") = "40" Or _
                    Session("UserLevel") = "50" Or _
                    Session("UserLevel") = "70" Or _
                    Session("UserLevel") = "80" Or _
                    Session("UserLevel") = "90" Or _
                    Session("UserLevel") = "100" Then

                lblBizUnit.Visible = True
                cboBizUnit.Visible = True
                FillDropDownList()

            Else

                lblBizUnit.Visible = False
                cboBizUnit.Visible = False

            End If

            clsSession.FlagForPOSDisp = False

        End If
    End Sub



    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblMemoID")

        If e.Row.RowType = DataControlRowType.DataRow Then

            If Len(e.Row.Cells(3).Text) > 50 Then e.Row.Cells(3).Text = Left(e.Row.Cells(3).Text, 50) & "..."

            e.Row.Cells(3).Text = "<a href='ViewMemo.aspx?MemoID=" & lb.Text & "'>" & e.Row.Cells(3).Text & "</a>"

            If Len(e.Row.Cells(6).Text) > 40 Then e.Row.Cells(6).Text = Left(e.Row.Cells(6).Text, 37) & "..."

        End If

    End Sub

    Private Sub FillDropDownList()

        Dim sqlAdatpter As SqlDataAdapter
        Dim sqlConn As SqlConnection
        Dim dtbl As New DataTable

        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_ViewPromoAnnouncementsDropDownList"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@UserGroupID", SqlDbType.SmallInt)
        sqlCmd.Parameters("@UserGroupID").Value = SystemUser.UserGroupID

        sqlAdatpter = New SqlDataAdapter(sqlCmd)
        sqlAdatpter.Fill(dtbl)

        clsPromo.FetchDropDownList(dtbl, cboBizUnit, "BIZUNIT", Nothing, Nothing, Nothing, Nothing, "[All Units]", False, "SEARCHTEXT")
        clsPromo.FetchDropDownList(dtbl, cboPromoType, "PROMOTYPE", Nothing, Nothing, Nothing, Nothing, "[All Types]", False, "SEARCHTEXT")

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        sqlAdatpter = Nothing
        dtbl = Nothing

        GC.Collect()

    End Sub


    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click

        blistErrorMsg.Items.Clear()

        If txtFrom.Text <> "" Or txtTo.Text <> "" Then


            If Not IsDate(txtFrom.Text) Then

                blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format

            End If

            If Not IsDate(txtTo.Text) And txtTo.Text = "" Then

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

        End If

        If blistErrorMsg.Items.Count > 0 Then

            gridRequests.DataSourceID = Nothing
            gridRequests.DataBind()

        Else

            gridRequests.DataSourceID = sqldsData.ID

        End If



    End Sub

End Class
