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
    public partial class Calendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HKeInvestData myHKeInvestData = new HKeInvestData();
            DataTable eventinfo = myHKeInvestData.getData("Select year, month, day From Event");
            SelectedDatesCollection eventdates = Calendar1.SelectedDates;
            foreach(DataRow row in eventinfo.Rows)
            {
                string year_string = row[0].ToString();
                int year = Int32.Parse(year_string);
                string month_string = row[1].ToString();
                int month = Int32.Parse(month_string);
                string day_string = row[2].ToString();
                int day = Int32.Parse(day_string);

                eventdates.Add(new DateTime(year,month,day));

            }
            


        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}