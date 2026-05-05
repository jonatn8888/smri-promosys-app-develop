Imports dsPromotionsTableAdapters
Imports System.Data
Imports System.Data.SqlClient


Partial Class SelectUserDept
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ' verify user and page request
        If (SystemUser.UserID = 0) Or (Request("myParams") = Nothing) Then Response.Redirect("InvalidAccess.aspx")

        If Not Page.IsPostBack() Then

            'lblCompName.Text = Session("CurrEnvName")

            Dim myParams = Request("myParams").ToString()

            Dim arr() = myParams.Split("|")

            Dim BU As String
            BU = arr(0)

            Dim UserID As String
            UserID = arr(1)

            Dim Mode_ As String
            Mode_ = arr(2)

            On Error GoTo HELL

            Dim SelectedDept_ As String
            SelectedDept_ = arr(3)


            Dim SelectedDept As String = String.Empty
            If arr(4) IsNot Nothing Then
                If Session("SelectedDept") IsNot Nothing Then
                    SelectedDept = arr(4)
                End If
            End If

HELL:

            If SelectedDept <> "" Then
                If SelectedDept IsNot Nothing Then
                    BindGrid(SelectedDept, Mode_, BU, UserID)
                End If
            Else
                Session("SelectedDept") = BU & "|" & SelectedDept_
                BindGrid(SelectedDept_, Mode_, BU, UserID)
            End If



            If BU <> "" Then
                lblDeptName.Text = GetBUDescription(BU)
                hdnSelectedBU.Value = BU.ToString()
            End If

        End If

    End Sub

    Private Sub BindGrid(ByVal GroupID As String, ByVal Mode_ As String, ByVal BU As String, ByVal userId As String)
        grdDepts.DataSource = Nothing
        grdDepts.DataBind()


        Dim sqlConn As SqlConnection = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand = New SqlCommand("USP_GetSelectedBUDept", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlCmd.CommandTimeout = 0


        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = userId
        sqlCmd.Parameters.Add("@BizUnit", SqlDbType.VarChar).Value = BU
        sqlCmd.Parameters.Add("@Mode", SqlDbType.VarChar).Value = Mode_
        sqlCmd.Parameters.Add("@GROUPID", SqlDbType.VarChar).Value = GroupID

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

        grdDepts.DataSource = dt
        grdDepts.DataBind()
    End Sub

    Protected Sub lnkDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDone.Click

        Dim DuplicateFound As Boolean = False
        Dim result As String = ""
        Dim lblGroupID As Label

        For Each row As GridViewRow In grdDepts.Rows()

            Dim cb As CheckBox = row.FindControl("chkRowSel")

            If cb IsNot Nothing AndAlso cb.Checked Then

                lblGroupID = row.FindControl("lblGroupID")

                If result <> "" Then
                    result &= ","
                End If
                result &= lblGroupID.Text ' Append selected item text

            End If
        Next


        Dim arr() = Session("SelectedDept").ToString().Split("|")

        Dim groupIDs As String


        groupIDs = GetGroupFiltered(arr(0), result, arr(1))


        If result <> "" Then
            ' SaveMultipleBranches(Session("GID"), Convert.ToInt32(oCompCode.Text), result)
            GetMultipleDeptAndStoreSession(Convert.ToInt32(lblGroupID.Text), groupIDs)
        Else
            GetMultipleDeptAndStoreSession(hdnSelectedBU.Value, groupIDs)
            clsSession.Message = "No Selected Department"
        End If

        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.modalWindow.hide();</script>")
    End Sub

    Protected Sub cmdProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdProcess.Click

        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.modalWindow.hide();</script>")

    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkALL.CheckedChanged

        If chkALL.Checked = True Then
            Dim row As GridViewRow
            For Each row In grdDepts.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In grdDepts.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                chkSel.Checked = False
            Next
        End If

    End Sub

    Private Function GetBUDescription(ByVal GroupId As String) As String
        Dim Description As String = String.Empty

        SqlDataBUDesc.SelectParameters("GroupId").DefaultValue = GroupId
        Dim dv As Data.DataView = CType(SqlDataBUDesc.Select(DataSourceSelectArguments.Empty), Data.DataView)

        If dv IsNot Nothing AndAlso dv.Count > 0 Then
            Description = dv(0)("Description").ToString()
        End If

        Return Description

    End Function

    Private Function GetGroupFiltered(ByVal BizUnit As String, ByVal SelectedGroup As String, ByVal ReAccumulatedGroup As String) As String
        Dim GroupID As String = String.Empty

        If SelectedGroup.Trim() = "" Then
            SelectedGroup = "0"
        End If

        If ReAccumulatedGroup.Trim() = "" Then
            ReAccumulatedGroup = "0"
        End If

        SqlDSGroupID.SelectParameters("BizUnit").DefaultValue = BizUnit
        SqlDSGroupID.SelectParameters("SelectedRow").DefaultValue = SelectedGroup
        SqlDSGroupID.SelectParameters("ReAccumulatedRow").DefaultValue = ReAccumulatedGroup
        Dim dv As Data.DataView = CType(SqlDSGroupID.Select(DataSourceSelectArguments.Empty), Data.DataView)

        If dv IsNot Nothing AndAlso dv.Count > 0 Then
            GroupID = dv(0)("GroupID").ToString()
        End If

        Return GroupID

    End Function

    Public Sub GetMultipleDeptAndStoreSession(ByVal BU As String, ByVal selectedDept As String)

        Session.Remove("SelectedDept")

        Session("SelectedDept") = BU & "|" & selectedDept

    End Sub


End Class
