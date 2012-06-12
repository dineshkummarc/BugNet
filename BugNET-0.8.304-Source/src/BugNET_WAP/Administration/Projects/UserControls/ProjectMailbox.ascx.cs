namespace BugNET.Administration.Projects.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using BugNET.BusinessLogicLayer;
	using BugNET.UserInterfaceLayer;
	using BugNET.UserControls;
	using System.Collections;
	using System.IO;

	/// <summary>
	///		Summary description for Mailboxes.
	/// </summary>
	public partial class Mailboxes : System.Web.UI.UserControl, IEditProjectControl
	{
		private int _ProjectId = -1;
		IList _Mailboxes = null;
		protected System.Web.UI.WebControls.TextBox txtMailboxeDesc;
		protected HtmlControl AddMailbox;

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
			this.dtgMailboxes.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dtgMailboxes_DeleteCommand);

		}
		#endregion

        /// <summary>
        /// Binds the mailboxes.
        /// </summary>
		private void BindMailboxes()
		{
			_Mailboxes = ProjectMailbox.GetMailboxsByProjectId(ProjectId);

			if (_Mailboxes.Count == 0)
			{
				lblMailboxes.Text = "There are no Mailboxes for this project.";
				lblMailboxes.Visible=true;
				dtgMailboxes.Visible=false;
			}
			else
			{
				dtgMailboxes.Visible=true;
				lblMailboxes.Visible=false;
				dtgMailboxes.DataSource = _Mailboxes;
				dtgMailboxes.DataBind();
			}

			txtMailbox.Text = String.Empty;
//			IssueAssignedUser.SelectedValue = -1;
//			IssueAssignedType.SelectedValue = 0;

//			if(Security.GetUserRole().Equals(Globals.ReadOnlyRole))
//				AddMailbox.Visible=false;
			
		}

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		protected void btnAdd_Click(object sender, EventArgs e)
		{
			ProjectMailbox newMailbox = new ProjectMailbox(txtMailbox.Text, ProjectId, IssueAssignedUser.SelectedValue,Guid.Empty, IssueAssignedType.SelectedValue);

            if (newMailbox.Add())
               BindMailboxes();
		}

		#region IEditProjectControl Members

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
        /// Gets a value indicating whether [show save button].
        /// </summary>
        /// <value><c>true</c> if [show save button]; otherwise, <c>false</c>.</value>
        public bool ShowSaveButton
        {
            get { return false; }
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
		public void Initialize()
		{
			BindMailboxes();

            IssueAssignedUser.DataSource = ITUser.GetUsersByProjectId(ProjectId);
			IssueAssignedUser.DataBind();

            IssueAssignedType.DataSource = IssueType.GetIssueTypesByProjectId(ProjectId);
			IssueAssignedType.DataBind();
		}

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
		public bool Update()
		{
			if (Page.IsValid)
				return true;
			else
				return false;
		}

		#endregion

        /// <summary>
        /// Handles the DeleteCommand event of the dtgMailboxes control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
		private void dtgMailboxes_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			_Mailboxes = ProjectMailbox.GetMailboxsByProjectId(ProjectId);
			ProjectMailbox mb = _Mailboxes[e.Item.DataSetIndex] as ProjectMailbox;

			if (mb != null && mb.Delete())
				BindMailboxes();
		}
        /// <summary>
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/Projects/EditProject.aspx?id=" + ProjectId.ToString());
        }
	}
}
