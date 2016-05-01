using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class SetAlert : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        static string accountNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
            accountNumber = myHKeInvestCode.getAccountNumber(User.Identity.Name);
        }

        // any validation for only 1 high and 1 low for a security?
        protected void cvSecurityCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string securityType = ddlSecurityType.SelectedItem.Text.Trim();
            string input = SecurityCode.Text.Trim();
            string sql = "SELECT code FROM securityHolding WHERE type = '" + securityType + "' AND accountNumber = '" + accountNumber + "'";
           
            DataTable dtSecurity = myHKeInvestData.getData(sql);
            if (dtSecurity == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned by the SQL statement, then display a message.
            if (dtSecurity.Rows.Count == 0)
            {
                cvSecurityCode.ErrorMessage = "You do not own this security.";
                return;
            }
            else
            {
                args.IsValid = true;
                return;
            }
        }

        protected void cvSet_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string alerttype = AlertType_RadioButtonList.SelectedItem.Text.Trim();
            string sql = "SELECT alertType FROM Alert WHERE accountNumber = '" + accountNumber + "'";
            DataTable dtAlert = myHKeInvestData.getData(sql);
            args.IsValid = false;
            foreach (DataRow row in dtAlert.Rows)
            {
                if (row["alertType"].ToString() == alerttype)
                {
                    args.IsValid = true;
                    return;
                }
            }
        }

        protected void Set_Click(object sender, EventArgs e)
        {
            string alerttype = AlertType_RadioButtonList.SelectedItem.Text.Trim();
            string securityType = ddlSecurityType.SelectedItem.Text.Trim();
            string input = SecurityCode.Text.Trim();
            string value = AlertValue.Text.Trim();

            string sql = "SELECT alertType FROM Alert WHERE accountNumber = '" + accountNumber + "'";
            DataTable dtAlert = myHKeInvestData.getData(sql);
           
            foreach (DataRow row in dtAlert.Rows)
            {
                if (row["alertType"].ToString() == alerttype)
                    return;
            }

            string sql2 = "INSERT INTO [Alert] VALUES ('" + accountNumber + "', '" + alerttype + "', '" + securityType + "', '" + input + "', " + value + ", NULL, NULL)";
            SqlTransaction trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData(sql2, trans);
        }
    }
}