<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecuritiesTransactions.aspx.cs" Inherits="HKeInvestWebApplication.SecuritiesTransactions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>

    <h2>Buy/Sell Securities</h2>
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label ID="lblAccountNumber" runat="server" Text="Account number:" CssClass="col-md-3" ></asp:Label>
            <asp:Label ID="lblAccountBalance" runat="server" Text="Account balance:" CssClass="col-md-3" ></asp:Label>
        </div>

        <div class="form-group">
            <div class="col-md-3">
                <asp:RadioButtonList ID="rblTransType" runat="server" CellPadding="5" CellSpacing="5" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblTransType_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="buy">Buy</asp:ListItem>
                    <asp:ListItem Value="sell">Sell</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                    <asp:ListItem Value="0">Security type</asp:ListItem>
                    <asp:ListItem Value="bond">Bond</asp:ListItem>
                    <asp:ListItem Value="stock">Stock</asp:ListItem>
                    <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <asp:Panel ID="BondPanel" runat="server" Visible="false">
            <div class="form-group">
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="BondCode" runat="server" Text="Bond Code" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="BondCode" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Bond Code is required." ControlToValidate="BondCode" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator> 
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lblBondAmount" AssociatedControlID="BondAmount" runat="server" Text="Amount in HKD" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="BondAmount" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Amount(Shares) to buy(sell) is required." ControlToValidate="BondAmount" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>   
            </div>
        </asp:Panel>

        <asp:Panel ID="UnitPanel" runat="server" Visible="false">
            <div class="form-group">
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="UnitCode" runat="server" Text="Unit Trust Code" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="UnitCode" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Unit Trust Code is required." ControlToValidate="UnitCode" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator> 
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lblUnitAmount" AssociatedControlID="UnitAmount" runat="server" Text="Amount in HKD" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="UnitAmount" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Amount to buy is required." ControlToValidate="UnitAmount" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>   
            </div>
        </asp:Panel>

        <asp:Panel ID="StockPanel" runat="server" Visible="false">
            <div class="form-group">
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="StockCode" runat="server" Text="Stock Code" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="StockCode" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Bond Code is required." ControlToValidate="StockCode" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator> 
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lblStockShares" AssociatedControlID="StockShares" runat="server" Text="Quantity of shares to buy" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="StockShares" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Quantity of shares to buy is required." ControlToValidate="StockShares" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>   
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="expDate" runat="server" Text="Expiary date of order" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="expDate" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Expiary date is required." ControlToValidate="expDate" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="StockCode" runat="server" Text="Stock Code" CssClass="control-label"></asp:Label>
                    <asp:RadioButtonList ID="rblOrderType" runat="server" CellPadding="5" CellSpacing="5">
                        <asp:ListItem Value="market">Market order</asp:ListItem>
                        <asp:ListItem Value="limit">Limit order</asp:ListItem>
                        <asp:ListItem Value="stop">Stop order</asp:ListItem>
                        <asp:ListItem Value="stop limit">Stop limit order</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="hPrice" runat="server" Text="High price of order" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="hPrice" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="lPrice" runat="server" Text="Low price of order" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="lPrice" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-4">
                    <asp:Label AssociatedControlID="rblIsAll" runat="server" Text="All or None order?" CssClass="control-label"></asp:Label>
                    <asp:RadioButtonList ID="rblIsAll" runat="server" CellPadding="5" CellSpacing="5">
                        <asp:ListItem Value="y">Yes</asp:ListItem>
                        <asp:ListItem Value="n">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>  
            </div>
        </asp:Panel>

        <div class="form-group">
            <div class="col-md-3"><asp:Button ID="Confirm" runat="server" Text="Confirm" CssClass="btn" OnClick="Confirm_Transaction"></asp:Button></div>
        </div>
    </div>

</asp:Content>
