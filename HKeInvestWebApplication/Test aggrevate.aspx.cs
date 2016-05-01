using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HKeInvestWebApplication.Code_File;


namespace HKeInvestWebApplication
{
    public partial class Test_aggrevate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            decimal count = myHKeInvestData.getAggregateValue("Select count(*) From [Client]");
            Label1.Text = count.ToString();
        }
    }
}