using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;
using System.Data.SqlClient;
using System.Data;

namespace HKeInvestWebApplication.EmployeeOnly
{
    public partial class AddEvents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = eventname.Text;
            string region = eventregion.Text;
            string date = eventdate.Text;
            string desc = description.Text;
            string y = year.Text;
            string m = month.Text;
            string d = day.Text;

            HKeInvestData myHKeInvestData = new HKeInvestData();
            SqlTransaction trans = myHKeInvestData.beginTransaction();
            myHKeInvestData.setData("INSERT INTO Event VALUES('" + name + "','" + region + "','" + date + "','" + desc + "','" + y + "','" + m + "','" + d + "')",trans);
            myHKeInvestData.commitTransaction(trans);
        }
    }
}