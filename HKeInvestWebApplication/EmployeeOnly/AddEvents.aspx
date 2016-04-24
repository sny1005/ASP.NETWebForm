<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEvents.aspx.cs" Inherits="HKeInvestWebApplication.EmployeeOnly.AddEvents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add Events</h2>
    <div>
        <asp:Label ID="Label1" runat="server" Text="EventName"></asp:Label>
        <asp:TextBox ID="eventname" runat="server"></asp:TextBox>
    </div>
    <div>

        <asp:Label ID="Label2" runat="server" Text="EventRegion"></asp:Label>
        <asp:TextBox ID="eventregion" runat="server"></asp:TextBox>

    </div>
    <div>
        <asp:Label ID="Label3" runat="server" Text="EventDate"></asp:Label><asp:TextBox ID="eventdate" runat="server" TextMode="Date"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="Label5" runat="server" Text="Descrption"></asp:Label>
        <asp:TextBox ID="description" runat="server"></asp:TextBox>
    </div>
    <div>
           <asp:Label ID="Label4" runat="server" Text="Year"></asp:Label>
    <asp:TextBox ID="year" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="Label6" runat="server" Text="Month"></asp:Label><asp:TextBox ID="month" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="Label7" runat="server" Text="Day"></asp:Label><asp:TextBox ID="day" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Button ID="Button1" runat="server" Text="Add" OnClick="Button1_Click" />
    </div>
 
    <p>
        &nbsp;</p>
</asp:Content>
