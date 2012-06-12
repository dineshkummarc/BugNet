<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" MasterPageFile="~/Shared/TwoColumn.master"
    Title="User Profile" Inherits="BugNET.UserProfile" meta:resourcekey="Page1" %>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="PageTitle">
    <h1 class="page-title">
        User Profile -
        <asp:Literal ID="litUserName" runat="Server" meta:resourcekey="litUserName1" /></h1>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Content">
    <asp:MultiView ID="ProfileView" ActiveViewIndex="0" runat="server">
        <asp:View ID="UserDetails" runat="server">
            <bn:Message ID="Message1" runat="server" Width="650px" Visible="False" />
            <h2>
                <asp:Label ID="Label6" runat="server" meta:resourcekey="TreeNode1"></asp:Label></h2>
            <table class="form" style="width: 500px" border="0" summary="update profile table">
                <tr>
                    <td>
                        <asp:Label ID="Label2" AssociatedControlID="UserName" runat="server" Text="<%$ Resources:CommonTerms, Username %>" />
                    </td>
                    <td class="field disabled">
                        <asp:TextBox ID="UserName" ReadOnly="True" Enabled="false" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic"
                            Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="UserName" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" AssociatedControlID="FirstName" runat="server" Text="<%$ Resources:CommonTerms, FirstName %>" />
                    </td>
                    <td class="field">
                        <asp:TextBox ID="FirstName" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic"
                            Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="FirstName" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" AssociatedControlID="LastName" runat="server" Text="<%$ Resources:CommonTerms, LastName %>" />
                    </td>
                    <td class="field">
                        <asp:TextBox ID="LastName" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic"
                            Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="LastName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" AssociatedControlID="FullName" runat="server" Text="<%$ Resources:CommonTerms, DisplayName %>" />
                    </td>
                    <td class="field">
                        <asp:TextBox ID="FullName" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic"
                            Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="FullName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" AssociatedControlID="Email" runat="server" Text="<%$ Resources:CommonTerms, Email %>"
                            meta:resourcekey="Label41" />
                    </td>
                    <td class="field">
                        <asp:TextBox ID="Email" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic"
                            Text="<%$ Resources:CommonTerms, Required %>" ControlToValidate="Email" />
                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                            ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ControlToValidate="Email" ErrorMessage="Invalid Email Format" Text="Invalid Email Format" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="input-group">
                        <div style="margin: 2em 0 0 0; border-top: 1px solid #ddd; padding-top: 5px;">
                            <asp:ImageButton runat="server" ID="Image2" OnClick="SaveButton_Click" CssClass="icon"
                                ImageUrl="~/Images/disk.gif" />
                            <asp:LinkButton ID="SaveButton" runat="server" CssClass="button" OnClick="SaveButton_Click"
                                Text="<%$ Resources:CommonTerms, Save %>" />
                            <asp:ImageButton runat="server" ID="Image4" OnClick="BackButton_Click" CausesValidation="false"  CssClass="icon"
                                ImageUrl="~/Images/lt.gif" />
                            <asp:LinkButton ID="BackButton" runat="server" CssClass="button" CausesValidation="false" OnClick="BackButton_Click"
                                Text="<%$ Resources:CommonTerms, Return %>" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="ManagePassword" runat="server">
            <bn:Message ID="Message2" runat="server" Width="650px" Visible="False" />
            <h2>
                <asp:Label ID="Label9" runat="server" meta:resourcekey="TreeNode2"></asp:Label></h2>
            <div style="font-weight: bold; margin-bottom: 10px;">
                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:CommonTerms, ChangePassword %>"></asp:Label></div>
            <table style="width: 650px; margin-top: 1em;" border="0" summary="update password table">
                <tr>
                    <td>
                        <asp:Label ID="Label7" runat="server" AssociatedControlID="CurrentPassword">Enter your old password:</asp:Label><br />
                    </td>
                    <td>
                        <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CurrentPassword"
                            SetFocusOnError="True" ErrorMessage="Password is required." ToolTip="Password is required."
                            ValidationGroup="pwdReset">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label11" AssociatedControlID="NewPassword" runat="server" Text="Enter your new password:"
                            meta:resourcekey="Label111" />
                    </td>
                    <td class="field">
                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" meta:resourcekey="NewPassword1" />
                        <asp:RequiredFieldValidator ID="rfvPassword" ValidationGroup="pwdReset" runat="server"
                            ControlToValidate="NewPassword" SetFocusOnError="True" Display="Dynamic">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmPassword" AssociatedControlID="ConfirmPassword" runat="server"
                            Text="<%$ Resources:ConfirmPassword %>" />
                    </td>
                    <td class="field">
                        <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="pwdReset"
                            runat="server" ControlToValidate="ConfirmPassword" SetFocusOnError="True" Display="Dynamic">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvPasswords" runat="server" SetFocusOnError="True" ControlToValidate="NewPassword"
                            ControlToCompare="ConfirmPassword" ValidationGroup="pwdReset" ErrorMessage="<%$ Resources:CommonTerms, ConfirmPasswordErrorMessage %>"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="font-weight: bold; margin: 2em 0 2em 0;">
                            <asp:Label ID="Label14" runat="server" Text="Security Question"></asp:Label></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="SecurityQuestion">
                            Security Question:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="SecurityQuestion" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" ControlToValidate="SecurityQuestion"
                            SetFocusOnError="True" ErrorMessage="<%$ Resources:SecurityQuestionRequiredErrorMessage %>"
                            ToolTip="Security question is required." ValidationGroup="pwdReset">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="SecurityAnswer">
                            Security Answer:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="SecurityAnswer" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="SecurityAnswer"
                            SetFocusOnError="True" ErrorMessage="<%$ Resources: SecurityAnswerRequiredErrorMessage %>"
                            ToolTip="Security answer is required." ValidationGroup="pwdReset">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="input-group">
                        <div style="margin: 2em 0 0 0; border-top: 1px solid #ddd; padding-top: 5px;">
                            <asp:ImageButton runat="server" ID="ImageButton5" ValidationGroup="pwdReset" OnClick="SavePasswordSettings_Click"
                                CssClass="icon" ImageUrl="~/Images/disk.gif" />
                            <asp:LinkButton ID="LinkButton4" OnClick="SavePasswordSettings_Click" ValidationGroup="pwdReset"
                                runat="server" Text="<%$ Resources:CommonTerms, Save %>" />
                            <asp:ImageButton runat="server" ID="ImageButton6" OnClick="BackButton_Click" CssClass="icon"
                                ImageUrl="~/Images/lt.gif" meta:resourcekey="ImageButton11" />
                            <asp:LinkButton ID="Linkbutton5" runat="server" CssClass="button" OnClick="BackButton_Click"
                                Text="<%$ Resources:CommonTerms, Return %>" />
                        </div>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:PasswordStrength ID="PS" runat="server" TargetControlID="NewPassword"
                DisplayPosition="RightSide" StrengthIndicatorType="Text" PreferredPasswordLength="10"
                PrefixText="Strength:" TextCssClass="TextIndicator_TextBox1" MinimumNumericCharacters="0"
                MinimumSymbolCharacters="0" RequiresUpperAndLowerCaseCharacters="false" TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
                CalculationWeightings="50;15;15;20" />
        </asp:View>
        <asp:View ID="Customize" runat="server">
            <bn:Message ID="Message3" runat="server" Width="650px" Visible="False" />
            <h2>
                <asp:Label ID="Label10" runat="server" meta:resourcekey="TreeNode3"></asp:Label></h2>
            <table class="form" style="width: 500px; margin-top: 0;" border="0" summary="update customize table">
                <tr>
                    <td colspan="2" style="font-weight: bold; margin-bottom: 10px; height: 17px;">
                        Issue List
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" AssociatedControlID="IssueListItems" runat="server" Text="Issue Page Size:"
                            meta:resourcekey="lblItemPageSize" />
                    </td>
                    <td>
                        <asp:DropDownList ID="IssueListItems" runat="server">
                            <asp:ListItem Text="5" Value="5" />
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="15" Value="15" />
                            <asp:ListItem Text="25" Value="25" />
                            <asp:ListItem Text="50" Value="50" />
                            <asp:ListItem Text="75" Value="75" />
                            <asp:ListItem Text="100" Value="100" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="font-weight: bold; margin-bottom: 10px; height: 17px;">
                        <br />
                        Preferences
                    </td>
                </tr>
                <tr>
                    <td>
                        Preferred Locale:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPreferredLocale" DataTextField="Text" DataValueField="Value"
                            runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding: 1em   0 1em 5px" class="input-group">
                        <div style="margin: 2em 0 0 0; border-top: 1px solid #ddd; padding-top: 5px;">
                            <asp:ImageButton runat="server" ID="ImageButton2" OnClick="SaveCustomSettings_Click"
                                CssClass="icon" ImageUrl="~/Images/disk.gif" />
                            <asp:LinkButton ID="SaveCustomSettings" OnClick="SaveCustomSettings_Click" runat="server"
                                Text="<%$ Resources:CommonTerms, Save %>" />
                            <asp:ImageButton runat="server" ID="ImageButton1" OnClick="BackButton_Click" CssClass="icon"
                                ImageUrl="~/Images/lt.gif" meta:resourcekey="ImageButton11" />
                            <asp:LinkButton ID="Linkbutton1" runat="server" CssClass="button" OnClick="BackButton_Click"
                                Text="<%$ Resources:CommonTerms, Return %>" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="Notifications" runat="server">
            <bn:Message ID="Message4" runat="server" Width="650px" Visible="False" />
            <h2>
                <asp:Label ID="Label13" runat="server" meta:resourcekey="TreeNode4" Text="Notifications"></asp:Label></h2>
            <table style="width: 650px; margin-top: 1em;" border="0" summary="update customize table">
                <tr>
                    <td colspan="3">
                        Receive notifications for projects:<br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="font-weight: bold">
                        All Projects
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td style="font-weight: bold">
                        Selected Projects
                    </td>
                </tr>
                <tr>
                    <td style="height: 108px; width: 110px;">
                        <asp:ListBox ID="lstAllProjects" SelectionMode="Multiple" runat="Server" Width="150"
                            Height="110px" />
                    </td>
                    <td style="height: 108px;">
                        <asp:Button Text="->" CssClass="button" Style="font: 9pt Courier" runat="server"
                            ID="Button1" OnClick="AddProjectNotification" />
                        <br />
                        <asp:Button Text="<-" CssClass="button" Style="font: 9pt Courier; clear: both;" runat="server"
                            ID="Button2" OnClick="RemoveProjectNotification" />
                    </td>
                    <td style="height: 108px;">
                        <asp:ListBox ID="lstSelectedProjects" SelectionMode="Multiple" runat="Server" Width="150"
                            Height="110px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <br />
                        Receive notifications by:
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:CheckBoxList ID="CheckBoxList1" RepeatColumns="4" RepeatDirection="Horizontal"
                            runat="server">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="input-group">
                        <div style="margin: 2em 0 0 0; border-top: 1px solid #ddd; padding-top: 5px;">
                            <asp:ImageButton runat="server" ID="ImageButton3" OnClick="SaveNotificationsButton_Click"
                                CssClass="icon" ImageUrl="~/Images/disk.gif" />
                            <asp:LinkButton ID="Linkbutton2" runat="server" CssClass="button" OnClick="SaveNotificationsButton_Click"
                                Text="<%$ Resources:CommonTerms, Save %>" />
                            <asp:ImageButton runat="server" ID="ImageButton4" OnClick="BackButton_Click" CssClass="icon"
                                ImageUrl="~/Images/lt.gif" />
                            <asp:LinkButton ID="Linkbutton3" runat="server" CssClass="button" OnClick="BackButton_Click"
                                Text="<%$ Resources:CommonTerms, Return %>" />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="Left">
    <asp:BulletedList ID="BulletedList4" DisplayMode="LinkButton" CssClass="sideMenu"
        runat="server" OnClick="BulletedList4_Click1">
        <asp:ListItem style="background-image: url('images/user_edit.gif')">User Details</asp:ListItem>
        <asp:ListItem style="background-image: url('images/key.gif')">Password</asp:ListItem>
        <asp:ListItem style="background-image: url('images/application_edit.gif')">Customize</asp:ListItem>
        <asp:ListItem style="background-image: url('images/email_go.gif')">Notifications</asp:ListItem>
    </asp:BulletedList>
</asp:Content>
