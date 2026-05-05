#Region " Imports "
Imports System.Data.SqlClient
Imports System.Data
#End Region

Partial Class CompanyMaintenance
    Inherits System.Web.UI.Page

#Region " Subs and Functions "


    Private Sub FillGridView()
        'If txtSearchInput.Text = "" Then
        'BindGridView("USP_SelectCompanies")
        'Else
        BindGridView("USP_SearchCompanies")
        'End If
    End Sub


    Private Sub BindGridView(ByVal StoredProc As String)
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New Data.SqlClient.SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = 4
        If StoredProc = "USP_SearchCompanies" Then
            sqlCmd.Parameters.Add("@WhereCriteria", SqlDbType.VarChar)
            sqlCmd.Parameters("@WhereCriteria").Value = txtSearchInput.Text.Trim
        End If
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_Companies")
        Dim dt As DataTable = ds.Tables("tbl_Companies")
        If dt.Rows.Count <> 0 Then
            Session("dsCompanies") = dt
            gvBranches.DataSource = dt
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = ""
            Me.lblRecordCount.Visible = True
            Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
        Else
            Session("tbl_Companies") = Nothing
            gvBranches.DataSource = Nothing
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = "No record found."
            Me.lblRecordCount.Visible = False
        End If

    End Sub

    Private Function SaveCompanies(ByVal StoredProc As String) As Integer
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
            .Parameters("@CompCode").Value = CInt(txtCompCode.Text)

            .Parameters.Add("@CompanyName", SqlDbType.VarChar)
            .Parameters("@CompanyName").Value = txtCompanyName.Text

            .Parameters.Add("@ShortName", SqlDbType.VarChar)
            .Parameters("@ShortName").Value = txtShortName.Text

            .Parameters.Add("@EnvCode", SqlDbType.VarChar)
            .Parameters("@EnvCode").Value = Me.ddlEnvCode.SelectedValue

            .Parameters.Add("@IsHidden", SqlDbType.SmallInt)
            .Parameters("@IsHidden").Value = chkIsActive.Checked

            If StoredProc = "USP_UpdateCompanies" Then
                .Parameters.Add("@RowID", SqlDbType.Int)
                .Parameters("@RowID").Value = CInt(hfRowID.Value)
            End If



            Return .ExecuteScalar()
        End With
    End Function

    Private Sub DeleteCompany()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            .CommandText = "USP_DeleteCompany"
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = 4
            .Parameters.Add("@RowID", SqlDbType.Int)
            .Parameters("@RowID").Value = CInt(hfRowID.Value)
            .ExecuteNonQuery()
        End With
    End Sub

    Protected Sub FillEnvironmentType()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT * FROM vw_SelectEnvironment ORDER BY EnvName ASC"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        ddlEnvCode.DataSource = dtTable
        ddlEnvCode.DataTextField = "EnvName"
        ddlEnvCode.DataValueField = "EnvCode"
        ddlEnvCode.DataBind()

    End Sub

#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FillGridView()
            tblSearchCompanies.Visible = True
            tblGridviewCompanies.Visible = True
            tblCompanyInput.Visible = False

            btnSave.Visible = False
            btnCancel.Visible = False
            btnDelete.Visible = False
        End If
    End Sub

    Protected Sub gvBranches_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBranches.RowCommand
        If e.CommandName = "select" Then
            FillEnvironmentType()
            ViewState("SavingStatus") = "edit"
            Dim i As Integer = 0
            Dim rowView As DataRowView
            Dim row As DataRow
            Dim dt As New DataTable
            dt = CType(Session("dsCompanies"), DataTable)

            For Each rowView In dt.DefaultView
                row = rowView.Row
                If row("RowID") = e.CommandArgument Then

                    Dim IsActiveValue As Boolean = CBool(dt.Rows(i)("IsHidden").ToString)

                    txtCompCode.Text = dt.Rows(i)("CompCode").ToString
                    txtCompanyName.Text = dt.Rows(i)("CompanyName").ToString
                    txtShortName.Text = dt.Rows(i)("ShortName").ToString
                    chkIsActive.Checked = CBool(dt.Rows(i)("IsHidden").ToString)
                    ddlEnvCode.SelectedValue = IIf(dt.Rows(i)("EnvCode").ToString = "", 0, dt.Rows(i)("EnvCode"))
                    hfRowID.Value = dt.Rows(i)("RowID").ToString

                    tblSearchCompanies.Visible = False
                    tblGridviewCompanies.Visible = False
                    tblCompanyInput.Visible = True

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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillGridView()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        ViewState("SavingStatus") = "delete"
        lblPopTitle.Value = "Delete Company"
        clsSession.Message = "Are you sure you want to delete this record?"
        clsSession.Icon = "inquiry"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub


    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If ViewState("SavingStatus") = "new" And ViewState("SavingStatus2") = Nothing Then

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompanies.Visible = True
            tblGridviewCompanies.Visible = True
            tblCompanyInput.Visible = False

            txtSearchInput.Text = ""
            FillGridView()
        ElseIf ViewState("SavingStatus") = "delete" Then

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompanies.Visible = True
            tblGridviewCompanies.Visible = True
            tblCompanyInput.Visible = False

            DeleteCompany()

            txtCompCode.Text = ""
            txtCompanyName.Text = ""
            txtShortName.Text = ""

            Me.ddlEnvCode.SelectedValue = 0

            txtSearchInput.Text = ""
            FillGridView()
        ElseIf ViewState("SavingStatus") = "edit" And ViewState("SavingStatus2") = Nothing Then


            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompanies.Visible = True
            tblGridviewCompanies.Visible = True
            tblCompanyInput.Visible = False

            FillGridView()
        ElseIf ViewState("SavingStatus2") = "duplicate" Then

        End If
        GC.Collect()
    End Sub

 

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        ViewState("SavingStatus") = "new"

        tblSearchCompanies.Visible = False
        tblGridviewCompanies.Visible = False
        tblCompanyInput.Visible = True

        txtCompCode.Text = ""
        txtCompanyName.Text = ""
        txtShortName.Text = ""

        ddlEnvCode.SelectedValue = 0

        blistErrorMsg.Items.Clear()

        btnCancel.Visible = True
        btnSave.Visible = True
        btnDelete.Visible = False
        cmdNew.Visible = False
    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        txtSearchInput.Text = ""
        FillGridView()

        tblSearchCompanies.Visible = True
        tblGridviewCompanies.Visible = True
        tblCompanyInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        cmdNew.Visible = True
        blistErrorMsg.Items.Clear()
    End Sub

    Protected Sub gvBranches_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBranches.PageIndexChanging
        Dim dt As New DataTable
        dt = CType(Session("dsCompanies"), DataTable)
        gvBranches.PageIndex = e.NewPageIndex
        gvBranches.DataSource = dt
        gvBranches.DataBind()
        Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result As Integer

        blistErrorMsg.Items.Clear()

        If txtCompCode.Text = "" Then
            blistErrorMsg.Items.Add("Company code must not be blank.")
        Else
            If Not IsNumeric(txtCompCode.Text.Trim) Then
                blistErrorMsg.Items.Add("Company code must be numeric.")
            End If
        End If

        If txtCompanyName.Text = "" Then
            blistErrorMsg.Items.Add("Company name must not be blank.")
        End If

        If txtShortName.Text = "" Then
            blistErrorMsg.Items.Add("Short name must not be blank.")
        End If

        If Me.ddlEnvCode.SelectedValue = 0 Then
            blistErrorMsg.Items.Add("Environment code must not be blank.")
        End If

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        If ViewState("SavingStatus") = "edit" Then
            result = SaveCompanies("USP_UpdateCompanies")
            If result <> -1 Then
                ViewState("SavingStatus2") = Nothing
                lblPopTitle.Value = "Update Successful"
                clsSession.Message = "Company record was successfully updated."
                clsSession.Icon = "success"
            Else
                ViewState("SavingStatus2") = "duplicate"
                lblPopTitle.Value = "Update Failed"
                clsSession.Message = "Duplicate record. Updating record failed."
                clsSession.Icon = "error"
            End If
        ElseIf ViewState("SavingStatus") = "new" Then
            result = SaveCompanies("USP_InsertCompanies")
            If result <> -1 Then
                lblPopTitle.Value = "Save Successful"
                clsSession.Message = "Company record was successfully saved."
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


  

#End Region

End Class
