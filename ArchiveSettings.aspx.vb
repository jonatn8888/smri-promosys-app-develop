' Object Name	    :       ArchiveSettings.aspx
' Purpose		    :       Requested by MPD dated November 16, 2011. Due to limited disk space in the PromoSys server and the continues
'                           posting of PromoSys transactions since its launch in July 2010 system
'                           resouces are being used up and slowdown is expected if no housekeeping
'                           is done on expired MPD announcement memos.
' Date Created	    :       06/11/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0               06/28/2012          Dow T. Carpio       Created this module.

Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class ArchiveSettings
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            StartupProcedures()

        End If

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "onload", "SetVisibility('" & radFrequency.SelectedValue & "');", True)

    End Sub

    Private Sub StartupProcedures()

        Try

            If SystemUser.UserID = 0 Then Response.Redirect("InvalidAccess.aspx")
            SearchRecord()
            ChangeInterface()

        Catch ex As Exception

            Throw ex

        End Try

    End Sub


    Private Sub ChangeInterface()

        'txtStartTime.Enabled = False

    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If InfoValidation() Then

            If SaveRecord() Then

                SetDefault()
                SearchRecord()
                clsSession.Message = "Scheduled task was successfully updated."
                clsSession.Icon = "success"
                'audit trail
                clsPromo.InsertAuditTrail(Format(CInt(clsPromo.Audit.Utilities), "0#"), hfID.Value, "PROMOSYS Utilities: Archive Scheduled Task was updated.", SystemUser.UserName, "Utilities")
                ClientScript.RegisterStartupScript(Me.GetType, "key", "<script>opentexteditor();</script>")

            End If


        End If

    End Sub

    Private Function InfoValidation() As Boolean

        InfoValidation = True
        blistErrorMsg.Items.Clear()

        If txtMonthsOld.Text = "" Then

            blistErrorMsg.Items.Add("Months old from current date is empty.")
            clsPromo.jsSetFocus(Page, txtMonthsOld.ClientID)
            InfoValidation = False

        Else

            If IsNumeric(txtMonthsOld.Text) = False Then

                blistErrorMsg.Items.Add("Invalid numeric/number input (Months old from current date is empty).")
                clsPromo.jsSetFocus(Page, txtMonthsOld.ClientID)
                InfoValidation = False

            Else

                If CInt(txtMonthsOld.Text) < 6 Then

                    blistErrorMsg.Items.Add("Months old should be greater than or equal to 6 months.")
                    clsPromo.jsSetFocus(Page, txtMonthsOld.ClientID)
                    InfoValidation = False

                End If

            End If

        End If

        If txtStartTime.Text = "" Then

            blistErrorMsg.Items.Add("Start time is empty.")
            clsPromo.jsSetFocus(Page, txtStartTime.ClientID)
            InfoValidation = False

        Else

            Dim t As DateTime
            If (DateTime.TryParse(txtStartTime.Text, t) = False) Then

                blistErrorMsg.Items.Add("Invalid time format (Start time).")
                clsPromo.jsSetFocus(Page, txtStartTime.ClientID)
                InfoValidation = False

            End If

        End If

        If radFrequency.SelectedValue = "W" Then

            Dim ctr = 0

            If chkWeekMon.Checked Or _
                chkWeekTue.Checked Or _
                chkWeekWed.Checked Or _
                chkWeekThu.Checked Or _
                chkWeekFri.Checked Or _
                chkWeekSat.Checked Or _
                chkWeekSun.Checked Then

                ctr += 1

            End If


            If ctr = 0 Then

                blistErrorMsg.Items.Add("Please select at least one(1) day in the scheduled task option.")
                clsPromo.jsSetFocus(Page, txtStartTime.ClientID)
                InfoValidation = False

            End If

        ElseIf radFrequency.SelectedValue = "M" Then


            Dim ctr = 0

            If chkMonthJan.Checked Or _
                chkMonthFeb.Checked Or _
                chkMonthMar.Checked Or _
                chkMonthApr.Checked Or _
                chkMonthMay.Checked Or _
                chkMonthJun.Checked Or _
                chkMonthJul.Checked Or _
                chkMonthAug.Checked Or _
                chkMonthSep.Checked Or _
                chkMonthOct.Checked Or _
                chkMonthNov.Checked Or _
                chkMonthDec.Checked Then

                ctr += 1

            End If


            If ctr = 0 Then

                blistErrorMsg.Items.Add("Please select at least one(1) month in the scheduled task option.")
                clsPromo.jsSetFocus(Page, txtStartTime.ClientID)
                InfoValidation = False

            End If

        ElseIf radFrequency.SelectedValue = "Q" Then


            Dim ctr = 0

            If chkQuarter1.Checked Or _
                chkQuarter2.Checked Or _
                chkQuarter3.Checked Or _
                chkQuarter4.Checked Then

                ctr += 1

            End If


            If ctr = 0 Then

                blistErrorMsg.Items.Add("Please select at least one(1) quarter in the scheduled task option.")
                clsPromo.jsSetFocus(Page, txtStartTime.ClientID)
                InfoValidation = False

            End If
        End If

    End Function

    Private Function SaveRecord() As Boolean

        Dim sqlConn As SqlConnection
        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand

        Try


            sqlConn.Open()
            sqlCmd = New SqlCommand
            sqlCmd.CommandText = "PMS_P_ARCHIVEREF_U"
            sqlCmd.Connection = sqlConn
            sqlCmd.CommandTimeout = 0
            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@ID", SqlDbType.Int)
            sqlCmd.Parameters("@ID").Value = IIf(CStr(hfID.Value) <> "", hfID.Value, 0)

            sqlCmd.Parameters.Add("@StartTime", SqlDbType.SmallDateTime)
            sqlCmd.Parameters("@StartTime").Value = txtStartTime.Text

            sqlCmd.Parameters.Add("@Day", SqlDbType.SmallInt)
            sqlCmd.Parameters("@Day").Value = GetDay()

            sqlCmd.Parameters.Add("@Schedule", SqlDbType.VarChar)
            sqlCmd.Parameters("@Schedule").Value = GetSchedule()

            sqlCmd.Parameters.Add("@MonthsOld", SqlDbType.SmallInt)
            sqlCmd.Parameters("@MonthsOld").Value = CInt(txtMonthsOld.Text)

            sqlCmd.Parameters.Add("@Frequency", SqlDbType.VarChar)
            sqlCmd.Parameters("@Frequency").Value = radFrequency.SelectedValue

            sqlCmd.Parameters.Add("@Remarks", SqlDbType.VarChar)
            sqlCmd.Parameters("@Remarks").Value = ""

            sqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit)
            sqlCmd.Parameters("@IsEnabled").Value = IIf(chkEnable.Checked = True, "True", "False").ToString

            sqlCmd.Parameters.Add("@UserCreated", SqlDbType.SmallInt)
            sqlCmd.Parameters("@UserCreated").Value = SystemUser.UserID

            sqlCmd.Parameters.Add("@UserModified", SqlDbType.SmallInt)
            sqlCmd.Parameters("@UserModified").Value = SystemUser.UserID

            sqlCmd.ExecuteNonQuery()

            SaveRecord = True

        Catch ex As Exception

            blistErrorMsg.Items.Add(ex.Message)
            SaveRecord = False

        End Try

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing

        GC.Collect()

    End Function

    Private Function GetDay() As Integer
        Dim _Output As String
        If radFrequency.SelectedValue = "D" Or _
                radFrequency.SelectedValue = "W" Then
            _Output = 0
        ElseIf radFrequency.SelectedValue = "M" Then
            _Output = ddlMonthDay.SelectedValue
        ElseIf radFrequency.SelectedValue = "Q" Then
            _Output = ddlQuarterDay.SelectedValue
        ElseIf radFrequency.SelectedValue = "A" Then
            _Output = ddlAnnualDay.SelectedValue
        Else
            _Output = 0
        End If
        Return _Output
    End Function

    Private Function GetSchedule() As String
        Dim _Output As String = ""

        If radFrequency.SelectedValue = "D" Then
            _Output = ""
        ElseIf radFrequency.SelectedValue = "W" Then
            If chkWeekMon.Checked Then
                _Output &= "1;"
            End If
            If chkWeekTue.Checked Then
                _Output &= "2;"
            End If
            If chkWeekWed.Checked Then
                _Output &= "3;"
            End If
            If chkWeekThu.Checked Then
                _Output &= "4;"
            End If
            If chkWeekFri.Checked Then
                _Output &= "5;"
            End If
            If chkWeekSat.Checked Then
                _Output &= "6;"
            End If
            If chkWeekSun.Checked Then
                _Output &= "7;"
            End If
            _Output = Left(_Output, Len(_Output) - 1)
        ElseIf radFrequency.SelectedValue = "M" Then
            If chkMonthJan.Checked Then
                _Output &= "1;"
            End If
            If chkMonthFeb.Checked Then
                _Output &= "2;"
            End If
            If chkMonthMar.Checked Then
                _Output &= "3;"
            End If
            If chkMonthApr.Checked Then
                _Output &= "4;"
            End If
            If chkMonthMay.Checked Then
                _Output &= "5;"
            End If
            If chkMonthJun.Checked Then
                _Output &= "6;"
            End If
            If chkMonthJul.Checked Then
                _Output &= "7;"
            End If
            If chkMonthAug.Checked Then
                _Output &= "8;"
            End If
            If chkMonthSep.Checked Then
                _Output &= "9;"
            End If
            If chkMonthOct.Checked Then
                _Output &= "10;"
            End If
            If chkMonthNov.Checked Then
                _Output &= "11;"
            End If
            If chkMonthDec.Checked Then
                _Output &= "12;"
            End If
            _Output = Left(_Output, Len(_Output) - 1)
        ElseIf radFrequency.SelectedValue = "Q" Then
            If chkQuarter1.Checked Then
                _Output &= "3;"
            End If
            If chkQuarter2.Checked Then
                _Output &= "6;"
            End If
            If chkQuarter3.Checked Then
                _Output &= "9;"
            End If
            If chkQuarter4.Checked Then
                _Output &= "12;"
            End If
            _Output = Left(_Output, Len(_Output) - 1)
        ElseIf radFrequency.SelectedValue = "A" Then
            _Output = ddlAnnualMonth.SelectedValue
        Else
            _Output = ""
        End If

        Return _Output

    End Function

    Private Sub SetDefault()
        'Week
        chkWeekMon.Checked = True
        chkWeekTue.Checked = True
        chkWeekWed.Checked = True
        chkWeekThu.Checked = True
        chkWeekFri.Checked = True
        chkWeekSat.Checked = True
        chkWeekSun.Checked = True

        'Month
        ddlMonthDay.SelectedIndex = 0
        chkMonthJan.Checked = True
        chkMonthFeb.Checked = True
        chkMonthMar.Checked = True
        chkMonthApr.Checked = True
        chkMonthJun.Checked = True
        chkMonthJul.Checked = True
        chkMonthAug.Checked = True
        chkMonthSep.Checked = True
        chkMonthOct.Checked = True
        chkMonthNov.Checked = True
        chkMonthDec.Checked = True

        'Quarter
        ddlQuarterDay.SelectedIndex = 0
        chkQuarter1.Checked = True
        chkQuarter2.Checked = True
        chkQuarter3.Checked = True
        chkQuarter4.Checked = True

        'Annual
        ddlAnnualMonth.SelectedIndex = 0
        ddlAnnualDay.SelectedIndex = 0

    End Sub
    Private Sub populateSchedule(ByVal Schedule As String)

        If radFrequency.SelectedValue = "D" Then
            'Do nothing
        ElseIf radFrequency.SelectedValue = "W" Then

            If splitSchedule(Schedule, "1") = True Then
                chkWeekMon.Checked = True
            Else
                chkWeekMon.Checked = False
            End If

            If splitSchedule(Schedule, "2") = True Then
                chkWeekTue.Checked = True
            Else
                chkWeekTue.Checked = False
            End If

            If splitSchedule(Schedule, "3") = True Then
                chkWeekWed.Checked = True
            Else
                chkWeekWed.Checked = False
            End If

            If splitSchedule(Schedule, "4") = True Then
                chkWeekThu.Checked = True
            Else
                chkWeekThu.Checked = False
            End If

            If splitSchedule(Schedule, "5") = True Then
                chkWeekFri.Checked = True
            Else
                chkWeekFri.Checked = False
            End If

            If splitSchedule(Schedule, "6") = True Then
                chkWeekSat.Checked = True
            Else
                chkWeekSat.Checked = False
            End If

            If splitSchedule(Schedule, "7") = True Then
                chkWeekSun.Checked = True
            Else
                chkWeekSun.Checked = False
            End If

        ElseIf radFrequency.SelectedValue = "M" Then

            If splitSchedule(Schedule, "1") = True Then
                chkMonthJan.Checked = True
            Else
                chkMonthJan.Checked = False
            End If

            If splitSchedule(Schedule, "2") = True Then
                chkMonthFeb.Checked = True
            Else
                chkMonthFeb.Checked = False
            End If

            If splitSchedule(Schedule, "3") = True Then
                chkMonthMar.Checked = True
            Else
                chkMonthMar.Checked = False
            End If

            If splitSchedule(Schedule, "4") = True Then
                chkMonthApr.Checked = True
            Else
                chkMonthApr.Checked = False
            End If

            If splitSchedule(Schedule, "5") = True Then
                chkMonthMay.Checked = True
            Else
                chkMonthMay.Checked = False
            End If

            If splitSchedule(Schedule, "6") = True Then
                chkMonthJun.Checked = True
            Else
                chkMonthJun.Checked = False
            End If
            If splitSchedule(Schedule, "7") = True Then
                chkMonthJul.Checked = True
            Else
                chkMonthJul.Checked = False
            End If
            If splitSchedule(Schedule, "8") = True Then
                chkMonthAug.Checked = True
            Else
                chkMonthAug.Checked = False
            End If
            If splitSchedule(Schedule, "9") = True Then
                chkMonthSep.Checked = True
            Else
                chkMonthSep.Checked = False
            End If

            If splitSchedule(Schedule, "10") = True Then
                chkMonthOct.Checked = True
            Else
                chkMonthOct.Checked = False
            End If
            If splitSchedule(Schedule, "11") = True Then
                chkMonthNov.Checked = True
            Else
                chkMonthNov.Checked = False
            End If
            If splitSchedule(Schedule, "12") = True Then
                chkMonthDec.Checked = True
            Else
                chkMonthDec.Checked = False
            End If

        ElseIf radFrequency.SelectedValue = "Q" Then

            If splitSchedule(Schedule, "3") = True Then
                chkQuarter1.Checked = True
            Else
                chkQuarter1.Checked = False
            End If

            If splitSchedule(Schedule, "6") = True Then
                chkQuarter2.Checked = True
            Else
                chkQuarter2.Checked = False
            End If

            If splitSchedule(Schedule, "9") = True Then
                chkQuarter3.Checked = True
            Else
                chkQuarter3.Checked = False
            End If

            If splitSchedule(Schedule, "12") = True Then
                chkQuarter4.Checked = True
            Else
                chkQuarter4.Checked = False
            End If

        ElseIf radFrequency.SelectedValue = "A" Then
            ddlAnnualMonth.SelectedValue = Schedule
        Else
            'Do nothing
        End If

    End Sub
    Public Function splitSchedule(ByVal Schedule As String, ByVal Value As String) As Boolean

        Dim arr() As String = Schedule.Split(";")
        Dim ctr = 0

        For i As Integer = 0 To arr.Length - 1

            'If arr(i).Contains(Value) Then
            Dim a = arr(i)
            If arr(i) = Value Then

                ctr += 1

            End If

        Next

        If ctr > 0 Then

            Return True

        Else

            Return False

        End If


    End Function
    'Public Function splitSchedule(ByVal Schedule As String, ByVal Value As String) As Boolean

    '    Dim strArray() As String = Schedule.Split(";")
    '    Dim strOutput As String
    '    Try

    '        If strArray.Find(strArray, Value) Then

    '        End If
    '        For Each strOutput In strArray

    '            Dim A As String = strArray(Value - 1)
    '            If strArray(Value - 1) = Value Then

    '                Return True

    '            Else

    '                Return False

    '            End If

    '        Next

    '    Catch ex As Exception

    '        Throw ex

    '    End Try

    'End Function

    'Public Function ConvertArraytoString(ByVal ArrayValue() As String, Optional ByVal Deliminator As String = ",") As String

    '    Dim strValue As String, strData As String

    '    Try

    '        strData = ""

    '        For Each strValue In ArrayValue

    '            If Not strValue Is Nothing And Trim(strValue) <> "" Then

    '                strData += Trim(strValue) & Deliminator

    '            End If

    '        Next

    '        strData = Left(strData, Len(strData) - 1)

    '        Return strData

    '    Catch ex As Exception

    '        Return ""

    '    End Try

    'End Function
    Private Sub SearchRecord()

        Dim sqlAdatpter As SqlDataAdapter
        Dim sqlConn As SqlConnection
        Dim dtbl As New DataTable

        sqlConn = New SqlConnection(clsPromo.SQLConnString())
        Dim sqlCmd As SqlCommand
        sqlConn.Open()
        sqlCmd = New SqlCommand
        sqlCmd.CommandText = "PMS_P_ARCHIVEREF_S"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandTimeout = 0
        sqlCmd.CommandType = CommandType.StoredProcedure

        sqlAdatpter = New SqlDataAdapter(sqlCmd)
        sqlAdatpter.Fill(dtbl)

        If dtbl.Rows.Count > 0 Then

            lblDateLastUpdate.Text = "(Updated as of " & IIf(clsPromo.FetchDataItem(dtbl, 0, "DATEMODIFIED").ToString <> Nothing, clsPromo.FetchDataItem(dtbl, 0, "DATEMODIFIED").ToString, clsPromo.FetchDataItem(dtbl, 0, "DATECREATED").ToString).ToString & ")"
            chkEnable.Checked = CType(IIf(clsPromo.FetchDataItem(dtbl, 0, "ISENABLED").ToString, True, False), Boolean)
            hfID.Value = clsPromo.FetchDataItem(dtbl, 0, "ID").ToString
            txtMonthsOld.Text = clsPromo.FetchDataItem(dtbl, 0, "MONTHSOLD").ToString
            radFrequency.SelectedValue = clsPromo.FetchDataItem(dtbl, 0, "FREQUENCY").ToString

            If radFrequency.SelectedValue = "M" Then
                ddlMonthDay.SelectedValue = clsPromo.FetchDataItem(dtbl, 0, "DAY").ToString
            ElseIf radFrequency.SelectedValue = "Q" Then
                ddlQuarterDay.SelectedValue = clsPromo.FetchDataItem(dtbl, 0, "DAY").ToString
            ElseIf radFrequency.SelectedValue = "A" Then
                ddlAnnualDay.SelectedValue = clsPromo.FetchDataItem(dtbl, 0, "DAY").ToString
            End If

            txtStartTime.Text = clsPromo.FetchDataItem(dtbl, 0, "STARTTIME").ToString
            populateSchedule(clsPromo.FetchDataItem(dtbl, 0, "SCHEDULE").ToString)

        Else

            lblDateLastUpdate.Text = Nothing
            chkEnable.Checked = False
            txtMonthsOld.Text = "6"
            txtStartTime.Text = " 1:00 AM"
            radFrequency.SelectedValue = "D"


        End If

        'sqlCmd.ExecuteNonQuery()

        If sqlConn.State <> ConnectionState.Closed Then sqlConn.Close()
        sqlConn = Nothing
        sqlCmd = Nothing
        sqlAdatpter = Nothing
        dtbl = Nothing

        GC.Collect()

    End Sub




End Class
