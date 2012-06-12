<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Shared/FullWidth.master" Title="Login" CodeBehind="Login.aspx.cs" Inherits="BugNET.Login" %>
<%@ Register Assembly="DotNetOpenAuth" Namespace="DotNetOpenAuth.OpenId.RelyingParty" TagPrefix="rp" %>

<%@ Register src="UserControls/LoginControl.ascx" tagname="LoginControl" tagprefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Content">    
    <h1>Login</h1>
    <asp:Panel ID="LoginPanel" runat="server">
        <asp:Label ID="loginFailedLabel" runat="server" EnableViewState="False" 
            Text="Login failed" Visible="False" Font-Bold="True" ForeColor="Red" />
        <asp:Label ID="loginCanceledLabel" runat="server" EnableViewState="False" 
            Text="Login canceled" Visible="False" Font-Bold="True" ForeColor="Red" />
      

        <asp:Panel ID="pnlOpenIDLogin" runat="server" Visible="false">
            <p  style="margin:1em 0 2em 0">
                <strong><asp:LinkButton ID="ShowNormal" runat="server" onclick="btnShowNormal_Click" Text="Standard Login" /></strong>
            </p>
            <p>
                <b>You can login using your OpenID</b></p>
                <rp:OpenIdLogin ID="OpenIdLogin1" runat="server" EnableRequestProfile="true" RequestNickname="Require"
                    OnLoggedIn="OpenIdLogin1_LoggedIn" RequestEmail="Require"  
                    RequestFullName="Require" />
        </asp:Panel>
        <asp:Panel ID="pnlNormalLogin" runat="server" Visible="true">
            <p style="margin:1em 0 2em 0">
                <strong><asp:LinkButton ID="btnShowOpenID" runat="server" onclick="btnShowOpenID_Click" Text="OpenID Login" /></strong>
            </p>
            <p>
                <b>You can login with your member account</b>
            </p>
            <uc1:LoginControl ID="LoginControl1" runat="server" />
        </asp:Panel>
        <asp:Panel ID="RegisterPanel" runat="server">
            <h2>OR</h2>
            <p>
                <b>If you don&#39;t have an account yet you can
                <asp:HyperLink ID="CreateUserLink" runat="server" NavigateUrl="~/Register.aspx">click here to get one</asp:HyperLink>
                .</b></p>
        </asp:Panel>
    </asp:Panel>
    
    <p><asp:Label ID="lblLoggedIn" runat="server" Visible="false"><br />You are already logged in.</asp:Label></p>
    <p>&nbsp;</p>

</asp:Content>
