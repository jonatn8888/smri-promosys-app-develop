
Imports System.Data.SqlClient
Imports System.Data



Partial Class DepartmentMaintenance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then
            FillGridView()
            tblSearchDepartment.Visible = True
            tblGridviewBranches.Visible = True
            tblBranchInput.Visible = False
            btnSave.Visible = False
            btnCancel.Visible = False
            btnDelete.Visible = False

            BindEnvironments()
            BindBizUnit()
            'txtDeptCode.Attributes.Add("readonly", "readonly")

            'txtDeptName.Attributes.Add("readonly", "readonly")
            'txtDeptShortName.Attributes.Add("readonly", "readonly")
        End If
    End Sub
    Private Sub BindEnvironments()
        Dim sqlConn As SqlConnection = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        Dim sqlReader As SqlDataReader

        Try
            sqlConn.Open()
            sqlCmd = New SqlCommand
            With sqlCmd
                .CommandText = "USP_GetEnvironment" ' Your stored procedure
                .Connection = sqlConn
                .CommandTimeout = 0
                .CommandType = CommandType.StoredProcedure
            End With

            sqlReader = sqlCmd.ExecuteReader()

            ddEnvironments.DataSource = sqlReader
            ddEnvironments.DataTextField = "ShortDesc"  ' Displayed text
            ddEnvironments.DataValueField = "EnvCode"   ' Value behind the scenes
            ddEnvironments.DataBind()

            ' Insert a default "select" option
            ddEnvironments.Items.Insert(0, New ListItem("-- Select Environment --", "0"))

        Catch ex As Exception
            ' Optional: Log or display error
            Throw ex
        Finally
            If sqlConn IsNot Nothing AndAlso sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try
    End Sub

    Private Sub BindBizUnit()
        Dim sqlConn As SqlConnection = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        Dim sqlReader As SqlDataReader

        Try
            sqlConn.Open()
            sqlCmd = New SqlCommand
            With sqlCmd
                .CommandText = "USP_SelectUserGroupsBizUnit" ' Your stored procedure
                .Connection = sqlConn
                .CommandTimeout = 0
                .CommandType = CommandType.StoredProcedure
            End With

            sqlReader = sqlCmd.ExecuteReader()

            ddlBizUnit.DataSource = sqlReader
            ddlBizUnit.DataTextField = "BizUnit"  ' Displayed text
            ddlBizUnit.DataValueField = "GroupID"   ' Value behind the scenes
            ddlBizUnit.DataBind()

        Catch ex As Exception
            ' Optional: Log or display error
            Throw ex
        Finally
            If sqlConn IsNot Nothing AndAlso sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        End Try
    End Sub


    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click


        If ViewState("SavingStatus") = "new" And ViewState("SavingStatus2") = Nothing Then
            'Me.panPopUp.Visible = False

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchDepartment.Visible = True
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
            cmdNew.Visible = True

            tblSearchDepartment.Visible = True
            tblGridviewBranches.Visible = True
            tblBranchInput.Visible = False

            FillGridView()
        ElseIf ViewState("SavingStatus2") = "duplicate" Then
            'Me.panPopUp.Visible = False
        End If

        If clsSession.DeleteStatus = "yes" Then
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchDepartment.Visible = True
            tblGridviewBranches.Visible = True
            tblBranchInput.Visible = False


            DeleteDepartment()
            txtDeptCode.Text = ""
            txtDeptName.Text = ""
            txtDeptShortName.Text = ""
            txtSearchInput.Text = ""
            FillGridView()
        End If
        GC.Collect()
    End Sub



    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        ViewState("SavingStatus") = "new"

        tblSearchDepartment.Visible = False
        tblGridviewBranches.Visible = False
        tblBranchInput.Visible = True

        txtDeptCode.Text = ""
        txtDeptName.Text = ""
        txtDeptShortName.Text = ""
        blistErrorMsg.Items.Clear()
        btnCancel.Visible = True
        btnSave.Visible = True
        btnDelete.Visible = False
        cmdNew.Visible = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        txtSearchInput.Text = ""
        FillGridView()


        tblSearchDepartment.Visible = True
        tblGridviewBranches.Visible = True
        tblBranchInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        cmdNew.Visible = True
        blistErrorMsg.Items.Clear()

    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillGridView()
        GridView1.Visible = True
        'If FillGridView() Then
        '    tblGridviewUsers.Visible = True
        '    lblValidateMessage.Text = ""
        'Else
        '   lblValidateMessage.Text = "No record found."
        '   Me.tblGridviewUsers.Visible = False
        'End If
    End Sub

    Protected Sub gvBranches_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Dim dt As New DataTable
        dt = CType(Session("dsDepartmentList"), DataTable)
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = dt
        GridView1.DataBind()
        Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
    End Sub



    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        '        ViewState("SavingStatus") = "delete"
        clsSession.DeleteStatus = "yes"

        lblPopTitle.Value = "Delete Department"
        clsSession.Message = "Are you sure you want to delete this record?"
        clsSession.Icon = "inquiry"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub

    Private Sub BindGridView(ByVal StoredProc As String)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        If StoredProc = "USP_SearchDepartment" Then
            sqlCmd.Parameters.Add("@WhereCriteria", SqlDbType.VarChar)
            sqlCmd.Parameters("@WhereCriteria").Value = txtSearchInput.Text.Trim
        End If
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_DepartmentList")
        Dim dt As DataTable = ds.Tables("tbl_DepartmentList")
        If dt.Rows.Count <> 0 Then
            Session("dsDepartmentList") = dt
            GridView1.DataSource = dt
            GridView1.DataBind()
            Me.lblValidateMessage.Text = ""
            Me.lblRecordCount.Visible = True
            Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
        Else
            Session("dsDepartmentList") = Nothing
            GridView1.DataSource = Nothing
            GridView1.DataBind()
            Me.lblValidateMessage.Text = "No record found."
            Me.lblRecordCount.Visible = False
        End If

    End Sub

    Private Sub FillGridView()
        'If txtSearchInput.Text = "" Then
        'BindGridView("USP_SelectBranches")
        'Else
        BindGridView("USP_SearchDepartment")
        'End If
    End Sub
    Private Function SaveDepartment(ByVal StoredProc As String) As Integer
        Using sqlConn As New SqlConnection(clsPromo.SQLConnString())
            Using sqlCmd As New SqlCommand(StoredProc, sqlConn)
                sqlCmd.CommandType = CommandType.StoredProcedure

                ' Clear any existing parameters
                sqlCmd.Parameters.Clear()

                ' Add parameters matching the stored procedure definition
                sqlCmd.Parameters.Add("@DepartmentCode", SqlDbType.SmallInt).Value = CInt(txtDeptCode.Text)
                sqlCmd.Parameters.Add("@EnvCode", SqlDbType.SmallInt).Value = CInt(ddEnvironments.SelectedValue)
                sqlCmd.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = txtDeptShortName.Text
                sqlCmd.Parameters.Add("@Category", SqlDbType.VarChar, 50).Value = ddCategory.SelectedValue
                sqlCmd.Parameters.Add("@Department", SqlDbType.VarChar, 50).Value = txtDeptName.Text
                sqlCmd.Parameters.Add("@BizUnitID", SqlDbType.VarChar, 50).Value = ddlBizUnit.SelectedItem.Text

                ' Add output parameter for result
                Dim resultParam As New SqlParameter("@Result", SqlDbType.Int)
                resultParam.Direction = ParameterDirection.Output
                sqlCmd.Parameters.Add(resultParam)

                ' Open connection and execute the command
                sqlConn.Open()
                sqlCmd.ExecuteNonQuery()



                Return Convert.ToInt32(resultParam.Value)

            End Using
        End Using
    End Function
    Private Function UpdateDepartment(ByVal StoredProc As String, ByVal GroupID As String) As Integer
        Using sqlConn As New SqlConnection(clsPromo.SQLConnString())
            Using sqlCmd As New SqlCommand(StoredProc, sqlConn)
                sqlCmd.CommandType = CommandType.StoredProcedure

                ' Clear any existing parameters
                sqlCmd.Parameters.Clear()

                ' Add parameters matching the stored procedure definition
                sqlCmd.Parameters.Add("@DepartmentCode", SqlDbType.SmallInt).Value = CInt(txtDeptCode.Text)
                sqlCmd.Parameters.Add("@EnvName", SqlDbType.VarChar, 100).Value = ddEnvironments.SelectedItem.Text
                sqlCmd.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = txtDeptShortName.Text
                sqlCmd.Parameters.Add("@Category", SqlDbType.VarChar, 50).Value = ddCategory.SelectedValue
                sqlCmd.Parameters.Add("@Department", SqlDbType.VarChar, 50).Value = txtDeptName.Text
                sqlCmd.Parameters.Add("@BizUnitID", SqlDbType.VarChar, 50).Value = ddlBizUnit.SelectedItem.Text
                sqlCmd.Parameters.Add("@GroupID", SqlDbType.SmallInt).Value = CInt(GroupID)
                ' Add output parameter for result
                Dim resultParam As New SqlParameter("@Result", SqlDbType.Int)
                resultParam.Direction = ParameterDirection.Output
                sqlCmd.Parameters.Add(resultParam)

                ' Open connection and execute the command
                sqlConn.Open()
                sqlCmd.ExecuteNonQuery()



                Return Convert.ToInt32(resultParam.Value)

            End Using
        End Using
    End Function



    Private Sub DeleteDepartment()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            .CommandText = "USP_DeleteDepartment"
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = 4
            .Parameters.Add("@GroupId", SqlDbType.Int)
            .Parameters("@GroupId").Value = CInt(hfRowID.Value)
            .ExecuteNonQuery()
        End With
    End Sub

    Protected Sub gvBranches_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        e.Row.Cells(0).Text = Format(Val(e.Row.Cells(0).Text), "0##")

    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        clsSession.DeleteStatus = "cancel"
        Dim result As Integer
        blistErrorMsg.Items.Clear()

        If txtDeptCode.Text = "" Then
            blistErrorMsg.Items.Add("Department code must not be blank.")
        Else
            If Not IsNumeric(txtDeptCode.Text.Trim) Then
                blistErrorMsg.Items.Add("Department code must be numeric.")
            End If
        End If
        If txtDeptName.Text = "" Then
            blistErrorMsg.Items.Add("Description must not be blank.")
        End If

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        If ViewState("SavingStatus") = "new" Then
            result = SaveDepartment("USP_InsertDepartment")
            If result = 1 Then
                ' Save successful
                ViewState("SavingStatus2") = Nothing
                lblPopTitle.Value = "Save Successful"
                clsSession.Message = "Department record was successfully saved."
                clsSession.Icon = "success"
                txtDeptCode.Text = ""
                txtDeptName.Text = ""
                txtDeptShortName.Text = ""

            ElseIf result = -1 Then
                ' Save failed due to duplicate
                ViewState("SavingStatus2") = "duplicate"
                lblPopTitle.Value = "Save Failed"
                clsSession.Message = "Duplicate record. Saving record failed."
                clsSession.Icon = "error"

            Else
                ' Handle any other failure
                ViewState("SavingStatus2") = "error"
                lblPopTitle.Value = "Save Failed"
                clsSession.Message = "An error occurred while saving the record."
                clsSession.Icon = "error"
            End If
        End If

        If ViewState("SavingStatus") = "edit" Then
            result = UpdateDepartment("USP_UpdateDepartment", ViewState("GroupID"))
            If result = 1 Then
                ' Save successful
                ViewState("SavingStatus2") = Nothing
                lblPopTitle.Value = "Update Successful"
                clsSession.Message = "Department record was successfully updated."
                clsSession.Icon = "success"
                txtDeptCode.Text = ""
                txtDeptName.Text = ""
                txtDeptShortName.Text = ""

            ElseIf result = -1 Then
                ' Save failed due to duplicate
                ViewState("SavingStatus2") = "duplicate"
                lblPopTitle.Value = "Update Failed"
                clsSession.Message = "Duplicate record. Update record failed."
                clsSession.Icon = "error"

            Else
                ' Handle any other failure
                ViewState("SavingStatus2") = "error"
                lblPopTitle.Value = "Update Failed"
                clsSession.Message = "An error occurred while updating the record."
                clsSession.Icon = "error"
            End If
        End If




        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")

    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "select" Then
            ViewState("SavingStatus") = "edit"

            Dim i As Integer = 0
            Dim rowView As DataRowView
            Dim row As DataRow
            Dim dt As New DataTable
            dt = CType(Session("dsDepartmentList"), DataTable)

            For Each rowView In dt.DefaultView
                row = rowView.Row
                If row("GroupId") = e.CommandArgument Then
                    ddCategory.SelectedValue = dt.Rows(i)("Category").ToString

                    ' Store BizUnit value for later selection
                    ViewState("SelectedBizUnit") = dt.Rows(i)("BizUnit").ToString

                    txtDeptCode.Text = dt.Rows(i)("DeptCode").ToString

                    'txtDeptName.Text = dt.Rows(i)("Description").ToString
                    'txtDeptShortName.Text = dt.Rows(i)("Department").ToString
                    txtDeptName.Text = dt.Rows(i)("Department").ToString
                    txtDeptShortName.Text = dt.Rows(i)("Description").ToString
                    hfRowID.Value = dt.Rows(i)("GroupId").ToString
                    'hfRowID.Value = ddEnvironments.SelectedValue
                    'ddEnvironments.Enabled = False
                    ViewState("GroupID") = dt.Rows(i)("GroupId").ToString
                    ' Populate Dropdowns
                    ddCategory.SelectedValue = dt.Rows(i)("Category").ToString


                    ' Get the clicked row
                    Dim rows As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                    ' Find the LinkButton in the row
                    Dim linkButtonEnvironment As LinkButton = CType(rows.FindControl("LinkButton5"), LinkButton)
                    ddEnvironments.SelectedItem.Text = linkButtonEnvironment.Text



                    Dim linkButtonBizUnit As LinkButton = CType(rows.FindControl("LinkButton4"), LinkButton)
                    ddlBizUnit.SelectedItem.Text = linkButtonBizUnit.Text







                    tblSearchDepartment.Visible = False
                    tblGridviewBranches.Visible = False
                    tblBranchInput.Visible = True

                    btnCancel.Visible = True
                    btnSave.Visible = True
                    btnDelete.Visible = True

                    cmdNew.Visible = False



                    Exit For
                End If
                i += 1
            Next
        End If
    End Sub


End Class