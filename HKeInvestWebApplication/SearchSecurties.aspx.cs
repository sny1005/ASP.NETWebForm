using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using HKeInvestWebApplication.Code_File;
using System.Data.SqlClient;
using System.Data;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Text;



namespace HKeInvestWebApplication
{
    public partial class SearchSecurities : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            //StockGV.Visible = false;
            //BondGV.Visible = false;
            //UnitTrustGV.Visible = false;
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            //// Nothing
            //if (ddlSecurityType.SelectedItem.Value == 0 && SecName.Text == null && SecCode.Text == null)
            //{
            //    args.IsValid = false;
            //    CVSearch.ErrorMessage = "Please fill in your searching information.";
            //}

            //// ALL Bond
            //if (ddlSecurityType.SelectedItem.Value == "bond" && SecName.Text == null && SecCode.Text == null)
            //{
            //    DataTable dtBond = myExternalFunctions.getSecuritiesData(“bond”);
            //    BondGV.DataSource = dtBond;
            //    BondGV.DataBind();
            //    BondGV.Visible = true;
            //}

            //// ALL Stock
            //if (ddlSecurityType.SelectedValue == "stock" && SecName.Text == null && SecCode.Text == null)
            //{
            //    DataTable dtStock = myExternalFunctions.getSecuritiesData(“stock”);
            //    StockGV.DataSource = dtStock;
            //    StockGV.DataBind();
            //    StockGV.Visible = true;
            //}

            //// ALL Unit Trust
            //if (ddlSecurityType.SelectedValue == "unit trust" && SecName.Text == null && SecCode.Text == null)
            //{
            //    DataTable dtUT = myExternalFunctions.getSecuritiesData(“unit trust”);
            //    UnitTrustGV.DataSource = dtUT;
            //    UnitTrustGV.DataBind();
            //    UnitTrustGV.Visible = true;
            //}

            //// Bond with Name
            //if (ddlSecurityType.SelectedItem.Value == "bond" && SecName.Text != null && SecCode.Text == null)
            //{
            //    string input = SecName.Text.Trim();
            //    DataTable dtBondN = myExternalFunctions.getSecuritiesByName(“bond”, input);
            //    BondGV.DataSource = dtBondN;
            //    BondGV.DataBind();
            //    BondGV.Visible = true;
            //}

            //// Stock with Name
            //if (ddlSecurityType.SelectedValue == "stock" && SecName.Text != null && SecCode.Text == null)
            //{
            //    string input = SecName.Text.Trim();
            //    DataTable dtStockN = myExternalFunctions.getSecuritiesByName(“stock”, input);
            //    StockGV.DataSource = dtStockN;
            //    StockGV.DataBind();
            //    StockGV.Visible = true;
            //}

            //// Unit Trust with Name
            //if (ddlSecurityType.SelectedValue == "unit trust" && SecName.Text != null && SecCode.Text == null)
            //{
            //    string input = SecName.Text.Trim();
            //    DataTable dtUTN = myExternalFunctions.getSecuritiesByName(“unit trust”, input);
            //    UnitTrustGV.DataSource = dtUTN;
            //    UnitTrustGV.DataBind();
            //    UnitTrustGV.Visible = true;
            //}

            //// Bond with Code
            //if (ddlSecurityType.SelectedItem.Value == "bond" && SecName.Text == null && SecCode.Text != null)
            //{
            //    string input = SecCode.Text.Trim();
            //    DataTable dtBondC = myExternalFunctions.getSecuritiesByCode(“bond”, input);
            //    BondGV.DataSource = dtBondC;
            //    BondGV.DataBind();
            //    BondGV.Visible = true;
            //}

            //// Stock with Code
            //if (ddlSecurityType.SelectedValue == "stock" && SecName.Text == null && SecCode.Text != null)
            //{
            //    string input = SecCode.Text.Trim();
            //    DataTable dtStockC = myExternalFunctions.getSecuritiesByCode(“stock”, input);
            //    StockGV.DataSource = dtStockC;
            //    StockGV.DataBind();
            //    StockGV.Visible = true;
            //}

            //// Unit Trust with Code
            //if (ddlSecurityType.SelectedValue == "unit trust" && SecName.Text == null && SecCode.Text != null)
            //{
            //    string input = SecCode.Text.Trim();
            //    DataTable dtUTC = myExternalFunctions.getSecuritiesByCode(“unit trust”, input);
            //    UnitTrustGV.DataSource = dtUTC;
            //    UnitTrustGV.DataBind();
            //    UnitTrustGV.Visible = true;
            //}


  
            //// Bond with Name + Code
            //if (ddlSecurityType.SelectedItem.Value == "bond" && SecName.Text != null && SecCode.Text != null)
            //{
            //    string inputN = SecName.Text.Trim();
            //    string inputC = SecCode.Text.Trim();
            //    DataTable dtBondN = myExternalFunctions.getSecuritiesByName(“bond”, inputN);
            //    DataTable dtBondC = myExternalFunctions.getSecuritiesByCode(“bond”, inputC);
            //    DataTable dtBondR;





            //    BondGV.DataSource = dtBondR;
            //    BondGV.DataBind();
            //    BondGV.Visible = true;
            //}

            //// Stock with Name + Code
            //if (ddlSecurityType.SelectedValue == "stock" && SecName.Text != null && SecCode.Text != null)
            //{
            //    string inputN = SecName.Text.Trim();
            //    string inputC = SecCode.Text.Trim();
            //    DataTable dtStockN = myExternalFunctions.getSecuritiesByName(“stock”, inputN);
            //    DataTable dtStockC = myExternalFunctions.getSecuritiesByCode(“stock”, inputC);
            //    DataTable dtStockR;





            //    StockGV.DataSource = dtStockR;
            //    StockGV.DataBind();
            //    StockGV.Visible = true;
            //}

            //// Unit Trust with Name + Code
            //if (ddlSecurityType.SelectedValue == "unit trust" && SecName.Text != null && SecCode.Text != null)
            //{
            //    string inputN = SecName.Text.Trim();
            //    string inputC = SecCode.Text.Trim();
            //    DataTable dtUTN = myExternalFunctions.getSecuritiesByName(“unit trust”, inputN);
            //    DataTable dtUTC = myExternalFunctions.getSecuritiesByCode(“unit trust”, inputC);
            //    DataTable dtUTR;





            //    UnitTrustGV.DataSource = dtUTR;
            //    UnitTrustGV.DataBind();
            //    UnitTrustGV.Visible = true;
            //}











        }



        //protected void StockGV_Sorting_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Reset visbility of controls and initialize values.

        //    string sql = "";
        //    string accountNumber = txtAccountNumber.Text;

        //    // *******************************************************************
        //    // Set the account number and security type from the web page. *
        //    // *******************************************************************
        //    string securityType = ddlSecurityType.SelectedValue; // Set the securityType from a web form control!

        //    // Check if an account number has been specified.
        //    if (accountNumber == "")
        //    {
        //        lblResultMessage.Text = "Please specify an account number.";
        //        lblResultMessage.Visible = true;
        //        ddlSecurityType.SelectedIndex = 0;
        //        return;
        //    }

        //    // No action when the first item in the DropDownList is selected.
        //    if (securityType == "0") { return; }

        //    // *****************************************************************************************
        //    // Construct the SQL statement to retrieve the first and last name of the client(s). *
        //    // *****************************************************************************************
        //    sql = "SELECT lastName, firstName FROM Client WHERE accountNumber = '" + accountNumber + "'"; // Complete the SQL statement.

        //    DataTable dtClient = myHKeInvestData.getData(sql);
        //    if (dtClient == null) { return; } // If the DataSet is null, a SQL error occurred.

        //    // If no result is returned by the SQL statement, then display a message.
        //    if (dtClient.Rows.Count == 0)
        //    {
        //        lblResultMessage.Text = "No such account number.";
        //        lblResultMessage.Visible = true;
        //        lblClientName.Visible = false;
        //        gvSecurityHolding.Visible = false;
        //        return;
        //    }

        //    // Show the client name(s) on the web page.
        //    string clientName = "Client(s): ";
        //    int i = 1;
        //    foreach (DataRow row in dtClient.Rows)
        //    {
        //        clientName = clientName + row["lastName"] + ", " + row["firstName"];
        //        if (dtClient.Rows.Count != i)
        //        {
        //            clientName = clientName + "and ";
        //        }
        //        i = i + 1;
        //    }
        //    lblClientName.Text = clientName;
        //    lblClientName.Visible = true;

        //    // *****************************************************************************************************************************
        //    sql = "SELECT code, name, shares, base, '0.00' as price, '0.00' AS value, '0.00' AS convertedValue FROM dbo.SecurityHolding WHERE accountNumber='" + accountNumber + "' AND type='" + securityType + "'"; // Complete the SQL statement.

        //    DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
        //    if (dtSecurityHolding == null) { return; } // If the DataSet is null, a SQL error occurred.

        //    // If no result is returned, then display a message that the account does not hold this type of security.
        //    if (dtSecurityHolding.Rows.Count == 0)
        //    {
        //        lblResultMessage.Text = "No " + securityType + "s held in this account.";
        //        lblResultMessage.Visible = true;
        //        gvSecurityHolding.Visible = false;
        //        return;
        //    }

        //    // For each security in the result, get its current price from an external system, calculate the total value
        //    // of the security and change the current price and total value columns of the security in the result.
        //    int dtRow = 0;
        //    foreach (DataRow row in dtSecurityHolding.Rows)
        //    {
        //        string securityCode = row["code"].ToString();
        //        decimal shares = Convert.ToDecimal(row["shares"]);
        //        decimal price = myExternalFunctions.getSecuritiesPrice(securityType, securityCode);
        //        decimal value = Math.Round(shares * price - (decimal).005, 2);
        //        dtSecurityHolding.Rows[dtRow]["price"] = price;
        //        dtSecurityHolding.Rows[dtRow]["value"] = value;
        //        dtRow = dtRow + 1;
        //    }

        //    // Set the initial sort expression and sort direction for sorting the GridView in ViewState.
        //    ViewState["SortExpression"] = "name";
        //    ViewState["SortDirection"] = "ASC";

        //    // Bind the GridView to the DataTable.
        //    gvSecurityHolding.DataSource = dtSecurityHolding;
        //    gvSecurityHolding.DataBind();

        //    // Set the visibility of controls and GridView data.
        //    gvSecurityHolding.Visible = true;
        //    ddlCurrency.Visible = true;
        //    gvSecurityHolding.Columns[myHKeInvestCode.getColumnIndexByName(gvSecurityHolding, "convertedValue")].Visible = false;
        //}



    }
}