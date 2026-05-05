

Imports System.Data
Imports System.Web.UI.ControlCollection

Partial Class PromoRequestSwipestakesMessage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'sqlDSPromoBinRange.SelectParameters("RequestID").DefaultValue = clsSession.CurrRequestID
            LoadPromotionMessage(RetrievePromoID())
        End If
    End Sub


    Private Function RetrievePromoID() As Long

        Dim drPromotions As DataRow = Nothing
        Dim strSQLcmd As String
        RetrievePromoID = 0
        strSQLcmd = "SELECT PromoID FROM Promotions " & _
                                "WHERE RequestID = " & clsSession.CurrRequestID

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQLcmd, drPromotions) Then
            RetrievePromoID = drPromotions("PromoID")
        End If
    End Function

    Private Sub LoadPromotionMessage(ByVal nPromoID As Long)

        Dim strSQL As String
        Dim drPromoMessage As DataRow = Nothing

        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "SELECT PM.PromoMessage FROM PromoMessages PM " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 1"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drPromoMessage) Then
            Dim sMsgLines() As String = drPromoMessage("PromoMessage").Split(vbCrLf)
            If sMsgLines.Length > 1 Then
                'For i As Integer = 1 To sMsgLines.Length
                '    Dim tempStr As String = IIf(i < 10, "0" & i.ToString(), i)
                '    txtXML_WinningMsg()
                'Next
                txtXML_WinningMsg01.Text = sMsgLines(0)
                txtXML_WinningMsg02.Text = sMsgLines(1)
                txtXML_WinningMsg03.Text = sMsgLines(2)
                txtXML_WinningMsg04.Text = sMsgLines(3)
                txtXML_WinningMsg05.Text = sMsgLines(4)
                txtXML_WinningMsg06.Text = sMsgLines(5)
                txtXML_WinningMsg07.Text = sMsgLines(6)
                txtXML_WinningMsg08.Text = sMsgLines(7)
                txtXML_WinningMsg09.Text = sMsgLines(8)
                txtXML_WinningMsg10.Text = sMsgLines(9)
                txtXML_WinningMsg11.Text = sMsgLines(10)
                txtXML_WinningMsg12.Text = sMsgLines(11)
                txtXML_WinningMsg13.Text = sMsgLines(12)
                txtXML_WinningMsg14.Text = sMsgLines(13)
                txtXML_WinningMsg15.Text = sMsgLines(14)
                txtXML_WinningMsg16.Text = sMsgLines(15)
                txtXML_WinningMsg17.Text = sMsgLines(16)
                txtXML_WinningMsg18.Text = sMsgLines(17)
                txtXML_WinningMsg19.Text = sMsgLines(18)
                txtXML_WinningMsg20.Text = sMsgLines(19)
            End If
        End If
        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "SELECT PM.PromoMessage FROM PromoMessages PM " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 2"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drPromoMessage) Then
            Dim sMsgLines() As String = drPromoMessage("PromoMessage").Split(vbCrLf)
            If sMsgLines.Length > 1 Then
                txtXML_NonwinningMsg01.Text = sMsgLines(0)
                txtXML_NonwinningMsg02.Text = sMsgLines(1)
                txtXML_NonwinningMsg03.Text = sMsgLines(2)
                txtXML_NonwinningMsg04.Text = sMsgLines(3)
                txtXML_NonwinningMsg05.Text = sMsgLines(4)
                txtXML_NonwinningMsg06.Text = sMsgLines(5)
                txtXML_NonwinningMsg07.Text = sMsgLines(6)
                txtXML_NonwinningMsg08.Text = sMsgLines(7)
                txtXML_NonwinningMsg09.Text = sMsgLines(8)
                txtXML_NonwinningMsg10.Text = sMsgLines(9)
                txtXML_NonwinningMsg11.Text = sMsgLines(10)
                txtXML_NonwinningMsg12.Text = sMsgLines(11)
                txtXML_NonwinningMsg13.Text = sMsgLines(12)
                txtXML_NonwinningMsg14.Text = sMsgLines(13)
                txtXML_NonwinningMsg15.Text = sMsgLines(14)
                txtXML_NonwinningMsg16.Text = sMsgLines(15)
                txtXML_NonwinningMsg17.Text = sMsgLines(16)
                txtXML_NonwinningMsg18.Text = sMsgLines(17)
                txtXML_NonwinningMsg19.Text = sMsgLines(18)
                txtXML_NonwinningMsg20.Text = sMsgLines(19)
            End If
        End If
        ' '' ---------------------------------------------------------------------------------------------
        strSQL = "SELECT PM.PromoMessage FROM PromoMessages PM " & _
                    "WHERE PromoID = " & nPromoID & " and MessageType = 3"

        If clsSystemApp.GetDataRow(clsPromo.SQLConnString, strSQL, drPromoMessage) Then
            Dim sMsgLines() As String = drPromoMessage("PromoMessage").Split(vbCrLf)
            If sMsgLines.Length > 1 Then
                txtXML_POSMsg01.Text = sMsgLines(0)
                txtXML_POSMsg02.Text = sMsgLines(1)
                txtXML_POSMsg03.Text = sMsgLines(2)
                txtXML_POSMsg04.Text = sMsgLines(3)
                txtXML_POSMsg05.Text = sMsgLines(4)
                txtXML_POSMsg06.Text = sMsgLines(5)
                txtXML_POSMsg07.Text = sMsgLines(6)
                txtXML_POSMsg08.Text = sMsgLines(7)
                txtXML_POSMsg09.Text = sMsgLines(8)
                txtXML_POSMsg10.Text = sMsgLines(9)
                txtXML_POSMsg11.Text = sMsgLines(10)
                txtXML_POSMsg12.Text = sMsgLines(11)
                txtXML_POSMsg13.Text = sMsgLines(12)
                txtXML_POSMsg14.Text = sMsgLines(13)
                txtXML_POSMsg15.Text = sMsgLines(14)
                txtXML_POSMsg16.Text = sMsgLines(15)
                txtXML_POSMsg17.Text = sMsgLines(16)
                txtXML_POSMsg18.Text = sMsgLines(17)
                txtXML_POSMsg19.Text = sMsgLines(18)
                txtXML_POSMsg20.Text = sMsgLines(19)
            End If
        End If
        ' '' ---------------------------------------------------------------------------------------------
    End Sub
End Class
