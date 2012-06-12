<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotificationSettings.ascx.cs" Inherits="BugNET.Administration.Host.UserControls.NotificationSettings" %>

<h2><asp:literal ID="Title" runat="Server" Text="<%$ Resources:NotificationSettings %>"  /></h2>
 <BN:Message ID="Message1" runat="server" visible="false"  /> 
 <table>
    <tr>
        <td><asp:Label ID="label1" runat="server" AssociatedControlID="AdminNotificationUser" Text="<%$ Resources:AdminNotificationUser %>" /></td>
        <td>        
            <asp:DropDownList ID="AdminNotificationUser" runat="server" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="label9" runat="server" AssociatedControlID="cblNotificationTypes"  Text="<%$ Resources:EnableNotificationTypes %>" /></td>
    </tr>
    <tr>
        <td> <asp:CheckBoxList ID="cblNotificationTypes"  RepeatColumns="4"  RepeatDirection="Horizontal" runat="server"> </asp:CheckBoxList></td>
    </tr>
</table>