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
            DataTable eventinfo = myHKeInvestData.getData("Select year, month, day From Events");
            SelectedDatesCollection eventdates = Calendar1.SelectedDates;
            //for( int i=1; i<= eventid.Rows.Count; i++ )
            foreach(DataRow row in eventinfo.Rows)
            {
                /*DataTable year = myHKeInvestData.getData("Select year from Events where Eventid = '" + i + "'");
                DataTable month= myHKeInvestData.getData("Select month from Events where Eventid = '" + i + "'");
                DataTable day = myHKeInvestData.getData("Select day from Events where Eventid = '" + i + "'");*/
                string year_string = row[0].ToString();
                int year = Int32.Parse(year_string);
                string month_string = row[1].ToString();
                int month = Int32.Parse(month_string);
                string day_string = row[2].ToString();
                int day = Int32.Parse(year_string);

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