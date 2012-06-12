﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionExpired.aspx.cs" Title="<%$ Resources:SessionExpired %>" Inherits="BugNET.Errors.SessionExpired" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><asp:literal ID="Literal1" runat="server" Text="<%$ Resources:SessionExpired %>" /></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:15px">
            <h1><asp:literal ID="litTitle" runat="server" Text="<%$ Resources:SessionExpired %>" /></h1>
            <p style="margin-top:1em"><asp:Label id="Label1" runat="server" Text="<%$ Resources:Message %>" /></p>
        </div>
    </form>
</body>
</html>
