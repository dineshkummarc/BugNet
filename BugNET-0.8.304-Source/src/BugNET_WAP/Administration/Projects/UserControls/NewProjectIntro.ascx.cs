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

	/// <summary>
	///		Summary description for NewProjectIntro.
	/// </summary>
	public partial class NewProjectIntro : System.Web.UI.UserControl, IEditProjectControl
	{
		private int _ProjectId = -1;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}
		
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		#region IEditProjectControl Members

        /// <summary>
        /// Inits this instance.
        /// </summary>
		public void Initialize()
		{}

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
		/// Updates the control and skips the wizard if the checkbox is checked
		/// </summary>
		/// <returns></returns>
		public bool Update() 
		{

            if (chkSkip.Checked)
            {
                Response.Cookies[Globals.SkipProjectIntro].Value = "1";
                Response.Cookies[Globals.SkipProjectIntro].Path = "/";
                Response.Cookies[Globals.SkipProjectIntro].Expires = DateTime.MaxValue;
                Response.Redirect("AddProject.aspx");
            }
    
			return true;
		}

		#endregion

        #region IEditProjectControl Members


        public bool ShowSaveButton
        {
            get { return false; }
        }

        #endregion
    }
}
