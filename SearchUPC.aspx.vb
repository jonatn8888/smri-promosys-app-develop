Imports System.Data

Partial Class SearchUPC
    Inherits System.Web.UI.Page

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

        LoadGridData()

    End Sub

    Protected Sub gridSearchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gridSearchResult.PageIndexChanging

        gridSearchResult.PageIndex = e.NewPageIndex

        LoadGridData()

    End Sub

    Protected Sub gridSearchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gridSearchResult.RowCommand

        If e.CommandName.ToUpper = "SELECT" Then

            Dim nRowSelected As Integer

            nRowSelected = e.CommandArgument

            'Marker0009
            If Val(gridSearchResult.Rows(nRowSelected).Cells(2).Text) < 1 Then

                Dim drPromoType As DataRow = Nothing
                Dim PromoTypeID As Integer
                Dim sQuery As String

                sQuery = "SELECT pt.* FROM PromoTypes pt INNER JOIN Promotions p ON p.PromoTypeID = pt.PromoTypeID " & _
                 "WHERE p.RequestID = 0" & clsSession.CurrRequestID

                If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sQuery, drPromoType) Then
                    PromoTypeID = drPromoType("PromoTypeID")
                End If

                If PromoTypeID = "346" Then
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "msg", "alert('Price should be at its regular price (not 0.01)');", True)
                    Exit Sub
                End If

            End If

            txtUPCnumber.Text = gridSearchResult.Rows(nRowSelected).Cells(0).Text
            txtDescription.Text = gridSearchResult.Rows(nRowSelected).Cells(1).Text
            txtUnitPrice.Text = gridSearchResult.Rows(nRowSelected).Cells(2).Text

            ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.searchWindow.hide();</script>")

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then
            txtUPCnumber.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            'txtSdepCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            'txtClassCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
            'txtSubClassCode.Attributes.Add("onkeypress", "return AllowNumericOnly(this);")
        End If

        LoadGridData()

    End Sub

    Protected Sub gridSearchResult_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridSearchResult.RowCreated

            If (e.Row.RowType = DataControlRowType.DataRow) Then

            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#E2DED6';document.body.style.cursor='hand'")
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

            'e.Row.ToolTip = "Click to select row"
            e.Row.Attributes("onclick") = Me.Page.ClientScript.GetPostBackClientHyperlink(gridSearchResult, "Select$" & e.Row.RowIndex)
        End If

    End Sub

    Private Sub LoadGridData()
        Dim sQuery As String
        Dim drPromoType As DataRow = Nothing
        Dim IsPromoPremium As Boolean = False
        Dim PosQualifiedItems As Integer = 0
        Dim PromoTypeID As Integer = 0

        sQuery = "SELECT p.PosQualifiedItems,pt.* FROM PromoTypes pt INNER JOIN Promotions p ON p.PromoTypeID = pt.PromoTypeID " & _
         "WHERE p.RequestID = 0" & clsSession.CurrRequestID

        'jsuy asc Excluded Items search based on Qualified Items selected  april/2026 
        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, sQuery, drPromoType) Then
            IsPromoPremium = (drPromoType("POS_PromoPremUPC_STATE") > 0)
            PosQualifiedItems = drPromoType("PosQualifiedItems")
            PromoTypeID = drPromoType("PromoTypeID")
        End If

        Dim dtTable As New DataTable
        Dim strQuery As String

        If (IsPromoPremium And (txtDescription.Text IsNot "") Or (txtUPCnumber.Text IsNot "")) Then
            strQuery = "SELECT UPC.UPCno, " & _
                     "UPC.Description, " & _
                     "UPC.UnitPrice, " & _
                     "dbo.Fn_FormatPromoCode(UPC.DeptCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(UPC.SubDeptCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(UPC.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(UPC.SubClassCode,'SCl') AS ItemCode " & _
                    "FROM MMS_UPC_Table AS UPC " & _
                    "WHERE UPC.UPCno LIKE '" & txtUPCnumber.Text & "%' " & _
                    "AND UPC.Description LIKE '%" & txtDescription.Text & "%'"

        ElseIf (txtDescription.Text IsNot "" Or txtUPCnumber.Text IsNot "") Then
            strQuery = "SELECT UPC.UPCno, " & _
                         "UPC.Description, " & _
                         "UPC.UnitPrice, " & _
                         "dbo.Fn_FormatPromoCode(UPC.DeptCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(UPC.SubDeptCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(UPC.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(UPC.SubClassCode,'SCl') AS ItemCode " & _
                        "FROM Promotions AS P " & _
                        "INNER JOIN PromoDetails AS D ON D.PromoID = P.PromoID " & _
                        "INNER JOIN MMS_UPC_Table AS UPC ON UPC.DeptCode = D.DepCode " & _
                        "   AND (D.SubDepCode = 0 OR UPC.SubDeptCode = D.SubDepCode) " & _
                        "   AND (D.ClassCode = 0 OR UPC.ClassCode = D.ClassCode) " & _
                        "   AND (D.SubClassCode = 0 OR UPC.SubClassCode = D.SubClassCode) " & _
                        "WHERE P.RequestID = 0" & clsSession.CurrRequestID & " " & _
                        "AND UPC.UPCno LIKE '" & txtUPCnumber.Text & "%' " & _
                        "AND UPC.Description LIKE '%" & txtDescription.Text & "%'"

            If Val(txtUnitPrice.Text) > 0 Then
                strQuery &= " AND UPC.UnitPrice = 0" & Val(txtUnitPrice.Text)
            End If
        End If

        'jsuy asc Special Discount Excluded Items search based on Qualified Items selected  april/2026 
        If (PromoTypeID = 327 And PosQualifiedItems <> 3 And ((txtDescription.Text IsNot "") Or (txtUPCnumber.Text IsNot ""))) Then
            strQuery = "SELECT UPC.UPCno, " & _
                   "UPC.Description, " & _
                   "UPC.UnitPrice, " & _
                   "dbo.Fn_FormatPromoCode(UPC.DeptCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(UPC.SubDeptCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(UPC.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(UPC.SubClassCode,'SCl') AS ItemCode " & _
                  "FROM MMS_UPC_Table AS UPC " & _
                  "WHERE UPC.UPCno LIKE '" & txtUPCnumber.Text & "%' " & _
                  "AND UPC.Description LIKE '%" & txtDescription.Text & "%'"

        End If

        clsSystemApp.GetDataTable(clsPromo.SQLConnString, strQuery, dtTable)
        gridSearchResult.DataSource = dtTable
        gridSearchResult.DataBind()

    End Sub

End Class
