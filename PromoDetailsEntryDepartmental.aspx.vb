#Region " Imports "
Imports System
Imports System.Web
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
#End Region

Partial Class PromoDetailsEntryDepartmental
    Inherits System.Web.UI.Page

#Region " SQL Validations "

    Private Function validateOtherRequest() As Object

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_ValidateDeptPromoDetails"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4

        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID 'Session("CurrRequestID")

        sqlCmd.Parameters.Add("@PeriodFrom", SqlDbType.DateTime)
        sqlCmd.Parameters("@PeriodFrom").Value = clsSession.PeriodFrom 'Session("PeriodFrom")

        sqlCmd.Parameters.Add("@PeriodTo", SqlDbType.DateTime)
        sqlCmd.Parameters("@PeriodTo").Value = clsSession.PeriodTo 'Session("PeriodTo")

        sqlCmd.Parameters.Add("@DepCode", SqlDbType.Int)
        sqlCmd.Parameters("@DepCode").Value = IIf(txtDep.Text = "", 0, txtDep.Text)

        sqlCmd.Parameters.Add("@SubDepCode", SqlDbType.Int)
        sqlCmd.Parameters("@SubDepCode").Value = IIf(txtSubDp.Text = "", 0, txtSubDp.Text)

        sqlCmd.Parameters.Add("@ClassCode", SqlDbType.Int)
        sqlCmd.Parameters("@ClassCode").Value = IIf(txtClass.Text = "", 0, txtClass.Text)

        ' Added dowcarpio08302012@smretailinc: Additional parameter for sub class code
        sqlCmd.Parameters.Add("@SubClassCode", SqlDbType.Int)
        sqlCmd.Parameters("@SubClassCode").Value = IIf(txtSubClass.Text = "", 0, txtSubClass.Text)

        validateOtherRequest = sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Function

    Private Function validateMarkDown(ByVal flag As String) As Int32
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_ValidateMarkDownExists"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4

        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID 'Session("CurrRequestID")

        sqlCmd.Parameters.Add("@DepCode", SqlDbType.Int)
        sqlCmd.Parameters("@DepCode").Value = IIf(txtDep.Text = "", 0, txtDep.Text)

        sqlCmd.Parameters.Add("@SubDepCode", SqlDbType.Int)
        sqlCmd.Parameters("@SubDepCode").Value = IIf(txtSubDp.Text = "", 0, txtSubDp.Text)

        sqlCmd.Parameters.Add("@ClassCode", SqlDbType.Int)
        sqlCmd.Parameters("@ClassCode").Value = IIf(txtClass.Text = "", 0, txtClass.Text)

        ' Added dowcarpio08302012@smretailinc: Additional parameter for sub class code
        sqlCmd.Parameters.Add("@SubClassCode", SqlDbType.Int)
        sqlCmd.Parameters("@SubClassCode").Value = IIf(txtSubClass.Text = "", 0, txtSubClass.Text)

        sqlCmd.Parameters.Add("@flag", SqlDbType.VarChar)
        sqlCmd.Parameters("@flag").Value = flag

        validateMarkDown = sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Function
    '

    Private Function validateattachmentRequired() As Int32
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_ValidateAttachmentRequired"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4

        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID

        validateattachmentRequired = sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Function


    Private Function validateattachment() As Int32
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_ValidateAttachment"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4

        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID

        validateattachment = sqlCmd.ExecuteScalar()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Function
#End Region

#Region " SQL Insert and Delete "

    Private Sub SavePromoDetails()

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand

        Try

            sqlConn.Open()
            sqlCmd = New SqlCommand
            sqlCmd.CommandText = "USP_InsertDeptPromoDetails"
            sqlCmd.Connection = sqlConn
            sqlCmd.CommandTimeout = 0
            sqlCmd.CommandType = 4
            sqlCmd.Parameters.Add("@PromoDesc", SqlDbType.Text)
            sqlCmd.Parameters("@PromoDesc").Value = litMechanics.Text.Trim
            sqlCmd.Parameters.Add("@PromoTypeID", SqlDbType.Int)
            sqlCmd.Parameters("@PromoTypeID").Value = cboPromoType.SelectedValue
            sqlCmd.Parameters.Add("@PeriodFrom", SqlDbType.DateTime)
            sqlCmd.Parameters("@PeriodFrom").Value = clsSession.PeriodFrom 'Session("PeriodFrom")
            sqlCmd.Parameters.Add("@PeriodTo", SqlDbType.DateTime)
            sqlCmd.Parameters("@PeriodTo").Value = clsSession.PeriodTo 'Session("PeriodTo")
            sqlCmd.Parameters.Add("@PercentDisc", SqlDbType.SmallInt)
            sqlCmd.Parameters("@PercentDisc").Value = 0
            sqlCmd.Parameters.Add("@DiscAmount", SqlDbType.Money)
            sqlCmd.Parameters("@DiscAmount").Value = 0
            sqlCmd.Parameters.Add("@RequestID", SqlDbType.SmallInt)
            sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID 'Session("CurrRequestID")
            sqlCmd.Parameters.Add("@DepCode", SqlDbType.SmallInt)
            sqlCmd.Parameters("@DepCode").Value = txtDep.Text.Trim
            sqlCmd.Parameters.Add("@SubDepCode", SqlDbType.SmallInt)
            sqlCmd.Parameters("@SubDepCode").Value = txtSubDp.Text.Trim
            sqlCmd.Parameters.Add("@ClassCode", SqlDbType.SmallInt)
            sqlCmd.Parameters("@ClassCode").Value = txtClass.Text.Trim
            sqlCmd.Parameters.Add("@SubClassCode", SqlDbType.SmallInt)
            sqlCmd.Parameters("@SubClassCode").Value = txtSubClass.Text.Trim ' Added dowcarpio08302012@smretailinc: Additional parameter for sub class code
            sqlCmd.Parameters.Add("@ShortDesc", SqlDbType.VarChar)
            sqlCmd.Parameters("@ShortDesc").Value = hfShortDesc.Value
            sqlCmd.Parameters.Add("@PercentDiscDept", SqlDbType.SmallInt)
            sqlCmd.Parameters("@PercentDiscDept").Value = IIf(txtDiscount.Text.Trim = "", 0, txtDiscount.Text.Trim)
            sqlCmd.Parameters.Add("@MarkdownFrom", SqlDbType.VarChar)
            sqlCmd.Parameters("@MarkdownFrom").Value = txtMDFrom.Text.Trim
            sqlCmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            ' Added dowcarpio08232012@smretailinc: close and dispose connection
            If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
            sqlConn = Nothing
            sqlCmd = Nothing

            GC.Collect()
        End Try

    End Sub

    Private Sub DeleteRecord(ByVal PromoID As String)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "USP_DeletePromodetails"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4
        sqlCmd.Parameters.Add("@PromoID", SqlDbType.Int)
        sqlCmd.Parameters("@PromoID").Value = PromoID
        sqlCmd.ExecuteNonQuery()

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()
    End Sub

#End Region

#Region " Fill Objects with data "

    Private Sub FillGridView()

        Dim StoredProc As String
        StoredProc = "USP_SelectPromoPromoDetails"

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand(StoredProc, sqlConn)

        Try

            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
            sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID


            Dim da As New SqlDataAdapter(sqlCmd)
            Dim ds As New DataSet
            sqlConn.Open()
            da.Fill(ds, "tbl_PromoPromoDetails")
            Dim dt As DataTable = ds.Tables("tbl_PromoPromoDetails")
            If dt.Rows.Count <> 0 Then
                Session("dsPromoPromoDetails") = dt
                gvPromoDetails.DataSource = dt
                gvPromoDetails.DataBind()
                Me.tblDelete.Visible = True
            Else
                Me.tblDelete.Visible = False
                Session("dsPromoPromoDetails") = dt
                gvPromoDetails.DataSource = Nothing
                gvPromoDetails.DataBind()
            End If
        Catch ex As Exception
        Finally

            ' Added dowcarpio08232012@smretailinc: close and dispose connection
            If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
            sqlConn = Nothing
            sqlCmd = Nothing

            GC.Collect()
        End Try
    End Sub

    Protected Sub gvPromoDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPromoDetails.PageIndexChanging
        Try
            gvPromoDetails.PageIndex = e.NewPageIndex
            gvPromoDetails.DataSource = CType(Session("dsPromoPromoDetails"), DataTable)
            gvPromoDetails.DataBind()
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub fillCboPromotype()
        Try
            cboPromoType.Items.Clear()
            cboPromoType.DataSource = sqldsPromoType
            cboPromoType.DataTextField = "TypeDesc"
            cboPromoType.DataValueField = "PromoTypeID"
            cboPromoType.DataBind()
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub

#End Region

#Region " Page Events "

    Protected Sub cboPromoType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPromoType.SelectedIndexChanged
        DisplayPromoTypeInfo()
    End Sub

    Protected Sub lnkEditMechanics_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkEditMechanics.Click
        hidBox.Value = litMechanics.Text
        clsSession.Mechanics = Server.HtmlDecode(litMechanics.Text.ToString)
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")
        'panPopUp.Visible = True
    End Sub

    'Protected Sub cmdPopOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopOK.Click
    'litMechanics.Text = Request("txtEditor")
    'panPopUp.Visible = False
    'End Sub

    'Protected Sub cmdPopCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopCancel.Click
    'panPopUp.Visible = False
    'End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Me.btnAdd.Text.ToLower = "add" Then
            ' Added dowcarpio08302012@smretailinc: Additional condition for sub class code
            If Me.txtDep.Text <> "" And Me.txtSubDp.Text <> "" And Me.txtClass.Text <> "" And Me.txtSubClass.Text <> "" And Me.lblItemDesc.Text <> "" Then
                SavePromoDetails()
                cmAdd_Click(sender, e)
                Button1_Click(sender, e)
                blistErrorMsg.Items.Clear()
            Else
                blistErrorMsg.Items.Clear()
                blistErrorMsg.Items.Add("No Promotion information.")
            End If
        Else
            UpdatePromoDetails()
            AddMode()
            FillGridView()
            Button1_Click(sender, e)
        End If
        'AttachmentPath = 
        ValidateAttachmentRequired(sender, e)
    End Sub

    Private Sub ValidateAttachmentRequired(ByVal sender As Object, ByVal e As System.EventArgs)
        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")
        lnkattachment.Visible = validateattachment()
        If cboPromoType.SelectedValue > -1 Then

            lnkattachment.Visible = validateattachment()

            If lnkattachment.Visible Then

                Dim f As New IO.FileInfo(clsPromo.pathAttachment & clsSession.CurrRequestID.ToString())

                If Not f.Exists Then
                    clsSession.AttachmentPath = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString()
                    Directory.CreateDirectory(clsSession.AttachmentPath)
                End If
                btnDownload_Click(sender, e)
            End If
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ' Added dowcarpio08302012@smretailinc: Additional attr assignment for sub class code
            txtSubClass.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtClass.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtDep.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtSubDp.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")

            If Request("Filename") <> Nothing Then
                Dim fname As String
                fname = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString & "\" & Request("Filename")
                Response.ContentType = "application/x-msdownload"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" & Request("Filename"))
                Response.TransmitFile(fname)
                Response.End()
            End If

            If Request("DownLoad") <> Nothing And Request("Path") <> Nothing Then
                Dim fname As String
                fname = Request("Path") & ".zip"
                Response.ContentType = "application/x-msdownload"
                Response.AppendHeader("Content-Disposition", "attachment;filename=" & Request("DownLoad"))
                Response.TransmitFile(fname)
                Response.End()
            End If

            Session("TransFlag") = "departmental"
            Me.tblDelete.Visible = False
            lblItemDesc.Attributes.Add("readonly", "readonly")
            fillCboPromotype()


            'Load Request Data
            Dim taPromoRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()
            Dim dtPromoRequests As dsPromotions.PromoRequestsDataTable
            Dim rowPromoRequest As dsPromotions.PromoRequestsRow
            dtPromoRequests = taPromoRequests.GetPromoRequestByID(clsSession.CurrRequestID) 'Session("CurrRequestID"))
            If dtPromoRequests.Rows.Count > 0 Then
                rowPromoRequest = dtPromoRequests.Rows(0)
                With rowPromoRequest
                    Session("PeriodFrom") = .PromoPeriodFrom
                    Session("PeriodTo") = .PromoPeriodTo
                End With
            End If
            FillGridView()

            ' Added dowcarpio09142012@smretailinc: Default and restrict to SMAC deal for all SACI request
            If SystemUser.UserGroupType = "SACI" Then

                cboPromoType.Enabled = False
                cboPromoType.SelectedValue = 48
                DisplayPromoTypeInfo()

            End If

        End If


        ValidateAttachmentRequired(sender, e)
       

    End Sub

    Protected Sub cmAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmAdd.Click
        Try
            txtDiscount.Text = ""
            txtMDFrom.Text = ""
            txtSubDp.Text = ""
            txtClass.Text = ""
            txtSubClass.Text = ""  ' Added dowcarpio08302012@smretailinc: clear sub class code
            txtDep.Text = ""
            lblItemDesc.Text = ""
            hfShortDesc.Value = ""
            cboPromoType.SelectedValue = 0
            litMechanics.Text = ""
            lnkEditMechanics.Enabled = False
            Button1_Click(sender, e)
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub


    Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Try
            For Each row As GridViewRow In gvPromoDetails.Rows
                Dim cb As CheckBox = row.FindControl("CheckBox1")
                If cb IsNot Nothing AndAlso cb.Checked Then
                    DeleteRecord(CType(row.FindControl("label3"), Label).Text)
                End If
            Next

            ' Revised dowcarpio11102012@smretailinc: no branch description indicated in promo request. Delete branch description upon deletion of all promotions.
            If gvPromoDetails.Rows.Count = 1 Then

                sqldsData.UpdateCommand = "UPDATE PromoRequests SET  Branches = NULL WHERE RequestID = 0" & clsSession.CurrRequestID
                sqldsData.Update()

            End If

            FillGridView()
            Button1_Click(sender, e)
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            If validateMarkDown("1") = 1 Then

                txtMDFrom.Enabled = False
                Me.Label5.Text = "Already exists for this item code."

                txtMDFrom.BackColor = Drawing.Color.LightGray
            Else

                txtMDFrom.Enabled = True
                Me.Label5.Text = ""

                txtMDFrom.BackColor = Drawing.Color.White
            End If

            If validateMarkDown("0") = 1 Then
                txtDiscount.Enabled = False
                Me.Label4.Text = "Already exists for this item code."
                txtDiscount.BackColor = Drawing.Color.LightGray
            Else
                txtDiscount.Enabled = True
                Me.Label4.Text = ""
                txtDiscount.BackColor = Drawing.Color.White
            End If

            fillCboPromotype()
            FillGridView()

            ' Added dowcarpio09142012@smretailinc: Default and restrict to SMAC deal for all SACI request
            If SystemUser.UserGroupType = "SACI" Then

                cboPromoType.Enabled = False
                cboPromoType.SelectedValue = 48
                DisplayPromoTypeInfo()

            End If

        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub

    Protected Sub lnkBranches_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBranches.Click
        btnAdd_Click(sender, e)
        blistErrorMsg.Items.Clear()

        If gvPromoDetails.Rows.Count() < 1 Then
            blistErrorMsg.Items.Add("No Promotion information.")
            Exit Sub
        End If

        If validateattachmentRequired() = 1 Then
            If lblFiles.Text = "" Then
                blistErrorMsg.Items.Add("Attachment required.")
                Exit Sub
            End If
        End If

        clsSession.IsDepartmental = 1
        Response.Redirect("PromoBranchesDepartmental.aspx")
    End Sub

    Protected Sub lnkRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRequest.Click
        clsSession.MsgTransFlowFlag = 3
        Response.Redirect("PromoRequestEntry.aspx")
    End Sub
    Protected Sub btnSeacrh_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSeacrh.ServerClick
        'If IsNumeric(txtDep.Text) = True And IsNumeric(txtClass.Text) = True And IsNumeric(txtSubDp.Text) = True Then
        ShowPromoItemInfo()
        Button1_Click(sender, e)
        'End If
    End Sub

    Protected Sub gvPromoDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPromoDetails.RowCommand
        Try
            If e.CommandName.ToLower = "edit" Then
                FillSpecificPromoDetails(e.CommandArgument)
                EditMode()
            End If
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try


            Dim chk As CheckBox
            chk = CType(gvPromoDetails.HeaderRow.FindControl("chkSelectAll"), CheckBox)
            If chk.Checked = True Then
                Dim row As GridViewRow
                For Each row In Me.gvPromoDetails.Rows
                    Dim chkSel As CheckBox
                    chkSel = CType(row.FindControl("CheckBox1"), CheckBox)
                    chkSel.Checked = True
                Next
            Else
                Dim row As GridViewRow
                For Each row In Me.gvPromoDetails.Rows
                    Dim chkSel As CheckBox
                    chkSel = CType(row.FindControl("CheckBox1"), CheckBox)
                    chkSel.Checked = False
                Next
            End If
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub
    Protected Sub gvPromoDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvPromoDetails.RowEditing

    End Sub
#End Region

#Region " Others "

    Private Sub DisplayPromoTypeInfo()
        Try
            Dim taPromoTypes As New dsPromotionsTableAdapters.PromoTypesTableAdapter()
            Dim dtPromoTypes As dsPromotions.PromoTypesDataTable
            Dim trowPromoType As dsPromotions.PromoTypesRow
            'panDiscount.Visible = False
            ' allow editting on mechanics textbox only if there's a valid selection
            If CInt(cboPromoType.SelectedValue) = -1 Then
                litMechanics.Text = ""
                lnkEditMechanics.Enabled = False
            Else
                ' get promo type fields
                dtPromoTypes = taPromoTypes.GetPromoTypeByID(CInt(cboPromoType.SelectedValue))
                trowPromoType = dtPromoTypes.Rows(0)

                '::ToDo:: default mechanics, layout, lead time, etc
                lnkEditMechanics.Enabled = True
                litMechanics.Text = trowPromoType.DefaultMechanics

                'Select Case trowPromoType.LayoutID
                '    Case 1
                '        'class discount - 
                '        'panDiscount.Visible = True
                '    Case 2
                '        'attachment needed
                'End Select
            End If
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub
    Private Sub FillSpecificPromoDetails(ByVal PromoID As Int32)
        Dim StoredProc As String
        StoredProc = "USP_SelectPromoPromoSpecificDetails"
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID 'Session("CurrRequestID")
        sqlCmd.Parameters.Add("@PromoID", SqlDbType.Int)
        sqlCmd.Parameters("@PromoID").Value = PromoID
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_PromoPromoSpecificDetails")
        Dim dt As DataTable = ds.Tables("tbl_PromoPromoSpecificDetails")
        If dt.Rows.Count <> 0 Then
            Title = dt.Rows(0)("PromoTypeID").ToString
            hidPromoTypeID.Value = dt.Rows(0)("PromoTypeID").ToString
            litMechanics.Text = dt.Rows(0)("PromoDesc").ToString

            'lblItemDesc.Text = dt.Rows(0)("PromoDesc")
            'hfShortDesc.Value = dt.Rows(0)("ShortDesc")
            'txtDep.Text = dt.Rows(0)("DepCode").ToString
            'txtClass.Text = dt.Rows(0)("ClassCode").ToString
            'txtSubDp.Text = dt.Rows(0)("SubDepCode").ToString



        Else

        End If
        Session("PromoID") = PromoID
        'Session("PromoID")
        'Session("CurrRequestID")

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        dt = Nothing

        GC.Collect()

    End Sub
    Private Sub EditMode()
        cboPromoType.SelectedValue = 0
        cboPromoType.Enabled = False
        txtMDFrom.Enabled = False

        txtDiscount.Enabled = False
        lnkEditMechanics.Enabled = True
        cboPromoType.BackColor = Drawing.Color.LightGray
        txtMDFrom.BackColor = Drawing.Color.LightGray

        txtDiscount.BackColor = Drawing.Color.LightGray
        btnAdd.Text = "Update"

        gvPromoDetails.Enabled = False
        cmAdd.Enabled = False
        btnSeacrh.Disabled = True
        cmdDelete.Enabled = False
    End Sub
    Private Sub AddMode()
        cboPromoType.SelectedValue = 0
        cboPromoType.Enabled = True
        txtMDFrom.Enabled = True

        txtDiscount.Enabled = True
        lnkEditMechanics.Enabled = True
        cboPromoType.BackColor = Drawing.Color.White
        txtMDFrom.BackColor = Drawing.Color.White

        txtDiscount.BackColor = Drawing.Color.White
        btnAdd.Text = "Add"

        gvPromoDetails.Enabled = True
        cmAdd.Enabled = True
        btnSeacrh.Disabled = False
        cmdDelete.Enabled = True

        litMechanics.Text = ""
    End Sub
    Private Sub UpdatePromoDetails()

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand

        Try

            sqlConn.Open()
            sqlCmd = New SqlCommand
            sqlCmd.CommandText = "USP_UpdatePromoPromoSpecificDetails"
            sqlCmd.Connection = sqlConn
            sqlCmd.CommandTimeout = 0
            sqlCmd.CommandType = 4


            sqlCmd.Parameters.Add("@RequestID", SqlDbType.SmallInt)
            sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID 'Session("CurrRequestID")

            sqlCmd.Parameters.Add("@PromoID", SqlDbType.SmallInt)
            sqlCmd.Parameters("@PromoID").Value = Session("PromoID")

            sqlCmd.Parameters.Add("@PromoDesc", SqlDbType.Text)
            sqlCmd.Parameters("@PromoDesc").Value = litMechanics.Text.Trim
            sqlCmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally

            ' Added dowcarpio08232012@smretailinc: close and dispose connection
            If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
            sqlConn = Nothing
            sqlCmd = Nothing

            GC.Collect()
        End Try
    End Sub
    Private Sub ShowPromoItemInfo()
        Try




            Dim taCategories As New dsPromotionsTableAdapters.DepSdepClassTableAdapter()
            Dim dtblCategories As New dsPromotions.DepSdepClassDataTable
            Dim trowCategory As dsPromotions.DepSdepClassRow

            ' force zero value for blank entries
            If txtDep.Text = "" Then txtDep.Text = "0"
            If txtSubDp.Text = "" Then txtSubDp.Text = "0"
            If txtClass.Text = "" Then txtClass.Text = "0"
            If txtSubClass.Text = "" Then txtSubClass.Text = "0" ' Added dowcarpio08302012@smretailinc: Additional condition for sub class code

            taCategories.GetCategoriesByCodeSP(dtblCategories, "", txtDep.Text, txtSubDp.Text, txtClass.Text, txtSubClass.Text, SystemUser.UserID)

            If dtblCategories.Rows.Count = 0 Then
                txtDep.Text = ""
                txtSubDp.Text = ""
                txtClass.Text = ""
                txtSubClass.Text = "" ' Added dowcarpio08302012@smretailinc: Additional condition for sub class code
                hfShortDesc.Value = ""
                lblItemDesc.Text = ""

                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openSearch();</script>")
            Else
                trowCategory = dtblCategories.Rows(0)
                lblItemDesc.Text = trowCategory.Description
                hfShortDesc.Value = trowCategory.ShortDesc
            End If

        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub

#End Region


    Protected Sub lnkattachment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkattachment.Click
        ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>openAttachment();</script>")
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Try
            Dim tr As Boolean = False
            Dim strDir As String
            Dim strFiles As String
            strFiles = ""
            'strDir = clsPromo.pathAttachment &  clsSession.CurrRequestID.ToString
            strDir = clsPromo.pathAttachment & clsSession.CurrRequestID.ToString

            'Dim di As New System.IO.DirectoryInfo(strDir)
            lblFiles.Text = ""
            If (System.IO.Directory.Exists(strDir)) And validateattachment() = 1 Then
                Dim dir As New System.IO.DirectoryInfo(strDir)
                Dim files As System.IO.FileInfo() = dir.GetFiles()
                For Each file As System.IO.FileInfo In files
                    tr = True
                    strFiles = strFiles & file.Name.ToString & ","
                    lblFiles.Text &= "<img src='Images/bullet green.gif' /><a href='PromoDetailsEntryDepartmental.aspx?FileName=" & file.Name.ToString & "'>" & file.Name.ToString & "</a> "
                Next
            Else
                If Directory.Exists(strDir) Then
                    Dim s As String
                    For Each s In System.IO.Directory.GetFiles(strDir)
                        System.IO.File.Delete(s)
                    Next s
                    Directory.Delete(strDir)
                    'Kill(strDir)
                    'di.Delete(True)
                End If
            End If
            If Len(strFiles) <> 0 Then
                'litResult.Text &= "<a href='POSscreen.aspx?DbName=" & NewDBFname & "'>" & NewDBFname.ToUpper() & ".DBF</a><br>"
                'lblFiles.Text &= "<a href='PromoDetailsEntryDepartmental.aspx?FileName=" & Left(strFiles, Len(strFiles) - 1) & "'>" & Left(strFiles, Len(strFiles) - 1) & "</a><br>"
                'Left(strFiles, Len(strFiles) - 1)
            Else
                lblFiles.Text = ""
            End If
            imgbtnDownload.Visible = tr
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub

    Protected Sub imgbtnDownload_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnDownload.Click
        clsPromo.CreateZipFile(clsPromo.pathAttachment & clsSession.CurrRequestID.ToString, 1)
        Response.Redirect("PromoDetailsEntryDepartmental.aspx?DownLoad=" & clsSession.CurrRequestID.ToString & ".zip" & "&Path=" & clsPromo.pathAttachment & "\" & clsSession.CurrRequestID.ToString)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            litMechanics.Text = hidBox.Value
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub



    Protected Sub gvPromoDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPromoDetails.RowDataBound
        Try
            Const nCol As Integer = 3
            Static rowPrevious As GridViewRow

            ' html decode promo description field in order to display properly
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(nCol).Text = "<strong>" & CType(e.Row.FindControl("lblItemCode"), Label).Text & "</strong>"

                ' Added dowcarpio08242012@smretailinc: Decode html  to display properly.
                e.Row.Cells(nCol + 1).Text = Server.HtmlDecode(CType(e.Row.FindControl("Literal2"), Literal).Text)

            End If


            If e.Row.RowIndex = 0 Then rowPrevious = e.Row

            If e.Row.RowIndex > 0 Then

                ' merge Description cells with same PromoID
                Dim lbPrev As New Label
                Dim lbCurr As New Label

                lbPrev = CType(rowPrevious.FindControl("lblItemCode"), Label)
                lbCurr = CType(e.Row.FindControl("lblItemCode"), Label)

                If lbCurr.Text = lbPrev.Text Then

                    If rowPrevious.Cells(nCol).RowSpan < 2 Then
                        'rowPrevious.Cells(0).RowSpan = 2
                        rowPrevious.Cells(nCol).RowSpan = 2
                    Else
                        'rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
                        rowPrevious.Cells(nCol).RowSpan = rowPrevious.Cells(nCol).RowSpan + 1
                    End If

                    'e.Row.Cells(0).Visible = False
                    e.Row.Cells(nCol).Visible = False
                Else
                    rowPrevious = e.Row
                End If

            End If
        Catch ex As Exception
        Finally
            GC.Collect()
        End Try
    End Sub
End Class
