Imports System.Data
Imports System.Collections.Generic

Partial Class SearchStore
    Inherits System.Web.UI.Page

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        ClearRowList()
        LoadGridData()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            txtBranchCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            hidOutputAll.Value = ""
            ClearRowList()
            LoadGridData()
        End If

    End Sub

    Private Sub LoadGridData()
        Dim dtTable As New DataTable
        Dim strQuery As String
        Dim strEnv As String = txtEnv.Text
        Dim strBranchCode As String = txtBranchCode.Text
        Dim strDescription As String = txtBranchName.Text

        strQuery = "EXEC USP_SEARCHSTORE '{0}','{1}','{2}'"
        strQuery = String.Format(strQuery, strEnv, strBranchCode, strDescription)
        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridSearchResult.DataSource = dtTable
        gridSearchResult.DataBind()

    End Sub

    Protected Sub chkRowSel_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        hidOutputAll.Value = ""
    End Sub

    Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim chk As CheckBox
            chk = CType(gridSearchResult.HeaderRow.FindControl("chkALL"), CheckBox)
            If chk.Checked = True Then
                hidOutputAll.Value = "All Stores"
                Dim row As GridViewRow
                For Each row In gridSearchResult.Rows
                    Dim chkSel As CheckBox
                    chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                    If chkSel.Checked = False Then
                        chkSel.Checked = True
                    End If
                    chkSel.Checked = True
                Next
            Else
                hidOutputAll.Value = ""
                Dim row As GridViewRow
                For Each row In gridSearchResult.Rows
                    Dim chkSel As CheckBox
                    chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
                    If chkSel.Checked = True Then
                        chkSel.Checked = False
                    End If
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

    Public Sub ClearRowList()
        'Dim RowList As List(Of String) = New List(Of String)
        'If Not Session("SelectedItemList") Is Nothing Then
        '    RowList = CType(Session("SelectedItemList"), List(Of String))
        'End If
        'RowList.Clear()
        Session.Remove("SelectedItemList")
    End Sub

    Protected Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        ClearRowList()
        Dim row As GridViewRow
        For Each row In gridSearchResult.Rows
            Dim chkSel As CheckBox
            '15-Nov-2018 - Remove special characters in Branch Description
            'Dim strCompBranch As String = row.Cells(5).Text + " " + row.Cells(4).Text
            Dim sCompBranch As String = row.Cells(5).Text
            Dim sDesc As String = row.Cells(4).Text
            sDesc = Regex.Replace(sDesc, "[^a-zA-Z 0-9-/-]", "")
            Dim strCompBranch As String = sCompBranch + " " + sDesc

            chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
            If (chkSel.Checked) Then
                AddRowToList(strCompBranch)
            End If
        Next

        Dim strOutput As String
        Dim List As List(Of String) = New List(Of String)
        If Not Session("SelectedItemList") Is Nothing Then
            List = CType(Session("SelectedItemList"), List(Of String))
            strOutput = String.Join(",", List.ToArray())
            hidOutputValue.Value = strOutput
        End If

        'hidOutputValue.Value = hidOutputValue.Value.Replace("&#209;", "Ñ").Replace("&amp;", "&").Replace("!", " ").Replace("'", " ").Replace(".", " ")
        ClearRowList()
        ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.searchWindow.hide();</script>")
    End Sub

End Class
