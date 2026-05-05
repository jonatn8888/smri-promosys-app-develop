

Imports System.Data
Imports System.Web.UI.ControlCollection

Partial Class BinRange
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            sqlDSPromoBinRange.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
            FillRangeTypeDropDownList(cboXML_RangeType)
            FillCardBrandDropDownList(cbo_CardBrand)
            FillIssuerDropDownList(cbo_Issuer)
        End If
    End Sub

    Private Function SaveBinRange() As Boolean
        blistErrorMsg.Items.Clear()
        SaveBinRange = True
        If chkXML_IsManual.Checked Then

            If cboXML_RangeType.SelectedValue = -1 Then
                blistErrorMsg.Items.Add("Range Type not specified.")
                SaveBinRange = False
            End If
            If String.IsNullOrEmpty(txtBin.Text) Then
                blistErrorMsg.Items.Add("BIN is required.")
                SaveBinRange = False
            End If
            If String.IsNullOrEmpty(txtPanLow.Text) Then
                blistErrorMsg.Items.Add("Pan Low is required.")
                SaveBinRange = False
            End If
            If String.IsNullOrEmpty(txtPanHigh.Text) Then
                blistErrorMsg.Items.Add("Pan High is required.")
                SaveBinRange = False
            End If
        Else
            If cboXML_RangeType.SelectedValue = -1 And cboXML_RangeType.SelectedItem.Text = "- - Select Value - -" Then
                blistErrorMsg.Items.Add("Range Type not specified.")
            End If
            If cbo_Issuer.SelectedValue = -1 And cbo_Issuer.SelectedItem.Text = "- - Select Value - -" Then
                blistErrorMsg.Items.Add("Issuer not specified.")
            End If
            If cbo_CardBrand.SelectedValue = -1 And cbo_CardBrand.SelectedItem.Text = "- - Select Value - -" Then
                blistErrorMsg.Items.Add("Card Bank not specified.")
            End If
        End If
        
    End Function

    Protected Sub cmdPopOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopOK.Click
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.binrangeditorwindow.hide();</script>")
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        For Each row As GridViewRow In gvBinRange.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing And cb.Checked Then
                'confirm deletion
                With sqlDSPromoBinRange.DeleteParameters
                    .Item("RequestID").DefaultValue = clsSession.CurrRequestID
                    .Item("RangeType").DefaultValue = row.Cells(1).Text.Replace("&nbsp;", "")
                    .Item("BIN").DefaultValue = row.Cells(2).Text.Replace("&nbsp;", "")
                    .Item("PanLow").DefaultValue = row.Cells(3).Text.Replace("&nbsp;", "")
                    .Item("PanHigh").DefaultValue = row.Cells(4).Text.Replace("&nbsp;", "")
                    .Item("PanLength").DefaultValue = row.Cells(5).Text.Replace("&nbsp;", "0")
                End With
                sqlDSPromoBinRange.Delete()
            End If
        Next
    End Sub

    Protected Sub linkBinRange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkBinRange.Click
        If SaveBinRange() Then
            If chkXML_IsManual.Checked Then
                With sqlDSPromoBinRange.InsertParameters
                    .Item("RequestID").DefaultValue = clsSession.CurrRequestID
                    .Item("RangeType").DefaultValue = cboXML_RangeType.SelectedValue
                    .Item("BIN").DefaultValue = txtBin.Text
                    .Item("PanLow").DefaultValue = txtPanLow.Text
                    .Item("PanHigh").DefaultValue = txtPanHigh.Text
                    .Item("PanLength").DefaultValue = txtPanLength.Text
                End With
                sqlDSPromoBinRange.Insert()
            Else
                SaveBinRangeBulk()
            End If


            txtBin.Text = ""
            txtPanLow.Text = ""
            txtPanHigh.Text = ""
            txtPanLength.Text = ""
        End If
    End Sub

    Private Sub SaveBinRangeBulk()
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As Data.SqlClient.SqlCommand
        sqlConn.Open()
        sqlCmd = New Data.SqlClient.SqlCommand
        sqlCmd.CommandText = "sp_InsertPromoBinRangeBulk"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4

        sqlCmd.Parameters.Add("@Issuer", SqlDbType.VarChar)
        sqlCmd.Parameters("@Issuer").Value = cbo_Issuer.SelectedItem.Text.Replace("-- ALL --", "ALL")

        sqlCmd.Parameters.Add("@CardBank", SqlDbType.VarChar)
        sqlCmd.Parameters("@CardBank").Value = cbo_CardBrand.SelectedItem.Text.Replace("-- ALL --", "ALL")

        sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar)
        sqlCmd.Parameters("@Type").Value = cboXML_RangeType.SelectedItem.Text.Replace("-- ALL --", "ALL")

        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = clsSession.CurrRequestID


        sqlCmd.ExecuteNonQuery()

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()

        sqlConn = Nothing
        sqlCmd = Nothing

        gvBinRange.DataBind()

    End Sub

    Private Sub FillRangeTypeDropDownList(ByRef cboListObj As DropDownList)

        Dim dtTable As New DataTable
        Dim strQuery As String = ""


        strQuery = "SELECT  ROW_NUMBER() OVER (ORDER BY Type) AS ElementValue, TYPE AS ElementName  " & _
                    "FROM PromoBinRangeMasterfile " & _
                    "GROUP BY Type"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        Dim ddItem As New ListItem

        ddItem.Text = "-- ALL --"
        ddItem.Value = -1

        cboListObj.ClearSelection()
        cboListObj.Items.Clear()
        cboListObj.Items.Add(ddItem)
        cboListObj.DataSource = dtTable
        cboListObj.DataValueField = "ElementValue"
        cboListObj.DataTextField = "ElementName"
        cboListObj.DataBind()
    End Sub

    Protected Sub FillCardBrandDropDownList(ByRef cboListObj As DropDownList)

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT ROW_NUMBER() OVER (ORDER BY CardBrand) AS ElementValue , CardBrand AS ElementName " & _
                    "FROM PromoBinRangeMasterfile " & _
                    "GROUP BY CardBrand"
        '"WHERE Type = '" & cboXML_RangeType.SelectedItem.Text & "' " & _

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        With cboListObj
            .ClearSelection()
            .Items.Clear()

            If dtTable.Rows.Count <> 1 Then
                Dim ddItem As New ListItem

                ddItem.Text = "-- ALL --"
                ddItem.Value = -1

                .Items.Add(ddItem)
            End If

            .DataSource = dtTable
            .DataBind()
        End With

        If cboListObj.Items.Count = 1 Then
            cboListObj.SelectedIndex = 0
        Else
            cboListObj.SelectedValue = -1
        End If

    End Sub

    Protected Sub FillIssuerDropDownList(ByRef cboListObj As DropDownList)

        Dim dtTable As New DataTable
        Dim strQuery As String = ""

        strQuery = "SELECT ROW_NUMBER() OVER (ORDER BY Issuer) AS ElementValue , Issuer AS ElementName " & _
                    "FROM PromoBinRangeMasterfile " & _
                    " GROUP BY Issuer"

        '"WHERE Type = '" & cboXML_RangeType.SelectedItem.Text & "' " & _
        '" AND (CardBrand = '" & cbo_CardBrand.SelectedItem.Text & "' OR LTRIM(RTRIM('" & cbo_CardBrand.SelectedItem.Text.Replace("-- ALL --", "ALL") & "')) = 'ALL')" & _

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        With cboListObj
            .ClearSelection()
            .Items.Clear()

            If dtTable.Rows.Count <> 1 Then
                Dim ddItem As New ListItem

                ddItem.Text = "-- ALL --"
                ddItem.Value = -1

                .Items.Add(ddItem)
            End If

            .DataSource = dtTable
            .DataBind()
        End With

        If cboListObj.Items.Count = 1 Then
            cboListObj.SelectedIndex = 0
        Else
            cboListObj.SelectedValue = -1
        End If

    End Sub

    Protected Sub cboXML_RangeType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboXML_RangeType.SelectedIndexChanged
        'FillCardBrandDropDownList(cbo_CardBrand)
        'FillIssuerDropDownList(cbo_Issuer)
    End Sub

    Protected Sub cbo_CardBrand_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_CardBrand.SelectedIndexChanged
        'FillIssuerDropDownList(cbo_Issuer)
    End Sub
End Class
