<%@ Page Language="C#" MasterPageFile="~/Shared/FullWidth.master" AutoEventWireup="true" Inherits="BugNET.ForgotPassword" Title="Forgot Password" Codebehind="ForgotPassword.aspx.cs" meta:resourcekey="Page" %>
<%@ Register TagPrefix="cc2" Namespace="Clearscreen.SharpHIP" Assembly="Clearscreen.SharpHIP" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
    <asp:PasswordRecovery ID="PasswordRecovery1" OnSendingMail="PasswordRecovery1_SendingMail"  maildefinition-from="userAdmin@your.site.name.here"  Runat="server">
        <MailDefinition From="userAdmin@your.site.name.here"></MailDefinition>
        <QuestionTemplate>            
            <h1><asp:Label ID="lblIdentityConfirmation" runat="server" Text="Identity Confirmation" style="color: #666" /></h1>          
            <p style="margin-top:1.5em;">Answer the following question to receive your password.</p>
            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
            <div class="fieldgroup" style="border:none;">
                <ol>                           
                    <li>
                        <span class="label">User Name:</span>
                        <asp:Literal ID="UserName" runat="server"></asp:Literal>
                    </li>
                    <li> 
                        <span class="label">Question:</span>
                        <asp:Literal ID="Question" runat="server"></asp:Literal>
                    </li>
                    <li>
                        <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Answer:</asp:Label>
                        <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" 
                            ControlToValidate="Answer" ErrorMessage="Answer is required." 
                            ToolTip="Answer is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                    </li>
                </ol>
            </div>
            <div class="submit">
                <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" 
                            ValidationGroup="PasswordRecovery1" />
            </div>     
        </QuestionTemplate>
        <SuccessTemplate>
            <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                <tr>
                    <td>
                        <table cellpadding="0">
                            <tr>
                                <td align="left" colspan="2" style="padding-bottom:10px">
                                    <h1><asp:Label ID="lblPasswordSentSuccess" runat="server" Text="Password Sent Succuessfully" style="color: #666" /></h1></td>
                            </tr>
                            <tr>
                                <td>Your password has been sent to you.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </SuccessTemplate>
        <UserNameTemplate>
            <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                <tr>
                    <td>
                        <table cellpadding="0">
                            <tr>
                                <td align="left" colspan="2" style="padding-bottom:10px"><h1> <asp:Label ID="lblTitle" runat="server" Text="Forgot Your Password?" style="color: #666" meta:resourcekey="lblTitle" ></asp:Label></h1></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblInstructions" runat="server" Text="Enter your Username to receive your password." meta:resourcekey="lblInstructions" /></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="UserName" ErrorMessage="<%$ Resources:CommonTerms, UsernameRequired.ErrorMessage %>" 
                                        ToolTip="<%$ Resources:CommonTerms, UsernameRequired.ErrorMessage %>" ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="<%$ Resources:CommonTerms, Submit %>" 
                                        ValidationGroup="PasswordRecovery1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </UserNameTemplate>
    </asp:PasswordRecovery>

</asp:Content>

