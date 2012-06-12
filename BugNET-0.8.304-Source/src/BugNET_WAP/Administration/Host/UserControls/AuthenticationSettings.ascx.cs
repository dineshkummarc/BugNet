using System;
using System.Web.UI;
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;

namespace BugNET.Administration.Host.UserControls
{
    public partial class AuthenticationSettings : System.Web.UI.UserControl, IEditHostSettingControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        #region IEditHostSettingControl Members


        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            if (Page.IsValid)
            {
                if (!SecurityModes.isHighSecurityMode())
                {
                    HostSetting.UpdateHostSetting("UserAccountSource", UserAccountSource.SelectedValue);
                    HostSetting.UpdateHostSetting("ADUserName", ADUserName.Text);
                    HostSetting.UpdateHostSetting("ADPath", ADPath.Text);
                    HostSetting.UpdateHostSetting("DisableUserRegistration", DisableUserRegistration.Checked.ToString());
                    HostSetting.UpdateHostSetting("DisableAnonymousAccess", DisableAnonymousAccess.Checked.ToString());
                    HostSetting.UpdateHostSetting("OpenIdAuthentication", OpenIdAuthentication.SelectedValue);
                    HostSetting.UpdateHostSetting("ADPassword", ADPassword.Text);
                }
            }

        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Initialize()
        {
            if (!SecurityModes.isHighSecurityMode())
            {
                try
                {
                    UserAccountSource.SelectedValue = HostSetting.GetHostSetting("UserAccountSource");
                }
                catch
                {
                    UserAccountSource.SelectedValue = "None";
                }
                ADUserName.Text = HostSetting.GetHostSetting("ADUserName");
                ADPath.Text = HostSetting.GetHostSetting("ADPath");
                DisableUserRegistration.Checked = Boolean.Parse(HostSetting.GetHostSetting("DisableUserRegistration"));
                DisableAnonymousAccess.Checked = Boolean.Parse(HostSetting.GetHostSetting("DisableAnonymousAccess"));
                OpenIdAuthentication.SelectedValue = HostSetting.GetHostSetting("OpenIdAuthentication");
                ADPassword.Attributes.Add("value", HostSetting.GetHostSetting("ADPassword"));
                ShowHideUserAccountSourceCredentials();
            }
            else
            {
                string tmpstr ="Security Mode";
                UserAccountSource.SelectedValue = "None";
                ADUserName.Text = tmpstr;
                ADPath.Text = tmpstr;
                DisableUserRegistration.Checked = Boolean.Parse(HostSetting.GetHostSetting("DisableUserRegistration"));
                DisableAnonymousAccess.Checked = Boolean.Parse(HostSetting.GetHostSetting("DisableAnonymousAccess"));
                OpenIdAuthentication.SelectedValue = HostSetting.GetHostSetting("OpenIdAuthentication");
                ADPassword.Attributes.Add("value", tmpstr);
                ShowHideUserAccountSourceCredentials();
            }
           
        }

        #endregion

        /// <summary>
        /// Handles the SelectedIndexChanged event of the UserAccountSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void UserAccountSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideUserAccountSourceCredentials();

        }

        /// <summary>
        /// Shows the hide user account source credentials.
        /// </summary>
        private void ShowHideUserAccountSourceCredentials()
        {
            if (UserAccountSource.SelectedValue != "None")
            {
                trADPassword.Visible = true;
                trADUserName.Visible = true;
                trADPath.Visible = true;
            }
            else
            {
                trADPassword.Visible = false;
                trADUserName.Visible = false;
                trADPath.Visible = false;
            }
        }
    }
}