using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data;
using HKeInvestWebApplication.ExternalSystems.Code_File;

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
                else if (DateTime.TryParse(value, out dt)) { return "System.DataTime"; }
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

        public bool isExistSecurity(string type, string code)
        {

            return false;
        }

        //returns all the incomplete orders' orderNumber as a list
        public Queue<string> getIncompleteOrder()
        {
            HKeInvestData myData = new HKeInvestData();
            Queue<string> orderNumbers = new Queue<string>();

            string sql = "SELECT [orderNumber] FROM [Order] WHERE [status] <> 'completed'";
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

            string sql = "SELECT [securityType] FROM [Order] WHERE [orderNumber] = '" + orderNumber + "'";
            DataTable recordTable = myData.getData(sql);

            DataRow[] record = recordTable.Select();
            if (record.Count() != 1)       //should never happen
                throw new Exception("Error! Returning non-single record!");
            return Convert.ToString(record[0]["securityType"]);
        }

        //public void updateAccountBalance()
        //{
        //    object[] para = { orderNumber, accountNumber, "bond", BondCode.Text, rblTransType.SelectedValue, "pending" };
        //    string sql = String.Format("INSERT INTO [Order] VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", para);
        //    SqlTransaction trans = myHKeInvestData.beginTransaction();
        //    myHKeInvestData.setData(sql, trans);

        //    //should not be written in this way to update the ac balance
        //    if (ddlSecurityType.SelectedValue == "bond" || ddlSecurityType.SelectedValue == "unit trust")
        //    {
        //        sql = String.Format("UPDATE [LoginAccount] SET [balance] = {0} WHERE [accountNumber] = '{1}'", balance, accountNumber);
        //        myHKeInvestData.setData(sql, trans);
        //        lblAccountBalance.Text = "Account balance: " + balance;
        //    }

        //    myHKeInvestData.commitTransaction(trans);
        //}
    }
}