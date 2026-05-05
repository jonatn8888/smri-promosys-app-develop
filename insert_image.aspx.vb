
Partial Class rte_insert_image
    Inherits System.Web.UI.Page

    Protected Sub Submit1_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit1.ServerClick
        If fileUpEx.HasFile Then
            AttachFile()
        End If
    End Sub

    Public Sub AttachFile()
        Dim filepath As String = fileUpEx.PostedFile.FileName
        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
        Dim r As Regex = New Regex(pat)

        Dim m As Match = r.Match(filepath)
        Dim file_ext As String = m.Groups(2).Captures(0).ToString()
        Dim filename As String = m.Groups(1).Captures(0).ToString()
        Dim file As String = filename & "." & file_ext

        'save the file to the server 
        If file_ext.ToLower = "jpg" Or file_ext.ToLower = "png" Or file_ext.ToLower = "gif" Or file_ext.ToLower = "bmp" Then
            fileUpEx.PostedFile.SaveAs(Server.MapPath("./AttachedImages") & "/" & file)
            url.Value = Left(Request.Url.ToString(), InStrRev(Request.Url.ToString(), "/")) & "AttachedImages/" & filename & "." & file_ext
            ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>AddImage();</script>")
            Label1.Text = ""
        Else
            Label1.Text = "Invalid file"
        End If
    End Sub


End Class
