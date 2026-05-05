Imports System.Security
Imports System.Data

Partial Class QuickMessage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("AD_UserName") = "" Then
            Response.Redirect("Default.aspx")
        Else

            Select Case Request("ViewMode")
                Case 0, Nothing     ' inbox

                    mvBody.ActiveViewIndex = 0
                    lblBoxTitle.Text = ":: inbox ::"

                    sqldsMessages.SelectCommand = "SELECT * FROM Messaging WHERE SentTo = '" & Session("AD_UserName").ToString() & "' AND IsDeleted = 0 ORDER BY DateSent DESC"
                    sqldsMessages.Select(DataSourceSelectArguments.Empty)

                    lnkDeleteMsg.Visible = (gridMessages.Rows.Count > 0)

                Case 1              ' read message

                    If IsPostBack Then Exit Sub

                    mvBody.ActiveViewIndex = 1

                    'load message data
                    sqldsData.SelectParameters("MessageID").DefaultValue = Request("MessageID").ToString()

                    Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
                    Dim dr As DataRow = dv.Table.Rows(0)

                    lblSubject.Text = dr("Subject")
                    lblMsgDate.Text = dr("DateSent")
                    lblSentBy.Text = dr("SentBy")
                    '::HERE::
                    'txtMsgBody.Text = Server.HtmlDecode(dr("MsgBody"))
                    txtMsgBody.Text = dr("MsgBody")

                    sqldsData.UpdateCommand = "UPDATE Messaging SET IsRead = 1 WHERE MessageID = " & dr("MessageID").ToString()
                    sqldsData.Update()
    
                Case 2              ' compose message

                    If IsPostBack Then Exit Sub

                    mvBody.ActiveViewIndex = 2

                    lblDateToday.Text = Today().ToShortDateString()

                    If Request("MessageID") = Nothing Then
                        ' compose new message
                        ' just initialize the controls
                        txtSubject.Text = ""
                        txtSendTo.Text = IIf(Session("AD_UserName").ToString() = "noel", "mpd", "noel")
                        txtNewMessage.Text = ""

                        lnkBackToMsg.Visible = False
                    Else
                        ' reply to message
                        ' load info from received message

                        sqldsData.SelectParameters("MessageID").DefaultValue = Request("MessageID").ToString()

                        Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
                        Dim dr As DataRow = dv.Table.Rows(0)

                        txtSubject.Text = "RE: " & dr("Subject")
                        txtSendTo.Text = dr("SentBy")
                        txtNewMessage.Text = "[-- user: " & dr("SentBy") & " wrote: " & vbCrLf & dr("MsgBody") & " --]" & vbCrLf

                        lnkBackToMsg.Visible = True
                    End If

                Case 3      ' sent items

                    '::ToDO:: restrict access (temp)
                    If Session("AD_UserName") <> "noel" Then Response.Redirect("QuickMessage.aspx", True)

                    mvBody.ActiveViewIndex = 0
                    lblBoxTitle.Text = ":: sent items ::"

                    sqldsMessages.SelectCommand = "SELECT * FROM Messaging WHERE SentBy = '" & Session("AD_UserName").ToString() & "' AND IsDeleted = 0 ORDER BY DateSent DESC"
                    sqldsMessages.Select(DataSourceSelectArguments.Empty)

                    lnkDeleteMsg.Visible = (gridMessages.Rows.Count > 0)

            End Select

        End If

    End Sub

    Protected Sub lnkCloseRead_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCloseRead.Click

        Response.Redirect("QuickMessage.aspx")

    End Sub

    Protected Sub gridMessages_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridMessages.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblMessageID")
        Dim lbRead As Label = e.Row.FindControl("lblIsRead")

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(2).Text = "<a href='QuickMessage.aspx?ViewMode=1&MessageID=" & lb.Text & "'>" & e.Row.Cells(2).Text & "</a>"

            If CInt(lbRead.Text) = 0 Then
                e.Row.Cells(2).Text = "<b>" & e.Row.Cells(2).Text & "</b>"
            End If
        End If


    End Sub

    Protected Sub lnkDeleteMsg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteMsg.Click

        For Each row As GridViewRow In gridMessages.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            Dim lb As Label = row.FindControl("lblMessageID")

            If cb IsNot Nothing AndAlso cb.Checked Then

                'sqldsMessages.DeleteParameters("MessageID").DefaultValue = CInt(lb.Text)
                'sqldsMessages.Delete()

                ' ::ToDo:: just tag entry as deleted for achive purposes. maintain a mailbox for each user

                ' just hide the message
                sqldsMessages.UpdateParameters("MessageID").DefaultValue = CInt(lb.Text)
                sqldsMessages.Update()

            End If
        Next

    End Sub

    Protected Sub lnkCloseWrite_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCloseWrite.Click

        Response.Redirect("QuickMessage.aspx")

    End Sub

    Protected Sub lnkReply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReply.Click

        Response.Redirect("QuickMessage.aspx?ViewMode=2&MessageID=" & Request("MessageID"))

    End Sub

    Protected Sub lnkSendMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSendMessage.Click

        blistError.Items.Clear()

        ' validate entries
        If txtSubject.Text = Nothing Then
            blistError.Items.Add("Message subject must not be left blank.")
        End If

        If txtNewMessage.Text = Nothing Then
            blistError.Items.Add("Cannot send an empty message.")
        End If

        If blistError.Items.Count() > 0 Then
            Exit Sub
        End If

        '::HERE::
        'sqldsMessages.InsertParameters("MsgBody").DefaultValue = Server.HtmlEncode(txtNewMessage.Text)
        sqldsMessages.InsertParameters("MsgBody").DefaultValue = txtNewMessage.Text

        sqldsMessages.InsertParameters("SentBy").DefaultValue = Session("AD_UserName").ToString()
        sqldsMessages.Insert()

        ' inform user that the message was sent

        Response.Redirect("QuickMessage.aspx")

    End Sub

    Protected Sub lnkBackToMsg_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBackToMsg.Click

        Response.Redirect("QuickMessage.aspx?ViewMode=1&MessageID=" & Request("MessageID"))

    End Sub
End Class
