using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;


namespace BugNET.Administration.Host.UserControls
{
    public partial class AttachmentSettings : System.Web.UI.UserControl, IEditHostSettingControl
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
                    HostSetting.UpdateHostSetting("AllowedFileExtensions", AllowedFileExtentions.Text.Trim());
                    HostSetting.UpdateHostSetting("FileSizeLimit", FileSizeLimit.Text.Trim());
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
                AllowedFileExtentions.Text = HostSetting.GetHostSetting("AllowedFileExtensions");
            }
            else
            {
                AllowedFileExtentions.Text = "Security Mode";
            }

            FileSizeLimit.Text = HostSetting.GetHostSetting("FileSizeLimit");
        }

        #endregion
    }
}