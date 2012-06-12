using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BugNET.BusinessLogicLayer;
using BugNET.UserInterfaceLayer;
using System.Web.Security;
using Clearscreen.SharpHIP;

namespace BugNET
{
    /// <summary>
    /// Summary description for Register.
    /// </summary>
    public partial class Register : System.Web.UI.Page
    {

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Page.Title = string.Format("{0} - {1}", GetLocalResourceObject("Page.Title").ToString(), HostSetting.GetHostSetting("ApplicationTitle"));

            #region Security Check
            //redirect to access denied if user registration disabled
            if (Boolean.Parse(HostSetting.GetHostSetting("DisableUserRegistration")))
                Response.Redirect("~/AccessDenied.aspx", true);
            #endregion

            // if you are logged in, you cant register
            if (Context.Request.IsAuthenticated)
            {
                Response.Redirect("~/", true);
            }
        }

        /// <summary>
        /// Handles the CreateUser event of the CreateUserWizard1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CreatingUser(object sender, LoginCancelEventArgs e)
        {
            HIPControl captcha = (HIPControl)CreateUserWizardStep0.ContentTemplateContainer.FindControl("CapchaTest");

            if (!captcha.IsValid || !Page.IsValid)
            {

                e.Cancel = true;
            }
        }

        /// <summary>
        /// Handles the CreatedUser event of the CreateUserWizard1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {

            string userName = CreateUserWizard1.UserName;

            MembershipUser user = ITUser.GetUser(userName);

            if (user != null)
            {
                TextBox FirstName = (TextBox)CreateUserWizardStep0.ContentTemplateContainer.FindControl("FirstName");
                TextBox LastName = (TextBox)CreateUserWizardStep0.ContentTemplateContainer.FindControl("LastName");
                TextBox FullName = (TextBox)CreateUserWizardStep0.ContentTemplateContainer.FindControl("FullName");

                WebProfile Profile = new WebProfile().GetProfile(user.UserName);

                Profile.FirstName = FirstName.Text;
                Profile.LastName = LastName.Text;
                Profile.DisplayName = FullName.Text;
                Profile.Save();

                //auto assign user to roles
                List<Role> roles = Role.GetAllRoles();
                foreach (Role r in roles)
                {
                    if (r.AutoAssign)
                        Role.AddUserToRole(user.UserName, r.Id);
                }

                //send notification this user was created
                ITUser.SendUserRegisteredNotification(user.UserName);
            }
        }


    }
}
