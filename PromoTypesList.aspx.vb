Imports System.Data.SqlClient
Imports System.Data

Partial Class PromoTypesList
    Inherits System.Web.UI.Page

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Dim dt As DataTable = CType(Session("dsPromoTypesList"), DataTable)
        Me.GridView1.PageIndex = e.NewPageIndex
        Me.GridView1.DataSource = dt
        Me.GridView1.DataBind()
        Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        Dim lb As Label = e.Row.FindControl("lblPromoTypeID")

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(1).Text = "<a href='PromoTypesEntry.aspx?xmode=1&promotypeid=" & lb.Text & "'>" & e.Row.Cells(1).Text & "</a>"
            e.Row.Cells(3).Text = IIf(e.Row.Cells(3).Text = "True", "<img src='Images/orange-blob.gif' />", "") & "</b>"

        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        'Response.Redirect("" & GridView1.SelectedValue.ToString())

    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        Response.Redirect("PromoTypesEntry.aspx?xmode=0&promotypeid=0")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Or SystemUser.UserAccessSettings <> 1) Then Response.Redirect("InvalidAccess.aspx")
        If (Session("UserLevel") Is Nothing) Or (Session("UserLevel") > SystemUser.UserRoles.Analyst) Then
            Response.Redirect("InvalidAccess.aspx")
        End If
        If Not IsPostBack Then
            FillGridView()
            cmdNew.Visible = False
        End If
    End Sub

    Private Sub FillGridView()
        'If txtSearchInput.Text = "" Then
        'BindGridView("USP_SelectBranches")
        'Else
        BindGridView("Usp_SearchPromoTypes")
        'End If
    End Sub

    Private Sub BindGridView(ByVal StoredProc As String)
        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As New SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        If StoredProc = "Usp_SearchPromoTypes" Then
            sqlCmd.Parameters.Add("@WhereCriteria", SqlDbType.VarChar)
            sqlCmd.Parameters("@WhereCriteria").Value = txtSearchInput.Text.Trim
        End If
        Dim da As New SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tblPromoTypes")
        Dim dt As DataTable = ds.Tables("tblPromoTypes")
        If dt.Rows.Count <> 0 Then
            Session("dsPromoTypesList") = dt
            GridView1.DataSource = dt
            GridView1.DataBind()
            Me.lblValidateMessage.Text = ""
            Me.lblRecordCount.Visible = True
            Me.lblRecordCount.Text = "Total no. of records: " & dt.Rows.Count.ToString
        Else
            Session("dsPromoTypesList") = Nothing
            GridView1.DataSource = Nothing
            GridView1.DataBind()
            Me.lblValidateMessage.Text = "No record found."
            Me.lblRecordCount.Visible = False
        End If

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FillGridView()
        GridView1.Visible = True
        'If FillGridView() Then
        '    tblGridviewUsers.Visible = True
        '    lblValidateMessage.Text = ""
        'Else
        '   lblValidateMessage.Text = "No record found."
        '   Me.tblGridviewUsers.Visible = False
        'End If
    End Sub
End Class
