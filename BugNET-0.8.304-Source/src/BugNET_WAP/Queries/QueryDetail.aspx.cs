using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BugNET.BusinessLogicLayer;
using BugNET.UserControls;
using System.Collections.Generic;

namespace BugNET.Queries
{
	/// <summary>
    /// This page displays the interface for building a query against the
    /// issues database.
	/// </summary>
    public partial class QueryDetail : BugNET.UserInterfaceLayer.BasePage 
	{
		protected DisplayIssues ctlDisplayIssues;
        int QueryId = 0;

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			//this.dropProjects.SelectedIndexChanged += new System.EventHandler(this.ProjectSelectedIndexChanged);
			this.ctlDisplayIssues.RebindCommand += new System.EventHandler(this.IssuesRebind);
		}
		#endregion

	       /// <summary>
        ///  The number of query clauses is stored in view state so that the
        /// interface can be recreated on each page request.
        /// </summary>
        /// <value>The clause count.</value>
		int ClauseCount 
		{
			get 
			{
				if (ViewState["ClauseCount"] == null)
					return 0;
				else
					return (int)ViewState["ClauseCount"];
			}
			set { ViewState["ClauseCount"] = value; }
		}

        /// <summary>
        /// Handles the Unload event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Unload(object sender, System.EventArgs e)
        {
            //remove the event handler
            SiteMap.SiteMapResolve -=
             new SiteMapResolveEventHandler(this.ExpandIssuePaths);
        }


        /// <summary>
        /// Builds the user interface for selecting query fields.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, System.EventArgs e) 
		{
            Message1.Visible = false;
          
            if (Request.QueryString["id"] != null)
            {
                try
                {
                    QueryId = Int32.Parse(Request.QueryString["id"]);

                }
                catch { }
            }

            // Set Project ID from Query String
            if (Request.QueryString["pid"] != null)
            {
                try
                {
                    ProjectId = Int32.Parse(Request.QueryString["pid"]);

                }
                catch { }
            }

            if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.ADD_QUERY.ToString()))
            {
                SaveQuery.Visible = false;
                btnSaveQuery.Visible = false;
            }


            DisplayClauses();

            

			if (!Page.IsPostBack) 
			{
                
                
                lblProjectName.Text = Project.GetProjectById(ProjectId).Name;

                //dropProjects.DataSource = Project.GetProjectsByMemberUserName(User.Identity.Name);
                //dropProjects.DataBind();

                //if (dropProjects.SelectedValue == 0)
                //    Response.Redirect( "~/NoProjects.aspx" );

                Results.Visible = false;

                if (QueryId != 0)
                {
                    //edit query.
                    int currentClauseCount = ClauseCount;
                    plhClauses.Controls.Clear();
                    Query query = Query.GetQueryById(QueryId);
                    txtQueryName.Text = query.Name;
                    chkGlobalQuery.Checked = query.IsPublic;
                    //ClauseCount = 0;

                    foreach (QueryClause qc in query.Clauses)
                    {
                        ClauseCount++;
                        AddClause(true, qc);
                    }

                }
                else
                {
                    ClauseCount = 3;
                    DisplayClauses();
                }

               
                    BindQueryFieldTypes();
			}

            // The ExpandIssuePaths method is called to handle
            // the SiteMapResolve event.
            SiteMap.SiteMapResolve += new SiteMapResolveEventHandler(this.ExpandIssuePaths);
		}

        /// <summary>
        /// Expands the issue paths.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.SiteMapResolveEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private SiteMapNode ExpandIssuePaths(Object sender, SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            // The current node, and its parents, can be modified to include
            // dynamic query string information relevant to the currently
            // executing request.
            if (ProjectId != 0)
            {
                tempNode.Url = tempNode.Url + "?id=" + ProjectId.ToString();
            }
            

            if ((null != (tempNode = tempNode.ParentNode)))
            {
                tempNode.Url = string.Format("~/Queries/QueryList.aspx?pid={0}", ProjectId);
            }

            return currentNode;

        }

        /// <summary>
        ///When a user pages or sorts the issues displayed by the DisplayIssues
        /// user control, this method is called. This method simply calls the ExecuteQuery()
        /// method to rebind the DisplayIssues control to its datasource.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void IssuesRebind(Object s, EventArgs e) 
		{
			ExecuteQuery();
		}

        /// <summary>
        /// When a user picks a new project from the dropdown list, this method
        /// is called. This method clears the current query clauses so the correct
        /// values can be generated for the selected project.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void ProjectSelectedIndexChanged(Object s, EventArgs e) 
		{
			BindQueryFieldTypes();
			foreach (PickQueryField ctlPickQueryField in plhClauses.Controls)
				ctlPickQueryField.Clear();
		}



        /// <summary>
        /// This method adds the number of clauses stored in the ClauseCount property.
        /// </summary>
		void DisplayClauses() 
		{
			for (int i=0 ;i < ClauseCount; i++)
				AddClause(false);
		}

        /// <summary>
        /// This method iterates through each of the query clauses and binds
        /// the clause to the proper data.
        ///
        /// </summary>
		void BindQueryFieldTypes() 
		{
			foreach (PickQueryField ctlPickQueryField in plhClauses.Controls) 
			{
                ctlPickQueryField.ProjectId = ProjectId;
			}
		}

        /// <summary>
        /// Adds the clause.
        /// </summary>
        /// <param name="bindData">if set to <c>true</c> [bind data].</param>
        void AddClause(bool bindData)
        {
            this.AddClause(false, null);
        }

        /// <summary>
        /// This method adds a new query clause to the user interface.
        /// </summary>
        /// <param name="bindData">if set to <c>true</c> [bind data].</param>
		void AddClause(bool bindData, QueryClause queryClause) 
		{
			PickQueryField ctlPickQueryField = (PickQueryField)Page.LoadControl( "~/UserControls/PickQueryField.ascx");

			plhClauses.Controls.Add( ctlPickQueryField );
			ctlPickQueryField.ProjectId = ProjectId;
            if(bindData)
                ctlPickQueryField.QueryClause = queryClause;
		}

        /// <summary>
        ///This method is called when a user clicks the Add Clause button.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void btnAddClauseClick(Object s, EventArgs e) 
		{
			ClauseCount ++;
			AddClause(true);
			btnRemoveClause.Enabled = true;
		}

        /// <summary>
        /// This method is called when a user clicks the Remove Clause button.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void btnRemoveClauseClick(Object s, EventArgs e) 
		{
			if (ClauseCount > 1) 
			{
				ClauseCount --;
				plhClauses.Controls.RemoveAt(plhClauses.Controls.Count-1);
			}

			if (ClauseCount < 2)
				btnRemoveClause.Enabled = false;
		}

        /// <summary>
        /// This method is called when a user clicks the Remove Clause button.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void btnPerformQueryClick(Object s, EventArgs e) 
		{
			ctlDisplayIssues.CurrentPageIndex = 0;
			ExecuteQuery();
		}


        /// <summary>
        /// This method is called when a user clicks the Save Query button.
        /// The method saves the query to a database table.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void btnSaveQueryClick(Object s, EventArgs e) 
		{
            if (Page.IsValid)
            {
              
                string queryName = txtQueryName.Text.Trim();
                string userName = Security.GetUserName();

                if (queryName == String.Empty)
                    return;

                List<QueryClause> queryClauses = BuildQuery();
                if (queryClauses.Count == 0)
                    return;

                bool success = false;

                if (QueryId == 0)
                    success = Query.SaveQuery(userName, ProjectId, queryName, chkGlobalQuery.Checked, queryClauses);
                else
                    success = Query.UpdateQuery(QueryId, userName, ProjectId, queryName, chkGlobalQuery.Checked, queryClauses);

                if (success)
                    Response.Redirect(string.Format("QueryList.aspx?pid={0}", ProjectId));
                else
                    Message1.ShowErrorMessage(GetLocalResourceObject("SaveQueryError").ToString());

            }

		}


        /// <summary>
        /// This method executes a query and displays the results.
        /// </summary>
		void ExecuteQuery() 
		{
            List<QueryClause> colQueryClauses = BuildQuery();

            if (colQueryClauses.Count > 0)
            {
                try
                {
                    List<Issue> colIssues = Issue.PerformQuery(ProjectId, colQueryClauses);
                    ctlDisplayIssues.DataSource = colIssues;
                    Results.Visible = true;
                    ctlDisplayIssues.DataBind();
                }
                catch
                {
                    Message1.ShowErrorMessage(GetLocalResourceObject("RunQueryError").ToString());
                }
               
            }
            else
            {
                Message1.ShowWarningMessage("Please select at least one clause for your query"); //TODO: Localize
            }
		}

        /// <summary>
        /// This method builds a database query by iterating through each query clause.
        /// </summary>
        /// <returns></returns>
		List<QueryClause> BuildQuery() 
		{
            List<QueryClause> colQueryClauses = new List<QueryClause>();

			foreach (PickQueryField ctlPickQuery in plhClauses.Controls) 
			{
				QueryClause objQueryClause = ctlPickQuery.QueryClause;
				if (objQueryClause != null)
					colQueryClauses.Add( objQueryClause );
			}

			return colQueryClauses;
		}


	
	}
}
