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
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;

namespace BugNET.Administration.Host.UserControls
{
    public partial class NotificationSettings : System.Web.UI.UserControl, IEditHostSettingControl
    {
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
                string notificationTypes = string.Empty;

                foreach (ListItem li in cblNotificationTypes.Items)
                {
                    if (li.Selected)
                        notificationTypes += li.Value + ";";

                }
                notificationTypes = notificationTypes.TrimEnd(';');
                
                HostSetting.UpdateHostSetting("EnabledNotificationTypes",notificationTypes);
                HostSetting.UpdateHostSetting("AdminNotificationUsername", AdminNotificationUser.SelectedValue);
            }
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Initialize()
        {
            NotificationManager nm = new NotificationManager();
            cblNotificationTypes.DataSource = nm.LoadNotificationTypes();
            cblNotificationTypes.DataTextField = "Name";
            cblNotificationTypes.DataValueField = "Name";
            cblNotificationTypes.DataBind();

            string[] notificationTypes = HostSetting.GetHostSetting("EnabledNotificationTypes").Split(';');
            foreach (string s in notificationTypes)
            {
                ListItem currentCheckBox = cblNotificationTypes.Items.FindByText(s);
                if (currentCheckBox != null)
                    currentCheckBox.Selected = true;
            }

            AdminNotificationUser.DataSource = ITUser.GetAllUsers();
            AdminNotificationUser.DataTextField = "DisplayName";
            AdminNotificationUser.DataValueField = "UserName";
            AdminNotificationUser.DataBind();
            AdminNotificationUser.SelectedValue = HostSetting.GetHostSetting("AdminNotificationUsername");
        }

        #endregion
    }
}