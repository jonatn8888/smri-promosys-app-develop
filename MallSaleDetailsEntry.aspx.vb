Imports System.Data
Imports System.Data.SqlClient

Partial Class MallSaleDetailsEntry
    Inherits System.Web.UI.Page

    Private Sub ShowParticipationDetail()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT E.RowID, E.PromoDescription, T.TypeDesc " & _
                    "FROM EventPartDetails AS E " & _
                    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = E.PromoTypeID " & _
                    "WHERE PEPnumber = '" & ViewState("PEPnumber") & "'"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        trPromotionRow.Visible = True
        gridPromoList.DataSource = dtTable
        gridPromoList.DataBind()

        lnkAddPromo.Visible = True

        If gridPromoList.Rows.Count > 0 Then
            lnkDeletePromo.Visible = True
        Else
            lnkDeletePromo.Visible = False
        End If

        ShowDetailEntryRows(False)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then

            txtDeptCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtSubDeptCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtClassCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")

            If Request("itemcode") <> "" Then
                Dim sItemCode As String

                sItemCode = Request("itemcode")

                txtDeptCode.Text = Left(sItemCode, 3)
                txtSubDeptCode.Text = Mid(sItemCode, 5, 3)
                txtClassCode.Text = Right(sItemCode, 3)

                ShowPromoItemInfo()

            Else

                ' initially hide other controls
                trPromotionRow.Visible = False
                ShowDetailEntryRows(False)

                ViewState("PEPnumber") = ""

            End If

            FillPromoTypeList()

            ViewState("ActionMode") = "View"

        End If

        'trErrorRow.Visible = (blistErrorMsg.Items.Count > 0)

    End Sub

    Protected Sub chkSelectAllRows_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox
        chk = CType(gridPromoList.HeaderRow.FindControl("chkSelectAllRows"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridPromoList.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridPromoList.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub chkRowSel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bAllSelected As Boolean = True

        For Each row As GridViewRow In gridPromoList.Rows
            Dim chkSel As CheckBox
            chkSel = CType(row.FindControl("chkRowSel"), CheckBox)

            If chkSel.Checked = False Then
                bAllSelected = False
                Exit For
            End If
        Next

        Dim chkAll As CheckBox
        chkAll = CType(gridPromoList.HeaderRow.FindControl("chkSelectAllRows"), CheckBox)

        chkAll.Checked = bAllSelected
    End Sub

    Protected Sub lnkDeletePromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeletePromo.Click

        Dim b As Boolean = False

        For Each row As GridViewRow In gridPromoList.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete Promotion"
            clsSession.Message = "Delete selected promotion(s) for this brand?"
            clsSession.Icon = "inquiry"
            ViewState("process") = "DeletePromo"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openmessagebox('','');</script>")
        End If

    End Sub

    Protected Sub cmdSearch_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.ServerClick
        ShowPromoItemInfo()
    End Sub

    Private Sub ShowPromoItemInfo()

        Dim nDeptCode As Integer
        Dim nSubDeptCode As Integer
        Dim nClassCode As Integer

        Try
            ' force zero value for blank entries
            nDeptCode = Val(txtDeptCode.Text)
            nSubDeptCode = Val(txtSubDeptCode.Text)
            nClassCode = Val(txtClassCode.Text)

            Dim drRow As DataRow
            Dim strQuery As String

            strQuery = "SELECT * FROM DepSdepClass " & _
                        "WHERE DeptCode = 0" & nDeptCode & _
                        " AND SubDepCode = 0" & nSubDeptCode & _
                        " AND ClassCode = 0" & nClassCode & _
                        " AND SubClassCode = 0"

            If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

                lblItemDesc.Text = drRow("Description").ToString

                ' get PEPnumber
                strQuery = "SELECT * FROM EventParticipation " & _
                            "WHERE DeptCode = 0" & nDeptCode & _
                            " AND SubDeptCode = 0" & nSubDeptCode & _
                            " AND ClassCode = 0" & nClassCode & _
                            " AND SubClassCode = 0"

                If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
                    ViewState("PEPnumber") = drRow("PEPnumber")
                Else
                    ViewState("PEPnumber") = 0
                End If

                ShowParticipationDetail()

            Else

                lblItemDesc.Text = ""
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearch();</script>")

            End If

        Catch ex As Exception
            blistErrorMsg.Items.Add("Error retrieving item data.")
        Finally
        End Try

    End Sub

    Private Sub ShowDetailEntryRows(ByVal bIsVisible As Boolean)

        trNewPromoRow.Visible = bIsVisible
        trPromoMechanicsRow.Visible = bIsVisible

        blistErrorMsg.Items.Clear()

    End Sub

    Protected Sub cmdReturnSearchItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturnSearchItem.Click

        ShowPromoItemInfo()

    End Sub

    Protected Sub lnkEditMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditMechanics.Click
        hidBox.Value = litMechanics.Text
        clsSession.Mechanics = Server.HtmlDecode(litMechanics.Text.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub

    Protected Sub lnkAddPromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddPromo.Click

        ViewState("ActionMode") = "AddNewPromo"
        ViewState("CurrDetailID") = 0

        trNewPromoRow.Visible = True

        cboPromoType.SelectedValue = -1
        DisplayPromoTypeInfo(-1)

    End Sub

    Protected Sub cboPromoType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPromoType.SelectedIndexChanged

        DisplayPromoTypeInfo(CInt(cboPromoType.SelectedValue))

    End Sub

    Private Sub DisplayPromoTypeInfo(ByVal PromotionType As Integer)

        ' hide all panels initially
        panClassDiscount.Visible = False
        panMarkdown.Visible = False
        panMechanics.Visible = False

        ShowDetailEntryRows(True)

        If PromotionType = -1 Then
            clsSession.PromoTypeID = 0
        Else

            ViewState("PromoTypeID") = PromotionType

            Dim drRow As DataRow
            Dim strQuery As String
            Dim nLayoutID As Integer

            strQuery = "SELECT * FROM PromoTypes " & _
                        "WHERE PromoTypeID = 0" & ViewState("PromoTypeID")

            If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

                nLayoutID = drRow("LayoutID")

                Select Case nLayoutID
                    Case 10, 11, 12 ' class discount (regular and with exempt)

                        panClassDiscount.Visible = True

                        txtDiscount.Text = ""
                        lblMechanics.Text = "Description:"

                        lblDiscount.Text = drRow("DefaultMechanics").ToString

                    Case 20             ' markdown

                        panMarkdown.Visible = True

                        txtMarkdown.Text = ""
                        lblMechanics.Text = "Description:"

                    Case Else   ' all others

                        panMechanics.Visible = True

                        lnkEditMechanics.Enabled = True
                        litMechanics.Text = Server.HtmlDecode(drRow("DefaultMechanics").ToString)
                        lblMechanics.Text = "Mechanics:"

                End Select

            End If

        End If

    End Sub

    Protected Sub cmdSavePromo_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSavePromo.ServerClick

        If SavePromotionInfo() Then

            ' refresh screen
            ShowParticipationDetail()

            ViewState("ActionMode") = "View"

        End If

    End Sub

    Private Function SavePromotionInfo() As Boolean

        Dim strQuery As String = ""
        Dim sErrMess As String = ""
        Dim sPromoDesc As String = ""

        SavePromotionInfo = True

        blistErrorMsg.Items.Clear()

        SavePromotionInfo = IsValidEntries()

        If Not SavePromotionInfo Then
            Exit Function
        End If

        If ViewState("ActionMode") = "AddNewPromo" Then

            ''------------------------------
            '' check if new participation
            ''------------------------------
            If Val(ViewState("PEPnumber")) = 0 Then

                ' get last PEPnumber
                Dim nLastPEPnumber As Long
                Dim sNewPEPNumber As String

                strQuery = "SELECT TOP 1 CAST(PEPnumber AS bigint) FROM EventParticipation ORDER BY PEPnumber DESC"

                nLastPEPnumber = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)

                ' check behaviour if blank table
                'If nLastPEPnumber = 0 Then nLastPEPnumber = 1

                sNewPEPNumber = Right("0000" & (nLastPEPnumber + 1), 4)

                ViewState("PEPnumber") = sNewPEPNumber

                ' create EventParticipation header
                strQuery = "INSERT INTO EventParticipation " & _
                            "(PEPnumber, DeptCode, SubDeptCode, ClassCode, SubClassCode, ItemDescription, EntryDate) VALUES " & _
                            "('" & ViewState("PEPnumber") & "', " & _
                            "0" & txtDeptCode.Text & ", " & _
                            "0" & txtSubDeptCode.Text & ", " & _
                            "0" & txtClassCode.Text & ", " & _
                            "0, " & _
                            "'" & Replace(lblItemDesc.Text, "'", "''") & "', " & _
                            "GETDATE())"

                '"SELECT CAST(scope_identity() AS bigint);"


                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
                    blistErrorMsg.Items.Add("Error creating header information.")
                    SavePromotionInfo = False
                    Exit Function
                End If

                'nRowID = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)

            End If

            ''------------------------------
            '' check if new promo entry
            ''------------------------------

            ' create initial promotion record
            strQuery = "INSERT INTO EventPartDetails " & _
                        "(PEPnumber, PromoTypeID, PromoDescription, PercentDisc, EntryDate, UserID) VALUES " & _
                        "('" & ViewState("PEPnumber") & "', " & cboPromoType.SelectedValue & ", '', 0, GETDATE(), " & SystemUser.UserID & ");" & _
                        "SELECT CAST(scope_identity() AS bigint);"

            ViewState("CurrDetailID") = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)

        End If

        ' PEPnumber and CurrDetailID should exist at this point
        If Val(ViewState("PEPnumber")) = 0 Or Val(ViewState("CurrDetailID")) = 0 Then

            ' error inserting data
            blistErrorMsg.Items.Add("Error accessing database. Unable to save information.")

            SavePromotionInfo = False

        Else

            '**************************'
            ' update details table
            '**************************'

            If panMarkdown.Visible Then

                '=========================================
                ' Markdown Promo
                '=========================================

                ' remove excess percent symbol
                sPromoDesc = Replace(Trim(txtMarkdown.Text) & lblMarkdownDesc.Text, "%%", "%")
                sPromoDesc = Replace(Server.HtmlEncode(sPromoDesc), "'", "''")

                clsSession.PercentDisc = 0

                strQuery = "UPDATE EventPartDetails SET " & _
                            "PromoDescription = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0 " & _
                            "WHERE PEPnumber = '" & ViewState("PEPnumber") & "' " & _
                            "AND RowID = 0" & ViewState("CurrDetailID")

            ElseIf panClassDiscount.Visible Then

                '=========================================
                ' Class Discount
                '=========================================
                Dim nPercentDisc As Integer

                'sPromoDesc = Replace(Server.HtmlEncode(Trim(txtDiscount.Text) & lblDiscount.Text), "'", "''")
                sPromoDesc = Replace(Trim(txtDiscount.Text) & lblDiscount.Text, "'", "''")
                nPercentDisc = CInt(txtDiscount.Text)

                strQuery = "UPDATE EventPartDetails SET " & _
                            "PromoDescription = '" & sPromoDesc & "', " & _
                            "PercentDisc = 0" & nPercentDisc & " " & _
                            "WHERE PEPnumber = '" & ViewState("PEPnumber") & "' " & _
                            "AND RowID = 0" & ViewState("CurrDetailID")

            Else

                ' save standard data
                strQuery = "UPDATE EventPartDetails SET " & _
                            "PromoDescription = '" & Replace(Server.HtmlEncode(litMechanics.Text), "'", "''") & "', " & _
                            "PercentDisc = 0" & clsSession.PercentDisc & " " & _
                            "WHERE PEPnumber = '" & ViewState("PEPnumber") & "' " & _
                            "AND RowID = 0" & ViewState("CurrDetailID")

            End If

            '*******************'
            '** execute query **'
            '*******************'

            If strQuery <> "" Then
                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
                    blistErrorMsg.Items.Add("Error accessing database. Unable to save information.")
                    SavePromotionInfo = False
                End If
            End If

        End If


    End Function

    Private Function IsValidEntries() As Boolean

        blistErrorMsg.Items.Clear()

        If cboPromoType.SelectedValue < 0 Then
            blistErrorMsg.Items.Add("Please select a Promo Type before proceeding.")
            IsValidEntries = False
            Exit Function
        End If

        '******************************************
        ' Check if promo type is already in list
        '******************************************
        Dim strQuery As String = ""
        Dim sErrMess As String = ""
        Dim nCount As Integer

        strQuery = "SELECT COUNT(*) FROM EventPartDetails " & _
                    "WHERE PEPnumber = '" & ViewState("PEPnumber") & "' " & _
                    "AND PromoTypeID = 0" & cboPromoType.SelectedValue & " " & _
                    "AND RowID <> 0" & CInt(ViewState("CurrDetailID").ToString)

        'If ViewState("ActionMode") = "AddNewPromo" Then
        '    strQuery &= " AND RowID <> 0" & CInt(ViewState("CurrDetailID").ToString)
        'End If

        nCount = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)
        If nCount > 0 Then
            blistErrorMsg.Items.Add("Promo Type is already included in the list.")
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

        ' flag successful entry
        IsValidEntries = (blistErrorMsg.Items.Count = 0)

    End Function

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Select Case ViewState("process")

            Case "DeletePromo"
                If clsSession.DeleteStatus = "yes" Then

                    Dim PromoList As String = ""

                    ' get checked entries
                    For Each row As GridViewRow In gridPromoList.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSel")
                        If cb IsNot Nothing And cb.Checked Then

                            If PromoList = "" Then
                                PromoList = row.Cells(1).Text
                            Else
                                PromoList = PromoList & "," & row.Cells(1).Text
                            End If

                        End If
                    Next

                    If PromoList <> "" Then

                        Dim strSQL As String
                        Dim strErrMess As String

                        ' TODO: stored procedure
                        strSQL = "DELETE FROM EventPartDetails " & _
                                 " WHERE PEPnumber = '" & ViewState("PEPnumber") & "' " & _
                                 " AND RowID IN (" & PromoList & ") "

                        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then

                            'TODO: log action

                            ShowParticipationDetail()
                        Else
                            ' error
                        End If

                    End If
                End If

        End Select

    End Sub

    Protected Sub cmdReturnMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturnMechanics.Click
        litMechanics.Text = hidBox.Value
    End Sub

    Protected Sub gridPromoList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridPromoList.RowCommand

        If e.CommandName.ToUpper = "SELECT" Then

            Dim nRowSelected As Integer

            nRowSelected = e.CommandArgument

            ViewState("CurrDetailID") = gridPromoList.Rows(nRowSelected).Cells(1).Text

            ViewState("ActionMode") = "EditDetail"

            ShowPromoDetailsInfo()

        End If

    End Sub

    Protected Sub gridPromoList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromoList.RowCreated
        If (e.Row.RowType = DataControlRowType.DataRow) Then

            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#E2DED6';document.body.style.cursor='hand'")
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

            'e.Row.ToolTip = "Click to select row"
            e.Row.Attributes("onclick") = Me.Page.ClientScript.GetPostBackClientHyperlink(gridPromoList, "Select$" & e.Row.RowIndex)
        End If

    End Sub

    Private Sub ShowPromoDetailsInfo()

        Dim strQuery As String
        Dim drRow As DataRow

        strQuery = "SELECT * FROM EventPartDetails " & _
                     " WHERE PEPnumber = '" & ViewState("PEPnumber") & "' " & _
                     " AND RowID = " & ViewState("CurrDetailID")

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then

            DisplayPromoTypeInfo(CInt(drRow("PromoTypeID")))

            cboPromoType.SelectedValue = CInt(drRow("PromoTypeID"))

            If panClassDiscount.Visible Then
                txtDiscount.Text = drRow("PercentDisc")

            ElseIf panMarkdown.Visible Then
                Dim nPos As Integer = InStr(UCase(drRow("PromoDescription").ToString), UCase(lblMarkdownDesc.Text)) - 1

                If nPos < 0 Then nPos = 0

                txtMarkdown.Text = Left(drRow("PromoDescription").ToString, nPos)

            Else
                litMechanics.Text = Server.HtmlDecode(drRow("PromoDescription").ToString)

            End If

        Else
            ' error retrieving data
        End If

    End Sub

    Private Sub FillPromoTypeList()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' TODO:: add field forEventPart
        'strQuery = "SELECT TypeDesc, PromoTypeID FROM PromoTypes WHERE PromoTypeID IN (7,18,85,86,87) ORDER BY TypeDesc"
        strQuery = "SELECT TypeDesc, PromoTypeID FROM PromoTypes WHERE ForEventParticipation = 1 ORDER BY TypeDesc"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboPromoType.DataSource = dtTable
        cboPromoType.DataBind()

    End Sub

    Protected Sub gridPromoList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromoList.RowDataBound

        Dim nDescCol As Integer = 3

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(nDescCol).Text = Server.HtmlDecode(e.Row.Cells(nDescCol).Text)
        End If

    End Sub

    Protected Sub lnkViewList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkViewList.Click

        Response.Redirect("MallSaleDetailListing.aspx")

    End Sub
End Class
