using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BugNET.BusinessLogicLayer;
using BugNET.UserInterfaceLayer;

namespace BugNET.Administration.Host.UserControls
{
    public partial class POP3Settings : System.Web.UI.UserControl, IEditHostSettingControl
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
                    HostSetting.UpdateHostSetting("Pop3ReaderEnabled", POP3ReaderEnabled.Checked.ToString());
                    HostSetting.UpdateHostSetting("Pop3Server", POP3Server.Text);
                    HostSetting.UpdateHostSetting("Pop3UseSSL", POP3UseSSL.Checked.ToString());
                    HostSetting.UpdateHostSetting("Pop3Username", POP3Username.Text);
                    HostSetting.UpdateHostSetting("Pop3Password", POP3Password.Text);
                    HostSetting.UpdateHostSetting("Pop3Interval", POP3Interval.Text);
                    HostSetting.UpdateHostSetting("Pop3DeleteAllMessages", POP3DeleteMessages.Checked.ToString());
                    HostSetting.UpdateHostSetting("Pop3InlineAttachedPictures", POP3ProcessAttachments.Checked.ToString());
                    HostSetting.UpdateHostSetting("Pop3BodyTemplate", POP3BodyTemplate.Text);
                    HostSetting.UpdateHostSetting("Pop3ReportingUsername", POP3ReportingUsername.Text);
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
                POP3ReaderEnabled.Checked = Boolean.Parse(HostSetting.GetHostSetting("Pop3ReaderEnabled"));
                POP3Server.Text = HostSetting.GetHostSetting("Pop3Server");
                POP3UseSSL.Checked = Boolean.Parse(HostSetting.GetHostSetting("Pop3UseSSL"));
                POP3Username.Text = HostSetting.GetHostSetting("Pop3Username");
                POP3Interval.Text = HostSetting.GetHostSetting("Pop3Interval");
                POP3DeleteMessages.Checked = Boolean.Parse(HostSetting.GetHostSetting("Pop3DeleteAllMessages"));
                POP3ProcessAttachments.Checked = Boolean.Parse(HostSetting.GetHostSetting("Pop3InlineAttachedPictures"));
                POP3BodyTemplate.Text = HostSetting.GetHostSetting("Pop3BodyTemplate");
                POP3ReportingUsername.Text = HostSetting.GetHostSetting("Pop3ReportingUsername");
                POP3Password.Attributes.Add("value", HostSetting.GetHostSetting("Pop3Password"));
            }
            else
            {
                string tmpstr = "Security Mode";
                POP3ReaderEnabled.Checked = false;
                POP3Server.Text = tmpstr;
                POP3UseSSL.Checked = false;
                POP3Username.Text = tmpstr;
                POP3Interval.Text = "10000";
                POP3DeleteMessages.Checked = false;
                POP3ProcessAttachments.Checked = false;
                POP3BodyTemplate.Text = HostSetting.GetHostSetting("Pop3BodyTemplate");
                POP3ReportingUsername.Text = tmpstr;
                POP3Password.Attributes.Add("value", tmpstr);
            }
        }

        #endregion
    }
}