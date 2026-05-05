Imports dsPromotionsTableAdapters
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Linq


Partial Class SelectUserBranches
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' verify user and page request
        If (SystemUser.UserID = 0) Or (Request("Params") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

        If Not Page.IsPostBack() Then

            Dim IsVstore As Boolean

            'lblCompName.Text = Session("CurrEnvName")
            Dim test As String
            test = Request("Params").ToString()


            Dim arr() = test.Split("|")

            Dim CompCode As String
            CompCode = arr(0)

            Dim UserID As String
            UserID = arr(1)

            Dim Mode_ As String
            Mode_ = arr(2)

            On Error GoTo Impyerno

            Dim SelectedBranch_ As String
            SelectedBranch_ = arr(3)


            Dim SelectedBranch As String = String.Empty
            If arr(4) IsNot Nothing Then
                If Session("SelectedBranches") IsNot Nothing Then
                    SelectedBranch = arr(4)
                End If
            End If

Impyerno:

            If SelectedBranch <> "" Then
                If SelectedBranch IsNot Nothing Then
                    BindGrid(SelectedBranch, Mode_, CompCode, UserID)
                End If
            Else
                Session("SelectedBranches") = CompCode & "|" & SelectedBranch_
                BindGrid(SelectedBranch_, Mode_, CompCode, UserID)
            End If



            If CompCode <> "" Then
                lblCompName.Text = GetCompanyName(CompCode)
                hdnCompCode.Value = CompCode.ToString()
            End If


        End If
    End Sub


    Protected Sub lnkDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDone.Click

        Dim DuplicateFound As Boolean = False
        Dim numPromoID As Integer
        Dim result As String = ""
        Dim oCompCode As Label


        For Each row As GridViewRow In gridBranches.Rows

            Dim cb As CheckBox = row.FindControl("chkRowSel")

            If cb IsNot Nothing AndAlso cb.Checked Then

                oCompCode = row.FindControl("lblCompCode")
                Dim oRowCode As Label = row.FindControl("lblRowCode")

                If result <> "" Then
                    result &= ","
                End If
                result &= oRowCode.Text.ToString().TrimStart("0"c) ' Append selected item text




            End If
        Next

        Dim arr() = Session("SelectedBranches").ToString().Split("|")

        Dim rowIDs As String


        rowIDs = GetRowFiltered(arr(0), result, arr(1))


        If result <> "" Then
            ' SaveMultipleBranches(Session("GID"), Convert.ToInt32(oCompCode.Text), result)
            GetMultipleBranchAndStoreSession(Convert.ToInt32(oCompCode.Text), rowIDs)
        Else
            GetMultipleBranchAndStoreSession(hdnCompCode.Value, rowIDs)
            clsSession.Message = "No Selected Branches"
        End If

        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.Branchwindow.hide();</script>")


    End Sub



    Protected Sub cmdProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProcess.Click

        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.Branchwindow.hide();</script>")

    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

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



    Public Sub GetMultipleBranchAndStoreSession(ByVal CompCode As String, ByVal selectedBranch As String)

        Session.Remove("SelectedBranches")

        Session("SelectedBranches") = CompCode & "|" & selectedBranch

    End Sub


    Private Function GetCompanyName(ByVal CompCode As String) As String
        Dim companyName As String = String.Empty

        SqlDataCompanyDesc.SelectParameters("CompCode").DefaultValue = CompCode
        Dim dv As Data.DataView = CType(SqlDataCompanyDesc.Select(DataSourceSelectArguments.Empty), Data.DataView)

        If dv IsNot Nothing AndAlso dv.Count > 0 Then
            companyName = dv(0)("CompanyName").ToString()
        End If

        Return companyName

    End Function


    Private Sub BindGrid(ByVal RowID As String, ByVal Mode_ As String, ByVal CompCode As String, ByVal userId As String)
        gridBranches.DataSource = Nothing
        gridBranches.DataBind()


        Dim sqlConn As SqlConnection = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand = New SqlCommand("USP_GetSelectedBranch", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlCmd.CommandTimeout = 0


        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userId
        sqlCmd.Parameters.Add("@CompCode", SqlDbType.VarChar).Value = CompCode
        sqlCmd.Parameters.Add("@Mode", SqlDbType.VarChar).Value = Mode_
        sqlCmd.Parameters.Add("@RowID", SqlDbType.VarChar).Value = RowID

        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(sqlCmd)


        Try
            sqlConn.Open()
            da.Fill(dt)
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
            sqlCmd.Dispose()
        End Try

        gridBranches.DataSource = dt
        gridBranches.DataBind()
    End Sub


    Private Function GetRowFiltered(ByVal CompCode As String, ByVal SelectedRow As String, ByVal ReAccumulatedRow As String) As String
        Dim RowID As String = String.Empty

        If SelectedRow.Trim() = "" Then
            SelectedRow = "0"
        End If

        If ReAccumulatedRow.Trim() = "" Then
            ReAccumulatedRow = "0"
        End If

        SqlDSRowID.SelectParameters("CompCode").DefaultValue = CompCode
        SqlDSRowID.SelectParameters("SelectedRow").DefaultValue = SelectedRow
        SqlDSRowID.SelectParameters("ReAccumulatedRow").DefaultValue = ReAccumulatedRow
        Dim dv As Data.DataView = CType(SqlDSRowID.Select(DataSourceSelectArguments.Empty), Data.DataView)

        If dv IsNot Nothing AndAlso dv.Count > 0 Then
            RowID = dv(0)("RowID").ToString()
        End If

        Return RowID

    End Function



End Class
