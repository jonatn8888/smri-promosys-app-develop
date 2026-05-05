Imports System.Data
Imports dsPromotionsTableAdapters
Imports System.IO
Imports System.Data.SqlClient
Imports System.Configuration
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Configuration.ConfigurationManager
Imports System.Collections.Generic
Imports System.Linq
Imports System.Drawing
Imports System.Drawing.Imaging
Imports ZXing
Imports ZXing.Common







Partial Class ViewRequest
    Inherits System.Web.UI.Page

#Region " Connection String "


    Private connString As System.Configuration.ConnectionStringSettings
    Private _ConnStr As String = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString '"Data Source=SMportal-DEV;Initial Catalog=Promo;Persist Security Info=True;User ID=admin;Password=password;"
    Public Property ConnStr() As String
        Get
            Return _ConnStr
        End Get
        Set(ByVal value As String)
            value = _ConnStr
        End Set
    End Property

#End Region

    Private Sub LoadRequestInformation()

        Dim taRequests As New dsPromotionsTableAdapters.PromoRequestsTableAdapter()
        Dim dtRequests As dsPromotions.PromoRequestsDataTable
        Dim rowRequests As dsPromotions.PromoRequestsRow

        dtRequests = taRequests.GetPromoRequestByID(clsSession.CurrRequestID)

        If dtRequests.Rows.Count = 0 Then
            Response.Redirect("InvalidAccess.aspx")
        Else

            rowRequests = dtRequests.Rows(0)

            With rowRequests
                lblDocNumber.Text = "PR-" & Format(.RequestID, "0####") '& "-" & Format(.RequestDate, "yy")
                lblDocDate.Text = Format(.RequestDate, "MMMM dd, yyyy")

                lblPromoTitle.Text = .Title.ToString()
                lblPromoPeriod.Text = .PromoPeriodFrom.ToLongDateString() & " to " & .PromoPeriodTo.ToLongDateString()

                litBranches.Text = .Branches.ToString()

                lblRequestedBy.Text = .RequestedBy.ToUpper()
                lblRequestPos.Text = .RequesterPos.ToString()

                lblApprovedBy.Text = .ApprovedBy.ToUpper()
                lblApproverPos.Text = .ApproverPos.ToString()

                ' Added dowcarpio07232012@smretailinc: Display 'Draft' status field for all draft promotional requests.
                If .Status = "Draft" Then
                    'ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>changeBGImage();</script>")
                    lblStatus.Text = "Draft"

                Else

                    trStatus.Visible = False
                End If

                'Added dowcarpio08162012@smretailinc: hide promo item for SBU Marketing Requestor
                'MALAgasino 20181227 - Commented code below for Merchandise level promos.
                'Rbs7281 20250813 - Safetly protect IsUPCLevel to not DbNull
                'rbs7281 8/13/2025 - 
                If .GroupType = "SBU" AndAlso Convert.ToInt32(If(Session("IsUPCLevel"), 0)) = 0 Then

                    gridPromotions.Columns(1).Visible = False

                End If


            End With
        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        '::ToDo:: parameter and security checking
        If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If clsSession.CurrRequestID = 0 Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack() Then

            ViewState("CallingPage") = Request.UrlReferrer.ToString()


            LoadRequestInformation()
            If Request("Report") = "True" Then
                lnkPrintMemo.Attributes.Add("OnClick", "return printSpecial()")
                lnkPrintMemo.Text = "Print this Document"
                lnkDone.Text = "Close Print Preview"
            Else
                lnkPrintMemo.Text = "Use As Template"
                lnkDone.Text = "Close Template Preview"
            End If
        End If

    End Sub


    Protected Sub gridPromotions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridPromotions.RowDataBound

        Const nCol As Integer = 2
        Static rowPrevious As GridViewRow

        ' html decode promo description field in order to display properly
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(nCol).Text = Server.HtmlDecode(e.Row.Cells(nCol).Text)
        End If

        If e.Row.RowIndex = 0 Then rowPrevious = e.Row

        If e.Row.RowIndex >= 0 Then
            Dim lbCurr As Label = e.Row.FindControl("lblPromoID")
            Dim gridview As GridView = e.Row.FindControl("gvBranches")
            Dim dt As DataTable
            dt = getbranches(lbCurr.Text)
            gridview.DataSource = dt
            gridview.DataBind()
        End If

        If e.Row.RowIndex > 0 Then

            ' merge Description cells with same PromoID
            Dim lbPrev As Label = rowPrevious.FindControl("lblPromoID")
            Dim lbCurr As Label = e.Row.FindControl("lblPromoID")
            If lbCurr.Text = lbPrev.Text Then

                If rowPrevious.Cells(nCol).RowSpan < 2 Then
                    rowPrevious.Cells(0).RowSpan = 2
                    rowPrevious.Cells(nCol).RowSpan = 2
                Else
                    rowPrevious.Cells(0).RowSpan = rowPrevious.Cells(0).RowSpan + 1
                    rowPrevious.Cells(nCol).RowSpan = rowPrevious.Cells(nCol).RowSpan + 1
                End If

                e.Row.Cells(0).Visible = False
                e.Row.Cells(nCol).Visible = False
            Else
                rowPrevious = e.Row
            End If

        End If

    End Sub

    Private Function getbranches(ByVal promoid As Int32) As DataTable

        '::ToDo:: Summarize branches by Promotion

        Dim StoredProc As String
        StoredProc = "SELECT ShortName as shortdesc  FROM PromoBranch WHERE PromoID = '" & promoid & "'"
        Dim sqlConn As SqlClient.SqlConnection
        sqlConn = New SqlClient.SqlConnection(ConnStr)
        Dim sqlCmd As New SqlClient.SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.Text
        Dim da As New SqlClient.SqlDataAdapter(sqlCmd)
        Dim ds As New DataSet
        sqlConn.Open()
        da.Fill(ds, "tbl_branches")
        Dim dt As DataTable = ds.Tables("tbl_branches")
        Return dt

        ' Added dowcarpio08232012@smretailinc: close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        da = Nothing
        dt = Nothing

        GC.Collect()

    End Function

    Protected Sub lnkDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDone.Click

        Response.Redirect(ViewState("CallingPage"))

    End Sub

    'Protected Sub lnkPrintMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintMemo.Click
    '    ClientScript.RegisterClientScriptBlock(Me.GetType, "print", "<script language='javascript' type='text/javascript'>return printdiv('main');</script>", False)
    'End Sub

    Protected Sub lnkPrintMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintMemo.Click

        If lnkPrintMemo.Text = "Use As Template" Then

            Dim StoredProc As String = "USP_UseTemplate"
            Dim sqlConn As New SqlClient.SqlConnection(ConnStr)
            Dim sqlCmd As New SqlClient.SqlCommand(StoredProc, sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            sqlConn.Open()

            sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = clsSession.CurrRequestID
            sqlCmd.Parameters.Add("@OriginalsRequestID", SqlDbType.Int).Direction = ParameterDirection.Output
            sqlCmd.Parameters.Add("@NewsRequestID", SqlDbType.Int).Direction = ParameterDirection.Output

            sqlCmd.ExecuteNonQuery()

            Dim OriginalRequestID As Integer = If(sqlCmd.Parameters("@OriginalsRequestID").Value Is DBNull.Value, 0, Convert.ToInt32(sqlCmd.Parameters("@OriginalsRequestID").Value))
            Dim NewRequestID As Integer = If(sqlCmd.Parameters("@NewsRequestID").Value Is DBNull.Value, 0, Convert.ToInt32(sqlCmd.Parameters("@NewsRequestID").Value))

            Dim CouponBarcode As String = String.Empty

            CouponBarcode = GetCouponBarcode(NewRequestID)
            AutoAttachment(CouponBarcode, NewRequestID)

            If sqlConn.State <> ConnectionState.Closed Then
                sqlConn.Close()
            End If
            sqlConn.Dispose()
            sqlCmd.Dispose()

            ' Pag-check kung may existing folder
            Dim posFileDrives As String = ConfigurationManager.ConnectionStrings("POSFileDrive").ConnectionString
            Dim sourceFolder As String = posFileDrives & OriginalRequestID.ToString()
            Dim destFolder As String = posFileDrives & NewRequestID.ToString()

            If System.IO.Directory.Exists(sourceFolder) Then
                lblPopTitle.Value = "Note"
                clsSession.Message = "The original memo has an attachment. Please ensure relevant file/s are uploaded in your request."
                clsSession.Icon = "fyi"
                ViewState("process") = "selectbranch"
                ViewState("save") = "2"
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

            Else
                Response.Redirect("PromoRequest.aspx?RequestID=" & NewRequestID)
            End If

            GC.Collect()
            NewRequestIDHidden.Value = (NewRequestID)



            'Response.Redirect("PromoRequest.aspx?RequestID=" & NewRequestIDHidden.Value)
        End If

    End Sub
    Private Sub ClearPromotionDetails()

        lblPopTitle.Value = "Change Promotion"
        clsSession.Message = "Are you sure you want to change the promotion? All information will be deleted."
        clsSession.Icon = "inquiry"
        ViewState("process") = "ChangePromotion"
        ClientScript.RegisterStartupScript(Me.GetType(), "key", "<script>alert('Popup is triggered');</script>")


    End Sub


    Private Function getRequestID() As Integer
        Dim StoredProc As String = "USP_UseTemplate"
        Dim sqlConn As New SqlClient.SqlConnection(ConnStr)
        Dim sqlCmd As New SqlClient.SqlCommand(StoredProc, sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlConn.Open()

        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = clsSession.CurrRequestID
        sqlCmd.Parameters.Add("@OriginalsRequestID", SqlDbType.Int).Direction = ParameterDirection.Output
        sqlCmd.Parameters.Add("@NewsRequestID", SqlDbType.Int).Direction = ParameterDirection.Output

        sqlCmd.ExecuteNonQuery()

        Dim OriginalRequestID As Integer = If(sqlCmd.Parameters("@OriginalsRequestID").Value Is DBNull.Value, 0, Convert.ToInt32(sqlCmd.Parameters("@OriginalsRequestID").Value))
        Dim NewRequestID As Integer = If(sqlCmd.Parameters("@NewsRequestID").Value Is DBNull.Value, 0, Convert.ToInt32(sqlCmd.Parameters("@NewsRequestID").Value))


        If sqlConn.State <> ConnectionState.Closed Then
            sqlConn.Close()
        End If
        sqlConn.Dispose()
        sqlCmd.Dispose()

        ' Pag-check kung may existing folder
        Dim posFileDrives As String = ConfigurationManager.ConnectionStrings("POSFileDrive").ConnectionString
        Dim sourceFolder As String = posFileDrives & OriginalRequestID.ToString()
        Dim destFolder As String = posFileDrives & NewRequestID.ToString()

        If System.IO.Directory.Exists(sourceFolder) Then
            lblPopTitle.Value = "Select Department"
            clsSession.Message = "Cannot select department. No business unit.<br/><br/>Please select a business unit from list."
            clsSession.Icon = "fyi"
            ViewState("process") = "selectbranch"
            ViewState("save") = "2"
            ClientScript.RegisterStartupScript(Me.GetType(), "key", "<script>alert('Popup is triggered');</script>")
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor('','');</script>")

        End If

        GC.Collect()
        Return NewRequestID
    End Function


    Protected Sub cmdPopUpOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPopUpOK.Click
        Response.Redirect("PromoRequest.aspx?RequestID=" & NewRequestIDHidden.Value)
    End Sub


    Protected Function GetCouponBarcode(ByVal newRequestID As Long) As String

        Dim drRow As DataRow = Nothing
        Dim strQuery As String

        Dim lResult As String = String.Empty

        strQuery = "SELECT PC.CouponCode  FROM PROMOTIONS P " & _
                    "LEFT JOIN PromotionCoupons PC " & _
                    " on p.PromoID = pc.promoid " & _
                    "WHERE P.RequestID = 0" & newRequestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
            lResult = drRow("CouponCode").ToString()
        End If

        Return lResult

    End Function

    Protected Sub AutoAttachment(ByVal CouponCode_ As String, ByVal RequestID_ As String)
        If Not String.IsNullOrEmpty(CouponCode_) Then
            ' Create the barcode writer
            Dim options As New ZXing.Common.EncodingOptions()
            options.Width = 300
            options.Height = 100
            options.Margin = 3

            Dim writer As New ZXing.BarcodeWriter()
            writer.Format = ZXing.BarcodeFormat.CODE_128
            writer.Options = options

            ' Generate the barcode
            Dim barcodeBmp As Bitmap = writer.Write(CouponCode_)

            ' === Add extra white space (top margin) ===
            Dim topMargin As Integer = 20 ' adjust this value for more or less space
            Dim newHeight As Integer = barcodeBmp.Height + topMargin
            Dim finalBmp As New Bitmap(barcodeBmp.Width, newHeight)

            Using g As Graphics = Graphics.FromImage(finalBmp)
                g.Clear(Color.White)
                g.DrawImage(barcodeBmp, 0, topMargin)
            End Using
            ' =========================================

            ' Save both to base64 (for UI) and to disk (for posting)
            Using ms As New MemoryStream()
                finalBmp.Save(ms, ImageFormat.Png)
                Dim base64String As String = Convert.ToBase64String(ms.ToArray())

                ' Display in UI
                ''imgBarcode.ImageUrl = "data:image/png;base64," & base64String
                clsSession.AttachmentPath = clsPromo.pathAttachment & RequestID_.ToString()

                ' Save to server
                Directory.CreateDirectory(clsSession.AttachmentPath)
                Dim filePath As String = Path.Combine(clsSession.AttachmentPath, CouponCode_ & ".png")
                File.WriteAllBytes(filePath, ms.ToArray())
            End Using
        End If
    End Sub

End Class
