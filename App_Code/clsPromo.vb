Imports Microsoft.VisualBasic
Imports System.DirectoryServices
Imports System.IO
Imports System.Collections.Generic
Imports System.Byte
Imports java.util.zip
Imports java.io
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports dsPromotionsTableAdapters

Public Class clsPromo

    Public Enum Audit As Integer

        UserMaintenance = 1
        Transaction = 2
        Utilities = 3

    End Enum

    Public Enum XMLfieldState As Integer

        UnusedField = 0
        HiddenField = 10
        LockedField = 20
        EnabledField = 30   'enabled, required
        OptionalField = 40  'enabled, optional

    End Enum

#Region " Connection String "

    Private Shared Function sConn() As String
        sConn = ConfigurationManager.ConnectionStrings("PromoConnectionString").ConnectionString
        Return sConn
    End Function

    Public Shared Property SQLConnString() As String
        Get
            Return sConn()
        End Get
        Set(ByVal Value As String)
            Value = sConn()
        End Set
    End Property

    Private Shared Function sAttachment() As String
        sAttachment = ConfigurationManager.ConnectionStrings("AttachFileDrive").ConnectionString
        Return sAttachment
    End Function

    Public Shared Property pathAttachment() As String
        Get
            Return sAttachment()
        End Get
        Set(ByVal Value As String)
            Value = sAttachment()
        End Set
    End Property

#End Region

#Region "Zip File"

    Public Shared Sub CreateZipFile(ByVal sPath As String, ByVal flag As Integer)
        Dim fos As java.io.FileOutputStream
        Dim zos As java.util.zip.ZipOutputStream
        Dim di As System.IO.DirectoryInfo
        Dim pth As String

        fos = Nothing
        zos = Nothing

        If flag = 0 Then
            pth = Left(sPath, Len(sPath) - 4)
        Else
            pth = sPath
        End If


        'check , is it a file existing in this path,   if true then zip a file
        If System.IO.File.Exists(pth) Then
            Dim fInfo As New FileInfo(pth)

            'create a zip file with same name in the same path
            fos = New java.io.FileOutputStream(pth.Replace(fInfo.Extension, ".zip"))
            zos = New java.util.zip.ZipOutputStream(fos)

            'procedure to zip one File
            ZipOneFile(fos, zos, sPath)

            'check , is it a directory existing in this path,if true then zip a directory
        ElseIf System.IO.Directory.Exists(pth) Then

            'create a zip file with same name in the same path
            fos = New java.io.FileOutputStream(pth & ".zip")
            zos = New java.util.zip.ZipOutputStream(fos)
            di = New System.IO.DirectoryInfo(pth)

            'procedure to zip a directory
            ZipDirectory(fos, zos, di, pth)
        End If


        zos.close()
        fos.close()

        zos.flush()
        fos.flush()
    End Sub

    Private Shared Sub ZipDirectory(ByVal fos As java.io.FileOutputStream, ByVal zos As java.util.zip.ZipOutputStream, ByVal di As System.IO.DirectoryInfo, ByVal SRootDir As String)
        Dim fis As java.io.FileInputStream
        Dim ze As java.util.zip.ZipEntry

        'to get file info from the directory
        Dim fInfos As System.IO.FileInfo() = di.GetFiles
        Dim fInfo As System.IO.FileInfo

        For Each fInfo In fInfos
            'give the zip entry or the folder arrangement for the file
            ze = New java.util.zip.ZipEntry(fInfo.FullName.Substring(SRootDir.LastIndexOf("\")))

            'The DEFLATED method is the one of the methods to zip a file
            ze.setMethod(ZipEntry.DEFLATED)

            zos.putNextEntry(ze)

            'Input stream for the file to zip
            fis = New java.io.FileInputStream(fInfo.FullName)

            'Copy stream is a simple method to read a file input stream (file to zip) and write it to a file output stream(new zip file)
            CopyStream(fis, zos)

            zos.closeEntry()
            fis.close()
        Next

        'If the directory contains the sub directory the call the same procedure
        Dim dinfos As System.IO.DirectoryInfo() = di.GetDirectories()
        Dim dinfo As System.IO.DirectoryInfo
        For Each dinfo In dinfos
            ZipDirectory(fos, zos, dinfo, SRootDir)
        Next

    End Sub

    Private Shared Sub ZipOneFile(ByVal fos As java.io.FileOutputStream, ByVal zos As java.util.zip.ZipOutputStream, ByVal sFullName As String)
        Dim fis As java.io.FileInputStream
        Dim ze As java.util.zip.ZipEntry
        'give the zip entry or the folder arrangement for the file
        ze = New java.util.zip.ZipEntry(sFullName.Substring(sFullName.LastIndexOf("\")))
        'The DEFLATED method is the one of the methods to zip a file

        ze.setMethod(ZipEntry.DEFLATED)
        zos.putNextEntry(ze)

        'Input stream for the file to zip
        fis = New java.io.FileInputStream(sFullName)
        'Copy stream is a simple method to read a file input stream (file to zip) and write it to a file output stream(new zip file)
        CopyStream(fis, zos)
        zos.closeEntry()
        fis.close()
    End Sub

    Private Shared Sub CopyStream(ByVal src As java.io.FileInputStream, ByVal dest As java.util.zip.ZipOutputStream)
        Dim reader As New java.io.InputStreamReader(src)
        Dim writer As New java.io.OutputStreamWriter(dest)
        While reader.ready
            writer.write(reader.read)
        End While
        writer.flush()
    End Sub

#End Region

    Public Shared Sub InsertAuditTrail(ByVal EntryType As String, ByVal EntryID As Integer, ByVal EntryAction As String, ByVal UserId As String, ByVal Description As String)
        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As Data.SqlClient.SqlCommand
        sqlConn.Open()
        sqlCmd = New Data.SqlClient.SqlCommand
        sqlCmd.CommandText = "USP_InsertAuditTrail"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = 4

        sqlCmd.Parameters.Add("@EntryType", SqlDbType.VarChar)
        sqlCmd.Parameters("@EntryType").Value = EntryType

        sqlCmd.Parameters.Add("@EntryID", SqlDbType.VarChar)
        sqlCmd.Parameters("@EntryID").Value = EntryID

        sqlCmd.Parameters.Add("@EntryAction", SqlDbType.VarChar)
        sqlCmd.Parameters("@EntryAction").Value = EntryAction

        sqlCmd.Parameters.Add("@UserId", SqlDbType.VarChar)
        sqlCmd.Parameters("@UserId").Value = UserId

        sqlCmd.Parameters.Add("@Description", SqlDbType.VarChar)
        sqlCmd.Parameters("@Description").Value = Description

        sqlCmd.ExecuteNonQuery()

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()

        sqlConn = Nothing
        sqlCmd = Nothing

    End Sub

    ' Procedure designed for tagging of status. (with validation to eliminate double posting)
    Public Shared Sub CRStatus(ByVal _CRID As Integer _
                        , ByVal _MemoID As Integer _
                        , ByVal _RequestID As Integer _
                        , ByVal _MemoNumber As String _
                        , ByVal _RequestType As String _
                        , ByVal _Status As String _
                        , ByVal _Remarks As String _
                        , ByVal _ApproverID As Integer _
                        , ByVal _EntryType As String _
                        , ByVal _EntryAction As String _
                        , ByVal _AuditDesc As String)

        Dim sqlConn As Data.SqlClient.SqlConnection
        sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As Data.SqlClient.SqlCommand

        sqlConn.Open()
        sqlCmd = New Data.SqlClient.SqlCommand
        sqlCmd.CommandText = "PMS_P_ChangeRequests_Stat"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@CRID", SqlDbType.Int)
        sqlCmd.Parameters("@CRID").Value = _CRID

        sqlCmd.Parameters.Add("@MemoID", SqlDbType.Int)
        sqlCmd.Parameters("@MemoID").Value = _MemoID

        sqlCmd.Parameters.Add("@RequestID", SqlDbType.Int)
        sqlCmd.Parameters("@RequestID").Value = _RequestID

        sqlCmd.Parameters.Add("@MemoNumber", SqlDbType.VarChar, 20)
        sqlCmd.Parameters("@MemoNumber").Value = _MemoNumber


        sqlCmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 10)
        sqlCmd.Parameters("@RequestType").Value = _RequestType

        sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar, 30)
        sqlCmd.Parameters("@Status").Value = _Status

        sqlCmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 200)
        sqlCmd.Parameters("@Remarks").Value = _Remarks

        sqlCmd.Parameters.Add("@ApproverID", SqlDbType.SmallInt)
        sqlCmd.Parameters("@ApproverID").Value = _ApproverID


        sqlCmd.Parameters.Add("@EntryType", SqlDbType.VarChar, 100)
        sqlCmd.Parameters("@EntryType").Value = _EntryType

        sqlCmd.Parameters.Add("@EntryAction", SqlDbType.VarChar, 8000)
        sqlCmd.Parameters("@EntryAction").Value = _EntryAction

        sqlCmd.Parameters.Add("@AuditDesc", SqlDbType.VarChar, 8000)
        sqlCmd.Parameters("@AuditDesc").Value = _AuditDesc

        sqlCmd.ExecuteNonQuery()

        ' close and dispose connection
        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

    End Sub


    Public Shared Sub renameFolder(ByVal path As String, ByVal Fromfol As String, ByVal Tofol As String)
        My.Computer.FileSystem.RenameDirectory(path + Fromfol, Tofol)
        GC.Collect()
    End Sub


    Public Shared Function FolderExists(ByVal sFullPath) As Object
        Dim myFSO
        myFSO = CreateObject("Scripting.FileSystemObject")
        FolderExists = myFSO.FolderExists(sFullPath)
        GC.Collect()
    End Function


    Public Shared Function OpenDBFConn(ByVal FolderPath) As Object
        Dim Conn
        Conn = CreateObject("ADODB.Connection")
        Conn.Open("Provider=VFPOLEDB.1;Data Source='" & FolderPath & "';Collating Sequence=MACHINE")
        OpenDBFConn = Conn
    End Function


    Public Shared Sub CreateFolder(ByVal strDirectory)
        Dim objFSO, objFolder

        objFSO = CreateObject("Scripting.FileSystemObject")

        ' Note If..Exists. Then, Else ... End If construction
        If objFSO.FolderExists(strDirectory) Then
            objFolder = objFSO.GetFolder(strDirectory)
        Else
            objFolder = objFSO.CreateFolder(strDirectory)
        End If
    End Sub


    Public Shared Function ValidateNewlyCreatedPosFile(ByVal strQuery As String, ByVal path As String) As String
        Dim DBFConn As OleDb.OleDbConnection
        Dim str As String = String.Empty
        DBFConn = New OleDb.OleDbConnection("Provider=VFPOLEDB.1;Data Source='" & path & "';Collating Sequence=MACHINE")
        Dim cmd As New Data.OleDb.OleDbCommand(strQuery, DBFConn)
        Try
            Dim da As New Data.OleDb.OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            DBFConn.Open()
            da.Fill(ds, "tblDBF")
            If ds.Tables(0).Rows.Count <> 0 Then
                str = ds.Tables(0).Rows(0)(0).ToString & "!@#" & ds.Tables(0).Rows(0)(1).ToString
            End If
            Return str
        Catch ex As Exception
        Finally
            DBFConn.Close()
            DBFConn.Dispose()
            cmd.Dispose()
            GC.Collect()
        End Try
        Return ""
    End Function


    Public Shared Function FetchDataItem(ByVal DataTableName As DataTable, ByVal intRow As Integer, ByVal FieldName As String) As Object

        Dim Value As Object

        If Not DataTableName.Rows(intRow).IsNull(FieldName) Then

            Value = (DataTableName.Rows(intRow).Item(FieldName) & Nothing)

        Else

            Value = String.Empty

        End If

        Return Value

    End Function


    Public Shared Sub jsSetFocus(ByVal PageName As System.Web.UI.Page, ByVal TargetClientID As String)

        Dim _JavaScript As String

        _JavaScript = "document.getElementById('" & TargetClientID.ToString & "').focus();" & _
                           "document.getElementById('" & TargetClientID.ToString & "').select();"

        PageName.ClientScript.RegisterStartupScript(PageName.GetType(), "SetFocus", "<script language='javascript'>" & _JavaScript & "</script>")

    End Sub


    Public Shared Sub FetchDropDownList(ByVal DataTable As System.Data.DataTable, ByRef DropDownList As Web.UI.WebControls.DropDownList, ByVal SearchType As String, ByVal SelectedValue As String, ByVal Map1 As String, ByVal Map2 As String, ByVal Map3 As String, ByVal DefOptionText As String, ByVal WithOtherValue As Boolean, Optional ByVal SortText As String = "SEQNO")

        Dim dtbl As DataTable = DataTable
        Dim dtnew As DataTable = Nothing
        Dim dvMenuGroup As New DataView(dtbl)
        Dim _RowFilter = " SEARCHTYPE = '" & SearchType & "' "
        Try

            If Not Map1 = Nothing Then

                _RowFilter &= " AND MAP1 = '" & IIf(Not Map1 = String.Empty, Map1, "A").ToString & "' "

            End If

            If Not Map2 = Nothing Then

                _RowFilter &= " AND MAP2 = '" & IIf(Not Map2 = String.Empty, Map2, "A").ToString & "' "

            End If

            If Not Map3 = Nothing Then

                _RowFilter &= " AND MAP3 = '" & IIf(Not Map3 = String.Empty, Map3, "A").ToString & "' "

            End If

            dvMenuGroup.Sort = SortText
            dvMenuGroup.RowFilter = _RowFilter

            dtnew = dtbl.Clone

            For Each dvr As DataRowView In dvMenuGroup
                dtnew.ImportRow(dvr.Row)
            Next

            With DropDownList

                .DataSource = dtnew.Copy
                .DataTextField = "SEARCHTEXT"
                .DataValueField = "SEARCHVALUE"
                .DataBind()

                If Not Trim(DefOptionText) = Nothing Then

                    .Items.Insert(0, New ListItem(DefOptionText, String.Empty))

                End If

                If Not Trim(SelectedValue) = Nothing Then

                    .SelectedValue = SelectedValue

                End If


                If WithOtherValue = True Then

                    .Items.Insert(dtnew.Rows.Count + 1, New ListItem("Others", "XX"))

                End If

            End With

        Catch ex As Exception

            Throw ex

        End Try

        dtnew = Nothing
        dvMenuGroup = Nothing
        dtbl = Nothing

    End Sub

    ' Added 07162012@smretailinc
    Public Shared Function formatParamValue(ByVal ParamValue As Object, Optional ByVal IsUpperCase As Boolean = False) As Object

        If Trim(ParamValue) = "" Then

            Return DBNull.Value
        Else

            Try

                If IsUpperCase Then

                    Return Trim(ParamValue).ToUpper.ToString

                Else

                    Return Trim(ParamValue).ToString

                End If


            Catch ex As Exception

                Return DBNull.Value

            End Try

        End If

    End Function

    ' Added 07162012@smretailinc: format date to MM/dd/yyyy
    Public Shared Function IsSMDate(ByVal DateString As String) As Boolean

        If IsDate(DateString) Then

            If DateString.Substring(DateString.LastIndexOf("/") + 1).Length = 4 Then

                IsSMDate = True

            Else

                IsSMDate = False

            End If

        Else

            IsSMDate = False

        End If

    End Function

    Public Shared Function CheckBoxImage(ByVal Key As String, ByVal Value As String) As String

        If Key = Value Then

            Return "~/images/1.gif"

        Else

            Return "~/images/0.gif"

        End If

    End Function

    ' added 20150109@smretailinc: to filter data table
    Public Shared Function FilterDataTable(ByVal DataTable As DataTable, ByVal RowFilter As String, Optional ByVal SortBy As String = Nothing) As DataTable

        Dim dtbl As DataTable = DataTable
        Dim dtnew As DataTable = Nothing
        Dim DataView As New DataView(dtbl)

        Try

            If SortBy IsNot Nothing Then
                DataView.Sort = SortBy
            End If

            DataView.RowFilter = RowFilter
            dtnew = dtbl.Clone

            For Each dvr As DataRowView In DataView

                dtnew.ImportRow(dvr.Row)

            Next

        Catch ex As Exception

            Throw ex
            Exit Function

        End Try

        Return dtnew.Copy()

        dtnew = Nothing
        DataView = Nothing
        dtbl = Nothing

    End Function

    Public Shared Function ConvertArraytoString(ByVal ArrayValue() As String, Optional ByVal Deliminator As String = ",") As String

        Dim strValue As String, strData As String

        Try

            strData = ""

            For Each strValue In ArrayValue

                If Not strValue Is Nothing And Trim(strValue) <> "" Then

                    strData += Trim(strValue) & Deliminator

                End If

            Next

            strData = Left(strData, Len(strData) - 1)

            Return strData

        Catch ex As Exception

            Return ""

        End Try

    End Function

    ' get state of promo request entry buttons
    Public Shared Function GetAddRequestButtonState() As Boolean

        Dim bReturnValue As Boolean = True
        Dim strQuery As String

        ' check restriction from calendar settings
        strQuery = "SELECT COUNT(*) " & _
                    "FROM SystemEventCalendar " & _
                    "WHERE EventType = 'PromoEntryDownTime' AND Disabled = 0 " & _
                    "AND DATEDIFF(d, PeriodFrom, GETDATE()) >= 0 " & _
                    "AND DATEDIFF(d, GETDATE(), PeriodTo) >= 0 "

        bReturnValue = (clsSystemApp.ExecuteScalarCommand(clsPromo.SQLConnString, strQuery) = 0)

        Return bReturnValue

    End Function

    'Public Shared Function GetPromoTypeLeadDays(ByVal PromoTypeID As Integer) As Integer

    '    Dim sqlConn As Data.SqlClient.SqlConnection
    '    Dim sqlCmd As Data.SqlClient.SqlCommand

    '    Dim nNumLeadDays As Integer

    '    sqlConn = New Data.SqlClient.SqlConnection(clsPromo.SQLConnString())
    '    sqlConn.Open()

    '    sqlCmd = New Data.SqlClient.SqlCommand
    '    sqlCmd.CommandText = "SELECT NumLeadDays FROM PromoTypes WHERE PromoTypeID = 0" & PromoTypeID
    '    sqlCmd.Connection = sqlConn
    '    sqlCmd.CommandTimeout = 0
    '    sqlCmd.CommandType = CommandType.Text

    '    'sqlCmd.Parameters.Add("@EntryType", SqlDbType.VarChar)
    '    'sqlCmd.Parameters("@EntryType").Value = EntryType

    '    'sqlCmd.Parameters.Add("@EntryID", SqlDbType.VarChar)
    '    'sqlCmd.Parameters("@EntryID").Value = EntryID

    '    'sqlCmd.Parameters.Add("@EntryAction", SqlDbType.VarChar)
    '    'sqlCmd.Parameters("@EntryAction").Value = EntryAction

    '    'sqlCmd.Parameters.Add("@UserId", SqlDbType.VarChar)
    '    'sqlCmd.Parameters("@UserId").Value = UserId

    '    'sqlCmd.Parameters.Add("@Description", SqlDbType.VarChar)
    '    'sqlCmd.Parameters("@Description").Value = Description

    '    nNumLeadDays = sqlCmd.ExecuteScalar

    '    If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()

    '    sqlConn = Nothing
    '    sqlCmd = Nothing

    '    Return nNumLeadDays

    'End Function

    Public Shared Function GetPromoGroupType(ByVal requestID As Integer) As String
        Dim strQuery As String = ""
        Dim drRow As DataRow = Nothing
        Dim groupType As String = ""

        strQuery = "SELECT U.GroupType FROM PromoRequests PR INNER JOIN UserGroups U " & _
                    "ON PR.OwnerGroup = U.GroupID WHERE PR.RequestID = 0" & requestID
        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
            groupType = drRow("GroupType").ToString
        End If

        Return groupType

    End Function

    Public Shared Function GetNextApprover(ByVal requestID As Integer) As String
        'chkXML_eCard1.Checked = (InStr(sValues, "01;") > 0)
        Dim strQuery As String = ""
        Dim drRow As DataRow = Nothing
        Dim nxt As String = ""

        'strQuery = "SELECT UG2.ApprovalFlow FROM Users U " & _
        '            "Left Join GroupAssignment GA ON U.UserID = GA.UserID " & _
        '            "LEFT JOIN UserGroups UG ON UG.GroupID = GA.GroupID " & _
        '            "INNER JOIN UserGroups UG2 ON UG2.BizUnit = UG.BizUnit AND UG2.DeptCode IS NULL " & _
        '            "WHERE U.UserID = '" & userID & "' AND UG2.GroupType = '" & SystemUser.UserGroupType & "'"

        strQuery = "SELECT U.ApprovalFlow FROM PromoRequests PR INNER JOIN UserGroups U " & _
                    "ON PR.OwnerGroup = U.GroupID WHERE PR.RequestID = 0" & requestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drRow) Then
            Dim appFlow() As String = drRow("ApprovalFlow").ToString.Split(New Char() {","c})
            Dim ctr As Integer = 0

            For Each v As String In appFlow
                If v = SystemUser.UserLevel Then
                    Exit For
                End If
                ctr = ctr + 1
            Next

            If ctr + 1 < appFlow.Length Then
                nxt = appFlow(ctr + 1).ToString
            Else
                nxt = ""
            End If
        End If

        Return nxt

    End Function

    Public Shared Function GetDocStatus(ByVal requestID As Integer, ByVal nxtApprover As Integer, Optional ByVal WorkFlowCode As String = "") As String
        Dim DocStatus As String = ""
        Dim isTemplated As Boolean = IsTemplatedPromo(clsSession.CurrMemoID)
        Dim GrpType As String = GetPromoGroupType(requestID)

        If GrpType = "SBU" Then
            Select Case nxtApprover
                Case "75"
                    DocStatus = "For Mdsg/Group Head Approval"
                Case "70"
                    DocStatus = "For BU Head Approval"
                Case "40"
                    DocStatus = "For MPD Processing"
                Case "30"
                    If isTemplated Then
                        DocStatus = "Approved"
                    Else
                        DocStatus = "For Review"
                    End If
                Case "20"
                    DocStatus = "Approved"
            End Select
        End If

        If GrpType = "REGULAR" Then
            Select Case nxtApprover
                Case "75"
                    DocStatus = "For Mdsg/Group Head Approval"
                Case "70"
                    DocStatus = "For MBU Approval"
                Case "40"
                    If WorkFlowCode = "10" Then
                        DocStatus = "Approved"
                    Else
                        DocStatus = "For MPD Processing"
                    End If
                Case "30"
                    DocStatus = "For Review"
                Case "20"
                    DocStatus = "Approved"
            End Select
        End If

        Return DocStatus
    End Function

    Public Shared Function IsTemplatedPromo(ByVal nMemoID As Long) As Boolean

        Dim strQuery As String
        Dim drPromoType As DataRow = Nothing
        Dim bResult As Boolean = False

        'strQuery = "SELECT TOP 1 P.PromoID, T.PromoTypeID, T.IsTemplated " & _
        '    "FROM Promotions AS P " & _
        '    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
        '    "WHERE P.MemoID = 0" & nMemoID & " " & _
        '    "ORDER BY P.PromoID"

        'CHECK_HERE! Remove hardcoding for Owner GroupID 99 (3-Day Sale)
        'temp fix: bypass promos requested prior 08/01/2018
        strQuery = "SELECT TOP 1 P.MemoID, R.RequestDate, P.PromoID, T.PromoTypeID, T.IsTemplated " & _
                    "FROM PromoRequests AS R " & _
                    "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                    "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                    "WHERE P.MemoID = 0" & nMemoID & " " & _
                    "AND DATEDIFF(d, '2018-08-01', R.RequestDate) >= 0 " & _
                    "AND R.OwnerGroup <> 99 " & _
                    "ORDER BY P.PromoID"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drPromoType) Then
            bResult = (drPromoType("IsTemplated") = 1)
        Else
            ' error
        End If

        Return bResult

    End Function

    Public Shared Function CreateDefaultMemo(ByVal nRequestID As Long, ByVal sNewStatus As String) As Boolean

        Dim lReturnValue As Boolean
        Dim strQuery As String
        Dim strUserInfo As String = ""
        Dim sGuidelineQuery As String = ""
        Dim drUser As DataRow = Nothing

        If IsTemplatedPromo(clsSession.CurrMemoID) Then
            sGuidelineQuery = "R.TemplatedGuideline"
        Else
            sGuidelineQuery = "T.DefaultGuideline"
        End If

        If sNewStatus = "Approved" Then

            Dim sNewMemoNumber As String = CreateNewMemoNumber()

            strQuery = "INSERT INTO Memos " & _
                        "(MemoNumber, MemoDate, Title, Branches, PromoPeriodFrom, PromoPeriodTo, Guidelines, RequestID, PreparedBy, PreparePos, ReviewedBy, ReviewerPos, ReviewDate, ApprovedBy, ApproverPos, ApproveDate, OwnerGroup, Remarks, Status, UserID, POSFile, EmailFg, CRFLAG) " & _
                        "SELECT '" & sNewMemoNumber & "', GETDATE(), R.Title, R.Branches, R.PromoPeriodFrom, R.PromoPeriodTo, " & sGuidelineQuery & ", R.RequestID, R.ReviewedBy, R.RequesterPos, R.ReviewedBy, R.ReviewerPos, R.ReviewDate, R.ApprovedBy, R.ApproverPos, GETDATE(), R.OwnerGroup, '', '" & sNewStatus & "', R.UserID, 0, 0, 'R' " & _
                        "FROM PromoRequests AS R " & _
                        "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                        "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                        "WHERE R.RequestID = 0" & nRequestID & "; " & _
                        "UPDATE Promotions SET MemoID = SCOPE_IDENTITY() " & _
                        "WHERE RequestID = 0" & nRequestID & "; "
        Else

            ' get user information
            strUserInfo = "SELECT SignName, SignPosition FROM Users WHERE UserID = " & SystemUser.UserID

            clsSystemApp.GetDataRow(clsPromo.SQLConnString, strUserInfo, drUser)
            'Dim drUser As DataRow = dvUsers.Table.Rows(0)

            strQuery = "INSERT INTO Memos " & _
                            "(MemoDate, Title, Branches, PromoPeriodFrom, PromoPeriodTo, Guidelines, RequestID, PreparedBy, PreparePos, ReviewedBy, ReviewerPos, ReviewDate, ApprovedBy, ApproverPos, OwnerGroup, Remarks, Status, UserID, POSFile, EmailFg, CRFLAG) " & _
                            "SELECT GETDATE(), R.Title, R.Branches, R.PromoPeriodFrom, R.PromoPeriodTo, " & sGuidelineQuery & ", R.RequestID, R.RequestedBy, R.ReviewerPos, '" & drUser("SignName") & "', '" & drUser("SignPosition") & "', GETDATE(),  '', '', R.OwnerGroup, '', '" & sNewStatus & "', " & SystemUser.UserID & ", 0, 0, 'R' " & _
                            "FROM PromoRequests AS R " & _
                            "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
                            "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
                            "WHERE R.RequestID = 0" & nRequestID & "; " & _
                            "UPDATE Promotions SET MemoID = SCOPE_IDENTITY() " & _
                            "WHERE RequestID = 0" & nRequestID & "; "

            'strQuery = "INSERT INTO Memos " & _
            '            "(MemoDate, Title, Branches, PromoPeriodFrom, PromoPeriodTo, Guidelines, RequestID, PreparedBy, PreparePos, ReviewedBy, ReviewerPos, ReviewDate, ApprovedBy, ApproverPos, ApproveDate, OwnerGroup, Remarks, Status, UserID, POSFile, EmailFg, CRFLAG) " & _
            '            "SELECT GETDATE(), R.Title, R.Branches, R.PromoPeriodFrom, R.PromoPeriodTo, " & sGuidelineQuery & ", R.RequestID, R.RequestedBy, R.RequesterPos, R.ReviewedBy, R.ReviewerPos, R.ReviewDate, R.ApprovedBy, R.ApproverPos, GETDATE(), R.OwnerGroup, '', '" & sNewStatus & "', " & SystemUser.UserID & ", 0, 0, 'R' " & _
            '            "FROM PromoRequests AS R " & _
            '            "INNER JOIN Promotions AS P ON P.RequestID = R.RequestID " & _
            '            "INNER JOIN PromoTypes AS T ON T.PromoTypeID = P.PromoTypeID " & _
            '            "WHERE R.RequestID = 0" & nRequestID & "; " & _
            '            "UPDATE Promotions SET MemoID = SCOPE_IDENTITY() " & _
            '            "WHERE RequestID = 0" & nRequestID & "; "

        End If

        If Not clsSystemApp.ExecuteNonQueryCommand(clsPromo.SQLConnString, strQuery, "") Then
            lReturnValue = False
        Else
            lReturnValue = True
        End If

    End Function

    Public Shared Function CreateNewMemoNumber() As String
        Dim sNewMemoNum As String = ""
        Dim strQuery As String = ""
        Dim drMemoNo As DataRow = Nothing

        ' get last memo series for this BizUnit

        strQuery = "SELECT TOP 1 MemoNumber, LEFT(RIGHT(MemoNumber,7),4) AS LastMemoCode, G.BizUnit " & _
                                    "FROM Memos " & _
                                    "LEFT JOIN UserGroups AS G ON Memos.OwnerGroup = G.GroupID " & _
                                    "WHERE Memos.MemoNumber IS NOT NULL " & _
                                    "AND RIGHT(Memos.MemoNumber,2) = RIGHT(CAST(Year(GetDate()) AS varchar(4)),2) " & _
                                    "AND G.BizUnit IN (SELECT BizUnit FROM UserGroups WHERE GroupID = 0" & GetPromoGroupType(clsSession.CurrRequestID) & ") " & _
                                    "ORDER BY LastMemoCode DESC"

        'Dim dvMemos As DataView = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drMemoNo) Then
            If drMemoNo.Table.Rows.Count() = 0 Then
                strQuery = "SELECT BizUnit FROM UserGroups WHERE GroupID = " & GetPromoGroupType(clsSession.CurrRequestID) 'ViewState("OwnerGroup")
                'dvMemos = CType(sqldsData.Select(DataSourceSelectArguments.Empty), DataView)

                If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strQuery, drMemoNo) Then
                    If drMemoNo.Table.Rows.Count() > 0 Then
                        sNewMemoNum = "MPD-" & UCase(Left(drMemoNo("BizUnit"), 4)) & "-0001" & Format(Now, "-yy")
                    Else
                        sNewMemoNum = "ERROR"
                    End If
                End If
            Else
                sNewMemoNum = "MPD-" & UCase(Left(drMemoNo("BizUnit"), 4)) & Format(drMemoNo("LastMemoCode") + 1, "-0###") & Format(Now, "-yy")
            End If
        End If

        CreateNewMemoNumber = sNewMemoNum
    End Function

End Class

