#Region "Imports"
Imports System.Data
Imports System.Data.SqlClient
#End Region

Partial Class ValueListMaintenance
    Inherits System.Web.UI.Page

#Region "Subs and Functions"

    Private Sub FillGridView()
        BindGridView()
    End Sub

    Private Sub BindGridView()
        Dim sqlConn As New SqlConnection(clsPromo.SQLConnString())
        Dim sql As String = "SELECT * FROM ResListValues WHERE GroupName = @GroupName" & _
                            "   AND Coalesce(EffectivityDate,DATEADD(SECOND, +1, GETDATE())) > GetDate()"
        Dim searchName As String = txtSearchInput.Text.Trim()
        Dim groupType As String = ddlSearchGroupType.SelectedValue

        If ViewState("LazyLoad") IsNot Nothing AndAlso ViewState("LazyLoad").ToString() = "False" Then
            sql &= " AND 1=0"
        Else
            If groupType <> "" Then sql &= " AND SubGroupName = @SubGroupName"
            If searchName <> "" Then sql &= " AND ElementName LIKE @ElementName"
        End If

        sql &= " ORDER BY GroupName, ElementID"

        Dim sqlCmd As New SqlCommand(sql, sqlConn)
        sqlCmd.Parameters.AddWithValue("@GroupName", "CompSponsorship")

        If groupType <> "" Then sqlCmd.Parameters.AddWithValue("@SubGroupName", groupType)
        If searchName <> "" Then sqlCmd.Parameters.AddWithValue("@ElementName", "%" & searchName & "%")

        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet

        sqlConn.Open()
        da.Fill(ds, "tbl_Res")
        sqlConn.Close()

        Dim dt As DataTable = ds.Tables("tbl_Res")

        If dt.Rows.Count > 0 Then
            Session("dsRes") = dt
            gvBranches.DataSource = dt
            gvBranches.DataBind()
            lblValidateMessage.Text = ""
            lblRecordCount.Visible = True
            lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count
        Else
            Session("dsRes") = Nothing
            gvBranches.DataSource = Nothing
            gvBranches.DataBind()
            lblValidateMessage.Text = If(ViewState("LazyLoad") = True, "No record found.", "Select Group Type and click Search")
            lblRecordCount.Visible = False
        End If

        tblSearchCompanies.Visible = True
        tblGridview.Visible = True
        tblCompanyInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        cmdNew.Visible = True
        blistErrorMsg.Items.Clear()
    End Sub

    Private Function BuildValue() As String
        Dim g As String = txtGroup.Text.Trim()
        Dim nm As String = txtName.Text.Trim().Replace(" ", "")
        Dim nick As String = txtNick.Text.Trim().Replace(" ", "")

        'Level 1: CompSponsorship_DAC
        'Level 2: CompSponsorship_DACFee
        'Level 3: CompSponsorship_DACFeeRate

        If nick <> "" Then
            Return g & "_" & nick
        End If

        Return g & "_" & nm
    End Function

    Private Function SaveData() As Integer
        Dim sqlConn As New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand("USP_SaveShoulderingEntityValues", sqlConn)

        Try
            sqlConn.Open()

            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.AddWithValue("@SavingStatus", ViewState("SavingStatus"))
            sqlCmd.Parameters.AddWithValue("@GroupName", txtGroup.Text.Trim())
            'sqlCmd.Parameters.AddWithValue("@ElementID", txtValue.Text.Trim())
            sqlCmd.Parameters.AddWithValue("@ElementName", txtName.Text.Trim())
            sqlCmd.Parameters.AddWithValue("@ElementNick", txtNick.Text.Trim())
            'sqlCmd.Parameters.AddWithValue("@ElementValue", txtValue.Text.Trim())
            sqlCmd.Parameters.AddWithValue("@SubGroupName", cmbGroupType.Text.Trim())
            sqlCmd.Parameters.AddWithValue("@IsActive", chkActive.Checked)

            If ViewState("SavingStatus") <> "new" Then
                sqlCmd.Parameters.AddWithValue("@ResourceID", hfRowID.Value)
            End If

            Return sqlCmd.ExecuteNonQuery()

        Catch ex As SqlException
            ViewState("ErrReturn") = ex.Message
            Return -1
        Finally
            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
            End If
        End Try
    End Function


    Private Sub DeleteRecord()
        Dim sqlConn As New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand
        sqlConn.Open()
        With sqlCmd
            '.CommandText = "DELETE FROM ResListValues WHERE ResourceID=" & hfRowID.Value
            .CommandText = "UPDATE ResListValues SET EffectivityDate = GetDate() WHERE ResourceID=" & hfRowID.Value
            .Connection = sqlConn
            .ExecuteNonQuery()
        End With

        lblPopTitle.Value = "Deleted Successfully"
        clsSession.Message = "Record successfully deleted."
        clsSession.Icon = "success"
        FillGridView()
        btnDelete.Visible = False
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
    End Sub

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        ViewState("LazyLoad") = False
        If Not IsPostBack Then
            FillGridView()

            'txtValue.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            tblSearchCompanies.Visible = True
            tblGridview.Visible = True
            tblCompanyInput.Visible = False

            txtGroup.Text = "CompSponsorship"
            txtGroup.Enabled = False

            btnSave.Visible = False
            btnDelete.Visible = False
            btnCancel.Visible = False
        End If
    End Sub

    Protected Sub gvBranches_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvBranches.RowCommand
        If e.CommandName = "select" Then
            ViewState("SavingStatus") = "edit"

            Dim dt As DataTable = CType(Session("dsRes"), DataTable)
            For Each row As DataRow In dt.Rows
                If row("ResourceID") = e.CommandArgument Then
                    txtGroup.Text = row("GroupName").ToString()
                    txtName.Text = row("ElementName").ToString()
                    txtNick.Text = row("ElementNick").ToString()
                    'txtValue.Text = row("ElementValue").ToString()
                    cmbGroupType.Text = row("SubGroupName").ToString()
                    chkActive.Checked = CBool(row("IsActive"))
                    hfRowID.Value = row("ResourceID").ToString()
                End If
            Next

            txtGroup.Enabled = False

            tblSearchCompanies.Visible = False
            tblGridview.Visible = False
            tblCompanyInput.Visible = True

            btnCancel.Visible = True
            btnSave.Visible = True
            cmdNew.Visible = False
            btnDelete.Visible = True
        End If
    End Sub

    Protected Sub gvBranches_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvBranches.PageIndexChanging
        Dim dt As DataTable = CType(Session("dsRes"), DataTable)
        gvBranches.PageIndex = e.NewPageIndex
        gvBranches.DataSource = dt
        gvBranches.DataBind()
        lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        ViewState("LazyLoad") = True
        FillGridView()
    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNew.Click
        ViewState("SavingStatus") = "new"
        ViewState("ErrReturn") = ""

        tblSearchCompanies.Visible = False
        tblGridview.Visible = False
        tblCompanyInput.Visible = True

        txtGroup.Text = "CompSponsorship"
        txtGroup.Enabled = False

        txtName.Text = ""
        txtNick.Text = ""
        'txtValue.Text = ""
        cmbGroupType.SelectedIndex = 0
        chkActive.Checked = False

        blistErrorMsg.Items.Clear()

        btnCancel.Visible = True
        btnSave.Visible = True
        cmdNew.Visible = False
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        txtSearchInput.Text = ""
        FillGridView()


    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        blistErrorMsg.Items.Clear()

        If txtName.Text = "" Then blistErrorMsg.Items.Add("Element Name is required.")
        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        If ViewState("SavingStatus") = "edit" Then
            Dim usageCount As Integer = GetShoulderingUsageCount(Convert.ToInt32(hfRowID.Value))

            If usageCount > 0 Then
                ViewState("process") = "approve_saving"
                lblPopTitle.Value = "Warning"
                clsSession.Message = "This value is referenced by " & usageCount.ToString() & _
                             " record(s) in ShoulderingEntity. Continue with update?"
                clsSession.Icon = "inquiry"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
                Exit Sub
            Else
                ViewState("process") = "approve_saving"
                lblPopTitle.Value = "Warning"
                clsSession.Message = "Are you sure you want to save the new Shouldering Entity?"
                clsSession.Icon = "inquiry"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
                Exit Sub
            End If
        ElseIf ViewState("SavingStatus") = "new" Then
            ViewState("process") = "approve_saving"
            lblPopTitle.Value = "Warning"
            clsSession.Message = "Are you sure you want to save the new Shouldering Entity?"
            clsSession.Icon = "inquiry"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        End If



        'ContinueSave()
    End Sub

    Private Sub ContinueSave()
        Dim result As Integer = SaveData()

        If result >= 1 Or result = 0 Then
            lblPopTitle.Value = "Save Successful"
            clsSession.Message = "Record successfully saved."
            clsSession.Icon = "success"
            FillGridView()
            btnDelete.Visible = False
        Else
            lblPopTitle.Value = "Save Failed"
            clsSession.Message = "Error: " & ViewState("ErrReturn")
            clsSession.Icon = "error"
        End If
        
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")


        'Response.Redirect("ValueListMaintenance.aspx")
    End Sub


    Private Function GetShoulderingUsageCount(ByVal resourceID As Integer) As Integer
        Dim sqlConn As New SqlConnection(clsPromo.SQLConnString())
        Dim sql As String = "SELECT COUNT(*) FROM ShoulderingEntity WHERE ShoulderingEntity_ResourceID = @ID"
        Dim sqlCmd As New SqlCommand(sql, sqlConn)
        sqlCmd.Parameters.AddWithValue("@ID", resourceID)

        sqlConn.Open()
        Dim count As Integer = Convert.ToInt32(sqlCmd.ExecuteScalar())
        sqlConn.Close()

        Return count
    End Function

    'Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
    '    clsSession.DeleteStatus = "cancel"
    '    ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.MsgBoxwindow.hide();</script>")
    'End Sub

#End Region

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If ViewState("SavingStatus") = "edit" Then
            Dim usageCount As Integer = GetShoulderingUsageCount(Convert.ToInt32(hfRowID.Value))

            If usageCount > 0 Then
                'lblPopTitle.Value = "Error"
                clsSession.Message = "This value is used by " & usageCount.ToString() & _
             " record(s) in ShoulderingEntity and cannot be modified."
                lblPopTitle.Value = "Confirm Deletion"
                clsSession.Icon = "inquiry"
                ViewState("process") = "approve_deletion"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
                'Exit Sub    'Stop here, don't save
            Else
                clsSession.Message = "Are you sure you want to delete this Shouldering Entity?"
                lblPopTitle.Value = "Confirm Deletion"
                clsSession.Icon = "inquiry"
                ViewState("process") = "approve_deletion"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
            End If
        End If
    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdPopUpOK.Click
        If ViewState("NeedWarningConfirm") IsNot Nothing AndAlso ViewState("NeedWarningConfirm") = True Then
            ViewState("NeedWarningConfirm") = Nothing
            ContinueSave()   ' <-- proceed with update
            Exit Sub
        End If

        'normal OK closes modal
        btnCancel.Visible = False
        btnSave.Visible = False
        cmdNew.Visible = True

        tblSearchCompanies.Visible = True
        tblGridview.Visible = True
        tblCompanyInput.Visible = False

        txtSearchInput.Text = ""
        FillGridView()
    End Sub


    Protected Sub cmdRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRedirect.Click
        If clsSession.DeleteStatus = "yes" Then
            Select Case ViewState("process").ToString()

                Case "approve_saving"
                    ContinueSave()
                    '    SubmitForFinalApproval()


                Case "approve_deletion"
                    DeleteRecord()
                    ' Exit Sub
                Case Else

                    ' do nothing -- INVALID PROCESS ENTRY

            End Select

            If ViewState("ErrReturn") = "" Then
                FillGridView() 'Response.Redirect("ValueListMaintenance.aspx")
            Else
                ViewState("ErrReturn") = ""
            End If


        End If
    End Sub

End Class
