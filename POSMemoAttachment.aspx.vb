Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class POSMemoAttachment
    Inherits System.Web.UI.Page

    Private connString As System.Configuration.ConnectionStringSettings

    Public Property POSAttachmentDrive() As String
        Get
            Return ConfigurationManager.ConnectionStrings("AttachFileDrive").ConnectionString
        End Get
        Set(ByVal value As String)
            value = ConfigurationManager.ConnectionStrings("AttachFileDrive").ConnectionString
        End Set
    End Property

    Public Property ConnStr() As String
        Get
            Return ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString
        End Get
        Set(ByVal value As String)
            value = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString
        End Set
    End Property

#Region " Gridview "


    Private Sub fillChildTable(ByVal DT As DataTable, ByVal i As Integer)
        Dim cnt As Integer = 0
        Dim gVChild As New GridView
        gVChild = gvPOSMemoWithAttachments.Rows(i).FindControl("gvMemoNumbers")
        gVChild.DataSource = DT
        gVChild.AllowSorting = True
        gVChild.DataBind()
        GC.Collect()

        For Each drow As DataRow In DT.Rows
            fillGrandChildTable(POSAttachmentDrive & drow("RequestID"), cnt, drow("RequestID"), i)
            cnt = cnt + 1
        Next



    End Sub
    
    Private Sub LoadGridviewDataFromFolder()
        
        MemosSqlWithAttachment.Dispose()
        gvPOSMemoWithAttachments.Dispose()
        MemosSqlWithAttachment = GetMemoPromPeriodFromSQL()
        gvPOSMemoWithAttachments.DataSource = MemosSqlWithAttachment
        gvPOSMemoWithAttachments.DataBind()
        GC.Collect()
    End Sub

    Dim MemosSqlWithAttachment As New DataTable

    Private Sub middleGridview()
        Dim aa As Integer = 0
        For Each DateRow As DataRow In MemosSqlWithAttachment.Rows
            MemosSqlWithAttachment = GetMemoNumberFromSQL(DateRow("PromoPeriodFrom"))
            fillChildTable(MemosSqlWithAttachment, aa)
            aa = aa + 1
        Next
        GC.Collect()
    End Sub



    Private Function GetMemoPromPeriodFromSQL() As DataTable
        Dim sqlConn As SqlClient.SqlConnection
        sqlConn = New SqlClient.SqlConnection(clsPromo.SQLConnString())
        Try
            sqlConn.Open()
            Dim sqlCmd As New SqlClient.SqlCommand("select distinct M.PromoPeriodFrom from Memos M INNER JOIN Promotions P ON P.MemoID = M.MemoID INNER JOIN PromoTypes PT ON PT.PromoTypeID = P.PromoTypeID where M.Status = 'Approved' AND  (PT.RequireAttachment = 1 OR PT.AllowAttachment  = 1) AND M.PromoPeriodFrom >='" & txtPeriodFrom.Text & "' AND M.PromoPeriodFrom <='" & txtPeriodTo.Text & "' order by M.PromoPeriodFrom", sqlConn)
            sqlCmd.CommandType = CommandType.Text

            Dim da As New SqlDataAdapter(sqlCmd)
            Dim ds As New DataSet

            da.Fill(ds, "tblPromoPeriod")
            Dim dt As DataTable = ds.Tables("tblPromoPeriod")
            Return dt
        Catch ex As Exception
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
            GC.Collect()
        End Try
    End Function


    Private Function GetMemoPromPeriodFromSQLSpecific() As DataTable
        Dim sqlConn As SqlClient.SqlConnection
        sqlConn = New SqlClient.SqlConnection(clsPromo.SQLConnString())
        Try
            sqlConn.Open()
            Dim sqlCmd As New SqlClient.SqlCommand("select distinct M.PromoPeriodFrom,M.RequestID from Memos M INNER JOIN Promotions P ON P.MemoID = M.MemoID INNER JOIN PromoTypes PT ON PT.PromoTypeID = P.PromoTypeID where M.Status = 'Approved' AND  (PT.RequireAttachment = 1 OR PT.AllowAttachment  = 1) AND M.PromoPeriodFrom >='" & txtPeriodFrom.Text & "' AND M.PromoPeriodFrom <='" & txtPeriodTo.Text & "' order by M.PromoPeriodFrom", sqlConn)
            sqlCmd.CommandType = CommandType.Text

            Dim da As New SqlDataAdapter(sqlCmd)
            Dim ds As New DataSet

            da.Fill(ds, "tblPromoPeriod")
            Dim dt As DataTable = ds.Tables("tblPromoPeriod")
            Return dt
        Catch ex As Exception
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
            GC.Collect()
        End Try
    End Function

    Private Sub fillGrandChildTable(ByVal path As String, ByVal i As Integer, ByVal RequestID As String, ByVal iii As Integer)
        Dim gVChild As New GridView
        Dim gVGranChild As New GridView
        Dim files() As String
        Dim ii As Integer = 0
        Dim Dt As System.Data.DataTable
        Dim dr As System.Data.DataRow
        Dt = New System.Data.DataTable
        Dt = New Data.DataTable
        Dt.Columns.Add("AttachedFiles")
        Dt.Columns.Add("RequestID")
        Dt.Columns.Add("Row")
        Dt.Columns.Add("MainTableRow")
        Dim d() As String


        If System.IO.Directory.Exists(path) Then
            files = System.IO.Directory.GetFiles(path, "*.*", IO.SearchOption.TopDirectoryOnly)

            For Each fname As String In files
                dr = Dt.NewRow()
                d = Split(fname, "\")
                dr("RequestID") = RequestID
                dr("AttachedFiles") = "<a href='POSMemoAttachment.aspx?AttachedFile=" & RequestID & "\" & d(UBound(d)) & "'>" & d(UBound(d)) & "</a>"

                dr("Row") = ii
                dr("MainTableRow") = i
                Dt.Rows.Add(dr)
                ii = ii + 1
            Next


            If ii <> 0 Then
                gVChild = gvPOSMemoWithAttachments.Rows(iii).FindControl("gvMemoNumbers")
                gVGranChild = gVChild.Rows(i).FindControl("gvFiles")
                gVGranChild.DataSource = Dt
                gVGranChild.AllowSorting = True
                gVGranChild.DataBind()
            Else
                gVChild = gvPOSMemoWithAttachments.Rows(iii).FindControl("gvMemoNumbers")
                gVChild.Rows(i).Visible = False
            End If
        Else
            gVChild = gvPOSMemoWithAttachments.Rows(iii).FindControl("gvMemoNumbers")
            gVChild.Rows(i).Visible = False
        End If
        GC.Collect()
    End Sub




#End Region

#Region " SQL For Gridview Loading "

    Private Function GetMemoNumberFromSQL(ByVal PromoPeriodFrom As String) As DataTable

        Dim sqlConn As SqlClient.SqlConnection
        sqlConn = New SqlClient.SqlConnection(clsPromo.SQLConnString())
        Try
            sqlConn.Open()
            Dim sqlCmd As New SqlClient.SqlCommand("Select distinct '<a href=''ViewMemo.aspx?MemoID=' + CAST(M.MemoID as varchar(10)) + '''>' +  M.MemoNumber + '</a>' as MemoNumber ,M.RequestID,M.MemoNumber as mn from Memos M INNER JOIN Promotions P ON P.MemoID = M.MemoID INNER JOIN PromoTypes PT ON PT.PromoTypeID = P.PromoTypeID where M.Status = 'Approved' AND  (PT.RequireAttachment = 1 OR PT.AllowAttachment  = 1) AND PromoPeriodFrom = '" & PromoPeriodFrom & "'", sqlConn)
            sqlCmd.CommandType = CommandType.Text

            Dim da As New SqlDataAdapter(sqlCmd)
            Dim ds As New DataSet

            da.Fill(ds, "tblMemosWithAttachment")
            Dim dt As DataTable = ds.Tables("tblMemosWithAttachment")
            Return dt
        Catch ex As Exception
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
            GC.Collect()
        End Try
    End Function

    Private Function GetMemoNumFromSQL(ByVal RequestID As String, ByVal PromoPeriodFrom As String) As DataTable
        Dim sqlConn As SqlClient.SqlConnection
        sqlConn = New SqlClient.SqlConnection(clsPromo.SQLConnString())
        Try
            sqlConn.Open()
            Dim sqlCmd As New SqlClient.SqlCommand("select M.MemoNumber,M.RequestID from Memos M INNER JOIN Promotions P ON P.MemoID = M.MemoID INNER JOIN PromoTypes PT ON PT.PromoTypeID = P.PromoTypeID where M.Status = 'Approved' AND  (PT.RequireAttachment = 1 OR PT.AllowAttachment  = 1) AND M.RequestID = '" & RequestID & "' AND M.PromoPeriodFrom = '" & PromoPeriodFrom & "'", sqlConn)
            sqlCmd.CommandType = CommandType.Text

            Dim da As New SqlDataAdapter(sqlCmd)
            Dim ds As New DataSet

            da.Fill(ds, "tblMemosWithAttachment")
            Dim dt As DataTable = ds.Tables("tblMemosWithAttachment")
            Return dt
        Catch ex As Exception
        Finally
            sqlConn.Close()
            sqlConn.Dispose()
            GC.Collect()
        End Try
    End Function

    

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If SystemUser.UserID = 0 Or SystemUser.UserLevel = 0 Then Response.Redirect("InvalidAccess.aspx")

        If Not IsPostBack Then
            txtPeriodFrom.Text = Now.ToShortDateString
            txtPeriodTo.Text = Now.ToShortDateString
            lblValidate.Text = ""
            gvPOSMemoWithAttachments.Visible = False
        End If

        If Request("AttachedFile") <> Nothing Then
            Dim fname As String
            fname = POSAttachmentDrive & Server.HtmlEncode(Request("AttachedFile"))
            Response.ContentType = "application/x-msdownload"
            Response.AppendHeader("Content-Disposition", "attachment;filename=" & Server.HtmlEncode(Request("AttachedFile")))
            Response.TransmitFile(fname)
            Response.End()
        End If

    End Sub


    Private Sub DisplayPosattachments()
        clsSession.FlagForPOSDisp = True
        LoadGridviewDataFromFolder()
        middleGridview()
    End Sub
    
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        If IsDate(txtPeriodFrom.Text) = True And IsDate(Me.txtPeriodTo.Text) = True Then
            lblValidate.Text = ""
            gvPOSMemoWithAttachments.Visible = True
            DisplayPosattachments()
        Else
            gvPOSMemoWithAttachments.Visible = False
            lblValidate.Text = "Invalid Date From or Date To."
        End If
    End Sub
End Class


