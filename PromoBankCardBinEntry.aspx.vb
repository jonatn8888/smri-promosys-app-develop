
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class PromoBankCardBinEntry
    Inherits System.Web.UI.Page

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0) Then Response.Redirect("InvalidAccess.aspx")

    End Sub

    Protected Sub cmdPopOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopOK.Click

        'HiddenField1.Value = sBranchList -- return value
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.texteditorwindow.hide();</script>")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' save PR or PromoID

        Try
            ViewState("PromoID") = clsSession.CurrPromoID       ' if clsSession.currPromoID = nothing

        Catch ex As Exception

            ViewState("PromoID") = 0
            ' OR redirect to invalid page

        End Try


        If Not Page.IsPostBack() Then

            ' TODO: delete line
            'Session("TransFlag") = "natural"

            FillBankNameList()
            RefreshMainGridView()

            '-- allow numeric values only
            txtBankBIN.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtBankPanLow.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtBankPanHigh.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")

        End If

    End Sub

    Protected Sub FillBankNameList()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT ElementName, ElementValue " & _
                    "FROM ResListValues " & _
                    "WHERE GroupName = 'BankNames' " & _
                    "ORDER BY ElementName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        With cboBankNames
            .ClearSelection()
            .Items.Clear()

            If dtTable.Rows.Count <> 1 Then
                Dim ddItem As New ListItem

                ddItem.Text = "-- Select Bank --"
                ddItem.Value = -1

                .Items.Add(ddItem)
            End If

            .DataSource = dtTable
            .DataBind()
        End With

        If cboBankNames.Items.Count = 1 Then
            cboBankNames.SelectedIndex = 0
        Else
            cboBankNames.SelectedValue = -1
        End If

        Dim e As System.EventArgs = Nothing
        'cboBankNames_SelectedIndexChanged(Me, e)

    End Sub

    'grid checkboxes
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

    Protected Sub chkRowSel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(gridPromoBankBins.HeaderRow.FindControl("chkALL"), CheckBox)

        If Not chk.Checked Then chk.Checked = False

    End Sub


    Protected Sub cmdAddToBIN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddToBIN.Click
        ' add to list
        Dim strQuery As String
        Dim strErrMess As String = ""
        Dim dtTbl As DataTable

        blistErrorMsg.Items.Clear()

        If cboBankNames.SelectedIndex = 0 Then
            blistErrorMsg.Items.Add("No bank selected")
            Exit Sub
        End If

        If txtBankBIN.Text = "" Then
            blistErrorMsg.Items.Add("BIN not specified")
            Exit Sub
        End If

        If Not IsNumeric(txtBankBIN.Text) Or txtBankBIN.Text.Length < 6 Then
            blistErrorMsg.Items.Add("Invalid BIN")
            Exit Sub
        End If

        'If txtBankPanLow.Text = "" Then
        '    blistErrorMsg.Items.Add("Pan Low not specified")
        '    Exit Sub
        'End If

        'If txtBankPanHigh.Text = "" Then
        '    blistErrorMsg.Items.Add("Pan High not specified")
        '    Exit Sub
        'End If


        ' check duplicates
        strQuery = "SELECT CardBIN FROM PromoBankBins " & _
                    "WHERE PromoID = 0" & ViewState("PromoID") & " " & _
                    "AND CardBIN = '" & txtBankBIN.Text & "' "

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTbl) Then
            If dtTbl.Rows.Count > 0 Then
                blistErrorMsg.Items.Add("This BIN is already included on the list.")
                Exit Sub
            End If
        End If

        AddPromoBankBinRange(clsSession.CurrPromoID)

        'refresh grid
        RefreshMainGridView()

        'clear UPC entry
        txtBankBIN.Text = ""
        txtBankPanLow.Text = ""
        txtBankPanHigh.Text = ""

        txtBankBIN.Focus()

    End Sub

    Private Sub RefreshMainGridView()

        Dim dtTable As New DataTable
        Dim drPromotions As DataRow = Nothing
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT * FROM PromoBankBins " & _
                    "WHERE PromoID = 0" & ViewState("PromoID")

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridPromoBankBins.DataSource = dtTable
        gridPromoBankBins.DataBind()

        dtTable = Nothing

        'lnkDeleteUPCPromoPrem.Enabled = (gridUPCPromoPrem.Rows.Count > 0)

    End Sub

    Private Function AddPromoBankBinRange(ByVal nPromoID As Long) As Boolean

        Dim strQuery As String
        Dim strErrMess As String = ""

        strQuery = "INSERT INTO PromoBankBins " & _
                    "(PromoID, CardName, CardBIN, CardPanLow, CardPanHigh, CardPanLength) " & _
                    "SELECT 0" & nPromoID & ", '" & _
                                cboBankNames.SelectedItem.ToString() & "', '" & _
                                txtBankBIN.Text & "', '" & _
                                txtBankPanLow.Text & "', '" & _
                                txtBankPanHigh.Text & "', '" & _
                                0 & "' "

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, strErrMess) Then
            Return True
        Else
            blistErrorMsg.Items.Add("Unable to add BINto Promo Premium UPC: " & strErrMess)
            Return False
        End If

    End Function

    Protected Sub lnkDeleteSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteSelected.Click

        'delete checked
        DeleteBankBinEntry()

        '-----------------------------------
        ' popup prompt
        '-----------------------------------

        'Dim b As Boolean = False

        'For Each row As GridViewRow In gridPromoUPC.Rows
        '    Dim cb As CheckBox = row.FindControl("chkRowSelUPC")
        '    If cb IsNot Nothing AndAlso cb.Checked Then
        '        b = True
        '        Exit For
        '    End If
        'Next

        'If b = True Then
        '    lblPopTitle.Value = "Delete UPC Barcodes"
        '    clsSession.Message = "Delete selected UPC Barcodes"
        '    clsSession.Icon = "inquiry"
        '    ViewState("process") = "DeleteUPC"
        '    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        'End If

    End Sub

    Private Sub DeleteBankBinEntry()

        Dim sEntryList As String = ""

        ' get checked entries
        For Each row As GridViewRow In gridPromoBankBins.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing And cb.Checked Then

                If sEntryList = "" Then
                    sEntryList = "'" & row.Cells(3).Text & "'"
                Else
                    sEntryList &= ",'" & row.Cells(3).Text & "'"
                End If

            End If
        Next

        If sEntryList <> "" Then

            Dim strSQL As String
            Dim strErrMess As String = ""

            strSQL = "DELETE FROM PromoBankBins " & _
                     " WHERE PromoID = 0" & clsSession.CurrPromoID & _
                     " AND CardBIN IN (" & sEntryList & ") "

            If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, strErrMess) Then
                RefreshMainGridView()
            Else
                ' error
            End If

        End If

    End Sub


End Class
