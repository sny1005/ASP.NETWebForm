using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace HKeInvestWebApplication
{
    public partial class Email_Test : System.Web.UI.Page
    {


        protected void Button1_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");
            mail.From = new MailAddress("lychowaa@cse.ust.hk", "<Sunny Chow>");
            mail.To.Add("csunny95@yahoo.com.hk");
            mail.Subject = "Test";
            mail.Body = "Hellow World";
            emailServer.Send(mail);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

    }
}