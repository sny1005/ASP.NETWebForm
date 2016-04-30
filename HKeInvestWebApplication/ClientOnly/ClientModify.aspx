<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientModify.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ClientModify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <h2>Modify Account Informations</h2>

  
    <div class="form-horizontal">
        <asp:ValidationSummary runat="server" CssClass="text-danger" EnableClientScript="False" />
         
        <br />
        <h6>Primary Account Holder - Personal Information</h6>

        <div class="form-group">
            <asp:Label ID="lblAccountNumber" runat="server" Text="Account number:" CssClass="col-md-3" ></asp:Label>
            <asp:Label ID="lblClientName" runat="server" Text="" Visible="False" CssClass="col-md-3"></asp:Label>
            <asp:Label ID="lblResultMessage" runat="server" Text="" Visible="False"></asp:Label>    
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="Email" runat="server" Text="Email" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Email" runat="server" CssClass="form-control" MaxLength="30" TextMode="Email"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="hPhone" runat="server" Text="Home Phone" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="hPhone" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
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
                </div>
            </div>
            <div class="row col-md-12">
                <asp:Label runat="server" Text="Street" CssClass="control-label col-md-2" AssociatedControlID="Street"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Street" runat="server" CssClass="form-control col-md-4" MaxLength="35"></asp:TextBox>
                </div>
            </div>
            <div class="row col-md-12">
                <asp:Label runat="server" Text="District" CssClass="control-label col-md-2" AssociatedControlID="District"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="District" runat="server" CssClass="form-control col-md-4" MaxLength="19"></asp:TextBox>
                </div>
            </div>
        </div>

        <br />
        <h6>Primary Account Holder - Employment Information</h6>

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
        <h6>Primary Account Holder - Disclosures and Regulatory Information</h6>

        <div class="form-group">
            <asp:Label AssociatedControlID="EmpByBroker" runat="server" Text="Are you employed by a registerd securities broker/dealer, investment advisor, bank or other financial institution?" CssClass="control-label col-md-4"></asp:Label>
            <div class="col-md-2">
                <asp:RadioButtonList ID="EmpByBroker" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                    <asp:ListItem Value="FALSE">No</asp:ListItem>
                </asp:RadioButtonList>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Employed by broker status is required." ControlToValidate="EmpByBroker" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>--%>
            </div>
            <asp:Label AssociatedControlID="CompanyDirector" runat="server" Text="Are you a director, 10% shareholder or policy-making officer of a publicly traded company?" CssClass="control-label col-md-4"></asp:Label>
            <div class="col-md-2">
                <asp:RadioButtonList ID="CompanyDirector" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                    <asp:ListItem Value="FALSE">No</asp:ListItem>
                </asp:RadioButtonList>
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Shareholder status is required." ControlToValidate="CompanyDirector" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>--%>
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
            <h6>Co-Account holder - Personal Information</h6>

            <div class="form-group">
                <asp:Label AssociatedControlID="Email2" runat="server" Text="Email" CssClass="control-label col-md-2"></asp:Label>
                <div class="col-md-4">
                    <asp:TextBox ID="Email2" runat="server" CssClass="form-control" MaxLength="30" TextMode="Email"></asp:TextBox>
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
                    </div>
                </div>
                <div class="row col-md-12">
                    <asp:Label runat="server" Text="Street" CssClass="control-label col-md-2" AssociatedControlID="Street2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="Street2" runat="server" CssClass="form-control col-md-4" MaxLength="35"></asp:TextBox>
                    </div>
                </div>
                <div class="row col-md-12">
                    <asp:Label runat="server" Text="District" CssClass="control-label col-md-2" AssociatedControlID="District2"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox ID="District2" runat="server" CssClass="form-control col-md-4" MaxLength="19"></asp:TextBox>
                    </div>
                </div>
            </div>

            <br />
            <h6>Co-Account holder - Employment Information</h6>

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
            <h6>Co-Account holder - Disclosures and Regulatory Information</h6>
            <div class="form-group">
                <asp:Label AssociatedControlID="EmpByBroker2" runat="server" Text="Are you employed by a registerd securities broker/dealer, investment advisor, bank or other financial institution?" CssClass="control-label col-md-4"></asp:Label>
                <div class="col-md-2">
                    <asp:RadioButtonList ID="EmpByBroker2" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                        <asp:ListItem Value="FALSE">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="Co-account holder's employed by broker status is required." ControlToValidate="EmpByBroker2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>--%>
                </div>
                <asp:Label AssociatedControlID="CompanyDirector2" runat="server" Text="Are you a director, 10% shareholder or policy-making officer of a publicly traded company?" CssClass="control-label col-md-4"></asp:Label>
                <div class="col-md-2">
                    <asp:RadioButtonList ID="CompanyDirector2" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="TRUE">Yes</asp:ListItem>
                        <asp:ListItem Value="FALSE">No</asp:ListItem>
                    </asp:RadioButtonList>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="Co-account holder's shareholder status is required." ControlToValidate="CompanyDirector2" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>--%>
                </div>
            </div>
        </asp:Panel>

        <%-- END OF CO-HOLDER SESSION --%>

        <br />
        <h6>Investment Profile</h6>
        <div class="form-group">
            <asp:Label AssociatedControlID="Objective" runat="server" Text="Investment objective" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="Objective" runat="server">
                    <asp:ListItem Value="preservation">Capital preservation</asp:ListItem>
                    <asp:ListItem Value="income">Income</asp:ListItem>
                    <asp:ListItem Value="growth">Growth</asp:ListItem>
                    <asp:ListItem Value="speculation">Speculation</asp:ListItem>
                </asp:RadioButtonList>
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
            </div>
            <asp:Label AssociatedControlID="Experience" runat="server" Text="Investment experience" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="Experience" runat="server">
                    <asp:ListItem Value="none">None</asp:ListItem>
                    <asp:ListItem Value="limited">Limited</asp:ListItem>
                    <asp:ListItem Value="good">Good</asp:ListItem>
                    <asp:ListItem Value="extensive">Extensive</asp:ListItem>
                </asp:RadioButtonList>
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
            </div>
            <asp:Label AssociatedControlID="NetWorth" runat="server" Text="Approximate liquid net worth" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:RadioButtonList ID="NetWorth" runat="server">
                    <asp:ListItem Value="<100000">under HK$100,000</asp:ListItem>
                    <asp:ListItem Value=">100001">HK$100,001 - HK$1,000,000</asp:ListItem>
                    <asp:ListItem Value=">1000001">HK$1,000,001 - HK$10,000,000</asp:ListItem>
                    <asp:ListItem Value=">10000000">more than HK$10,000,000</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>

        <br />
        <h6>Account Feature</h6>
        <p>Do you want to sweep your Free Credit Balance into the sweep fund?</p>
        <div class="form-group">
            <div class="col-md-4"><asp:CheckBox ID="Fund" runat="server" Text="Yes, sweep my Free Credit Balance into the Fund."/></div>
        </div>

        <br />
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10"><asp:Button ID="Register" runat="server" Text="UPDATE INFORMATION" CssClass="btn" OnClick="Register_Click"></asp:Button></div>
        </div>
    </div>
</asp:Content>

