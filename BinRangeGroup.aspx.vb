Imports System.Data
Imports System.Data.SqlClient

Partial Class BinRangeGroup
    Inherits System.Web.UI.Page

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        'Dim lb As Label = e.Row.FindControl("lblPromoTypeID")

        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    e.Row.Cells(1).Text = "<a href='PromoTypeEntry.aspx?xmode=1&promotypeid=" & lb.Text & "'>" & e.Row.Cells(1).Text & "</a>"
        '    e.Row.Cells(3).Text = IIf(e.Row.Cells(3).Text = "True", "<img src='Images/orange-blob.gif' />", "") & "</b>"

        'End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        'Response.Redirect("" & GridView1.SelectedValue.ToString())

    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")

        If (Session("UserLevel") Is Nothing) Or (Session("UserLevel") > SystemUser.UserRoles.Analyst) Then
            Response.Redirect("InvalidAccess.aspx")
        End If

        If Not IsPostBack Then
            FillEnvCode()
        Else
            sqlds.SelectParameters("envCode").DefaultValue = CInt(cboEnvCode.SelectedValue.ToString())
            If txtSearchInput.Text.Trim() = "" Then
                sqlds.SelectParameters("searchParam").DefaultValue = " "
            Else
                sqlds.SelectParameters("searchParam").DefaultValue = txtSearchInput.Text.Trim()
            End If
            Dim dv As DataView = CType(sqlds.Select(DataSourceSelectArguments.Empty), DataView)
            GridView1.DataBind()
            Me.lblRecordCount.Text = "Total no. of records: " & dv.Table.Rows.Count
        End If
    End Sub

    Private Sub FillEnvCode()

        Dim dtTable As New DataTable
        Dim strQuery As String

        strQuery = "SELECT * FROM Environments ORDER BY EnvName"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        cboEnvCode.DataSource = dtTable
        cboEnvCode.DataBind()

    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "View" Then

            Dim arg As String()
            arg = e.CommandArgument.ToString().Split("|")
            Dim env = arg(0)
            Dim id = arg(1)

            Response.Redirect("BinRangeGroupEntry.aspx?env=" & env & "&id=" & id)
        End If
    End Sub
End Class
