<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HKeInvestWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>HKeInvest</h1>
        <p class="lead">Hong Kong Electronic Investments LLC (HKeInvest) management system allows its clients to manage the securities that they own and to trade these securities through the Web</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
         <div class="col-md-4">
            <h2>Application Form</h2>
            <p>
               For security account application, complete the form and either taken to or mailed to HKeInvest’s office for processing.
            </p>
            <p>
                <a class="btn btn-default" href="Application Form.pdf">Download Form</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Search Securities</h2>
            <p>
                Start searching for investment opportunities!
            </p>
            <p>
                <a class="btn btn-default" href="SearchSecurities.aspx">GO!</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Securities Transaction (Client Only)</h2>
            <p>
                Start trading bond, unit trust and stock here!
            </p>
            <p>
                <a class="btn btn-default" href="ClientOnly/SecuritiesTransactions.aspx">Trade Now!</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Check Current Security Holdings (Client Only)</h2>
            <p>
                Start viewing bond, unit trust and stock in your account!
            </p>
            <p>
                <a class="btn btn-default" href="ClientOnly/ClientSecurityHoldingDetails.aspx">View Now!</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Track Profit and Loss (Client Only)</h2>
            <p>
                View the current status of your account!
            </p>
            <p>
                <a class="btn btn-default" href="ClientOnly/trackProfitOrLoss.aspx">Try!</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Set Alert (Client Only)</h2>
            <p>
                Remind yourself!
            </p>
            <p>
                <a class="btn btn-default" href="ClientOnly/SetAlert.aspx">Set Now!</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Modify account information (Client Only)</h2>
            <p>
                Remind yourself!
            </p>
            <p>
                <a class="btn btn-default" href="ClientOnly/ClientModify.aspx">Modify Now!</a>
            </p>
        </div>
         <div class="col-md-4">
            <h2>Important Event</h2>
            <p>
              Check for important dates and events here.
            </p>
            <p>
                <a class="btn btn-default" href="Calendar.aspx">View Here</a>
            </p>
        </div>
                 <div class="col-md-4">
            <h2>Service Fees</h2>
            <p>
              Check for HkeInvest Sevice Fees
            </p>
            <p>
                <a class="btn btn-default" href="ServiceFees.aspx">View Here</a>
            </p>
        </div>
    </div>

</asp:Content>
