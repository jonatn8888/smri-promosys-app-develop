#Region " Imports "
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports System.Collections.Generic
Imports System.Linq

#End Region

Partial Class UserMaintenance
    Inherits System.Web.UI.Page


#Region " Subs and Functions "

    Private Function UpdateUser(ByVal UserId As Integer) As Integer
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_UpdateUsers"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserName", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserName").Value = Me.txtUserName.Text.Trim
        sqlCmd.Parameters.Add("@UserDept", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserDept").Value = Me.ddDepartment.SelectedValue
        sqlCmd.Parameters.Add("@SignName", SqlDbType.VarChar)
        sqlCmd.Parameters("@SignName").Value = Me.txtSignName.Text.Trim
        sqlCmd.Parameters.Add("@SignPosition", SqlDbType.VarChar)
        sqlCmd.Parameters("@SignPosition").Value = Me.txtPosition.Text.Trim
        sqlCmd.Parameters.Add("@UserLevel", SqlDbType.SmallInt)
        sqlCmd.Parameters("@UserLevel").Value = CInt(Me.ddUserLevel.SelectedValue)

        Dim A As String = IIf(CStr(ViewState("GroupID")) <> "", ViewState("GroupID"), 0)
        sqlCmd.Parameters.Add("@GroupID", SqlDbType.SmallInt)
        sqlCmd.Parameters("@GroupID").Value = IIf(CStr(ViewState("GroupID")) <> "", ViewState("GroupID"), 0)
        sqlCmd.Parameters.Add("@UserID", SqlDbType.SmallInt)
        sqlCmd.Parameters("@UserID").Value = UserId

        sqlCmd.Parameters.Add("@EmailAdd", SqlDbType.VarChar)
        sqlCmd.Parameters("@EmailAdd").Value = txtLN.Text.Trim

        sqlCmd.Parameters.Add("@BranchCode", SqlDbType.Int)
        sqlCmd.Parameters("@BranchCode").Value = 0
        sqlCmd.Parameters.Add("@CompCode", SqlDbType.Int)
        sqlCmd.Parameters("@CompCode").Value = ddlCompany.SelectedValue


        sqlCmd.Parameters.Add("@AccessSettings", SqlDbType.Bit)
        sqlCmd.Parameters("@AccessSettings").Value = Me.chkAccessSetting.Checked

        Dim result As String = ""
        Dim Resu As String = ""



        Return sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()

    End Function

    Private Function fillGridView() As Boolean
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand("USP_SearchUsers", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlCmd.Parameters.Add("@SearchParam", SqlDbType.VarChar)
        sqlCmd.Parameters("@SearchParam").Value = txtSearch.Text.Trim
        Try
            Dim da As New SqlDataAdapter(sqlCmd)
            Dim ds As New DataSet
            sqlConn.Open()
            da.Fill(ds, "tbl_Users")
            Dim dt As DataTable = ds.Tables("tbl_Users")
            dt.PrimaryKey = New DataColumn() {dt.Columns("UserID")}

            Session("ds") = dt
            If dt.Rows.Count <> 0 Then
                gvUsers.DataSource = dt
                gvUsers.DataBind()
                Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
                Return True
            Else
                Me.lblRecordCount.Text = ""
                Return False
            End If

            da = Nothing
            ds = Nothing
            dt = Nothing

        Catch ex As Exception
        Finally
            ' Added dowcarpio08232012@smretailinc: close and dispose connection
            If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
            sqlConn = Nothing
            sqlCmd = Nothing
            GC.Collect()
        End Try
    End Function
    Private Function GroupAssignDatatable(ByVal userid As Int32) As DataTable
        Dim sqlConn As SqlConnection
        Dim ta As New SqlDataAdapter
        Dim dt As New DataTable

        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_SelectGroupAssignUser"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserID").Value = userid
        ta.SelectCommand = sqlCmd
        ta.Fill(dt)
        Return dt

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing
        ta = Nothing

        GC.Collect()

    End Function

    Private Function SaveUser() As Integer
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_InsertUsers"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserName", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserName").Value = txtUserName.Text.Trim
        sqlCmd.Parameters.Add("@UserDept", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserDept").Value = ddDepartment.SelectedValue
        sqlCmd.Parameters.Add("@SignName", SqlDbType.VarChar)
        sqlCmd.Parameters("@SignName").Value = txtSignName.Text.Trim
        sqlCmd.Parameters.Add("@SignPosition", SqlDbType.VarChar)
        sqlCmd.Parameters("@SignPosition").Value = txtPosition.Text.Trim
        sqlCmd.Parameters.Add("@UserLevel", SqlDbType.SmallInt)
        sqlCmd.Parameters("@UserLevel").Value = CInt(ddUserLevel.SelectedValue)
        sqlCmd.Parameters.Add("@GroupID", SqlDbType.SmallInt)

        ' Revised dowcarpio07032012@smretailinc erroneous ViewState("GroupID")
        Try
            If ViewState("GroupID").ToString = "" Then
                sqlCmd.Parameters("@GroupID").Value = 0
            Else
                sqlCmd.Parameters("@GroupID").Value = IIf(ViewState("GroupID") <> 0 Or ViewState("GroupID") = 7 Or ViewState("GroupID") = 8, CInt(ViewState("GroupID")), 0)
            End If
        Catch ex As Exception
            sqlCmd.Parameters("@GroupID").Value = 0
        End Try

        sqlCmd.Parameters.Add("@EmailAdd", SqlDbType.VarChar)
        sqlCmd.Parameters("@EmailAdd").Value = txtLN.Text.Trim

        sqlCmd.Parameters.Add("@BranchCode", SqlDbType.Int)
        sqlCmd.Parameters("@BranchCode").Value = 0 'ddlBranch.SelectedValue
        sqlCmd.Parameters.Add("@CompCode", SqlDbType.Int)
        sqlCmd.Parameters("@CompCode").Value = ddlCompany.SelectedValue

        sqlCmd.Parameters.Add("@AccessSettings", SqlDbType.Bit)
        sqlCmd.Parameters("@AccessSettings").Value = Me.chkAccessSetting.Checked

        Return sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()

    End Function

    Private Sub fillCheckBox(ByVal rowNum As Integer, ByVal dt As DataTable)
        Dim BoolVal As Integer
        chkAccessSetting.Visible = False
        panAccessSetting.Visible = False
        lblAccessSetting.Visible = False

        If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Analyst Or Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Reviewer Or Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.MemoApprover Then '4
            chkAccessSetting.Visible = True
            panAccessSetting.Visible = True
            lblAccessSetting.Visible = True
            If rowNum <> -1 Then

                BoolVal = dt.Rows(rowNum)("AccessSettings")
                If BoolVal = 1 Then
                    chkAccessSetting.Checked = True
                ElseIf BoolVal = 0 Then
                    chkAccessSetting.Checked = False
                End If

            End If
        End If
    End Sub

    Private Sub fillTextboxes(ByVal rowNum As Integer, ByVal dt As DataTable)
        If rowNum <> -1 Then
            'Dim dt As DataTable

            fillDropDowns()

            txtUserName.Text = dt.Rows(rowNum)("UserName")

            If dt.Rows(rowNum)("UserDept").ToString = "" Then
                ddDepartment.SelectedIndex = -1
            Else
                ddDepartment.SelectedValue = dt.Rows(rowNum)("UserDept")
            End If

            txtSignName.Text = dt.Rows(rowNum)("SignName")
            txtPosition.Text = dt.Rows(rowNum)("SignPosition")

            ' Revised dowcarpio07032012@smretailinc erroneous dt.Rows(rowNum)("UserLevel")
            Try
                ddUserLevel.SelectedValue = dt.Rows(rowNum)("UserLevel")
            Catch ex As Exception
                'do nothing
            End Try

            txtLN.Text = dt.Rows(rowNum)("EmailAdd").ToString
            Session("GID") = dt.Rows(rowNum)("UserID")
            HiddenUserIDField.Value = dt.Rows(rowNum)("UserID")

            InitialUserLevel.Value = ddUserLevel.SelectedValue

            LoadCompanyBranchTree()
            LoadBUDeptTree()

            If dt.Rows(rowNum)("UserLevel") = SystemUser.UserRoles.Analyst Then
                ViewState("flag") = 1
                ViewState("dtGrpAssign") = GroupAssignDatatable(dt.Rows(rowNum)("UserID"))
                fillGridGroupAssignment()

                'cjg2243 SR#4406088 : start
                panGroupAssignment.Visible = True
                'lblGA.Visible = True
                'panDept.Visible = False
                'lblDept.Visible = False
                panBizUnit.Visible = False
                lblBizUnit.Visible = False
                'cjg2243 SR#4406088 : end
                panCategory.Visible = False
                lblCategory.Visible = False

                cmdSelectBranch.Visible = False

                panCompany.Visible = False
                lblCompany.Visible = False

                'cjg2243 SR#4406088 : start
                'panDepartmentViewer.Visible = False
                'lblDepartmentViewer.Visible = False
                'panBizUnitViewer.Visible = False
                'lblBizUnitViewer.Visible = False
                'cjg2243 SR#4406088 : end

                tvBUDept.Visible = False
                panTreeBUDept.Visible = False
                tvCompanies.Visible = False
                panTreeCompanies.Visible = False
                panDelCompanies.Visible = False
                lnkdelTvCompanies.Visible = False
                panDelBUDept.Visible = False
                lnkdelTvBUDept.Visible = False



            ElseIf dt.Rows(rowNum)("UserLevel") = SystemUser.UserRoles.PromoRequestor Then
                '•CMM / MM 6
                'cjg2243 SR#4406088 : start
                'panDept.Visible = True
                'lblDept.Visible = True
                panBizUnit.Visible = True
                lblBizUnit.Visible = True
                'cjg2243 SR#4406088 : end
                panCategory.Visible = False
                lblCategory.Visible = False
                panGroupAssignment.Visible = False
                cmdSelectBranch.Visible = False
                lblGA.Visible = False

                panCompany.Visible = False
                lblCompany.Visible = False
                tvBUDept.Visible = True
                panTreeBUDept.Visible = True
                tvCompanies.Visible = False
                panTreeCompanies.Visible = False
                panDelCompanies.Visible = False
                lnkdelTvCompanies.Visible = False
                panDelBUDept.Visible = True
                lnkdelTvBUDept.Visible = True

                'cjg2243 SR#4406088 : start
                'panDepartmentViewer.Visible = False
                'lblDepartmentViewer.Visible = False
                'panBizUnitViewer.Visible = False
                'lblBizUnitViewer.Visible = False
                'cjg2243 SR#4406088 : end

                ' Revised dowcarpio07032012@smretailinc erroneous ViewState("GroupID")
                Try
                    ddlBizUnit.SelectedValue = dt.Rows(rowNum)("PRGroupID")
                    InitialBizUnit.Value = ddlBizUnit.SelectedValue
                    ViewState("GroupID") = dt.Rows(rowNum)("PRGroupID")
                Catch ex As Exception
                    'do nothing
                End Try

            ElseIf dt.Rows(rowNum)("UserLevel") = SystemUser.UserRoles.RequestApprover Then
                '•MBU 5
                'cjg2243 SR#4406088 : start
                'panDept.Visible = False
                'lblDept.Visible = False
                panBizUnit.Visible = True
                lblBizUnit.Visible = True
                'cjg2243 SR#4406088 : end

                panCategory.Visible = False
                lblCategory.Visible = False

                panGroupAssignment.Visible = False
                lblGA.Visible = False
                cmdSelectBranch.Visible = False
                panCompany.Visible = False
                lblCompany.Visible = False
                tvBUDept.Visible = True
                panTreeBUDept.Visible = True
                tvCompanies.Visible = False
                panTreeCompanies.Visible = False
                panDelCompanies.Visible = False
                lnkdelTvCompanies.Visible = False
                panDelBUDept.Visible = True
                lnkdelTvBUDept.Visible = True
                'cjg2243 SR#4406088 : start
                'panDepartmentViewer.Visible = False
                'lblDepartmentViewer.Visible = False
                'panBizUnitViewer.Visible = False
                'lblBizUnitViewer.Visible = False
                'cjg2243 SR#4406088 : end

                ' Revised dowcarpio07032012@smretailinc erroneous ViewState("GroupID")
                Try
                    ddlBizUnit.SelectedIndex = -1
                    InitialBizUnit.Value = ddlBizUnit.SelectedValue
                    ViewState("GroupID") = dt.Rows(rowNum)("GroupID")
                Catch ex As Exception
                    'do nothing
                End Try

            ElseIf dt.Rows(rowNum)("UserLevel") = SystemUser.UserRoles.Reviewer Then
                '•Pre-Approver 3
                'cjg2243 SR#4406088 : start
                'panDept.Visible = False
                'lblDept.Visible = False
                panBizUnit.Visible = False
                lblBizUnit.Visible = False
                'cjg2243 SR#4406088 : end
                panCategory.Visible = True
                lblCategory.Visible = True
                panGroupAssignment.Visible = True
                lblGA.Visible = True
                cmdSelectBranch.Visible = False
                panCompany.Visible = False
                lblCompany.Visible = False

                'cjg2243 SR#4406088 : start
                'panDepartmentViewer.Visible = False
                'lblDepartmentViewer.Visible = False
                'panBizUnitViewer.Visible = False
                'lblBizUnitViewer.Visible = False
                'cjg2243 SR#4406088 : end

                'rbs7281 with category
                tvBUDept.Visible = False
                panTreeBUDept.Visible = False
                tvCompanies.Visible = False
                panTreeCompanies.Visible = False
                panDelCompanies.Visible = False
                lnkdelTvCompanies.Visible = False
                panDelBUDept.Visible = False
                lnkdelTvBUDept.Visible = False

                ' Revised dowcarpio07032012@smretailinc erroneous ViewState("GroupID")
                Try
                    ddlCategory.SelectedValue = dt.Rows(rowNum)("GroupID")
                    InitialBizUnit.Value = ddlBizUnit.SelectedValue
                    ViewState("GroupID") = dt.Rows(rowNum)("GroupID")
                Catch ex As Exception
                    'do nothing
                End Try


            ElseIf dt.Rows(rowNum)("UserLevel") = SystemUser.UserRoles.MemoApprover Or Me.ddUserLevel.SelectedValue = "" Then
                '•Approver 2
                panGroupAssignment.Visible = True
                lblGA.Visible = True

                'cjg2243 SR#4406088 : start
                'panDept.Visible = False
                'lblDept.Visible = False
                panBizUnit.Visible = False
                lblBizUnit.Visible = False
                'cjg2243 SR#4406088 : end

                panCategory.Visible = False
                lblCategory.Visible = False
                cmdSelectBranch.Visible = False
                panCompany.Visible = False
                lblCompany.Visible = False

                'cjg2243 SR#4406088 : start
                'panDepartmentViewer.Visible = False
                'lblDepartmentViewer.Visible = False
                'panBizUnitViewer.Visible = False
                'lblBizUnitViewer.Visible = False
                'cjg2243 SR#4406088 : end


                ViewState("flag") = 1
                ViewState("dtGrpAssign") = GroupAssignDatatable(dt.Rows(rowNum)("UserID"))
                fillGridGroupAssignment()

                tvBUDept.Visible = False
                panTreeBUDept.Visible = False
                tvCompanies.Visible = False
                panTreeCompanies.Visible = False
                panDelCompanies.Visible = False
                lnkdelTvCompanies.Visible = False
                panDelBUDept.Visible = False
                lnkdelTvBUDept.Visible = False


            ElseIf ddUserLevel.SelectedValue = SystemUser.UserRoles.AnnouncementViewer Then
                ddlCompany.SelectedIndex = -1 'For Tree View, since theres no default selection

                panGroupAssignment.Visible = False
                lblGA.Visible = False

                'cjg2243 SR#4406088 : start
                'panDept.Visible = False
                'lblDept.Visible = False
                panBizUnit.Visible = True
                lblBizUnit.Visible = True
                'cjg2243 SR#4406088 : end

                panCategory.Visible = False
                lblCategory.Visible = False
                cmdSelectBranch.Visible = True
                panCompany.Visible = True
                lblCompany.Visible = True

                tvBUDept.Visible = True
                panTreeBUDept.Visible = True
                tvCompanies.Visible = True
                panTreeCompanies.Visible = True

                panDelCompanies.Visible = True
                lnkdelTvCompanies.Visible = True
                panDelBUDept.Visible = True
                lnkdelTvBUDept.Visible = True

                'panBizUnitViewer.Visible = True
                'lblBizUnitViewer.Visible = True 'cjg2243 SR#4406088

                'cjg2243 SR#4406088 : start
                ddlBizUnit.SelectedIndex = -1

                'cjg2243 SR#4406088 : end

                Dim compCode As String = dt.Rows(rowNum)("CompCode").ToString()

                ViewState("GroupID") = dt.Rows(rowNum)("GroupID")

                Dim intflag As Integer
                intflag = ValidateIfDeptOrBiz()

                If intflag = 0 Then
                    ddlBizUnitViewer.SelectedValue = dt.Rows(rowNum)("GroupID")
                    ddlDepartmentViewer.SelectedValue = 0

                    'cjg2243 SR#4406088 : start
                    'panDepartmentViewer.Visible = False
                    'lblDepartmentViewer.Visible = False
                    'cjg2243 SR#4406088 : end
                Else

                    'cjg2243 SR#4406088 : start
                    'panDepartmentViewer.Visible = True
                    'lblDepartmentViewer.Visible = True
                    'cjg2243 SR#4406088 : end

                    ddlBizUnitViewer.SelectedValue = intflag

                    ddlDepartmentViewer.DataSource = getDTDropDown("USP_SelectUserGroupsDptViewer " & ddlBizUnitViewer.SelectedValue)
                    ddlDepartmentViewer.DataTextField = "Department"
                    ddlDepartmentViewer.DataValueField = "GroupID"
                    ddlDepartmentViewer.DataBind()

                    ddlDepartmentViewer.SelectedValue = dt.Rows(rowNum)("GroupID")
                End If

            End If

            ddlBizUnit.SelectedIndex = -1

            'rbs7281 5/5/2025 retrieve the updated BU and Dept Selection for Request Reviewer
            ViewState("GroupID") = dt.Rows(rowNum)("GroupID")

        Else
            txtUserName.Text = ""
            ddDepartment.SelectedIndex = -1
            txtSignName.Text = ""
            txtPosition.Text = ""
            ddUserLevel.SelectedIndex = -1
            txtLN.Text = ""

            panGroupAssignment.Visible = False
            lblGA.Visible = False

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = False
            panBizUnit.Visible = True
            lblBizUnit.Visible = True
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False
            cmdSelectBranch.Visible = True
            panCompany.Visible = False
            lblCompany.Visible = False

            tvBUDept.Visible = True
            panTreeBUDept.Visible = True
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False


        End If
    End Sub
    Private Sub deleteUser(ByVal rowNum As Integer)
        Dim dt As DataTable
        dt = Session("ds")
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_DeleteUser"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserID").Value = rowNum
        sqlCmd.ExecuteNonQuery()


        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing

        GC.Collect()
    End Sub

    'USP_SelectCheckIfDeptOrBiz

    Public Sub fillGroupAssignmentDropDown()

        ddGroupAssignment.DataSource = objDSGroupAssignment1
        ddGroupAssignment.DataTextField = "Description"
        ddGroupAssignment.DataValueField = "GroupID"
        ddGroupAssignment.DataBind()
        GC.Collect()
    End Sub
    Public Sub fillGridGroupAssignment()
        Dim dt As DataTable
        dt = CType(ViewState("dtGrpAssign"), DataTable)
        gvGroupAssign.DataSource = dt
        gvGroupAssign.DataBind()

        ddGroupAssignment.Items.Clear()
        ddGroupAssignment.DataSource = objDSGroupAssignment2
        ddGroupAssignment.DataTextField = "Description"
        ddGroupAssignment.DataValueField = "GroupID"
        ddGroupAssignment.DataBind()
        GC.Collect()
    End Sub
    Public Sub fillDropDowns()

        ddlDepartment.DataSource = objDSDept

        ddlDepartment.DataTextField = "Department"

        ddlDepartment.DataValueField = "GroupID"

        ddlDepartment.DataBind()

        ddlDepartment.SelectedIndex = -1


        ddlBizUnit.DataSource = objDSBizUnit
        ddlBizUnit.DataTextField = "Description" '"BizUnit"
        ddlBizUnit.DataValueField = "GroupID"
        ddlBizUnit.DataBind()
        ddlBizUnit.SelectedIndex = -1

        ddlCategory.DataSource = getDTDropDown("USP_SelectUserGroupsCategory")
        ddlCategory.DataTextField = "Category"
        ddlCategory.DataValueField = "GroupID"
        ddlCategory.DataBind()
        ddlCategory.SelectedIndex = -1

        ddlCompany.DataSource = objCompany
        ddlCompany.DataTextField = "CompanyName"
        ddlCompany.DataValueField = "CompCode"
        ddlCompany.DataBind()
        ddlCompany.SelectedIndex = -1

        ddlBizUnitViewer.DataSource = objDSBizUnit
        ddlBizUnitViewer.DataTextField = "Description" '"BizUnit"
        ddlBizUnitViewer.DataValueField = "GroupID"
        ddlBizUnitViewer.DataBind()
        ddlBizUnitViewer.SelectedIndex = -1


        GC.Collect()
    End Sub
    Public Sub resetGroupAssignment()
        Dim dt As New DataTable
        ViewState("flag") = 0
        ViewState("dtGrpAssign") = Nothing
        ViewState("dtGrpAssign") = dt
        ddGroupAssignment.Items.Clear()
        fillGroupAssignmentDropDown()
        gvGroupAssign.DataSource = Nothing
        gvGroupAssign.DataBind()
        GC.Collect()
    End Sub

    Private Function getDTDropDown(ByVal usp As String) As DataTable
        'GC.Collect() -- Revised dowcarpio08232012@smretailinc: commented this line.
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand

        Dim ds As New DataSet

        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = usp
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = CommandType.Text

        Dim da As New SqlDataAdapter(sqlCmd)
        da.Fill(ds, "tblDropDown")
        Dim dt As DataTable = ds.Tables("tblDropDown")
        Return dt

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing
        ds = Nothing
        da = Nothing

        GC.Collect()

    End Function


    Private Sub SaveGroupAssignment(ByVal userid As Int32, ByVal groupdid As Int32)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_InsertGroupAssignment"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserID").Value = userid
        sqlCmd.Parameters.Add("@GroupID", SqlDbType.VarChar)
        sqlCmd.Parameters("@GroupID").Value = groupdid
        sqlCmd.ExecuteNonQuery()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Sub

    Private Sub saveRecord(ByVal userid As Int32, ByVal GroupID As String)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_DeleteGroupAssignment"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserID").Value = userid
        sqlCmd.ExecuteNonQuery()

        Dim dt As DataTable
        Dim i As Integer
        dt = ViewState("dtGrpAssign")

        If gvGroupAssign.Rows.Count() = 0 Then
            SaveGroupAssignment(userid, -1)
        End If

        If GroupID = Nothing Then

            For i = 0 To dt.Rows.Count - 1
                SaveGroupAssignment(userid, dt.Rows(i)(0))
            Next

        Else

            SaveGroupAssignment(userid, GroupID)

        End If




        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing

        GC.Collect()
    End Sub

    'Added dowcarpio09242012@smretailinc: to save promo requestor group assignment
    'Private Sub saveRecordPerGroup(ByVal userid As Int32)
    '    Dim sqlConn As SqlConnection
    '    sqlConn = New SqlConnection(clsPromo.SQLConnString())
    '    Dim sqlCmd As SqlCommand
    '    sqlConn.Open()
    '    sqlCmd = New SqlCommand
    '    sqlCmd.CommandText = "USP_DeleteGroupAssignment"
    '    sqlCmd.Connection = sqlConn
    '    sqlCmd.CommandTimeout = 0
    '    sqlCmd.CommandType = 4
    '    sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar)
    '    sqlCmd.Parameters("@UserID").Value = userid
    '    sqlCmd.ExecuteNonQuery()

    '    Dim dt As DataTable
    '    Dim i As Integer
    '    dt = ViewState("dtGrpAssign")

    '    For i = 0 To dt.Rows.Count - 1
    '        SaveGroupAssignment(userid, dt.Rows(i)(0))
    '    Next

    '    ' Added dowcarpio08232012@smretailinc: close and dispose connection
    '    If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
    '    sqlConn = Nothing
    '    sqlCmd = Nothing
    '    dt = Nothing

    '    GC.Collect()
    'End Sub

    Private Sub setupcontrols(ByVal UserLevel As String)

        'cjg2243 SR#4406088 : add fields for admin to select the assigned BU and department for all user roles

        If UserLevel = SystemUser.UserRoles.PromoRequestor Then
            '•CMM / MM 6

            'cjg2243 SR#4406088 : start
            'panDept.Enabled = True
            'lblDept.Enabled = True
            panBizUnit.Enabled = True
            lblBizUnit.Enabled = True
            'cjg2243 SR#4406088 : end

            panCategory.Enabled = False
            lblCategory.Enabled = False
            panGroupAssignment.Enabled = False
            lblGA.Enabled = False
            cmdSelectBranch.Visible = False
            panCompany.Visible = False
            lblCompany.Visible = False

            'cjg2243 SR#4406088 : start
            'panDept.Visible = True
            'lblDept.Visible = True
            panBizUnit.Visible = True
            lblBizUnit.Visible = True
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False
            panGroupAssignment.Visible = False
            lblGA.Visible = False

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = False
            'lblBizUnitViewer.Visible = False
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            ''cjg2243 SR#4406088 : end
            chkAccessSetting.Visible = False
            lblAccessSetting.Visible = False
            panAccessSetting.Visible = False
            tvBUDept.Visible = True
            panTreeBUDept.Visible = True
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False
            panDelCompanies.Visible = False
            lnkdelTvCompanies.Visible = False
            panDelBUDept.Visible = True
            lnkdelTvBUDept.Visible = True

        ElseIf UserLevel = SystemUser.UserRoles.RequestApprover Then
            '•MBU 5

            'cjg2243 SR#4406088 : start
            'panDept.Enabled = False
            'lblDept.Enabled = False
            panBizUnit.Enabled = True
            lblBizUnit.Enabled = True
            'cjg2243 SR#4406088 : end

            panCategory.Enabled = False
            lblCategory.Enabled = False
            panGroupAssignment.Enabled = False
            lblGA.Enabled = False
            cmdSelectBranch.Visible = False
            panCompany.Visible = False
            lblCompany.Visible = False

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = False
            panBizUnit.Visible = True
            lblBizUnit.Visible = True
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False
            panGroupAssignment.Visible = False
            lblGA.Visible = False

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = False
            'lblBizUnitViewer.Visible = False
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            'cjg2243 SR#4406088 : end

            ddlBizUnit.SelectedIndex = -1
            ViewState("GroupID") = ddlBizUnit.SelectedValue
            chkAccessSetting.Visible = False
            lblAccessSetting.Visible = False
            panAccessSetting.Visible = False
            tvBUDept.Visible = True
            panTreeBUDept.Visible = True
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False
            panDelCompanies.Visible = False
            lnkdelTvCompanies.Visible = False
            panDelBUDept.Visible = True
            lnkdelTvBUDept.Visible = True


        ElseIf UserLevel = SystemUser.UserRoles.Analyst Then
            '•MPD Analyst 4

            'cjg2243 SR#4406088 : start
            'panDept.Enabled = False
            'lblDept.Enabled = False
            panBizUnit.Enabled = False
            lblBizUnit.Enabled = False
            panBizUnit.Visible = False
            lblBizUnit.Visible = False
            'cjg2243 SR#4406088 : end

            panCategory.Enabled = False
            lblCategory.Enabled = False
            panGroupAssignment.Enabled = True
            lblGA.Enabled = True
            panGroupAssignment.Visible = True
            lblGA.Visible = True

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = False
            'panBizUnit.Visible = False
            'lblBizUnit.Visible = False
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False
            cmdSelectBranch.Visible = False
            panCompany.Visible = False
            lblCompany.Visible = False

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = False
            'lblBizUnitViewer.Visible = False
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            'cjg2243 SR#4406088 : end
            ddlBizUnit.SelectedIndex = -1
            ViewState("GroupID") = ddlBizUnit.SelectedValue
            CategorySelection("0")
            chkAccessSetting.Visible = True
            lblAccessSetting.Visible = True
            panAccessSetting.Visible = True
            tvBUDept.Visible = False
            panTreeBUDept.Visible = False
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False
            panDelCompanies.Visible = False
            lnkdelTvCompanies.Visible = False
            panDelBUDept.Visible = False
            lnkdelTvBUDept.Visible = False

        ElseIf UserLevel = SystemUser.UserRoles.Reviewer Then
            '•Pre-Approver 3

            'cjg2243 SR#4406088 : start
            'panDept.Enabled = False
            'lblDept.Enabled = False
            panBizUnit.Enabled = False
            lblBizUnit.Enabled = False
            'cjg2243 SR#4406088 : end

            panCategory.Enabled = True
            lblCategory.Enabled = True
            panGroupAssignment.Enabled = True
            lblGA.Enabled = True
            panGroupAssignment.Visible = True
            lblGA.Visible = True
            cmdSelectBranch.Visible = False
            panCompany.Visible = False
            lblCompany.Visible = False

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = False
            panBizUnit.Visible = False
            lblBizUnit.Visible = False
            'cjg2243 SR#4406088 : end

            panCategory.Visible = True
            lblCategory.Visible = True

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = False
            'lblBizUnitViewer.Visible = False
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            'cjg2243 SR#4406088 : end

            'ddlCategory.SelectedValue = 0
            CategorySelection(ddlBizUnit.SelectedValue)
            chkAccessSetting.Visible = False
            lblAccessSetting.Visible = False
            panAccessSetting.Visible = False
            tvBUDept.Visible = False
            panTreeBUDept.Visible = False
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False
            panDelCompanies.Visible = False
            lnkdelTvCompanies.Visible = False
            panDelBUDept.Visible = True
            lnkdelTvBUDept.Visible = True


        ElseIf UserLevel = SystemUser.UserRoles.MemoApprover Or Me.ddUserLevel.SelectedValue = "" Then
            '•Approver 2

            'cjg2243 SR#4406088 : start
            'panDept.Enabled = False
            'lblDept.Enabled = False
            panBizUnit.Enabled = False
            lblBizUnit.Enabled = False
            'cjg2243 SR#4406088 : end

            panCategory.Enabled = False
            lblCategory.Enabled = False
            panGroupAssignment.Enabled = True
            lblGA.Enabled = True
            panGroupAssignment.Visible = True
            lblGA.Visible = True

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = False
            panBizUnit.Visible = False
            lblBizUnit.Visible = False
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False

            'Branch False

            panCompany.Visible = False
            lblCompany.Visible = False

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = False
            'lblBizUnitViewer.Visible = False
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            'cjg2243 SR#4406088 : end

            'ViewState("GroupID") = 0 'rbs7281 #4406088 Requirement is to add fields for admin to select the assigned BU and department for all user roles.
            CategorySelection("0")
            chkAccessSetting.Visible = False
            lblAccessSetting.Visible = False
            panAccessSetting.Visible = False
            tvBUDept.Visible = False
            panTreeBUDept.Visible = False
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False
            panDelCompanies.Visible = False
            lnkdelTvCompanies.Visible = False
            panDelBUDept.Visible = True
            lnkdelTvBUDept.Visible = True


        ElseIf UserLevel = SystemUser.UserRoles.POSpersonnel Or UserLevel = "" Then
            'ViewState("GroupID") = 0 'rbs7281 #4406088 Requirement is to add fields for admin to select the assigned BU and department for all user roles.

            'cjg2243 SR#4406088 : start
            'panDept.Enabled = False
            'lblDept.Enabled = False
            panBizUnit.Enabled = True
            lblBizUnit.Enabled = True
            'cjg2243 SR#4406088 : end

            panCategory.Enabled = False
            lblCategory.Enabled = False
            panGroupAssignment.Enabled = False
            lblGA.Enabled = False
            panGroupAssignment.Visible = False
            lblGA.Visible = False

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = False
            panBizUnit.Visible = True
            lblBizUnit.Visible = True
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False
            'Branch False
            panCompany.Visible = False
            lblCompany.Visible = False

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = False
            'lblBizUnitViewer.Visible = False
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            'cjg2243 SR#4406088 : end
            ddlBizUnit.SelectedIndex = -1
            ViewState("GroupID") = ddlBizUnit.SelectedValue
            chkAccessSetting.Visible = False
            lblAccessSetting.Visible = False
            panAccessSetting.Visible = False
            tvBUDept.Visible = False
            panTreeBUDept.Visible = False
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False
            panDelCompanies.Visible = False
            lnkdelTvCompanies.Visible = False
            panDelBUDept.Visible = True
            lnkdelTvBUDept.Visible = True

        ElseIf UserLevel = SystemUser.UserRoles.AnnouncementViewer Then
            'ViewState("GroupID") = 0 'rbs7281 #4406088 Requirement is to add fields for admin to select the assigned BU and department for all user roles.

            'cjg2243 SR#4406088 : start
            'panDept.Enabled = False
            'lblDept.Enabled = False
            panBizUnit.Enabled = True
            lblBizUnit.Enabled = True
            'cjg2243 SR#4406088 : end
            cmdSelectBranch.Visible = True
            panCategory.Enabled = False
            lblCategory.Enabled = False
            panGroupAssignment.Enabled = False
            lblGA.Enabled = False
            panGroupAssignment.Visible = False
            lblGA.Visible = False

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = True
            panBizUnit.Visible = True
            lblBizUnit.Visible = True
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False
            'Branch True
            panCompany.Visible = True
            lblCompany.Visible = True

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = True
            'lblBizUnitViewer.Visible = True
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            'cjg2243 SR#4406088 : end


            ddlBizUnitViewer.SelectedIndex = -1
            'ddlBranch.SelectedIndex = -1
            ddlCompany.SelectedIndex = -1
            ddlDepartmentViewer.SelectedIndex = -1

            'ViewState("GroupID") = 0 'rbs7281 #4406088 Requirement is to add fields for admin to select the assigned BU and department for all user roles.
            ddlBizUnit.SelectedIndex = -1
            ViewState("GroupID") = ddlBizUnit.SelectedValue
            chkAccessSetting.Visible = False
            lblAccessSetting.Visible = False
            tvBUDept.Visible = True
            panTreeBUDept.Visible = True
            tvCompanies.Visible = True
            panTreeCompanies.Visible = True
            panDelCompanies.Visible = True
            lnkdelTvCompanies.Visible = True
            panDelBUDept.Visible = True
            lnkdelTvBUDept.Visible = True

        Else ' Added dowcarpio09252012@smretailinc: initialize control
            'ViewState("GroupID") = 0 'rbs7281 #4406088 Requirement is to add fields for admin to select the assigned BU and department for all user roles.

            If UserLevel = SystemUser.UserRoles.RequestReviewer Then 'Request Reviewer to enable
                panBizUnit.Enabled = True
                lblBizUnit.Enabled = True
                panBizUnit.Visible = True
                lblBizUnit.Visible = True
                chkAccessSetting.Visible = False
                lblAccessSetting.Visible = False
                panAccessSetting.Visible = False
                tvBUDept.Visible = True
                panTreeBUDept.Visible = True
                tvCompanies.Visible = False
                panTreeCompanies.Visible = False
                panDelCompanies.Visible = False
                lnkdelTvCompanies.Visible = False
                panDelBUDept.Visible = True
                lnkdelTvBUDept.Visible = True
            End If


            'cjg2243 SR#4406088 : start
            'panDept.Enabled = False
            'lblDept.Enabled = False
            'panBizUnit.Enabled = False
            'lblBizUnit.Enabled = False
            'cjg2243 SR#4406088 : end

            panCategory.Enabled = False
            lblCategory.Enabled = False
            panGroupAssignment.Enabled = False
            lblGA.Enabled = False
            panGroupAssignment.Visible = False
            lblGA.Visible = False

            'cjg2243 SR#4406088 : start
            'panDept.Visible = False
            'lblDept.Visible = False
            'panBizUnit.Visible = False
            'lblBizUnit.Visible = False
            'cjg2243 SR#4406088 : end

            panCategory.Visible = False
            lblCategory.Visible = False
            'Branch False
            panCompany.Visible = False
            lblCompany.Visible = False

            tvBUDept.Visible = False
            tvCompanies.Visible = False

            tvBUDept.Visible = True
            panTreeBUDept.Visible = True
            tvCompanies.Visible = False
            panTreeCompanies.Visible = False
            panDelCompanies.Visible = False
            lnkdelTvCompanies.Visible = False
            panDelBUDept.Visible = True
            lnkdelTvBUDept.Visible = True

            'cjg2243 SR#4406088 : start
            'panBizUnitViewer.Visible = False
            'lblBizUnitViewer.Visible = False
            'panDepartmentViewer.Visible = False
            'lblDepartmentViewer.Visible = False
            'cjg2243 SR#4406088 : end

            ddlBizUnitViewer.SelectedIndex = -1
            'ddlBranch.SelectedIndex = -1
            ddlCompany.SelectedIndex = -1
            ddlDepartmentViewer.SelectedIndex = -1

            ddlBizUnit.SelectedIndex = -1
            ViewState("GroupID") = ddlBizUnit.SelectedValue
        End If

        'cjg2243 SR#4406088 : start
        panDept.Visible = False
        lblDept.Visible = False
        panDepartmentViewer.Visible = False
        lblDepartmentViewer.Visible = False
        panBizUnitViewer.Visible = False
        lblBizUnitViewer.Visible = False
        'cjg2243 SR#4406088 : end

        GC.Collect()
    End Sub

    Private Function ValidateUserNameUpdating(ByVal UserId As Integer) As Integer
        'GC.Collect() 'Revised dowcarpio08232012@smretailinc: commented this line
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_ValidateUpdateUsers"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserName", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserName").Value = Me.txtUserName.Text.Trim
        sqlCmd.Parameters.Add("@UserID", SqlDbType.Int)
        sqlCmd.Parameters("@UserID").Value = UserId
        Return sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        GC.Collect()

    End Function

    Private Function ValidateUserNameSaving() As Integer
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_ValidateUsersName"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserName", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserName").Value = Me.txtUserName.Text.Trim
        Return sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Function

#End Region

#Region " Events "

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        MyBase.OnPreRender(e)
        Dim strDisAbleBackButton As String
        strDisAbleBackButton = "<script language='javascript'>"
        strDisAbleBackButton += "window.history.forward(1);"
        strDisAbleBackButton += vbLf & "</script>"
        ClientScript.RegisterClientScriptBlock(Me.Page.[GetType](), "clientScript", strDisAbleBackButton)
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim UserID As String = ""

        Dim lastClicked As String
        If Request.Form("__EVENTTARGET") = Nothing Then
            lastClicked = ""
        Else
            lastClicked = Request.Form("__EVENTTARGET").ToString()
        End If

        If lastClicked.Contains("gvUsers") Then
            tvBUDept.Nodes.Clear()
        End If

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")

        Me.lblValidateMessage.Text = ""

        If Not IsPostBack Then
            txtSearch.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + btnSearch.UniqueID + "','')")

            If (SystemUser.UserLevel > SystemUser.UserRoles.Analyst) Then Response.Redirect("InvalidAccess.aspx")

            fillGridView()
            ViewState("flag") = 0
            Dim dt As New DataTable
            ViewState("dtGrpAssign") = dt

            'cjg2243 SR#4406088 : start
            If Session("deptSelected") = Nothing Then
                Session("deptSelected") = " "
            End If
            If Session("bizUnit") = Nothing Then
                Session("bizUnit") = "0"
            End If
            'cjg2243 SR#4406088 : end

            'Dim rootWebConfig As System.Configuration.Configuration
            'rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/ReportPageSite")
            'connString = rootWebConfig.ConnectionStrings.ConnectionStrings("AccountRemittanceConnectionString")

            Me.tblSearchUsers.Visible = True
            Me.tblGridviewUsers.Visible = True

            Me.tblUserInput.Visible = False
            Me.btnCancel.Visible = False
            Me.btnSave.Visible = False
            Me.cmdNew.Visible = True


            setupcontrols(0)
            fillGroupAssignmentDropDown()
            fillDropDowns()
            GetTreeGroupIDs()
            GetTreeRowIDs()

        End If

        'BranchDeptLoader(lastClicked, Session("GID"))



        Me.gvGroupAssign.Focus()
    End Sub

    Protected Sub gvUsers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUsers.RowCommand
        If e.CommandName.ToLower = "view" Then

            Dim i As Integer = 0
            Dim rowView As DataRowView
            Dim row As DataRow
            For Each rowView In CType(Session("ds"), DataTable).DefaultView
                row = rowView.Row
                If row("UserID") = e.CommandArgument Then
                    ViewState("RowNum") = i
                    setupcontrols(CType(Session("ds"), DataTable).Rows(i)("UserLevel"))
                    fillTextboxes(i, Session("ds"))
                    fillCheckBox(i, Session("ds"))
                    Exit For
                End If
                i = i + 1
            Next

            'tables
            Me.tblSearchUsers.Visible = False
            Me.tblGridviewUsers.Visible = False
            Me.tblUserInput.Visible = True

            'buttons
            Me.btnCancel.Visible = True
            Me.btnSave.Visible = True
            Me.cmdNew.Visible = False
            Me.btnDelete.Visible = True

            btnSave.Text = "Update"

            ViewState("DeleteID") = e.CommandArgument

            'cjg2243 SR#4406088 : start
            Session("deptSelected") = " "
            'cjg2243 SR#4406088 : end
        End If
    End Sub

    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click

        Select Case ViewState("save").ToString()
            Case "0" 'for Saving Message
                If clsSession.DeleteStatus = "yes" Then

                    Dim saveResult As String = String.Empty
                    saveResult = ViewState("saveResult").ToString()

                    SaveUsersTreeView(saveResult)

                    lblPopTitle.Value = "Updating Successful"
                    clsSession.Message = "User record was successfully updated."
                    clsSession.Icon = "success"
                    ViewState("save") = "1"
                    ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
                    'audit trail
                    clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.UserMaintenance), "0#"), Session("GID"), "Edit user " & txtUserName.Text.Trim, SystemUser.UserName, "User Maintenance")
                End If
            Case "2"
                'visible false
            Case "4" 'For Department Selection
                If clsSession.DeleteStatus = "yes" Then
                    DeleteTreeViewDept()
                End If
            Case "5" 'For Department Selection
                If clsSession.DeleteStatus = "yes" Then
                    DeleteTreeViewCompBranch()
                End If
            Case "1"

                'tables
                Me.tblSearchUsers.Visible = True
                Me.tblGridviewUsers.Visible = True
                Me.tblUserInput.Visible = False

                'buttons
                Me.btnCancel.Visible = False
                Me.btnSave.Visible = False
                Me.cmdNew.Visible = True
                Me.btnDelete.Visible = False

                Me.txtSearch.Text = ""
                fillGridView()

            Case "3"

                If clsSession.DeleteStatus = "yes" Then
                    'delete
                    'If ViewState("SavingStatus") <> "cancel" Then
                    deleteUser(ViewState("DeleteID"))

                    fillGridView()
                    'tables
                    Me.tblSearchUsers.Visible = True
                    Me.tblGridviewUsers.Visible = True
                    Me.tblUserInput.Visible = False

                    'buttons
                    Me.btnCancel.Visible = False
                    Me.btnSave.Visible = False
                    Me.cmdNew.Visible = True
                    Me.btnDelete.Visible = False
                    Me.txtSearch.Text = ""
                    fillGridView()

                    'clsSession.Message = "Deleted Branches: <br>" & _
                    '                     "<div style='width:350px; height:130px; overflow:auto; background-color: WhiteSmoke; padding: 10px 10px 10px 10px;'>" & _
                    '                     clsSession.Message & "</div><br>"
                    'clsSession.Icon = "fyi"
                    'lblPopTitle.Value = "Promotions"
                    'ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('313','508');</script>")

                    'audit trail
                    clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.UserMaintenance), "0#"), ViewState("DeleteID"), "Delete user " & txtUserName.Text.Trim, SystemUser.UserName, "User Maintenance")
                End If

        End Select


        GC.Collect()

    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If fillGridView() Then
            tblGridviewUsers.Visible = True
            lblValidateMessage.Text = ""
        Else
            lblValidateMessage.Text = "No record found."
            Me.tblGridviewUsers.Visible = False
        End If

        Session.Remove("IsBranchModified")
    End Sub
    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        tvBUDept.Nodes.Clear()
        tvCompanies.Nodes.Clear()

        setupcontrols(0)
        Session.Remove("GID")

        Session.Remove("SelectedBranches")
        Session.Remove("SelectedDept")

        'tables
        tblSearchUsers.Visible = False
        tblGridviewUsers.Visible = False
        tblUserInput.Visible = True
        'buttons
        btnCancel.Visible = True
        btnSave.Visible = True
        cmdNew.Visible = False
        btnDelete.Visible = False

        btnSave.Text = "Save"
        fillTextboxes(-1, Session("ds"))
        fillCheckBox(-1, Session("ds"))

        panGroupAssignment.Visible = False
        lblGA.Visible = False

        'cjg2243 SR#4406088 : start
        'panDept.Visible = False
        'lblDept.Visible = False
        panBizUnit.Visible = False
        lblBizUnit.Visible = False
        'cjg2243 SR#4406088 : end

        panCategory.Visible = False
        lblCategory.Visible = False

        'cjg2243 SR#4406088 : start
        'panBizUnitViewer.Visible = False
        'lblBizUnitViewer.Visible = False
        'panDepartmentViewer.Visible = False
        'lblDepartmentViewer.Visible = False
        'cjg2243 SR#4406088 : end
        panTreeBUDept.Visible = False
        panTreeCompanies.Visible = False
        tvBUDept.Visible = False
        tvBUDept.Visible = False
        panDelCompanies.Visible = False
        lnkdelTvCompanies.Visible = False
        panDelBUDept.Visible = False
        lnkdelTvBUDept.Visible = False

    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        tvBUDept.Nodes.Clear()
        tvCompanies.Nodes.Clear()

        blistErrorMsg.Items.Clear()
        Me.txtSearch.Text = ""
        fillGridView()
        'tables
        Me.tblSearchUsers.Visible = True
        Me.tblGridviewUsers.Visible = True


        Me.tblUserInput.Visible = False
        'buttons
        Me.btnCancel.Visible = False
        Me.btnSave.Visible = False
        Me.cmdNew.Visible = True
        Me.btnDelete.Visible = False

        blistErrorMsg.Items.Clear()

        resetGroupAssignment()

        Session.Remove("GID")

    End Sub
    ' Revised dowcarpio09252012@smretailinc: Revised 
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ' validate entries

        blistErrorMsg.Items.Clear()
        If Me.txtUserName.Text <> "" And btnSave.Text = "Save" Then
            If ValidateUserNameSaving() = 0 Then
                blistErrorMsg.Items.Add("User name already exists.")
            End If
        End If

        If Me.txtUserName.Text = "" Then
            blistErrorMsg.Items.Add("User name must not be blank.")
        End If

        If Me.txtSignName.Text = "" Then
            blistErrorMsg.Items.Add("Sign name must not be blank.")
        End If
        If Me.txtPosition.Text = "" Then
            blistErrorMsg.Items.Add("Position must not be blank.")
        End If

        If Me.ddUserLevel.SelectedIndex = -1 Then
            blistErrorMsg.Items.Add("User level must not be blank.")
        End If

        If ddDepartment.SelectedIndex = 0 Then
            blistErrorMsg.Items.Add("Division must not be blank.")
        End If

        ' Added dowcarpio09242012@smretailinc: additional validation for user level
        If Me.ddUserLevel.SelectedIndex = 0 Then
            blistErrorMsg.Items.Add("User level must not be blank.")
        End If

        If Me.ddUserLevel.SelectedIndex <> -1 Then
            'rbs7281 P1 UAT Mantis#62727
            
            'If ddlBizUnit.SelectedIndex = 0 Then
            '    blistErrorMsg.Items.Add("Business unit must not be blank.")
            'End If --For Tree VIew
            If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.PromoRequestor Then
                Dim parentWithChildrenCount As Integer = 0

                For Each node As TreeNode In tvBUDept.Nodes()
                    If node.ChildNodes.Count > 0 Then
                        parentWithChildrenCount += 1
                    End If
                Next

                If parentWithChildrenCount > 1 Then
                    blistErrorMsg.Items.Add("A Promo Requester can be assigned to only one BU.")
                End If

                If parentWithChildrenCount = 0 Then
                    blistErrorMsg.Items.Add("A Promo Requester is required to have only one Business Unit.")
                End If
            ElseIf Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Analyst Then '4 
                Dim dt As DataTable
                dt = ViewState("dtGrpAssign")
                If dt.Rows.Count = 0 Then
                    ' blistErrorMsg.Items.Add("Group assignment must not be blank.") 
                End If 'For MPD – BU field is optional, not mandatory (if no BU is selected, all BU will be accessed) rbs7281 4/30/2025

            ElseIf Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Reviewer Then ' 3
                If Me.ddlCategory.SelectedIndex = 0 Then
                    blistErrorMsg.Items.Add("Category must not be blank.")
                End If
            ElseIf Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.MemoApprover Then ' 2

            ElseIf Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.AnnouncementViewer Then '8
                'If Me.ddlBranch.SelectedValue = -1 Then
                '    blistErrorMsg.Items.Add("Branch must not be blank.")
                'End If
                'If Me.ddlCompany.SelectedValue = 0 Then
                '    blistErrorMsg.Items.Add("Company must not be blank.")
                'End If

                'If Me.ddlBizUnitViewer.SelectedValue = 0 Then
                '    blistErrorMsg.Items.Add("Business unit must not be blank.")
                'End If

            End If
        End If
        If blistErrorMsg.Items.Count > 0 Then Exit Sub
        Dim saveResult As Integer

        If btnSave.Text = "Save" Then
            If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Analyst Then '4
                ViewState("GroupID") = 0
            End If

            saveResult = SaveUser()
            If saveResult <> 0 And saveResult <> -1 Then

                'Reagan 3/21/2025 Multibranch Selection
                Dim result As String = ""

                If BUSelection(ddUserLevel.SelectedValue) = False Then
                    If saveResult <> 0 And saveResult <> -1 Then

                        Dim SelectedGroupId As String = String.Empty

                        For Each BUDeptNode As TreeNode In tvBUDept.Nodes
                            For Each DeptNode As TreeNode In BUDeptNode.ChildNodes
                                Dim groupID As String = DeptNode.Value
                                Dim Department As String = DeptNode.Text

                                SelectedGroupId &= groupID & ","
                            Next
                        Next
                        UpdateDepts(saveResult, SelectedGroupId, SystemUser.UserID)
                    End If
                End If

                If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.AnnouncementViewer Then
                    Dim SelectedRowId As String = String.Empty

                    For Each companyNode As TreeNode In tvCompanies.Nodes
                        For Each branchNode As TreeNode In companyNode.ChildNodes
                            Dim rowID As String = branchNode.Value
                            Dim branchName As String = branchNode.Text

                            SelectedRowId &= rowID & ","
                        Next
                    Next

                    SaveBranchAssignment(saveResult, SelectedRowId, SystemUser.UserID)

                End If

                Dim Analyst As String = SystemUser.UserRoles.Analyst

                If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Analyst Or Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Reviewer Or Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.MemoApprover Then ' 4

                    saveRecord(saveResult, Nothing)

                ElseIf Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.PromoRequestor Then

                    'saveRecord(saveResult, ddlDepartment.SelectedValue)

                End If
                lblPopTitle.Value = "Saving Successful"
                clsSession.Message = "User record was successfully saved."
                clsSession.Icon = "success"
                ViewState("save") = "1"
                'audit trail
                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.UserMaintenance), "0#"), saveResult, "Add user " & txtUserName.Text.Trim, SystemUser.UserName, "User Maintenance")
            Else
                lblPopTitle.Value = "Saving Failed"
                clsSession.Message = "Duplicate Record Found."
                clsSession.Icon = "error"
                ViewState("save") = "2"
            End If

        Else

            Dim dt As DataTable = Session("ds")

            If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Analyst Then '4
                ViewState("GroupID") = 0
            End If

            saveResult = UpdateUser(HiddenUserIDField.Value)

            If saveResult <> -1 And ViewState("ErrMsg") Is Nothing Then
                If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Analyst Or Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.Reviewer Or Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.MemoApprover Then '4

                    saveRecord(saveResult, Nothing)

                    'ElseIf Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.PromoRequestor Then

                    'saveRecord(saveResult, ddlDepartment.SelectedValue) -- Business Unit is now the Department Dropdown before rbs7281

                End If

            ElseIf saveResult <> -1 And ViewState("ErrMsg") IsNot Nothing And Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.AnnouncementViewer Then
                Select Case ViewState("ErrMsg").ToString()
                    Case "1"
                        lblPopTitle.Value = "Updating Failed"
                        clsSession.Icon = "error"
                        clsSession.Message = "No Branch Selected"
                        ViewState("save") = "2"
                        ViewState.Remove("ErrMsg")
                End Select
            Else
                lblPopTitle.Value = "Updating Failed"
                clsSession.Icon = "error"
                clsSession.Message = "Record already exists."
                ViewState("save") = "2"
            End If

            'PromoRequestor only takes one BU and required to be assigned
            If tvCompanies.Nodes.Count = 0 And tvBUDept.Nodes.Count = 0 And Me.ddUserLevel.SelectedValue <> SystemUser.UserRoles.PromoRequestor Then
                MessageBox("If both the Company and Business Unit lists are left empty, all Companies and Business Units will be assigned automatically. Are you sure you want to proceed?", "0", "Warning", "warning", saveResult)
            ElseIf tvCompanies.Nodes.Count = 0 And tvBUDept.Nodes.Count > 0 And Me.ddUserLevel.SelectedValue <> SystemUser.UserRoles.PromoRequestor Then
                MessageBox("If the Company lists are left empty, all will be assigned automatically. Are you sure you want to proceed?", "0", "Warning", "warning", saveResult)
            ElseIf tvCompanies.Nodes.Count > 0 And tvBUDept.Nodes.Count = 0 And Me.ddUserLevel.SelectedValue <> SystemUser.UserRoles.PromoRequestor Then
                MessageBox("If the Business Unit lists are left empty, all will be assigned automatically. Are you sure you want to proceed?", "0", "Warning", "warning", saveResult)
            Else
                SaveUsersTreeView(saveResult)
                lblPopTitle.Value = "Updating Successful"
                clsSession.Message = "User record was successfully updated."
                clsSession.Icon = "success"
                ViewState("save") = "1"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
                'audit trail
                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.UserMaintenance), "0#"), Session("GID"), "Edit user " & txtUserName.Text.Trim, SystemUser.UserName, "User Maintenance")
            End If

        End If
        'cjg2243 SR#4406088 : end

        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub

    Protected Sub gvGroupAssign_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGroupAssign.RowCommand
        If e.CommandName = "Delete" Then

            Dim dt As DataTable
            dt = CType(ViewState("dtGrpAssign"), DataTable)
            dt.PrimaryKey = New DataColumn() {dt.Columns("GroupID")}
            Dim rowIndex As String

            Dim i As Integer = 0
            Dim rowView As DataRowView
            Dim row As DataRow
            For Each rowView In CType(ViewState("dtGrpAssign"), DataTable).DefaultView
                row = rowView.Row
                If row("GroupID") = e.CommandArgument Then
                    Exit For
                End If
                i = i + 1
            Next
            Session("GID") = e.CommandArgument
            ViewState("Desc") = CType(ViewState("dtGrpAssign"), DataTable).Rows(i)("Description")
            rowIndex = dt.Rows.IndexOf(dt.Rows.Find(Session("GID")))
            dt.Rows(rowIndex).Delete()
            dt.AcceptChanges()
            ViewState("dtGrpAssign") = dt
            Me.gvGroupAssign.DataSource = dt
            Me.gvGroupAssign.DataBind()
            ViewState("flag") = 1
        End If
    End Sub
    Protected Sub ddUserLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUserLevel.SelectedIndexChanged
        If ddUserLevel.SelectedValue <> 0 Then
            blistErrorMsg.Items.Clear()
            setupcontrols(ddUserLevel.SelectedValue)
        End If
    End Sub
    Protected Sub ddlCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCategory.SelectedIndexChanged
        ViewState("GroupID") = ddlCategory.SelectedValue
    End Sub
    Protected Sub ddlBizUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBizUnit.SelectedIndexChanged
        ViewState("GroupID") = ddlBizUnit.SelectedValue
        Session("bizUnit") = ddlBizUnit.SelectedValue
    End Sub
    Protected Sub ddlDepartment_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartment.SelectedIndexChanged
        'ViewState("GroupID") = ddlDepartment.SelectedValue 'Department is not existing anymore
    End Sub
    Protected Sub btnAddGroupAssignment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddGroupAssignment.Click
        If Me.ddGroupAssignment.SelectedValue <> 0 Then
            Dim dt As DataTable
            dt = CType(ViewState("dtGrpAssign"), DataTable)
            Dim dr As DataRow
            If dt.Rows.Count = 0 And ViewState("flag") = 0 Then
                dt.Columns.Add("GroupID")
                dt.Columns.Add("Description")
                ViewState("flag") = 1
            End If


            dr = dt.NewRow()

            dr("GroupID") = Me.ddGroupAssignment.SelectedValue
            dr("Description") = Me.ddGroupAssignment.SelectedItem
            dt.Rows.Add(dr)

            ViewState("dtGrpAssign") = dt

            Me.gvGroupAssign.DataSource = dt
            Me.gvGroupAssign.DataBind()
            Me.ddGroupAssignment.Items.Remove(Me.ddGroupAssignment.SelectedItem)
        End If
    End Sub
    Protected Sub gvUsers_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUsers.PageIndexChanging
        Dim dt As DataTable = CType(Session("ds"), DataTable)
        Me.gvUsers.PageIndex = e.NewPageIndex
        Me.gvUsers.DataSource = dt
        Me.gvUsers.DataBind()
        Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        lblPopTitle.Value = "Delete User"
        clsSession.Message = "Are you sure you want to delete this record?"
        ViewState("save") = "3"
        clsSession.Icon = "inquiry"
        ViewState("SavingStatus") = "delete"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")

    End Sub

#End Region

    Protected Sub ddlBizUnitViewer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBizUnitViewer.SelectedIndexChanged
        If ddlBizUnitViewer.SelectedIndex = -1 Then
            ViewState("GroupID") = 0
        Else
            ViewState("GroupID") = ddlBizUnitViewer.SelectedValue

            'cjg2243 SR#4406088 : start
            'panDepartmentViewer.Visible = True
            'lblDepartmentViewer.Visible = True
            'cjg2243 SR#4406088 : end

            ddlDepartmentViewer.DataSource = getDTDropDown("USP_SelectUserGroupsDptViewer " & ddlBizUnitViewer.SelectedValue)
            ddlDepartmentViewer.DataTextField = "Department"
            ddlDepartmentViewer.DataValueField = "GroupID"
            ddlDepartmentViewer.DataBind()
        End If
    End Sub

    Protected Sub ddlDepartmentViewer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDepartmentViewer.SelectedIndexChanged
        If ddlDepartmentViewer.SelectedIndex <> -1 Then
            ViewState("GroupID") = ddlDepartmentViewer.SelectedValue
        Else
            ViewState("GroupID") = 0
        End If
    End Sub


    Private Function ValidateIfDeptOrBiz() As Integer
        'GC.Collect() ' Revised dowcarpio08232012@smretailinc: commented this line
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_SelectCheckIfDeptOrBiz"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@GroupID", SqlDbType.VarChar)
        sqlCmd.Parameters("@GroupID").Value = ViewState("GroupID")

        Return sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Function




    Protected Sub cmdSelectBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectBranch.Click
        CompaniesLastAction.Value = "Edit"

        If ddlCompany.SelectedIndex = 0 Then

            lblPopTitle.Value = "Select Branch"
            clsSession.Message = "Cannot select branches. No company selected.<br/><br/>Please select a company from list."
            clsSession.Icon = "fyi"
            ViewState("process") = "selectbranch"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")
            ViewState("save") = "2"
        Else

            Session("TransFlag") = "natural"

            Dim branchParams As String = ""
            Dim Mode As String = "1"
            Dim SelectedBranch As String = ""

            SelectedBranch = Session("SelectedBranches")

            Dim joinRowIds As String = String.Empty
            Dim isCompanyExist As Boolean = False


            For Each companyNode As TreeNode In tvCompanies.Nodes
                For Each branchNode As TreeNode In companyNode.ChildNodes
                    Dim rowID As String = branchNode.Value
                    Dim branchName As String = branchNode.Text
                    Dim companyCode As String = companyNode.Value

                    joinRowIds &= rowID & ","

                Next
            Next

            For Each CompanyNode As TreeNode In tvCompanies.Nodes
                Dim CompanyID As String = CompanyNode.Value

                If CompanyID = ddlCompany.SelectedValue.ToString() Then
                    isCompanyExist = True
                End If
            Next

            SelectedBranch = joinRowIds


            If isCompanyExist = False Then
                ViewState.Remove("branchParams")
            Else
                Dim modifyCompBranches As String = ViewState("branchParams")
                Dim modArr() = modifyCompBranches.Split("|")

                ViewState("branchParams") = ddlCompany.SelectedValue.ToString() & "|" & Session("GID") & "|" & Mode & "|" & SelectedBranch
            End If

            If ViewState("branchParams") IsNot Nothing Then
                branchParams = ViewState("branchParams")
            Else
                branchParams = ddlCompany.SelectedValue.ToString() & "|" & Session("GID") & "|" & Mode & "|" & SelectedBranch
            End If


            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openBranches('" & branchParams & "','','');</script>")

            'Response.Redirect("SelectBranches.aspx?EnvCode=" & cboCompany.SelectedValue.ToString())
        End If
    End Sub


    Protected Sub cmdLoadForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLoadForm.Click
        BranchDeptLoader("", Session("GID"))
    End Sub


    Private Sub UpdateDepts(ByVal userID As String, ByVal selected As String, ByVal LoginUserId As String)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_UpdateUserGroups"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserID").Value = userID
        sqlCmd.Parameters.Add("@GroupIDs", SqlDbType.VarChar)
        sqlCmd.Parameters("@GroupIDs").Value = selected
        sqlCmd.Parameters.Add("@LoginUsedID", SqlDbType.VarChar)
        sqlCmd.Parameters("@LoginUsedID").Value = LoginUserId
        sqlCmd.ExecuteNonQuery()
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
    End Sub

    Protected Sub cmdSelectDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelectDept.Click
        BUDeptLastAction.Value = "Edit"

        If ddlBizUnit.SelectedIndex = 0 Then

            lblPopTitle.Value = "Select Department"
            clsSession.Message = "Cannot select department. No business unit.<br/><br/>Please select a business unit from list."
            clsSession.Icon = "fyi"
            ViewState("process") = "selectbranch"
            ViewState("save") = "2"
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

        Else
            Session("TransFlag") = "natural"

            Dim deptParams As String = ""
            Dim Mode As String = "1"
            Dim SelectedDept As String = ""

            SelectedDept = Session("SelectedDept")

            Dim joinGroupIds As String = String.Empty
            Dim isBUExist As Boolean = False

            For Each BUDeptNode As TreeNode In tvBUDept.Nodes
                For Each DepartmentNode As TreeNode In BUDeptNode.ChildNodes
                    Dim GroupID As String = DepartmentNode.Value
                    Dim Department As String = DepartmentNode.Text
                    Dim PGroupID As String = BUDeptNode.Value

                    joinGroupIds &= GroupID & ","

                Next
            Next

            For Each BUDeptNode As TreeNode In tvBUDept.Nodes
                Dim PGroupID As String = BUDeptNode.Value

                If PGroupID = ddlBizUnit.SelectedValue.ToString() Then
                    isBUExist = True
                End If
            Next

            SelectedDept = joinGroupIds

            If isBUExist = False Then
                ViewState.Remove("deptParams")
            Else
                Dim modifyDept As String = ViewState("deptParams")
                Dim modArr() = modifyDept.Split("|")

                ViewState("deptParams") = ddlBizUnit.SelectedValue.ToString() & "|" & Session("GID") & "|" & Mode & "|" & SelectedDept
            End If

            If ViewState("deptParams") IsNot Nothing Then
                deptParams = ViewState("deptParams")
            Else
                deptParams = ddlBizUnit.SelectedValue.ToString() & "|" & Session("GID") & "|" & Mode & "|" & SelectedDept
            End If

            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openModalDept('" & deptParams & "','','');</script>")



            'Response.Redirect("SelectBranches.aspx?EnvCode=" & cboCompany.SelectedValue.ToString())
        End If
    End Sub


    Protected Sub ddlCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCompany.SelectedIndexChanged
        Dim CompanyCode As String = ddlCompany.SelectedValue

    End Sub


    Protected Sub chkBizUnitAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkAll As CheckBox = CType(sender, CheckBox)

        For Each row As GridViewRow In gvGroupAssign.Rows
            Dim chkSel As CheckBox = CType(row.FindControl("chkBizUnitSelect"), CheckBox)
            Dim GroupID As CheckBox = CType(row.FindControl("GroupID"), CheckBox)
            If chkSel IsNot Nothing Then
                chkSel.Checked = chkAll.Checked
            End If
        Next
    End Sub



    Protected Sub lnkRemoveBizUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRemoveBizUnit.Click
        Dim dt As DataTable = CType(ViewState("dtGrpAssign"), DataTable)

        If dt Is Nothing Then
            Exit Sub
        End If

        For Each row As GridViewRow In gvGroupAssign.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkSelect As CheckBox = CType(row.FindControl("chkBizUnitSelect"), CheckBox)
                If chkSelect IsNot Nothing AndAlso chkSelect.Checked Then
                    Dim desc As String = row.Cells(2).Text


                    ' Loop to find and remove matching row
                    For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                        If dt.Rows(i)("Description").ToString() = desc.Replace("amp;", "") Then
                            Dim groupId As String = dt.Rows(i)("GroupID").ToString()
                            ddGroupAssignment.Items.Add(New ListItem(desc, groupId))
                            dt.Rows.RemoveAt(i)
                        End If
                    Next

                    ' Optionally add back to dropdown

                End If
            End If
        Next
        Dim sortedItems = Me.ddGroupAssignment.Items.Cast(Of ListItem)().OrderBy(Function(i) i.Text).ToList()
        Me.ddGroupAssignment.Items.Clear()
        Me.ddGroupAssignment.Items.AddRange(sortedItems.ToArray())

        ViewState("dtGrpAssign") = dt
        Me.gvGroupAssign.DataSource = dt
        Me.gvGroupAssign.DataBind()
    End Sub




    Protected Sub gvGroupAssign_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvGroupAssign.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowID As Integer = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "GroupID"))
            Dim BizUnit As String = GetBizUnitByID(rowID)

            If rowID <> 0 Then
                ' Set the Label control inside the TemplateField
                Dim lblBizUnits As Label = CType(e.Row.FindControl("lblBizUnits"), Label)
                If lblBizUnits IsNot Nothing Then
                    lblBizUnits.Text = BizUnit
                End If
            End If
        End If
    End Sub



    Private Function GetBizUnitByID(ByVal id As Integer) As String
        Dim result As String = String.Empty

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())

        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "SELECT BizUnit from UserGroups where GroupID = @ID"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = CommandType.Text ' Equivalent to CommandType = 1
        sqlCmd.Parameters.Add("@ID", SqlDbType.Int).Value = id

        Dim reader As SqlDataReader = sqlCmd.ExecuteReader()
        If reader.Read() Then
            result = reader("BizUnit").ToString()
        End If
        reader.Close()

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        Return result
    End Function

    Private Sub BranchDeptLoader(ByVal lastClicked As String, ByVal UserID As String)

        'cjg2243 SR#4406088 : start
        If Session("GID") = Nothing Then
            Session("GID") = "0"
        End If

        If lastClicked.Contains("gvUsers") Then
            Session.Remove("SelectedBranches")
            Session.Remove("SelectedDept")
            ViewState("branchParams") = Nothing
            ViewState("deptParams") = Nothing
            tvCompanies.Nodes.Clear()
            tvBUDept.Nodes.Clear()
        End If


        Dim SelectedBranches As String = ""
        Dim SelectedDept As String = ""

        SelectedBranches = Session("SelectedBranches")
        SelectedDept = Session("SelectedDept")

        If SelectedDept IsNot Nothing Then
            If SelectedDept <> "" Then
                If Not lastClicked.Contains("lnkdelTvBUDept") Then
                    If BUDeptLastAction.Value = "Edit" Then
                        LoadBUDeptTree(SelectedDept)
                    Else
                        LoadBUDeptTree(SelectedDept, GetTreeGroupIDs())
                    End If

                End If
            End If
        End If

        If SelectedBranches IsNot Nothing Then
            If SelectedBranches <> "" Then
                If Not lastClicked.Contains("lnkdelTvCompanies") Then
                    If CompaniesLastAction.Value = "Edit" Then
                        LoadCompanyBranchTree(SelectedBranches)
                    Else
                        LoadCompanyBranchTree(SelectedBranches, GetTreeRowIDs())
                    End If

                End If
            End If
        End If

    End Sub

    Private Sub CategorySelection(ByVal SelectedCategory As String)
        ddlCategory.SelectedIndex = -1
        ViewState("GroupID") = SelectedCategory
    End Sub

    Private Function BUSelection(ByVal SelectedBU As String) As Boolean
        Dim ReturnVal As Boolean = False

        Dim allowedValues As String() = {SystemUser.UserRoles.Analyst, SystemUser.UserRoles.Reviewer, SystemUser.UserRoles.MemoApprover}

        If allowedValues.Contains(SelectedBU) Then
            ReturnVal = True
        Else
            ReturnVal = False
        End If

        Return ReturnVal
    End Function

    Private Sub LoadCompanyBranchTree(Optional ByVal SelectedBranches As String = "", Optional ByVal SelectedBranchAfterDeletion As String = "")

        Dim CompCode As String = ""
        Dim RowidParams As String = ""
        Dim Mode_ As String = ""



        Dim Arr() = SelectedBranches.Split("|")

        If SelectedBranches <> "" Then
            RowContainer_.Value &= Arr(1) + ","
            CompCode = Arr(0)
            RowidParams = Arr(1) + ","
            Mode_ = "2"
            tvCompanies.Nodes.Clear()
        Else
            CompCode = "0"
            RowidParams = "0"
            Mode_ = "0"
            RowContainer_.Value = ""
        End If


        If Session("GID") IsNot Nothing Then

            If Session("GID") Is Nothing Then
                Throw New Exception("Session GID is missing.")
            End If

            If RowidParams.Trim() = "" Then
                RowidParams = "0"
            End If

            sqldsBranches.SelectParameters("userID").DefaultValue = Session("GID").ToString()
            sqldsBranches.SelectParameters("CompCode").DefaultValue = CInt(CompCode)
            sqldsBranches.SelectParameters("Mode").DefaultValue = CInt(Mode_)
            sqldsBranches.SelectParameters("ROWID").DefaultValue = RowidParams
            Dim dv As DataView = CType(sqldsBranches.Select(DataSourceSelectArguments.Empty), DataView)

            ' Reuse previous dictionary if needed
            Dim companyNodes As New Dictionary(Of String, TreeNode)

            ' Load existing nodes to avoid duplicates
            For Each node As TreeNode In tvCompanies.Nodes
                If Not companyNodes.ContainsKey(node.Value) Then
                    companyNodes.Add(node.Value, node)
                End If
            Next



            For Each row As DataRowView In dv
                Dim companyCode As String = row("CompCode").ToString()
                Dim companyName As String = row("CompanyName").ToString()
                Dim branchName As String = row("BranchName").ToString()
                Dim rowID As String = row("rowID").ToString()

                ' Reuse or create company node
                Dim parentNode As TreeNode
                If Not companyNodes.ContainsKey(companyCode) Then
                    parentNode = New TreeNode(companyName)
                    parentNode.Value = companyCode
                    parentNode.SelectAction = TreeNodeSelectAction.None
                    tvCompanies.Nodes.Add(parentNode)
                    companyNodes.Add(companyCode, parentNode)
                Else
                    parentNode = companyNodes(companyCode)
                End If

                ' Avoid duplicate branches under same company
                Dim branchExists As Boolean = parentNode.ChildNodes.Cast(Of TreeNode).Any(Function(n) n.Value = rowID)
                If Not branchExists Then
                    Dim childNode As New TreeNode(branchName)
                    childNode.Value = rowID
                    parentNode.ChildNodes.Add(childNode)
                End If
            Next
        End If


        Dim RetCompany As String = ddlCompany.SelectedValue.ToString()
        Dim branchParams As String = ""
        Dim Mode As String = "1"
        Dim SelectedBranch As String = ""

        SelectedBranch = Session("SelectedBranches")

        If RetCompany <> "0" Then
            branchParams = RetCompany & "|" & Session("GID") & "|" & Mode & "|" & GetTreeRowIDs() 'Changes here, must remove the unchecked rowid

            ViewState("branchParams") = branchParams
        End If
    End Sub

    Private Sub LoadBUDeptTree(Optional ByVal SelectedDept As String = "", Optional ByVal SelectedDeptAfterDeletion As String = "")

        Dim BizUnit As String = ""
        Dim GroupidParams As String = ""
        Dim Mode_ As String = ""



        Dim Arr() = SelectedDept.Split("|")

        If SelectedDept <> "" And SelectedDeptAfterDeletion = "" Then
            GroupContainer_.Value &= Arr(1) + ","
            BizUnit = Arr(0)
            GroupidParams = Arr(1) + ","
            Mode_ = "2"
            tvBUDept.Nodes.Clear()
        ElseIf SelectedDeptAfterDeletion <> "" And SelectedDept <> "" Then
            GroupContainer_.Value &= Arr(1) + ","
            BizUnit = Arr(0)
            GroupidParams = SelectedDeptAfterDeletion + ","
            Mode_ = "2"
            tvBUDept.Nodes.Clear()
        Else
            BizUnit = "0"
            GroupidParams = "0"
            Mode_ = "0"
            GroupContainer_.Value = ""
        End If

        If Session("GID") IsNot Nothing Then

            If Session("GID") Is Nothing Then
                Throw New Exception("Session GID is missing.")
            End If

            If GroupidParams.Trim() = "" Then
                GroupidParams = "0"
            End If

            sqldsBUDept.SelectParameters("userID").DefaultValue = Session("GID").ToString()
            sqldsBUDept.SelectParameters("BizUnit").DefaultValue = BizUnit
            sqldsBUDept.SelectParameters("Mode").DefaultValue = CInt(Mode_)
            sqldsBUDept.SelectParameters("GROUPID").DefaultValue = GroupidParams
            Dim dv2 As DataView = CType(sqldsBUDept.Select(DataSourceSelectArguments.Empty), DataView)

            ' Reuse previous dictionary if needed
            Dim BizUnitNodes As New Dictionary(Of String, TreeNode)

            ' Load existing nodes to avoid duplicates
            For Each node As TreeNode In tvBUDept.Nodes
                If Not BizUnitNodes.ContainsKey(node.Value) Then
                    BizUnitNodes.Add(node.Value, node)
                End If
            Next



            For Each row As DataRowView In dv2
                Dim ParentGroupID As String = row("ParentGroupID").ToString()
                Dim BizUnit_ As String = row("BizUnit").ToString()
                Dim Department As String = row("Department").ToString()
                Dim GroupID As String = row("GroupID").ToString()

                ' Reuse or create company node
                Dim parentNode As TreeNode
                If Not BizUnitNodes.ContainsKey(ParentGroupID) Then
                    parentNode = New TreeNode(BizUnit_)
                    parentNode.Value = ParentGroupID
                    parentNode.SelectAction = TreeNodeSelectAction.None
                    tvBUDept.Nodes.Add(parentNode)
                    BizUnitNodes.Add(ParentGroupID, parentNode)
                Else
                    parentNode = BizUnitNodes(ParentGroupID)
                End If

                ' Avoid duplicate branches under same company
                Dim branchExists As Boolean = parentNode.ChildNodes.Cast(Of TreeNode).Any(Function(n) n.Value = GroupID)
                If Not branchExists Then
                    Dim childNode As New TreeNode(Department)
                    childNode.Value = GroupID
                    parentNode.ChildNodes.Add(childNode)
                End If
            Next
        End If

        Dim RetDept As String = ddlBizUnit.SelectedValue.ToString()
        Dim deptParams As String = ""
        Dim Mode As String = "1"
        Dim SelectedDept_ As String = ""

        SelectedDept_ = Session("SelectedDept")

        If RetDept <> "0" Then
            deptParams = RetDept & "|" & Session("GID") & "|" & Mode & "|" & GetTreeGroupIDs() 'Changes here, must remove the unchecked rowid

            ViewState("deptParams") = deptParams
        End If
    End Sub



    Private Sub SaveBranchAssignment(ByVal userid As Int32, ByVal RowID As String, ByVal LoginUserID As Int32)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_InsertBranchAssignment"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@UserID", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserID").Value = userid
        sqlCmd.Parameters.Add("@RowID", SqlDbType.VarChar)
        sqlCmd.Parameters("@RowID").Value = RowID
        sqlCmd.Parameters.Add("@LoginUsedID", SqlDbType.VarChar)
        sqlCmd.Parameters("@LoginUsedID").Value = LoginUserID
        sqlCmd.ExecuteNonQuery()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Sub


    Private Function GetTreeGroupIDs() As String

        Dim joinGroupIds As String = String.Empty


        For Each BizUnitNodes As TreeNode In tvBUDept.Nodes()
            For Each deptNode As TreeNode In BizUnitNodes.ChildNodes
                Dim GroupId As String = deptNode.Value
                Dim DeptName As String = deptNode.Text
                Dim BizUnitCode As String = BizUnitNodes.Value

                ' Example: Output or use it
                joinGroupIds &= GroupId & ","
            Next
        Next

        Return joinGroupIds

    End Function

    Private Function GetTreeRowIDs() As String

        Dim joinRowIds As String = String.Empty


        For Each companyNode As TreeNode In tvCompanies.Nodes
            For Each branchNode As TreeNode In companyNode.ChildNodes
                Dim rowID As String = branchNode.Value
                Dim branchName As String = branchNode.Text
                Dim companyCode As String = companyNode.Value

                ' Example: Output or use it
                joinRowIds &= rowID & ","

            Next
        Next

        Return joinRowIds

    End Function

    Private Sub GetCheckedNodes(ByVal node As TreeNode, ByVal list As List(Of String))
        If node.Checked Then
            list.Add(Server.HtmlEncode(node.Text))
        End If

        For Each child As TreeNode In node.ChildNodes
            GetCheckedNodes(child, list)
        Next
    End Sub



    Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
        ' Register the TreeView to accept postback data
        ClientScript.RegisterForEventValidation(tvBUDept.UniqueID)

        ' Important: Call base to render the page
        MyBase.Render(writer)
    End Sub



    Protected Sub lnkdelTvBUDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkdelTvBUDept.Click
        lblPopTitle.Value = "Delete Department(s)"
        clsSession.Message = "Are you sure you want to delete the selected department?"
        ViewState("save") = "4"
        clsSession.Icon = "inquiry"
        ViewState("SavingStatus") = "delete"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub

    Protected Sub lnkdelTvCompanies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkdelTvCompanies.Click
        lblPopTitle.Value = "Delete Branch(es)"
        clsSession.Message = "Are you sure you want to delete the selected branches?"
        ViewState("save") = "5"
        clsSession.Icon = "inquiry"
        ViewState("SavingStatus") = "delete"
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
    End Sub

    Private Sub DeleteTreeViewDept()
        ' Loop in reverse through parent nodes
        For i As Integer = tvBUDept.Nodes.Count - 1 To 0 Step -1
            Dim parentNode As TreeNode = tvBUDept.Nodes(i)

            ' Loop in reverse through child nodes
            For j As Integer = parentNode.ChildNodes.Count - 1 To 0 Step -1
                Dim childNode As TreeNode = parentNode.ChildNodes(j)
                If childNode.Checked Then
                    parentNode.ChildNodes.Remove(childNode)
                End If
            Next

            ' After removing child nodes, remove parent if empty
            If parentNode.ChildNodes.Count = 0 Then
                tvBUDept.Nodes.Remove(parentNode)
            End If
        Next

        BUDeptLastAction.Value = "Delete"
    End Sub

    Private Sub DeleteTreeViewCompBranch()
        ' Loop in reverse through parent nodes
        For i As Integer = tvCompanies.Nodes.Count - 1 To 0 Step -1
            Dim parentNode As TreeNode = tvCompanies.Nodes(i)

            ' Loop in reverse through child nodes
            For j As Integer = parentNode.ChildNodes.Count - 1 To 0 Step -1
                Dim childNode As TreeNode = parentNode.ChildNodes(j)
                If childNode.Checked Then
                    parentNode.ChildNodes.Remove(childNode)
                End If
            Next

            ' After removing child nodes, remove parent if empty
            If parentNode.ChildNodes.Count = 0 Then
                tvCompanies.Nodes.Remove(parentNode)
            End If
        Next

        CompaniesLastAction.Value = "Delete"
    End Sub


    Protected Sub lnkselAllTvBUDept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkselAllTvBUDept.Click
        For i As Integer = tvBUDept.Nodes.Count - 1 To 0 Step -1
            Dim parentNode As TreeNode = tvBUDept.Nodes(i)

            ' Loop in reverse through child nodes
            For j As Integer = parentNode.ChildNodes.Count - 1 To 0 Step -1
                Dim childNode As TreeNode = parentNode.ChildNodes(j)
                If childNode.Checked = True Then
                    childNode.Checked = False
                Else
                    childNode.Checked = True
                End If

            Next

        Next
    End Sub

    Protected Sub lnkselAllTvCompanies_Click_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkselAllTvCompanies_Click.Click
        For i As Integer = tvCompanies.Nodes.Count - 1 To 0 Step -1
            Dim parentNode As TreeNode = tvCompanies.Nodes(i)

            For j As Integer = parentNode.ChildNodes.Count - 1 To 0 Step -1
                Dim childNode As TreeNode = parentNode.ChildNodes(j)
                If childNode.Checked = True Then
                    childNode.Checked = False
                Else
                    childNode.Checked = True
                End If

            Next

        Next
    End Sub


    Private Sub MessageBox(ByVal sMessage As String, ByVal sProcess As String, ByVal sTitle As String, ByVal sIcon As String, ByVal sSaveResult As String)

        ' prompt for confirmation
        clsSession.Message = sMessage
        clsSession.Icon = sIcon
        lblPopTitle.Value = sTitle
        ViewState("save") = sProcess
        ViewState("SavingStatus") = "delete"
        ViewState("saveResult") = sSaveResult
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

    End Sub

    Private Sub SaveUsersTreeView(ByVal saveResult As String)
        If BUSelection(ddUserLevel.SelectedValue) = False Then
            If saveResult <> 0 And saveResult <> -1 Then

                UpdateDepts(saveResult, GetTreeGroupIDs(), SystemUser.UserID) 'babalikan kita
            End If
        End If

        If Me.ddUserLevel.SelectedValue = SystemUser.UserRoles.AnnouncementViewer Then
            If saveResult <> 0 And saveResult <> -1 Then

                SaveBranchAssignment(saveResult, GetTreeRowIDs(), SystemUser.UserID)
            End If
        End If
    End Sub


End Class