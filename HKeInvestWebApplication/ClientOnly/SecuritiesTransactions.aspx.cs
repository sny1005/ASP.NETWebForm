using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication
{
    public partial class SecuritiesTransactions : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        static string accountNumber = null;
        static decimal balance = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.Visible = false;

            if (!Page.IsPostBack)
            {
                if (Session["CurrencyData"] == null)
                {
                    DataTable CurrencyTable = myHKeInvestCode.CurrencyData();
                    string[,] CurrencyData = new string[CurrencyTable.Columns.Count, CurrencyTable.Rows.Count];

                    int i = 0;
                    foreach (DataRow row in CurrencyTable.Rows)
                    {
                        CurrencyData[0, i] = Convert.ToString(row["currency"]);
                        CurrencyData[1, i] = Convert.ToString(row["rate"]);
                        i++;
                    }

                    Session.Add("CurrencyData", CurrencyData);
                }

                //get the account number of the current logged in user
                string username = User.Identity.Name;
                accountNumber = myHKeInvestCode.getAccountNumber(username);

                lblAccountNumber.Text = "Account number: " + accountNumber;
            }

            //update account balance every page load
            balance = myHKeInvestCode.getAccountBalance(accountNumber);
            lblAccountBalance.Text = "Account balance: " + balance;
        }

        //UI change
        protected void ddlSecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSecurityType.SelectedValue == "bond")
            {
                BondPanel.Visible = true;
                UnitPanel.Visible = false;
                StockPanel.Visible = false;
            }
            else if (ddlSecurityType.SelectedValue == "unit trust")
            {
                BondPanel.Visible = false;
                UnitPanel.Visible = true;
                StockPanel.Visible = false;
            }
            else if (ddlSecurityType.SelectedValue == "stock")
            {
                BondPanel.Visible = false;
                UnitPanel.Visible = false;
                StockPanel.Visible = true;
            }
        }

        protected void Confirm_Transaction(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            string orderNumber = null;
            decimal amount = 0;

            if (rblTransType.SelectedValue == "buy")
            {
                if (ddlSecurityType.SelectedValue == "bond")
                {
                    //if (!sufficientBalance(BondAmount.Text)) { return; }

                    orderNumber = myExternalFunctions.submitBondBuyOrder(BondCode.Text, BondAmount.Text);
                    amount = Convert.ToDecimal(BondAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "unit trust")
                {
                    //if (!sufficientBalance(UnitAmount.Text)) { return; }

                    orderNumber = myExternalFunctions.submitUnitTrustBuyOrder(UnitCode.Text, UnitAmount.Text);
                    amount = Convert.ToDecimal(UnitAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "stock")
                {
                    //if (!sufficientBalance(StockShares.Text, "stock", StockCode.Text)) { return; }

                    orderNumber = myExternalFunctions.submitStockBuyOrder(StockCode.Text, StockShares.Text, rblOrderType.SelectedValue, expDate.Text, rblIsAll.SelectedValue, hPrice.Text, lPrice.Text);
                }
            }

            //
            //this part is incomplete!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //
            else if (rblTransType.SelectedValue == "sell")
            {
                if (ddlSecurityType.SelectedValue == "bond")
                {
                    orderNumber = myExternalFunctions.submitBondSellOrder(BondCode.Text, BondAmount.Text);
                    amount = Convert.ToDecimal(BondAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "unit trust")
                {
                    orderNumber = myExternalFunctions.submitUnitTrustSellOrder(UnitCode.Text, UnitAmount.Text);
                    amount = Convert.ToDecimal(UnitAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "stock")
                {
                    orderNumber = myExternalFunctions.submitStockSellOrder(StockCode.Text, StockShares.Text, rblOrderType.SelectedValue, expDate.Text, rblIsAll.SelectedValue, lPrice.Text, hPrice.Text);
                    //amountPaid = Convert.ToDecimal(StockShares.Text);
                }

                //should not be written in this way to calculate amount of money used
                balance += amount;
            }

            if (orderNumber == null)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Error, Order is not submitted";
                return;
            }
            lblStatus.Visible = true;
            lblStatus.Text = String.Format("Order submitted successfully, your order number is {0}", orderNumber);

            //TODO: change variables dynamically
            object[] para = { orderNumber, accountNumber, "bond", BondCode.Text, rblTransType.SelectedValue, "pending"};
            string sql = String.Format("INSERT INTO [Order] VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", para);
            //TODO: need to insert to bondbuy or stock table also
            SqlTransaction trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, trans);

            //should not be written in this way to update the ac balance
            if (ddlSecurityType.SelectedValue == "bond" || ddlSecurityType.SelectedValue == "unit trust")
            {
                sql = String.Format("UPDATE [LoginAccount] SET [balance] = {0} WHERE [accountNumber] = '{1}'", balance, accountNumber);
                myHKeInvestData.setData(sql, trans);
                lblAccountBalance.Text = "Account balance: " + balance;
            }

            myHKeInvestData.commitTransaction(trans);
        }

        //UI dynamic change
        protected void rblTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblTransType.SelectedValue == "sell")
            {
                lblBondAmount.Text = "Amount of shares to sell";
                lblUnitAmount.Text = "Amount of shares to sell";
                lblStockShares.Text = "Quantity of shares to sell";
            }
            else if (rblTransType.SelectedValue == "buy")
            {
                lblBondAmount.Text = "Amount in HKD to buy";
                lblUnitAmount.Text = "Amount in HKD to buy";
                lblStockShares.Text = "Quantity of shares to buy";
            }
        }

        protected void cvShares_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rblTransType.SelectedValue == "buy")
            {
                int remainder = Convert.ToInt32(StockShares.Text) % 100;
                if (remainder != 0)
                {
                    cvShares.ErrorMessage = "Quantity of shares to buy must be a multiple of 100.";
                    args.IsValid = false;
                    return;
                }

                decimal sharesValue = myExternalFunctions.getSecuritiesPrice("stock", StockCode.Text) * Convert.ToDecimal(StockShares.Text);

                if (Convert.ToDecimal(args.Value) > balance)
                {
                    cvShares.ErrorMessage = "Account balance is insufficient to place the order.";
                    args.IsValid = false;
                    return;
                }
            }
        }

        protected void cvhPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rblTransType.SelectedValue == "buy" && (rblOrderType.SelectedValue == "limit" || rblOrderType.SelectedValue == "stop limit"))
            {
                if (hPrice.Text != "") { return; }
                args.IsValid = false;
                cvhPrice.ErrorMessage = "Highest buying price is needed.";
                return;
            }
            else if (rblTransType.SelectedValue == "sell" && (rblOrderType.SelectedValue == "stop" || rblOrderType.SelectedValue == "stop limit"))
            {
                if (hPrice.Text != "") { return; }
                args.IsValid = false;
                cvhPrice.ErrorMessage = "Highest selling price is needed.";
                return;
            }
        }

        protected void cvlPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rblTransType.SelectedValue == "buy" && (rblOrderType.SelectedValue == "stop" || rblOrderType.SelectedValue == "stop limit"))
            {
                if (lPrice.Text != "") { return; }
                args.IsValid = false;
                cvlPrice.ErrorMessage = "Lowest buying price is needed.";
                return;
            }
            else if (rblTransType.SelectedValue == "sell" && (rblOrderType.SelectedValue == "limit" || rblOrderType.SelectedValue == "stop limit"))
            {
                if (lPrice.Text != "") { return; }
                args.IsValid = false;
                cvlPrice.ErrorMessage = "Lowest selling price is needed.";
                return;
            }
        }

        protected void cvAmount_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rblTransType.SelectedValue == "buy")
            {
                if (Convert.ToDecimal(args.Value) > balance)
                {
                    args.IsValid = false;
                    return;
                }
            }

        }

        //helper function to convert currency to target base
        private string HKDToBase(string type, string code, string amountHKD)
        {
            DataTable securityData = myExternalFunctions.getSecuritiesByCode(type, code);

            string baseCurrency = "";
            if(securityData == null) { return "-1"; }
            foreach (DataRow row in securityData.Rows)
            {
                baseCurrency = (string)row["base"];
            }

            string toRate = myHKeInvestCode.findCurrencyRate((string[,])Session["CurrencyData"], baseCurrency);
            return Convert.ToString(myHKeInvestCode.convertCurrency("HKD", "1", baseCurrency, toRate, Convert.ToDecimal(amountHKD)));
        }

        // check whether balance in account is sufficient to place order
        // dun need to use
        private bool sufficientBalance(string amount, string type = "", string code = "")
        {
            if (type == "")
            {
                if(Convert.ToDecimal(amount) > balance)
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Insufficient balance in account to place order.";
                    lblStatus.CssClass = "text-danger";
                    return false;
                }
                return true;
            }

            decimal sharesValue = myExternalFunctions.getSecuritiesPrice(type, code) * Convert.ToDecimal(amount);
            if (sharesValue > balance)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Insufficient balance in account to place order.";
                lblStatus.CssClass = "text-danger";
                return false;
            }
            return true;
        }
    }
}