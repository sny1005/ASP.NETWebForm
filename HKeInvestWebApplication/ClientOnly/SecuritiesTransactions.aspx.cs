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

                //update account balance
                balance = myHKeInvestCode.getAccountBalance(accountNumber);
                lblAccountBalance.Text = "Account balance: " + Convert.ToString(balance);
            }


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
            string amount = "";

            // { orderNumber, accountNumber, type, code, buyOrSell, "pending" };
            object[] orderPara = new object[7];
            orderPara[1] = accountNumber;
            orderPara[5] = "pending";

            if (rblTransType.SelectedValue == "buy")
            {
                orderPara[4] = "buy";

                if (ddlSecurityType.SelectedValue == "bond")
                {
                    orderPara[2] = "bond";
                    orderPara[3] = BondCode.Text;
                    amount = (string)BondAmount.Text;
                    //if (!sufficientBalance(BondAmount.Text)) { return; }

                    orderNumber = myExternalFunctions.submitBondBuyOrder(BondCode.Text, BondAmount.Text);

                }
                else if (ddlSecurityType.SelectedValue == "unit trust")
                {
                    orderPara[2] = "unit trust";
                    orderPara[3] = UnitCode.Text;
                    amount = (string)UnitAmount.Text;
                    //if (!sufficientBalance(UnitAmount.Text)) { return; }

                    orderNumber = myExternalFunctions.submitUnitTrustBuyOrder(UnitCode.Text, UnitAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "stock")
                {
                    orderPara[2] = "stock";
                    orderPara[3] = StockCode.Text;
                    amount = (string)StockShares.Text;
                    //if (!sufficientBalance(StockShares.Text, "stock", StockCode.Text)) { return; }

                    orderNumber = myExternalFunctions.submitStockBuyOrder(StockCode.Text, StockShares.Text, rblOrderType.SelectedValue, expDate.Text, rblIsAll.SelectedValue, hPrice.Text, lPrice.Text);
                }
            }

            else if (rblTransType.SelectedValue == "sell")
            {
                orderPara[4] = "sell";

                if (ddlSecurityType.SelectedValue == "bond")
                {
                    orderPara[2] = "bond";
                    orderPara[3] = BondCode.Text;
                    amount = (string)BondAmount.Text;

                    orderNumber = myExternalFunctions.submitBondSellOrder(BondCode.Text, BondAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "unit trust")
                {
                    orderPara[2] = "unit trust";
                    orderPara[3] = UnitCode.Text;
                    amount = (string)UnitAmount.Text;

                    orderNumber = myExternalFunctions.submitUnitTrustSellOrder(UnitCode.Text, UnitAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "stock")
                {
                    orderPara[2] = "stock";
                    orderPara[3] = StockCode.Text;

                    orderNumber = myExternalFunctions.submitStockSellOrder(StockCode.Text, StockShares.Text, rblOrderType.SelectedValue, expDate.Text, rblIsAll.SelectedValue, lPrice.Text, hPrice.Text);
                }
            }

            //get datetime
            orderPara[6] = DateTime.Now.ToString("MM/dd/yyyy");

            // check if order is successfully submitted to External System, and display result
            if (orderNumber == null)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Error, Order is not submitted";
                return;
            }
            lblStatus.Visible = true;
            lblStatus.Text = String.Format("Order submitted successfully, your order number is {0}", orderNumber);

            // set up sql to create a copy of Order from External System
            orderPara[0] = orderNumber;
            string sql = string.Format("INSERT INTO [Order] VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', NULL)", orderPara);

            SqlTransaction trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, trans);    //insert into order

            if ((string)orderPara[2] == "stock")
            {
                // stock operation
                object[] stockPara = { orderNumber, StockShares.Text, rblOrderType.SelectedValue, expDate.Text, rblIsAll.SelectedIndex, "NULL", "NULL" };
                sql = string.Format("INSERT INTO [StockOrder] VALUES ({0}, {1}, '{2}', {3}, {4}, ", stockPara);

                if ((string)orderPara[4] == "buy")      //buy stock
                {
                    // Check for order type and set SQL statement accordingly
                    switch (rblOrderType.SelectedValue)
                    {
                        case "market":
                            sql = sql + "NULL, NULL)";
                            break;
                        case "limit":
                            sql = sql + hPrice.Text + ", NULL)";
                            break;
                        case "stop":
                            sql = sql + "NULL, " + lPrice.Text + ") ";
                            break;
                        default:
                            sql = sql + hPrice.Text + ", " + lPrice.Text + ")";
                            break;
                    }
                }
                else             //sell stock
                {
                    switch (rblOrderType.SelectedValue)
                    {
                        case "market":
                            sql = sql + "NULL, NULL)";
                            break;
                        case "limit":
                            sql = sql + lPrice.Text + ", NULL)";
                            break;
                        case "stop":
                            sql = sql + "NULL, " + hPrice.Text + ") ";
                            break;
                        default:
                            sql = sql + lPrice.Text + ", " + hPrice.Text + ")";
                            break;
                    }
                }
                myHKeInvestData.setData(sql, trans);
            }
            else
            {
                // bond/unit trust operation
                if (rblTransType.SelectedValue == "buy")
                    sql = string.Format("INSERT INTO [BuyBondOrder] VALUES ('{0}', '{1}')", orderNumber, amount);
                else
                    sql = string.Format("INSERT INTO [SellBondOrder] VALUES ('{0}', '{1}')", orderNumber, amount);

                myHKeInvestData.setData(sql, trans);
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
            decimal value = Convert.ToDecimal(args.Value);
            if (value <= 0)
            {
                cvShares.ErrorMessage = "Quantity of shares must greater than 0";
                args.IsValid = false;
                return;
            }

            // check buy conditions
            if (rblTransType.SelectedValue == "buy")
            {
                int intValue;
                if (int.TryParse(args.Value, out intValue))
                {
                    int remainder = intValue % 100;
                    if (remainder != 0)
                    {
                        cvShares.ErrorMessage = "Quantity of shares to buy must be a multiple of 100.";
                        args.IsValid = false;
                        return;
                    }

                    decimal sharesValue = myExternalFunctions.getSecuritiesPrice("stock", StockCode.Text) * Convert.ToDecimal(args.Value);
                    if (sharesValue > balance)
                    {
                        cvShares.ErrorMessage = "Account balance is insufficient to place the order.";
                        args.IsValid = false;
                        return;
                    }
                    return;
                }

                cvShares.ErrorMessage = "Quantity of shares to buy must be an integer.";
                args.IsValid = false;
                return;
            }
            // check sell conditions
            else
            {
                string code = StockCode.Text;

                string sql = "SELECT [shares] FROM [SecurityHolding] WHERE [accountNumber] = '" + accountNumber + "' AND [type] = 'stock' AND [code] = '" + code + "'";
                DataTable Table = myHKeInvestData.getData(sql);
                DataRow[] record = Table.Select();
                if (record.Count() != 1)
                    throw new Exception("Error! Returning more than 1 record or stock is not owned by account");
                else
                {
                    decimal ownedShares = Convert.ToDecimal(record[0]["shares"]);

                    sql = "SELECT [shares] from [Order] JOIN [StockOrder] on [Order].[orderNumber] = [StockOrder].[orderNumber] WHERE [securityType] = 'stock' AND [buyOrSell] = 'sell' AND [securityCode] = '" + code + "'";
                    Table = myHKeInvestData.getData(sql);

                    decimal pendingShares = 0;
                    foreach (DataRow row in Table.Rows)
                    {
                        pendingShares += Convert.ToDecimal(row["shares"]);
                    }

                    if ((pendingShares + Convert.ToDecimal(args.Value)) <= ownedShares)
                        return;

                    cvShares.ErrorMessage = "You cannot have simultaneous sell orders for the same security whoose quantity exceeds the amount pf shreas that you own.";
                    args.IsValid = false;
                    return;
                }
            }
        }

        protected void cvhPrice_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rblTransType.SelectedValue == "buy" && (rblOrderType.SelectedValue == "limit" || rblOrderType.SelectedValue == "stop limit"))
            {
                if (hPrice.Text == "")
                {
                    args.IsValid = false;
                    cvhPrice.ErrorMessage = "Highest buying price is needed.";
                    return;
                }
            }
            else if (rblTransType.SelectedValue == "sell" && (rblOrderType.SelectedValue == "stop" || rblOrderType.SelectedValue == "stop limit"))
            {
                if (hPrice.Text != "")
                {
                    args.IsValid = false;
                    cvhPrice.ErrorMessage = "Highest selling price is needed.";
                    return;
                }
                
            }

            // check that high price must be larger than low price when the order type is stop limit
            if (rblOrderType.SelectedValue == "stop limit")
            {
                if (Convert.ToDecimal(hPrice.Text) <= Convert.ToDecimal(lPrice.Text))
                {
                    args.IsValid = false;
                    cvhPrice.ErrorMessage = "High price must be larger than low price.";
                    return;
                }
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
            decimal value = Convert.ToDecimal(args.Value);
            if (value <= 0)
            {
                if (source.Equals(cvBondAmount))
                    cvBondAmount.ErrorMessage = "Amount in HKD must greater than 0";
                else if (source.Equals(cvUnitAmount))
                    cvUnitAmount.ErrorMessage = "Amount in HKD must greater than 0";

                args.IsValid = false;
                return;
            }

            if (rblTransType.SelectedValue == "buy")
            {
                if (value > balance)
                {
                    if (source.Equals(cvBondAmount))
                        cvBondAmount.ErrorMessage = "Account balance is insufficient to place the order.";
                    else if (source.Equals(cvUnitAmount))
                        cvUnitAmount.ErrorMessage = "Account balance is insufficient to place the order.";

                    args.IsValid = false;
                    return;
                }
                return;
            }
            // sell conditions
            else
            {
                string code;
                string type = ddlSecurityType.SelectedValue;
                if (type == "bond")
                    code = BondCode.Text;
                else
                    code = UnitCode.Text;

                string sql = "SELECT [shares] FROM [SecurityHolding] WHERE [accountNumber] = '" + accountNumber + "' AND [type] = '" + type + "' AND [code] = '" + code + "'";
                DataTable Table = myHKeInvestData.getData(sql);
                DataRow[] record = Table.Select();
                if (record.Count() != 1)
                    throw new Exception("Error! Returning more than 1 record or stock is not owned by account");
                else
                {
                    decimal ownedShares = Convert.ToDecimal(record[0]["shares"]);

                    sql = "SELECT [shares] from [Order] JOIN [SellBondOrder] on [Order].[orderNumber] = [SellBondOrder].[orderNumber] WHERE [securityType] = '" + type + "' AND [buyOrSell] = 'sell' AND [securityCode] = '" + code + "'";
                    Table = myHKeInvestData.getData(sql);

                    decimal pendingShares = 0;
                    foreach (DataRow row in Table.Rows)
                    {
                        pendingShares += Convert.ToDecimal(row["shares"]);
                    }

                    if ((pendingShares + Convert.ToDecimal(args.Value)) <= ownedShares)
                        return;

                    if (type == "bond")
                        cvBondAmount.ErrorMessage = "You cannot have simultaneous sell orders for the same security whoose quantity exceeds the amount pf shreas that you own.";
                    else
                        cvUnitAmount.ErrorMessage = "You cannot have simultaneous sell orders for the same security whoose quantity exceeds the amount pf shreas that you own.";
                    args.IsValid = false;
                    return;
                }
            }
        }

        protected void cvCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (source.Equals(cvBondCode))
            {
                if (myHKeInvestCode.securityCodeIsValid("bond", args.Value))
                    return;
                cvBondCode.ErrorMessage = "Bond code invalid";
                args.IsValid = false;
                return;
            }
            else if (source.Equals(cvUnitCode))
            {
                if (myHKeInvestCode.securityCodeIsValid("unit trust", args.Value))
                    return;
                cvUnitCode.ErrorMessage = "Unit trust code invalid";
                args.IsValid = false;
                return;
            }
            else    //cvStockCode
            {
                if (myHKeInvestCode.securityCodeIsValid("stock", args.Value))
                    return;
                cvStockCode.ErrorMessage = "Stock code invalid";
                args.IsValid = false;
                return;
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
        // legacy function
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