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
            StockGV.Visible = false;
            BondGV.Visible = false;
            UTGV.Visible = false;
        }

        protected void BondGV_Sorting(object sender, GridViewSortEventArgs e)
        {
            switch (e.SortExpression)
            {
                case "code":
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        //BondGV.DataSource = // Asc query 
                        //BondGV.DataBind();
                    }
                    else
                    {
                        //BondGV.DataSource = // Desc query 
                        //BondGV.DataBind();
                    }
                    break;

                case "name":
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        //BondGV.DataSource = // Asc query 
                        //BondGV.DataBind();
                    }
                    else
                    {
                        //BondGV.DataSource = // Desc query 
                        //BondGV.DataBind();
                    }
                    break;
            }
        }


        protected void Search_Click(object sender, EventArgs e)
        {

            // all bond
            if (ddlSecurityType.SelectedValue == "bond" && (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == true))
            {
                DataTable dtbond = myExternalFunctions.getSecuritiesData("bond");
                BondGV.DataSource = dtbond;
                BondGV.DataBind();
                BondGV.Visible = true;
                return;
            }

            // all stock
            if (ddlSecurityType.SelectedValue == "stock" && (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == true))
            {
                DataTable dtstock = myExternalFunctions.getSecuritiesData("stock");
                StockGV.DataSource = dtstock;
                StockGV.DataBind();
                StockGV.Visible = true;
                return;
            }

            // all unit trust
            if (ddlSecurityType.SelectedValue == "unit trust" && (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == true))
            {
                DataTable dtUT = myExternalFunctions.getSecuritiesData("unit trust");
                UTGV.DataSource = dtUT;
                UTGV.DataBind();
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
                    BondGV.DataSource = dtBondB;
                    BondGV.DataBind();
                    BondGV.Visible = true;
                    return;
                }
                else if (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtBondC = myExternalFunctions.getSecuritiesByCode("bond", input);
                    BondGV.DataSource = dtBondC;
                    BondGV.DataBind();
                    BondGV.Visible = true;
                    return;
                }
                else
                {
                    string input = SecName.Text.Trim();
                    DataTable dtBondN = myExternalFunctions.getSecuritiesByName("bond", input);
                    BondGV.DataSource = dtBondN;
                    BondGV.DataBind();
                    BondGV.Visible = true;
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
                    StockGV.DataSource = dtStockB;
                    StockGV.DataBind();
                    StockGV.Visible = true;
                    return;
                }
                else if (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtStockC = myExternalFunctions.getSecuritiesByCode("stock", input);
                    StockGV.DataSource = dtStockC;
                    StockGV.DataBind();
                    StockGV.Visible = true;
                    return;
                }
                else
                {
                    string input = SecName.Text.Trim();
                    DataTable dtStockN = myExternalFunctions.getSecuritiesByName("stock", input);
                    StockGV.DataSource = dtStockN;
                    StockGV.DataBind();
                    StockGV.Visible = true;
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
                    UTGV.DataSource = dtUTB;
                    UTGV.DataBind();
                    UTGV.Visible = true;
                    return;
                }
                else if (string.IsNullOrEmpty(SecName.Text) == true && string.IsNullOrEmpty(SecCode.Text) == false)
                {
                    string input = SecCode.Text.Trim();
                    DataTable dtUTC = myExternalFunctions.getSecuritiesByCode("unit trust", input);
                    UTGV.DataSource = dtUTC;
                    UTGV.DataBind();
                    UTGV.Visible = true;
                    return;
                }
                else
                {
                    string input = SecName.Text.Trim();
                    DataTable dtUTN = myExternalFunctions.getSecuritiesByName("unit trust", input);
                    UTGV.DataSource = dtUTN;
                    UTGV.DataBind();
                    UTGV.Visible = true;
                    return;
                }
            }
        }
    }
}