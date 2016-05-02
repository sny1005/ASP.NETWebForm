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

        protected void Page_Load(object sender, EventArgs e){
            StockGV.Visible = false;
            BondGV.Visible = false;
            UnitTrustGV.Visible = false;
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            // ALL Bond
            if (ddlSecurityType.SelectedValue == "bond" && SecName.Text == null && SecCode.Text == null)
            {
                DataTable dtBond = myExternalFunctions.getSecuritiesData("bond");
                BondGV.DataSource = dtBond;
                BondGV.DataBind();
                BondGV.Visible = true;
            }

            // ALL Stock
            if (ddlSecurityType.SelectedValue == "stock" && SecName.Text == null && SecCode.Text == null)
            {
                DataTable dtStock = myExternalFunctions.getSecuritiesData("stock");
                StockGV.DataSource = dtStock;
                StockGV.DataBind();
                StockGV.Visible = true;
            }

            // ALL Unit Trust
            if (ddlSecurityType.SelectedValue == "unit trust" && SecName.Text == null && SecCode.Text == null)
            {
                DataTable dtUT = myExternalFunctions.getSecuritiesData("unit trust");
                UnitTrustGV.DataSource = dtUT;
                UnitTrustGV.DataBind();
                UnitTrustGV.Visible = true;
            }

            // Bond with Code
            if (ddlSecurityType.SelectedItem.Value == "bond" && SecName.Text == null && SecCode.Text != null)
            {
                string input = SecCode.Text.Trim();
                DataTable dtBondC = myExternalFunctions.getSecuritiesByCode("bond", input);
                BondGV.DataSource = dtBondC;
                BondGV.DataBind();
                BondGV.Visible = true;
            }

            // Stock with Code
            if (ddlSecurityType.SelectedValue == "stock" && SecName.Text == null && SecCode.Text != null)
            {
                string input = SecCode.Text.Trim();
                DataTable dtStockC = myExternalFunctions.getSecuritiesByCode("stock", input);
                StockGV.DataSource = dtStockC;
                StockGV.DataBind();
                StockGV.Visible = true;
            }

            // Unit Trust with Code
            if (ddlSecurityType.SelectedValue == "unit trust" && SecName.Text == null && SecCode.Text != null)
            {
                string input = SecCode.Text.Trim();
                DataTable dtUTC = myExternalFunctions.getSecuritiesByCode("unit trust", input);
                UnitTrustGV.DataSource = dtUTC;
                UnitTrustGV.DataBind();
                UnitTrustGV.Visible = true;
            }

            // Bond with Name
            if (ddlSecurityType.SelectedItem.Value == "bond" && SecName.Text != null)
            {
                string input = SecName.Text.Trim();
                DataTable dtBondN = myExternalFunctions.getSecuritiesByName("bond", input);
                BondGV.DataSource = dtBondN;
                BondGV.DataBind();
                BondGV.Visible = true;
            }

            // Stock with Name
            if (ddlSecurityType.SelectedValue == "stock" && SecName.Text != null)
            {
                string input = SecName.Text.Trim();
                DataTable dtStockN = myExternalFunctions.getSecuritiesByName("stock", input);
                StockGV.DataSource = dtStockN;
                StockGV.DataBind();
                StockGV.Visible = true;
            }

            // Unit Trust with Name
            if (ddlSecurityType.SelectedValue == "unit trust" && SecName.Text != null)
            {
                string input = SecName.Text.Trim();
                DataTable dtUTN = myExternalFunctions.getSecuritiesByName("unit trust", input);
                UnitTrustGV.DataSource = dtUTN;
                UnitTrustGV.DataBind();
                UnitTrustGV.Visible = true;
            }
        }
    }
}