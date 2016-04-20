<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetAlert.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.SetAlert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Set Alert on Securities</h2>
    <div><asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="False" CssClass="text-danger" /></div>
    <div>
        <asp:RadioButtonList ID="AlertType_RadioButtonList" runat="server" AutoPostBack="True">
            <asp:ListItem>Low Value Alert</asp:ListItem>
            <asp:ListItem>High Value Alert</asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="col-md-3">
        <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="True">
            <asp:ListItem>Stock</asp:ListItem>
            <asp:ListItem>Bond</asp:ListItem>
            <asp:ListItem>Unit Trust</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <div class="col-md-4">
            <asp:Label runat="server" Text="Security Code" AssociatedControlID="SecurityCode" CssClass="control-label"></asp:Label>
            <asp:TextBox ID="SecurityCode" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
            <asp:CustomValidator ID="cvSecurityCode" runat="server" ControlToValidate="SecurityCode" Display="Dynamic" EnableClientScript="False" ErrorMessage="This security is not owned." ForeColor="Red" OnServerValidate="cvSecurityCode_ServerValidate">*</asp:CustomValidator>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="SecurityCode" CssClass="text-danger" Display="Dynamic" EnableClientScript="False" ErrorMessage="Security Code is requried." ForeColor="Red">*</asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-4">
            <asp:Label runat="server" Text="Alert Value" AssociatedControlID="AlertValue" CssClass="control-label"></asp:Label>
            <asp:TextBox ID="AlertValue" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ErrorMessage="Alert value is required." ControlToValidate="AlertValue" Display="Dynamic" EnableClientScript="False" ForeColor="Red">*</asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <asp:Button runat="server" Text="Set" CssClass="btn btn-default" ID="Set" OnClick="Set_Click" />
            <asp:CustomValidator ID="cvSet" runat="server" Display="Dynamic" EnableClientScript="False" ErrorMessage="CustomValidator" OnServerValidate="cvSet_ServerValidate" Visible="False"></asp:CustomValidator>
        </div>
    </div>
    
</asp:Content>
