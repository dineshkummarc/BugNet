<%@ Page language="c#" Inherits="BugNET.NotFound" MasterPageFile="~/Shared/FullWidth.master" Title="Resource Not Found" Codebehind="NotFound.aspx.cs" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Content">
	<div style="padding:5px;">
	    <h1><asp:Label id="lblTitle" runat="server" Text="Resource Not Found" /></h1>
        
		 <p style="margin-top:1em"><asp:Label id="Label1" runat="server" Text="The resource you were looking for is missing.<Br />You could have been looking for an item which has been changed or deleted." /></p>
		 <p style="margin-top:1em"><asp:Label id="Label2" runat="server" Text="Please <a href='../Default.aspx'>Return to the home page</a> and contact the  administrator." /></p>
	</div>
</asp:Content>
	
