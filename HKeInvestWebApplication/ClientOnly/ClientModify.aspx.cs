using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class ClientModify : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        static string accountNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
            //get the account number of the current logged in user
            string username = User.Identity.Name;
            string sql = "select accountNumber from LoginAccount where username ='" + username + "'";

            DataTable dtclient = myHKeInvestData.getData(sql);
            if (dtclient == null) { return; } // if the dataset is null, a sql error occurred.

            foreach (DataRow row in dtclient.Rows)
            {
                accountNumber = (string)row["accountNumber"];
            }
            lblAccountNumber.Text = "account number: " + accountNumber;
            lblAccountNumber.Visible = true;
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
                string acNo = AccountNumber.Text;

                decimal initialBalance;
                if (Transfer.Checked && Cheque.Checked)         //both contain value
                    initialBalance = Convert.ToDecimal(TransferV.Text) + Convert.ToDecimal(ChequeV.Text);
                else if (Transfer.Checked)                      //tranfer contains value
                    initialBalance = Convert.ToDecimal(TransferV.Text);
                else                                            //only chequeV contain value
                    initialBalance = Convert.ToDecimal(ChequeV.Text);

                //the account will be successfully updated
                string sql = "INSERT INTO [Account] VALUES ('" + acNo + "', '" + acType.SelectedValue + "', " + initialBalance + ", NULL)";

                SqlTransaction myTransaction = myHKeInvest.beginTransaction();
                myHKeInvest.setData(sql, myTransaction);

                //insert all required fields first
                sql = "INSERT INTO [Client] (title, accountNumber, firstName, lastName, dateOfBirth, email, HKIDPassportNumber, citizenship, residence, building, street, district, employmentStatus, employByBroker, publiclyTradedCompany, primarySourceFund, investObjective, investKnowledge, investExperience, annualIncome, liquidNetWorth, freeBalanceToFund) ";
                sql = sql + "VALUES ('" + title.SelectedValue + "', '" + acNo + "', '" + FirstName.Text + "', '" + LastName.Text + "', CONVERT(date, '" + DateOfBirth.Text + "', 103), '" + Email.Text + "', '" + HKID.Text + "', '" + Citizen.Text + "', '" + Residence.Text + "', '" + Building.Text + "', '" + Street.Text + "', '" + District.Text + "', '" + EmpStatus.SelectedValue + "', '" + EmpByBroker.SelectedValue + "', '" + CompanyDirector.SelectedValue + "', '" + PrimarySource.SelectedValue + "', '" + Objective.SelectedValue + "', '" + Knowledge.SelectedValue + "', '" + Experience.SelectedValue + "', '" + Income.SelectedValue + "', '" + NetWorth.SelectedValue + "', '" + Fund.Checked + "')";
                myHKeInvest.setData(sql, myTransaction);

                //insert optional fields
                //insert passport number if needed
                if (ID_PP.SelectedValue == "Passport Number")
                {
                    sql = "UPDATE [Client] SET passportIssueCountry = '" + IssueCountry.Text + "' WHERE accountNumber = '" + acNo + "'";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //insert phone numbers
                if (hPhone.Text != "")
                {
                    sql = "UPDATE [Client] SET homePhone = '" + hPhone.Text + "' WHERE accountNumber = '" + acNo + "'";
                    myHKeInvest.setData(sql, myTransaction);
                }
                if (hFax.Text != "")
                {
                    sql = "UPDATE [Client] SET homeFax = '" + hFax.Text + "' WHERE accountNumber = '" + acNo + "'";
                    myHKeInvest.setData(sql, myTransaction);
                }
                if (bPhone.Text != "")
                {
                    sql = "UPDATE [Client] SET businessPhone = '" + bPhone.Text + "' WHERE accountNumber = '" + acNo + "'";
                    myHKeInvest.setData(sql, myTransaction);
                }
                if (bFax.Text != "")
                {
                    sql = "UPDATE [Client] SET businessFax = '" + bFax.Text + "' WHERE accountNumber = '" + acNo + "'";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //insert employmeny information
                if (EmpStatus.SelectedValue == "Employed")
                {
                    sql = "UPDATE [Client] SET occupation = '" + Occupation.Text + "', yearsWithEmployer = '" + yrWithEmp.Text + "', employerName = '" + Employer.Text + "', employerPhone = '" + EmployerPhone.Text + "', businessNature = '" + Business.Text + "' WHERE accountNumber = '" + acNo + "'";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //insert specific primary source fof fund
                if (PrimarySource.SelectedValue == "other")
                {
                    sql = "UPDATE [Client] SET specificSource = '" + SpecificSource.Text + "' WHERE accountNumber = '" + acNo + "'";
                    myHKeInvest.setData(sql, myTransaction);
                }

                //end of primary account holder infomation
                myHKeInvest.commitTransaction(myTransaction);


                //
                //INSERT CO HOLDER'S INFORMATION
                //
                if (acType.SelectedIndex != 0)
                {
                    myTransaction = myHKeInvest.beginTransaction();

                    //insert all required fields first
                    sql = "INSERT INTO [Client] (title, accountNumber, firstName, lastName, dateOfBirth, email, HKIDPassportNumber, citizenship, residence, building, street, district, employmentStatus, employByBroker, publiclyTradedCompany, primarySourceFund, investObjective, investKnowledge, investExperience, annualIncome, liquidNetWorth, freeBalanceToFund) ";
                    sql = sql + "VALUES ('" + title2.SelectedValue + "', '" + acNo + "', '" + FirstName2.Text + "', '" + LastName2.Text + "', CONVERT(date, '" + DateOfBirth2.Text + "', 103), '" + Email2.Text + "', '" + HKID2.Text + "', '" + Citizen2.Text + "', '" + Residence2.Text + "', '" + Building2.Text + "', '" + Street2.Text + "', '" + District2.Text + "', '" + EmpStatus2.SelectedValue + "', '" + EmpByBroker2.SelectedValue + "', '" + CompanyDirector2.SelectedValue + "', '" + PrimarySource.SelectedValue + "', '" + Objective.SelectedValue + "', '" + Knowledge.SelectedValue + "', '" + Experience.SelectedValue + "', '" + Income.SelectedValue + "', '" + NetWorth.SelectedValue + "', '" + Fund.Checked + "')";
                    myHKeInvest.setData(sql, myTransaction);
                    myHKeInvest.commitTransaction(myTransaction);       //need to commit transaction before being able to retreive information from the database


                    //get co-ac holder's clientNumber
                    sql = "SELECT clientNumber from Client WHERE accountNumber = '" + acNo + "' AND firstName = '" + FirstName2.Text + "'";
                    DataTable dtClient = myHKeInvest.getData(sql);
                    string cNo = "";
                    foreach (DataRow row in dtClient.Rows)
                    {
                        cNo = Convert.ToString(row["clientNumber"]);
                    }

                    myTransaction = myHKeInvest.beginTransaction();
                    //insert optional fields
                    //insert passport number if needed
                    if (ID_PP2.SelectedValue == "Passport Number")
                    {
                        sql = "UPDATE [Client] SET passportIssueCountry = '" + IssueCountry2.Text + "' WHERE clientNumber = '" + cNo + "'";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    //insert phone numbers
                    if (hPhone2.Text != "")
                    {
                        sql = "UPDATE [Client] SET homePhone = '" + hPhone2.Text + "' WHERE clientNumber = '" + cNo + "'";
                        myHKeInvest.setData(sql, myTransaction);
                    }
                    if (hFax2.Text != "")
                    {
                        sql = "UPDATE [Client] SET homeFax = '" + hFax2.Text + "' WHERE clientNumber = '" + cNo + "'";
                        myHKeInvest.setData(sql, myTransaction);
                    }
                    if (bPhone2.Text != "")
                    {
                        sql = "UPDATE [Client] SET businessPhone = '" + bPhone2.Text + "' WHERE clientNumber = '" + cNo + "'";
                        myHKeInvest.setData(sql, myTransaction);
                    }
                    if (bFax2.Text != "")
                    {
                        sql = "UPDATE [Client] SET businessFax = '" + bFax2.Text + "' WHERE clientNumber = '" + cNo + "'";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    //insert employmeny information
                    if (EmpStatus2.SelectedValue == "Employed")
                    {
                        sql = "UPDATE [Client] SET occupation = '" + Occupation2.Text + "', yearsWithEmployer = '" + yrWithEmp2.Text + "', employerName = '" + Employer2.Text + "', employerPhone = '" + EmployerPhone2.Text + "', businessNature = '" + Business2.Text + "' WHERE clientNumber = '" + cNo + "'";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    //insert specific primary source fof fund
                    if (PrimarySource.SelectedValue == "other")
                    {
                        sql = "UPDATE [Client] SET specificSource = '" + SpecificSource.Text + "' WHERE clientNumber = '" + cNo + "'";
                        myHKeInvest.setData(sql, myTransaction);
                    }

                    //END of optional fields for co-ac holder
                    myHKeInvest.commitTransaction(myTransaction);
                }
            }
        }

        protected void acType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (acType.SelectedIndex == 0)
                CoHolderPanel.Visible = false;
            else
                CoHolderPanel.Visible = true;
        }
    }








}
}