using System;
using System.Collections.Generic;
using System.Text;
using BugNET.UserInterfaceLayer;
using System.Net.Mail;
using System.Net;
using System.Web;
using log4net;

namespace BugNET.BusinessLogicLayer
{
    public enum EmailFormatTypes : int
    {
        Text = 1,
        HTML = 2
    }

    public class EmailNotificationType : INotificationType
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailNotificationType));

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotificationType"/> class.
        /// </summary>
        public EmailNotificationType()
        {
            this.Enabled = NotificationManager.IsNotificationTypeEnabled(this.Name);
        }

        #region INotificationType Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "Email"; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EmailNotificationType"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; internal set; }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <param name="context">The context.</param>
        public void SendNotification(INotificationContext context)
        {
            try
            {
                System.Web.Security.MembershipUser user = ITUser.GetUser(context.Username);

                //check if this user had this notifiction type enabled in their profile.
                if (user != null && ITUser.IsNotificationTypeEnabled(context.Username, this.Name))
                {
                    string From = HostSetting.GetHostSetting("HostEmailAddress");
                    string SMTPServer = HostSetting.GetHostSetting("SMTPServer");
                    int SMTPPort = HostSetting.GetHostSetting("SMTPPort", 25);
                    bool SMTPAuthentictation = HostSetting.GetHostSetting("SMTPAuthentication", false);
                    bool SMTPUseSSL = HostSetting.GetHostSetting("SMTPUseSSL", false);
                    string SMTPDomain = HostSetting.GetHostSetting("SMTPDomain", string.Empty);

                    // Only fetch the password if you need it
                    string SMTPUsername = "";
                    string SMTPPassword = "";

                    if (SMTPAuthentictation)
                    {
                        SMTPUsername = HostSetting.GetHostSetting("SMTPUsername");
                        SMTPPassword = HostSetting.GetHostSetting("SMTPPassword");
                    }

                    SmtpClient smtp = new SmtpClient()
                    {
                        Host = SMTPServer,
                        Port = SMTPPort,
                        EnableSsl = SMTPUseSSL,
                    };

                    if (SMTPAuthentictation)
                        smtp.Credentials = new NetworkCredential(SMTPUsername, SMTPPassword, SMTPDomain);

                    MailMessage message = new MailMessage();
                    message.Body = context.BodyText;
                    message.From = new MailAddress(From);
                    message.Subject = context.Subject.Trim();
                    message.To.Add(new MailAddress(user.Email, context.UserDisplayName));
                    message.IsBodyHtml = (context.EmailFormatType == EmailFormatTypes.HTML);

                    smtp.Send(message);

                    // try to clean up the credentials
                    if (smtp.Credentials != null)
                        smtp.Credentials = null;

                    SMTPPassword = "                 ";
                    SMTPUsername = "                 ";
                }

            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        #endregion

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void ProcessException(Exception ex)
        {

            //set user to log4net context, so we can use %X{user} in the appenders
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                MDC.Set("user", HttpContext.Current.User.Identity.Name);

            if (Log.IsErrorEnabled)
                Log.Error("Email Notification Error", ex);
        }
    }
}
