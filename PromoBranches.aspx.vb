Imports dsPromotionsTableAdapters
Imports System.Data
Imports System.Data.SqlClient

Partial Class PromoBranches
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0) Or (SystemUser.UserLevel > SystemUser.UserRoles.PromoRequestor) Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then

            ' TODO: check source of session parameter 
            If Session("TransFlag") = "natural" Then
                Me.tdPromoDetails.Visible = True
            Else
                Me.tdPromoDetails.Visible = False
            End If

            Dim strSQL As String

            ' get Promotion with most branches for this request if PromoID is not supplied
            ' .:. PromoID not supplied -- page was called from request preview 
            If clsSession.CurrPromoID = 0 Then

                Dim drBrCount As DataRow

                strSQL = "SELECT TOP 1 PromoID, Count(BranchCode) " & _
                          "FROM PromoBranch " & _
                          "WHERE PromoID IN (SELECT PromoID FROM Promotions WHERE RequestID = 0" & clsSession.CurrRequestID & ") " & _
                          "GROUP BY PromoID " & _
                          "ORDER BY Count(BranchCode) DESC"

                If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drBrCount) Then

                    clsSession.CurrPromoID = CInt(drBrCount("PromoID").ToString())

                End If

            End If

            InitEnvironmentList(clsSession.CurrRequestID)

            sqldsPromoBranches.SelectParameters("PromoID").DefaultValue = clsSession.CurrPromoID

            'sqldsPromoBranches.SelectCommand.ToString() pang check lang ng query hehehe rbs7281 

            gridBranches.DataSource = sqldsPromoBranches
            gridBranches.DataBind()
            If gridBranches.Rows.Count = 0 Then
                If gridBranches.HeaderRow IsNot Nothing Then
                    Dim chkHeader As CheckBox = CType(gridBranches.HeaderRow.FindControl("chkALL"), CheckBox)
                    If chkHeader IsNot Nothing Then chkHeader.Visible = False
                End If
            End If


        End If

        If gridBranches.Rows.Count <> 0 Then
            'chkALL.Visible = True
        Else
            'chkALL.Visible = False
        End If

        trFindBranch.Visible = False

    End Sub

    Private Function GetUserBizUnit(ByVal nGroupID As Integer) As String

        Dim drUser As DataRow = Nothing
        Dim strSQL As String
        Dim sBizUnit As String = ""

        strSQL = "SELECT BizUnit " & _
                    "FROM Users AS U " & _
                    "INNER JOIN UserGroups AS G ON G.GroupID = U.GroupID " & _
                    "WHERE BizUnit = 'BCR' " & _
                    "AND U.UserID = 0" & SystemUser.UserID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drUser) Then
            sBizUnit = drUser("BizUnit").ToString()
        End If

        GetUserBizUnit = sBizUnit
    End Function

    Private Sub InitEnvironmentList(ByVal RequestID As Long)
        Dim dtEnvList As New DataTable() ' Initialize the DataTable here
        Dim strSPName As String = "usp_GetEnvironmentList"

        Try
            Using conn As New SqlConnection(clsPromo.SQLConnString)
                Using cmd As New SqlCommand(strSPName, conn)
                    cmd.CommandType = CommandType.StoredProcedure

                    ' Add parameters based on user group type
                    Select Case SystemUser.UserGroupType
                        Case "SBU"
                            cmd.Parameters.AddWithValue("@UserGroupType", "SBU")
                            cmd.Parameters.AddWithValue("@UserID", SystemUser.UserID)
                            cmd.Parameters.AddWithValue("@RequestID", DBNull.Value)
                            tdPromoDetails.Visible = False
                        Case Else
                            cmd.Parameters.AddWithValue("@UserGroupType", "OTHER")
                            cmd.Parameters.AddWithValue("@UserID", SystemUser.UserID)
                            cmd.Parameters.AddWithValue("@RequestID", RequestID)
                    End Select

                    conn.Open()
                    Using adapter As New SqlDataAdapter(cmd)
                        adapter.Fill(dtEnvList)
                    End Using

                    If dtEnvList.Rows.Count > 0 Then
                        cboCompany.DataSource = dtEnvList
                        cboCompany.DataBind()
                        If dtEnvList.Rows.Count = 1 Then cboCompany.SelectedIndex = 1
                    Else
                        ' Handle no results
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Handle exceptions (consider logging the actual error)
            Throw ' Re-throw the exception if you want calling code to handle it
        End Try
    End Sub

    Protected Sub cmdSelectBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectBranch.Click

        If cboCompany.SelectedIndex = 0 Then

            lblPopTitle.Value = "Select Branch"
            clsSession.Message = "Cannot select branches. No company selected.<br/><br/>Please select a company from list."
            clsSession.Icon = "fyi"
            ViewState("process") = "selectbranch"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

        Else
            Session("TransFlag") = "natural"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBranches('" & cboCompany.SelectedValue.ToString() & "','','');</script>")

            'Response.Redirect("SelectBranches.aspx?EnvCode=" & cboCompany.SelectedValue.ToString())
        End If

    End Sub

    Protected Sub lnkRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRequest.Click

        Response.Redirect("PromoRequestEntry.aspx")

    End Sub

    Protected Sub lnkPromoInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoInfo.Click

        Response.Redirect("PromoEntry.aspx")

    End Sub

    Protected Sub lnkPromoDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoDetails.Click

        Response.Redirect("PromoDetailsEntry.aspx")

    End Sub

    Protected Sub lnkPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPreview.Click
        If clsSession.PromoTypeID = 290 Then
            ValidateBranchSeeding()
        Else
            AnalyzeBranch()
        End If
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

    Protected Sub lnkSaveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click

        lnkPreview_Click(sender, e)

    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        If clsSession.DeleteStatus = "yes" Then
            For Each row As GridViewRow In gridBranches.Rows
                Dim cb As CheckBox = row.FindControl("chkRowSel")
                Dim lbCompCode As Label = row.FindControl("lblCompCode")
                Dim lbBranchCode As Label = row.FindControl("lblBranchCode")

                If cb IsNot Nothing AndAlso cb.Checked Then
                    With sqldsPromoBranches.DeleteParameters
                        .Item("RequestID").DefaultValue = clsSession.CurrRequestID
                        .Item("CompCode").DefaultValue = lbCompCode.Text
                        .Item("BranchCode").DefaultValue = lbBranchCode.Text
                    End With
                    sqldsPromoBranches.Delete()
                End If
            Next

            gridBranches.DataSource = sqldsPromoBranches
            gridBranches.DataBind()
            If gridBranches.Rows.Count = 0 Then
                If gridBranches.HeaderRow IsNot Nothing Then
                    Dim chkHeader As CheckBox = CType(gridBranches.HeaderRow.FindControl("chkALL"), CheckBox)
                    If chkHeader IsNot Nothing Then chkHeader.Visible = False
                End If
            End If


            'clsSession.Message = "Deleted Branches: <br>" & _
            '                     "<div style='width:350px; height:130px; overflow:auto; background-color: WhiteSmoke; padding: 10px 10px 10px 10px;'>" & _
            '                     clsSession.Message & "</div><br>"
            'clsSession.Icon = "fyi"
            'lblPopTitle.Value = "Promotions"
            'ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('313','508');</script>")

        End If

        GC.Collect()

    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkHeader As CheckBox = CType(gridBranches.HeaderRow.FindControl("chkALL"), CheckBox)

        If chkHeader Is Nothing Then Exit Sub

        For Each row As GridViewRow In gridBranches.Rows
            Dim chkRow As CheckBox = CType(row.FindControl("chkRowSel"), CheckBox)

            If chkRow IsNot Nothing Then
                chkRow.Checked = chkHeader.Checked
            End If
        Next

    End Sub


    Protected Sub lnkRemoveBranch_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRemoveBranch.Click
        Dim b As Boolean = False

        For Each row As GridViewRow In gridBranches.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing AndAlso cb.Checked Then
                b = True
                Exit For
            End If
        Next

        If b = True Then
            lblPopTitle.Value = "Delete Branch"
            clsSession.Message = "Delete selected branches?"
            clsSession.Icon = "inquiry"
            ViewState("process") = "delete"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        End If
    End Sub

    Protected Sub cmdLoadForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLoadForm.Click
        populateBranches()
    End Sub

    ' Added To display selected promo branches. (Copy code from formloaad_Click)
    Private Sub populateBranches()
        gridBranches.DataSource = sqldsPromoBranches
        gridBranches.DataBind()
        If gridBranches.Rows.Count = 0 Then
            If gridBranches.HeaderRow IsNot Nothing Then
                Dim chkHeader As CheckBox = CType(gridBranches.HeaderRow.FindControl("chkALL"), CheckBox)
                If chkHeader IsNot Nothing Then chkHeader.Visible = False
            End If
        End If

    End Sub

    ' Select All Branches Button
    'Protected Sub cmdAllBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAllBranches.Click

    '    Dim dtPromoBranches As dsPromotions.PromoBranchDataTable
    '    Dim taPromoBranches As New dsPromotionsTableAdapters.PromoBranchTableAdapter()
    '    Dim duplicate As Boolean = False

    '    If cboCompany.SelectedValue = -1 Then Exit Sub

    '    Dim taCompBranches As New dsPromotionsTableAdapters.Branches_Of_CompTableAdapter()
    '    Dim dtCompBranches As dsPromotions.Branches_Of_CompDataTable
    '    Dim trowCompBranch As dsPromotions.Branches_Of_CompRow

    '    dtCompBranches = taCompBranches.GetBranchesOfCompByEnvCode(cboCompany.SelectedValue)

    '    ' add to promo-branches table
    '    For Each trowCompBranch In dtCompBranches.Rows
    '        ' ::ToDo:: check if already in the grid
    '        dtPromoBranches = taPromoBranches.GetPromoBranchByID(clsSession.CurrPromoID, trowCompBranch.CompCode, trowCompBranch.BranchCode)

    '        If dtPromoBranches.Rows.Count = 0 Then
    '            ' add comp-branch to this promo
    '            With sqldsPromoBranches.InsertParameters
    '                .Item("PromoID").DefaultValue = clsSession.CurrPromoID
    '                .Item("CompCode").DefaultValue = trowCompBranch.CompCode
    '                .Item("BranchCode").DefaultValue = trowCompBranch.BranchCode
    '                .Item("ShortName").DefaultValue = Left(trowCompBranch.CompNick.Trim() & "-" & trowCompBranch.BranchNick.Trim(), 50)
    '            End With
    '            sqldsPromoBranches.Insert()
    '        Else
    '            duplicate = True
    '        End If
    '    Next

    '    If duplicate Then
    '        lblPopTitle.Value = "Promotion Branches"
    '        clsSession.Message = "Some of the items you've selected are already in the list"
    '        clsSession.Icon = "fyi"
    '        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
    '    End If

    'End Sub

    Private Sub AnalyzeBranch()
        '::ToDo:: Analyse branch list and update appropriately
        Dim BrDesc As String = SummarizeBranches(clsSession.CurrRequestID).Replace("'", "''")

        Dim strSQL As String = "UPDATE PromoRequests SET Branches = '" & BrDesc & "' WHERE RequestID = " & clsSession.CurrRequestID
        Dim sErr As String = ""

        If clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErr) Then

            Response.Redirect("PromoRequestPreview.aspx")

        Else

            ' error
            ' message: sErr

        End If
    End Sub

    Private Sub ValidateBranchSeeding()
        ' '' 
        Dim strSelectPromoBranch As String
        Dim dtPromoBranch As DataTable = Nothing
        ' '' ---------------------------------------------------------------------------------------------
        strSelectPromoBranch = "SELECT PB.ShortName " & _
                    " FROM PromoBranch PB WITH(NOLOCK)" & _
                    " INNER JOIN Promotions P WITH(NOLOCK) " & _
                    " ON PB.PromoID = P.PromoID " & _
                    " LEFT JOIN PromoSeed PS " & _
                    "ON SUBSTRING(PS.CompBranch,4,4) = RIGHT('0000'+ISNULL(CAST(PB.BranchCode AS nvarchar), ''),4) " & _
                    "   AND P.RequestID = PS.RequestID " & _
                    "WHERE PB.PromoID = " & clsSession.CurrPromoID & _
                    "   AND PS.CompBranch IS NULL " & _
                    "ORDER BY PB.ShortName "

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strSelectPromoBranch, dtPromoBranch) Then
            If dtPromoBranch.Rows.Count() > 0 Then
                lblPopTitle.Value = "Promotion Branches"
                clsSession.Message = "Missing seeding files for "
                clsSession.Icon = "fyi"

                For Each drRow As DataRow In dtPromoBranch.Rows
                    clsSession.Message = clsSession.Message & drRow("ShortName").ToString & ", "
                Next
                clsSession.Message = clsSession.Message.Substring(0, clsSession.Message.LastIndexOf(",")) & "."
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openMissingSeeding('','');</script>")
            Else
                AnalyzeBranch()
            End If
        End If
    End Sub

    Protected Sub cmdMissingSeeding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMissingSeeding.Click
        If clsSession.DeleteStatus = "yes" Then
            AnalyzeBranch()
        End If
    End Sub

End Class
