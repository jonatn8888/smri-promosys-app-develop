Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Object
Imports ICSharpCode.SharpZipLib.Zip

Partial Class UploadReseedFiles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            fillChildTable(clsSession.AttachmentPath)
        End If
    End Sub

    Private Sub fillChildTable(ByVal path As String)
        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)
        Dim files() As String
        Dim Dt As System.Data.DataTable
        Dim dr As System.Data.DataRow
        Dt = New System.Data.DataTable
        Dt = New Data.DataTable
        Dt.Columns.Add("AttachedFiles")
        Dim d() As String
        Dim dd As String
        dd = ""
        '' test
        Dim pathTemp As String


        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = sRequestID & "-Swipestakes"
        Dim reseedFolder As String = "\CR" & sCRID
        pathTemp = swipestakeSeedFolder & subFolderName & reseedFolder
        Directory.CreateDirectory(pathTemp)

        files = System.IO.Directory.GetFiles(pathTemp, "*.*")

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

    Public Sub DeleteFile(ByVal path As String, ByVal fileName As String)
        Dim FileToDelete As String
        FileToDelete = path
        If System.IO.File.Exists(FileToDelete) = True Then
            DeleteCSV(fileName, path)
            System.IO.File.Delete(FileToDelete)
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        '' ADD
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
        blistErrorMsg.Items.Clear()
        Dim filepath As String = fileUpEx.PostedFile.FileName
        Dim filename As String = Path.GetFileNameWithoutExtension(fileUpEx.PostedFile.FileName)
        Dim file_ext As String = Path.GetExtension(fileUpEx.PostedFile.FileName)
        Dim file As String = filename & file_ext

        If IsValidExtension(file_ext) Then
            'save the file to the server 
            SaveSwipestakes(file)
        End If

        fillChildTable(clsSession.AttachmentPath)
    End Sub

    Private Function IsValidExtension(ByVal extension As String) As Boolean
        Dim validExtension() As String = {".zip", ".csv"}
        IsValidExtension = True
        If Not Array.IndexOf(New String() {".zip", ".csv"}, LCase(extension)) > -1 Then
            blistErrorMsg.Items.Add("Invalid file extension.")
            IsValidExtension = False
        End If
    End Function

    Private Function UnZip(ByVal filename As String, ByVal DownloadPath As String) As Boolean
        Try
            Dim dir As String = DownloadPath & Split(filename, ".")(0)
            If Not Directory.Exists(dir) Then Directory.CreateDirectory(dir)
            Dim x As New ICSharpCode.SharpZipLib.Zip.FastZip
            x.ExtractZip(DownloadPath & filename, dir, "")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub SaveSwipestakes(ByVal file As String)
        '' SAVE FILE IN CR PATH
        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)

        Dim filePath As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = sRequestID & "-Swipestakes"
        Dim reseedFolder As String = "\CR" & sCRID
        filePath = swipestakeSeedFolder & subFolderName & reseedFolder

        'Dim subFolderName As String = clsSession.CurrRequestID.ToString & "-Swipestakes"
        ''Dim file As String = swipestakesFileName & swipestakesFileExtension

        'Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        'Directory.CreateDirectory(swipestakeSeedFolder & subFolderName)


        fileUpEx.PostedFile.SaveAs(filePath & "\" & file)

        If Path.GetExtension(fileUpEx.PostedFile.FileName) = ".zip" Then
            Dim t As New Boolean
            t = UnZip(file, filePath & "\")

            Dim fileExtractionPath As String = filePath & "\" & Split(file, ".")(0)

            Dim tempFiles() As String
            tempFiles = System.IO.Directory.GetFiles(fileExtractionPath, "*.*")


            '' Validate CSV file before executing update
            Dim validFile As Boolean = True
            For Each fname As String In tempFiles

                Dim records() As String = System.IO.File.ReadAllLines(fname, System.Text.Encoding.Default)

                For i As Integer = 0 To records.Length - 1
                    Dim record() As String = records(i).Split(";")
                    If Not record(0).Replace(",", "").Replace(" ", "") = "" Then
                        ValidateCSV(record(0), Path.GetFileName(fname), i)
                    End If

                Next
            Next


            validFile = IIf(blistErrorMsg.Items.Count() > 0, False, True)

            '' ----------------------------------------------------------------------------------------------
            '' ----------------------------------------------------------------------------------------------
            '' Insert CSV data if no errors found.
            If validFile Then
                For Each fname As String In tempFiles

                    Dim records() As String = System.IO.File.ReadAllLines(fname, System.Text.Encoding.Default)

                    '' Every File Validation
                    For i As Integer = 0 To records.Length - 1
                        Dim record() As String = records(i).Split(";")
                        If Not record(0).Replace(",", "").Replace(" ", "") = "" Then
                            ValidateDuplicate(record(0), Path.GetFileName(fname), i)
                        End If

                    Next

                    For i As Integer = 0 To records.Length - 1
                        Dim record() As String = records(i).Split(";")
                        If Not record(0).Replace(",", "").Replace(" ", "") = "" Then
                            SaveCSV(record(0))
                        End If
                    Next
                Next

                '' Remove
                validFile = IIf(blistErrorMsg.Items.Count() > 0, False, True)

                If Not validFile Then
                    For Each fname As String In tempFiles
                        Dim records() As String = System.IO.File.ReadAllLines(fname, System.Text.Encoding.Default)

                        '' Every File Validation
                        For i As Integer = 0 To records.Length - 1
                            Dim record() As String = records(i).Split(";")
                            If Not record(0).Replace(",", "").Replace(" ", "") = "" Then
                                DeleteRecord(record(0), Path.GetFileName(fname), i)
                            End If

                        Next
                    Next

                    DeleteFile(filePath & "\" & file, file)
                End If
            Else
                DeleteFile(filePath & "\" & file, file)
            End If

            '' ----------------------------------------------------------------------------------------------
            '' ----------------------------------------------------------------------------------------------


        Else
            Dim records() As String = System.IO.File.ReadAllLines(filePath & "\" & file, System.Text.Encoding.Default)


            '' Validate CSV file before executing update
            Dim validFile As Boolean = True

            For i As Integer = 0 To records.Length - 1
                Dim record() As String = records(i).Split(";")
                If Not record(0).Replace(",", "") = "" Then
                    ValidateCSV(record(0), file, i)
                End If
            Next

            validFile = IIf(blistErrorMsg.Items.Count() > 0, False, True)

            '' Insert CSV data if no errors found.
            If validFile Then
                For i As Integer = 0 To records.Length - 1
                    Dim record() As String = records(i).Split(";")
                    If Not record(0).Replace(",", "") = "" Then
                        SaveCSV(record(0))
                    End If
                Next
            Else
                DeleteFile(filePath & "\" & file, file)
            End If
        End If
    End Sub

    Private Sub SaveCSV(ByVal record As String)
        '' SAVE FILE DATA IN DATABASE
        Dim strSQL As String
        Dim sErrMess As String = ""

        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)

        Dim temp() As String = record.Split(",")
        If temp(0).Contains("STORE") Then
            Exit Sub
        End If
        '' '' ---------------------------------------------------------------------------------------------
        'strSQL = "INSERT INTO PromoTemp " & _
        '           "(RequestID, BranchCode, PromoDescription, StartDate, EndDate, Threshold, MaxNumber, Counter, WinningMessage, NonWinningMessage, POSMessage, TenderType, Prize) " & _
        '           "VALUES (" & clsSession.CurrRequestID & ", " & _
        '               "'" & temp(0) & "', " & _
        '               "'" & temp(1) & "', " & _
        '               "'" & temp(2) & "', " & _
        '               "'" & temp(3) & "', " & _
        '               temp(4) & ", " & _
        '               temp(5) & ", " & _
        '               temp(6) & ", " & _
        '               "'" & temp(7) & "', " & _
        '               "'" & temp(8) & "', " & _
        '               "'" & temp(9) & "', " & _
        '               "'" & temp(10) & "', " & _
        '               temp(11) & ")"
        '' validate
        Dim temporaryStartDate As String = Right("00000000" & temp(2), 8)
        Dim temporaryEndDate As String = Right("00000000" & temp(3), 8)

        '' --------------------------------------------------
        Dim temporaryMaxNumber As String = IIf(temp(5) = "", "0", temp(5))
        Dim temporaryCounter As String = IIf(temp(6) = "", "0", temp(6))


        strSQL = "INSERT INTO PromoSeedCR " & _
                   "(RequestID, CRID, CompBranch, StartDate, EndDate, MaxNumber, Counter, Prize) " & _
                   "VALUES (" & sRequestID & ", " & _
                       sCRID & ", " & _
                       "'" & temp(0) & "', " & _
                       "'" & temporaryStartDate.Substring(4, 4) & "-" & temporaryStartDate.Substring(0, 2) & "-" & temporaryStartDate.Substring(2, 2) & "', " & _
                       "'" & temporaryEndDate.Substring(4, 4) & "-" & temporaryEndDate.Substring(0, 2) & "-" & temporaryEndDate.Substring(2, 2) & "', " & _
                       IIf(temporaryCounter = "0", "0", temporaryMaxNumber) & ", " & _
                       IIf(temporaryMaxNumber = "0", "0", temporaryCounter) & ", " & _
                       temp(11) & ")"

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
            'blistErrorMsg.Items.Add("Error in saving Winning Messages: " & sErrMess)
            Exit Sub
        End If

    End Sub

    Private Sub DeleteCSV(ByVal fileName As String, ByVal filePath As String)
        Dim strSQL As String
        Dim sErrMess As String = ""
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)

        If fileName.Substring(fileName.Length - 4, 4) = ".zip" Then

            Dim tempFiles() As String
            tempFiles = System.IO.Directory.GetFiles(filePath.Replace(".zip", ""), "*.*")

            For Each fname As String In tempFiles

                Dim compBranchCode As String = fname.Replace(filePath.Replace(".zip", "") & "\", "").Substring(0, 7)

                '' '' ---------------------------------------------------------------------------------------------
                strSQL = "DELETE PromoSeedCR " & _
                           "WHERE RequestID = " & sRequestID & " and CompBranch = '" & compBranchCode & "'"

                If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
                    'blistErrorMsg.Items.Add("Error in saving Winning Messages: " & sErrMess)
                    Exit Sub
                Else
                    System.IO.File.Delete(fname)
                End If
            Next
            Directory.Delete(filePath.Replace(".zip", ""))
        Else
            Dim compBranchCode As String = fileName.Substring(0, 7)

            strSQL = "DELETE PromoSeedCR " & _
                           "WHERE RequestID = " & sRequestID & " and CompBranch = '" & compBranchCode & "'"

            If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
                'blistErrorMsg.Items.Add("Error in saving Winning Messages: " & sErrMess)
                Exit Sub
            End If

        End If

    End Sub

    Private Sub DeleteRecord(ByVal record As String, ByVal file As String, ByVal lineNumber As Integer)
        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)

        Dim temp() As String = record.Split(",")
        If temp(0).Contains("STORE") Then
            Exit Sub
        End If

        Dim temporaryStartDate As String = Right("00000000" & temp(2), 8)
        Dim temporaryEndDate As String = Right("00000000" & temp(3), 8)
        Dim temporaryCompanyBranchCode As String = Right("0000000" & temp(0), 7)

        Dim strSQL As String
        Dim sErrMess As String = ""
        strSQL = "DELETE PromoSeedCR " & _
                   " WHERE RequestID = " & sRequestID & " and CompBranch = '" & temporaryCompanyBranchCode & "'" & _
                   " AND CRID = " & sCRID & _
                   " AND StartDate = '" & temporaryStartDate.Substring(4, 4) & "-" & temporaryStartDate.Substring(0, 2) & "-" & temporaryStartDate.Substring(2, 2) & "'" & _
                   " AND EndDate = '" & temporaryEndDate.Substring(4, 4) & "-" & temporaryEndDate.Substring(0, 2) & "-" & temporaryEndDate.Substring(2, 2) & "'"


        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strSQL, sErrMess) Then
        End If
    End Sub

    Private Sub ValidateCSV(ByVal record As String, ByVal file As String, ByVal lineNumber As Integer)
        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)

        Dim temp() As String = record.Split(",")
        If temp(0).Contains("STORE") Then
            Exit Sub
        End If

        Dim temporaryCompanyBranchCode As String = Right("0000000" & temp(0), 7)
        '' Validate branch
        Dim strSelectPromoBranch As String
        Dim dtPromoBranch As DataTable = Nothing

        strSelectPromoBranch = "SELECT PB.ShortName" & _
                            " FROM PromoBranch PB WITH(NOLOCK)" & _
                            " INNER JOIN Promotions P WITH(NOLOCK)" & _
                                " ON P.PromoID = PB.PromoID" & _
                            " WHERE(P.RequestID = " & sRequestID & ")" & _
                                " AND CompCode = '" & temp(0).Substring(0, 3) & "'" & _
                                " AND BranchCode = '" & temp(0).Substring(3, 4) & "' "

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strSelectPromoBranch, dtPromoBranch) Then
            If Not dtPromoBranch.Rows.Count() > 0 Then
                blistErrorMsg.Items.Add("File: " & file & ", Line: " & lineNumber + 1 & " - Invalid Branch Code.")
            Else
                ValidateDuplicate(record, file, lineNumber)
            End If
        End If
    End Sub

    Private Sub ValidateDuplicate(ByVal record As String, ByVal file As String, ByVal lineNumber As Integer)
        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)

        Dim temp() As String = record.Split(",")

        Dim temporaryStartDate As String = Right("00000000" & temp(2), 8)
        Dim temporaryEndDate As String = Right("00000000" & temp(3), 8)
        Dim temporaryCompanyBranchCode As String = Right("0000000" & temp(0), 7)

        Dim strSelectSeeding As String
        Dim dtSelectedSeeding As DataTable = Nothing
        strSelectSeeding = "SELECT PSCR.RequestID " & _
                            " FROM PromoSeedCR PSCR WITH(NOLOCK)" & _
                            " WHERE PSCR.RequestID = " & sRequestID & _
                                " AND PSCR.CRID = " & sCRID & _
                                " AND PSCR.StartDate = '" & temporaryStartDate.Substring(4, 4) & "-" & temporaryStartDate.Substring(0, 2) & "-" & temporaryStartDate.Substring(2, 2) & "'" & _
                                " AND PSCR.EndDate = '" & temporaryEndDate.Substring(4, 4) & "-" & temporaryEndDate.Substring(0, 2) & "-" & temporaryEndDate.Substring(2, 2) & "'" & _
                                " AND PSCR.CompBranch = '" & temporaryCompanyBranchCode & "'"

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strSelectSeeding, dtSelectedSeeding) Then
            If dtSelectedSeeding.Rows.Count() > 0 Then
                blistErrorMsg.Items.Add("File: " & file & ", Line:" & lineNumber + 1 & " - Duplicate Reseeding.")
            Else
                ValidatePromoPeriod(record, file, lineNumber)
            End If
        End If

    End Sub

    Private Sub ValidatePromoPeriod(ByVal record As String, ByVal file As String, ByVal lineNumber As Integer)
        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)

        Dim temp() As String = record.Split(",")

        Dim temporaryStartDate As String = Right("00000000" & temp(2), 8)
        Dim temporaryEndDate As String = Right("00000000" & temp(3), 8)
        Dim temporaryCompanyBranchCode As String = Right("0000000" & temp(0), 7)

        Dim strSelectSeeding As String
        Dim dtSelectedSeeding As DataTable = Nothing
        strSelectSeeding = "SELECT PR.RequestID " & _
                            " FROM PromoRequests PR WITH(NOLOCK)" & _
                            " WHERE PR.RequestID = " & sRequestID & _
                                " AND '" & temporaryStartDate.Substring(4, 4) & "-" & temporaryStartDate.Substring(0, 2) & "-" & temporaryStartDate.Substring(2, 2) & "' BETWEEN PromoPeriodFrom AND PromoPeriodTo"

        If clsSystemApp.GetDataTable(clsPromo.SQLConnString, strSelectSeeding, dtSelectedSeeding) Then
            If Not dtSelectedSeeding.Rows.Count() > 0 Then
                blistErrorMsg.Items.Add("File: " & file & ", Line:" & lineNumber + 1 & " - Date is not within Promotion Date.")
            End If
        End If

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
        '' DELETE
        blistErrorMsg.Items.Clear()
        Dim sCRID As String = clsEncryptDecrypt.DecryptText(Request("CRID").ToString, SystemUser.EncryptKey.ToString)
        Dim sRequestID As String = clsEncryptDecrypt.DecryptText(Request("RequestID").ToString, SystemUser.EncryptKey.ToString)
        Dim chkDelete As CheckBox

        Dim i As Integer = 0

        ''
        Dim pathTemp As String
        Dim swipestakeSeedFolder As String = System.Configuration.ConfigurationManager.ConnectionStrings("SwipeStakesSeedFolder").ConnectionString
        Dim subFolderName As String = sRequestID & "-Swipestakes"
        Dim reseedFolder As String = "\CR" & sCRID
        pathTemp = swipestakeSeedFolder & subFolderName & reseedFolder


        For i = 0 To GridView1.Rows.Count - 1
            chkDelete = CType(GridView1.Rows(i).Cells(0).FindControl("CheckBox1"), CheckBox)
            If chkDelete.Checked = True Then
                DeleteFile(pathTemp & "\" & GridView1.Rows(i).Cells(1).Text, GridView1.Rows(i).Cells(1).Text)
            End If
        Next

        fillChildTable(clsSession.AttachmentPath)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        ClientScript.RegisterStartupScript(Me.GetType, "close", "<script>parent.attachmentwindow.hide();</script>")
    End Sub
End Class
