' Object Name	    :       clsEncryptDecrypt.vb
' Purpose		    :       Encryption/decryption class library of Global Parameters
' Date Created	    :       06/11/2012
' User Created	    :       Dow T. Carpio
' REVISIONS:
' Ver				Date				Author				Description
' ----------------------------------------------------------------------
' 1.0              06/11/2012     Dow T. Carpio     Created this class library.

Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography

Public Class clsEncryptDecrypt

#Region "Initialization and Finalize"
#End Region

#Region "Fields"
#End Region

#Region "Properties"
#End Region

#Region "Methods, Events and Delegates"
    Public Shared Function EncryptText(ByVal strText As String, ByVal EncryptionKey As String) As String

        Try

            EncryptText = Encrypt(strText, EncryptionKey)

        Catch ex As Exception

            Return Nothing

        End Try

    End Function

    Public Shared Function DecryptText(ByVal strText As String, ByVal EncryptionKey As String) As String

        Dim EncryptedText As String = strText

        Try

            EncryptedText = EncryptedText.Replace("BFX1", "=")
            DecryptText = Decrypt(EncryptedText.Replace(" ", "+"), EncryptionKey)

        Catch ex As Exception

            Return Nothing

        End Try

    End Function

    Private Shared Function Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String

        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim strKey As String
        Try

            byKey = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))

            Dim des As New DESCryptoServiceProvider
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            strKey = Convert.ToBase64String(ms.ToArray())
            strKey = strKey.Replace("=", "BFX1")

            Return strKey

        Catch ex As Exception

            Return ex.Message

        End Try

    End Function

    Private Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String

        Dim byKey() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte

        Try

            byKey = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))

            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(strText)

            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)

            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()

            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8

            Return encoding.GetString(ms.ToArray())

        Catch ex As Exception

            Return ex.Message

        End Try

    End Function

#End Region

End Class
