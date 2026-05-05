

Imports System.Data
Imports System.Web.UI.ControlCollection

Partial Class Seed
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            sqlDSPromoSeed.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
        End If
    End Sub

    Protected Sub linkSeeding_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkSeeding.Click
        If SaveSeed() Then
            With sqlDSPromoSeed.InsertParameters
                .Item("RequestID").DefaultValue = clsSession.CurrRequestID
                .Item("StartDate").DefaultValue = txtStartDate.Text
                .Item("EndDate").DefaultValue = txtEndDate.Text
                .Item("MaxNumber").DefaultValue = txtMaxNumber.Text
                .Item("Counter").DefaultValue = txtCounter.Text
                .Item("Prize").DefaultValue = txtPrice.Text
            End With
            sqlDSPromoSeed.Insert()

            txtStartDate.Text = ""
            txtEndDate.Text = ""
            txtMaxNumber.Text = ""
            txtCounter.Text = ""
            txtPrice.Text = ""
        End If
    End Sub

    Private Function SaveSeed() As Boolean
        blistErrorMsg.Items.Clear()
        SaveSeed = True
        If String.IsNullOrEmpty(txtStartDate.Text) Then
            blistErrorMsg.Items.Add("Start date is required.")
            SaveSeed = False
        Else
            If CDate(txtStartDate.Text) < clsSession.PeriodFrom Or CDate(txtStartDate.Text) > clsSession.PeriodTo Then
                blistErrorMsg.Items.Add("Invalid Start Date.")
                SaveSeed = False
            End If
        End If
        If String.IsNullOrEmpty(txtEndDate.Text) Then
            blistErrorMsg.Items.Add("End date is required.")
            SaveSeed = False
        Else
            If CDate(txtEndDate.Text) < clsSession.PeriodFrom Or CDate(txtEndDate.Text) > clsSession.PeriodTo Then
                blistErrorMsg.Items.Add("Invalid End Date.")
                SaveSeed = False
            End If
        End If
        If String.IsNullOrEmpty(txtMaxNumber.Text) Then
            blistErrorMsg.Items.Add("Max number is required.")
            SaveSeed = False
        End If
        If String.IsNullOrEmpty(txtCounter.Text) Then
            blistErrorMsg.Items.Add("Counter is required.")
            SaveSeed = False
        End If
        If String.IsNullOrEmpty(txtPrice.Text) Then
            blistErrorMsg.Items.Add("Price is required.")
            SaveSeed = False
        End If
    End Function

    Protected Sub cmdPopOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopOK.Click
        ClientScript.RegisterStartupScript(Me.GetType, "CloseWindow", "<script>parent.seededitorwindow.hide();</script>")
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        For Each row As GridViewRow In gvSeeding.Rows
            Dim cb As CheckBox = row.FindControl("chkRowSel")
            If cb IsNot Nothing And cb.Checked Then
                'confirm deletion
                With sqlDSPromoSeed.DeleteParameters
                    .Item("RequestID").DefaultValue = clsSession.CurrRequestID
                    .Item("StartDate").DefaultValue = CType(row.Cells(1).Text.Replace("&nbsp;", ""), DateTime).ToShortDateString
                    .Item("EndDate").DefaultValue = CType(row.Cells(2).Text.Replace("&nbsp;", ""), DateTime).ToShortDateString
                    .Item("MaxNumber").DefaultValue = row.Cells(3).Text.Replace("&nbsp;", "")
                    .Item("Counter").DefaultValue = row.Cells(4).Text.Replace("&nbsp;", "")
                    .Item("Prize").DefaultValue = row.Cells(5).Text.Replace("&nbsp;", "")
                End With
                sqlDSPromoSeed.Delete()
            End If
        Next
    End Sub

    'Protected Sub chkALL_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkALL.CheckedChanged

    '    If chkALL.Checked = True Then
    '        Dim row As GridViewRow
    '        For Each row In gridBranches.Rows
    '            Dim chkSel As CheckBox
    '            chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
    '            chkSel.Checked = True
    '        Next
    '    Else
    '        Dim row As GridViewRow
    '        For Each row In gridBranches.Rows
    '            Dim chkSel As CheckBox
    '            chkSel = CType(row.FindControl("chkRowSel"), CheckBox)
    '            chkSel.Checked = False
    '        Next
    '    End If

    'End Sub
End Class
