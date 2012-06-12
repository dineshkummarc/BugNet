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
using System.IO;
using BugNET.BusinessLogicLayer;

namespace BugNET.Administration.Host.UserControls
{
    public partial class SubversionSettings : System.Web.UI.UserControl, IEditHostSettingControl
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
                    // Validate the subversion integration fields
                    if (EnableRepoCreation.Checked)
                    {
                        RepoRootPath.Text = RepoRootPath.Text.Trim();
                        RepoBackupPath.Text = RepoBackupPath.Text.Trim();

                        if (!RepoRootPath.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
                            RepoRootPath.Text += Path.DirectorySeparatorChar;

                        if (RepoRootPath.Text.Length == 0 || !Directory.Exists(RepoRootPath.Text))
                        {
                            Message1.Text = "The repository root directory does not exist. To enable subversion administration a valid local folder must exist to hold the repositories.";
                            Message1.IconType = BugNET.UserControls.Message.MessageType.Error;
                            Message1.Visible = true;
                            return;
                        }

                        //Make sure the backup path is valid if it was given
                        if (RepoBackupPath.Text.Length > 0)
                        {
                            if (!RepoBackupPath.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
                                RepoBackupPath.Text += Path.DirectorySeparatorChar;

                            if (!Directory.Exists(RepoBackupPath.Text))
                            {
                                Message1.Text = "The repository backup directory does not exist. To disable backup capabilities leave the field empty.";
                                Message1.IconType = BugNET.UserControls.Message.MessageType.Error;
                                Message1.Visible = true;
                                return;
                            }
                            else
                            {
                                if (string.Compare(RepoBackupPath.Text, RepoRootPath.Text, true) == 0)
                                {
                                    Message1.Text = "The repository root and backup path can not be the same direcory.";
                                    Message1.IconType = BugNET.UserControls.Message.MessageType.Error;
                                    Message1.Visible = true;
                                    return;
                                }
                            }

                        }
                    } //End subversion integration validation


                    HostSetting.UpdateHostSetting("EnableRepositoryCreation", EnableRepoCreation.Checked.ToString());
                    HostSetting.UpdateHostSetting("RepositoryRootPath", RepoRootPath.Text);
                    HostSetting.UpdateHostSetting("RepositoryRootUrl", RepoRootUrl.Text);
                    HostSetting.UpdateHostSetting("RepositoryBackupPath", RepoBackupPath.Text);
                    HostSetting.UpdateHostSetting("SvnAdminEmailAddress", SvnAdminEmailAddress.Text);
                    HostSetting.UpdateHostSetting("SvnHookPath", SvnHookPath.Text);
                }
            }
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        public void Initialize()
        {
            bool enableRepoCreation = false;
            Boolean.TryParse(HostSetting.GetHostSetting("EnableRepositoryCreation"), out enableRepoCreation);

            EnableRepoCreation.Checked = enableRepoCreation;

            if (!SecurityModes.isHighSecurityMode())
            {
                RepoRootPath.Text = HostSetting.GetHostSetting("RepositoryRootPath");
                RepoRootUrl.Text = HostSetting.GetHostSetting("RepositoryRootUrl");
                RepoBackupPath.Text = HostSetting.GetHostSetting("RepositoryBackupPath");
                SvnAdminEmailAddress.Text = HostSetting.GetHostSetting("SvnAdminEmailAddress");
                SvnHookPath.Text = HostSetting.GetHostSetting("SvnHookPath");
            }
            else
            {
                string tmpstr = "Security Mode";
                RepoRootPath.Text = tmpstr;
                RepoRootUrl.Text = "http://" + tmpstr;
                RepoBackupPath.Text = tmpstr;
                SvnAdminEmailAddress.Text = tmpstr;
                SvnHookPath.Text = tmpstr;
            }
        }

        #endregion
    }
}