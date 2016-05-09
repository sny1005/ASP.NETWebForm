using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Net.Mail;
using System.Net;

namespace HKeInvestWebApplication.Code_File
{
    //**********************************************************
    //*  THE CODE IN THIS CLASS CAN BE MODIFIED AND ADDED TO.  *
    //**********************************************************
    public class HKeInvestCode
    {
        public string getDataType(string value)
        {
            // Returns the data type of value. Tests for more types can be added if needed.
            if (value != null)
            {
                int n; decimal d; DateTime dt;
                if (int.TryParse(value, out n)) { return "System.Int32"; }
                else if (decimal.TryParse(value, out d)) { return "System.Decimal"; }
                else if (DateTime.TryParse(value, out dt)) { return "System.DateTime"; }
            }
            return "System.String";
        }

        public string getSortDirection(System.Web.UI.StateBag viewState, string sortExpression)
        {
            // If the GridView is sorted for the first time or sorting is being done on a new column, 
            // then set the sort direction to "ASC" in ViewState.
            if (viewState["SortDirection"] == null || viewState["SortExpression"].ToString() != sortExpression)
            {
                viewState["SortDirection"] = "ASC";
            }
            // Othewise if the same column is clicked for sorting more than once, then toggle its SortDirection.
            else if (viewState["SortDirection"].ToString() == "ASC")
            {
                viewState["SortDirection"] = "DESC";
            }
            else if (viewState["SortDirection"].ToString() == "DESC")
            {
                viewState["SortDirection"] = "ASC";
            }
            return viewState["SortDirection"].ToString();
        }

        public DataTable unloadGridView(GridView gv)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                dt.Columns.Add(((BoundField)gv.Columns[i]).DataField);
            }

            // For correct sorting, set the data type of each DataTable column based on the values in the GridView.
            gv.SelectedIndex = 0;
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                dt.Columns[i].DataType = Type.GetType(getDataType(gv.SelectedRow.Cells[i].Text));

            }

            // Load the GridView data into the DataTable.
            foreach (GridViewRow row in gv.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < gv.Columns.Count; j++)
                {
                    dr[((BoundField)gv.Columns[j]).DataField.ToString().Trim()] = row.Cells[j].Text;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public int getColumnIndexByName(GridView gv, string columnName)
        {
            // Helper method to get GridView column index by a column's DataField name.
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (((BoundField)gv.Columns[i]).DataField.ToString().Trim() == columnName.Trim())
                { return i; }
            }
            MessageBox.Show("Column '" + columnName + "' was not found \n in the GridView '" + gv.ID.ToString() + "'.");
            return -1;
        }

        public decimal convertCurrency(string fromCurrency, string fromCurrencyRate, string toCurrency, string toCurrencyRate, decimal value)
        {
            if(fromCurrency == toCurrency)
            {
                return value;
            }
            else
            {
                return Math.Round(Convert.ToDecimal(fromCurrencyRate) / Convert.ToDecimal(toCurrencyRate) * value - (decimal).005, 2);
            }
        }

        //addtional helper function to get the exchange rate for the target currency
        public string findCurrencyRate(string[,] currency, string target)
        {
            for(int i=0; i < currency.GetLength(1); i++)
            {
                if (currency[0, i] == target.Trim())
                    return currency[1, i];
            }
            return "-1";
        }

        //access the external system and put the currency data into view state
        public DataTable CurrencyData()
        {
            //declare local objects;
            ExternalFunctions myExternalFunctions = new ExternalFunctions();

            // Get the available currencies to populate the DropDownList.
            DataTable dtCurrency = myExternalFunctions.getCurrencyData();

            return dtCurrency;
        }

        public string getAccountNumber(string username)
        {
            HKeInvestWebApplication.Code_File.HKeInvestData myData = new HKeInvestData();

            string sql = "select [accountNumber] from [LoginAccount] where username ='" + username + "'";

            DataTable dtclient = myData.getData(sql);
            if (dtclient == null) { return ""; } // if the dataset is null, a sql error occurred.
            else if (dtclient.Rows.Count > 1)   //should never happen
            {
                System.Web.HttpContext.Current.Response.Write("Databse error, returning more than one account!");
                return "";
            }

            string accountNumber = "";
            foreach (DataRow row in dtclient.Rows)
            {
                accountNumber = (string)row["accountnumber"];
            }
            return accountNumber;
        }

        // get account balance given a specific account number
        public decimal getAccountBalance(string accountNumber)
        {
            HKeInvestWebApplication.Code_File.HKeInvestData myData = new HKeInvestData();

            string sql = "select [balance] from [LoginAccount] where accountNumber ='" + accountNumber + "'";

            DataTable dtclient = myData.getData(sql);
            if (dtclient == null) { return -1; } // if the dataset is null, a sql error occurred.
            else if (dtclient.Rows.Count > 1)   //should never happen
            {
                MessageBox.Show("Database Error! Returning more than 1 entry!");
                return -1;
            }

            decimal balance = -1;
            foreach (DataRow row in dtclient.Rows)
            {
                balance = Convert.ToDecimal(row["balance"]);
            }
            return balance;
        }

        //returns all the incomplete orders' orderNumber in our database as a list
        public Queue<string> getIncompleteOrder()
        {
            HKeInvestData myData = new HKeInvestData();
            Queue<string> orderNumbers = new Queue<string>();

            string sql = "SELECT [orderNumber] FROM [Order] WHERE [status] <> 'completed' AND [status] <> 'cancelled'";
            DataTable orders = myData.getData(sql);

            foreach (DataRow row in orders.Rows)
            {
                orderNumbers.Enqueue(Convert.ToString(row["orderNumber"]));
            }
            return orderNumbers;
        }

        public string getTypeFromOrder(string orderNumber)
        {
            HKeInvestData myData = new HKeInvestData();
            orderNumber = orderNumber.Trim();

            string sql = "SELECT [securityType] FROM [Order] WHERE [orderNumber] = " + orderNumber;
            DataTable recordTable = myData.getData(sql);

            DataRow[] record = recordTable.Select();
            if (record.Count() != 1)       //should never happen
                throw new Exception("Error! Returning non-single record!");
            return Convert.ToString(record[0]["securityType"]);
        }

        public bool isExistTransaction(string transactionNumber)
        {
            HKeInvestData myData = new HKeInvestData();
            transactionNumber = transactionNumber.Trim();

            string sql = "SELECT [transactionNumber] FROM [Transaction] WHERE [transactionNumber] = " + transactionNumber;
            DataTable recordTable = myData.getData(sql);

            DataRow[] record = recordTable.Select();
            if (record.Count() == 1)
                return true;
            else
                return false;
        }

        public bool isBuyOrder(string orderNumber)
        {
            HKeInvestData myData = new HKeInvestData();
            orderNumber = orderNumber.Trim();

            string sql = "SELECT [buyOrSell] FROM [Order] WHERE [orderNumber] = " + orderNumber;
            DataTable recordTable = myData.getData(sql);

            DataRow[] record = recordTable.Select();
            if (record.Count() == 1)
            {
                if (Convert.ToString(record[0]["buyOrSell"]).Trim() == "buy")
                    return true;
                return false;
            }
            else        //should never happen
                throw new Exception("Error! Returning more than 1 record!");
        }

        public decimal getOwnedShares(string accountNumber, string type, string code)
        {
            HKeInvestData myData = new HKeInvestData();
            string sql = string.Format("SELECT [shares] FROM [SecurityHolding] WHERE [accountNumber] = '{0}' AND [type] = '{1}' AND [code] = '{2}'", accountNumber, type, code);
            DataTable Table = myData.getData(sql);
            DataRow[] record = Table.Select();
            if (record.Count() == 1)
                return Convert.ToDecimal(record[0]["shares"]);
            else if (record.Count() == 0)
                return 0;
            else
                throw new Exception("Error!Returning more than 1 record!");
        }

        public string getOrderType(string orderNumber)
        {
            orderNumber = orderNumber.Trim();
            HKeInvestData myData = new HKeInvestData();
            string sql = "SELECT [orderType] FROM [StockOrder] WHERE [orderNumber] = '" + orderNumber + "'";

            DataTable Table = myData.getData(sql);
            DataRow[] record = Table.Select();
            if (record.Count() == 1)
                return Convert.ToString(record[0]["orderType"]);
            else
                throw new Exception("Error!Returning non-single record!");
        }

        public DateTime getSubmittedDate(string orderNumber)
        {
            orderNumber = orderNumber.Trim();
            HKeInvestData myData = new HKeInvestData();
            string sql = "SELECT [dateSubmitted] FROM [Order] WHERE [orderNumber] = '" + orderNumber + "'";

            DataTable Table = myData.getData(sql);
            DataRow[] record = Table.Select();
            if (record.Count() == 1)
                return Convert.ToDateTime(record[0]["dateSubmitted"]);
            else
                throw new Exception("Error!Returning non-single record!");
        }

        public void getSecurityNameBase(string securityType, string securityCode, out string name, out string baseCurrency)
        {
            securityType.Trim();
            securityCode.Trim();
            ExternalFunctions myExternal = new ExternalFunctions();
            DataTable Table = myExternal.getSecuritiesByCode(securityType, securityCode);
            if (Table == null)
            {
                name = "";
                baseCurrency = "";
                return;
            }
            DataRow[] record = Table.Select();
            if (record.Count() != 1)       //should never happen
                throw new Exception("Error! Returning non-single record!");
            name = Convert.ToString(record[0]["name"]).Trim();
            if (securityType == "stock")    // all stocks have base "HKD"
                baseCurrency = "HKD";
            else
                baseCurrency = Convert.ToString(record[0]["base"]).Trim();
        }

        public decimal getAccountAsset(string accountNumber)
        {
            accountNumber = accountNumber.Trim();
            decimal asset;
            HKeInvestData myData = new HKeInvestData();
            ExternalFunctions myExternal = new ExternalFunctions();
            string sql = "SELECT [balance] FROM [LoginAccount] WHERE [LoginAccount].[accountNumber] = '" + accountNumber + "'";

            DataTable Table = myData.getData(sql);
            DataRow[] record = Table.Select();
            if (record.Count() == 0)
                throw new Exception("Error! No record retrieved!");
            else
                asset = Convert.ToDecimal(record[0]["balance"]);


            sql = "SELECT [code], [type], [shares], [base] FROM [LoginAccount] JOIN [SecurityHolding] ON [LoginAccount].[accountNumber] = [SecurityHolding].[accountNumber] WHERE [LoginAccount].[accountNumber] = '" + accountNumber + "'";
            Table = myData.getData(sql);
            foreach (DataRow row in Table.Rows)
            {
                decimal price = myExternal.getSecuritiesPrice((string)row["type"], (string)row["code"]);
                string rate = Convert.ToString(myExternal.getCurrencyRate((string)row["base"])).Trim();
                asset += convertCurrency("from", rate, "HKD", "1", price);
            }
            return asset;
        }

        public decimal getFeeCharged(string orderNumber)
        {
            decimal fee;
            orderNumber = orderNumber.Trim();
            HKeInvestData myData = new HKeInvestData();
            string sql = "SELECT [feeCharged] FROM [Order] WHERE [orderNumber] = '" + orderNumber + "'";

            DataTable Table = myData.getData(sql);
            DataRow[] record = Table.Select();

            if (record.Count() == 0)
                throw new Exception("Error! No record retrieved!");
            else
                decimal.TryParse(record[0]["feeCharged"].ToString().Trim(), out fee);

            return fee;
        }

        public bool securityCodeIsValid(string securityType, string securityCode)
        {
            ExternalFunctions myExternal = new ExternalFunctions();
            DataTable dt = myExternal.getSecuritiesByCode(securityType, securityCode);
            if (dt == null)
                return false;
            return true;
        }

        public void sendemail (string target, string subject, string body)
        {
            target = target.Trim();
            subject = subject.Trim();
            body = body.Trim();
            MailMessage mail = new MailMessage();
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");
            mail.From = new MailAddress("comp3111_team109@cse.ust.hk", "InvestPro");
            mail.To.Add(target);
            mail.Subject = subject;
            mail.Body = body;

            // Send the message.
            emailServer.Send(mail);
        }

    }
}