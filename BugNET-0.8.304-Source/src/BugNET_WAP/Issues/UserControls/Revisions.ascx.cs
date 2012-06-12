using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;

namespace BugNET.Issues.UserControls
{
    public partial class Revisions : System.Web.UI.UserControl, IIssueTab
    {
        private int _IssueId = 0;
        private int _ProjectId = 0;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region IIssueTab Members

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
            IssueRevisionsDataGrid.Columns[0].HeaderText = GetLocalResourceObject("IssueRevisionsDataGrid.RevisionHeader.Text").ToString();
            IssueRevisionsDataGrid.Columns[1].HeaderText = GetLocalResourceObject("IssueRevisionsDataGrid.AuthorHeader.Text").ToString();
            IssueRevisionsDataGrid.Columns[2].HeaderText = GetLocalResourceObject("IssueRevisionsDataGrid.RevisionDateHeader.Text").ToString();
            IssueRevisionsDataGrid.Columns[3].HeaderText = GetLocalResourceObject("IssueRevisionsDataGrid.RepositoryHeader.Text").ToString();
            IssueRevisionsDataGrid.Columns[4].HeaderText = GetLocalResourceObject("IssueRevisionsDataGrid.MessageHeader.Text").ToString();

            BindIssueRevisions();
        }

        #endregion

        private void BindIssueRevisions()
        {
            List<IssueRevision> revisions = IssueRevision.GetIssueRevisionsByIssueId(IssueId);
            if (revisions.Count == 0)
            {
                IssueRevisionsLabel.Text = GetLocalResourceObject("NoRevisions").ToString();
                IssueRevisionsLabel.Visible = true;
                IssueRevisionsDataGrid.Visible = false;
            }
            else
            {
                IssueRevisionsDataGrid.DataSource = revisions;
                IssueRevisionsDataGrid.DataKeyField = "IssueId";
                IssueRevisionsDataGrid.DataBind();
                IssueRevisionsLabel.Visible = false;
                IssueRevisionsDataGrid.Visible = true;
            }
        }
    }
}