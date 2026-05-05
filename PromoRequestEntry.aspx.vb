Imports dsPromotionsTableAdapters
Imports System.Data

Partial Class PromoRequestEntry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtCountCharacters()

        If Session("UserID") = Nothing Then
            Response.Redirect("InvalidAccess.aspx")
        End If

        If Not IsPostBack() Then


            If Session("CurrRequestID") = 0 Then
                lblRequestID.Text = "New"
                lblReqDate.Text = Format(Now(), "MMMM dd, yyyy")
            Else
                'get Request information

                Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()
                Dim dtPromoRequests As dsPromotions.PromoRequestsDataTable
                Dim rowPromoRequest As dsPromotions.PromoRequestsRow

                dtPromoRequests = taPromoRequests.GetPromoRequestByID(Session("CurrRequestID"))

                If dtPromoRequests.Rows.Count = 0 Then
                    ' ::ToDo:: error
                Else
                    rowPromoRequest = dtPromoRequests.Rows(0)

                    With rowPromoRequest
                        lblRequestID.Text = "PR-" & Format(.RequestID, "0####") & Format(Now.Year, "-0#")
                        lblReqDate.Text = .RequestDate.ToLongDateString()

                        txtReqTitle.Text = .Title.ToString()

                        ' Revised dowcarpio09182012@smretailinc: Format date to MM/dd/yyyy
                        txtPeriodFrom.Text = .PromoPeriodFrom
                        txtPeriodTo.Text = .PromoPeriodTo
                    End With
                End If
            End If

            ' Hide PromoDetail tab for SBU Marketing requestor and enable lnkBranches link button: MPD UAT Findings (10/09/2012)
            If SystemUser.UserGroupType = "SBU" Or SystemUser.UserGroupType = "BCR" Then

                trPromoDetails.Visible = False
                lnkBranches.Visible = True

            End If



        End If

    End Sub

    Protected Sub lnkPromoInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoInfo.Click

        '::ToDo:: client-side validation for blank entries

        Dim MinimumLeadDays As Integer = 3
        Dim MaxLeadMonths As Integer = 3

        blistErrorMsg.Items.Clear()

        'validate entries
        If txtReqTitle.Text.Trim() = "" Then
            blistErrorMsg.Items.Add("Promo title not indicated.")
        End If

        If Not IsDate(txtPeriodFrom.Text) Then

            blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format

        ElseIf DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < 1 Then

            blistErrorMsg.Items.Add("Request must be at least a calendar day before the start of promo.")

        ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (11 * 60)) Then  ' past 2pm
            'NBS:20200616
            'ElseIf (DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) = 1) And (Now.TimeOfDay.TotalMinutes > (14 * 60)) Then  ' past 2pm

            blistErrorMsg.Items.Add("Request past the 11AM cut-off time must be at least 2 calendar days before the start of promo.")

        End If

        If Not IsDate(txtPeriodTo.Text) Then
            blistErrorMsg.Items.Add("Blank or invalid promo end date format.")  ' invalid date format

        ElseIf CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then
            blistErrorMsg.Items.Add("End of promo must not be earlier than the start date.")

        ElseIf DateDiff(DateInterval.Day, Today(), CDate(txtPeriodTo.Text)) <= 0 Then
            blistErrorMsg.Items.Add("End of promo must be atleast a day from the current date.")  ' expired promo

        End If

        If blistErrorMsg.Items.Count = 0 Then

            ' disallow requests that are more than 3 months ahead .: 8/25/2010 :.
            If CDate(txtPeriodFrom.Text) > DateAdd(DateInterval.Month, MaxLeadMonths, Today()) Then
                blistErrorMsg.Items.Add("Requests that are more than 3 months ahead are not allowed.")
            End If

            'If DateDiff(DateInterval.Day, CDate(lblReqDate.Text), CDate(txtPeriodTo.Text)) <= MinimumLeadDays Then
            '    blistErrorMsg.Items.Add("Requests must be at least 3 working days before the start of promo.")
            'End If

        End If

        ' display error message and skip other commands
        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        ' RESTORE THIS SECTION!
        ' warn user if date of request is less than the 10-day lead time
        'If CDate(txtPeriodFrom.Text) <= DateAdd(DateInterval.Day, 10, Today()) Then
        '    ' show prompt
        '    lblPopTitle.Value = "Warning"
        '    clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br />" & _
        '                         "<b>Request will be forwarded to VP-MPD upon approval of MBU head.</b><br><br>" & _
        '                         "Do you still wish to continue?"
        '    clsSession.Icon = "warning"
        '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>msgbox('218','');</script>")
        'Else ...

        ' save entries
        If SaveTransEntries() Then
            Response.Redirect("PromoEntry.aspx")
        Else
            blistErrorMsg.Items.Add("Error encountered during saving of data.")
        End If

        'End If  -- restore this line

    End Sub

    Protected Sub lnkSaveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click

        Response.Redirect("PromoRequestList.aspx")

    End Sub

    Private Function SaveTransEntries() As Boolean

        Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter
        Dim bResult As Boolean = False

        ' get user information to be posted in the document
        sqldsData.SelectCommand = "SELECT SignName, SignPosition FROM Users WHERE UserID = " & SystemUser.UserID

        Dim dvUsers As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
        Dim drUser As DataRow = dvUsers.Table.Rows(0)

        If clsSession.CurrRequestID = 0 Then
            ' new entry

            clsSession.CurrRequestID = taPromoRequests.AddPromoRequest(Now(), txtReqTitle.Text.ToUpper(), CDate(txtPeriodFrom.Text), CDate(txtPeriodTo.Text), drUser("SignName"), drUser("SignPosition"), "Draft", SystemUser.UserGroupID, SystemUser.UserID)
            bResult = True

        Else
            ' just update the record
            Dim dtPromoRequests As dsPromotions.PromoRequestsDataTable
            Dim rowPromoRequest As dsPromotions.PromoRequestsRow

            dtPromoRequests = taPromoRequests.GetPromoRequestByID(clsSession.CurrRequestID)

            If dtPromoRequests.Rows.Count > 0 Then

                rowPromoRequest = dtPromoRequests.Rows(0)

                ' ensure that existing transactions are not overwritten
                If rowPromoRequest.Status = "Draft" Or _
                   rowPromoRequest.Status = "Returned" Then

                    With rowPromoRequest
                        .RequestDate = Now()            ' update entry date
                        .Title = txtReqTitle.Text.ToUpper
                        .PromoPeriodFrom = CDate(txtPeriodFrom.Text)
                        .PromoPeriodTo = CDate(txtPeriodTo.Text)
                        .RequestedBy = drUser("SignName")
                        .RequesterPos = drUser("SignPosition")
                        .OwnerGroup = SystemUser.UserGroupID
                        ' .Status = "Draft"
                    End With

                    taPromoRequests.Update(dtPromoRequests)

                    ' Added dowcarpio09062012@smretailinc: Update promotion details. Because of the conflict issue reported by MPD 09/05/2012
                    taPromoRequests.UpdatePromoRequestDetails(CDate(txtPeriodFrom.Text), CDate(txtPeriodTo.Text), clsSession.CurrRequestID)

                    bResult = True
                End If

            End If

        End If

        SaveTransEntries = bResult

    End Function


    'Protected Sub Button1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.ServerClick
    '    Response.Redirect("PromoDetailsEntryDepartmental.aspx")
    'End Sub

    'Protected Sub Button2_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.ServerClick
    '    Response.Redirect("PromoEntry.aspx")
    'End Sub

    Protected Sub btnProcess_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.ServerClick
        If clsSession.DeleteStatus = "yes" Then
            SaveTransEntries()
            Response.Redirect("PromoEntry.aspx")
        End If
    End Sub

    Protected Sub txtCountCharacters()
        Dim script As String = _
            "<script type='text/javascript'>" & vbCrLf & _
            "    function countWords(textbox, labelId, maxChars) {" & vbCrLf & _
            "        setTimeout(function () {" & vbCrLf & _
            "            var text = textbox.value;" & vbCrLf & _
            "            var charCount = text.length;" & vbCrLf & _
            "            if (charCount > maxChars) {" & vbCrLf & _
            "                textbox.value = text.substring(0, maxChars);" & vbCrLf & _
            "                charCount = maxChars;" & vbCrLf & _
            "            }" & vbCrLf & _
            "            var label = document.getElementById(labelId);" & vbCrLf & _
            "            if (label) {" & vbCrLf & _
            "                label.innerHTML = charCount + ' / ' + maxChars;" & vbCrLf & _
            "            }" & vbCrLf & _
            "        }, 0);" & vbCrLf & _
            "    }" & vbCrLf & _
            "</script>"


        ClientScript.RegisterStartupScript(Me.GetType(), "countWordsScript", script)
    End Sub

End Class
