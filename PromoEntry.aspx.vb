Option Explicit On

Imports dsPromotionsTableAdapters
Imports System.Data

Imports System
Imports System.Web
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Collections.Generic


Partial Class PromoEntry
    Inherits System.Web.UI.Page

    Protected Sub cboPromoCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPromoCategory.SelectedIndexChanged
        'test commit
        If cboPromoCategory.SelectedValue = "-" Then
            'hide all if invalid selection
        Else
        End If

        FillPromoSubCategoryList()
        FillPromoTypeList()

    End Sub

    Protected Sub cboPromoSubCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPromoSubCategory.SelectedIndexChanged

        If cboPromoSubCategory.SelectedValue = "-" Then
            'hide all if invalid selection
        Else
        End If

        FillPromoTypeList()

    End Sub

    Protected Sub FillTypeCategoryList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        Dim strGroupType As String
        Dim strGroupTypeALL As String = "ALL"

        If SystemUser.UserGroupType.ToString() = "CM" Then
            strGroupType = "REGULAR"
        Else
            strGroupType = SystemUser.UserGroupType.ToString()
        End If


        strQuery = "SELECT DISTINCT TypeCategory " & _
                    "FROM PromoTypes " & _
                    "WHERE ForMPDuseOnly = 0 " & _
                    "AND TypeCategory IS NOT NULL " & _
                    "AND GroupType in ('" & strGroupType & "','" & strGroupTypeALL & "') " & _
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

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        Dim strGroupType As String
        Dim strGroupTypeALL As String = "ALL"

        If SystemUser.UserGroupType.ToString() = "CM" Then
            strGroupType = "REGULAR"
        ElseIf SystemUser.UserGroupType.ToString() = "BCR" Then
            strGroupType = "SBU"
        Else
            strGroupType = SystemUser.UserGroupType.ToString()
        End If

        strQuery = "SELECT DISTINCT TypeSubCategory " & _
                    "FROM PromoTypes " & _
                    "WHERE ForMPDuseOnly = 0 " & _
                    "AND TypeSubCategory IS NOT NULL " & _
                    "AND TypeCategory = '" & cboPromoCategory.SelectedValue.ToString() & "' " & _
                    "AND GroupType in ('" & strGroupType & "','" & strGroupTypeALL & "') " & _
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

    End Sub

    ' mad7740 : start
    Protected Sub FillPromoApplicationList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        'ResListValues setup in list Values 
        Dim strGroupType As String

        If SystemUser.UserGroupType.ToString() = "CM" Or SystemUser.UserGroupType.ToString() = "SBU" Then
            strGroupType = "TplProcessType" 'REGULAR
        ElseIf SystemUser.UserGroupType.ToString() = "BCR" Or SystemUser.UserGroupType.ToString() = "REGULAR" Then
            strGroupType = "PosProcessType" 'SBU
        Else
            strGroupType = SystemUser.UserGroupType.ToString()
        End If

        strQuery = "SELECT ElementValue, ElementName " & _
                   "FROM ResListValues " & _
                   "WHERE GroupName = '" & strGroupType & "' " & _
                   "AND IsActive = 1 " & _
                   "ORDER BY ElementID"


        'Mantis#64851 Excluded the reason of intermittent issue for Promo Application, by adding Is Active Condition

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        ddItem.Text = "-- Select Value --"
        ddItem.Value = -1

        With cboTPL_ProcessType
            .ClearSelection()
            .Items.Clear()
            .DataSource = dtTable
            .Items.Insert(0, ddItem)
            .DataBind()
        End With

        cboTPL_ProcessType.SelectedValue = -1
    End Sub
    Private Sub FillCardsGrid()

        Dim dtTable As New DataTable
        Dim drPromotions As DataRow = Nothing
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT CardName FROM BankBins GROUP BY CardName ORDER BY CardName;"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        lstBankBins.DataSource = dtTable
        'lstBankBins.DataValueField = "CardName"
        lstBankBins.DataTextField = "CardName"
        lstBankBins.DataBind()

        dtTable = Nothing

        'lnkDeleteUPCPromoPrem.Enabled = (gridUPCPromoPrem.Rows.Count > 0)

    End Sub
    Protected Sub ShowBankBins(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dtTable As New DataTable
        Dim drPromotions As DataRow = Nothing
        Dim strQuery As String

        ' load data to grid
        Dim selected As String = "'"
        Dim item As ListItem
        For Each item In lstBankBins.Items
            Dim x As Boolean = item.Selected
            If x = True Then
                selected = selected + item.Text & "','"
            End If
        Next
        selected = selected & "'"
        strQuery = "SELECT * FROM BankBins WHERE CardName IN (" & selected & ") ORDER BY CardName, CardBin;"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridPromoBankBins.DataSource = dtTable
        gridPromoBankBins.DataBind()

        dtTable = Nothing
    End Sub
    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridPromoBankBins.HeaderRow.FindControl("chkALL"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridPromoBankBins.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridPromoBankBins.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub
    Protected Sub FillQualifiedItemList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        Dim strGroupType As String = ""
        Dim strCondition As String = ""

        If SystemUser.UserGroupType.ToString() = "CM" Then
            strGroupType = "REGULAR"
        ElseIf SystemUser.UserGroupType.ToString() = "BCR" Then
            strGroupType = "SBU"
        Else
            strGroupType = SystemUser.UserGroupType.ToString()
        End If

        If strGroupType = "SBU" Then
            strCondition = "AND ElementValue <> 3"
        End If

        strQuery = "SELECT ElementValue, ElementName " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'PosQualifiedItems' " & _
                    strCondition & _
                    "ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        ddItem.Text = "-- Select Value --"
        ddItem.Value = -1

        With cboXML_QualifiedItems
            .ClearSelection()
            .Items.Clear()
            .Items.Insert(0, ddItem)
            .DataSource = dtTable
            '.SelectedValue = Nothing
            '.Text = Nothing
            .DataBind()
        End With

        cboXML_QualifiedItems.SelectedValue = -1
    End Sub
    Protected Sub FillShoulderingEntityList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT CAST(ElementValue AS INT) 'ElementValue', ElementName " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'ChargeableEntity' " & _
                    "ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        ddItem.Text = "-- Select Value --"
        ddItem.Value = -1

        With cboXML_Sponsorship
            .SelectedValue = Nothing
            .ClearSelection()
            .Items.Clear()
            .DataSource = dtTable
            .Items.Insert(0, ddItem)
            'For Each row As DataRow In dtTable.Rows
            '    ddItem = New ListItem
            '    ddItem.Text = row.Item(1)
            '    ddItem.Value = row.Item(0)
            '    .Items.Add(ddItem)
            'Next row
            .DataBind()
        End With

        cboXML_Sponsorship.SelectedValue = -1
    End Sub
    ' mad7740 : end

    'rbs7281 : start
    Protected Sub FillRebateDiscTypeList()
        trXML_RebateDiscType.Visible = True
        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT CAST(ElementValue AS INT) 'ElementValue', ElementName " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'DiscountType' " & _
                    "ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        ddItem.Text = "-- Select Value --"
        ddItem.Value = -1

        With cboXML_RebateDiscType
            .SelectedValue = Nothing
            .ClearSelection()
            .Items.Clear()
            .DataSource = dtTable
            .Items.Insert(0, ddItem)
            'For Each row As DataRow In dtTable.Rows
            '    ddItem = New ListItem
            '    ddItem.Text = row.Item(1)
            '    ddItem.Value = row.Item(0)
            '    .Items.Add(ddItem)
            'Next row
            .DataBind()
        End With

        cboXML_Sponsorship.SelectedValue = -1
    End Sub
    'rbs7281 : end

    Protected Sub FillPromoTypeList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""
        Dim strGroupType As String
        Dim strGroupTypeALL As String = "ALL"

        If SystemUser.UserGroupType.ToString() = "CM" Then
            strGroupType = "REGULAR"
        ElseIf SystemUser.UserGroupType.ToString() = "BCR" Then
            strGroupType = "SBU"
        Else
            strGroupType = SystemUser.UserGroupType.ToString()
        End If

        strQuery = "SELECT PromoTypeID, TypeDesc " & _
                    "FROM PromoTypes " & _
                    "WHERE ForMPDuseOnly = 0 " & _
                    "AND TypeCategory = '" & cboPromoCategory.SelectedValue.ToString() & "' " & _
                    "AND TypeSubCategory = '" & cboPromoSubCategory.SelectedValue.ToString() & "' " & _
                    "AND GroupType in ('" & strGroupType & "','" & strGroupTypeALL & "') " & _
                    "ORDER BY TypeDesc"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        With cboPromoType
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

        If cboPromoType.Items.Count = 1 Then
            cboPromoType.SelectedIndex = 0
        Else
            cboPromoType.SelectedValue = -1
        End If

        Dim e As System.EventArgs = Nothing
        cboPromoType_SelectedIndexChanged(Me, e)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        ' -- #TODO: Upgrade to ViewState variable
        'If Request("RequestID") <> Nothing Then
        '    ViewState("RequestID") = Request("RequestID")
        '    clsSession.CurrRequestID = Request("RequestID")
        'End If

        If Not IsPostBack() Then
            'Dim DT As New DataTable
            'DT.Columns.Add("StartDate")
            'DT.Columns.Add("EndDate")
            'DT.Columns.Add("MaxNumber")
            'DT.Columns.Add("Counter")
            'DT.Columns.Add("Prize")
            'DT.Rows.Add(txtStartDate.Text, txtEndDate.Text, txtMaxNumber.Text, txtCounter.Text, txtPrice.Text)
            'gvSeeding.DataSource = DT
            'gvSeeding.DataBind()

            ' register client-side scripts
            RegisterLocalClientSideScripts()

            Dim drPromoRequest As DataRow = Nothing
            Dim strSQLcmd As String

            FillTypeCategoryList()
            LoadEligibleCards()

            strSQLcmd = "SELECT * FROM PromoRequests " & _
                        "WHERE RequestID = 0" & clsSession.CurrRequestID

            If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromoRequest) Then

                ViewState("PromoPeriodFrom") = drPromoRequest("PromoPeriodFrom").ToString()
                ViewState("PromoPeriodTo") = drPromoRequest("PromoPeriodTo").ToString()

                clsSession.PeriodFrom = ViewState("PromoPeriodFrom")
                clsSession.PeriodTo = ViewState("PromoPeriodTo")

                ' load saved Promotions data if PromoRequest exists

                If clsSession.CurrPromoID > 0 Then

                    LoadPromotionData(clsSession.CurrPromoID)

                Else

                    '::::::::::::::::::::::::::::::::::::::::::::::::::::::
                    ' New Promotion Entry
                    '::::::::::::::::::::::::::::::::::::::::::::::::::::::

                    If SystemUser.UserGroupType = "SACI" Then

                        cboPromoType.Enabled = False
                        cboPromoType.DataBind()             ' fill dropdown list early in order to access data

                        cboPromoType.SelectedValue = 93     ' TODO: remove hard-coding

                        DisplayPromoTypeLayout(CInt(cboPromoType.SelectedValue))

                    ElseIf SystemUser.UserGroupType = "SBU" Or SystemUser.UserGroupType = "BCR" Then ' Default and restrict to Event (SBU Mktg)   

                        'show promo types exclusive to SBU and BCR groups
                        'CHECK_HERE!
                        'FillPromoTypeSelection("SBU")

                        lblMechanics.Visible = False
                        panMechanics.Visible = False
                        panClassDiscount.Visible = False
                        panMarkdown.Visible = False

                    Else

                        'CHECK_HERE!
                        'FillPromoTypeSelection("REGULAR")

                        lblMechanics.Visible = False
                        panMechanics.Visible = False
                        panClassDiscount.Visible = False
                        panMarkdown.Visible = False

                    End If

                    FillPromoTypeList()

                End If

            End If

            ' file download conditions
            If Request("Filename") <> Nothing Then
                Dim fname As String
                fname = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString & "\" & Request("Filename")
                Response.ContentType = "application/x-msdownload"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" & Request("Filename"))
                Response.TransmitFile(fname)
                Response.End()
            End If

            If Request("DownLoad") <> Nothing And Request("Path") <> Nothing Then
                Dim fname As String

                'fname = Server.MapPath("~/posfiles" & Session("FolderParent").ToString & "/" & Request("DbName"))
                fname = Request("Path") & ".zip"

                Response.ContentType = "application/x-msdownload"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" & Request("DownLoad"))
                Response.TransmitFile(fname)
                Response.End()
            End If

            btnDownload_Click(sender, e)
            btnSeedDownload_Click(sender, e)

        End If  ' ispostback


        If panSMACdeals.Visible Then
            ReComputeValues()
        End If

        ' for SBU Marketing and BCR   
        If SystemUser.UserGroupType = "SBU" Or SystemUser.UserGroupType = "BCR" Then

            trPromoDetails.Visible = False

            'MALAgasino 20180910 - If UPC Level = 1, allow adding promo details. As per new SBU Template requirement,
            'Start of Code
            Dim drPromoType As DataRow

            drPromoType = GetPromoTypeInfo(cboPromoType.SelectedValue)

            If Not (IsDBNull(drPromoType) Or drPromoType Is Nothing) Then

                'rbs7281 20250730 : SBU and MBU requirements for Promo Detail
                'Start code

                If drPromoType("GroupType") = "ALL" Then

                    If (drPromoType("IsUPCLevel") = True) Or (drPromoType("Pos_AllowCoupons") = 1) Then

                        'rbs7281 20250730 : SBU and MBU requirements for Promo Detail
                        'Start code
                        If (SystemUser.UserGroupType <> "SBU") Then
                            trPromoDetails.Visible = True
                            lnkBranches.Enabled = False
                        Else
                            If drPromoType("PromoTypeID") = "288" Then
                                trPromoDetails.Visible = True
                                lnkBranches.Enabled = False
                            Else
                                trPromoDetails.Visible = False
                                lnkBranches.Enabled = True
                            End If

                            If drPromoType("Pos_AllowCoupons") = 1 Then
                                trPromoDetails.Visible = True
                                lnkBranches.Enabled = False

                            End If
                        End If
                        'End Code
                    Else
                        trPromoDetails.Visible = True
                    End If
                Else
                    'rbs7281 20250903 retain the existing
                    If (drPromoType("IsUPCLevel") = True) Or (drPromoType("Pos_AllowCoupons") = 1) Then
                        trPromoDetails.Visible = True
                    Else

                        lnkBranches.Enabled = True

                        '' validate if already have branches
                        'sqldsData.SelectCommand = "Select PromoID from PromoBranch Where PromoID In (Select PromoID From Promotions Where RequestID = " & clsSession.CurrRequestID & ")"
                        'Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

                        'If dv.Count > 0 Then
                        '    lnkBranches.Enabled = False
                        'Else
                        '    lnkBranches.Enabled = True
                        'End If

                    End If
                End If
                'End Code
            Else

                lnkBranches.Enabled = True

                If (SystemUser.UserGroupType <> "SBU") Then 'rbs7281 included in first view for MBU templates to show Promo Detail
                    trPromoDetails.Visible = True
                    lnkBranches.Enabled = False
                End If
                '' validate if already have branches
                'sqldsData.SelectCommand = "Select PromoID from PromoBranch Where PromoID In (Select PromoID From Promotions Where RequestID = " & clsSession.CurrRequestID & ")"
                'Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

                'If dv.Count > 0 Then
                '    lnkBranches.Enabled = False
                'Else
                '    lnkBranches.Enabled = True
                'End If
            End If


            'End of Code


            'lnkBranches.Enabled = True
            'cboPromoType.Style("visibility") = "hidden"
            'lblPromoType.Text = "Event"

        ElseIf SystemUser.UserGroupType = "SACI" Then   ' Default and restrict to SMAC deal for all SACI request

            cboPromoType.Style("visibility") = "hidden"
            lblPromoType.Text = "SMAC DEALS"
        End If

        ' transfer code after getting value of v_AllowAttachment/v_RequireAttachment MPD UAT Findings (10/09/2012)
        'lnkattachment.Visible = (ViewState("v_AllowAttachment") Or ViewState("v_RequireAttachment"))
        'lblAttachment.Visible = lnkattachment.Visible

        'trPromoAttachment.Visible = (ViewState("v_AllowAttachment") Or ViewState("v_RequireAttachment"))

    End Sub

    Private Sub LoadPromotionData(ByVal nPromoID As Long)

        Dim strSQLcmd As String
        Dim strSQLcmdcards As String
        Dim drPromotion As DataRow = Nothing
        Dim fillCards As DataTable = Nothing


        strSQLcmd = "SELECT P.*, T.TypeCategory, T.TypeSubCategory " & _
                    "FROM Promotions AS P " & _
                    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                    "WHERE PromoID = 0" & nPromoID

        strSQLcmdcards = "SELECT x.CardName, x.CardBin " & _
                    "FROM PromoBankBins as x " & _
                    "INNER JOIN Promotions as p on p.PromoID = x.PromoID " & _
                    "WHERE x.PromoID = 0" & nPromoID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromotion) Then

            If SystemUser.UserGroupType = "SACI" Or _
                SystemUser.UserGroupType = "CREDIT" Then

                cboPromoType.Enabled = False

                'cboPromoType.DataBind()     ' fill dropdown list early in order to access data

            Else

                'cboPromoType.DataBind()     ' fill dropdown list early in order to access data

            End If

            ' set dropdown selections
            cboPromoCategory.SelectedValue = drPromotion("TypeCategory")

            FillPromoSubCategoryList()
            cboPromoSubCategory.SelectedValue = drPromotion("TypeSubCategory")

            FillPromoTypeList()
            cboPromoType.SelectedValue = CInt(drPromotion("PromoTypeID"))

            DisplayPromoTypeLayout(CInt(cboPromoType.SelectedValue))

            ' assign values
            'rbs7281 - MW Update
            If panDiscCharging.Visible Then
                LoadShoulderingEntityDefaultSelection(True)
                If cboCompSponsorship.SelectedValue Then
                    cboCompSponsorship.SelectedValue = drPromotion("CompSponsorship").ToString
                End If
            End If

            If panChargeableEntity.Visible Then
                cboChargeableEntity.SelectedValue = drPromotion("TplChargeEntity").ToString

                txtTplChargeEnt_VendorCode.Text = drPromotion("TplChargeEnt_VendorCode").ToString
                txtTplChargeEnt_DSsub.Text = drPromotion("TplChargeEnt_DSsub").ToString
                txtTplChargeEnt_BUsub.Text = drPromotion("TplChargeEnt_BUsub").ToString

                ShowChargeableEntityValues()
            End If

            If panTemplated.Visible Then

                If trTPL_NumMonths.Visible Then txtTPL_NumMonths.Text = drPromotion("TplNumMonths").ToString
                If trTPL_BankList.Visible Then txtTPL_BankList.Text = drPromotion("TplBankList").ToString
                If trTpl_SubsidyRate.Visible Then txtTpl_SubsidyRate.Text = drPromotion("TplSubsidyRate").ToString
                If trTPL_BrandNames.Visible Then txtTPL_BrandNames.Text = drPromotion("TplBrandNames").ToString
                If trTPL_RefMemo.Visible Then txtTPL_RefMemo.Text = drPromotion("TplRefMemo").ToString
                If trTPL_ProcessType.Visible Then cboTPL_ProcessType.SelectedValue = drPromotion("TplProcessType").ToString

                If trTPL_PurchaseReq.Visible Then cboTPL_PurchaseReq.SelectedValue = drPromotion("TplPurchaseReq").ToString
                If trTPL_ReqAmount.Visible Then txtTPL_ReqAmount.Text = Format(IIf(IsDBNull(drPromotion("TplReqAmount")), 0, drPromotion("TplReqAmount")), "#,##0.00")

                '@@here_discountamount
                If trTPL_DiscAmount.Visible Then txtTPL_DiscAmount.Text = drPromotion("DiscAmount").ToString

                If trTPL_PercentDisc.Visible Then txtTPL_PercentDisc.Text = drPromotion("TplPercentDisc").ToString

                ' NBS20190822: add this field
                If trTPL_TransDesc.Visible Then txtTPL_TransDesc.Text = drPromotion("TplTransDesc").ToString
                If trTPL_FreeItems.Visible Then txtTPL_FreeItems.Text = drPromotion("TplFreeItems").ToString
                If trTPL_Prizes.Visible Then txtTPL_Prizes.Text = drPromotion("TplPrizes").ToString
                If trTPL_DeptName.Visible Then txtTPL_DeptName.Text = drPromotion("TplDeptName").ToString
                If trTPL_SellingArea.Visible Then txtTPL_SellingArea.Text = drPromotion("TplSellingArea").ToString
                If trTPL_EventTime.Visible Then txtTPL_EventTime.Text = drPromotion("TplEventTime").ToString
                If trTPL_ItemName.Visible Then txtTPL_ItemName.Text = drPromotion("TplItemName").ToString
                If trTPL_CelebName.Visible Then txtTPL_CelebName.Text = drPromotion("TplCelebName").ToString
                If trTPL_BonusPoints.Visible Then txtTPL_BonusPoints.Text = drPromotion("TplBonusPoints").ToString

                If trTPL_ActivityName.Visible Then txtTPL_ActivityName.Text = drPromotion("TplActivityName").ToString
                If trTPL_BuyQty.Visible Then txtTPL_BuyQty.Text = drPromotion("TplBuyQty").ToString
                If trTPL_TakeQty.Visible Then txtTPL_TakeQty.Text = drPromotion("TplTakeQty").ToString

                If trPOS_StdExclusion.Visible Then chkPOS_StdExclusion.Checked = (drPromotion("PosStdExclusion") = 1)
                If trPOS_PermExclusion.Visible Then chkPOS_PermExclusion.Checked = (drPromotion("PosPermExclusion") = 1)

                ' mad7740
                If trTPL_MinAmount.Visible Then
                    txtTPL_MinAmount.Text = FormatMoneyField(IIf(IsDBNull(drPromotion("PosMinAmnt")), 0, drPromotion("PosMinAmnt")))
                End If

                'cjg2243

                'rbs7281 ToString() Inclusion to prevent issue when DBNull returned
                FillPromoApplicationList()
                cboTPL_ProcessType.SelectedValue = drPromotion("TplProcessType").ToString()
                FillQualifiedItemList()
                cboXML_QualifiedItems.SelectedValue = drPromotion("PosQualifiedItems").ToString()
                FillShoulderingEntityList()
                cboXML_Sponsorship.SelectedValue = drPromotion("CompSponsorship").ToString()

                If trTPL_percentage.Visible Then
                    txtTPL_percentage.Text = FormatMoneyField(IIf(IsDBNull(drPromotion("PercentDisc")), 0, drPromotion("PercentDisc")))
                End If

                FillCardsGrid()
                If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strSQLcmdcards, fillCards) Then
                    If panBankBins.Visible Then

                        Dim countRow As Integer = lstBankBins.Items.Count
                        ' Loop through each item in the CheckBoxList
                        For Each item As ListItem In lstBankBins.Items
                            Dim foundRow As DataRow = clsSystemApp.FindRowByColumnValue(fillCards, "CardName", item.Value)
                            If foundRow IsNot Nothing Then
                                item.Selected = True

                            End If
                        Next
                    End If
                End If






                ' Convert the list to an array if needed






                'cjg2243
                'jps4226

            End If
            'mad7740 discount capping 
            If trTPL_discountcapping.Visible Then
                txtTPL_discountcapping.Text = FormatMoneyField(IIf(IsDBNull(drPromotion("PosMaxAmnt")), 0, drPromotion("PosMaxAmnt")))
            End If

            'tbs7281 Start
            If trXML_DiscCapAmount.Visible Then
                If drPromotion("PosMaxAmnt").ToString >= "1" And drPromotion("PosMaxAmnt").ToString IsNot Nothing Then
                    chk_DiscCapTickBox.Checked = True
                    txtXML_DiscCap.Visible = True
                    txtXML_DiscCap.Text = drPromotion("PosMaxAmnt").ToString
                Else
                    chk_DiscCapTickBox.Checked = False
                    txtXML_DiscCap.Visible = False
                End If

            End If
            'rbs7281 End

            'mad7740 percent discount
            If txtTPL_percentage.Visible Then
                If txtTPL_percentage.Text = "" Then
                    blistErrorMsg.Items.Add("Percent Discount must be specified.")
                ElseIf Not IsNumeric(txtTPL_percentage.Text) Then
                    blistErrorMsg.Items.Add("Percent Discount is not in a valid numeric format.")
                ElseIf Val(txtTPL_percentage.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Percent Discount must be greater than zero.")
                ElseIf Val(txtTPL_percentage.Text) > 100 Then
                    blistErrorMsg.Items.Add("Percent Discount must not exceed 100%.")
                End If
            End If
        End If
        'end mad7740
        If panClassDiscount.Visible Then
            txtDiscount.Text = drPromotion("PercentDisc").ToString()
        End If

        If panMarkdown.Visible Then
            Dim nPos As Integer = InStr(UCase(drPromotion("PromoDesc").ToString()), UCase(lblMarkdownDesc.Text)) - 1

            If nPos < 0 Then nPos = 0

            txtMarkdown.Text = Left(drPromotion("PromoDesc").ToString(), nPos)

        End If

        If panSMACdeals.Visible Then

            If drPromotion("OriginalValue").ToString() <> "" Then
                txtOrigValue.Text = Format(drPromotion("OriginalValue"), "#,##0.00")
                txtPromoValue.Text = Format(drPromotion("OriginalValue") - drPromotion("DiscAmount"), "#,##0.00")
                txtAllocation.Text = Format(drPromotion("Allocation"), "#,##0")
                txtOnlineSellingStart.Text = drPromotion("OnlineSellingStart")
                txtOnlineSellingEnd.Text = drPromotion("OnlineSellingEnd")
            End If

            'cboVendorType.SelectedValue = rowPromotion.WebAdRate

            If drPromotion("VendorType").ToString() = "O" Then
                cboVendorType.SelectedIndex = 1
            Else
                cboVendorType.SelectedIndex = 2
            End If

        End If

        If panPromoPlanCost.Visible Then
            ' get data from rowPromotion object
            txtPlanPromoSales.Text = Format(drPromotion("PlanPromoSales"), "#,##0.00")
            txtPlanPromoCost.Text = Format(drPromotion("PlanPromoCost"), "#,##0.00")
            txtPlanMargin.Text = Format(drPromotion("PlanPercentMargin"), "##0.00")
        End If

        If panSpecialPromo.Visible Then
            ' get data from rowPromotion object
            txtPurchaseQty.Text = Format(drPromotion("Element1"), "#,##0")
            txtFreeQty.Text = Format(drPromotion("Element2"), "#,##0")
            txtPromoPrice.Text = Format(drPromotion("DiscAmount"), "#,##0.00")
        End If

        If panBuy1Take1.Visible Then
            ' use default value of objects
            'txtB1T1_BuyQty.Text = 1
            'txtB1T1_TakeQty.Text = 1
            'txtB1T1_PercentDisc.Text = rowPromotion.PercentDisc.ToString
        End If

        If panAnyXForP.Visible Then
            ' get data from rowPromotion object
            txtAnyX4P_BuyQty.Text = Format(drPromotion("Element1"), "#,##0")
            txtAnyX4P_PromoPrice.Text = Format(drPromotion("Element3"), "#,##0.00")
        End If

        If panBuyGetSameForP.Visible Then
            ' get data from rowPromotion object
            txtBG4Ps_BuyQty.Text = Format(drPromotion("Element2") - drPromotion("Element1"), "#,##0")
            txtBG4Ps_TakeQty.Text = Format(drPromotion("Element1"), "#,##0")
            txtBG4Ps_DiscAmount.Text = Format(drPromotion("Element3"), "#,##0.00")
        End If

        If panBuyGetDiffForP.Visible Then
            ' get data from rowPromotion object
            txtBG4Pd_BuyQty.Text = Format(drPromotion("Element2"), "#,##0")
            txtBG4Pd_TakeQty.Text = Format(drPromotion("Element1"), "#,##0")
            txtBG4Pd_DiscAmount.Text = Format(drPromotion("Element3"), "#,##0.00")
        End If

        If panBuyGetSamePercent.Visible Then
            ' get data from rowPromotion object
            txtBGPerOffs_BuyQty.Text = Format(CInt(drPromotion("Element2").ToString) - CInt(drPromotion("Element1").ToString), "#,##0")
            txtBGPerOffs_TakeQty.Text = Format(CInt(drPromotion("Element1").ToString), "#,##0")
            txtBGPerOffs_PercentDisc.Text = Format(CSng(drPromotion("Element3").ToString), "##0.00")
        End If

        If panBuyGetDiffPercent.Visible Then
            ' get data from rowPromotion object
            txtBGPerOffd_BuyQty.Text = Format(CInt(drPromotion("Element2").ToString), "#,##0")
            txtBGPerOffd_TakeQty.Text = Format(CInt(drPromotion("Element1").ToString), "#,##0")
            txtBGPerOffd_PercentDisc.Text = Format(CSng(drPromotion("Element3").ToString), "##0.00")
        End If

        If panAnyXYforP.Visible Then
            ' get data from rowPromotion object
            'txtAnyXY_BuyQty.Text = 2
            txtAnyXY_PromoPrice.Text = Format(drPromotion("DiscAmount").ToString, "##0.00")
        End If

        '@@here::PosQualifiedItems
        If trXML_QualifiedItems.Visible Then
            cboXML_QualifiedItems.SelectedValue = drPromotion("PosQualifiedItems").ToString()
        End If

        '@@here::PosPromoSubType
        If trXML_PosPromoSubType.Visible Then
            cboXML_PosPromoSubType.SelectedValue = drPromotion("PosPromoSubType").ToString()
        End If

        '@@here::PromoNotes
        If trTPL_PromoNotes.Visible Then
            litTPLPromoNotes.Text = Server.HtmlDecode(drPromotion("TplPromoNotes").ToString())
        End If

        If panGenericHostXML.Visible Then
            ' retrieve saved promotion values

            'If trXML_Priority.Visible Then cboXML_Priority.SelectedValue = drPromotion("PosPriority")
            'If trXML_CompessID.Visible Then txtXML_CompessID.Text = drPromotion("PosCompressID")

            cboXML_Priority.SelectedValue = drPromotion("PosPriority").ToString
            txtXML_CompessID.Text = drPromotion("PosCompressID").ToString

            ' active days
            If trXML_ActiveDays.Visible Then
                chkMonday.Checked = (InStr(drPromotion("PosActiveDays").ToString, "M") > 0)
                chkTuesday.Checked = (InStr(drPromotion("PosActiveDays").ToString, "T") > 0)
                chkWednesday.Checked = (InStr(drPromotion("PosActiveDays").ToString, "W") > 0)
                chkThursday.Checked = (InStr(drPromotion("PosActiveDays").ToString, "H") > 0)
                chkFriday.Checked = (InStr(drPromotion("PosActiveDays").ToString, "F") > 0)
                chkSaturday.Checked = (InStr(drPromotion("PosActiveDays").ToString, "S") > 0)
                chkSunday.Checked = (InStr(drPromotion("PosActiveDays").ToString, "N") > 0)
            End If

            ' activation time
            'If trXML_ActDeacTime.Visible Then
            '    txtXML_AcTime.Text = drPromotion("PosActime").ToString
            '    txtXML_Deactime.Text = drPromotion("PosDeactime").ToString
            'End If
            If cboXML_StartTime.Visible Then
                cboXML_StartTime.Text = drPromotion("PosActime").ToString
                cboXML_EndTime.Visible = True
                cboXML_EndTime.Text = drPromotion("PosDeactime").ToString
            End If


            txtXML_Descr1prm.Text = drPromotion("PosDescr1prm").ToString
            txtXML_Descr2prm.Text = drPromotion("PosDescr2prm").ToString
            txtXML_ReceiptDesc1.Text = drPromotion("PosInfotext1").ToString
            txtXML_ReceiptDesc2.Text = drPromotion("PosInfotext2").ToString

            cboXML_ActionType.SelectedValue = drPromotion("PosActionType").ToString
            cboXML_ExclusionType.SelectedValue = drPromotion("PosExclType").ToString

            txtXML_MaxAmount.Text = drPromotion("PosMaxAmnt").ToString
            txtXML_MaxQty.Text = drPromotion("PosMaxQty").ToString

            cboXML_ProcessType.SelectedValue = drPromotion("PosProcType").ToString
            cboXML_DiscountType.SelectedValue = drPromotion("PosDiscType").ToString
            cboXML_QualifiedItems.SelectedValue = drPromotion("PosQualifiedItems").ToString
            cboXML_PosPromoSubType.SelectedValue = drPromotion("PosPromoSubType").ToString



            If trXML_CouponMsg.Visible Then
                If Not IsDBNull(drPromotion("PosMessageString")) Then
                    FillCouponMessage(drPromotion("PosMessageString"))
                End If

                'RPR1001 01272020 - eRaffle
                'TYPEDESC='eRaffle'
                If (drPromotion("TypeCategory") = "eRaffle Promo") Then
                    txtXML_CouponMsg01.Enabled = False
                    txtXML_CouponMsg02.Enabled = False
                Else
                    txtXML_CouponMsg01.Enabled = True
                    txtXML_CouponMsg02.Enabled = True
                End If

            End If

            'MALAgasino 20180906 - Use try catch to avoid error
            Try
                InitializeXMLDiscountField(drPromotion("PromoTypeID"), drPromotion("PosDiscType"), CSng(drPromotion("DiscAmount")), CSng(drPromotion("PercentDisc")), ViewState("EligibleCards_STATE"))
                SetDiscConditionDropDownList(cboXML_Condition1)
            Catch ex As Exception

            End Try


            'RBS7281 8/18/2024 - Retrieve data for rebate discount type
            If cboXML_RebateDiscType.Visible = True Then
                GetDiscConDetails(clsSession.PromoTypeID, 1)
            End If

            'MALAgasino 20180910 - Retrieve data for SMAC KIt Price
            If trXML_SMACKitPrice.Visible Then

                txtXML_SMACKitPrice.Text = FormatMoneyField(drPromotion("PosSMACKitPrice"))
            End If

            If trXML_EligibleCards.Visible Then
                FillEligibleCards(drPromotion("PosEligibleCards").ToString)
            End If

            If trXML_MinAmount.Visible Then
                txtXML_MinAmount.Text = FormatMoneyField(IIf(IsDBNull(drPromotion("PosMinAmnt")), 0, drPromotion("PosMinAmnt")))
            End If

            'general coupon :: TODO::HERE
            If trXML_RuleValue2.Visible Then
                txtXML_RuleValue2.Text = Format(IIf(IsDBNull(drPromotion("PosMinAmnt")), 0, drPromotion("PosMinAmnt")), "#,###.00")
            End If

            If trXML_QualifiedCust.Visible Then
                txtQualifiedCust.Text = drPromotion("PosQualifiedCust").ToString
            End If

            If trXML_NameOfPartner.Visible Then
                txtNameOfPartner.Text = drPromotion("PosNameOfPartners").ToString
            End If

            If trXML_ProofOfMembership.Visible Then
                txtProofOfMembership.Text = drPromotion("PosProofOfMembership").ToString
            End If

            If trXML_PartnerEstabGWP.Visible Then
                txtPartnerEstabGWP.Text = drPromotion("PosPartnerEstabGWP").ToString
            End If

        End If

        If panSwipestakes.Visible Then
            LoadPromotionMessage(drPromotion("PromoID"))
            LoadPromoBinRange(drPromotion("RequestID"))
            LoadPromoSeed(drPromotion("RequestID"))

            '@@here::PosQualifiedItems
            If trXML_TenderType.Visible Then
                cboXML_TenderType.SelectedValue = drPromotion("PosTenderType").ToString()
            End If
        End If

        If PanRebate.Visible Then
            LoadRebatePromoBinRange(drPromotion("RequestID"))
        End If

        If panDiscCharging.Visible Then
            LoadPromoShoulderingEntity(clsSession.CurrRequestID)
        End If

        If panBarcode.Visible Then
            If trXML_Barcode.Visible Then
                txtXML_Barcode.Text = drPromotion("PosBarcode").ToString()
            End If
        End If

        litMechanics.Text = Server.HtmlDecode(drPromotion("PromoDesc").ToString)
        'RemoveMechanicsExtendedDetails()
        'lnkEditMechanics.Enabled = True

    End Sub

    Private Sub InitDiscConditionDropDownList(ByRef cboListObj As DropDownList, ByVal nDiscCondValue As Integer)

        Dim drRow As DataRow = Nothing
        Dim strQuery As String = ""

        strQuery = "SELECT ElementName, ElementValue, SubGroupName " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'PosDiscCondition' " & _
                    "AND ElementValue = " & nDiscCondValue & " and IsActive = 1 " & _
                    "ORDER BY SequenceNo"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

            ' loyalty group if SubGroupName is not null
            If Not IsDBNull(drRow("SubGroupName")) Then
                ' dropdown list
                FillResDropDownList(cboListObj, "PosDiscCondition", drRow("SubGroupName"))
            End If

        Else
            ' error
        End If

    End Sub

    Private Sub SetDiscConditionDropDownList(ByRef cboListObj As DropDownList)

        Dim drRow As DataRow = Nothing
        Dim strQuery As String = ""

        strQuery = "SELECT DiscCond, DiscAmnt1 " & _
                    "FROM PromotionConditions " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

            If Not IsDBNull(drRow("DiscCond")) Then
                cboListObj.SelectedValue = drRow("DiscCond")
            End If

        Else
            ' error
        End If

    End Sub

    Private Sub InitializeXMLDiscountField(ByVal PromotionTypeID As Integer, ByVal PosXML_DiscountType As Integer, ByVal nDiscAmount As Single, ByVal nPercentDisc As Single, Optional ByVal POS_EligibleCardsState As Integer = 0)

        trXML_DiscAmount.Visible = False
        trXML_PercentDisc.Visible = False
        trXML_EligibleCards.Visible = (POS_EligibleCardsState >= clsPromo.XMLfieldState.LockedField)

        If PromotionTypeID = 106 Then       ' TODO:: remove hardcoding 115 (UAT)
            lblXML_DiscAmount.Text = "Special Price:"
        Else
            lblXML_DiscAmount.Text = "Amount Discount:"
        End If

        txtXML_DiscAmount.Text = ""
        txtXML_PercentDisc.Text = ""

        Dim bShowDiscountField As Boolean = False

        ' get default value from config
        Dim strQuery As String
        Dim drTypeCondition As DataRow = Nothing

        strQuery = "SELECT * " & _
                    "FROM PromoTypeConditions " & _
                    "WHERE PromoTypeID = 0" & PromotionTypeID

        If Not clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drTypeCondition) Then
            '@HERE: Notify - Error Accessing Database
            Exit Sub
        End If

        'trXML_Condition1.Visible = (drTypeCondition("DiscCond_STATE") >= clsPromo.XMLfieldState.LockedField)

        If drTypeCondition("Condition_STATE") = clsPromo.XMLfieldState.EnabledField Or _
            drTypeCondition("Condition_STATE") = clsPromo.XMLfieldState.LockedField Then

            bShowDiscountField = True

        End If

        Select Case PosXML_DiscountType
            Case 1, 23, 24 ' amount            ' TODO:: remove hardcoding

                trXML_DiscAmount.Visible = bShowDiscountField
                txtXML_DiscAmount.Enabled = (drTypeCondition("Condition_STATE") = clsPromo.XMLfieldState.EnabledField)

                If nDiscAmount = -1 Then
                    txtXML_DiscAmount.Text = drTypeCondition("DiscAmnt1")   ' use default value

                ElseIf nDiscAmount = 0 Then
                    txtXML_DiscAmount.Text = ""

                Else
                    txtXML_DiscAmount.Text = nDiscAmount
                End If

            Case 2, 21, 22  'percentage         ' TODO:: remove hardcoding

                trXML_PercentDisc.Visible = bShowDiscountField
                txtXML_PercentDisc.Enabled = (drTypeCondition("Condition_STATE") = clsPromo.XMLfieldState.EnabledField)

                If nPercentDisc = -1 Then
                    txtXML_PercentDisc.Text = drTypeCondition("DiscAmnt1")   ' use default value

                ElseIf nPercentDisc = 0 Then
                    txtXML_PercentDisc.Text = ""

                Else
                    txtXML_PercentDisc.Text = nPercentDisc
                End If

                'If PromotionTypeID = 223 Then
                '    lblXML_PercentDisc.Text = "Percent Markdown:"
                'End If


        End Select

    End Sub

    Private Sub ClearPromotionDetails()

        lblPopTitle.Value = "Change Promotion"
        clsSession.Message = "Are you sure you want to change the promotion? All information will be deleted."
        clsSession.Icon = "inquiry"
        ViewState("process") = "ChangePromotion"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openConfirmationBox('','');</script>")

    End Sub


    Protected Sub cboPromoType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPromoType.SelectedIndexChanged

        ' confirm chamge in promo type
        ' << popup window >>

        If cboPromoType.SelectedValue <> -1 And IsPostBack And clsSession.CurrPromoID > 0 Then
            'no valid selection
            'hide all

            ClearPromotionDetails()
        Else
        End If

        ' clear fields before setting new ones
        'ClearPromotionDetails()

        ' clear error messages from previous promo type
        blistErrorMsg.Items.Clear()

        DisplayPromoTypeLayout(CInt(cboPromoType.SelectedValue))
        btnDownload_Click(sender, e)
        btnSeedDownload_Click(sender, e)

    End Sub

    Private Sub DisplayPromoTypeLayout(ByVal PromotionTypeID As Integer)
        'trXML_Sponsorship2.Visible = False
        trXML_RebateDiscType.Visible = False '20250910 Hide Rebate/Disc first, and only intended for Rebate Template
        panBankBins.Visible = False 'Mantis#62999 rbs7281: need to initially disable
        PanRebate.Visible = False
        txtXML_DiscCap.Visible = False
        trXML_DiscountType.Visible = False
        panClassDiscount.Visible = False
        panMarkdown.Visible = False
        panSMACdeals.Visible = False
        panMechanics.Visible = False
        panBuy1Take1.Visible = False
        panAnyXForP.Visible = False
        panBuyGetSameForP.Visible = False
        panBuyGetSamePercent.Visible = False
        panBuyGetDiffForP.Visible = False
        panBuyGetDiffPercent.Visible = False
        panAnyXYforP.Visible = False
        panGenericHostXML.Visible = False
        panComboOffer.Visible = False
        panSwipestakes.Visible = False  ' LTE 20201003
        panBarcode.Visible = False      ' LTE 20220504

        panSpecialPromo.Visible = False
        panPromoPlanCost.Visible = False
        'lblPromoAnalytics.visible = false

        panDiscCharging.Visible = False
        trPromoAttachment.Visible = False

        panTemplated.Visible = False

        InitializeTemplatedRows(0)
        trXML_QualifiedItems.Visible = False
        trXML_PosPromoSubType.Visible = False

        lnkBranches.Enabled = True

        If PromotionTypeID = -1 Then
            clsSession.PromoTypeID = 0
        Else

            clsSession.PromoTypeID = PromotionTypeID

            Dim drPromoType As DataRow = Nothing
            Dim strProcessType As String = Nothing
            Dim strQuery As String

            strQuery = "SELECT * " & _
                        "FROM PromoTypes " & _
                        "WHERE PromoTypeID = 0" & PromotionTypeID

            If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drPromoType) Then

                'Get the Process Type based on setup of the promotype
                'and GroupType of the current user
                Select Case drPromoType("GroupType")
                    Case "ALL"
                        If SystemUser.UserGroupType = "REGULAR" Then
                            strProcessType = "PosProcessType"
                        Else
                            strProcessType = "TplProcessType"
                        End If
                    Case "REGULAR"
                        strProcessType = "PosProcessType"
                    Case Else
                        strProcessType = "TplProcessType"
                End Select

                'rbs7281 6/27/2025
                InitializeXMLInputField(trPOS_StdExclusion, chkPOS_StdExclusion, drPromoType("POS_StdExclusion").ToString(), drPromoType("POS_StdExclusion_STATE").ToString())

                InitializeXMLInputField(trXML_DiscCapAmount, chk_DiscCapTickBox, If(String.IsNullOrEmpty(drPromoType("TPL_DiscountCappedChk").ToString()), "0", drPromoType("TPL_DiscountCappedChk").ToString()), If(String.IsNullOrEmpty(drPromoType("TPL_DiscountCappedChk_STATE").ToString()), "0", drPromoType("TPL_DiscountCappedChk_STATE").ToString()))

                Select Case drPromoType("LayoutID")

                    Case 2 'Technical Promo
                        panTemplated.Visible = True
                        lnkEditMechanics.Enabled = True
                        panMechanics.Visible = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))

                    Case 10, 11, 12      ' class discount (regular and with exempt)

                        panClassDiscount.Visible = True

                        txtDiscount.Text = ""
                        lblMechanics.Text = "Description:"

                        lblDiscount.Text = drPromoType("DefaultMechanics")

                    Case 20     ' markdown

                        panMarkdown.Visible = True

                        txtMarkdown.Text = ""
                        lblMechanics.Text = "Description:"

                    Case 30     ' SMAC deals

                        panSMACdeals.Visible = True
                        panMechanics.Visible = True
                        lnkEditMechanics.Enabled = True

                        txtDiscount.Text = ""
                        txtMarkdown.Text = ""
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Description:"
                        lnkattachment.Visible = True
                        lblAttachment.Visible = True

                        'trPromoAttachment.Visible = True

                        'RemoveMechanicsExtendedDetails()

                    Case 110  ' Buy Y, Get X for P_ discount

                        panBuyGetSameForP.Visible = True
                        panMechanics.Visible = True
                        lnkEditMechanics.Enabled = True

                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                    Case 115  ' Buy Y, Get X for P_ discount (diff item)

                        panBuyGetDiffForP.Visible = True
                        panMechanics.Visible = True
                        lnkEditMechanics.Enabled = True

                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                    Case 120  ' buy 1 take 1

                        panBuy1Take1.Visible = True
                        panMechanics.Visible = True

                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                    Case 130  ' Any X for P

                        panAnyXForP.Visible = True
                        panMechanics.Visible = True
                        panTemplated.Visible = True
                        panGenericHostXML.Visible = True
                        panComboOffer.Visible = True

                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                        'GenericHostXML
                        trXML_Descr1prm.Visible = False
                        trXML_Descr2prm.Visible = False
                        trXML_ReceiptDesc1.Visible = False
                        trXML_ReceiptDesc2.Visible = False
                        trXML_ActionType.Visible = False
                        trXML_DiscountType.Visible = False
                        trXML_ProcessType.Visible = False
                        trXML_MaxQty.Visible = False
                        trXML_MaxAmount.Visible = False
                        trXML_MinAmount.Visible = False
                        trXML_CompessID.Visible = False
                        trXML_ExclusionType.Visible = False
                        trXML_ActDeacTime.Visible = False
                        trXML_ActiveDays.Visible = False
                        trXML_Priority.Visible = False
                        trXML_RuleValue1.Visible = False
                        trXML_RuleValue2.Visible = False
                        trXML_RuleValue3.Visible = False
                        trXML_RuleValue4.Visible = False
                        trXML_Condition1.Visible = False
                        trXML_EligibleCards.Visible = False
                        trXML_DiscAmount.Visible = False
                        trXML_PercentDisc.Visible = False
                        trXML_SMACKitPrice.Visible = False
                        trXML_CouponMsg.Visible = False

                        'Templated Fields
                        trTPL_PurchaseReq.Visible = False
                        trTPL_BuyQty.Visible = False
                        trTPL_TakeQty.Visible = False
                        trTPL_DiscAmount.Visible = False
                        trTPL_PercentDisc.Visible = False
                        trTPL_ReqAmount.Visible = False
                        trTPL_NumMonths.Visible = False
                        trTPL_BonusPoints.Visible = False
                        trTPL_BankList.Visible = False
                        trTpl_SubsidyRate.Visible = False
                        trTPL_BrandNames.Visible = False
                        trTPL_CelebName.Visible = False
                        trTPL_ItemName.Visible = False
                        trTPL_DeptName.Visible = False
                        trTPL_ActivityName.Visible = False
                        trTPL_SellingArea.Visible = False
                        trTPL_EventTime.Visible = False
                        trTPL_TransDesc.Visible = False   'NBS20190822
                        trTPL_FreeItems.Visible = False
                        trTPL_Prizes.Visible = False
                        trTPL_RefMemo.Visible = False
                        trTPL_ProcessType.Visible = False
                        trTPL_MinAmount.Visible = False ' mad7740
                        trTPL_discountcapping.Visible = False 'mad7740    

                        trPOS_StdExclusion.Visible = False
                        trPOS_PermExclusion.Visible = False

                        'Combo Offer
                        trXML_QualifiedCust.Visible = False
                        trXML_NameOfPartner.Visible = False
                        trXML_ProofOfMembership.Visible = False
                        trXML_PartnerEstabGWP.Visible = False

                        If (drPromoType("POS_EligibleCards_STATE") <> 0) Then
                            InitializeXMLDiscountField(drPromoType("PromoTypeID"), 0, 0, 0, drPromoType("POS_EligibleCards_STATE"))
                        End If
                        'If (drPromoType("TPL_ProcessType_STATE") <> 0) Then
                        '    InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), strProcessType)
                        '    'InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), "TplProcessType")
                        'End If
                        InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), strProcessType)

                        If (drPromoType("POS_QualifiedCust_STATE") <> 0) Then
                            InitializeXMLInputField(trXML_QualifiedCust, txtQualifiedCust, drPromoType("POS_QualifiedCust").ToString(), drPromoType("POS_QualifiedCust_STATE"))
                        End If



                    Case 140  ' Buy Y, Get X at % discount

                        panBuyGetSamePercent.Visible = True
                        panMechanics.Visible = True

                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                    Case 145  ' Buy Y, Get X at % discount (diff item)

                        panBuyGetDiffPercent.Visible = True
                        panMechanics.Visible = True

                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                    Case 150  ' Any XY for P_ amount

                        panAnyXYforP.Visible = True
                        panMechanics.Visible = True

                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                    Case 200   ' generic types for HOST XML

                        panGenericHostXML.Visible = True
                        panMechanics.Visible = True
                        panComboOffer.Visible = True


                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                        ' show custom settings

                        If SystemUser.UserGroupType = "SBU" Then
                            InitializeXMLInputField(trXML_Priority, cboXML_Priority, Convert.ToInt32(drPromoType("POS_Priority_SBU")), drPromoType("POS_Priority_SBU_STATE").ToString(), "PosPriority")
                        Else
                            InitializeXMLInputField(trXML_Priority, cboXML_Priority, drPromoType("POS_Priority"), drPromoType("POS_Priority_STATE"), "PosPriority")
                        End If

                        InitializeXMLInputField(trXML_CompessID, txtXML_CompessID, drPromoType("POS_CompressID"), drPromoType("POS_CompressID_STATE"))

                        ' active days
                        InitializeXMLInputField(trXML_ActiveDays, chkMonday, InStr(drPromoType("POS_Activedays"), "M") > 0, drPromoType("POS_Activedays_STATE"))
                        InitializeXMLInputField(trXML_ActiveDays, chkTuesday, InStr(drPromoType("POS_Activedays"), "T") > 0, drPromoType("POS_Activedays_STATE"))
                        InitializeXMLInputField(trXML_ActiveDays, chkWednesday, InStr(drPromoType("POS_Activedays"), "W") > 0, drPromoType("POS_Activedays_STATE"))
                        InitializeXMLInputField(trXML_ActiveDays, chkThursday, InStr(drPromoType("POS_Activedays"), "H") > 0, drPromoType("POS_Activedays_STATE"))
                        InitializeXMLInputField(trXML_ActiveDays, chkFriday, InStr(drPromoType("POS_Activedays"), "F") > 0, drPromoType("POS_Activedays_STATE"))
                        InitializeXMLInputField(trXML_ActiveDays, chkSaturday, InStr(drPromoType("POS_Activedays"), "S") > 0, drPromoType("POS_Activedays_STATE"))
                        InitializeXMLInputField(trXML_ActiveDays, chkSunday, InStr(drPromoType("POS_Activedays"), "N") > 0, drPromoType("POS_Activedays_STATE"))

                        'MALAgasino 20181001 - Change Activity time to dropdown selection
                        'InitializeXMLInputField(trXML_ActDeacTime, txtXML_AcTime, drPromoType("POS_Actime"), drPromoType("POS_Actime_STATE"))
                        'InitializeXMLInputField(trXML_ActDeacTime, txtXML_Deactime, drPromoType("POS_Deactime"), drPromoType("POS_Actime_STATE"))
                        'InitializeXMLInputField(trXML_ActDeacTime, cboXML_StartTime, drPromoType("POS_Actime"), drPromoType("POS_Actime_STATE"))
                        'InitializeXMLInputField(trXML_ActDeacTime, cboXML_EndTime, drPromoType("POS_Deactime"), drPromoType("POS_Actime_STATE"))
                        If drPromoType("POS_Actime_STATE") >= 20 Then
                            trXML_ActDeacTime.Visible = True
                            cboXML_StartTime.Visible = True
                            cboXML_EndTime.Visible = True
                            FillStartTime()
                            FillEndTime()
                            cboXML_EndTime.SelectedIndex = cboXML_EndTime.Items.Count - 1 'confirmed with mpd for end time to default to 23:59 rbs7281 20250806 
                        Else
                            trXML_ActDeacTime.Visible = False
                        End If

                        InitializeXMLInputField(trXML_Descr1prm, txtXML_Descr1prm, drPromoType("POS_Descr1prm"), drPromoType("POS_Descr1prm_STATE"))
                        InitializeXMLInputField(trXML_Descr2prm, txtXML_Descr2prm, drPromoType("POS_Descr2prm"), drPromoType("POS_Descr2prm_STATE"))
                        InitializeXMLInputField(trXML_ReceiptDesc1, txtXML_ReceiptDesc1, drPromoType("POS_InfoText1"), drPromoType("POS_InfoText1_STATE"))
                        InitializeXMLInputField(trXML_ReceiptDesc2, txtXML_ReceiptDesc2, drPromoType("POS_InfoText2"), drPromoType("POS_InfoText2_STATE"))

                        InitializeXMLInputField(trXML_ActionType, cboXML_ActionType, drPromoType("POS_ActionType"), drPromoType("POS_ActionType_STATE"), "PosActionType")
                        InitializeXMLInputField(trXML_ExclusionType, cboXML_ExclusionType, drPromoType("POS_ExclType"), drPromoType("POS_ExclType_STATE"), "PosPromoExclType")

                        InitializeXMLInputField(trXML_MaxAmount, txtXML_MaxAmount, drPromoType("POS_MaxAmnt"), drPromoType("POS_MaxAmnt_STATE"))


                        InitializeXMLInputField(trXML_MaxQty, txtXML_MaxQty, drPromoType("POS_MaxQty"), drPromoType("POS_MaxQty_STATE"))
                        InitializeXMLInputField(trXML_MinAmount, txtXML_MinAmount, drPromoType("POS_MinAmnt"), drPromoType("POS_MinAmnt_STATE"))

                        InitializeXMLInputField(trXML_ProcessType, cboXML_ProcessType, drPromoType("POS_ProcType"), drPromoType("POS_ProcType_STATE"), strProcessType)
                        InitializeXMLInputField(trXML_DiscountType, cboXML_DiscountType, drPromoType("POS_DiscType"), drPromoType("POS_DiscType_STATE"), "PosDiscType")

                        'MALAgasino 20180910 - Exclude "Selected Items Only" if Group type = BCR
                        'Start of Code
                        If (drPromoType("GroupType") = "SBU") Then
                            InitializeXMLInputField(trXML_QualifiedItems, cboXML_QualifiedItems, drPromoType("POS_QualifiedItems"), drPromoType("POS_QualifiedItems_STATE"), "PosQualifiedItems", 3)
                        Else
                            'mantis 67410 RBS7281 [Qualified Items] "Selected Items" available to SBU User
                            If SystemUser.UserGroupType = "SBU" Then
                                InitializeXMLInputField(trXML_QualifiedItems, cboXML_QualifiedItems, drPromoType("POS_QualifiedItems"), drPromoType("POS_QualifiedItems_STATE"), "PosQualifiedItems", 3)
                            Else
                                InitializeXMLInputField(trXML_QualifiedItems, cboXML_QualifiedItems, drPromoType("POS_QualifiedItems"), drPromoType("POS_QualifiedItems_STATE"), "PosQualifiedItems")
                            End If
                            'Original Code

                        End If
                        'End of Code
                        InitializeXMLInputField(trXML_PosPromoSubType, cboXML_PosPromoSubType, drPromoType("POS_PromoSubType"), drPromoType("POS_PromoSubType_STATE"), "PosPromoSubType")

                        ViewState("POS_MessageString_STATE") = drPromoType("POS_MessageString_STATE")
                        InitializeXMLInputField(trXML_CouponMsg, txtXML_CouponMsg01, "", ViewState("POS_MessageString_STATE"))

                        'RPR1001 01272020 - eRaffle
                        'TYPEDESC='eRaffle'
                        If (drPromoType("TypeDesc") = "eRaffle") Then
                            txtXML_CouponMsg01.Text = "CONGRATULATIONS!"
                            txtXML_CouponMsg02.Text = "YOU HAVE #NUMBER RAFFLE ENTRY/ENTRIES"

                            txtXML_CouponMsg01.Enabled = False
                            txtXML_CouponMsg02.Enabled = False
                        Else
                            txtXML_CouponMsg01.Text = ""
                            txtXML_CouponMsg02.Text = ""

                            txtXML_CouponMsg01.Enabled = True
                            txtXML_CouponMsg02.Enabled = True
                        End If

                        InitializeXMLDiscountField(PromotionTypeID, drPromoType("POS_DiscType").ToString, -1, -1, drPromoType("POS_EligibleCards_STATE"))
                        ViewState("EligibleCards_STATE") = drPromoType("POS_EligibleCards_STATE")

                        'If (drPromoType("TypeCategory") = "Sale Event") Then
                        '    lblXML_PercentDisc.Text = "Percent Markdown:"
                        'Else
                        '    lblXML_PercentDisc.Text = "Percent Discount:"
                        'End If

                        InitializeXMLRuleValueFields(PromotionTypeID)

                        'InitializeXMLInputField(trXML_EligibleCards, chkXML_eCardAll, InStr(drPromoType("POS_EligibleCards").ToString(), "00;") > 0, drPromoType("POS_EligibleCards_STATE"))

                        'MALAgasino 20180906 - Initialize CompSponsorship. Code below.
                        If (drPromoType("GroupType") = "SBU") Then
                            If drPromoType("CompSponsorship_Mode").ToString() = 2 Then
                                InitializeXMLInputField(trXML_CompSponsorship2, cboCompSponsorship, drPromoType("CompSponsorship_Value"), drPromoType("CompSponsorship_STATE"), "CompSponsorship")
                            Else
                                InitializeXMLInputField(trXML_CompSponsorship, linkShoulderingEntity, drPromoType("CompSponsorship_Value"), drPromoType("CompSponsorship_STATE"), "CompSponsorship")
                            End If

                            'InitializeXMLInputField(trXML_Sponsorship2, linkShoulderingEntity, drPromoType("CompSponsorship_Value"), drPromoType("CompSponsorship_STATE"), "CompSponsorship")

                        End If
                        'InitializeXMLInputField(trXML_DiscCondition, cboXML_DiscCondition, drPromoType("POS_DiscCondition"), drPromoType("POS_DiscCondition_STATE"), "PosDiscCondition")
                        'InitializeXMLInputField(trXML_DiscAmount, txtXML_DiscAmount, drPromoType("POS_DiscAmount1"), drPromoType("POS_DiscAmount1_STATE"))

                        'trXML_PercentDisc.Visible = (drPromoType("POS_PercentDisc_STATE") > 0)
                        'txtXML_PercentDisc.Text = drPromoType("POS_PercentDisc")

                        'MALAgasino 20180910 - If UPC Level = 1, allow adding promo details. As per new SBU Template requirement,
                        'rbs7281 Except SBU Templates with Coupon, Other SBU template must not have Promo Details with Item Selection.
                        'Start of Code
                        Dim allowDetails As Boolean = (drPromoType("IsUPCLevel") = True) Or _
                                                      (drPromoType("POS_AllowCoupons") = 1) Or _
                                                      (drPromoType("POS_AllowDeptCodes") = 1)

                        If drPromoType("GroupType") = "ALL" Then 'rbs7281 8/13/2025 retain the existing logic if not ALL

                            If allowDetails Then
                                If SystemUser.UserGroupType <> "SBU" Then
                                    trPromoDetails.Visible = True
                                    lnkBranches.Enabled = False
                                ElseIf drPromoType("PromoTypeID") = "288" Or drPromoType("POS_AllowCoupons") = 1 Then
                                    trPromoDetails.Visible = True
                                    lnkBranches.Enabled = False
                                Else
                                    trPromoDetails.Visible = False
                                    lnkBranches.Enabled = True
                                End If
                            Else
                                If SystemUser.UserGroupType <> "SBU" Then
                                    trPromoDetails.Visible = True
                                    lnkBranches.Enabled = False
                                Else
                                    trPromoDetails.Visible = False
                                    lnkBranches.Enabled = True
                                End If
                            End If
                        Else
                            If allowDetails Then
                                trPromoDetails.Visible = True
                                lnkBranches.Enabled = False
                            Else
                                trPromoDetails.Visible = False
                                lnkBranches.Enabled = True
                            End If

                        End If



                        'End of Code

                        'MALAgasino 20180910 - Add SMAC Kit Price for SBU. Start of CodeTry
                        InitializeXMLInputField(trXML_SMACKitPrice, txtXML_SMACKitPrice, drPromoType("POS_SMACKitPrice"), drPromoType("POS_SMACKitPrice_STATE"))
                        'MALAgasino 20180910 - Add SMAC Kit Price for SBU. End of Code


                        'MALAgasino 20180911 - Add fields for Partners of the Month Category
                        InitializeXMLInputField(trXML_QualifiedCust, txtQualifiedCust, drPromoType("POS_QualifiedCust").ToString(), drPromoType("POS_QualifiedCust_STATE"))
                        InitializeXMLInputField(trXML_NameOfPartner, txtNameOfPartner, drPromoType("POS_NameOfPartners").ToString(), drPromoType("POS_NameOfPartners_STATE"))
                        InitializeXMLInputField(trXML_ProofOfMembership, txtProofOfMembership, drPromoType("POS_ProofOfMembership").ToString(), drPromoType("POS_ProofOfMembership_STATE"))
                        InitializeXMLInputField(trXML_PartnerEstabGWP, txtPartnerEstabGWP, drPromoType("POS_PartnerEstabGWP").ToString(), drPromoType("POS_PartnerEstabGWP_STATE"))

                        ViewState("IsEligibleCardsRequired") = drPromoType("IsEligibleCardsRequired")

                        'InitializeXMLInputField(trXML_PercentDisc, txtXML_PercentDisc, If(String.IsNullOrEmpty(drPromoType("TPL_PercentDisc").ToString()), "0", drPromoType("TPL_PercentDisc").ToString()), If(String.IsNullOrEmpty(drPromoType("TPL_PercentDisc_STATE").ToString()), "0", drPromoType("TPL_PercentDisc_STATE").ToString()))

                        If PromotionTypeID = 59 Then
                            trXML_PercentDisc.Visible = False
                            If cboXML_RebateDiscType.SelectedValue = "1" Then
                                trXML_DiscCapAmount.Visible = False
                            ElseIf cboXML_RebateDiscType.SelectedValue = "2" Then
                                trXML_DiscCapAmount.Visible = True
                            Else
                                trXML_DiscCapAmount.Visible = False
                            End If
                            PanRebate.Visible = True
                            FillRebateDiscTypeList()
                            GetDiscConDetails(clsSession.PromoTypeID, 1)
                            LoadRebatePromoBinRange(clsSession.CurrRequestID)
                        End If

                    Case 290
                        '' LTE 20201003 - Remove mechanics if PromoType is in array (290 = Swipestakes)
                        panSwipestakes.Visible = IIf(Array.IndexOf(New Integer() {290}, PromotionTypeID) >= 0, True, False)
                        panGenericHostXML.Visible = IIf(Array.IndexOf(New Integer() {290}, PromotionTypeID) >= 0, True, False)

                        panMechanics.Visible = True
                        InitializeXMLInputField(trXML_QualifiedItems, cboXML_QualifiedItems, drPromoType("POS_QualifiedItems"), drPromoType("POS_QualifiedItems_STATE"), "PosQualifiedItems")
                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                        DisablePanelFields()

                        '' Barcode
                        panBarcode.Visible = True
                        InitializeXMLInputField(trXML_Barcode, panBarcode, drPromoType("POS_Barcode"), drPromoType("POS_Barcode_STATE"))


                        '' Swipestakes
                        trXML_EligibleCards.Visible = True
                        ViewState("IsEligibleCardsRequired") = drPromoType("IsEligibleCardsRequired")

                        InitializeXMLInputField(trXML_TenderType, cboXML_TenderType, drPromoType("POS_TenderType"), drPromoType("POS_TenderType_STATE"), "PosTenderType")
                        InitializeXMLInputField(trXML_MinAmount, txtXML_MinAmount, drPromoType("POS_MinAmnt"), drPromoType("POS_MinAmnt_STATE"))

                        If Array.IndexOf(New Integer() {290}, PromotionTypeID) >= 0 Then
                            LoadPromoBinRange(clsSession.CurrRequestID)
                            LoadPromoSeed(clsSession.CurrRequestID)

                            trPromoSeedAttachment.Visible = True
                            linkSeedAttachment.Visible = True
                        End If



                    Case 300 'MALAgasino 20180102 - Layout ID that will depend on the STATE column of fields.
                        ' @NBS:HERE: no existing PromoType using this layoutID

                        'litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        'lblMechanics.Text = "Mechanics:"
                        'HideTemplatedRows()

                        ''Combo Offer
                        'InitializeXMLInputField(trXML_QualifiedCust, txtQualifiedCust, drPromoType("POS_QualifiedCust").ToString(), drPromoType("POS_QualifiedCust_STATE"))
                        'InitializeXMLInputField(trXML_NameOfPartner, txtNameOfPartner, drPromoType("POS_NameOfPartners").ToString(), drPromoType("POS_NameOfPartners_STATE"))
                        'InitializeXMLInputField(trXML_ProofOfMembership, txtProofOfMembership, drPromoType("POS_ProofOfMembership").ToString(), drPromoType("POS_ProofOfMembership_STATE"))
                        'InitializeXMLInputField(trXML_PartnerEstabGWP, txtPartnerEstabGWP, drPromoType("POS_PartnerEstabGWP").ToString(), drPromoType("POS_PartnerEstabGWP_STATE"))

                        ''Templated fields
                        'InitializeXMLInputField(trTPL_PurchaseReq, cboTPL_PurchaseReq, drPromoType("TPL_PurchaseReq"), drPromoType("TPL_PurchaseReq_STATE"), "TplPurchaseReq", drPromoType("TPL_PurchaseReq_ExList").ToString())
                        'InitializeXMLInputField(trTPL_BuyQty, txtTPL_BuyQty, drPromoType("TPL_BuyQty"), drPromoType("TPL_BuyQty_STATE"))
                        'InitializeXMLInputField(trTPL_TakeQty, txtTPL_TakeQty, drPromoType("TPL_TakeQty"), drPromoType("TPL_TakeQty_STATE"))
                        ''TPL Disc Amount
                        ''TPL Percent Disc
                        'InitializeXMLInputField(trTPL_ReqAmount, txtTPL_ReqAmount, drPromoType("TPL_ReqAmount"), drPromoType("TPL_ReqAmount_STATE"))
                        'InitializeXMLInputField(trTPL_NumMonths, txtTPL_NumMonths, drPromoType("TPL_NumMonths"), drPromoType("TPL_NumMonths_STATE"))
                        'InitializeXMLInputField(trTPL_BankList, txtTPL_BankList, drPromoType("TPL_BankList"), drPromoType("TPL_BankList_STATE"))
                        'InitializeXMLInputField(trTPL_BrandNames, txtTPL_BrandNames, drPromoType("TPL_BrandNames"), drPromoType("TPL_BrandNames_STATE"))
                        'InitializeXMLInputField(trTPL_CelebName, txtTPL_CelebName, drPromoType("TPL_CelebName"), drPromoType("TPL_CelebName_STATE"))
                        'InitializeXMLInputField(trTPL_ItemName, txtTPL_ItemName, drPromoType("TPL_BrandNames"), drPromoType("TPL_BrandNames_STATE"))
                        'InitializeXMLInputField(trTPL_DeptName, txtTPL_DeptName, drPromoType("TPL_DeptName"), drPromoType("TPL_DeptName_STATE"))
                        'InitializeXMLInputField(trTPL_ActivityName, txtTPL_ActivityName, drPromoType("TPL_ActivityName"), drPromoType("TPL_ActivityName_STATE"))
                        'InitializeXMLInputField(trTPL_SellingArea, txtTPL_SellingArea, drPromoType("TPL_SellingArea"), drPromoType("TPL_SellingArea_STATE"))
                        'InitializeXMLInputField(trTPL_EventTime, txtTPL_EventTime, drPromoType("TPL_EventTime"), drPromoType("TPL_EventTime_STATE"))

                        ''NBS20190822
                        'InitializeXMLInputField(trTPL_TransDesc, txtTPL_TransDesc, drPromoType("TPL_TransDesc"), drPromoType("TPL_TransDesc_STATE"))

                        'InitializeXMLInputField(trTPL_FreeItems, txtTPL_FreeItems, drPromoType("TPL_FreeItems"), drPromoType("TPL_FreeItems_STATE"))
                        'InitializeXMLInputField(trTPL_Prizes, txtTPL_Prizes, drPromoType("TPL_Prizes"), drPromoType("TPL_Prizes_STATE"))
                        'InitializeXMLInputField(trTPL_RefMemo, txtTPL_RefMemo, drPromoType("TPL_RefMemo"), drPromoType("TPL_RefMemo_STATE"))
                        'InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), "TplProcessType")

                        ''Generic Host Fields
                        'InitializeXMLInputField(trXML_Descr1prm, txtXML_Descr1prm, drPromoType("POS_Descr1prm"), drPromoType("POS_Descr1prm_STATE"))
                        'InitializeXMLInputField(trXML_Descr2prm, txtXML_Descr2prm, drPromoType("POS_Descr2prm"), drPromoType("POS_Descr2prm_STATE"))
                        'InitializeXMLInputField(trXML_ReceiptDesc1, txtXML_ReceiptDesc1, drPromoType("POS_InfoText1"), drPromoType("POS_InfoText1_STATE"))
                        'InitializeXMLInputField(trXML_ReceiptDesc2, txtXML_ReceiptDesc2, drPromoType("POS_InfoText2"), drPromoType("POS_InfoText2_STATE"))
                        'InitializeXMLInputField(trXML_ActionType, cboXML_ActionType, drPromoType("POS_ActionType"), drPromoType("POS_ActionType_STATE"), "PosActionType")
                        'InitializeXMLInputField(trXML_DiscountType, cboXML_DiscountType, drPromoType("POS_DiscType"), drPromoType("POS_DiscType_STATE"), "PosDiscType")
                        'InitializeXMLInputField(trXML_ProcessType, cboXML_ProcessType, drPromoType("POS_ProcType"), drPromoType("POS_ProcType_STATE"), "PosProcessType")
                        'InitializeXMLInputField(trXML_MaxQty, txtXML_MaxQty, drPromoType("POS_MaxQty"), drPromoType("POS_MaxQty_STATE"))
                        'InitializeXMLInputField(trXML_MaxAmount, txtXML_MaxAmount, drPromoType("POS_MaxAmnt"), drPromoType("POS_MaxAmnt_STATE"))
                        'InitializeXMLInputField(trXML_MinAmount, txtXML_MinAmount, drPromoType("POS_MinAmnt"), drPromoType("POS_MinAmnt_STATE"))
                        'InitializeXMLInputField(trXML_CompessID, txtXML_CompessID, drPromoType("POS_CompressID"), drPromoType("POS_CompressID_STATE"))
                        'InitializeXMLInputField(trXML_ExclusionType, cboXML_ExclusionType, drPromoType("POS_ExclType"), drPromoType("POS_ExclType_STATE"), "PosPromoExclType")
                        'If drPromoType("POS_Actime_STATE") >= 20 Then
                        '    trXML_ActDeacTime.Visible = True
                        '    cboXML_StartTime.Visible = True
                        '    cboXML_EndTime.Visible = True
                        '    FillStartTime()
                        '    FillEndTime()
                        'Else
                        '    trXML_ActDeacTime.Visible = False
                        'End If
                        'InitializeXMLInputField(trXML_ActiveDays, chkMonday, InStr(drPromoType("POS_Activedays"), "M") > 0, drPromoType("POS_Activedays_STATE"))
                        'InitializeXMLInputField(trXML_ActiveDays, chkTuesday, InStr(drPromoType("POS_Activedays"), "T") > 0, drPromoType("POS_Activedays_STATE"))
                        'InitializeXMLInputField(trXML_ActiveDays, chkWednesday, InStr(drPromoType("POS_Activedays"), "W") > 0, drPromoType("POS_Activedays_STATE"))
                        'InitializeXMLInputField(trXML_ActiveDays, chkThursday, InStr(drPromoType("POS_Activedays"), "H") > 0, drPromoType("POS_Activedays_STATE"))
                        'InitializeXMLInputField(trXML_ActiveDays, chkFriday, InStr(drPromoType("POS_Activedays"), "F") > 0, drPromoType("POS_Activedays_STATE"))
                        'InitializeXMLInputField(trXML_ActiveDays, chkSaturday, InStr(drPromoType("POS_Activedays"), "S") > 0, drPromoType("POS_Activedays_STATE"))
                        'InitializeXMLInputField(trXML_ActiveDays, chkSunday, InStr(drPromoType("POS_Activedays"), "N") > 0, drPromoType("POS_Activedays_STATE"))
                        'InitializeXMLInputField(trXML_Priority, cboXML_Priority, drPromoType("POS_Priority"), drPromoType("POS_Priority_STATE"), "PosPriority")
                        'InitializeXMLRuleValueFields(PromotionTypeID)
                        'InitializeXMLDiscountField(PromotionTypeID, drPromoType("POS_DiscType").ToString, -1, -1, drPromoType("POS_EligibleCards_STATE"))
                        'InitializeXMLInputField(trXML_CompSponsorship, cboCompSponsorship, drPromoType("CompSponsorship_Value"), drPromoType("CompSponsorship_STATE"), "CompSponsorship")
                        'ViewState("EligibleCards_STATE") = drPromoType("POS_EligibleCards_STATE")
                        'If (drPromoType("GroupType") = "SBU") Then
                        '    InitializeXMLInputField(trXML_QualifiedItems, cboXML_QualifiedItems, drPromoType("POS_QualifiedItems"), drPromoType("POS_QualifiedItems_STATE"), "PosQualifiedItems", 3)
                        'Else
                        '    'Original Code
                        '    InitializeXMLInputField(trXML_QualifiedItems, cboXML_QualifiedItems, drPromoType("POS_QualifiedItems"), drPromoType("POS_QualifiedItems_STATE"), "PosQualifiedItems")
                        'End If

                        'ViewState("v_AllowAttachment") = CBool(drPromoType("AllowAttachment"))
                        'ViewState("v_RequireAttachment") = CBool(drPromoType("RequireAttachment"))

                        'If (ViewState("v_AllowAttachment") Or ViewState("v_RequireAttachment")) Then
                        '    Dim f As New IO.FileInfo(clsPromo.pathAttachment & clsSession.CurrRequestID.ToString())

                        '    If Not f.Exists Then
                        '        clsSession.AttachmentPath = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString()
                        '        Directory.CreateDirectory(clsSession.AttachmentPath)
                        '    End If

                        '    ' attachment
                        '    trPromoAttachment.Visible = (CBool(drPromoType("AllowAttachment")) Or CBool(drPromoType("RequireAttachment")))
                        '    ViewState("AllowAttachement") = CBool(drPromoType("AllowAttachment"))
                        '    lnkattachment.Visible = True

                        'End If

                    Case 400 'For Watsons Specials
                        panGenericHostXML.Visible = True
                        panMechanics.Visible = True

                        lnkEditMechanics.Enabled = False
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                        trXML_Descr1prm.Visible = False
                        trXML_Descr2prm.Visible = False
                        trXML_ReceiptDesc1.Visible = False
                        trXML_ReceiptDesc2.Visible = False
                        trXML_ActionType.Visible = False
                        trXML_DiscountType.Visible = False
                        trXML_ProcessType.Visible = False
                        trXML_MaxQty.Visible = False
                        trXML_MaxAmount.Visible = False
                        trXML_MinAmount.Visible = False
                        trXML_CompessID.Visible = False
                        trXML_ExclusionType.Visible = False
                        trXML_ActDeacTime.Visible = False
                        trXML_ActiveDays.Visible = False
                        trXML_Priority.Visible = False
                        trXML_RuleValue1.Visible = False
                        trXML_RuleValue2.Visible = False
                        trXML_RuleValue3.Visible = False
                        trXML_RuleValue4.Visible = False
                        trXML_Condition1.Visible = False
                        trXML_EligibleCards.Visible = False
                        trXML_DiscAmount.Visible = False
                        trXML_PercentDisc.Visible = False
                        trXML_SMACKitPrice.Visible = False
                        trXML_CouponMsg.Visible = False

                        InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), strProcessType)
                        InitializeXMLInputField(trXML_MinAmount, txtXML_MinAmount, drPromoType("POS_MinAmnt"), drPromoType("POS_MinAmnt_STATE"))

                    Case Else   ' all others

                        panMechanics.Visible = True

                        'InitializeXMLInputField(trXML_QualifiedItems, cboXML_QualifiedItems, drPromoType("POS_QualifiedItems"), 30)

                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drPromoType("DefaultMechanics"))
                        lblMechanics.Text = "Mechanics:"

                        ' mad7740
                        If PromotionTypeID = 287 Then
                            trTPL_ProcessType.Visible = True
                            FillPromoApplicationList()
                            trTPL_MinAmount.Visible = True
                            'InitializeXMLInputField(trTPL_MinAmount, txtTPL_MinAmount, drPromoType("POS_MinAmnt"), drPromoType("POS_MinAmnt_STATE"))
                            InitializeXMLInputField(trTPL_MinAmount, txtTPL_MinAmount, drPromoType("POS_MinAmnt"), 30)
                            trTPL_discountcapping.Visible = True
                            InitializeXMLInputField(trTPL_discountcapping, txtTPL_discountcapping, drPromoType("POS_MinAmnt"), 30)
                            FillQualifiedItemList()
                            FillCardsGrid()
                            FillShoulderingEntityList()
                            trTPL_percentage.Visible = True
                            trXML_Sponsorship.Visible = True
                            panCommonFields.Visible = True
                            trXML_QualifiedItems.Visible = True
                            btnShowBankBins.Visible = True
                            panBankBins.Visible = True 'Mantis #62999 rbs7281 panel must show 
                        End If

                        'rbs7281 8/27/2025
                        panMechanics.Visible = True
                        Dim objContainerTableRow As HtmlTableRow = trXML_QualifiedItems
                        Dim objInputField As Object = cboXML_QualifiedItems
                        InitializeXMLInputField(objContainerTableRow, objInputField, drPromoType("POS_QualifiedItems").ToString, drPromoType("POS_QualifiedItems_STATE").ToString, "PosQualifiedItems")
                        cboXML_QualifiedItems = CType(objInputField, DropDownList)
                        trXML_QualifiedItems = objContainerTableRow


                        'cboXML_QualifiedItems.SelectedValue = drPromotion("PosQualifiedItems").ToString
                        'txtXML_PercentDisc.Enabled = (drTypeCondition("Condition_STATE") = clsPromo.XMLfieldState.EnabledField)

                End Select

                '-----------------------------------------------------
                ' options for templated promotions
                '-----------------------------------------------------
                If drPromoType("IsTemplated") = 1 Then
                    panTemplated.Visible = True

                    If drPromoType("LayoutID") <> 2 Then
                        lnkEditMechanics.Enabled = False
                    End If


                    'If SystemUser.UserLevel.ToString() = "80" Then '3-014 rbs7281 editable for PromoRequestor
                    '    lnkEditMechanics.Enabled = True
                    'End If
                Else
                    panTemplated.Visible = False
                End If

                ' months
                If Not IsDBNull(drPromoType("TPL_NumMonths_STATE")) Then
                    trTPL_NumMonths.Visible = (drPromoType("TPL_NumMonths_STATE") = 30)
                End If

                ' banks
                If Not IsDBNull(drPromoType("TPL_BankList_STATE")) Then
                    trTPL_BankList.Visible = (drPromoType("TPL_BankList_STATE") = 30)
                End If

                ' subsidy rate
                If Not IsDBNull(drPromoType("TPL_SubsidyRate_STATE")) Then
                    trTpl_SubsidyRate.Visible = (drPromoType("TPL_SubsidyRate_STATE") = 30)
                End If

                ' brands
                If Not IsDBNull(drPromoType("TPL_BrandNames_STATE")) Then
                    trTPL_BrandNames.Visible = (drPromoType("TPL_BrandNames_STATE") = 30)
                End If

                If Not IsDBNull(drPromoType("TPL_ReqAmount_STATE")) Then
                    If (drPromoType("TPL_ReqAmount_STATE") = 30) Then
                        trTPL_ReqAmount.Visible = True
                        txtTPL_ReqAmount.Enabled = True
                        txtTPL_ReqAmount.Text = Format(drPromoType("TPL_ReqAmount"), "#,##0.00")
                    ElseIf (drPromoType("TPL_ReqAmount_STATE") = 20) Then
                        trTPL_ReqAmount.Visible = True
                        txtTPL_ReqAmount.Enabled = False
                        txtTPL_ReqAmount.Text = Format(drPromoType("TPL_ReqAmount"), "#,##0.00")
                    Else
                        trTPL_ReqAmount.Visible = False
                    End If
                    'trTPL_ReqAmount.Visible = (drPromoType("TPL_ReqAmount_STATE") = 30)
                End If

                If Not IsDBNull(drPromoType("TPL_DiscAmount_STATE")) Then
                    trTPL_DiscAmount.Visible = (drPromoType("TPL_DiscAmount_STATE") > 10)
                    tdDiscAmount.Visible = True
                    tdPromoPrice.Visible = False
                    ' 10-DEC-2019 RPR for Any X for P (Smac Triggered)
                    If drPromoType("shortdesc").ToString.Equals("AnyXForP_S") Then
                        tdDiscAmount.Visible = False
                        tdPromoPrice.Visible = True
                    End If
                End If

                If Not IsDBNull(drPromoType("TPL_PercentDisc_STATE")) Then
                    trTPL_PercentDisc.Visible = (drPromoType("TPL_PercentDisc_STATE") > 10)
                    ' 10-DEC-2019 RPR Buy1Take1 (Smac Triggered)
                    If drPromoType("TPL_PercentDisc_STATE") = 20 Then
                        txtTPL_PercentDisc.Text = 100
                        txtTPL_PercentDisc.Enabled = False
                    Else
                        txtTPL_PercentDisc.Enabled = True

                    End If




                End If

                tdPercentMarkDown.Visible = True
                tdPercentDisc.Visible = False
                ' 10-DEC-2019 Buy 1 Take 1 (SMAC Triggered)
                ' 10-DEC-2019 Buy Y Get X at % (SMAC Item) (SMAC Triggered)
                If drPromoType("shortdesc").ToString.Equals("B1T1SMAC") Or _
                    drPromoType("shortdesc").ToString.Equals("BuyYGetX%S") Or _
                    drPromoType("shortdesc").ToString.Equals("BuyYGetX%D") Then
                    tdPercentMarkDown.Visible = False
                    tdPercentDisc.Visible = True
                End If

                'NBS20190822
                If Not IsDBNull(drPromoType("TPL_TransDesc_STATE")) Then
                    trTPL_TransDesc.Visible = (drPromoType("TPL_TransDesc_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_FreeItems_STATE")) Then
                    trTPL_FreeItems.Visible = (drPromoType("TPL_FreeItems_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_Prizes_STATE")) Then
                    trTPL_Prizes.Visible = (drPromoType("TPL_Prizes_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_DeptName_STATE")) Then
                    trTPL_DeptName.Visible = (drPromoType("TPL_DeptName_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_SellingArea_STATE")) Then
                    trTPL_SellingArea.Visible = (drPromoType("TPL_SellingArea_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_EventTime_STATE")) Then
                    trTPL_EventTime.Visible = (drPromoType("TPL_EventTime_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_ItemName_STATE")) Then
                    trTPL_ItemName.Visible = (drPromoType("TPL_ItemName_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_CelebName_STATE")) Then
                    trTPL_CelebName.Visible = (drPromoType("TPL_CelebName_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_BonusPoints_STATE")) Then
                    trTPL_BonusPoints.Visible = (drPromoType("TPL_BonusPoints_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_ActivityName_STATE")) Then
                    trTPL_ActivityName.Visible = (drPromoType("TPL_ActivityName_STATE") > 10)
                End If

                ' standard and permanent exclusions
                'If Not IsDBNull(drPromoType("POS_StdExclusion_STATE")) Then
                '    trPOS_StdExclusion.Visible = (drPromoType("POS_StdExclusion_STATE") > 10)
                'End If

                If Not IsDBNull(drPromoType("POS_PermExclusion_STATE")) Then
                    trPOS_PermExclusion.Visible = (drPromoType("POS_PermExclusion_STATE") > 10)
                End If

                If Not IsDBNull(drPromoType("TPL_BuyQty_STATE")) Then
                    trTPL_BuyQty.Visible = (drPromoType("TPL_BuyQty_STATE") > 10)
                    ' 10-DEC-2019 RPR Buy1Take1 (Smac Triggered)
                    If drPromoType("TPL_BuyQty_STATE") = 20 Then
                        txtTPL_BuyQty.Text = drPromoType("TPL_BuyQty")
                        txtTPL_BuyQty.Enabled = False
                    Else
                        txtTPL_BuyQty.Enabled = True
                    End If
                End If

                If Not IsDBNull(drPromoType("TPL_TakeQty_STATE")) Then
                    trTPL_TakeQty.Visible = (drPromoType("TPL_TakeQty_STATE") > 10)
                    ' 10-DEC-2019 RPR Buy1Take1 (Smac Triggered)
                    If drPromoType("TPL_TakeQty_STATE") = 20 Then
                        txtTPL_TakeQty.Text = drPromoType("TPL_TakeQty")
                        txtTPL_TakeQty.Enabled = False
                    Else
                        txtTPL_TakeQty.Enabled = True
                    End If
                End If

                '@@here::PromoNotes
                If Not IsDBNull(drPromoType("TPL_PromoNotes_STATE")) Then
                    trTPL_PromoNotes.Visible = (drPromoType("TPL_PromoNotes_STATE") > 10)
                End If

                'MALAgasino 20180906 - Check State first before initializing value to avoid errors. Start of Code
                'Original Code is commented below
                'InitializeXMLInputField(trTPL_RefMemo, txtTPL_RefMemo, drPromoType("TPL_RefMemo"), drPromoType("TPL_RefMemo_STATE"))
                'InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), "TplProcessType")
                'InitializeXMLInputField(trTPL_PurchaseReq, cboTPL_PurchaseReq, drPromoType("TPL_PurchaseReq"), drPromoType("TPL_PurchaseReq_STATE"), "TplPurchaseReq", drPromoType("TPL_PurchaseReq_ExList").ToString())

                If (drPromoType("TPL_RefMemo_STATE") <> 0) Then
                    InitializeXMLInputField(trTPL_RefMemo, txtTPL_RefMemo, drPromoType("TPL_RefMemo"), drPromoType("TPL_RefMemo_STATE"))
                End If

                InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), strProcessType)
                'If (drPromoType("TPL_ProcessType_STATE") <> 0) Then
                'Select Case drPromoType("GroupType")
                '    Case "REGULAR"
                '        InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), "PosProcessType")
                '    Case Else
                '        InitializeXMLInputField(trTPL_ProcessType, cboTPL_ProcessType, drPromoType("TPL_ProcessType"), drPromoType("TPL_ProcessType_STATE"), "TplProcessType")
                'End Select
                'trTPL_ProcessType.Visible = (drPromoType("TPL_ProcessType_STATE") = 30)
                'End If

                If (drPromoType("TPL_PurchaseReq_STATE") <> 0) Then
                    InitializeXMLInputField(trTPL_PurchaseReq, cboTPL_PurchaseReq, drPromoType("TPL_PurchaseReq"), drPromoType("TPL_PurchaseReq_STATE"), "TplPurchaseReq", drPromoType("TPL_PurchaseReq_ExList").ToString())
                    'trTPL_PurchaseReq.Visible = (drPromoType("TPL_PurchaseReq_STATE") > 10)
                End If

                'MALAgasino - 20180906 - Set panTemplated to true if one of elements is true. - Start of Code.
                'NBS20190822: arranged condition; added TPL_TransDesc_STATE
                'If (drPromoType("TPL_PurchaseReq_STATE") Is System.DBNull.Value And drPromoType("TPL_PurchaseReq_STATE") > 10) Or _
                '    (drPromoType("TPL_BuyQty_STATE")) > 10 Or _
                '    (drPromoType("TPL_TakeQty_STATE")) > 10 Or _
                '    (drPromoType("TPL_DiscAmount_STATE")) > 10 Or _
                '    (drPromoType("TPL_PercentDisc_STATE")) > 10 Or _
                '    (drPromoType("TPL_ReqAmount_STATE")) > 10 Or _
                '    (drPromoType("TPL_NumMonths_STATE")) > 10 Or _
                '    (drPromoType("TPL_BonusPoints_STATE") > 10 Or _
                '    (drPromoType("TPL_BankList_STATE")) > 10 Or _
                '    (drPromoType("TPL_BrandNames_STATE")) > 10 Or _
                '    (drPromoType("TPL_CelebName_STATE")) > 10 Or _
                '    (drPromoType("TPL_ItemName_STATE")) > 10 Or _
                '    (drPromoType("TPL_FreeItems_STATE")) > 10 Or _
                '    (drPromoType("TPL_ActivityName_STATE")) > 10 Or _
                '    (drPromoType("TPL_BonusPoints_STATE")) > 10 Or _
                '    (drPromoType("TPL_ProcessType_STATE")) > 10) Or _
                '    (drPromoType("TPL_TransDesc_STATE")) > 10 Then

                '    panTemplated.Visible = True
                '    'MALAgasino - 20180906 - Set panTemplated to true if one of elements is true. - End of Code.
                'End If

                panTemplated.Visible = True

                'balik

                ' sponsorship
                If drPromoType("CompSponsorship_STATE") = 30 Then
                    panDiscCharging.Visible = True
                    panDiscCharging.Enabled = True 'Mantis#64636 rbs7281 20250718 P3 QAT : 30 in state should enable the selection.
                    LoadShoulderingEntityDefaultSelection(True)
                    'rbs7281 MW update 20251125
                    If SystemUser.UserGroupType = "SBU" Then
                        FillResDropDownList(cboCompSponsorship, "CompSponsorship", "SBU", drPromoType("CompSponsorship_Exlist")) 'rbs7281 Include Exclusion column per promotype.
                    Else
                        FillResDropDownList(cboCompSponsorship, "CompSponsorship", "MBU", drPromoType("CompSponsorship_Exlist"))
                    End If
                ElseIf drPromoType("CompSponsorship_STATE") = 20 Then
                    'MALAgasino 20180905 - Added code for CompSponsorship_STATE = 20
                    panDiscCharging.Visible = True
                    panDiscCharging.Enabled = False

                    LoadShoulderingEntityDefaultSelection(True)
                Else
                    ' Else If drPromoType("CompSponsorship_STATE") = 20
                    panDiscCharging.Visible = False
                    LoadShoulderingEntityDefaultSelection(False)
                End If

                '
                ' chargeable entity
                '
                If drPromoType("Tpl_ChargeEntity_STATE") = 30 Then
                    panChargeableEntity.Visible = True

                    FillResDropDownList(cboChargeableEntity, "ChargeableEntity")

                    'If SystemUser.UserGroupType = "SBU" Then
                    '    FillResDropDownList(cboChargeableEntity, "ChargeableEntity")
                    'End If
                ElseIf drPromoType("Tpl_ChargeEntity_STATE") = 20 Then
                    panChargeableEntity.Visible = True
                    panChargeableEntity.Enabled = False
                Else
                    ' Else If drPromoType("CompSponsorship_STATE") = 20
                    panChargeableEntity.Visible = False
                End If

                '::::::::::::::::::::::::::::::::::::::::::::::::::::::
                ' get Promo Type Attachment settings
                '::::::::::::::::::::::::::::::::::::::::::::::::::::::


                ViewState("v_AllowAttachment") = CBool(drPromoType("AllowAttachment"))
                ViewState("v_RequireAttachment") = CBool(drPromoType("RequireAttachment"))

                If (ViewState("v_AllowAttachment") Or ViewState("v_RequireAttachment")) Then



                    Dim f As New IO.FileInfo(clsPromo.pathAttachment & clsSession.CurrRequestID.ToString())

                    If Not f.Exists Then
                        clsSession.AttachmentPath = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString()
                        Directory.CreateDirectory(clsSession.AttachmentPath)
                    End If

                    ' attachment
                    trPromoAttachment.Visible = (CBool(drPromoType("AllowAttachment")) Or CBool(drPromoType("RequireAttachment")))
                    ViewState("AllowAttachement") = CBool(drPromoType("AllowAttachment"))
                    lnkattachment.Visible = True

                End If

            Else
                ' error accessing database or promotype not existing
                ' ERROR MESSAGE: CODE 2001
                ' CHECK_HERE!
            End If

        End If

    End Sub

    Private Sub DisablePanelFields()
        If panGenericHostXML.Visible Then
            Dim panGenericHostXMLControl As Control
            For Each panGenericHostXMLControl In panGenericHostXML.Controls
                If TypeOf panGenericHostXMLControl Is HtmlControls.HtmlTableRow Then
                    panGenericHostXMLControl.Visible = False
                End If
            Next
        End If
    End Sub
    Private Sub HideTemplatedRows()
        panComboOffer.Visible = True
        panTemplated.Visible = True
        panGenericHostXML.Visible = True
        panCommonFields.Visible = True
        panDiscCharging.Visible = True

        'Hide Combo offer fields
        trXML_QualifiedCust.Visible = False
        trXML_NameOfPartner.Visible = False
        trXML_ProofOfMembership.Visible = False
        trXML_PartnerEstabGWP.Visible = False


        'Hide templated fields
        trTPL_PurchaseReq.Visible = False
        trTPL_BuyQty.Visible = False
        trTPL_TakeQty.Visible = False
        trTPL_DiscAmount.Visible = False
        trTPL_PercentDisc.Visible = False
        trTPL_ReqAmount.Visible = False
        trTPL_NumMonths.Visible = False
        trTPL_BonusPoints.Visible = False
        trTPL_BankList.Visible = False
        trTpl_SubsidyRate.Visible = False
        trTPL_BrandNames.Visible = False
        trTPL_CelebName.Visible = False
        trTPL_ItemName.Visible = False
        trTPL_DeptName.Visible = False
        trTPL_ActivityName.Visible = False
        trTPL_SellingArea.Visible = False
        trTPL_EventTime.Visible = False
        trTPL_TransDesc.Visible = False     'NBS20190822
        trTPL_FreeItems.Visible = False
        trTPL_Prizes.Visible = False
        trTPL_RefMemo.Visible = False
        trTPL_ProcessType.Visible = False
        trPOS_StdExclusion.Visible = False
        trPOS_PermExclusion.Visible = False

        'Generic Host XML fields
        trXML_Descr1prm.Visible = False
        trXML_Descr2prm.Visible = False
        trXML_ReceiptDesc1.Visible = False
        trXML_ReceiptDesc2.Visible = False
        trXML_ActionType.Visible = False
        trXML_DiscountType.Visible = False
        trXML_ProcessType.Visible = False
        trXML_MaxQty.Visible = False
        trXML_MaxAmount.Visible = False
        trXML_MinAmount.Visible = False
        trXML_CompessID.Visible = False
        trXML_ExclusionType.Visible = False
        trXML_ActDeacTime.Visible = False
        trXML_ActiveDays.Visible = False
        trXML_Priority.Visible = False
        trXML_RuleValue1.Visible = False
        trXML_RuleValue2.Visible = False
        trXML_RuleValue3.Visible = False
        trXML_RuleValue4.Visible = False
        trXML_Condition1.Visible = False
        trXML_EligibleCards.Visible = False
        trXML_DiscAmount.Visible = False
        trXML_PercentDisc.Visible = False
        trXML_SMACKitPrice.Visible = False
        trXML_CouponMsg.Visible = False

        'Common fields
        trXML_QualifiedItems.Visible = False
        trXML_PosPromoSubType.Visible = False
        trTPL_PromoNotes.Visible = False

        'Discount Charging fields
        'balik
        'trXML_CompSponsorship.Visible = False
        LoadShoulderingEntityDefaultSelection(False)
        trXML_ChargeableEntity.Visible = False

        ''Swipestakes
        trXML_TenderType.Visible = False

    End Sub


    Private Sub InitializeTemplatedRows(ByVal InitCode As Integer)

        If InitCode = 0 Then
            trTPL_BankList.Visible = False
            trTpl_SubsidyRate.Visible = False
            trTPL_BrandNames.Visible = False
            trTPL_DeptName.Visible = False
            trTPL_TransDesc.Visible = False     'NBS20190822
            trTPL_FreeItems.Visible = False
            trTPL_ItemName.Visible = False
            trTPL_NumMonths.Visible = False
            trTPL_Prizes.Visible = False
            trTPL_ProcessType.Visible = False
            trTPL_PurchaseReq.Visible = False
            trTPL_RefMemo.Visible = False
            trTPL_ReqAmount.Visible = False
            trTPL_SellingArea.Visible = False
            trTPL_EventTime.Visible = False
            trTPL_BonusPoints.Visible = False
            trTPL_CelebName.Visible = False

            trTPL_PercentDisc.Visible = False
            trTPL_DiscAmount.Visible = False

            trTPL_ActivityName.Visible = False
            trTPL_BuyQty.Visible = False
            trTPL_TakeQty.Visible = False
            trTPL_PromoNotes.Visible = False
        End If

    End Sub

    Private Function InsertPlanCost_Info() As String
        Dim strResult As String

        strResult = ""

        InsertPlanCost_Info = strResult
    End Function

    Private Sub RetrieveXMLRuleValueFields(ByVal nPromoID As Long)

        'Dim dtPromoTypeRules As DataTable = Nothing
        'Dim strQuery As String

        'If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtPromoTypeRules) Then

        'End If

        'trXML_RuleValue1.Visible = False
        'trXML_RuleValue2.Visible = False
        'trXML_RuleValue3.Visible = False
        'trXML_RuleValue4.Visible = False

        'strQuery = "SELECT * " & _
        '            "FROM PromotionRules " & _
        '            "WHERE PromoID = 0" & nPromoID


    End Sub

    Private Sub InitializeXMLRuleValueFields(ByVal PromotionTypeID As Integer)

        Dim dtPromoTypeRules As DataTable = Nothing
        Dim strQuery As String

        trXML_RuleValue1.Visible = False
        trXML_RuleValue2.Visible = False
        trXML_RuleValue3.Visible = False
        trXML_RuleValue4.Visible = False

        strQuery = "SELECT * " & _
                    "FROM PromoTypeRules " & _
                    "WHERE PromoTypeID = 0" & PromotionTypeID

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtPromoTypeRules) Then

            Dim i As Integer

            For Each drRow As DataRow In dtPromoTypeRules.Rows

                i = CInt(drRow("SequenceNo").ToString)

                Dim trXML As HtmlTableRow = CType(panGenericHostXML.FindControl("trXML_RuleValue" & i), HtmlTableRow)
                Dim lblXML As Label = CType(panGenericHostXML.FindControl("lblXML_RuleValue" & i), Label)
                Dim txtXML As TextBox = CType(panGenericHostXML.FindControl("txtXML_RuleValue" & i), TextBox)

                trXML.Visible = (drRow("Rule_STATE") = clsPromo.XMLfieldState.EnabledField)

                If drRow("ValueType") = 1 Or drRow("ValueType") = 4 Then

                    ' quantity
                    txtXML.Text = CInt(drRow("RuleValue"))

                    Select Case CInt(drRow("RuleCondition"))
                        Case 0

                            If drRow("SequenceNo") = 1 Then
                                lblXML.Text = "Discounted Quantity:"
                            Else
                                lblXML.Text = "Total Purchase Qty:"
                            End If

                        Case 1
                            lblXML.Text = "Minimum Quantity:"
                        Case 2
                            lblXML.Text = "Maximum Quantity:"
                        Case 10
                            lblXML.Text = "For Every Item:"

                    End Select

                ElseIf drRow("ValueType") = 2 Then

                    ' amount
                    txtXML.Text = CSng(drRow("RuleValue"))

                    Select Case CInt(drRow("RuleCondition"))
                        Case 0  ' unused
                            lblXML.Text = "Amount:"
                        Case 1
                            lblXML.Text = "Required Amount:"
                        Case 2
                            lblXML.Text = "Maximum Amount:"
                        Case 10
                            lblXML.Text = "For Every Amount:"
                    End Select

                End If

            Next

        Else
            ' error
        End If

    End Sub

    Private Sub ReComputeValues()

        Dim nOrigValue As Double = Val(txtOrigValue.Text.Replace(",", ""))
        Dim nPromoValue As Double = Val(txtPromoValue.Text.Replace(",", ""))
        Dim nAllocation As Double = Val(txtAllocation.Text.Replace(",", ""))

        ' compute values 
        Dim nDiscAmount As Double = nOrigValue - nPromoValue

        ' fixed rate of 10% starting April 4, 2014 as per MCI/MPD mandate
        Dim nWAPrate As Double = 10
        'Dim nWAPrate As Double = cboVendorType.SelectedValue

        Dim nPromoBudget As Double = nDiscAmount * nAllocation
        Dim nWebAdPlacement As Double = nPromoValue * nAllocation * (nWAPrate / 100)
        Dim nTotalBudget As Double = nWebAdPlacement + nPromoBudget

        ViewState("OrigValue") = nOrigValue
        ViewState("PromoValue") = nPromoValue
        ViewState("Allocation") = nAllocation

        ViewState("DiscAmount") = nDiscAmount
        ViewState("WAPrate") = nWAPrate
        ViewState("PromoBudget") = nPromoBudget
        ViewState("WebAdPlacement") = nWebAdPlacement
        ViewState("TotalBudget") = nTotalBudget

        ' update label values
        lblDiscAmount.Text = Format(nDiscAmount, "#,##0.00")
        lblWebAdRate.Text = Format(nWAPrate, "##0") & "%"
        lblPromoBudget.Text = Format(nPromoBudget, "#,##0.00")
        lblWebAdPlacement.Text = Format(nWebAdPlacement, "#,##0.00")
        lblTotalBudget.Text = Format(nTotalBudget, "#,##0.00")

    End Sub

    Protected Sub lnkPromoDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoDetails.Click

        If SavePromotionInfo() Then

            clsSession.PromoTypeID = CInt(cboPromoType.SelectedValue)

            ' next screen
            Response.Redirect("PromoDetailsEntry.aspx")

        End If

    End Sub

    Private Function IsValidEntries() As Boolean

        If cboPromoType.SelectedValue < 0 Then
            blistErrorMsg.Items.Add("Please select a Promo Type before proceeding.")
            IsValidEntries = False
            Exit Function
        End If

        If panMechanics.Visible And litMechanics.Text = "" Then
            blistErrorMsg.Items.Add("Promo mechanics not specified.")
        End If

        '*************************************
        ' Class Discount Promotions
        '*************************************

        If panClassDiscount.Visible Then

            If txtDiscount.Text = "" Then
                blistErrorMsg.Items.Add("Please indicate discount rate.")

            ElseIf CInt(txtDiscount.Text) = 0 Then
                blistErrorMsg.Items.Add("Discount rate must be greater than zero.")

            ElseIf CInt(txtDiscount.Text) Mod 5 <> 0 Then
                blistErrorMsg.Items.Add("Discount rate must be in increments of 5.")

            ElseIf CInt(txtDiscount.Text) > 100 Then
                blistErrorMsg.Items.Add("Discount rate must not exceed 100%.")

            End If

        End If

        '*************************************
        ' Markdown Promotions
        '*************************************

        If panMarkdown.Visible And txtMarkdown.Text = "" Then
            blistErrorMsg.Items.Add("Please indicate markdown rate.")
        End If

        '*************************************
        ' SMAC Deals Promotions
        '*************************************
        If panSMACdeals.Visible Then

            ReComputeValues()

            If ViewState("OrigValue") = 0 Then
                blistErrorMsg.Items.Add("Original Value should be greater than zero.")
            End If

            If ViewState("PromoValue") = 0 Then
                blistErrorMsg.Items.Add("Discounted Value should be greater than zero.")
            End If

            If ViewState("PromoValue") > ViewState("OrigValue") Then
                blistErrorMsg.Items.Add("Discounted Value should not be greater than the Original Value.")
            End If

            If ViewState("Allocation") = 0 Then
                blistErrorMsg.Items.Add("Deal Allocation should have a value.")
            End If

            If (Not IsDate(txtOnlineSellingStart.Text)) Or (Not IsDate(txtOnlineSellingEnd.Text)) Then
                blistErrorMsg.Items.Add("Redemption period is blank or has an invalid date format.") ' invalid date format
            Else
                If CDate(txtOnlineSellingEnd.Text) < CDate(txtOnlineSellingStart.Text) Then
                    blistErrorMsg.Items.Add("Redemption period has an invalid date range.")
                Else

                    If CDate(txtOnlineSellingStart.Text) <= CDate(ViewState("PromoPeriodTo")) Then
                        blistErrorMsg.Items.Add("Redemption date should occur after the online selling period.")
                    End If

                    ' 7 day selling duration
                    'If DateDiff(DateInterval.Day, CDate(txtOnlineSellingStart.Text), CDate(txtOnlineSellingEnd.Text)) < 7 Then
                    '    blistErrorMsg.Items.Add("Online deal selling should atleast be 7 days long.")
                    'End If

                    ' check with promo redemption period

                End If
            End If

            ' outright or store consignor is a required field.
            If cboVendorType.SelectedIndex = 0 Then

                blistErrorMsg.Items.Add("Please select outright or store consignor.")

            End If
        End If

        '*************************************
        ' Special Promotions
        '*************************************

        If panSpecialPromo.Visible Then

            If txtPurchaseQty.Text = "" Or Val(txtPurchaseQty.Text) = 0 Then
                blistErrorMsg.Items.Add("Purchase Quantity must be specified.")
            Else
                If Not IsNumeric(txtPurchaseQty.Text) Then
                    blistErrorMsg.Items.Add("Purchase Quantity is not in a valid numeric format.")
                ElseIf Val(txtPurchaseQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Purchase Quantity must be greater than one.")
                End If
            End If

            If Val(txtFreeQty.Text) = 0 Or txtFreeQty.Text = "" Then
                blistErrorMsg.Items.Add("Free Quantity must be specified.")
            Else
                If Not IsNumeric(txtFreeQty.Text) Then
                    blistErrorMsg.Items.Add("Free Quantity is not in a valid numeric format.")
                ElseIf Val(txtFreeQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Free Quantity must be greater than one.")
                ElseIf Val(txtFreeQty.Text) > Val(txtPurchaseQty.Text) Then
                    blistErrorMsg.Items.Add("Free Quantity must not be greater than the purchase quantity.")
                End If
            End If

            If Val(txtPromoPrice.Text) = 0 Or txtPromoPrice.Text = "" Then
                blistErrorMsg.Items.Add("Promo amount must be greater than zero.")
            Else
                If Not IsNumeric(txtPromoPrice.Text) Then
                    blistErrorMsg.Items.Add("Promo amount is not in a valid numeric format.")
                ElseIf Val(txtPromoPrice.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Promo amount must be greater than zero.")
                End If
            End If
        End If

        '*************************************
        ' Any X for P
        '*************************************

        If panAnyXForP.Visible Then

            If txtAnyX4P_BuyQty.Text = "" Then
                blistErrorMsg.Items.Add("Buy Quantity must be specified.")
            Else
                If Not IsNumeric(txtAnyX4P_BuyQty.Text) Then
                    blistErrorMsg.Items.Add("Buy Quantity is not in a valid numeric format.")
                ElseIf Val(txtAnyX4P_BuyQty.Text) < 2 Then
                    blistErrorMsg.Items.Add("Buy Quantity must be greater than one.")
                End If
            End If

            If txtAnyX4P_PromoPrice.Text = "" Then
                blistErrorMsg.Items.Add("Promo Price must be specified.")
            Else
                If Not IsNumeric(txtAnyX4P_PromoPrice.Text) Then
                    blistErrorMsg.Items.Add("Promo Price is not in a valid numeric format.")
                ElseIf Val(txtAnyX4P_PromoPrice.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Promo Price must be greater than zero.")
                End If
            End If

        End If

        '*************************************
        ' Buy Y, Get X for P (same item)
        '*************************************

        If panBuyGetSameForP.Visible Then

            If txtBG4Ps_BuyQty.Text = "" Then
                blistErrorMsg.Items.Add("Buy Quantity must be specified.")
            Else
                If Not IsNumeric(txtBG4Ps_BuyQty.Text) Then
                    blistErrorMsg.Items.Add("Buy Quantity is not in a valid numeric format.")
                ElseIf Val(txtBG4Ps_BuyQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Buy Quantity must be greater than zero.")
                End If
            End If

            If txtBG4Ps_TakeQty.Text = "" Then
                blistErrorMsg.Items.Add("Take Quantity must be specified.")
            Else
                If Not IsNumeric(txtBG4Ps_TakeQty.Text) Then
                    blistErrorMsg.Items.Add("Take Quantity is not in a valid numeric format.")
                ElseIf Val(txtBG4Ps_TakeQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Take Quantity must be greater than zero.")
                End If
            End If

            If txtBG4Ps_DiscAmount.Text = "" Then
                blistErrorMsg.Items.Add("Amount of discount must be specified.")
            Else
                If Not IsNumeric(txtBG4Ps_DiscAmount.Text) Then
                    blistErrorMsg.Items.Add("Amount of discount is not in a valid numeric format.")
                ElseIf Val(txtBG4Ps_DiscAmount.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Amount of discount must be greater than zero.")
                End If
            End If

        End If

        '*************************************
        ' Buy Y, Get X for P (diff item)
        '*************************************

        If panBuyGetDiffForP.Visible Then

            If txtBG4Pd_BuyQty.Text = "" Then
                blistErrorMsg.Items.Add("Buy Quantity must be specified.")
            Else
                If Not IsNumeric(txtBG4Pd_BuyQty.Text) Then
                    blistErrorMsg.Items.Add("Buy Quantity is not in a valid numeric format.")
                ElseIf Val(txtBG4Pd_BuyQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Buy Quantity must be greater than zero.")
                End If
            End If

            If txtBG4Pd_TakeQty.Text = "" Then
                blistErrorMsg.Items.Add("Take Quantity must be specified.")
            Else
                If Not IsNumeric(txtBG4Pd_TakeQty.Text) Then
                    blistErrorMsg.Items.Add("Take Quantity is not in a valid numeric format.")
                ElseIf Val(txtBG4Pd_TakeQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Take Quantity must be greater than zero.")
                End If
            End If

            If txtBG4Pd_DiscAmount.Text = "" Then
                blistErrorMsg.Items.Add("Discount Amount must be specified.")
            Else
                If Not IsNumeric(txtBG4Pd_DiscAmount.Text) Then
                    blistErrorMsg.Items.Add("Discount Amount is not in a valid numeric format.")
                ElseIf Val(txtBG4Pd_DiscAmount.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Discount Amount must be greater than zero.")
                End If
            End If

        End If

        '*************************************
        ' Buy Y, Get X at % discount (same item)
        '*************************************

        If panBuyGetSamePercent.Visible Then

            If txtBGPerOffs_BuyQty.Text = "" Then
                blistErrorMsg.Items.Add("Buy Quantity must be specified.")
            Else
                If Not IsNumeric(txtBGPerOffs_BuyQty.Text) Then
                    blistErrorMsg.Items.Add("Buy Quantity is not in a valid numeric format.")
                ElseIf Val(txtBGPerOffs_BuyQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Buy Quantity must be greater than zero.")
                End If
            End If

            If txtBGPerOffs_TakeQty.Text = "" Then
                blistErrorMsg.Items.Add("Take Quantity must be specified.")
            Else
                If Not IsNumeric(txtBGPerOffs_TakeQty.Text) Then
                    blistErrorMsg.Items.Add("Take Quantity is not in a valid numeric format.")
                ElseIf Val(txtBGPerOffs_TakeQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Take Quantity must be greater than zero.")
                End If
            End If

            If txtBGPerOffs_PercentDisc.Text = "" Then
                blistErrorMsg.Items.Add("Percent Discount must be specified.")
            Else
                If Not IsNumeric(txtBGPerOffs_PercentDisc.Text) Then
                    blistErrorMsg.Items.Add("Percent Discount is not in a valid numeric format.")
                ElseIf Val(txtBGPerOffs_PercentDisc.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Percent Discount must be greater than zero.")
                End If
            End If

        End If

        '*************************************
        ' Buy Y, Get X at % discount (diff item)
        '*************************************

        If panBuyGetDiffPercent.Visible Then

            If txtBGPerOffd_BuyQty.Text = "" Then
                blistErrorMsg.Items.Add("Buy Quantity must be specified.")
            Else
                If Not IsNumeric(txtBGPerOffd_BuyQty.Text) Then
                    blistErrorMsg.Items.Add("Buy Quantity is not in a valid numeric format.")
                ElseIf Val(txtBGPerOffd_BuyQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Buy Quantity must be greater than zero.")
                End If
            End If

            If txtBGPerOffd_TakeQty.Text = "" Then
                blistErrorMsg.Items.Add("Take Quantity must be specified.")
            Else
                If Not IsNumeric(txtBGPerOffd_TakeQty.Text) Then
                    blistErrorMsg.Items.Add("Take Quantity is not in a valid numeric format.")
                ElseIf Val(txtBGPerOffd_TakeQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Take Quantity must be greater than zero.")
                End If
            End If

            If txtBGPerOffd_PercentDisc.Text = "" Then
                blistErrorMsg.Items.Add("Percent Discount must be specified.")
            Else
                If Not IsNumeric(txtBGPerOffd_PercentDisc.Text) Then
                    blistErrorMsg.Items.Add("Percent Discount is not in a valid numeric format.")
                ElseIf Val(txtBGPerOffd_PercentDisc.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Percent Discount must be greater than zero.")
                End If
            End If

        End If

        '*************************************
        ' Any XY for P
        '*************************************

        If panAnyXYforP.Visible Then

            If txtAnyXY_BuyQty.Text = "" Then
                blistErrorMsg.Items.Add("Buy Quantity must be specified.")
            Else
                If Not IsNumeric(txtAnyXY_BuyQty.Text) Then
                    blistErrorMsg.Items.Add("Buy Quantity is not in a valid numeric format.")
                ElseIf Val(txtAnyXY_BuyQty.Text) < 1 Then
                    blistErrorMsg.Items.Add("Buy Quantity must be greater than zero.")
                End If
            End If

            If txtAnyXY_PromoPrice.Text = "" Then
                blistErrorMsg.Items.Add("Discount Amount must be specified.")
            Else
                If Not IsNumeric(txtAnyXY_PromoPrice.Text) Then
                    blistErrorMsg.Items.Add("Discount Amount is not in a valid numeric format.")
                ElseIf Val(txtAnyXY_PromoPrice.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Discount Amount must be greater than zero.")
                End If
            End If

        End If

        '*************************************
        ' XML Promotions
        '*************************************

        If panGenericHostXML.Visible Then

            ' validate Promotion Application
            If trXML_ProcessType.Visible Then
                If cboXML_ProcessType.SelectedValue = -1 Then
                    blistErrorMsg.Items.Add("Promotion Application not specified.")
                End If
            End If

            If trXML_Condition1.Visible Then
                If cboXML_Condition1.SelectedValue = -1 Then
                    blistErrorMsg.Items.Add(Replace(lblXML_Condition1.Text, ":", "") & " not specified.")
                End If
            End If

            If trXML_EligibleCards.Visible And ViewState("IsEligibleCardsRequired") = 1 Then
                If Not IsEligibleCardSelected() Then
                    blistErrorMsg.Items.Add("Eligible cards not specified.")
                End If
            End If

            If trXML_PercentDisc.Visible Then
                If txtXML_PercentDisc.Text = "" Then
                    blistErrorMsg.Items.Add("Percent Discount must be specified.")
                ElseIf Not IsNumeric(txtXML_PercentDisc.Text) Then
                    blistErrorMsg.Items.Add("Percent Discount is not in a valid numeric format.")
                ElseIf Val(txtXML_PercentDisc.Text) <= 0 Then
                    blistErrorMsg.Items.Add("Percent Discount must be greater than zero.")
                ElseIf Val(txtXML_PercentDisc.Text) > 100 Then
                    blistErrorMsg.Items.Add("Percent Discount must not exceed 100%.")
                End If
            End If

            If trXML_DiscAmount.Visible Then
                If txtXML_DiscAmount.Text = "" Then
                    blistErrorMsg.Items.Add(lblXML_DiscAmount.Text.Replace(":", "") & " must be specified.")
                Else
                    If Not IsNumeric(txtXML_DiscAmount.Text) Then
                        blistErrorMsg.Items.Add(lblXML_DiscAmount.Text.Replace(":", "") & " is not in a valid numeric format.")
                    ElseIf Val(txtXML_DiscAmount.Text) <= 0 Then
                        blistErrorMsg.Items.Add(lblXML_DiscAmount.Text.Replace(":", "") & " must be greater than zero.")
                    End If
                End If
            End If

            ' check minimum amount requirement
            If trXML_RuleValue2.Visible Then
                If txtXML_RuleValue2.Text = "" Then
                    blistErrorMsg.Items.Add(lblXML_RuleValue2.Text.Replace(":", "") & " not specified.")
                Else
                    If Not IsNumeric(txtXML_RuleValue2.Text) Then
                        blistErrorMsg.Items.Add(lblXML_RuleValue2.Text.Replace(":", "") & " is not in a valid numeric format.")
                    ElseIf Val(txtXML_RuleValue2.Text) <= 0 Then
                        blistErrorMsg.Items.Add(lblXML_RuleValue2.Text.Replace(":", "") & " must be greater than zero.")
                    End If
                End If
            End If

            If ViewState("POS_MessageString_STATE") = clsPromo.XMLfieldState.EnabledField Then
                If Replace(GetCouponMessageEntry(), vbCrLf, "") = "" Then
                    blistErrorMsg.Items.Add("Coupon message not specified.")
                End If
            End If

            If cboXML_StartTime.Visible And cboXML_StartTime.Text = "" Then
                blistErrorMsg.Items.Add("Time not specified.")
            End If

            If (cboXML_StartTime.Text > cboXML_EndTime.Text) Then
                blistErrorMsg.Items.Add("Start time must not be later than end time.") 'Mantis#64834 rbs7281 7/23/2025.
            End If


        End If

        '@@here_qualifieditems
        If trXML_QualifiedItems.Visible Then
            If cboXML_QualifiedItems.SelectedValue = -1 Then
                blistErrorMsg.Items.Add("Qualified items not specified.")
            End If
        End If

        '@@here_promosubtype
        If trXML_PosPromoSubType.Visible Then
            If cboXML_PosPromoSubType.SelectedValue = -1 Then
                blistErrorMsg.Items.Add("Subtype not specified.")
            End If
        End If

        '@@here_discountamount
        If trTPL_DiscAmount.Visible Then
            If txtTPL_DiscAmount.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Amount Discount not specified.")
            ElseIf Not IsNumeric(txtTPL_DiscAmount.Text) Then
                blistErrorMsg.Items.Add("Amount Discount is not in a valid numeric format.")
            ElseIf Val(txtTPL_DiscAmount.Text) = 0 Then
                blistErrorMsg.Items.Add("Amount Discount must be specified.")
            End If
        End If

        'SR#6193598 rbs7281: Shouldering Entity Validation
        If cboCompSponsorship.Visible Then
            If cboCompSponsorship.SelectedValue = "-1" Then
                blistErrorMsg.Items.Add("Shouldering Entity must be specified.")
            End If
        End If

        If cboXML_Sponsorship.Visible Then
            If cboXML_Sponsorship.SelectedValue = "-1" Then
                blistErrorMsg.Items.Add("Shouldering Entity must be specified.")
            End If
        End If

        If trTPL_PercentDisc.Visible Then
            If txtTPL_PercentDisc.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Percent Markdown not specified.")
            ElseIf Not IsNumeric(txtTPL_PercentDisc.Text) Then
                blistErrorMsg.Items.Add("Percent Markdown is not in a valid numeric format.")
            ElseIf Val(txtTPL_PercentDisc.Text) <= 0 Then
                blistErrorMsg.Items.Add("Percent Markdown must be greater than zero.")
            ElseIf Val(txtTPL_PercentDisc.Text) > 100 Then
                blistErrorMsg.Items.Add("Percent Markdown must not exceed 100%.")
            End If
        End If

        If trTpl_SubsidyRate.Visible Then
            If txtTpl_SubsidyRate.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Subsidy Rate not specified.")
            ElseIf Not IsNumeric(txtTpl_SubsidyRate.Text) Then
                blistErrorMsg.Items.Add("Subsidy Rate is not in a valid numeric format.")
            ElseIf Val(txtTpl_SubsidyRate.Text) = 0 Then
                blistErrorMsg.Items.Add("Subsidy Rate must be greater than zero.")
            End If
        End If

        If txtXML_MinAmount.Visible Then
            If txtXML_MinAmount.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Minimum Amount not specified.")
            ElseIf txtXML_MinAmount.Text <= 0 Then
                blistErrorMsg.Items.Add("Minimum Amount must be greater than 0.")
            End If
        End If

        'Mantis#67516/67505 - Correct handling of the amount 
        If txtXML_MinAmount.Visible And txtXML_DiscAmount.Visible Then
            ' Ensure both fields have valid numeric values before comparing
            Dim minAmount As Decimal
            Dim discAmount As Decimal

            If Decimal.TryParse(txtXML_MinAmount.Text.Trim(), minAmount) AndAlso _
               Decimal.TryParse(txtXML_DiscAmount.Text.Trim(), discAmount) Then

                If minAmount < discAmount Then
                    blistErrorMsg.Items.Add("Minimum Amount cannot be less than Discount Amount.")
                End If
            End If
        End If

        If txtQualifiedCust.Visible And txtQualifiedCust.Text = "" Then
            blistErrorMsg.Items.Add("Qualified Customer not specified.")
        End If

        If txtNameOfPartner.Visible And txtNameOfPartner.Text = "" Then
            blistErrorMsg.Items.Add("Name of Partner not specified.")
        End If

        If txtProofOfMembership.Visible And txtProofOfMembership.Text = "" Then
            blistErrorMsg.Items.Add("Proof of Membership not specified.")
        End If

        '*************************************
        ' Templated Promotions
        '*************************************

        If panTemplated.Visible Then

            If txtTPL_ActivityName.Visible And txtTPL_ActivityName.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Name of Activity not specified.")
            End If

            If txtTPL_BankList.Visible And txtTPL_BankList.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Participating banks not specified.")
            End If

            If txtTpl_SubsidyRate.Visible And txtTpl_SubsidyRate.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Subsidy Rate not specified.")
            End If

            If txtTPL_BonusPoints.Visible And txtTPL_BonusPoints.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Bonus Points not specified.")
            End If

            If txtTPL_BrandNames.Visible And txtTPL_BrandNames.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Product Name(s) not specified.")
            End If

            If txtTPL_BuyQty.Visible And txtTPL_BuyQty.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Buy Quantity not specified.")
            End If

            If txtTPL_CelebName.Visible And txtTPL_CelebName.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Celebrity/Mascot name not specified.")
            End If

            If txtTPL_DeptName.Visible And txtTPL_DeptName.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Department not specified.")
            End If

            If txtTPL_EventTime.Visible And txtTPL_EventTime.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Time of Event/Activity not specified.")
            End If

            ' NBS20190822
            If txtTPL_TransDesc.Visible And txtTPL_TransDesc.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Transaction Details not specified.")
            End If

            If txtTPL_FreeItems.Visible And txtTPL_FreeItems.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Free Items not specified.")
            End If

            If txtTPL_ItemName.Visible And txtTPL_ItemName.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Items not specified.")
            End If

            If txtTPL_NumMonths.Visible And txtTPL_NumMonths.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Number of Months not specified.")
            End If

            If txtTPL_Prizes.Visible And txtTPL_Prizes.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Prize(s) not specified.")
            End If

            If txtTPL_RefMemo.Visible And txtTPL_RefMemo.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Reference Memo not specified.")
            End If

            If txtTPL_ReqAmount.Visible And txtTPL_ReqAmount.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Required Amount not specified.")
            End If

            If txtTPL_SellingArea.Visible And txtTPL_SellingArea.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Event/Activity Area not specified.")
            End If

            If txtTPL_TakeQty.Visible And txtTPL_TakeQty.Text.Trim() = "" Then
                blistErrorMsg.Items.Add("Take Quantity not specified.")
            End If

            'If cboTPL_ProcessType.Visible And cboTPL_ProcessType.SelectedValue = -1 Then
            '    blistErrorMsg.Items.Add("Process Type not specified.")
            'End If

            If cboTPL_PurchaseReq.Visible And cboTPL_PurchaseReq.SelectedValue = -1 Then
                blistErrorMsg.Items.Add("Purchase Requirement not specified.")
            End If

            If cboTPL_ProcessType.Visible And cboTPL_ProcessType.SelectedValue = -1 Then
                blistErrorMsg.Items.Add("Process Type not specified.")
            End If

            If txtXML_SMACKitPrice.Visible And txtXML_SMACKitPrice.Text = "" Then
                blistErrorMsg.Items.Add("SMAC Kit Price not specified.")
            End If
        End If

        If panChargeableEntity.Visible Then

            If trTplChargeEnt_VendorCode.Visible Then
                If txtTplChargeEnt_VendorCode.Text = "" Then
                    blistErrorMsg.Items.Add("Vendor code not specified.")
                End If
                If Not IsNumeric(txtTplChargeEnt_VendorCode.Text) Or txtTplChargeEnt_VendorCode.Text.Length() < 6 Then
                    blistErrorMsg.Items.Add("Invalid Vendor code.")
                End If
            End If

            If trTplChargeEnt_DSsub.Visible Then

                Dim IsChargeEntError As Boolean = False

                If txtTplChargeEnt_DSsub.Text = "" Or Not IsNumeric(txtTplChargeEnt_DSsub.Text) Then
                    IsChargeEntError = True
                    blistErrorMsg.Items.Add("DS Contribution not specified or invalid numeric format.")
                End If

                If txtTplChargeEnt_BUsub.Text = "" Or Not IsNumeric(txtTplChargeEnt_BUsub.Text) Then
                    IsChargeEntError = True
                    blistErrorMsg.Items.Add("MBU Contribution not specified or invalid numeric format.")
                End If

                If Not IsChargeEntError Then
                    If Decimal.Parse(txtTplChargeEnt_DSsub.Text) + Decimal.Parse(txtTplChargeEnt_BUsub.Text) <> 100 Then
                        blistErrorMsg.Items.Add("DS and MBU contributions should total 100%")
                    End If
                End If

            End If

        End If

        '*************************************
        ' Promo Plan Entries
        '*************************************

        If trPromoAnalytics.Visible Then

            Dim nPlanPromoSales As Double = Val(txtPlanPromoSales.Text.Replace(",", ""))
            Dim nPlanPromoCost As Double = Val(txtPlanPromoCost.Text.Replace(",", ""))
            Dim nPlanMargin As Double = Val(txtPlanMargin.Text.Replace(",", ""))

            ' if textboxes are not empty then check if numeric
            If nPlanPromoSales <> 0 Then
                If Not IsNumeric(txtPlanPromoSales.Text) Then
                    blistErrorMsg.Items.Add("Planned Promo Sales is not in a valid numeric format.")
                ElseIf nPlanPromoCost = 0 Then
                    blistErrorMsg.Items.Add("Planned Promo Cost must be provided if Planned Sales is specified.")
                End If
            End If

            If nPlanPromoCost <> 0 Then
                If Not IsNumeric(txtPlanPromoCost.Text) Then
                    blistErrorMsg.Items.Add("Planned Promo Cost is not in a valid numeric format.")
                ElseIf nPlanPromoSales = 0 Then
                    blistErrorMsg.Items.Add("Planned Promo Sales must be provided if Planned Cost is specified.")
                ElseIf nPlanPromoCost > nPlanPromoSales Then
                    blistErrorMsg.Items.Add("Planned Promo Cost should not exceed the Planned Promo Sales.")
                End If
            End If

            If nPlanMargin <> 0 Then
                If Not IsNumeric(txtPlanMargin.Text) Then
                    blistErrorMsg.Items.Add("Planned Promo Margin is not in a valid numeric format.")
                ElseIf nPlanPromoCost = 0 Or nPlanPromoSales = 0 Then
                    blistErrorMsg.Items.Add("Planned Promo Cost and Sales must be provided if Planned Margin is specified.")
                ElseIf nPlanMargin > 100 Then
                    blistErrorMsg.Items.Add("Planned Promo Margin should not exceed 100%.")
                End If
            End If

            ' check attachment
        End If

        If panSwipestakes.Visible Then
            'If txtXML_MinAmount.Text = "" Then
            '    blistErrorMsg.Items.Add("Minimum Amount must be specified.")
            'Else
            '    If Not IsNumeric(txtXML_MinAmount.Text) Then
            '        blistErrorMsg.Items.Add("Minimum Amount is not in a valid numeric format.")
            '    ElseIf Val(txtXML_MinAmount.Text) < 1 Then
            '        blistErrorMsg.Items.Add("Minimum Amount must be greater than zero.")
            '    End If
            'End If

            '@@here_promosubtype
            If trXML_TenderType.Visible Then
                If cboXML_TenderType.SelectedValue = -1 Then
                    blistErrorMsg.Items.Add("Tender Type not specified.")
                End If
            End If


            '@@Validate Promo Message
            Dim WinningMessage As String
            WinningMessage = txtXML_WinningMsg01.Text & vbCrLf & _
                    txtXML_WinningMsg02.Text & vbCrLf & _
                    txtXML_WinningMsg03.Text & vbCrLf & _
                    txtXML_WinningMsg04.Text & vbCrLf & _
                    txtXML_WinningMsg05.Text & vbCrLf & _
                    txtXML_WinningMsg06.Text & vbCrLf & _
                    txtXML_WinningMsg07.Text & vbCrLf & _
                    txtXML_WinningMsg08.Text & vbCrLf & _
                    txtXML_WinningMsg09.Text & vbCrLf & _
                    txtXML_WinningMsg10.Text & vbCrLf & _
                    txtXML_WinningMsg11.Text & vbCrLf & _
                    txtXML_WinningMsg12.Text & vbCrLf & _
                    txtXML_WinningMsg13.Text & vbCrLf & _
                    txtXML_WinningMsg14.Text & vbCrLf & _
                    txtXML_WinningMsg15.Text & vbCrLf & _
                    txtXML_WinningMsg16.Text & vbCrLf & _
                    txtXML_WinningMsg17.Text & vbCrLf & _
                    txtXML_WinningMsg18.Text & vbCrLf & _
                    txtXML_WinningMsg19.Text & vbCrLf & _
                    txtXML_WinningMsg20.Text
            If Replace(WinningMessage, vbCrLf, "") = "" Then
                blistErrorMsg.Items.Add("Winning Message is required.")
            End If

            Dim NonwinningMessage As String
            NonwinningMessage = txtXML_NonwinningMsg01.Text & vbCrLf & _
                    txtXML_NonwinningMsg02.Text & vbCrLf & _
                    txtXML_NonwinningMsg03.Text & vbCrLf & _
                    txtXML_NonwinningMsg04.Text & vbCrLf & _
                    txtXML_NonwinningMsg05.Text & vbCrLf & _
                    txtXML_NonwinningMsg06.Text & vbCrLf & _
                    txtXML_NonwinningMsg07.Text & vbCrLf & _
                    txtXML_NonwinningMsg08.Text & vbCrLf & _
                    txtXML_NonwinningMsg09.Text & vbCrLf & _
                    txtXML_NonwinningMsg10.Text & vbCrLf & _
                    txtXML_NonwinningMsg11.Text & vbCrLf & _
                    txtXML_NonwinningMsg12.Text & vbCrLf & _
                    txtXML_NonwinningMsg13.Text & vbCrLf & _
                    txtXML_NonwinningMsg14.Text & vbCrLf & _
                    txtXML_NonwinningMsg15.Text & vbCrLf & _
                    txtXML_NonwinningMsg16.Text & vbCrLf & _
                    txtXML_NonwinningMsg17.Text & vbCrLf & _
                    txtXML_NonwinningMsg18.Text & vbCrLf & _
                    txtXML_NonwinningMsg19.Text & vbCrLf & _
                    txtXML_NonwinningMsg20.Text
            If Replace(NonwinningMessage, vbCrLf, "") = "" Then
                blistErrorMsg.Items.Add("Nonwinning Message is required.")
            End If

            Dim POSMessage As String
            POSMessage = txtXML_POSMsg01.Text & vbCrLf & _
                    txtXML_POSMsg02.Text & vbCrLf & _
                    txtXML_POSMsg03.Text & vbCrLf & _
                    txtXML_POSMsg04.Text & vbCrLf & _
                    txtXML_POSMsg05.Text & vbCrLf & _
                    txtXML_POSMsg06.Text & vbCrLf & _
                    txtXML_POSMsg07.Text & vbCrLf & _
                    txtXML_POSMsg08.Text & vbCrLf & _
                    txtXML_POSMsg09.Text & vbCrLf & _
                    txtXML_POSMsg10.Text & vbCrLf & _
                    txtXML_POSMsg11.Text & vbCrLf & _
                    txtXML_POSMsg12.Text & vbCrLf & _
                    txtXML_POSMsg13.Text & vbCrLf & _
                    txtXML_POSMsg14.Text & vbCrLf & _
                    txtXML_POSMsg15.Text & vbCrLf & _
                    txtXML_POSMsg16.Text & vbCrLf & _
                    txtXML_POSMsg17.Text & vbCrLf & _
                    txtXML_POSMsg18.Text & vbCrLf & _
                    txtXML_POSMsg19.Text & vbCrLf & _
                    txtXML_POSMsg20.Text
            If Replace(POSMessage, vbCrLf, "") = "" Then
                blistErrorMsg.Items.Add("POS Message is required.")
            End If
        End If



        If ViewState("v_RequireAttachment") Then
            If lblFiles.Text = "" Then
                blistErrorMsg.Items.Add("Attachment is required for this type of promotion.")
            End If
        End If


        If CountShoulderingEntity(clsSession.CurrRequestID) = 0 And trXML_CompSponsorship.Visible = True Then
            If cboPromoType.SelectedValue <> 2 And cboPromoType.SelectedValue <> 294 Then
                blistErrorMsg.Items.Add("Shouldering Entity must be specified.")
                IsValidEntries = False
                Exit Function
            Else

            End If

        End If


        ' flag successful entry
        IsValidEntries = (blistErrorMsg.Items.Count = 0)

    End Function

    Private Sub FillCouponMessage(ByVal sMessage As String)

        Dim sMsgLines() As String = sMessage.Split(vbCrLf)
        'Dim i As Integer

        If sMsgLines.Length > 1 Then
            txtXML_CouponMsg01.Text = sMsgLines(0)
            txtXML_CouponMsg02.Text = sMsgLines(1)
            txtXML_CouponMsg03.Text = sMsgLines(2)
            txtXML_CouponMsg04.Text = sMsgLines(3)
            txtXML_CouponMsg05.Text = sMsgLines(4)
            txtXML_CouponMsg06.Text = sMsgLines(5)
            txtXML_CouponMsg07.Text = sMsgLines(6)
            txtXML_CouponMsg08.Text = sMsgLines(7)
            txtXML_CouponMsg09.Text = sMsgLines(8)
            txtXML_CouponMsg10.Text = sMsgLines(9)
            txtXML_CouponMsg11.Text = sMsgLines(10)
            txtXML_CouponMsg12.Text = sMsgLines(11)
            txtXML_CouponMsg13.Text = sMsgLines(12)
            txtXML_CouponMsg14.Text = sMsgLines(13)
            txtXML_CouponMsg15.Text = sMsgLines(14)
            txtXML_CouponMsg16.Text = sMsgLines(15)
            txtXML_CouponMsg17.Text = sMsgLines(16)
            txtXML_CouponMsg18.Text = sMsgLines(17)
            txtXML_CouponMsg19.Text = sMsgLines(18)
            txtXML_CouponMsg20.Text = sMsgLines(19)
        End If

    End Sub

    Private Function GetCouponMessageEntry() As String

        Dim ResultStr As String

        ResultStr = txtXML_CouponMsg01.Text & vbCrLf & _
                    txtXML_CouponMsg02.Text & vbCrLf & _
                    txtXML_CouponMsg03.Text & vbCrLf & _
                    txtXML_CouponMsg04.Text & vbCrLf & _
                    txtXML_CouponMsg05.Text & vbCrLf & _
                    txtXML_CouponMsg06.Text & vbCrLf & _
                    txtXML_CouponMsg07.Text & vbCrLf & _
                    txtXML_CouponMsg08.Text & vbCrLf & _
                    txtXML_CouponMsg09.Text & vbCrLf & _
                    txtXML_CouponMsg10.Text & vbCrLf & _
                    txtXML_CouponMsg11.Text & vbCrLf & _
                    txtXML_CouponMsg12.Text & vbCrLf & _
                    txtXML_CouponMsg13.Text & vbCrLf & _
                    txtXML_CouponMsg14.Text & vbCrLf & _
                    txtXML_CouponMsg15.Text & vbCrLf & _
                    txtXML_CouponMsg16.Text & vbCrLf & _
                    txtXML_CouponMsg17.Text & vbCrLf & _
                    txtXML_CouponMsg18.Text & vbCrLf & _
                    txtXML_CouponMsg19.Text & vbCrLf & _
                    txtXML_CouponMsg20.Text

        If Replace(ResultStr, vbCrLf, "") = "" Then ResultStr = ""

        GetCouponMessageEntry = ResultStr

    End Function

    Private Function SavePromotionInfo(Optional ByVal bSaveBasicInfoOnly As Boolean = False) As Boolean

        Dim strQuery As String = ""
        Dim sErrMess As String = ""
        Dim sPromoDesc As String = ""

        SavePromotionInfo = True

        blistErrorMsg.Items.Clear()

        If bSaveBasicInfoOnly Then
            SavePromotionInfo = True
        Else
            SavePromotionInfo = IsValidEntries()
        End If

        If Not SavePromotionInfo Then
            Exit Function
        End If

        '------------------------------
        ' check if new promo entry
        '------------------------------

        If clsSession.CurrPromoID = 0 Then
            ' create new promotion record with initial data
            'cjg2243 20250726
            strQuery = "INSERT INTO Promotions " & _
                        "(PromoTypeID, PeriodFrom, PeriodTo, PromoDesc, PercentDisc, RequestID"

            If tdDiscAmount.Visible = True Then
                strQuery = strQuery & ", DiscAmount"
            End If

            strQuery = strQuery & ") VALUES " & _
                "(0" & cboPromoType.SelectedValue & ", " & _
                "'" & clsSession.PeriodFrom.ToShortDateString & "', '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                "'" & Server.HtmlEncode(Replace(litMechanics.Text, "'", "''")) & "', " & _
                "0, 0" & clsSession.CurrRequestID

            If tdDiscAmount.Visible = True Then
                strQuery = strQuery & ", " & txtTPL_DiscAmount.Text
            End If

            strQuery = strQuery & "); " & _
                "SELECT CAST(scope_identity() AS bigint);"

            clsSession.CurrPromoID = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)
        End If

        ' PromoID should exist at this point
        If clsSession.CurrPromoID = 0 Then

            ' error inserting data
            blistErrorMsg.Items.Add("Error accessing database. Unable to save information.")

            SavePromotionInfo = False

        Else

            '**************************'
            ' update promotion table
            '**************************'
            Dim strQueryAddCondition As String = ""
            Dim strCompSponsorship As String = GetCompSponsorshipDetails()

            'rbs7281 Promo MW Update: Theres a seperate saving for Shouldering Entity 20251125
            'rbs7281 Promo MW Update: Should be retro-actively adjust the setup from prev version 20250108
            If trXML_CompSponsorship2.Visible = True Then
                If panDiscCharging.Visible Then
                    strQueryAddCondition = ", CompSponsorship = 0" & strCompSponsorship & " "
                Else
                    'TODO: retrieve default value in PromoTypes table
                    If trXML_Sponsorship.Visible = False Then ' cjg2243
                        strQueryAddCondition = ", CompSponsorship = 0 "
                    End If
                End If
            Else
                'Do Nothing
                strQueryAddCondition = ", CompSponsorship = 0" & strCompSponsorship & " "
            End If


            'NBS:20210412
            If panChargeableEntity.Visible Then
                strQueryAddCondition = ", TplChargeEntity = 0" & cboChargeableEntity.SelectedValue.ToString() & " "

                If trTplChargeEnt_VendorCode.Visible Then strQueryAddCondition &= ", TplChargeEnt_VendorCode = '" & txtTplChargeEnt_VendorCode.Text & "' "

                If trTplChargeEnt_DSsub.Visible Then
                    strQueryAddCondition &= ", TplChargeEnt_DSsub = 0" & txtTplChargeEnt_DSsub.Text & " "
                Else
                    strQueryAddCondition &= ", TplChargeEnt_DSsub = 0 "
                End If

                If trTplChargeEnt_BUsub.Visible Then
                    strQueryAddCondition &= ", TplChargeEnt_BUsub = 0" & txtTplChargeEnt_BUsub.Text & " "
                Else
                    strQueryAddCondition &= ", TplChargeEnt_BUsub = 0 "
                End If

            Else
                'TODO: retrieve default value in PromoTypes table
                strQueryAddCondition &= ", TplChargeEntity = 0, TplChargeEnt_VendorCode = '',  TplChargeEnt_DSsub = 0, TplChargeEnt_BUsub = 0 "
            End If
            If panTemplated.Visible Then

                If trTPL_NumMonths.Visible Then strQueryAddCondition = strQueryAddCondition & ", TplNumMonths = '" & txtTPL_NumMonths.Text.Replace("'", "''") & "' "
                If trTPL_BankList.Visible Then strQueryAddCondition &= ", TplBankList = '" & txtTPL_BankList.Text.Replace("'", "''") & "' "
                If trTpl_SubsidyRate.Visible Then strQueryAddCondition &= ", TplSubsidyRate = 0" & txtTpl_SubsidyRate.Text.Replace(",", "") & " "
                If trTPL_BrandNames.Visible Then strQueryAddCondition &= ", TplBrandNames = '" & txtTPL_BrandNames.Text.Replace("'", "''") & "' "
                If trTPL_RefMemo.Visible Then strQueryAddCondition &= ", TplRefMemo = '" & txtTPL_RefMemo.Text.Replace("'", "''") & "' "
                'If trTPL_ProcessType.Visible Then strQueryAddCondition &= ", TplProcessType = 0" & cboTPL_ProcessType.SelectedValue.ToString() & " " ' cjg2243

                If trTPL_PurchaseReq.Visible Then strQueryAddCondition &= ", TplPurchaseReq = 0" & cboTPL_PurchaseReq.SelectedValue.ToString() & " "
                If trTPL_ReqAmount.Visible Then strQueryAddCondition &= ", TplReqAmount = 0" & txtTPL_ReqAmount.Text.Replace(",", "") & " "

                '@@here_discountamount
                'If trTPL_DiscAmount.Visible Then strQueryAddCondition &= ", PercentDisc = 0, DiscAmount = 0" & txtTPL_DiscAmount.Text & " "
                If trTPL_PercentDisc.Visible Then strQueryAddCondition &= ", TplPercentDisc = 0" & txtTPL_PercentDisc.Text & " "

                ' 10-DEC-2019 Any X for P (SMAC Triggered)
                'If trTPL_DiscAmount.Visible Then strQueryAddCondition &= ", PercentDisc = 0, DiscAmount = 0" & txtTPL_DiscAmount.Text & " "
                If trTPL_DiscAmount.Visible Then
                    txtXML_DiscAmount.Text = txtTPL_DiscAmount.Text
                    'strQueryAddCondition &= ",  PercentDisc = 0, DiscAmount = 0" & txtTPL_DiscAmount.Text & " "
                End If

                'NBS20190822
                If trTPL_TransDesc.Visible Then strQueryAddCondition &= ", TplTransDesc = '" & txtTPL_TransDesc.Text.Replace("'", "''") & "' "

                If trTPL_FreeItems.Visible Then strQueryAddCondition &= ", TplFreeItems = '" & txtTPL_FreeItems.Text.Replace("'", "''") & "' "
                If trTPL_Prizes.Visible Then strQueryAddCondition &= ", TplPrizes = '" & txtTPL_Prizes.Text.Replace("'", "''") & "' "
                If trTPL_DeptName.Visible Then strQueryAddCondition &= ", TplDeptName = '" & txtTPL_DeptName.Text.Replace("'", "''") & "' "
                If trTPL_SellingArea.Visible Then strQueryAddCondition &= ", TplSellingArea = '" & txtTPL_SellingArea.Text & "' "
                If trTPL_EventTime.Visible Then strQueryAddCondition &= ", TplEventTime = '" & txtTPL_EventTime.Text.Replace("'", "''") & "' "
                If trTPL_ItemName.Visible Then strQueryAddCondition &= ", TplItemName = '" & txtTPL_ItemName.Text.Replace("'", "''") & "' "
                If trTPL_CelebName.Visible Then strQueryAddCondition &= ", TplCelebName = '" & txtTPL_CelebName.Text.Replace("'", "''") & "' "
                If trTPL_BonusPoints.Visible Then strQueryAddCondition &= ", TplBonusPoints = '" & txtTPL_BonusPoints.Text.Replace("'", "''") & "' "

                If trTPL_ActivityName.Visible Then strQueryAddCondition &= ", TplActivityName = '" & txtTPL_ActivityName.Text.Replace("'", "''") & "' "
                If trTPL_BuyQty.Visible Then strQueryAddCondition &= ", TplBuyQty = 0" & txtTPL_BuyQty.Text & " "
                If trTPL_TakeQty.Visible Then strQueryAddCondition &= ", TplTakeQty = 0" & txtTPL_TakeQty.Text & " "

                If trPOS_StdExclusion.Visible Then strQueryAddCondition &= ", PosStdExclusion = 0" & IIf(chkPOS_StdExclusion.Checked, "1", "0") & " "
                If trPOS_PermExclusion.Visible Then strQueryAddCondition &= ", PosPermExclusion = 0" & IIf(chkPOS_PermExclusion.Checked, "1", "0") & " "

                ' cjg2243
                If trTPL_discountcapping.Visible Then strQueryAddCondition &= ", PosMaxAmnt = 0" & txtTPL_discountcapping.Text & " "
                If trTPL_percentage.Visible Then strQueryAddCondition &= ", PercentDisc = 0" & txtTPL_percentage.Text & " "
                If trTPL_MinAmount.Visible Then strQueryAddCondition &= ", PosMinAmnt = 0" & txtTPL_MinAmount.Text & " "
                If trXML_Sponsorship.Visible Then strQueryAddCondition &= ", CompSponsorship = 0" & cboXML_Sponsorship.SelectedValue & " "
                If trTPL_ProcessType.Visible Then strQueryAddCondition &= ", TplProcessType = 0" & cboTPL_ProcessType.SelectedValue & " "

                If panBankBins.Visible Then
                    Dim strBankBinQry As New ArrayList
                    strBankBinQry.Add("DELETE PromoBankBins WHERE PromoID=" & clsSession.CurrPromoID.ToString())

                    Dim row As GridViewRow
                    For Each row In gridPromoBankBins.Rows
                        Dim chkSel As CheckBox
                        chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                        If chkSel.Checked Then
                            Dim sCardName As String = HttpUtility.HtmlDecode(row.Cells(2).Text)
                            Dim sBankBinID As String = HttpUtility.HtmlDecode(row.Cells(3).Text)
                            Dim sCardBIN As String = HttpUtility.HtmlDecode(row.Cells(5).Text)
                            Dim sCardPanLow As String = HttpUtility.HtmlDecode(row.Cells(6).Text)
                            Dim sCardPanHigh As String = HttpUtility.HtmlDecode(row.Cells(7).Text)
                            Dim sCardPanLength As String = HttpUtility.HtmlDecode(row.Cells(8).Text)
                            Dim strIns = "INSERT INTO PromoBankBins (PromoID, CardName, CardBIN, CardPanLow, CardPanHigh, CardPanLength) " _
                                & "VALUES (" & clsSession.CurrPromoID.ToString() & ",'" _
                                & sCardName & "','" & sBankBinID & "','" & sCardPanLow & "','" & sCardPanHigh & "'" _
                                & sCardPanLength _
                                & ",'" _
                                & "'" _
                                & ")"
                            strBankBinQry.Add(strIns)
                        End If
                    Next

                    For Each s As String In strBankBinQry
                        clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, s, sErrMess)
                    Next
                End If
            End If

            '@@here::PromoNotes
            If trTPL_PromoNotes.Visible Then strQueryAddCondition &= ", TplPromoNotes = '" & Server.HtmlEncode(litTPLPromoNotes.Text.Replace("'", "''")) & "' "

            '@@here::PosQualifiedItems
            If trXML_QualifiedItems.Visible Then strQueryAddCondition &= ", PosQualifiedItems = " & cboXML_QualifiedItems.SelectedValue & " "

            '@@here::PosPromoSubType
            If trXML_PosPromoSubType.Visible Then strQueryAddCondition &= ", PosPromoSubType = " & cboXML_PosPromoSubType.SelectedValue & " "


            If panSwipestakes.Visible Then
                '@@here::PosPromoSubType
                If trXML_TenderType.Visible Then strQueryAddCondition &= ", PosTenderType = " & cboXML_TenderType.SelectedValue & " "
                CheckBinRange(cboXML_TenderType.SelectedValue)
                SavePromotionMessage(clsSession.CurrPromoID)
                If blistErrorMsg.Items.Count > 0 Then
                    SavePromotionInfo = False
                    Exit Function
                End If
            End If

            If panBarcode.Visible Then
                If trXML_Barcode.Visible Then strQueryAddCondition &= ", PosBarcode = '" & txtXML_Barcode.Text & "' "
            End If


            If panMarkdown.Visible Then

                '=========================================
                ' Markdown Promo
                '=========================================

                ' remove excess percent symbol
                sPromoDesc = Replace(Trim(txtMarkdown.Text) & lblMarkdownDesc.Text, "%%", "%")
                sPromoDesc = Replace(Server.HtmlEncode(sPromoDesc), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                            "DiscAmount = 0 " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = " & clsSession.CurrPromoID

            ElseIf panClassDiscount.Visible Then

                '=========================================
                ' Class Discount
                '=========================================
                sPromoDesc = Replace(Server.HtmlEncode(Trim(txtDiscount.Text) & lblDiscount.Text), "'", "''")

                clsSession.PercentDisc = CInt(txtDiscount.Text)

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                            "DiscAmount = 0 " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = " & clsSession.CurrPromoID

            ElseIf panSMACdeals.Visible Then

                '=========================================
                ' SMAC Deals
                '=========================================

                ReComputeValues()

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                            "DiscAmount = 0" & ViewState("DiscAmount") & ", " & _
                            "OriginalValue = 0" & ViewState("OrigValue") & ", " & _
                            "Allocation = 0" & ViewState("Allocation") & ", " & _
                            "OnlineSellingStart = '" & txtOnlineSellingStart.Text & "', " & _
                            "OnlineSellingEnd = '" & txtOnlineSellingEnd.Text & "', " & _
                            "VendorType = '" & Left(cboVendorType.SelectedItem.Text, 1) & "', " & _
                            "WebAdRate = 0" & ViewState("WAPrate") & ", " & _
                            "PromoBudget = 0" & ViewState("PromoBudget") & ", " & _
                            "WebAdPlacement = 0" & ViewState("WebAdPlacement") & " " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = " & clsSession.CurrPromoID

            ElseIf panBuy1Take1.Visible Then

                '=========================================
                ' Special Promotions - 20141204 NBSantos
                '=========================================

                Dim nBuyQty As Integer = Val(txtB1T1_BuyQty.Text)
                Dim nTakeQty As Integer = Val(txtB1T1_TakeQty.Text)
                Dim nPercentDisc As Integer = Val(txtB1T1_PercentDisc.Text)

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = nPercentDisc

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                            "DiscAmount = 0, " & _
                            "Element1 = NULL, " & _
                            "Element2 = NULL, " & _
                            "Element3 = 50.00 " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panAnyXForP.Visible Then

                Dim nPurchaseQty As Integer = Val(txtAnyX4P_BuyQty.Text.Replace(",", ""))
                Dim nPromoPrice As Single = Val(txtAnyX4P_PromoPrice.Text.Replace(",", ""))

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                            "DiscAmount = " & nPromoPrice & ", " & _
                            "Element1 = 0" & nPurchaseQty & ", " & _
                            "Element2 = NULL, " & _
                            "Element3 = 0" & nPromoPrice & " " & _
                            IIf(trXML_EligibleCards.Visible, _
                            ",PosEligibleCards = '" & EligibleCardsEncode() & "'", " ") & _
                            strQueryAddCondition & _
                            IIf(trXML_QualifiedCust.Visible, ", PosQualifiedCust = '" & txtQualifiedCust.Text & "' ", "") & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panBuyGetSameForP.Visible Then

                Dim nBuyQty As Integer = Val(txtBG4Ps_BuyQty.Text.Replace(",", ""))
                Dim nTakeQty As Integer = Val(txtBG4Ps_TakeQty.Text.Replace(",", ""))
                Dim nDiscAmount As Single = Val(txtBG4Ps_DiscAmount.Text.Replace(",", ""))

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0, " & _
                            "DiscAmount = 0" & nDiscAmount & ", " & _
                            "Element1 = 0" & nTakeQty & ", " & _
                            "Element2 = 0" & nBuyQty + nTakeQty & ", " & _
                            "Element3 = 0" & nDiscAmount & " " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panBuyGetDiffForP.Visible Then

                Dim nBuyQty As Integer = Val(txtBG4Pd_BuyQty.Text.Replace(",", ""))
                Dim nTakeQty As Integer = Val(txtBG4Pd_TakeQty.Text.Replace(",", ""))
                Dim nDiscAmount As Single = Val(txtBG4Pd_DiscAmount.Text.Replace(",", ""))

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0, " & _
                            "DiscAmount = 0" & nDiscAmount & ", " & _
                            "Element1 = 0" & nTakeQty & ", " & _
                            "Element2 = 0" & nBuyQty & ", " & _
                            "Element3 = 0" & nDiscAmount & " " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panBuyGetSamePercent.Visible Then

                Dim nBuyQty As Integer = Val(txtBGPerOffs_BuyQty.Text.Replace(",", ""))
                Dim nTakeQty As Integer = Val(txtBGPerOffs_TakeQty.Text.Replace(",", ""))
                Dim nPercentDisc As Single = CInt(txtBGPerOffs_PercentDisc.Text)

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = nPercentDisc

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & nPercentDisc & ", " & _
                            "DiscAmount = 0, " & _
                            "Element1 = 0" & nTakeQty & ", " & _
                            "Element2 = 0" & nBuyQty + nTakeQty & ", " & _
                            "Element3 = 0" & nPercentDisc & " " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panBuyGetDiffPercent.Visible Then

                Dim nBuyQty As Integer = Val(txtBGPerOffd_BuyQty.Text.Replace(",", ""))
                Dim nTakeQty As Integer = Val(txtBGPerOffd_TakeQty.Text.Replace(",", ""))
                Dim nPercentDisc As Single = CInt(txtBGPerOffd_PercentDisc.Text)

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = nPercentDisc

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & nPercentDisc & ", " & _
                            "DiscAmount = 0, " & _
                            "Element1 = 0" & nTakeQty & ", " & _
                            "Element2 = 0" & nBuyQty & ", " & _
                            "Element3 = 0" & nPercentDisc & " " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panAnyXYforP.Visible Then

                Dim nBuyQty As Integer = Val(txtAnyXY_BuyQty.Text.Replace(",", ""))
                Dim nPromoPrice As Single = Val(txtAnyXY_PromoPrice.Text.Replace(",", ""))

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                            "DiscAmount = 0" & nPromoPrice & ", " & _
                            "Element1 = 1, " & _
                            "Element2 = 1, " & _
                            "Element3 = NULL " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panSpecialPromo.Visible Then

                '=========================================
                ' Special Promotions - 20140822 NBSantos
                '=========================================

                Dim nPurchaseQty As Integer = Val(txtPurchaseQty.Text.Replace(",", ""))
                Dim nFreeQty As Integer = Val(txtFreeQty.Text.Replace(",", ""))
                Dim nPromoPrice As Single = Val(txtPromoPrice.Text.Replace(",", ""))

                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                            "DiscAmount = 0" & nPromoPrice & ", " & _
                            "Element1 = 0" & nPurchaseQty & ", " & _
                            "Element2 = 0" & nFreeQty & " " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

            ElseIf panGenericHostXML.Visible Then


                sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = CreateUpdateQueryForXML(strQueryAddCondition)
            Else

                ' save standard data
                clsSession.PercentDisc = 0

                strQuery = "UPDATE Promotions SET " & _
                            "RequestID = " & clsSession.CurrRequestID & ", " & _
                            "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                            "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                            "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                            "PromoDesc = '" & Replace(Server.HtmlEncode(litMechanics.Text), "'", "''") & "' " & _
                            strQueryAddCondition & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

                '"PercentDisc = 0" & clsSession.PercentDisc & ", " & _
                '"DiscAmount = 0 " & _

            End If






            '*******************'
            '** execute query **'
            '*******************'

            If strQuery <> "" Then
                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
                    blistErrorMsg.Items.Add("Error accessing database. Unable to save information.")
                    SavePromotionInfo = False
                    Exit Function
                End If

            End If

            '===============================================================================
            ' save XML Promotion Rules for HostXML transactions
            '===============================================================================

            If panGenericHostXML.Visible Then
                SavePromotionRules(clsSession.CurrPromoID)
                SavePromotionCondition(clsSession.CurrPromoID)

                If blistErrorMsg.Items.Count > 0 Then
                    SavePromotionInfo = False
                    Exit Function
                End If
            End If

            '===============================================================================
            ' save Plan Promo Sales and Cost if encoded - 20140521 NBSantos
            '===============================================================================

            If txtPlanPromoSales.Text <> "" And trPromoAnalytics.Visible Then
                Dim nPlanPromoSales As Double = Val(txtPlanPromoSales.Text.Replace(",", ""))
                Dim nPlanPromoCost As Double = Val(txtPlanPromoCost.Text.Replace(",", ""))
                Dim nPlanPercentMargin As Single = Val("0" & txtPlanMargin.Text.Replace(",", ""))

                strQuery = "UPDATE Promotions SET " & _
                            "PlanPromoSales = " & nPlanPromoSales & ", " & _
                            "PlanPromoCost = " & nPlanPromoCost & ", " & _
                            "PlanPercentMargin = " & nPlanPercentMargin & " " & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
                    blistErrorMsg.Items.Add("Error accessing database. Unable to save analytics information.")
                    SavePromotionInfo = False
                End If

            End If

            '--------------------------------
            ' save default workflowcode
            '--------------------------------
            'strQuery = "UPDATE PromoRequests SET " & _
            '            "WorkFlowCode = '" & ViewState("WorkFlowCode") & "' " & _
            '            "WHERE RequestID = 0" & clsSession.CurrRequestID
            strQuery = "UPDATE PromoRequests " & _
                        "SET WorkFlowCode = T.WorkFlowCode, " & _
                            "TemplatedGuideline = '&lt;p&gt;&lt;b&gt;' + T.TypeDesc + '&lt;/b&gt;&lt;br /&gt;' + CAST(T.DefaultGuideline AS VARCHAR(MAX)) + '&lt;/p&gt;' " & _
                        "FROM PromoTypes AS T " & _
                        "WHERE T.PromoTypeID = 0" & cboPromoType.SelectedValue & " " & _
                        "AND RequestID = 0" & clsSession.CurrRequestID

            If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
                blistErrorMsg.Items.Add("Error accessing database. Unable to save workflow information.")
                SavePromotionInfo = False
            End If

        End If
                                                                                                                    
        SaveSpecialHandling()

    End Function

    Private Sub SavePromotionMessage(ByVal nPromoID As Long)

        Dim strSQL As String
        Dim sErrMess As String = ""

        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "DELETE PromoMessages " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 1"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Error in saving Winning Messages: " & sErrMess)
            Exit Sub
        End If

        Dim WinningMessage As String

        WinningMessage = txtXML_WinningMsg01.Text & vbCrLf & _
                    txtXML_WinningMsg02.Text & vbCrLf & _
                    txtXML_WinningMsg03.Text & vbCrLf & _
                    txtXML_WinningMsg04.Text & vbCrLf & _
                    txtXML_WinningMsg05.Text & vbCrLf & _
                    txtXML_WinningMsg06.Text & vbCrLf & _
                    txtXML_WinningMsg07.Text & vbCrLf & _
                    txtXML_WinningMsg08.Text & vbCrLf & _
                    txtXML_WinningMsg09.Text & vbCrLf & _
                    txtXML_WinningMsg10.Text & vbCrLf & _
                    txtXML_WinningMsg11.Text & vbCrLf & _
                    txtXML_WinningMsg12.Text & vbCrLf & _
                    txtXML_WinningMsg13.Text & vbCrLf & _
                    txtXML_WinningMsg14.Text & vbCrLf & _
                    txtXML_WinningMsg15.Text & vbCrLf & _
                    txtXML_WinningMsg16.Text & vbCrLf & _
                    txtXML_WinningMsg17.Text & vbCrLf & _
                    txtXML_WinningMsg18.Text & vbCrLf & _
                    txtXML_WinningMsg19.Text & vbCrLf & _
                    txtXML_WinningMsg20.Text

        If Replace(WinningMessage, vbCrLf, "") = "" Then WinningMessage = ""

        strSQL = "INSERT INTO PromoMessages " & _
                    "(PromoID, MessageType, PromoMessage) " & _
                    "VALUES (" & _
                        nPromoID & ", " & _
                         "1, " & _
                        "'" & WinningMessage.ToUpper() & "')"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to update promotion condition: " & sErrMess)
            Exit Sub
        End If
        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "DELETE PromoMessages " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 2"

        Dim NonwinningMessage As String

        NonwinningMessage = txtXML_NonwinningMsg01.Text & vbCrLf & _
                    txtXML_NonwinningMsg02.Text & vbCrLf & _
                    txtXML_NonwinningMsg03.Text & vbCrLf & _
                    txtXML_NonwinningMsg04.Text & vbCrLf & _
                    txtXML_NonwinningMsg05.Text & vbCrLf & _
                    txtXML_NonwinningMsg06.Text & vbCrLf & _
                    txtXML_NonwinningMsg07.Text & vbCrLf & _
                    txtXML_NonwinningMsg08.Text & vbCrLf & _
                    txtXML_NonwinningMsg09.Text & vbCrLf & _
                    txtXML_NonwinningMsg10.Text & vbCrLf & _
                    txtXML_NonwinningMsg11.Text & vbCrLf & _
                    txtXML_NonwinningMsg12.Text & vbCrLf & _
                    txtXML_NonwinningMsg13.Text & vbCrLf & _
                    txtXML_NonwinningMsg14.Text & vbCrLf & _
                    txtXML_NonwinningMsg15.Text & vbCrLf & _
                    txtXML_NonwinningMsg16.Text & vbCrLf & _
                    txtXML_NonwinningMsg17.Text & vbCrLf & _
                    txtXML_NonwinningMsg18.Text & vbCrLf & _
                    txtXML_NonwinningMsg19.Text & vbCrLf & _
                    txtXML_NonwinningMsg20.Text

        If Replace(NonwinningMessage, vbCrLf, "") = "" Then NonwinningMessage = ""


        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Error in saving Winning Messages: " & sErrMess)
            Exit Sub
        End If

        strSQL = "INSERT INTO PromoMessages " & _
                    "(PromoID, MessageType, PromoMessage) " & _
                    "VALUES (" & _
                        nPromoID & ", " & _
                         "2, " & _
                        "'" & NonwinningMessage.ToUpper() & "')"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to update promotion condition: " & sErrMess)
            Exit Sub
        End If
        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "DELETE PromoMessages " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 3"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Error in saving Winning Messages: " & sErrMess)
            Exit Sub
        End If

        Dim POSMessage As String

        POSMessage = txtXML_POSMsg01.Text & vbCrLf & _
                    txtXML_POSMsg02.Text & vbCrLf & _
                    txtXML_POSMsg03.Text & vbCrLf & _
                    txtXML_POSMsg04.Text & vbCrLf & _
                    txtXML_POSMsg05.Text & vbCrLf & _
                    txtXML_POSMsg06.Text & vbCrLf & _
                    txtXML_POSMsg07.Text & vbCrLf & _
                    txtXML_POSMsg08.Text & vbCrLf & _
                    txtXML_POSMsg09.Text & vbCrLf & _
                    txtXML_POSMsg10.Text & vbCrLf & _
                    txtXML_POSMsg11.Text & vbCrLf & _
                    txtXML_POSMsg12.Text & vbCrLf & _
                    txtXML_POSMsg13.Text & vbCrLf & _
                    txtXML_POSMsg14.Text & vbCrLf & _
                    txtXML_POSMsg15.Text & vbCrLf & _
                    txtXML_POSMsg16.Text & vbCrLf & _
                    txtXML_POSMsg17.Text & vbCrLf & _
                    txtXML_POSMsg18.Text & vbCrLf & _
                    txtXML_POSMsg19.Text & vbCrLf & _
                    txtXML_POSMsg20.Text

        If Replace(POSMessage, vbCrLf, "") = "" Then POSMessage = ""

        strSQL = "INSERT INTO PromoMessages " & _
                    "(PromoID, MessageType, PromoMessage) " & _
                    "VALUES (" & _
                        nPromoID & ", " & _
                         "3, " & _
                        "'" & POSMessage.ToUpper() & "')"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to update promotion condition: " & sErrMess)
            Exit Sub
        End If

    End Sub
    Private Sub CheckBinRange(ByVal selectedIndex As Integer)

        Dim strSQL As String
        Dim sErrMess As String = ""

        If Not selectedIndex = 2 Then
            ' '' ---------------------------------------------------------------------------------------------
            strSQL = "DELETE PromoBinRange " & _
                        "WHERE RequestID = " & clsSession.CurrRequestID

            If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
                blistErrorMsg.Items.Add("Error in saving Promo Bin Range" & sErrMess)
                Exit Sub
            End If
            ' '' ---------------------------------------------------------------------------------------------
        End If
    End Sub

    Private Sub LoadPromotionMessage(ByVal nPromoID As Long)

        Dim strSQL As String
        Dim drPromoMessage As DataRow = Nothing

        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "SELECT PM.PromoMessage FROM PromoMessages PM " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 1"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drPromoMessage) Then
            Dim sMsgLines() As String = drPromoMessage("PromoMessage").Split(vbCrLf)
            If sMsgLines.Length > 1 Then
                'For i As Integer = 1 To sMsgLines.Length
                '    Dim tempStr As String = IIf(i < 10, "0" & i.ToString(), i)
                '    txtXML_WinningMsg()
                'Next
                txtXML_WinningMsg01.Text = sMsgLines(0)
                txtXML_WinningMsg02.Text = sMsgLines(1)
                txtXML_WinningMsg03.Text = sMsgLines(2)
                txtXML_WinningMsg04.Text = sMsgLines(3)
                txtXML_WinningMsg05.Text = sMsgLines(4)
                txtXML_WinningMsg06.Text = sMsgLines(5)
                txtXML_WinningMsg07.Text = sMsgLines(6)
                txtXML_WinningMsg08.Text = sMsgLines(7)
                txtXML_WinningMsg09.Text = sMsgLines(8)
                txtXML_WinningMsg10.Text = sMsgLines(9)
                txtXML_WinningMsg11.Text = sMsgLines(10)
                txtXML_WinningMsg12.Text = sMsgLines(11)
                txtXML_WinningMsg13.Text = sMsgLines(12)
                txtXML_WinningMsg14.Text = sMsgLines(13)
                txtXML_WinningMsg15.Text = sMsgLines(14)
                txtXML_WinningMsg16.Text = sMsgLines(15)
                txtXML_WinningMsg17.Text = sMsgLines(16)
                txtXML_WinningMsg18.Text = sMsgLines(17)
                txtXML_WinningMsg19.Text = sMsgLines(18)
                txtXML_WinningMsg20.Text = sMsgLines(19)
            End If
        End If
        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "SELECT PM.PromoMessage FROM PromoMessages PM " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 2"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drPromoMessage) Then
            Dim sMsgLines() As String = drPromoMessage("PromoMessage").Split(vbCrLf)
            If sMsgLines.Length > 1 Then
                txtXML_NonwinningMsg01.Text = sMsgLines(0)
                txtXML_NonwinningMsg02.Text = sMsgLines(1)
                txtXML_NonwinningMsg03.Text = sMsgLines(2)
                txtXML_NonwinningMsg04.Text = sMsgLines(3)
                txtXML_NonwinningMsg05.Text = sMsgLines(4)
                txtXML_NonwinningMsg06.Text = sMsgLines(5)
                txtXML_NonwinningMsg07.Text = sMsgLines(6)
                txtXML_NonwinningMsg08.Text = sMsgLines(7)
                txtXML_NonwinningMsg09.Text = sMsgLines(8)
                txtXML_NonwinningMsg10.Text = sMsgLines(9)
                txtXML_NonwinningMsg11.Text = sMsgLines(10)
                txtXML_NonwinningMsg12.Text = sMsgLines(11)
                txtXML_NonwinningMsg13.Text = sMsgLines(12)
                txtXML_NonwinningMsg14.Text = sMsgLines(13)
                txtXML_NonwinningMsg15.Text = sMsgLines(14)
                txtXML_NonwinningMsg16.Text = sMsgLines(15)
                txtXML_NonwinningMsg17.Text = sMsgLines(16)
                txtXML_NonwinningMsg18.Text = sMsgLines(17)
                txtXML_NonwinningMsg19.Text = sMsgLines(18)
                txtXML_NonwinningMsg20.Text = sMsgLines(19)
            End If
        End If
        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "SELECT PM.PromoMessage FROM PromoMessages PM " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 3"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drPromoMessage) Then
            Dim sMsgLines() As String = drPromoMessage("PromoMessage").Split(vbCrLf)
            If sMsgLines.Length > 1 Then
                txtXML_POSMsg01.Text = sMsgLines(0)
                txtXML_POSMsg02.Text = sMsgLines(1)
                txtXML_POSMsg03.Text = sMsgLines(2)
                txtXML_POSMsg04.Text = sMsgLines(3)
                txtXML_POSMsg05.Text = sMsgLines(4)
                txtXML_POSMsg06.Text = sMsgLines(5)
                txtXML_POSMsg07.Text = sMsgLines(6)
                txtXML_POSMsg08.Text = sMsgLines(7)
                txtXML_POSMsg09.Text = sMsgLines(8)
                txtXML_POSMsg10.Text = sMsgLines(9)
                txtXML_POSMsg11.Text = sMsgLines(10)
                txtXML_POSMsg12.Text = sMsgLines(11)
                txtXML_POSMsg13.Text = sMsgLines(12)
                txtXML_POSMsg14.Text = sMsgLines(13)
                txtXML_POSMsg15.Text = sMsgLines(14)
                txtXML_POSMsg16.Text = sMsgLines(15)
                txtXML_POSMsg17.Text = sMsgLines(16)
                txtXML_POSMsg18.Text = sMsgLines(17)
                txtXML_POSMsg19.Text = sMsgLines(18)
                txtXML_POSMsg20.Text = sMsgLines(19)
            End If
        End If
        ' '' ---------------------------------------------------------------------------------------------
    End Sub
    Private Sub LoadPromoBinRange(ByVal nRequestID As Long)
        Dim drPromoSeed As DataRow = Nothing

        ' '' ---------------------------------------------------------------------------------------------
        sqldsData.SelectCommand = "SELECT RLV.ElementName, PBR.BIN, PBR.PanLow, PBR.PanHigh, PBR.PanLength FROM PromoBinRange PBR LEFT JOIN ResListValues RLV ON PBR.RangeType = RLV.ElementID AND RLV.GroupName = 'RangeType'" & _
                    "WHERE RequestID = " & nRequestID & ""
        Dim dvPromoBinRange As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
        If IsNothing(dvPromoBinRange) Or dvPromoBinRange.Count = 0 Then
            dvPromoBinRange.AddNew()
            divBinRange.Attributes.Add("style", "height:100%")
        Else
            divBinRange.Attributes.Add("style", "height:250px; overflow:auto")
        End If

        gvBinRange.DataSource = dvPromoBinRange
        gvBinRange.DataBind()

        ' '' ---------------------------------------------------------------------------------------------
    End Sub

    'rbs7281 20250814 Rebate Load Bin Range update
    Private Sub LoadRebatePromoBinRange(ByVal nRequestID As Long)
        ' '' ---------------------------------------------------------------------------------------------
        sqldsData.SelectCommand = "SELECT RLV.ElementName, PBR.BIN, PBR.PanLow, PBR.PanHigh, PBR.PanLength FROM PromoBinRange PBR LEFT JOIN ResListValues RLV ON PBR.RangeType = RLV.ElementID AND RLV.GroupName = 'RangeType'" & _
                    "WHERE RequestID = " & nRequestID & ""
        Dim dvPromoRebateBinRange As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
        If IsNothing(dvPromoRebateBinRange) Or dvPromoRebateBinRange.Count = 0 Then
            dvPromoRebateBinRange.AddNew()
            divBinRange.Attributes.Add("style", "height:100%")
        Else
            divBinRange.Attributes.Add("style", "height:250px; overflow:auto")
        End If

        gvRebateBinRange.DataSource = dvPromoRebateBinRange
        gvRebateBinRange.DataBind()

        ' '' ---------------------------------------------------------------------------------------------
    End Sub

    Private Sub LoadPromoSeed(ByVal nRequestID As Long)
        Dim drPromoSeed As DataRow = Nothing

        ' '' ---------------------------------------------------------------------------------------------
        sqldsData.SelectCommand = "SELECT SUBSTRING(PS.CompBranch,4,4) +' - '+ B.BranchName AS Branch, PS.StartDate, PS.EndDate, PS.MaxNumber, PS.Counter, PS.Prize FROM PromoSeed PS " & _
                    " LEFT JOIN Branches B " & _
                    " ON SUBSTRING(PS.CompBranch,4,4) = RIGHT('0000'+ISNULL(CAST(B.BranchCode AS nvarchar), ''),4) " & _
                    " WHERE RequestID = " & nRequestID & "" & _
                    " ORDER BY B.BranchName"
        Dim dvPromoSeed As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
        If IsNothing(dvPromoSeed) Or dvPromoSeed.Count = 0 Then
            dvPromoSeed.AddNew()
            divSeeding.Attributes.Add("style", "height:100%")
        Else
            divSeeding.Attributes.Add("style", "height:250px; overflow:auto")
        End If

        gvSeeding.DataSource = dvPromoSeed
        gvSeeding.DataBind()
        ' '' ---------------------------------------------------------------------------------------------
    End Sub

    Private Sub SavePromotionCondition(ByVal nPromoID As Long)

        Dim strSQL As String
        Dim sErrMess As String = ""

        ' delete rules
        strSQL = "DELETE PromotionConditions " & _
                    "WHERE PromoID = 0" & nPromoID

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to update promotion condition: " & sErrMess)
            Exit Sub
        End If

        ' get discount value
        Dim nDiscountValue As Single = 0

        If txtXML_DiscAmount.Text <> "" Then
            nDiscountValue = CSng(txtXML_DiscAmount.Text)
        End If

        If txtXML_PercentDisc.Text <> "" Then
            nDiscountValue = CInt(txtXML_PercentDisc.Text)
        End If

        ' replace with updated data
        strSQL = "INSERT INTO PromotionConditions " & _
                    "(PromoID, DiscCond, DiscAmnt1, SequenceNo) " & _
                    "SELECT " & nPromoID & ", " & _
                    "CASE WHEN DiscCond_STATE = 30 THEN " & cboXML_Condition1.SelectedValue & " ELSE DiscCond END, " & _
                    "CASE WHEN Condition_STATE = 30 THEN " & nDiscountValue & " ELSE DiscAmnt1 END, " & _
                    "SequenceNo " & _
                    "FROM PromoTypeConditions " & _
                    "WHERE PromoTypeID = " & cboPromoType.SelectedValue

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to update promotion condition: " & sErrMess)
            Exit Sub
        End If


    End Sub

    Private Sub SavePromotionRules(ByVal nPromoID As Long)

        Dim strSQL As String
        Dim sErrMess As String = ""

        ' delete rules
        strSQL = "DELETE PromotionRules " & _
                    "WHERE PromoID = 0" & nPromoID

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to update promotion rules: " & sErrMess)
            Exit Sub
        End If

        ' replace with updated data
        strSQL = "INSERT INTO PromotionRules " & _
                    "(PromoID, RuleType, RuleCondition, ValueType, RuleValue, SequenceNo) " & _
                    "SELECT " & nPromoID & ", RuleType, RuleCondition, ValueType, RuleValue, SequenceNo " & _
                    "FROM PromoTypeRules WHERE PromoTypeID = " & cboPromoType.SelectedValue

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to update promotion rules: " & sErrMess)
            Exit Sub
        End If

    End Sub
    Private Function GetPromoTypeInfo(ByVal id As Integer) As DataRow
        Dim sQuery As String
        Dim drPromoType As DataRow = Nothing

        sQuery = "SELECT * FROM PromoTypes " & _
                 "WHERE PromoTypeID = 0" & cboPromoType.SelectedValue

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sQuery, drPromoType) Then
            Return drPromoType
        End If

        Return Nothing
    End Function
    Private Function CreateUpdateQueryForXML(ByVal AdditionalSetColumnQuery As String) As String

        Dim sQuery As String
        Dim drPromoType As DataRow = Nothing

        sQuery = "SELECT * FROM PromoTypes " & _
                    "WHERE PromoTypeID = 0" & cboPromoType.SelectedValue

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sQuery, drPromoType) Then

            Dim sAssignedVSPromoID As String = "0"
            Dim nMaxAmount As Single = Val(txtXML_MaxAmount.Text.Replace(",", ""))
            Dim nXML_RuleValue2 As Single = Val(Replace(txtXML_RuleValue2.Text, ",", ""))   ' minimum amount

            Dim sActiveDays As String
            Dim sPromoDesc As String

            sActiveDays = IIf(chkMonday.Checked, "M", "") & _
                            IIf(chkTuesday.Checked, "T", "") & _
                            IIf(chkWednesday.Checked, "W", "") & _
                            IIf(chkThursday.Checked, "H", "") & _
                            IIf(chkFriday.Checked, "F", "") & _
                            IIf(chkSaturday.Checked, "S", "") & _
                            IIf(chkSunday.Checked, "N", "")

            ' correct single quotes
            sPromoDesc = Replace(Server.HtmlEncode(litMechanics.Text), "'", "''")

            'MALAgasino 20180907 - Customize promodesc for Sale Event promo category. Start of Code
            Dim tempPromoDesc As String = ""
            If Not IsDBNull(drPromoType("PromoDescTempo")) And drPromoType("GroupType") = "SBU" Then
                tempPromoDesc = drPromoType("PromoDescTempo").ToString.Replace("@EventNameTempo", txtTPL_ActivityName.Text)
                tempPromoDesc = tempPromoDesc.Replace("@PercentDiscTempo", txtTPL_PercentDisc.Text)
            ElseIf Not IsDBNull(drPromoType("PromoDescTempo")) And drPromoType("GroupType") = "REGULAR" Then
                Dim HasMinimumAmount As String = String.Empty

                If txtXML_MinAmount.Text = "" Then
                    HasMinimumAmount = "0"
                Else
                    HasMinimumAmount = txtXML_MinAmount.Text
                End If
                tempPromoDesc = drPromoType("PromoDescTempo").ToString.Replace("@DiscAmtTemp", FormatMoneyField(HasMinimumAmount))
            End If


            '' LTE 20220406 1423 - Added condition for MPR from RuleValue
            Dim purchaseRequirementAmount As String = RetrievePurchaseRequirementAmount(drPromoType)

            'MALAgasino 20180907 - Customize proddesc for Sale Event promo category. End of Code

            Dim CappedAmount As String = "0"
            Dim DiscCappedField As String = "POS_MaxAmnt_STATE" 'Mantis#64950
            Dim DiscCappedCheckValue As String = "POS_MaxAmnt"

            'rbs7281 07/29/2025
            If (chk_DiscCapTickBox.Checked = True And txtXML_MinAmount.Visible = True) Then 'Mantis65127

                CappedAmount = txtXML_DiscCap.Text
                If CInt(purchaseRequirementAmount) < Convert.ToDouble(txtXML_DiscCap.Text) Then
                    blistErrorMsg.Items.Add("Discount capped amount should be less or equal to the MPR")
                End If

            End If

            If drPromoType("TPL_ReqAmount_STATE") > 10 Then
                If chk_DiscCapTickBox.Checked = True Then
                    CappedAmount = txtXML_DiscCap.Text
                    If Convert.ToDouble(txtTPL_ReqAmount.Text) < Convert.ToDouble(IIf(IsNumeric(txtXML_DiscCap.Text), txtXML_DiscCap.Text, "0")) Then
                        blistErrorMsg.Items.Add("Discount capped amount should be less or equal to the MPR")
                    End If
                End If
            End If


            If chk_DiscCapTickBox.Checked = True Then 'Mantis#64786 rbs7281 7/23/2025
                DiscCappedField = "TPL_DiscountCappedChk_STATE" 'Mantis#64786 rbs7281 7/23/2025
                DiscCappedCheckValue = "TPL_DiscountCappedChk"


                CappedAmount = txtXML_DiscCap.Text

            End If


            Dim valPriority As String
            If SystemUser.UserGroupType = "SBU" Then
                valPriority = GetXMLfieldValue(drPromoType("POS_Priority_SBU_STATE"), cboXML_Priority.SelectedValue, drPromoType("POS_Priority_SBU"))
            Else
                valPriority = GetXMLfieldValue(drPromoType("POS_Priority_STATE"), cboXML_Priority.SelectedValue, drPromoType("POS_Priority"))
            End If




            ' define query string
            sQuery = "UPDATE Promotions SET " & _
                        "RequestID = " & clsSession.CurrRequestID & ", " & _
                        "PromoTypeID = 0" & cboPromoType.SelectedValue & ", " & _
                        "PeriodFrom = '" & clsSession.PeriodFrom.ToShortDateString & "', " & _
                        "PeriodTo = '" & clsSession.PeriodTo.ToShortDateString & "', " & _
                        "PosActionType = " & GetXMLfieldValue(drPromoType("POS_ActionType_STATE"), cboXML_ActionType.SelectedValue, drPromoType("POS_ActionType")) & ", " & _
                        "PosCompressID = " & GetXMLfieldValue(drPromoType("POS_CompressID_STATE"), txtXML_CompessID.Text, drPromoType("POS_CompressID")) & ", " & _
                        "PosExclType = " & GetXMLfieldValue(drPromoType("POS_ExclType_STATE"), cboXML_ExclusionType.SelectedValue, drPromoType("POS_ExclType")) & ", " & _
                        "PosPriority = " & valPriority.ToString() & ", " & _
                        "PosMessageFlag = " & IIf(Len(GetCouponMessageEntry.Replace(vbCrLf, "")) = 0, "0", "1") & ", " & _
                        "PosMessageID = 0, " & _
                        "PosActiveDays = " & GetXMLfieldValue(drPromoType("POS_Activedays_STATE"), sActiveDays, drPromoType("POS_Activedays"), True) & ", " & _
                        "PosActime = " & GetXMLfieldValue(drPromoType("POS_Actime_STATE"), cboXML_StartTime.Text, drPromoType("POS_Actime"), True) & ", " & _
                        "PosDeactime = " & GetXMLfieldValue(drPromoType("POS_Actime_STATE"), cboXML_EndTime.Text, drPromoType("POS_Deactime"), True) & ", " & _
                        "PosDescr1prm = " & GetXMLfieldValue(drPromoType("POS_Descr1prm_STATE"), txtXML_Descr1prm.Text, drPromoType("POS_Descr1prm"), True) & ", " & _
                        "PosDescr2prm = " & GetXMLfieldValue(drPromoType("POS_Descr2prm_STATE"), txtXML_Descr2prm.Text, drPromoType("POS_Descr2prm"), True) & ", " & _
                        "PosInfotext1 = " & GetXMLfieldValue(drPromoType("POS_InfoText1_STATE"), txtXML_ReceiptDesc1.Text, drPromoType("POS_InfoText1"), True) & ", " & _
                        "PosInfotext2 = " & GetXMLfieldValue(drPromoType("POS_InfoText2_STATE"), txtXML_ReceiptDesc2.Text, drPromoType("POS_InfoText2"), True) & ", " & _
                        "PosMaxQty = " & GetXMLfieldValue(drPromoType("POS_MaxQty_STATE"), txtXML_MaxQty.Text, drPromoType("POS_MaxQty")) & ", " & _
                        "PosMaxAmnt = " & GetXMLfieldValue(drPromoType(DiscCappedField).ToString(), IIf(IsNumeric(CappedAmount), Convert.ToDouble(CappedAmount), "0"), drPromoType(DiscCappedCheckValue).ToString()) & ", " & _
                        "PosMinAmnt = " & purchaseRequirementAmount & ", " & _
                        "PosProcType = " & GetXMLfieldValue(drPromoType("POS_ProcType_STATE"), cboXML_ProcessType.SelectedValue, drPromoType("POS_ProcType")) & ", " & _
                        "PosDiscType = " & GetXMLfieldValue(drPromoType("POS_DiscType_STATE"), cboXML_DiscountType.SelectedValue, drPromoType("POS_DiscType")) & ", " & _
                        "PosSMACKitPrice = " & GetXMLfieldValue(drPromoType("POS_SMACKitPrice_STATE"), txtXML_SMACKitPrice.Text, drPromoType("POS_SMACKitPrice")) & ", " & _
                        "PosQualifiedCust = " & GetXMLfieldValue(drPromoType("POS_QualifiedCust_STATE"), txtQualifiedCust.Text, drPromoType("POS_QualifiedCust"), True) & ", " & _
                        "PosNameOfPartners = " & GetXMLfieldValue(drPromoType("POS_NameOfPartners_STATE"), txtNameOfPartner.Text, drPromoType("POS_NameOfPartners"), True) & ", " & _
                        "PosProofOfMembership = " & GetXMLfieldValue(drPromoType("POS_ProofOfMembership_STATE"), txtProofOfMembership.Text, drPromoType("POS_ProofOfMembership"), True) & ", " & _
                        "PosPartnerEstabGWP = " & GetXMLfieldValue(drPromoType("POS_PartnerEstabGWP_STATE"), txtPartnerEstabGWP.Text, drPromoType("POS_PartnerEstabGWP"), True) & ", " & _
                        IIf(drPromoType("POS_EligibleCards_STATE") > 10, _
                            "PosEligibleCards = '" & EligibleCardsEncode() & "', ", _
                             "PosEligibleCards = '" & drPromoType("POS_EligibleCards") & "', ") & _
                        IIf((txtXML_DiscAmount.Text <> ""), _
                            "PercentDisc = 0, DiscAmount = 0" & txtXML_DiscAmount.Text, _
                            "DiscAmount = 0, PercentDisc = 0" & txtXML_PercentDisc.Text) & ", " & _
                            "PosMessageString = '" & GetCouponMessageEntry().Replace("'", "''") & "', " & _
                            "PosPromoID = 0" & sAssignedVSPromoID & " " & _
                            AdditionalSetColumnQuery & _
                        IIf(tempPromoDesc <> "", ",PromoDesc = '" + tempPromoDesc + "'", ",PromoDesc = '" & sPromoDesc & "'") & _
                        "WHERE PromoID = 0" & clsSession.CurrPromoID
            '"PosEligibleCards = '" & EligibleCardsEncode() & "', " & _
            '"PromoDesc = '" & sPromoDesc & "', " & _

            ' "CompSponsorship = " & GetXMLfieldValue(drPromoType("CompSponsorship_STATE"), cboCompSponsorship.SelectedValue, drPromoType("CompSponsorship_Value")) & ", " & _

        Else
            ' raise error
        End If

        CreateUpdateQueryForXML = sQuery

    End Function

    Private Function GetXMLfieldValue(ByVal ColumnState As Integer, ByVal InputValue As String, ByVal ConfigValue As String, Optional ByVal IsStringData As Boolean = False) As String

        Dim sResult As String = ""

        Select Case ColumnState
            Case 0                  ' hidden / unused

                sResult = "NULL"

            Case 10, 20             ' hidden; visible but disabled; use predefined value

                ' get value from configuration
                If IsStringData Then
                    sResult = "'" & ConfigValue & "'"
                Else
                    sResult = ConfigValue
                End If

            Case 30                 ' visible, enabled / predefined value as default

                ' get value from inputfield
                If IsStringData Then
                    sResult = "'" & InputValue & "'"
                Else
                    sResult = InputValue
                End If

        End Select

        GetXMLfieldValue = sResult

    End Function

    Protected Sub lnkRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRequest.Click

        clsSession.MsgTransFlowFlag = 2
        Response.Redirect("PromoRequestEntry.aspx")

    End Sub

    'Protected Sub RemoveMechanicsExtendedDetails()

    '    '***** Dim sPromoMechanics As String = Server.HtmlDecode(litMechanics.Text)

    '    ' remove extra details if any
    '    If InStr(litMechanics.Text, "<ExtraDetails>") > 0 Then
    '        litMechanics.Text = Left(litMechanics.Text, InStr(litMechanics.Text, "<ExtraDetails>") - 1)
    '    End If


    'End Sub

    Protected Sub lnkEditMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditMechanics.Click

        hidBox.Value = litMechanics.Text
        clsSession.Mechanics = Server.HtmlDecode(litMechanics.Text.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")

    End Sub

    'NBS2021-04-07
    Protected Sub lnkShowBankBinEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkShowBankBinEntry.Click

        If SavePromotionInfo(True) Then

            hidBox.Value = 0
            'clsSession.Mechanics = Server.HtmlDecode(litMechanics.Text.ToString)
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openPromoBankCardBinEntry();</script>")
        End If

    End Sub
    Protected Sub lnkSaveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click

        '::ToDo:: warn or auto save promo header entry

        'Response.Redirect("PromoRequestPreview.aspx")

        'MALAgasino 20180910 - If UPCLevel = 1, allow adding promo details. As per new SBU Template requirement,
        'Start of Code
        Dim drPromoType As DataRow

        drPromoType = GetPromoTypeInfo(cboPromoType.SelectedValue)

        If Not (IsDBNull(drPromoType) Or drPromoType Is Nothing) Then
            If (drPromoType("IsUPCLevel") = True) Then
                trPromoDetails.Visible = True
            End If
        End If

        'End of Code


        ' for promotions with no dp/sdp/cl encoding : SBU Mktg./BCR
        If SystemUser.UserGroupType = "SBU" Or SystemUser.UserGroupType = "BCR" Then

            If SavePromotionInfo() Then

                clsSession.IsDepartmental = 0

                If trPromoDetails.Visible = True Then

                    Response.Redirect("PromoDetailsEntry.aspx")

                ElseIf lnkBranches.Enabled = True Then

                    Response.Redirect("PromoBranches.aspx")

                Else
                    Response.Redirect("PromoRequestPreview.aspx")

                End If


            End If

        Else

            lnkPromoDetails_Click(sender, e)

        End If


    End Sub

    Protected Sub lnkattachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkattachment.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openAttachment();</script>")
    End Sub

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & clsSession.CurrRequestID.ToString, 1)
        Response.Redirect("PromoEntry.aspx?DownLoad=" & clsSession.CurrRequestID.ToString & ".zip" & "&Path=" & clsPromo.pathAttachment & clsSession.CurrRequestID.ToString)
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String

        strFiles = ""
        strDir = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString
        'Dim di As New System.IO.DirectoryInfo(strDir)
        lblFiles.Text = ""

        If System.IO.Directory.Exists(strDir) Then
            Dim dir As New System.IO.DirectoryInfo(strDir)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href='PromoEntry.aspx?FileName=" & file.Name.ToString & "'>" & file.Name.ToString & "</a> "
            Next
        Else
            If Directory.Exists(strDir) Then
                Dim s As String
                For Each s In System.IO.Directory.GetFiles(strDir)
                    System.IO.File.Delete(s)
                Next s
                Directory.Delete(strDir)
                'Kill(strDir)
                'di.Delete(True)
            End If
        End If

        If Len(strFiles) <> 0 Then
            'lblFiles.Text = Left(strFiles, Len(strFiles) - 1)
        Else
            lblFiles.Text = ""
        End If

        imgbtnDownload.Visible = tr

    End Sub

    Protected Sub cmdCopyValueToMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCopyValueToMechanics.Click
        litMechanics.Text = hidBox.Value
    End Sub

    Protected Sub lnkBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBranches.Click

        'blistErrorMsg.Items.Clear()
        'If gridItems.Rows.Count() < 1 Then
        '    blistErrorMsg.Items.Add("No Dp/Sdp/Class code specified.")
        '    Exit Sub
        'End If

        If SavePromotionInfo() Then

            clsSession.IsDepartmental = 0
            Response.Redirect("PromoBranches.aspx")

        End If

    End Sub

    Private Sub FillResDropDownList(ByRef cboListObj As DropDownList, ByVal sGroupName As String, Optional ByVal sSubGroupName As String = "", Optional ByVal sExcludeList As String = "")

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        If sSubGroupName <> "" Then
            strQuery = "AND SubGroupName = '" & sSubGroupName & "' "
        End If

        If sExcludeList <> "" Then
            strQuery &= "AND SequenceNo NOT IN (" + sExcludeList + ") "
        End If

        strQuery = "SELECT ElementName, ElementValue " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = '" & sGroupName & "' and IsActive = 1 " & _
                    strQuery & _
                    "ORDER BY SequenceNo"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        cboListObj.ClearSelection()
        cboListObj.Items.Clear()

        cboListObj.AppendDataBoundItems = True
        ddItem.Text = "-- Select Value --"
        ddItem.Value = -1

        cboListObj.Items.Add(ddItem)
        cboListObj.DataSource = dtTable
        cboListObj.DataValueField = "ElementValue"
        cboListObj.DataTextField = "ElementName"
        cboListObj.DataBind()

    End Sub

    Private Sub RegisterLocalClientSideScripts()

        txtDiscount.Attributes.Add("onkeypress", "return AllowIntegerOnly(this);")

        txtOrigValue.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")
        txtPromoValue.Attributes.Add("onkeypress", "return AllowIntegerOnly(this);")
        txtAllocation.Attributes.Add("onkeypress", "return AllowIntegerOnly(this);")

        txtPlanPromoSales.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")
        txtPlanPromoCost.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")
        txtPlanMargin.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")

        txtAnyX4P_PromoPrice.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")

        txtTPL_BuyQty.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")
        txtTPL_ReqAmount.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")

        'txtPurchaseQty.Attributes.Add("onkeypress", "return AllowIntegerOnly(this);")
        'txtFreeQty.Attributes.Add("onkeypress", "return AllowIntegerOnly(this);")
        'txtPromoPrice.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")

    End Sub

    Private Function ClearPromotionsAndDetails() As Boolean
        Dim strQuery As String = ""
        Dim sErrMess As String = ""
        Dim desc As String = ""
        Dim drPromoType As DataRow = Nothing

        strQuery = "SELECT * " & _
                    "FROM PromoTypes " & _
                    "WHERE PromoTypeID = 0" & cboPromoType.SelectedValue

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drPromoType) Then
            desc = drPromoType("DefaultMechanics").ToString
        End If

        strQuery = "DELETE From PromoDetails " & _
                "WHERE PromoID = " & clsSession.CurrPromoID

        strQuery += " Delete From PromoUPCs " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID


        strQuery += " DELETE From PromoPremiumUPCs " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID

        strQuery += " DELETE From PromotionCoupons " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID

        'NBS20190822: Added TplTransDesc
        strQuery += " UPDATE Promotions " & _
                    "SET [PromoTypeID]= " & cboPromoType.SelectedValue & ", [PromoDesc] = '" & desc & "', [PercentDisc] = 0, [DiscAmount]= NULL,[CompSponsorship] = NULL, " & _
                    "[VendorCode] = NULL,[PromoBarCode] = NULL,[PromoEventCode] = NULL,[OriginalValue] = NULL,[Allocation] = NULL," & _
                    "[OnlineSellingStart] = NULL,	[OnlineSellingEnd] = NULL, [OtherInfo] = NULL,	[VendorType] = NULL,  " & _
                    "[WebAdRate] = NULL,[PromoBudget]=NULL,	[WebAdPlacement] = NULL,[SeqNo] = NULL,	[PlanPromoSales] = NULL, " & _
                    "[PlanPromoCost] =NULL,	[PlanPercentMargin] = NULL,[ActualPromoCost] = NULL,[Element1] =NULL,	[Element2] = NULL, " & _
                    "[Element3] = NULL,[PosPromoID] = NULL,	[PosActionType] = NULL,	[PosCompressID] = NULL,	[PosExclType] = NULL, " & _
                    "[PosPriority] = NULL,	[PosMessageFlag] = NULL,[PosMessageID] = NULL,	[PosActiveDays] = NULL,	[PosActime] = NULL," & _
                    "[PosDeactime] = NULL,[PosDescr1prm] = NULL,[PosDescr2prm] = NULL,[PosInfotext1] = NULL,	[PosInfotext2] = NULL, " & _
                    "[PosMaxAmnt] = NULL,[PosMaxQty] = NULL,[PosProcType] = NULL,[PosDiscType] = NULL,[PosStandardExempt] = NULL, " & _
                    "[PosPermanentExempt] = NULL,[PosQualifiedItems] = NULL,[PosMessageString] = NULL,	[PosEligibleCards] = NULL, " & _
                    "[PosAddedToExclusion] = NULL,[PosMinAmnt] = NULL,[TplNumMonths] = NULL,	[TplBankList] = NULL,[TplBrandNames] = NULL, " & _
                    "[TplRefMemo] = NULL,	[TplProcessType] = NULL,[TplPurchaseReq] = NULL,[TplReqAmount] = 0,	[TplFreeItems] = NULL, " & _
                    "[TplPrizes] = NULL,[TplDeptName] = NULL,[TplSellingArea] = NULL,[TplItemName] = NULL,	[TplBonusPoints] = NULL, " & _
                    "[TplCelebName] = NULL,	[TplEventTime] = NULL,	[TplActivityName] = NULL,[TplBuyQty] = NULL,	[TplTakeQty] = NULL, " & _
                    "[TenderTypeCode] = NULL,[TplPromoNotes] = NULL,	[PosSMACKitPrice] =0,	[PosQualifiedCust] = NULL, " & _
                    "[PosNameOfPartners] = NULL, [PosProofOfMembership] = NULL, [TplPercentDisc] = NULL, [TplTransDesc] = NULL " & _
                    "WHERE PromoID = 0" & clsSession.CurrPromoID

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
            blistErrorMsg.Items.Add("Unable to clear promotion and details: " & sErrMess)
            Return False
            Exit Function
        End If

        Return True
    End Function

    Private Sub ClearFields()
        txtDiscount.Text = ""
        txtMarkdown.Text = ""
        txtPurchaseQty.Text = ""
        txtFreeQty.Text = ""
        txtPromoPrice.Text = ""
        txtB1T1_BuyQty.Text = ""
        txtB1T1_TakeQty.Text = ""
        txtB1T1_PercentDisc.Text = ""
        txtAnyX4P_BuyQty.Text = ""
        txtAnyX4P_PromoPrice.Text = ""


        txtBG4Ps_BuyQty.Text = ""
        txtBG4Ps_TakeQty.Text = ""
        txtBG4Ps_DiscAmount.Text = ""

        txtBG4Pd_BuyQty.Text = ""
        txtBG4Pd_TakeQty.Text = ""
        txtBG4Pd_DiscAmount.Text = ""

        txtBGPerOffs_BuyQty.Text = ""
        txtBGPerOffs_TakeQty.Text = ""
        txtBGPerOffs_PercentDisc.Text = ""

        txtBGPerOffd_BuyQty.Text = ""
        txtBGPerOffd_TakeQty.Text = ""
        txtBGPerOffd_PercentDisc.Text = ""

        txtAnyXY_BuyQty.Text = ""
        txtAnyXY_PromoPrice.Text = ""

        txtOrigValue.Text = ""
        txtPromoValue.Text = ""
        lblDiscAmount.Text = ""
        txtAllocation.Text = ""
        txtOnlineSellingStart.Text = ""
        txtOnlineSellingEnd.Text = ""

        cboVendorType.SelectedIndex = -1
        lblWebAdRate.Text = ""
        lblPromoBudget.Text = ""
        lblWebAdPlacement.Text = ""
        lblTotalBudget.Text = ""

        txtQualifiedCust.Text = ""
        txtNameOfPartner.Text = ""
        txtProofOfMembership.Text = ""
        txtPartnerEstabGWP.Text = ""

        cboTPL_PurchaseReq.SelectedIndex = -1
        txtTPL_BuyQty.Text = "0"
        txtTPL_TakeQty.Text = "0"
        txtTPL_DiscAmount.Text = "0"
        txtTPL_PercentDisc.Text = "0"
        txtTPL_ReqAmount.Text = "0"
        txtTPL_NumMonths.Text = "0"
        txtTPL_BonusPoints.Text = "0"
        txtTpl_SubsidyRate.Text = ""
        txtTPL_BankList.Text = ""
        txtTPL_BrandNames.Text = ""
        txtTPL_CelebName.Text = ""
        txtTPL_ItemName.Text = ""
        txtTPL_DeptName.Text = ""
        txtTPL_ActivityName.Text = ""
        txtTPL_SellingArea.Text = ""
        txtTPL_EventTime.Text = ""
        txtTPL_TransDesc.Text = ""      'NBS20190822
        txtTPL_FreeItems.Text = ""
        txtTPL_Prizes.Text = ""
        txtTPL_RefMemo.Text = ""
        cboTPL_ProcessType.SelectedIndex = -1

        txtXML_Descr1prm.Text = ""
        txtXML_Descr2prm.Text = ""
        txtXML_ReceiptDesc1.Text = ""
        txtXML_ReceiptDesc2.Text = ""
        cboXML_ActionType.SelectedIndex = -1
        cboXML_DiscountType.SelectedIndex = -1
        cboXML_ProcessType.SelectedIndex = -1
        txtXML_MaxQty.Text = ""
        txtXML_MaxAmount.Text = ""
        txtXML_CompessID.Text = ""
        cboXML_ExclusionType.SelectedIndex = -1
        'txtXML_AcTime.Text = ""
        'txtXML_Deactime.Text = ""
        cboXML_StartTime.Text = "00:00"
        cboXML_EndTime.Text = "00:00"
        cboXML_Priority.SelectedIndex = -1

        txtXML_RuleValue1.Text = ""
        txtXML_RuleValue2.Text = ""
        txtXML_RuleValue3.Text = ""
        txtXML_RuleValue4.Text = ""
        cboXML_Condition1.SelectedIndex = -1
        txtXML_DiscAmount.Text = ""
        txtXML_PercentDisc.Text = ""
        txtXML_SMACKitPrice.Text = ""

        cboXML_QualifiedItems.SelectedIndex = -1
        cboXML_TenderType.SelectedIndex = -1
        litTPLPromoNotes.Text = ""

        'rbs7281 Promo MW Update 20251125
        If trXML_CompSponsorship2.Visible = True Then
            cboCompSponsorship.SelectedIndex = -1
        End If

        cboChargeableEntity.SelectedIndex = -1

        txtPlanPromoSales.Text = ""
        txtPlanPromoCost.Text = ""
        txtPlanMargin.Text = ""

        For Each item As ListItem In cblEligibleCards.Items
            item.Selected = False
        Next

        txtXML_CouponMsg01.Text = ""
        txtXML_CouponMsg02.Text = ""
        txtXML_CouponMsg03.Text = ""
        txtXML_CouponMsg04.Text = ""
        txtXML_CouponMsg05.Text = ""
        txtXML_CouponMsg06.Text = ""
        txtXML_CouponMsg07.Text = ""
        txtXML_CouponMsg08.Text = ""
        txtXML_CouponMsg09.Text = ""
        txtXML_CouponMsg10.Text = ""
        txtXML_CouponMsg11.Text = ""
        txtXML_CouponMsg12.Text = ""
        txtXML_CouponMsg13.Text = ""
        txtXML_CouponMsg14.Text = ""
        txtXML_CouponMsg15.Text = ""
        txtXML_CouponMsg16.Text = ""
        txtXML_CouponMsg17.Text = ""
        txtXML_CouponMsg18.Text = ""
        txtXML_CouponMsg19.Text = ""
        txtXML_CouponMsg20.Text = ""


    End Sub

    Private Sub InitializeXMLInputField(ByRef objContainerTableRow As HtmlTableRow, ByRef objInputField As Object, ByVal strDefaultValue As String, ByVal nInputState As Short, Optional ByVal sDropDownReference As String = "", Optional ByVal sExcludeList As String = "")

        Select Case nInputState
            Case 0      ' hidden / unused field (NULL)

                objContainerTableRow.Visible = False

            Case 10, 20   ' hidden / use predefined value

                objContainerTableRow.Visible = (nInputState = 20)

                If TypeOf objInputField Is TextBox Then

                    CType(objInputField, TextBox).Text = strDefaultValue
                    CType(objInputField, TextBox).Enabled = False

                ElseIf TypeOf objInputField Is DropDownList Then

                    FillResDropDownList(CType(objInputField, DropDownList), sDropDownReference, "", sExcludeList)

                    CType(objInputField, DropDownList).SelectedValue = strDefaultValue
                    CType(objInputField, DropDownList).Enabled = False

                ElseIf TypeOf objInputField Is CheckBox Then
                    CType(objInputField, CheckBox).Checked = CBool(strDefaultValue)
                    CType(objInputField, CheckBox).Enabled = False

                End If

            Case 30, 40     ' visible, enabled / predefined value as default / optional (20190805)

                objContainerTableRow.Visible = True

                If TypeOf objInputField Is TextBox Then

                    CType(objInputField, TextBox).Text = strDefaultValue
                    CType(objInputField, TextBox).Enabled = True

                ElseIf TypeOf objInputField Is DropDownList Then

                    FillResDropDownList(CType(objInputField, DropDownList), sDropDownReference, "", sExcludeList)

                    CType(objInputField, DropDownList).SelectedValue = strDefaultValue
                    CType(objInputField, DropDownList).Enabled = True

                ElseIf TypeOf objInputField Is CheckBox Then

                    CType(objInputField, CheckBox).Checked = CBool(strDefaultValue)
                    CType(objInputField, CheckBox).Enabled = True

                End If

        End Select

    End Sub
    Function FormatMoneyField(ByRef value As Double) As String
        FormatMoneyField = Format(value, "#,###.##")
        If (Right(FormatMoneyField, 1) = ".") Then
            FormatMoneyField = Left(FormatMoneyField, Len(FormatMoneyField) - 1)
        End If
    End Function

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

                If ctr < 47 Then
                    list.Value = time.ToString("HH:mm")
                    list.Text = time.ToString("HH:mm")
                Else
                    list.Value = "23:59"
                    list.Text = "23:59"
                End If

                '.Items.Insert(list.Value, list.Text)
                .Items.Add(New ListItem(list.Text, list.Text))

            End With

            time = time.AddMinutes(30)
            ctr = ctr + 1
        End While

    End Sub


    'Private Sub AssignValueToInputField(ByRef objContainerTableRow As HtmlTableRow, ByRef objInputField As Object, ByVal strDataValue As String)

    '    If objContainerTableRow.Visible Then
    '        If TypeOf objInputField Is TextBox Then
    '            CType(objInputField, TextBox).Text = CStr(strDataValue)

    '        ElseIf TypeOf objInputField Is DropDownList Then
    '            CType(objInputField, DropDownList).SelectedValue = strDataValue

    '        ElseIf TypeOf objInputField Is CheckBox Then
    '            CType(objInputField, CheckBox).Checked = CBool(strDataValue)

    '        End If
    '    End If

    'End Sub

#Region "Individual Loyalty Cards selection Event Handlers, Subs and Functions"

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim bCheck As Boolean = chkSelectAll.Checked

        For Each item As ListItem In cblEligibleCards.Items
            item.Selected = bCheck
        Next

    End Sub

    Private Function IsEligibleCardSelected() As Boolean

        For Each item As ListItem In cblEligibleCards.Items
            If item.Selected Then
                Return True
            End If
        Next

        Return False

    End Function

    Private Function EligibleCardsEncode() As String

        Dim sResult As New StringBuilder()

        For Each item As ListItem In cblEligibleCards.Items
            If item.Selected Then
                sResult.Append(item.Value.PadLeft(2, "0"c) & ";")
            End If
        Next

        Return sResult.ToString()
    End Function

    Private Sub FillEligibleCards(ByVal sValues As String)

        For Each item As ListItem In cblEligibleCards.Items
            item.Selected = sValues.Contains(item.Value.PadLeft(2, "0"c) & ";")
        Next

    End Sub

#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Select Case ViewState("process")
            Case "ChangePromotion"

                If clsSession.DeleteStatus = "yes" Then
                    If (ClearPromotionsAndDetails()) Then
                        ClearFields()
                        DisplayPromoTypeLayout(CInt(cboPromoType.SelectedValue))
                    End If
                Else
                    Response.Redirect(Request.RawUrl)

                End If


        End Select
    End Sub

    Protected Sub lnkEditPromoNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditPromoNotes.Click

        hidBox.Value = Server.HtmlDecode(litTPLPromoNotes.Text.ToString)
        clsSession.Mechanics = Server.HtmlDecode(litTPLPromoNotes.Text.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openPromoNotesEditor();</script>")

    End Sub

    Protected Sub cmdCopyValueToPromoNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCopyValueToPromoNotes.Click
        litTPLPromoNotes.Text = hidBox.Value
    End Sub

    Protected Sub cboChargeableEntity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboChargeableEntity.SelectedIndexChanged

        ShowChargeableEntityValues()

    End Sub

    Private Sub ShowChargeableEntityValues()

        If cboChargeableEntity.SelectedValue = 20 Or _
                cboChargeableEntity.SelectedValue = 30 Or _
                cboChargeableEntity.SelectedValue = 50 Then

            trTplChargeEnt_VendorCode.Visible = True

            If cboChargeableEntity.SelectedValue = 30 Or _
                    cboChargeableEntity.SelectedValue = 50 Then

                trTplChargeEnt_DSsub.Visible = True
                trTplChargeEnt_BUsub.Visible = True
            Else
                trTplChargeEnt_DSsub.Visible = False
                trTplChargeEnt_BUsub.Visible = False
            End If

        Else
            'hide all
            trTplChargeEnt_VendorCode.Visible = False
            trTplChargeEnt_DSsub.Visible = False
            trTplChargeEnt_BUsub.Visible = False
        End If

    End Sub



    Dim DT As New DataTable
    'Protected Sub btnSeeding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeeding.Click
    '    'Dim DT As New DataTable

    '    'DT.Columns.Add("StartDate")
    '    'DT.Columns.Add("EndDate")
    '    'DT.Columns.Add("MaxNumber")
    '    'DT.Columns.Add("Counter")
    '    'DT.Columns.Add("Prize")
    '    'DT.Rows.Add(txtStartDate.Text, txtEndDate.Text, txtMaxNumber.Text, txtCounter.Text, txtPrice.Text)
    '    'gvSeeding.DataSource = DT
    '    'gvSeeding.DataBind()


    '    ''hidBox.Value = Server.HtmlDecode(litTPLPromoNotes.Text.ToString)
    '    ''clsSession.Mechanics = Server.HtmlDecode(litTPLPromoNotes.Text.ToString)
    '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSeedEditor();</script>")
    'End Sub

    Protected Sub linkSeeding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSeeding.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSeedEditor();</script>")
    End Sub

    Protected Sub cmdCopyDatasource_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCopyDatasource.Click
        LoadPromoBinRange(clsSession.CurrRequestID)
        LoadPromoSeed(clsSession.CurrRequestID)
        LoadRebatePromoBinRange(clsSession.CurrRequestID)
        LoadPromoShoulderingEntity(clsSession.CurrRequestID)
    End Sub

    Protected Sub linkBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBinRangeEditor();</script>")
    End Sub

    Protected Sub linkSeedAttachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSeedAttachment.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSeedAttachment();</script>")

    End Sub

    Protected Sub btnSeedDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeedDownload.Click
        Dim tr As Boolean = False
        Dim strDir As String
        Dim strFiles As String

        strFiles = ""
        strDir = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString

        Dim pathTemp As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = clsSession.CurrRequestID.ToString & "-Swipestakes"
        pathTemp = swipestakeSeedFolder & subFolderName

        'Dim di As New System.IO.DirectoryInfo(strDir)
        lblSeedFile.Text = ""

        If System.IO.Directory.Exists(pathTemp) Then
            Dim dir As New System.IO.DirectoryInfo(pathTemp)
            Dim files As System.IO.FileInfo() = dir.GetFiles()
            For Each file As System.IO.FileInfo In files
                tr = True
                strFiles = strFiles & file.Name.ToString & ","
                lblSeedFile.Text &= "<img src='Images/bullet green.gif' /><a href='PromoEntry.aspx?FileName=" & file.Name.ToString & "'>" & file.Name.ToString & "</a> "
            Next
        Else
            If Directory.Exists(pathTemp) Then
                Dim s As String
                For Each s In System.IO.Directory.GetFiles(pathTemp)
                    System.IO.File.Delete(s)
                Next s
                Directory.Delete(pathTemp)
                'Kill(strDir)
                'di.Delete(True)
            End If
        End If

        If Len(strFiles) <> 0 Then
            'lblFiles.Text = Left(strFiles, Len(strFiles) - 1)
        Else
            lblSeedFile.Text = ""
        End If

        imgbtnSeedDownload.Visible = tr
        LoadPromoSeed(clsSession.CurrRequestID)

    End Sub

    Protected Sub imgbtnSeedDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnSeedDownload.Click

        Dim pathTemp As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = clsSession.CurrRequestID.ToString & "-Swipestakes"
        pathTemp = swipestakeSeedFolder & subFolderName

        clsPromo.CreateZipFile(pathTemp, 1)
        Response.Redirect("PromoEntry.aspx?DownLoad=" & clsSession.CurrRequestID.ToString & ".zip" & "&Path=" & pathTemp)
    End Sub


    Private Function RetrievePurchaseRequirementAmount(ByVal drPromoType As DataRow) As String
        '' LTE 20220406 1423 - Added condition for MPR from RuleValue
        Return IIf(trXML_RuleValue2.Visible _
                   , Val(Replace(txtXML_RuleValue2.Text, ",", "")) _
                   , GetXMLfieldValue(drPromoType("POS_MinAmnt_STATE"), txtXML_MinAmount.Text.Replace(",", ""), drPromoType("POS_MinAmnt")))
    End Function

    Protected Sub chk_DiscCapTickBox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_DiscCapTickBox.CheckedChanged
        If Me.chk_DiscCapTickBox.Checked = True Then
            lblXML_DiscCapTickBox.Visible = True
            txtXML_DiscCap.Visible = True
        Else
            txtXML_DiscCap.Visible = False
        End If
    End Sub

    Private Sub MessageBox(ByVal sMessage As String, ByVal sProcess As String, ByVal sTitle As String, ByVal sIcon As String)

        ' prompt for confirmation
        clsSession.Message = sMessage
        clsSession.Icon = sIcon
        lblPopTitle.Value = sTitle
        ViewState("process") = sProcess
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Private Function IsZeroEquivalent(ByVal Value As Object) As Boolean 'Mantis#64786,64788 rbs7281 7/23/2025
        If Value Is Nothing Then Return True

        Dim str = Value.ToString().Trim.ToUpper()

        Return str = "" OrElse str = "NA" OrElse str = "0" OrElse str = "0.000" OrElse IsNumeric(str) AndAlso Val(str) = 0

    End Function


    Private Sub SaveSpecialHandling()
        Dim ConnStr As String = System.Configuration.ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString

        Using conn As New SqlConnection(ConnStr)
            conn.Open()

            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Using cmd As New SqlCommand("USP_RequestSpecialConditions", conn, tran)
                        cmd.CommandType = CommandType.StoredProcedure

                        '--- Parameters
                        cmd.Parameters.AddWithValue("@RequestID", clsSession.CurrRequestID)


                        cmd.ExecuteNonQuery()
                    End Using


                    tran.Commit()
                    'lblResult.Text = "Rows saved successfully using stored procedure."
                    'lblResult.ForeColor = Drawing.Color.Green

                Catch ex As Exception
                    tran.Rollback()
                    'lblResult.Text = "Error saving rows: " & ex.Message
                    'lblResult.ForeColor = Drawing.Color.Red
                End Try
            End Using
        End Using
    End Sub

    Protected Sub cboXML_RebateDiscType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboXML_RebateDiscType.SelectedIndexChanged
        Dim selectedValue As String = cboXML_RebateDiscType.SelectedValue

        If selectedValue = "1" Then 'Amount Discount
            InitializeXMLDiscountField(clsSession.PromoTypeID, selectedValue, 0, 0, ViewState("EligibleCards_STATE"))
            trXML_DiscCapAmount.Visible = False
            txtXML_DiscCap.Text = String.Empty
            chk_DiscCapTickBox.Checked = False
        ElseIf selectedValue = "2" Then '% Discount
            InitializeXMLDiscountField(clsSession.PromoTypeID, selectedValue, 0, 0, ViewState("EligibleCards_STATE"))
            trXML_DiscCapAmount.Visible = True
            chk_DiscCapTickBox.Visible = True
        Else
            trXML_PercentDisc.Visible = False
            trXML_DiscAmount.Visible = False
        End If
    End Sub

    Private Sub GetDiscConDetails(ByVal PromotypeID As String, ByVal DiscCon As Int16)
        Dim drRow As DataRow = Nothing
        Dim strQuery As String = ""
        txtXML_DiscAmount.Text = ""
        txtXML_PercentDisc.Text = ""

        strQuery = "SELECT PercentDisc,DiscAmount " & _
                    "FROM Promotions  " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

            If IsZeroEquivalent(drRow("PercentDisc")) = False Then
                cboXML_RebateDiscType.SelectedValue = "2"
                InitializeXMLDiscountField(clsSession.PromoTypeID, 2, 0, drRow("PercentDisc"), ViewState("EligibleCards_STATE"))
            End If

            If IsZeroEquivalent(drRow("DiscAmount")) = False Then
                cboXML_RebateDiscType.SelectedValue = "1"
                InitializeXMLDiscountField(clsSession.PromoTypeID, 1, drRow("DiscAmount"), 0, ViewState("EligibleCards_STATE"))
            End If

        Else
            ' error
        End If


    End Sub

    Private Sub LoadEligibleCards()
        cblEligibleCards.ClearSelection()
        Dim dt As New DataTable()

        Using con As New SqlConnection(clsPromo.SQLConnString)
            Using cmd As New SqlCommand("SELECT CardID, CardName FROM dbo.VW_PosEligibleCards  WHERE GroupName = 'PosEligibleCards'", con)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        cblEligibleCards.DataSource = dt
        cblEligibleCards.DataTextField = "CardName"
        cblEligibleCards.DataValueField = "CardID"
        cblEligibleCards.DataBind()
    End Sub


    Protected Sub gvRebateBinRange_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvRebateBinRange.Sorting
        Dim nRequestID As String = clsSession.CurrRequestID.ToString()

        sqldsData.SelectCommand = "SELECT RLV.ElementName, PBR.BIN, PBR.PanLow, PBR.PanHigh, PBR.PanLength FROM PromoBinRange PBR LEFT JOIN ResListValues RLV ON PBR.RangeType = RLV.ElementID AND RLV.GroupName = 'RangeType'" & _
                                  "WHERE RequestID = " & nRequestID & ""

        Dim dvPromoRebateBinRange As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dvPromoRebateBinRange Is Nothing OrElse dvPromoRebateBinRange.Count = 0 Then
            dvPromoRebateBinRange.AddNew()
            divBinRange.Attributes.Add("style", "height:100%")
        Else
            divBinRange.Attributes.Add("style", "height:250px; overflow:auto")
        End If

        If dvPromoRebateBinRange IsNot Nothing Then
            Dim sortDirection As String = "ASC"

            ' Toggle sort direction
            If ViewState("SortExpression") IsNot Nothing AndAlso ViewState("SortExpression").ToString() = e.SortExpression Then
                If ViewState("SortDirection") IsNot Nothing AndAlso ViewState("SortDirection").ToString() = "ASC" Then
                    sortDirection = "DESC"
                End If
            End If

            ViewState("SortExpression") = e.SortExpression
            ViewState("SortDirection") = sortDirection

            dvPromoRebateBinRange.Sort = e.SortExpression & " " & sortDirection

            gvRebateBinRange.DataSource = dvPromoRebateBinRange
            gvRebateBinRange.DataBind()
        End If
    End Sub

    Protected Sub linkRebateBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkRebateBinRange.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openRebateBinRangeEditor();</script>")
    End Sub
    
    Protected Sub linkShoulderingEntity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkShoulderingEntity.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openShoulderingEntity(" & clsSession.CurrRequestID & ");</script>")
    End Sub


    Private Function CountShoulderingEntity(ByVal RequestID As Integer) As Integer

        Dim RetCount As Integer = 0
        Dim strQuery As String = ""

        If clsSession.CurrRequestID <> 0 Then
            ' create new promotion record with initial data
            'cjg2243 20250726
            strQuery = "SELECT COUNT(1) from ShoulderingEntity s where RequestID = " & RequestID.ToString() & " "

            RetCount = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)
        End If

        Return RetCount
    End Function



    Private Sub LoadPromoShoulderingEntity(ByVal nRequestID As Long)

        ' Clear previous settings
        sqldsData.SelectParameters.Clear()

        ' Call stored procedure instead of inline SQL
        sqldsData.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
        sqldsData.SelectCommand = "usp_GetShoulderingEntityDetails"

        ' Pass parameter
        sqldsData.SelectParameters.Add("RequestID", DbType.Int64, nRequestID.ToString())

        ' Execute
        Dim dvPromoShoulderingEntity As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dvPromoShoulderingEntity Is Nothing OrElse dvPromoShoulderingEntity.Count = 0 Then
            dvPromoShoulderingEntity.AddNew()
            divCompSponsorship.Attributes.Add("style", "height:1%")
        Else
            divCompSponsorship.Attributes.Add("style", "height:150px; overflow:auto")
        End If

        gridCompSponsorship.DataSource = dvPromoShoulderingEntity
        gridCompSponsorship.DataBind()

        'To avoid unnecessary error in PromoSeed
        sqldsData.SelectCommandType = SqlDataSourceCommandType.Text
        sqldsData.SelectParameters.Clear()

    End Sub

    Private Sub LoadShoulderingEntityDefaultSelection(ByVal isVisible As Boolean)
        Dim drPromoType As DataRow

        drPromoType = GetPromoTypeInfo(cboPromoType.SelectedValue)

        If isVisible = True Then
            If drPromoType("CompSponsorship_Mode").ToString() = "2" Then
                trXML_CompSponsorship2.Visible = True
                trXML_CompSponsorship.Visible = False
                divCompSponsorship.Visible = False
            Else
                divCompSponsorship.Visible = True
                trXML_CompSponsorship2.Visible = False
                trXML_CompSponsorship.Visible = True
            End If
        Else
            trXML_CompSponsorship2.Visible = False
            trXML_CompSponsorship.Visible = False
        End If

    End Sub


    Private Function GetCompSponsorshipDetails() As String
        Dim compSponsorship As String = ""

        Using conn As New SqlConnection(clsPromo.SQLConnString)
            Using cmd As New SqlCommand("usp_GetCompSponsorshipDetails", conn)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = clsSession.CurrRequestID

                conn.Open()

                Dim result As Object = cmd.ExecuteScalar()

                If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                    compSponsorship = result.ToString()
                End If
            End Using
        End Using

        Return compSponsorship
    End Function







End Class
