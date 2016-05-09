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
                gvSecurity.Visible = false;
                gvIndividual.Visible = false;
            }

            else if(rbDisplayType.SelectedIndex == 1) //one type of security
            {
                lblSecurityCode.Visible = false;
                txtSecurityCode.Visible = false;
                ddlSecurityType.Visible = true;
                gvSecurity.Visible = false;
                gvIndividual.Visible = false;
            }
            else    //all
            {
                lblSecurityCode.Visible = false;
                txtSecurityCode.Visible = false;
                ddlSecurityType.Visible = false;
                gvSecurity.Visible = false;
                gvIndividual.Visible = false;

                //get account number
                string userName = User.Identity.Name;
                string userAccountNumber = myHKeInvestCode.getAccountNumber(userName);

                // TODO: get buy amount (bug on getting buy value of stock)
                string sql = "SELECT orderNumber, securityType, securityCode FROM [Order] WHERE buyOrSell = 'buy' AND (status = 'completed' OR status = 'partial') AND accountNumber = '" + userAccountNumber + "'";
                DataTable dtOrder = myHKeInvestData.getData(sql);
                decimal totalBuyAmount = 0;
                if (dtOrder.Rows.Count != 0)
                {
                    foreach (DataRow row in dtOrder.Rows)
                    {
                        string orderNumber = row["orderNumber"].ToString().Trim();
                        string type = row["securityType"].ToString().Trim();
                        string code = row["securityCode"].ToString().Trim();

                        string find = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = '" + orderNumber + "'";
                        DataTable dtBuy = myHKeInvestData.getData(find);
                        foreach (DataRow Row in dtBuy.Rows)
                        {
                            //convert to HKD
                            string name, currency;
                            myHKeInvestCode.getSecurityNameBase(type, code, out name, out currency);
                            decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                            decimal executePriceHKD = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", Convert.ToDecimal(Row["executePrice"]));
                           
                            totalBuyAmount = totalBuyAmount + (executePriceHKD * Convert.ToDecimal(Row["executeShares"]));
                        }
                    }
                   
                }

                //get sell amount
                //sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE buyOrSell = 'sell' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
                sql = "SELECT orderNumber, securityType, securityCode  FROM [Order] WHERE buyOrSell = 'sell' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "'";
                DataTable dtSellOrder = myHKeInvestData.getData(sql);
                decimal totalSellAmount = 0;
                if (dtSellOrder.Rows.Count != 0)
                {
                    foreach(DataRow row in dtSellOrder.Rows)
                    {
                        string orderNumber = row["orderNumber"].ToString().Trim();
                        string type = row["securityType"].ToString().Trim();
                        string code = row["securityCode"].ToString().Trim();

                        string find = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = '" + orderNumber + "'";
                        DataTable dtSell = myHKeInvestData.getData(find);
       
                            foreach (DataRow Row in dtSell.Rows)
                            {
                            
                            // need to convert back to HKD!!!!!!!!!!
                            string sql2 = "SELECT base FROM [SecurityHolding] WHERE type = '" + type + "' AND code = '" + code + "'AND accountNumber = '" + userAccountNumber + "' ";
                            DataTable dtBase = myHKeInvestData.getData(sql2);
                            string currency = dtBase.Rows[0]["base"].ToString().Trim();
                            decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                            decimal executePriceHKD = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", Convert.ToDecimal(Row["executePrice"]));

                            totalSellAmount = totalSellAmount + (executePriceHKD * Convert.ToDecimal(Row["executeShares"]));
                            }
                    }
                }
                //fee
                sql = "SELECT feeCharged FROM [Order] WHERE accountNumber = '" + userAccountNumber + "'";
                decimal totalFeeCharged = 0;
                DataTable dtFee = myHKeInvestData.getData(sql);
                if(dtFee.Rows.Count != 0) { 
                    foreach (DataRow row in dtFee.Rows)
                    {
                        // remember to handle null values
                        decimal fee;
                        if (!decimal.TryParse(row["feeCharged"].ToString().Trim(), out fee))
                            continue;
                        totalFeeCharged += fee;
                    }
                }

                //current value
                sql = "SELECT type, code, shares FROM [SecurityHolding] WHERE accountNumber = '" + userAccountNumber + "'";
                DataTable dtValue = myHKeInvestData.getData(sql);
                decimal currentValue = 0;
                if (dtValue.Rows.Count != 0) { 
                    foreach(DataRow row in dtValue.Rows)
                    {
                        string type = row["type"].ToString().Trim();
                        string code = row["code"].ToString().Trim();
                        string name, currency;  //name is dummy variable

                        //convert to HKD
                        myHKeInvestCode.getSecurityNameBase(type, code, out name, out currency);
                        decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                        decimal currentPrice = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", myExternalFunctions.getSecuritiesPrice(type, code));

                        currentValue = currentValue + Convert.ToDecimal(row["shares"]) * currentPrice;
                    }
                }

                decimal profitOrLoss = currentValue + totalSellAmount - totalBuyAmount - totalFeeCharged; ;

                //view result
                DataTable source = new DataTable();
                source.Columns.Add("buyAmount");
                source.Columns.Add("sellAmount");
                source.Columns.Add("fee");
                source.Columns.Add("profit");

                if(viewIn.SelectedIndex==0) //dollar
                {
                    DataRow record = source.NewRow();
                    record["buyAmount"] = totalBuyAmount;
                    record["sellAmount"] = totalSellAmount;
                    record["fee"] = totalFeeCharged;

                    //convert to abs amount
                    if (profitOrLoss < 0)
                    {
                        profitOrLoss = Math.Abs(profitOrLoss);
                        record["profit"] = "(" + profitOrLoss + ")";
                    }
                    else
                        record["profit"] = profitOrLoss;

                    source.Rows.Add(record);
                    source.AcceptChanges();

                    gvSecurity.DataSource = source;
                    gvSecurity.DataBind();
                    gvSecurity.Visible = true;
                }
                else if(viewIn.SelectedIndex==1) //percentage
                {
                    DataRow record = source.NewRow();
                    record["buyAmount"] = totalBuyAmount;
                    record["sellAmount"] = totalSellAmount;
                    record["fee"] = totalFeeCharged;

                    if (profitOrLoss == 0)
                        record["profit"] = 0 + "%";
                    else if (profitOrLoss < 0)
                    {
                        profitOrLoss = Math.Abs(profitOrLoss);
                        record["profit"] = "(" + (profitOrLoss / (totalBuyAmount + totalFeeCharged) * 100) + ") %";
                    }
                    else
                        record["profit"] = (profitOrLoss/(totalBuyAmount+totalFeeCharged)*100)+"%";

                    source.Rows.Add(record);
                    source.AcceptChanges();

                    gvSecurity.DataSource = source;
                    gvSecurity.DataBind();
                    gvSecurity.Visible = true;

                }
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
                string type = ddlSecurityType.SelectedValue;

                //get buy amount of one type
                /*string sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE securityType = '"+type+"' AND buyOrSell = 'buy' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
                //SELECT "executePrice", "executeShares" FROM "Transaction" WHERE "orderNumber" = (SELECT "orderNumber" FROM 'Order' WHERE buyOrSell = 'buy' AND "status" = 'executed' AND "accountNumber" = 'PO00000001')
                DataTable dtBuy = myHKeInvestData.getData(sql);
                decimal totalBuyAmount = 0;
                if (dtBuy.Rows.Count != 0)
                {
                    foreach (DataRow row in dtBuy.Rows)
                        totalBuyAmount = totalBuyAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
                }*/
                string sql = "SELECT orderNumber, securityType, securityCode  FROM [Order] WHERE securityType = '" + type+"' AND buyOrSell = 'buy' AND (status = 'completed' OR status = 'partial') AND accountNumber = '" + userAccountNumber + "'";
                DataTable dtOrder = myHKeInvestData.getData(sql);
                decimal totalBuyAmount = 0;
                if (dtOrder.Rows.Count != 0)
                {
                    foreach (DataRow row in dtOrder.Rows)
                    {
                        string orderNumber = row["orderNumber"].ToString().Trim();
                        string type1 = row["securityType"].ToString().Trim();
                        string code = row["securityCode"].ToString().Trim();

                        string find = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = '" + orderNumber + "'";
                        DataTable dtBuy = myHKeInvestData.getData(find);
                        foreach (DataRow Row in dtBuy.Rows)
                        {
                            // need to convert back to HKD!!!!!!!!!!
                            string sql2 = "SELECT base FROM [SecurityHolding] WHERE type = '" + type1 + "' AND code = '" + code + "'AND accountNumber = '" + userAccountNumber + "' ";
                            DataTable dtBase = myHKeInvestData.getData(sql2);
                            string currency = dtBase.Rows[0]["base"].ToString().Trim();
                            decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                            decimal executePriceHKD = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", Convert.ToDecimal(Row["executePrice"]));

                            totalBuyAmount = totalBuyAmount + (executePriceHKD * Convert.ToDecimal(Row["executeShares"]));
                        }
                    }

                }

                //get sell amount of one type
                /*sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE securityType = '"+type+"' AND buyOrSell = 'sell' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
                DataTable dtSell = myHKeInvestData.getData(sql);
                decimal totalSellAmount = 0;
                if (dtSell.Rows.Count != 0)
                {
                    foreach (DataRow row in dtSell.Rows)
                        totalSellAmount = totalSellAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
                }*/
                sql = "SELECT orderNumber, securityType, securityCode FROM [Order] WHERE securityType = '" + type + "' AND  buyOrSell = 'sell' AND (status = 'completed' OR status = 'partial') AND accountNumber = '" + userAccountNumber + "'";
                DataTable dtSellOrder = myHKeInvestData.getData(sql);
                decimal totalSellAmount = 0;
                if (dtSellOrder.Rows.Count != 0)
                {
                    foreach (DataRow row in dtSellOrder.Rows)
                    {
                        string orderNumber = row["orderNumber"].ToString().Trim();
                        string type1 = row["securityType"].ToString().Trim();
                        string code = row["securityCode"].ToString().Trim();
                        string find = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = '" + orderNumber + "'";
                        DataTable dtSell = myHKeInvestData.getData(find);

                        foreach (DataRow Row in dtSell.Rows)
                        {
                            // need to convert back to HKD!!!!!!!!!!
                            string sql2 = "SELECT base FROM [SecurityHolding] WHERE type = '" + type1 + "' AND code = '" + code + "'AND accountNumber = '" + userAccountNumber + "' ";
                            DataTable dtBase = myHKeInvestData.getData(sql2);
                            string currency = dtBase.Rows[0]["base"].ToString().Trim();
                            decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                            decimal executePriceHKD = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", Convert.ToDecimal(Row["executePrice"]));

                            totalSellAmount = totalSellAmount + (executePriceHKD * Convert.ToDecimal(Row["executeShares"]));
                        }
                    }
                }


                //fee of one type
                sql = "SELECT feeCharged FROM [Order] WHERE securityType = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
                decimal totalFeeCharged = 0;
                DataTable dtFee = myHKeInvestData.getData(sql);
                if (dtFee.Rows.Count != 0)
                {
                    foreach (DataRow row in dtFee.Rows)
                    {
                        // remember to handle null values
                        decimal fee;
                        if (!decimal.TryParse(row["feeCharged"].ToString().Trim(), out fee))
                            continue;
                        totalFeeCharged += fee;
                    }
                }

                //current value of one type
                sql = "SELECT type, code, shares FROM [SecurityHolding] WHERE type = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
                DataTable dtValue = myHKeInvestData.getData(sql);
                decimal currentValue = 0;
                if (dtValue.Rows.Count != 0)
                {
                    foreach (DataRow row in dtValue.Rows)
                    {
                        string code = row["code"].ToString().Trim();
                        string name, currency;  //name is dummy variable

                        //convert to HKD
                        myHKeInvestCode.getSecurityNameBase(type, code, out name, out currency);
                        decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                        decimal currentPrice = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", myExternalFunctions.getSecuritiesPrice(type, code));

                        currentValue = currentValue + Convert.ToDecimal(row["shares"]) * currentPrice;
                    }
                }

                decimal profitOrLoss = currentValue + totalSellAmount - totalBuyAmount - totalFeeCharged; ;

                //view result
                DataTable source = new DataTable();
                source.Columns.Add("buyAmount");
                source.Columns.Add("sellAmount");
                source.Columns.Add("fee");
                source.Columns.Add("profit");

                if (viewIn.SelectedIndex == 0) //dollar
                {
                    DataRow record = source.NewRow();
                    record["buyAmount"] = totalBuyAmount;
                    record["sellAmount"] = totalSellAmount;
                    record["fee"] = totalFeeCharged;

                    //convert to abs amount
                    if (profitOrLoss < 0)
                    {
                        profitOrLoss = Math.Abs(profitOrLoss);
                        record["profit"] = "(" + profitOrLoss + ")";
                    }
                    else
                        record["profit"] = profitOrLoss;

                    source.Rows.Add(record);
                    source.AcceptChanges();

                    gvSecurity.DataSource = source;
                    gvSecurity.DataBind();
                    gvSecurity.Visible = true;
                }
                else if (viewIn.SelectedIndex == 1) //percentage
                {
                    DataRow record = source.NewRow();
                    record["buyAmount"] = totalBuyAmount;
                    record["sellAmount"] = totalSellAmount;
                    record["fee"] = totalFeeCharged;

                    if (profitOrLoss == 0)
                        record["profit"] = 0 + "%";
                    else if (profitOrLoss < 0)
                    {
                        profitOrLoss = Math.Abs(profitOrLoss);
                        record["profit"] = "(" + (profitOrLoss / (totalBuyAmount + totalFeeCharged) * 100) + ") %";
                    }
                    else
                        record["profit"] = (profitOrLoss / (totalBuyAmount + totalFeeCharged) * 100) + "%";

                    source.Rows.Add(record);
                    source.AcceptChanges();

                    gvSecurity.DataSource = source;
                    gvSecurity.DataBind();
                    gvSecurity.Visible = true;

                }
            }
        }

        protected void txtSecurityCode_TextChanged(object sender, EventArgs e)
        {
            //get account number
            string userName = User.Identity.Name;
            string userAccountNumber = myHKeInvestCode.getAccountNumber(userName);

            //get chosen type and code
            string type = ddlSecurityType.SelectedValue;
            string code = txtSecurityCode.Text.Trim();

            //get buy amount of one security
            /*string sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE securityCode = '"+code+"' AND securityType = '" + type + "' AND buyOrSell = 'buy' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
            //SELECT "executePrice", "executeShares" FROM "Transaction" WHERE "orderNumber" = (SELECT "orderNumber" FROM 'Order' WHERE buyOrSell = 'buy' AND "status" = 'executed' AND "accountNumber" = 'PO00000001')
            DataTable dtBuy = myHKeInvestData.getData(sql);
            decimal totalBuyAmount = 0;
            if (dtBuy.Rows.Count != 0)
            {
                foreach (DataRow row in dtBuy.Rows)
                    totalBuyAmount = totalBuyAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
            }*/
            string sql = "SELECT orderNumber, securityType, securityCode FROM [Order] WHERE securityCode = '" + code + "' AND  securityType = '" + type + "' AND buyOrSell = 'buy' AND (status = 'completed' OR status = 'partial') AND accountNumber = '" + userAccountNumber + "'";
            DataTable dtOrder = myHKeInvestData.getData(sql);
            decimal totalBuyAmount = 0;
            if (dtOrder.Rows.Count != 0)
            {
                foreach (DataRow row in dtOrder.Rows)
                {
                    string orderNumber = row["orderNumber"].ToString().Trim();
                    string type1 = row["securityType"].ToString().Trim();
                    string code1 = row["securityCode"].ToString().Trim();
                    string find = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = '" + orderNumber + "'";
                    DataTable dtBuy = myHKeInvestData.getData(find);
                    foreach (DataRow Row in dtBuy.Rows)
                    {
                        // need to convert back to HKD!!!!!!!!!!
                        string sql2 = "SELECT base FROM [SecurityHolding] WHERE type = '" + type1 + "' AND code = '" + code1 + "'AND accountNumber = '" + userAccountNumber + "' ";
                        DataTable dtBase = myHKeInvestData.getData(sql2);
                        string currency = dtBase.Rows[0]["base"].ToString().Trim();
                        decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                        decimal executePriceHKD = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", Convert.ToDecimal(Row["executePrice"]));

                        totalBuyAmount = totalBuyAmount + (executePriceHKD * Convert.ToDecimal(Row["executeShares"]));
                    }
                }

            }


            //get sell amount of one security
            /*sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = (SELECT orderNumber FROM [Order] WHERE securityCode = '" + code + "' AND securityType = '" + type + "' AND buyOrSell = 'sell' AND status = 'completed' AND accountNumber = '" + userAccountNumber + "')";
            DataTable dtSell = myHKeInvestData.getData(sql);
            decimal totalSellAmount = 0;
            if (dtSell.Rows.Count != 0)
            {
                foreach (DataRow row in dtSell.Rows)
                    totalSellAmount = totalSellAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
            }*/
            sql = "SELECT orderNumber, securityType, securityCode FROM [Order] WHERE securityCode = '" + code + "' AND  securityType = '" + type + "' AND  buyOrSell = 'sell' AND (status = 'completed' OR status = 'partial') AND accountNumber = '" + userAccountNumber + "'";
            DataTable dtSellOrder = myHKeInvestData.getData(sql);
            decimal totalSellAmount = 0;
            if (dtSellOrder.Rows.Count != 0)
            {
                foreach (DataRow row in dtSellOrder.Rows)
                {
                    string orderNumber = row["orderNumber"].ToString().Trim();
                    string type1 = row["securityType"].ToString().Trim();
                    string code1 = row["securityCode"].ToString().Trim();
                    string find = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = '" + orderNumber + "'";
                    DataTable dtSell = myHKeInvestData.getData(find);

                    foreach (DataRow Row in dtSell.Rows)
                    {
                        // need to convert back to HKD!!!!!!!!!!
                        string sql2 = "SELECT base FROM [SecurityHolding] WHERE type = '" + type1 + "' AND code = '" + code1 + "'AND accountNumber = '" + userAccountNumber + "' ";
                        DataTable dtBase = myHKeInvestData.getData(sql2);
                        string currency = dtBase.Rows[0]["base"].ToString().Trim();
                        decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                        decimal executePriceHKD = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", Convert.ToDecimal(Row["executePrice"]));

                        totalSellAmount = totalSellAmount + (executePriceHKD * Convert.ToDecimal(Row["executeShares"]));
                    }
                }
            }

            //fee of one security
            sql = "SELECT feeCharged FROM [Order] WHERE securityCode = '" + code + "' AND securityType = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
            decimal totalFeeCharged = 0;
            DataTable dtFee = myHKeInvestData.getData(sql);
            if (dtFee.Rows.Count != 0)
            {
                foreach (DataRow row in dtFee.Rows)
                {
                    // remember to handle null values
                    decimal fee;
                    if (!decimal.TryParse(row["feeCharged"].ToString().Trim(), out fee))
                        continue;
                    totalFeeCharged += fee;
                }
            }

            //current value of one security
            sql = "SELECT type, code, shares, name, base FROM [SecurityHolding] WHERE code = '" + code + "' AND type = '" + type + "' AND accountNumber = '" + userAccountNumber + "'";
            DataTable dtValue = myHKeInvestData.getData(sql);
            decimal shares = 0;
            decimal currentValue = 0;
            string name = null;
            if (dtValue.Rows.Count != 0)
            {
                foreach (DataRow row in dtValue.Rows)
                {
                    string currency = row["base"].ToString().Trim();
                    name = row["name"].ToString().Trim();

                    //convert to HKD
                    decimal fromRate = myExternalFunctions.getCurrencyRate(currency);
                    decimal currentPrice = myHKeInvestCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", myExternalFunctions.getSecuritiesPrice(type, code));

                    currentValue = currentValue + Convert.ToDecimal(row["shares"]) * currentPrice;
                }
            }

            decimal profitOrLoss = currentValue + totalSellAmount - totalBuyAmount - totalFeeCharged; ;

            //view result
            DataTable source = new DataTable();
            source.Columns.Add("type");
            source.Columns.Add("code");
            source.Columns.Add("name");
            source.Columns.Add("shares");
            source.Columns.Add("buyAmount");
            source.Columns.Add("sellAmount");
            source.Columns.Add("fee");
            source.Columns.Add("profit");

            DataRow record = source.NewRow();
            record["type"] = type;
            record["code"] = code;
            record["name"] = name;
            record["shares"] = shares;
            record["buyAmount"] = totalBuyAmount;
            record["sellAmount"] = totalSellAmount;
            record["fee"] = totalFeeCharged;

            if (viewIn.SelectedIndex == 0)//dollar
            {
                //convert to abs amount
                if (profitOrLoss < 0)
                {
                    profitOrLoss = Math.Abs(profitOrLoss);
                    record["profit"] = "(" + profitOrLoss + ")";
                }
                else
                    record["profit"] = profitOrLoss;
            }
            else if (viewIn.SelectedIndex == 1)//percentage
            {
                if (profitOrLoss == 0)
                    record["profit"] = 0 + "%";
                else if (profitOrLoss < 0)
                {
                    profitOrLoss = Math.Abs(profitOrLoss);
                    record["profit"] = "(" + (profitOrLoss / (totalBuyAmount + totalFeeCharged) * 100) + ") %";
                }
                else
                    record["profit"] = (profitOrLoss / (totalBuyAmount + totalFeeCharged) * 100) + "%";
            }

            source.Rows.Add(record);
            source.AcceptChanges();

            gvIndividual.DataSource = source;
            gvIndividual.DataBind();
            gvIndividual.Visible = true;
            
        }

        protected void gvIndividual_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void viewIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbDisplayType.SelectedIndex == 0)//all
                rbDisplayType_SelectedIndexChanged(sender, e);
            else if (rbDisplayType.SelectedIndex == 1)//type
                ddlSecurityType_SelectedIndexChanged(sender, e);
            else if (rbDisplayType.SelectedIndex == 2)
                txtSecurityCode_TextChanged(sender, e);
        }
    }
}