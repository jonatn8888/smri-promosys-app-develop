<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="ViewMemoMultiplePromo.aspx.vb" Inherits="ViewMemoMultiplePromo" Title ="Promotional Memo" %>
<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
<script type="text/javascript">
function OpenViewBranches(height,width)
{
    if (height == "") height= "250";
   
    if (width == "") width= "550";
    
    var title
    var page
    page = "MemoBranches.aspx";
    title = "Branches";
    Auditwindow=dhtmlmodal.open('Branches', 'iframe', page, title, 'width='+ width + 'px,height=' + height + 'px,center=1,resize=0,scrolling=0',"recall")
    Auditwindow.onclose=function()
    { 
	return true 
	}
}
</script>

<script type="text/javascript">
    var gAutoPrint = true; // Tells whether to automatically call the print function
    function printSpecial()
    {
        if (document.getElementById != null)
        {
            var html = '<HTML>\n<HEAD>\n';

            if (document.getElementsByTagName != null)
            {
            var headTags = document.getElementsByTagName("head");
                if (headTags.length > 0)
                html += headTags[0].innerHTML;
            }
            html += '\n</HE>\n<BODY>\n';
            var printReadyElem = document.getElementById("mainMemo");
            
            if (printReadyElem != null)
            {
                html += printReadyElem.innerHTML;
            }
            else
            {
                alert("Could not find the printReady function");
                return;
            }
            html += '\n</BO>\n</HT>';
            var printWin = window.open("","printSpecial");
            printWin.innerWidth =10;
            printWin.innerHeight = 10;
            printWin.screenX = screen.width;
            printWin.screenY = screen.height;
            alwaysLowered = true;

            printWin.document.open();
            printWin.document.write(html);
			
            // printWin.document.close();
            // if (gAutoPrint)
            // printWin.print();
            // printWin.close();
        }
        else
        {
            alert("The print ready feature is only available if you are using an browser. Please update your browswer.");
        }
}

function changeBGImage()
 
    {
    
         document.getElementById('tdMain').style.backgroundImage = "url(images/cancelled2.gif)";
         document.getElementById('tdMain').style.backgroundRepeat = "repeat-x";
         /*document.getElementById('tdMain').style.backgroundAttachment="fixed";*/
         document.getElementById('tdMain').style.backgroundPosition="200 200"; 

    }
    
    </script>

    <br />
    <br />
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkPrintMemo" runat="server">Print this Report</asp:LinkButton><br />
        <br />
        <asp:LinkButton ID="lnkBtnViewBranches" runat="server">View Branches</asp:LinkButton><br />
        <br />
        <asp:LinkButton
            ID="lnkDone" runat="server">Back to Announcements</asp:LinkButton><br />
        <br />
    </div>
    
    <div id="contents">
    
    <div id = "mainMemo">
         <table>
     <tr>
     <td id="tdMain">
     
    <table>
            <tr>
                <td style="height: 16px">        <table cellpadding="3px" cellspacing="0px" style="width: 702px">
            <tr>
                <td colspan="3" style="height: 6px; text-align: center">
                    <p class="unbold"><span>Merchandise
                        Planning Division</span></p></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td colspan="3" style="height: 5px; text-align: center">
                    <span style="font-size: 10pt; color: #000000; font-family: Verdana">
                    <asp:Label ID="lblReportTitle" runat="server"></asp:Label></span></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; height: 11px;">
                   <p><span style="font-size: 10pt">
                                    <asp:Label ID="lblMemoDate" runat="server" Width="117px" Font-Size="9pt"></asp:Label></span></p></td>
                <td colspan="2" style="width: 541px; border-right: black 1px solid; border-top: black 1px solid; border-bottom: black 1px solid; height: 11px; border-left-width: 1px; border-left-color: black; text-align: right;" align="right">
                <asp:Label ID="lblMemoNumber"
                                    runat="server" Width="171px" Font-Bold="True" Font-Italic="False" Font-Size="9pt">MPD-PR-</asp:Label></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px; border-right: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; height: 11px; border-top-width: 1px; border-top-color: black;" valign="top">
                    <p><span style="font-size: 10pt"><span style="font-size: 9pt; font-family: Arial"><strong>
                        Promo Title</strong></span></span></p></td>
                <td colspan="2" style="width: 541px; border-right: black 1px solid; border-bottom: black 1px solid; height: 11px; border-top-width: 1px; border-left-width: 1px; border-left-color: black; border-top-color: black;">
                    <asp:Label ID="lblPromoTitle" runat="server" Width="528px" Font-Bold="True" Font-Size="9pt"></asp:Label></td>
            </tr>
            <tr   id="trRefMemos" runat="server" style="font-family: Trebuchet MS">
                <td style="border-top-width: 1px; border-right: black 1px solid; border-left: black 1px solid;
                    width: 554px; border-top-color: black; border-bottom: black 1px solid;"
                    valign="top">
                    <p><span style="font-size: 10pt"><span style="font-size: 9pt; font-family: Arial">
                    Reference
                    <br />
                    Memo(s)</span></span></p></td>
                <td colspan="2" style="border-top-width: 1px; border-right: black 1px solid; border-left-width: 1px;
                    border-left-color: black; width: 541px; border-top-color: black; border-bottom: black 1px solid;">
                    <div class="literal">
                    <asp:GridView ID="gridRefMemos" runat="server" AutoGenerateColumns="False" BorderColor="Black" CellPadding="4" DataKeyNames="CRID"
                        DataSourceID="sqlRefMemos" ForeColor="#333333" ShowHeader="False"
                        Width="554px">
                        <RowStyle BackColor="White" ForeColor="Black" />
                        <Columns>
                            <asp:TemplateField Visible="False">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("MEMONUMBER") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnMemoNumber" runat="server" Text='<%# Bind("MEMONUMBER") %>'></asp:LinkButton>
                                    <asp:HiddenField ID="hidMemoID" runat="server" Value='<%# Bind("MEMOID") %>' />
                                    <asp:HiddenField ID="hidCRID" runat="server" Value='<%# Bind("CRID") %>' /><asp:HiddenField ID="hidReqType" runat="server" Value='<%# Bind("REQUESTTYPE") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MEMONUMBER" HeaderText="MemoNumber">
                                <ItemStyle Width="25%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TITLE" HeaderText="Title">
                                <ItemStyle Width="75%" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <EmptyDataTemplate>
                            &nbsp;
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="White" Font-Bold="True" Font-Names="Trebuchet MS" Font-Size="9pt"
                            ForeColor="Black" Height="5px" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
                    </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px; height: 1px;">
                    <span style="font-size: 7pt">&nbsp;</span></td>
                <td colspan="2" style="width: 541px; height: 1px;">
                    </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px; height: 5px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid;">
                    <p><span style="font-size: 9pt"><strong>Promo Type</strong></span></p></td>
                <td colspan="2" style="width: 541px; height: 5px; border-right: black 1px solid; border-top: black 1px solid; border-left-width: 1px; border-left-color: black; border-bottom: black 1px solid;">
                    <asp:Label ID="lblPromoType" runat="server" Font-Size="9pt" Width="528px"></asp:Label></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px; border-top-width: 1px; border-right: black 1px solid; border-left: black 1px solid; border-top-color: black; border-bottom: black 1px solid;">
                    <p><span style="font-size: 9pt"><strong>Promo Period</strong></span></p></td>
                <td colspan="2" style="width: 541px; border-top-width: 1px; border-right: black 1px solid; border-left-width: 1px; border-left-color: black; border-top-color: black; border-bottom: black 1px solid;">
                    <asp:Label ID="lblPromoPeriod" runat="server" Width="528px" Font-Size="9pt"></asp:Label></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px; height: 100%; border-top-width: 1px; border-right: black 1px solid; border-left: black 1px solid; border-top-color: black; border-bottom: black 1px solid;" valign="top">
                    <p><span style="font-size: 9pt"><strong>Branches</strong></span></p></td>
                <td colspan="2" style="width: 541px; border-top-width: 1px; border-right: black 1px solid; border-left-width: 1px; border-left-color: black; border-top-color: black; border-bottom: black 1px solid;" valign="top">
                    <div class="literal" style="overflow:hidden; width:569px; height:100%">
                        <asp:Literal ID="litBranches" runat="server"></asp:Literal>
                    </div>
                </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                    <td style="width: 554px; border-top-width: 1px; border-right: black 1px solid; border-left: black 1px solid; border-top-color: black; border-bottom: black 1px solid; height: 100%
                  ;" valign="top">
                    <p><span style="font-size: 9pt"><strong>Promotion:</strong></span></p></td>
                <td colspan="2" style="width: 541px; border-top-width: 1px; border-right: black 1px solid; border-left-width: 1px; border-left-color: black; border-top-color: black; border-bottom: black 1px solid; height: 2px;">
                <div class="literal">
                                    <asp:GridView  ID="gridPromotions" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="PromoID" DataSourceID="sqldsPromos"
                                        ForeColor="#333333" Width="568px" Height="93px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                        <RowStyle BackColor="White" ForeColor="Black" />
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPromoID" runat="server" Text='<%# Eval("PromoID") %>' Width="10px"></asp:Label>
                                                    <asp:Label ID="lblPromoTypeID" runat="server">'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dp/SDp/Cl/SCl" SortExpression="ItemCode" Visible="False">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeptSdeptClass" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Dept/SDep/Class" SortExpression="ItemCode" HtmlEncode="False">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PromoDesc" HeaderText="Description" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle HorizontalAlign="Center" />                                                
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Branches" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle Width="80px" HorizontalAlign="Center" VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <asp:GridView ID="gvBranches"  ShowHeader="false" runat="server" AutoGenerateColumns="False" Font-Size="8pt"
                                                        Width="258px">
                                                        <Columns>
                                                            <asp:BoundField DataField="shortdesc" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            &nbsp;
                                        </EmptyDataTemplate>
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="White" Font-Bold="True" ForeColor="Black" Font-Size="10pt" Height="5px" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
                                    </asp:GridView>
                </div>
                </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px; border-top-width: 1px; border-right: black 1px solid; border-left: black 1px solid; border-top-color: black; border-bottom: black 1px solid; height: 100%;" valign="top">
                    <p><span style="font-size: 9pt"><strong>Guideline:</strong></span></p></td>
                <td colspan="2" style="width: 541px; border-top-width: 1px; border-right: black 1px solid; border-left-width: 1px; border-left-color: black; border-top-color: black; border-bottom: black 1px solid; height: 100%;" valign="top">
                   <div class="literal"><asp:Literal ID="litGuidelines" runat="server"></asp:Literal></div>
                </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 554px">
                </td>
                <td colspan="2" style="width: 541px">
                </td>
            </tr>
        </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
        <table cellpadding="2" cellspacing="0" style="width: 702px">
            <tr>
                <td style="height: 22px; width: 42px;">
                </td>
                <td style="height: 22px; width: 215px; text-align: center;">
                    <asp:Label ID="lblPrepBy" runat="server" Font-Size="9pt" Width="210px" Font-Bold="True"></asp:Label></td>
                <td style="height: 22px; text-align: center;">
                    <asp:Label ID="lblReviewedBy" runat="server" Font-Size="9pt" Width="210px" Font-Bold="True"></asp:Label></td>
                <td style="height: 22px; text-align: center">
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Size="9pt" Width="210px" Font-Bold="True"></asp:Label></td>
            </tr>
            <tr>
                <td style="height: 15px; width: 42px;">
                </td>
                <td style="height: 15px; width: 215px; text-align: center;">
                    <asp:Label ID="lblPreparePos" runat="server" Font-Size="9pt" Width="210px"></asp:Label></td>
                <td style="height: 15px; text-align: center;">
                    <asp:Label ID="lblReviewerPos" runat="server" Font-Size="9pt" Width="210px"></asp:Label></td>
                <td style="height: 15px; text-align: center">
                    <asp:Label ID="lblApproverPos" runat="server" Font-Size="9pt" Width="210px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 42px">
                </td>
                <td style="width: 215px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
                </td>
            </tr>
        </table>
               </td>
       </tr>
       </table> 
    </div>
        <br />
        <asp:ImageButton ID="imgbtnDownload" runat="server" BackColor="White" BorderColor="White"
            BorderStyle="Solid" ImageUrl="~/Images/down.gif" ToolTip="Download attached files"
            Visible="False" /><asp:Literal ID="lblFiles" runat="server"></asp:Literal><br />
        <asp:SqlDataSource ID="sqldsPromos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+ '<br>' +RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, P.PromoDesc, TypeDesc&#13;&#10;FROM Promotions P inner join PromoDetails D ON P.PromoID = D.PromoID&#13;&#10;inner join PromoTypes PT ON P.PromoTypeID = PT.PromoTypeID  &#13;&#10;WHERE &#13;&#10;P.MemoID = @MemoID&#13;&#10;ORDER BY 1" UpdateCommand="UPDATE Promotions SET MemoID = @MemoID WHERE (RequestID = @RequestID)">
            <SelectParameters>
                <asp:SessionParameter Name="MemoID" SessionField="CurrMemoID" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter DefaultValue="0" Name="MemoID" />
                <asp:SessionParameter Name="RequestID" SessionField="RequestID" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDSPromotype" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="USP_SelectPromoTypeByMemoID" UpdateCommand="UPDATE Promotions SET MemoID = @MemoID WHERE (RequestID = @RequestID)" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="MemoID" SessionField="CurrMemoID" />
            </SelectParameters>
        </asp:SqlDataSource><asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
        SelectCommand="SELECT 1"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlRefMemos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
            SelectCommand="SELECT MemoNumber,Title , CRID, MemoID, RequestType &#13;&#10;FROM ChangeRequests &#13;&#10;WHERE Status = 'Approved' &#13;&#10;AND MemoID = @MemoID&#13;&#10;ORDER BY FinalApproveDate ASC&#13;&#10;">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="MemoID" SessionField="CurrMemoID" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    
</asp:Content>