<%@ Control Language="c#" Inherits="BugNET.UserControls.DisplayIssues" CodeBehind="DisplayIssues.ascx.cs" %>
<%@ Register TagPrefix="it" TagName="TextImage" Src="~/UserControls/TextImage.ascx" %>
<%@ Register TagPrefix="it" TagName="PickCategory" Src="~/UserControls/PickCategory.ascx" %>
<%@ Register TagPrefix="it" TagName="PickMilestone" Src="~/UserControls/PickMilestone.ascx" %>
<%@ Register TagPrefix="it" TagName="PickType" Src="~/UserControls/PickType.ascx" %>
<%@ Register TagPrefix="it" TagName="PickStatus" Src="~/UserControls/PickStatus.ascx" %>
<%@ Register TagPrefix="it" TagName="PickPriority" Src="~/UserControls/PickPriority.ascx" %>
<%@ Register TagPrefix="it" TagName="PickSingleUser" Src="~/UserControls/PickSingleUser.ascx" %>
<%@ Register TagPrefix="it" TagName="PickResolution" Src="~/UserControls/PickResolution.ascx" %>
<%@ Register TagPrefix="it" TagName="DisplayCustomFields" Src="~/UserControls/DisplayCustomFields.ascx" %>

<script type="text/javascript" src="<%=ResolveUrl("~/js/lib/adapter/ext/ext-base.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/js/lib/ext-all.js")%>"></script>
<link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/js/lib/resources/css/ext-all.css")%>" />

<script type="text/javascript">
    var tog = false;
    function checkAll() {
        $("#<%=gvIssues.ClientID%> :checkbox").attr('checked', !tog);
        tog = !tog;
    }
    $(document).ready(function () {

        $('#<%=SaveIssues.ClientID %>').click(function () {

            var checked = $("#<%=gvIssues.ClientID%> :checkbox:checked");
            var nbChecked = checked.size();
            var processResult = 0;           

            if (nbChecked<1) {
                Ext.Msg.alert('Error', 'You have not selected any issues to edit.');
                return false;
            } else {
                Ext.Msg.alert('Saved!', nbChecked + ' issues changed.');
                return true;
            }            
        });

        $('#editProperties').click(function () {
            var div = $('#SetProperties');
            var flag = $(div).is(":hidden") ? true : false;
            // Havent displayed yet, let check some things
            if (flag) {
                //find all checked checkboxes + radio buttons on the GridView
                var checked = $("#<%=gvIssues.ClientID%> :checkbox:checked");
                var nbChecked = checked.size();
                if (nbChecked < 1) {
                    Ext.Msg.alert('Error', 'You have not selected any issues to edit.');
                } else {

                    $('#SetProperties').slideToggle('fast', function () {
                        Ext.Msg.alert('Warning', 'You are now editing the ' + nbChecked + ' issues you selected!');
                    });
                }
            } else {
                $('#SetProperties').hide();
            }

        });
        $('#showColumns').click(function () {
            $('#ChangeColumns').slideToggle('fast');
        });
    });
</script>
<script type="text/javascript">
    // Show user a warning 
    Ext.BLANK_IMAGE_URL = '<%=ResolveUrl("~/js/lib/resources/images/default/s.gif")%>';
    Ext.util.CSS.swapStyleSheet('theme', "<%=ResolveUrl("~/js/lib/resources/css/xtheme-gray.css")%>");
</script>
<asp:Panel ID="OptionsContainerPanel" runat="server" CssClass="issueListOptionsContainer">
    <div style="height: 25px; background-color: #F1F2EC; width: 100%;">
        <asp:Panel ID="LeftButtonContainerPanel" CssClass="leftButtonContainerPanel" runat="server">
            For Selected:&nbsp; <span id="EditIssueProperties"><a href="#" id="editProperties"><asp:Label ID="EditPropertiesLabel" meta:resourcekey="EditPropertiesLabel" runat="server"></asp:Label></a></span>
        </asp:Panel>
        <div id="rightButtonContainer">
            <p id="AddRemoveColumns">
                <a href="#" id="showColumns">
                    <asp:Label ID="Label12" meta:resourcekey="SelectColumnsLinkButton" runat="server"></asp:Label></a>
            </p>
            <p id="ExportExcel">
                <asp:LinkButton  CausesValidation="false" ID="ExportExcelButton" runat="server"
                    OnClick="ExportExcelButton_Click">Export</asp:LinkButton>
            </p>
            <p id="Rss">
                <asp:HyperLink ID="lnkRSS" runat="server">RSS</asp:HyperLink>
            </p>
        </div>
    </div>
    <div id="SetProperties" style="display: none;">
        <div style="margin: 0px 10px 10px 10px; padding: 10px 0 5px 0; border-bottom: 1px dotted #cccccc;">
            <asp:Label ID="Label10" runat="server" meta:resourcekey="SetPropertiesLabel"></asp:Label></div>
        <div style="margin-left: 10px; margin-right: 10px; padding-bottom: 10px;color: #55555F;">
            <table width="100%">
                <tr>
                    <td style="width: 15%;">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:CommonTerms, Category %>"></asp:Label>:
                    </td>
                    <td style="width: 35%;">
                        <it:PickCategory ID="dropCategory" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                    <td style="width: 15%;">
                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:CommonTerms, Owner %>"></asp:Label>:
                    </td>
                    <td style="width: 35%;">
                        <it:PickSingleUser ID="dropOwner" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:CommonTerms, Milestone %>"></asp:Label>:
                    </td>
                    <td>
                        <it:PickMilestone ID="dropMilestone" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label11" AssociatedControlID="dropAffectedMilestone" runat="server"
                            Text="<%$ Resources:CommonTerms, AffectedMilestone %>"></asp:Label>:
                    </td>
                    <td>
                        <it:PickMilestone ID="dropAffectedMilestone" DisplayDefault="true" Required="false"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:CommonTerms, Type %>"></asp:Label>:
                    </td>
                    <td>
                        <it:PickType ID="dropType" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:CommonTerms, Resolution %>" />:
                    </td>
                    <td>
                        <it:PickResolution ID="dropResolution" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:CommonTerms, Priority %>" />:
                    </td>
                    <td>
                        <it:PickPriority ID="dropPriority" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:CommonTerms, Status %>" />:
                    </td>
                    <td>
                        <it:PickStatus ID="dropStatus" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:CommonTerms, Assigned %>" />:
                    </td>
                    <td>
                        <it:PickSingleUser ID="dropAssigned" DisplayDefault="true" Required="false" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:CommonTerms, DueDate %>" />:
                    </td>
                    <td>
                        <asp:TextBox ID="DueDate" Width="100" CssClass="dateField embed" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <it:DisplayCustomFields ID="ctlCustomFields" runat="server" />
            <br />


            <asp:Panel ID="Panel1" runat="server">
<strong>Warning:</strong> You are about to BULK-EDIT the selected Issues.<br />
<asp:Button ID="SaveIssues" runat="server" OnClick="SaveIssues_Click" CausesValidation="false"
                Text="<%$ Resources:CommonTerms, Save %>" />
                
                  <input type="button" id="CancelEditProperties" onclick="$('#SetProperties').toggle();"
                runat="server" value="<%$ Resources:CommonTerms, Cancel%>" />         
            
        </asp:Panel>    
            
        </div>
    </div>
    <asp:Panel ID="SelectColumnsPanel" Visible="False" runat="Server">
        <div id="ChangeColumns" style="display: none; width: 100%; background-color: #FFFAF6;
            margin-bottom: 10px;">
            <div style="margin: 0px 10px 10px 10px; padding: 10px 0 5px 0; border-bottom: 1px dotted #cccccc;">
                <asp:Literal ID="Literal3" runat="server" meta:resourcekey="SelectColumnsLiteral"></asp:Literal></div>
            <div style="margin-left: 10px; margin-right: 10px; padding-bottom: 10px;">
                <asp:CheckBoxList ID="lstIssueColumns" Width="100%" RepeatColumns="7" CellPadding="0"
                    CellSpacing="0" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Project %>" Value="4" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Votes %>" Value="5" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Category %>" Value="6" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Creator %>" Value="7" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Owner %>" Value="8" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Assigned %>" Value="9" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Type %>" Value="10" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Milestone %>" Value="11" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, AffectedMilestone %>" Value="12" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Status %>" Value="13" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Priority %>" Value="14" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Resolution %>" Value="15" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, DueDate %>" Value="16" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Estimation %>" Value="17" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Progress %>" Value="18" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, TimeLogged %>" Value="19" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, Created %>" Value="20" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, LastUpdate %>" Value="21" />
                    <asp:ListItem Text="<%$ Resources:CommonTerms, LastUpdateUser %>" Value="22" />
                </asp:CheckBoxList>
                <br />
                <asp:Button ID="SaveButton" runat="server" Text="<%$ Resources:CommonTerms, Save %>"
                    CssClass="standardText" OnClick="SaveClick" ValidationGroup="AddRemoveColumns">
                </asp:Button>&nbsp;&nbsp;
                <input type="button" id="Button1" onclick="$('#ChangeColumns').toggle();" runat="server"
                    value="<%$ Resources:CommonTerms, Cancel%>" />
            </div>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:GridView ID="gvIssues" SkinID="GridView" AllowPaging="True" AllowSorting="True"
    DataKeyNames="Id" PagerStyle-HorizontalAlign="right" OnRowCommand="gvIssues_RowCommand"
    OnRowCreated="gvIssues_RowCreated" OnRowDataBound="gvIssues_RowDataBound" OnSorting="gvIssues_Sorting"
    OnPageIndexChanging="gvIssues_PageIndexChanging" PageSize="10" runat="server">
    <Columns>
        <asp:TemplateField>
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <HeaderTemplate>
                <asp:Image ID="CheckAllImage" ImageUrl="~/Images/tick.gif" onclick="checkAll();"
                    runat="server" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="cbSelectAll" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                <asp:Image runat="server" ID="imgPrivate" meta:resourcekey="imgPrivate" AlternateText="Private"
                    ImageUrl="~/images/lock.gif"></asp:Image>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="FullId" HeaderText="<%$ Resources:CommonTerms, Id %>"
            SortExpression="Id" ItemStyle-Wrap="false">
            <ItemStyle Wrap="False"></ItemStyle>
        </asp:BoundField>
        <asp:HyperLinkField HeaderStyle-HorizontalAlign="Left" HeaderText="<%$ Resources:CommonTerms, Title %>"
            SortExpression="Title" DataNavigateUrlFormatString="~/Issues/IssueDetail.aspx?id={0}"
            DataNavigateUrlFields="Id" DataTextField="Title">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:HyperLinkField>
        <asp:BoundField DataField="ProjectName" HeaderText="Project" SortExpression="Project"
            ItemStyle-Wrap="false" Visible="false">
            <ItemStyle Wrap="False"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="<%$ Resources:CommonTerms, Votes %>" SortExpression="Votes"
            Visible="false">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "Votes")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Category" HeaderText="<%$ Resources:CommonTerms, Category %>"
            Visible="false">
            <ItemStyle HorizontalAlign="Left" CssClass="gridItem"></ItemStyle>
            <ItemTemplate>
                &nbsp;<%# DataBinder.Eval(Container.DataItem, "CategoryName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Creator" HeaderText="<%$ Resources:CommonTerms, Creator %>"
            Visible="false">
            <ItemStyle HorizontalAlign="Left" CssClass="gridItem"></ItemStyle>
            <ItemTemplate>
                &nbsp;<%# DataBinder.Eval(Container.DataItem, "CreatorDisplayName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Owner" HeaderText="<%$ Resources:CommonTerms, Owner %>"
            Visible="false">
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            <ItemTemplate>
                &nbsp;<%# DataBinder.Eval(Container.DataItem, "OwnerDisplayName")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Assigned" HeaderText="<%$ Resources:CommonTerms, Assigned %>"
            Visible="false">
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
            <ItemTemplate>
                &nbsp;<%# DataBinder.Eval(Container.DataItem, "AssignedDisplayName" )%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="IssueType" HeaderText="<%$ Resources:CommonTerms, Type %>"
            Visible="false">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                &nbsp;<it:TextImage ID="ctlType" ImageDirectory="/IssueType" Text='<%# DataBinder.Eval(Container.DataItem, "IssueTypeName" )%>'
                    ImageUrl='<%# DataBinder.Eval(Container.DataItem, "IssueTypeImageUrl" )%>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Milestone" HeaderText="<%$ Resources:CommonTerms, Milestone %>"
            Visible="false">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                &nbsp;<it:TextImage ID="ctlMilestone" ImageDirectory="/Milestone" Text='<%# DataBinder.Eval(Container.DataItem, "MilestoneName" )%>'
                    ImageUrl='<%# DataBinder.Eval(Container.DataItem, "MilestoneImageUrl" )%>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="AffectedMilestone" HeaderText="<%$ Resources:CommonTerms, AffectedMilestone %>"
            Visible="false">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                &nbsp;<it:TextImage ID="ctlAffectedMilestone" ImageDirectory="/Milestone" Text='<%# DataBinder.Eval(Container.DataItem, "AffectedMilestoneName" )%>'
                    ImageUrl='<%# DataBinder.Eval(Container.DataItem, "AffectedMilestoneImageUrl" )%>'
                    runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Status" HeaderText="<%$ Resources:CommonTerms, Status %>"
            Visible="false">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                &nbsp;<it:TextImage ID="ctlStatus" ImageDirectory="/Status" Text='<%# DataBinder.Eval(Container.DataItem, "StatusName" )%>'
                    ImageUrl='<%# DataBinder.Eval(Container.DataItem, "StatusImageUrl" )%>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Priority" HeaderText="<%$ Resources:CommonTerms, Priority %>"
            Visible="false">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                &nbsp;<it:TextImage ID="ctlPriority" ImageDirectory="/Priority" Text='<%# DataBinder.Eval(Container.DataItem, "PriorityName" )%>'
                    ImageUrl='<%# DataBinder.Eval(Container.DataItem, "PriorityImageUrl" )%>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Resolution" HeaderText="<%$ Resources:CommonTerms, Resolution %>"
            Visible="false">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                &nbsp;<it:TextImage ID="ctlResolution" ImageDirectory="/Resolution" Text='<%# DataBinder.Eval(Container.DataItem, "ResolutionName" )%>'
                    ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ResolutionImageUrl" )%>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:CommonTerms, DueDate%>" SortExpression="DueDate"
            Visible="false">
            <ItemTemplate>
                <%#(DateTime)Eval("DueDate") == DateTime.MinValue ? "none" : Eval("DueDate", "{0:d}") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:CommonTerms, EstimationHours %>" SortExpression="Estimation"
            Visible="false">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "Estimation")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Progress" HeaderText="<%$ Resources:CommonTerms, Progress %>"
            Visible="false">
            <ItemStyle HorizontalAlign="center" />
            <HeaderStyle HorizontalAlign="center" />
            <ItemTemplate>
                <div id="Progress" runat="server" style="vertical-align: top; font-size: 8px; border: 1px solid #ccc;
                    width: 100px; height: 7px; margin: 5px; text-align: center;">
                    <div id="ProgressBar" runat='server' style="text-align: left; background-color: #C4EFA1;
                        height: 7px;">
                        &nbsp;</div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:CommonTerms, TimeLoggedHours %>" SortExpression="TimeLogged"
            Visible="false">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "TimeLogged")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Created" HeaderText="<%$ Resources:CommonTerms, Created %>">
            <ItemTemplate>
                &nbsp;<%# DataBinder.Eval(Container.DataItem, "DateCreated", "{0:d}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="LastUpdate" HeaderText="<%$ Resources:CommonTerms, LastUpdate %>">
            <ItemTemplate>
                &nbsp;<%# DataBinder.Eval(Container.DataItem, "LastUpdate", "{0:d}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="LastUpdateUser" HeaderText="<%$ Resources:CommonTerms, LastUpdateUser %>">
            <ItemTemplate>
                &nbsp;<%# DataBinder.Eval(Container.DataItem, "LastUpdateDisplayName")%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerStyle HorizontalAlign="Right" VerticalAlign="Bottom" />
    <PagerTemplate>
        <div class="pager">
            <span class="results1">
                <asp:ImageButton ID="btnFirst" CausesValidation="false" runat="server" ImageUrl='<%#gvIssues.PagerSettings.FirstPageImageUrl%>'
                    CommandArgument="First" ImageAlign="AbsBottom" CommandName="Page" />
                <asp:ImageButton ID="btnPrevious" CausesValidation="false"  runat="server" ImageUrl='<%#gvIssues.PagerSettings.PreviousPageImageUrl%>'
                    CommandArgument="Prev" ImageAlign="AbsBottom" CommandName="Page" />
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CommonTerms, Page %>" />
                <asp:DropDownList ID="ddlPages" runat="server" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:CommonTerms, Of %>" />
                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                <asp:ImageButton ID="btnNext" CausesValidation="false"  runat="server" ImageUrl='<%#gvIssues.PagerSettings.NextPageImageUrl%>'
                    CommandArgument="Next" CommandName="Page" ImageAlign="AbsBottom" />
                <asp:ImageButton ID="btnLast" CausesValidation="false"  runat="server" ImageUrl='<%#gvIssues.PagerSettings.LastPageImageUrl%>'
                    CommandArgument="Last" CommandName="Page" ImageAlign="AbsBottom" />
            </span><span class="results2">
                <asp:Label ID="LabelRows" runat="server" Text="Results per page:" AssociatedControlID="DropDownListPageSize" />
                <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="droplist"
                    OnSelectedIndexChanged="DropDownListPageSize_SelectedIndexChanged">
                    <asp:ListItem Value="5" />
                    <asp:ListItem Value="10" />
                    <asp:ListItem Value="15" />
                    <asp:ListItem Value="25" />
                    <asp:ListItem Value="50" />
                    <asp:ListItem Value="75" />
                    <asp:ListItem Value="100" />
                </asp:DropDownList>
            </span>
        </div>
    </PagerTemplate>
</asp:GridView>
<div style="width: 100%; padding-top: 10px">
    <asp:Label ID="lblResults" ForeColor="Red" Font-Italic="True" runat="server" Text="<%$ Resources:CommonTerms, NoIssueResults %>" />
</div>
