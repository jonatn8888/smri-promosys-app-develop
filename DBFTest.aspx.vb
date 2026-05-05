
Partial Class DBFTest
    Inherits System.Web.UI.Page

    Protected Sub cmdCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCopy.Click

        FileSystem.FileCopy(MapPath("./js/close.gif"), MapPath("\\10.101.21.132\Promotion$\POSFolder\Target.gif"))

    End Sub

    Protected Sub cmdMakeDir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdMakeDir.Click

        'FileSystem.MkDir("\\10.101.21.132\Promotion$\POSFolder\" & txtFolderName.Text)

        My.Computer.FileSystem.CreateDirectory("\\10.101.21.132\Promotion$\POSFolder\" & txtFolderName.Text)
    End Sub

End Class
