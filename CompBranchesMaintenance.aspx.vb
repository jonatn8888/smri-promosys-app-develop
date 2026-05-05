#Region " Imports "
Imports System.Data.SqlClient
Imports System.Data
#End Region

Partial Class CompBranchesMaintenance
    Inherits System.Web.UI.Page



#Region " Subs and Functions "



    Private Sub FillGridView()
        If txtSearchInput.Text = "" Then
            BindGridView("USP_SelectCompBranches")
        Else
            BindGridView("USP_SearchCompBranches")
        End If
    End Sub


    Private Sub BindGridView(ByVal StoredProc As String)
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New Data.SqlClient.SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = 4
        If StoredProc = "USP_SearchCompBranches" Then
            sqlCmd.Parameters.Add("@WhereCriteria", SqlDbType.VarChar)
            sqlCmd.Parameters("@WhereCriteria").Value = txtSearchInput.Text.Trim
        End If
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_CompBranches")
        Dim dt As DataTable = ds.Tables("tbl_CompBranches")
        If dt.Rows.Count <> 0 Then
            Session("dsCompBranches") = dt
            gvBranches.DataSource = dt
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = ""
            Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
        Else
            Session("tbl_CompBranches") = Nothing
            gvBranches.DataSource = Nothing
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = "No record found."
            Me.lblRecordCount.Text = ""
        End If
    End Sub

    Private Function SaveCompBranches(ByVal StoredProc As String) As Integer
        Using sqlConn As New SqlConnection(clsPromo.SQLConnString())
            Using sqlCmd As New SqlCommand(StoredProc, sqlConn)
                sqlConn.Open()

                With sqlCmd
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = 0

                    .Parameters.Add("@CompCode", SqlDbType.SmallInt).Value = CShort(ddlCompCode.SelectedValue)

                    .Parameters.Add("@StoreCompCode", SqlDbType.SmallInt).Value = CShort(ddlVSCompCode.SelectedValue)

                    .Parameters.Add("@BranchCode", SqlDbType.SmallInt).Value = CShort(ddlBrancode.SelectedValue)

                    .Parameters.Add("@OtherInfo", SqlDbType.VarChar, 50).Value = txtOtherInfo.Text.Trim()

                    .Parameters.Add("@NoDiscFile", SqlDbType.Bit).Value = If(chkDiscFile.Checked, 0, 1) ' reverse

                    .Parameters.Add("@IsVisualStore", SqlDbType.Bit).Value = If(chkIsVisual.Checked, 1, 0)

                    .Parameters.Add("@IsActive", SqlDbType.Bit).Value = chkIsActive.Checked

                    .Parameters.Add("@StoreGroup", SqlDbType.VarChar, 20).Value = ddlStoreGroup.SelectedValue.ToString()

                    If StoredProc = "USP_UpdateCompBranches" Then
                        .Parameters.Add("@RowID", SqlDbType.Int).Value = CInt(hfRowID.Value)
                    End If

                    .Parameters.Add("@LoginUserID", SqlDbType.VarChar, 20).Value = SystemUser.UserID

                    Return CInt(.ExecuteScalar())
                End With
            End Using
        End Using
    End Function


    Private Sub DeleteCompany()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            .CommandText = "USP_DeleteCompBranch"
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = 4
            .Parameters.Add("@RowID", SqlDbType.Int)
            .Parameters("@RowID").Value = CInt(hfRowID.Value)
            .ExecuteNonQuery()
        End With
    End Sub

    Protected Sub FillTypeStoreGroup()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT CASE WHEN SubGroupName = 'Online Store' then  ElementName else  " & _
                    " Concat(SubGroupName,' - ',ElementName) end as ElementName, ElementValue " & _
                   "FROM ResListValues " & _
                   "WHERE GroupName = 'StoreGroup'"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        ddlStoreGroup.DataSource = dtTable
        ddlStoreGroup.DataTextField = "ElementName"
        ddlStoreGroup.DataValueField = "ElementValue"
        ddlStoreGroup.DataBind()

        ddlStoreGroup.Items.Insert(0, New ListItem("-- Select Value --", "-1"))

    End Sub

    Protected Sub FillTypeStoreCompany()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT CompanyName as StoreCompanyName, CompCode as StoreCompCode " & _
                   "FROM Companies " & _
                   "WHERE IsHidden = 0 " & _
                   "ORDER BY COMPANYNAME ASC"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        ddlVSCompCode.DataSource = dtTable
        ddlVSCompCode.DataTextField = "StoreCompanyName"   ' 👈 REQUIRED
        ddlVSCompCode.DataValueField = "StoreCompCode" ' 👈 REQUIRED
        ddlVSCompCode.DataBind()

        ddlVSCompCode.Items.Insert(0, New ListItem("-- Select Value --", "-1"))

    End Sub



#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FillGridView()
            tblSearchCompBranch.Visible = True
            tblGridviewCompBranch.Visible = True
            tblCompBranchInput.Visible = False

            btnSave.Visible = False
            btnCancel.Visible = False
            btnDelete.Visible = False
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillGridView()
    End Sub

    Protected Sub gvBranches_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBranches.PageIndexChanging
        Dim dt As New DataTable
        dt = CType(Session("dsCompBranches"), DataTable)
        gvBranches.PageIndex = e.NewPageIndex
        gvBranches.DataSource = dt
        gvBranches.DataBind()
        lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
    End Sub

    Protected Sub gvBranches_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBranches.RowCommand

        If e.CommandName = "select" Then
            FillTypeStoreGroup()
            FillTypeStoreCompany()
            ViewState("SavingStatus") = "edit"
            Dim i As Integer = 0
            Dim rowView As DataRowView
            Dim row As DataRow
            Dim dt As New DataTable
            dt = CType(Session("dsCompBranches"), DataTable)

            For Each rowView In dt.DefaultView
                row = rowView.Row
                If row("RowID") = e.CommandArgument Then

                    lblCompBranch.Text = Right("000" + dt.Rows(i)("CompCode").ToString, 3) _
                                        + "-" + Right("0000" + dt.Rows(i)("BranchCode").ToString, 4)

                    Dim IsActiveValue As Boolean = CBool(dt.Rows(i)("InActive").ToString)

                    Me.ddlCompCode.SelectedValue = dt.Rows(i)("CompCode").ToString
                    Me.ddlVSCompCode.SelectedValue = IIf(String.IsNullOrEmpty(dt.Rows(i)("StoreCompCode").ToString), "-1", dt.Rows(i)("StoreCompCode").ToString)
                    Me.ddlBrancode.SelectedValue = dt.Rows(i)("BranchCode").ToString
                    Me.txtOtherInfo.Text = dt.Rows(i)("OtherInfo").ToString
                    Me.chkDiscFile.Checked = Not CBool(dt.Rows(i)("NoDiscFile").ToString)
                    Me.chkIsVisual.Checked = dt.Rows(i)("IsVisualStore").ToString
                    Me.ddlStoreGroup.SelectedValue = IIf(String.IsNullOrEmpty(dt.Rows(i)("StoreGroupId").ToString()), "-1", dt.Rows(i)("StoreGroupId").ToString())
                    Me.chkIsActive.Checked = IsActiveValue

                    hfRowID.Value = dt.Rows(i)("RowID").ToString

                    tblSearchCompBranch.Visible = False
                    tblGridviewCompBranch.Visible = False
                    tblCompBranchInput.Visible = True


                    btnCancel.Visible = True
                    btnSave.Visible = True
                    btnDelete.Visible = True
                    cmdNew.Visible = False

                    Exit For
                End If
                i = i + 1
            Next
        End If

        lblCompBranch.Visible = True
    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        ViewState("SavingStatus") = "new"

        'lblCompBranch.Visible = False
        FillTypeStoreGroup()
        FillTypeStoreCompany()
        lblCompBranch.Text = ""
        Me.ddlCompCode.ClearSelection()
        Me.ddlBrancode.ClearSelection()

        tblSearchCompBranch.Visible = False
        tblGridviewCompBranch.Visible = False
        tblCompBranchInput.Visible = True


        chkDiscFile.Checked = False
        chkIsVisual.Checked = False
        chkIsActive.Checked = False
        txtOtherInfo.Text = ""
        ' ddlBrancode.SelectedIndex = 0
        ' ddlCompCode.SelectedValue = 0


        blistErrorMsg.Items.Clear()

        btnCancel.Visible = True
        btnSave.Visible = True
        btnDelete.Visible = False
        cmdNew.Visible = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        txtSearchInput.Text = ""
        FillGridView()


        tblSearchCompBranch.Visible = True
        tblGridviewCompBranch.Visible = True
        tblCompBranchInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        cmdNew.Visible = True
        blistErrorMsg.Items.Clear()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim result As Integer

        blistErrorMsg.Items.Clear()

        If ddlBrancode.SelectedValue = "0" Then
            blistErrorMsg.Items.Add("Branch name must not be blank.")
        End If

        If ddlCompCode.SelectedValue = "0" Then
            blistErrorMsg.Items.Add("Company name must not be blank.")
        End If

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        If ViewState("SavingStatus") = "edit" Then
            result = SaveCompBranches("USP_UpdateCompBranches")
            If result <> -1 Then
                lblPopTitle.Text = "Update Successful"
                litPopMessage.Text = "Company-Branch record was successfully updated."
                ViewState("SavingStatus2") = Nothing
                panPopUp.Visible = True
            Else
                ViewState("SavingStatus2") = "duplicate"
                lblPopTitle.Text = "Update Failed"
                litPopMessage.Text = "Duplicate record. Updating record failed."
                panPopUp.Visible = True
            End If
        ElseIf ViewState("SavingStatus") = "new" Then
            result = SaveCompBranches("USP_InsertCompBranches")
            If result <> -1 Then
                lblPopTitle.Text = "Save Successful"
                litPopMessage.Text = "Company-Branch record was successfully saved."
                ViewState("SavingStatus2") = Nothing
                panPopUp.Visible = True
            Else
                ViewState("SavingStatus2") = "duplicate"
                lblPopTitle.Text = "Save Failed"
                litPopMessage.Text = "Duplicate record. Saving record failed."
                panPopUp.Visible = True
            End If
        End If

    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If ViewState("SavingStatus") = "new" And ViewState("SavingStatus2") = Nothing Then
            Me.panPopUp.Visible = False

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompBranch.Visible = True
            tblGridviewCompBranch.Visible = True
            tblCompBranchInput.Visible = False

            txtSearchInput.Text = ""
            FillGridView()
        ElseIf ViewState("SavingStatus") = "delete" Then
            Me.panPopUp.Visible = False

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompBranch.Visible = True
            tblGridviewCompBranch.Visible = True
            tblCompBranchInput.Visible = False


            DeleteCompany()

            ddlBrancode.SelectedIndex = 0
            'ddlCompCode.SelectedValue = 0
            ddlCompCode.SelectedIndex = 0
            txtOtherInfo.Text = ""
            chkDiscFile.Checked = False
            chkIsVisual.Checked = False

            txtSearchInput.Text = ""
            FillGridView()
        ElseIf ViewState("SavingStatus") = "edit" And ViewState("SavingStatus2") = Nothing Then
            Me.panPopUp.Visible = False

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompBranch.Visible = True
            tblGridviewCompBranch.Visible = True
            tblCompBranchInput.Visible = False

            FillGridView()
        ElseIf ViewState("SavingStatus2") = "duplicate" Then
            Me.panPopUp.Visible = False
        End If
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.panPopUp.Visible = False
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        ViewState("SavingStatus") = "delete"
        lblPopTitle.Text = "Delete Company-Branch"
        litPopMessage.Text = "Are you sure you want to delete this record?"
        panPopUp.Visible = True
    End Sub

    Protected Sub imgClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        panPopUp.Visible = False
    End Sub

#End Region
    
    Protected Sub ddlCompCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompCode.SelectedIndexChanged
        UpdateLabelCode()
    End Sub

    Protected Sub ddlBrancode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBrancode.SelectedIndexChanged
        UpdateLabelCode()
    End Sub

    Private Sub UpdateLabelCode()
        Dim companyCode As String = ""
        Dim branchCode As String = ""

        ' Get selected value from Company dropdown
        If ddlCompCode.SelectedIndex <> -1 AndAlso ddlCompCode.SelectedValue <> "" Then
            companyCode = ddlCompCode.SelectedValue
        End If

        ' Get selected value from Branch dropdown
        If ddlBrancode.SelectedIndex <> -1 AndAlso ddlBrancode.SelectedValue <> "" Then
            branchCode = ddlBrancode.SelectedValue
        End If

        lblCompBranch.Text = companyCode & "-" & branchCode
    End Sub
End Class
