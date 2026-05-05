Imports Microsoft.VisualBasic

Public Class clsSession

    Private Shared _PeriodFrom As String = "PeriodFrom"
    Private Shared _PeriodTo As String = "PeriodTo"

    Private Shared _CurrPromoID As String = "CurrPromoID"
    Private Shared _CurrRequestID As String = "CurrRequestID"
    Private Shared _CurrMemoID As String = "CurrMemoID"

    Private Shared _PercentDisc As String = "PercentDisc"
    Private Shared _PromoTypeID As String = "PromoTypeID"

    Private Shared _Mechanics As String = "Mechanics"

    Private Shared _Message As String = "Message"
    Private Shared _Notification As String = "Notification" ' Added dowcarpio20121220@smretailinc: allow approver to edit date period if late requests

    Private Shared _Icon As String = "Icon"
    Private Shared _DeleteStatus As String = "DeleteStatus"
    Private Shared _MsgTransFlowFlag As String = "MsgTransFlowFlag"
    Private Shared _FlagForPOSDisp As String = "FlagForPOSDisp"
    Private Shared _CurrFilterSelection As String = "CurrFilterSelection"
    Private Shared _DefPromoBrList As String = "DefPromoBrList"

    Private Shared _AttachmentPath As String = "AttachmentPath"
    Private Shared _IsDepartmental As String = "IsDepartmental"

    Private Shared _GroupString As String = "GroupString"
    'Private Shared _rptEventNumber As String = "rptEventNumber"
    'Private Shared _rptTranType As String = "rptTranType"
    'Private Shared _rptBranchCode As String = "rptBranchCode"

    Public Shared Property IsDepartmental() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_IsDepartmental) Is Nothing) Then
                Return 0
            Else
                Return CStr(HttpContext.Current.Session(_IsDepartmental))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_IsDepartmental)
            Else
                HttpContext.Current.Session(_IsDepartmental) = value
            End If
        End Set
    End Property

    Public Shared Property AttachmentPath() As String
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_AttachmentPath) Is Nothing) Then
                Return ""
            Else
                Return CStr(HttpContext.Current.Session(_AttachmentPath))
            End If
        End Get

        Set(ByVal value As String)
            If value = "" Then
                HttpContext.Current.Session.Remove(_AttachmentPath)
            Else
                HttpContext.Current.Session(_AttachmentPath) = value
            End If
        End Set
    End Property

    Public Shared Property FlagForPOSDisp() As Boolean

        Get
            ' Check for null first
            If (HttpContext.Current.Session(_FlagForPOSDisp) Is Nothing) Then
                Return False
            Else
                Return CBool(HttpContext.Current.Session(_FlagForPOSDisp))
            End If
        End Get

        Set(ByVal value As Boolean)
            If value = False Then
                HttpContext.Current.Session.Remove(_FlagForPOSDisp)
            Else
                HttpContext.Current.Session(_FlagForPOSDisp) = value
            End If
        End Set
    End Property

    Public Shared Property DeleteStatus() As String
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_DeleteStatus) Is Nothing) Then
                Return ""
            Else
                Return CStr(HttpContext.Current.Session(_DeleteStatus))
            End If
        End Get

        Set(ByVal value As String)
            If value = "" Then
                HttpContext.Current.Session.Remove(_DeleteStatus)
            Else
                HttpContext.Current.Session(_DeleteStatus) = value
            End If
        End Set
    End Property

    Public Shared Property Icon() As String
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_Icon) Is Nothing) Then
                Return ""
            Else
                Return CStr(HttpContext.Current.Session(_Icon))
            End If
        End Get

        Set(ByVal value As String)
            If value = "" Then
                HttpContext.Current.Session.Remove(_Icon)
            Else
                HttpContext.Current.Session(_Icon) = value
            End If
        End Set
    End Property


    Public Shared Property Message() As String
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_Message) Is Nothing) Then
                Return ""
            Else
                Return CStr(HttpContext.Current.Session(_Message))
            End If
        End Get

        Set(ByVal value As String)
            If value = "" Then
                HttpContext.Current.Session.Remove(_Message)
            Else
                HttpContext.Current.Session(_Message) = value
            End If
        End Set
    End Property

    Public Shared Property Notification() As String
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_Notification) Is Nothing) Then
                Return ""
            Else
                Return CStr(HttpContext.Current.Session(_Notification))
            End If
        End Get

        Set(ByVal value As String)
            If value = "" Then
                HttpContext.Current.Session.Remove(_Notification)
            Else
                HttpContext.Current.Session(_Notification) = value
            End If
        End Set
    End Property
    Public Shared Property Mechanics() As String
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_Mechanics) Is Nothing) Then
                Return ""
            Else
                Return CStr(HttpContext.Current.Session(_Mechanics))
            End If
        End Get

        Set(ByVal value As String)
            If value = "" Then
                HttpContext.Current.Session.Remove(_Mechanics)
            Else
                HttpContext.Current.Session(_Mechanics) = value
            End If
        End Set
    End Property


    Public Shared Property DefPromoBrList() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_DefPromoBrList) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_DefPromoBrList))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_DefPromoBrList)
            Else
                HttpContext.Current.Session(_DefPromoBrList) = value
            End If
        End Set
    End Property

    Public Shared Property CurrFilterSelection() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_CurrFilterSelection) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_CurrFilterSelection))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_CurrFilterSelection)
            Else
                HttpContext.Current.Session(_CurrFilterSelection) = value
            End If
        End Set
    End Property

    Public Shared Property MsgTransFlowFlag() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_MsgTransFlowFlag) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_MsgTransFlowFlag))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_MsgTransFlowFlag)
            Else
                HttpContext.Current.Session(_MsgTransFlowFlag) = value
            End If
        End Set
    End Property

    Public Shared Property CurrMemoID() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_CurrMemoID) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_CurrMemoID))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_CurrMemoID)
            Else
                HttpContext.Current.Session(_CurrMemoID) = value
            End If
        End Set
    End Property

    Public Shared Property PromoTypeID() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_PromoTypeID) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_PromoTypeID))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_PromoTypeID)
            Else
                HttpContext.Current.Session(_PromoTypeID) = value
            End If
        End Set
    End Property

    Public Shared Property PercentDisc() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_PercentDisc) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_PercentDisc))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_PercentDisc)
            Else
                HttpContext.Current.Session(_PercentDisc) = value
            End If
        End Set
    End Property

    Public Shared Property CurrRequestID() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_CurrRequestID) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_CurrRequestID))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_CurrRequestID)
            Else
                HttpContext.Current.Session(_CurrRequestID) = value
            End If
        End Set
    End Property

    Public Shared Property CurrPromoID() As Integer
        Get
            ' Check for null first
            If (HttpContext.Current.Session(_CurrPromoID) Is Nothing) Then
                Return 0
            Else
                Return CInt(HttpContext.Current.Session(_CurrPromoID))
            End If
        End Get

        Set(ByVal value As Integer)
            If value = 0 Then
                HttpContext.Current.Session.Remove(_CurrPromoID)
            Else
                HttpContext.Current.Session(_CurrPromoID) = value
            End If
        End Set
    End Property

    Public Shared Property PeriodFrom() As Date
        Get
            If (HttpContext.Current.Session(_PeriodFrom) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_PeriodFrom).ToString()
            End If
        End Get

        Set(ByVal value As Date)
            HttpContext.Current.Session(_PeriodFrom) = value
        End Set
    End Property

    Public Shared Property PeriodTo() As Date
        Get
            If (HttpContext.Current.Session(_PeriodTo) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_PeriodTo).ToString()
            End If
        End Get

        Set(ByVal value As Date)
            HttpContext.Current.Session(_PeriodTo) = value
        End Set
    End Property

    ' Added dowcarpio201501@smertailinc: PRF Updates
    Public Shared Property GroupString() As String
        Get
            If (HttpContext.Current.Session(_GroupString) Is Nothing) Then
                Return Nothing
            Else
                Return HttpContext.Current.Session(_GroupString).ToString()
            End If
        End Get

        Set(ByVal value As String)
            HttpContext.Current.Session(_GroupString) = value
        End Set
    End Property

    'Public Shared Property rptEventNumber() As String
    '    Get
    '        ' Check for null first
    '        If (HttpContext.Current.Session(_rptEventNumber) Is Nothing) Then
    '            Return ""
    '        Else
    '            Return CStr(HttpContext.Current.Session(_rptEventNumber))
    '        End If
    '    End Get

    '    Set(ByVal value As String)
    '        If value = "" Then
    '            HttpContext.Current.Session.Remove(_rptEventNumber)
    '        Else
    '            HttpContext.Current.Session(_rptEventNumber) = value
    '        End If
    '    End Set
    'End Property

    'Public Shared Property rptBranchCode() As String
    '    Get
    '        ' Check for null first
    '        If (HttpContext.Current.Session(_rptBranchCode) Is Nothing) Then
    '            Return ""
    '        Else
    '            Return CStr(HttpContext.Current.Session(_rptBranchCode))
    '        End If
    '    End Get

    '    Set(ByVal value As String)
    '        If value = "" Then
    '            HttpContext.Current.Session.Remove(_rptBranchCode)
    '        Else
    '            HttpContext.Current.Session(_rptBranchCode) = value
    '        End If
    '    End Set
    'End Property

    'Public Shared Property rptTranType() As String
    '    Get
    '        ' Check for null first
    '        If (HttpContext.Current.Session(_rptTranType) Is Nothing) Then
    '            Return ""
    '        Else
    '            Return CStr(HttpContext.Current.Session(_rptTranType))
    '        End If
    '    End Get

    '    Set(ByVal value As String)
    '        If value = "" Then
    '            HttpContext.Current.Session.Remove(_rptTranType)
    '        Else
    '            HttpContext.Current.Session(_rptTranType) = value
    '        End If
    '    End Set
    'End Property

End Class
