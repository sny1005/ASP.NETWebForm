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
                    if (!status.Equals("pending"))
                    {
                        // if the status is updated to any other states, need to synchronise 2 databases
                        //if it is a stock need to check if adding partial transaction is needed
                        if (myCode.getTypeFromOrder(orderNumbers.First()).Trim() == "stock")
                        {
                            DataTable transTable = myExternal.getOrderTransaction(orderNumbers.First());
                            //transTable.Rows.Count;
                            //
                            //TODO : update transaction table
                            //currently working on inserting new record into the transaction table for a specific updated order
                            //
                            //
                        }






                        // this part is working
                        SqlTransaction trans = myData.beginTransaction();

                        string sql = string.Format("UPDATE [Order] SET [status] = '{0}' WHERE [orderNumber] = '{1}'", status, orderNumbers.First());
                        myData.setData(sql, trans);

                        myData.commitTransaction(trans);
                    }








                    orderNumbers.Dequeue();
                }
/*
    check order,  reduce cost by only checking incomplete orders
    if any changes
        update transaction
        update security holding
        calculate gain using formula if sell
        subtract fee from balance if buy
        update login ac, change balance
        update order, change status     
        send invoice       
*/
            } while (true);
        }

        private void synchronizeData()
        {

            return;
        }
    }
}