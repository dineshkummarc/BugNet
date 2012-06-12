<%@ Page Language="c#" Inherits="BugNET.Issues.IssueList" MasterPageFile="~/Shared/FullWidth.master"
    Title="Issue List" CodeBehind="IssueList.aspx.cs" AutoEventWireup="True" meta:resourcekey="Page" %>

<%@ Register TagPrefix="it" TagName="DisplayIssues" Src="~/UserControls/DisplayIssues.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="it" TagName="PickProject" Src="~/UserControls/PickProject.ascx" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="PageTitle">
    <script type="text/javascript">

        $(document).ready(function () {
            $('.dateField').datepick({ showOn: 'button',
                buttonImageOnly: true, buttonImage: '<%=ResolveUrl("~/Images/calendar.gif")%>'
            });
        });
	
    </script>
    <div class="centered-content">
        <h1 class="page-title">
            <asp:Label ID="lblProjectName" runat="Server" /></h1>
        <h1 class="page-title">
            <asp:Literal ID="ltProject" runat="server" Visible="false"></asp:Literal>
            <span>
                <asp:Literal ID="litProjectCode" Visible="False" runat="Server"></asp:Literal></span>
        </h1>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="ViewIssuesLabel" runat="server" Text="View Issues:" meta:resourcekey="ViewIssuesLabel"></asp:Label>
                <asp:DropDownList ID="dropView" CssClass="standardText" AutoPostBack="True" runat="Server"
                    OnSelectedIndexChanged="ViewSelectedIndexChanged">
                    <asp:ListItem Text="-- Select a View --" Value="" meta:resourcekey="ListItem6" />
                    <asp:ListItem Text="Relevant to You" Value="Relevant" meta:resourcekey="ListItem1" />
                    <asp:ListItem Text="Assigned to You" Value="Assigned" meta:resourcekey="ListItem2" />
                    <asp:ListItem Text="Owned by You" Value="Owned" meta:resourcekey="ListItem3" />
                    <asp:ListItem Text="Created by You" Value="Created" meta:resourcekey="ListItem4" />
                    <asp:ListItem Text="All Issues" Value="All" meta:resourcekey="ListItem5" />
                    <asp:ListItem Text="Open Issues" Value="Open" Selected="True" meta:resourcekey="ListItem7" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <it:DisplayIssues ID="ctlDisplayIssues" runat="server" OnRebindCommand="IssuesRebind">
                </it:DisplayIssues>
            </td>
        </tr>
    </table>
</asp:Content>
