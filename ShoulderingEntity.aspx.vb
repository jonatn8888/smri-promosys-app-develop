Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class ShoulderingEntity
    Inherits System.Web.UI.Page

    Private ReadOnly ConnStr As String = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString

    Private Property dtRows() As DataTable
        Get
            Return TryCast(ViewState("dtRows"), DataTable)
        End Get
        Set(ByVal value As DataTable)
            ViewState("dtRows") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then
            InitializeGrid()
        End If
    End Sub

    '--- Initial Load
    Private Sub InitializeGrid()
        Dim requestId As String = Request("RequestID")
        Dim DefaultVal As String = String.Empty
        Dim dtExisting As DataTable = GetExistingShoulderingRows(requestId)

        If dtExisting IsNot Nothing AndAlso dtExisting.Rows.Count > 0 Then
            dtRows = dtExisting
        Else

            If clsSession.PromoTypeID = "18" Then
                DefaultVal = "760"
            End If

            Dim dt As New DataTable()
            dt.Columns.Add("shoulderingentityid", GetType(String))
            dt.Columns.Add("Percent", GetType(String))
            dt.Columns.Add("MainSelection", GetType(String))
            dt.Columns.Add("OtherBU", GetType(String))
            dt.Columns.Add("FinancingSelection", GetType(String))
            dt.Columns.Add("FinancialPartnerSubType", GetType(String))
            dt.Columns.Add("FinancialPartnerSelection", GetType(String))
            dt.Rows.Add("", "", DefaultVal, "", "", "", "")
            dtRows = dt
        End If

        BindGrid()
    End Sub


    '--- Bind the grid
    Private Sub BindGrid()
        gvRows.DataSource = dtRows
        gvRows.DataBind()
    End Sub

    '--- Load dropdowns per row
    Protected Sub gvRows_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblID As Label = CType(e.Row.FindControl("lblID"), Label)
            Dim ddlMain As DropDownList = CType(e.Row.FindControl("ddlMain"), DropDownList)
            Dim ddlOtherBU As DropDownList = CType(e.Row.FindControl("ddlOtherBU"), DropDownList)
            Dim ddlFinancing As DropDownList = CType(e.Row.FindControl("ddlFinancing"), DropDownList)
            Dim ddlFinancialPartnerSubType As DropDownList = CType(e.Row.FindControl("ddlFinancialPartnerSubType"), DropDownList)
            Dim ddlFinancialPartner As DropDownList = CType(e.Row.FindControl("ddlFinancialPartner"), DropDownList)
            Dim txtPercent As TextBox = CType(e.Row.FindControl("txtPercent"), TextBox)

            'Load main dropdown
            LoadMainDropdown(ddlMain)

            'Load Financing dropdown
            ddlFinancing.Items.Clear()
            Dim arr As New ArrayList()

            arr.Add(New ListItem("Bank", "1"))
            arr.Add(New ListItem("Consumer Financing", "3"))
            arr.Add(New ListItem("eWallet Partners", "2"))

            ' Sort by Text
            arr.Sort(New ListItemTextComparer())

            ddlFinancing.Items.Clear()
            ddlFinancing.Items.Add(New ListItem("--Select--", ""))

            For Each item As ListItem In arr
                ddlFinancing.Items.Add(item)
            Next


            ddlFinancialPartnerSubType.Items.Clear()
            ddlFinancialPartnerSubType.Items.Add(New ListItem("--Select--", ""))
            ddlFinancialPartnerSubType.Items.Add(New ListItem("Card Brand", "1"))
            ddlFinancialPartnerSubType.Items.Add(New ListItem("Bank Partners", "2"))

            'Restore saved values from ViewState
            Dim dt As DataTable = dtRows
            If e.Row.RowIndex < dt.Rows.Count Then
                lblID.Text = dt.Rows(e.Row.RowIndex)("shoulderingentityid").ToString()
                txtPercent.Text = dt.Rows(e.Row.RowIndex)("Percent").ToString()
                ddlMain.SelectedValue = dt.Rows(e.Row.RowIndex)("MainSelection").ToString()
                ddlOtherBU.SelectedValue = dt.Rows(e.Row.RowIndex)("OtherBU").ToString()
                'ddlFinancialPartnerSubType.SelectedValue = dt.Rows(e.Row.RowIndex)("FinancialPartnerSubType").ToString()
                ddlFinancing.SelectedValue = dt.Rows(e.Row.RowIndex)("FinancingSelection").ToString()
                '5:33 Pm rbs7281

                ddlFinancing.Visible = (ddlMain.SelectedItem.Text = "Financial Partners")

                ddlFinancialPartnerSubType.Visible = (ddlFinancing.SelectedItem.Text = "Bank")

                ddlFinancialPartner.Items.Clear()
                ddlFinancialPartner.Items.Add(New ListItem("--Select--", ""))

                Dim savedSubType As String = dt.Rows(e.Row.RowIndex)("FinancialPartnerSubType").ToString()
                If ddlFinancialPartnerSubType.Items.FindByValue(savedSubType) IsNot Nothing Then
                    ddlFinancialPartnerSubType.SelectedValue = savedSubType
                End If


                If ddlMain.SelectedValue <> "" Then
                    ddlOtherBU.Visible = (ddlMain.SelectedItem.Text = "MBU/RA")


                    Dim dtBU As DataTable = GetBizUnitDropdowns()
                    If dtBU.Rows.Count > 0 Then
                        For Each dr As DataRow In dtBU.Rows
                            ddlOtherBU.Items.Add(New ListItem(dr("Description").ToString(), dr("GroupID").ToString()))
                        Next
                    Else
                        ddlOtherBU.Items.Add(New ListItem("--No records found--", ""))
                    End If

                End If

                If ddlFinancing.SelectedValue <> "" Then
                    Dim dtFP As DataTable = GetFinancialPartner(ddlFinancing.SelectedValue, ddlFinancialPartnerSubType.SelectedValue)
                    'Dim dtFPS As DataTable = GetFinancialPartner(ddlFinancing.SelectedValue, ddlFinancialPartnerSubType.SelectedValue)

                    If dtFP.Rows.Count > 0 Then
                        For Each dr As DataRow In dtFP.Rows
                            ddlFinancialPartner.Items.Add(New ListItem(dr("ElementName").ToString(), dr("ElementValue").ToString()))
                        Next
                    Else
                        ddlFinancialPartner.Items.Add(New ListItem("--No records found--", ""))
                    End If

                End If

                ddlFinancing.SelectedValue = dt.Rows(e.Row.RowIndex)("FinancingSelection").ToString()

                Dim otherBUValue As String = dt.Rows(e.Row.RowIndex)("OtherBU").ToString()
                If ddlOtherBU.Items.FindByValue(otherBUValue) IsNot Nothing Then
                    ddlOtherBU.SelectedValue = otherBUValue
                Else
                    ddlOtherBU.SelectedIndex = 0  ' "--Select--"
                End If
                ddlOtherBU.Visible = (ddlMain.SelectedItem.Text = "MBU/RA")

                ddlFinancialPartnerSubType.SelectedValue = dt.Rows(e.Row.RowIndex)("FinancialPartnerSubType").ToString()
                ddlFinancialPartner.SelectedValue = dt.Rows(e.Row.RowIndex)("FinancialPartnerSelection").ToString()
                ddlFinancialPartner.Visible = ddlFinancing.Visible
            End If
        End If
    End Sub

    '--- Load the main dropdown values from SP
    Private Sub LoadMainDropdown(ByVal ddl As DropDownList)
        Using conn As New SqlConnection(ConnStr)
            Using cmd As New SqlCommand("sp_GetDropDownItems", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@RequestID", clsSession.CurrRequestID)
                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                ddl.DataSource = reader
                ddl.DataTextField = "DisplayName"
                ddl.DataValueField = "Code"
                ddl.DataBind()
            End Using
        End Using
        ddl.Items.Insert(0, New ListItem("--Select--", ""))
    End Sub

    Private Function GetBizUnitDropdowns() As DataTable

        Dim dt As New DataTable()

        Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString)
            Using cmd As New SqlCommand("USP_SelectUserGroupsBizUnit", conn)
                cmd.CommandType = CommandType.StoredProcedure
                'cmd.Parameters.AddWithValue("@Partner", partnerCode)

                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Private Function GetFinancialPartnerValues(ByVal partnerCode As String, ByVal DropDownType As String) As DataTable

        Dim dt As New DataTable()

        Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString)
            Using cmd As New SqlCommand("SP_GetFinancialPartner", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Partner", partnerCode)
                cmd.Parameters.AddWithValue("@DropDownType", DropDownType)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    '--- Save all current grid values to ViewState
    Private Sub SaveGridToDataTable()
        Dim dt As DataTable = dtRows
        dt.Rows.Clear()

        For Each row As GridViewRow In gvRows.Rows
            Dim lblID As Label = CType(row.FindControl("lblID"), Label)
            Dim txtPercent As TextBox = CType(row.FindControl("txtPercent"), TextBox)
            Dim ddlMain As DropDownList = CType(row.FindControl("ddlMain"), DropDownList)
            Dim ddlOtherBU As DropDownList = CType(row.FindControl("ddlOtherBU"), DropDownList)
            Dim ddlFinancing As DropDownList = CType(row.FindControl("ddlFinancing"), DropDownList)
            Dim ddlFinancialPartnerSubType As DropDownList = CType(row.FindControl("ddlFinancialPartnerSubType"), DropDownList)
            Dim ddlFinancialPartner As DropDownList = CType(row.FindControl("ddlFinancialPartner"), DropDownList)

            Dim ShoulEntityID As String = IIf(lblID.Text <> "0", lblID.Text, "0")
            Dim percent As String = txtPercent.Text.Trim()
            Dim mainSel As String = ddlMain.SelectedValue
            Dim otherBU As String = ddlOtherBU.SelectedValue
            Dim financeSel As String = If(ddlFinancing.SelectedValue = "", "0", ddlFinancing.SelectedValue)
            Dim CardBrand As String = If(ddlFinancialPartnerSubType.SelectedValue = "", "0", ddlFinancialPartnerSubType.SelectedValue)
            Dim financialPartnerSel As String = If(ddlFinancialPartner.SelectedValue = "", "0", ddlFinancialPartner.SelectedValue)

            dt.Rows.Add(ShoulEntityID, percent, mainSel, otherBU, financeSel, CardBrand, financialPartnerSel)
        Next

        dtRows = dt
    End Sub

    '--- Add Row Button
    Protected Sub btnAddRow_Click(ByVal sender As Object, ByVal e As EventArgs)
        SaveGridToDataTable()

        Dim dt As DataTable = dtRows
        If dt.Rows.Count >= 3 Then
            lblResult.Text = "You can only have up to 3 rows."
            Exit Sub
        End If


        Dim requestId As String = Request("RequestID")
        Dim dtExisting As DataTable = GetExistingShoulderingRows(requestId)

        If dtExisting.Rows.Count > 0 Then
            dt.Rows.Add("", "", "", "", "", "", "")
        Else
            dt.Rows.Add("", "", "", "", "", "")
        End If

        dtRows = dt
        BindGrid()
    End Sub

    '--- Remove Row Button
    Protected Sub gvRows_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvRows.RowCommand
        If e.CommandName = "RemoveRow" Then
            SaveGridToDataTable()

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim dt As DataTable = dtRows

            If dt.Rows.Count > 1 Then
                dt.Rows.RemoveAt(index)
            End If

            dtRows = dt
            BindGrid()
        End If
    End Sub

    Protected Sub ddlMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddl As DropDownList = CType(sender, DropDownList)
        Dim row As GridViewRow = CType(ddl.NamingContainer, GridViewRow)
        Dim ddlFinancing As DropDownList = CType(row.FindControl("ddlFinancing"), DropDownList)
        Dim ddlFinancialPartnerSubType As DropDownList = CType(row.FindControl("ddlFinancialPartnerSubType"), DropDownList)
        Dim ddlFinancialPartner As DropDownList = CType(row.FindControl("ddlFinancialPartner"), DropDownList)

        ddlFinancing.Visible = (ddl.SelectedItem.Text = "Financial Partners")
        ddlFinancialPartner.Visible = ddlFinancing.Visible

        ddlFinancialPartnerSubType.Visible = (ddlFinancing.SelectedItem.Text = "Bank")

        ' Clear selections when hidden
        If Not ddlFinancing.Visible Then
            ddlFinancing.SelectedIndex = 0
            ddlFinancialPartner.SelectedIndex = 0
            'ddlFinancialPartnerSubType.SelectedIndex = 0
        End If

        SaveGridToDataTable()
        BindGrid() ' Refresh row so RowDataBound reloads FinancialPartner
    End Sub

    Protected Sub ddlFinancingType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddlFinancing As DropDownList = CType(sender, DropDownList)
        Dim row As GridViewRow = CType(ddlFinancing.NamingContainer, GridViewRow)
        Dim ddlFinancialPartnerSubType As DropDownList = CType(row.FindControl("ddlFinancialPartnerSubType"), DropDownList)
        Dim ddlFinancialPartner As DropDownList = CType(row.FindControl("ddlFinancialPartner"), DropDownList)
        Dim selectedValue As String = ddlFinancing.SelectedValue

        ddlFinancialPartner.Items.Clear()
        ddlFinancialPartner.Items.Add(New ListItem("--Select--", ""))

        If Not String.IsNullOrEmpty(selectedValue) Then
            If selectedValue = 1 Then
                ddlFinancialPartnerSubType.Visible = True
                ddlFinancialPartnerSubType.Items.Clear()
                ddlFinancialPartnerSubType.Items.Add(New ListItem("--Select--", ""))
                ddlFinancialPartnerSubType.Items.Add(New ListItem("Card Brand", "1"))
                ddlFinancialPartnerSubType.Items.Add(New ListItem("Bank Partners", "2"))
                ddlFinancialPartner.Items.Clear()
            Else
                ddlFinancialPartnerSubType.Items.Clear()
                ddlFinancialPartnerSubType.Visible = False

                Dim dt As DataTable = GetFinancialPartner(selectedValue, "2")
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        ddlFinancialPartner.Items.Add(New ListItem(dr("ElementName").ToString(), dr("ElementValue").ToString()))
                    Next
                Else
                    ddlFinancialPartner.Items.Add(New ListItem("--No records found--", ""))
                End If

            End If
        Else
            ddlFinancialPartnerSubType.Items.Clear()
            ddlFinancialPartnerSubType.Items.Add(New ListItem("--Select--", ""))
            ddlFinancialPartnerSubType.Items.Add(New ListItem("Card Brand", "1"))
            ddlFinancialPartnerSubType.Items.Add(New ListItem("Bank Partners", "2"))
            ddlFinancialPartner.Items.Clear()
        End If


        ddlFinancialPartner.Visible = ddlFinancing.Visible
        SaveGridToDataTable()
    End Sub

    '--- Get financial partner data from SP
    Private Function GetFinancialPartner(ByVal partnerCode As String, ByVal DropDownType As String) As DataTable
        Dim dt As New DataTable()

        Using conn As New SqlConnection(ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString)
            Using cmd As New SqlCommand("SP_GetFinancialPartner", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Partner", partnerCode)
                cmd.Parameters.AddWithValue("@DropDownType", DropDownType)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function



    '--- Save each row using ShoulderingEntity_Update stored procedure
    Private Sub SaveRowsToDatabaseUsingSP()
        SaveGridToDataTable() ' Ensure ViewState is up to date

        Using conn As New SqlConnection(ConnStr)
            conn.Open()

            Using tran As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim joinRequestID As String = String.Empty
                    Dim lblID As New Label()

                    For Each ReqIDrow As GridViewRow In gvRows.Rows
                        lblID = CType(ReqIDrow.FindControl("lblID"), Label)
                        joinRequestID &= lblID.Text & ","
                    Next

                    For Each row As GridViewRow In gvRows.Rows
                        lblID = CType(row.FindControl("lblID"), Label)
                        Dim txtPercent As TextBox = CType(row.FindControl("txtPercent"), TextBox)
                        Dim ddlMain As DropDownList = CType(row.FindControl("ddlMain"), DropDownList)
                        Dim ddlOtherBU As DropDownList = CType(row.FindControl("ddlOtherBU"), DropDownList)
                        Dim ddlFinancing As DropDownList = CType(row.FindControl("ddlFinancing"), DropDownList)
                        Dim ddlFinancialPartner As DropDownList = CType(row.FindControl("ddlFinancialPartner"), DropDownList)
                        Dim ddlFinancialPartnerSubType As DropDownList = CType(row.FindControl("ddlFinancialPartnerSubType"), DropDownList)
                        Dim RequestID As String = Request("RequestID").ToString()


                        Using cmd As New SqlCommand("ShoulderingEntity_Update", conn, tran)
                            cmd.CommandType = CommandType.StoredProcedure

                            '--- Parameters
                            cmd.Parameters.AddWithValue("@shoulderingentityid", lblID.Text) ' or existing ID if editing
                            cmd.Parameters.AddWithValue("@RequestID", RequestID) ' replace with actual promo ID
                            cmd.Parameters.AddWithValue("@shoulderingentity_resourceid", IIf(ddlMain.Visible, ddlMain.SelectedValue, DBNull.Value)) ' adjust if needed
                            cmd.Parameters.AddWithValue("@percentage", If(Decimal.TryParse(txtPercent.Text.Trim(), Nothing), Convert.ToDecimal(txtPercent.Text.Trim()), 0))
                            cmd.Parameters.AddWithValue("@financialpartnertype_resourceid", IIf(ddlFinancing.Visible, ddlFinancing.SelectedValue, DBNull.Value))
                            cmd.Parameters.AddWithValue("@specificpartner_rowid", IIf(ddlFinancialPartner.Visible, ddlFinancialPartner.SelectedValue, DBNull.Value))
                            cmd.Parameters.AddWithValue("@groupID", IIf(ddlOtherBU.Visible, ddlOtherBU.SelectedValue, DBNull.Value))
                            cmd.Parameters.AddWithValue("@FinancialPartnerSubType", IIf(ddlFinancialPartnerSubType.Visible, ddlFinancialPartnerSubType.SelectedValue, DBNull.Value))
                            cmd.Parameters.AddWithValue("@EntityList", joinRequestID)
                            cmd.ExecuteNonQuery()
                        End Using
                    Next


                    'SP Saving Point
                    tran.Commit()
                    lblResult.Text = "Rows saved successfully using stored procedure."
                    lblResult.ForeColor = Drawing.Color.Green
                    ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.shoulderentityeditorwindow.hide();</script>")

                Catch ex As Exception
                    tran.Rollback()
                    lblResult.Text = "Error saving rows: " & ex.Message
                    lblResult.ForeColor = Drawing.Color.Red
                End Try
            End Using
        End Using
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        ' First, run the validation logic
        If Not ValidateRows() Then
            Exit Sub ' Stop saving if validation fails
        End If

        ' If validation passes, save data to DB
        SaveRowsToDatabaseUsingSP()

        'ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.shoulderentityeditorwindow.hide();</script>")
    End Sub


    '--- Get saved shouldering rows from DB if existing
    Private Function GetExistingShoulderingRows(ByVal RequestID As String) As DataTable
        Dim ReqID = " "
        ReqID = If(RequestID, "0")

        Dim dt As New DataTable()

        Using conn As New SqlConnection(ConnStr)
            Using cmd As New SqlCommand("ShoulderingEntity_GetByRequestID", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@RequestID", ReqID)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Private Function ValidateRows() As Boolean
        lblResult.Text = ""
        lblResult.ForeColor = Drawing.Color.Black

        SaveGridToDataTable() ' Make sure we have the latest data

        Dim totalPercent As Decimal = 0
        Dim count As Integer = gvRows.Rows.Count
        Dim MoveForward As Boolean = False

        ' --- Validate number of rows
        If count < 1 OrElse count > 3 Then
            lblResult.Text = "Number of rows must be between 1 and 3."
            lblResult.ForeColor = Drawing.Color.Red
            Return False
        End If

        ' --- Check for duplicate categories
        ' --- Check for duplicate Category + Financing combination
        Dim combinations As New List(Of String)()

        For Each row As GridViewRow In gvRows.Rows
            Dim ddlMain As DropDownList = CType(row.FindControl("ddlMain"), DropDownList)
            Dim ddlFinancing As DropDownList = CType(row.FindControl("ddlFinancing"), DropDownList)
            Dim ddlFinancialPartnerSubType As DropDownList = CType(row.FindControl("ddlFinancialPartnerSubType"), DropDownList)
            Dim ddlFinancialPartner As DropDownList = CType(row.FindControl("ddlFinancialPartner"), DropDownList)

            Dim category As String = ddlMain.SelectedValue.Trim()
            Dim financing As String = ddlFinancing.SelectedValue.Trim()
            Dim FinancialPartnerSubType As String = ddlFinancialPartnerSubType.SelectedValue.Trim()
            Dim FinancialPartner As String = ddlFinancialPartner.SelectedValue.Trim()

            ' --- Basic validation
            If String.IsNullOrEmpty(category) Then
                lblResult.Text = "Please select a category for each row."
                lblResult.ForeColor = Drawing.Color.Red
                Return False
            End If

            ' If financing is hidden or not applicable, normalize it
            If Not ddlFinancing.Visible OrElse String.IsNullOrEmpty(financing) Then
                financing = "0"
            End If

            ' --- Create composite key
            Dim key As String = category & "|" & financing & "|" & FinancialPartnerSubType & "|" & FinancialPartner

            ' --- Check duplicates
            If combinations.Contains(key) Then
                lblResult.Text = "Duplicate category with the same financing is not allowed."
                lblResult.ForeColor = Drawing.Color.Red
                Return False
            Else
                combinations.Add(key)
            End If

            'Mantis#69502 Able to submit promo despite financing partner, selection 1 and selection 2 are blank 
            If ddlMain.SelectedValue.ToString() = "857" Or ddlMain.SelectedValue.ToString() = "860" Then
                If ddlFinancing.SelectedItem.ToString() = "Bank" And (ddlFinancialPartner.Text = "" And ddlFinancialPartnerSubType.Text = "") Then
                    lblResult.Text = "Required field must not be blank."
                    lblResult.ForeColor = Drawing.Color.Red
                    Return False
                ElseIf ddlFinancing.SelectedItem.ToString() <> "Bank" And ddlFinancialPartner.Text = "" Then
                    lblResult.Text = "Required field must not be blank."
                    lblResult.ForeColor = Drawing.Color.Red
                    Return False
                End If
            Else
                'Return True
            End If
        Next


        ' --- Validate percentage total
        For Each row As GridViewRow In gvRows.Rows
            Dim txt As TextBox = CType(row.FindControl("txtPercent"), TextBox)
            Dim val As Decimal

            If txt.Text = "0.00" Then
                lblResult.Text = "0 is not acceptable input for percentage"
                lblResult.ForeColor = Drawing.Color.Red
                Return False
            End If

            If Decimal.TryParse(txt.Text, val) Then
                totalPercent += val
            Else
                lblResult.Text = "Invalid percentage input. Please enter valid numbers."
                lblResult.ForeColor = Drawing.Color.Red
                Return False
            End If
        Next

        ' --- Must equal exactly 100%
        If totalPercent <> 100 Then
            lblResult.Text = "Total percentage must be exactly 100% (Current total: " & totalPercent & "%)"
            lblResult.ForeColor = Drawing.Color.Red
            Return False
        End If

        ' --- All checks passed
        lblResult.Text = "Validation successful. Saving data..."
        lblResult.ForeColor = Drawing.Color.Green
        Return True
    End Function



    Protected Sub ddlOtherBU_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub ddlFinancialPartnerSubType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Test
        Dim ddlFinancialPartnerSubType As DropDownList = CType(sender, DropDownList)
        Dim row As GridViewRow = CType(ddlFinancialPartnerSubType.NamingContainer, GridViewRow)
        Dim ddlFinancing As DropDownList = CType(row.FindControl("ddlFinancing"), DropDownList)
        Dim ddlFinancialPartner As DropDownList = CType(row.FindControl("ddlFinancialPartner"), DropDownList)
        Dim finSelectedValue As String = ddlFinancing.SelectedValue

        Dim subType As String = ddlFinancialPartnerSubType.SelectedValue

        If Not String.IsNullOrEmpty(subType) AndAlso subType <> "0" Then
            ddlFinancialPartner.Items.Clear()
            Dim dt2 As DataTable = GetFinancialPartner(finSelectedValue, subType)
            If dt2.Rows.Count > 0 Then
                ddlFinancialPartnerSubType.Visible = (ddlFinancing.SelectedItem.Text = "Bank")
                For Each dr2 As DataRow In dt2.Rows
                    ddlFinancialPartner.Items.Add(New ListItem(dr2("ElementName").ToString(), dr2("ElementValue").ToString()))
                Next
            Else
                ddlFinancialPartner.Items.Add(New ListItem("--No records found--", ""))
            End If
        Else
            ddlFinancialPartner.Items.Clear()
        End If

    End Sub

    Protected Sub ddlFinancialPartner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

End Class

Public Class ListItemTextComparer
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements IComparer.Compare

        Dim a As ListItem = CType(x, ListItem)
        Dim b As ListItem = CType(y, ListItem)

        Return String.Compare(a.Text, b.Text)
    End Function
End Class
