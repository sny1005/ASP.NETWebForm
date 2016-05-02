<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchSecurities.aspx.cs" Inherits="HKeInvestWebApplication.SearchSecurities1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h10>Search Securites</h10>
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label runat="server" Text="Security Type : " CssClass="control-label col-md-2" AssociatedControlID="ddlSecurityType" ></asp:Label>
            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="True" ControlToValidate="ddlSecurityType">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RVST" runat="server" ErrorMessage="* Please fill in your searching information." ControlToValidate="ddlSecurityType" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" Text="* Please fill in your searching information." InitialValue="0"></asp:RequiredFieldValidator>
        </div>

        <div class="form-group">
            <asp:Label runat="server" Text="Security Name : " CssClass="control-label col-md-2" AssociatedControlID="SecName"></asp:Label>
            <asp:TextBox ID="SecName" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:Label runat="server" Text="Secutity Code : " CssClass="control-label col-md-2" AssociatedControlID="SecCode"></asp:Label>
            <asp:TextBox ID="SecCode" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
            <asp:RangeValidator ID="RAVSecCode" runat="server" ErrorMessage="Only Integer is accepted." MaximumValue="9999" Text="* Only Integer is accepted in searching Security Code" ControlToValidate="SecCode" CssClass="text-danger" EnableClientScript="False"></asp:RangeValidator>
        </div>
     
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10"><asp:Button ID="Search" runat="server" Text="SEARCH" CssClass="btn" OnClick="Search_Click" ControlToValidate="Search"></asp:Button></div>
        </div>

        <div class="form-group">
            <div>
            <asp:GridView ID="BondGV" runat="server" Visible="False" AllowSorting="True" OnSorting="BondGV_Sorting" AutoGenerateColumns="False" CellPadding="5" EmptyDataText = "No records Found" >
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Bond Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Bond Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="launchDate" HeaderText="Launch date" ReadOnly="True" SortExpression="date" />
                    <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" SortExpression="base"/>
                    <asp:BoundField DataField="size" DataFormatString="{0:n2}" HeaderText="Total Monetary value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Current price per share" ReadOnly="True" SortExpression="price"/>
                    <asp:BoundField DataField="sixMonths" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last six months" ReadOnly="True" SortExpression="cagp06"/>
                    <asp:BoundField DataField="oneYear" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last one year" ReadOnly="True" SortExpression="cagp12"/>
                    <asp:BoundField DataField="threeYears" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last three years" ReadOnly="True" SortExpression="cagp36"/>
                    <asp:BoundField DataField="sinceLaunch" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage since the bond was launched" ReadOnly="True" SortExpression="cagp00"/>
                </Columns>
            </asp:GridView>
            </div>
            
            <div>
            <asp:GridView ID="StockGV" runat="server" Visible="False" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" EmptyDataText = "No records Found">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Stock code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Stock Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="close" DataFormatString="{0:n2}" HeaderText="Most recent closing price per share" ReadOnly="True" SortExpression="recentclosing" />
                    <asp:BoundField DataField="changeDollar" DataFormatString="{0:n2}" HeaderText="Last trading day change" ReadOnly="True" SortExpression="lastchange" />
                    <asp:BoundField DataField="changePercent" DataFormatString="{0:n2}" HeaderText="Last trading day percentage change" ReadOnly="True" SortExpression="lastpercentage" />
                    <asp:BoundField DataField="volume" DataFormatString="{0:n2}" HeaderText="Last trading day volume of shares" ReadOnly="True" SortExpression="lastvolume" />
                    <asp:BoundField DataField="high" DataFormatString="{0:n2}" HeaderText="highprice" ReadOnly="True" />
                    <asp:BoundField DataField="low" DataFormatString="{0:n2}" HeaderText="lowprice" ReadOnly="True" />
                    <asp:BoundField DataField="peRatio" DataFormatString="{0:n2}" HeaderText="Price earnings ratio of the stock" ReadOnly="True" SortExpression="pe" />
                    <asp:BoundField DataField="yield" DataFormatString="{0:n2}" HeaderText="Yield of the stock" ReadOnly="True" SortExpression="yield" />
                </Columns>
            </asp:GridView>
            </div>

            <div>
            <asp:GridView ID="UTGV" runat="server" Visible="False" AllowSorting="True" AutoGenerateColumns="False" CellPadding="5" EmptyDataText = "No records Found">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="launchDate" HeaderText="Launch date" ReadOnly="True" SortExpression="date" />
                    <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" SortExpression="base"/>
                    <asp:BoundField DataField="size" DataFormatString="{0:n2}" HeaderText="Total Monetary value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Current price per share" ReadOnly="True" SortExpression="price"/>
                    <asp:BoundField DataField="riskReturn" HeaderText="Risk/return rating" ReadOnly="True" SortExpression="rating"/>
                    <asp:BoundField DataField="sixMonths" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last six months" ReadOnly="True" SortExpression="cagp06"/>
                    <asp:BoundField DataField="oneYear" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last one year" ReadOnly="True" SortExpression="cagp12"/>
                    <asp:BoundField DataField="threeYears" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last three years" ReadOnly="True" SortExpression="cagp36"/>
                    <asp:BoundField DataField="sinceLaunch" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage since the bond was launched" ReadOnly="True" SortExpression="cagp00"/>
                </Columns>
            </asp:GridView>
            </div>

        </div>
    </div>

</asp:Content>
