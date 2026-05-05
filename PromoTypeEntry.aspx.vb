
Imports System.Data

Partial Class PromoTypeEntry
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
            cmdDelete.Attributes.Add("onclick", "return confirm_delete()")

            ViewState("PromoTypeID") = Request("PromoTypeID")

            InitializeInputControls()
            FillTypeCategoryList()
            'InitializePromoRange(ViewState("PromoTypeID"))

            If CInt(ViewState("PromoTypeID")) = 0 Then

                ThisPageTitle = "Promo Type Maintenance - [New Entry]"
                cmdDelete.Visible = False

                trConditions.Visible = False
                trRules.Visible = False

            Else

                ' fill controls with data
                LoadRowDataToControls(ViewState("PromoTypeID"))
                InitializePromoRange(ViewState("PromoTypeID"))

                ' if invalid PromoType ID
                'Response.Redirect("InvalidAccess.aspx")
            End If

            Me.Title = ThisPageTitle

        Else
            If (cboXML_EligibleCards_State.SelectedValue <> 0) Then
                ViewState("SubGroupName") = "LoyaltyCardLevel"
            Else
                ViewState("SubGroupName") = ""
            End If

            If cboXML_DiscCondition.SelectedValue = -1 Then
                If (ViewState("SubGroupName") <> "") Then
                    FillResDropDownList(cboXML_DiscCondition, "PosDiscCondition", ViewState("SubGroupName"))
                Else
                    FillResDropDownList(cboXML_DiscCondition, "PosDiscCondition")
                End If
            End If
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

        If (litDefMechanics.Text = "" Or litDefMechanics.Text = "<p>&nbsp;</p>") And Not chkMpdUseOnly.Checked Then
            blistErrorMsg.Items.Add("Default Promo Mechanics must not be blank.")
        End If

        If (litDefGuidelines.Text = "" Or litDefGuidelines.Text = "<p>&nbsp;</p>") Then
            blistErrorMsg.Items.Add("Default Guideline(s) must not be blank.")
        End If

        ' Added dowcarpio03072013@smretailinc: additional field for Group Type/IsExclusive
        If cmbGroupType.SelectedIndex <= 0 Then
            blistErrorMsg.Items.Add("Please select group type.")
        End If

        'If txtPromoIDfrom.Text = "" Then
        '    blistErrorMsg.Items.Add("POS Promo ID not specified.")
        'End If

        If cboXML_Priority.SelectedIndex = -1 Then
            blistErrorMsg.Items.Add("Promotion priority not specified.")
        End If

        If cboXML_ProcessType.SelectedIndex = -1 Then
            blistErrorMsg.Items.Add("Promotion process type not specified.")
        End If

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

        If CInt(Request("xMode")) = 0 And CInt(ViewState("PromoTypeID")) = 0 Then

            ' create new row with initial values
            strSQLcmd = "INSERT INTO PromoTypes (TypeDesc, LayoutID, ProcessType) " & _
                        "VALUES ('" & txtPromoType.Text & "', 200, 'GenericHostXML'); " & _
                        "SELECT CAST(scope_identity() AS bigint);"

            ViewState("PromoTypeID") = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strSQLcmd)

            If ViewState("PromoTypeID") = 0 Then
                blistErrorMsg.Items.Add("Unable to create record for the new promotion type.")
                Exit Sub
            End If
        End If

        Dim sActiveDays As String

        sActiveDays = IIf(chkMonday.Checked, "M", "") & _
                        IIf(chkTuesday.Checked, "T", "") & _
                        IIf(chkWednesday.Checked, "W", "") & _
                        IIf(chkThursday.Checked, "H", "") & _
                        IIf(chkFriday.Checked, "F", "") & _
                        IIf(chkSaturday.Checked, "S", "") & _
                        IIf(chkSunday.Checked, "N", "")

        ' update fields for GenericHostXML types
        strSQLcmd = "UPDATE PromoTypes SET " & _
                        "TypeCategory = '" & IIf(cboPromoCategory.Enabled = True, cboPromoCategory.Text, txtTypeCategory.Text) & "', " & _
                        "TypeSubCategory = '" & IIf(cboPromoSubCategory.Enabled = True, cboPromoSubCategory.Text, txtTypeSubCategory.Text) & "', " & _
                        "TypeDesc = '" & txtPromoType.Text & "', " & _
                        "ShortDesc = '" & txtShortDesc.Text & "', " & _
                        "DefaultMechanics = '" & Server.HtmlEncode(litDefMechanics.Text) & "', " & _
                        "DefaultGuideline = '" & Server.HtmlEncode(litDefGuidelines.Text) & "', " & _
                        "ForMPDuseOnly = " & IIf(chkMpdUseOnly.Checked, 1, 0) & ", " & _
                        "ForEventParticipation = " & IIf(chkForSMACdeals.Checked, 1, 0) & ", " & _
                        "AllowAttachment = " & IIf(chkAllow.Checked, 1, 0) & ", " & _
                        "RequireAttachment = " & IIf(chkRequired.Checked, 1, 0) & ", " & _
                        "WithEventCode = 0, " & _
                        "IsMarkDown = 0, " & _
                        "IsClassDiscount = " & IIf(chkIsClassDiscount.Checked, 1, 0) & ", " & _
                        "GroupType = '" & cmbGroupType.SelectedValue & "', " & _
                        "IsExclusive = " & IIf(chkIsExclusive.Checked, 1, 0) & ", " & _
                        "IsUPCLevel = " & IIf(chkUPClevel.Checked, 1, 0) & ", " & _
                        "POS_PromoIDfrom = 0" & txtPromoIDfrom.Text & ", " & _
                        "POS_PromoIDto = 0" & txtPromoIDto.Text & ", " & _
                        "POS_ActionType = " & ValidateDropDownField(cboXML_ActionType_State.SelectedValue, cboXML_ActionType.SelectedValue) & ", " & _
                        "POS_ActionType_STATE = 0" & cboXML_ActionType_State.SelectedValue & ", " & _
                        "POS_CompressID = 0" & IIf(txtXML_CompressID.Text <> "", txtXML_CompressID.Text, "") & ", " & _
                        "POS_CompressID_STATE = 0" & cboXML_CompressID_State.SelectedValue & ", " & _
                        "POS_ExclType = " & ValidateDropDownField(cboXML_ExclusionType_State.SelectedValue, cboXML_ExclusionType.SelectedValue) & ", " & _
                        "POS_ExclType_STATE = 0" & cboXML_ExclusionType_State.SelectedValue & ", " & _
                        "POS_Priority = " & ValidateDropDownField(cboXML_Priority_State.SelectedValue, cboXML_Priority.SelectedValue) & ", " & _
                        "POS_Priority_STATE = 0" & cboXML_Priority_State.SelectedValue & ", " & _
                        "POS_Message_Flag = 0" & IIf(Val(txtXML_MessageID.Text) = 0 Or cboXML_MessageID_State.SelectedValue = 0, 0, 1) & ", " & _
                        "POS_Message_Flag_STATE = 0" & cboXML_MessageID_State.SelectedValue & ", " & _
                        "POS_Message_ID = 0" & txtXML_MessageID.Text & ", " & _
                        "POS_Message_ID_STATE = 0" & cboXML_MessageID_State.SelectedValue & ", " & _
                        "POS_Activedays = '" & sActiveDays & "', " & _
                        "POS_Activedays_STATE = 0" & cboXML_ActiveDays_State.SelectedValue & ", " & _
                        "POS_Actime = '" & cboXML_StartTime.Text & "', " & _
                        "POS_Actime_STATE = 0" & cboXML_ActDeacTime_State.SelectedValue & ", " & _
                        "POS_Deactime = '" & cboXML_EndTime.Text & "', " & _
                        "POS_Deactime_STATE = 0" & cboXML_ActDeacTime_State.SelectedValue & ", " & _
                        "POS_Descr1prm = " & ValidateInputField(cboXML_Descr1prm_State.SelectedValue, txtXML_Descr1prm.Text) & ", " & _
                        "POS_Descr1prm_STATE = 0" & cboXML_Descr1prm_State.SelectedValue & ", " & _
                        "POS_Descr2prm = " & ValidateInputField(cboXML_Descr2prm_State.SelectedValue, txtXML_Descr2prm.Text) & ", " & _
                        "POS_Descr2prm_STATE = 0" & cboXML_Descr2prm_State.SelectedValue & ", " & _
                        "POS_Infotext1 = " & ValidateInputField(cboXML_ReceiptDesc1_State.SelectedValue, txtXML_ReceiptDesc1.Text) & ", " & _
                        "POS_Infotext1_STATE = 0" & cboXML_ReceiptDesc1_State.SelectedValue & ", " & _
                        "POS_Infotext2 = " & ValidateInputField(cboXML_ReceiptDesc2_State.SelectedValue, txtXML_ReceiptDesc2.Text) & ", " & _
                        "POS_Infotext2_STATE = 0" & cboXML_ReceiptDesc2_State.SelectedValue & ", " & _
                        "POS_MaxAmnt = 0" & txtXML_MaxAmount.Text & ", " & _
                        "POS_MaxAmnt_STATE = 0" & cboXML_MaxAmount_State.SelectedValue & ", " & _
                        "POS_MaxQty = 0" & txtXML_MaxQty.Text & ", " & _
                        "POS_MaxQty_STATE = 0" & cboXML_MaxQty_State.SelectedValue & ", " & _
                        "POS_ProcType = " & ValidateDropDownField(cboXML_ProcessType_State.SelectedValue, cboXML_ProcessType.SelectedValue, "ProcType") & ", " & _
                        "POS_ProcType_STATE = 0" & cboXML_ProcessType_State.SelectedValue & ", " & _
                        "POS_DiscType = " & ValidateDropDownField(cboXML_DiscountType_State.SelectedValue, cboXML_DiscountType.SelectedValue) & ", " & _
                        "POS_DiscType_STATE = 0" & cboXML_DiscountType_State.SelectedValue & ", " & _
                        "POS_AllowDeptCodes = 0" & IIf(chkXML_DeptLevel.Checked, 1, 0) & ", " & _
                        "POS_AllowBarcodes = 0" & IIf(chkXML_BarcodeLevel.Checked, 1, 0) & ", " & _
                        "POS_AllowCoupons = 0" & IIf(chkXML_CouponLevel.Checked, 1, 0) & ", " & _
                        "POS_MaxCoupons = 0" & IIf(chkXML_CouponLevel.Checked, txtXML_MaxCoupons.Text, 0) & ", " & _
                        "POS_QualifiedCust = " & ValidateInputField(cboXML_QualifiedCust_State.SelectedValue, txtXML_QualifiedCust.Text) & ", " & _
                        "POS_QualifiedCust_STATE = 0" & cboXML_QualifiedCust_State.SelectedValue & ", " & _
                        "POS_NameOfPartners = " & ValidateInputField(cboXML_NameOfPartners_State.SelectedValue, txtXML_NameOfPartners.Text) & ", " & _
                        "POS_NameOfPartners_STATE = 0" & cboXML_NameOfPartners_State.SelectedValue & ", " & _
                        "POS_ProofOfMembership = " & ValidateInputField(cboXML_ProofOfMemb_State.SelectedValue, txtXML_ProofOfMemb.Text) & ", " & _
                        "POS_ProofOfMembership_STATE = 0" & cboXML_ProofOfMemb_State.SelectedValue & ", " & _
                        "POS_PartnerEstabGWP = " & ValidateInputField(cboXML_PartnerEstablishmentGWP_State.SelectedValue, txtXML_PartnerEstablishmentGWP.Text) & ", " & _
                        "POS_PartnerEstabGWP_STATE = 0" & cboXML_PartnerEstablishmentGWP_State.SelectedValue & ", " & _
                        "POS_QualifiedItems = " & ValidateDropDownField(cboXML_QualifiedItems_State.SelectedValue, cboXML_QualifiedItems.SelectedValue, "QualifiedItems") & ", " & _
                        "POS_QualifiedItems_STATE = 0" & cboXML_QualifiedItems_State.SelectedValue & ", " & _
                        "POS_EligibleCards_STATE = 0" & cboXML_EligibleCards_State.SelectedValue & ", " & _
                        "POS_PromoPremUPC_STATE = 0" & cboXLM_UPCPromoPremium.SelectedValue & ", " & _
                        "POS_SMACKitPrice = 0" & txtXML_SMACKitPrice.Text & ", " & _
                        "POS_SMACKitPrice_STATE = 0" & cboXML_SMACKitPrice_State.SelectedValue & ", " & _
                        "TPL_NumMonths = '" & txtTPL_NumMonths.Text & "', " & _
                        "TPL_NumMonths_STATE = 0" & cboTPL_NumMonths_State.SelectedValue & ", " & _
                        "TPL_BankList_STATE = 0" & cboTPL_BankList_State.SelectedValue & ", " & _
                        "TPL_BankList = " & ValidateInputField(cboTPL_BankList_State.SelectedValue, txtTPL_BankList.Text) & ", " & _
                        "TPL_BrandNames_STATE = 0" & cboTPL_BrandNames_State.SelectedValue & ", " & _
                        "TPL_BrandNames = " & ValidateInputField(cboTPL_BrandNames_State.SelectedValue, txtTPL_BrandNames.Text) & ", " & _
                        "TPL_ProcessType_STATE = 0" & cboTPL_ProcType_State.SelectedValue & ", " & _
                        "TPL_ProcessType = 0" & ValidateDropDownField(cboTPL_ProcType_State.SelectedValue, cboTPL_ProcType.SelectedValue) & ", " & _
                        "TPL_PurchaseReq_STATE = 0" & cboTPL_PurchReq_State.SelectedValue & ", " & _
                        "TPL_PurchaseReq = 0" & ValidateDropDownField(cboTPL_PurchReq_State.SelectedValue, cboTPL_PurchReq.SelectedValue) & ", " & _
                        "TPL_ReqAmount= 0" & txtTPL_RequiredAmt.Text & ", " & _
                        "TPL_ReqAmount_STATE = 0" & cboTPL_RequiredAmt_State.SelectedValue & ", " & _
                        "TPL_FreeItems_STATE = 0" & cboTPL_FreeItems_State.SelectedValue & ", " & _
                        "TPL_FreeItems = " & ValidateInputField(cboTPL_FreeItems_State.SelectedValue, txtTPL_FreeItems.Text) & ", " & _
                        "TPL_Prizes_STATE = 0" & cboTPL_Prizes_State.SelectedValue & ", " & _
                        "TPL_Prizes = " & ValidateInputField(cboTPL_Prizes_State.SelectedValue, txtTPL_Prizes.Text) & ", " & _
                        "TPL_DeptName = " & ValidateInputField(cboTPL_DepName_State.SelectedValue, txtTPL_DepName.Text) & ", " & _
                        "TPL_DeptName_STATE = 0" & cboTPL_DepName_State.SelectedValue & ", " & _
                        "TPL_SellingArea = " & ValidateInputField(cboTPL_SellingArea_State.SelectedValue, txtTPL_SellingArea.Text) & ", " & _
                        "TPL_SellingArea_STATE = 0" & cboTPL_SellingArea_State.SelectedValue & ", " & _
                        "TPL_ItemName = " & ValidateInputField(cboTPL_ItemName_State.SelectedValue, txtTPL_ItemName.Text) & ", " & _
                        "TPL_ItemName_STATE = 0" & cboTPL_ItemName_State.SelectedValue & ", " & _
                        "TPL_BonusPoints = '" & txtTPL_BonusPoints.Text & "', " & _
                        "TPL_BonusPoints_STATE = 0" & cboTPL_BonusPoints_State.SelectedValue & ", " & _
                        "TPL_CelebName = " & ValidateInputField(cboTPL_CelebName_State.SelectedValue, txtTPL_CelebName.Text) & ", " & _
                        "TPL_CelebName_STATE = 0" & cboTPL_CelebName_State.SelectedValue & ", " & _
                        "TPL_EventTime = " & ValidateInputField(cboTPL_EventTime_State.SelectedValue, txtTPL_EventTime.Text) & ", " & _
                        "TPL_EventTime_STATE = 0" & cboTPL_EventTime_State.SelectedValue & ", " & _
                        "TPL_BuyQty = 0" & txtTPL_BuyQty.Text & ", " & _
                        "TPL_BuyQty_STATE = 0" & cboTPL_BuyQty_State.SelectedValue & ", " & _
                        "TPL_TakeQty = 0" & txtTPL_TakeQty.Text & ", " & _
                        "TPL_TakeQty_STATE = 0" & cboTPL_TakeQty_State.SelectedValue & ", " & _
                        "TPL_ActivityName = " & ValidateInputField(cboTPL_ActivityName_State.SelectedValue, txtTPL_ActivityName.Text) & ", " & _
                        "TPL_ActivityName_STATE = 0" & cboTPL_ActivityName_State.SelectedValue & ", " & _
                        "TPL_PromoNotes_STATE = 0" & cboTPL_PromoNotes_State.SelectedValue & ", " & _
                        "TPL_DiscAmount_STATE = 0" & cboTPL_DiscountAmt_State.SelectedValue & ", " & _
                        "TPL_PercentDisc_STATE = 0" & cboTPL_PercentDisc_State.SelectedValue & ", " & _
                        "CompSponsorship_STATE = 0" & cboXML_Sponsorship_State.SelectedValue & ", " & _
                        "CompSponsorship_Value = " & ValidateDropDownField(cboXML_Sponsorship_State.SelectedValue, cboXML_Sponsorship.SelectedValue, "Compsponsorship") & "," & _
                        IIf(cboXML_EligibleCards_State.SelectedValue <> 0, _
                            "POS_EligibleCards = '" & EligibleCardsEncode() & "', ", _
                             "POS_EligibleCards = ''" & ", ") & _
                         "IsEligibleCardsRequired =" & IIf(chk_IsEligibleRequired.Checked, 1, 0) & " " & _
                        "WHERE PromoTypeID = 0" & ViewState("PromoTypeID")

        '"POS_RuleType = " & cboxml_rule & ", " & _
        '"POS_RuleType_STATE = 0" & x & ", " & _
        '"POS_RuleValueType = " & x & ", " & _
        '"POS_RuleValueType_STATE = 0" & x & ", " & _
        '"POS_RuleCondition = " & x & ", " & _
        '"POS_RuleCondition_STATE = 0" & x & ", " & _
        '"POS_RuleValue = " & x & ", " & _
        '"POS_RuleValue_STATE = 0" & cboxml_rul & ", " & _
        '"VSLP_InterfacePath = " & x & ", " & _

        '"DefCCLGuideline" & x & ", " & _
        '"DefADDGuideline" & x & ", " & _
        '"DefEXTGuideline" & x & ", " & _
        '"DefCRFGuideline" & x & ", " & _
        '"WithPCReq= " & x & ", " & _
        '"POS_ValidFrom = " & x & ", " & _
        '"POS_ValidTo = " & x & ", " & _

        Dim sErrMess As String = ""

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQLcmd, sErrMess) Then
            blistErrorMsg.Items.Add("Error encountered while saving data:" & sErrMess)
            Exit Sub
        End If

        Response.Redirect("PromoTypeList.aspx")

    End Sub

    Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

        Response.Redirect("PromoTypeList.aspx")

    End Sub

    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

        'TODO:: verify deletion (client side scripting)

        Dim sErrMsg As String = ""

        strSQLcmd = "DELETE FROM PromoTypes WHERE PromoTypeID = 0" & ViewState("PromoTypeID")

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQLcmd, sErrMsg) Then
            Response.Redirect("PromoTypeList.aspx")
        Else
            ' error in accessing data: sErrMsg
        End If

    End Sub

    Protected Sub lnkEditMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditMechanics.Click

        'lblPopTitle.Text = "Default Mechanics"
        ViewState("ActiveTextBox") = "Mechanics"
        'hidBox.Value = litDefMechanics.Text

        hidBox.Value = litDefMechanics.Text
        clsSession.Mechanics = Server.HtmlDecode(litDefMechanics.Text.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
        'panPopUp.Visible = True
    End Sub

    Protected Sub lnkEditGuidelines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditGuidelines.Click

        'lblPopTitle.Text = "Default Guidelines"
        ViewState("ActiveTextBox") = "Guidelines"
        'hidBox.Value = litDefGuidelines.Text
        hidBox.Value = litDefGuidelines.Text
        clsSession.Mechanics = Server.HtmlDecode(litDefGuidelines.Text.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
        'panPopUp.Visible = True

    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ViewState("ActiveTextBox") = "Mechanics" Then
            litDefMechanics.Text = hidBox.Value
        Else
            litDefGuidelines.Text = hidBox.Value
        End If
    End Sub

    ' Added dowcarpio03072013@smretailinc: additional field for Group Type/IsExclusive
    Protected Sub cmbGroupType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGroupType.SelectedIndexChanged

        If cmbGroupType.SelectedIndex > 0 Then

            chkIsExclusive.Enabled = True

        Else

            chkIsExclusive.Enabled = False

        End If
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

        strSQLcmd = "SELECT * FROM PromoTypes WHERE PromoTypeID = 0" & nPromoTypeID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromoType) Then

            ThisPageTitle = "Promo Type - " & drPromoType("TypeDesc") & " [Edit]"

            txtPromoType.Text = drPromoType("TypeDesc").ToString
            txtShortDesc.Text = drPromoType("ShortDesc").ToString
            litDefMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics").ToString)
            litDefGuidelines.Text = Server.HtmlDecode(drPromoType("DefaultGuideline").ToString)

            ' Added dowcarpio03072013@smretailinc: additional field for Group Type/IsExclusive
            cmbGroupType.SelectedValue = drPromoType("GroupType").ToString
            'chkIsExclusive.Checked = IIf(drPromoType("IsExclusive") = "1", True, False)
            chkIsExclusive.Checked = (drPromoType("IsExclusive").ToString = "1") ' noelbsantos: simplified above line

            chkMpdUseOnly.Checked = CBool(drPromoType("ForMPDuseOnly"))
            chkAllow.Checked = CBool(drPromoType("AllowAttachment"))
            chkRequired.Checked = CBool(drPromoType("RequireAttachment"))
            chkIsClassDiscount.Checked = CBool(drPromoType("IsClassDiscount"))
            chkForSMACdeals.Checked = CBool(drPromoType("ForEventParticipation"))
            chkUPClevel.Checked = CBool(drPromoType("IsUPClevel"))

            'cboVSProcessType.SelectedValue = drPromoType("ProcessType")

            'txtPromoIDfrom.Text = drPromoType("POS_PromoIDfrom").ToString
            'txtPromoIDto.Text = drPromoType("POS_PromoIDto").ToString

            cboXML_Priority.SelectedValue = drPromoType("POS_Priority").ToString
            cboXML_Priority_State.SelectedValue = drPromoType("POS_Priority_STATE")

            txtXML_CompressID.Text = drPromoType("POS_CompressID").ToString
            cboXML_CompressID_State.SelectedValue = drPromoType("POS_CompressID_STATE")

            chkMonday.Checked = (InStr(drPromoType("POS_Activedays").ToString, "M") > 0)
            chkTuesday.Checked = (InStr(drPromoType("POS_Activedays").ToString, "T") > 0)
            chkWednesday.Checked = (InStr(drPromoType("POS_Activedays").ToString, "W") > 0)
            chkThursday.Checked = (InStr(drPromoType("POS_Activedays").ToString, "H") > 0)
            chkFriday.Checked = (InStr(drPromoType("POS_Activedays").ToString, "F") > 0)
            chkSaturday.Checked = (InStr(drPromoType("POS_Activedays").ToString, "S") > 0)
            chkSunday.Checked = (InStr(drPromoType("POS_Activedays").ToString, "N") > 0)
            cboXML_ActiveDays_State.SelectedValue = drPromoType("POS_ActiveDays_STATE")

            'txtXML_AcTime.Text = drPromoType("POS_Actime").ToString
            'txtXML_Deactime.Text = drPromoType("POS_Deactime").ToString
            'MALAgasino 20181001 - Change activity time to dropdown selection
            FillStartTime()
            FillEndTime()
            cboXML_StartTime.Text = drPromoType("POS_Actime").ToString
            cboXML_EndTime.Text = drPromoType("POS_Deactime").ToString
            cboXML_ActDeacTime_State.SelectedValue = drPromoType("POS_Actime_STATE").ToString()
            'cboXML_ActDeacTime_State.SelectedValue = drPromoType("POS_Deactime_STATE")

            txtXML_Descr1prm.Text = drPromoType("POS_Descr1prm").ToString
            cboXML_Descr1prm_State.SelectedValue = drPromoType("POS_Descr1prm_STATE").ToString()
            txtXML_Descr2prm.Text = drPromoType("POS_Descr2prm").ToString
            cboXML_Descr2prm_State.SelectedValue = drPromoType("POS_Descr2prm_STATE").ToString()

            txtXML_ReceiptDesc1.Text = drPromoType("POS_Infotext1").ToString
            cboXML_ReceiptDesc1_State.SelectedValue = drPromoType("POS_Infotext1_STATE").ToString()
            txtXML_ReceiptDesc2.Text = drPromoType("POS_Infotext2").ToString
            cboXML_ReceiptDesc2_State.SelectedValue = drPromoType("POS_Infotext2_STATE").ToString()

            cboXML_ActionType.SelectedValue = drPromoType("POS_ActionType").ToString
            cboXML_ActionType_State.SelectedValue = drPromoType("POS_ActionType_STATE").ToString()
            cboXML_ExclusionType.SelectedValue = drPromoType("POS_ExclType").ToString
            cboXML_ExclusionType_State.SelectedValue = drPromoType("POS_ExclType_STATE").ToString()

            txtXML_MaxAmount.Text = drPromoType("POS_MaxAmnt").ToString
            cboXML_MaxAmount_State.SelectedValue = drPromoType("POS_MaxAmnt_STATE").ToString()

            txtXML_MaxQty.Text = drPromoType("POS_MaxQty").ToString
            cboXML_MaxQty_State.SelectedValue = drPromoType("POS_MaxQty_STATE").ToString()

            cboXML_ProcessType.SelectedValue = drPromoType("POS_ProcType").ToString
            cboXML_ProcessType_State.SelectedValue = drPromoType("POS_ProcType_STATE").ToString()

            cboXML_DiscountType.SelectedValue = drPromoType("POS_DiscType").ToString
            cboXML_DiscountType_State.SelectedValue = drPromoType("POS_DiscType_STATE").ToString()

            txtXML_MessageID.Text = drPromoType("POS_Message_ID").ToString
            cboXML_MessageID_State.SelectedValue = drPromoType("POS_Message_ID_STATE").ToString()

            cboXML_QualifiedItems.SelectedValue = drPromoType("POS_QualifiedItems").ToString()
            cboXML_QualifiedItems_State.SelectedValue = drPromoType("POS_QualifiedItems_STATE").ToString()

            chkXML_DeptLevel.Checked = (drPromoType("POS_AllowDeptCodes").ToString = "1")
            chkXML_BarcodeLevel.Checked = (drPromoType("POS_AllowBarcodes").ToString = "1")
            chkXML_CouponLevel.Checked = (drPromoType("POS_AllowCoupons").ToString = "1")
            txtXML_MaxCoupons.Text = drPromoType("POS_MaxCoupons").ToString

            txtXML_SMACKitPrice.Text = drPromoType("POS_SMACKitPrice").ToString
            cboXML_SMACKitPrice_State.SelectedValue = drPromoType("POS_SMACKitPrice_STATE").ToString()

            txtXML_QualifiedCust.Text = drPromoType("POS_QualifiedCust").ToString
            cboXML_QualifiedCust_State.SelectedValue = drPromoType("POS_QualifiedCust_STATE").ToString()
            txtXML_NameOfPartners.Text = drPromoType("POS_NameOfPartners").ToString
            cboXML_NameOfPartners_State.SelectedValue = drPromoType("POS_NameOfPartners_STATE").ToString()
            txtXML_ProofOfMemb.Text = drPromoType("POS_ProofOfMembership").ToString
            cboXML_ProofOfMemb_State.SelectedValue = drPromoType("POS_ProofOfMembership_STATE").ToString()
            txtXML_PartnerEstablishmentGWP.Text = drPromoType("POS_PartnerEstabGWP").ToString
            cboXML_PartnerEstablishmentGWP_State.SelectedValue = drPromoType("POS_PartnerEstabGWP_STATE").ToString()

            cboXLM_UPCPromoPremium.SelectedValue = drPromoType("POS_PromoPremUPC_STATE")

            txtTPL_NumMonths.Text = drPromoType("TPL_NumMonths").ToString
            cboTPL_NumMonths_State.SelectedValue = drPromoType("TPL_NumMonths_STATE").ToString().ToString()

            txtTPL_BankList.Text = drPromoType("TPL_BankList").ToString
            cboTPL_BankList_State.SelectedValue = drPromoType("TPL_BankList_STATE").ToString().ToString()

            txtTPL_BrandNames.Text = drPromoType("TPL_BrandNames").ToString
            cboTPL_BrandNames_State.SelectedValue = drPromoType("TPL_BrandNames_STATE").ToString().ToString()

            cboTPL_ProcType.SelectedValue = drPromoType("TPL_ProcessType")
            cboTPL_ProcType_State.SelectedValue = drPromoType("TPL_ProcessType_STATE").ToString().ToString()

            cboTPL_PurchReq.SelectedValue = drPromoType("TPL_PurchaseReq")
            cboTPL_PurchReq_State.SelectedValue = drPromoType("TPL_PurchaseReq_STATE").ToString().ToString()

            txtTPL_RequiredAmt.Text = drPromoType("TPL_ReqAmount").ToString()
            cboTPL_RequiredAmt_State.SelectedValue = drPromoType("TPL_ReqAmount_STATE").ToString()

            txtTPL_FreeItems.Text = drPromoType("TPL_FreeItems").ToString
            cboTPL_FreeItems_State.SelectedValue = drPromoType("TPL_FreeItems_STATE").ToString()
            txtTPL_Prizes.Text = drPromoType("TPL_Prizes").ToString
            cboTPL_Prizes_State.SelectedValue = drPromoType("TPL_Prizes_STATE").ToString()
            txtTPL_DepName.Text = drPromoType("TPL_DeptName").ToString
            cboTPL_DepName_State.SelectedValue = drPromoType("TPL_DeptName_STATE").ToString()
            txtTPL_SellingArea.Text = drPromoType("TPL_SellingArea").ToString
            cboTPL_SellingArea_State.SelectedValue = drPromoType("TPL_SellingArea_STATE").ToString()
            txtTPL_ItemName.Text = drPromoType("TPL_ItemName").ToString
            cboTPL_ItemName_State.SelectedValue = drPromoType("TPL_ItemName_STATE").ToString()
            txtTPL_BonusPoints.Text = drPromoType("TPL_BonusPoints").ToString
            cboTPL_BonusPoints_State.SelectedValue = drPromoType("TPL_BonusPoints_STATE").ToString()
            txtTPL_CelebName.Text = drPromoType("TPL_CelebName").ToString
            cboTPL_CelebName_State.SelectedValue = drPromoType("TPL_CelebName_STATE").ToString()
            txtTPL_EventTime.Text = drPromoType("TPL_EventTime").ToString
            cboTPL_EventTime_State.SelectedValue = drPromoType("TPL_EventTime_STATE").ToString()
            txtTPL_BuyQty.Text = drPromoType("TPL_BuyQty").ToString
            cboTPL_BuyQty_State.SelectedValue = drPromoType("TPL_BuyQty_STATE").ToString()
            txtTPL_TakeQty.Text = drPromoType("TPL_TakeQty").ToString
            cboTPL_TakeQty_State.SelectedValue = drPromoType("TPL_TakeQty_STATE").ToString()
            txtTPL_ActivityName.Text = drPromoType("TPL_ActivityName").ToString
            cboTPL_ActivityName_State.SelectedValue = drPromoType("TPL_ActivityName_STATE").ToString()

            cboTPL_PromoNotes_State.SelectedValue = drPromoType("TPL_PromoNotes_STATE").ToString()
            cboTPL_DiscountAmt_State.SelectedValue = drPromoType("TPL_DiscAmount_STATE").ToString()
            cboTPL_PercentDisc_State.SelectedValue = drPromoType("TPL_PercentDisc_STATE").ToString()

            cboXML_Sponsorship.SelectedValue = drPromoType("CompSponsorship_Value").ToString()
            cboXML_Sponsorship_State.SelectedValue = drPromoType("CompSponsorship_STATE").ToString()

            cboXML_EligibleCards_State.SelectedValue = drPromoType("POS_EligibleCards_STATE").ToString()
            chk_IsEligibleRequired.Checked = CBool(drPromoType("IsEligibleCardsRequired"))


            CType(chkXML_eCardAll, CheckBox).Enabled = True

            chkXML_eCard1.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "01") > 0)
            chkXML_eCard2.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "02") > 0)
            chkXML_eCard3.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "03") > 0)
            chkXML_eCard4.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "04") > 0)
            chkXML_eCard5.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "05") > 0)
            chkXML_eCard6.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "06") > 0)
            chkXML_eCard7.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "07") > 0)
            chkXML_eCard8.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "08") > 0)
            chkXML_eCard9.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "09") > 0)
            chkXML_eCard10.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "10") > 0)
            chkXML_eCard11.Checked = (InStr(drPromoType("POS_EligibleCards").ToString, "11") > 0)

            FillTypeCategoryList()
            cboPromoCategory.SelectedValue = drPromoType("TypeCategory").ToString()
            FillPromoSubCategoryList()
            cboPromoSubCategory.SelectedValue = drPromoType("TypeSubCategory").ToString()

            If (cboXML_EligibleCards_State.SelectedValue <> 0) Then
                ViewState("SubGroupName") = "LoyaltyCardLevel"
            End If

            If (ViewState("SubGroupName") <> "") Then
                FillResDropDownList(cboXML_DiscCondition, "PosDiscCondition", ViewState("SubGroupName"))
            Else
                FillResDropDownList(cboXML_DiscCondition, "PosDiscCondition")
            End If

            ShowPromoTypeConditions()
            ShowPromoTypeRules()


            SetConditionLevel()
            SetRuleLevel()

        Else

            ' error encountered

        End If

    End Sub

    Private Sub SetConditionLevel()
        Dim strQuery As String = ""
        Dim dtRow As DataRow
        Dim SeqNo As Integer = 0

        strQuery = "SELECT CASE WHEN  c.SequenceNo IS NOT NULL THEN c.SequenceNo ELSE 0 END AS SequenceNo FROM PromoTypes P LEFT JOIN PromoTypeConditions C " & _
                    "ON C.PromoTypeID = P.PromoTypeID WHERE p.PromoTypeID = 0" & ViewState("PromoTypeID") & _
                    " ORDER BY c.SequenceNo DESC"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, dtRow) Then
            SeqNo = dtRow("SequenceNo") + 1
            txtXML_CondSeqNo.Text = SeqNo
        Else
            txtXML_CondSeqNo.Text = 1
        End If


    End Sub

    Private Sub SetRuleLevel()
        Dim strQuery As String = ""
        Dim dtRow As DataRow
        Dim SeqNo As Integer = 0

        strQuery = "SELECT CASE WHEN  pt.SequenceNo IS NOT NULL THEN pt.SequenceNo ELSE 0 END AS SequenceNo FROM PromoTypes P LEFT JOIN PromoTypeRules PT " & _
                    "ON PT.PromoTypeID = P.PromoTypeID WHERE p.PromoTypeID = 0" & ViewState("PromoTypeID") & _
                    " ORDER BY PT.SequenceNo DESC"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, dtRow) Then
            SeqNo = dtRow("SequenceNo") + 1
            txtXML_RuleSeqNo.Text = SeqNo
        Else
            txtXML_RuleSeqNo.Text = 1
        End If


    End Sub

    Private Sub InitializePromoRange(ByRef promoID As Integer)
        Dim dt As New DataTable
        Dim strQuery As String

        strQuery = "SELECT POS_PromoIDfrom, POS_PromoIDto, r.ElementValue, r.ElementName " & _
                    "FROM PromoTypePromoIDRanges p " & _
                    "INNER JOIN ResListValues r ON p.CompSponsorship = r.ElementID " & _
                    "WHERE r.GroupName = 'CompSponsorship' AND PromoTypeID = 0" & promoID

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dt)

        If dt.Rows.Count > 0 Then
            gvPromoIDRange.DataSource = dt
            gvPromoIDRange.DataBind()
        Else
            gvPromoIDRange.DataSource = New ArrayList()
            gvPromoIDRange.DataBind()
            lnkDeletePromoRange.Visible = False
        End If

        txtPromoIDfrom.Text = ""
        txtPromoIDto.Text = ""
        FillCompSponsorship()

    End Sub

    Private Sub FillCompSponsorship()
        Dim strQuery As String
        Dim dtTable As New DataTable

        strQuery = "SELECT ElementValue,SubGroupName + '-' +  ElementName AS ElementName " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'CompSponsorship' ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboPromoIDCompSponsorship.ClearSelection()
        cboPromoIDCompSponsorship.Items.Clear()

        Dim lstItem As New ListItem

        lstItem.Value = -1
        lstItem.Text = "-- Select Value --"
        cboPromoIDCompSponsorship.Items.Add(lstItem)

        cboPromoIDCompSponsorship.DataSource = dtTable
        cboPromoIDCompSponsorship.DataBind()

    End Sub

    Private Sub InsertPromoIDRange(ByRef promoID As Integer, ByRef IDfrom As String, ByRef IDto As String, ByRef compSponsorship As Integer)
        Dim strQuery As String
        Dim sErrMess As String = ""
        Dim dtTable As New DataTable

        blistErrPromoRange.Items.Clear()

        If cboPromoIDCompSponsorship.SelectedValue = "" Then
            blistErrPromoRange.Items.Add("Please select a Shouldering Entity for Promo range.")
        ElseIf txtPromoIDfrom.Text = "" Or txtPromoIDto.Text = "" Then
            blistErrPromoRange.Items.Add("Range must not be blank")
        Else
            strQuery = "SELECT * FROM PromoTypePromoIDRanges " & _
                        "WHERE PromoTypeID = 0" & promoID & " AND CompSponsorship = '" & compSponsorship & "'"

            clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

            If dtTable.Rows.Count <= 0 Then

                strQuery = "INSERT INTO PromoTypePromoIDRanges (PromoTypeID, POS_PromoIDfrom, POS_PromoIDto, CompSponsorship) " & _
                "VALUES (" & promoID & ",'" & IDfrom & "','" & IDto & "','" & compSponsorship & "')"

                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
                    blistErrPromoRange.Items.Add("Error encountered while saving data:" & sErrMess)
                    Exit Sub
                End If
            End If
        End If

    End Sub
    Private Sub InitializeInputControls()

        ' XML options
        FillResDropDownList(cboXML_ActionType, "PosActionType")
        FillResDropDownList(cboXML_DiscountType, "PosDiscType")
        FillResDropDownList(cboXML_ProcessType, "PosProcessType")
        FillResDropDownList(cboXML_ExclusionType, "PosPromoExclType")

        If (ViewState("SubGroupName") <> "") Then
            FillResDropDownList(cboXML_DiscCondition, "PosDiscCondition", ViewState("SubGroupName"))
        Else
            FillResDropDownList(cboXML_DiscCondition, "PosDiscCondition")
        End If
        FillResDropDownList(cboXML_Priority, "PosPriority")

        FillResDropDownList(cboXML_RuleType, "PosRuleType")
        FillResDropDownList(cboXML_RuleValueType, "PosRuleValueType")
        FillResDropDownList(cboXML_RuleCondition, "PosRuleCondType")
        FillResDropDownList(cboXML_QualifiedItems, "PosQualifiedItems")

        'UPC Premium Promo
        FillResDropDownList(cboXLM_UPCPromoPremium, "PosUPCPromoPremState")

        ' VS process type (hide for the time being)
        trVSProcessType.Visible = False
        'FillResDropDownList(cboVSProcessType, "PromoTypeVSprocess")

        'CompSponsorship
        FillResDropDownList(cboXML_Sponsorship, "CompSponsorship", SystemUser.UserGroupType)
        FillResDropDownList(cboPromoIDCompSponsorship, "CompSponsorship")

        'Promotion Application
        FillResDropDownList(cboTPL_ProcType, "TplProcessType")

        'Purchase Requirement
        FillResDropDownList(cboTPL_PurchReq, "TplPurchaseReq")

        ' state selection
        FillResDropDownList(cboXML_ActDeacTime_State, "PromoTypeState")
        FillResDropDownList(cboXML_ActionType_State, "PromoTypeState")
        FillResDropDownList(cboXML_ActiveDays_State, "PromoTypeState")
        FillResDropDownList(cboXML_CompressID_State, "PromoTypeState")
        FillResDropDownList(cboXML_Descr1prm_State, "PromoTypeState")
        FillResDropDownList(cboXML_Descr2prm_State, "PromoTypeState")
        FillResDropDownList(cboXML_DiscountType_State, "PromoTypeState")
        FillResDropDownList(cboXML_ExclusionType_State, "PromoTypeState")
        FillResDropDownList(cboXML_MaxAmount_State, "PromoTypeState")
        FillResDropDownList(cboXML_MaxQty_State, "PromoTypeState")
        FillResDropDownList(cboXML_MessageID_State, "PromoTypeState")
        FillResDropDownList(cboXML_Priority_State, "PromoTypeState")
        FillResDropDownList(cboXML_ProcessType_State, "PromoTypeState")
        FillResDropDownList(cboXML_ReceiptDesc1_State, "PromoTypeState")
        FillResDropDownList(cboXML_ReceiptDesc2_State, "PromoTypeState")
        FillResDropDownList(cboXML_Condition_State, "PromoTypeState")
        FillResDropDownList(cboXML_DiscCondition_State, "PromoTypeState")
        FillResDropDownList(cboXML_Rule_State, "PromoTypeState")
        FillResDropDownList(cboXML_SMACKitPrice_State, "PromoTypeState")
        FillResDropDownList(cboXML_QualifiedCust_State, "PromoTypeState")
        FillResDropDownList(cboXML_NameOfPartners_State, "PromoTypeState")
        FillResDropDownList(cboXML_ProofOfMemb_State, "PromoTypeState")
        FillResDropDownList(cboXML_QualifiedItems_State, "PromoTypeState")
        FillResDropDownList(cboXML_EligibleCards_State, "PromoTypeState")
        FillResDropDownList(cboXML_Sponsorship_State, "PromoTypeState")
        FillResDropDownList(cboTPL_NumMonths_State, "PromoTypeState")
        FillResDropDownList(cboTPL_BankList_State, "PromoTypeState")
        FillResDropDownList(cboTPL_BrandNames_State, "PromoTypeState")
        FillResDropDownList(cboTPL_ProcType_State, "PromoTypeState")
        FillResDropDownList(cboTPL_PurchReq_State, "PromoTypeState")
        FillResDropDownList(cboTPL_RequiredAmt_State, "PromoTypeState")
        FillResDropDownList(cboTPL_FreeItems_State, "PromoTypeState")
        FillResDropDownList(cboTPL_Prizes_State, "PromoTypeState")
        FillResDropDownList(cboTPL_DepName_State, "PromoTypeState")
        FillResDropDownList(cboTPL_SellingArea_State, "PromoTypeState")
        FillResDropDownList(cboTPL_ItemName_State, "PromoTypeState")
        FillResDropDownList(cboTPL_BonusPoints_State, "PromoTypeState")
        FillResDropDownList(cboTPL_EventTime_State, "PromoTypeState")
        FillResDropDownList(cboTPL_CelebName_State, "PromoTypeState")
        FillResDropDownList(cboTPL_BuyQty_State, "PromoTypeState")
        FillResDropDownList(cboTPL_TakeQty_State, "PromoTypeState")
        FillResDropDownList(cboTPL_ActivityName_State, "PromoTypeState")
        FillResDropDownList(cboTPL_PromoNotes_State, "PromoTypeState")
        FillResDropDownList(cboTPL_DiscountAmt_State, "PromoTypeState")
        FillResDropDownList(cboTPL_PercentDisc_State, "PromoTypeState")
        FillResDropDownList(cboXML_PartnerEstablishmentGWP_State, "PromoTypeState")

        FillStartTime()
        FillEndTime()

        SetConditionLevel()
        SetRuleLevel()

        txtPromoIDfrom.Text = ""
        txtPromoIDto.Text = ""

        If gvPromoIDRange.Rows.Count <= 0 Then
            lnkDeletePromoRange.Visible = False
        End If
        FillCompSponsorship()

        cboTPL_DiscountAmt_State.Visible = True
        cboTPL_PercentDisc_State.Visible = True

        txtTypeCategory.Visible = False
        txtTypeSubCategory.Visible = False
        btnCancelAddTypeCat.Visible = False
        btnCancelAddTypeSub.Visible = False

        If SystemUser.UserGroupType = "SBU" And ViewState("PromoTypeID") < 0 Then
            cmbGroupType.SelectedValue = "SBU"
            cmbGroupType.Enabled = False
        End If

    End Sub

    Private Sub FillResDropDownList(ByRef cboListObj As DropDownList, ByVal sGroupName As String, Optional ByVal sSubGroup As String = "")

        Dim dtTable As New DataTable
        Dim strQuery As String

        If (sSubGroup <> "") Then

            strQuery = "SELECT ElementName, ElementValue " & _
                        "FROM ResListValues " & _
                        "WHERE GroupName = '" & sGroupName & "' " & _
                        "AND SubGroupName = '" & sSubGroup & "' " & _
                        "ORDER BY SequenceNo"
        Else
            strQuery = "SELECT ElementName, ElementValue " & _
                        "FROM ResListValues " & _
                        "WHERE GroupName = '" & sGroupName & "' " & _
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

    Private Sub ShowPromoTypeConditions()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT PC.*, RV.ElementName FROM PromoTypeConditions PC " & _
                    "INNER JOIN ResListValues RV ON PC.DiscCond = RV.ElementValue " & _
                    "WHERE RV.GroupName = 'PosDiscCondition' AND PC.PromoTypeID  = 0" & ViewState("PromoTypeID") & _
                    " ORDER BY PC.SequenceNo"
        'IIf(ViewState("SubGroupName") <> "", " AND RV.SubGroupName = '" & ViewState("SubGroupName") & "'", " ") & _


        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridConditions.DataSource = dtTable
        gridConditions.DataBind()
    End Sub
    Private Sub FillStartTime()
        Dim list As New ListItem
        Dim time As DateTime
        Dim ctr As Integer

        time = "12:00 AM"
        ctr = 0

        'With cboXML_StartTime
        '    .ClearSelection()
        '    .Items.Clear()

        While ctr < 48
            With cboXML_StartTime

                list.Value = time.ToString("HH:mm")
                list.Text = time.ToString("HH:mm")

                '.Items.Insert(list.Value, list.Text)
                .Items.Add(New ListItem(list.Text, list.Text))

            End With

            time = time.AddMinutes(30)
            ctr = ctr + 1
        End While

    End Sub

    Private Sub FillEndTime()
        Dim list As New ListItem
        Dim time As DateTime
        Dim ctr As Integer

        time = "12:00 AM"
        ctr = 0

        'With cboXML_StartTime
        '    .ClearSelection()
        '    .Items.Clear()

        While ctr < 48
            With cboXML_EndTime

                list.Value = time.ToString("HH:mm")
                list.Text = time.ToString("HH:mm")

                '.Items.Insert(list.Value, list.Text)
                .Items.Add(New ListItem(list.Text, list.Text))

            End With

            time = time.AddMinutes(30)
            ctr = ctr + 1
        End While

    End Sub
    Protected Sub FillTypeCategoryList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        Dim strGroupType As String


        If SystemUser.UserGroupType.ToString() = "CM" Then
            strGroupType = "REGULAR"
        Else
            strGroupType = SystemUser.UserGroupType.ToString()
        End If


        strQuery = "SELECT DISTINCT TypeCategory " & _
                    "FROM PromoTypes " & _
                    "WHERE ForMPDuseOnly = 0 " & _
                    "AND TypeCategory IS NOT NULL " & _
                    "AND GroupType = '" & strGroupType & "' " & _
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
            Dim strGroupType As String

            If SystemUser.UserGroupType.ToString() = "CM" Then
                strGroupType = "REGULAR"
            Else
                strGroupType = SystemUser.UserGroupType.ToString()
            End If


            strQuery = "SELECT DISTINCT TypeSubCategory " & _
                        "FROM PromoTypes " & _
                        "WHERE ForMPDuseOnly = 0 " & _
                        "AND TypeSubCategory IS NOT NULL " & _
                        "AND TypeCategory = '" & cboPromoCategory.SelectedValue & "' " & _
                        "AND GroupType = '" & strGroupType & "' " & _
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
    Private Sub ShowPromoTypeRules()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT PR.*, " & _
                        "V1.ElementName AS RuleTypeDesc, " & _
                        "V2.ElementName AS RuleValueTypeDesc, " & _
                        "V3.ElementName AS RuleConditionDesc " & _
                    "FROM PromoTypeRules AS PR " & _
                    "LEFT JOIN ResListValues AS V1 ON V1.GroupName = 'PosRuleType' AND V1.ElementValue = PR.RuleType " & _
                    "LEFT JOIN ResListValues AS V2 ON V2.GroupName = 'PosRuleValueType' AND V2.ElementValue = PR.ValueType " & _
                    "LEFT JOIN ResListValues AS V3 ON V3.GroupName = 'PosRuleCondType' AND V3.ElementValue = PR.RuleCondition " & _
                    "WHERE PromoTypeID  = 0" & ViewState("PromoTypeID") & _
                    " ORDER BY SequenceNo"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridPosRules.DataSource = dtTable
        gridPosRules.DataBind()

    End Sub

    Private Function EligibleCardsEncode() As String

        Dim sResult As String = ""

        sResult = IIf(chkXML_eCard1.Checked, "01;", "")
        sResult &= IIf(chkXML_eCard2.Checked, "02;", "")
        sResult &= IIf(chkXML_eCard3.Checked, "03;", "")
        sResult &= IIf(chkXML_eCard4.Checked, "04;", "")
        sResult &= IIf(chkXML_eCard5.Checked, "05;", "")
        sResult &= IIf(chkXML_eCard6.Checked, "06;", "")
        sResult &= IIf(chkXML_eCard7.Checked, "07;", "")
        sResult &= IIf(chkXML_eCard8.Checked, "08;", "")
        sResult &= IIf(chkXML_eCard9.Checked, "09;", "")
        sResult &= IIf(chkXML_eCard10.Checked, "10;", "")
        sResult &= IIf(chkXML_eCard11.Checked, "11;", "")

        EligibleCardsEncode = sResult
    End Function

    Protected Sub chkALL_Condition_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridConditions.HeaderRow.FindControl("chkALL_Condition"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridConditions.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelCondition"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridConditions.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelCondition"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub chkALL_PosRules_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridPosRules.HeaderRow.FindControl("chkALL_PosRules"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridPosRules.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelPosRules"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridPosRules.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelPosRules"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub chkRowSelCondition_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(gridConditions.HeaderRow.FindControl("chkALL_Condition"), CheckBox)

        If Not chk.Checked Then chk.Checked = False

    End Sub

    Protected Sub chkRowSelPosRules_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(gridPosRules.HeaderRow.FindControl("chkALL_PosRules"), CheckBox)

        If Not chk.Checked Then chk.Checked = False

    End Sub

    Protected Sub lnkDeleteSelectRule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteSelectRule.Click

        Dim b As Boolean = False

        For Each row As GridViewRow In gridPosRules.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSelPosRules")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete Promo Rule"
            clsSession.Message = "Delete selected rules from list?"
            clsSession.Icon = "inquiry"
            ViewState("process") = "DeletePromoRule"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>ShowMsgBox('','');</script>")
        End If

    End Sub

    Protected Sub lnkDeleteSelectCondition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteSelectCondition.Click

        Dim b As Boolean = False

        For Each row As GridViewRow In gridConditions.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSelCondition")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete Promo Condition"
            clsSession.Message = "Delete selected conditions from list?"
            clsSession.Icon = "inquiry"
            ViewState("process") = "DeleteCondition"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>ShowMsgBox('','');</script>")
        End If

    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Select Case ViewState("process")
            Case "DeletePromoRule"

                'delete confirmation
                If clsSession.DeleteStatus = "yes" Then

                    Dim sRowList As String = ""

                    ' get checked entries
                    For Each row As GridViewRow In gridPosRules.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSelPosRules")
                        If cb IsNot Nothing And cb.Checked Then

                            If sRowList = "" Then
                                sRowList = "'" & row.Cells(1).Text & "'"
                            Else
                                sRowList = sRowList & ",'" & row.Cells(1).Text & "'"
                            End If

                        End If
                    Next

                    If sRowList <> "" Then

                        Dim strSQL As String
                        Dim strErrMess As String = ""

                        strSQL = "DELETE FROM PromoTypeRules " & _
                                 " WHERE PromoTypeID = 0" & ViewState("PromoTypeID") & _
                                 " AND SequenceNo IN (" & sRowList & ") "

                        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then
                            ShowPromoTypeRules()
                            SetRuleLevel()
                        Else
                            ' error
                            blistErrorMsg.Items.Add("Error deleting data:" & strErrMess)
                        End If

                    End If
                End If

            Case "DeleteCondition"

                'delete confirmation
                If clsSession.DeleteStatus = "yes" Then

                    Dim sRowList As String = ""

                    ' get checked entries
                    For Each row As GridViewRow In gridConditions.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSelCondition")
                        If cb IsNot Nothing And cb.Checked Then

                            If sRowList = "" Then
                                sRowList = "'" & row.Cells(1).Text & "'"
                            Else
                                sRowList = sRowList & ",'" & row.Cells(1).Text & "'"
                            End If

                        End If
                    Next

                    If sRowList <> "" Then

                        Dim strSQL As String
                        Dim strErrMess As String = ""

                        strSQL = "DELETE FROM PromoTypeConditions " & _
                                 " WHERE PromoTypeID = 0" & ViewState("PromoTypeID") & _
                                 " AND SequenceNo IN (" & sRowList & ") "

                        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then
                            ShowPromoTypeConditions()
                            SetConditionLevel()
                        Else
                            ' error
                            blistErrorMsg.Items.Add("Error deleting data:" & strErrMess)
                        End If

                    End If
                End If

        End Select

    End Sub

    Protected Sub cmdAddCondition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddCondition.Click

        Dim strQuery As String
        Dim strErrMess As String = ""
        Dim dtTbl As DataTable

        blistErrorMsg.Items.Clear()

        If ViewState("PromoTypeID") = 0 Then
            blistErrorMsg.Items.Add("Please save first before creating Promo Condition.")
            Exit Sub
        End If

        If txtXML_DiscAmount.Text = "" Then
            blistErrorMsg.Items.Add("Condition Amount not indicated.")
            Exit Sub
        End If

        If cboXML_DiscCondition.SelectedIndex = -1 Then
            blistErrorMsg.Items.Add("Condition not specified.")
            Exit Sub

        End If

        If txtXML_CondSeqNo.Text = "" Then
            blistErrorMsg.Items.Add("Condition level not specified.")
            Exit Sub
        ElseIf CInt(txtXML_CondSeqNo.Text) = 0 Then
            blistErrorMsg.Items.Add("Invalid condition level.")
            Exit Sub
        End If

        ' check duplicates
        strQuery = "SELECT SequenceNo FROM PromoTypeConditions " & _
                    "WHERE PromoTypeID = 0" & ViewState("PromoTypeID") & _
                    " AND DiscCond = 0" & cboXML_DiscCondition.SelectedValue & _
                    " AND DiscAmnt1 = 0" & txtXML_DiscAmount.Text

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTbl) Then
            If dtTbl.Rows.Count > 0 Then
                blistErrorMsg.Items.Add("Condition is already included on the list.")
                Exit Sub
            End If
        End If

        ' insert data to table and grid
        strQuery = "INSERT INTO PromoTypeConditions " & _
                    "(PromoTypeID, DiscCond, DiscCond_STATE, DiscAmnt1, SequenceNo, Condition_STATE) VALUES (" & _
                    ViewState("PromoTypeID") & ", " & _
                    cboXML_DiscCondition.SelectedValue & ", " & _
                    cboXML_DiscCondition_State.SelectedValue & ", " & _
                    txtXML_DiscAmount.Text & ", " & _
                    txtXML_CondSeqNo.Text & ", " & _
                    cboXML_Condition_State.SelectedValue & ")"

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
            ShowPromoTypeConditions()

            'clear entry
            txtXML_DiscAmount.Text = ""
            txtXML_CondSeqNo.Text = ""
            cboXML_DiscCondition.SelectedIndex = -1
            cboXML_DiscCondition_State.SelectedIndex = -1

            SetConditionLevel()

        Else
            blistErrorMsg.Items.Add("Unable to add promo type condition: " & strErrMess)
        End If

    End Sub

    Protected Sub cmdAddPosRules_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddPosRules.Click

        Dim strQuery As String
        Dim strErrMess As String = ""
        Dim dtTbl As DataTable

        blistErrorMsg.Items.Clear()

        If ViewState("PromoTypeID") = 0 Then
            blistErrorMsg.Items.Add("Please save first before creating Promo Rules.")
            Exit Sub
        End If

        If cboXML_RuleType.SelectedValue = -1 Then
            blistErrorMsg.Items.Add("Rule Type not specified.")
            Exit Sub
        End If

        If cboXML_RuleValueType.SelectedValue = -1 Then
            blistErrorMsg.Items.Add("Rule Value Type not specified.")
            Exit Sub
        End If

        If cboXML_RuleCondition.SelectedValue = -1 Then
            blistErrorMsg.Items.Add("Rule Condition not specified.")
            Exit Sub
        End If

        If txtXML_RuleValue.Text = "" Then
            blistErrorMsg.Items.Add("Rule Value not indicated.")
            Exit Sub
        End If

        ' make Rule Level read only with auto assign number
        If txtXML_RuleSeqNo.Text = "" Then
            blistErrorMsg.Items.Add("Rule Level not indicated.")
            Exit Sub
        ElseIf CInt(txtXML_RuleSeqNo.Text) = 0 Then
            blistErrorMsg.Items.Add("Invalid Rule Level.")
            Exit Sub
        End If

        ' check duplicates
        strQuery = "SELECT SequenceNo FROM PromoTypeRules " & _
                    "WHERE PromoTypeID = 0" & ViewState("PromoTypeID") & _
                    " AND RuleType = 0" & cboXML_RuleType.SelectedValue & _
                    " AND ValueType = 0" & cboXML_RuleValueType.SelectedValue & _
                    " AND RuleCondition = 0" & cboXML_RuleCondition.SelectedValue
        '" AND RuleValue = 0" & txtXML_RuleValue.Text

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTbl) Then
            If dtTbl.Rows.Count > 0 Then
                blistErrorMsg.Items.Add("Rule is already included on the list.")
                Exit Sub
            End If
        End If

        ' insert data to table and grid
        strQuery = "INSERT INTO PromoTypeRules " & _
                    "(PromoTypeID, RuleType, ValueType, RuleCondition, RuleValue, Rule_State, SequenceNo) VALUES (" & _
                    ViewState("PromoTypeID") & ", " & _
                    cboXML_RuleType.SelectedValue & ", " & _
                    cboXML_RuleValueType.SelectedValue & ", " & _
                    cboXML_RuleCondition.SelectedValue & ", " & _
                    txtXML_RuleValue.Text & ", " & _
                    cboXML_Rule_State.SelectedValue & ", " & _
                    txtXML_RuleSeqNo.Text & ")"
        'txtXML_RuleValue.Text

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
            ShowPromoTypeRules()

            'clear entries
            'txtXML_RuleValue.Text = ""
            txtXML_RuleSeqNo.Text = ""
            cboXML_RuleType.SelectedIndex = -1
            cboXML_RuleValueType.SelectedIndex = -1
            cboXML_RuleCondition.SelectedIndex = -1
            cboXML_Rule_State.SelectedIndex = -1

            SetRuleLevel()

        Else
            blistErrorMsg.Items.Add("Unable to add promo type rule: " & strErrMess)
        End If

    End Sub

    Protected Sub chkXML_eCardAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkXML_eCardAll.CheckedChanged
        Dim bCheck As Boolean = chkXML_eCardAll.Checked

        chkXML_eCard1.Checked = bCheck
        chkXML_eCard2.Checked = bCheck
        chkXML_eCard3.Checked = bCheck
        chkXML_eCard4.Checked = bCheck
        chkXML_eCard5.Checked = bCheck
        chkXML_eCard6.Checked = bCheck
        chkXML_eCard7.Checked = bCheck
        chkXML_eCard8.Checked = bCheck
        chkXML_eCard9.Checked = bCheck
        chkXML_eCard10.Checked = bCheck
        chkXML_eCard11.Checked = bCheck
    End Sub

    Protected Sub cboTPL_DiscountAmt_State_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTPL_DiscountAmt_State.SelectedIndexChanged
        If (cboTPL_DiscountAmt_State.SelectedValue > 0) Then
            cboTPL_PercentDisc_State.SelectedValue = 0
        End If
    End Sub

    Protected Sub cboTPL_PercentDisc_State_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTPL_PercentDisc_State.SelectedIndexChanged
        If (cboTPL_PercentDisc_State.SelectedValue > 0) Then
            cboTPL_DiscountAmt_State.SelectedValue = 0
        End If
    End Sub

    Protected Sub cboPromoCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPromoCategory.SelectedIndexChanged
        FillPromoSubCategoryList()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub btnAddPromoRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPromoRange.Click
        InsertPromoIDRange(ViewState("PromoTypeID"), txtPromoIDfrom.Text, txtPromoIDto.Text, cboPromoIDCompSponsorship.SelectedValue)
        InitializePromoRange(ViewState("PromoTypeID"))
    End Sub

    Protected Sub lnkDeletePromoRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeletePromoRange.Click
        Dim strQuery As String
        Dim strErrMess As String = ""


        For Each row As GridViewRow In gvPromoIDRange.Rows
            Dim cb As CheckBox = row.FindControl("chkDelPromoRange")
            If cb IsNot Nothing AndAlso cb.Checked Then
                strQuery = "DELETE FROM PromoTypePromoIDRanges " & _
                            "WHERE PromoTypeID = '" & ViewState("PromoTypeID") & "' AND POS_PromoIDfrom = '" & row.Cells(1).Text & "' " & _
                            "AND POS_PromoIDto = '" & row.Cells(2).Text & "' AND CompSponsorship = '" & row.Cells(3).Text & "'"

                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
                    blistErrorMsg.Items.Add("Error deleting data:" & strErrMess)
                Else
                    InitializePromoRange(ViewState("PromoTypeID"))
                End If

                Exit For
            End If
        Next
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

    Protected Sub cboXML_EligibleCards_State_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboXML_EligibleCards_State.SelectedIndexChanged
        If (cboXML_EligibleCards_State.SelectedValue <> 0) Then
            ViewState("SubGroupName") = "LoyaltyCardLevel"
        End If
    End Sub
End Class
