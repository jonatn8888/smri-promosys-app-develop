Imports System.Data

Partial Class SearchDepSdep
    Inherits System.Web.UI.Page

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        SearchItem()
    End Sub

    Private Sub SearchItem()

        Dim strQuery As String = ""

        If txtDepCode.Text.Trim <> "" Then
            strQuery &= " AND DeptCode = 0" & CInt(txtDepCode.Text.Trim)
        End If

        If txtSdepCode.Text.Trim <> "" Then
            strQuery &= " AND SubDepCode = 0" & CInt(txtSdepCode.Text.Trim)
        End If

        If txtClassCode.Text.Trim <> "" Then
            strQuery &= " AND ClassCode = 0" & CInt(txtClassCode.Text.Trim)
        End If

        ' Added dowcarpio08082013@smretailinc: to validate subclass and class discount promotion Per MPD c/o Ms Jessica
        Dim PromoType As String
        Try
            PromoType = Session("PromoType").ToString
        Catch ex As Exception
            PromoType = 0
        End Try

        If PromoType = "65" Then
            ' Added dowcarpio08292012@smretailinc: Added new condition for sub class.
            If txtSubClassCode.Text.Trim() = "" Or txtSubClassCode.Text.Trim() = "0" Then
                strQuery &= " AND SubClassCode <> 0"
            Else
                strQuery &= " AND SubClassCode = 0" & CInt(txtSubClassCode.Text.Trim)
            End If
        Else
            strQuery &= " AND SubClassCode = 0"
        End If


        If (SystemUser.UserGroupType <> "CM") And (SystemUser.UserGroupType <> "SBU") And (SystemUser.UserLevel <> SystemUser.UserRoles.Analyst) Then
            strQuery &= "AND DeptCode IN ( " & _
                                          "SELECT DeptCode FROM UserGroups WHERE GroupID IN ( " & _
                                                                                             "SELECT GroupID FROM GroupAssignment WHERE UserID = 0" & SystemUser.UserID & ")" & _
                                         ") "
        End If

        ' Revised dowcarpio10242012@smretailinc: exclude current condition to filter item per group for char  merch
        ' Revised dowcarpio08312012@smretailinc: format codes using [dbo].[Fn_FormatPromoCode]
        sqldsData.SelectCommand = "SELECT dbo.Fn_FormatPromoCode(DeptCode,'Dp') AS cDeptCode, " & _
                                        "dbo.Fn_FormatPromoCode(SubDepCode,'SDp') AS cSubDepCode, " & _
                                        "dbo.Fn_FormatPromoCode(ClassCode,'Cl') AS cClassCode, " & _
                                        "dbo.Fn_FormatPromoCode(SubClassCode,'SCl') AS cSubClassCode, " & _
                                        "Description, ShortDesc, EnvCode " & _
                                  "FROM DepSdepClass " & _
                                  "WHERE IsHidden = 0 " & _
                                  "AND Description LIKE '%" & txtDescription.Text.Trim() + "%' " & strQuery & " ORDER BY Description"

        sqldsData.Select(DataSourceSelectArguments.Empty)

        gvDeptSdeptClass.DataBind()

        '''''
        'With gvDeptSdeptClass.Rows.Count
        '    Dim r As GridViewRow
        '    For Each r In gvDeptSdeptClass.Rows
        '        r.Cells(0).Attributes.Add("onmouseover", "this.style.cursor = 'hand';")
        '        r.Cells(0).Attributes.Add("onmouseout", "this.style.cursor = 'hand';")
        '    Next
        'End With
    End Sub

    Protected Sub gvDeptSdeptClass_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDeptSdeptClass.PageIndexChanging

        'gvDeptSdeptClass.PageIndex = e.NewPageIndex
        'gvDeptSdeptClass.DataSource = CType(Session("DepSdepClass"), DataTable)
        'gvDeptSdeptClass.DataBind()
        'Dim r As GridViewRow
        'For Each r In gvDeptSdeptClass.Rows
        '    'r.Cells(0).Attributes.Add("onclick", "GetChildFormValue('" & r.Cells(3).Text & "','" & r.Cells(2).Text & "','" & r.Cells(1).Text & "','" & r.Cells(5).Text & "','" & r.Cells(6).Text & "','" & Request.QueryString("txt") & "','" & Request.QueryString("txt2") & "','" & Request.QueryString("txt3") & "','" & Request.QueryString("txt4") & "','" & Request.QueryString("txt5") & "');")
        '    r.Cells(0).Attributes.Add("onmouseover", "this.style.cursor = 'hand';")
        '    r.Cells(0).Attributes.Add("onmouseout", "this.style.cursor = 'hand';")
        'Next

    End Sub


    Protected Sub gvDeptSdeptClass_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDeptSdeptClass.RowCommand

        If e.CommandName.ToLower = "select" Then

            Dim strDetails() As String

            strDetails = Split(e.CommandArgument, ";")

            ' Revised dowcarpio08292012@smretailinc: additional commarg. for sub class variable 
            txtDepCode.Text = strDetails(0).Trim
            txtSdepCode.Text = strDetails(1).Trim
            txtClassCode.Text = strDetails(2).Trim
            txtSubClassCode.Text = strDetails(3).Trim
            txtDescription.Text = strDetails(4).Trim
            hidShortDesc.Value = strDetails(5).Trim
            hidEnvCode.Value = strDetails(6).Trim

            'Dim dKey As DataKey = gvDeptSdeptClass.DataKeys(gvDeptSdeptClass.SelectedIndex)
            'www.centurysendsavior.com/KotoWitchbladeMPU.html
            'txtDepCode.Text = dKey.Values("cDeptCode").ToString()
            'txtSdepCode.Text = ""
            'txtClassCode.Text = ""
            'txtDescription.Text = ""
            'hidShortDesc.Value = ""

            ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.itemsearchwindow.hide();</script>")
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Revised dowcarpio08292012@smretailinc: additional commarg. for sub class variable 
        If Not IsPostBack() Then
            txtDepCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtSdepCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtClassCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            txtSubClassCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
        Else
            SearchItem()
        End If

    End Sub

End Class
