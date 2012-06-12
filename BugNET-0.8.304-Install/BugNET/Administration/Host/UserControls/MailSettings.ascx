<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailSettings.ascx.cs"
    Inherits="BugNET.Administration.Host.UserControls.MailSettings" %>
<h2>
    <asp:Literal ID="Title" runat="Server" Text="<%$ Resources:MailSettings %>" /></h2>
<bn:Message ID="Message1" runat="server" Visible="false" />
<table class="form">
    <tr>
        <td>
            <asp:Label ID="label9" runat="server" AssociatedControlID="SMTPServer" Text="<%$ Resources:Server %>" />
        </td>
        <td>
            <asp:TextBox ID="SMTPServer" runat="Server" Width="300px" MaxLength="256" />
            &nbsp;
            <asp:LinkButton ID="TestEmailSettings" runat="server" OnClick="TestEmailSettings_Click">Test</asp:LinkButton>
            <asp:Label ID="lblEmail" runat="server" Font-Bold="true" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="label26" runat="server" AssociatedControlID="SMTPPort" CssClass="col1b"
                Text="<%$ Resources:Port %>" />
        </td>
        <td>
            <asp:TextBox ID="SMTPPort" Width="50px" runat="Server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="label22" runat="server" AssociatedControlID="HostEmail" Text="<%$ Resources:HostEmail %>" />
        </td>
        <td>
            <asp:TextBox ID="HostEmail" runat="Server" Width="300px" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic"
                Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="HostEmail" />
            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                ControlToValidate="HostEmail" ErrorMessage="Invalid Email Format" Text="Invalid Email Format" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="label27" runat="server" AssociatedControlID="SMTPUseSSL" CssClass="col1b"
                Text="SSL" />
        </td>
        <td class="input-group">
            <asp:CheckBox ID="SMTPUseSSL" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="label10" runat="server" AssociatedControlID="SMTPEnableAuthentication"
                Text="<%$ Resources:EnableAuthentication %>" />
        </td>
        <td class="input-group">
            <asp:CheckBox ID="SMTPEnableAuthentication" AutoPostBack="true" OnCheckedChanged="SMTPEnableAuthentication_CheckChanged"
                runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr id="trSMTPUsername" runat="server">
        <td>
            <asp:Label ID="label11" runat="server" AssociatedControlID="SMTPUsername" Text="<%$ Resources:CommonTerms, Username %>" />
        </td>
        <td>
            <asp:TextBox ID="SMTPUsername" runat="Server" />
        </td>
    </tr>
    <tr id="trSMTPPassword" runat="server">
        <td>
            <asp:Label ID="label12" runat="server" AssociatedControlID="SMTPPassword" Text="<%$ Resources:CommonTerms, Password %>" />
        </td>
        <td>
            <asp:TextBox ID="SMTPPassword" TextMode="Password" runat="Server" />
        </td>
    </tr>
    <tr id="trSMTPDomain" runat="server">
        <td>
            <asp:Label ID="label3" runat="server" AssociatedControlID="SMTPDomain" Text="Domain" />
        </td>
        <td>
            <asp:TextBox ID="SMTPDomain" runat="Server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="label1" runat="server" AssociatedControlID="SMTPEmailFormat" CssClass="col1b"
                Text="<%$ Resources:EmailFormat %>" />
        </td>
        <td class="input-group">
            <asp:RadioButtonList ID="SMTPEmailFormat" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
                <asp:ListItem Value="1" Text="Text" Selected="True" />
                <asp:ListItem Value="2" Text="HTML" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="label2" runat="server" AssociatedControlID="SMTPEmailTemplateRoot"
                CssClass="col1b" Text="<%$ Resources:EmailTemplateRoot %>" />
        </td>
        <td>
            <asp:TextBox ID="SMTPEmailTemplateRoot" runat="Server" />
        </td>
    </tr>
</table>
