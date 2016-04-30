<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServiceFees.aspx.cs" Inherits="HKeInvestWebApplication.ServiceFees" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="text-center">

        <h2>HKeInvest Service Fees</h2>

    </div>
    <table style="width:100%;">
        <tr>
            <td style="height: 20px; text-decoration: underline; background-color: #3399FF;"><strong>Stock Order</strong></td>
            <td style="height: 20px; text-decoration: underline; background-color: #3399FF;">Assets Less than HK$1,000,000</td>
            <td style="height: 20px; text-decoration: underline; background-color: #3399FF;">Assests HK$1,000,000 or more</td>
        </tr>
        <tr>
            <td style="height: 21px">Minimum fee</td>
            <td style="height: 21px">$150</td>
            <td style="height: 21px">$100</td>
        </tr>
        <tr>
            <td style="height: 20px">Market order</td>
            <td style="height: 20px">0.4%</td>
            <td style="height: 20px">0.2%</td>
        </tr>
                <tr>
            <td style="height: 20px">Limit or stop order</td>
            <td style="height: 20px">0.6%</td>
            <td style="height: 20px">0.4%</td>
        </tr>
                <tr>
            <td style="height: 20px">Stop limit order</td>
            <td style="height: 20px">0.8%</td>
            <td style="height: 20px">0.6%</td>
        </tr>
                <tr>
            <td style="height: 21px; text-decoration: underline; background-color: #3399FF;"><strong>Bond/Unit Trust Order</strong></td>
            <td style="height: 21px; text-decoration: underline; background-color: #3399FF;">Assets less than HK$500,000</td>
            <td style="height: 21px; text-decoration: underline; background-color: #3399FF;">Assets HK$500,000 or more</td>
        </tr>
                        <tr>
            <td style="height: 20px">Buying fee</td>
            <td style="height: 20px">5%</td>
            <td style="height: 20px">3%</td>
        </tr>
                        <tr>
            <td style="height: 20px">Selling fee</td>
            <td style="height: 20px">$100</td>
            <td style="height: 20px">$50</td>
        </tr>

    </table>
</asp:Content>
