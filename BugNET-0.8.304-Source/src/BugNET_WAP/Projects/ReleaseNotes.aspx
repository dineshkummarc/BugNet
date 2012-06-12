<%@ Page Title="Release Notes" Language="C#" meta:resourcekey="Page" ValidateRequest="false"
    MasterPageFile="~/Shared/FullWidth.Master" AutoEventWireup="true" CodeBehind="ReleaseNotes.aspx.cs"
    Inherits="BugNET.Projects.ReleaseNotes" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="server">
    <h1 class="page-title">
        <asp:Literal ID="Literal1" runat="server" />
        -
        <asp:Literal EnableViewState="true" ID="litProject" runat="server" />
        -
        <asp:Literal EnableViewState="true" ID="litMilestone" runat="server" /></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
    <asp:Repeater runat="server" ID="rptReleaseNotes" OnItemDataBound="rptReleaseNotes_ItemDataBound">
        <ItemTemplate>
            <h2>
                <asp:Literal ID="IssueType" runat="server"></asp:Literal></h2>
            <asp:Repeater ID="IssuesList" runat="server" OnItemDataBound="IssueList_ItemDataBound">
                <HeaderTemplate>
                    <ul style="margin-left: 35px">
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <asp:Literal ID="Issue" runat="server"></asp:Literal></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <h2>
        <asp:Literal ID="Literal2" runat="server" meta:resourcekey="EditCopyNotes" /></h2>
    <p>
        <asp:Literal ID="Literal3" runat="server" meta:resourcekey="EditCopyNotesDesc" /></p>
    <asp:TextBox ID="Output" runat="server" Width="600px" Height="450px" TextMode="MultiLine"
        EnableViewState="false"></asp:TextBox>
</asp:Content>
