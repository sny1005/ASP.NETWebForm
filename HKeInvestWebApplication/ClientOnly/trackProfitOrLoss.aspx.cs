using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class trackProfitOrLoss : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void rbDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbDisplayType.SelectedIndex == 2) //individual security
            {
                ddlSecurityType.Visible = true;
                lblSecurityCode.Visible = true;
                txtSecurityCode.Visible = true;
                
            }

            else if(rbDisplayType.SelectedIndex == 1) //one type of security
            {
                ddlSecurityType.Visible = true;
                
            }
            else    //all
            {
                //get account number
                string userName = User.Identity.Name;
                string sql = "SELECT accountNumber FROM LoginAccount WHERE userName = '" + userName + "' ";
                DataTable dtClient = myHKeInvestData.getData(sql);
                string userAccountNumber = dtClient.Rows[0].ToString();

                //get buy amount
                sql = "SELECT executePrice, executeShares FROM Transaction WHERE orderNumber = (SELECT orderNumber FROM Order WHERE buyOrSell = buy AND status = executed AND accountNumber = '"+userAccountNumber+"')";
                DataTable dtBuy = myHKeInvestData.getData(sql);
                decimal totalBuyAmount = 0;
                foreach (DataRow row in dtBuy.Rows)
                {
                    totalBuyAmount = totalBuyAmount + (Convert.ToDecimal(row["executePrice"])*Convert.ToDecimal(row["executeShares"]));
                }

                //get sell amount
                sql = "SELECT executePrice, executeShares FROM Transaction WHERE orderNumber = (SELECT orderNumber FROM Order WHERE buyOrSell = sell AND status = executed AND accountNumber = '" + userAccountNumber + "')";
                DataTable dtSell = myHKeInvestData.getData(sql);
                decimal totalSellAmount = 0;
                foreach (DataRow row in dtSell.Rows)
                {
                    totalSellAmount = totalSellAmount + (Convert.ToDecimal(row["executePrice"]) * Convert.ToDecimal(row["executeShares"]));
                }

                //fee
                sql = "SELECT feeCharged FROM Order WHERE accountNumber = '" + userAccountNumber + "'";
                decimal totalFeeCharged = 0;
                DataTable dtFee = myHKeInvestData.getData(sql);
                foreach (DataRow row in dtFee.Rows)
                {
                    totalFeeCharged = totalFeeCharged + Convert.ToDecimal(row["feeCharged"]);
                }

                //current value
                sql = "SELECT type, code, shares FROM SecurityHolding WHERE accountNumber = '" + userAccountNumber + "'";
                DataTable dtValue = myHKeInvestData.getData(sql);
                decimal currentValue = 0;
                foreach(DataRow row in dtValue.Rows)
                {
                    currentValue = currentValue + Convert.ToDecimal(row["shares"]) * myExternalFunctions.getSecuritiesPrice(row["type"].ToString(), row["code"].ToString());
                }

                //view result
                DataTable source = new DataTable();
                source.Rows[0][0]=totalBuyAmount;
                source.Rows[0][1]=totalSellAmount;
                source.Rows[0][2] = totalFeeCharged;
                source.Rows[0][3] = currentValue + totalSellAmount - totalBuyAmount - totalFeeCharged;
                gvSecurity.DataSource = source;
                gvSecurity.Visible = true;
            }

        }

        protected void ddlSecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvIndividual_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

    }
}