Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Class UploadedFiles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fillChildTable(clsSession.AttachmentPath)
        End If
    End Sub

    Private Sub fillChildTable(ByVal path As String)
        Dim files() As String
        Dim Dt As System.Data.DataTable
        Dim dr As System.Data.DataRow
        Dt = New System.Data.DataTable
        Dt = New Data.DataTable
        Dt.Columns.Add("AttachedFiles")
        Dim d() As String
        Dim dd As String
        dd = ""
        files = System.IO.Directory.GetFiles(clsSession.AttachmentPath, "*.*", IO.SearchOption.AllDirectories)

        For Each fname As String In files
            dr = Dt.NewRow()
            d = Split(fname, "\")
            dr("AttachedFiles") = d(UBound(d))
            Dt.Rows.Add(dr)
        Next
        If Dt.Rows.Count <> 0 Then
            Button1.Visible = True
        Else
            Button1.Visible = False
        End If
        Session("dt") = Dt
        GridView1.DataSource = Dt
        GridView1.DataBind()
    End Sub

    Public Sub DeleteFile(ByVal path As String)
        Dim FileToDelete As String
        FileToDelete = path
        If System.IO.File.Exists(FileToDelete) = True Then
            System.IO.File.Delete(FileToDelete)
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim a() As String
        a = clsSession.AttachmentPath.Split("\")

        Try

            If fileUpEx.HasFile Then
                AttachFile(a(UBound(a)))
            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    Public Sub AttachFile(ByVal MemoFolder As String)
        Dim filepath As String = fileUpEx.PostedFile.FileName
        'Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
        'Dim r As Regex = New Regex(pat)

        'Dim m As Match = r.Match(filepath)
        'Dim file_ext As String = m.Groups(2).Captures(0).ToString()
        'Dim filename As String = m.Groups(1).Captures(0).ToString()
        Dim filename As String = Path.GetFileNameWithoutExtension(fileUpEx.PostedFile.FileName)
        Dim file_ext As String = Path.GetExtension(fileUpEx.PostedFile.FileName)
        Dim file As String = filename & file_ext

        Directory.CreateDirectory(clsSession.AttachmentPath)

        'save the file to the server 
        fileUpEx.PostedFile.SaveAs(clsSession.AttachmentPath & "\" & file)

        fillChildTable(clsSession.AttachmentPath)
    End Sub

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox
        chk = CType(GridView1.HeaderRow.FindControl("chkSelectAll"), CheckBox)
        If chk.Checked = True Then
            Dim row As GridViewRow
            For Each row In Me.GridView1.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("CheckBox1"), CheckBox)
                chkSel.Checked = True
            Next
        Else
            Dim row As GridViewRow
            For Each row In Me.GridView1.Rows
                Dim chkSel As CheckBox
                chkSel = CType(row.FindControl("CheckBox1"), CheckBox)
                chkSel.Checked = False
            Next
        End If
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataSource = CType(Session("dt"), DataTable)
        GridView1.DataBind()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim chkDelete As CheckBox

        Dim i As Integer = 0

        For i = 0 To GridView1.Rows.Count - 1
            chkDelete = CType(GridView1.Rows(i).Cells(0).FindControl("CheckBox1"), CheckBox)
            If chkDelete.Checked = True Then
                DeleteFile(clsSession.AttachmentPath & "\" & GridView1.Rows(i).Cells(1).Text)
            End If
        Next
        fillChildTable(clsSession.AttachmentPath)

    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.attachmentwindow.hide();</script>")
    End Sub
End Class
