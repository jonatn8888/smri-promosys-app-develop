
Imports System.Data

Partial Class PromoTypesEntry
    Inherits System.Web.UI.Page

    'Dim taPromoTypeAdapter As New dsPromotionsTableAdapters.PromoTypesTableAdapter()
    'Dim dtblPromoType As dsPromotions.PromoTypesDataTable
    'Dim trowPromoType As dsPromotions.PromoTypesRow

    Dim drPromoType As DataRow
    Dim strSQLcmd As String
    Dim ThisPageTitle As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")

        blistErrorMsg.Items.Clear()
        If Not IsPostBack Then
            cmdSave.Attributes.Add("onclick", "return validate()")
            ViewState("PromoTypeID") = Request("PromoTypeID")
            InitializeInputControls()
            FillTypeCategoryList()

            If CInt(ViewState("PromoTypeID")) = 0 Then
                ThisPageTitle = "Promo Type Maintenance - [New Entry]"
            Else

                ' fill controls with data
                LoadRowDataToControls(ViewState("PromoTypeID"))

                ' if invalid PromoType ID
                'Response.Redirect("InvalidAccess.aspx")
            End If

            ShowPromoTypeRegular()
            ShowPromoTypeSBU()
            Me.Title = ThisPageTitle
        End If
    End Sub


    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        ' validate entries
        blistErrorMsg.Items.Clear()

        If txtPromoType.Text = "" Then
            blistErrorMsg.Items.Add("Description of promo type must not be blank.")
        End If

        If txtShortDesc.Text = "" Then
            blistErrorMsg.Items.Add("Short description must not be blank.")
        End If

        ' Added dowcarpio03072013@smretailinc: additional field for Group Type/IsExclusive
        If cmbGroupType.SelectedIndex <= 0 Then
            blistErrorMsg.Items.Add("Please select group type.")
        End If

        'If txtPromoIDfrom.Text = "" Then
        '    blistErrorMsg.Items.Add("POS Promo ID not specified.")
        'End If

        'If cboXML_Priority.SelectedIndex = -1 Then
        '    blistErrorMsg.Items.Add("Promotion priority not specified.")
        'End If

        'If cboXML_ProcessType.SelectedIndex = -1 Then
        '    blistErrorMsg.Items.Add("Promotion process type not specified.")
        'End If

        'If Not (chkXML_BarcodeLevel.Checked Or chkXML_DeptLevel.Checked Or chkXML_CouponLevel.Checked) Then
        '    blistErrorMsg.Items.Add("There should be at least one item level selected.")
        'End If

        If CategoryExist(txtTypeCategory.Text, "") Then
            blistErrorMsg.Items.Add("Promo Category already exist. Please choose from the list.")
        End If

        If CategoryExist("", txtTypeSubCategory.Text) Then
            blistErrorMsg.Items.Add("Promo Type already exist. Please choose from the list.")
        End If

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        ' check if new entry

        Dim userId = SystemUser.UserID
        If CInt(Request("xMode")) = 0 And CInt(ViewState("PromoTypeID")) = 0 Then

            ' create new row with initial values
            strSQLcmd = "INSERT INTO PromoTypes (TypeDesc, LayoutID, ProcessType, CreatedBy, CreatedDate, " & _
                        "NumLeadDaysApprovalBU, ApprovalCutoffBU, NumLeadDaysApprovalMPD, ApprovalCutoffMPD,NumLeadDays) " & _
                        "VALUES ('" & txtPromoType.Text & "', 200, 'GenericHostXML'," & userId & ",GETDATE(), " & _
                        txtNumLeadDaysApprovalBU.Text & ", '" & cboApprovalCutoffBU.SelectedValue & "', " & _
                        txtNumLeadDaysApprovalMPD.Text & ", '" & cboApprovalCutoffMPD.SelectedValue & "', '" & txtNumLeadDaysRequestor.Text & "'); " & _
                        "SELECT CAST(scope_identity() AS bigint);"

            ViewState("PromoTypeID") = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strSQLcmd)
            If ViewState("PromoTypeID") = 0 Then
                blistErrorMsg.Items.Add("Unable to create record for the new promotion type.")
                Exit Sub
            End If
        End If

        ' update fields for GenericHostXML types]
        '        "TypeCategory = '" & IIf(cboPromoCategory.Enabled = True, cboPromoCategory.Text, txtTypeCategory.Text) & "', " & _
        '"TypeSubCategory = '" & IIf(cboPromoSubCategory.Enabled = True, cboPromoSubCategory.Text, txtTypeSubCategory.Text) & "', " & _

        ' Validation of Comp Sponsorship -- Added 2025-06-17 2SAM due to value of CompSponsorship to be mapped in ResListValues
        Dim compSponsorshipValue As Integer = 0
        Select Case cmbGroupType.SelectedValue
            Case "SBU"
                compSponsorshipValue = 10
        End Select

        strSQLcmd = "UPDATE PromoTypes SET " & _
                        "TypeDesc = '" & txtPromoType.Text & "', " & _
                        "ShortDesc = '" & txtShortDesc.Text & "', " & _
                        "ForMPDuseOnly = " & IIf(chkMpdUseOnly.Checked, 1, 0) & ", " & _
                        "AllowAttachment = " & IIf(chkAllow.Checked, 1, 0) & ", " & _
                        "RequireAttachment = " & IIf(chkRequired.Checked, 1, 0) & ", " & _
                        "GroupType = '" & cmbGroupType.SelectedValue & "', " & _
                        "CompSponsorship_Value = '" & compSponsorshipValue & "', " & _
                        "POS_Priority = " & ValidateDropDownField(10, cboXML_Priority.SelectedValue) & ", " & _
                        "POS_Priority_SBU = " & ValidateDropDownField(10, cboXML_Priority_SBU.SelectedValue) & ", " & _
                        "ModifiedBy = " & userId & ", " & _
                        "NumLeadDaysApprovalBU = " & txtNumLeadDaysApprovalBU.Text & ", " & _
                        "ApprovalCutoffBU = '" & cboApprovalCutoffBU.SelectedValue & "', " & _
                        "NumLeadDaysApprovalMPD = " & IIf(Trim(txtNumLeadDaysApprovalMPD.Text) = "0" Or Trim(txtNumLeadDaysApprovalMPD.Text) = "", "NULL ", txtNumLeadDaysApprovalMPD.Text) & ", " & _
                        "ApprovalCutoffMPD = " & IIf(Trim(txtNumLeadDaysApprovalMPD.Text) = "0" Or Trim(txtNumLeadDaysApprovalMPD.Text) = "", "NULL ", "'" & cboApprovalCutoffMPD.SelectedValue & "'") & ", " & _
                        "NumLeadDays = " & IIf(Trim(txtNumLeadDaysRequestor.Text) = "0" Or Trim(txtNumLeadDaysRequestor.Text) = "", "NULL ", txtNumLeadDaysRequestor.Text) & ", " & _
                        "requestorCutoff = '" & cboRequestorCutoff.SelectedValue & "', " & _
                        "ModifiedDate = GETDATE()," & _
                        "WorkFlowCode =  " & IIf(Trim(txtNumLeadDaysApprovalMPD.Text) = "0", "0", "CASE WHEN isMPDEscalate <> 0 Then 10 ELSE 40 END ") & "   " & _
                        "WHERE PromoTypeID = 0" & ViewState("PromoTypeID")
        Dim sErrMess As String = ""

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQLcmd, sErrMess) Then
            blistErrorMsg.Items.Add("Error encountered while saving data:" & sErrMess)
            Exit Sub
        End If
        Response.Redirect("PromoTypesList.aspx")
    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

        Response.Redirect("PromoTypesList.aspx")

    End Sub

    Private Function ValidateInputField(ByVal ColumnState As Integer, ByRef InputValue As String) As String
        Dim value As String = ""
        Select Case ColumnState
            Case 0
                value = "''"
            Case 10, 20, 30
                value = "'" & InputValue & "'"
        End Select

        ValidateInputField = value
    End Function

    Private Function CategoryExist(ByRef typeCat As String, ByRef typeSubCat As String) As Boolean
        Dim strSQL As String = ""
        Dim dtTable As New DataTable

        If typeCat <> "" Then
            strSQL = "SELECT DISTINCT(TypeCategory) " & _
            "FROM PromoTypes WHERE TypeCategory IS NOT NULL AND TypeCategory = '" & typeCat & "'"
        ElseIf typeSubCat <> "" Then
            strSQL = "SELECT DISTINCT(TypeSubCategory) " & _
                        "FROM PromoTypes WHERE TypeSubCategory is not NULL AND TypeSubCategory = '" & typeSubCat & "'"
        End If

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strSQL, dtTable)

        If dtTable.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Function ValidateDropDownField(ByVal ColumnState As Integer, ByRef Index As Integer, Optional ByRef name As String = "") As Integer
        Dim value As Integer = -1

        If (ColumnState > 0) Then
            value = Index
        End If

        ValidateDropDownField = value

    End Function
    Private Sub LoadRowDataToControls(ByVal nPromoTypeID As Long)

        strSQLcmd = "SELECT TypeDesc, " & _
            " ShortDesc, " & _
            " GroupType, " & _
            " ForMPDuseOnly, " & _
            " AllowAttachment, " & _
            " RequireAttachment, " & _
            " POS_Priority, " & _
            " POS_Priority_SBU, " & _
            " POS_Priority_STATE, " & _
            " POS_Priority_SBU_STATE, " & _
            " TypeCategory, " & _
            " TypeSubCategory, NumLeadDaysApprovalBU, ApprovalCutoffBU, NumLeadDaysApprovalMPD, ApprovalCutoffMPD, " & _
            " NumLeadDays, requestorCutoff " & _
            " FROM PromoTypes " & _
            " WHERE PromoTypeID = 0" & nPromoTypeID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromoType) Then

            ThisPageTitle = "Promo Type - " & drPromoType("TypeDesc") & " [Edit]"

            'txtTypeCategory.Text = drPromoType("TypeCategory").ToString
            'txtTypeSubCategory.Text = drPromoType("TypeSubCategory").ToString
            txtPromoType.Text = drPromoType("TypeDesc").ToString
            txtShortDesc.Text = drPromoType("ShortDesc").ToString

            ' Added dowcarpio03072013@smretailinc: additional field for Group Type/IsExclusive
            cmbGroupType.SelectedValue = drPromoType("GroupType").ToString
            'chkIsExclusive.Checked = IIf(drPromoType("IsExclusive") = "1", True, False)

            chkMpdUseOnly.Checked = CBool(drPromoType("ForMPDuseOnly"))
            chkAllow.Checked = CBool(drPromoType("AllowAttachment"))
            chkRequired.Checked = CBool(drPromoType("RequireAttachment"))

            'cboVSProcessType.SelectedValue = drPromoType("ProcessType")

            'txtPromoIDfrom.Text = drPromoType("POS_PromoIDfrom").ToString
            'txtPromoIDto.Text = drPromoType("POS_PromoIDto").ToString

            cboXML_Priority.SelectedValue = drPromoType("POS_Priority").ToString
            cboXML_Priority_SBU.SelectedValue = drPromoType("POS_Priority_SBU").ToString

            'cboXML_ActDeacTime_State.SelectedValue = drPromoType("POS_Deactime_STATE")

            'cboXML_ProcessType.SelectedValue = drPromoType("POS_ProcType").ToString
            'cboXML_ProcessType_State.SelectedValue = drPromoType("POS_ProcType_STATE")

            FillTypeCategoryList()
            cboPromoCategory.SelectedValue = drPromoType("TypeCategory")
            FillPromoSubCategoryList()
            cboPromoSubCategory.SelectedValue = drPromoType("TypeSubCategory")

            If drPromoType("NumLeadDaysApprovalBU") Is Nothing Then
                txtNumLeadDaysApprovalBU.Text = "0"
            Else
                If drPromoType("NumLeadDaysApprovalBU").ToString() = "" Then
                    txtNumLeadDaysApprovalBU.Text = "0"
                Else
                    txtNumLeadDaysApprovalBU.Text = drPromoType("NumLeadDaysApprovalBU")
                End If
            End If

            If drPromoType("ApprovalCutoffBU") Is Nothing Then
                cboApprovalCutoffBU.SelectedValue = "00:00"
            Else
                If drPromoType("ApprovalCutoffBU").ToString() = "" Then
                    cboApprovalCutoffBU.SelectedValue = "00:00"
                Else
                    cboApprovalCutoffBU.SelectedValue = drPromoType("ApprovalCutoffBU")
                End If
            End If

            If drPromoType("NumLeadDaysApprovalMPD") Is Nothing Then
                txtNumLeadDaysApprovalMPD.Text = "0"
            Else
                If drPromoType("NumLeadDaysApprovalMPD").ToString() = "" Then
                    txtNumLeadDaysApprovalMPD.Text = "0"
                Else
                    txtNumLeadDaysApprovalMPD.Text = drPromoType("NumLeadDaysApprovalMPD")
                End If
            End If

            If drPromoType("ApprovalCutoffMPD") Is Nothing Then
                cboApprovalCutoffMPD.SelectedValue = "00:00"
            Else
                If drPromoType("ApprovalCutoffMPD").ToString() = "" Then
                    cboApprovalCutoffMPD.SelectedValue = "00:00"
                Else
                    cboApprovalCutoffMPD.SelectedValue = drPromoType("ApprovalCutoffMPD")
                End If
            End If

            txtNumLeadDaysRequestor.Text = drPromoType("NumLeadDays").ToString()

            cboRequestorCutoff.SelectedValue = drPromoType("requestorCutoff").ToString()

        End If

    End Sub

    Private Sub InitializeInputControls()

        ' XML options
        'FillResDropDownList(cboXML_ProcessType, "PosProcessType")
        FillResDropDownList(cboXML_Priority, "PosPriority")
        FillResDropDownList(cboXML_Priority_SBU, "PosPriority")
        'FillResDropDownList(cboXML_ProcessType_State, "PromoTypeState")

        txtTypeCategory.Visible = False
        txtTypeSubCategory.Visible = False
        btnCancelAddTypeCat.Visible = False
        btnCancelAddTypeSub.Visible = False

        cboPromoCategory.Enabled = False
        cboPromoSubCategory.Enabled = False
        btnAddTypeCategory.Visible = False
        btnAddTypeSubCategory.Visible = False
        'trProcessType.Visible = False

        'If SystemUser.UserGroupType = "SBU" And ViewState("PromoTypeID") < 0 Then
        '    cmbGroupType.SelectedValue = "SBU"
        '    cmbGroupType.Enabled = False
        'End If

    End Sub

    Private Sub FillResDropDownList(ByRef cboListObj As DropDownList, ByVal sGroupName As String, Optional ByVal sSubGroup As String = "")

        Dim dtTable As New DataTable
        Dim strQuery As String

        If (sSubGroup <> "") Then

            strQuery = "SELECT ElementName, ElementValue " & _
                        "FROM ResListValues " & _
                        "WHERE GroupName = '" & sGroupName & "' " & _
                        "AND SubGroupName = '" & sSubGroup & "' " & _
                        "AND IsActive = 1 " & _
                        "ORDER BY SequenceNo"
        Else
            strQuery = "SELECT ElementName, ElementValue " & _
                        "FROM ResListValues " & _
                        "WHERE GroupName = '" & sGroupName & "' " & _
                        "AND IsActive = 1 " & _
                        "ORDER BY SequenceNo"
        End If

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboListObj.ClearSelection()
        cboListObj.Items.Clear()

        Dim lstItem As New ListItem

        lstItem.Value = -1
        lstItem.Text = "-- Select Value --"
        cboListObj.Items.Add(lstItem)

        cboListObj.DataSource = dtTable
        cboListObj.DataBind()

    End Sub

    Protected Sub FillTypeCategoryList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        'Dim strGroupType As String


        'If SystemUser.UserGroupType.ToString() = "CM" Then
        '    strGroupType = "REGULAR"
        'Else
        '    strGroupType = SystemUser.UserGroupType.ToString()
        'End If


        strQuery = "SELECT DISTINCT TypeCategory " & _
                    "FROM PromoTypes " & _
                    "WHERE TypeCategory IS NOT NULL " & _
                    "ORDER BY TypeCategory"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        ddItem.Text = "-- Select Value --"
        ddItem.Value = -1

        With cboPromoCategory
            .ClearSelection()
            .Items.Clear()
            .Items.Add(ddItem)
            .DataSource = dtTable
            .DataBind()
        End With

        cboPromoCategory.SelectedValue = -1
    End Sub

    Protected Sub FillPromoSubCategoryList()

        If (cboPromoCategory.SelectedValue <> "") Then
            Dim dtTable As New DataTable
            Dim strQuery As String = ""
            'Dim strGroupType As String

            'If SystemUser.UserGroupType.ToString() = "CM" Then
            '    strGroupType = "REGULAR"
            'Else
            '    strGroupType = SystemUser.UserGroupType.ToString()
            'End If


            strQuery = "SELECT DISTINCT TypeSubCategory " & _
                        "FROM PromoTypes " & _
                        "WHERE TypeSubCategory IS NOT NULL " & _
                        "AND TypeCategory = '" & cboPromoCategory.SelectedValue & "' " & _
                        "ORDER BY TypeSubCategory"

            clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

            With cboPromoSubCategory
                .ClearSelection()
                .Items.Clear()

                If dtTable.Rows.Count <> 1 Then
                    Dim ddItem As New ListItem

                    ddItem.Text = "-- Select Value --"
                    ddItem.Value = -1

                    .Items.Add(ddItem)
                End If

                .DataSource = dtTable
                .DataBind()
            End With

            If cboPromoSubCategory.Items.Count = 1 Then
                cboPromoSubCategory.SelectedIndex = 0
            Else
                cboPromoSubCategory.SelectedValue = -1
            End If
        End If


    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Select Case ViewState("process")
            Case "DeletePromoRule"

                'delete confirmation
                If clsSession.DeleteStatus = "yes" Then
                    Dim sRowList As String = ""
                End If

            Case "DeleteCondition"

                'delete confirmation
                If clsSession.DeleteStatus = "yes" Then
                    Dim sRowList As String = ""
                End If

        End Select

    End Sub

    Protected Sub cboPromoCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPromoCategory.SelectedIndexChanged
        FillPromoSubCategoryList()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub btnAddTypeCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTypeCategory.Click
        cboPromoCategory.Enabled = False
        txtTypeCategory.Visible = True
        txtTypeCategory.Text = ""
        btnCancelAddTypeCat.Visible = True

    End Sub

    Protected Sub btnAddTypeSubCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddTypeSubCategory.Click
        cboPromoSubCategory.Enabled = False
        txtTypeSubCategory.Visible = True
        txtTypeSubCategory.Text = ""
        btnCancelAddTypeSub.Visible = True
    End Sub

    Protected Sub btnCancelAddTypeCat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelAddTypeCat.Click
        cboPromoCategory.Enabled = True
        txtTypeCategory.Visible = False
        btnCancelAddTypeCat.Visible = False
    End Sub

    Protected Sub btnCancelAddTypeSub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelAddTypeSub.Click
        cboPromoSubCategory.Enabled = True
        txtTypeSubCategory.Visible = False
        btnCancelAddTypeSub.Visible = False
    End Sub

    Private Sub ShowPromoTypeRegular()
        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT RV.ElementName,case when RV.ElementValue = 0then 'Optimize' else 'Stand Alone' end ElementValue " & _
            "FROM ResListValues RV " & _
            "WHERE RV.GroupName = 'PosProcessType' AND RV.IsActive = 1 " & _
            "ORDER BY RV.SequenceNo"
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridProcessTypeMBU.DataSource = dtTable
        gridProcessTypeMBU.DataBind()


        trProcessTypeSBU.Visible = True
        If cmbGroupType.SelectedValue = "REGULAR" Or cmbGroupType.SelectedValue = "-1" Then
            trProcessTypeSBU.Visible = False
        End If

        If cmbGroupType.SelectedValue = "REGULAR" Then
            cboXML_Priority.Enabled = True
            cboXML_Priority_SBU.Enabled = False
        ElseIf cmbGroupType.SelectedValue = "SBU" Then
            cboXML_Priority.Enabled = False
            cboXML_Priority_SBU.Enabled = True
        Else
            cboXML_Priority.Enabled = True
            cboXML_Priority_SBU.Enabled = True
        End If

    End Sub

    Private Sub ShowPromoTypeSBU()
        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT RV.ElementName,case when RV.ElementValue = 0then 'Optimize' else 'Stand Alone' end ElementValue " & _
            "FROM ResListValues RV " & _
            "WHERE RV.GroupName = 'TplProcessType' AND RV.IsActive = 1 " & _
            "ORDER BY RV.SequenceNo"
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridProcessTypeSBU.DataSource = dtTable
        gridProcessTypeSBU.DataBind()

        trProcessTypeMBU.Visible = True
        If cmbGroupType.SelectedValue = "SBU" Or cmbGroupType.SelectedValue = "-1" Then
            trProcessTypeMBU.Visible = False
        End If
    End Sub

    Protected Sub cmbGroupType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGroupType.SelectedIndexChanged
        ShowPromoTypeRegular()
        ShowPromoTypeSBU()
    End Sub
End Class
