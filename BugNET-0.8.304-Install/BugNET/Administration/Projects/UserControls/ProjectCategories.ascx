<%@ Control Language="c#" Inherits="BugNET.Administration.Projects.UserControls.ProjectCategories"
    CodeBehind="ProjectCategories.ascx.cs" AutoEventWireup="True" %>
<%@ Register TagPrefix="it" TagName="PickCategory" Src="~/UserControls/PickCategory.ascx" %>
<%@ Register Src="~/UserControls/CategoryTreeView.ascx" TagName="CategoryTreeView"
    TagPrefix="it" %>
<script type="text/javascript" src="<%=ResolveUrl("~/js/lib/adapter/ext/ext-base.js")%>"></script>
<script type="text/javascript" src="<%=ResolveUrl("~/js/lib/ext-all.js")%>"></script>
<link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/js/lib/resources/css/ext-all.css")%>" />
<style type="text/css">
    #tree-div .root-node .x-tree-node-icon
    {
        background-image: url(../../images/plugin_disabled.gif);
    }
    #tree-div .category-node .x-tree-node-icon
    {
        background-image: url(../../images/plugin.gif);
    }
</style>
<script type="text/javascript">
    var tree;
     /*seeds for the new node suffix*/
    var cseed = 0;
    Ext.BLANK_IMAGE_URL = '<%=ResolveUrl("~/js/lib/resources/images/default/s.gif")%>';
    Ext.util.CSS.swapStyleSheet('theme', "<%=ResolveUrl("~/js/lib/resources/css/xtheme-gray.css")%>");

    Ext.onReady(function(){
        // shorthand
        var Tree = Ext.tree;

        tree = new Tree.TreePanel({
            animate:true, 
            loader: new Tree.TreeLoader({dataUrl:'<%=ResolveUrl("~/UserControls/TreeHandler.ashx?id=")%><%=ProjectId %>'}),
            enableDD:true,
            containerScroll: true,
            dropConfig: {appendOnly:true},
            renderTo: 'tree-div',
 
            root: {
                nodeType: 'async',
                text: 'Root Category',
                draggable:false,
                id:'0',
                cls: 'root-node',
                draggable:false,
                disabled:true
            }
        });

        // add a tree sorter in folder mode                
        new Tree.TreeSorter(tree, {folderSort:true});            
        
        tree.loader.requestMethod = "GET";
        
        var ge = new Ext.tree.TreeEditor(tree, {
            allowBlank:false,
            blankText:'A name is required',
            selectOnFocus:true
        });
        tree.Editor=ge;
        
        tree.addListener("textchange", textChanged);
        tree.addListener("movenode", move);
        tree.addListener("beforeappend",beforeappend);
        tree.addListener("removenode",remove);
        
        // render the tree
        tree.render();
        tree.getRootNode().expand();
        
        //Events

        //node removed
        function remove(tree, parentNode, node)
        {  
            if(node && node.attributes.allowDelete)
            {
                BugNET.Webservices.BugNetServices.DeleteComponent(node.id);    
            }
        }
        
        //before node append
        function beforeappend(tree,parentNode,node,index)
        {
            //check if node is new
            if(node.attributes.isNew)
            {
                //add this new node to the database and set the new id
               BugNET.Webservices.BugNetServices.AddCategory(<%=ProjectId %>,node.text,parentNode.id,function(result){
                 node.id = result;
                 node.attributes.isNew = false;
               });   
            }
        }
        
        //node text changed
        function textChanged(node, text, oldText) 
        { 
            BugNET.Webservices.BugNetServices.ChangeTreeNode(node.id,text,oldText);
        }
        
        //node moved
        function move(tree,node,oldParent,newParent,index)
        {
            //alert("Id:" + node.id + " Old Parent: " + oldParent.id + " New Parent: " + newParent.id);
            BugNET.Webservices.BugNetServices.MoveNode(node.id,oldParent.id,newParent.id,index,SucceededCallback,OnError);
        }
       
        // This is the callback function that
        // processes the Web Service return value.
        function SucceededCallback(result)
        {
           return;
        }
        
        function OnError(result)
        {
          alert("Error: " + result.get_message());
        }   
    });
   
    //adds a new category to the tree
    function AddCategory()
    {    
        // BGN-1292 by smoss
        // Will select the root node as the parent if nothing is selected.
        if(tree.getSelectionModel().getSelectedNode())
        {
            var text ;
            // improved category naming
            if (tree.getRootNode() == tree.getSelectionModel().getSelectedNode()) 
            { text = 'Base Category '+(++cseed); 
            } else 
            { text = 'Category '+(++cseed); }                        
            var newNode = new Ext.tree.TreeNode({id: "new"+ cseed, text: text,cls:"category-node",isNew: true, leaf: false});
            tree.getSelectionModel().getSelectedNode().appendChild(newNode);
            newNode.ensureVisible(); 
            tree.Editor.triggerEdit(newNode);
        } else 
        {
            // get the root node
            var root;
            root = tree.getRootNode();        
            // improved category naming
            var text = 'Base Category '+(++cseed);
            var newNode = new Ext.tree.TreeNode({id: "new"+ cseed, text: text,cls:"category-node",isNew: true, leaf: false});            
            root.appendChild(newNode);
            newNode.ensureVisible(); 
            tree.Editor.triggerEdit(newNode);            
        }
    }
    
    function DeleteCategory()
    {
        var selectedNode = tree.getSelectionModel().getSelectedNode();
        if(selectedNode && selectedNode.id == 0)
	    {
	        Ext.Msg.alert('Status', 'You cannot delete the root category.');
	    }
	    else
        {
            if(selectedNode.lastChild)
		    {
                Ext.Msg.alert('Status', 'You must delete all child categories of this parent first.');
            }
            else
            {
              $find('DeleteCategoryMP').show();
            }
        }  
        
    }
    
    function onOk() 
    {
        var selectedNode = tree.getSelectionModel().getSelectedNode();       
        document.getElementById('<%=HiddenField1.ClientID %>').value = selectedNode.id;
    }
</script>
<div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
        CssClass="errors" />
    <h2>
        <asp:Literal ID="CategoriesTitle" runat="Server" meta:resourcekey="CategoriesTitle" /></h2>
    <br />
    <asp:CustomValidator Display="dynamic" meta:resourcekey="CategoryValidator" runat="server"
        OnServerValidate="CategoryValidation_Validate" ID="ComponentValidation" />
    <p>
        <asp:Label ID="DescriptionLabel" runat="server" meta:resourcekey="DescriptionLabel" />
    </p>
    <bn:Message ID="Message1" runat="server" />
    <br />
    <img alt="Add Category" type="image" src="../../images/plugin_add.gif" onclick="AddCategory();"
        class="icon" />
    <a href="#" onclick="AddCategory();">Add Category</a> &nbsp;
    <asp:LinkButton ID="LinkButton1" runat="server" Text="Not Used" Style="display: none;"></asp:LinkButton>
    <img alt="Delete Category" type="image" src="../../images/plugin_delete.gif" onclick="DeleteCategory();"
        class="icon" />
    <a href="#" onclick="DeleteCategory();">Delete Category</a>
    <br />
    <br />
    <div id="tree-div">
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
</div>
<asp:Panel ID="pnlDeleteCategory" runat="server" CssClass="ModalPopup" Style="display: none;">
    <asp:Panel ID="pnlHeader" runat="server" CssClass="ModalHeader">
        Delete Category</asp:Panel>
    <div class="ModalContainer">
        Please select between the following options:
        <br />
        <br />
        <table cellspacing="10" style="margin-left: 10px; text-align: left;">
            <tr>
                <td>
                    <asp:RadioButton ID="RadioButton1" GroupName="DeleteCategory" runat="server" Checked="true"
                        Height="30px" Text="&nbsp;&nbsp;Delete this category and all assigned issues." />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButton ID="RadioButton2" GroupName="DeleteCategory" runat="server" Height="30px"
                        Text="&nbsp;&nbsp;Assign all issues to an existing category." />
                    <div style="margin: 0 0 0 35px;">
                        <it:PickCategory ID="DropCategory" DisplayDefault="true" Required="false" runat="Server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButton ID="RadioButton3" GroupName="DeleteCategory" runat="server" Height="30px"
                        Text="&nbsp;&nbsp;Assign all issues to a new category. " />
                    <div style="margin: 0 0 0 35px;">
                        <asp:TextBox ID="NewCategoryTextBox" runat="server" Text=""></asp:TextBox>
                    </div>
                </td>
            </tr>
        </table>
        <p style="text-align: center;">
            <asp:Button ID="OkButton" runat="server" OnClick="OkButton_Click" OnClientClick="onOk();"
                Text="Ok" />
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
        </p>
    </div>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="LinkButton1"
    PopupControlID="pnlDeleteCategory" BackgroundCssClass="modalBackground" CancelControlID="CancelButton"
    DropShadow="false" BehaviorID="DeleteCategoryMP" PopupDragHandleControlID="pnlHeader" />
<ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="LinkButton1"
    DisplayModalPopupID="ModalPopupExtender2" />
<ajaxToolkit:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="NewCategoryTextBox"
    WatermarkText="Enter a new category" WatermarkCssClass="watermarked" />
