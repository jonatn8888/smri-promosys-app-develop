Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.Data.SqlClient

Partial Class PromoMemoEntry
    Inherits System.Web.UI.Page

    Private ReadOnly ConnStr As String = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString

    Private Sub LoadPromoRequestInfo()
        Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()
        Dim dtPromoRequests As dsPromotions.PromoRequestsDataTable
        Dim rowPromoRequest As dsPromotions.PromoRequestsRow

        ':: Request("v2") = RequestID
        dtPromoRequests = taPromoRequests.GetPromoRequestByID(ViewState("RequestID"))

        If dtPromoRequests.Rows.Count = 0 Then
            ' ::ToDo:: data error page
        Else

            lblMemoDate.Text = Today.ToLongDateString()

            rowPromoRequest = dtPromoRequests.Rows(0)

            With rowPromoRequest
                lblRequestID.Text = Format(.RequestID, "0####")
                lblBranches.Text = .Branches.ToString()
                txtPromoTitle.Text = .Title.ToString()

                txtPeriodFrom.Text = .PromoPeriodFrom
                txtPeriodTo.Text = .PromoPeriodTo

                ViewState("OwnerGroup") = .OwnerGroup

                ' hide promo item for SBU Marketing and BCR
                If .GroupType = "SBU" Or .GroupType = "BCR" Then

                    gridPromotions.Columns(1).Visible = False

                End If

                hidIsPTFilter.Value = .GroupType

            End With
        End If
    End Sub

    Private Sub LoadMemoInformation()

        Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter()
        Dim dtMemos As dsPromotions.MemosDataTable
        Dim rowMemos As dsPromotions.MemosRow

        'new request string for memoid and requestid (encrypted)
        'dtMemos = taMemos.GetMemoByID(clsSession.CurrMemoID)
        dtMemos = taMemos.GetMemoByID(ViewState("MemoID"))

        If dtMemos.Rows.Count = 0 Then

            '::ToDO:: ERROR redirection
            ' No Memo Record Found, Possible data corruption, etc...
            Response.Redirect("InvalidAccess.aspx")

        Else

            rowMemos = dtMemos.Rows(0)

            With rowMemos

                If .MemoNumber.ToString() = "" Then
                    lblMemoID.Text = "Draft #" & Format(.MemoID, "0####")
                Else
                    lblMemoID.Text = .MemoNumber.ToString()
                End If

                lblMemoDate.Text = .MemoDate.ToLongDateString()

                'clsSession.CurrRequestID = CInt(.RequestID)

                lblRequestID.Text = "PR-" & Format(.RequestID, "0####") & Format(Now.Year, "-0#")

                txtPromoTitle.Text = .Title.ToString()
                txtPeriodFrom.Text = .PromoPeriodFrom.ToShortDateString()
                txtPeriodTo.Text = .PromoPeriodTo.ToShortDateString()

                lblBranches.Text = .Branches.ToString()

                Dim sMemoGuidelines As String = Server.HtmlDecode(.Guidelines.ToString())

                ' delete additional details if any
                If InStr(sMemoGuidelines, "<ExtraDetails>") > 0 Then
                    sMemoGuidelines = Left(sMemoGuidelines, InStr(sMemoGuidelines, "<ExtraDetails>") - 1)
                End If

                litGuidelines.Text = sMemoGuidelines

                ViewState("OwnerGroup") = .OwnerGroup

                lblRemarks.Text = ""        ' clear remarks (for a fresh start)

                ' hide promo item for SBU Marketing Requestor
                If .GroupType = "SBU" Or .GroupType = "BCR" Then

                    gridPromotions.Columns(1).Visible = False

                End If

                ' Added 03022013@smretailinc: to filder promotype per group
                hidIsPTFilter.Value = .GroupType
            End With
        End If

    End Sub

    Private Sub StampPromoEventCode(ByRef nSeriesStart As Integer, ByRef nSeriesEnd As Integer)

        ' generate new event code
        Dim NewPromoEventCode As String

        ' get last event code for the year (check only those with memo)
        sqldsData.SelectCommand = "SELECT TOP 1 PromoEventCode, PromoID " & _
                                  "FROM Promotions AS P LEFT JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                                  "WHERE(T.WithEventCode = 1) AND P.PromoEventCode BETWEEN 0" & nSeriesStart & " AND 0" & nSeriesEnd & _
                                  " AND MemoID IN (SELECT MemoID FROM Memos WHERE Year(PromoPeriodFrom) = Year(GetDate()))" & _
                                  "ORDER BY PromoEventCode DESC"

        Dim dvPromo As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dvPromo.Table.Rows.Count = 0 Then
            NewPromoEventCode = Format(nSeriesStart, "0###")
        Else
            ' create new event number
            Dim dr As DataRow = dvPromo.Table.Rows(0)
            NewPromoEventCode = Format(Val(dr("PromoEventCode")) + 1, "0###")
        End If

        lblPromoEventCode.Text = NewPromoEventCode

        ' stamp event number to this promotion
        sqldsData.UpdateCommand = "UPDATE Promotions SET PromoEventCode = '" & NewPromoEventCode & "' " & _
                                  "WHERE RequestID = 0" & ViewState("RequestID")
        sqldsData.Update()

    End Sub

    Private Sub DisplaySMACDetails()

        ' new request string for requestid
        ' get request promotion type
        sqldsData.SelectCommand = "SELECT T.*, P.* FROM Promotions AS P " & _
                                  "LEFT JOIN PromoTypes AS T ON P.PromoTypeID = T.PromoTypeID " & _
                                  "WHERE RequestID = 0" & ViewState("RequestID")

        Dim dvPromo As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        Dim dr As DataRow = dvPromo.Table.Rows(0)

        ' check if SMAC details should be shown
        If dr("WithEventCode") = True Then

            panSMACdetails.Visible = True

            '' assign event code if there's currently none
            'If dr("PromoEventCode").ToString() = "" Then
            '    If CInt(dr("LayoutID").ToString()) = 3 Then
            '        StampPromoEventCode(8000, 9999)
            '    Else
            '        StampPromoEventCode(1, 7999)
            '    End If
            'Else
            '    lblPromoEventCode.Text = dr("PromoEventCode").ToString()
            'End If

            ' set object's client-side script
            txtVendorCode.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")
            'txtPercentDisc.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")
            'txtDiscAmount.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")

            ' retrieve promotion information
            cboMerchGroup.SelectedValue = dr("MerchGroup").ToString()
            cboBusinessUnit.SelectedValue = dr("BusinessUnit").ToString()
            cboCompSponsorship.SelectedValue = dr("CompSponsorship").ToString()

            txtVendorCode.Text = dr("VendorCode").ToString()
            txtDiscAmount.Text = dr("DiscAmount").ToString()
            txtBarcode.Text = dr("PromoBarCode").ToString()
            txtPercentDisc.Text = 0

            'lblDiscountAmount.Text = txtDiscAmount.Text


            'If dr("PromoTypeID") = 48 Then
            '    'txtBarcode.Text = dr("PromoBarcode")
            '    txtBarcode.Enabled = False
            'Else
            '    txtBarcode.Attributes.Add("onkeypress", "return AllowDecimalOnly(this);")
            'End If

            ' show the "Specific Details" row
            tblrowSMACDetails.Visible = True

        Else
            panSMACdetails.Visible = False
            tblrowSMACDetails.Visible = False
        End If

    End Sub

    Private Sub LoadGuidelinesFromRequest(ByVal nPromoRequestID As Long)

        Dim strSQLcmd As String
        Dim drPromotion As DataRow = Nothing

        strSQLcmd = "SELECT TemplatedGuideline FROM PromoRequests " & _
                    "WHERE RequestID = 0" & nPromoRequestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromotion) Then
            litGuidelines.Text = drPromotion("TemplatedGuideline").ToString()
        Else
            blistErrorMsg.Items.Add("Unable to retrieve promo request information.")
        End If

    End Sub

    Private Sub LoadDefaultGuidelines()
        Dim taPromoTypes As New dsPromotionsTableAdapters.PromoTypesTableAdapter()
        Dim dtPromoTypes As dsPromotions.PromoTypesDataTable
        Dim rowPromoType As dsPromotions.PromoTypesRow

        ' get promo types from requests and display corresponding guidelines
        sqldsData.SelectCommand = "SELECT DISTINCT PromoTypeID FROM Promotions WHERE RequestID = 0" & ViewState("RequestID")

        Dim dvPromo As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        For Each dr As DataRow In dvPromo.Table.Rows
            dtPromoTypes = taPromoTypes.GetPromoTypeByID(dr("PromoTypeID"))
            rowPromoType = dtPromoTypes.Rows(0)

            litGuidelines.Text &= "<p><b>" & rowPromoType.TypeDesc & "</b><br />" & rowPromoType.DefaultGuideline & "</p>"
        Next

        'dtPromoTypes = taPromoTypes.GetPromoTypeByID(nTypeID)
    End Sub

    Private Sub AppendExtraDetailsToGuidelines(ByVal RefMemoID As Integer)

        Dim NewGuidelines As String

        sqldsData.SelectCommand = "SELECT Guidelines FROM Memos WHERE MemoID = 0" & RefMemoID

        Dim dvMemos As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If dvMemos.Table.Rows.Count > 0 Then
            Dim drMemo As DataRow = dvMemos.Table.Rows(0)

            ' get guidelines
            NewGuidelines = Server.HtmlDecode(drMemo("Guidelines"))

            ' delete details if its already there (redundancy check)
            If InStr(NewGuidelines, "<ExtraDetails>") > 0 Then
                NewGuidelines = Left(NewGuidelines, InStr(NewGuidelines, "<ExtraDetails>") - 1)
            End If

            ' append details
            NewGuidelines &= "<ExtraDetails><pre style=""font-family: Verdana, Arial, sans serif;"">" & _
                             "<b><u>SPECIFIC DETAILS</u></b> <br/>" & _
                             "  Discount" & StrDup(2, vbTab) & ": " & IIf(Val(txtPercentDisc.Text) > 0, txtPercentDisc.Text & "%", "Php " & Format(Val(txtDiscAmount.Text), "#,##0.00")) & "<br/>" & _
                             "  Merchandise Group" & StrDup(1, vbTab) & ": " & cboMerchGroup.SelectedItem.Text & "<br/>" & _
                             "  Business Unit" & StrDup(2, vbTab) & ": " & cboBusinessUnit.SelectedItem.Text & "<br/>" & _
                             "  Company to Shoulder" & StrDup(1, vbTab) & ": " & cboCompSponsorship.SelectedItem.Text & "<br/>" & _
                             "  Vendor Code" & StrDup(2, vbTab) & ": " & txtVendorCode.Text & "<br/>" & _
                             "</pre></ExtraDetails>"
            '                             "  Event Code" & StrDup(2, vbTab) & ": " & lblPromoEventCode.Text & "<br/>" & _
            ' -- "  Barcode" & StrDup(3, vbTab) & ": " & txtBarcode.Text & "<br/>" & _

            ' save new guidelines
            With sqldsData
                .UpdateCommand = "UPDATE Memos SET Guidelines = @Guidelines " & _
                                 "WHERE MemoID = 0" & RefMemoID
                .UpdateParameters.Add("Guidelines", Server.HtmlEncode(NewGuidelines))
                .Update()
            End With

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '::ToDo:: parameter and security checking

        If SystemUser.UserID = 0 Or SystemUser.UserLevel > SystemUser.UserRoles.Analyst Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack() Then

            ViewState("RequestID") = clsEncryptDecrypt.DecryptText(Request("v2").ToString, SystemUser.UserName).ToString

            If Request("xmode") = 1 Then    ' edit exisiting

                ViewState("MemoID") = clsEncryptDecrypt.DecryptText(Request("v1"), SystemUser.UserName).ToString()

                ' check for MemoID
                If ViewState("MemoID") = 0 Then Response.Redirect("InvalidAccess.aspx")

                'load memo information
                LoadMemoInformation()

                'display SMAC details (if any)
                'DisplaySMACDetails()

            Else                            ' create new memo

                ' check for RequestID
                If ViewState("RequestID") = 0 Then Response.Redirect("InvalidAccess.aspx")

                LoadPromoRequestInfo()

                If IsTemplatedPromo(ViewState("RequestID")) = True And IsRebateTemplatePromo(ViewState("RequestID") = False) Then
                    LoadGuidelinesFromRequest(ViewState("RequestID"))
                ElseIf IsTemplatedPromo(ViewState("RequestID")) = True And IsRebateTemplatePromo(ViewState("RequestID") = True) Then
                    LoadDefaultGuidelines()
                Else
                    LoadDefaultGuidelines()
                End If
            End If

            DisplaySMACDetails()            ' show/hide SMAC-related entry fields

            sqldsPromos.SelectParameters("RequestID").DefaultValue = ViewState("RequestID")

            'cboPromoType.Items.Clear()
            'cboPromoType.Items.Insert(0, New ListItem("- Select Promo Type -", "-1"))
            ' filter promotype per group
            If hidIsPTFilter.Value = "SBU" Then

                sqldsPromoType.SelectCommand = "SELECT * FROM PromoTypes WHERE (ForMPDuseOnly = 0) AND GroupType = '" & hidIsPTFilter.Value & "' ORDER BY TypeDesc"
                cboPromoType.DataBind()

            Else

                sqldsPromoType.SelectCommand = "SELECT * FROM PromoTypes WHERE IsExclusive = '0' ORDER BY TypeDesc"
                cboPromoType.DataBind()

            End If

        End If

        lnkEditPromo.Visible = (gridPromotions.Rows.Count() > 0)

        If clsSession.PromoTypeID.ToString() = "59" Then
            Dim sqlText As String = sqldsPromos.SelectCommand
            sqlText = sqlText.Replace("P.PromoDesc", "dbo.Fn_FormatPromoDesc(P.PromoID," & SystemUser.UserID & ") AS PromoDesc")
            sqlText = sqlText.Replace("@RequestID", "'" & Session("CurrRequestID") & "'")
            sqldsPromos.SelectCommand = sqlText

            lnkEditPromo.Visible = False
            txtPromoTitle.Enabled = False
            txtPeriodFrom.Enabled = False
            txtPeriodTo.Enabled = False
            cboPromoType.Visible = False
            lnkInsertGuidelines.Visible = False
        End If
        'rbs7281 
        calPeriodFrom.Disabled = False
        calPeriodTo.Disabled = False
        txtPeriodFrom.ReadOnly = False
        txtPeriodTo.ReadOnly = False

    End Sub

    Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

        Const nCol As Integer = 2
        Static rowPrevious As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            'rowPrevious.Cells(nCol).Text = "<a href='PromoEntry.aspx?PromoID=" & lb.Text & "'>" & rowPrevious.Cells(nCol).Text & "</a>"
            e.Row.Cells(nCol).Text = Server.HtmlDecode(e.Row.Cells(nCol).Text)
        End If

        If e.Row.RowIndex = 0 Then rowPrevious = e.Row

        If e.Row.RowIndex > 0 Then

            ' merge Description cells with same PromoID
            Dim lbPrev As Label = rowPrevious.FindControl("lblPromoID")
            Dim lbCurr As Label = e.Row.FindControl("lblPromoID")

            If lbCurr.Text = lbPrev.Text Then

                If rowPrevious.Cells(nCol).RowSpan < 2 Then
                    rowPrevious.Cells(0).RowSpan = 2
                    rowPrevious.Cells(nCol).RowSpan = 2
                Else
                    rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
                    rowPrevious.Cells(nCol).RowSpan = rowPrevious.Cells(nCol).RowSpan + 1
                End If

                e.Row.Cells(0).Visible = False
                e.Row.Cells(nCol).Visible = False
            Else
                rowPrevious = e.Row
            End If

        End If

    End Sub

    Protected Sub lnkCreateMemoDraft_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click

        blistErrorMsg.Items.Clear()

        ' check promo title
        If txtPromoTitle.Text = "" Then
            blistErrorMsg.Items.Add("Promo title should not be left blank.")
        End If

        ' Validate promo start and end date.
        Try

            If Not IsDate(txtPeriodFrom.Text) Then

                blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format

            End If
            If CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then
                blistErrorMsg.Items.Add("Start date of promo must not be later than the end date.")

            End If

            'If CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then
            '    blistErrorMsg.Items.Add("End of promo end date must not be earlier than the start date.")
            'End If

        Catch ex As Exception

            blistErrorMsg.Items.Add("Blank or invalid promo start date format.")

        End Try

        ' check SMAC deals details
        If panSMACdetails.Visible Then

            '
            ' for other promo types with event code
            '
            'If Val(txtPercentDisc.Text) > 0 And Val(txtDiscAmount.Text) > 0 Then
            '    blistErrorMsg.Items.Add("Only one mode of discount should have a value.")
            'End If

            'If Val(txtPercentDisc.Text) = 0 And Val(txtDiscAmount.Text) = 0 Then
            '    blistErrorMsg.Items.Add("Discount not specified.")
            'End If

            If cboMerchGroup.SelectedIndex = 0 Then
                blistErrorMsg.Items.Add("Mechandise Group not specified.")
            End If

            If cboBusinessUnit.SelectedIndex = 0 Then
                blistErrorMsg.Items.Add("Business Unit not specified.")
            End If

            If cboCompSponsorship.SelectedIndex = 0 Then
                blistErrorMsg.Items.Add("Company to Shoulder entry not specified.")
            End If

            If (cboCompSponsorship.SelectedValue <> 1) And (txtVendorCode.Text = "") Then
                blistErrorMsg.Items.Add("Vendor Code not specified.")
            End If

            'If txtBarcode.Text = "" Then
            '    blistErrorMsg.Items.Add("Barcode not specified.")
            'End If

        End If

        If blistErrorMsg.Items.Count() = 0 Then
            ViewState("proc") = "create"
            clsSession.Icon = "inquiry"

            If hidIsPTFilter.Value = "SBU" Or IsTemplatedPromo(ViewState("RequestID")) Then
                clsSession.Message = "Submit this memo for approval?"
            Else
                clsSession.Message = "Submit this memo for review?"
            End If

            lblPopTitle.Value = "Promotion"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>msgbox('','');</script>")
        End If

    End Sub

    Private Function IsTemplatedPromo(ByVal nRequestID As Long) As Boolean

        Dim strQuery As String
        Dim drPromoType As DataRow = Nothing
        Dim bResult As Boolean = False

        strQuery = "SELECT TOP 1 P.PromoID, T.PromoTypeID, T.IsTemplated " & _
                    "FROM Promotions AS P " & _
                    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                    "WHERE P.RequestID = 0" & nRequestID & " " & _
                    "ORDER BY P.PromoID"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drPromoType) Then
            bResult = (drPromoType("IsTemplated") = 1)
        Else
            ' error
        End If

        Return bResult

    End Function

    Protected Sub lnkEditGuidelines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditGuidelines.Click

        hidBox.Value = litGuidelines.Text
        clsSession.Mechanics = litGuidelines.Text.ToString()
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('" & cmdGuidelines.ClientID & "', 'Promotion Guidelines');</script>")

    End Sub

    Protected Sub lnkInsertGuidelines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkInsertGuidelines.Click

        If cboPromoType.SelectedValue < 0 Then Exit Sub

        sqldsData.SelectCommand = "SELECT * FROM PromoTypes WHERE PromoTypeID = " & cboPromoType.SelectedValue

        Dim dv As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        Dim dr As DataRow = dv.Table.Rows(0)

        litGuidelines.Text &= "<p><b>" & dr("TypeDesc") & "</b><br />" & dr("DefaultGuideline") & "</p>"

    End Sub

    Protected Sub lnkDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDone.Click

        ViewState("proc") = "done"
        clsSession.Icon = "inquiry"
        clsSession.Message = "Discard changes made to this document?"
        lblPopTitle.Value = "Promotion"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>msgbox('','');</script>")

    End Sub

    Protected Sub cmdGuidelines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGuidelines.Click

        litGuidelines.Text = hidBox.Value
        GC.Collect()

    End Sub

    Protected Sub cmdMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMechanics.Click

        ' save new promo mechanics text

        sqldsData.UpdateCommand = "UPDATE Promotions SET PromoDesc = '" & Server.HtmlEncode(Replace(hidBox.Value, "'", "''")) & "' WHERE PromoID = 0" & clsSession.CurrPromoID
        sqldsData.Update()

        gridPromotions.DataBind()

        GC.Collect()

    End Sub

    Protected Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click

        If clsSession.DeleteStatus = "yes" Then

            Select Case ViewState("proc").ToString()
                Case "done"

                    ' redirect to memo view if we are just editing a returned memo draft
                    ' new request string for requestid
                    Response.Redirect("PromoRequest.aspx?RequestID=" & ViewState("RequestID"))

                Case "create"

                    Dim TrailMessage As String
                    Dim taMemos As New dsPromotionsTableAdapters.MemosTableAdapter()
                    Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()

                    Dim NewMemoID As Integer

                    If Request("xmode") = 1 Then

                        ' update existing memo
                        'taMemos.UpdateMemo(Now(), txtPromoTitle.Text.ToUpper(), lblBranches.Text, CDate(txtPeriodFrom.Text), _
                        '                   CDate(txtPeriodTo.Text), Server.HtmlEncode(litGuidelines.Text), SystemUser.UserSignName, _
                        '                   SystemUser.UserSignPosition, CInt(ViewState("OwnerGroup")), "", "For Review", SystemUser.UserID, _
                        '                   ViewState("MemoID")) 'clsSession.CurrMemoID

                        Dim memoId As Integer? = Nothing
                        If ViewState("MemoID") IsNot Nothing AndAlso IsNumeric(ViewState("MemoID")) Then
                            memoId = Convert.ToInt32(ViewState("MemoID"))
                        End If

                        Dim ownerGroup As Integer? = Nothing
                        If ViewState("OwnerGroup") IsNot Nothing AndAlso IsNumeric(ViewState("OwnerGroup")) Then
                            ownerGroup = Convert.ToInt32(ViewState("OwnerGroup"))
                        End If

                        Dim periodFrom As DateTime? = Nothing
                        If Not String.IsNullOrEmpty(txtPeriodFrom.Text) Then
                            Dim tempDate As DateTime
                            If DateTime.TryParse(txtPeriodFrom.Text, tempDate) Then
                                periodFrom = tempDate
                            End If
                        End If

                        Dim periodTo As DateTime? = Nothing
                        If Not String.IsNullOrEmpty(txtPeriodTo.Text) Then
                            Dim tempDate As DateTime
                            If DateTime.TryParse(txtPeriodTo.Text, tempDate) Then
                                periodTo = tempDate
                            End If
                        End If

                        MemoUpdate(memoId, Now(), txtPromoTitle.Text.ToUpper(), lblBranches.Text, periodFrom, _
                                           periodTo, Server.HtmlEncode(litGuidelines.Text), SystemUser.UserSignName, _
                                           SystemUser.UserSignPosition, ownerGroup, "", "For Review", SystemUser.UserID)

                        'taPromoRequests.UpdatePromoRequestDetails(CDate(txtPeriodFrom.Text), CDate(txtPeriodTo.Text), CInt(ViewState("RequestID")))

                        NewMemoID = ViewState("MemoID")
                        TrailMessage = "Updated and re-submitted memo draft."

                    Else

                        ' save entries to new Memo record
                        'NewMemoID = taMemos.AddMemo(Now(), txtPromoTitle.Text.ToUpper(), CDate(txtPeriodFrom.Text), CDate(txtPeriodTo.Text), _
                        '                            lblBranches.Text, Server.HtmlEncode(litGuidelines.Text), ViewState("RequestID").ToString(), _
                        '                            SystemUser.UserSignName, SystemUser.UserSignPosition, "For Review", _
                        '                            CInt(ViewState("OwnerGroup")), SystemUser.UserID)

                        ''taPromoRequests.UpdatePromoRequestDetails(CDate(txtPeriodFrom.Text), CDate(txtPeriodTo.Text), CInt(ViewState("RequestID")))

                        '' update promo references
                        'sqldsPromos.UpdateParameters("MemoID").DefaultValue = NewMemoID
                        'sqldsPromos.Update()


                        NewMemoID = MemoInsert(Now(), txtPromoTitle.Text.ToUpper(), lblBranches.Text, CDate(txtPeriodFrom.Text), CDate(txtPeriodTo.Text), _
                                                     Server.HtmlEncode(litGuidelines.Text), ViewState("RequestID").ToString(), _
                                                    SystemUser.UserSignName, SystemUser.UserSignPosition, CInt(ViewState("OwnerGroup")), _
                                                    "For Review", SystemUser.UserID)


                        sqldsPromos.UpdateParameters("MemoID").DefaultValue = NewMemoID
                        sqldsPromos.Update()

                        TrailMessage = "Created and submitted memo draft."

                    End If

                    ' update promotion specific details if enabled

                    If panSMACdetails.Visible Then

                        txtBarcode.Text = ""

                        ' new request string for requestid
                        sqldsData.UpdateCommand = "UPDATE Promotions SET " & _
                                                  "MerchGroup = '" & cboMerchGroup.SelectedValue & "', " & _
                                                  "BusinessUnit = '" & cboBusinessUnit.SelectedValue & "', " & _
                                                  "CompSponsorship = '" & cboCompSponsorship.SelectedValue & "', " & _
                                                  "VendorCode = '" & txtVendorCode.Text & "', " & _
                                                  "PromoBarcode = '" & txtBarcode.Text & "', " & _
                                                  "PercentDisc = 0" & txtPercentDisc.Text & ", " & _
                                                  "DiscAmount = 0" & txtDiscAmount.Text & " " & _
                                                  "WHERE RequestID = 0" & ViewState("RequestID")

                        sqldsData.Update()

                        AppendExtraDetailsToGuidelines(NewMemoID)

                    End If

                    ' new request string for requestid
                    ' update PromoRequest status
                    sqldsData.UpdateCommand = "UPDATE PromoRequests SET Status = 'For Review' WHERE RequestID = 0" & ViewState("RequestID")
                    sqldsData.Update()

                    ' new request string for requestid
                    ' audit trail
                    clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Transaction), "0#"), ViewState("RequestID"), TrailMessage, SystemUser.UserName, "Promotion Transaction")
                    Response.Redirect("PromoMemoListMain.aspx")

                Case Else

                    ' invalid call

                    ' code must not reach this area
                    ' otherwise, an error has occured

            End Select

        End If

        GC.Collect()

    End Sub

    Protected Sub lnkEditBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditBranch.Click
        'view promo request branch list

        '::ToDo:: Display Promo Request Branch List
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>OpenViewBranches('','');</script>")

    End Sub

    Protected Sub lnkEditPromo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditPromo.Click

        ' get promo details of checked row and copy it to the hidden textbox
        hidBox.Value = ""
        clsSession.CurrPromoID = 0

        For Each row As GridViewRow In gridPromotions.Rows

            Dim cb As CheckBox = row.FindControl("chkRowSel")

            If cb IsNot Nothing AndAlso cb.Checked Then
                Dim lbType As Label = row.FindControl("lblPromoTypeID")

                ' disallow edit of Class Discount and Markdowns

                If (CInt(lbType.Text) = 7) Then

                    ' display error message
                    ' Cannot edit description of Markdown promotions.

                ElseIf (CInt(lbType.Text) = 18) Then

                    ' display error message
                    ' Cannot edit description of Class Discount promotions.

                Else

                    Dim lbID As Label = row.FindControl("lblPromoID")

                    clsSession.CurrPromoID = CInt(lbID.Text)
                    hidBox.Value = row.Cells(2).Text
                End If

                Exit For
            End If
        Next

        If hidBox.Value <> "" Then
            clsSession.Mechanics = hidBox.Value
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('" & cmdMechanics.ClientID & "', 'Promotion Mechanics');</script>")
        End If

    End Sub

    Private Sub MemoUpdate(ByVal MemoId As Integer?, ByVal MemoDate As DateTime?, ByVal Title As String, ByVal Branches As String, ByVal PromoPeriodFrom As DateTime?, ByVal PromoPeriodTo As DateTime?, ByVal Guidelines As String, ByVal PreparedBy As String, ByVal PreparePos As String, ByVal OwnerGroup As Integer?, ByVal Remarks As String, ByVal Status As String, ByVal UserID As Integer?)


        Try
            Using conn As New SqlConnection(ConnStr)
                Using cmd As New SqlCommand("USP_Memo_Update", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    ' Add parameters aligned to the stored procedure
                    cmd.Parameters.AddWithValue("@MemoId", IIf(MemoId.HasValue, MemoId, DBNull.Value))
                    cmd.Parameters.AddWithValue("@MemoDate", IIf(MemoDate.HasValue, MemoDate, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Title", IIf(String.IsNullOrEmpty(Title), DBNull.Value, Title))
                    cmd.Parameters.AddWithValue("@Branches", IIf(String.IsNullOrEmpty(Branches), DBNull.Value, Branches))
                    cmd.Parameters.AddWithValue("@PromoPeriodFrom", IIf(PromoPeriodFrom.HasValue, PromoPeriodFrom, DBNull.Value))
                    cmd.Parameters.AddWithValue("@PromoPeriodTo", IIf(PromoPeriodTo.HasValue, PromoPeriodTo, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Guidelines", IIf(String.IsNullOrEmpty(Guidelines), DBNull.Value, Guidelines))
                    cmd.Parameters.AddWithValue("@PreparedBy", IIf(String.IsNullOrEmpty(PreparedBy), DBNull.Value, PreparedBy))
                    cmd.Parameters.AddWithValue("@PreparePos", IIf(String.IsNullOrEmpty(PreparePos), DBNull.Value, PreparePos))
                    cmd.Parameters.AddWithValue("@OwnerGroup", IIf(OwnerGroup.HasValue, OwnerGroup, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Remarks", IIf(String.IsNullOrEmpty(Remarks), DBNull.Value, Remarks))
                    cmd.Parameters.AddWithValue("@Status", IIf(String.IsNullOrEmpty(Status), DBNull.Value, Status))
                    cmd.Parameters.AddWithValue("@UserID", IIf(UserID.HasValue, UserID, DBNull.Value))

                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' Optional: log or handle errors
            ' lblResult.Text = "Error inserting memo: " & ex.Message
        End Try
    End Sub


    Private Function MemoInsert(ByVal MemoDate As DateTime?, ByVal Title As String, ByVal Branches As String, ByVal PromoPeriodFrom As DateTime?, ByVal PromoPeriodTo As DateTime?, ByVal Guidelines As String, ByVal RequestID As Integer?, ByVal PreparedBy As String, ByVal PreparePos As String, ByVal OwnerGroup As Integer?, ByVal Status As String, ByVal UserID As Integer?) As Integer?

        Dim newMemoID As Integer? = Nothing

        Try
            Using conn As New SqlConnection(ConnStr)
                Using cmd As New SqlCommand("USP_Memo_Insert", conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    ' Add parameters aligned to the stored procedure
                    cmd.Parameters.AddWithValue("@MemoDate", IIf(MemoDate.HasValue, MemoDate, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Title", IIf(String.IsNullOrEmpty(Title), DBNull.Value, Title))
                    cmd.Parameters.AddWithValue("@Branches", IIf(String.IsNullOrEmpty(Branches), DBNull.Value, Branches))
                    cmd.Parameters.AddWithValue("@PromoPeriodFrom", IIf(PromoPeriodFrom.HasValue, PromoPeriodFrom, DBNull.Value))
                    cmd.Parameters.AddWithValue("@PromoPeriodTo", IIf(PromoPeriodTo.HasValue, PromoPeriodTo, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Guidelines", IIf(String.IsNullOrEmpty(Guidelines), DBNull.Value, Guidelines))
                    cmd.Parameters.AddWithValue("@RequestID", IIf(RequestID.HasValue, RequestID, DBNull.Value))
                    cmd.Parameters.AddWithValue("@PreparedBy", IIf(String.IsNullOrEmpty(PreparedBy), DBNull.Value, PreparedBy))
                    cmd.Parameters.AddWithValue("@PreparePos", IIf(String.IsNullOrEmpty(PreparePos), DBNull.Value, PreparePos))
                    cmd.Parameters.AddWithValue("@OwnerGroup", IIf(OwnerGroup.HasValue, OwnerGroup, DBNull.Value))
                    cmd.Parameters.AddWithValue("@Status", IIf(String.IsNullOrEmpty(Status), DBNull.Value, Status))
                    cmd.Parameters.AddWithValue("@UserID", IIf(UserID.HasValue, UserID, DBNull.Value))

                    conn.Open()
                    Dim result As Object = cmd.ExecuteScalar()

                    If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                        newMemoID = Convert.ToInt32(result)
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Optional: log or handle errors
            ' lblResult.Text = "Error inserting memo: " & ex.Message
        End Try
        Return newMemoID
    End Function


    Private Function IsRebateTemplatePromo(ByVal nRequestID As Long) As Boolean

        Dim strQuery As String
        Dim drPromoType As DataRow = Nothing
        Dim bResult As Boolean = False

        strQuery = "SELECT TOP 1 P.PromoID, T.PromoTypeID, T.IsTemplated " & _
                    "FROM Promotions AS P " & _
                    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                    "WHERE P.RequestID = 0" & nRequestID & " " & _
                    "ORDER BY P.PromoID"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drPromoType) Then
            bResult = (drPromoType("PromoTypeID") = 59)
        Else
            ' error
        End If

        Return bResult

    End Function

End Class
