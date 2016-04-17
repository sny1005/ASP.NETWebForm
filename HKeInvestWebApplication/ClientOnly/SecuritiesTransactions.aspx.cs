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
                //get the account number of the current logged in user
                string username = User.Identity.Name;
                string sql = "select [Client].[accountNumber], [accountBalance] from [Account] FULL OUTER JOIN [Client] ON [Account].[accountNumber]=[Client].[accountNumber] where username ='" + username + "'";    //need to modify in order to get account balance

                DataTable dtclient = myHKeInvestData.getData(sql);
                if (dtclient == null) { return; } // if the dataset is null, a sql error occurred.
                else if (dtclient.Rows.Count > 1)
                {
                    System.Web.HttpContext.Current.Response.Write("Databse error, returning more than one account!");
                    return;
                }

                foreach (DataRow row in dtclient.Rows)
                {
                    accountNumber = (string)row["accountnumber"];
                    balance = Convert.ToDecimal(row["accountBalance"]);
                }
                lblAccountNumber.Text = "Account number: " + accountNumber;
                lblAccountBalance.Text = "Account balance: " + balance;
            }
        }

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

            //HOW TO CHECK ACCOUNT BALANCE?????
            if (rblTransType.SelectedValue == "buy")
            {
                if (ddlSecurityType.SelectedValue == "bond")
                {
                    orderNumber = myExternalFunctions.submitBondBuyOrder(BondCode.Text, BondAmount.Text);
                    amount = Convert.ToDecimal(BondAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "unit trust")
                {
                    orderNumber = myExternalFunctions.submitUnitTrustBuyOrder(UnitCode.Text, UnitAmount.Text);
                    amount = Convert.ToDecimal(UnitAmount.Text);
                }
                else if (ddlSecurityType.SelectedValue == "stock")
                {
                    orderNumber = myExternalFunctions.submitStockBuyOrder(StockCode.Text, StockShares.Text, rblOrderType.SelectedValue, expDate.Text, rblIsAll.SelectedValue, hPrice.Text, lPrice.Text);
                    //amountPaid = Convert.ToDecimal(StockShares.Text);
                }

                //should not be written in this way to calculate amount of money used
                balance -= amount;
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

            string sql = String.Format("INSERT INTO [Order] VALUES ('{0}', '{1}')", orderNumber, accountNumber);
            SqlTransaction trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql, trans);

            //should not be written in this way to update the ac balance
            if (ddlSecurityType.SelectedValue == "bond" || ddlSecurityType.SelectedValue == "unit trust")
            {
                sql = String.Format("UPDATE [Client] SET [accountBalance] = {0} WHERE [accountNumber] = '{1}'", balance, accountNumber);
                myHKeInvestData.setData(sql, trans);
                lblAccountBalance.Text = "Account balance: " + balance;
            }

            myHKeInvestData.commitTransaction(trans);
        }

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
                if (remainder != 0) { args.IsValid = false; }
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
    }
}