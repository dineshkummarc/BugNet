﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BugNET.UserInterfaceLayer;

namespace BugNET.Administration.Users
{
    public partial class ManageUser : BasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (TabId == 2)
                    ShowRolesPanel(this, new EventArgs());
                else
                    ShowMembershipPanel(this, new EventArgs());
            }
        }

        /// <summary>
        /// Gets the tab id.
        /// </summary>
        /// <value>The tab id.</value>
        public int TabId
        {

            get
            {
                if (Request.QueryString["tabid"] != null && Request.QueryString["tabid"].Length > 0)
                    return Convert.ToInt32(Request.QueryString["tabid"]);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Shows the panel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ShowMembershipPanel(object sender, EventArgs e)
        {
            ctlMembership.DataBind();
            pnlMembership.Visible = true;
            pnlPassword.Visible = false;
            pnlProfile.Visible = false;
            pnlDelete.Visible = false;
            pnlRoles.Visible = false;
            cmdManageDetails.Enabled = false;
            ibMembership.Enabled = false;
            cmdManageRoles.Enabled = true;
            ibManageRoles.Enabled = true;
            cmdManageProfile.Enabled = true;
            ibManageProfile.Enabled = true;
            cmdManagePassword.Enabled = true;
            ibManagePassword.Enabled = true;
            cmdDelete.Enabled = true;
            ibDelete.Enabled = true;
        }

        /// <summary>
        /// Shows the roles panel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ShowRolesPanel(object sender, EventArgs e)
        {
            ctlRoles.DataBind();
            pnlMembership.Visible = false;
            pnlPassword.Visible = false;
            pnlProfile.Visible = false;
            pnlRoles.Visible = true;
            pnlDelete.Visible = false;
            cmdManageDetails.Enabled = true;
            ibMembership.Enabled = true;
            cmdManageRoles.Enabled = false;
            ibManageRoles.Enabled = false;
            cmdManageDetails.Enabled = true;
            ibMembership.Enabled = true;
            cmdManageProfile.Enabled = true;
            ibManageProfile.Enabled = true;
            cmdManagePassword.Enabled = true;
            ibManagePassword.Enabled = true;
            cmdDelete.Enabled = true;
            ibDelete.Enabled = true;
        }

        /// <summary>
        /// Shows the profile panel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ShowProfilePanel(object sender, EventArgs e)
        {
            ctlProfile.DataBind();
            pnlMembership.Visible = false;
            pnlPassword.Visible = false;
            pnlDelete.Visible = false;
            pnlProfile.Visible = true;
            pnlRoles.Visible = false;
            cmdManageDetails.Enabled = true;
            ibMembership.Enabled = true;
            cmdManageRoles.Enabled = true;
            ibManageRoles.Enabled = true;
            cmdManageDetails.Enabled = true;
            ibMembership.Enabled = true;
            cmdManageProfile.Enabled = false;
            ibManageProfile.Enabled = false;
            cmdManagePassword.Enabled = true;
            ibManagePassword.Enabled = true;
            cmdDelete.Enabled = true;
            ibDelete.Enabled = true;
        }

        /// <summary>
        /// Shows the delete panel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ShowDeletePanel(object sender, EventArgs e)
        {
            ctlDeleteUser.DataBind();
            pnlMembership.Visible = false;
            pnlPassword.Visible = false;
            pnlProfile.Visible = false;
            pnlDelete.Visible = true;
            pnlRoles.Visible = false;
            cmdManageDetails.Enabled = true;
            cmdDelete.Enabled = false;
            ibDelete.Enabled = false;
            ibMembership.Enabled = true;
            cmdManageRoles.Enabled = true;
            ibManageRoles.Enabled = true;
            cmdManageDetails.Enabled = true;
            ibMembership.Enabled = true;
            cmdManageProfile.Enabled = true;
            ibManageProfile.Enabled = true;
            cmdManagePassword.Enabled = true;
            ibManagePassword.Enabled = true;
        }

        /// <summary>
        /// Shows the password panel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ShowPasswordPanel(object sender, EventArgs e)
        {
            ctlPassword.DataBind();
            pnlMembership.Visible = false;
            pnlPassword.Visible = true;
            pnlProfile.Visible = false;
            pnlDelete.Visible = false;
            pnlRoles.Visible = false;
            cmdManageDetails.Enabled = true;
            cmdDelete.Enabled = true;
            ibDelete.Enabled = true;
            ibMembership.Enabled = true;
            cmdManageRoles.Enabled = true;
            ibManageRoles.Enabled = true;
            cmdManageDetails.Enabled = true;
            ibMembership.Enabled = true;
            cmdManageProfile.Enabled = true;
            ibManageProfile.Enabled = true;
            cmdManagePassword.Enabled = false;
            ibManagePassword.Enabled = false;
        }

        /// <summary>
        /// Handles the Click event of the cmdCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/Users/UserList.aspx");
        }

        /// <summary>
        /// Handles the Click event of the AddUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Administration/Users/AddUser.aspx");
        }
    }
}
