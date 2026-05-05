#Region " Imports "
Imports System.Data.SqlClient
Imports System.Data
#End Region
Partial Class EmailMaintenance
    Inherits System.Web.UI.Page

    Private Sub FillGridView()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand("USP_SearchEmail", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar)
        sqlCmd.Parameters("@EmailAddress").Value = txtSearchInput.Text.Trim

        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()

        da.Fill(ds, "tbl_Email")
        Dim dt As DataTable = ds.Tables("tbl_Email")
        If dt.Rows.Count <> 0 Then
            Session("dsEmail") = dt
            gvBranches.DataSource = dt
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = ""
            Me.lblRecordCount.Visible = True
            Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
        Else
            Session("dsEmail") = Nothing
            gvBranches.DataSource = Nothing
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = "No record found."
            Me.lblRecordCount.Visible = False
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then
            FillGridView()
            tblSearchEmail.Visible = True
            tblGridviewEmail.Visible = True
            tblEmailInput.Visible = False

            btnSave.Visible = False
            btnCancel.Visible = False
            btnDelete.Visible = False
        End If
    End Sub

    Protected Sub gvBranches_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBranches.RowCommand

        If e.CommandName = "select" Then


            ViewState("SavingStatus") = "edit"
            Dim i As Integer = 0
            Dim rowView As DataRowView
            Dim row As DataRow
            Dim dt As New DataTable
            dt = CType(Session("dsEmail"), DataTable)

            For Each rowView In dt.DefaultView
                row = rowView.Row
                If row("RowID") = e.CommandArgument Then

                    ViewState("RowID") = e.CommandArgument
                    ddlCompany.SelectedValue = dt.Rows(i)("CompCode").ToString
                    ddlBranch.SelectedValue = dt.Rows(i)("BranchCode").ToString
                    ddlGroup.SelectedValue = dt.Rows(i)("GroupID").ToString

                    txtEmail.Text = dt.Rows(i)("eadd").ToString
                    txtDescription.Text = dt.Rows(i)("Description").ToString

                    tblSearchEmail.Visible = False
                    tblGridviewEmail.Visible = False
                    tblEmailInput.Visible = True

                    btnCancel.Visible = True
                    btnSave.Visible = True
                    btnDelete.Visible = True
                    cmdNew.Visible = False

                    Exit For
                End If
                i = i + 1
            Next
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        txtSearchInput.Text = ""
        FillGridView()


        tblSearchEmail.Visible = True
        tblGridviewEmail.Visible = True
        tblEmailInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        cmdNew.Visible = True
        blistErrorMsg.Items.Clear()
    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        ViewState("SavingStatus") = "new"

        tblSearchEmail.Visible = False
        tblGridviewEmail.Visible = False
        tblEmailInput.Visible = True

        ddlBranch.SelectedValue = 0
        ddlCompany.SelectedValue = 0
        ddlGroup.SelectedValue = 0
        txtDescription.Text = ""
        txtEmail.Text = ""

        blistErrorMsg.Items.Clear()

        btnCancel.Visible = True
        btnSave.Visible = True
        btnDelete.Visible = False
        cmdNew.Visible = False
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        'ViewState("SavingStatus") = "delete"
        lblPopTitle.value = "Delete Email"
        clsSession.Message = "Are you sure you want to delete this record?"
        clsSession.Icon = "inquiry"
        clsSession.DeleteStatus = "yes"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub
    Private Sub DeleteEmail()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            .CommandText = "USP_DeleteEmail"
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = 4
            .Parameters.Add("@RowID", SqlDbType.Int)
            .Parameters("@RowID").Value = ViewState("RowID")
            .ExecuteNonQuery()
        End With
    End Sub
    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If ViewState("SavingStatus") = "new" And ViewState("SavingStatus2") = Nothing Then


            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchEmail.Visible = True
            tblGridviewEmail.Visible = True
            tblEmailInput.Visible = False

            txtSearchInput.Text = ""
            FillGridView()
        
        ElseIf ViewState("SavingStatus") = "edit" And ViewState("SavingStatus2") = Nothing Then


            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchEmail.Visible = True
            tblGridviewEmail.Visible = True
            tblEmailInput.Visible = False

            FillGridView()
        ElseIf ViewState("SavingStatus2") = "duplicate" Then

        End If




        If clsSession.DeleteStatus = "yes" Then


            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchEmail.Visible = True
            tblGridviewEmail.Visible = True
            tblEmailInput.Visible = False


            DeleteEmail()

            ddlBranch.SelectedIndex = 0
            ddlCompany.SelectedIndex = 0
            ddlGroup.SelectedIndex = 0
            txtEmail.Text = ""
            txtDescription.Text = ""


            txtSearchInput.Text = ""
            FillGridView()
        End If
        GC.Collect()
    End Sub

 

    Protected Sub gvBranches_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBranches.PageIndexChanging
        Dim dt As New DataTable
        dt = CType(Session("dsEmail"), DataTable)
        gvBranches.PageIndex = e.NewPageIndex
        gvBranches.DataSource = dt
        gvBranches.DataBind()
        lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        clsSession.DeleteStatus = "cancel"
        Dim result As Integer

        blistErrorMsg.Items.Clear()

        If txtEmail.Text.Trim = "" Then
            blistErrorMsg.Items.Add("Email Address must not be blank.")
        Else
            If ddlCompany.SelectedValue <> "0" Then
                If ddlGroup.SelectedValue = "0" Or ddlBranch.SelectedValue = "0" Then
                    blistErrorMsg.Items.Add("Group/Branch must not be blank.")
                End If
            End If
        End If


        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        If ViewState("SavingStatus") = "edit" Then
            result = SaveEmail("USP_UpdateEmail")
            If result <> -1 Then
                lblPopTitle.value = "Update Successful"
                clsSession.Message = "Email record was successfully updated."
                clsSession.Icon = "success"
                ViewState("SavingStatus2") = Nothing

            Else
                ViewState("SavingStatus2") = "duplicate"
                clsSession.Icon = "error"
                lblPopTitle.value = "Update Failed"
                clssession.message = "Duplicate record. Updating record failed."

            End If
        ElseIf ViewState("SavingStatus") = "new" Then
            result = SaveEmail("USP_InsertEmail")
            If result <> -1 Then
                lblPopTitle.Value = "Save Successful"
                clsSession.Message = "Email record was successfully saved."
                clsSession.Icon = "success"
                ViewState("SavingStatus2") = Nothing

            Else
                ViewState("SavingStatus2") = "duplicate"
                clsSession.Icon = "error"
                lblPopTitle.Value = "Save Failed"
                clsSession.Message = "Duplicate record. Saving record failed."

            End If
        End If
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub


    Private Function SaveEmail(ByVal StoredProc As String) As Integer
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            .CommandText = StoredProc
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = 4
            .Parameters.Add("@CompCode", SqlDbType.SmallInt)
            .Parameters("@CompCode").Value = CInt(ddlCompany.SelectedValue)
            .Parameters.Add("@BranchCode", SqlDbType.VarChar)
            .Parameters("@BranchCode").Value = CInt(Me.ddlBranch.SelectedValue)
            .Parameters.Add("@GroupID", SqlDbType.VarChar)
            .Parameters("@GroupID").Value = ddlGroup.Text
            .Parameters.Add("@EmailAddress", SqlDbType.VarChar)
            .Parameters("@EmailAddress").Value = txtEmail.Text
            .Parameters.Add("@Description", SqlDbType.VarChar)
            .Parameters("@Description").Value = txtDescription.Text
            If StoredProc = "USP_UpdateEmail" Then
                .Parameters.Add("@RowID", SqlDbType.Int)
                .Parameters("@RowID").Value = ViewState("RowID")
            End If
            Return .ExecuteScalar()
        End With
    End Function

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillGridView()
    End Sub
End Class
