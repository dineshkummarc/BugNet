<%@ Page Language="C#" MasterPageFile="~/Shared/FullWidth.Master" AutoEventWireup="true"
    CodeBehind="AddUser.aspx.cs" Inherits="BugNET.Administration.Users.AddUser" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
    <h1 class="page-title">
        <asp:Literal ID="Title" runat="Server" Text="<%$ Resources:AddUser %>" /></h1>
    <table class="form">
        <tr>
            <td colspan="2">
                <bn:Message ID="Message1" runat="server" Visible="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-bottom: 5px;">
                <asp:Label ID="DescriptionLabel" runat="server" meta:resourcekey="DescriptionLabel" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" AssociatedControlID="UserName" runat="server" Text="<%$ Resources:CommonTerms, Username %>" />
            </td>
            <td>
                <asp:TextBox ID="UserName" runat="server" />
                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" Text="<%$ Resources:CommonTerms, Required %>"
                    Display="Dynamic" ControlToValidate="UserName"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" AssociatedControlID="FirstName" runat="server" Text="<%$ Resources:CommonTerms,FirstName %>" />
            </td>
            <td>
                <asp:TextBox ID="FirstName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="<%$ Resources:CommonTerms, Required %>"
                    ErrorMessage="*" ControlToValidate="FirstName" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" CssClass="col1" AssociatedControlID="LastName" runat="server"
                    Text="<%$ Resources:CommonTerms,LastName %>" />
            </td>
            <td>
                <asp:TextBox ID="LastName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="<%$ Resources:CommonTerms, Required %>"
                    ErrorMessage="*" ControlToValidate="LastName" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" CssClass="col1" AssociatedControlID="DisplayName" runat="server"
                    Text="<%$ Resources:CommonTerms,DisplayName %>" />
            </td>
            <td>
                <asp:TextBox ID="DisplayName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                    Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="DisplayName"
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label40" CssClass="col1" AssociatedControlID="Email" runat="server"
                    Text="<%$ Resources:CommonTerms,Email %>" />
            </td>
            <td>
                <asp:TextBox ID="Email" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                    Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="Email" Display="Dynamic"
                    Enabled="false"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    ControlToValidate="Email" ErrorMessage="Invalid Email Format" Text="Invalid Email Format" />
            </td>
        </tr>
        <tr id="SecretQuestionRow" runat="server" visible="false">
            <td>
                <asp:Label ID="Label4" runat="server" AssociatedControlID="SecretQuestion" Text="<%$ Resources:SecurityQuestion %>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="SecretQuestion" MaxLength="128" TabIndex="104" Columns="30" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Text="<%$ Resources:CommonTerms, Required %>"
                    ControlToValidate="SecretQuestion" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="SecretAnswerRow" runat="server" visible="false">
            <td>
                <asp:Label ID="Label6" runat="server" AssociatedControlID="SecretAnswer" Text="<%$ Resources:SecretAnswer %>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="SecretAnswer" MaxLength="128" TabIndex="105" Columns="30" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Text="<%$ Resources:CommonTerms, Required %>"
                    ControlToValidate="SecretAnswer" Display="Dynamic" EnableClientScript="true"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="padding: 10px 0 3px 0px;">
                <strong>
                    <asp:Literal ID="Literal1" runat="Server" Text="<%$ Resources:CommonTerms,Password %>" /></strong>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="white-space: normal; padding-bottom: 5px;">
                <asp:Literal ID="Literal2" runat="Server" Text="<%$ Resources:PasswordDescription %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label10" AssociatedControlID="chkRandomPassword" runat="server" Text="<%$ Resources:RandomPassword %>" />
            </td>
            <td class="input-group">
                <asp:CheckBox ID="chkRandomPassword" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label42" AssociatedControlID="Password" runat="server" Text="<%$ Resources:CommonTerms,Password %>" />
            </td>
            <td>
                <asp:TextBox ID="Password" TextMode="password" runat="server" />
                <asp:RequiredFieldValidator ID="rvPassword" runat="server" ErrorMessage="*" EnableClientScript="true"
                    Enabled="false" ControlToValidate="Password" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label41" AssociatedControlID="ConfirmPassword" runat="server" Text="<%$ Resources:ConfirmPassword %>" />
            </td>
            <td>
                <asp:TextBox ID="ConfirmPassword" TextMode="password" runat="server" />
                <asp:RequiredFieldValidator ID="rvConfirmPassword" runat="server" ErrorMessage="*"
                    EnableClientScript="true" Enabled="false" ControlToValidate="ConfirmPassword"
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:ActiveUser %>" />
            </td>
            <td class="input-group">
                <asp:CheckBox runat="server" ID="ActiveUser" Text="" TabIndex="106" Checked="True" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="white-space: normal; padding: 10px 0 10px 15px;">
                <asp:CompareValidator ID="cvPassword" EnableClientScript="false" Enabled="false"
                    Display="dynamic" ControlToCompare="ConfirmPassword" ControlToValidate="Password"
                    runat="server" ErrorMessage="<%$ Resources:ConfirmPasswordError %>"></asp:CompareValidator>
            </td>
            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
        </tr>
    </table>
    <p>
        <asp:ImageButton runat="server" ImageUrl="~/Images/disk.gif" CssClass="icon" AlternateText="<%$ Resources:AddNewUser %>"
            OnClick="AddNewUser_Click" ID="ImageButton2" />
        <asp:LinkButton ID="LinkButton2" runat="server" Text="<%$ Resources:AddNewUser %>"
            OnClick="AddNewUser_Click" />
        &nbsp;
        <asp:ImageButton runat="server" ImageUrl="~/Images/lt.gif" CssClass="icon" CausesValidation="false"
            AlternateText="<%$ Resources:BackToUserList %>" ID="ImageButton3" OnClick="cmdCancel_Click" />
        <asp:HyperLink ID="ReturnLink" runat="server" NavigateUrl="~/Administration/Users/UserList.aspx"
            Text="<%$ Resources:BackToUserList %>"></asp:HyperLink>
    </p>
</asp:Content>
