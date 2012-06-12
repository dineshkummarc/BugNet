namespace BugNET.UserControls
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using BugNET.BusinessLogicLayer;
    using BugNET.UserInterfaceLayer;
    using BugNET.UserControls;


    public partial class Banner : System.Web.UI.UserControl
    {

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //hide user registration if disabled in host settings
            if (!Page.User.Identity.IsAuthenticated && Boolean.Parse(HostSetting.GetHostSetting("DisableUserRegistration")))
            {
                if (LoginView1.FindControl("lnkRegister") != null)
                    LoginView1.FindControl("lnkRegister").Visible = false;
                if (LoginView1.FindControl("lblBar") != null)
                    LoginView1.FindControl("lblBar").Visible = false;
            }

            //add so the login/logout link would be hidden for someone using windows authentication
            if (HttpContext.Current.User.Identity.AuthenticationType == "Negotiate")
            {
                if (LoginView1.FindControl("LoginStatus1") != null)
                    LoginView1.FindControl("LoginStatus1").Visible = false;
                if (LoginView1.FindControl("lblBar") != null)
                    LoginView1.FindControl("lblBar").Visible = false;
            }

            // Make the search links invisible to anonymous users if set in Host Settings
            if (Boolean.Parse(HostSetting.GetHostSetting("DisableAnonymousAccess")) && (!Page.User.Identity.IsAuthenticated))            
                Panel1.Visible = false;

            AppTitle.Text = HostSetting.GetHostSetting("ApplicationTitle").ToString();

            // If we are in the Administration Section and we are running in 
            // HighSecurityMode, Display a warning.

            if (Context.Request.Path.ToLower().Contains("/administration/"))
                if (SecurityModes.isHighSecurityMode())
                {
                    AppTitle.Text += " [High Security Mode]";
                }

            ddlProject.DataTextField = "Name";
            ddlProject.DataValueField = "Id";

            if (!Page.IsPostBack)
            {
                if (Page.User.Identity.IsAuthenticated)
                {
                    ddlProject.DataSource = Project.GetProjectsByMemberUserName(Security.GetUserName(), true);
                    ddlProject.DataBind();
                    ddlProject.Items.Insert(0, new ListItem("-- Select Project --"));
                }
                else if (!Page.User.Identity.IsAuthenticated && !Boolean.Parse(HostSetting.GetHostSetting("DisableAnonymousAccess")))
                {
                    ddlProject.DataSource = Project.GetPublicProjects();
                    ddlProject.DataBind();
                    ddlProject.Items.Insert(0, new ListItem("-- Select Project --"));
                }
                else
                {
                    pnlHeaderNav.Visible = false;
                }

                int projectId = Request.QueryString["pid"].ToInt();

                if (projectId > 0)
                {
                    ListItem li = ddlProject.Items.FindByValue(projectId.ToString());
                    if (li != null) ddlProject.SelectedIndex = ddlProject.Items.IndexOf(li);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Profile control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Profile_Click(object s, EventArgs e)
        {
            Response.Redirect(string.Format("~/UserProfile.aspx?referrerurl={0}", Request.RawUrl));
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlProject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            int projectId = ddlProject.SelectedValue.ToInt();

            // if no project id selected send em to the resource not found page
            // todo: select validation for ddlProject at UI level, stop postback using jQuery events
            if (projectId <= 0) {
                Response.Redirect("~/Errors/NotFound.aspx", true);
            }

            Response.Redirect(string.Format("~/Projects/ProjectSummary.aspx?pid={0}", projectId));
        }

        /// <summary>
        /// Handles the Click event of the btnSearch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Stewart Moss
            // Apr 9 2010 
            //
            // Some code improvements to reduce errors on searching using the quick find box.
            // Also new feature to recognize a BugNet IssueID and try to search on that.
            // eg "AGN-123 or BUGNET-12 or B-9912"

            string strsearch = txtIssueId.Text.Trim();
            bool validflag = false;
            if (strsearch.Length != 0)
            {
                // turn off the error message
                this.QuickError.Visible = false;
                int IssueId = -1;
                try
                {
                    // First check.. Is this an integer?
                    IssueId = int.Parse(strsearch);
                    validflag = true;
                }
                catch
                {
                    // Not an integer.
                    //
                    // Test if the search box contain a valid BUGNET reference number 
                    // eg "AGN-123 or BUGNET-12 or B-9912"

                    if (strsearch.Contains("-"))
                    {
                        try
                        {
                            IssueId = int.Parse(strsearch.Substring(strsearch.IndexOf('-') + 1));
                            validflag = true;
                        }
                        catch
                        {
                            // the invalid flag is already set
                        }
                    }

                    if (!validflag)
                    {
                        // Display secondary search error
                        this.QuickError.Visible = true;
                        this.QuickError.Text = "Enter a number or a BugNet ID.";
                        return;
                    }
                }

                // zero is a reserved ID
                if (IssueId == 0)
                {
                    validflag = false;
                }

                if (validflag)
                {
                    if (Issue.IsValidId(IssueId))
                    {
                        Response.Redirect(string.Format("~/Issues/IssueDetail.aspx?&id={0}", IssueId.ToString()));
                    }
                    else validflag = false;
                }

                if (!validflag)
                {
                    // Display primary search error
                    this.QuickError.Visible = true;
                    this.QuickError.Text = "Invalid Issue ID";
                }
            }
        }
    }
}