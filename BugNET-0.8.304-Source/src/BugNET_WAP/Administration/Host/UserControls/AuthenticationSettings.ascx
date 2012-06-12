<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuthenticationSettings.ascx.cs" Inherits="BugNET.Administration.Host.UserControls.AuthenticationSettings" %>
 <h2><asp:literal ID="Title" runat="Server" Text="<%$ Resources:AuthenticationSettings %>"  /></h2>
 <table class="form">
    <tr>
        <td><asp:Label ID="label3" runat="server" AssociatedControlID="UserAccountSource" Text="<%$ Resources:UserAccountSource %>" /></td>
        <td class="input-group"><asp:radiobuttonlist RepeatDirection="Horizontal" 
            OnSelectedIndexChanged="UserAccountSource_SelectedIndexChanged" cssClass="checkboxlist" 
            AutoPostBack="true" id="UserAccountSource" runat="server">
            <asp:listitem id="option3" runat="server" value="None" />
            <asp:listitem id="option1" runat="server" value="WindowsSAM" />
            <asp:listitem id="option2" runat="server" value="ActiveDirectory" />     
        </asp:radiobuttonlist></td>
    </tr>
    <tr id="trADPath" runat="server">
        <td><asp:Label ID="label25" runat="server" AssociatedControlID="ADPath"  Text="<%$ Resources:DomainPath %>" /></td>
        <td> <asp:TextBox id="ADPath"  Runat="Server" Width="300px" /></td>
    </tr>
     <tr id="trADUserName" runat="server">
        <td><asp:Label ID="label4" runat="server" AssociatedControlID="ADUserName"  Text="<%$ Resources:CommonTerms, Username %>" /></td>
        <td><asp:TextBox id="ADUserName"  Runat="Server" /></td>
    </tr>
     <tr id="trADPassword" runat="server">
        <td><asp:Label ID="label5" runat="server" AssociatedControlID="ADPassword"  Text="<%$ Resources:CommonTerms, Password %>"/></td>
        <td><asp:TextBox TextMode="Password" id="ADPassword"  Runat="Server" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label7" runat="server" AssociatedControlID="DisableAnonymousAccess" Text="<%$ Resources:DisableAnonymousAccess %>" /></td>
        <td class="input-group"><asp:CheckBox  id="DisableAnonymousAccess"  Runat="Server" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label8" runat="server" AssociatedControlID="DisableUserRegistration" Text="<%$ Resources:DisableUserRegistration %>" /></td>
        <td class="input-group"><asp:CheckBox id="DisableUserRegistration"  Runat="Server" /></td>
    </tr> 
    <tr>
        <td><asp:Label ID="label1" runat="server" AssociatedControlID="OpenIdAuthentication" Text="<%$ Resources:OpenIdAuthentication %>" /></td>
        <td class="input-group">
            <asp:RadioButtonList ID="OpenIdAuthentication" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Text="<%$ Resources:CommonTerms, Enable %>" Value="True" />
                <asp:ListItem Text="<%$ Resources:CommonTerms, Disable %>" Value="False" Selected="True" />
            </asp:RadioButtonList>
        </td>
    </tr> 
</table>