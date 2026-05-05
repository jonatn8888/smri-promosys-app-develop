Imports System.Data.SqlClient
Imports System.Data
Partial Class CreateTransactionTemplate
    Inherits System.Web.UI.Page

    Private Sub BindGridResults()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand("xUSP_SearchTransTemplate", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@UserID", SqlDbType.SmallInt)
        sqlCmd.Parameters("@UserID").Value = SystemUser.UserID

        If ddlCriteria.SelectedValue = "Title" Then
            sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
            sqlCmd.Parameters("@RequestID").Value = 0
            sqlCmd.Parameters.Add("@Title", SqlDbType.VarChar)
            sqlCmd.Parameters("@Title").Value = txtSearch.Text.Trim
        Else
            sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
            'TODO: remove restriction on textbox and strip non-numeric characters for RequestID
            sqlCmd.Parameters("@RequestID").Value = txtSearch.Text.Trim
            sqlCmd.Parameters.Add("@Title", SqlDbType.VarChar)
            sqlCmd.Parameters("@Title").Value = ""
        End If
        sqlCmd.Parameters.Add("@WhereCriteria", SqlDbType.VarChar)
        sqlCmd.Parameters("@WhereCriteria").Value = ddlCriteria.SelectedValue
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tblTrans")
        Dim dt As DataTable = ds.Tables("tblTrans")

        Me.gridRequests.DataSource = dt
        Me.gridRequests.DataBind()

        GC.Collect()
    End Sub


    Protected Sub gridRequests_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridRequests.RowCommand
        'If dt.Rows.Count > 0 Then

        For Each row As GridViewRow In gridRequests.Rows
            Dim promoID As String = DirectCast(row.FindControl("PromoID"), Label).Text
            Session("PromoID") = promoID
        Next
        clsSession.CurrRequestID = e.CommandArgument
        Response.Redirect("ViewRequest.aspx?Report=False")
        'Else
        '    lblValidateMessage.Text = "No record found."
        'End If
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Me.lblValidateMessage.Text = ""
        If Me.txtSearch.Text <> "" Then
            If (Not IsNumeric(txtSearch.Text) And Me.ddlCriteria.SelectedValue = "Request ID") Then
                Me.lblValidateMessage.Text = "Invalid input."
                Exit Sub
            End If
            BindGridResults()
        Else
            Me.lblValidateMessage.Text = "Input required."
        End If
    End Sub


End Class
