using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Data;

namespace HKeInvestWebApplication
{
    public partial class SearchSecurities1 : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BondGV.Visible = false;
                StockGV.Visible = false;
                UTGV.Visible = false;
            }
        }

        protected void BondGV_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSecurityHolding = myHKeInvestCode.unloadGridView(BondGV);
            string sortExpression = e.SortExpression.ToLower();
            ViewState["SortExpression"] = sortExpression;
            dtSecurityHolding.DefaultView.Sort = sortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dtSecurityHolding.AcceptChanges();
            BondGV.DataSource = dtSecurityHolding.DefaultView;
            BondGV.DataBind();
        }

        protected void StockGV_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSecurityHolding = myHKeInvestCode.unloadGridView(StockGV);
            string sortExpression = e.SortExpression.ToLower();
            ViewState["SortExpression"] = sortExpression;
            dtSecurityHolding.DefaultView.Sort = sortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dtSecurityHolding.AcceptChanges();
            StockGV.DataSource = dtSecurityHolding.DefaultView;
            StockGV.DataBind();
        }

        protected void UTGV_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtSecurityHolding = myHKeInvestCode.unloadGridView(UTGV);
            string sortExpression = e.SortExpression.ToLower();
            ViewState["SortExpression"] = sortExpression;
            dtSecurityHolding.DefaultView.Sort = sortExpression + " " + myHKeInvestCode.getSortDirection(ViewState, e.SortExpression);
            dtSecurityHolding.AcceptChanges();
            UTGV.DataSource = dtSecurityHolding.DefaultView;
            UTGV.DataBind();
        }

        protected void Search_Click(object sender, EventArgs e)
        {

            // all bond
            if (ddlSecurityType.SelectedValue == "bond" && (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == true))
            {
                DataTable dtbond = myExternalFunctions.getSecuritiesData("bond");
                foreach (DataRow row in dtbond.Rows)
                {
                    if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                    if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                    if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                    if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                    if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                    if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                }

                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "DESC";

                BondGV.DataSource = dtbond;
                BondGV.DataBind();
                BondGV.Sort("name", SortDirection.Descending);
                BondGV.Visible = true;
                StockGV.Visible = false;
                UTGV.Visible = false;
                return;
            }

            // all stock
            if (ddlSecurityType.SelectedValue == "stock" && (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == true))
            {
                DataTable dtstock = myExternalFunctions.getSecuritiesData("stock");
                foreach (DataRow row in dtstock.Rows)
                {
                    if (row["changeDollar"].ToString().Trim() == "" || row["changeDollar"] == null) row["changeDollar"] = (decimal)0.00;
                    if (row["changePercent"].ToString().Trim() == "" || row["changePercent"] == null) row["changePercent"] = (decimal)0.00;
                    if (row["volume"].ToString().Trim() == "" || row["volume"] == null) row["volume"] = (decimal)0.00;
                    if (row["high"].ToString().Trim() == "" || row["high"] == null) row["high"] = (decimal)0.00;
                    if (row["low"].ToString().Trim() == "" || row["low"] == null) row["low"] = (decimal)0.00;
                    if (row["peRatio"].ToString().Trim() == "" || row["peRatio"] == null) row["peRatio"] = (decimal)0.00;
                    if (row["yield"].ToString().Trim() == "" || row["yield"] == null) row["yield"] = (decimal)0.00;
                }

                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "DESC";

                StockGV.DataSource = dtstock;
                StockGV.DataBind();
                StockGV.Sort("name", SortDirection.Descending);
                BondGV.Visible = false;
                StockGV.Visible = true;
                UTGV.Visible = false;
                return;
            }

            // all unit trust
            if (ddlSecurityType.SelectedValue == "unit trust" && (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == true))
            {
                DataTable dtUT = myExternalFunctions.getSecuritiesData("unit trust");
                foreach (DataRow row in dtUT.Rows)
                {
                    if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                    if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                    if (row["riskReturn"].ToString().Trim() == "" || row["riskReturn"] == null) row["riskReturn"] = (decimal)0.00;
                    if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                    if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                    if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                    if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                }

                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "DESC";

                UTGV.DataSource = dtUT;
                UTGV.DataBind();
                UTGV.Sort("name", SortDirection.Descending);
                BondGV.Visible = false;
                StockGV.Visible = false;
                UTGV.Visible = true;
                return;
            }

            //bond case
            if (ddlSecurityType.SelectedValue == "bond")
            {
                if (string.IsNullOrEmpty(SecName.Text) == false && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtBondB = myExternalFunctions.getSecuritiesByCode("bond", input);
                    foreach (DataRow row in dtBondB.Rows)
                    {
                        if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                        if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                        if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                        if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                        if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                        if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    BondGV.DataSource = dtBondB;
                    BondGV.DataBind();
                    BondGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = true;
                    StockGV.Visible = false;
                    UTGV.Visible = false;
                    return;
                }
                else if (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtBondC = myExternalFunctions.getSecuritiesByCode("bond", input);
                    foreach (DataRow row in dtBondC.Rows)
                    {
                        if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                        if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                        if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                        if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                        if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                        if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    BondGV.DataSource = dtBondC;
                    BondGV.DataBind();
                    BondGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = true;
                    StockGV.Visible = false;
                    UTGV.Visible = false;
                    return;
                }
                else
                {
                    string input = SecName.Text.Trim();
                    DataTable dtBondN = myExternalFunctions.getSecuritiesByName("bond", input);
                    foreach (DataRow row in dtBondN.Rows)
                    {
                        if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                        if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                        if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                        if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                        if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                        if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    BondGV.DataSource = dtBondN;
                    BondGV.DataBind();
                    BondGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = true;
                    StockGV.Visible = false;
                    UTGV.Visible = false;
                    return;
                }
            }


            //stock case
            if (ddlSecurityType.SelectedValue == "stock")
            {
                if (string.IsNullOrEmpty(SecName.Text) == false && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtStockB = myExternalFunctions.getSecuritiesByCode("stock", input);
                    foreach (DataRow row in dtStockB.Rows)
                    {
                        if (row["changeDollar"].ToString().Trim() == "" || row["changeDollar"] == null) row["changeDollar"] = (decimal)0.00;
                        if (row["changePercent"].ToString().Trim() == "" || row["changePercent"] == null) row["changePercent"] = (decimal)0.00;
                        if (row["volume"].ToString().Trim() == "" || row["volume"] == null) row["volume"] = (decimal)0.00;
                        if (row["high"].ToString().Trim() == "" || row["high"] == null) row["high"] = (decimal)0.00;
                        if (row["low"].ToString().Trim() == "" || row["low"] == null) row["low"] = (decimal)0.00;
                        if (row["peRatio"].ToString().Trim() == "" || row["peRatio"] == null) row["peRatio"] = (decimal)0.00;
                        if (row["yield"].ToString().Trim() == "" || row["yield"] == null) row["yield"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    StockGV.DataSource = dtStockB;
                    StockGV.DataBind();
                    StockGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = false;
                    StockGV.Visible = true;
                    UTGV.Visible = false;
                    return;
                }
                else if (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtStockC = myExternalFunctions.getSecuritiesByCode("stock", input);
                    foreach (DataRow row in dtStockC.Rows)
                    {
                        if (row["changeDollar"].ToString().Trim() == "" || row["changeDollar"] == null) row["changeDollar"] = (decimal)0.00;
                        if (row["changePercent"].ToString().Trim() == "" || row["changePercent"] == null) row["changePercent"] = (decimal)0.00;
                        if (row["volume"].ToString().Trim() == "" || row["volume"] == null) row["volume"] = (decimal)0.00;
                        if (row["high"].ToString().Trim() == "" || row["high"] == null) row["high"] = (decimal)0.00;
                        if (row["low"].ToString().Trim() == "" || row["low"] == null) row["low"] = (decimal)0.00;
                        if (row["peRatio"].ToString().Trim() == "" || row["peRatio"] == null) row["peRatio"] = (decimal)0.00;
                        if (row["yield"].ToString().Trim() == "" || row["yield"] == null) row["yield"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    StockGV.DataSource = dtStockC;
                    StockGV.DataBind();
                    StockGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = false;
                    StockGV.Visible = true;
                    UTGV.Visible = false;
                    return;
                }
                else
                {
                    string input = SecName.Text.Trim();
                    DataTable dtStockN = myExternalFunctions.getSecuritiesByName("stock", input);
                    foreach (DataRow row in dtStockN.Rows)
                    {
                        if (row["changeDollar"].ToString().Trim() == "" || row["changeDollar"] == null) row["changeDollar"] = (decimal)0.00;
                        if (row["changePercent"].ToString().Trim() == "" || row["changePercent"] == null) row["changePercent"] = (decimal)0.00;
                        if (row["volume"].ToString().Trim() == "" || row["volume"] == null) row["volume"] = (decimal)0.00;
                        if (row["high"].ToString().Trim() == "" || row["high"] == null) row["high"] = (decimal)0.00;
                        if (row["low"].ToString().Trim() == "" || row["low"] == null) row["low"] = (decimal)0.00;
                        if (row["peRatio"].ToString().Trim() == "" || row["peRatio"] == null) row["peRatio"] = (decimal)0.00;
                        if (row["yield"].ToString().Trim() == "" || row["yield"] == null) row["yield"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    StockGV.DataSource = dtStockN;
                    StockGV.DataBind();
                    StockGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = false;
                    StockGV.Visible = true;
                    UTGV.Visible = false;
                    return;
                }
            }

            //unit trust case
            if (ddlSecurityType.SelectedValue == "unit trust")
            {
                if (string.IsNullOrEmpty(SecName.Text) == false && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtUTB = myExternalFunctions.getSecuritiesByCode("unit trust", input);
                    foreach (DataRow row in dtUTB.Rows)
                    {
                        if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                        if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                        if (row["riskReturn"].ToString().Trim() == "" || row["riskReturn"] == null) row["riskReturn"] = (decimal)0.00;
                        if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                        if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                        if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                        if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    UTGV.DataSource = dtUTB;
                    UTGV.DataBind();
                    UTGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = false;
                    StockGV.Visible = false;
                    UTGV.Visible = true;
                    return;
                }
                else if (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtUTC = myExternalFunctions.getSecuritiesByCode("unit trust", input);
                    foreach (DataRow row in dtUTC.Rows)
                    {
                        if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                        if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                        if (row["riskReturn"].ToString().Trim() == "" || row["riskReturn"] == null) row["riskReturn"] = (decimal)0.00;
                        if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                        if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                        if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                        if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    UTGV.DataSource = dtUTC;
                    UTGV.DataBind();
                    UTGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = false;
                    StockGV.Visible = false;
                    UTGV.Visible = true;
                    return;
                }
                else
                {
                    string input = SecName.Text.Trim();
                    DataTable dtUTN = myExternalFunctions.getSecuritiesByName("unit trust", input);
                    foreach (DataRow row in dtUTN.Rows)
                    {
                        if (row["size"].ToString().Trim() == "" || row["size"] == null) row["size"] = (decimal)0.00;
                        if (row["price"].ToString().Trim() == "" || row["price"] == null) row["price"] = (decimal)0.00;
                        if (row["riskReturn"].ToString().Trim() == "" || row["riskReturn"] == null) row["riskReturn"] = (decimal)0.00;
                        if (row["sixMonths"].ToString().Trim() == "" || row["sixMonths"] == null) row["sixMonths"] = (decimal)0.00;
                        if (row["oneYear"].ToString().Trim() == "" || row["oneYear"] == null) row["oneYear"] = (decimal)0.00;
                        if (row["threeYears"].ToString().Trim() == "" || row["threeYears"] == null) row["threeYears"] = (decimal)0.00;
                        if (row["sinceLaunch"].ToString().Trim() == "" || row["sinceLaunch"] == null) row["sinceLaunch"] = (decimal)0.00;
                    }

                    ViewState["SortExpression"] = "name";
                    ViewState["SortDirection"] = "DESC";

                    UTGV.DataSource = dtUTN;
                    UTGV.DataBind();
                    UTGV.Sort("name", SortDirection.Descending);
                    BondGV.Visible = false;
                    StockGV.Visible = false;
                    UTGV.Visible = true;
                    return;
                }
            }
        }
    }
}