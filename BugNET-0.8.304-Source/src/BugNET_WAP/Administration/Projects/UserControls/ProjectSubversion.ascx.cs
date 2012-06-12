namespace BugNET.Administration.Projects.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Collections;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using BugNET.BusinessLogicLayer;
	using BugNET.UserInterfaceLayer;
	using BugNET.UserControls;


	/// <summary>
	///		Summary description for ProjectDescription.
	/// </summary>
	public partial class ProjectSubversion : System.Web.UI.UserControl,IEditProjectControl
	{

        private int _ProjectId = -1;
        
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, System.EventArgs e)
		{

		}

        /// <summary>
        /// Handles the Click event of the createRepoBttn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void createRepoBttn_Click(object sender, EventArgs e)
        {
            string name = repoName.Text.Trim();

            if (SecurityModes.isHighSecurityMode())
            {
                createErrorLbl.Text = "Security Mode";
                return;
            }

            if(!SubversionIntegration.IsValidSubversionName(name)) {
                createErrorLbl.Text = Logging.GetErrorMessageResource("InvalidRepositoryName");
                return;
            }

            svnOut.Text = SubversionIntegration.CreateRepository(name);

            string rootUrl = HostSetting.GetHostSetting("RepoRootUrl");
            if(!rootUrl.EndsWith("/"))
                rootUrl += "/";

            svnUrl.Text = rootUrl + name;

        }

        /// <summary>
        /// Handles the Click event of the createTagBttn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void createTagBttn_Click(object sender, EventArgs e)
        {
            string name = tagName.Text.Trim();

            if (SecurityModes.isHighSecurityMode())
            {
                lblError.Text = "Security Mode";
                return;
            }
 
            if (tagComment.Text.Trim().Length == 0)
                createTagErrorLabel.Text = Logging.GetErrorMessageResource("RepositoryCommentRequired");
            else if(!SubversionIntegration.IsValidSubversionName(name))
                createTagErrorLabel.Text = Logging.GetErrorMessageResource("InvalidRepositoryTagName");
            else
                svnOut.Text = SubversionIntegration.CreateTag(this.ProjectId, name, tagComment.Text, tagUserName.Text.Trim(), tagPassword.Text.Trim());
        }

		#region IEditProjectControl Members

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>The project id.</value>
		public int ProjectId
		{
            get
            {
                return _ProjectId;
            }

            set { _ProjectId = value; }
		}

        /// <summary>
        /// Gets a value indicating whether [show save button].
        /// </summary>
        /// <value><c>true</c> if [show save button]; otherwise, <c>false</c>.</value>
        public bool ShowSaveButton
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
		public void Initialize()
		{
           Project projectToUpdate = Project.GetProjectById(ProjectId);

           if (!SecurityModes.isHighSecurityMode())
           {
               svnUrl.Text = projectToUpdate.SvnRepositoryUrl;
           }
           else 
               svnUrl.Text = "Security Mode";            

           bool svnAdminEnabled = bool.Parse(HostSetting.GetHostSetting("EnableRepositoryCreation"));

           if (svnAdminEnabled)
           {
               createErrorLbl.Text = "";
               createRepoBttn.Enabled = true;
               repoName.Enabled = true;
           }
           else
           {
               createErrorLbl.Text = GetLocalResourceObject("RepositoryCreationDisabled").ToString();
               createRepoBttn.Enabled = false;
               repoName.Enabled = false;
           }
			
		}

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
		public bool Update()
		{
			if (Page.IsValid) 
			{
                if (!SecurityModes.isHighSecurityMode())
                {
                    Project projectToUpdate = Project.GetProjectById(ProjectId);
                    projectToUpdate.SvnRepositoryUrl = svnUrl.Text;


                    if (projectToUpdate.Save())
                    {
                        ProjectId = projectToUpdate.Id;
                        return true;
                    }
                    else
                        lblError.Text = Logging.GetErrorMessageResource("RepositoryProjectSaveError");
                }
                else
                {
                    lblError.Text = "Security Mode";
                }
            }
			return false;
		}
      

		#endregion
	}
}
