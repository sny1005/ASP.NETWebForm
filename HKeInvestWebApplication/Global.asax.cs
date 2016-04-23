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
            //Thread mythread = new Thread(PeriodicTasks);
            //mythread.IsBackground = true;
            //mythread.Start();
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

                while (orderNumbers.Count != 0)
                {
                    string status = myExternal.getOrderStatus(orderNumbers.First()).Trim();
                    if (!status.Equals("pending"))      // if the order is still pending in external system, there is no update on that order
                    {
                        string sql;
                        SqlTransaction trans = myData.beginTransaction();

                        // update order status
                        sql = string.Format("UPDATE [Order] SET [status] = '{0}' WHERE [orderNumber] = '{1}'", status, orderNumbers.First());
                        myData.setData(sql, trans);

                        // update transaction table
                        DataTable transTable = myExternal.getOrderTransaction(orderNumbers.First());
                        updateTransaction(ref transTable, myData, myCode, orderNumbers.First(), trans);

                        myData.commitTransaction(trans);

                        // setup for updating SecurityHolding
                        string accountNumber, securityType, securityCode, name, currency;
                        getOrderDetails(myExternal, myData, orderNumbers.First(), out sql, out accountNumber, out securityType, out securityCode, out name, out currency);

                        //get executed shares and price from retrieved new transaction record earlier
                        List<decimal> executeShares = new List<decimal>();
                        List<decimal> executePrice = new List<decimal>();
                        DataRow[] record = transTable.Select();
                        if (record.Count() >= 1)
                        {
                            for (int i = 0; i < record.Count(); i++)
                            {
                                executeShares.Add(Convert.ToDecimal(record[i]["executeShares"]));
                                executePrice.Add(Convert.ToDecimal(record[i]["executePrice"]));
                            }
                        }
                        else        // no new transaction for a partial stock order
                            continue;

                        trans = myData.beginTransaction();
                        // if the order is a buy order
                        // need to add the new/extra security holding record and decrease ac balance taking commision into account
                        if (myCode.isBuyOrder(orderNumbers.First()))
                        {
                            updateBuyHolding(ref myData, ref myCode, ref trans, accountNumber, securityType, securityCode, name, currency, executeShares.Sum());

                            // calculate money used and update account
                            decimal totalExpenditure = 0;
                            decimal acBalance = myCode.getAccountBalance(accountNumber);
                            string[,] CurrencyData = getCurrencyData(myCode);
                            while (executeShares.Count() != 0)
                            {
                                string fromRate = myCode.findCurrencyRate(CurrencyData, currency);
                                decimal executePriceHKD = myCode.convertCurrency(currency, fromRate, "HKD", "1", executePrice.First());

                                decimal expenditure = executePriceHKD * executeShares.First();         //money spent without comission

                                if (securityType == "stock")
                                {
                                    expenditure += stockFee(myCode, orderNumbers.First(), expenditure, acBalance);
                                }
                                else        //the order is on bond or unit trust
                                {
                                    if (acBalance < 500000)
                                        expenditure += expenditure * (decimal)0.05;                     //buying fee for assets less than HK$ 500,000
                                    else
                                        expenditure += expenditure * (decimal)0.03;                     //buying fee for assets more than or equal HK$ 500,000
                                }

                                totalExpenditure += expenditure;
                                executePrice.RemoveAt(0);
                                executeShares.RemoveAt(0);
                            }

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
                            string[,] CurrencyData = getCurrencyData(myCode);
                            string fromRate = myCode.findCurrencyRate(CurrencyData, currency);
                            while (executeShares.Count() != 0)
                            {
                                decimal executePriceHKD = myCode.convertCurrency(currency, fromRate, "HKD", "1", executePrice.First());

                                decimal revenue = executePriceHKD * executeShares.First();         //money gained without comission

                                if (securityType == "stock")
                                {
                                    revenue -= stockFee(myCode, orderNumbers.First(), revenue, acBalance);
                                }
                                else        //the order is on bond or unit trust
                                {
                                    if (acBalance < 500000)
                                        revenue -= 100;                    //selling fee for assets less than HK$ 500,000
                                    else
                                        revenue -= 50;                     //selling fee for assets more than or equal HK$ 500,000
                                }

                                totalRevenue += revenue;
                                executePrice.RemoveAt(0);
                                executeShares.RemoveAt(0);
                            }

                            acBalance += totalRevenue;
                            sql = "UPDATE [LoginAccount] SET [balance] = " + acBalance + " WHERE [accountNumber] = '" + accountNumber + "'";
                            myData.setData(sql, trans);
                        }
                        myData.commitTransaction(trans);

                        // TODO: send invoice







                    }
                    orderNumbers.Dequeue();
                }

                /*
                check order,  reduce cost by only checking incomplete orders
                if any changes
                    update transaction(finished)
                    update order, change status (finished)
                    if(buy) (finished)
                        increase security holding
                        calculate money used
                        update ac balance
                    else
                        decrease security holding
                        calculate gain
                        update ac balance
                    send invoice       
                */

            } while (true);
        }

        private static string[,] getCurrencyData(HKeInvestCode myCode)
        {
            // get currency data
            DataTable CurrencyTable = myCode.CurrencyData();
            string[,] CurrencyData = new string[CurrencyTable.Columns.Count, CurrencyTable.Rows.Count];

            int i = 0;
            foreach (DataRow row in CurrencyTable.Rows)
            {
                CurrencyData[0, i] = Convert.ToString(row["currency"]);
                CurrencyData[1, i] = Convert.ToString(row["rate"]);
                i++;
            }
            return CurrencyData;
        }

        private static decimal stockFee(HKeInvestCode myCode, string orderNumber, decimal expenditure, decimal acBalance)
        {
            decimal fee;
            string orderType = myCode.getOrderType(orderNumber).Trim();
            if (acBalance < 1000000)
            {
                if (orderType.Equals("market"))
                    fee = expenditure * (decimal)0.04;
                else if (orderType.Equals("stop limit"))
                    fee = expenditure * (decimal)0.08;
                else
                    fee = expenditure * (decimal)0.06;

                fee = Math.Min(150, fee);
            }
            else
            {
                if (orderType.Equals("market"))
                    fee = expenditure * (decimal)0.02;
                else if (orderType.Equals("stop limit"))
                    fee = expenditure * (decimal)0.06;
                else
                    fee = expenditure * (decimal)0.04;

                fee = Math.Min(100, fee);
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
                sql = string.Format("INSERT INTO [SecurityHolding] VALUES ( '{0}', '{1}', '{2}', '{3}', {4}, '{5}', NULL, NULL)", para);
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

        private static void getOrderDetails(ExternalFunctions myExternal, HKeInvestData myData, string orderNumber, out string sql, out string accountNumber, out string securityType, out string securityCode, out string name, out string currency)
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
            Table = myExternal.getSecuritiesByCode(securityType, securityCode);
            record = Table.Select();
            if (record.Count() != 1)       //should never happen
                throw new Exception("Error! Returning non-single record!");
            name = Convert.ToString(record[0]["name"]).Trim();
            if (securityType == "stock")    // all stocks have base "HKD"
                currency = "HKD";
            else
                currency = Convert.ToString(record[0]["base"]).Trim();
        }

        // the transaction table will be modified and the transTable is reduced to holding new transaction records only
        private static void updateTransaction(ref DataTable transTable, HKeInvestData myData, HKeInvestCode myCode, string orderNumber, SqlTransaction trans)
        {
            if (transTable == null)
                throw new Exception("Database error! Cannot retrieve records!");
            else if (transTable.Rows.Count == 1)
            {
                DataRow[] record = transTable.Select();
                DataRow row = record[0];
                // TODO: may need to update the stock order as well...
                object[] para = { Convert.ToString(row["transactionNumber"]), Convert.ToString(row["referenceNumber"]), Convert.ToString(row["executeDate"]), Convert.ToString(row["executeShares"]), Convert.ToString(row["executePrice"]) };
                string sql = string.Format("INSERT INTO [Transaction] VALUES ( {0}, {1}, '{2}', {3}, {4})", para);
                myData.setData(sql, trans);
            }
            else
            {
                for (int i=0; i<transTable.Rows.Count; i++)
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
                    {
                        row.Delete();
                    }
                }
            }




        }
    }
}