#Region " Imports "
Imports System.Data.SqlClient
Imports System.Data
#End Region

Partial Class eWalletPartners
    Inherits System.Web.UI.Page

#Region " Subs and Functions "

    Private Sub FillGridView()
        BindGridView()
    End Sub

    Private Sub RefreshPage()
        txtSearchInput.Text = ""
        FillGridView()

        tblSearchCompanies.Visible = True
        tblGridview.Visible = True
        tblCompanyInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        cmdNew.Visible = True
        blistErrorMsg.Items.Clear()
    End Sub

    Private Sub BindGridView()
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())

        Dim sql As String = "SELECT * FROM eWalletPartners WHERE  Coalesce(EffectivityEndDate,DATEADD(SECOND, +1, GETDATE())) > GetDate()"
        If txtSearchInput.Text.Trim <> "" Then
            sql = sql & "AND (Description LIKE '%" & txtSearchInput.Text.Trim & "%' )  "
        End If
        sql = sql & " ORDER BY Description"

        Dim sqlCmd As New Data.SqlClient.SqlCommand(sql, sqlConn)
        sqlCmd.CommandType = CommandType.Text
        sqlCmd.CommandText = sql
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_eWallet")
        Dim dt As DataTable = ds.Tables("tbl_eWallet")
        If dt.Rows.Count <> 0 Then
            Session("dsWallet") = dt
            gvBranches.DataSource = dt
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = ""
            Me.lblRecordCount.Visible = True
            Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
        Else
            Session("dsWallet") = Nothing
            gvBranches.DataSource = Nothing
            gvBranches.DataBind()
            Me.lblValidateMessage.Text = "No record found."
            Me.lblRecordCount.Visible = False
        End If

    End Sub

    Private Function SaveData() As Integer
        On Error GoTo HELL

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd

            If ViewState("SavingStatus") = "new" Then
                .CommandText = " IF EXISTS  (SELECT 1 FROM eWalletPartners  WHERE Description = '" & txtDescription.Text.Trim() & "' AND EffectivityEndDate is null) " & _
                                " BEGIN " & _
                                " RAISERROR('Duplicate record already exists.', 16, 1) " & _
                                " END " & _
                                " ELSE IF EXISTS  (SELECT 1 FROM eWalletPartners  WHERE Description = '" & txtDescription.Text.Trim() & "' AND EffectivityEndDate < GetDate()) " & _
                                " BEGIN " & _
                                " UPDATE eWalletPartners SET EffectivityEndDate = null, Description='" & txtDescription.Text.Trim() & "' WHERE Description='" & txtDescription.Text.Trim() & "'" & _
                                " END " & _
                                " ELSE " & _
                                " BEGIN " & _
                                " INSERT INTO eWalletPartners (Description) VALUES ('" & txtDescription.Text.Trim() & "')" & _
                                " END "
            Else
                .CommandText = "UPDATE eWalletPartners SET Description='" & txtDescription.Text.Trim() & "' WHERE RowID=" & hfRowID.Value.ToString()
            End If
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = CommandType.Text

            Return .ExecuteScalar()
        End With

HELL:
        Return -1
    End Function

    Private Sub DeleteRecord()
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        With sqlCmd
            '.CommandText = "DELETE eWalletPartners WHERE Description='" & txtDescription.Text.Trim() & "'"
            .CommandText = "UPDATE eWalletPartners SET EffectivityEndDate = GetDate() WHERE Description='" & txtDescription.Text.Trim() & "'"
            .Connection = sqlConn
            .CommandTimeout = 0
            .CommandType = CommandType.Text
            .ExecuteNonQuery()
        End With
        clsSession.Message = "EWallet Partner record was successfully deleted."
        clsSession.Icon = "success"
    End Sub

    Private Function GetShoulderingUsageCount(ByVal resourceID As Integer) As Integer
        Dim sqlConn As New SqlConnection(clsPromo.SQLConnString())
        Dim sql As String = "SELECT COUNT(*) FROM ShoulderingEntity WHERE SpecificPartner_RowID = @ID"
        Dim sqlCmd As New SqlCommand(sql, sqlConn)
        sqlCmd.Parameters.AddWithValue("@ID", resourceID)

        sqlConn.Open()
        Dim count As Integer = Convert.ToInt32(sqlCmd.ExecuteScalar())
        sqlConn.Close()

        Return count
    End Function


#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FillGridView()
            tblSearchCompanies.Visible = True
            tblGridview.Visible = True
            tblCompanyInput.Visible = False

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
            dt = CType(Session("dsWallet"), DataTable)

            For Each rowView In dt.DefaultView
                row = rowView.Row
                If row("RowID") = e.CommandArgument Then
                    txtDescription.Text = dt.Rows(i)("Description").ToString

                    hfRowID.Value = dt.Rows(i)("RowID").ToString

                    tblSearchCompanies.Visible = False
                    tblGridview.Visible = False
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


        Dim usageCount As Integer = GetShoulderingUsageCount(Convert.ToInt32(hfRowID.Value))

        If usageCount > 0 Then
            clsSession.Message = "This value is used by " & usageCount.ToString() & _
            " record(s), are you sure you want to delete this values."
            lblPopTitle.Value = "Confirm Deletion"
            clsSession.Icon = "inquiry"
            ViewState("process") = "approve_deletion"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
            'Exit Sub    'Stop here, don't save
        Else
            clsSession.Message = "Are you sure want to delete this record"
            lblPopTitle.Value = "Confirm Deletion"
            clsSession.Icon = "inquiry"
            ViewState("process") = "approve_deletion"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
            Exit Sub
        End If

        'lblPopTitle.Value = "Delete eWallet Partner"
        'clsSession.Message = "Are you sure you want to delete this record?"
        'clsSession.Icon = "inquiry"
        'ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub


    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If ViewState("SavingStatus") = "new" And ViewState("SavingStatus2") = Nothing Then

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompanies.Visible = True
            tblGridview.Visible = True
            tblCompanyInput.Visible = False

            txtSearchInput.Text = ""
            FillGridView()
        ElseIf ViewState("SavingStatus") = "delete" Then

            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompanies.Visible = True
            tblGridview.Visible = True
            tblCompanyInput.Visible = False

            DeleteRecord()

            txtDescription.Text = ""

            txtSearchInput.Text = ""
            FillGridView()
        ElseIf ViewState("SavingStatus") = "edit" And ViewState("SavingStatus2") = Nothing Then


            btnCancel.Visible = False
            btnSave.Visible = False
            btnDelete.Visible = False
            cmdNew.Visible = True

            tblSearchCompanies.Visible = True
            tblGridview.Visible = True
            tblCompanyInput.Visible = False

            FillGridView()
        ElseIf ViewState("SavingStatus2") = "duplicate" Then

        End If
        GC.Collect()
    End Sub



    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        ViewState("SavingStatus") = "new"

        tblSearchCompanies.Visible = False
        tblGridview.Visible = False
        tblCompanyInput.Visible = True

        txtDescription.Text = ""

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
        tblGridview.Visible = True
        tblCompanyInput.Visible = False

        btnCancel.Visible = False
        btnSave.Visible = False
        btnDelete.Visible = False
        cmdNew.Visible = True
        blistErrorMsg.Items.Clear()
    End Sub

    Protected Sub gvBranches_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvBranches.PageIndexChanging
        Dim dt As New DataTable
        dt = CType(Session("dsWallet"), DataTable)
        gvBranches.PageIndex = e.NewPageIndex
        gvBranches.DataSource = dt
        gvBranches.DataBind()
        Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        blistErrorMsg.Items.Clear()

        If txtDescription.Text = "" Then
            blistErrorMsg.Items.Add("eWallet Partner is required.")
        End If

        If blistErrorMsg.Items.Count > 0 Then Exit Sub


        lblPopTitle.Value = "Warning"
        clsSession.Message = "Are you sure you want to update this eWalletPartner record?"
        clsSession.Icon = "inquiry"
        ViewState("process") = "approve_saving"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Protected Sub cmdRedirect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRedirect.Click
        Dim result As Integer
        If clsSession.DeleteStatus = "yes" Then
            Select Case ViewState("process").ToString()


                Case "approve_saving"
                    If ViewState("SavingStatus") = "edit" Then
                        result = SaveData()
                        If result <> -1 Then
                            ViewState("SavingStatus2") = Nothing
                            lblPopTitle.Value = "Update Successful"
                            clsSession.Message = "EWallet Partner record was successfully updated."
                            clsSession.Icon = "success"
                        Else
                            ViewState("SavingStatus2") = "duplicate"
                            lblPopTitle.Value = "Update Failed"
                            clsSession.Message = "Duplicate record. Updating record failed."
                            clsSession.Icon = "error"
                        End If
                    ElseIf ViewState("SavingStatus") = "new" Then
                        result = SaveData()
                        If result <> -1 Then
                            lblPopTitle.Value = "Save Successful"
                            clsSession.Message = "EWallet Partner record was successfully saved."
                            clsSession.Icon = "success"
                            ViewState("SavingStatus2") = Nothing
                        Else
                            ViewState("SavingStatus2") = "duplicate"
                            clsSession.Icon = "error"
                            lblPopTitle.Value = "Save Failed"
                            clsSession.Message = "Duplicate record. Saving record failed."

                        End If
                    End If


                Case "approve_deletion"
                    DeleteRecord()
                    ' Exit Sub
                Case Else

                    ' do nothing -- INVALID PROCESS ENTRY

            End Select
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

        End If

        RefreshPage()
    End Sub



#End Region


End Class
