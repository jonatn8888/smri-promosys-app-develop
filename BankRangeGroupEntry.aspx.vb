#Region " Imports "
Imports System.Data.SqlClient
Imports System.Data
#End Region

Partial Class BankRangeGroupEntry
    Inherits System.Web.UI.Page
    Dim strQuery As String


#Region " Subs and Functions "
    Private Sub FillIssuer()

        Dim dtTable As New DataTable

        strQuery = "SELECT * FROM ResListValues WHERE GroupName = 'BankNames' ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboIssuer.DataSource = dtTable
        cboIssuer.DataBind()
    End Sub

    Private Sub FillCardBrand()

        Dim dtTable As New DataTable

        strQuery = "SELECT * FROM ResListValues WHERE GroupName = 'CardBrand' ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboBrand.DataSource = dtTable
        cboBrand.DataBind()
    End Sub

    Private Sub FillCardType()

        Dim dtTable As New DataTable

        strQuery = "SELECT * FROM ResListValues WHERE GroupName = 'CardType' ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboType.DataSource = dtTable
        cboType.DataBind()
    End Sub

    Private Sub ComboUpdate(ByVal cbo As DropDownList, ByVal slotNum As String)
        Dim dtTable As New DataTable

        strQuery = "EXEC USP_LoyaltyGrpGetByEnvBankBin " & Request.QueryString("env") & ", " & Request.QueryString("id") & ", 2, " & slotNum

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        If dtTable.Rows.Count > 0 Then
            cbo.SelectedValue = Convert.ToInt64(dtTable.Rows(0).Item("LoyaltyGroupId"))
            If cbo.Attributes.Item("Tag") <> Nothing Then
                cbo.Attributes.Remove("Tag")
            End If
            cbo.Attributes.Add("Tag", dtTable.Rows(0).Item("LoyaltyGroupId").ToString())
        Else
            cbo.SelectedValue = "-1"
        End If
    End Sub

    Public Sub ComboUpdateBySelection(ByVal cbo As DropDownList, ByVal e As System.EventArgs)
        'Dim sErr1 As String = ""

        'If cbo.SelectedValue.ToString() = "-1" Then
        '    Dim LoyaltyGroupID As String = cbo.ID.Replace("cboLoyalGroup", "")
        '    strQuery = "DELETE LoyaltyGroupBin WHERE EnvCode = " & Request.QueryString("env") & " AND BankBin = " & Request.QueryString("id") & " AND LoyaltyGroupID = " & LoyaltyGroupID
        '    clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErr1)
        'Else
        '    strQuery = "INSERT INTO LoyaltyGroupBin SELECT " & Request.QueryString("env") & ", " & Request.QueryString("id") & ", 0, " & cbo.SelectedValue.ToString()
        '    clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErr1)
        'End If

        'ComboUpdate(cboLoyalGroup1, 1)
        'ComboUpdate(cboLoyalGroup2, 2)
        'ComboUpdate(cboLoyalGroup3, 3)
        'ComboUpdate(cboLoyalGroup4, 4)
        'ComboUpdate(cboLoyalGroup5, 5)
        'ComboUpdate(cboLoyalGroup6, 6)
        'ComboUpdate(cboLoyalGroup7, 7)
        'ComboUpdate(cboLoyalGroup8, 8)
        'ComboUpdate(cboLoyalGroup9, 9)
        'ComboUpdate(cboLoyalGroup10, 10)
    End Sub

    Private Sub FillLoyaltyGroups()
        Dim dtTable As New DataTable

        ' loyalty group dropdowns
        strQuery = "EXEC USP_LoyaltyGrpGetByEnvBankBin " & Request.QueryString("env") & ", " & Request.QueryString("id") & ", 0, 0"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboLoyalGroup1.DataSource = dtTable
        cboLoyalGroup1.DataBind()
        cboLoyalGroup2.DataSource = dtTable
        cboLoyalGroup2.DataBind()
        cboLoyalGroup3.DataSource = dtTable
        cboLoyalGroup3.DataBind()
        cboLoyalGroup4.DataSource = dtTable
        cboLoyalGroup4.DataBind()
        cboLoyalGroup5.DataSource = dtTable
        cboLoyalGroup5.DataBind()
        cboLoyalGroup6.DataSource = dtTable
        cboLoyalGroup6.DataBind()
        cboLoyalGroup7.DataSource = dtTable
        cboLoyalGroup7.DataBind()
        cboLoyalGroup8.DataSource = dtTable
        cboLoyalGroup8.DataBind()
        cboLoyalGroup9.DataSource = dtTable
        cboLoyalGroup9.DataBind()
        cboLoyalGroup10.DataSource = dtTable
        cboLoyalGroup10.DataBind()
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FillIssuer()
            FillCardBrand()
            FillCardType()
            FillLoyaltyGroups()
        Else
            Dim ctrlID As String = ""
            ctrlID = lastCtrl.Value

            If ctrlID = "btnSaveInfo" Then
                Dim sChkVal As String = "0"
                If chkActive.Checked = True Then
                    sChkVal = "1"
                End If

                Dim sError1 As String
                sError1 = ""
                strQuery = "UPDATE BankBin SET Issuer_ResourceID = " & cboIssuer.SelectedValue.ToString() & ", CardBrand_ResourceID = " & cboBrand.SelectedValue.ToString() & ", CardType_ResourceID = " & cboType.SelectedValue.ToString() & ", IsActive = " & sChkVal & " WHERE EnvCode = " & Request.QueryString("env") & " AND BankBin = " & Request.QueryString("id")
                clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sError1)
            End If

            If Request.Form("__EVENTTARGET") = Nothing Then
                ctrlID = ""
            Else
                ctrlID = Request.Form("__EVENTTARGET").ToString()
            End If

            If ctrlID.Contains("cboLoyalGroup") Then
                Dim sErr1 As String = ""
                Dim sCtrl1 As String() = ctrlID.Split("$")
                Dim sCtrl As String = sCtrl1(sCtrl1.Length - 1)
                Dim oDrop As DropDownList
                oDrop = Table1.FindControl(sCtrl)

                If oDrop.SelectedValue.ToString() = "-1" Then
                    Dim LoyaltyGroupID As String = "0"

                    If oDrop.Attributes.Item("Tag") <> Nothing Then
                        LoyaltyGroupID = oDrop.Attributes.Item("Tag")
                    End If

                    strQuery = "DELETE LoyaltyGroupBin WHERE EnvCode = " & Request.QueryString("env") & " AND BankBin = " & Request.QueryString("id") & " AND LoyaltyGroupID = " & LoyaltyGroupID
                    clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErr1)
                Else
                    strQuery = "INSERT INTO LoyaltyGroupBin SELECT " & Request.QueryString("env") & ", " & Request.QueryString("id") & ", 0, " & oDrop.SelectedValue.ToString()
                    clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErr1)
                End If

                oDrop = Nothing
            End If

        End If

        Dim dtTable As New DataTable

        ' loyalty group
        strQuery = "SELECT * FROM BankBin WHERE EnvCode = " & Request.QueryString("env") & " AND BankBin = '" & Request.QueryString("id") & "'"
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        If dtTable.Rows.Count > 0 Then
            txtBinRange.Text = Request.QueryString("id")
            cboIssuer.SelectedValue = Convert.ToInt64(dtTable.Rows(0).Item("Issuer_ResourceID"))
            cboBrand.SelectedValue = Convert.ToInt64(dtTable.Rows(0).Item("CardBrand_ResourceID"))
            cboType.SelectedValue = Convert.ToInt64(dtTable.Rows(0).Item("CardType_ResourceID"))
            chkActive.Checked = Convert.ToBoolean(dtTable.Rows(0).Item("IsActive"))
        End If

        ' environment
        strQuery = "SELECT * FROM Environments WHERE EnvCode = " & Request.QueryString("env")
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        If dtTable.Rows.Count > 0 Then
            txtCompany.Text = dtTable.Rows(0).Item("EnvName").ToString()
        End If

        ' loyalty group 1
        strQuery = "SELECT * FROM Environments WHERE EnvCode = " & Request.QueryString("env")
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        If dtTable.Rows.Count > 0 Then
            txtCompany.Text = dtTable.Rows(0).Item("EnvName").ToString()
        End If

        ComboUpdate(cboLoyalGroup1, 1)
        ComboUpdate(cboLoyalGroup2, 2)
        ComboUpdate(cboLoyalGroup3, 3)
        ComboUpdate(cboLoyalGroup4, 4)
        ComboUpdate(cboLoyalGroup5, 5)
        ComboUpdate(cboLoyalGroup6, 6)
        ComboUpdate(cboLoyalGroup7, 7)
        ComboUpdate(cboLoyalGroup8, 8)
        ComboUpdate(cboLoyalGroup9, 9)
        ComboUpdate(cboLoyalGroup10, 10)
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

#End Region
End Class
