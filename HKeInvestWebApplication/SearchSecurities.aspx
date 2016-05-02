<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchSecurities.aspx.cs" Inherits="HKeInvestWebApplication.SearchSecurities1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h10>Search Securites</h10>
    <div class="form-horizontal">
        <div class="form-group">
            <asp:Label runat="server" Text="Security Type : " CssClass="control-label col-md-2" ></asp:Label>
            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="True">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label runat="server" Text="Security Name : " CssClass="control-label col-md-2" AssociatedControlID="SecName"></asp:Label>
            <asp:TextBox ID="SecName" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:Label runat="server" Text="Secutity Code : " CssClass="control-label col-md-2" AssociatedControlID="SecCode"></asp:Label>
            <asp:TextBox ID="SecCode" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
            <asp:RangeValidator ID="RVSecCode" runat="server" ErrorMessage="Only Integer is accepted." MaximumValue="9999" Text="* Only Integer is accepted in searching Security Code" ControlToValidate="SecCode"></asp:RangeValidator>
        </div>
     
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10"><asp:Button ID="Search" runat="server" Text="SEARCH" CssClass="btn" OnClick="Search_Click"></asp:Button></div>
            <asp:CustomValidator ID="CVSearch" runat="server" ErrorMessage="Please fill in your searching information." Text="* Please fill in your searching information." Display="Dynamic"></asp:CustomValidator>
        </div>
          
        <div class="form-group">
         <div>
            <asp:GridView ID="StockGV" runat="server" Visible="False" AutoGenerateColumns="False" CellPadding="5">
                <Columns>
                    <asp:BoundField DataField="StockCode" HeaderText="Stock code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="StockName" HeaderText="Stock Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="RecentClosing" DataFormatString="{0:n2}" HeaderText="Most recent closing price per share" ReadOnly="True" SortExpression="recentclosing" />
                    <asp:BoundField DataField="LastChange" DataFormatString="{0:n2}" HeaderText="Last trading day change" ReadOnly="True" SortExpression="lastchange" />
                    <asp:BoundField DataField="LastPercent" DataFormatString="{0:n2}" HeaderText="Last trading day percentage change" ReadOnly="True" SortExpression="lastpercentage" />
                    <asp:BoundField DataField="LastVolume" DataFormatString="{0:n2}" HeaderText="Last trading day volume of shares" ReadOnly="True" SortExpression="lastvolume" />
                    <asp:BoundField DataField="HighPrice" DataFormatString="{0:n2}" HeaderText="highprice" ReadOnly="True" />
                    <asp:BoundField DataField="LowPrice" DataFormatString="{0:n2}" HeaderText="lowprice" ReadOnly="True" />
                    <asp:BoundField DataField="PE" DataFormatString="{0:n2}" HeaderText="Price earnings ratio of the stock" ReadOnly="True" SortExpression="pe" />
                    <asp:BoundField DataField="Yield" DataFormatString="{0:n2}" HeaderText="Yield of the stock" ReadOnly="True" SortExpression="yield" />
                </Columns>
            </asp:GridView>
        </div>

         <div>
            <asp:GridView ID="BondGV" runat="server" Visible="False" AutoGenerateColumns="False" CellPadding="5">
                <Columns>
                    <asp:BoundField DataField="BondCode" HeaderText="Bond Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="Bondname" HeaderText="Bond Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="Launch" HeaderText="Launch date" ReadOnly="True" SortExpression="date" />
                    <asp:BoundField DataField="Base" HeaderText="Base" ReadOnly="True" SortExpression="base"/>
                    <asp:BoundField DataField="Value" DataFormatString="{0:n2}" HeaderText="Total Monetary value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="Price" DataFormatString="{0:n2}" HeaderText="Current price per share" ReadOnly="True" SortExpression="price"/>
                    <asp:BoundField DataField="CAGP06" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last six months" ReadOnly="True" SortExpression="cagp06"/>
                    <asp:BoundField DataField="CAGP12" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last one year" ReadOnly="True" SortExpression="cagp12"/>
                    <asp:BoundField DataField="CAGP36" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last three years" ReadOnly="True" SortExpression="cagp36"/>
                    <asp:BoundField DataField="CAGP00" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage since the bond was launched" ReadOnly="True" SortExpression="cagp00"/>
                </Columns>
            </asp:GridView>
         </div>

         <div>
            <asp:GridView ID="UnitTrustGV" runat="server" Visible="False" AutoGenerateColumns="False" CellPadding="5">
                <Columns>
                    <asp:BoundField DataField="UnitTrustCode" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="UnitTrustName" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="Launch" HeaderText="Launch date" ReadOnly="True" SortExpression="date" />
                    <asp:BoundField DataField="Base" HeaderText="Base" ReadOnly="True" SortExpression="base"/>
                    <asp:BoundField DataField="Value" DataFormatString="{0:n2}" HeaderText="Total Monetary value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="Price" DataFormatString="{0:n2}" HeaderText="Current price per share" ReadOnly="True" SortExpression="price"/>
                    <asp:BoundField DataField="Rating" HeaderText="Risk/return rating" ReadOnly="True" SortExpression="rating"/>
                    <asp:BoundField DataField="CAGP06" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last six months" ReadOnly="True" SortExpression="cagp06"/>
                    <asp:BoundField DataField="CAGP12" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last one year" ReadOnly="True" SortExpression="cagp12"/>
                    <asp:BoundField DataField="CAGP36" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage last three years" ReadOnly="True" SortExpression="cagp36"/>
                    <asp:BoundField DataField="CAGP00" DataFormatString="{0:n2}" HeaderText="Compound annual growth percentage since the bond was launched" ReadOnly="True" SortExpression="cagp00"/>
                </Columns>
            </asp:GridView>
        </div>
        </div>
    </div>

</asp:Content>
