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

        protected void cvSecurityCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string securityType = ddlSecurityType.SelectedValue;

            //check if such security exists
            if (!myHKeInvestCode.securityCodeIsValid(securityType, args.Value))
            {
                cvSecurityCode.ErrorMessage = "Security code invalid";
                args.IsValid = false;
                return;
            }

            string sql = "SELECT code FROM securityHolding WHERE type = '" + securityType + "' AND accountNumber = '" + accountNumber + "' AND code = '" + args.Value + "'";
           
            DataTable dtSecurity = myHKeInvestData.getData(sql);
            if (dtSecurity == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned by the SQL statement, then display a message.
            if (dtSecurity.Rows.Count == 0)
            {
                args.IsValid = false;
                cvSecurityCode.ErrorMessage = "You do not own this security.";
                return;
            }
        }

        protected void cvAlertValue_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal value;
            if (!decimal.TryParse(args.Value, out value)) { return; }

            string alertType = AlertType_RadioButtonList.SelectedValue;
            string securityType = ddlSecurityType.SelectedValue;
            string code = SecurityCode.Text.Trim();
            
            

            string sql = "SELECT [value] FROM [Alert] WHERE alertType <> '" + alertType + "' AND type = '" + securityType + "' AND accountNumber = '" + accountNumber + "' AND code = '" + code + "'";
            DataTable dtAlert = myHKeInvestData.getData(sql);

            if (dtAlert.Rows.Count == 0 || dtAlert == null) { return; }
            //if itself is low value
            if (alertType == "lowValue")
            {
                if( (decimal)dtAlert.Rows[0]["value"] < value)
                {
                    cvAlertValue.ErrorMessage = "Low value is greater than high value";
                    args.IsValid = false;
                    return;
                }
            }
            else
            {
                if ((decimal)dtAlert.Rows[0]["value"] > value)
                {
                    cvAlertValue.ErrorMessage = "High value is smaller than low value";
                    args.IsValid = false;
                    return;
                }
            }
        }
        
        // any validation for only 1 high and 1 low for a security?
        protected void cvSet_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string alerttype = AlertType_RadioButtonList.SelectedValue;
            string sql = "SELECT alertType FROM Alert WHERE alertType = '" + alerttype + "' AND type = '" + ddlSecurityType.SelectedValue + "' AND accountNumber = '" + accountNumber + "' AND code = '" + SecurityCode.Text + "'";
            DataTable dtAlert = myHKeInvestData.getData(sql);

            if(dtAlert == null) { return; }
            if (dtAlert.Rows.Count == 1)
            {
                args.IsValid = false;
                cvSet.ErrorMessage = "You have already set a " + AlertType_RadioButtonList.SelectedItem.Text.Trim() + " for this security.";
                return;
            }
        }

        protected void Set_Click(object sender, EventArgs e)
        {
            lblmsg.Visible = false;

            if (Page.IsValid)
            {
                string alerttype = AlertType_RadioButtonList.SelectedValue;
                string securityType = ddlSecurityType.SelectedValue;
                string input = SecurityCode.Text.Trim();
                string value = AlertValue.Text.Trim();

                string sql = "SELECT alertType FROM Alert WHERE accountNumber = '" + accountNumber + "'";
                DataTable dtAlert = myHKeInvestData.getData(sql);

                foreach (DataRow row in dtAlert.Rows)
                {
                    if (row["alertType"].ToString() == alerttype)
                        return;
                }

                string sql2 = "INSERT INTO [Alert] ([accountNumber], [type], [code], [alertType], [value], [dateOfTrigger], [lastUpdate]) VALUES ('" + accountNumber + "', '" + securityType + "', '" + input + "', '" + alerttype + "', " + value + ", NULL, NULL)";
                SqlTransaction trans = myHKeInvestData.beginTransaction();
                myHKeInvestData.setData(sql2, trans);
                myHKeInvestData.commitTransaction(trans);
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (SecurityCode.Text.Trim() != "")
            {
                string alerttype = AlertType_RadioButtonList.SelectedValue;
                string securityType = ddlSecurityType.SelectedValue;
                string code = SecurityCode.Text.Trim();

                string sql = "DELETE FROM Alert WHERE accountNumber = '" + accountNumber + "' AND type = '" + securityType + "' AND code = '" + code + "' AND alerttype = '" + alerttype + "'";

                SqlTransaction trans = myHKeInvestData.beginTransaction();
                myHKeInvestData.setData(sql, trans);
                myHKeInvestData.commitTransaction(trans);
                return;
            }
            lblmsg.Visible = true;
            lblmsg.Text = "SecurityCode is required";
        }
    }
}