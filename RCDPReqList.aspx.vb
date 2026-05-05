' Object Name	    :       CreateRCPDRequest.aspx
' Purpose		    :       Cancellation/addedum/extention/clarification search parameter module
' Date Created	    :       11/09/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0              11/09/2012     Dow T. Carpio     Created this control.

Imports System.Data

Partial Class RCDPReqList
    Inherits System.Web.UI.Page

    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound

        Dim lbReq As Label = e.Row.FindControl("lblCRID")
        Dim lb As Label = e.Row.FindControl("lblMemoID")
        Dim lbReqID As Label = e.Row.FindControl("lblRequestID")
        Dim sRedTag As String = ""
        Dim sPage As String = ""

        'If CDate(txtPeriodFrom.Text) <= DateAdd(DateInterval.Day, 10, Today()) Then
        '    blistErrorMsg.Items.Add("Request must be at least 10 days before the Promo Period.")
        'End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            'If (DateDiff(DateInterval.Day, Today(), CDate(e.Row.Cells(3).Text)) < 4 _
            '  And e.Row.Cells(4).Text <> "Approved") Or (e.Row.Cells(4).Text = "Returned") Then sRedTag = "style='color: red'"

            e.Row.Cells(2).ToolTip = e.Row.Cells(2).Text

            If Len(e.Row.Cells(2).Text) > 50 Then
                e.Row.Cells(2).Text = Left(e.Row.Cells(2).Text, 45) & "..."
            End If

            If e.Row.Cells(5).Text = "For Review" Or _
                e.Row.Cells(5).Text = "For Final Approval" Or _
                e.Row.Cells(5).Text = "Returned to Analyst" Or _
                    e.Row.Cells(5).Text = "Approved" Then

                sPage = "RCDPMemoView.aspx"
            Else


                sPage = "RCDPMemo.aspx"

            End If

            e.Row.Cells(1).Text = "<a href='" & sPage & "?CRID=" & clsEncryptDecrypt.EncryptText(lbReq.Text, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(lbReqID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(1).Text & "</span></a>"
            e.Row.Cells(2).Text = "<a href='" & sPage & "?CRID=" & clsEncryptDecrypt.EncryptText(lbReq.Text, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(lbReqID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(2).Text & "</span></a>"
            e.Row.Cells(3).Text = "<a href='" & sPage & "?CRID=" & clsEncryptDecrypt.EncryptText(lbReq.Text, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(lbReqID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(3).Text & "</span></a>"
            e.Row.Cells(4).Text = "<a href='" & sPage & "?CRID=" & clsEncryptDecrypt.EncryptText(lbReq.Text, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(lbReqID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(4).Text & "</span></a>"
            e.Row.Cells(5).Text = "<a href='" & sPage & "?CRID=" & clsEncryptDecrypt.EncryptText(lbReq.Text, SystemUser.EncryptKey.ToString) & "&MemoID=" & clsEncryptDecrypt.EncryptText(lb.Text, SystemUser.EncryptKey.ToString) & "&RequestID=" & clsEncryptDecrypt.EncryptText(lbReqID.Text, SystemUser.EncryptKey.ToString) & "&v1=" & Request("v1").ToString & "&stat=" & clsEncryptDecrypt.EncryptText("VIEW", SystemUser.EncryptKey.ToString) & "'><span " & sRedTag & ">" & e.Row.Cells(5).Text & "</span></a>"

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' restrict access to promo requestor only
        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0) And SystemUser.UserLevel <> SystemUser.UserRoles.PromoRequestor Then Response.Redirect("InvalidAccess.aspx")

        ' clear selection
        clsSession.CurrRequestID = 0
        clsSession.CurrPromoID = 0

        'lnkPromotionalRequests.Visible = (SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Or SystemUser.UserLevel = SystemUser.UserRoles.Administrator)
        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString

        Dim _v2 As String = ""

        Try

            _v2 = clsEncryptDecrypt.DecryptText(Request("v2").ToString, SystemUser.EncryptKey.ToString).ToString

        Catch ex As Exception

            'Do nothing

        End Try

        If Not IsPostBack() Then

            ' populate date period
            txtFrom.Text = Format(Now.AddDays(-1 * 90), "MM/dd/yyyy")
            txtTo.Text = Format(Now, "MM/dd/yyyy")

            If strType = "CCL" Then

                ButtonProperties(False, True, True, True, True)
                lblRequstTypeDesc.Text = "Cancellation"

                gridRequests.Columns(5).HeaderText = "Cancellation Status"

            ElseIf strType = "ADD" Then

                ButtonProperties(True, False, True, True, True)
                lblRequstTypeDesc.Text = "Addendum"

                gridRequests.Columns(4).Visible = False
                gridRequests.Columns(5).HeaderText = "Addendum Status"

            ElseIf strType = "EXT" Then

                ButtonProperties(True, True, False, True, True)
                lblRequstTypeDesc.Text = "Extension"

                gridRequests.Columns(4).Visible = False
                gridRequests.Columns(5).HeaderText = "Extension Status"

            ElseIf strType = "CRF" Then

                ButtonProperties(True, True, True, False, True)
                lblRequstTypeDesc.Text = "Clarification"

                gridRequests.Columns(5).HeaderText = "Clarification Status"

            ElseIf strType = "SWP" Then

                ButtonProperties(True, False, True, False, False)
                lblRequstTypeDesc.Text = "Swipestakes Reseeding"

                gridRequests.Columns(5).HeaderText = "Swipestakes Reseeding Status"


            Else

                Response.Redirect("InvalidAccess.aspx")

            End If

            ' dowcarpio20131001@smretail: combine active, drafts and approved status as requested by MPD. UAT Findings 20130916
            With cboFilterStatus

                '.Items.Add(New ListItem("All", ""))
                .Items.Add(New ListItem("Active Promo Requests", " AND CR.Status IN ('For MBU Approval','For MCI Approval','For MPD Processing','For Review','For Final Approval','Returned','Returned to Analyst', 'For Mdsg/Group Head Approval', 'For BU Head Approval')"))
                '.Items.Add(New ListItem("For MBU Approval", " AND CR.Status = 'For MBU Approval'"))
                '.Items.Add(New ListItem("For MCI Approval", " AND CR.Status = 'For MCI Approval'"))
                '.Items.Add(New ListItem("For MPD Processing", " AND CR.Status = 'For MPD Processing'"))
                '.Items.Add(New ListItem("For Review", " AND CR.Status = 'For Review'"))
                '.Items.Add(New ListItem("For Final Approval", " AND CR.Status = 'For Final Approval'"))
                .Items.Add(New ListItem("Approved Requests", " AND CR.Status = 'Approved'"))
                '.Items.Add(New ListItem("Returned to Analyst", " AND CR.Status = 'Returned to Analyst'"))
                '.Items.Add(New ListItem("Returned", " AND CR.Status = 'Returned'"))
                .Items.Add(New ListItem("Drafts", " AND CR.Status = 'Draft'"))

                .SelectedIndex = clsSession.CurrFilterSelection
                clsSession.CurrFilterSelection = 0

            End With


            If _v2 <> "" Then

                cboFilterStatus.SelectedIndex = 8

            Else
                If SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Then

                    lnkCreateNewRequest.Enabled = True

                End If

                ' dowcarpio20131001@smretail: deleted dropdownlist defaults
                'If SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Then
                '    cboFilterStatus.SelectedIndex = 0
                '    lnkCreateNewRequest.Enabled = True
                'ElseIf SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover Then
                '    cboFilterStatus.SelectedIndex = 1
                'ElseIf SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover Then
                '    cboFilterStatus.SelectedIndex = 2
                'ElseIf SystemUser.UserLevel = SystemUser.UserRoles.Analyst Then
                '    cboFilterStatus.SelectedIndex = 3
                'ElseIf SystemUser.UserLevel = SystemUser.UserRoles.Reviewer Then
                '    cboFilterStatus.SelectedIndex = 4
                'ElseIf SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover Then
                '    cboFilterStatus.SelectedIndex = 5
                'End If

            End If

            populateDataView()

        End If


    End Sub

    'Protected Sub lnkPromotionalRequests_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromotionalRequests.Click

    '    clsSession.MsgTransFlowFlag = 1
    '    clsSession.CurrRequestID = 0
    '    Response.Redirect("PromoReqListCMM.aspx")

    'End Sub

    Private Sub populateDataView()

        Dim ctr As Integer = 0

        blistErrorMsg.Items.Clear()

        If txtFrom.Text <> "" Or txtTo.Text <> "" Then

            If clsPromo.IsSMDate(txtFrom.Text) = False Or txtFrom.Text = "" Then

                blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format
                ctr += 1

            End If


            If clsPromo.IsSMDate(txtTo.Text) = False Or txtTo.Text = "" Then

                blistErrorMsg.Items.Add("Blank or invalid promo end date format.") ' invalid date format
                ctr += 1

            End If

            If ctr = 0 Then

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

            Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString

            With sqldsRequests

                Dim strSQL As String = "SELECT M.MEMONUMBER,CR.* FROM ChangeRequests CR INNER JOIN Memos M ON M.Memoid = CR.MemoID "

                If SystemUser.UserLevel = SystemUser.UserRoles.PromoRequestor Then

                    strSQL &= " WHERE CR.Status NOT IN ('Temp','Disapproved','Deleted') AND CR.RequestType = '" & strType & "' " & cboFilterStatus.SelectedValue
                    strSQL &= " AND CR.USERCREATED = 0" & SystemUser.UserID

                ElseIf (SystemUser.UserLevel = SystemUser.UserRoles.RequestApprover) Then

                    strSQL &= " WHERE CR.Status NOT IN ('Temp','Disapproved','Deleted') AND CR.RequestType = '" & strType & "' " & cboFilterStatus.SelectedValue & _
                     " AND CR.OwnerGroup IN (" & _
                                         " SELECT GroupID FROM UserGroups WHERE BizUnit IN (" & _
                                         " SELECT BizUnit FROM UserGroups WHERE GroupID IN (" & _
                                         " SELECT GroupID FROM GroupAssignment WHERE UserID = 0" & SystemUser.UserID & ")))"

                ElseIf (SystemUser.UserLevel = SystemUser.UserRoles.RequestReviewer) Then

                    strSQL &= " WHERE CR.Status NOT IN ('Temp','Disapproved','Deleted') AND CR.RequestType = '" & strType & "' " & cboFilterStatus.SelectedValue & _
                     " AND CR.OwnerGroup IN (" & _
                                         " SELECT GroupID FROM UserGroups WHERE BizUnit IN (" & _
                                         " SELECT BizUnit FROM UserGroups WHERE GroupID IN (" & _
                                         " SELECT GroupID FROM GroupAssignment WHERE UserID = 0" & SystemUser.UserID & ")))"

                ElseIf SystemUser.UserLevel = SystemUser.UserRoles.SMACapprover Then

                    strSQL &= " WHERE CR.Status NOT IN ('Temp','Disapproved','Deleted') AND CR.RequestType = '" & strType & "' " & cboFilterStatus.SelectedValue

                ElseIf SystemUser.UserLevel = SystemUser.UserRoles.Analyst Then

                    strSQL &= " WHERE CR.Status NOT IN ('Temp','Disapproved','Deleted')  AND CR.RequestType = '" & strType & "' " & cboFilterStatus.SelectedValue & _
                    "AND CR.OwnerGroup IN (SELECT GroupID FROM UserGroups WHERE BizUnit IN " & _
                    "(SELECT BizUnit FROM UserGroups WHERE GroupID IN " & _
                    "(SELECT GroupID FROM dbo.GroupAssignment WHERE UserID = " & SystemUser.UserID & "))) "

                ElseIf SystemUser.UserLevel = SystemUser.UserRoles.Reviewer Then

                    strSQL &= " WHERE CR.Status NOT IN ('Temp','Disapproved','Deleted')  AND CR.RequestType = '" & strType & "' " & cboFilterStatus.SelectedValue & _
                     " AND CR.OwnerGroup IN (SELECT GroupID FROM UserGroups WHERE Category IN " & _
                     "(SELECT Category FROM UserGroups WHERE GroupID IN " & _
                     " (SELECT GroupID FROM GroupAssignment WHERE UserID = 0" & SystemUser.UserID & ")))"

                ElseIf SystemUser.UserLevel = SystemUser.UserRoles.MemoApprover Then

                    strSQL &= " WHERE CR.Status NOT IN ('Temp','Disapproved','Deleted')  AND CR.RequestType = '" & strType & "' " & cboFilterStatus.SelectedValue

                Else

                    strSQL &= " WHERE CR.Status IN ('No Data') '"

                End If

                If txtFrom.Text <> "" And txtTo.Text <> "" Then

                    strSQL &= " AND CR.RequestDate BETWEEN  CAST('" & txtFrom.Text & "' AS DATETIME) AND CAST('" & txtTo.Text & "'  AS DATETIME) +1 "

                End If

                strSQL &= " ORDER BY PromoPeriodFrom"

                .SelectCommand = strSQL

                .Select(DataSourceSelectArguments.Empty)

            End With

            gridRequests.DataSourceID = sqldsRequests.ID
            gridRequests.DataBind()

        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        clsSession.CurrFilterSelection = cboFilterStatus.SelectedIndex

    End Sub

 

    ' To set button properties per type
    Private Sub ButtonProperties(ByVal boolCancellation As Boolean _
                                 , ByVal boolAddendum As Boolean _
                                 , ByVal boolExtention As Boolean _
                                 , ByVal boolClarification As Boolean _
                                 , ByVal boolSwipestakesReseeding As Boolean)

        lnkCancellation.Enabled = boolCancellation
        'lnkAddendum.Enabled = boolAddendum
        lnkExtension.Enabled = boolExtention
        'lnkClarification.Enabled = boolClarification
        lnkSwipestakesReseeding.Enabled = boolSwipestakesReseeding

    End Sub

    Protected Sub lnkCancellation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCancellation.Click
        Server.Transfer("RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("CCL", SystemUser.EncryptKey.ToString))
    End Sub

    'Protected Sub lnkAddendum_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddendum.Click
    '    Server.Transfer("RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("ADD", SystemUser.EncryptKey.ToString))
    'End Sub

    Protected Sub lnkExtension_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkExtension.Click
        Server.Transfer("RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("EXT", SystemUser.EncryptKey.ToString))
    End Sub

    'Protected Sub lnkClarification_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkClarification.Click
    '    Server.Transfer("RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("CRF", SystemUser.EncryptKey.ToString))
    'End Sub

    ' Revised dowcarpio03092013@smretail: creation of request except thu, fri, sat, sun 
    Protected Sub lnkCreateNewRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCreateNewRequest.Click

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString

        'Temp Disabled for dev
        'If strType = "CCL" And (Today().ToString("ddd").ToString = "Thu" Or _
        '        Today().ToString("ddd").ToString = "Fri" Or _
        '        Today().ToString("ddd").ToString = "Sat" Or _
        '        Today().ToString("ddd").ToString = "Sun") Then


        '    ViewState("proc") = "do_nothing"
        '    lblPopTitle.Value = "Error"
        '    clsSession.Icon = "error"
        '    clsSession.Message = "Unable to create cancellation request.<br /><br />" & _
        '                         "Processing Days: Monday, Tuesday and Wednesday only.<br /><br />"

        '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('200','');</script>")

        'Else

        Server.Transfer("CreateRCPDRequest.aspx?v1=" & Request("v1").ToString)
        'End If


    End Sub

    Protected Sub gridRequests_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridRequests.SelectedIndexChanged

    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click

        populateDataView()

    End Sub

    Protected Sub cboFilterStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilterStatus.SelectedIndexChanged

        populateDataView()

    End Sub

    Protected Sub lnkSwipestakesReseeding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSwipestakesReseeding.Click
        Server.Transfer("RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("SWP", SystemUser.EncryptKey.ToString))
    End Sub
End Class
