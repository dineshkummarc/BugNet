using System;
using System.Web.UI;
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using log4net;

namespace BugNET.Administration.Host.UserControls
{
    public partial class MailSettings : System.Web.UI.UserControl, IEditHostSettingControl
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MailSettings));

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
                    HostSetting.UpdateHostSetting("HostEmailAddress", HostEmail.Text);
                    HostSetting.UpdateHostSetting("SMTPServer", SMTPServer.Text);
                    HostSetting.UpdateHostSetting("SMTPAuthentication", SMTPEnableAuthentication.Checked.ToString());
                    HostSetting.UpdateHostSetting("SMTPUsername", SMTPUsername.Text);
                    HostSetting.UpdateHostSetting("SMTPPassword", SMTPPassword.Text);
                    HostSetting.UpdateHostSetting("SMTPDomain", SMTPDomain.Text);
                    HostSetting.UpdateHostSetting("SMTPPort", SMTPPort.Text);
                    HostSetting.UpdateHostSetting("SMTPUseSSL", SMTPUseSSL.Checked.ToString());
                    HostSetting.UpdateHostSetting("SMTPEMailFormat", SMTPEmailFormat.SelectedValue);
                    HostSetting.UpdateHostSetting("SMTPEmailTemplateRoot", SMTPEmailTemplateRoot.Text);
                }
            }

        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            if (!SecurityModes.isHighSecurityMode())
            {

                HostEmail.Text = HostSetting.GetHostSetting("HostEmailAddress");
                SMTPServer.Text = HostSetting.GetHostSetting("SMTPServer");
                SMTPEnableAuthentication.Checked = Boolean.Parse(HostSetting.GetHostSetting("SMTPAuthentication"));
                SMTPUsername.Text = HostSetting.GetHostSetting("SMTPUsername");
                SMTPPort.Text = HostSetting.GetHostSetting("SMTPPort");
                SMTPUseSSL.Checked = Boolean.Parse(HostSetting.GetHostSetting("SMTPUseSSL"));
                SMTPPassword.Attributes.Add("value", HostSetting.GetHostSetting("SMTPPassword"));
                ShowSMTPAuthenticationFields();
                SMTPEmailFormat.SelectedValue = HostSetting.GetHostSetting("SMTPEMailFormat", (int)EmailFormatTypes.Text).ToString();
                SMTPEmailTemplateRoot.Text = HostSetting.GetHostSetting("SMTPEmailTemplateRoot", "~/templates").ToString();
                SMTPDomain.Text = HostSetting.GetHostSetting("SMTPDomain", string.Empty).ToString();
            }
            else
            {
                string tmpstr = "Security Mode";
                HostEmail.Text = tmpstr;
                SMTPServer.Text = tmpstr;
                SMTPEnableAuthentication.Checked = Boolean.Parse(HostSetting.GetHostSetting("SMTPAuthentication"));
                SMTPUsername.Text = tmpstr;
                SMTPPort.Text = "25";
                SMTPUseSSL.Checked = false;
                SMTPPassword.Attributes.Add("value", tmpstr);                
                SMTPEmailFormat.SelectedValue = "";
                SMTPEmailTemplateRoot.Text = "~/templates";
            }
        }

        #endregion

        /// <summary>
        /// Tests the email settings.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void TestEmailSettings_Click(object sender, EventArgs e)
        {
            //TODO: Localize these strings
            try
            {
                if (!string.IsNullOrEmpty(HostEmail.Text))
                {
                    SmtpClient smtp = new SmtpClient(SMTPServer.Text, int.Parse(SMTPPort.Text));
                    smtp.EnableSsl = SMTPUseSSL.Checked;

                    if (SMTPEnableAuthentication.Checked)
                        smtp.Credentials = new NetworkCredential(SMTPUsername.Text, SMTPPassword.Text, SMTPDomain.Text);

                    MailMessage message = new MailMessage(HostEmail.Text, HostEmail.Text, string.Format("{0} SMTP Configuration Test", HostSetting.GetHostSetting("ApplicationTitle")), string.Empty);
                    message.IsBodyHtml = false;

                    smtp.Send(message);

                    lblEmail.Text = "<br/> The email was sent successfully!";
                    lblEmail.ForeColor = Color.Green;
                }
                else
                {
                    lblEmail.Text = "<br/> Please enter a host email address";
                    lblEmail.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            {
                lblEmail.Text = string.Format("<br/> {0}.  Please see the error log for more details.", ex.Message);
                lblEmail.ForeColor = Color.Red;
                Log.Error("SMTP Configuration Test Error", ex);
            }

        }

        /// <summary>
        /// Handles the CheckChanged event of the SMTPEnableAuthentication control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SMTPEnableAuthentication_CheckChanged(object sender, EventArgs e)
        {
            ShowSMTPAuthenticationFields();
        }

        /// <summary>
        /// Shows the SMTP authentication fields.
        /// </summary>
        private void ShowSMTPAuthenticationFields()
        {
            if (SMTPEnableAuthentication.Checked)
            {
                trSMTPUsername.Visible = true;
                trSMTPPassword.Visible = true;
                trSMTPDomain.Visible = true;
            }
            else
            {
                trSMTPUsername.Visible = false;
                trSMTPPassword.Visible = false;
                trSMTPDomain.Visible = false;
            }
        }

    }
}
