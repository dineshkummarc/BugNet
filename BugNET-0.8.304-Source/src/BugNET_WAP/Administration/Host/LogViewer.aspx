<%@ Page Language="C#" MasterPageFile="~/Shared/FullWidth.master" AutoEventWireup="true" Inherits="BugNET.Administration.Host.LogViewer" Title="<%$ Resources:LogViewer %>" Codebehind="LogViewer.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">
    <h1 class="page-title"><asp:literal ID="LogViewerTitle" runat="Server" Text="<%$ Resources:LogViewer %>"  /></h1>
    <script type="text/javascript">
        function ExpandDetails(eid)
        {
            var el = document.getElementById(eid);
            var row = el.parentNode.parentNode.previousSibling;
            if(el.style.display=='none')
            {
               el.style.display='block';
               row.style.backgroundColor = '#F7F7EC';
               row.style.fontWeight = 'bold';
            }
            else
            {
                el.style.display='none';
                el.parentElement.parentElement.previousSibling.style.backgroundColor = '';
                row.style.fontWeight = 'normal';
            }
        }  
    </script>

    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:FilterBy %>"></asp:Label> <asp:DropDownList
        ID="FilterDropDown" runat="server" OnSelectedIndexChanged="FilterDropDown_SelectedIndexChanged" AutoPostBack="true">
        <asp:ListItem Text="<%$ Resources:SelectLevel %>" Value="" />
        <asp:ListItem Text="<%$ Resources:Error %>" Value="ERROR" />
        <asp:ListItem Text="<%$ Resources:Warning %>" Value="WARN" />
        <asp:ListItem Text="<%$ Resources:Info %>" Value="INFO" />
		<asp:ListItem Text="<%$ Resources:Debug %>" Value="DEBUG" />
    </asp:DropDownList>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" RenderMode="inline" runat="Server">
   	    <ContentTemplate> 			
            <asp:GridView ID="gvLog" runat="server" SkinID="GridView"
            AllowPaging="True" 
            AllowSorting="True"
            OnRowCreated="gvLog_RowCreated" 
            OnPageIndexChanging="gvLog_PageIndexChanging"
            OnSorting="gvLog_Sorting"
            PagerSettings-Mode="NumericFirstLast"
            PagerStyle-HorizontalAlign="right"
            BorderWidth="0px" 
            PageSize="15" 
            GridLines="None"  
            AutoGenerateColumns="False"
            OnRowDataBound="gvLog_RowDataBound" >
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="<%$ Resources:CommonTerms, Id %>" SortExpression="Id" ItemStyle-VerticalAlign="Top"  ItemStyle-Width="50px"/>
                     <asp:BoundField DataField="Date" HeaderText="<%$ Resources:CommonTerms, Date %>" SortExpression="Date" DataFormatString="{0:d}  {0:HH\:mm\:ss}" ItemStyle-VerticalAlign="Top"  ItemStyle-Width="120px"/>
                    <asp:TemplateField HeaderText="<%$ Resources:Level %>" SortExpression="Level">
                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" Width="100px" />
                        <ItemTemplate>
                            <asp:image id="imgLevel" runat="server" CssClass="icon" ImageUrl=""></asp:image>&nbsp;<asp:Label ID="LevelLabel" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Logger %>" SortExpression="Logger" ItemStyle-VerticalAlign="Top" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="LoggerLabel" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>             
                    <asp:TemplateField HeaderText="<%$ Resources:Message %>" SortExpression="Message" ItemStyle-Wrap="true" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="MessageLabel" runat="server"></asp:Label>                
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:BoundField DataField="User" HeaderText="<%$ Resources:User %>" SortExpression="User" ItemStyle-Width="50px" />
                     <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-Width="0px">
                         <ItemTemplate>
                  	            </td>
				            </tr>
				            <tr>
					            <td colspan="7">
					                <div style="display:none;background-color:#efefef;padding:10px;overflow-x:scroll;overflow-y: hidden; " id='Exception_<%#Eval("Id")%>'>
					                    <table style="table-layout:fixed;width:100%;">
					                        <tr>
					                            <td><pre><asp:Label ID="ExceptionLabel"  runat="server" Width="100%"></asp:Label></pre></td>
					                        </tr>
					                    </table>     		                                                       
                                    </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle HorizontalAlign="Right" VerticalAlign="Bottom" /> 
                <PagerTemplate>
                    <asp:ImageButton ID="btnFirst" runat="server"
                        ImageUrl='<%#gvLog.PagerSettings.FirstPageImageUrl%>'  CommandArgument="First" ImageAlign="AbsBottom" CommandName="Page"/>
                    <asp:ImageButton ID="btnPrevious" runat="server"
                        ImageUrl='<%#gvLog.PagerSettings.PreviousPageImageUrl%>' CommandArgument="Prev"  ImageAlign="AbsBottom" CommandName="Page" />
                    <asp:literal ID="Literal1" runat="Server" Text="<%$ Resources:CommonTerms, Page %>"  />
                    <asp:DropDownList ID="ddlPages" runat="server" Width="50px" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList> <asp:literal ID="NoEntries" runat="Server" Text="<%$ Resources:CommonTerms,Of %>"  /> <asp:Label ID="lblPageCount"
                        runat="server"></asp:Label>
                    <asp:ImageButton ID="btnNext" runat="server"
                        ImageUrl='<%#gvLog.PagerSettings.NextPageImageUrl%>' CommandArgument="Next" CommandName="Page"  ImageAlign="AbsBottom" />
                    <asp:ImageButton ID="btnLast" runat="server"
                        ImageUrl='<%#gvLog.PagerSettings.LastPageImageUrl%>' CommandArgument="Last" CommandName="Page"  ImageAlign="AbsBottom" />
                </PagerTemplate>
                <EmptyDataTemplate>
                    <p style="font-style:italic;"> <asp:literal ID="NoEntries" runat="Server" Text="<%$ Resources:NoEntries %>"  /></p>
                </EmptyDataTemplate>
                
            </asp:GridView>
     </ContentTemplate>
    </asp:UpdatePanel>
    <div style="padding:10px 0 10px 0;">
        <asp:imagebutton runat="server" id="save" CssClass="icon" OnClick="cmdClearLog_Click"  ImageUrl="~/Images/page_white_delete.gif" />
        <asp:LinkButton ID="cmdClearLog" OnClick="cmdClearLog_Click" runat="server" Text="Clear Log" />
    </div>
</asp:Content>


