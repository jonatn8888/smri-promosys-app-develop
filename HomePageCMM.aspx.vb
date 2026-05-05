
Imports System.Data

Partial Class HomePageCMM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (SystemUser.UserID = 0) Or (SystemUser.UserLevel <> SystemUser.UserRoles.PromoRequestor) Then Response.Redirect("Default.aspx")

        If Not IsPostBack Then

            lblHeader.Text = "Welcome, " & SystemUser.UserSignName

            Dim nCount As Integer
            Dim MyView As DataView

            sqldsData.SelectCommand = "SELECT Count(*) AS MyCount FROM PromoRequests WHERE Status = 'Approved' AND UserID = " & SystemUser.UserID
            MyView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
            nCount = CInt(MyView(0)("MyCount"))

            If nCount > 0 Then
                litStatusMsg.Text = "<a href='PromoReqListCMM.aspx'>You have " & nCount & " newly approved promo requests.</a><br />"
            End If

            sqldsData.SelectCommand = "SELECT Count(*) AS MyCount FROM PromoRequests WHERE Status = 'Returned' AND UserID = " & SystemUser.UserID
            MyView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
            nCount = CInt(MyView(0)("MyCount"))

            If nCount > 0 Then
                litStatusMsg.Text &= "<a href='PromoReqListCMM.aspx'>You have " & nCount & " returned promo requests.</a><br />"
            End If

            ' Added dowcarpio01162013@smretailinc due for actions requests for RCDP
            sqldsData.SelectCommand = "SELECT Count(*) AS MyCount FROM ChangeRequests WHERE Status = 'Returned' AND RequestType = 'CCL' AND UserCreated = " & SystemUser.UserID
            MyView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
            nCount = CInt(MyView(0)("MyCount"))

            If nCount > 0 Then
                litStatusMsg.Text &= "<a href='RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("CCL", SystemUser.EncryptKey.ToString) & "&v2=" & clsEncryptDecrypt.EncryptText("Returned", SystemUser.EncryptKey.ToString) & "'>You have " & nCount & " returned cancellation requests.</a><br />"
            End If

            ' Added dowcarpio01162013@smretailinc due for actions requests for RCDP
            sqldsData.SelectCommand = "SELECT Count(*) AS MyCount FROM ChangeRequests WHERE Status = 'Returned' AND RequestType = 'EXT' AND UserCreated = " & SystemUser.UserID
            MyView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)
            nCount = CInt(MyView(0)("MyCount"))

            If nCount > 0 Then
                litStatusMsg.Text &= "<a href='RCDPReqList.aspx?v1=" & clsEncryptDecrypt.EncryptText("EXT", SystemUser.EncryptKey.ToString) & "&v2=" & clsEncryptDecrypt.EncryptText("Returned", SystemUser.EncryptKey.ToString) & "'>You have " & nCount & " returned extension requests.</a><br />"
            End If

            If litStatusMsg.Text = "" Then
                litStatusMsg.Text = "You have no new alerts."
            End If

        End If

        LoadStoreWidePromos()

    End Sub

    Protected Sub LoadStoreWidePromos()

        Dim dtTable As New DataTable
        Dim strQuery As String

        strQuery = "SELECT * FROM Memos " & _
                    "WHERE (PromoPeriodFrom > GETDATE() OR (GETDATE() BETWEEN PromoPeriodFrom AND PromoPeriodTo)) " & _
                    "AND Status = 'Approved' " & _
                    "AND OwnerGroup IN " & _
                    "(SELECT GroupID FROM UserGroups WHERE DeptCode IS NULL AND GroupType IN ('SBU','CM', 'BCR')) " & _
                    "ORDER BY PromoPeriodFrom DESC, PromoPeriodTo"
        '"ORDER BY ApproveDate DESC, PromoPeriodFrom, PromoPeriodTo"

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridStoreWidePromos.DataSource = dtTable
        gridStoreWidePromos.DataBind()

    End Sub

    Protected Sub gridStoreWidePromos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridStoreWidePromos.PageIndexChanging
        gridStoreWidePromos.PageIndex = e.NewPageIndex
        gridStoreWidePromos.DataBind()
    End Sub

    Protected Sub gridStoreWidePromos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridStoreWidePromos.RowCommand

        If e.CommandName.ToUpper = "SELECT" Then

            Dim lbMemoID As Label
            Dim nRowSelected As Integer

            nRowSelected = e.CommandArgument
            lbMemoID = gridStoreWidePromos.Rows(nRowSelected).FindControl("lblMemoID")

            Response.Redirect("ViewMemo.aspx?MemoID=" & lbMemoID.Text)
        End If

    End Sub

    Protected Sub gridStoreWidePromos_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridStoreWidePromos.RowCreated

        If (e.Row.RowType = DataControlRowType.DataRow) Then

            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#E2DED6'")
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

            'e.Row.ToolTip = "Click to select row"
            e.Row.Attributes("onclick") = Me.Page.ClientScript.GetPostBackClientHyperlink(gridStoreWidePromos, "Select$" & e.Row.RowIndex)
        End If

    End Sub

    Protected Sub gridStoreWidePromos_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gridStoreWidePromos.Sorting
        Dim dtTable As DataTable

        dtTable = TryCast(gridStoreWidePromos.DataSource, DataTable)

        If Not dtTable Is Nothing Then

            Dim dvView As DataView = New DataView(dtTable)
            dvView.Sort = e.SortExpression & " " & ConvertSortDirectionToSql(e.SortDirection)

            gridStoreWidePromos.DataSource = dvView
            gridStoreWidePromos.DataBind()

        End If

    End Sub

    Protected Function ConvertSortDirectionToSql(ByVal sdSortDir As SortDirection) As String

        Dim newSortDir As String = ""

        If sdSortDir = SortDirection.Ascending Then
            newSortDir = "ASC"
        Else
            newSortDir = "DESC"
        End If

        ConvertSortDirectionToSql = newSortDir
    End Function

End Class
