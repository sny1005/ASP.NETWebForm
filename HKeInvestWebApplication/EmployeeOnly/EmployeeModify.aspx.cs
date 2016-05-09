using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication.EmployeeOnly
{
    public partial class EmployeeModify : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        string accountNumber;
        int i = 1;
        string last1 = "";
        string last2 = "";
 
        protected void Page_Load(object sender, EventArgs e)
        {
            //Primary1.Visible = false;
            //Primary2.Visible = false;
            //CoHolderPanel.Visible = false;
            lblResultMessage.Text = "";
            lblResultMessage.Visible = false;
            lblClientName.Visible = false;
        }

        protected void Check_Click(object sender, EventArgs e)
        {
            string sqlC = "";
            //// Get username
            //string username = User.Identity.Name;
            //string sql = "select accountNumber from LoginAccount where username ='" + username + "'";

            //DataTable dtclient = myHKeInvestData.getData(sql);
            //if (dtclient == null) { return; } // if the dataset is null, a sql error occurred.

            //// Get account number
            //foreach (DataRow row in dtclient.Rows)
            //{
            //    accountNumber = (string)row["accountNumber"];
            //}
            //lblAccountNumber.Text = "account number: " + accountNumber;
            //lblAccountNumber.Visible = true;

            // Get username
            //string userName = User.Identity.Name;
            accountNumber = txtAccountNumber.Text.Trim();
            sqlC = "SELECT lastName, firstName FROM Client WHERE accountNumber = '" + accountNumber+ "'"; 

            DataTable dtClient = myHKeInvestData.getData(sqlC);
            if (dtClient == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned by the SQL statement, then display a message.
            if (dtClient.Rows.Count == 0)
            {
                lblResultMessage.Text = "No such account number.";
                lblResultMessage.Visible = true;
                lblClientName.Visible = false;
                return;
            }

            // Show the client name(s) on the web page.
            string clientName = "Client(s): ";
            foreach (DataRow row in dtClient.Rows)
            {
                clientName = clientName + row["lastName"] + ", " + row["firstName"];
                if (last1 == "")
                {
                    last1 = last1 + row["lastName"];
                }
                else if (last2 == "")
                {
                    last2 = last2 + row["lastName"];
                }
                else
                {

                }
                if (dtClient.Rows.Count != i)
                {
                    clientName = clientName + "and ";
                    CoHolderPanel.Visible = true;
                    i = i + 1;
                }
            }
            lblClientName.Text = clientName;
            lblClientName.Visible = true;
            Primary1.Visible = true;
            Primary2.Visible = true;
        }

        protected void cvOccupation_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (EmpStatus.SelectedValue == "Employed")
            {
                if (args.Value == "") args.IsValid = false;
            }
        }

        protected void cvOccupation2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (EmpStatus2.SelectedValue == "Employed")
            {
                if (args.Value == "") args.IsValid = false;
            }
        }

        protected void cvSpecificSource_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (PrimarySource.SelectedValue == "other" && args.Value == "") args.IsValid = false;
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HKeInvestData myHKeInvest = new HKeInvestData();
                string acNo = txtAccountNumber.Text.Trim();
                string sql = "";
         
                sql = "SELECT lastName, firstName FROM Client WHERE accountNumber = '" + accountNumber + "'";

                DataTable dtClient = myHKeInvestData.getData(sql);
                if (dtClient == null) { return; } // If the DataSet is null, a SQL error occurred.

                // If no result is returned by the SQL statement, then display a message.
                if (dtClient.Rows.Count == 0)
                {
                    lblResultMessage.Text = "No such account number.";
                    lblResultMessage.Visible = true;
                    lblClientName.Visible = false;
                    return;
                }

                SqlTransaction myTransaction = myHKeInvest.beginTransaction();

                if (title.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET title = '" + title.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')" ;
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (FirstName.Text != "")
                {
                    sql = "UPDATE [Client] SET firstName = '" + FirstName.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (LastName.Text != "")
                {
                    sql = "UPDATE [Client] SET lastName = '" + LastName.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Citizen.Text != "")
                {
                    sql = "UPDATE [Client] SET citizenship = '" + Citizen.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Residence.Text != "")
                {
                    sql = "UPDATE [Client] SET residence = '" + Residence.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (HKID.Text != "")
                {
                    sql = "UPDATE [Client] SET HKIDPassportNumber = '" + HKID.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Email.Text != "")
                {
                    sql = "UPDATE [Client] SET email = '" + Email.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Building.Text != "")
                {
                    sql = "UPDATE [Client] SET building = '" + Building.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Street.Text != "")
                {
                    sql = "UPDATE [Client] SET street = '" + Street.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (District.Text != "")
                {
                    sql = "UPDATE [Client] SET district = '" + District.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (EmpStatus.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET employmentStatus = '" + EmpStatus.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (EmpByBroker.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET employByBroker = '" + EmpByBroker.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (CompanyDirector.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET publiclyTradedCompany = '" + CompanyDirector.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (PrimarySource.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET primarySourceFund = '" + PrimarySource.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Objective.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET investObjective = '" + Objective.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Knowledge.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET investKnowledge = '" + Knowledge.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Experience.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET investExperience = '" + Experience.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Income.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET annualIncome = '" + Income.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (NetWorth.SelectedValue != "")
                {
                    sql = "UPDATE [Client] SET liquidNetWorth = '" + NetWorth.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (Fund.Checked)
                {
                    sql = "UPDATE [Client] SET freeBalanceToFund = '" + Fund.Checked + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //insert phone numbers
                if (hPhone.Text != "")
                {
                    sql = "UPDATE [Client] SET homePhone = '" + hPhone.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (hFax.Text != "")
                {
                    sql = "UPDATE [Client] SET homeFax = '" + hFax.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (bPhone.Text != "")
                {
                    sql = "UPDATE [Client] SET businessPhone = '" + bPhone.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                if (bFax.Text != "")
                {
                    sql = "UPDATE [Client] SET businessFax = '" + bFax.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //insert employmeny information
                if (EmpStatus.SelectedValue == "Employed")
                {
                    sql = "UPDATE [Client] SET occupation = '" + Occupation.Text + "', yearsWithEmployer = '" + yrWithEmp.Text + "', employerName = '" + Employer.Text + "', employerPhone = '" + EmployerPhone.Text + "', businessNature = '" + Business.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //insert specific primary source fof fund
                if (PrimarySource.SelectedValue == "other")
                {
                    sql = "UPDATE [Client] SET specificSource = '" + SpecificSource.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last1 + "')";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //end of primary account holder infomation
                //myHKeInvest.commitTransaction(myTransaction);

                //INSERT CO HOLDER'S INFORMATION

                if (i != 1)
                {
                    //myTransaction = myHKeInvest.beginTransaction();
                    //get co-ac holder's clientNumber
                    sql = "SELECT clientNumber from Client WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";

                    if (title2.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET title = '" + title2.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (FirstName2.Text != "")
                    {
                        sql = "UPDATE [Client] SET firstName = '" + FirstName2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (LastName2.Text != "")
                    {
                        sql = "UPDATE [Client] SET lastName = '" + LastName2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Citizen2.Text != "")
                    {
                        sql = "UPDATE [Client] SET citizenship = '" + Citizen2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Residence2.Text != "")
                    {
                        sql = "UPDATE [Client] SET residence = '" + Residence2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (HKID2.Text != "")
                    {
                        sql = "UPDATE [Client] SET HKIDPassportNumber = '" + HKID2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Email2.Text != "")
                    {
                        sql = "UPDATE [Client] SET email = '" + Email2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Building2.Text != "")
                    {
                        sql = "UPDATE [Client] SET building = '" + Building2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Street2.Text != "")
                    {
                        sql = "UPDATE [Client] SET street = '" + Street2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (District2.Text != "")
                    {
                        sql = "UPDATE [Client] SET district = '" + District2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (EmpStatus2.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET employmentStatus = '" + EmpStatus2.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (EmpByBroker2.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET employByBroker = '" + EmpByBroker2.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (CompanyDirector2.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET publiclyTradedCompany = '" + CompanyDirector2.SelectedValue + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    // myHKeInvest.commitTransaction(myTransaction);       //need to commit transaction before being able to retreive information from the database
                    //insert phone numbers
                    if (hPhone2.Text != "")
                    {
                        sql = "UPDATE [Client] SET homePhone = '" + hPhone2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }
                    if (hFax2.Text != "")
                    {
                        sql = "UPDATE [Client] SET homeFax = '" + hFax2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }
                    if (bPhone2.Text != "")
                    {
                        sql = "UPDATE [Client] SET businessPhone = '" + bPhone2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }
                    if (bFax2.Text != "")
                    {
                        sql = "UPDATE [Client] SET businessFax = '" + bFax2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    //insert employmeny information
                    if (EmpStatus2.SelectedValue == "Employed")
                    {
                        sql = "UPDATE [Client] SET occupation = '" + Occupation2.Text + "', yearsWithEmployer = '" + yrWithEmp2.Text + "', employerName = '" + Employer2.Text + "', employerPhone = '" + EmployerPhone2.Text + "', businessNature = '" + Business2.Text + "' WHERE (accountNumber = '" + acNo + "' AND lastName = '" + last2 + "')";
                        myHKeInvest.setData(sql, myTransaction);
                    }


                    if (PrimarySource.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET primarySourceFund = '" + PrimarySource.SelectedValue + "' WHERE ((accountNumber = '" + acNo + "') AND (lastName = '" + last2 + "')) ";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Objective.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET investObjective = '" + Objective.SelectedValue + "' WHERE ((accountNumber = '" + acNo + "') AND (lastName = '" + last2 + "')) ";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Knowledge.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET investKnowledge = '" + Knowledge.SelectedValue + "' WHERE ((accountNumber = '" + acNo + "') AND (lastName = '" + last2 + "')) ";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Experience.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET investExperience = '" + Experience.SelectedValue + "' WHERE ((accountNumber = '" + acNo + "') AND (lastName = '" + last2 + "')) ";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Income.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET annualIncome = '" + Income.SelectedValue + "' WHERE ((accountNumber = '" + acNo + "') AND (lastName = '" + last2 + "')) ";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (NetWorth.SelectedValue != "")
                    {
                        sql = "UPDATE [Client] SET liquidNetWorth = '" + NetWorth.SelectedValue + "' WHERE ((accountNumber = '" + acNo + "') AND (lastName = '" + last2 + "')) ";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    if (Fund.Checked)
                    {
                        sql = "UPDATE [Client] SET freeBalanceToFund = '" + Fund.Checked + "' WHERE ((accountNumber = '" + acNo + "') AND (lastName = '" + last2 + "')) ";
                        myHKeInvest.setData(sql, myTransaction);
                    }
                }
                myHKeInvest.commitTransaction(myTransaction);
                lblResultMessage.Visible = true;
                lblResultMessage.Text = "Account info updated successfully!";
                return;
            }
            lblResultMessage.Visible = false;
        }
    }
}