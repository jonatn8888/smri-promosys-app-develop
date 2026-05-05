' Object Name	    :       CreateRCPDRequest.aspx
' Purpose		    :       Cancellation/addedum/extention/clarification search parameter module
' Date Created	    :       11/09/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0              11/09/2012     Dow T. Carpio     Created this control.


Imports System.Data.SqlClient
Imports System.Data
Partial Class CreateRCPDRequest
    Inherits System.Web.UI.Page

    Private Sub BindGridReults()

        Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand("USP_SearchRCPDList", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar)

        'If strType = "CCL" Then

        sqlCmd.Parameters("@Type").Value = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString

        'Else

        '    sqlCmd.Parameters("@Type").Value = ""

        'End If

        sqlCmd.Parameters.Add("@SearchField", SqlDbType.VarChar)
        sqlCmd.Parameters("@SearchField").Value = ddlCriteria.SelectedValue

        sqlCmd.Parameters.Add("@SearchValue", SqlDbType.VarChar)
        sqlCmd.Parameters("@SearchValue").Value = txtSearch.Text

        sqlCmd.Parameters.Add("@UserID", SqlDbType.SmallInt)
        sqlCmd.Parameters("@UserID").Value = SystemUser.UserID

        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()

        da.Fill(ds, "tblTrans")
        Dim dt As DataTable = ds.Tables("tblTrans")

        Me.gridRequests.DataSource = dt
        Me.gridRequests.DataBind()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        da = Nothing
        ds = Nothing

        GC.Collect()

    End Sub


    'Protected Sub gridRequests_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridRequests.RowCommand
    '    'If dt.Rows.Count > 0 Then
    '    clsSession.CurrRequestID = e.CommandArgument
    '    Response.Redirect("ViewRequest.aspx?Report=False")
    '    'Else
    '    '    lblValidateMessage.Text = "No record found."
    '    'End If
    'End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Me.lblValidateMessage.Text = ""
        If Me.txtSearch.Text <> "" Then
            If (Not IsNumeric(txtSearch.Text) And Me.ddlCriteria.SelectedValue = "Request ID") Then
                Me.lblValidateMessage.Text = "Invalid input."
                Exit Sub
            End If
            BindGridReults()
        Else
            Me.lblValidateMessage.Text = "Input required."
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' restrict access to promo requestor only
        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0) And SystemUser.UserLevel <> SystemUser.UserRoles.PromoRequestor Then Response.Redirect("InvalidAccess.aspx")

        If Not Page.IsPostBack Then

            ' RCPD Type are as follows:
            '   CCL - Cancellation
            '   ADD - Addendum
            '   EXT - Extension
            '   CRF - Clarification
            '   SWP - Swipestakes Reseeding

            Dim strType As String = clsEncryptDecrypt.DecryptText(Request("v1").ToString, SystemUser.EncryptKey.ToString).ToString

            If strType = "CCL" Then

                lblRequstTypeDesc.Text = "Cancellation"

            ElseIf strType = "ADD" Then

                lblRequstTypeDesc.Text = "Addendum"

            ElseIf strType = "EXT" Then

                lblRequstTypeDesc.Text = "Extension"

            ElseIf strType = "CRF" Then

                lblRequstTypeDesc.Text = "Clarification"

            ElseIf strType = "SWP" Then

                lblRequstTypeDesc.Text = "Swipestakes Reseeding"

            Else

                Response.Redirect("InvalidAccess.aspx")

            End If

        End If

    End Sub

    Protected Sub lnkBackToPreviousPage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBackToPreviousPage.Click
        Server.Transfer("RCDPReqList.aspx?v1=" & Request("v1").ToString)
    End Sub

    ' Added dowcarpio11132012@smretailinc: to view approved memo 
    Protected Sub gridRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridRequests.RowDataBound
        Dim lb As Label = e.Row.FindControl("lblMemoID")

        If e.Row.RowType = DataControlRowType.DataRow Then

            'If Len(e.Row.Cells(3).Text) > 50 Then e.Row.Cells(3).Text = Left(e.Row.Cells(3).Text, 50) & "..."

            e.Row.Cells(3).Text = "<a href='ViewMemo.aspx?MemoID=" & lb.Text & "&Report=False&v1=" & Request("v1").ToString & "'>" & e.Row.Cells(3).Text & "</a>"

            'If Len(e.Row.Cells(6).Text) > 40 Then e.Row.Cells(6).Text = Left(e.Row.Cells(6).Text, 37) & "..."

        End If
    End Sub

    Protected Sub gridRequests_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridRequests.SelectedIndexChanged

    End Sub
End Class
