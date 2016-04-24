using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using HKeInvestWebApplication.Code_File;

namespace HKeInvestWebApplication
{
    public partial class Email_Test : System.Web.UI.Page
    {


        protected void Button1_Click(object sender, EventArgs e)
        {
            HKeInvestCode myHkeInvestCode = new HKeInvestCode();
            myHkeInvestCode.sendemail("csunny95@yahoo.com.hk", "InvestProSystemTest", "Test");  

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

    }
}