<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="HKeInvestWebApplication.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <asp:ValidationSummary runat="server" CssClass="text-danger" EnableClientScript="False" />

        <div class="form-group">
            <asp:Label AssociatedControlID="AccountNumber" runat="server" Text="Account #" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="AccountNumber" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Account Number is required." ControlToValidate="AccountNumber" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
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
                <asp:TextBox ID="DateOfBirth" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Date of Birth is required." ControlToValidate="DateOfBirth" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Date of Birth is not valid" ControlToValidate="DateOfBirth" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="(^(((0[1-9]|1[0-9]|2[0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)">*</asp:RegularExpressionValidator>
            </div>
            <asp:Label AssociatedControlID="Email" runat="server" Text="Email" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Email" runat="server" CssClass="form-control" MaxLength="30" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Email address is required." ControlToValidate="Email" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="UserName" runat="server" Text="User Name" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="UserName" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="User Name is required." ControlToValidate="UserName" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="User name must contain only letters and digits." ControlToValidate="UserName" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="^\w+$">*</asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="User name length must between 6-10 characters." ControlToValidate="UserName" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="^.{6,10}$">*</asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label AssociatedControlID="Password" runat="server" Text="Password" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="Password" runat="server" Textmode="Password" CssClass="form-control" MaxLength="15"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Password is required." ControlToValidate="Password" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Password length must be 8-15 characters." ControlToValidate="Password" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="^.{8,15}$">*</asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Password must contain at least 2 nonalphanumeric characters." ControlToValidate="Password" Display="Dynamic" CssClass="text-danger" EnableClientScript="False" ValidationExpression="\w*\W\w*\W+\w*">*</asp:RegularExpressionValidator>
            </div>
            <asp:Label AssociatedControlID="ConfirmPassword" runat="server" Text="Confirm Password" CssClass="control-label col-md-2"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox ID="ConfirmPassword" runat="server" Textmode="Password" CssClass="form-control" MaxLength="15"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Confirm Password is required." ControlToValidate="ConfirmPassword" EnableClientScript="False" CssClass="text-danger" Display="Dynamic">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Confirm password does not match with password." CssClass="text-danger" Display="Dynamic" ControlToCompare="Password" ControlToValidate="ConfirmPassword" EnableClientScript="False">*</asp:CompareValidator>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
