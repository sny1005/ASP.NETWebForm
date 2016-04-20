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
    public partial class SetAlert : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cvSecurityCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string securityType = ddlSecurityType.SelectedItem.Text.Trim();
            char code = Convert.ToChar( SecurityCode.Text.Trim());
            string sql = "";

            DataTable dtSecurity = myHKeInvestData.getData(sql);
            if (dtSecurity == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned by the SQL statement, then display a message.
            if (dtSecurity.Rows.Count == 0)
            {
                cvSecurityCode.ErrorMessage = "You do not own this security.";
            };
        }
    }
}