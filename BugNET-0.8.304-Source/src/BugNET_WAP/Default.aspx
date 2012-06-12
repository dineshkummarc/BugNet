<%@ Page Language="c#" Inherits="BugNET._Default" Title="Home" MasterPageFile="~/Shared/Default.master"
    CodeBehind="Default.aspx.cs" meta:resourcekey="Page" %>

<%@ Import Namespace="BugNET.BusinessLogicLayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:Repeater ID="rptProject" runat="Server">
        <ItemTemplate>
            <ajaxToolkit:HoverMenuExtender ID="hme2" runat="Server" TargetControlID="testlink"
                PopupControlID="PopupMenu" PopupPosition="Bottom" OffsetX="0" OffsetY="5" PopDelay="100" />
            <div class="project">
                <div class="projectImageGroup">
                    <a href="Projects/ProjectSummary.aspx?pid=<%# ((Project)Container.DataItem).Id %>">
                        <asp:Image CssClass="projectImage thumb" BorderWidth="1px" runat="server" Height="62"
                            Width="62" AlternateText="Project Image" ID="ProjectImage" /></a>
                </div>
                <div class="projectDetails">
                    <h1>
                        <a href="Projects/ProjectSummary.aspx?pid=<%# ((Project)Container.DataItem).Id %>">
                            <%#((Project)Container.DataItem).Name%>
                            <span>(<%#((Project)Container.DataItem).Code%>) </span></a>
                    </h1>
                    <asp:Panel CssClass="quickLinks" ID="Panel9" runat="server">
                        Managed By
                        <%#((Project)Container.DataItem).ManagerDisplayName%>
                        <a id="testlink" class="quickLink" runat="server">Quick Links / Filters
                            <asp:Image AlternateText="down" CssClass="icon" runat="server" ImageUrl="~/images/bullet_arrow_down.gif"
                                ID="Image90" /></a>
                    </asp:Panel>
                    <p class="projectDesc">
                        <%#Server.HtmlDecode(((Project)Container.DataItem).Description)%></p>
                    <asp:Panel CssClass="popupMenu" ID="PopupMenu" Style="width: 320px" runat="server">
                        <div style="float: left">
                            <span class="popupTitle">Quick Links</span>
                            <ul style="width: 140px">
                                <li><a href="Projects/ProjectSummary.aspx?pid=<%# ((Project)Container.DataItem).Id %>">
                                    Project Summary</a></li>
                                <li><a href="Queries/QueryList.aspx?pid=<%# ((Project)Container.DataItem).Id %>">Queries</a>
                                </li>
                                <li><a href="Projects/ChangeLog.aspx?pid=<%# ((Project)Container.DataItem).Id %>">Change
                                    Log</a></li>
                                <li><a href="Projects/Roadmap.aspx?pid=<%# ((Project)Container.DataItem).Id %>">RoadMap</a></li>
                                 <li id="ProjectCalendar" runat="server"><a href="Projects/ProjectCalendar.aspx?pid=<%# ((Project)Container.DataItem).Id %>">
                                    Calendar</a></li>
                                <li id="ReportIssue" runat="server"><a href="Issues/IssueDetail.aspx?pid=<%# ((Project)Container.DataItem).Id %>">
                                    New Issue</a></li>
                                <li id="Settings" runat="server"><a href="Administration/Projects/EditProject.aspx?id=<%# ((Project)Container.DataItem).Id %>&tid=1">
                                    Settings</a></li>
                            </ul>
                        </div>
                        <div style="float: right">
                            <span class="popupTitle">Filters</span>
                            <ul style="width: 140px">
                                <li><a href='Issues/IssueList.aspx?pid=<%# ((Project)Container.DataItem).Id %>'>
                                    <asp:Label ID="Label1" runat="server" Text="All" meta:resourcekey="lblAll" /></a></li>
                                <li><a href="Issues/IssueList.aspx?pid=<%# ((Project)Container.DataItem).Id %>&amp;cr=1">
                                    Created recently</a></li>
                                <li><a href="Issues/IssueList.aspx?pid=<%# ((Project)Container.DataItem).Id %>&amp;ur=1">
                                    Updated recently</a></li>
                                <li id="AssignedUserFilter" runat="server">
                                    <asp:HyperLink ID="AssignedToUser" runat="server" meta:resourcekey="AssignedToUser">Assigned to me</asp:HyperLink></li>
                            </ul>
                        </div>
                    </asp:Panel>
                </div>
                <ul class="actions">
                    <li>
                        <asp:Label CssClass="issues" ID="OpenIssues" runat="Server" /></li>
                    <li>
                        <asp:Label ID="NextMilestoneDue" CssClass="nextMilestone" runat="Server" /></li>
                    <li>
                        <asp:Label ID="MilestoneComplete" CssClass="progressBar" runat="Server" /></li>
                </ul>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Label ID="lblMessage" Visible="False" runat="server" meta:resourcekey="lblMessage"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Left" runat="server">
    <div class="welcomemessage">
        <h2 class="title">
            <asp:Label ID="lblApplicationTitle" runat="server" meta:resourcekey="lblApplicationTitle">BugNET Issue Tracker</asp:Label></h2>
        <div class="content">
            <p>
                <asp:Label ID="WelcomeMessage" runat="Server"></asp:Label>
            </p>
        </div>
    </div>
</asp:Content>
