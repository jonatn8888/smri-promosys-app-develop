
Imports System.Data.SqlClient
Imports System.Data

Partial Class BranchMaintenance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then
            FillGridView()
            tblSearchBranches.Visible = True
            tblGridviewBranches.Visible = True
            tblBranchInput.Visible = False
            btnSave.Visible = False
            btnCancel.Visible = False
            btnDelete.Visible = False

            txtBranchCode.Attributes.Add("readonly", "readonly")
            txtMMSName.Attributes.Add("readonly", "readonly")
            txtLongName.Attributes.Add("readonly", "readonly")
        End If
    End Sub


    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If ViewState("SavingStatus") = "new" And ViewState("SavingStatus2") = Nothing Then
            'Me.panPopUp.Visible = False

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            'cmdNew.Visible = True

            tblSearchBranches.Visible = True
            tblGridviewBranches.Visible = True
            tblBranchInput.Visible = False

            txtSearchInput.Text = ""
            FillGridView()
            'ElseIf ViewState("SavingStatus") = "delete" Then
            '    'Me.panPopUp.Visible = False

            '    If clsSession.DeleteStatus <> "cancel" Then


            '    End If

        ElseIf ViewState("SavingStatus") = "edit" And ViewState("SavingStatus2") = Nothing Then
            'Me.panPopUp.Visible = False

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            'cmdNew.Visible = True

            tblSearchBranches.Visible = True
            tblGridviewBranches.Visible = True
            tblBranchInput.Visible = False

            FillGridView()
        ElseIf ViewState("SavingStatus2") = "duplicate" Then
            'Me.panPopUp.Visible = False
        End If

        If clsSession.DeleteStatus = "yes" Then
            btnSave.Visible = False
            btnDelete.Visible = False
            'cmdNew.Visible = True

            tblSearchBranches.Visible = True
            tblGridviewBranches.Visible = True
            tblBranchInput.Visible = False


            DeleteBranch()

            txtBranchCode.Text = ""
            txtLongName.Text = ""
            txtShortName.Text = ""
            chkDeptStore.Checked = False
            txtMMSName.Text = ""

            txtSearchInput.Text = ""
            FillGridView()
        End If
        GC.Collect()
    End Sub



    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        ViewState("SavingStatus") = "new"

        tblSearchBranches.Visible = False
        tblGridviewBranches.Visible = False
        tblBranchInput.Visible = True

        txtBranchCode.Text = ""
        txtLongName.Text = ""
        txtShortName.Text = ""
        chkDeptStore.Checked = False
        txtMMSName.Text = ""

        blistErrorMsg.Items.Clear()

        btnCancel.Visible = True
        btnSave.Visible = True
        btnDelete.Visible = False
        cmdNew.Visible = False


    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        txtSearchInput.Text = ""
        FillGridView()


        tblSearchBranches.Visible = True
        tblGridviewBranches.Visible = True
        tblBranchInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        'cmdNew.Visible = True
        blistErrorMsg.Items.Clear()

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillGridView()
    End Sub

    Protected Sub gvBranches_PageIndexChanging(ByVal sender As Object,ByVal e As GridViewPageEventArgs) Handles gvBranches.PageIndexChanging

        gvBranches.PageIndex = e.NewPageIndex
        FillGridView()   ' rebind SAME query

    End Sub




    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        '        ViewState("SavingStatus") = "delete"
        clsSession.DeleteStatus = "yes"

        lblPopTitle.Value = "Delete Branch"
        clsSession.Message = "Are you sure you want to delete this record?"
        clsSession.Icon = "inquiry"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub

    Private Sub BindGridView(ByVal StoredProc As String)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        If StoredProc = "USP_SearchBranches" Then
            sqlCmd.Parameters.Add("@WhereCriteria", SqlDbType.VarChar)
            sqlCmd.Parameters("@WhereCriteria").Value = txtSearchInput.Text.Trim
        End If
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_Branches")
        Dim dt As DataTable = ds.Tables("tbl_Branches")
        If dt.Rows.Count <> 0 Then
            Session("dsBranches") = dt
            gvBranches.DataSource = dt
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = ""
            Me.lblRecordCount.Visible = True
            Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
        Else
            Session("dsBranches") = Nothing
            gvBranches.DataSource = Nothing
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = "No record found."
            Me.lblRecordCount.Visible = False
        End If

    End Sub

    Private Sub FillGridView()
        'If txtSearchInput.Text = "" Then
        'BindGridView("USP_SelectBranches")
        'Else
        BindGridView("USP_SearchBranches")
        'End If
    End Sub

    Private Function SaveBranches(ByVal StoredProc As String) As Integer
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
            .Parameters.Add("@BranchCode", SqlDbType.SmallInt)
            .Parameters("@BranchCode").Value = CInt(txtBranchCode.Text)

            .Parameters.Add("@BranchName", SqlDbType.VarChar)
            .Parameters("@BranchName").Value = txtLongName.Text

            .Parameters.Add("@ShortName", SqlDbType.VarChar)
            .Parameters("@ShortName").Value = txtShortName.Text

            .Parameters.Add("@MMSname", SqlDbType.VarChar)
            .Parameters("@MMSname").Value = txtMMSName.Text

            .Parameters.Add("@IsDeptStore", SqlDbType.Bit)
            .Parameters("@IsDeptStore").Value = IIf(chkDeptStore.Checked = True, 1, 0)

            .Parameters.Add("@IsHidden", SqlDbType.SmallInt)
            .Parameters("@IsHidden").Value = chkIsActive.Checked

            If StoredProc <> "USP_InsertBranches" Then
                .Parameters.Add("@RowID", SqlDbType.Int)
                .Parameters("@RowID").Value = CInt(hfRowID.Value)
            End If

            Return .ExecuteScalar()
        End With
    End Function

    Private Sub DeleteBranch()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            .CommandText = "USP_DeleteBranch"
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = 4
            .Parameters.Add("@RowID", SqlDbType.Int)
            .Parameters("@RowID").Value = CInt(hfRowID.Value)
            .ExecuteNonQuery()
        End With
    End Sub

    Protected Sub gvBranches_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBranches.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = Format(Val(e.Row.Cells(0).Text), "0##")
        End If

    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        clsSession.DeleteStatus = "cancel"
        Dim result As Integer
        blistErrorMsg.Items.Clear()
        If txtBranchCode.Text = "" Then
            blistErrorMsg.Items.Add("Branch code must not be blank.")
        Else
            If Not IsNumeric(txtBranchCode.Text.Trim) Then
                blistErrorMsg.Items.Add("Branch code must be numeric.")
            End If
        End If
        If txtLongName.Text = "" Then
            blistErrorMsg.Items.Add("Branch name must not be blank.")
        End If
        If txtMMSName.Text = "" Then
            blistErrorMsg.Items.Add("MMS name must not be blank.")
        End If
        If txtShortName.Text = "" Then
            blistErrorMsg.Items.Add("Short name must not be blank.")
        End If
        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        If ViewState("SavingStatus") = "edit" Then
            result = SaveBranches("USP_UpdateBranches")
            If result <> -1 Then
                lblPopTitle.Value = "Update Successful"
                clsSession.Message = "Branch record was successfully updated."
                clsSession.Icon = "success"
                ViewState("SavingStatus2") = Nothing
            Else
                ViewState("SavingStatus2") = "duplicate"

                lblPopTitle.Value = "Update Failed"
                clsSession.Message = "Duplicate record. Updating record failed."
                clsSession.Icon = "error"
            End If
        ElseIf ViewState("SavingStatus") = "new" Then
            result = SaveBranches("USP_InsertBranches")
            If result <> -1 Then
                ViewState("SavingStatus2") = Nothing
                lblPopTitle.Value = "Save Successful"
                clsSession.Message = "Branch record was successfully saved."
                clsSession.Icon = "success"
            Else
                ViewState("SavingStatus2") = "duplicate"
                lblPopTitle.Value = "Save Failed"
                clsSession.Message = "Duplicate record. Saving record failed."
                clsSession.Icon = "error"
            End If
        End If

        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")

    End Sub

    Protected Sub gvBranches_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBranches.RowCommand
        If e.CommandName = "select" Then

            ViewState("SavingStatus") = "edit"

            Dim i As Integer = 0
            Dim rowView As DataRowView
            Dim row As DataRow
            Dim dt As New DataTable
            dt = CType(Session("dsBranches"), DataTable)

            For Each rowView In dt.DefaultView
                row = rowView.Row
                If row("RowID") = e.CommandArgument Then
                    txtBranchCode.Text = dt.Rows(i)("BranchCode").ToString
                    txtLongName.Text = dt.Rows(i)("BranchName").ToString
                    txtShortName.Text = dt.Rows(i)("ShortName").ToString
                    chkDeptStore.Checked = dt.Rows(i)("IsDeptStore").ToString
                    chkIsActive.Checked = CBool(dt.Rows(i)("IsHidden").ToString)
                    txtMMSName.Text = dt.Rows(i)("MMSname").ToString
                    hfRowID.Value = dt.Rows(i)("RowID").ToString

                    tblSearchBranches.Visible = False
                    tblGridviewBranches.Visible = False
                    tblBranchInput.Visible = True

                    btnCancel.Visible = True
                    btnSave.Visible = True
                    btnDelete.Visible = True
                    cmdNew.Visible = False

                    Exit For
                End If
                i = i + 1
            Next
        Else


        End If
    End Sub




End Class
