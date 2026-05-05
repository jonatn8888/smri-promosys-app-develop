Imports dsPromotionsTableAdapters
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports ZXing
Imports ZXing.Common
Imports System.Linq

Partial Class PromoDetailsEntry
    Inherits System.Web.UI.Page

    'Private nPromoTypeID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        Try
            ViewState("PromoTypeID") = clsSession.PromoTypeID

        Catch ex As Exception

            ViewState("PromoTypeID") = 0
            ' OR redirect to invalid page

        End Try

        ' ------------------------------------------------------
        ' check if we are to show the Promo Branches link
        ' ------------------------------------------------------
        sqldsData.SelectCommand = "SELECT PromoID FROM PromoBranch WHERE PromoID IN (SELECT PromoID FROM Promotions WHERE RequestID = 0" & clsSession.CurrRequestID & ")"

        Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dv.Count > 0 Then

            Dim dr As DataRow = dv.Table.Rows(0)

            ' Revised dowcarpio08292012@smretailinc Temp deleted -ok now
            lnkBranches.Visible = False '(dr("PromoID").ToString() = "")

        Else

            lnkBranches.Visible = True

        End If

        ':::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
        '
        '::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        If Not IsPostBack() Then

            ' TODO: delete line
            'Session("TransFlag") = "natural"
            FillBarcodeAutoManual()
            FillValueType()
            txtClassCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtDepCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtSdepCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtSClassCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")

            ' show relevant information already entered by user
            ' ShowPromoRequestHeaderInfo()

            sqldsPromoDetails.SelectParameters("PromoID").DefaultValue = clsSession.CurrPromoID

            Dim strQuery As String = ""
            Dim drRow As DataRow
            Dim bIsUPCLevel As Boolean = False

            ' get Promo Layout ID
            If ViewState("PromoTypeID") <> 0 Then

                strQuery = "SELECT * FROM PromoTypes " & _
                            "WHERE PromoTypeID = 0" & ViewState("PromoTypeID")

                If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

                    ViewState("ProcessType") = drRow("ProcessType").ToString
                    ViewState("LayoutID") = drRow("LayoutID").ToString
                    ViewState("QualifiedItems") = GetQualifiedItemsOption(clsSession.CurrPromoID)

                    Dim arrGroupType() As String = {"SBU", "SBU2", "BCR"}

                    'NBS: moved outside if statement (ref#101)
                    ' show/hide panItemHierarchy
                    If Not arrGroupType.Contains(SystemUser.UserGroupType.ToString.ToUpper()) Then
                        panItemHierarchy.Visible = CBool(drRow("POS_AllowDeptCodes"))
                    End If

                    'NBS: moved outside if statement (ref#102)
                    ' show/hide coupon discount barcode entry
                    panPromoCoupons.Visible = CBool(drRow("POS_AllowCoupons"))
                    lblCouponHeader.Text = "Valid Coupons"

                    '@HERE:: Remove condition for GenericHostXML
                    If ViewState("ProcessType") = "GenericHostXML" Then

                        '@HERE: NBS-original position (ref#101)
                        '' show/hide panItemHierarchy
                        ' panItemHierarchy.Visible = CBool(drRow("POS_AllowDeptCodes"))

                        ' show/hide UPC entry
                        If Not arrGroupType.Contains(SystemUser.UserGroupType.ToString.ToUpper()) Then
                            panUPCdetails.Visible = CBool(drRow("POS_AllowBarcodes"))
                        End If

                        lblUPCpanelHeader.Text = "Eligible Items"

                        '@HERE: NBS-original position (ref#102)
                        '' show/hide coupon discount barcode entry
                        'panPromoCoupons.Visible = CBool(drRow("POS_AllowCoupons"))
                        'lblCouponHeader.Text = "Valid Coupons"

                        If panUPCdetails.Visible Then
                            Select Case ViewState("QualifiedItems")
                                Case 1, 2, 4
                                    lblUPCpanelHeader.Text = "Excluded Items"

                                Case 3
                                    lblUPCpanelHeader.Text = "Eligible Items"

                            End Select
                        End If

                        ' MALAgasino 20180912 
                        ' show/hide Free Items/PWP Items
                        If (drRow("POS_PromoPremUPC_STATE") > 0) Then
                            If (drRow("POS_PromoPremUPC_STATE") = 10) Then
                                panUPCPromoPremium.Visible = True
                                lbl_UPCPanPrem.Text = "Free Item/s"
                                panUPCdetails.Visible = False
                                gridItems.Visible = False
                                tbl_PromoDetails.Visible = False
                                lnkDeleteSelected.Visible = False

                            ElseIf (drRow("POS_PromoPremUPC_STATE") = 20) Then
                                panUPCPromoPremium.Visible = True
                                lbl_UPCPanPrem.Text = "PWP Item/s"
                                panUPCdetails.Visible = False
                                gridItems.Visible = False
                                tbl_PromoDetails.Visible = False
                                lnkDeleteSelected.Visible = False
                                txtDiscountedPrice.Enabled = True

                            ElseIf (drRow("POS_PromoPremUPC_STATE") = 30) Then
                                panUPCPromoPremium.Visible = False
                                panUPCdetails.Visible = False
                                gridItems.Visible = True
                                tbl_PromoDetails.Visible = True
                                lnkDeleteSelected.Visible = True
                                txtDiscountedPrice.Enabled = False
                            End If

                        End If

                        '*************************************************
                        ' get UPCRem Settings
                        '*************************************************
                        cboUPCrem.Enabled = (CInt(drRow("POS_UPCruleXY_STATE").ToString) = 30)

                        ' if more than one promo rule
                        'Dim nRuleCount As Integer = 1

                        'If panPromoCoupons.Visible Then nRuleCount += 1

                        'If GetPromoRuleCount(clsSession.CurrPromoID) > nRuleCount Then
                        '    cboUPCrem.Enabled = True
                        'End If


                    Else

                        ' for the other promotion types
                        If Not arrGroupType.Contains(SystemUser.UserGroupType.ToString.ToUpper()) Then
                            panUPCdetails.Visible = CBool(drRow("IsUPCLevel"))
                        End If

                        ' check process type
                        Select Case ViewState("LayoutID")
                            Case 11
                                ' -- Class Discount with Exemptions
                                lblUPCpanelHeader.Text = "Exempted Items"

                            Case Else
                                ' -- Non-Templated (with Qualified Items)
                                Select Case ViewState("QualifiedItems")
                                    Case 1, 2, 4
                                        lblUPCpanelHeader.Text = "Excluded Items"

                                    Case 3
                                        lblUPCpanelHeader.Text = "Eligible Items"

                                End Select

                                'Case Else
                                '    ' all others
                                '    lblUPCpanelHeader.Text = "Eligible Items"

                        End Select

                        '**************************************************************
                        '** Enable X/Y combobox based on POS_UPCruleXY_STATE column

                        cboUPCrem.Enabled = (CInt(drRow("POS_UPCruleXY_STATE").ToString) = 30)

                        '--------------------------------------------------------------
                        '-- Previous Code:
                        '--------------------------------------------------------------

                        'ViewState("LayoutID") = CInt(drRow("LayoutID").ToString)

                        'Select Case ViewState("LayoutID")
                        '    Case 110, 140, 150, 115, 145        'TODO:: remove hardcoding
                        '        cboUPCrem.Enabled = True
                        '    Case Else
                        '        cboUPCrem.Enabled = (CInt(drRow("POS_UPCruleXY_STATE").ToString) = 30)
                        'End Select

                        '**************************************************************

                        ' show/hide Free Items/PWP Items
                        If (drRow("POS_PromoPremUPC_STATE") > 0) Then
                            If (drRow("POS_PromoPremUPC_STATE") = 10) Then
                                panUPCPromoPremium.Visible = True
                                lbl_UPCPanPrem.Text = "Free Item/s"
                                panUPCdetails.Visible = False
                                gridItems.Visible = False
                                tbl_PromoDetails.Visible = False
                                lnkDeleteSelected.Visible = False

                            ElseIf (drRow("POS_PromoPremUPC_STATE") = 20) Then
                                panUPCPromoPremium.Visible = True
                                lbl_UPCPanPrem.Text = "PWP Item/s"
                                panUPCdetails.Visible = False
                                gridItems.Visible = False
                                tbl_PromoDetails.Visible = False
                                lnkDeleteSelected.Visible = False
                                txtDiscountedPrice.Enabled = True

                            ElseIf (drRow("POS_PromoPremUPC_STATE") = 30) Then
                                panUPCPromoPremium.Visible = False
                                panUPCdetails.Visible = False
                                gridItems.Visible = True
                                tbl_PromoDetails.Visible = True
                                lnkDeleteSelected.Visible = True
                                txtDiscountedPrice.Enabled = False
                            End If
                        End If

                        End If

                Else
                        ' 
                        ' error accessing table
                        ' 
                        panUPCdetails.Visible = False
                        panPromoCoupons.Visible = False

                End If
                'Non templated
                tr_ValueType.Visible = False
                If ViewState("PromoTypeID") = "2" Or ViewState("PromoTypeID") = "294" Then
                    tr_ValueType.Visible = True
                End If
            End If

            If panUPCdetails.Visible Then ShowPromoUPCList()
            If panPromoCoupons.Visible Then ShowPromoCouponList()
            If panUPCPromoPremium.Visible Then ShowPromoPremiumList()

        End If


        LoadBarcode()

        DefaultSelection()

    End Sub

    Private Function GetQualifiedItemsOption(ByVal nPromoID As Long) As Integer

        Dim nResult As Integer = -1
        Dim drRow As DataRow
        Dim strQuery As String

        strQuery = "SELECT PosQualifiedItems FROM Promotions " & _
                    "WHERE PromoID = 0" & nPromoID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
            If Not IsDBNull(drRow("PosQualifiedItems")) Then
                nResult = drRow("PosQualifiedItems")
            End If
        End If

        Return nResult

    End Function

    Private Function GetPromoRuleCount(ByVal nPromoID As Long) As Long

        Dim nResult As Long

        Dim strQuery As String

        strQuery = "SELECT COUNT(*) FROM PromotionRules " & _
                    "WHERE PromoID = 0" & nPromoID

        nResult = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)

        Return nResult

    End Function

    Private Sub ShowPromoRequestHeaderInfo()

        ' ::ToDo:: Show some significant data already entered?

        'lblPromoTitle.Text = Session("PromoTitle")

        'Dim taPromotions As New dsPromotionsTableAdapters.PromotionsTableAdapter()
        'Dim dtPromotions As dsPromotions.PromotionsDataTable
        'Dim rowPromotion As dsPromotions.PromotionsRow

        'dtPromotions = taPromotions.GetPromotionByID(Session("CurrPromoID"))

        'If dtPromotions.Rows.Count > 0 Then
        '    rowPromotion = dtPromotions.Rows(0)
        '    lblPeriodFrom.Text = rowPromotion.PeriodFrom.ToLongDateString
        '    lblPeriodTo.Text = rowPromotion.PeriodTo.ToLongDateString
        'End If

    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ShowPromoItemInfo()
    End Sub

    Private Sub ShowPromoItemInfo()

        Dim strQuery As String = ""

        If txtDepCode.Text.Trim() = "" Then
            strQuery = " AND DeptCode = -1"
        Else
            strQuery = " AND DeptCode = 0" & CInt(txtDepCode.Text.Trim())
        End If

        If txtSdepCode.Text.Trim() = "" Then
            strQuery &= " AND SubDepCode = -1"
        Else
            strQuery &= " AND SubDepCode = 0" & CInt(txtSdepCode.Text.Trim())
        End If

        If txtClassCode.Text.Trim() = "" Then
            strQuery &= " AND ClassCode = -1"
        Else
            strQuery &= " AND ClassCode = 0" & CInt(txtClassCode.Text.Trim())
        End If

        '        If hfShortDesc.Value.Trim() <> "" Then
        '            strQuery &= " AND ShortDesc = '" & hfShortDesc.Value.ToString() & "'"
        '        End If

        ' HERE TODO: remove hard-coding of PromoTypeID
        If ViewState("PromoTypeID") <> 18 Then
            If txtSClassCode.Text.Trim() = "" Or txtSClassCode.Text.Trim() = "0" Then
                strQuery &= " AND SubClassCode <> 0"
            Else
                strQuery &= " AND SubClassCode = 0" & CInt(txtSClassCode.Text.Trim())
            End If
        Else
            strQuery &= " AND SubClassCode = 0"         ' default to zero if class level only (class discount)
        End If

        'If Not IsNothing(ViewState("ShortDesc")) Then
        '    If ViewState("ShortDesc").ToString <> "" Then
        '        strQuery &= " AND ShortDesc = '" & ViewState("ShortDesc").ToString() & "'"
        '    End If
        'End If

        If SystemUser.UserGroupType <> "CM" And SystemUser.UserGroupType <> "SBU" Then

            strQuery &= " AND DeptCode IN ( " & _
                           "SELECT DeptCode FROM UserGroups WHERE GroupID IN ( " & _
                           "SELECT GroupID FROM GroupAssignment WHERE UserID = 0" & SystemUser.UserID & "))"

        End If

        '08312012: format codes using [dbo].[Fn_FormatPromoCode]
        sqldsData.SelectCommand = "SELECT RowID" & _
                                    ", EnvCode " & _
                                    ", dbo.Fn_FormatPromoCode(DeptCode,'Dp') AS DeptCode" & _
                                    ", dbo.Fn_FormatPromoCode(SubDepCode,'SDp') AS SubDepCode " & _
                                    ", dbo.Fn_FormatPromoCode(ClassCode,'Cl') AS ClassCode " & _
                                    ", dbo.Fn_FormatPromoCode(SubClassCode,'SCl') AS SubClassCode " & _
                                    ", Description " & _
                                    ", ShortDesc " & _
                                    ", IsHidden FROM DepSdepClass " & _
                                  "WHERE IsHidden = 0 " & strQuery

        Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dv.Table.Rows.Count = 0 Then

            ' show search window if invalid item

            txtDepCode.Text = ""
            txtSdepCode.Text = ""
            txtClassCode.Text = ""
            txtSClassCode.Text = ""
            hfShortDesc.Value = ""
            lblItemDesc.Text = ""
            ViewState("ShortDesc") = ""

            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearch();</script>")
        Else

            lblItemDesc.Text = dv.Table.Rows(0)("Description").ToString()
            ViewState("ShortDesc") = dv.Table.Rows(0)("ShortDesc").ToString()

        End If
    End Sub

    'Private Function ValidateItemCode() As Boolean
    '    Dim taCategories As New dsPromotionsTableAdapters.DepSdepClassTableAdapter()
    '    Dim dtblCategories As New dsPromotions.DepSdepClassDataTable
    '    Dim trowCategory As dsPromotions.DepSdepClassRow
    '    taCategories.GetCategoriesByCodeSP(dtblCategories, "", txtDepCode.Text, txtSdepCode.Text, txtClassCode.Text, SystemUser.UserID)

    '    If dtblCategories.Rows.Count = 0 Then
    '        lblItemDesc.Text = "Code does not exists."
    '        Return False
    '    Else
    '        trowCategory = dtblCategories.Rows(0)
    '        lblItemDesc.Text = trowCategory.Description
    '        ViewState("ShortDesc") = trowCategory.ShortDesc
    '        Return True
    '    End If
    'End Function


#Region "Previous SQL Statement - Promo Type Specific"

    'sqldsData.SelectCommand = "SELECT * FROM Promo.dbo.Promotions AS P LEFT JOIN Promo.dbo.PromoDetails AS D ON P.PromoID = D.PromoID " & _
    '                          "WHERE DepCode = " & CInt(txtDepCode.Text) & " AND SubDepCode = " & CInt(txtSdepCode.Text) & _
    '                          " AND ClassCode = " & CInt(txtClassCode.Text) & " AND PromoTypeID = " & Session("PromoTypeID") & " AND " & _
    '                          "(('" & Session("PeriodFrom") & "' BETWEEN P.PeriodFrom AND P.PeriodTo) OR " & _
    '                          " ('" & Session("PeriodTo") & "' BETWEEN P.PeriodFrom AND P.PeriodTo) OR " & _
    '                          " (P.PeriodFrom BETWEEN '" & Session("PeriodFrom") & "' AND '" & Session("PeriodTo") & "') OR " & _
    '                          " (P.PeriodTo BETWEEN '" & Session("PeriodFrom") & "' AND '" & Session("PeriodTo") & "'))"

#End Region

    Protected Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

        Dim flag As Boolean = True

        ShowPromoItemInfo()

        blistErrorMsg.Items.Clear()

        If IsNothing(ViewState("ShortDesc")) Or ViewState("ShortDesc").ToString = "" Then
            blistErrorMsg.Items.Add("Please enter a valid Dp/SDp/Cl/SCl code.")
            Exit Sub
        End If

        ' default to zero
        If txtSClassCode.Text = "" Then txtSClassCode.Text = "0"

        ' check if already in list
        For Each row As GridViewRow In gridItems.Rows

            ' Added dowcarpio08292012@smretailinc: added new condition for sub class code
            If row.Cells(2).Text = CInt(txtDepCode.Text) And _
                    ((row.Cells(3).Text = CInt(txtSdepCode.Text)) Or (CInt(txtSdepCode.Text) = 0) Or (row.Cells(3).Text = 0)) And _
                    ((row.Cells(4).Text = CInt(txtClassCode.Text)) Or (CInt(txtClassCode.Text) = 0) Or (row.Cells(4).Text = 0)) And _
                    ((row.Cells(5).Text = CInt(txtSClassCode.Text)) Or (CInt(txtSClassCode.Text) = 0) Or (row.Cells(5).Text = 0)) Then
                blistErrorMsg.Items.Add("Dp/SDp/Cl/SCl Code already included in this promo.")
                Exit For
            End If
        Next

        If blistErrorMsg.Items.Count() = 0 Then
            ' added dowcarpio@smretailinc: added error handler for subclass = null or empty
            Try
                ' Added dowcarpio08082013@smretailinc: to validate subclass and class discount promotion Per MPD c/o Ms Jessica
                Dim PromoType As String
                Try
                    PromoType = clsSession.PromoTypeID
                Catch ex As Exception
                    PromoType = 0
                End Try

                If PromoType = 65 Then

                    If CInt(IIf(Trim(txtSClassCode.Text) = "", 0, txtSClassCode.Text)) = 0 Then

                        Throw New Exception("No sub class code specified.")
                    End If

                End If
                '::ToDO:: include 0 - subdep and 0 - class
                ' check if it conflicts with other promos
                ' Added dowcarpio08292012@smretailinc: added new condition for sub class code
                sqldsData.SelectCommand = "SELECT * FROM Promotions AS P LEFT JOIN PromoDetails AS D ON P.PromoID = D.PromoID LEFT JOIN PromoRequests AS PR ON PR.RequestID = P.RequestID " & _
                                          "WHERE DepCode = " & CInt(txtDepCode.Text) & " AND SubDepCode = " & CInt(txtSdepCode.Text) & " AND ClassCode = " & CInt(txtClassCode.Text) & " AND SubClassCode = " & CInt(txtSClassCode.Text) & _
                                          " AND (('" & Session("PeriodFrom") & "' BETWEEN P.PeriodFrom AND P.PeriodTo) OR " & _
                                          " ('" & Session("PeriodTo") & "' BETWEEN P.PeriodFrom AND P.PeriodTo) OR " & _
                                          " (P.PeriodFrom BETWEEN '" & Session("PeriodFrom") & "' AND '" & Session("PeriodTo") & "') OR " & _
                                          " (P.PeriodTo BETWEEN '" & Session("PeriodFrom") & "' AND '" & Session("PeriodTo") & "'))" & _
                                          " AND UPPER(PR.Status) <> 'DRAFT'"

                Dim dvConflicts As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

                clsSession.Message = ""

                For Each drConflict As DataRow In dvConflicts.Table.Rows
                    clsSession.Message &= "Ref# PR-" & drConflict("RequestID") & "<br />"
                Next

                If clsSession.Message <> "" Then
                    flag = False
                    clsSession.Message = "One or more promotional conflicts detected for this item: <br>" & _
                                         "<div style='width:350px; height:130px; overflow:auto; background-color: WhiteSmoke; padding: 10px 10px 10px 10px;'>" & _
                                         clsSession.Message & "</div><br>Do you still wish to continue?"

                    clsSession.Icon = "inquiry"
                    lblPopTitle.Value = "Promotions"
                    ViewState("process") = "add"
                    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('313','508');</script>")
                End If

            Catch ex As Exception

                blistErrorMsg.Items.Add("No sub class code specified.")

                'clear item entry
                txtDepCode.Text = ""
                txtSdepCode.Text = ""
                txtClassCode.Text = ""
                txtSClassCode.Text = ""
                lblItemDesc.Text = ""
                ViewState("ShortDesc") = ""

                Exit Sub

            End Try

        End If

        If flag = True Then
            If blistErrorMsg.Items.Count = 0 Then
                SaveDetailsEntry()
            End If
        End If

    End Sub

    ' Revised dowcarpio08292012@smretailinc: added new param for sub class code and clear sub classcode 
    Private Sub SaveDetailsEntry()

        Dim nPromoTypeID As Integer

        Try
            nPromoTypeID = clsSession.PromoTypeID
        Catch ex As Exception
            nPromoTypeID = 0
        End Try

        ':::::::::::::::::::::::::::::::::::::::::::::
        ' get promo type configuration
        ':::::::::::::::::::::::::::::::::::::::::::::

        Dim strSQL As String
        'Dim drPromoType As DataRow

        'strSQL = "SELECT ProcessType FROM PromoTypes WHERE PromoTypeID = " & nPromoTypeID

        'If Not clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drPromoType) Then
        '    blistErrorMsg.Items.Add("Error accessing database.")
        '    Exit Sub
        'End If

        ':::::::::::::::::::::::::::::::::::::::::::::
        ' add Dep/Sdep/Class code to details
        ':::::::::::::::::::::::::::::::::::::::::::::

        Dim strErrMess As String = ""
        Dim nSubClassCode As Integer = 0

        ' TODO:: Remove hardcoded PromoTypeID
        ' force up to class code only if Class Discount
        If nPromoTypeID = 18 Then
            nSubClassCode = 0
        Else
            nSubClassCode = CInt(IIf(Trim(txtSClassCode.Text) = "", 0, txtSClassCode.Text))
        End If

        If ViewState("ProcessType") = "GenericHostXML" Then

            '::::::::: XML promotions :::::::::'

            strSQL = "INSERT INTO PromoDetails " & _
                        "(PromoID, DepCode, SubDepCode, ClassCode, ShortDesc, PercentDisc, PeriodFrom, PeriodTo, SubClassCode, SeqNo, PosAddItemSlot, IsExcluded, PosRuleLevel, DiscAmount) " & _
                        "VALUES (" & _
                        clsSession.CurrPromoID & ", " & _
                        CInt(txtDepCode.Text) & ", " & _
                        CInt(txtSdepCode.Text) & ", " & _
                        CInt(txtClassCode.Text) & ", " & _
                        "'" & ViewState("ShortDesc") & "', " & _
                        clsSession.PercentDisc & ", " & _
                        "'" & Format(clsSession.PeriodFrom, "yyyy-MM-dd") & "', " & _
                        "'" & Format(clsSession.PeriodTo, "yyyy-MM-dd") & "', " & _
                        nSubClassCode & ", "

            strSQL &= "NULL, "  ' SeqNo
            strSQL &= "1, "     ' PosAddItemSlot - default to 1, update later in UPC function
            strSQL &= "0, "     ' IsExcluded
            strSQL &= "1, "     ' PosRuleLevel
            strSQL &= "0)"      ' DiscAmount

        Else

            '::::::::: standard promotions :::::::::'

            strSQL = "INSERT INTO PromoDetails " & _
                        "(PromoID, DepCode, SubDepCode, ClassCode, ShortDesc, PercentDisc, PeriodFrom, PeriodTo, SubClassCode) " & _
                        "VALUES (" & _
                        clsSession.CurrPromoID & ", " & _
                        CInt(txtDepCode.Text) & ", " & _
                        CInt(txtSdepCode.Text) & ", " & _
                        CInt(txtClassCode.Text) & ", " & _
                        "'" & ViewState("ShortDesc") & "', " & _
                        clsSession.PercentDisc & ", " & _
                        "'" & Format(clsSession.PeriodFrom, "yyyy-MM-dd") & "', " & _
                        "'" & Format(clsSession.PeriodTo, "yyyy-MM-dd") & "', " & _
                        nSubClassCode.ToString & ")"

        End If

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then
            'ShowPromoUPCInfo()

            gridItems.DataBind()
            'sqldsPromoDetails.DataBind()

            'clear item entry
            txtDepCode.Text = ""
            txtSdepCode.Text = ""
            txtClassCode.Text = ""
            txtSClassCode.Text = ""
            lblItemDesc.Text = ""
            ViewState("ShortDesc") = ""

        Else
            blistErrorMsg.Items.Add("Unable to add item hierarchy code.")
        End If


    End Sub

    Protected Sub lnkRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRequest.Click

        clsSession.MsgTransFlowFlag = 2
        Response.Redirect("PromoRequestEntry.aspx")

    End Sub

    Protected Sub lnkBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBranches.Click

        ValidatePromotionDetails()

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        clsSession.IsDepartmental = 0
        Response.Redirect("PromoBranches.aspx")

    End Sub

    Protected Sub lnkPromoInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoInfo.Click

        Response.Redirect("PromoEntry.aspx")

    End Sub

    Protected Sub lnkSaveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click

        '::ToDo:: warn or auto save promo header entry

        ValidatePromotionDetails()

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        Response.Redirect("PromoRequestPreview.aspx")

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        lblItemDesc.Text = hfDesc.Value
        ViewState("ShortDesc") = hfShortDesc.Value

        GC.Collect()

    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Select Case ViewState("process")
            Case "add"
                'conflict
                If clsSession.DeleteStatus = "yes" Then
                    If blistErrorMsg.Items.Count = 0 Then
                        SaveDetailsEntry()
                    End If
                End If

            Case "delete"
                'delete selected item hierarchy
                If clsSession.DeleteStatus = "yes" Then
                    For Each row As GridViewRow In gridItems.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSel")
                        If cb IsNot Nothing And cb.Checked Then
                            'confirm deletion
                            'Response.Write("Deleted: " & row.Cells(2).Text & "-" & row.Cells(3).Text & "-" & row.Cells(4).Text & "<br>")
                            With sqldsPromoDetails.DeleteParameters
                                .Item("PromoID").DefaultValue = Session("CurrPromoID")
                                .Item("DepCode").DefaultValue = row.Cells(2).Text
                                .Item("SubDepCode").DefaultValue = row.Cells(3).Text
                                .Item("ClassCode").DefaultValue = row.Cells(4).Text
                                .Item("SubClassCode").DefaultValue = row.Cells(5).Text ' Added dowcarpio08292012@smretailinc: added new delete param for sub class code 
                            End With
                            sqldsPromoDetails.Delete()
                        End If
                    Next
                End If

            Case "DeleteCoupon"
                'delete selected coupons
                If clsSession.DeleteStatus = "yes" Then

                    Dim CouponList As String = ""

                    imgBarcode.ImageUrl = ""

                    ' get checked entries
                    For Each row As GridViewRow In gridPromoCoupons.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSelCoupon")
                        If cb IsNot Nothing And cb.Checked Then

                            If CouponList = "" Then
                                CouponList = "'" & row.Cells(1).Text & "'"
                            Else
                                CouponList &= ",'" & row.Cells(1).Text & "'"
                            End If

                        End If
                    Next

                    If CouponList <> "" Then

                        Dim strSQL As String
                        Dim strErrMess As String = ""

                        strSQL = "DELETE FROM PromotionCoupons " & _
                                 " WHERE PromoID = 0" & clsSession.CurrPromoID & _
                                 " AND CouponCode IN (" & CouponList & ") "

                        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then
                            ShowPromoCouponList()
                        Else
                            ' error
                        End If

                    End If
                End If

            Case "DeleteUPC"
                'delete selected UPC
                If clsSession.DeleteStatus = "yes" Then

                    Dim UPCList As String = ""

                    ' get checked entries
                    For Each row As GridViewRow In gridPromoUPC.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSelUPC")
                        If cb IsNot Nothing And cb.Checked Then

                            If UPCList = "" Then
                                UPCList = "'" & row.Cells(1).Text & "'"
                            Else
                                UPCList &= ",'" & row.Cells(1).Text & "'"
                            End If

                        End If
                    Next

                    If UPCList <> "" Then

                        Dim strSQL As String
                        Dim strErrMess As String = ""

                        strSQL = "DELETE FROM PromoUPCs " & _
                                 " WHERE PromoID = 0" & clsSession.CurrPromoID & _
                                 " AND UPCno IN (" & UPCList & ") "

                        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then
                            ShowPromoUPCList()
                        Else
                            ' error
                        End If

                    End If
                End If

            Case "DeleteUPCPrem"
                If clsSession.DeleteStatus = "yes" Then

                    Dim UPCPremList As String = ""

                    ' get checked entries
                    For Each row As GridViewRow In gridUPCPromoPrem.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSelUPCPrem")
                        If cb IsNot Nothing And cb.Checked Then

                            If UPCPremList = "" Then
                                UPCPremList = "'" & row.Cells(3).Text & "'"
                            Else
                                UPCPremList &= ",'" & row.Cells(3).Text & "'"
                            End If

                        End If
                    Next

                    If UPCPremList <> "" Then

                        Dim strSQL As String
                        Dim strErrMess As String = ""

                        strSQL = "DELETE FROM PromoPremiumUPCs " & _
                                 " WHERE PromoID = 0" & clsSession.CurrPromoID & _
                                 " AND UPCno IN (" & UPCPremList & ") "

                        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then
                            ShowPromoPremiumList()
                        Else
                            ' error
                        End If

                    End If
                End If


        End Select

        GC.Collect()

    End Sub

    Protected Sub lnkDeleteSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteSelected.Click

        Dim b As Boolean = False
        For Each row As GridViewRow In gridItems.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete Item Code"
            clsSession.Message = "Delete selected item codes"
            clsSession.Icon = "inquiry"
            ViewState("process") = "delete"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        End If

    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridItems.HeaderRow.FindControl("chkALL"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridItems.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridItems.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub chkALL_UPC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridPromoUPC.HeaderRow.FindControl("chkALL_UPC"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridPromoUPC.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelUPC"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridPromoUPC.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelUPC"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub
    Protected Sub chkALL_UPCPrem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridUPCPromoPrem.HeaderRow.FindControl("chkALL_UPCPrem"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridUPCPromoPrem.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelUPCPrem"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridUPCPromoPrem.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelUPCPrem"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub chkALL_Coupon_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridPromoCoupons.HeaderRow.FindControl("chkALL_Coupon"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridPromoCoupons.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelCoupon"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridPromoCoupons.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSelCoupon"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub cmdSearchUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearchUPC.Click

        If gridItems.Rows.Count > 0 Then

            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearchUPC();</script>")
            ShowPromoUPCList()

        End If

    End Sub

    Private Sub ShowPromoCouponList()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT * FROM PromotionCoupons " & _
                    "WHERE PromoID = 0" & clsSession.CurrPromoID

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridPromoCoupons.DataSource = dtTable
        gridPromoCoupons.DataBind()
        ' cleanup
        dtTable = Nothing

        ' limit number of coupon entries
        Dim drRow As DataRow
        Dim nMaxCoupons As Integer = 0

        strQuery = "SELECT POS_MaxCoupons FROM PromoTypes " & _
                    "WHERE PromoTypeID = 0" & ViewState("PromoTypeID")

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

            nMaxCoupons = drRow("POS_MaxCoupons")

            cmdAddCoupon.Enabled = ((nMaxCoupons = -1) Or (nMaxCoupons > 0 And gridPromoCoupons.Rows.Count < nMaxCoupons))
            cmdGenerate.Enabled = ((nMaxCoupons = -1) Or (nMaxCoupons > 0 And gridPromoCoupons.Rows.Count < nMaxCoupons))
            cboDrop_BarcodeManual.Enabled = ((nMaxCoupons = -1) Or (nMaxCoupons > 0 And gridPromoCoupons.Rows.Count < nMaxCoupons))
            lnkDeleteSelectedCoupon.Enabled = (gridPromoCoupons.Rows.Count > 0)

        End If

    End Sub

    Private Sub ShowPromoPremiumList()

        Dim dtTable As New DataTable
        Dim drPromotions As DataRow = Nothing
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT * FROM PromoPremiumUPCs " & _
                    "WHERE PromoID = 0" & clsSession.CurrPromoID

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridUPCPromoPrem.DataSource = dtTable
        gridUPCPromoPrem.DataBind()

        dtTable = Nothing

        strQuery = "SELECT POS_PromoPremUPC_STATE FROM PromoTypes pt " & _
                    "INNER JOIN Promotions p on pt.PromoTypeID = p.PromoTypeID " & _
                    "WHERE p.PromoID = 0" & clsSession.CurrPromoID

        If Not IsDBNull(clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drPromotions)) Then
            If (drPromotions("POS_PromoPremUPC_STATE") = 10) Then
                gridUPCPromoPrem.Columns(6).Visible = False
                txtDiscountedPrice.Visible = False
                lblUPCPremDiscountedPrice.Visible = False
            End If
        End If

        lnkDeleteUPCPromoPrem.Enabled = (gridUPCPromoPrem.Rows.Count > 0)

    End Sub


    Private Sub ShowPromoUPCList()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT * FROM PromoUPCs " & _
                    "WHERE PromoID = 0" & clsSession.CurrPromoID

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridPromoUPC.DataSource = dtTable
        gridPromoUPC.DataBind()

        lnkDeleteSelectedUPC.Enabled = (gridPromoUPC.Rows.Count > 0)

    End Sub

    Protected Sub cmdAddUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddUPC.Click

        Dim strQuery As String
        Dim strErrMess As String = ""
        Dim dtTbl As DataTable

        blistErrorMsg.Items.Clear()

        If gridItems.Rows.Count = 0 Then
            blistErrorMsg.Items.Add("No Dept/SubDept/Class code indicated.")
            Exit Sub
        End If

        If txtUPCno.Text = "" Then
            blistErrorMsg.Items.Add("UPC code not specified.")
            Exit Sub
        End If

        ' check duplicates
        strQuery = "SELECT UPCno FROM PromoUPCs " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID & _
                    "AND UPCno = '" & txtUPCno.Text & "' "

        If cboUPCrem.Enabled Then
            strQuery &= "AND Remark = '" & cboUPCrem.SelectedItem.ToString & "' "
        End If

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTbl) Then
            If dtTbl.Rows.Count > 0 Then
                blistErrorMsg.Items.Add("UPC is already included on the list.")
                Exit Sub
            End If
        End If

        ' check if included under list of departments
        If Not IsValidUPCnumber(txtUPCno.Text, clsSession.CurrRequestID) Then
            blistErrorMsg.Items.Add("Invalid UPC or UPC not under the listed item codes.")
            Exit Sub
        End If

        ' check if same price-point (for B1T1 and AnyXforP)
        If ViewState("ProcessType") = "B1T1" Or _
           ViewState("ProcessType") = "AnyXforP" Then

            If Not IsSamePricePoint(txtUPCno.Text, clsSession.CurrRequestID) Then
                blistErrorMsg.Items.Add("Item should be of same price-point.")
                Exit Sub
            End If

        End If

        '------------------------------------------------
        ' check other promotions for conflicts (UPC level)
        '------------------------------------------------
        'If Not IsValidUPCnumber(txtUPCno.Text, clsSession.CurrRequestID) Then
        '    blistErrorMsg.Items.Add("Item should be of same.")
        '    Exit Sub
        'End If

        ' insert data to PromoUPCs table and show to grid

        If AddUPCtoPromoTable(clsSession.CurrPromoID) Then

            UpdatePromoDetailsData(clsSession.CurrPromoID)

            ShowPromoUPCList()

            'clear UPC entry
            txtUPCno.Text = ""
            lblUPCdesc.Text = ""

            ' Grouping
            ' txtGroupNo.Text = ""
        End If

    End Sub

    Private Function UpdatePromoDetailsData(ByVal nPromoID As Long) As Boolean
        Dim strQuery As String
        Dim strErrMess As String = ""

        strQuery = "UPDATE PromoDetails " & _
                    "SET PosAddItemSlot = 0 " & _
                    "FROM PromoDetails AS D " & _
                    "INNER JOIN PromoUPCs AS U ON U.PromoID = D.PromoID " & _
                         "AND dbo.fn_IsUnderItemLevel(LEFT(ItemCode, 3), D.DepCode,'Dp') = 1 " & _
                         "AND dbo.fn_IsUnderItemLevel(SUBSTRING(ItemCode, 5, 3), D.SubDepCode,'SDp') = 1 " & _
                         "AND dbo.fn_IsUnderItemLevel(SUBSTRING(ItemCode, 9, 3), D.ClassCode,'Cl') = 1 " & _
                         "AND dbo.fn_IsUnderItemLevel(SUBSTRING(ItemCode, 13, 3), D.SubClassCode,'SCl') = 1 " & _
                    "WHERE D.PromoID = 0" & nPromoID

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
            Return True
        Else
            blistErrorMsg.Items.Add("Unable to update promo details: " & strErrMess)
            Return False
        End If

    End Function

    Private Function AddUPCtoPromoTable(ByVal nPromoID As Long) As Boolean

        Dim strQuery As String
        Dim strErrMess As String = ""
        Dim sRemark As String = ""
        Dim nPosRuleLevel As Integer = 1
        Dim nIsExcluded As Integer = 0

        If cboUPCrem.Enabled Then
            sRemark = cboUPCrem.SelectedItem.ToString


            '@@HERE:: change logic
            If ViewState("ProcessType") = "GenericHostXML" Then

                If cboUPCrem.SelectedItem.ToString = "Y" Then
                    nPosRuleLevel = 2

                    If GetPromoRuleCount(nPromoID) > 2 Then
                        nPosRuleLevel += 1
                    End If
                End If

            End If

        End If

        Select Case ViewState("QualifiedItems")
            Case 1, 2, 4
                nIsExcluded = 1
            Case 3
                nIsExcluded = 0
        End Select

        '@@HERE recode AddItemSlot's logic
        strQuery = "INSERT INTO PromoUPCs " & _
                    "(PromoID, SKUno, Description, UPCno, UnitPrice, ItemCode, GroupNo, Remark, PosAddItemSlot, IsExcluded, PosRuleLevel ) " & _
                    "SELECT TOP 1 " & nPromoID & ", SKUno, Description, UPCno, UnitPrice, " & _
                    "dbo.Fn_FormatPromoCode(DeptCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(SubDeptCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(SubClassCode,'SCl'), " & _
                    "1, '" & _
                    sRemark & "', 1, " & _
                    nIsExcluded & ", " & _
                    nPosRuleLevel & " " & _
                    "FROM MMS_UPC_Table " & _
                    "WHERE UPCno = '" & txtUPCno.Text & "' " & _
                    "AND DeptCode IN (SELECT DISTINCT DepCode FROM PromoDetails WHERE PromoID = 0" & nPromoID & ")"

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
            Return True
        Else
            blistErrorMsg.Items.Add("Unable to add UPC to promo: " & strErrMess)
            Return False
        End If

    End Function

    Private Function AddUPCtoPromoPremTable(ByVal nPromoID As Long) As Boolean

        Dim strQuery As String
        Dim strErrMess As String = ""

        If (txtDiscountedPrice.Text = "") Then
            txtDiscountedPrice.Text = 0
        End If

        strQuery = "INSERT INTO PromoPremiumUPCs " & _
                    "(PromoID, SKUno, Description, UPCno, UnitPrice, DiscountedPrice, ItemCode, PosAddItemSlot, IsExcluded) " & _
                    "Select Top 1 " & nPromoID & ", SKUno, Description, UPCno, UnitPrice," & txtDiscountedPrice.Text & ", " & _
                    "dbo.Fn_FormatPromoCode(DeptCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(SubDeptCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(SubClassCode,'SCl'), " & _
                    "0 , 0 FROM MMS_UPC_Table WHERE UPCno = '" & txtPromoPremiumUPCno.Text & "' "

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
            Return True
        Else
            blistErrorMsg.Items.Add("Unable to add UPC to Promo Premium UPC: " & strErrMess)
            Return False
        End If

    End Function

    Private Function IsValidUPCnumber(ByVal sUPCnumber As String, ByVal nRequestID As Long) As Boolean
        Dim strQuery As String

        strQuery = "SELECT Count(*) " & _
                    "FROM Promotions AS P " & _
                    "INNER JOIN PromoDetails AS D ON D.PromoID = P.PromoID " & _
                    "INNER JOIN MMS_UPC_Table AS UPC ON UPC.DeptCode = D.DepCode AND (D.SubDepCode = 0 OR UPC.SubDeptCode = D.SubDepCode) AND (D.ClassCode = 0 OR UPC.ClassCode = D.ClassCode) AND (D.SubClassCode = 0 OR UPC.SubClassCode = D.SubClassCode) " & _
                    "WHERE P.RequestID = 0" & nRequestID.ToString & " " & _
                    "AND UPC.UPCno = '" & sUPCnumber & "'"

        IsValidUPCnumber = (clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery) > 0)

    End Function

    Protected Function IsSamePricePoint(ByVal sUPCnumber As String, ByVal nRequestID As Long) As Boolean

        If gridPromoUPC.Rows.Count > 0 Then
            Dim strQuery As String

            strQuery = "SELECT COUNT(UPCno) " & _
                        "FROM MMS_UPC_Table " & _
                        "WHERE UPCno = '" & sUPCnumber & "' " & _
                        "AND UnitPrice IN (SELECT UPC.UnitPrice " & _
                                         "FROM PromoUPCs AS UPC " & _
                                         "INNER JOIN Promotions AS P ON P.PromoID = UPC.PromoID " & _
                                         "WHERE P.RequestID = 0" & nRequestID.ToString & " )"

            IsSamePricePoint = (clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery) > 0)
        Else

            IsSamePricePoint = True

        End If

    End Function

    Protected Sub cmdProcessUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProcessUPC.Click

        lblUPCdesc.Text = hfDesc.Value

    End Sub

    Protected Sub chkRowSel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(gridItems.HeaderRow.FindControl("chkALL"), CheckBox)

        If Not chk.Checked Then chk.Checked = False

    End Sub

    Protected Sub chkRowSelUPC_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(gridPromoUPC.HeaderRow.FindControl("chkALL_UPC"), CheckBox)

        If Not chk.Checked Then chk.Checked = False

    End Sub

    Protected Sub chkRowSelUPCPrem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(gridUPCPromoPrem.HeaderRow.FindControl("chkALL_UPCPrem"), CheckBox)

        If Not chk.Checked Then chk.Checked = False

    End Sub

    Protected Sub chkRowSelCoupon_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(gridPromoCoupons.HeaderRow.FindControl("chkALL_Coupon"), CheckBox)

        If Not chk.Checked Then chk.Checked = False

    End Sub

    Protected Sub lnkDeleteSelectedUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteSelectedUPC.Click

        Dim b As Boolean = False

        For Each row As GridViewRow In gridPromoUPC.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSelUPC")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete UPC Barcodes"
            clsSession.Message = "Delete selected UPC Barcodes"
            clsSession.Icon = "inquiry"
            ViewState("process") = "DeleteUPC"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        End If

    End Sub

    Private Sub ValidatePromotionDetails()
        blistErrorMsg.Items.Clear()

        If ViewState("ProcessType") = "GenericHostXML" Then

            ' should select at least a Class or UPC code
            If gridItems.Rows.Count() < 1 And (panUPCdetails.Visible And gridPromoUPC.Rows.Count() < 1) Then
                blistErrorMsg.Items.Add("At least one Dp/SDp/Cl/SCl code or UPC should be specified.")
            End If

            If SystemUser.UserGroupType = "SBU" And gridItems.Rows.Count < 1 And gridItems.Visible = True Then
                blistErrorMsg.Items.Add("At least one Dp/SDp/Cl/SCl code or UPC should be specified.")
            End If
        Else

            ' check item hierarcy
            If gridItems.Rows.Count() < 1 And ViewState("ProcessType") <> "" Then
                blistErrorMsg.Items.Add("No Dp/SDp/Cl/SCl code specified.")
            End If

            ' check UPC list
            'If ViewState("ProcessType") <> "SMACdeals" And panUPCdetails.Visible And gridPromoUPC.Rows.Count() < 1 Then
            '    blistErrorMsg.Items.Add("No UPC/barcode specified.")
            'End If
        End If

        ' check eligible item
        If panUPCdetails.Visible Then

            If (ViewState("QualifiedItems") = 3) And (gridPromoUPC.Rows.Count() < 1) Then
                blistErrorMsg.Items.Add("No Eligible Items specified.")
            End If

        End If

        'Check GWP and PWP Items

        If panUPCPromoPremium.Visible And gridUPCPromoPrem.Rows.Count() < 1 Then
            blistErrorMsg.Items.Add("No GWP/PWP Item specified.")
        End If

        ' check Coupon list
        If panPromoCoupons.Visible And gridPromoCoupons.Rows.Count() < 1 Then
            If GetBarcodeRequiredResult(ViewState("PromoTypeID")) = "1" Then
                blistErrorMsg.Items.Add("No Coupon Code/Number specified.")
            End If
        End If

        If blistErrorMsg.Items.Count > 0 Then Exit Sub

        Dim strQuery As String = ""
        Dim drRow As DataRow

        '------------------------------------------------
        ' update Element3 of AnyXY promotion
        '------------------------------------------------
        If ViewState("ProcessType") = "AnyXYforP" Then

            strQuery = "SELECT TOP 1 UPC1.UnitPrice+UPC2.UnitPrice AS UnitPriceXY " & _
                        "FROM PromoUPCs AS UPC1 " & _
                        "INNER JOIN PromoUPCs AS UPC2 ON UPC1.PromoID = UPC2.PromoID " & _
                        "WHERE UPC1.PromoID = 0" & clsSession.CurrPromoID & " " & _
                        "AND UPC1.Remark = 'X' " & _
                        "AND UPC2.Remark = 'Y'"

            If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

                Dim nUnitPriceXY As Single
                Dim sErrMess As String = ""

                nUnitPriceXY = drRow("UnitPriceXY")

                strQuery = "UPDATE Promotions SET " & _
                            "Element3 = " & nUnitPriceXY & " - DiscAmount " & _
                            "WHERE PromoID = 0" & clsSession.CurrPromoID

                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then

                    ' error
                    blistErrorMsg.Items.Add("Error updating promo information: " & sErrMess)
                    Exit Sub

                End If

            Else

                ' error
                blistErrorMsg.Items.Add("Error acquiring promo information.")
                Exit Sub

            End If

        End If

        '------------------------------------------------
        ' generic host XML check
        '------------------------------------------------
        If ViewState("ProcessType") = "GenericHostXML" Then

            ' check if 
        End If

    End Sub

    Protected Sub cmdAddCoupon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddCoupon.Click

        Dim strQuery As String
        Dim strErrMess As String = ""
        Dim dtTbl As DataTable
        Dim drRow As DataRow
        Dim Prefix As String = ""

        blistErrorMsg.Items.Clear()

        If lblCouponCode.Value = "" Then
            blistErrorMsg.Items.Add("Coupon code not specified.")
            Exit Sub
        ElseIf lblCouponCode.Value.Length() < 13 Then
            blistErrorMsg.Items.Add("Coupon barcode must be atleast 13 digits long.")
            Exit Sub
        End If

        ' check duplicates
        strQuery = "SELECT CouponCode FROM PromotionCoupons " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID & _
                    "AND UPCno = '" & lblCouponCode.Value & "' "

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTbl) Then
            If dtTbl.Rows.Count > 0 Then
                blistErrorMsg.Items.Add("Coupon code is already included on the list.")
                Exit Sub
            End If
        End If

        ' insert data to PromoUPCs table and grid
        ' TODO::remove hardcoding of PosRuleLevel = 2
        strQuery = "INSERT INTO PromotionCoupons " & _
                    "(PromoID, CouponCode, PosRuleLevel, PosAddItemSlot, SeqNo) " & _
                    "VALUES (" & clsSession.CurrPromoID & ", '" & Replace(lblCouponCode.Value, "'", "''") & "', 2, 1, 1)"

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
            ShowPromoCouponList()
            AutoAttachment()
            'clear entry
            lblCouponCode.Value = ""

        Else
            blistErrorMsg.Items.Add("Unable to add Coupon Code.")
        End If

    End Sub

    Protected Sub lnkDeleteSelectedCoupon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteSelectedCoupon.Click

        Dim b As Boolean = False

        For Each row As GridViewRow In gridPromoCoupons.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSelCoupon")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete Coupon Code"
            clsSession.Message = "Delete selected coupon code / number?"
            clsSession.Icon = "inquiry"
            ViewState("process") = "DeleteCoupon"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        End If

    End Sub

    Protected Sub btnSearchUPCPromoPrem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchUPCPromoPrem.Click

        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearchPromoPremiumUPC();</script>")
        'ShowPromoUPCList()
    End Sub

    Protected Sub cmdProcessPromoPremUPC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProcessPromoPremUPC.Click
        txtPromoPremiumUPCno.Text = hfPromoPremUPCno.Value
        lblPromoPremiumDesc.Text = hfPromoPremDesc.Value
    End Sub

    Protected Sub btnAddPromoPrem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPromoPrem.Click
        Dim strQuery As String
        Dim strErrMess As String = ""
        Dim dtTbl As DataTable

        blistErrorMsg.Items.Clear()

        If txtPromoPremiumUPCno.Text = "" Then
            blistErrorMsg.Items.Add("UPC code not specified.")
            Exit Sub
        End If

        ' check duplicates
        strQuery = "SELECT UPCno FROM PromoPremiumUPCs " & _
                    "WHERE PromoID = " & clsSession.CurrPromoID & _
                    "AND UPCno = '" & txtPromoPremiumUPCno.Text & "' "

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTbl) Then
            If dtTbl.Rows.Count > 0 Then
                blistErrorMsg.Items.Add("UPC is already included on the list.")
                Exit Sub
            End If
        End If

        If gridUPCPromoPrem.Columns(6).Visible And txtDiscountedPrice.Text = "" Then
            blistErrorMsg.Items.Add("Discounted Price must not be blank.")
        Else
            AddUPCtoPromoPremTable(clsSession.CurrPromoID)

            UpdatePromoDetailsData(clsSession.CurrPromoID)

            ShowPromoPremiumList()

            'clear UPC entry
            txtPromoPremiumUPCno.Text = ""
            lblPromoPremiumDesc.Text = ""
            txtDiscountedPrice.Text = ""

            ' Grouping
            ' txtGroupNo.Text = ""
        End If

    End Sub

    Protected Sub lnkDeleteUPCPromoPrem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteUPCPromoPrem.Click
        Dim b As Boolean = False

        For Each row As GridViewRow In gridUPCPromoPrem.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSelUPCPrem")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete UPC Barcodes"
            clsSession.Message = "Delete selected UPC Barcodes"
            clsSession.Icon = "inquiry"
            ViewState("process") = "DeleteUPCPrem"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        End If
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As EventArgs)
        GenerateBarcode()
    End Sub

    Private Sub GenerateBarcode()

        Dim drRow As DataRow
        Dim drCouponPrepRow As DataRow
        Dim sqlCmd As String
        Dim strQuery As String
        Dim sManual As String = String.Empty
        Dim delimiterComma As String = ", "
        Dim Prefix As String = String.Empty
        Dim GotoGenerate As Boolean = False


        If ViewState("PromoTypeID") <> 0 Then

            strQuery = "SELECT * FROM PromoTypes " & _
                                        "WHERE PromoTypeID = 0" & ViewState("PromoTypeID")

            If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drCouponPrepRow) Then

                Prefix = drCouponPrepRow("CouponBarcodePrefix")

                If ViewState("PromoTypeID") = 2 Or ViewState("PromoTypeID") = 294 Then
                    Prefix = cboDrop_ValueType.SelectedValue
                End If

                If (txtInputBarcode.Text).Length = 13 And cboDrop_BarcodeManual.SelectedValue = 1 Then
                    GotoGenerate = True
                    blistErrorMsg.Items.Clear()
                ElseIf (txtInputBarcode.Text).Length <> 13 And cboDrop_BarcodeManual.SelectedValue = 1 Then
                    blistErrorMsg.Items.Add("Coupon barcode must be atleast 13 digits long.")
                    GotoGenerate = False
                Else
                    If cboDrop_BarcodeManual.SelectedValue = 2 Then
                        GotoGenerate = True
                        blistErrorMsg.Items.Clear()
                    Else
                        blistErrorMsg.Items.Add("The coupon barcode field must not be left empty.")
                        GotoGenerate = False
                    End If
                End If

                If Not Char.IsDigit(txtInputBarcode.Text) And txtInputBarcode.Text <> "" Then
                    blistErrorMsg.Items.Add("Coupon barcode must be a digit.")
                    GotoGenerate = False
                End If
                'Non-Templated
                If (Prefix Is Nothing Or Trim(Prefix) = "") Then
                    GotoGenerate = False
                    blistErrorMsg.Items.Add("There's no prefix assigned.")
                End If

            End If


        End If

        If GotoGenerate Then
            sqlCmd = "USP_GenerateBarcode " & clsSession.CurrRequestID & delimiterComma & "'" & txtInputBarcode.Text & "'" & delimiterComma & "'" & cboDrop_ValueType.SelectedValue & "'"
            If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sqlCmd, drRow) Then

                lblCouponCode.Value = drRow("POSBARCODE").ToString

                If lblCouponCode.Value = "" Then
                    MessageBox("Coupon prefix hasn't been set up.", "do_nothing", "Setup Required", "error")
                End If

            Else
                'do nothing
            End If

            If Not String.IsNullOrEmpty(lblCouponCode.Value) Then
                ' Create the barcode writer

                Dim options As New ZXing.Common.EncodingOptions()
                options.Width = 300
                options.Height = 100
                options.Margin = 3

                Dim writer As New ZXing.BarcodeWriter()
                writer.Format = ZXing.BarcodeFormat.CODE_128
                writer.Options = options

                ' Generate the barcode
                Dim bmp As Bitmap = writer.Write(lblCouponCode.Value)

                ' Save to memory stream as base64
                Using ms As New MemoryStream()
                    bmp.Save(ms, ImageFormat.Png)
                    Dim base64String As String = Convert.ToBase64String(ms.ToArray())
                    imgBarcode.ImageUrl = "data:image/png;base64," & base64String
                End Using
            End If
        End If
    End Sub

    Private Sub LoadBarcode()

        Dim drRow As DataRow
        Dim sqlCmd As String
        Dim barcodeLoader As String

        sqlCmd = "	 SELECT PC.COUPONCODE " & _
                 "   FROM PROMOTIONS PR " & _
                 "   INNER JOIN PROMOTIONCOUPONS pc on " & _
                 "   PC.PROMOID = PR.PROMOID " & _
                 "   WHERE PR.REQUESTID =" & clsSession.CurrRequestID


        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sqlCmd, drRow) Then

            barcodeLoader = drRow("COUPONCODE").ToString()
            If barcodeLoader <> "" And barcodeLoader IsNot Nothing Then
                cmdGenerate.Enabled = False
            Else
                cmdGenerate.Enabled = True
            End If

        Else
            'do nothing
        End If

        If Not String.IsNullOrEmpty(barcodeLoader) Then
            ' Create the barcode writer

            Dim options As New ZXing.Common.EncodingOptions()
            options.Width = 300
            options.Height = 100
            options.Margin = 5

            Dim writer As New ZXing.BarcodeWriter()
            writer.Format = ZXing.BarcodeFormat.CODE_128
            writer.Options = options

            ' Generate the barcode
            Dim bmp As Bitmap = writer.Write(barcodeLoader)

            ' Save to memory stream as base64
            Using ms As New MemoryStream()
                bmp.Save(ms, ImageFormat.Png)
                Dim base64String As String = Convert.ToBase64String(ms.ToArray())
                imgBarcode.ImageUrl = "data:image/png;base64," & base64String
            End Using
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

    Protected Sub FillBarcodeAutoManual()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT CAST(ElementValue AS INT) 'ElementValue', ElementName " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'BarcodeManualAuto' " & _
                    "ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        With cboDrop_BarcodeManual
            .SelectedValue = Nothing
            .ClearSelection()
            .Items.Clear()
            .DataSource = dtTable
            'For Each row As DataRow In dtTable.Rows
            '    ddItem = New ListItem
            '    ddItem.Text = row.Item(1)
            '    ddItem.Value = row.Item(0)
            '    .Items.Add(ddItem)
            'Next row
            .DataBind()
        End With

    End Sub

    Protected Sub FillValueType()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT CAST(ElementValue AS INT) 'ElementValue', ElementName " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'BarcodeCouponType' " & _
                    "ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        With cboDrop_ValueType
            .SelectedValue = Nothing
            .ClearSelection()
            .Items.Clear()
            .DataSource = dtTable
            'For Each row As DataRow In dtTable.Rows
            '    ddItem = New ListItem
            '    ddItem.Text = row.Item(1)
            '    ddItem.Value = row.Item(0)
            '    .Items.Add(ddItem)
            'Next row
            .DataBind()
        End With

    End Sub

    Private Sub DefaultSelection()
        If cboDrop_BarcodeManual.SelectedValue = 2 Then
            tr_CouponManual.Visible = False
            txtInputBarcode.Text = String.Empty
        Else
            tr_CouponManual.Visible = True
        End If
    End Sub

    Private Sub RefreshSelection()
        If cboDrop_BarcodeManual.SelectedValue = 2 Then
            tr_CouponManual.Visible = False
            txtInputBarcode.Text = String.Empty
        Else
            tr_CouponManual.Visible = True
        End If
    End Sub


    Protected Sub cboDrop_BarcodeManual_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDrop_BarcodeManual.SelectedIndexChanged
        DefaultSelection()
    End Sub

    Protected Sub cboDrop_ValueType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDrop_ValueType.SelectedIndexChanged
        imgBarcode.ImageUrl = ""
    End Sub

    Protected Sub AutoAttachment()
        If Not String.IsNullOrEmpty(lblCouponCode.Value) Then
            ' Create the barcode writer
            Dim options As New ZXing.Common.EncodingOptions()
            options.Width = 300
            options.Height = 100
            options.Margin = 3

            Dim writer As New ZXing.BarcodeWriter()
            writer.Format = ZXing.BarcodeFormat.CODE_128
            writer.Options = options

            ' Generate the barcode
            Dim barcodeBmp As Bitmap = writer.Write(lblCouponCode.Value)

            ' === Add extra white space (top margin) ===
            Dim topMargin As Integer = 20 ' adjust this value for more or less space
            Dim newHeight As Integer = barcodeBmp.Height + topMargin
            Dim finalBmp As New Bitmap(barcodeBmp.Width, newHeight)

            Using g As Graphics = Graphics.FromImage(finalBmp)
                g.Clear(Color.White)
                g.DrawImage(barcodeBmp, 0, topMargin)
            End Using
            ' =========================================

            ' Save both to base64 (for UI) and to disk (for posting)
            Using ms As New MemoryStream()
                finalBmp.Save(ms, ImageFormat.Png)
                Dim base64String As String = Convert.ToBase64String(ms.ToArray())

                ' Display in UI
                imgBarcode.ImageUrl = "data:image/png;base64," & base64String

                ' Save to server
                Directory.CreateDirectory(clsSession.AttachmentPath)
                Dim filePath As String = Path.Combine(clsSession.AttachmentPath, lblCouponCode.Value & ".png")
                File.WriteAllBytes(filePath, ms.ToArray())
            End Using
        End If
    End Sub

    Private Function GetBarcodeRequiredResult(ByVal PromotypeID As String) As String
        Dim drRow As DataRow = Nothing
        Dim strQuery As String = ""
        Dim strRequired = ""


        strQuery = "SELECT CouponBarcodePrefix_REQUIRE " & _
                    " FROM Promotypes  " & _
                    " WHERE PromoTypeID = " & PromotypeID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

            strRequired = drRow("CouponBarcodePrefix_REQUIRE").ToString()

        Else
            ' error
        End If

        Return strRequired

    End Function




End Class

