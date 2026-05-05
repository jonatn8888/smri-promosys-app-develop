Imports System.Data
Imports System.Data.SqlClient

Partial Class MallSaleCreatePromo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack() Then

            If Session("CurrRequestID") = 0 Then
                lblReqDate.Text = Format(Now(), "MMMM dd, yyyy")
            Else
                'get Request information

            End If

            FillUserGroupList()
            FillEnvironmentList()

            InitializeBusinessUnitGrid()
            DisplayCompBranchGrid()
        End If

    End Sub

    Private Sub InitializeBusinessUnitGrid()

        Dim dtTable As New DataTable

        dtTable.Columns.Add("GroupID", GetType(Integer))
        dtTable.Columns.Add("Description", GetType(String))

        DisplayBizUnitGrid(dtTable)

        'Store the DataTable in ViewState
        ViewState("BusinessUnitTable") = dtTable

    End Sub

    Private Sub DisplayCompBranchGrid()

        Dim dtTable As New DataTable

        'dtTable.Columns.Add("CompCode", GetType(Integer))
        'dtTable.Columns.Add("BranchCode", GetType(Integer))
        'dtTable.Columns.Add("BranchName", GetType(String))

        ' display branches of environment
        Dim strQuery As String

        strQuery = "SELECT C.CompCode, B.BranchCode, B.BranchName " & _
                    "FROM CompBranches AS CB " & _
                    "INNER JOIN Companies AS C ON C.CompCode = CB.CompCode " & _
                    "INNER JOIN Branches AS B ON B.BranchCode = CB.BranchCode " & _
                    "WHERE EnvCode = 0" & cboEnvironment.SelectedValue

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        gridBranches.DataSource = dtTable
        gridBranches.DataBind()

        ''Store the DataTable in ViewState
        'ViewState("CompBranchTable") = dtTable

    End Sub

    Private Sub FillEnvironmentList()

        Dim dtTable As New DataTable
        Dim strQuery As String

        cboEnvironment.Items.Clear()

        Dim sBizUnitList As String = ""

        For Each row As GridViewRow In gridBusinessUnit.Rows
            If sBizUnitList = "" Then
                sBizUnitList = row.Cells(1).Text
            Else
                sBizUnitList &= ", " & row.Cells(1).Text
            End If
        Next

        strQuery = "SELECT EnvCode, ShortDesc+' - '+EnvName AS EnvDescription FROM Environments " & _
                   "WHERE IsHidden = 0 AND EnvCode IN " & _
                   "(SELECT DISTINCT EnvCode FROM DepSdepClass WHERE DeptCode IN " & _
                   "(SELECT DeptCode FROM UserGroups WHERE BizUnit IN " & _
                   "(SELECT BizUnit FROM UserGroups WHERE GroupID IN (" & sBizUnitList & ")))) " & _
                   "ORDER BY ShortDesc"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboEnvironment.DataSource = dtTable
        cboEnvironment.DataBind()

    End Sub

    Private Sub FillUserGroupList()

        Dim dtTable As New DataTable
        Dim strQuery As String

        strQuery = "SELECT GroupID, BizUnit+' - '+Description AS FullDesc " & _
                    "FROM UserGroups " & _
                    "WHERE BizUnit IS NOT NULL AND Department IS NULL AND GroupType = 'REGULAR' " & _
                    "ORDER BY BizUnit"


        'cboUserGroups.Items.Clear()

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboUserGroups.DataSource = dtTable
        cboUserGroups.DataBind()

    End Sub

    'Protected Sub lnkPromoInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoInfo.Click

    '    ''::ToDo:: client-side validation for blank entries

    '    'Dim nRushLeadDays As Integer = 3
    '    'Dim ctr As Integer = 0

    '    'blistErrorMsg.Items.Clear()

    '    ''validate entries
    '    'If txtReqTitle.Text = "" Then
    '    '    blistErrorMsg.Items.Add("Promo title not indicated.")
    '    'End If

    '    'If Not IsDate(txtPeriodFrom.Text) Then

    '    '    blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format
    '    '    ctr += 1

    '    '    'ElseIf DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < 0 Then

    '    '    '    blistErrorMsg.Items.Add("Request must be at least a " & nRushLeadDays & " working days before the Promo Period.")

    '    'Else

    '    '    ' add one more day if later than 6pm
    '    '    If Now.Hour > 17 Then
    '    '        nRushLeadDays += 1
    '    '    End If

    '    '    If DateDiff(DateInterval.Day, Today(), CDate(txtPeriodFrom.Text)) < nRushLeadDays Then
    '    '        blistErrorMsg.Items.Add("Request must be at least " & nRushLeadDays & " working days before the Promo Period.")
    '    '    End If
    '    'End If

    '    'If Not IsDate(txtPeriodTo.Text) Then

    '    '    blistErrorMsg.Items.Add("Blank or invalid promo end date format.") ' invalid date format
    '    '    ctr += 1
    '    'Else
    '    '    If CDate(txtPeriodTo.Text) < CDate(txtPeriodFrom.Text) Then
    '    '        blistErrorMsg.Items.Add("End of promo must not be earlier than the start date.")
    '    '    End If
    '    'End If

    '    '' Added dowcarpio08212012@smretailinc: show error message when start/end date is/are null.
    '    'If ctr = 0 Then

    '    '    ' disallow requests that are more than 3 months ahead .: 8/25/2010 :.
    '    '    If CDate(txtPeriodFrom.Text) > DateAdd(DateInterval.Month, 3, Today()) Then
    '    '        blistErrorMsg.Items.Add("Requests that are more than 3 months ahead are not allowed.")
    '    '    End If

    '    'End If

    '    '' display error message and skip other commands
    '    'If blistErrorMsg.Items.Count > 0 Then Exit Sub

    '    '' RESTORE THIS SECTION!
    '    '' warn user if date of request is less than the 10-day lead time
    '    ''If CDate(txtPeriodFrom.Text) <= DateAdd(DateInterval.Day, 10, Today()) Then
    '    ''    ' show prompt
    '    ''    lblPopTitle.Value = "Warning"
    '    ''    clsSession.Message = "Date of promo effectivity does not comply with the 10-day processing period.<br />" & _
    '    ''                         "<b>Request will be forwarded to VP-MPD upon approval of MBU head.</b><br><br>" & _
    '    ''                         "Do you still wish to continue?"
    '    ''    clsSession.Icon = "warning"
    '    ''    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>msgbox('218','');</script>")

    '    ''Else

    '    '' save entries
    '    'If SaveTransEntries() Then
    '    '    Response.Redirect("PromoEntry.aspx")
    '    'End If

    '    ''End If  -- restore this line

    'End Sub

    Protected Sub lnkSaveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click

        If ValidateEntries() Then

            If SaveTransactionEntries() Then
                Response.Redirect("PromoMemoListMPA.aspx")
            End If

        End If

    End Sub

    Private Function ValidateEntries() As Boolean

        ' clear error message field
        blistErrorMsg.Items.Clear()

        '
        ' validate entries
        '

        If txtReqTitle.Text = "" Then
            blistErrorMsg.Items.Add("Promo title not indicated.")
        End If

        If Not IsDate(txtPeriodFrom.Text) Then
            blistErrorMsg.Items.Add("Blank or invalid promo start date format.") ' invalid date format
        End If

        If Not IsDate(txtPeriodTo.Text) Then
            blistErrorMsg.Items.Add("Blank or invalid promo end date format.") ' invalid date format
        End If

        If gridBusinessUnit.Rows.Count < 1 Then
            blistErrorMsg.Items.Add("No participating Business Unit specified.")
        End If

        If gridBranches.Rows.Count < 1 Then
            blistErrorMsg.Items.Add("No participating branch specified.")
        End If

        ValidateEntries = (blistErrorMsg.Items.Count = 0)

    End Function

    Private Function SaveTransactionEntries() As Boolean

        Dim dtTable As New DataTable
        Dim sErrMess As String = ""
        Dim strQuery As String

        Dim sBizUnitList As String = ""

        For Each row As GridViewRow In gridBusinessUnit.Rows
            If sBizUnitList = "" Then
                sBizUnitList = row.Cells(1).Text
            Else
                sBizUnitList &= ", " & row.Cells(1).Text
            End If
        Next

        ' create request
        strQuery = "INSERT INTO PromoRequests " & _
                    "(RequestDate, Title, PromoPeriodFrom, PromoPeriodTo, Branches, RequestedBy, RequesterPos, ApprovedBy, ApproverPos, ApproveDate, OwnerGroup, Remarks, Status, UserID) " & _
                    "VALUES " & _
                    "(GETDATE(), '" & txtReqTitle.Text.ToUpper & "', '" & txtPeriodFrom.Text & "', '" & txtPeriodTo.Text & "', '', 'AUTO', 'AUTO', 'AUTO', 'AUTO', GETDATE(), 99, '', 'For MPD Processing', 0" & SystemUser.UserID & ") " & _
                    "SELECT SCOPE_IDENTITY();"

        Dim nCurrRequestID As Integer
        nCurrRequestID = clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery)

        ' insert promotions
        strQuery = "INSERT INTO Promotions " & _
                    "(PromoDesc, PromoTypeID, PeriodFrom, PeriodTo, PercentDisc, DiscAmount, RequestID, MemoID, Status, Remarks) " & _
                    "SELECT PromoDescription, PromoTypeID, '" & txtPeriodFrom.Text & "', '" & txtPeriodTo.Text & "', PercentDisc, 0, 0" & nCurrRequestID & ", NULL, '', E.PEPnumber " & _
                    "FROM dbo.EventParticipation AS E " & _
                    "INNER JOIN EventPartDetails AS D ON D.PEPnumber = E.PEPnumber " & _
                    "WHERE E.DeptCode IN (SELECT DeptCode FROM UserGroups " & _
                                           "WHERE BizUnit IN (SELECT BizUnit FROM UserGroups WHERE GroupID IN (" & sBizUnitList & ") " & _
                                         ") AND DeptCode IS NOT NULL) " & _
                    "ORDER BY DeptCode, SubDeptCode, ClassCode, SubClassCode"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
            blistErrorMsg.Items.Add("Error in creating promotions.")
            SaveTransactionEntries = False
            Exit Function
        End If

        ' insert details
        strQuery = "INSERT INTO PromoDetails " & _
                    "(PromoID, DepCode, SubDepCode, ClassCode, ShortDesc, PercentDisc, PeriodFrom, PeriodTo, SubClassCode) " & _
                    "SELECT P.PromoID, DeptCode, SubDeptCode, ClassCode, LEFT(REPLACE(ItemDescription, ' ', ''),8), D.PercentDisc, '" & txtPeriodFrom.Text & "', '" & txtPeriodTo.Text & "', SubClassCode " & _
                    "FROM dbo.EventParticipation AS E " & _
                    "INNER JOIN EventPartDetails AS D ON D.PEPnumber = E.PEPnumber " & _
                    "INNER JOIN Promotions AS P ON P.Remarks = E.PEPnumber AND P.PromoTypeID = D.PromoTypeID " & _
                    "WHERE E.DeptCode IN (SELECT DeptCode FROM UserGroups " & _
                                           "WHERE BizUnit IN (SELECT BizUnit FROM UserGroups WHERE GroupID IN (" & sBizUnitList & ") " & _
                                         ") AND DeptCode IS NOT NULL) " & _
                    "AND P.Remarks IS NOT NULL " & _
                    "ORDER BY P.PromoID, DeptCode, SubDeptCode, ClassCode, SubClassCode"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
            blistErrorMsg.Items.Add("Error in creating promotion details.")
            SaveTransactionEntries = False
            Exit Function
        End If

        ' remove remarks tagging
        strQuery = "UPDATE Promotions SET Remarks = NULL WHERE Remarks IS NOT NULL"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
            blistErrorMsg.Items.Add("Error in clearing remarks tag.")
            SaveTransactionEntries = False
            Exit Function
        End If

        '
        ' insert branches
        '
        Dim nCompCode As Integer
        Dim nBranchCode As Integer

        For Each row As GridViewRow In gridBranches.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing And cb.Checked Then

                nCompCode = row.Cells(1).Text
                nBranchCode = row.Cells(2).Text

                ' insert branches
                strQuery = "INSERT INTO PromoBranch " & _
                            "(PromoID, CompCode, BranchCode, ShortName) " & _
                            "SELECT PromoID, 0" & nCompCode & ", BranchCode, ShortName " & _
                            "FROM Promotions " & _
                            "INNER JOIN Branches ON BranchCode = 0" & nBranchCode & _
                            "WHERE RequestID = 0" & nCurrRequestID

                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
                    blistErrorMsg.Items.Add("Error in inserting branches.")
                    SaveTransactionEntries = False
                    Exit Function
                End If

            End If
        Next

        ' update summary of branches

        strQuery = "UPDATE PromoRequests " & _
                    "SET Branches = '" & SummarizeBranches(nCurrRequestID) & "' " & _
                    "WHERE RequestID = 0" & nCurrRequestID

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, sErrMess) Then
            blistErrorMsg.Items.Add("Error updating branch summary.")
            SaveTransactionEntries = False
            Exit Function
        End If

        SaveTransactionEntries = True

    End Function

    Protected Sub cmdAddBizUnit_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddBizUnit.ServerClick

        AddBusinessUnit(cboUserGroups.SelectedValue, cboUserGroups.SelectedItem.ToString)

        FillEnvironmentList()

    End Sub

    Private Sub RemoveBusinessUnit(ByVal sUnitList As String)

        Dim dtTable As DataTable

        If sUnitList <> "" Then
            dtTable = ViewState("BusinessUnitTable")

            Dim drRows = dtTable.Select("GroupID IN (" & sUnitList & ")")

            For Each row As DataRow In drRows
                row.Delete()
            Next

            dtTable.AcceptChanges()

            DisplayBizUnitGrid(dtTable)

            lnkDeleteBizUnit.Visible = (gridBusinessUnit.Rows.Count > 0)
        End If

    End Sub

    Private Sub AddBusinessUnit(ByVal GroupID As Integer, ByVal Description As String)

        Dim dtTable As DataTable

        dtTable = ViewState("BusinessUnitTable")

        Dim drRows As DataRow() = dtTable.Select("GroupID = " & GroupID.ToString)

        If drRows.Length = 0 Then
            dtTable.Rows.Add(GroupID, Description)

            DisplayBizUnitGrid(dtTable)

            lnkDeleteBizUnit.Visible = True
        End If

    End Sub

    Private Sub DisplayBizUnitGrid(ByVal dtBizUnitTable As DataTable)

        Dim dvView As New DataView(dtBizUnitTable)

        dvView.Sort = "Description"

        gridBusinessUnit.DataSource = dvView.ToTable()
        gridBusinessUnit.DataBind()

    End Sub

    Protected Sub cmdSelectBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectBranch.Click

        DisplayCompBranchGrid()

    End Sub

    Protected Sub chkSelectAllBranchRows_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        ' select all

    End Sub

    Private Function SummarizeBranches(ByVal RequestID As Integer) As String

        Dim BrSummDesc As String = ""

        ' get number of branches in promotion by environment
        sqldsData.SelectCommand = "SELECT E.EnvCode, E.ShortDesc AS EnvNick, Count(*) AS NumBranches FROM PromoBranch AS PB " & _
                                  "LEFT JOIN Companies AS C ON C.CompCode = PB.CompCode " & _
                                  "LEFT JOIN Environments AS E ON E.EnvCode = C.EnvCode " & _
                                  "WHERE C.IsHidden = 0 And E.IsHidden = 0 " & _
                                  "AND PB.PromoID = (SELECT TOP 1 PromoID FROM Promotions WHERE RequestID = 0" & RequestID & ") " & _
                                  "GROUP BY E.EnvCode, E.ShortDesc"

        Dim dvPromoEnvs As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        For Each drPromoEnv As DataRow In dvPromoEnvs.Table.Rows

            ' get actual number of branches per environment
            sqldsData.SelectCommand = "SELECT CB.CompCode, CB.BranchCode " & _
                                      "FROM CompBranches AS CB " & _
                                      "LEFT JOIN Companies AS C ON CB.CompCode = C.CompCode " & _
                                      "WHERE C.IsHidden = 0 AND CB.InActive = 0 AND C.EnvCode = 0" & drPromoEnv("EnvCode")

            Dim dvEnvBranches As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

            ' NBSantos: add condition to single branch display
            ' compare number of all branches per environment to that of the promo
            If (dvEnvBranches.Table.Rows.Count() > 3) And (CInt(drPromoEnv("NumBranches")) = dvEnvBranches.Table.Rows.Count()) Then

                If BrSummDesc <> "" Then BrSummDesc &= "<br />"

                BrSummDesc &= drPromoEnv("EnvNick") & " - All Branches"

            ElseIf dvEnvBranches.Table.Rows.Count() > 10 And (dvEnvBranches.Table.Rows.Count() - CInt(drPromoEnv("NumBranches"))) < 8 Then

                ' get list of branches that are not included in the promotion
                sqldsData.SelectCommand = "SELECT CB.CompCode, CB.BranchCode, B.ShortName " & _
                                          "FROM CompBranches AS CB " & _
                                          "LEFT JOIN Companies AS C ON C.CompCode = CB.CompCode " & _
                                          "LEFT JOIN Branches AS B ON B.BranchCode = CB.BranchCode " & _
                                          "WHERE C.IsHidden = 0 And B.IsHidden = 0 AND CB.InActive = 0 AND C.EnvCode = 0" & drPromoEnv("EnvCode") & " " & _
                                          "AND Cast(CB.CompCode AS varchar(5))+'-'+Cast(CB.BranchCode AS varchar(5)) NOT IN (" & _
                                                  "SELECT Cast(PB.CompCode AS varchar(5))+'-'+Cast(PB.BranchCode AS varchar(5)) " & _
                                                  "FROM PromoBranch AS PB LEFT JOIN Companies AS C ON C.CompCode = PB.CompCode " & _
                                                  "WHERE C.IsHidden = 0 And C.EnvCode = 0" & drPromoEnv("EnvCode") & " " & _
                                                  "AND PB.PromoID = (SELECT TOP 1 PromoID FROM Promotions WHERE RequestID = 0" & RequestID & ")) " & _
                                          "ORDER BY B.ShortName"

                Dim dvExcludedBranches As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
                Dim ExcludedList As String = ""

                ' get short names of excluded branches
                For Each drExBranch As DataRow In dvExcludedBranches.Table.Rows
                    If ExcludedList <> "" Then ExcludedList &= ", "
                    ExcludedList &= drExBranch("ShortName")
                Next

                If BrSummDesc <> "" Then BrSummDesc &= "<br />"

                If ExcludedList <> "" Then ExcludedList = " EXCEPT " & ExcludedList

                BrSummDesc &= drPromoEnv("EnvNick") & " - All Branches" & ExcludedList

            Else
                ' get branches from a Promotion of this request
                ' (assumes that all promotions under this request has identical branches)
                sqldsData.SelectCommand = "SELECT B.ShortName AS BrNick, C.CompCode, B.BranchCode, C.EnvCode FROM PromoBranch AS PB " & _
                                          "LEFT JOIN Branches AS B ON B.BranchCode = PB.BranchCode " & _
                                          "LEFT JOIN Companies AS C ON C.CompCode = PB.CompCode " & _
                                          "WHERE B.IsHidden = 0 AND C.IsHidden = 0 AND C.EnvCode = 0" & drPromoEnv("EnvCode") & " " & _
                                          "AND PB.PromoID = (SELECT TOP 1 PromoID FROM Promotions WHERE RequestID = 0" & RequestID & ") " & _
                                          "ORDER BY B.ShortName"

                Dim dvBranches As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
                Dim CurrEnv As String = ""

                ' combine short descriptions of branches per environment

                For Each dr As DataRow In dvBranches.Table.Rows

                    ' next environment
                    If CurrEnv <> drPromoEnv("EnvNick") Then

                        If BrSummDesc <> "" Then BrSummDesc &= "<br />"

                        BrSummDesc &= drPromoEnv("EnvNick") & " - " & dr("BrNick")

                        CurrEnv = drPromoEnv("EnvNick")
                    Else

                        BrSummDesc &= ", " & dr("BrNick")
                    End If
                Next


            End If
        Next

        ' check if string exceeds field length
        If Len(BrSummDesc) > 500 Then BrSummDesc = Left(BrSummDesc, 495) & "..."

        SummarizeBranches = BrSummDesc

    End Function

    Protected Sub lnkDeleteBizUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteBizUnit.Click

        Dim b As Boolean = False

        ' scan for checked entries
        For Each row As GridViewRow In gridBusinessUnit.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Remove Business Units"
            clsSession.Message = "Remove selected business unit(s) from the list?"
            clsSession.Icon = "inquiry"
            ViewState("process") = "RemoveBizUnits"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>java_msgbox('','');</script>")
        End If

    End Sub

    Protected Sub btnProcess_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.ServerClick

        Select Case ViewState("process")

            Case "RemoveBizUnits"

                If clsSession.DeleteStatus = "yes" Then

                    Dim sCheckedRows As String = ""

                    ' get checked entries
                    For Each row As GridViewRow In gridBusinessUnit.Rows
                        Dim cb As CheckBox = row.FindControl("chkRowSel")
                        If cb IsNot Nothing And cb.Checked Then

                            If sCheckedRows = "" Then
                                sCheckedRows = row.Cells(1).Text
                            Else
                                sCheckedRows = sCheckedRows & "," & row.Cells(1).Text
                            End If

                        End If
                    Next

                    RemoveBusinessUnit(sCheckedRows)

                End If

        End Select

    End Sub

End Class
