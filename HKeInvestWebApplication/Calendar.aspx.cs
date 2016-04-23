using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HKeInvestWebApplication
{
    public partial class Calendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SelectedDatesCollection eventdates = Calendar1.SelectedDates;
            eventdates.Add(new DateTime(2016, 4, 24));
            eventdates.Add(new DateTime(2016, 4, 25));
            eventdates.Add(new DateTime(2016, 4, 26));

        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}