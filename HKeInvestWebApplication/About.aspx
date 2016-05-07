<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="HKeInvestWebApplication.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Additional Features - Calendar</h3>
    <h3><span style="font-weight: normal">Aim</span></h3>
    <p><strong>This additional feature aims at providing the latest and the most important financial information across the globe to clients of HKeInvest. We believe by providing up-to- date and accurate information, our clients would be better equipped against the fast-changing investment environment. Therefore, we would like to introduce the user-friendly and straightforward calendar system into the system.</strong></p>
    <h3><span lang="EN-US" style="font-weight: normal">Guidelines</span></h3>
    <h4><span style="font-weight: normal">Access to the calendar</span></h4>
    <p class="MsoNormal">
        <span lang="EN-US" style="font-weight: 700">The calendar function can be access through the “View here” button under the “Important Event” on the homepage of the website.<o:p></o:p></span></p>
    <p>
        <img src="calendarguide/access.jpg" style="width: 70%; height: 70%" /></p>
    <h4><span style="font-weight: normal">Important Events are hightlighted</span></h4>
    <p><strong>Once you entered the calendar page, the day with important events will be hightlighted</strong></p>
    <p>
        <img src="calendarguide/highlight.jpg" style="width: 585px; height: 551px" /></p>
    <h4>Click on a specific day to show the event information</h4>
    <p><strong>You can click on a specific day, and the event information will be shown in the grid view below</strong></p>
    <p>
        <img src="calendarguide/highlight.jpg" style="width: 585px; height: 551px" /></p>
    <h4>Add Important Events to the calendar(Employee Only)</h4>
    <p>E<strong>mployee can add important events to the calendar by access to /EmployeeOnly/AddEvents.aspx and fill in the event information.</strong></p>
    <p>
        <img src="calendarguide/addevent.jpg" style="width: 615px; height: 672px" /></p>
    <p><strong>After adding, the calendar will be automatically updated</strong></p>
    <p>
        <img src="calendarguide/updated.jpg" style="width: 679px; height: 538px" /></p>
</asp:Content>
