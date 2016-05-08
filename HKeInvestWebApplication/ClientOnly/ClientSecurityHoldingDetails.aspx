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
            <asp:GridView ID="gvSecurityHolding" runat="server" Visible="False" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" CellPadding="5" AllowSorting="true">
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

    <br />
    <h5>Rrquirement 6c - listing of active orders</h5>
    <div class="form-horizontal">
        <asp:Label ID="lblActiveBond" runat="server" Text="Listing of active bond/unit trust order" Visible="False" CssClass="h6"></asp:Label>
        <div>
            <asp:GridView ID="gvActiveBond" runat="server" Visible="false" AutoGenerateColumns="False" OnSorting="gvActive_Sorting" CellPadding="5" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="orderNumber" HeaderText="Order Reference Number" ReadOnly="True"/>
                    <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="True"/>
                    <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True"/>
                    <asp:BoundField DataField="securitycode" HeaderText="Security Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Security Name" ReadOnly="True" />
                    <asp:BoundField DataField="dateSubmitted" DataFormatString="{0:d}" HeaderText="Date Submitted" ReadOnly="True" SortExpression="datesubmitted" />
                    <asp:BoundField DataField="status" HeaderText="Current status" ReadOnly="True"/>
                    <asp:BoundField DataField="amount" DataFormatString="{0:n2}" HeaderText="Dollar amount/Quantity of shares" ReadOnly="True"/>
                </Columns>
            </asp:GridView>
        </div>
        <br />

        <asp:Label ID="lblActiveStock" runat="server" Text="Listing of active stock order" Visible="False" CssClass="h6"></asp:Label>
        <div>
            <asp:GridView ID="gvActiveStock" runat="server" Visible="false" AutoGenerateColumns="False" OnSorting="gvActive_Sorting" CellPadding="5" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="orderNumber" HeaderText="Order Reference Number" ReadOnly="True"/>
                    <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="True"/>
                    <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True"/>
                    <asp:BoundField DataField="securitycode" HeaderText="Security Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Security Name" ReadOnly="True" />
                    <asp:BoundField DataField="dateSubmitted" DataFormatString="{0:d}" HeaderText="Date Submitted" ReadOnly="True" SortExpression="datesubmitted" />
                    <asp:BoundField DataField="expiaryDay" HeaderText="Expiary Date" ReadOnly="True"/>
                    <asp:BoundField DataField="status" HeaderText="Current status" ReadOnly="True"/>
                    <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Quantity of shares" ReadOnly="True"/>
                    <asp:BoundField DataField="limitPrice" DataFormatString="{0:n2}" HeaderText="Limit Price" ReadOnly="True"/>
                    <asp:BoundField DataField="stopPrice" DataFormatString="{0:n2}" HeaderText="Stop Price" ReadOnly="True"/>
                </Columns>
            </asp:GridView>
        </div>

        <div class="form-group">
            <div class="col-md-3"><asp:Button ID="generate" runat="server" Text="Generate 6c" CssClass="btn" OnClick="generate6c_Click"></asp:Button></div>
        </div>
    </div>

    <br />
    <h5>Rrquirement 6d - listing of order history</h5>
    <div class="form-horizontal">
        <div class="form-group">                
            <asp:Label ID="Label1" runat="server" Text="From: " CssClass="control-label col-md-1"></asp:Label>
            <div class="col-md-2">
                <asp:TextBox ID="startDate" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:Label ID="Label2" runat="server" Text="To: " CssClass="control-label col-md-1"></asp:Label>
            <div class="col-md-2">
                <asp:TextBox ID="endDate" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">       
            <asp:Label ID="Label3" runat="server" Text="Filter listing by: " CssClass="h6 col-md-2"></asp:Label>
        </div>
        <div class="form-group">       
            <asp:Label ID="Label4" runat="server" Text="Security Code: " CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-2">
                <asp:TextBox ID="codeFilter" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlOrderFilter" runat="server">
                    <asp:ListItem Value="0">Type of Order</asp:ListItem>
                    <asp:ListItem Value="buy">Buy</asp:ListItem>
                    <asp:ListItem Value="sell">Sell</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlTypeFilter" runat="server">
                    <asp:ListItem Value="0">Security type</asp:ListItem>
                    <asp:ListItem Value="bond">Bond</asp:ListItem>
                    <asp:ListItem Value="stock">Stock</asp:ListItem>
                    <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlStatusFilter" runat="server">
                    <asp:ListItem Value="0">Order Status</asp:ListItem>
                    <asp:ListItem Value="pending">Pending</asp:ListItem>
                    <asp:ListItem Value="partial">Partial</asp:ListItem>
                    <asp:ListItem Value="completed">Completed</asp:ListItem>
                    <asp:ListItem Value="cancelled">Cancelled</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>

        <div class="form-group">
            <asp:GridView ID="gvHistory" runat="server" Visible="false" AutoGenerateColumns="False" OnSorting="gvHistory_Sorting" CellPadding="5" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="orderNumber" HeaderText="Order Reference Number" ReadOnly="True"/>
                    <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="True"/>
                    <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True" SortExpression="securitytype"/>
                    <asp:BoundField DataField="securityCode" HeaderText="Security Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Security Name" ReadOnly="True" SortExpression="name"/>
                    <asp:BoundField DataField="dateSubmitted" DataFormatString="{0:d}" HeaderText="Date Submitted" ReadOnly="True" SortExpression="datesubmitted" />
                    <asp:BoundField DataField="status" HeaderText="Current status" ReadOnly="True" SortExpression="status"/>
                    <asp:BoundField DataField="amount" DataFormatString="{0:n2}" HeaderText="Dollar amount/Quantity of shares" ReadOnly="True"/>
                    <asp:BoundField DataField="feeCharged" DataFormatString="{0:n2}" HeaderText="Total Fee Charged" ReadOnly="True"/>
                    <asp:BoundField DataField="totalShares" DataFormatString="{0:n2}" HeaderText="Total number of shares bought/sold" ReadOnly="True"/>
                    <asp:BoundField DataField="totalAmount" DataFormatString="{0:n2}" HeaderText="Total executed dollar amount" ReadOnly="True"/>
                </Columns>
            </asp:GridView>
        </div>

        <div class="form-group">
            <asp:GridView ID="gvTransaction" runat="server" Visible="false" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" CellPadding="5" AllowSorting="True">
                <Columns>
                    <asp:BoundField DataField="transactionNumber" HeaderText="Transaction Number" ReadOnly="True"/>
                    <asp:BoundField DataField="executeDate" DataFormatString="{0:d}" HeaderText="Date Executed" ReadOnly="True"/>
                    <asp:BoundField DataField="executeShares" HeaderText="Quantity of Shares" ReadOnly="True"/>
                    <asp:BoundField DataField="executePrice" HeaderText="Price paid per share in base currency" ReadOnly="True" />
                </Columns>
            </asp:GridView>
        </div>


        <div class="form-group">
            <div class="col-md-3"><asp:Button ID="Button1" runat="server" Text="Generate 6d" CssClass="btn" OnClick="generate6d_Click"></asp:Button></div>
        </div>
    </div>
</asp:Content>
