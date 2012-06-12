<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggingSettings.ascx.cs" Inherits="BugNET.Administration.Host.UserControls.LoggingSettings" %>
<h2><asp:literal ID="Title" runat="Server" Text="<%$ Resources:LoggingSettings %>"  /></h2>
<BN:Message ID="Message1" runat="server" visible="false"  /> 
<table class="form">
    <tr>
        <td><asp:Label ID="label23" runat="server" AssociatedControlID="EmailErrors" Text="<%$ Resources:EmailErrorMessages %>" /></td>
        <td class="input-group"><asp:checkbox cssClass="checkboxlist" id="EmailErrors" runat="server"></asp:checkbox></td>
    </tr>
     <tr>
        <td><asp:Label ID="label24" runat="server" AssociatedControlID="ErrorLoggingEmail" CssClass="col1b" Text="<%$ Resources:FromAddress %>" /></td>
        <td><asp:TextBox id="ErrorLoggingEmail" Runat="Server" Width="300px" /></td>
    </tr>
</table>