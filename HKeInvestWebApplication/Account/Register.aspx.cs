using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using HKeInvestWebApplication.Models;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using HKeInvestWebApplication.Code_File;
using System.Data.SqlClient;

namespace HKeInvestWebApplication.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();

                string acNo = AccountNumber.Text;
                string fName = FirstName.Text;
                string lName = LastName.Text;
                string dob = DateOfBirth.Text;
                string email = Email.Text;

                string sql = "SELECT accountNumber, isPrimary FROM Client WHERE accountNumber = '" + acNo + "' AND firstName ='" + fName + "' AND lastName = '" + lName + "' AND dateOfBirth = CONVERT(date, '" + dob + "', 103) AND email = '" + email + "'";

                HKeInvestData myHKeInvestData = new HKeInvestData();
                System.Data.DataTable dtClient = myHKeInvestData.getData(sql);
                if (dtClient.Rows.Count == 0 || dtClient == null) // If the DataSet is null, a SQL error occurred.
                {
                    ErrorMessage.Text = "Account information is incorrect.";
                    return;
                }
                else
                {
                    System.Data.DataRow[] row = dtClient.Select();
                    if ((bool)row[0]["isPrimary"] == false)
                    {
                        ErrorMessage.Text = "You must use the information of the primary account holder to register a login account.";
                        return;
                    }
                }

                var user = new ApplicationUser() { UserName = UserName.Text.ToLower(), Email = Email.Text };
                IdentityResult result = manager.Create(user, Password.Text);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    //string code = manager.GenerateEmailConfirmationToken(user.Id);
                    //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    //Assign user to Client role.
                    IdentityResult roleResult = manager.AddToRole(user.Id, "Client");
                    if (!roleResult.Succeeded)
                    {
                        ErrorMessage.Text = roleResult.Errors.FirstOrDefault();
                    }

                    //relate the newly created username with existing record
                    sql = "UPDATE LoginAccount SET username = '" + user.UserName + "' WHERE accountNumber = '" + acNo + "'";
                    SqlTransaction trans = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);

                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                }
            }
        }

        // 2 LEGACY function
/*        protected void cvAcNo_ServerValidate(object source, ServerValidateEventArgs args)
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
                    cvAcNo.ErrorMessage = "Account Number must start with repeating your last name twice.";
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

        protected void cvDOB_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string dob = DateOfBirth.Text.Trim();
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
            return;
        }*/
    }
}