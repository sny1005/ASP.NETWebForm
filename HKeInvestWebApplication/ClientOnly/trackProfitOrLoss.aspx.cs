using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class trackProfitOrLoss : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        static string accountNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
            accountNumber = myHKeInvestCode.getAccountNumber(User.Identity.Name);
        }

        protected void rbDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbDisplayType.SelectedIndex == 2) //individual security
            {
                ddlSecurityType.Visible = true;
                lblSecurityCode.Visible = true;
                txtSecurityCode.Visible = true;
                
            }

            else if(rbDisplayType.SelectedIndex == 1) //one type of security
            {
                lblSecurityCode.Visible = false;
                txtSecurityCode.Visible = false;
                ddlSecurityType.Visible = true;
                
            }
            else    //all
            {
                lblSecurityCode.Visible = false;
                txtSecurityCode.Visible = false;
                ddlSecurityType.Visible = false;

                //get account number
                string userName = User.Identity.Name;
                string userAccountNumber = myHKeInvestCode.getAccountNumber(userName);
                
                //get buy amount
                string sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE buyOrSell = 'buy' AND status = 'completed' AND accountNumber = '"+userAccountNumber+"')";
                //SELECT "executePrice", "executeShares" FROM "Transaction" WHERE "orderNumber" = (SELECT "orderNumber" FROM 'Order' WHERE buyOrSell = 'buy' AND "status" = 'executed' AND "accountNumber" = 'PO00000001')
                DataTable dtBuy = myHKeInvestData.getData(sql);
                decimal totalBuyAmount = 0;
                if(dtBuy.Rows.Count != 0)
                { 
                    foreach (DataRow row in dtBuy.Rows)
                    {
                        // need to convert back to HKD!!!!!!!!!!







                        totalBuyAmount = totalBuyAmount + (Convert.ToDecimal(row["executePrice"])*Convert.ToDecimal(row["executeShares"]));

                    }
                }

                //get sell amount
                sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE buyOrSell = 'sell' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
                DataTable dtSell = myHKeInvestData.getData(sql);
                decimal totalSellAmount = 0;
                if(dtSell.Rows.Count != 0)
                { 
                    foreach (DataRow row in dtSell.Rows)
                    {
                        // need to convert back to HKD!!!!!!!!!!








                        totalSellAmount = totalSellAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
                    }
                }

                //fee
                sql = "SELECT feeCharged FROM [Order] WHERE accountNumber = '" + userAccountNumber + "'";
                decimal totalFeeCharged = 0;
                DataTable dtFee = myHKeInvestData.getData(sql);
                if(dtFee.Rows.Count != 0) { 
                foreach (DataRow row in dtFee.Rows)
                    totalFeeCharged = totalFeeCharged + Convert.ToDecimal(row["feeCharged"]);
                }

                //current value
                sql = "SELECT type, code, shares FROM [SecurityHolding] WHERE accountNumber = '" + userAccountNumber + "'";
                DataTable dtValue = myHKeInvestData.getData(sql);
                decimal currentValue = 0;
                if (dtValue.Rows.Count != 0) { 
                foreach(DataRow row in dtValue.Rows)  //NEED TO HANDLE NULL VALUES
                    currentValue = currentValue + Convert.ToDecimal(row["shares"]) * myExternalFunctions.getSecuritiesPrice(row["type"].ToString(), row["code"].ToString());
                }

                decimal profitOrLoss = currentValue + totalSellAmount - totalBuyAmount - totalFeeCharged; ;

                //view result
                DataTable source = new DataTable();
                source.Columns.Add("buyAmount");
                source.Columns.Add("sellAmount");
                source.Columns.Add("fee");
                source.Columns.Add("profit");

                DataRow record = source.NewRow();
                record["buyAmount"] = totalBuyAmount;
                record["sellAmount"] = totalSellAmount;
                record["fee"] = totalFeeCharged;
                record["profit"] = profitOrLoss;

                source.Rows.Add(record);
                source.AcceptChanges();

                gvSecurity.DataSource = source;
                gvSecurity.DataBind();
                gvSecurity.Visible = true;
            }

        }

        protected void ddlSecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rbDisplayType.SelectedIndex.Equals(1)) //one type of security
            {
                //get account number
                string userName = User.Identity.Name;
                string userAccountNumber = myHKeInvestCode.getAccountNumber(userName);

                //get chosen type
                string type = ddlSecurityType.SelectedItem.ToString();

                //get buy amount of one type
                string sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE securityType = '"+type+"' AND buyOrSell = 'buy' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
                //SELECT "executePrice", "executeShares" FROM "Transaction" WHERE "orderNumber" = (SELECT "orderNumber" FROM 'Order' WHERE buyOrSell = 'buy' AND "status" = 'executed' AND "accountNumber" = 'PO00000001')
                DataTable dtBuy = myHKeInvestData.getData(sql);
                decimal totalBuyAmount = 0;
                if (dtBuy.Rows.Count != 0)
                {
                    foreach (DataRow row in dtBuy.Rows)
                        totalBuyAmount = totalBuyAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
                }

                //get sell amount of one type
                sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE securityType = '"+type+"' AND buyOrSell = 'sell' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
                DataTable dtSell = myHKeInvestData.getData(sql);
                decimal totalSellAmount = 0;
                if (dtSell.Rows.Count != 0)
                {
                    foreach (DataRow row in dtSell.Rows)
                        totalSellAmount = totalSellAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
                }

                //fee of one type
                sql = "SELECT feeCharged FROM [Order] WHERE securityType = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
                decimal totalFeeCharged = 0;
                DataTable dtFee = myHKeInvestData.getData(sql);
                if (dtFee.Rows.Count != 0)
                {
                    foreach (DataRow row in dtFee.Rows)
                        totalFeeCharged = totalFeeCharged + Convert.ToDecimal(row["feeCharged"]);
                }

                //current value of one type
                sql = "SELECT type, code, shares FROM [SecurityHolding] WHERE type = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
                DataTable dtValue = myHKeInvestData.getData(sql);
                decimal currentValue = 0;
                if (dtValue.Rows.Count != 0)
                {
                    foreach (DataRow row in dtValue.Rows)
                        currentValue = currentValue + Convert.ToDecimal(row["shares"]) * myExternalFunctions.getSecuritiesPrice(row["type"].ToString(), row["code"].ToString());
                }

                decimal profitOrLoss = currentValue + totalSellAmount - totalBuyAmount - totalFeeCharged; ;

                //view result
                DataTable source = new DataTable();
                source.Columns.Add(); source.Columns.Add(); source.Columns.Add(); source.Columns.Add(); source.Columns.Add();
                //source.Rows.Add(totalBuyAmount, totalSellAmount, totalFeeCharged, profitOrLoss);
                DataRow newRow = source.NewRow();
                newRow[0] = totalBuyAmount;
                newRow[1] = totalSellAmount;
                newRow[2] = totalFeeCharged;
                newRow[3] = profitOrLoss;

                gvSecurity.DataSource = source;
                gvSecurity.Visible = true;
            }
        }

        protected void txtSecurityCode_TextChanged(object sender, EventArgs e)
        {
            //get account number
            string userName = User.Identity.Name;
            string userAccountNumber = myHKeInvestCode.getAccountNumber(userName);

            //get chosen type and code
            string type = ddlSecurityType.SelectedItem.ToString();
            string code = txtSecurityCode.Text.Trim();

            //get buy amount of one security
            string sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE secuirtyCode = '"+code+"' AND securityType = '" + type + "' AND buyOrSell = 'buy' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
            //SELECT "executePrice", "executeShares" FROM "Transaction" WHERE "orderNumber" = (SELECT "orderNumber" FROM 'Order' WHERE buyOrSell = 'buy' AND "status" = 'executed' AND "accountNumber" = 'PO00000001')
            DataTable dtBuy = myHKeInvestData.getData(sql);
            decimal totalBuyAmount = 0;
            if (dtBuy.Rows.Count != 0)
            {
                foreach (DataRow row in dtBuy.Rows)
                    totalBuyAmount = totalBuyAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
            }

            //get sell amount of one security
            sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE secuirtyCode = '" + code + "' AND securityType = '" + type + "' AND buyOrSell = 'sell' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
            DataTable dtSell = myHKeInvestData.getData(sql);
            decimal totalSellAmount = 0;
            if (dtSell.Rows.Count != 0)
            {
                foreach (DataRow row in dtSell.Rows)
                    totalSellAmount = totalSellAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
            }

            //fee of one security
            sql = "SELECT feeCharged FROM [Order] WHERE secuirtyCode = '" + code + "' AND securityType = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
            decimal totalFeeCharged = 0;
            DataTable dtFee = myHKeInvestData.getData(sql);
            if (dtFee.Rows.Count != 0)
            {
                foreach (DataRow row in dtFee.Rows)
                    totalFeeCharged = totalFeeCharged + Convert.ToDecimal(row["feeCharged"]);
            }

            //current value of one security
            sql = "SELECT type, code, shares, name FROM [SecurityHolding] WHERE code = '" + code + "' AND type = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
            DataTable dtValue = myHKeInvestData.getData(sql);
            decimal shares = 0;
            decimal currentValue = 0;
            string name = null;
            if (dtValue.Rows.Count != 0)
            {
                foreach (DataRow row in dtValue.Rows)
                {
                    currentValue = currentValue + Convert.ToDecimal(row["shares"]) * myExternalFunctions.getSecuritiesPrice(row["type"].ToString(), row["code"].ToString());
                    shares = shares + Convert.ToDecimal(row["shares"]);
                    name = row["name"].ToString();
                }
            }
            decimal profitOrLoss = currentValue + totalSellAmount - totalBuyAmount - totalFeeCharged; ;

            //view result
            DataTable source = new DataTable();
            int i = 0;
            while (i < 8)
            {
                source.Columns.Add();
                i++;
            }
            //source.Rows.Add(totalBuyAmount, totalSellAmount, totalFeeCharged, profitOrLoss);
            DataRow newRow = source.NewRow();
            newRow[0] = type;
            newRow[1] = code;
            newRow[2] = name;
            newRow[3] = shares;
            newRow[4] = totalBuyAmount;
            newRow[5] = totalSellAmount;
            newRow[6] = totalFeeCharged;
            newRow[7] = profitOrLoss;

            gvIndividual.DataSource = source;
            gvIndividual.Visible = true;
        }

        protected void gvIndividual_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        
    }
}