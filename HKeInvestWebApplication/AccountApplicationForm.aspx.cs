using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using HKeInvestWebApplication.Code_File;
using System.Data.SqlClient;
using System.Data;


namespace HKeInvestWebApplication
{
    public partial class ApplicationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cvAcNo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string acn = AccountNumber.Text.Trim();
            string lastname = LastName.Text.Trim();
            lastname = lastname.ToUpper();

            if (lastname.Length > 1)
            {
                lastname = lastname.Substring(0, 2);
                if (!acn.Contains(lastname))
                {
                    args.IsValid = false;
                    cvAcNo.ErrorMessage = "Account Number must start with the first two letters of your Last Name in uppercase.";
                    return;
                }
            }
            else
            {
                if (acn[0] != acn[1] || acn[0] != lastname[0])
                {
                    args.IsValid = false;
                    cvAcNo.ErrorMessage = "As your last name contains only 1 character, account number must start with repeating the character twice.";
                    return;
                }
            }

            Regex exp = new Regex(@"^\w{2}\d{8}$");
            if (exp.IsMatch(acn)) return;
            else
            {
                args.IsValid = false;
                cvAcNo.ErrorMessage = "Account Number must be followed by 8 digits";
                return;
            }
        }

        //LEGACY function
/*        protected void cvDOB_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string dob = DateOfBirth.Text.Trim();

            Regex exp = new Regex(@"^\d{2}\/\d{2}\/\d{4}$");
            if (!exp.IsMatch(dob))
            {
                args.IsValid = false;
                cvDOB.ErrorMessage = "Date of Birth is not valid.";
                cvDOB2.ErrorMessage = "Co-account holder date of Birth is not valid.";
                return;
            }

            int day = Convert.ToInt32(dob.Substring(0, 2));
            int month = Convert.ToInt32(dob.Substring(3, 2));
            int year = Convert.ToInt32(dob.Substring(6, 4));
            
            if (day > 0 && day <= 31)
            {
                if (month > 0 && month <= 12)
                {
                    if (year > 1900 && year <= 2016) return;
                }
            }
            args.IsValid = false;
            cvDOB.ErrorMessage = "Date of Birth is not valid.";
            cvDOB2.ErrorMessage = "Co-account holder date of Birth is not valid.";
            return;
        }*/

        protected void cvPhone_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hPhone.Text != "" || hFax.Text != "" || bPhone.Text != "" || bFax.Text != "")
                return;
            args.IsValid = false;
            cvPhone.ErrorMessage = "At least one of the phone numbers must be filled in.";
            cvPhone2.ErrorMessage = "At least one of the co-account holder's phone numbers must be filled in.";
        }

        protected void cvIssueCountry_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ID_PP.SelectedValue == "Passport Number" && IssueCountry.Text == "")
            {
                args.IsValid = false;
                cvIssueCountry.ErrorMessage = "Passport country of issue is required.";
                return;
            }
        }
        protected void cvIssueCountry2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ID_PP2.SelectedValue == "Passport Number" && IssueCountry2.Text == "")
            {
                args.IsValid = false;
                cvIssueCountry2.ErrorMessage = "Co-account holder passport country of issue is required.";
                return;
            }
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

        protected void cvDeposit_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //none selected
            if(!Cheque.Checked && !Transfer.Checked)
            {
                args.IsValid = false;
                cvDeposit.ErrorMessage = "Please choose one or more payment method.";
                return;
            }

            //cheque selected but no value
            if(Cheque.Checked && ChequeV.Text == "")
            {
                args.IsValid = false;
                cvDeposit.ErrorMessage = "Please indicate the amount you would like to pay by cheque";
                return;
            }

            //transfer selected but no value
            if (Transfer.Checked && TransferV.Text == "")
            {
                args.IsValid = false;
                cvDeposit.ErrorMessage = "Please indicate the amount you would like to transfer";
                return;
            }

            //make sure amount >=20000
            decimal chequeV = 0, transferV = 0;
            if (Cheque.Checked)
                if (decimal.TryParse(ChequeV.Text, out chequeV)) ;
            if (Transfer.Checked)
                if (decimal.TryParse(TransferV.Text, out transferV)) ;

            decimal temp = chequeV + transferV;
                if (temp >= 20000) return;
            
            args.IsValid = false;
            cvDeposit.ErrorMessage = "A HK$20,000 minimum deposit is required to open an account.";
            return;
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
                string sql = "INSERT INTO [LoginAccount] VALUES ('" + acNo + "', '" + acType.SelectedValue + "', " + initialBalance + ", NULL)";

                SqlTransaction myTransaction = myHKeInvest.beginTransaction();
                myHKeInvest.setData(sql, myTransaction);

                //insert all required fields first
                sql = "INSERT INTO [Client] (isPrimary, title, accountNumber, firstName, lastName, dateOfBirth, email, HKIDPassportNumber, citizenship, residence, building, street, district, employmentStatus, employByBroker, publiclyTradedCompany, primarySourceFund, investObjective, investKnowledge, investExperience, annualIncome, liquidNetWorth, freeBalanceToFund) ";
                sql = sql + "VALUES ('true', '" + title.SelectedValue + "', '" + acNo + "', '" + FirstName.Text + "', '" + LastName.Text + "', CONVERT(date, '" + DateOfBirth.Text + "', 103), '" + Email.Text + "', '" + HKID.Text + "', '" + Citizen.Text + "', '" + Residence.Text + "', '" + Building.Text + "', '" + Street.Text + "', '" + District.Text + "', '" + EmpStatus.SelectedValue + "', '" + EmpByBroker.SelectedValue + "', '" + CompanyDirector.SelectedValue + "', '" + PrimarySource.SelectedValue + "', '" + Objective.SelectedValue + "', '" + Knowledge.SelectedValue + "', '" + Experience.SelectedValue + "', '" + Income.SelectedValue + "', '" + NetWorth.SelectedValue + "', '" + Fund.Checked + "')";
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
                if(EmpStatus.SelectedValue == "Employed")
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
                    sql = "INSERT INTO [Client] (isPrimary, title, accountNumber, firstName, lastName, dateOfBirth, email, HKIDPassportNumber, citizenship, residence, building, street, district, employmentStatus, employByBroker, publiclyTradedCompany, primarySourceFund, investObjective, investKnowledge, investExperience, annualIncome, liquidNetWorth, freeBalanceToFund) ";
                    sql = sql + "VALUES ('false', '" + title2.SelectedValue + "', '" + acNo + "', '" + FirstName2.Text + "', '" + LastName2.Text + "', CONVERT(date, '" + DateOfBirth2.Text + "', 103), '" + Email2.Text + "', '" + HKID2.Text + "', '" + Citizen2.Text + "', '" + Residence2.Text + "', '" + Building2.Text + "', '" + Street2.Text + "', '" + District2.Text + "', '" + EmpStatus2.SelectedValue + "', '" + EmpByBroker2.SelectedValue + "', '" + CompanyDirector2.SelectedValue + "', '" + PrimarySource.SelectedValue + "', '" + Objective.SelectedValue + "', '" + Knowledge.SelectedValue + "', '" + Experience.SelectedValue + "', '" + Income.SelectedValue + "', '" + NetWorth.SelectedValue + "', '" + Fund.Checked + "')";
                    myHKeInvest.setData(sql, myTransaction);
                    myHKeInvest.commitTransaction(myTransaction);       //need to commit transaction before being able to retreive information from the database


                    //get co-ac holder's clientNumber
                    sql = "SELECT clientNumber from Client WHERE accountNumber = '" + acNo + "' AND firstName = '" + FirstName2.Text + "'";
                    DataTable dtClient = myHKeInvest.getData(sql);
                    string cNo = "";
                    foreach(DataRow row in dtClient.Rows)
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