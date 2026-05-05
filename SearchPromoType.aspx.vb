Imports System.Data
Imports System.Collections.Generic

Partial Class SearchPromoType
    Inherits System.Web.UI.Page

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        LoadGridData()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            hidOutputAll.Value = ""
            ClearRowList()
            LoadGridData()
        End If
    End Sub

    Private Sub LoadGridData()
        Dim dtTable As New DataTable
        Dim strQuery As String
        Dim strTypeDesc As String = txtTypeDesc.Text
        Dim strProcessGroup As String = txtProcessGroup.Text
        strQuery = "EXEC USP_SEARCHPROMOTYPE '{0}','{1}'"
        strQuery = String.Format(strQuery, strTypeDesc, strProcessGroup)
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridSearchResult.DataSource = dtTable
        gridSearchResult.DataBind()
    End Sub

    Protected Sub chkRowSel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        hidOutputAll.Value = ""
        Dim chkSender As CheckBox = TryCast(sender, CheckBox)
        Dim grdViewRow As GridViewRow = TryCast(chkSender.NamingContainer, GridViewRow)
        '15-Nov-2018 - Remove special characters in Branch Description
        'Dim strPromoTypeID As String = grdViewRow.Cells(1).Text + "=" + grdViewRow.Cells(2).Text
        Dim sCode As String = grdViewRow.Cells(1).Text
        Dim sDesc As String = grdViewRow.Cells(2).Text
        sDesc = Regex.Replace(sDesc, "[^a-zA-Z 0-9-/-]", "")
        Dim strPromoTypeID As String = sCode + "=" + sDesc

        If chkSender.Checked Then
            AddRowToList(strPromoTypeID)
        Else
            RemoveRowToList(strPromoTypeID)
        End If
    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim chk As CheckBox
            chk = CType(gridSearchResult.HeaderRow.FindControl("chkALL"), CheckBox)
            If chk.Checked = True Then
                hidOutputAll.Value = "All PromoTypes"
                Dim row As GridViewRow
                For Each row In gridSearchResult.Rows
                    Dim chkSel As CheckBox
                    Dim strPromoTypeID As String = row.Cells(1).Text + "=" + row.Cells(2).Text
                    chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                    chkSel.Checked = True

                    AddRowToList(strPromoTypeID)
                Next
            Else
                hidOutputAll.Value = ""
                Dim row As GridViewRow
                For Each row In gridSearchResult.Rows
                    Dim chkSel As CheckBox
                    Dim strPromoTypeID As String = row.Cells(1).Text
                    chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                    chkSel.Checked = False

                    RemoveRowToList(strPromoTypeID)
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub AddRowToList(ByVal strSelected As String)
        Dim RowList As List(Of String) = New List(Of String)
        If Not Session("SelectedItemList") Is Nothing Then
            RowList = CType(Session("SelectedItemList"), List(Of String))
        End If
        If RowList.IndexOf(strSelected) < 0 Then
            RowList.Add(strSelected)
        End If
        Session("SelectedItemList") = RowList
    End Sub

    Public Sub RemoveRowToList(ByVal strSelected As String)
        Dim RowList As List(Of String) = New List(Of String)
        If Not Session("SelectedItemList") Is Nothing Then
            RowList = CType(Session("SelectedItemList"), List(Of String))
        End If
        RowList.Remove(strSelected)
        Session("SelectedItemList") = RowList
    End Sub

    Public Sub ClearRowList()
        Dim RowList As List(Of String) = New List(Of String)
        If Not Session("SelectedItemList") Is Nothing Then
            RowList = CType(Session("SelectedItemList"), List(Of String))
        End If
        RowList.Clear()
        Session.Remove("SelectedItemList")
    End Sub

    Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        Dim strOutput As String
        Dim List As List(Of String) = New List(Of String)
        If Not Session("SelectedItemList") Is Nothing Then
            List = CType(Session("SelectedItemList"), List(Of String))
            strOutput = String.Join("|", List.ToArray())
            hidOutputValue.Value = strOutput
        End If
        ClearRowList()
        ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.searchWindow.hide();</script>")
    End Sub

End Class
