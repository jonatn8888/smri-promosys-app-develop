Imports dsPromotionsTableAdapters
Imports System.Data
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class PromoBranchesDepartmental
    Inherits System.Web.UI.Page

    Dim nPromoID As Integer

    Protected Sub cmdAllBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAllBranches.Click
        Dim dtPromoBranches As dsPromotions.PromoBranchDataTable
        Dim taPromoBranches As New dsPromotionsTableAdapters.PromoBranchTableAdapter()
        Dim duplicate As Boolean = False

        If cboCompany.SelectedIndex = 0 Then
            lblPopTitle.Value = "Add All Branches"
            clsSession.Message = "Cannot add all branches. No company selected.<br/><br/>Please select a company from list."
            clsSession.Icon = "fyi"
            ViewState("process") = "addbranch"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        Else
            ' no selection made
            If cboCompany.SelectedValue = -1 Then Exit Sub

            Dim taCompBranches As New dsPromotionsTableAdapters.Branches_Of_CompTableAdapter()
            Dim dtCompBranches As dsPromotions.Branches_Of_CompDataTable
            Dim trowCompBranch As dsPromotions.Branches_Of_CompRow

            dtCompBranches = taCompBranches.GetBranchesOfCompByEnvCode(cboCompany.SelectedValue)

            Dim dt As DataTable
            dt = FillPromoIDDT()
            Dim i As Integer 'dt.Rows(i)(0)

            For i = 0 To dt.Rows.Count - 1

                For Each trowCompBranch In dtCompBranches.Rows

                    ' ::ToDo:: check if already in the grid
                    dtPromoBranches = taPromoBranches.GetPromoBranchByID(dt.Rows(i)(0), trowCompBranch.CompCode, trowCompBranch.BranchCode)

                    'maurice
                    If dtPromoBranches.Rows.Count = 0 Then
                        ' add comp-branch to this promo
                        With sqldsPromoBranches.InsertParameters
                            .Item("PromoID").DefaultValue = dt.Rows(i)(0)
                            .Item("CompCode").DefaultValue = trowCompBranch.CompCode
                            .Item("BranchCode").DefaultValue = trowCompBranch.BranchCode
                            .Item("ShortName").DefaultValue = Left(trowCompBranch.CompNick.Trim() & "-" & trowCompBranch.BranchNick.Trim(), 50)
                        End With

                        sqldsPromoBranches.Insert()
                    Else

                        duplicate = True
                    End If
                Next
            Next

            If duplicate Then
                lblPopTitle.Value = "Promotion Branches"
                clsSession.Message = "Some of the items you've selected are already in the list"
                clsSession.Icon = "fyi"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
            End If

            'check if selected company is SM Dept. Stores
            'If cboCompany.SelectedValue = 0 Then
            '    'ViewState("SelBranchDesc") = "All SM Dept. Store Branches"
            'Else
            '    'dtBranches = taBranches.GetBranchByCode(cboCompany.SelectedValue, 0)
            '    'trowBranch = dtBranches.Rows(0)
            '    'ViewState("SelBranchDesc") = "All Branches - " & trowBranch.LongName.ToString()
            'End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0) Or (SystemUser.UserLevel <> SystemUser.UserRoles.PromoRequestor) Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then
            If Session("TransFlag") = "natural" Then
                Me.tdPromoDetails.Visible = True
            Else
                Me.tdPromoDetails.Visible = False
            End If
            'sqldsEnvironments.SelectCommand = "SELECT * FROM Environments WHERE IsHidden = 0 AND EnvCode = "

            sqldsEnvironments.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
            sqldsPromoBranches.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID

            populateBranches()

        End If

        trFindBranch.Visible = False

    End Sub

    Private Function FillPromoIDDT() As DataTable
        Dim StoredProc As String

        StoredProc = "SELECT B.PromoID" & _
                     " FROM Promotions B " & _
                     "WHERE B.RequestID = '" & clsSession.CurrRequestID & "'"

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())

        Dim sqlCmd As New SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.Text

        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_PromoID")

        Dim dt As DataTable = ds.Tables("tbl_PromoID")
        Return dt

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        da = Nothing
        ds = Nothing

        GC.Collect()
    End Function

    Protected Sub cmdSelectBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectBranch.Click

        If cboCompany.SelectedValue = -1 Then
            lblPopTitle.Value = "Select Branch"
            clsSession.Message = "Cannot select branches. No company selected.<br/><br/>Please select a company from list."
            clsSession.Icon = "fyi"
            ViewState("process") = "selectbranch"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        Else
            Session("TransFlag") = "departmental"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBranches('" & cboCompany.SelectedValue.ToString() & "','','');</script>")
            'Response.Redirect("SelectBranches.aspx?EnvCode=" & cboCompany.SelectedValue.ToString())
        End If

    End Sub

    Protected Sub lnkRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRequest.Click
        clsSession.MsgTransFlowFlag = 3
        Response.Redirect("PromoRequestEntry.aspx")

    End Sub

    Protected Sub lnkPromoInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoInfo.Click

        Response.Redirect("PromoDetailsEntryDepartmental.aspx")

    End Sub

    Protected Sub lnkPromoDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPromoDetails.Click

        Response.Redirect("PromoDetailsEntry.aspx")

    End Sub

    Protected Sub lnkDeleteSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDeleteSelected.Click

        ' ::ToDo:: confirmation script
        ' ::ToDo:: confirmation script
        ' ::maurice::
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



    Protected Sub lnkPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPreview.Click

        '::ToDo:: Analyse branch list and update appropriately

        Dim BrDesc As String = SummarizeBranches(clsSession.CurrRequestID)

        sqldsData.UpdateCommand = "UPDATE PromoRequests SET Branches = '" & BrDesc & "' WHERE RequestID = " & clsSession.CurrRequestID
        sqldsData.Update()

        '::ToDO:: Check why use RequestID parameter
        Response.Redirect("PromoRequestPreview.aspx?RequestID=" & clsSession.CurrRequestID)

    End Sub

    ' Revised dowcarpio10032012@smretailinc: Include InActive tag in the conditions
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


    'Private Function SummarizeBranches(ByVal PromoID As Integer) As String
    '    'Dim SelBranchDesc As String = ""
    '    'Dim CurrComp As Integer = 0

    '    'Dim taBranches As New dsPromotionsTableAdapters.BranchesTableAdapter()
    '    'Dim dtBranches As dsPromotions.BranchesDataTable
    '    'Dim trowBranch As dsPromotions.BranchesRow

    '    ''::ToDO:: parse branches on all promos in request
    '    '' combine short descriptions of branches
    '    'sqldsData.SelectCommand = "SELECT * FROM PromoBranch WHERE PromoID = " & PromoID & " ORDER BY CompCode, ShortName"

    '    'Dim dvBranch As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

    '    'For Each dr As DataRow In dvBranch.Table.Rows

    '    '    If CurrComp <> dr("CompCode") Then

    '    '        If SelBranchDesc <> "" Then SelBranchDesc &= "<br>"

    '    '        dtBranches = taBranches.GetBranchByCode(dr("CompCode"))
    '    '        trowBranch = dtBranches.Rows(0)

    '    '        SelBranchDesc &= trowBranch.ShortName.ToString() & " - " & dr("ShortName")

    '    '        CurrComp = dr("CompCode")
    '    '    Else

    '    '        SelBranchDesc &= ", " & dr("ShortName")
    '    '    End If
    '    'Next

    '    ''If Len(SelBranchDesc) > 500 Then SelBranchDesc = "Various Branches"

    '    SummarizeBranches = "" ' SelBranchDesc
    'End Function

    Protected Sub lnkSaveMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSaveMemo.Click
        lnkPreview_Click(sender, e)
    End Sub


    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        If clsSession.DeleteStatus = "yes" Then
            For Each row As GridViewRow In gridBranches.Rows
                Dim cb As CheckBox = row.FindControl("chkRowSel")
                If cb IsNot Nothing AndAlso cb.Checked Then
                    'clsSession.Message = clsSession.Message & clsSession.CurrPromoID & "-" & row.Cells(1).Text & "-" & row.Cells(2).Text & "-" & row.Cells(3).Text & "<br/>"
                    With sqldsPromoBranches.DeleteParameters
                        ' Revised dowcarpio08242012@smretailinc: cannot retrieve value in row. Added hidden field to used in delete parameters  
                        .Item("RequestID").DefaultValue = clsSession.CurrRequestID ' Added dowcarpio08242012@smretailinc: Include RequestID in the delete condition.
                        .Item("CompCode").DefaultValue = CType(row.FindControl("hfCompCode"), HiddenField).Value 'row.Cells(1).Text
                        .Item("BranchCode").DefaultValue = CType(row.FindControl("hfBranchCode"), HiddenField).Value 'row.Cells(2).Text
                    End With
                    ' Revised dowcarpio08242012@smretailinc: Include RequestID parameter to delete.
                    sqldsPromoBranches.Delete()
                End If
            Next
            'clsSession.Message = "Deleted Branches: <br>" & _
            '                                "<div style='width:350px; height:130px; overflow:auto; background-color: WhiteSmoke; padding: 10px 10px 10px 10px;'>" & _
            '                                clsSession.Message & "</div><br>"
            'clsSession.Icon = "fyi"
            'lblPopTitle.Value = "Promotions"
            'ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('313','508');</script>")

            populateBranches()

        End If
        GC.Collect()
    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkALL.CheckedChanged
        'Dim chk As CheckBox
        'chk = CType(gridBranches.HeaderRow.FindControl("chkALL"), CheckBox)
        If chkALL.Checked = True Then
            Dim row As GridViewRow
            For Each row In gridBranches.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In gridBranches.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub formloaad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles formloaad.Click
        gridBranches.DataSource = sqldsPromoBranches
        gridBranches.DataBind()
    End Sub

    ' Added dowcarpio08242012@smretailinc:To display selected promo branches. (Copy code from formloaad_Click)
    Private Sub populateBranches()
        gridBranches.DataSource = sqldsPromoBranches
        gridBranches.DataBind()
    End Sub
End Class
