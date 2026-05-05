Imports Microsoft.VisualBasic
Imports System.DirectoryServices
Imports dsPromotionsTableAdapters
Imports System.Web

Public Class SystemUser

    Public Enum UserRoles As Integer
        Administrator = 10
        MemoApprover = 20
        Reviewer = 30
        Analyst = 40                    ' MPD Analyst (preparation of memo draft)
        SMACapprover = 50               ' SMAC Deals approver
        ExecutiveApprover = 60          ' HTS (handling all BUs)
        RequestApprover = 70            ' MBU head
        RequestReviewer = 75            ' Merchandise/Group Head
        PromoRequestor = 80             ' buyers / merchandisers
        POSpersonnel = 90               ' POS
        AnnouncementViewer = 100        ' branch operation users
        BatchSaleRequestor = 110        ' SCO users for corporate sales
    End Enum

    Private Shared _UserID As String = "UserID"
    Private Shared _UserName As String = "UserName"
    Private Shared _UserLevel As String = "UserLevel"
    Private Shared _UserGroupID As String = "UserGroupID"
    Private Shared _UserGroupType As String = "UserGroupType"
    Private Shared _UserBizUnit As String = "UserBizUnit"       'additional information
    Private Shared _UserBranch As String = "UserBranch"
    Private Shared _UserLogName As String = "UserLogName"
    Private Shared _UserSignName As String = "UserSignName"
    Private Shared _UserSignPosition As String = "UserSignPosition"
    Private Shared _UserAccessSettings As String = "UserAccessSettings"
    Private Shared _EncryptKey As String = "EncryptKey"

    Public Shared Sub LogOut()
        ' clear user session data
        SystemUser.UserName = Nothing
        SystemUser.UserLogName = Nothing

        SystemUser.UserID = 0
        SystemUser.UserLevel = 0
        SystemUser.UserGroupID = 0
        SystemUser.UserGroupType = Nothing
        SystemUser.UserBizUnit = Nothing

        SystemUser.UserSignName = Nothing
        SystemUser.UserSignPosition = Nothing

        SystemUser.UserAccessSettings = 0
        _EncryptKey = Nothing
    End Sub

    Public Shared Property UserAccessSettings() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_UserAccessSettings) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_UserAccessSettings))
            End If
        End Get

        Private Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_UserAccessSettings)
            Else
                HttpContext.Current.Session(_UserAccessSettings) = value
            End If
        End Set

    End Property

    Public Shared Property UserName() As String
        Get
            If (HttpContext.Current.Session(_UserName) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_UserName).ToString()
            End If
        End Get

        Private Set(ByVal value As String)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove(_UserName)
            Else
                HttpContext.Current.Session(_UserName) = value
            End If
        End Set
    End Property

    Public Shared Property UserGroupID() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_UserGroupID) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_UserGroupID))
            End If
        End Get

        Private Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_UserGroupID)
            Else
                HttpContext.Current.Session(_UserGroupID) = value
            End If
        End Set
    End Property

    'Added 09102012@smretailinc: Additional session for group type.
    Public Shared Property UserGroupType() As String
        Get
            If (HttpContext.Current.Session(_UserGroupType) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_UserGroupType).ToString()
            End If
        End Get

        Private Set(ByVal value As String)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove(_UserGroupType)
            Else
                HttpContext.Current.Session(_UserGroupType) = value
            End If
        End Set
    End Property

    '20190930::additional session for business unit.
    Public Shared Property UserBizUnit() As String
        Get
            If (HttpContext.Current.Session(_UserBizUnit) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_UserBizUnit).ToString()
            End If
        End Get

        Private Set(ByVal value As String)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove(_UserBizUnit)
            Else
                HttpContext.Current.Session(_UserBizUnit) = value
            End If
        End Set
    End Property

    'Added 12112012@smretailinc: Additional session for encryption key.
    Public Shared Property EncryptKey() As String
        Get
            If (HttpContext.Current.Session(_EncryptKey) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_EncryptKey).ToString()
            End If
        End Get

        Private Set(ByVal value As String)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove(_EncryptKey)
            Else
                HttpContext.Current.Session(_EncryptKey) = value
            End If
        End Set
    End Property

    Public Shared Property UserLogName() As String
        Get
            If (HttpContext.Current.Session(_UserLogName) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_UserLogName).ToString()
            End If
        End Get

        Private Set(ByVal value As String)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove(_UserLogName)
            Else
                HttpContext.Current.Session(_UserLogName) = value
            End If
        End Set
    End Property

    Public Shared Property UserID() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_UserID) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_UserID))
            End If
        End Get

        Private Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_UserID)
            Else
                HttpContext.Current.Session(_UserID) = value
            End If
        End Set

    End Property

    Public Shared Property UserLevel() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_UserLevel) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_UserLevel))
            End If

        End Get

        Private Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_UserLevel)
            Else
                HttpContext.Current.Session(_UserLevel) = value
            End If
        End Set

    End Property

    Private Shared Function GetOrgUnitString(ByVal DistName As String) As String
        Dim sUnitKeys As String = "|"
        Dim sKeys() As String

        sKeys = DistName.Split(",")

        For Each s As String In sKeys
            If Left(s, 3) = "OU=" Then
                sUnitKeys &= Mid(s, 4) & "|"
            End If
        Next

        GetOrgUnitString = sUnitKeys

    End Function

    Public Shared Property UserSignName() As String
        Get
            If (HttpContext.Current.Session(_UserSignName) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_UserSignName).ToString()
            End If
        End Get

        Private Set(ByVal value As String)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove(_UserSignName)
            Else
                HttpContext.Current.Session(_UserSignName) = value
            End If
        End Set
    End Property

    Public Shared Property UserSignPosition() As String
        Get
            If (HttpContext.Current.Session(_UserSignPosition) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_UserSignPosition).ToString()
            End If
        End Get

        Private Set(ByVal value As String)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove(_UserSignPosition)
            Else
                HttpContext.Current.Session(_UserSignPosition) = value
            End If
        End Set
    End Property

    Private Shared Function UpdateLastLoginDate() As Boolean

        'Dim dr As Data

    End Function

    Private Shared Function LoadDataFromUserTable(ByVal UserLogName As String) As Boolean

        Dim taUsers As New dsPromotionsTableAdapters.UsersTableAdapter()
        Dim dtUsers As dsPromotions.UsersDataTable
        Dim rowUser As dsPromotions.UsersRow
        Dim bResult As Boolean

        SystemUser.UserLogName = UserLogName 'REMOVE THIS IF DEPLOYMENT
        dtUsers = taUsers.GetUserByUserName(SystemUser.UserLogName)

        If dtUsers.Rows.Count = 0 Then
            bResult = False
        Else
            rowUser = dtUsers.Rows(0)

            With rowUser
                SystemUser.UserLevel = .UserLevel
                SystemUser.UserID = .UserID
                SystemUser.UserSignName = .SignName
                SystemUser.UserSignPosition = .SignPosition
                SystemUser.UserGroupID = .GroupID

                Try
                    SystemUser.UserGroupType = .GroupType
                Catch ex As Exception
                    SystemUser.UserGroupType = ""
                End Try

                Try
                    SystemUser.UserBizUnit = .BizUnit
                Catch ex As Exception
                    SystemUser.UserBizUnit = ""
                End Try

                SystemUser.UserAccessSettings = .AccessSettings

                ' Added 12112012@smretailinc: new variable for encryption key words
                SystemUser.EncryptKey = SystemUser.UserName & .SignName & .SignPosition

                'update last login date and time
                'SystemUser
            End With

            bResult = True
        End If

        ' clean-up
        taUsers.Dispose()
        dtUsers.Dispose()

        LoadDataFromUserTable = bResult
    End Function

    Private Shared Function IsLDAPvalidated(ByVal LDAPconnString As String, ByVal UserDomain As String, ByVal UserLogName As String, ByVal Password As String) As Boolean

        'Dim Entry As DirectoryEntry = New DirectoryEntry("LDAP://mcidc01.mci.sm-shoemart.com:389", "MCI\" & UserLogName, UserPassword)
        'Dim Entry As DirectoryEntry = New DirectoryEntry("LDAP://smretail.com", "SMRETAIL\" & UserLogName, Password)

        Dim Entry As DirectoryEntry = New DirectoryEntry(LDAPconnString, UserDomain & "\" & UserLogName, Password)
        Dim Searcher As DirectorySearcher = New DirectorySearcher(Entry)
        Dim srADuser As System.DirectoryServices.SearchResult
        'Dim GroupKeys As String = ""
        Dim bResult As Boolean = False

        ' validate user using Active Directory
        Try
            Searcher.Filter = ("(anr=" & UserLogName & ")")
            srADuser = Searcher.FindOne()

            SystemUser.UserName = srADuser.Properties("cn").Item(0).ToString()

            'GroupKeys = srADuser.Properties("distinguishedname").Item(0).ToString()
            ''SystemUser.UserGroupID = SystemUser.GetOrgUnitString(GroupKeys)

            SystemUser.UserLogName = UserLogName

            bResult = True

        Catch ex As Exception

            bResult = False

        Finally

            Entry.Dispose()
            Searcher.Dispose()

        End Try

        Return bResult

    End Function

    Public Shared Function InitializeUser(ByVal UserLogName As String, ByVal UserPassword As String) As Boolean

        Dim bResult As Boolean

        ' check smretail domain
        'bResult = IsLDAPvalidated("LDAP://smretail.com", "SMRETAIL", UserLogName, UserPassword)
        'If Not bResult Then bResult = IsLDAPvalidated("LDAP://smretailinc.com", "SMRETAILINC", UserLogName, UserPassword)
        'If Not bResult Then bResult = IsLDAPvalidated("LDAP://mcidc01.mci.sm-shoemart.com:389", "MCI", UserLogName, UserPassword)
        bResult = True
        ' get data from Users table: test if user is part of the core user group
        If bResult Then
            Try

                '  bResult = SystemUser.LoadDataFromUserTable()

                bResult = SystemUser.LoadDataFromUserTable(UserLogName)
                SystemUser.UserName = UserLogName

                'If SystemUser.LoadDataFromUserTable() Then

                '    ' user is part of the core user group
                '    bResult = True

                'Else

                '    ' ::TODO:: 
                '    ' check user's unit keys if there is a valid Branch or area where they are authorized
                '    If 1 = 1 Then
                '        ' SystemUser.UserBranch = 
                '        ' SystemUser.UserDept =
                '        SystemUser.UserLevel = SystemUser.UserRoles.AnnouncementViewer
                '    Else
                '        bResult = False
                '    End If

                'End If

            Catch ex As Exception

                ' problem accessing SQL table
                bResult = False

            End Try
        End If

        If Not bResult Then SystemUser.LogOut()

        InitializeUser = bResult

    End Function

End Class
