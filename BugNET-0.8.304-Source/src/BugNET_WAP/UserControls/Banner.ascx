<%@ Control Language="C#" AutoEventWireup="true" Inherits="BugNET.UserControls.Banner" CodeBehind="Banner.ascx.cs" %>
<%@ Register Src="tabmenu.ascx" TagName="tabmenu" TagPrefix="uc1" %>
<div id="dashboard">
    <asp:LoginView ID="LoginView1" runat="server">
        <LoggedInTemplate>
            <span style="padding-left: 15px;">
                <asp:LinkButton ID="Profile" runat="server" OnClick="Profile_Click" CausesValidation="false">
                    <asp:Image ID="EditProfile" runat="server" CssClass="icon" Style="padding-right: 2px;" Visible="false" ImageUrl="~/images/user.gif" />
                    <asp:LoginName ID="LoginName1" FormatString="{0}" runat="server" />
                </asp:LinkButton>
                <asp:Label ID="lblBar" runat="server" Text=" | " />
                <asp:LoginStatus ID="LoginStatus1" LogoutPageUrl="~/Default.aspx" LogoutAction="Redirect" runat="server" />
            </span>
        </LoggedInTemplate>
        <AnonymousTemplate>
            <span style="padding-left: 15px;">
                <asp:HyperLink ID="lnkRegister" NavigateUrl="~/Register.aspx" runat="server" Text="Register"></asp:HyperLink>
                <asp:Label ID="lblBar" runat="server" Text=" | " />
                <asp:LoginStatus ID="LoginStatus1" runat="server" />
            </span>
        </AnonymousTemplate>
    </asp:LoginView>
    <asp:Panel ID="Panel1" runat="server">
        <p id="search">
            <asp:Label ID="QuickError" Visible="false" runat="server" ForeColor="Red" />
            <asp:HyperLink ID="lnkAdvSearch" runat="server" NavigateUrl="~/Issues/IssueSearch.aspx">Advanced Search</asp:HyperLink>
            &nbsp;| &nbsp;<asp:TextBox ID="txtIssueId" Height="12" Width="40" runat="Server" />
            <asp:LinkButton ID="btnSearch" OnClick="btnSearch_Click" CausesValidation="false" runat="server" Text="Find" />
            &nbsp;|
        </p>
    </asp:Panel>
    <p id="help">
        <a target="_blank" href="http://bugnetproject.com/Documentation/tabid/57/topic/User%20Guide/Default.aspx">Help</a>
    </p>
</div>
<div id="header">
    <h1 class="title">
        <asp:HyperLink ID="lnkLogo" runat="server" NavigateUrl="~/Default.aspx">
            <asp:Image CssClass="logo" runat="server" SkinID="Logo" ID="Logo" AlternateText="Logo" /><asp:Literal ID="AppTitle" runat="server"></asp:Literal>
        </asp:HyperLink></h1>
    <div id="tabsB">
        <uc1:tabmenu ID="Tabmenu1" runat="server" />
    </div>
</div>
<asp:Panel ID="pnlHeaderNav" CssClass="header-nav" runat="server">
    <asp:DropDownList CssClass="header-nav-ddl" ID="ddlProject" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="true"
        runat="server">
        <asp:ListItem Text="BugNET" Value="BugNET"></asp:ListItem>
    </asp:DropDownList>
</asp:Panel>
<div class="breadcrumb">
    <asp:UpdateProgress ID="progress1" runat="server">
        <ProgressTemplate>
            <div class="progress">
                <asp:Image ID="Image1" runat="Server" CssClass="icon" ImageUrl="~/images/indicator.gif" AlternateText="Indicator" />
                Please Wait...
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:SiteMapPath ID="SiteMapPath1" PathSeparator=" > " runat="server" />
</div>
