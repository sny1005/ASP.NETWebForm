<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountApplicationForm.aspx.cs" Inherits="HKeInvestWebApplication.ApplicationPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h4>New Client Application</h4>
    <div class="form-horizontal">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="False" CssClass="text-danger" />


        <br />
        <h6>1. Account Type</h6>
        <div class="form-group">
            <asp:Label runat="server" Text="Account Type" CssClass="control-label col-md-2" AssociatedControlID="acType"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="acType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="acType_SelectedIndexChanged">
                    <asp:ListItem Value="individual">Individual</asp:ListItem>
                    <asp:ListItem Value="survivorship">Joint Tenants with Right of Survivorship</asp:ListItem>
                    <asp:ListItem Value="common">Joint Tenants in Common</asp:ListItem>
                </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Account Type is required." ControlToValidate="acType" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>


        <br />
        <h6>2. Client Information</h6>
        <div class="form-group">
            <asp:Label runat="server" Text="Title" CssClass="control-label col-md-2" AssociatedControlID="title"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="title" runat="server">
                    <asp:ListItem>Mr.</asp:ListItem>
                    <asp:ListItem>Mrs.</asp:ListItem>
                    <asp:ListItem>Ms.</asp:ListItem>
                    <asp:ListItem>Dr.</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Title is required." ControlToValidate="title" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="AccountNumber" runat="server" Text="Account #" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="AccountNumber" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Account Number is required." ControlToValidate="AccountNumber" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvAcNo" runat="server" ErrorMessage="Invalid Account Number format" ControlToValidate="AccountNumber" EnableClientScript="False" CssClass="text-danger" Display="Dynamic" OnServerValidate="cvAcNo_ServerValidate">*</asp:CustomValidator>                
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="FirstName" runat="server" Text="First Name" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="FirstName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="First Name is required." ControlToValidate="FirstName" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label AssociatedControlID="LastName" runat="server" Text="Last Name" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="LastName" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Last Name is required." ControlToValidate="LastName" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="DateOfBirth" runat="server" Text="Date of Birth" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="DateOfBirth" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Date of Birth is required." ControlToValidate="DateOfBirth" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Date of Birth must be in the format of dd/mm/yyyy" ControlToValidate="DateOfBirth" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="(^(((0[1-9]|1[0-9]|2[0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)">*</asp:RegularExpressionValidator>
            </div>
            <asp:Label AssociatedControlID="Email" runat="server" Text="Email" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Email" runat="server" CssClass="form-control" MaxLength="30" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Email address is required." ControlToValidate="Email" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="ID_PP" runat="server" Text="Please choose between HKID and Passport Number" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="ID_PP" runat="server">
                    <asp:ListItem>HKID</asp:ListItem>
                    <asp:ListItem>Passport Number</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Please choose either HKID or Passport Number" ControlToValidate="ID_PP" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="HKID" runat="server" Text="HKID/Passport#" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="HKID" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="A HKID or Passport number is required." ControlToValidate="HKID" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label AssociatedControlID="IssueCountry" runat="server" Text="Passport country of issue" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="IssueCountry" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                <asp:CustomValidator ID="cvIssueCountry" runat="server" ControlToValidate="IssueCountry" EnableClientScript="False" CssClass="text-danger" Display="Dynamic" OnServerValidate="cvIssueCountry_ServerValidate" ValidateEmptyText="True">*</asp:CustomValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="Citizen" runat="server" Text="Country of citizenship" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Citizen" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Country of citizenship is required." ControlToValidate="Citizen" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label AssociatedControlID="Residence" runat="server" Text="Country of legal residence" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Residence" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Country of legal residence is required." ControlToValidate="Residence" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="hPhone" runat="server" Text="Home Phone" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="hPhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                <asp:CustomValidator ID="cvPhone" runat="server" Text="*" OnServerValidate="cvPhone_ServerValidate" ControlToValidate="hPhone" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False"></asp:CustomValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Phone number must contain 8 digits" ControlToValidate="hPhone" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
            </div>
            <asp:Label AssociatedControlID="hFax" runat="server" Text="Home Fax" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="hFax" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Home fax must contain 8 digits" ControlToValidate="hFax" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="bPhone" runat="server" Text="Business Phone" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="bPhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Business phone must contain 8 digits" ControlToValidate="bPhone" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
            </div>
            <asp:Label AssociatedControlID="bFax" runat="server" Text="Business Fax" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="bFax" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Business fax must contain 8 digits" ControlToValidate="bFax" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" Text="Home address" CssClass="control-label col-md-2"></asp:Label>
            <div class="row col-md-12">
                <asp:Label runat="server" Text="Building" CssClass="control-label col-md-2" AssociatedControlID="Building"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Building" runat="server" CssClass="form-control col-md-4" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Building is required." ControlToValidate="Building" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row col-md-12">
                <asp:Label runat="server" Text="Street" CssClass="control-label col-md-2" AssociatedControlID="Street"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Street" runat="server" CssClass="form-control col-md-4" MaxLength="35"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Street is required." ControlToValidate="Street" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row col-md-12">
                <asp:Label runat="server" Text="District" CssClass="control-label col-md-2" AssociatedControlID="District"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="District" runat="server" CssClass="form-control col-md-4" MaxLength="19"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="District is required." ControlToValidate="District" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <br />
        <h6>3. Employment Information</h6>
        <div class="form-group">
            <asp:Label AssociatedControlID="EmpStatus" runat="server" Text="Employment Status" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="EmpStatus" runat="server">
                    <asp:ListItem>Employed</asp:ListItem>
                    <asp:ListItem>Self-employed</asp:ListItem>
                    <asp:ListItem>Retired</asp:ListItem>
                    <asp:ListItem>Student</asp:ListItem>
                    <asp:ListItem>Not Employed</asp:ListItem>
                    <asp:ListItem>Homemaker</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Employment status is required." ControlToValidate="EmpStatus" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="Occupation" runat="server" Text="Specific occupation" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Occupation" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                <asp:CustomValidator ID="cvOccupation" runat="server" Text="*" OnServerValidate="cvOccupation_ServerValidate" ControlToValidate="Occupation" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Specific occupation is required."></asp:CustomValidator>
            </div>
            <asp:Label AssociatedControlID="yrWithEmp" runat="server" Text="Years with employer" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="yrWithEmp" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
                <asp:CustomValidator ID="cvyrWithEmp" runat="server" Text="*" OnServerValidate="cvOccupation_ServerValidate" ControlToValidate="yrWithEmp" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Years with employer is required."></asp:CustomValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Please enter an integer for years with employer." ControlToValidate="yrWithEmp" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="^\d{1,2}$">*</asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="Employer" runat="server" Text="Employer name" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Employer" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                <asp:CustomValidator ID="cvEmployer" runat="server" Text="*" OnServerValidate="cvOccupation_ServerValidate" ControlToValidate="Employer" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Employer name is required."></asp:CustomValidator>
            </div>
            <asp:Label AssociatedControlID="EmployerPhone" runat="server" Text="Employer phone" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="EmployerPhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                <asp:CustomValidator ID="cvEmpPhone" runat="server" Text="*" OnServerValidate="cvOccupation_ServerValidate" ControlToValidate="EmployerPhone" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Employer phone is required."></asp:CustomValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="Employer phone must contain 8 digits" ControlToValidate="EmployerPhone" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="Business" runat="server" Text="Nature of business" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Business" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                <asp:CustomValidator ID="cvBusiness" runat="server" Text="*" OnServerValidate="cvOccupation_ServerValidate" ControlToValidate="Business" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Nature of business is required."></asp:CustomValidator>
            </div>
        </div>


        <br />
        <h6>4. Disclosures and Regulatory Information</h6>
        <div class="form-group">
            <asp:Label AssociatedControlID="EmpByBroker" runat="server" Text="Are you employed by a registerd securities broker/dealer, investment advisor, bank or other financial institution?" CssClass="control-label col-md-4"></asp:Label>
            <div class="col-md-2">
                <asp:RadioButtonList ID="EmpByBroker" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                    <asp:ListItem Value="FALSE">No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Employed by broker status is required." ControlToValidate="EmpByBroker" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label AssociatedControlID="CompanyDirector" runat="server" Text="Are you a director, 10% shareholder or policy-making officer of a publicly traded company?" CssClass="control-label col-md-4"></asp:Label>
            <div class="col-md-2">
                <asp:RadioButtonList ID="CompanyDirector" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                    <asp:ListItem Value="FALSE">No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Shareholder status is required." ControlToValidate="CompanyDirector" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="PrimarySource" runat="server" Text="Describe the primary source of funds deposited to this account." CssClass="control-label col-md-4"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="PrimarySource" runat="server">
                    <asp:ListItem Value="salary">salary/wages/savings</asp:ListItem>
                    <asp:ListItem Value="investment">investment/capital gains</asp:ListItem>
                    <asp:ListItem Value="family">family/relatives/inheritance</asp:ListItem>
                    <asp:ListItem Value="other">other</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Primary source of fund is required." ControlToValidate="PrimarySource" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <div class="row col-md-12">
                <asp:Label AssociatedControlID="SpecificSource" runat="server" Text="Please specify" CssClass="control-label col-md-4"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="SpecificSource" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                    <asp:CustomValidator ID="cvSpecificSource" runat="server" Text="*" OnServerValidate="cvSpecificSource_ServerValidate" ControlToValidate="SpecificSource" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Please specify your primary source of fund."></asp:CustomValidator>
                </div>
            </div>
        </div>




        <%-- START OF CO-HOLDER SESSION --%>
        <br />
        <asp:Panel ID="CoHolderPanel" runat="server" Visible="False">
            <h6>2B. Co-account holder information</h6>
            <div class="form-group">
                <asp:Label runat="server" Text="Co-account holder title" CssClass="control-label col-md-2" AssociatedControlID="title2"></asp:Label>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="title2" runat="server">
                        <asp:ListItem>Mr.</asp:ListItem>
                        <asp:ListItem>Mrs.</asp:ListItem>
                        <asp:ListItem>Ms.</asp:ListItem>
                        <asp:ListItem>Dr.</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rvtitle2" runat="server" ErrorMessage="Co-account holder title is required." ControlToValidate="title2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="FirstName2" runat="server" Text="First Name" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="FirstName2" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvFirstName2" runat="server" ErrorMessage="Co-account holder first Name is required." ControlToValidate="FirstName2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
                <asp:Label AssociatedControlID="LastName2" runat="server" Text="Last Name" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="LastName2" runat="server" CssClass="form-control" MaxLength="35"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvLasttName2" runat="server" ErrorMessage="Co-account holder last Name is required." ControlToValidate="LastName2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="DateOfBirth2" runat="server" Text="Date of Birth" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="DateOfBirth2" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Date of Birth is required." ControlToValidate="DateOfBirth2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="Co-holder date of Birth must be in the format dd/mm/yyyy" ControlToValidate="DateOfBirth2" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="(^(((0[1-9]|1[0-9]|2[0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)">*</asp:RegularExpressionValidator>
                </div>
                <asp:Label AssociatedControlID="Email2" runat="server" Text="Email" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Email2" runat="server" CssClass="form-control" MaxLength="30" TextMode="Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="Co-account holder email address is required." ControlToValidate="Email2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="ID_PP2" runat="server" Text="Please choose between HKID and Passport Number" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="ID_PP2" runat="server">
                        <asp:ListItem>HKID</asp:ListItem>
                        <asp:ListItem>Passport Number</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="Please choose either HKID or Passport Number for co-account holder." ControlToValidate="ID_PP2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label AssociatedControlID="HKID2" runat="server" Text="HKID/Passport#" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="HKID2" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="Co-account holder's HKID or Passport number is required." ControlToValidate="HKID2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
                <asp:Label AssociatedControlID="IssueCountry2" runat="server" Text="Passport country of issue" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="IssueCountry2" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                    <asp:CustomValidator ID="cvIssueCountry2" runat="server" ControlToValidate="IssueCountry2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic" OnServerValidate="cvIssueCountry2_ServerValidate" ValidateEmptyText="True">*</asp:CustomValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="Citizen2" runat="server" Text="Country of citizenship" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Citizen2" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="Co-account holder's country of citizenship is required." ControlToValidate="Citizen2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
                <asp:Label AssociatedControlID="Residence2" runat="server" Text="Country of legal residence" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Residence2" runat="server" CssClass="form-control" MaxLength="70"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="Co-account holder country of legal residence is required." ControlToValidate="Residence2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="hPhone2" runat="server" Text="Home Phone" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="hPhone2" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:CustomValidator ID="cvPhone2" runat="server" Text="*" OnServerValidate="cvPhone_ServerValidate" ControlToValidate="hPhone2" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False"></asp:CustomValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage="Co-account holder home phone must contain 8 digits" ControlToValidate="hPhone2" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
                </div>
                <asp:Label AssociatedControlID="hFax2" runat="server" Text="Home Fax" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="hFax2" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ErrorMessage="Co-account holder home fax must contain 8 digits" ControlToValidate="hFax2" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="bPhone2" runat="server" Text="Business Phone" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="bPhone2" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ErrorMessage="Co-account holder business phone must contain 8 digits" ControlToValidate="bPhone2" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
                </div>
                <asp:Label AssociatedControlID="bFax2" runat="server" Text="Business Fax" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="bFax2" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ErrorMessage="Co-account holder business fax must contain 8 digits" ControlToValidate="bFax2" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label runat="server" Text="Home address" CssClass="control-label col-md-2"></asp:Label>
                <div class="row col-md-12">
                    <asp:Label runat="server" Text="Building" CssClass="control-label col-md-2" AssociatedControlID="Building2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="Building2" runat="server" CssClass="form-control col-md-4" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ErrorMessage="Co-account holder building is required." ControlToValidate="Building2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row col-md-12">
                    <asp:Label runat="server" Text="Street" CssClass="control-label col-md-2" AssociatedControlID="Street2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="Street2" runat="server" CssClass="form-control col-md-4" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ErrorMessage="Co-account holder street is required." ControlToValidate="Street2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row col-md-12">
                    <asp:Label runat="server" Text="District" CssClass="control-label col-md-2" AssociatedControlID="District2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="District2" runat="server" CssClass="form-control col-md-4" MaxLength="19"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ErrorMessage="Co-account holder district is required." ControlToValidate="District2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <br />
            <h6>3B. Co-account holder employment Information</h6>
            <div class="form-group">
                <asp:Label AssociatedControlID="EmpStatus2" runat="server" Text="Employment Status" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:RadioButtonList ID="EmpStatus2" runat="server">
                        <asp:ListItem>Employed</asp:ListItem>
                        <asp:ListItem>Self-employed</asp:ListItem>
                        <asp:ListItem>Retired</asp:ListItem>
                        <asp:ListItem>Student</asp:ListItem>
                        <asp:ListItem>Not Employed</asp:ListItem>
                        <asp:ListItem>Homemaker</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="Co-account holder's employment status is required." ControlToValidate="EmpStatus2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="Occupation2" runat="server" Text="Specific occupation" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Occupation2" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                    <asp:CustomValidator ID="cvOccupation2" runat="server" Text="*" OnServerValidate="cvOccupation2_ServerValidate" ControlToValidate="Occupation2" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Co-account holder specific occupation is required."></asp:CustomValidator>
                </div>
                <asp:Label AssociatedControlID="yrWithEmp2" runat="server" Text="Years with employer" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="yrWithEmp2" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
                    <asp:CustomValidator ID="cvyrWithEmp2" runat="server" Text="*" OnServerValidate="cvOccupation2_ServerValidate" ControlToValidate="yrWithEmp2" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Co-account holder years with employer is required."></asp:CustomValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ErrorMessage="Please enter an integer for years with employer." ControlToValidate="yrWithEmp2" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="^\d{1,2}$">*</asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="Employer2" runat="server" Text="Employer name" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Employer2" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                    <asp:CustomValidator ID="CustomValidator6" runat="server" Text="*" OnServerValidate="cvOccupation2_ServerValidate" ControlToValidate="Employer2" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Co-account holder employer name is required."></asp:CustomValidator>
                </div>
                <asp:Label AssociatedControlID="EmployerPhone2" runat="server" Text="Employer phone" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="EmployerPhone2" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                    <asp:CustomValidator ID="CustomValidator7" runat="server" Text="*" OnServerValidate="cvOccupation2_ServerValidate" ControlToValidate="EmployerPhone2" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Co-account holder employer phone is required."></asp:CustomValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ErrorMessage="Employer phone must contain 8 digits" ControlToValidate="EmployerPhone2" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\d{8}">*</asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="form-group">
                <asp:Label AssociatedControlID="Business2" runat="server" Text="Nature of business" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Business2" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                    <asp:CustomValidator ID="CustomValidator8" runat="server" Text="*" OnServerValidate="cvOccupation2_ServerValidate" ControlToValidate="Business2" ValidateEmptyText="True" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Co-account holder's nature of business is required."></asp:CustomValidator>
                </div>
            </div>


            <br />
            <h6>4B. Co-account holder disclosures and Regulatory Information</h6>
            <div class="form-group">
                <asp:Label AssociatedControlID="EmpByBroker2" runat="server" Text="Are you employed by a registerd securities broker/dealer, investment advisor, bank or other financial institution?" CssClass="control-label col-md-4"></asp:Label>
                <div class="col-md-2">
                    <asp:RadioButtonList ID="EmpByBroker2" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                        <asp:ListItem Value="FALSE">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="Co-account holder's employed by broker status is required." ControlToValidate="EmpByBroker2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
                <asp:Label AssociatedControlID="CompanyDirector2" runat="server" Text="Are you a director, 10% shareholder or policy-making officer of a publicly traded company?" CssClass="control-label col-md-4"></asp:Label>
                <div class="col-md-2">
                    <asp:RadioButtonList ID="CompanyDirector2" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                        <asp:ListItem Value="FALSE">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="Co-account holder's shareholder status is required." ControlToValidate="CompanyDirector2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </div>
        </asp:Panel>

        <%-- END OF CO-HOLDER SESSION --%>



        <br />
        <h6>5. Investment Profile</h6>
        <div class="form-group">
            <asp:Label AssociatedControlID="Objective" runat="server" Text="Investment objective" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="Objective" runat="server">
                    <asp:ListItem Value="preservation">Capital preservation</asp:ListItem>
                    <asp:ListItem Value="income">Income</asp:ListItem>
                    <asp:ListItem Value="growth">Growth</asp:ListItem>
                    <asp:ListItem Value="speculation">Speculation</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Investment objective is required." ControlToValidate="Objective" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="Knowledge" runat="server" Text="Investment knowledge" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="Knowledge" runat="server">
                    <asp:ListItem Value="none">None</asp:ListItem>
                    <asp:ListItem Value="limited">Limited</asp:ListItem>
                    <asp:ListItem Value="good">Good</asp:ListItem>
                    <asp:ListItem Value="extensive">Extensive</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="Investment knowledge is required." ControlToValidate="Knowledge" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label AssociatedControlID="Experience" runat="server" Text="Investment experience" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="Experience" runat="server">
                    <asp:ListItem Value="none">None</asp:ListItem>
                    <asp:ListItem Value="limited">Limited</asp:ListItem>
                    <asp:ListItem Value="good">Good</asp:ListItem>
                    <asp:ListItem Value="extensive">Extensive</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Investment experience is required." ControlToValidate="Experience" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
        <asp:Label AssociatedControlID="Income" runat="server" Text="Annual income" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="Income" runat="server">
                    <asp:ListItem Value="<20000">under HK$20,000</asp:ListItem>
                    <asp:ListItem Value=">20001">HK$20,001 - HK$200,000</asp:ListItem>
                    <asp:ListItem Value=">200001">HK$200,001 - HK$2,000,000</asp:ListItem>
                    <asp:ListItem Value=">2000000">more than HK$2,000,000</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="Annual income is required." ControlToValidate="Income" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
            <asp:Label AssociatedControlID="NetWorth" runat="server" Text="Approximate liquid net worth" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="NetWorth" runat="server">
                    <asp:ListItem Value="<100000">under HK$100,000</asp:ListItem>
                    <asp:ListItem Value=">100001">HK$100,001 - HK$1,000,000</asp:ListItem>
                    <asp:ListItem Value=">1000001">HK$1,000,001 - HK$10,000,000</asp:ListItem>
                    <asp:ListItem Value=">10000000">more than HK$10,000,000</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="Liquid net worth is required." ControlToValidate="NetWorth" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>


        <br />
        <h6>6. Account Feature</h6>
        <p>Do you want to sweep your Free Credit Balance into the sweep fund?</p>
        <div class="form-group">
            <div class="col-md-4"><asp:CheckBox ID="Fund" runat="server" Text="Yes, sweep my Free Credit Balance into the Fund."/></div>
        </div>

        <br />
        <h6>7. Initial Account Deposit</h6>
        <div>A <i><b>HK$20,000 minimum deposit</b></i> is required to open an account.</div>
        <p>Check one or more:</p>
        <asp:CustomValidator ID="cvDeposit" runat="server" Text="*" OnServerValidate="cvDeposit_ServerValidate" ControlToValidate="ChequeV" ValidateEmptyText="true" CssClass="text-danger" Display="Dynamic" EnableClientScript="False"></asp:CustomValidator>
        <div class="form-group">
            <asp:CheckBox ID="Cheque" runat="server" Text="A cheque for the value below will be made payable to HKeInvest LLC."  CssClass="col-md-4"/>
            <div class="col-md-4">
                <asp:TextBox ID="ChequeV" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="Cheque value is not an valid number." CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ControlToValidate="ChequeV" ValidationExpression="^\d+\.*\d*" Text="*"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:CheckBox ID="Transfer" runat="server" Text="A completed Account Trasfer Form for the value indicated will be attached." CssClass="col-md-4" />
            <div class="col-md-4">
                <asp:TextBox ID="TransferV" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator runat="server" ErrorMessage="Transfer value is not an valid number." CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ControlToValidate="TransferV" ValidationExpression="^\d+\.*\d*" Text="*"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10"><asp:Button ID="Register" runat="server" Text="Register" CssClass="btn" OnClick="Register_Click"></asp:Button></div>
        </div>
    </div>

</asp:Content>
