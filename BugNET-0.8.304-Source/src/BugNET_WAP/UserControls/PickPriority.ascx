<%@ Control Language="c#" Inherits="BugNET.UserControls.PickPriority" Codebehind="PickPriority.ascx.cs" %>
<asp:DropDownList id="ddlPriority" runat="Server" />
<asp:RequiredFieldValidator id="reqVal" Visible="false" Display="dynamic" ControlToValidate="ddlPriority" InitialValue="0" Text="(required)"
	Runat="Server" meta:resourcekey="reqVal" />
