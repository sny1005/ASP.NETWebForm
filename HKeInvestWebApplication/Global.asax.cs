using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Data;
using System.Data.SqlClient;

namespace HKeInvestWebApplication
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Add periordic task for system to check order and transaction information
            Thread mythread = new Thread(PeriodicTasks);
            mythread.IsBackground = true;
            mythread.Start();
        }

        private void PeriodicTasks()
        {
            ExternalFunctions myExternal = new ExternalFunctions();
            HKeInvestData myData = new HKeInvestData();
            HKeInvestCode myCode = new HKeInvestCode();
            
            do
            {
                // Place the method call for the periodic task here.
                Thread.Sleep(10 * 1000);
                Queue<string> orderNumbers = myCode.getIncompleteOrder();

                buySellUpdate(myExternal, ref myData, ref myCode, orderNumbers);
                triggerAlert(myExternal, ref myData, ref myCode);

            } while (true);
        }

       private static void buySellUpdate(ExternalFunctions myExternal, ref HKeInvestData myData, ref HKeInvestCode myCode, Queue<string> orderNumbers)
        {
            while (orderNumbers.Count != 0)
            {
                string status = myExternal.getOrderStatus(orderNumbers.First()).Trim();
                if (!status.Equals("pending"))      // if the order is still pending in external system, there is no update on that order
                    //if (status.Equals("completed") || status.Equals("cancelled"))      // if the order is still pending in external system, there is no update on that order
                {
                    string sql;
                    SqlTransaction trans = myData.beginTransaction();

                    // update order status
                    sql = string.Format("UPDATE [Order] SET [status] = '{0}' WHERE [orderNumber] = '{1}'", status, orderNumbers.First());
                    myData.setData(sql, trans);

                    // update transaction table
                    DataTable transTable = myExternal.getOrderTransaction(orderNumbers.First());

                    if (transTable == null) //there is no transaction for that order (i.e. cancelled)
                    {
                        myData.commitTransaction(trans);
                        return;
                    }

                    updateTransaction(ref transTable, myData, myCode, orderNumbers.First(), trans);
                    myData.commitTransaction(trans);

                    // setup for updating SecurityHolding
                    string accountNumber, securityType, securityCode, name, currency;
                    getOrderDetails(ref myExternal, ref myCode, ref myData, orderNumbers.First(), out sql, out accountNumber, out securityType, out securityCode, out name, out currency);

                    //get executed shares and price from retrieved new transaction record earlier
                    List<decimal> executeShares = new List<decimal>();
                    List<decimal> executePrice = new List<decimal>();
                    List<decimal> fee = new List<decimal>();
                    List<decimal> dollarAmount = new List<decimal>();
                    DataRow[] record = transTable.Select();

                    // shares and price of new transactions
                    if (record.Count() >= 1)
                    {
                        for (int i = 0; i < record.Count(); i++)
                        {
                            executeShares.Add(Convert.ToDecimal(record[i]["executeShares"]));
                            executePrice.Add(Convert.ToDecimal(record[i]["executePrice"]));
                        }
                    }
                    else                     // no new transaction for a partial stock order
                    {              
                        orderNumbers.Dequeue();
                        continue;
                    }

                    trans = myData.beginTransaction();
                    // if the order is a buy order
                    // need to add the new/extra security holding record and decrease ac balance taking commision into account
                    decimal asset = myCode.getAccountAsset(accountNumber);
                    bool isBuyOrder = myCode.isBuyOrder(orderNumbers.First());
                    if (isBuyOrder)         //if buy order
                    {
                        updateBuyHolding(ref myData, ref myCode, ref trans, accountNumber, securityType, securityCode, name, currency, executeShares.Sum());

                        // calculate money used and update account
                        decimal totalExpenditure = 0;
                        decimal acBalance = myCode.getAccountBalance(accountNumber);
                        for (int i = 0; i < executePrice.Count(); i++)
                        {
                            decimal fromRate = myExternal.getCurrencyRate(currency);
                            decimal executePriceHKD = myCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", executePrice[i]);
                            executePrice[i] = executePriceHKD;                              //change the executePrice to base HKD

                            decimal expenditure = executePriceHKD * executeShares[i];       //money spent without comission
                            dollarAmount.Add(expenditure);

                            if (securityType == "stock")
                            {
                                fee.Add(stockFee(myCode, orderNumbers.First(), expenditure, asset));
                                expenditure += fee.Last();
                            }
                            else        //the order is on bond or unit trust
                            {
                                if (asset < 500000)
                                {
                                    fee.Add(expenditure * (decimal)0.05);
                                    expenditure += fee.Last();                     //buying fee for assets less than HK$ 500,000
                                }
                                else
                                {
                                    fee.Add(expenditure * (decimal)0.03);         //buying fee for assets more than or equal HK$ 500,000
                                    expenditure += fee.Last();
                                }
                            }

                            totalExpenditure += expenditure;
                        }

                        decimal alreadyCharged = myCode.getFeeCharged(orderNumbers.First());
                        // store total fee charged for the order
                        sql = "UPDATE [Order] SET [feeCharged] = " + (alreadyCharged + fee.Sum()) + " WHERE [orderNumber] = '" + orderNumbers.First().Trim() + "'";
                        myData.setData(sql, trans);

                        // update account balance
                        acBalance -= totalExpenditure;
                        sql = "UPDATE [LoginAccount] SET [balance] = " + acBalance + " WHERE [accountNumber] = '" + accountNumber + "'";
                        myData.setData(sql, trans);
                    }
                    // if the order is a sell order
                    // need to delete(subtract) the sold(shares) security holding record and increase ac balance taking commision into account
                    else
                    {
                        // repeat same procedure as buy
                        updateSellHolding(ref myData, ref myCode, ref trans, accountNumber, securityType, securityCode, name, currency, executeShares.Sum());

                        // calculate money used and update account
                        decimal totalRevenue = 0;
                        decimal acBalance = myCode.getAccountBalance(accountNumber);
                        decimal fromRate = myExternal.getCurrencyRate(currency);
                        for (int i = 0; i < executePrice.Count(); i++)
                        {
                            decimal executePriceHKD = myCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", executePrice[i]);
                            executePrice[i] = executePriceHKD;                              //change the executePrice to base HKD

                            decimal revenue = executePriceHKD * executeShares[i];         //money gained without comission
                            dollarAmount.Add(revenue);

                            if (securityType == "stock")
                            {
                                fee.Add(stockFee(myCode, orderNumbers.First(), revenue, asset));
                                revenue -= fee.Last();
                            }
                            else        //the order is on bond or unit trust
                            {
                                if (asset < 500000)
                                {
                                    fee.Add(100);
                                    revenue -= 100;                    //selling fee for assets less than HK$ 500,000
                                }
                                else
                                {
                                    fee.Add(50);
                                    revenue -= 50;                     //selling fee for assets more than or equal HK$ 500,000
                                }
                            }

                            totalRevenue += revenue;
                        }

                        decimal alreadyCharged = myCode.getFeeCharged(orderNumbers.First());
                        // store total fee charged for the order
                        sql = "UPDATE [Order] SET [feeCharged] = " + (alreadyCharged + fee.Sum()) + " WHERE [orderNumber] = '" + orderNumbers.First().Trim() + "'";
                        myData.setData(sql, trans);

                        // update account balance
                        acBalance += totalRevenue;
                        sql = "UPDATE [LoginAccount] SET [balance] = " + acBalance + " WHERE [accountNumber] = '" + accountNumber + "'";
                        myData.setData(sql, trans);
                    }
                    myData.commitTransaction(trans);

                    //send email and charge fee if order is finished
                    if (status.Equals("completed") || status.Equals("cancelled"))
                    {
                        // prepares mail body
                        // general information
                        char nl = '\n';
                        string mailBody = "Order number: " + orderNumbers.First() + nl;
                        mailBody += "Account number: " + accountNumber + nl;
                        mailBody += "Buy/Sell: " + isBuyOrder + nl;
                        mailBody += "Security code: " + securityCode + nl;
                        mailBody += "Security name: " + name + nl;

                        // list to store info for each transaction
                        List<decimal> sharesPer = new List<decimal>();
                        List<decimal> pricePer = new List<decimal>();

                        decimal totalShares = 0, totalAmount = 0;

                        sql = "SELECT executePrice, executeShares FROM [Transaction] WHERE orderNumber = '" + orderNumbers.First() + "'";
                        DataTable dt = myData.getData(sql);

                        foreach (DataRow row in dt.Rows)
                        {
                            // store info for every transaction
                            sharesPer.Add(Convert.ToDecimal(row["executeShares"]));
                            pricePer.Add(Convert.ToDecimal(row["executePrice"]));

                            // convert back to HKD
                            decimal fromRate = myExternal.getCurrencyRate(currency);
                            decimal executePriceHKD = myCode.convertCurrency(currency, Convert.ToString(fromRate).Trim(), "HKD", "1", Convert.ToDecimal(row["executePrice"]));

                            totalAmount += executePriceHKD * Convert.ToDecimal(row["executeShares"]);
                            totalShares += Convert.ToDecimal(row["executeShares"]);
                        }

                        // if the security is a stock, extra info in email
                        if (securityType == "stock")
                        {
                            mailBody += nl;
                            string orderType = myCode.getOrderType(orderNumbers.First());
                            DateTime date = myCode.getSubmittedDate(orderNumbers.First());
                            decimal totalFee = myData.getAggregateValue("SELECT SUM(shares) FROM [StockOrder] WHERE orderNumber = '" + orderNumbers.First() + "'");

                            mailBody += "Order type: " + orderType + nl;
                            mailBody += "Submitted date: " + date + nl;
                            mailBody += "Total shares bought/sold: " + totalShares + nl;
                            mailBody += "Total dollar amount: " + totalAmount + nl;
                            mailBody += "Total fee charged: " + totalFee + nl;
                        }

                        // for each transaction
                        mailBody += nl;

                        decimal numberOfTrans = myData.getAggregateValue("SELECT COUNT(*) FROM [Transaction] WHERE orderNumber = '" + orderNumbers.First() + "'");
                        for (int i = 0; i < (int)numberOfTrans; i++)
                        {
                            mailBody += "Transaction number: " + transTable.Rows[i]["transactionNumber"] + nl;
                            mailBody += "Quantity of shares: " + sharesPer[i] + nl;
                            mailBody += "Price per share: " + pricePer[i] + nl;
                            mailBody += nl;
                        }

                        // TODO: send the invoice
                        //System.Windows.Forms.MessageBox.Show(mailBody);
                        sql = "SELECT [email] from [Client] WHERE [isPrimary] = 'true' AND [accountNumber] = '" + accountNumber + "'";
                        DataTable Table = myData.getData(sql);
                        record = Table.Select();
                        if (record.Count() != 1)
                            throw new Exception("Error! Returning non-single record!");
                        else
                        {
                            string mailTo = record[0]["email"].ToString().Trim();
                            string subject = "Order Invoice";
                            myCode.sendemail(mailTo, subject, mailBody);
                        }
                    }
                }
                orderNumbers.Dequeue();
            }
        }
       
        private static decimal stockFee(HKeInvestCode myCode, string orderNumber, decimal expenditure, decimal asset)
        {
            decimal fee;
            string orderType = myCode.getOrderType(orderNumber).Trim();
            if (asset < 1000000)
            {
                if (orderType.Equals("market"))
                    fee = expenditure * (decimal)0.004;
                else if (orderType.Equals("stop limit"))
                    fee = expenditure * (decimal)0.008;
                else
                    fee = expenditure * (decimal)0.006;

                fee = Math.Max(150, fee);
            }
            else
            {
                if (orderType.Equals("market"))
                    fee = expenditure * (decimal)0.002;
                else if (orderType.Equals("stop limit"))
                    fee = expenditure * (decimal)0.006;
                else
                    fee = expenditure * (decimal)0.004;

                fee = Math.Max(100, fee);
            }

            return fee;
        }

        private static void updateBuyHolding(ref HKeInvestData myData, ref HKeInvestCode myCode, ref SqlTransaction trans, string accountNumber, string securityType, string securityCode, string name, string currency, decimal shares)
        {
            string sql;
            // get current shares first
            decimal ownedShares = myCode.getOwnedShares(accountNumber, securityType, securityCode);

            if (ownedShares == 0)
            {
                // new security bought by account
                object[] para = { accountNumber, securityType, securityCode, name, shares, currency };
                sql = string.Format("INSERT INTO [SecurityHolding] VALUES ( '{0}', '{1}', '{2}', '{3}', {4}, '{5}')", para);
                myData.setData(sql, trans);
            }
            else
            {
                // the security is already owned by ac
                ownedShares += shares;
                object[] para = { ownedShares, accountNumber, securityType, securityCode };
                sql = string.Format("UPDATE [SecurityHolding] SET [shares] = {0} WHERE [accountNumber] = '{1}' AND [type] = '{2}' AND [code] = '{3}'", para);
                myData.setData(sql, trans);
            }
        }

        private static void updateSellHolding(ref HKeInvestData myData, ref HKeInvestCode myCode, ref SqlTransaction trans, string accountNumber, string securityType, string securityCode, string name, string currency, decimal shares)
        {
            string sql;

            // get current shares first
            decimal ownedShares = myCode.getOwnedShares(accountNumber, securityType, securityCode);
            if (ownedShares <= 0)
                throw new Exception("The security is not owned by the account!");
            else
            {
                ownedShares -= shares;
                if (ownedShares == 0)       //all shares of that security is sold
                {
                    object[] para = { accountNumber, securityType, securityCode };
                    sql = string.Format("DELETE FROM [SecurityHolding] WHERE [accountNumber] = '{0}' AND [type] = '{1}' AND [code] = '{2}'", para);
                    myData.setData(sql, trans);
                    return;
                }
                // sold part of the shares owned
                else
                {
                    object[] para = { ownedShares, accountNumber, securityType, securityCode };
                    sql = string.Format("UPDATE [SecurityHolding] SET [shares] = {0} WHERE [accountNumber] = '{1}' AND [type] = '{2}' AND [code] = '{3}'", para);
                    myData.setData(sql, trans);
                }
            }
        }

        private static void getOrderDetails(ref ExternalFunctions myExternal, ref HKeInvestCode myCode, ref HKeInvestData myData, string orderNumber, out string sql, out string accountNumber, out string securityType, out string securityCode, out string name, out string currency)
        {
            // get accountNumber, security type and code
            sql = "SELECT [accountNumber], [securityType], [securityCode] FROM [Order] WHERE [orderNumber] = " + orderNumber;
            DataTable Table = myData.getData(sql);
            DataRow[] record = Table.Select();
            if (record.Count() != 1)       //should never happen
                throw new Exception("Error! Returning non-single record!");
            accountNumber = Convert.ToString(record[0]["accountNumber"]).Trim();
            securityType = Convert.ToString(record[0]["securityType"]).Trim();
            securityCode = Convert.ToString(record[0]["securityCode"]).Trim();

            // get security name, base
            myCode.getSecurityNameBase(securityType, securityCode, out name, out currency);
        }

        // the transaction table will be modified and the transTable is reduced to holding new transaction records only
        private static void updateTransaction(ref DataTable transTable, HKeInvestData myData, HKeInvestCode myCode, string orderNumber, SqlTransaction trans)
        {
            if (transTable == null) //there is no transaction for that order
                return;
            else
            {
                for (int i = 0; i < transTable.Rows.Count; i++)
                {
                    //if it is a stock need to check if adding partial transaction is needed
                    DataRow row = transTable.Rows[i];
                    if (!myCode.isExistTransaction(Convert.ToString(row["transactionNumber"])))
                    {
                        // TODO: may need to update the stock order as well...
                        object[] para = { Convert.ToString(row["transactionNumber"]), Convert.ToString(row["referenceNumber"]), Convert.ToString(row["executeDate"]), Convert.ToString(row["executeShares"]), Convert.ToString(row["executePrice"]) };
                        string sql = string.Format("INSERT INTO [Transaction] VALUES ( {0}, {1}, '{2}', {3}, {4})", para);
                        myData.setData(sql, trans);
                    }
                    else
                        row.Delete();
                }
                transTable.AcceptChanges();
            }
        }

        private static void triggerAlert(ExternalFunctions myExternal, ref HKeInvestData myData, ref HKeInvestCode myCode)
        {
            string sql = "SELECT * FROM [Alert]";
            DataTable dtAlert = myData.getData(sql);
            if (dtAlert.Rows.Count == 0 || dtAlert == null) //no alert record
                return;
            foreach(DataRow row in dtAlert.Rows)
            {
                string accountNumber = row["accountNumber"].ToString().Trim();
                string alertType = row["alertType"].ToString().Trim();
                string type = row["type"].ToString().Trim();
                string code = row["code"].ToString().Trim();
                decimal value = Convert.ToDecimal(row["value"]);

                // date and last update can be null
                // will cause exception here
                // need fix
                DateTime date;
                DateTime.TryParse(row["dateOfTrigger"].ToString().Trim(), out date);
                //decimal lastPrice = Convert.ToDecimal(row["lastUpdate"]);

                //temp fix
                decimal lastPrice = -1;
                decimal.TryParse(row["lastUpdate"].ToString().Trim(), out lastPrice);

                string findEmail = "SELECT email FROM [Client] WHERE accountNumber = '"+accountNumber+"' AND isPrimary = 'true'";
                DataTable dtEmail = myData.getData(findEmail);
              
                string email = dtEmail.Rows[0][0].ToString().Trim();
              
                DateTime current = DateTime.Now.Date;
                if (date == current) //already sent email today
                    break;

                string subject = "Alert From HKeInvest";
                string body = "Dear Customer," + Environment.NewLine + "The price of the following security passes/reaches '"+value+"': " + Environment.NewLine + "Security Type: '"+type+"' " + Environment.NewLine + "Secuirty Code: '"+code+"'";

                decimal currentPrice = myExternal.getSecuritiesPrice(type, code);

                //begin transaction
                SqlTransaction trans = myData.beginTransaction();

                //low value
                if (alertType == "lowAlert" && lastPrice != -1)
                {
                    if (value == currentPrice || (currentPrice < value && value < lastPrice)) // reach or pass
                    {
                        myCode.sendemail(email, subject, body);
                        
                        string insert = "UPDATE [Alert] SET dateOfTrigger = '" + current + "' WHERE accountNumber='" + accountNumber + "' AND alertType = '" + alertType + "' AND type = '" + type + "' AND code = '" + code + "' AND value = '" + value + "'"; //record date
                        myData.setData(insert, trans);
                    }
                }
                //high value
               else if (alertType == "highAlert" && lastPrice != -1)
                {
                    if (value == currentPrice || (currentPrice > value && value > lastPrice)) // reach or pass
                    {
                        myCode.sendemail(email, subject, body);
                        string insert = "UPDATE [Alert] SET dateOfTrigger = '" + current + "' WHERE accountNumber='" + accountNumber + "' AND alertType = '" + alertType + "' AND type = '" + type + "' AND code = '" + code + "' AND value = '" + value + "'"; //record date
                        myData.setData(insert, trans);
                    }
                }
                
                sql = "UPDATE [Alert] SET lastUpdate = '"+currentPrice+"' WHERE accountNumber='"+accountNumber+"' AND alertType = '"+alertType+"' AND type = '"+type+"' AND code = '"+code+"' AND value = '"+value+"'"; //update lastest price
                myData.setData(sql, trans);
                myData.commitTransaction(trans);
            }
        }
    }
}