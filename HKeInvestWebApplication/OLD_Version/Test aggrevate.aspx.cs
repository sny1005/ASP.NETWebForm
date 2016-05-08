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
            decimal count = 10;
            string result = count.ToString().PadLeft(8, '0');
            Label1.Text = result;
        }
    }
}