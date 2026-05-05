Imports dsPromotionsTableAdapters
Imports System.Data

Partial Class SelectBranches
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' verify user and page request
        If (SystemUser.UserID = 0) Or (SystemUser.UserLevel <> SystemUser.UserRoles.PromoRequestor) Or (Request("EnvCode") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

        If Not Page.IsPostBack() Then
            LoadStoreGroups()
            Dim IsVstore As Boolean

            'lblCompName.Text = Session("CurrEnvName")
            sqldsBranches.SelectParameters("EnvCode").DefaultValue = Request("EnvCode")

            IsVstore = IsPromoForVisualStore()

            If IsVstore Then
                sqldsBranches.SelectParameters("IsVisualStore").DefaultValue = 1
            Else
                sqldsBranches.SelectParameters("IsVisualStore").DefaultValue = 0
            End If

            sqldsBranches.SelectParameters("StoreGroupID").DefaultValue = "-1"

            Dim MyView As DataView = CType(sqldsBranches.Select(DataSourceSelectArguments.Empty), DataView)

            Dim Strings As String = sqldsBranches.SelectCommand.ToString()

            ' TODO :: error message to indicate a possible data corruption or un-updated table
            If MyView.Count > 0 Then
                lblCompName.Text = MyView(0)("EnvName").ToString()
                If IsVstore Then lblCompName.Text += " - Visual Stores"
            End If

        End If
    End Sub

    Private Function IsPromoForVisualStore() As Boolean

        Dim bResult As Boolean

        sqldsData.SelectCommand = "SELECT * FROM Promotions WHERE PromoTypeID = 48 AND RequestID = " & clsSession.CurrRequestID

        Dim dvTypes As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        bResult = (dvTypes.Table.Rows.Count() > 0)

        Return bResult

        'Dim drType As DataRow = dvTypes.Table.Rows(0)
        'ViewState("v_AllowAttachment") = CBool(drType("AllowAttachment"))
        'ViewState("v_RequireAttachment") = CBool(drType("RequireAttachment"))

    End Function

    Protected Sub lnkDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDone.Click

        Dim taPromoBranches As New dsPromotionsTableAdapters.PromoBranchTableAdapter()
        Dim dtPromoBranches As dsPromotions.PromoBranchDataTable
        Dim DuplicateFound As Boolean = False
        Dim numPromoID As Integer

        For Each row As GridViewRow In gridBranches.Rows

            Dim cb As CheckBox = row.FindControl("chkRowSel")

            If cb IsNot Nothing AndAlso cb.Checked Then

                Dim oShortName As Label = row.FindControl("lblShortName")
                Dim oCompCode As Label = row.FindControl("lblCompCode")
                Dim oBranchCode As Label = row.FindControl("lblBranchCode")

                If clsSession.IsDepartmental = 0 Then
                    ' check for duplicates
                    dtPromoBranches = taPromoBranches.GetPromoBranchByID(clsSession.CurrPromoID, CShort(oCompCode.Text), CShort(oBranchCode.Text))

                    If dtPromoBranches.Rows.Count = 0 Then
                        taPromoBranches.AddPromoBranch(clsSession.CurrPromoID, CShort(oCompCode.Text), CShort(oBranchCode.Text), oShortName.Text)
                    Else
                        DuplicateFound = True ' duplicate detected
                    End If
                Else
                    numPromoID = getTopOnePromoID()
                    dtPromoBranches = taPromoBranches.GetPromoBranchByID(numPromoID, CShort(oCompCode.Text), CShort(oBranchCode.Text))

                    If dtPromoBranches.Rows.Count = 0 Then
                        taPromoBranches.AddPromoBranch(numPromoID, CShort(oCompCode.Text), CShort(oBranchCode.Text), oShortName.Text)
                    Else
                        DuplicateFound = True ' duplicate detected
                    End If
                End If

            

            End If
        Next

        ' inform user of duplicates found in selection
        If DuplicateFound Then
            lblPopTitle.Value = "Promotion Branches"
            clsSession.Message = "Some of the items you've selected are already in the list"
            clsSession.Icon = "fyi"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
        Else
            ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.Branchwindow.hide();</script>")
        End If

    End Sub


    Private Function getTopOnePromoID() As Integer
        Dim dt As New DataTable
        Dim sqlConn As SqlClient.SqlConnection
        sqlConn = New SqlClient.SqlConnection(clsPromo.SQLConnString())
        Try
            Dim sendQuery As String
            sendQuery = "SELECT TOP 1 PromoID from Promotions where RequestID = " & clsSession.CurrRequestID.ToString & _
                        " ORDER BY PromoID DESC"
            Dim sqlCmd As New SqlClient.SqlCommand(sendQuery, sqlConn)
            sqlCmd.CommandType = CommandType.Text
            sqlConn.Open()
            Dim result As Integer = sqlCmd.ExecuteScalar
            sqlConn.Close()
            sqlCmd.Dispose()
            Return result
        Catch ex As Exception
        Finally
            sqlConn.Dispose()
            dt.Dispose()
            GC.Collect()
        End Try

    End Function

    Private Sub LoadStoreGroups()

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT ElementName, ElementValue " & _
                   "FROM ResListValues " & _
                   "WHERE GroupName = 'StoreGroup'"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        ddlStoreGroup.DataSource = dtTable
        ddlStoreGroup.DataTextField = "ElementName"
        ddlStoreGroup.DataValueField = "ElementValue"
        ddlStoreGroup.DataBind()

        ddlStoreGroup.Items.Insert(0, New ListItem("Physical Store", "4"))
        ddlStoreGroup.Items.Insert(0, New ListItem("-- Select All --", "-1"))
    End Sub


    Protected Sub cmdProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProcess.Click

        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.Branchwindow.hide();</script>")

    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkAll As CheckBox = CType(sender, CheckBox)

        For Each row As GridViewRow In gridBranches.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkSel As CheckBox = CType(row.FindControl("chkRowSel"), CheckBox)
                If chkSel IsNot Nothing Then
                    chkSel.Checked = chkAll.Checked
                End If
            End If
        Next

    End Sub


    Protected Sub gridBranches_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridBranches.SelectedIndexChanged

    End Sub

    Protected Sub ddlStoreGroup_SelectedIndexChanged(  ByVal sender As Object, e As EventArgs)
        sqldsBranches.SelectParameters("StoreGroupID").DefaultValue = ddlStoreGroup.SelectedValue()

        gridBranches.DataBind()

    End Sub

End Class
