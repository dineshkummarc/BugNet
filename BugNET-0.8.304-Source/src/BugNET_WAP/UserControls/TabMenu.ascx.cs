namespace BugNET.UserControls
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Collections;
    using System.Collections.Generic;
    using BugNET.BusinessLogicLayer;
    using BugNET.UserInterfaceLayer;
    using BugNET.UserControls;


    /// <summary>
    ///		Summary description for TabMenu.
    /// </summary>
    public partial class TabMenu : System.Web.UI.UserControl
    {
        ArrayList Tabs;
        private string _CurrentTab = string.Empty;
        /// <summary>
        /// Menu Type Enumeration
        /// </summary>
        public enum MenuType
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Default
            /// </summary>
            Default = 1,
            /// <summary>
            /// Project
            /// </summary>
            Project = 2
        }
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //we can infer if the project id parameter is set that 
            //we should  display project menu items.
            if (ProjectId != -1)
                CurrentMenuType = MenuType.Project;

            Tabs = new ArrayList();
            Tabs.Add(new Tab(GetLocalResourceObject("Home").ToString(), "~/Default.aspx"));
            if (CurrentMenuType == MenuType.Project)
            {
                Tabs.Add(new Tab(GetLocalResourceObject("Issues").ToString(), string.Format("~/Issues/IssueList.aspx?pid={0}", ProjectId)));
                Tabs.Add(new Tab(GetLocalResourceObject("Queries").ToString(), string.Format("~/Queries/QueryList.aspx?pid={0}", ProjectId)));
                Tabs.Add(new Tab(GetLocalResourceObject("ProjectSummary").ToString(), string.Format("~/Projects/ProjectSummary.aspx?pid={0}", ProjectId)));
                Tabs.Add(new Tab(GetLocalResourceObject("Roadmap").ToString(), string.Format("~/Projects/Roadmap.aspx?pid={0}", ProjectId)));
                Tabs.Add(new Tab(GetLocalResourceObject("ChangeLog").ToString(), string.Format("~/Projects/ChangeLog.aspx?pid={0}", ProjectId)));
                if (!string.IsNullOrEmpty(Project.GetProjectById(ProjectId).SvnRepositoryUrl))
                    Tabs.Add(new Tab(GetLocalResourceObject("Source").ToString(), string.Format("~/SvnBrowse/SubversionBrowser.aspx?pid={0}", ProjectId)));

                if (Context.User.Identity.IsAuthenticated)
                {
                    //check add issue permission
                    if (ITUser.HasPermission(ProjectId, Globals.Permission.ADD_ISSUE.ToString()))
                        Tabs.Insert(2, new Tab(GetLocalResourceObject("NewIssue").ToString(), string.Format("~/Issues/IssueDetail.aspx?pid={0}", ProjectId)));
                    if (ITUser.HasPermission(ProjectId, Globals.Permission.VIEW_PROJECT_CALENDAR.ToString()))
                        Tabs.Insert(5,new Tab(GetLocalResourceObject("Calendar").ToString(), string.Format("~/Projects/ProjectCalendar.aspx?pid={0}", ProjectId)));
                    if (ITUser.IsInRole(Globals.ProjectAdminRole))
                        Tabs.Add(new Tab("Edit Project", string.Format("~/Administration/Projects/EditProject.aspx?id={0}", ProjectId)));
                }
            }

            if (Context.User.Identity.IsAuthenticated)
            {
                Tabs.Add(new Tab("My Issues", "~/Issues/MyIssues.aspx"));

                if (ITUser.IsInRole(Globals.SuperUserRole))
                    Tabs.Add(new Tab(GetLocalResourceObject("Admin").ToString(), "~/Administration/Admin.aspx"));
               
            }        

            Menu.DataSource = Tabs;
            Menu.DataBind();
        }


        /// <summary>
        /// Gets or sets the current tab.
        /// </summary>
        /// <value>The current tab.</value>
        public string CurrentTab
        {
            get { return _CurrentTab; }
            set { _CurrentTab = value; }
        }

        /// <summary>
        /// Menu Type Enumeration Property
        /// </summary>
        public MenuType CurrentMenuType
        {
            get
            {
                if (ViewState["currentMenuType"] == null)
                    return MenuType.Default;

                return (MenuType)ViewState["currentMenuType"];
            }
            set { ViewState["currentMenuType"] = value; }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the Menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
        private void Menu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Tab objTab = (Tab)e.Item.DataItem;
                //Try comparing a contains of current url with link url to show selected tab            
                if (SiteMap.CurrentNode != null)
                {
                  //This needs to be a recursive function so it can go N levels deep.
                    if (((Tab)e.Item.DataItem).Text.Contains(SiteMap.CurrentNode.Title) || (SiteMap.CurrentNode.ParentNode != null && ((Tab)e.Item.DataItem).Text.Contains(SiteMap.CurrentNode.ParentNode.Title) && SiteMap.CurrentNode.ParentNode.Title != "Home"))
                    {
                        ((Tab)e.Item.DataItem).CssClass = "selected";
                    }
                }
 
                e.Item.Controls.Add(new System.Web.UI.LiteralControl("<li>"));
                e.Item.Controls.Add((Tab)e.Item.DataItem);
                e.Item.Controls.Add(new System.Web.UI.LiteralControl("</li>"));
            } 


        }

        /// <summary>
        /// Retrieves the project Id from the base page class
        /// </summary>
        public int ProjectId
        {
            get
            {
                try
                {
                    // do the as test to to see if the basepage is the same as page
                    // if not the page parameter will be null and no exception will bethrown
                    BugNET.UserInterfaceLayer.BasePage page = this.Page as BugNET.UserInterfaceLayer.BasePage;

                    if (page != null)
                    {
                        return page.ProjectId;
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch
                {
                    return -1;
                }
            }
        }

        #region Web Form Designer generated code
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Menu.ItemDataBound += new RepeaterItemEventHandler(Menu_ItemDataBound);

        }
        #endregion

    }
}