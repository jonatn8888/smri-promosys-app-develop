<%@ Page Language="VB" MasterPageFile="~/PromoPage.master" AutoEventWireup="false" CodeFile="ViewRequest.aspx.vb" Inherits="ViewRequest" Title ="Promotion Request" %>

<asp:Content ID = "Content1" ContentPlaceHolderID = "ContentPlaceHolder1" Runat = "Server">
      
    
<script type="text/javascript">
function opentexteditor()
{
    var url
    var title
    url = "MsgBox.aspx";
    title = document.getElementById("<%=lblPopTitle.ClientID%>").value; 
    MsgBoxwindow=dhtmlmodal.open('MsgBox', 'iframe', url, title, 'width=510px,height=163px,center=1,resize=0,scrolling=0',"recall")
    MsgBoxwindow.onclose=function()
    { 
    var theform = this.contentDoc.forms[0] 
    document.getElementById("<%=cmdPopUpOK.ClientID%>").click(); 
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
            var printReadyElem = document.getElementById("main");
            
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
            
            var winW =630, winH = 460;

            if (parseInt(navigator.appVersion)>3) {
             if (navigator.appName=="Netscape") {
              winW = printWin.innerWidth - 626;
              winH = printWin.innerHeight - 459;
             }
             if (navigator.appName.indexOf("Microsoft")!=-1) {
              winW = printWin.document.body.offsetWidth - 626;
              winH = printWin.document.body.offsetHeight - 459;
             }
            }
                    
            printWin.screenX = screen.width;
            printWin.screenY = screen.height;
            alwaysLowered = true;

            printWin.document.open();
            printWin.document.write(html);
            printWin.document.close();
            if (gAutoPrint)
            printWin.print();
            printWin.close();
        }
        else
        {
            alert("The print ready feature is only available if you are using an browser. Please update your browswer.");
        }
}

    </script>
        
    <br />
    <br />
        
    <div id="menu">
        <br />
        <br />
        <asp:LinkButton ID="lnkPrintMemo" runat="server"  Height="5px">Print this Document</asp:LinkButton><br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:LinkButton ID="lnkDone" runat="server">Close Print Preview</asp:LinkButton>
        <br />
        <br />
    </div>
    
    <div id="contents">
    <div id="main">
     <div id="body">
     
        <table cellpadding="3px" cellspacing="0px" style="width: 700px; position: static">
            <tr>
                <td colspan="2" style="height: 6px; text-align: center">
                     <p class="unbold"><span style="font-family: Verdana;"><span style="font-family: Tahoma;">Merchandise Planning Division</span></span></p></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td colspan="2" style="text-align: center; width: 677px;">
                    <span style="font-size: 10pt; color: #000000; font-family: Verdana">
                        <strong>PROMOTION REQUEST</strong>
                    </span>
                </td>
            </tr>
            <!--Added dowcarpio07232012@smretailinc: Display 'Draft' status field for all draft promotional requests.-->
            <tr id="trStatus" runat="server"  style="font-family: Trebuchet MS">
                <td colspan="2" style="width: 677px; text-align: center">
                    <table cellpadding="3" cellspacing="0" style="width: 700px; position: static">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Names="Tahoma" ForeColor="Green"
                                    Width="207px">
                             </asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 122px; border-right: black 1px solid; border-top: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid;">
                    <p><span style="font-size: 10pt">
                                    <asp:Label ID="lblDocDate" runat="server" Width="117px" Font-Size="9pt"></asp:Label></span></p></td>
                <td style="width: 550px; border-right: black 1px solid; border-top: black 1px solid; border-bottom: black 1px solid; border-left-width: 1px; border-left-color: black; text-align: right; font-size: 7pt;" align="right">
                <asp:Label ID="lblDocNumber" runat="server" Width="171px" Font-Bold="True" Font-Italic="False" Font-Size="9pt">PR-</asp:Label></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 122px; border-right: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; border-top-width: 1px; border-top-color: black;" valign="top">
                    <p><span style="font-size: 10pt"><span style="font-size: 9pt;">
                        Promo Title</span></span></p></td>
                <td style="width: 550px; border-right: black 1px solid; border-bottom: black 1px solid; border-top-width: 1px; border-left-width: 1px; border-left-color: black; border-top-color: black;">
                    <asp:Label ID="lblPromoTitle" runat="server" Width="562px" Font-Bold="True" Font-Size="9pt"></asp:Label></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 122px; height: 1px;">
                    <span style="font-size: 7pt">&nbsp;</span></td>
                <td style="width: 550px; height: 1px;">
                    </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 122px; border-right: black 1px solid; border-left: black 1px solid; border-bottom: black 1px solid; border-top: black 1px solid;">
                    <p><span style="font-size: 9pt">Promo Period</span></p></td>
                <td style="width: 550px; border-right: black 1px solid; border-left-width: 1px; border-left-color: black; border-bottom: black 1px solid; border-top: black 1px solid;">
                    <asp:Label ID="lblPromoPeriod" runat="server" Width="528px" Font-Size="9pt"></asp:Label></td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 122px; height: 100%; border-top-width: 1px; border-right: black 1px solid; border-left: black 1px solid; border-top-color: black; border-bottom: black 1px solid;" valign="top">
                    <p><span style="font-size: 9pt">Branches</span></p></td>
                <td style="width: 550px; height: 100%; border-top-width: 1px; border-right: black 1px solid; border-left-width: 1px; border-left-color: black; border-top-color: black; border-bottom: black 1px solid;" valign="top">
                    <div class="literal" style="overflow:hidden; width:569px; height:100%">
                        <asp:Literal ID="litBranches" runat="server"></asp:Literal>
                    </div>
                </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 122px; border-top-width: 1px; border-right: black 1px solid; border-left: black 1px solid; border-top-color: black; border-bottom: black 1px solid; height: 185px;" valign="top">
                    <p><span style="font-size: 9pt">Promotions:</span></p></td>
                <td style="height: 185px; border-bottom: black 1px solid; border-right: black 1px solid; width: 550px;" valign="top">

                <asp:GridView id="gridPromotions" runat="server" DataSourceID="sqldsPromos" DataKeyNames="PromoID" AutoGenerateColumns="False" cssclass="mGrid" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" Width="550px">
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPromoID" runat="server" Text='<%# Eval("PromoID") %>' Width="10px"></asp:Label>
                                                    <asp:Label ID="lblPromoTypeID" runat="server" Text='<%# Eval("PromoTypeID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Dp/SDp/Cl/SCl" ReadOnly="True" SortExpression="ItemCode" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <ItemStyle HorizontalAlign="Center" Width="100px" VerticalAlign="Top"  />
                                                <HeaderStyle HorizontalAlign="Center"  />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PromoDesc" HeaderText="Description" SortExpression="PromoDesc" HtmlEncode="False" HtmlEncodeFormatString="False" >
                                                <HeaderStyle HorizontalAlign="Center"  />
                                                <ItemStyle VerticalAlign="Top"  />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Branches" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center"  />
                                                <ItemStyle Width="150px" VerticalAlign="Top"  />
                                                <ItemTemplate>
                                                    <asp:GridView ShowHeader="false" ID="gvBranches" runat="server" AutoGenerateColumns="False" Font-Size="8pt" Width="136px">
                                                        <Columns>
                                                            <asp:BoundField DataField="shortdesc" ></asp:BoundField>
                                                        </Columns>
                                                        <RowStyle BorderStyle="None" BorderWidth="0px"  />
                                                    </asp:GridView>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pgr"  />
                                        <EmptyDataTemplate>
                                            <asp:CheckBox ID="chkRowSel" runat="server"  />
                                        </EmptyDataTemplate>
                                        <HeaderStyle Font-Size="10pt" Height="5px" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"  />
                                        <AlternatingRowStyle CssClass="alt"  />
                                    </asp:GridView>
                </td>
            </tr>
            <tr style="font-family: Trebuchet MS">
                <td style="width: 122px; height: 7px;">
                </td>
                <td style="width: 550px; height: 7px;">
                    &nbsp;</td>
            </tr>
        </table>
        
     </div>
     <div id="footer">
            <table cellpadding="2" cellspacing="0" style="width: 700px">
            <tr>
                <td style="height: 22px; width: 122px;">
                </td>
                <td style="height: 22px; width: 291px; text-align: center;">
                    <asp:Label ID="lblRequestedBy" runat="server" Font-Size="9pt" Width="210px" Font-Bold="True"></asp:Label></td>
                <td style="height: 22px; text-align: center">
                    <asp:Label ID="lblApprovedBy" runat="server" Font-Size="9pt" Width="210px" Font-Bold="True"></asp:Label></td>
            </tr>
            <tr>
                <td style="height: 15px; width: 122px;">
                </td>
                <td style="height: 15px; width: 291px; text-align: center;">
                    <asp:Label ID="lblRequestPos" runat="server" Font-Size="9pt" Width="210px"></asp:Label></td>
                <td style="height: 15px; text-align: center">
                    <asp:Label ID="lblApproverPos" runat="server" Font-Size="9pt" Width="210px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 122px">
                </td>
                <td style="width: 291px">
                </td>
                <td>
                </td>
            </tr>
        </table>
        </div>
    </div>
    
    <asp:SqlDataSource ID="sqldsData" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        SelectCommand="SELECT 1">
                                        </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sqldsPromos" runat="server" ConnectionString="<%$ ConnectionStrings:PromoConnectionString %>"
                                        
                                            SelectCommand="SELECT dbo.Fn_FormatPromoCode(D.DepCode,'Dp')+'-'+dbo.Fn_FormatPromoCode(D.SubDepCode,'SDp')+'-'+dbo.Fn_FormatPromoCode(D.ClassCode,'Cl')+'-'+dbo.Fn_FormatPromoCode(D.SubClassCode,'SCl')+'<br />'+RTrim(D.ShortDesc) AS ItemCode,  P.PromoID, dbo.Fn_FormatPromoDesc(P.PromoID,@UserID) AS PromoDesc, P.PromoTypeID&#13;&#10;FROM Promotions AS P left outer join PromoDetails AS D on  P.PromoID = D.PromoID&#13;&#10;WHERE P.RequestID = @RequestID&#13;&#10;ORDER BY P.PromoID"  UpdateCommand="UPDATE Promotions SET MemoID = @MemoID WHERE RequestID = @RequestID and PromoID = @PromoID">
                                           <SelectParameters>
                                            <asp:SessionParameter Name="RequestID" SessionField="CurrRequestID" />
                                            <asp:SessionParameter Name="UserID" SessionField="UserID" />
                                        </SelectParameters>
                                        <UpdateParameters>
                                            <asp:Parameter DefaultValue="0" Name="MemoID" />
                                            <asp:SessionParameter Name="RequestID" SessionField="RequestID" />
                                            <asp:SessionParameter Name="PromoID" SessionField="PromoId" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
    </div>
        
    <div style="display:none">
      <asp:HiddenField ID="NewRequestIDHidden" runat="server" />
        <asp:Button ID="cmdPopUpOK" runat="server" Text="Button" />
        <asp:HiddenField ID="lblPopTitle" runat="server" />
    </div>

</asp:Content>