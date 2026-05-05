#Region " Imports "
Imports System.Data.SqlClient
Imports System.Data
#End Region

Partial Class BinRangeGroupEntry
    Inherits System.Web.UI.Page

    Dim dtTable As New DataTable
    Dim strQuery As String



#Region " Subs and Functions "
    Private Sub FillBankBin()
        ' bin dropdown
        strQuery = "EXEC USP_BankBinGetByEnvLoyaltyGrp " & Request.QueryString("env") & ", " & Request.QueryString("id") & ", 1, ''"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboBankBin.DataSource = dtTable
        cboBankBin.DataBind()
    End Sub

    Private Sub FillGridView()
        ' gridview
        sqlds.SelectParameters("EnvCode").DefaultValue = CInt(Request.QueryString("env"))
        sqlds.SelectParameters("LoyaltyGroupId").DefaultValue = CInt(Request.QueryString("id"))
        sqlds.SelectParameters("ViewType").DefaultValue = 2
        sqlds.SelectParameters("Search").DefaultValue = " "
        Dim dv As DataView = CType(sqlds.Select(DataSourceSelectArguments.Empty), DataView)
        GridView1.DataBind()

        lblRecordCount.Text = "Total no. of records: " & dv.Table.Rows.Count
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FillBankBin()
            FillGridView()
        Else
            Dim ctrlID As String = ""
            ctrlID = lastCtrl.Value

            If ctrlID = "btnSaveDesc" Then
                Dim sError1 As String
                sError1 = ""
                strQuery = "UPDATE LoyaltyGroup SET Description = '" & txtDescription.Text & "' WHERE EnvCode = " & Request.QueryString("env") & " AND LoyaltyGroupId = " & Request.QueryString("id")
                clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sError1)

            ElseIf ctrlID = "btnRemove" Then
                Dim hasChkd As Boolean
                hasChkd = False

                For Each row As GridViewRow In GridView1.Rows
                    Dim lnkChk As CheckBox
                    Dim lnkBankBin As Label

                    lnkChk = row.FindControl("lnkChk")
                    lnkBankBin = row.FindControl("lnkBankBin")

                    If lnkChk.Checked = True Then
                        hasChkd = True
                        Dim sError1 As String
                        sError1 = ""
                        strQuery = "DELETE LoyaltyGroupBin WHERE EnvCode = " & Request.QueryString("env") & " AND LoyaltyGroupId = " & Request.QueryString("id") & " AND BankBin = " & lnkBankBin.Text
                        clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sError1)
                    End If
                Next

                If hasChkd = True Then
                    FillGridView()
                End If

            End If

        End If

        lastCtrl.Value = ""

        ' loyalty group
        strQuery = "SELECT * FROM LoyaltyGroup WHERE EnvCode = " & Request.QueryString("env") & " AND LoyaltyGroupId = " & Request.QueryString("id")
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        If dtTable.Rows.Count > 0 Then
            txtLoyaltyGroupId.Text = dtTable.Rows(0).Item("LoyaltyGroupId").ToString()
            txtDescription.Text = dtTable.Rows(0).Item("Description").ToString()
        End If

        ' environment
        strQuery = "SELECT * FROM Environments WHERE EnvCode = " & Request.QueryString("env")
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        If dtTable.Rows.Count > 0 Then
            txtCompany.Text = dtTable.Rows(0).Item("EnvName").ToString()
        End If
    End Sub

    'Protected Sub gvBranches_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBranches.RowCommand

    '    If e.CommandName = "select" Then
    '        ViewState("SavingStatus") = "edit"
    '        Dim i As Integer = 0
    '        Dim rowView As DataRowView
    '        Dim row As DataRow
    '        Dim dt As New DataTable
    '        dt = CType(Session("dsCompBranches"), DataTable)

    '        For Each rowView In dt.DefaultView
    '            row = rowView.Row
    '            If row("RowID") = e.CommandArgument Then

    '                lblCompBranch.Text = Right("000" + dt.Rows(i)("CompCode").ToString, 3) _
    '                                    + "-" + Right("0000" + dt.Rows(i)("BranchCode").ToString, 4)

    '                Me.ddlCompCode.SelectedValue = dt.Rows(i)("CompCode").ToString
    '                Me.ddlBrancode.SelectedValue = dt.Rows(i)("BranchCode").ToString
    '                Me.txtOtherInfo.Text = dt.Rows(i)("OtherInfo").ToString
    '                Me.chkDiscFile.Checked = Not CBool(dt.Rows(i)("NoDiscFile").ToString)
    '                Me.chkIsVisual.Checked = dt.Rows(i)("IsVisualStore").ToString

    '                hfRowID.Value = dt.Rows(i)("RowID").ToString

    '                tblSearchCompBranch.Visible = False
    '                tblGridviewCompBranch.Visible = False
    '                tblCompBranchInput.Visible = True


    '                btnCancel.Visible = True
    '                btnSave.Visible = True
    '                btnDelete.Visible = True
    '                cmdNew.Visible = False

    '                Exit For
    '            End If
    '            i = i + 1
    '        Next
    '    End If
    'End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

    End Sub

    Protected Sub btnAddBin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBin.Click
        If cboBankBin.Items.Count > 0 Then
            If cboBankBin.SelectedValue <> 0 Then
                Dim sError1 As String
                sError1 = ""
                strQuery = "INSERT INTO LoyaltyGroupBin SELECT " & Request.QueryString("env") & ", " & cboBankBin.SelectedValue.ToString() & ", 0, " & Request.QueryString("id")
                clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sError1)

                FillBankBin()
                FillGridView()
            End If
        End If
    End Sub

    Protected Sub btnSaveDesc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDesc.Click
        
    End Sub

#End Region
End Class
