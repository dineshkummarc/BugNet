<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubversionSettings.ascx.cs" Inherits="BugNET.Administration.Host.UserControls.SubversionSettings" %>
<h2><asp:literal ID="Title" runat="Server" Text="<%$ Resources:SubversionSettings %>"  /></h2>
<BN:Message ID="Message1" runat="server" visible="false"  /> 
<table class="form">
    <tr>
        <td><asp:Label ID="label29" runat="server" AssociatedControlID="EnableRepoCreation" 
                Text="<%$ Resources:EnableAdministration %>" /></td>
        <td class="input-group"><asp:checkbox cssClass="checkboxlist" 
                id="EnableRepoCreation" runat="server" ></asp:checkbox></td>
    </tr>
     <tr>
        <td><asp:Label ID="label32" runat="server" AssociatedControlID="RepoRootUrl" Text="<%$ Resources:ServerRootUrl %>" /></td>
        <td><asp:TextBox id="RepoRootUrl" Runat="Server" Width="300px" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label30" runat="server" AssociatedControlID="RepoRootPath" Text="<%$ Resources:RootFolder %>" /></td>
        <td><asp:TextBox id="RepoRootPath" Runat="Server" Width="300px" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="label34" runat="server" AssociatedControlID="SvnHookPath"  Text="<%$ Resources:HooksExecutableFile %>" /></td>
        <td><asp:TextBox id="SvnHookPath" Runat="Server" Width="300px" /></td>
    </tr>
     
    <tr>
        <td><asp:Label ID="label31" runat="server" AssociatedControlID="RepoBackupPath" Text="<%$ Resources:BackupFolder %>" /></td>
        <td><asp:TextBox id="RepoBackupPath" Runat="Server" Width="300px" /></td>
    </tr>
     <tr>
        <td><asp:Label ID="label33" runat="server" AssociatedControlID="SvnAdminEmailAddress" Text="<%$ Resources:AdministratorEmail %>" /></td>
        <td><asp:TextBox id="SvnAdminEmailAddress" Runat="Server" Width="300px" /></td>
    </tr>
</table>