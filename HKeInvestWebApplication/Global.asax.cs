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
                Thread.Sleep(10*1000);
                Queue<string> orderNumbers = myCode.getIncompleteOrder();

                while (orderNumbers.Count != 0)
                {
                    string status = myExternal.getOrderStatus(orderNumbers.First()).Trim();
                    if (!status.Equals("pending"))      // if the order is still pending, there is no update on that order
                    {
                        string sql;
                        SqlTransaction trans = myData.beginTransaction();

                        // update order status
                        sql = string.Format("UPDATE [Order] SET [status] = '{0}' WHERE [orderNumber] = '{1}'", status, orderNumbers.First());
                        myData.setData(sql, trans);

                        // update transaction table
                        DataTable transTable = myExternal.getOrderTransaction(orderNumbers.First());
                        updateTransaction(transTable, myData, myCode, orderNumbers.First(), trans);

                        myData.commitTransaction(trans);

                        // TODO: move getHoldingDetails() into "new security" section
                        // setup for updating SecurityHolding
                        string accountNumber, securityType, securityCode, name, currency;
                        getHoldingDetails(myExternal, myData, orderNumbers.First(), out sql, out accountNumber, out securityType, out securityCode, out name, out currency);

                        //get executed shares from retrieved transaction record earlier
                        DataRow[] record = transTable.Select();
                        string shares = Convert.ToString(record[0]["executeShares"]).Trim();

                        trans = myData.beginTransaction();
                        // if the order is a buy order
                        // need to add the new/extra security holding record and decrease ac balance taking commision into account
                        if (myCode.isBuyOrder(orderNumbers.First()))
                        {
                            //assume new security
                            object[] para = { accountNumber, securityType, securityCode, name, shares, currency };
                            sql = string.Format("INSERT INTO [SecurityHolding] VALUES ( '{0}', '{1}', '{2}', '{3}', {4}, '{5}', NULL, NULL)", para);
                            myData.setData(sql, trans);

                            //
                            // TODO: implement the case that the security is already owned by the ac
                            //
                        }
                        // if the order is a sell order
                        // need to delete(subtract) the sold(shares) security holding record and increase ac balance taking commision into account
                        else
                        {

                        }

                        myData.commitTransaction(trans);
                    }
                    orderNumbers.Dequeue();
                }
                /*
                check order,  reduce cost by only checking incomplete orders
                if any changes
                    update transaction(finished)
                    update order, change status (finished)
                    if(buy)
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

        private static void getHoldingDetails(ExternalFunctions myExternal, HKeInvestData myData, string orderNumber, out string sql, out string accountNumber, out string securityType, out string securityCode, out string name, out string currency)
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

        private static void updateTransaction(DataTable transTable, HKeInvestData myData, HKeInvestCode myCode, string orderNumber, SqlTransaction trans)
        {
            if (transTable == null)
                throw new Exception("Database error! Cannot retrieve records!");
            foreach (DataRow row in transTable.Rows)
            {
                //if it is a stock need to check if adding partial transaction is needed
                if (!myCode.isExistTransaction(Convert.ToString(row["transactionNumber"])))
                {
                    object[] para = { Convert.ToString(row["transactionNumber"]), Convert.ToString(row["referenceNumber"]), Convert.ToString(row["executeDate"]), Convert.ToString(row["executeShares"]), Convert.ToString(row["executePrice"]) };
                    string sql = string.Format("INSERT INTO [Transaction] VALUES ( {0}, {1}, '{2}', {3}, {4})", para);
                    myData.setData(sql, trans);
                }
            }
        }
    }
}