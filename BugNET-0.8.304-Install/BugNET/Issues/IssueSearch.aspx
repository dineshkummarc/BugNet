<%@ Page Language="c#" Inherits="BugNET.SearchResults" MasterPageFile="~/Shared/FullWidth.master"
    Title="Full Text Search" CodeBehind="IssueSearch.aspx.cs" %>

<%@ Register Src="~/UserControls/DisplayIssues.ascx" TagName="DisplayIssues" TagPrefix="uc1" %>
<%@ Register Src="UserControls/History.ascx" TagName="History" TagPrefix="uc2" %>
<%@ Register Assembly="BugNET" Namespace="BugNET.UserInterfaceLayer" TagPrefix="cc1" %>
<%@ Register TagPrefix="it" TagName="TextImage" Src="~/UserControls/TextImage.ascx" %>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="Content">
    <script src="../js/jquery.cookie.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.milestoneGroupHeader').click(function () {
                var li = $(this).parent();
                if (li.hasClass('active')) {
                    li.removeClass('active');
                    li.children('.milestoneContent').slideUp();
                    $.cookie(li.attr('id'), null);
                }
                else {
                    li.addClass('active');
                    li.children('.milestoneContent').slideDown();
                    $.cookie(li.attr('id'), 'expanded');
                }

            });
            $("li.expand").each(function () {
                var isExpanded = $.cookie($(this).attr('id'));
                if (isExpanded == 'expanded') {
                    $(this).addClass('active');
                    $(this).children('.milestoneContent').show();
                }
            });
        });
    </script>
    <h1>
        Full Text Search</h1>
    <p style="margin-top: 1em;">
        This will search all description and summary fields across all issues on the system,
        for which you have access.</p>
    <br />
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>&nbsp;
    <asp:Button ID="btnGlobalSearch" runat="server" OnClick="Button1_Click" Text="Search" />
    &nbsp;<asp:HyperLink ID="srchOptions" runat="server" Visible="False">Options...</asp:HyperLink>
    <asp:Panel ID="pnlOptions" runat="server">
        <asp:CheckBox ID="chkComments" runat="server" Text="Search Comments (may be slow)" />
        <br />
        <asp:CheckBox ID="chkUsername" runat="server" Text="Include username fields" Visible="False" />
        <br />
        <asp:CheckBox ID="chkCommentUsername" runat="server" OnCheckedChanged="chkCommentUsername_CheckedChanged"
            Text="Search comment usernames" Visible="False" />
        <br />
        <asp:CheckBox ID="chkHistory" runat="server" OnCheckedChanged="chkHistory_CheckedChanged"
            Text="Search History records (super slow)" Visible="False" />
        <asp:CheckBox ID="chkExcludeClosedIssues" Checked="True" runat="server"
            Text="Exclude Closed Issues" />
    </asp:Panel>
    <!-- <uc1:DisplayIssues ID="ctlBugs" runat="server" Visible="false" ShowProjectColumn="true"
        EnableViewState="True" OnRebindCommand="IssuesRebind" /> -->
    <asp:Panel ID="pnlSearchResults" runat="server">
       <table class="grid">
       <tr>
       <td class="gridHeader" style="border: 1px solid Black;">
       <h2 align="center">Search Results</h2>
       </td>
       </tr>
        </table>
        <asp:Label ID="lblSearchSummary" Text="Please Search..." ForeColor="Red" Font-Italic="true"  runat="server" /><br />

        <asp:Repeater runat="server" ID="SearchProjectRepeater" OnItemDataBound="SearchProjectRepeater_ItemDataBound">
            <HeaderTemplate>
                <ul class="milestoneList">
            </HeaderTemplate>
            <ItemTemplate>
                <li class="expand" id='Project-<%#DataBinder.Eval(Container.DataItem, "Id") %>'>
                    <div class="milestoneGroupHeader">
                        <div class="issueCount">
                            <asp:HyperLink ID="IssuesCount" runat="server" />
                        </div>
                        <asp:HyperLink ID="ProjectLink" CssClass="milestoneName" runat="server" />
                        <asp:Label CssClass="milestoneNotes" ID="ProjectDescription" runat="Server"></asp:Label>
                        <br style="clear: both;" />
                        <span class="milestoneReleaseDate">
                            <asp:Label ID="lblReleaseDate" runat="Server"></asp:Label>
                        </span><span style="font-weight: normal;">
                            <asp:HyperLink ID="FoundLink" runat="server" Text="Search Results" />
                        </span>
                    </div>
                    <div class="milestoneContent" style="display: none;">
                        <asp:Repeater ID="IssuesList" runat="server" OnItemDataBound="IssuesListRepeater_ItemDataBound">
                            <HeaderTemplate>
                                <table class="grid">
                                    <tr>
                                        <td runat="server" id="tdId" class="gridHeader">
                                            <asp:LinkButton ID="LinkButton2" runat="server" Text="<%$ Resources:CommonTerms, Id %>" />
                                        </td>
                                        <td runat="server" id="tdCategory" class="gridHeader">
                                            <asp:LinkButton ID="lnkCategory" runat="server" Text="<%$ Resources:CommonTerms, Category %>" />
                                        </td>
                                        <td runat="server" id="tdIssueType" class="gridHeader" style="text-align: center;">
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="<%$ Resources:CommonTerms, Type %>" />
                                        </td>
                                        <td runat="server" id="tdPriority" class="gridHeader" style="text-align: center;">
                                            <asp:LinkButton ID="LinkButton8" runat="server" Text="<%$ Resources:CommonTerms, Priority %>" />
                                        </td>
                                        <td runat="server" id="tdTitle" class="gridHeader">
                                            <asp:LinkButton ID="LinkButton3" runat="server" Text="<%$ Resources:CommonTerms, Title %>" />
                                        </td>
                                        <td runat="server" id="tdAssigned" class="gridHeader">
                                            <asp:LinkButton ID="LinkButton4" runat="server" Text="<%$ Resources:CommonTerms, AssgnedTo %>" />
                                        </td>
                                        <td runat="server" id="tdStatus" class="gridHeader" style="text-align: center;">
                                            <asp:LinkButton ID="LinkButton6" runat="server" Text="<%$ Resources:CommonTerms, Status %>" />
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href='../Issues/IssueDetail.aspx?id=<%#DataBinder.Eval(Container.DataItem, "Id") %>'>
                                            <%#DataBinder.Eval(Container.DataItem, "FullId") %></a>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblComponent" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName" )%>'
                                            runat="Server"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <it:TextImage ID="ctlType" ImageDirectory="/IssueType" Text='<%# DataBinder.Eval(Container.DataItem, "IssueTypeName" )%>'
                                            ImageUrl='<%# DataBinder.Eval(Container.DataItem, "IssueTypeImageUrl" )%>' runat="server" />
                                    </td>
                                    <td style="text-align: center;">
                                        <it:TextImage ID="ctlPriority" ImageDirectory="/Priority" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityName" )%>'
                                            ImageUrl='<%# DataBinder.Eval(Container.DataItem, "PriorityImageUrl" )%>' runat="server" />
                                    </td>
                                    <td>
                                        <a href='../Issues/IssueDetail.aspx?id=<%#DataBinder.Eval(Container.DataItem, "Id") %>'>
                                            <asp:Label ID="lblSummary" Text='<%# DataBinder.Eval(Container.DataItem, "Title" )%>'
                                                runat="Server"></asp:Label>
                                        </a>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAssignedTo" Text='<%# DataBinder.Eval(Container.DataItem, "AssignedDisplayName" )%>'
                                            runat="Server"></asp:Label>
                                    </td>
                                    <td style="text-align: center;">
                                        <it:TextImage ID="ctlStatus" ImageDirectory="/Status" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName" )%>'
                                            ImageUrl='<%# DataBinder.Eval(Container.DataItem, "StatusImageUrl" )%>' runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <asp:Panel ID="pnlIssueComments" runat="server">
                                            <table class="subgrid">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCommentCount" Text="0 comments found." runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Repeater ID="IssuesCommentList" OnItemDataBound="IssuesCommentListRepeater_ItemDataBound"
                                                runat="server">
                                                <HeaderTemplate>
                                                    <table id="tabIssueComments" class="subgrid">
                                                        <tr>
                                                            <td runat="server" id="tdCreator" class="subgridHeader">
                                                                <asp:LinkButton ID="lnkCreator" runat="server" Text="<%$ Resources:CommonTerms, Creator %>" />
                                                            </td>
                                                            <td runat="server" id="tdDateCreate" class="subgridHeader" style="text-align: center;">
                                                                <asp:LinkButton ID="lnkDateCreate" runat="server" Text="Date" />
                                                            </td>
                                                            <td runat="server" id="tdComment" class="subgridHeader" style="text-align: center;">
                                                                <asp:LinkButton ID="lnkComment" runat="server" Text="Comment" />
                                                            </td>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCreator" Text='<%# DataBinder.Eval(Container.DataItem, "CreatorDisplayName" )%>'
                                                                runat="Server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDateCreated" Text='<%# DataBinder.Eval(Container.DataItem, "DateCreated" )%>'
                                                                runat="Server"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left;">
                                                            <asp:Label ID="lblComment" Text='' runat="Server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <br />
</asp:Content>
