﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasicSettings.ascx.cs" Inherits="BugNET.Administration.Host.UserControls.BasicSettings" %>
<h2><asp:literal ID="Title" runat="Server" Text="<%$ Resources:BasicSettings %>"  /></h2>
<BN:Message ID="Message1" runat="server" visible="false"  /> 
 <div class="fieldgroup" style="border:none;">
     <ol>
        <li>
            <asp:Label ID="label28" runat="server" AssociatedControlID="ApplicationTitle"  Text="<%$ Resources:Title %>" />
            <asp:TextBox ID="ApplicationTitle" runat="Server" MaxLength="500" 
                    Width="300px" ></asp:TextBox>
        </li>
        <li>
            <asp:Label ID="label1" runat="server" AssociatedControlID="WelcomeMessageHtmlEditor" Text="<%$ Resources:WelcomeMessage %>" />
            <bn:HtmlEditor id="WelcomeMessageHtmlEditor" Width="80%" runat="server" />
        </li>
        <li>
            <asp:Label ID="label2" runat="server" AssociatedControlID="DefaultUrl"  Text="<%$ Resources:DefaultUrl %>" />
            <asp:TextBox id="DefaultUrl" Runat="Server" Width="300px" />
        </li>
        <li>
            <asp:Label ID="label3" runat="server" AssociatedControlID="EnableGravatar"  Text="Enable Gravatar" />
            <asp:CheckBox  id="EnableGravatar"  Runat="Server" />
        </li>
     </ol>
</div>