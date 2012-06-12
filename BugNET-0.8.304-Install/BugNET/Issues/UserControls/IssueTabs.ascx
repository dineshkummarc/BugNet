<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IssueTabs.ascx.cs" Inherits="BugNET.Issues.UserControls.IssueTabs" %>
<asp:UpdatePanel ID="IssueTabsUpdatePanel" runat="server">
    <ContentTemplate>
        <asp:DataList id="lstTabs" CellSpacing="0" style="position:relative;top:2px;" CellPadding="0" RepeatDirection="Horizontal" 
            OnItemDataBound="lstTabs_ItemDataBound" OnItemCommand="lstTabs_ItemCommand" Runat="Server">
	        <ItemStyle CssClass="adminTabInactive" />
	        <SelectedItemStyle CssClass="adminTabActive" />
	        <ItemTemplate>
                <asp:Image ID="TabIcon" CssClass="icon" runat="server" />&nbsp;<asp:LinkButton id="lnkTab" CausesValidation="false" Runat="Server" />
	        </ItemTemplate>
	        <SelectedItemTemplate>
		        <asp:Image ID="TabIcon" CssClass="icon" runat="server" />&nbsp;<asp:LinkButton id="lnkTab" CausesValidation="false" Runat="Server" />
	        </SelectedItemTemplate>
        </asp:DataList>
        <div style="padding:2em;border:1px solid #D5D291;">    
            <asp:PlaceHolder id="plhContent" Runat="Server" />			
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
