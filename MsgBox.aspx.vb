Imports System.Data
Imports System.IO
Imports System.Data.SqlClient

Partial Class MsgBox
    Inherits System.Web.UI.Page

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        ' Added dowcarpio20121220@smretailinc: ' Added dowcarpio20121220@smretailinc: allow approver to edit date period if late requests
        Dim _id As String = ""
        Try
            _id = clsEncryptDecrypt.DecryptText(Request("id").ToString, SystemUser.EncryptKey.ToString).Replace(" ", "/")
        Catch ex As Exception
            'Do nothing
        End Try

        If Len(_id) > 0 Then
            Dim ctr As Integer = 0
            blistErrorMsg.Items.Clear()

            If Not IsDate(txtPeriodFrom.Text) Then

                blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format
                ctr = 1

            End If

            If Not IsDate(txtPeriodTo.Text) Then

                blistErrorMsg.Items.Add("Blank or invalid promo end date format.") ' invalid date format

            Else

                Try
                    'temp only 20140523 revert per Ms. Jess (MPD)
                    If CDate(txtPeriodFrom.Text) <= CDate(Today()) Then

                        blistErrorMsg.Items.Add("Starting date of promotion must not be earlier or equal than today's date")

                    End If

                    If CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then

                        blistErrorMsg.Items.Add("End date of promotion must not be earlier than the starting date")

                    End If

                    'If CDate(txtPeriodFrom.Text) = CDate(hidCurrPeriodFrom.Value) Then

                    '    blistErrorMsg.Items.Add("Starting date of promotion must not equal to the current start date (" & hidCurrPeriodFrom.Value & ")")

                    'End If

                    'If CDate(txtPeriodTo.Text) = CDate(hidCurrPeriodTo.Value) Then

                    '    blistErrorMsg.Items.Add("End date of promotion must not be equal to the current end date (" & hidCurrPeriodTo.Value & ")")

                    'End If

                Catch ex As Exception

                    If ctr = 0 Then

                        blistErrorMsg.Items.Add("Blank or invalid promo start/end date format.")

                    End If

                End Try

            End If

            If blistErrorMsg.Items.Count > 0 Then Exit Sub

            UpdateRequestDatePeriod()

        End If

        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.MsgBoxwindow.hide();</script>")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Added dowcarpio20121220@smretailinc: ' Added dowcarpio20121220@smretailinc: allow approver to edit date period if late requests
        Dim _id As String = ""
        Try
            _id = clsEncryptDecrypt.DecryptText(Request("id").ToString, SystemUser.EncryptKey.ToString).Replace(" ", "/")
        Catch ex As Exception
            'Do nothing
        End Try

        If Not Page.IsPostBack Then

            If Len(_id) > 0 Then

                divTextDatePeriod.Visible = True
               
                populateRequestInfo()

            Else

                divTextDatePeriod.Visible = False

            End If

        End If

    End Sub

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        litPopMessage.Text = clsSession.Message
        'lblNotification.Text = clsSession.Notification

        Select Case clsSession.Icon
            Case "success"
                clsSession.DeleteStatus = "cancel"
                imgIcon.ImageUrl = imgOK.ImageUrl
                cmdPopUpOK.Text = "Ok"
                cmdCancel.Visible = False
            Case "error"
                clsSession.DeleteStatus = "cancel"
                imgIcon.ImageUrl = imgError.ImageUrl
                cmdCancel.Visible = False
                cmdPopUpOK.Text = "Ok"
            Case "inquiry"
                clsSession.DeleteStatus = "yes"
                imgIcon.ImageUrl = imgQuestion.ImageUrl
                cmdPopUpOK.Text = "Yes"
                cmdCancel.Text = "No"
            Case "fyi"
                clsSession.DeleteStatus = "cancel"
                imgIcon.ImageUrl = imgFYI.ImageUrl
                clsSession.DeleteStatus = "cancel"
                cmdPopUpOK.Text = "Ok"
                cmdCancel.Visible = False
            Case "warning"
                clsSession.DeleteStatus = "yes"
                imgIcon.ImageUrl = imgWarning.ImageUrl
                cmdPopUpOK.Text = "Yes"
                cmdCancel.Text = "No"
            Case "confirm"
                clsSession.DeleteStatus = "cancel"
                imgIcon.ImageUrl = imgFYI.ImageUrl
                cmdCancel.Text = "Cancel"
                cmdPopUpOK.Text = "Ok"
        End Select

    End Sub

   
    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        clsSession.DeleteStatus = "cancel"
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.MsgBoxwindow.hide();</script>")
    End Sub


    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'If Not Page.IsPostBack Then
        'clsSession.Notification = ""
        ' End If

    End Sub

    Private Sub populateRequestInfo()

        Dim _id As String = ""
        Try
            _id = clsEncryptDecrypt.DecryptText(Request("id").ToString, SystemUser.EncryptKey.ToString).Replace(" ", "/")
        Catch ex As Exception
            'Do nothing
        End Try

        Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()
        Dim dtPromoRequests As New dsPromotions.PromoRequestsDataTable
        Dim rowPromoRequest As dsPromotions.PromoRequestsRow
        dtPromoRequests = taPromoRequests.GetPromoRequestByID(_id)
        If dtPromoRequests.Rows.Count = 0 Then
            ' ::ToDo:: error
        Else
            rowPromoRequest = dtPromoRequests.Rows(0)
            With rowPromoRequest
                lblPromoTitle.Text = .Title.ToString()
                txtPeriodFrom.Text = .PromoPeriodFrom
                txtPeriodTo.Text = .PromoPeriodTo

                hidCurrPeriodFrom.Value = .PromoPeriodFrom
                hidCurrPeriodTo.Value = .PromoPeriodTo
            End With
        End If
    End Sub

    Private Function UpdateRequestDatePeriod() As Integer
        Dim _id As String = ""
        Try
            _id = clsEncryptDecrypt.DecryptText(Request("id").ToString, SystemUser.EncryptKey.ToString).Replace(" ", "/")
        Catch ex As Exception
            'Do nothing
        End Try
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            .CommandText = "[dbo].[USP_UpdateRequestDatePeriod]"
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = 4
            .Parameters.Add("@RequestID", SqlDbType.SmallInt)
            .Parameters("@RequestID").Value = _id
            .Parameters.Add("@PeriodFrom", SqlDbType.SmallDateTime)
            .Parameters("@PeriodFrom").Value = txtPeriodFrom.Text
            .Parameters.Add("@PeriodTo", SqlDbType.SmallDateTime)
            .Parameters("@PeriodTo").Value = txtPeriodTo.Text
            .ExecuteNonQuery()
        End With

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        GC.Collect()

    End Function
End Class
