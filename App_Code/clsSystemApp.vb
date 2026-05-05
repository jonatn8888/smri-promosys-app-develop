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
Imports System

Public Class clsSystemApp

#Region "Generic Data Manipulation"

    Public Shared Function GetDataRow(ByVal strConnString As String, ByVal sqlCmdString As String, ByRef drResultRow As DataRow) As Boolean

        '-- , ByRef strErrorMsg As String

        Dim sqlConnect As SqlConnection
        Dim sqlCmd As SqlCommand

        Dim bSuccess As Boolean = False

        'Dim drResultRow As DataRow

        If strConnString <> "" Then

            sqlConnect = New SqlConnection(strConnString)

            Try
                sqlConnect.Open()
                sqlCmd = New SqlCommand(sqlCmdString, sqlConnect)

                Dim daSQL As New SqlDataAdapter()
                Dim ds As New DataSet

                daSQL.SelectCommand = sqlCmd
                daSQL.Fill(ds)

                ' get first row only
                drResultRow = ds.Tables(0).Rows(0)

                bSuccess = True

                ' clean-up
                ds.Dispose()
                daSQL.Dispose()
                sqlCmd.Dispose()

                sqlConnect.Close()

            Catch ex As Exception

                bSuccess = False
                Console.WriteLine(ex.Message)

            End Try
        End If

        Return bSuccess

    End Function

    Public Shared Function GetDataTable(ByVal strConnString As String, ByVal sqlCmdString As String, ByRef dtResultTable As DataTable) As Boolean

        ' --- , ByRef sErrorMsg As String

        Dim sqlConnect As SqlConnection
        Dim sqlCmd As SqlCommand

        Dim bSuccess As Boolean

        If strConnString <> "" Then

            sqlConnect = New SqlConnection(strConnString)

            Try
                sqlConnect.Open()
                sqlCmd = New SqlCommand(sqlCmdString, sqlConnect)

                Dim dt As New DataTable

                dt.Load(sqlCmd.ExecuteReader())

                dtResultTable = dt

                bSuccess = True

                ' clean-up
                dt.Dispose()
                sqlCmd.Dispose()

                sqlConnect.Close()

            Catch ex As Exception

                bSuccess = False
                Console.WriteLine(ex.Message)

            End Try
        End If

        Return bSuccess

    End Function

    Public Shared Function ExecuteNonQueryCommand(ByVal strConnString As String, ByVal sqlCmdString As String, ByRef sErrorMsg As String) As Boolean

        Dim sqlConnect As SqlConnection
        Dim sqlCmd As SqlCommand

        Dim bSuccess As Boolean

        If strConnString <> "" Then

            sqlConnect = New SqlConnection(strConnString)

            Try
                sqlConnect.Open()
                sqlCmd = New SqlCommand(sqlCmdString, sqlConnect)

                sqlCmd.ExecuteNonQuery()

                bSuccess = True

                ' clean-up
                sqlCmd.Dispose()
                sqlConnect.Close()

            Catch ex As Exception

                bSuccess = False
                sErrorMsg = ex.Message

            End Try
        End If

        Return bSuccess

    End Function

    Public Shared Function ExecuteScalarCommand(ByVal strConnString As String, ByVal sqlCmdString As String) As Int32

        Dim sqlConnect As SqlConnection
        Dim sqlCmd As SqlCommand

        Dim nNewRowID As Int32 = 0

        sqlConnect = New SqlConnection(strConnString)

        Try
            sqlConnect.Open()
            sqlCmd = New SqlCommand(sqlCmdString, sqlConnect)

            nNewRowID = Convert.ToInt32(sqlCmd.ExecuteScalar())

            ' clean-up
            sqlCmd.Dispose()
            sqlConnect.Close()

        Catch ex As Exception

            Console.WriteLine(ex.Message)

        End Try

        Return nNewRowID

    End Function

    Public Shared Function FindRowByColumnValue(ByVal dt As DataTable, ByVal columnName As String, ByVal value As Object) As DataRow
        For Each row As DataRow In dt.Rows
            If row.Item(columnName) = value Then
                Return row
            End If
        Next
        Return Nothing
    End Function

#End Region

End Class
