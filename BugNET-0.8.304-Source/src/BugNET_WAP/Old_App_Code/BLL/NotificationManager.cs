using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Security;
using System.Threading;
using log4net;
using System.Web;
using System.Collections;
using System.Threading;


namespace BugNET.BusinessLogicLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class NotificationManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NotificationManager));
        private List<INotificationType> plugins = null;
        private string _Username;
        private string _UserDisplayName;
        private string _Subject;
        private string _BodyText;
        private EmailFormatTypes _EmailFormatType;

        // What is being currently notified, and what is awaiting notification.  
        private static Queue<NotificationContext> _NotificationQueue = new Queue<NotificationContext>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationManager"/> class.
        /// </summary>
        public NotificationManager()
        {
            _EmailFormatType = EmailFormatTypes.Text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationManager"/> class.
        /// </summary>
        /// <param name="emailFormatType">Type of the email format.</param>
        public NotificationManager(EmailFormatTypes emailFormatType) : this()
        {
            _EmailFormatType = emailFormatType;
        }

        /// <summary>
        /// Loads the notification types from the current assembly.
        /// </summary>
        public List<INotificationType> LoadNotificationTypes()
        {
            plugins = new List<INotificationType>();
            Assembly asm = this.GetType().Assembly;

            foreach (Type t in asm.GetTypes())
            {
                foreach (Type iface in t.GetInterfaces())
                {
                    if (iface.Equals(typeof(INotificationType)))
                    {
                        try
                        {
                            INotificationType notificationType = (INotificationType)Activator.CreateInstance(t);
                            plugins.Add(notificationType);
                            if (Log.IsDebugEnabled) Log.DebugFormat("Type: {0} Enabled: {1}", notificationType.Name, notificationType.Enabled);      
                            break;
                        }
                        catch (Exception ex) 
                        {
                            if (Log.IsErrorEnabled) Log.Error(string.Format(Logging.GetErrorMessageResource("CouldNotLoadNotificationType"), t.FullName),ex);
                        }
                    }
                }
            }
            return plugins;
        }

        /// <summary>
        /// Recurses the loaded notification plugins and if enabled will send the notifications using the plugin.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="bodyText">The body text.</param>
        public void SendNotification(string username, string subject, string bodyText)
        {
            SendNotification(username, subject, bodyText, "");
        }

        /// <summary>
        /// Recurses the loaded notification plugins and if enabled will send the notifications using the plugin.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="bodyText">The body text.</param>
        /// <param name="userDisplayName">The users display name</param>
        public void SendNotification(string username, string subject, string bodyText, string userDisplayName)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException("subject");

            if (string.IsNullOrEmpty(bodyText))
                throw new ArgumentNullException("bodyText");

            _UserDisplayName = userDisplayName;
            _Username = username;
            _Subject = subject;
            _BodyText = bodyText;

            // _NotificationQueue must be protected with Locks
            lock (_NotificationQueue) {

            // En-queue notification into the send queue
            _NotificationQueue.Enqueue(new NotificationContext(_Username, _Subject, _BodyText, _EmailFormatType, _UserDisplayName));

            // If we only have one notification in the queue we will need to start a new thread
            if (_NotificationQueue.Count > 1)
            {

              // Create and Start a New Thread
              (new System.Threading.Thread(() => {

                // Get the First Notification that need sending from the queue
                NotificationContext NotificationToSend = null;
                lock (_NotificationQueue) {
                  NotificationToSend = _NotificationQueue.Dequeue();
                }

                // Whilst we still have queued notifications to send, we stay in this thread sending them
                while (NotificationToSend != null)
                {

                  // In The Thread Send The Notification
                  foreach (INotificationType nt in plugins)
                  {
                    //if plugin is enabled globally though application settings
                    if (nt.Enabled)
                      nt.SendNotification(NotificationToSend);
                  }

                  // Sleep for 10 seconds, enable the line below to test the queuing of notifications is working correctly
                  //Thread.Sleep(10000);

                  // We attempt to get the next Notification that need sending from the queue
                  lock (_NotificationQueue)
                  {

                    // Get a new NotificationToSend if we can, otherwise reset
                    if (_NotificationQueue.Count > 0)
                      NotificationToSend = _NotificationQueue.Dequeue();
                    else
                      NotificationToSend = null;

                  }

                }

              }
              )).Start();

            }

          }

        }

    

        /// <summary>
        /// Loads the notification template.
        /// </summary>
        public string LoadEmailNotificationTemplate(string templateName)
        {
            string templateKey = (_EmailFormatType == EmailFormatTypes.Text) ? "" : "HTML";

            string template = LoadNotificationTemplate(string.Concat(templateName, templateKey));

            return XmlXslTransform.LoadEmailXslTemplate(template);
        }

        /// <summary>
        /// Loads the notification template.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public string LoadNotificationTemplate(string templateName)
        {
            return HttpContext.GetGlobalResourceObject("Notification", templateName) as string;
        }

        /// <summary>
        /// Generates the content of the notification.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public string GenerateNotificationContent(string template, Dictionary<string, object> data)
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            using (System.Xml.XmlWriter xml = new System.Xml.XmlTextWriter(writer))
            {
                xml.WriteStartElement("root");

                foreach (DictionaryEntry de in HostSetting.GetHostSettings())
                    xml.WriteElementString(string.Concat("HostSetting_", de.Key), de.Value.ToString());

                foreach (var item in data.Keys)
                {
                    if (typeof(IToXml).IsAssignableFrom(data[item].GetType()))
                    {
                        IToXml iXml = (IToXml)data[item];
                        xml.WriteRaw(iXml.ToXml());
                    }
                    else if (item.StartsWith("RawXml"))
                    {
                        xml.WriteRaw(data[item].ToString());
                    }
                    else
                    {
                        xml.WriteElementString(item,data[item].ToString());
                    }
                }

                xml.WriteEndElement();

                return XmlXslTransform.Transform(writer.ToString(), template);
            }
        }

        /// <summary>
        /// Determines whether [is notification type enabled] [the specified notification type].
        /// </summary>
        /// <param name="notificationType">Type of the notification.</param>
        /// <returns>
        /// 	<c>true</c> if [is notification type enabled] [the specified notification type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotificationTypeEnabled(string notificationType)
        {
            if (string.IsNullOrEmpty(notificationType))
                throw new ArgumentNullException("notificationType");

            string[] notificationTypes = HostSetting.GetHostSetting("EnabledNotificationTypes").Split(';');
            foreach (string s in notificationTypes)
            {
                if (s.Equals(notificationType))
                    return true;
            }
            return false;
        }
    }
}
