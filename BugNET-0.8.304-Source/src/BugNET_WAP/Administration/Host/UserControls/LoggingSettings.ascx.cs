using System;
using System.Web.UI;
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;

namespace BugNET.Administration.Host.UserControls
{
    public partial class LoggingSettings : System.Web.UI.UserControl, IEditHostSettingControl
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
                if (EmailErrors.Checked)
                    Logging.ConfigureEmailLoggingAppender();
                else
                    Logging.RemoveEmailLoggingAppender();

                HostSetting.UpdateHostSetting("ErrorLoggingEmailAddress", ErrorLoggingEmail.Text);
                HostSetting.UpdateHostSetting("EmailErrors", EmailErrors.Checked.ToString());
            }
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Initialize()
        {
            ErrorLoggingEmail.Text = HostSetting.GetHostSetting("ErrorLoggingEmailAddress");
            EmailErrors.Checked = Boolean.Parse(HostSetting.GetHostSetting("EmailErrors"));
        }

        #endregion
    }
}