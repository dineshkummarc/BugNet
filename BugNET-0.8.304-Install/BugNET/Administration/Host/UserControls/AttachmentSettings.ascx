<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttachmentSettings.ascx.cs" Inherits="BugNET.Administration.Host.UserControls.AttachmentSettings" %>
<h2><asp:literal ID="LogViewerTitle" runat="Server" Text="<%$ Resources:AttachmentSettings %>"  /></h2>
<bn:Message ID="Message1" runat="server" visible="false"  /> 
<table class="form">
     <tr>
        <td><asp:Label ID="label25" runat="server" AssociatedControlID="AllowedFileExtentions" Text="<%$ Resources:AllowedFileExtentions %>" /></td>
        <td> <asp:TextBox id="AllowedFileExtentions"  Runat="Server"  /> (seperated by semi colon)</td>
    </tr>
    <tr>
        <td><asp:Label ID="label1" runat="server" AssociatedControlID="FileSizeLimit"   Text="<%$ Resources:FileSizeLimit %>" /></td>
        <td> <asp:TextBox id="FileSizeLimit"  Runat="Server" /> (bytes)</td>
    </tr>
</table>