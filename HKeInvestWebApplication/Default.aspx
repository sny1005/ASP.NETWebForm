<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HKeInvestWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>HKeInvest</h1>
        <p class="lead">Hong Kong Electronic Investments LLC (HKeInvest) management system allows its clients to manage the securities that they own and to trade these securities through the Web</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Securities Transaction</h2>
            <p>
                Start trading bond, unit trust and stock here!
            </p>
            <p>
                <a class="btn btn-default" href="ClientOnly/SecuritiesTransactions.aspx">Trade Now!</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Check Current Security Holdings</h2>
            <p>
                Start viewing bond, unit trust and stock in your account!
            </p>
            <p>
                <a class="btn btn-default" href="ClientOnly/ClientSecurityHoldingDetails.aspx">View Now!</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
