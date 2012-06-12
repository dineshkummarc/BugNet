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

namespace BugNET.Administration.Projects
{
    public partial class CloneProject : BasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ITUser.HasPermission(Convert.ToInt32(Request.QueryString["id"]), Globals.Permission.ADMIN_CLONE_PROJECT.ToString()))
                Response.Redirect("~/Errors/AccessDenied.aspx");

            if (!IsPostBack)
            {
                // Bind projects to dropdownlist
                ddlProjects.DataSource = Project.GetAllProjects();
                ddlProjects.DataBind();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnClone control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnClone_Click(object sender, System.EventArgs e)
        {
            if (IsValid)
            {
                bool success = Project.CloneProject(Convert.ToInt32(ddlProjects.SelectedValue), txtNewProjectName.Text);
                if (success)
                    Response.Redirect("ProjectList.aspx");
                else
                    lblError.Text = Logging.GetErrorMessageResource("CloneProjectError");
            }

        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("ProjectList.aspx");
        }
    }
}
