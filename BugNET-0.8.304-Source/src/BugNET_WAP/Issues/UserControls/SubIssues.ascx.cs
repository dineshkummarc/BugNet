namespace BugNET.Issues.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using BugNET.BusinessLogicLayer;
    using BugNET.UserInterfaceLayer;

	/// <summary>
	///		Summary description for SubIssues.
	/// </summary>
	public partial class SubIssues : System.Web.UI.UserControl, IIssueTab
	{
		
		private int _IssueId = 0;
        private int _ProjectId = 0;

        /// <summary>
        /// Binds the related.
        /// </summary>
		protected void BindRelated() 
		{
			grdIssues.DataSource = RelatedIssue.GetChildIssues(_IssueId);
			grdIssues.DataKeyField = "IssueId";
			grdIssues.DataBind();
		}


        /// <summary>
        /// GRDs the bugs item data bound.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
		protected void grdIssuesItemDataBound(Object s, DataGridItemEventArgs e) 
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
			{
				RelatedIssue currentIssue = (RelatedIssue)e.Item.DataItem;

				Label lblIssueId = (Label)e.Item.FindControl( "lblIssueId" );
				lblIssueId.Text = currentIssue.IssueId.ToString();
                Label lblIssueStatus = (Label)e.Item.FindControl("IssueStatusLabel");
                lblIssueStatus.Text = currentIssue.Status;
                Label lblIssueResolution = (Label)e.Item.FindControl("IssueResolutionLabel");
                lblIssueResolution.Text = currentIssue.Resolution;
			}
		}

        /// <summary>
        /// GRDs the bugs item command.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
		protected void grdIssuesItemCommand(Object s, DataGridCommandEventArgs e) 
		{
			int IssueId = (int)grdIssues.DataKeys[e.Item.ItemIndex];
			RelatedIssue.DeleteChildIssue(_IssueId, IssueId);
			BindRelated();
		}

        /// <summary>
        /// Adds the related bug.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void AddRelatedIssue(Object s, EventArgs e) 
		{
			if (txtIssueId.Text == String.Empty)
				return;

			if (Page.IsValid) 
			{
				RelatedIssue.CreateNewChildIssue(_IssueId, Int32.Parse(txtIssueId.Text) );
				txtIssueId.Text = String.Empty;
				BindRelated();
			}
		}



        #region IIssueTab Members

        /// <summary>
        /// Gets or sets the bug id.
        /// </summary>
        /// <value>The bug id.</value>
        public int IssueId
        {
            get { return _IssueId; }
            set { _IssueId = value; }
        }


        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>The project id.</value>
        public int ProjectId
        {
            get { return _ProjectId; }
            set { _ProjectId = value; }
        }


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            BindRelated();

            if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.DELETE_SUB_ISSUE.ToString()))
                grdIssues.Columns[4].Visible = false;

            //check users role permission for adding a related issue
            if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.ADD_SUB_ISSUE.ToString()))
                AddSubIssuePanel.Visible = false;
        }
        #endregion
    }
}
