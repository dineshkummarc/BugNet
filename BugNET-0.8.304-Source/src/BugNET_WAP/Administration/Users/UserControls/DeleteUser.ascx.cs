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
using BugNET.BusinessLogicLayer;

namespace BugNET.Administration.Users.UserControls
{
    public partial class DeleteUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public Guid UserId
        {
            get
            {
                if (Request.QueryString["user"] != null && Request.QueryString["user"].Length != 0)
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

        /// <summary>
        /// Handles the Click event of the cmdUnauthorizeAccount control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdUnauthorizeAccount_Click(object sender, EventArgs e)
        {
            try
            {           
                MembershipUser objUser = ITUser.GetUser(UserId);

                // Do not disable superusers in HighSecurityMode
                if ((!ITUser.IsInRole(objUser.UserName, Globals.SuperUserRole) &&
                    (!SecurityModes.isHighSecurityMode())))
                {
                    objUser.IsApproved = false;
                    ITUser.UpdateUser(objUser);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdDeleteUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void cmdDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipUser objUser = ITUser.GetUser(UserId);
                // Do not delete superusers in HighSecurityMode
                if ((!ITUser.IsInRole(objUser.UserName, Globals.SuperUserRole) &&
                    (!SecurityModes.isHighSecurityMode())))
                {

                    System.Web.Security.Membership.DeleteUser(objUser.UserName);                    
                }
                Response.Redirect("~/Administration/Users/UserList.aspx");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}