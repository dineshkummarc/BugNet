using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BugNET.BusinessLogicLayer;
using BugNET.UserInterfaceLayer;

namespace BugNET.Administration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Admin : BasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ITUser.IsInRole(Globals.SuperUserRole) && !ITUser.IsInRole(Globals.ProjectAdminRole))
                Response.Redirect("~/Errors/AccessDenied.aspx");            

            if (!Page.IsPostBack)
            {
                // Display a HighSecurityMode warning.
                if (SecurityModes.isHighSecurityMode())
                    lblHighSecurity.Visible = true;


                //hide log viewer and host settings for non superusers
                if (!ITUser.IsInRole(Globals.SuperUserRole))
                {
                    lnkConfiguration.Visible = false;
                    Image4.Visible = false;
                    Image5.Visible = false;
                    lnkLogViewer.Visible = false;
                    //lnkNewProject.Visible = false;
                    //Image3.Visible = false;
                }
            }
        }
    }
}
