<%@ Page Language="VB" AutoEventWireup="false" ValidateRequest="false" CodeFile="QuickMessage.aspx.vb" Inherits="QuickMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Quick Mail - Simple Messaging System</title>
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)" /><meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)" /> 

    <link rel="stylesheet" href=".\css\main.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div id = "contents">


    <span style="font-family: Trebuchet MS; font-size: 16pt; color: #ffff00">Quick Mail</span>   
    <hr style="width: 100%" />

    <div id="nav_bar">
        <ul>
            <li><a href="Default.aspx">Home</a></li>
            <li><a href="QuickMessage.aspx">Inbox</a></li>
            <li><a href="QuickMessage.aspx?ViewMode=3">Sent Items</a></li>
            <li><a href="QuickMessage.aspx?ViewMode=2">Write New</a></li>
        </ul>
    </div>

    <br /><br /><br />
 
    <asp:MultiView ID="mvBody" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewInbox" runat="server">
                <span style="font-size: 12pt; color: #ffffff"></span>
                <asp:Label ID="lblBoxTitle" runat="server" Font-Bold="True" Font-Names="Trebuchet MS"
                    Font-Size="Medium" ForeColor="White" Text=":: inbox ::"></asp:Label><br />
                
                <br />
                <span style="font-size: 10pt; color: #ffffff">
                <asp:GridView ID="gridMessages" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="WhiteSmoke" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" DataSourceID="sqldsMessages"
                    GridLines="Horizontal" Height="36px" Width="624px">
                    <EmptyDataTemplate>
                    <span style="font-size: 12pt; color: black">You're Inbox is empty.</span>
                    </EmptyDataTemplate>

                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRowSel" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblMessageID" runat="server" Text='<%# Eval("MessageID", "{0:D}") %>'></asp:Label>
                                <asp:Label ID="lblIsRead" runat="server" Text='<%# Eval("IsRead", "{0:D}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
                        <asp:BoundField DataField="DateSent" DataFormatString="{0:MM-dd-yyyy}" HeaderText="Date"
                            SortExpression="DateSent">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SentBy" HeaderText="From" SortExpression="SentBy">
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                </asp:GridView>
                    <asp:LinkButton ID="lnkDeleteMsg" runat="server" ForeColor="White">Delete Selected</asp:LinkButton><br />
                </span>
                <asp:SqlDataSource ID="sqldsMessages" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                    SelectCommand="SELECT * FROM Messaging ORDER BY DateSent DESC" DeleteCommand="DELETE FROM Messaging WHERE MessageID = @MessageID" InsertCommand="INSERT INTO Messaging (Subject, DateSent, MsgBody, SentBy, SentTo) VALUES (@Subject, @DateSent, @MsgBody, @SentBy, @SentTo)" UpdateCommand="UPDATE Messaging SET SentTo = '' WHERE MessageID = @MessageID">
                    <DeleteParameters>
                        <asp:Parameter Name="MessageID" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="txtSubject" Name="Subject" PropertyName="Text" />
                        <asp:ControlParameter ControlID="lblDateToday" Name="DateSent" PropertyName="Text" />
                        <asp:Parameter Name="MsgBody" />
                        <asp:Parameter Name="SentBy" />
                        <asp:ControlParameter ControlID="txtSendTo" Name="SentTo" PropertyName="Text" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MessageID" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                
            </asp:View>
            
        <asp:View ID="viewReadMessage" runat="server">
            <span style="font-size: 12pt; color: #ffffff"><strong>:: view message ::</strong></span>
            <br />
            <table cellpadding="4" cellspacing="0" style="width: 693px; height: 42px" class="simple-table">
                <tr>
                    <td style="width: 123px">
                        &nbsp;</td>
                    <td style="width: 390px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 123px">
                        <span style="color: lightgrey">Date:</span></td>
                    <td style="width: 390px">
                        <asp:Label ID="lblMsgDate" runat="server" ForeColor="White" Width="314px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 123px">
                        <span style="color: lightgrey">Subject:</span></td>
                    <td style="width: 390px">
                        <asp:Label ID="lblSubject" runat="server" ForeColor="White" Width="314px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 123px; height: 6px">
                        <span style="color: lightgrey">From:</span></td>
                    <td style="width: 390px; height: 6px">
                        <asp:Label ID="lblSentBy" runat="server" ForeColor="White" Width="314px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 123px; height: 6px;">
                        <span style="color: darkkhaki">&nbsp;</span></td>
                    <td style="width: 390px; height: 6px;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 123px" valign="top">
                        <span style="color: gainsboro">Message:</span></td>
                    <td style="width: 390px">
                        <asp:TextBox ID="txtMsgBody" runat="server" Height="128px" ReadOnly="True" TextMode="MultiLine"
                            Width="558px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 123px" valign="top">
                    </td>
                    <td style="width: 390px">
                        <asp:LinkButton ID="lnkReply" runat="server">Reply to this Message</asp:LinkButton>
                        </td>
                </tr>
                <tr>
                    <td style="width: 123px" valign="top">
                    </td>
                    <td style="width: 390px">
                        <asp:LinkButton ID="lnkCloseRead" runat="server">Back to Inbox</asp:LinkButton></td>
                </tr>
            </table>
        </asp:View>
        
        <asp:View ID="viewWriteMessage" runat="server">
            <span style="font-size: 12pt; color: #ffffff"><strong>:: new message ::</strong></span>
            <br />
            <table cellpadding="3" cellspacing="0" style="width: 696px" class="simple-table">
                <tr>
                    <td style="width: 116px">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 116px; height: 11px">
                        <span style="color: #d3d3d3">Date:</span></td>
                    <td style="height: 11px">
                        <asp:Label ID="lblDateToday" runat="server" ForeColor="White" Width="314px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 116px; height: 16px">
                        <span style="color: #d3d3d3">Subject:</span></td>
                    <td style="height: 16px">
                        <asp:TextBox ID="txtSubject" runat="server" Width="449px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 116px; height: 1px">
                        <span style="color: #d3d3d3">To:</span></td>
                    <td style="height: 1px">
                        <asp:TextBox ID="txtSendTo" runat="server" Width="213px" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 116px">
                        &nbsp;</td>
                    <td>
                        <asp:BulletedList ID="blistError" runat="server" CssClass="error-list" ForeColor="LightPink" Width="400px">
                        </asp:BulletedList>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 116px; height: 90px">
                        <span style="color: #d3d3d3">Message:</span></td>
                    <td style="height: 90px">
                        <asp:TextBox ID="txtNewMessage" runat="server" Height="128px" TextMode="MultiLine"
                            Width="558px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 116px">
                        </td>
                    <td>
                        <asp:LinkButton ID="lnkSendMessage" runat="server">Send Message</asp:LinkButton></td>
                </tr>
                <tr>
                    <td style="width: 116px">
                        </td>
                    <td>
                        <asp:LinkButton ID="lnkCloseWrite" runat="server">Back to Inbox</asp:LinkButton></td>
                </tr>
                <tr>
                    <td style="width: 116px">
                        </td>
                    <td>
                        <asp:LinkButton ID="lnkBackToMsg" runat="server">Back to Message</asp:LinkButton></td>
                </tr>
            </table>
        </asp:View>
        </asp:MultiView>
            <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                    SelectCommand="SELECT * FROM Messaging WHERE MessageID = @MessageID">
                <SelectParameters>
                    <asp:Parameter Name="MessageID" />
                </SelectParameters>
            </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
