<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="POP3Settings.ascx.cs" Inherits="BugNET.Administration.Host.UserControls.POP3Settings" %>

<h2><asp:literal ID="Title" runat="Server" Text="<%$ Resources:POP3Settings %>"  /></h2>
 <table class="form">
    <tr>
        <td><asp:Label ID="label13" runat="server" AssociatedControlID="POP3ReaderEnabled"  Text="<%$ Resources:Enable %>" /></td>
        <td class="input-group"> <asp:checkbox id="POP3ReaderEnabled" runat="server"></asp:checkbox></td>
    </tr>
     <tr>
        <td><asp:Label ID="label14" runat="server" AssociatedControlID="POP3Server" Text="<%$ Resources:Server %>" /></td>
        <td><asp:TextBox id="POP3Server" Runat="Server" Width="200px" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label15" runat="server" AssociatedControlID="POP3Username" Text="<%$ Resources:CommonTerms, Username %>" /></td>
        <td><asp:TextBox id="POP3Username" Runat="Server" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label16" runat="server" AssociatedControlID="POP3Password" Text="<%$ Resources:CommonTerms, Password %>" /></td>
        <td><asp:TextBox id="POP3Password" TextMode="Password"  Runat="Server" /></td>
    </tr>
      <tr>
        <td><asp:Label ID="label29" runat="server" AssociatedControlID="POP3UseSSL" CssClass="col1b" Text="<%$ Resources:SSL %>" /></td>
        <td class="input-group"><asp:CheckBox ID="POP3UseSSL" runat="server" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label17" runat="server" AssociatedControlID="POP3Interval" Text="<%$ Resources:PollingInterval %>" /></td>
        <td><asp:TextBox id="POP3Interval" Runat="Server" Width="50px" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label18" runat="server" AssociatedControlID="POP3DeleteMessages" Text="<%$ Resources:DeleteProcessedMessages %>" /></td>
        <td class="input-group"><asp:checkbox id="POP3DeleteMessages" runat="server"></asp:checkbox></td>
    </tr>
     <tr>
        <td><asp:Label ID="label19" runat="server" AssociatedControlID="POP3ProcessAttachments" Text="<%$ Resources:ProcessAttachments %>" /></td>
        <td class="input-group"><asp:checkbox id="POP3ProcessAttachments" runat="server"></asp:checkbox></td>
    </tr>
     <tr>
        <td><asp:Label ID="label20" runat="server" AssociatedControlID="POP3BodyTemplate" Text="<%$ Resources:BodyTemplate %>" /></td>
        <td><asp:TextBox id="POP3BodyTemplate" Runat="Server" Width="300px" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label21" runat="server" AssociatedControlID="POP3ReportingUsername" Text="<%$ Resources:ReportingUsername %>" /></td>
        <td><asp:TextBox id="POP3ReportingUsername" Runat="Server" /></td>
    </tr>
</table>  