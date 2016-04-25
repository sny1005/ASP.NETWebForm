<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSecurityHoldingDetails.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ClientSecurityHoldingsDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Holding Details</h2>
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label ID="lblAccountNumber" runat="server" Text="Account number:" CssClass="col-md-3" ></asp:Label>
            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" Visible="False" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                <asp:ListItem Value="0">Currency</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblClientName" runat="server" Text="" Visible="False" CssClass="col-md-3"></asp:Label>
            <asp:Label ID="lblResultMessage" runat="server" Text="" Visible="False"></asp:Label>
        </div>
        <div>
            <asp:GridView ID="gvSecurityHolding" runat="server" Visible="False" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" CellPadding="5" OnSelectedIndexChanged="gvSecurityHolding_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Shares" ReadOnly="True" SortExpression="shares" />
                    <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" />
                    <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Price" ReadOnly="True" />
                    <asp:BoundField DataField="value" DataFormatString="{0:n2}" HeaderText="Value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="convertedValue" DataFormatString="{0:n2}" HeaderText="Value in" ReadOnly="True" SortExpression="convertedValue" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <h5>Rrquirement 6c - listing of active orders</h5>
    <div class="form-horizontal">
        <asp:Label ID="lblActiveBond" runat="server" Text="Listing of active bond/unit trust order" Visible="False" CssClass="h6"></asp:Label>
        <div>
            <asp:GridView ID="gvActiveBond" runat="server" Visible="true" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" CellPadding="5" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="orderNumber" HeaderText="Order Reference Number" ReadOnly="True"/>
                    <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="True"/>
                    <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True"/>
                    <asp:BoundField DataField="securitycode" HeaderText="Security Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Security Name" ReadOnly="True" />
                    <asp:BoundField DataField="dateSubmitted" HeaderText="Date Submitted" ReadOnly="True" SortExpression="date" />
                    <asp:BoundField DataField="status" HeaderText="Current status" ReadOnly="True"/>
                    <asp:BoundField DataField="amount" DataFormatString="{0:n2}" HeaderText="Dollar amount/Quantity of shares" ReadOnly="True"/>
                </Columns>
            </asp:GridView>
        </div>
        <br />

        <asp:Label ID="lblActiveStock" runat="server" Text="Listing of active stock order" Visible="False" CssClass="h6"></asp:Label>
        <div>
            <asp:GridView ID="gvActiveStock" runat="server" Visible="true" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" CellPadding="5" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="orderNumber" HeaderText="Order Reference Number" ReadOnly="True"/>
                    <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="True"/>
                    <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True"/>
                    <asp:BoundField DataField="securitycode" HeaderText="Security Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Security Name" ReadOnly="True" />
                    <asp:BoundField DataField="dateSubmitted" HeaderText="Date Submitted" ReadOnly="True" SortExpression="date" />
                    <asp:BoundField DataField="expiaryDay" HeaderText="Expiary Date" ReadOnly="True" SortExpression="date" />
                    <asp:BoundField DataField="status" HeaderText="Current status" ReadOnly="True"/>
                    <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Quantity of shares" ReadOnly="True"/>
                    <asp:BoundField DataField="limitPrice" DataFormatString="{0:n2}" HeaderText="Limit Price" ReadOnly="True"/>
                    <asp:BoundField DataField="stopPrice" DataFormatString="{0:n2}" HeaderText="Stop Price" ReadOnly="True"/>
                </Columns>
            </asp:GridView>
        </div>

        <div class="form-group">
            <div class="col-md-3"><asp:Button ID="generate" runat="server" Text="Generate" CssClass="btn" OnClick="generate_Click"></asp:Button></div>
        </div>
    </div>
</asp:Content>
