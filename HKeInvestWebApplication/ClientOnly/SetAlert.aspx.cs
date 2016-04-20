using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class SetAlert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cvSecurityCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string securityType = ddlSecurityType.SelectedItem.Text.Trim();
            char code = Convert.ToChar( SecurityCode.Text.Trim()); 

            
        }
    }
}