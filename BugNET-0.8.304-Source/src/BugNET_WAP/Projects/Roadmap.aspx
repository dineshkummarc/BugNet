<%@ Page Language="C#" MasterPageFile="~/Shared/FullWidth.master" Title="Road Map"
    AutoEventWireup="true" meta:resourcekey="Page" Inherits="BugNET.Projects.Roadmap"
    CodeBehind="RoadMap.aspx.cs" %>

<%@ Register TagPrefix="it" TagName="TextImage" Src="~/UserControls/TextImage.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="Server">
    <script src="..//js/jquery.cookie.js" type="text/javascript"></script>
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
    <div class="centered-content">
        <h1 class="page-title">
            <asp:Literal ID="Literal1" runat="server" meta:resourcekey="TitleLabel" />
            -
            <asp:Literal ID="ltProject" runat="server" />
            <span>(<asp:Literal ID="litProjectCode" runat="Server"></asp:Literal>)</span>
        </h1>
    </div>
    <asp:Repeater runat="server" ID="RoadmapRepeater" OnItemDataBound="RoadmapRepeater_ItemDataBound">
        <HeaderTemplate>
            <ul class="milestoneList">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="expand" id='milestoneRoadmap-<%#DataBinder.Eval(Container.DataItem, "Id") %>'>
                <div class="milestoneGroupHeader">
                    <div class="milestoneProgress">
                        &nbsp;<asp:Label ID="PercentLabel" runat="Server" Style="float: right; padding-left: 10px;
                            width: 30px; text-align: right;"></asp:Label>
                        <div class="roadMapProgress" style="float: right; vertical-align: top; background-color: #FA0000;
                            font-size: 8px; width: 135px; height: 15px; margin: 0px; text-align: center;">
                            <div id="ProgressBar" runat='server' style="display: block; height: 15px; background: #6FB423 url('../images/fade.png') repeat-x 0% -3px;">
                                &nbsp;</div>
                        </div>
                        <br />
                        <asp:Label Style="float: left;" ID="lblProgress" runat="Server"></asp:Label>
                    </div>
                    <asp:HyperLink ID="MilestoneLink" CssClass="milestoneName" runat="server" />
                    <asp:Label CssClass="milestoneNotes" ID="MilestoneNotes" runat="Server"></asp:Label>
                    <br style="clear: both;" />
                    <span class="milestoneReleaseDate">
                        <asp:Label ID="lblDueDate" runat="Server"></asp:Label></span>
                </div>
                <div class="milestoneContent" style="display: none;">
                    <asp:Repeater ID="IssuesList" OnItemCreated="rptRoadMap_ItemCreated" runat="server">
                        <HeaderTemplate>
                            <table class="grid">
                                <tr>
                                    <td runat="server" id="tdId" class="gridHeader">
                                        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="SortIssueIdClick" Text="<%$ Resources:CommonTerms, Id %>" />
                                    </td>
                                    <td runat="server" id="tdCategory" class="gridHeader">
                                        <asp:LinkButton ID="lnkCategory" runat="server" Text="<%$ Resources:CommonTerms, Category %>"
                                            OnClick="SortCategoryClick" />
                                    </td>
                                    <td runat="server" id="tdIssueType" class="gridHeader" style="text-align: center;">
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="SortIssueTypeClick" Text="<%$ Resources:CommonTerms, Type %>" />
                                    </td>
                                    <td runat="server" id="tdPriority" class="gridHeader" style="text-align: center;">
                                        <asp:LinkButton ID="LinkButton8" runat="server" Text="<%$ Resources:CommonTerms, Priority %>"
                                            OnClick="SortPriorityClick" />
                                    </td>
                                    <td runat="server" id="tdTitle" class="gridHeader">
                                        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="SortTitleClick" Text="<%$ Resources:CommonTerms, Title %>" />
                                    </td>
                                    <td runat="server" id="tdAssigned" class="gridHeader">
                                        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="SortAssignedUserClick" Text="<%$ Resources:CommonTerms, AssgnedTo %>" />
                                    </td>
                                    <td runat="server" id="tdDueDate" class="gridHeader">
                                        <asp:LinkButton ID="LinkButton5" runat="server" OnClick="SortDueDateClick" Text="<%$ Resources:CommonTerms, DueDate %>" />
                                    </td>
                                    <td runat="server" id="tdStatus" class="gridHeader" style="text-align: center;">
                                        <asp:LinkButton ID="LinkButton6" runat="server" OnClick="SortStatusClick" Text="<%$ Resources:CommonTerms, Status %>" />
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
                                            runat="Server"></asp:Label></a>
                                </td>
                                <td>
                                    <asp:Label ID="lblAssignedTo" Text='<%# DataBinder.Eval(Container.DataItem, "AssignedDisplayName" )%>'
                                        runat="Server"></asp:Label>
                                </td>
                                <td>
                                    <%#(DateTime)Eval("DueDate") == DateTime.MinValue ? "none" : Eval("DueDate", "{0:d}") %>
                                </td>
                                <td style="text-align: center;">
                                    <it:TextImage ID="ctlStatus" ImageDirectory="/Status" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName" )%>'
                                        ImageUrl='<%# DataBinder.Eval(Container.DataItem, "StatusImageUrl" )%>' runat="server" />
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
</asp:Content>
