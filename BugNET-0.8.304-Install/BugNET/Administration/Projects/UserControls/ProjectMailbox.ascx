<%@ Register TagPrefix="it" TagName="PickType" Src="~/UserControls/PickType.ascx" %>
<%@ Register TagPrefix="it" TagName="PickSingleUser" Src="~/UserControls/PickSingleUser.ascx" %>
<%@ Control Language="c#" Inherits="BugNET.Administration.Projects.UserControls.Mailboxes" Codebehind="ProjectMailbox.ascx.cs" %>

<h2>Mailboxes</h2>
<p class="desc"><asp:label id="lblMailboxes" runat="server" visible="false" /></p>
<asp:datagrid id="dtgMailboxes" runat="server" SkinID="DataGrid" Width="100%"
    CellPadding="3" GridLines="None" BorderWidth="0" autogeneratecolumns="False">
	<Columns>
		<asp:BoundColumn DataField="Mailbox" HeaderText="Email Address"></asp:BoundColumn>
		<asp:BoundColumn DataField="AssignToName" HeaderText="Assign To"></asp:BoundColumn>
		<asp:BoundColumn DataField="IssueTypeName" HeaderText="Issue Type"></asp:BoundColumn>
		<asp:ButtonColumn Text="Delete" CommandName="Delete"></asp:ButtonColumn>
	</Columns>
</asp:datagrid>
<br />
<div class="fieldgroup">  
	<h3>New Mailbox</h3>
	<ol>
        <li>
            <asp:Label ID="label1" runat="server" AssociatedControlID="txtMailbox" Text="Email Address:" />
            <asp:textbox id="txtMailbox" runat="server" enableviewstate="false"></asp:textbox> 
            <asp:RequiredFieldValidator id="reqVal" Display="dynamic" ControlToValidate="txtMailBox" Text=" (required)" Runat="Server" />
            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" 
                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                ControlToValidate="txtMailbox" ErrorMessage="Invalid Email Format" Text="Invalid Email Format" />
        </li>
        <li>
            <asp:Label ID="label2" runat="server" AssociatedControlID="IssueAssignedUser" Text="Assign To:" />
            <it:picksingleuser id="IssueAssignedUser"   Runat="Server" Required="true" DisplayDefault="true"></it:picksingleuser>
        </li>
        <li>
            <asp:Label ID="label3"  runat="server" AssociatedControlID="IssueAssignedType" Text="Issue Type:" />
            <it:picktype id="IssueAssignedType" DisplayLabel="True" Runat="Server" Required="true" DisplayDefault="true"></it:picktype>
        </li>       
    </ol>
</div>   
<div class="submit">
    <asp:Button Text="Add Mailbox" CausesValidation="true" runat="server" id="Button1" onclick="btnAdd_Click" />
</div>

