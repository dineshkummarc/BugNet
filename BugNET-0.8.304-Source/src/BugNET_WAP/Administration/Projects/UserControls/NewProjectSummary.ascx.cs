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
	///		Summary description for NewProjectSummary.
	/// </summary>
	public partial class NewProjectSummary : System.Web.UI.UserControl,IEditProjectControl
	{

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

		private int _ProjectId = -1;


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
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
		public bool Update() 
		{
			return true;
		}

        /// <summary>
        /// Initializes this instance.
        /// </summary>
		public void Initialize() 
		{
		}

        #region IEditProjectControl Members


        public bool ShowSaveButton
        {
            get { return false; }
        }

        #endregion
    }
}
