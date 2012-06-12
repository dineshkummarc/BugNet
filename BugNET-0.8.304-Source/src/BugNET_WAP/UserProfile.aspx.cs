using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BugNET.UserInterfaceLayer;
using BugNET.BusinessLogicLayer;
using log4net;
using System.Collections.Generic;
using System.Globalization;

namespace BugNET
{
    public partial class UserProfile : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserProfile));

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {        
            if (!Page.IsPostBack)
            {
                foreach (ListItem li in BulletedList4.Items)
                    li.Attributes.Add("class", "off");

                BulletedList4.Items[0].Attributes.Add("class", "on");

                List<string> resources = Resources.GetInstalledLanguageResources();
                List<ListItem> resourceItems = new List<ListItem>();
                foreach(string code in resources)
                {
                    CultureInfo cultureInfo = new CultureInfo(code, false );
                    resourceItems.Add(new ListItem(cultureInfo.DisplayName,code));
                }
                ddlPreferredLocale.DataSource = resourceItems;
                ddlPreferredLocale.DataBind();

                MembershipUser objUser = ITUser.GetUser(User.Identity.Name);
                litUserName.Text = User.Identity.Name;
                FirstName.Text = WebProfile.Current.FirstName;
                LastName.Text = WebProfile.Current.LastName;
                FullName.Text = WebProfile.Current.DisplayName;              
                ddlPreferredLocale.SelectedValue = WebProfile.Current.PreferredLocale;
                IssueListItems.SelectedValue = WebProfile.Current.IssuesPageSize.ToString();
                if(objUser !=null)
                {
                    UserName.Text =objUser.UserName;
                    Email.Text = objUser.Email;
                }

                objUser = null;
            }

        }

       

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AddProjectNotification(Object s, EventArgs e)
        {
            //The users must be added to a list first because the collection can not
            //be modified while we iterate through it.
            var usersToAdd = new List<ListItem>();

            foreach (ListItem item in lstAllProjects.Items)
                if (item.Selected)
                    usersToAdd.Add(item);


            foreach (var item in usersToAdd)
            {
                ProjectNotification pn = new ProjectNotification(Convert.ToInt32(item.Value),Security.GetUserName());
                if (pn.Save())
                {
                    lstSelectedProjects.Items.Add(item);
                    lstAllProjects.Items.Remove(item);
                }
            }

            lstSelectedProjects.SelectedIndex = -1;
        }

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void RemoveProjectNotification(Object s, EventArgs e)
        {
            //The users must be added to a list first because the collection can not
            //be modified while we iterate through it.
            var usersToRemove = new List<ListItem>();

            foreach (ListItem item in lstSelectedProjects.Items)
                if (item.Selected)
                    usersToRemove.Add(item);


            foreach (var item in usersToRemove)
            {
                if (ProjectNotification.DeleteProjectNotification(Convert.ToInt32(item.Value),Security.GetUserName()))
                {
                    lstAllProjects.Items.Add(item);
                    lstSelectedProjects.Items.Remove(item);
                }
            }

            lstAllProjects.SelectedIndex = -1;
        }


        /// <summary>
        /// Handles the Click event of the SavePasswordSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SavePasswordSettings_Click(object sender, EventArgs e)
        {
          
            MembershipUser CurrentUser = Membership.GetUser();
            String currentPassword = CurrentPassword.Text;
            String newPassword = NewPassword.Text;
            String securityQuestion = SecurityQuestion.Text;
            String securityAnswer = SecurityAnswer.Text;

            //Set the password
            bool PasswordChanged = CurrentUser.ChangePassword(currentPassword, newPassword);
            bool SecurityQuestionChanged = false;
            if (PasswordChanged == true)
            {
                Message2.ShowSuccessMessage(GetLocalResourceObject("PasswordChanged").ToString());

                if (Log.IsInfoEnabled)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                    Log.Info("Password changed");
                }

                SecurityQuestionChanged = CurrentUser.ChangePasswordQuestionAndAnswer(newPassword, securityQuestion, securityAnswer);

            }
            else
            {
                if (Log.IsErrorEnabled)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                    Log.Error("Password update failure");
                }

                Message2.ShowErrorMessage(GetLocalResourceObject("PasswordChangeError").ToString());
            }
            if (SecurityQuestionChanged == true)
            {
                Message2.ShowSuccessMessage("Security question was changed successfully");
            }
            else
            {
                //if (Log.IsErrorEnabled)
                //{
                //    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                //        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                //    Log.Error("Password update failure");
                //}
                //Message2.ShowErrorMessage(GetLocalResourceObject("PasswordChangeError").ToString());
            }
            //else { lblCurrentPassword.Visible = true; }
        }

        protected void BulletedList4_Click1(object sender, BulletedListEventArgs e)
        {

            //Label1.Text = "The Index of Item you clicked: " + e.Index + "<br> The value of Item you clicked: " + BulletedList4.Items[e.Index].Text;
            foreach(ListItem li in BulletedList4.Items)
                li.Attributes.Add("class", "off");

            BulletedList4.Items[e.Index].Attributes.Add("class", "on");
            
            ProfileView.ActiveViewIndex = e.Index;

            switch (ProfileView.ActiveViewIndex)
            {
                case 3:
                    lstAllProjects.Items.Clear();
                    lstSelectedProjects.Items.Clear();
                    lstAllProjects.DataSource = Project.GetProjectsByMemberUserName(Security.GetUserName());
                    lstAllProjects.DataTextField = "Name";
                    lstAllProjects.DataValueField = "Id";
                    lstAllProjects.DataBind();

                    // Copy selected users into Selected Users List Box
                    List<ProjectNotification> projectNotifications = ProjectNotification.GetProjectNotificationsByUsername(Security.GetUserName());
                    foreach (ProjectNotification currentNotification in projectNotifications)
                    {
                        ListItem matchItem = lstAllProjects.Items.FindByValue(currentNotification.ProjectId.ToString());
                        if (matchItem != null)
                        {
                            lstSelectedProjects.Items.Add(matchItem);
                            lstAllProjects.Items.Remove(matchItem);
                        }
                    }

                    //ProjectNotification.GetProjectNotificationsByUsername(Security.GetUserName());
                    NotificationManager nm = new NotificationManager();
                    CheckBoxList1.DataSource = nm.LoadNotificationTypes().FindAll(delegate(INotificationType t) { return t.Enabled == true; });
                    CheckBoxList1.DataTextField = "Name";
                    CheckBoxList1.DataValueField = "Name";
                    CheckBoxList1.DataBind();
                    string[] notificationTypes = WebProfile.Current.NotificationTypes.Split(';');
                    foreach (string s in notificationTypes)
                    {
                        ListItem currentCheckBox = CheckBoxList1.Items.FindByText(s);
                        if (currentCheckBox != null)
                            currentCheckBox.Selected = true;

                    }
                    break;
            }

        }


        /// <summary>
        /// Handles the Click event of the SaveNotificationsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SaveNotificationsButton_Click(object sender, EventArgs e)
        {
            try
            {
                string notificationTypes = string.Empty;

                foreach (ListItem li in CheckBoxList1.Items)
                {
                    if (li.Selected)
                        notificationTypes += li.Value + ";";
                        
                }
                notificationTypes = notificationTypes.TrimEnd(';');
                WebProfile.Current.NotificationTypes = notificationTypes;
                WebProfile.Current.Save();
                Message4.ShowSuccessMessage(GetLocalResourceObject("ProfileSaved").ToString());
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                    Log.Error("Profile update error", ex);
                }

                Message4.ShowErrorMessage(GetLocalResourceObject("ProfileUpdateError").ToString());
            }
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SaveButton_Click(object s, EventArgs e)
        {
            MembershipUser objUser = ITUser.GetUser(User.Identity.Name);

            objUser.Email = Email.Text;
            WebProfile.Current.FirstName = FirstName.Text;
            WebProfile.Current.LastName = LastName.Text;
            WebProfile.Current.DisplayName = FullName.Text;

            try
            {
                ITUser.UpdateUser(objUser);
                WebProfile.Current.Save();
                Message1.ShowSuccessMessage(GetLocalResourceObject("ProfileSaved").ToString());

                if (Log.IsInfoEnabled)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                    Log.Info("Profile updated");
                }
            }
            catch(Exception ex)
            {
                if (Log.IsErrorEnabled)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                    Log.Error("Profile update error", ex);
                }

                Message1.ShowErrorMessage(GetLocalResourceObject("ProfileUpdateError").ToString());
            }
            

        }

        /// <summary>
        /// Handles the Click event of the BackButton control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BackButton_Click(object s, EventArgs e)
        {
            String url = Request.QueryString["referrerurl"];

            if (!string.IsNullOrEmpty(url))
                Response.Redirect(url);
        }


        /// <summary>
        /// Handles the Click event of the SaveCustomizeSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SaveCustomSettings_Click(object sender, EventArgs e)
        { 
            WebProfile.Current.IssuesPageSize = Convert.ToInt32(IssueListItems.SelectedValue);
            WebProfile.Current.PreferredLocale = ddlPreferredLocale.SelectedValue;

            try
            {
                WebProfile.Current.Save();
                Message3.ShowSuccessMessage("Your custom settings have been updated successfully.");

                if (Log.IsInfoEnabled)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                    Log.Info("Profile updated");
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                        MDC.Set("user", HttpContext.Current.User.Identity.Name);
                    Log.Error("Profile update error", ex);
                }
                Message3.ShowErrorMessage("Your custom settings could not be updated.");

            }

        }
    }
}
