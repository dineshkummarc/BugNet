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

namespace BugNET.Administration.Users.UserControls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Roles : System.Web.UI.UserControl
    {
        

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
                lblError.Visible = false;
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        private void BindData()
        { 
          
            MembershipUser user = ITUser.GetUser(UserId);
            lblUserName.Text = user.UserName;

            dropProjects.DataSource = Project.GetAllProjects();
            dropProjects.DataBind();


            if (ITUser.IsInRole(Globals.SuperUserRole))
            {              
                chkSuperUsers.Visible = true;
                chkSuperUsers.Checked = ITUser.IsInRole(lblUserName.Text, 0, Globals.SuperUserRole);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlProjects control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropProjects.SelectedValue != 0)
            {
                RoleList.Items.Clear();
                int projectId = dropProjects.SelectedValue;
                List<Role> roles = Role.GetRolesByProject(projectId);
                foreach (Role r in roles)
                {
                    if (r.ProjectId != 0)
                    {
                        ListItem roleListItem = new ListItem();
                        roleListItem.Text = r.Name;
                        roleListItem.Value = r.Id.ToString();
                        roleListItem.Selected = ITUser.IsInRole(lblUserName.Text, projectId, r.Name);
                        RoleList.Items.Add(roleListItem);

                    }
                }
            }
            else
            {
                RoleList.Items.Clear();
            }
        }

        /// <summary>
        /// Handles the CheckChanged event of the chkSuperUsers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void chkSuperUsers_CheckChanged(object sender, EventArgs e)
        {
            string userName = lblUserName.Text;

            if (chkSuperUsers.Checked && !ITUser.IsInRole(userName, 0, Globals.SuperUserRole))
                Role.AddUserToRole(userName, 1);
            else if (!chkSuperUsers.Checked && ITUser.IsInRole(userName, 0, Globals.SuperUserRole))
                Role.RemoveUserFromRole(userName, 1);
        }
        /// <summary>
        /// CMDs the update roles.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cmdUpdateRoles_Click(object sender, EventArgs e)
        {
            UpdateRolesFromList();
            BindData();
        }

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls.
        /// </summary>
        public override void DataBind()
        {
            BindData();
        }
        /// <summary>
        /// Updates the roles from list.
        /// </summary>
        private void UpdateRolesFromList()
        {
            string userName = lblUserName.Text;

            //if (chkSuperUsers.Visible && !ITUser.IsInRole(userName,0,Globals.SuperUserRole))
            //    Role.AddUserToRole(userName, 1);
            //else if (chkSuperUsers.Visible && !ITUser.IsInRole(userName, 0, Globals.SuperUserRole))
            //    Role.RemoveUserFromRole(userName, 1);

            foreach (ListItem roleListItem in RoleList.Items)
            {
                string roleName = roleListItem.Text;                           
                bool enableRole = roleListItem.Selected;

                if (enableRole && !ITUser.IsInRole(userName,dropProjects.SelectedValue, roleName))
                {
                    Role.AddUserToRole(userName, Convert.ToInt32(roleListItem.Value));
                }
                else if (!enableRole && ITUser.IsInRole(userName,dropProjects.SelectedValue, roleName))
                {
                    Role.RemoveUserFromRole(userName, Convert.ToInt32(roleListItem.Value));
                }
            }
            lblError.Text = GetLocalResourceObject("UserRolesUpdateSuccess").ToString();
            lblError.Visible = true;
            
            
        }
        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public Guid UserId
        {
            get
            {
                if (Request.QueryString["user"] != null || Request.QueryString["user"].Length != 0)
                    try
                    {
                        return new Guid(Request.QueryString["user"].ToString());
                    }
                    catch
                    {
                        throw new Exception(Logging.GetErrorMessageResource("QueryStringError"));
                    }
                else
                    return Guid.Empty;
            }
        }
    }
}