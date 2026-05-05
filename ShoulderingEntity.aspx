<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ShoulderingEntity.aspx.vb"
    Inherits="ShoulderingEntity" EnableViewState="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Shouldering Entity</title>
    <style>
       body {
                font-family: "Segoe UI", Tahoma, sans-serif;
                background-color: #f4f6f8;
                margin: 0;
                padding: 0;
            }

            .container {
                max-width: 95%;
                margin: 20px auto;
                background-color: #fff;
                padding: 20px;
                box-shadow: 0 3px 10px rgba(0,0,0,0.1);
                border-radius: 8px;
                overflow-x: auto; /* Horizontal scroll for wide tables */
            }

            h2 {
                text-align: center;
                color: #004b91;
                margin-bottom: 20px;
            }

            .responsive-grid {
                width: 100%;
                min-width: 1000px; /* Prevent squeezing */
                border-collapse: collapse;
            }

            .responsive-grid th, .responsive-grid td {
                border: 1px solid #ddd;
                padding: 10px 12px;
                text-align: center;
            }

            .responsive-grid th {
                background-color: #2B73A7;
                color: #fff;
                font-weight: 500;
            }

            .responsive-grid select, .responsive-grid input[type="text"] {
                width: 100%;
                max-width: 100%;
                padding: 6px;
                border: 1px solid #ccc; /* ✅ Fixed typo */
                border-radius: 4px;
                box-sizing: border-box;
            }

            .responsive-grid select:focus, .responsive-grid input[type="text"]:focus {
                border-color: #007bff;
                outline: none;
            }

            .action-buttons {
                text-align: center;
                margin-top: 20px;
            }

            .aspNetButton {
                padding: 8px 18px;
                margin: 0 5px;
                background-color: #007bff;
                color: #fff;
                border: none;
                border-radius: 5px;
                cursor: pointer;
                font-size: 14px;
                transition: background 0.3s;
            }

            .aspNetButton:hover {
                background-color: #0056b3;
            }

            .remove-btn {
                padding: 5px 12px;
                background-color: #dc3545;
                color: #fff;
                border: none;
                border-radius: 4px;
                cursor: pointer;
                transition: background 0.3s;
            }

            .remove-btn:hover {
                background-color: #b52a37;
            }

            #lblResult {
                display: block;
                margin-top: 15px;
                text-align: center;
                font-weight: bold;
            }

    </style>
  
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Shouldering Entity<asp:GridView ID="gvRows" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                CssClass="responsive-grid" OnRowDataBound="gvRows_RowDataBound">
                <Columns>
                
                    <asp:TemplateField HeaderText="ShoulderingEntity" Visible=false HeaderStyle-Width="20px" HeaderStyle-Font-Size="Small">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Entity" HeaderStyle-Width="100px" HeaderStyle-Font-Size="Small">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlMain" runat="server" AutoPostBack="True" 
                                OnSelectedIndexChanged="ddlMain_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="MBU/RA" HeaderStyle-Width="100px" Visible="false" HeaderStyle-Font-Size="Small">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlOtherBU" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlOtherBU_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Financing Partner" HeaderStyle-Width="100px" HeaderStyle-Font-Size="Small">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlFinancing" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlFinancingType_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Selection 1" HeaderStyle-Width="100px" HeaderStyle-Font-Size="Small">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlFinancialPartnerSubType"  runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlFinancialPartnerSubType_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Selection 2" HeaderStyle-Width="100px" HeaderStyle-Font-Size="Small">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlFinancialPartner" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlFinancialPartner_SelectedIndexChanged"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Percentage" HeaderStyle-Width="20px" HeaderStyle-Font-Size="Small">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPercent" CssClass="decimal2" Width="50px" runat="server"></asp:TextBox>                           
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Button ID="btnRemove" runat="server" Text="Remove"
                                CommandName="RemoveRow"
                                CommandArgument="<%# Container.DataItemIndex %>"
                                CssClass="remove-btn" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            </h2>

            <div class="action-buttons">
                <asp:Button ID="btnAddRow" runat="server" Text="Add Row" CssClass="aspNetButton" OnClick="btnAddRow_Click" />
     
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="aspNetButton" />
            </div>

            <asp:Label ID="lblResult" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
<script type="text/javascript" src="jQuery/jquery-1.4.3.min.js"></script>
<script type="text/javascript">
$(function () {

    $(".decimal2").each(function () {
        if ($.trim($(this).val()) === "") {
            $(this).val("0.00");
        }
    });

    $(".decimal2").focus(function () {
        var $this = $(this);
        // Remove default if cursor at start and value is 0.00
        if ($this.val() === "0.00") {
            // Give user a clean field to type
            $this.val("");
        }
    });

    $(".decimal2").keypress(function (e) {
        var char = String.fromCharCode(e.which);
        var val = $(this).val();

        // Allow backspace, delete, tab
        if (e.which === 0 || e.which === 8 || e.which === 9) return true;

        // Only digits or dot
        if (!char.match(/[0-9.]/)) return false;

        // Only one decimal point
        if (char === "." && val.indexOf(".") !== -1) return false;

        // Limit decimal places to 2
        if (val.indexOf(".") !== -1) {
            var dec = val.split(".")[1];
            if (dec && dec.length >= 2) return false;
        }

        return true;
    });

    $(".decimal2").blur(function () {
        var $this = $(this);
        var v = $.trim($this.val());

        // If empty or invalid, set default 0.00
        if (v === "" || v === "." || isNaN(v)) {
            $this.val("0.00");
            return;
        }

        // Format to 2 decimals
        $this.val(parseFloat(v).toFixed(2));
    });

});


</script>
