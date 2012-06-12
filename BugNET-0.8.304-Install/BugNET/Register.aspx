<%@ Page language="c#" Inherits="BugNET.Register" MasterPageFile="~/Shared/FullWidth.master" Title="Register" Codebehind="Register.aspx.cs" meta:resourcekey="Page" %>
<%@ Register TagPrefix="cc2" Namespace="Clearscreen.SharpHIP" Assembly="Clearscreen.SharpHIP" %>

<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="Content">
    
    <asp:CreateUserWizard ID="CreateUserWizard1"  OnCreatingUser="CreatingUser"
    runat="server" ContinueDestinationPageUrl="~/Default.aspx" UserNameLabelText="Username:" meta:resourcekey="CreateNewUserWizard"
    InstructionText="Please enter your details and confirm your password to register an account."  OnCreatedUser="CreateUserWizard1_CreatedUser" >
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep0"  runat="server">
                <ContentTemplate>
                    <table border="0">
                    <tr>
                        <td align="left" colspan="2"><h1><asp:Label ID="TitleLabel" meta:resourcekey="TitleLabel" style="color: #666" runat="server" Text="Sign up for your new account"></asp:Label></h1></td>
                    </tr>
                        <tr>
                            
                            <td align="center" colspan="2" style="height: 35px">
                                <asp:Label ID="InstructionsLabel" runat="server"  
                                meta:resourcekey="InstructionsLabel" Text="Please enter your details and confirm your password to register an account."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="UserNameLabel" runat="server"  
                                Text="<%$ Resources:CommonTerms, Username %>" AssociatedControlID="UserName">Username:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"  meta:resourcekey="UserNameRequired" ControlToValidate="UserName"
                                    ErrorMessage="Username is required." ToolTip="Username is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="FirstNameLabel" runat="server" Text="<%$ Resources:CommonTerms, FirstName %>" AssociatedControlID="FirstName">First Name:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" meta:resourcekey="FirstNameRequired"  ControlToValidate="FirstName"
                                    ErrorMessage="First Name is required." ToolTip="First Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="LastNameLabel" runat="server" Text="<%$ Resources:CommonTerms, LastName %>" AssociatedControlID="FirstName">Last Name:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" meta:resourcekey="LastNameRequired" ControlToValidate="LastName"
                                    ErrorMessage="Last Name is required." ToolTip="Last Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="FullNameLabel" runat="server" AssociatedControlID="FullName" meta:resourcekey="FullNameLabel">Display Name:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="FullName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="FullNameRequired" runat="server" meta:resourcekey="FullNameRequired" ControlToValidate="FullName"
                                    ErrorMessage="Full Name is required." ToolTip="Full Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="PasswordLabel" runat="server" Text="<%$ Resources:CommonTerms, Password %>" AssociatedControlID="Password">Password:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" meta:resourcekey="PasswordRequired" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" Text="<%$ Resources:CommonTerms, ConfirmPassword %>" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" meta:resourcekey="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                    ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required."
                                    ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                     
                        <tr>
                            <td align="right">
                                <asp:Label ID="EmailLabel" runat="server" meta:resourcekey="EmailLabel" AssociatedControlID="Email">E-mail:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired"  meta:resourcekey="EmailRequired" runat="server"  Display="Dynamic" ControlToValidate="Email"
                                    ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                 <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  ValidationGroup="CreateUserWizard1"
                                    ControlToValidate="Email" ErrorMessage="Invalid Email Format" Text="Invalid Email Format" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">
                                    Security Question:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="Question" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" ControlToValidate="Question"
                                    ErrorMessage="<%$ Resources:SecurityQuestionRequiredErrorMessage %>" ToolTip="Security question is required."
                                    ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">
                                    Security Answer:</asp:Label></td>
                            <td>
                                <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer"
                                    ErrorMessage="<%$ Resources: SecurityAnswerRequiredErrorMessage %>" ToolTip="Security answer is required."
                                    ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ErrorMessage="<%$ Resources:CommonTerms, ConfirmPasswordErrorMessage %>" ControlToCompare="Password"
                                    ControlToValidate="ConfirmPassword" Display="Dynamic"
                                    ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                            </td>
                        </tr>
                         <tr>
	                       <td colspan="2">
	                       <br />
	                       <cc2:hipcontrol id="CapchaTest" runat="server" 
                                   TrustAuthenticatedUsers="False" AutoRedirect="False" 
                                JavascriptURLDetection="False" ValidationMode="ViewState" Width="400px">
                                
                                </cc2:hipcontrol></td>
	                    </tr>
                        <tr>
                            <td align="center" colspan="2" style="color: red">
                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep runat="server">
            </asp:CompleteWizardStep>
        </WizardSteps>
        <TitleTextStyle Font-Bold="True" HorizontalAlign="Left" Height="50px" Font-Size="18px" />
        <InstructionTextStyle Height="35px" />
    </asp:CreateUserWizard>
</asp:Content>
			


