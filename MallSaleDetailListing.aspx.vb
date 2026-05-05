Imports System.Data
Imports System.Data.SqlClient

Partial Class MallSaleDetailListing
    Inherits System.Web.UI.Page

    Private Sub ShowParticipationList()

        Dim dtTable As New DataTable
        Dim strQuery As String

        ' load data to grid
        strQuery = "SELECT dbo.Fn_FormatPromoCode(E.DeptCode,'Dp')+'-'+" & _
                         "dbo.Fn_FormatPromoCode(E.SubDeptCode,'SDp')+'-'+" & _
                         "dbo.Fn_FormatPromoCode(E.ClassCode,'Cl') AS ItemCode, " & _
                         "ItemDescription AS ItemDesc, " & _
                         "PromoDescription AS PromoDesc " & _
                    "FROM EventParticipation AS E " & _
                    "INNER JOIN EventPartDetails AS D ON D.PEPnumber = E.PEPnumber " & _
                    "INNER JOIN UserGroups AS G ON G.DeptCode = E.DeptCode " & _
                    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = D.PromoTypeID " & _
                    "INNER JOIN GroupAssignment AS A ON A.GroupID = G.GroupID " & _
                    "WHERE A.UserID = 0" & SystemUser.UserID & " " & _
                    "ORDER BY ItemCode"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)

        gridBrandPromos.DataSource = dtTable
        gridBrandPromos.DataBind()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        ShowParticipationList()


    End Sub

    Protected Sub gridBrandPromos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridBrandPromos.RowCommand

        If e.CommandName.ToUpper = "SELECT" Then

            Dim nRowSelected As Integer

            nRowSelected = e.CommandArgument

            Response.Redirect("MallSaleDetailsEntry.aspx?itemcode=" & gridBrandPromos.Rows(nRowSelected).Cells(1).Text, False)

        End If

    End Sub

    Protected Sub gridBrandPromos_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridBrandPromos.RowCreated

        If (e.Row.RowType = DataControlRowType.DataRow) Then

            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#E2DED6';document.body.style.cursor='hand'")
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

            'e.Row.ToolTip = "Click to select row"
            e.Row.Attributes("onclick") = Me.Page.ClientScript.GetPostBackClientHyperlink(gridBrandPromos, "Select$" & e.Row.RowIndex)
        End If

    End Sub

    Protected Sub gridBrandPromos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridBrandPromos.RowDataBound

        Dim nDescCol As Integer = 3
        'Static rowPrevious As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(nDescCol).Text = Server.HtmlDecode(e.Row.Cells(nDescCol).Text)
        End If

        'If e.Row.RowIndex = 0 Then rowPrevious = e.Row

        'If e.Row.RowIndex > 0 Then

        '    Dim nCol As Integer = 1

        '    ' merge Description cells with same item code
        '    Dim sPrev As String = rowPrevious.Cells(nCol).Text
        '    Dim sCurr As String = e.Row.Cells(nCol).Text

        '    If sCurr = sPrev Then

        '        If rowPrevious.Cells(nCol).RowSpan < 2 Then
        '            rowPrevious.Cells(0).RowSpan = 2
        '            rowPrevious.Cells(nCol).RowSpan = 2
        '            rowPrevious.Cells(2).RowSpan = 2
        '        Else
        '            rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
        '            rowPrevious.Cells(nCol).RowSpan = rowPrevious.Cells(nCol).RowSpan + 1
        '            rowPrevious.Cells(2).RowSpan = rowPrevious.Cells(nCol).RowSpan + 1
        '        End If

        '        e.Row.Cells(0).Visible = False
        '        e.Row.Cells(2).Visible = False
        '        e.Row.Cells(nCol).Visible = False
        '    Else
        '        rowPrevious = e.Row
        '    End If

        'End If

    End Sub

    Protected Sub lnkDetailsEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDetailsEntry.Click

        Response.Redirect("MallSaleDetailsEntry.aspx")

    End Sub

    Protected Sub gridBrandPromos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        gridBrandPromos.PageIndex = e.NewPageIndex
        gridBrandPromos.DataBind()

    End Sub

End Class
