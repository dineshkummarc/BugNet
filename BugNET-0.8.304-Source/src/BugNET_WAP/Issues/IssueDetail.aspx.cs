using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BugNET.BusinessLogicLayer;
using BugNET.UserInterfaceLayer;
using BugNET.UserControls;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace BugNET.Issues
{
    /// <summary>
    /// Issue Detail Page
    /// </summary>
    public partial class IssueDetail : BugNET.UserInterfaceLayer.BasePage
    {

        #region Private Events
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                lnkDelete.Attributes.Add("onclick", string.Format("return confirm('{0}');", GetLocalResourceObject("DeleteIssue").ToString()));
                imgDelete.Attributes.Add("onclick", string.Format("return confirm('{0}');", GetLocalResourceObject("DeleteIssue").ToString()));

                // Get Issue Id from Query String
                if (Request.QueryString["id"] != null)
                { 
                    try
                    {
                        IssueId = Int32.Parse(Request.QueryString["id"]);
                    }
                    catch
                    {
                        ErrorRedirector.TransferToNotFoundPage(Page);
                    }
                }

                // Get Project Id from Query String
                if (Request.QueryString["pid"] != null)
                { 
                    try
                    {
                        ProjectId = Int32.Parse(Request.QueryString["pid"]);
                    }
                    catch
                    {
                        ErrorRedirector.TransferToNotFoundPage(Page);
                    }
                }

                // If don't know project or issue then redirect to something missing page
                if (ProjectId == 0 && IssueId == 0)
                    ErrorRedirector.TransferToSomethingMissingPage(Page);

                // Initialize for Adding or Editing
                if (IssueId == 0)
                {
                    BindOptions();
                    pnlAddAttachment.Visible = true;
                    TitleTextBox.Visible = true;
                    DescriptionHtmlEditor.Visible = true;
                    Description.Visible = false;
                    TitleLabel.Visible = false;
                    DisplayTitleLabel.Visible = false;
                    string test = GetLocalResourceObject("PageTitleNewIssue").ToString();
                    Page.Title = test;
                    lblIssueNumber.Text = "N/A";
                    VoteButton.Visible = false;
                }
                else
                {
                    BindValues();
                }

                //set the referrer url
                if (Request.UrlReferrer != null)
                {
                    if (Request.UrlReferrer.ToString() != Request.Url.ToString())
                        Session["ReferrerUrl"] = Request.UrlReferrer.ToString();
                }
                else
                {
                    Session["ReferrerUrl"] = string.Format("~/Issues/IssueList.aspx?pid={0}", ProjectId);
                }

            }

            //need to rebind these on every postback because of dynamic controls
            if (IssueId == 0)
            {
                ctlCustomFields.DataSource = CustomField.GetCustomFieldsByProjectId(ProjectId);
            }
            else
            {
                ctlCustomFields.DataSource = CustomField.GetCustomFieldsByIssueId(IssueId);
            }
            ctlCustomFields.DataBind();

            // The ExpandIssuePaths method is called to handle
            // the SiteMapResolve event.
            SiteMap.SiteMapResolve += new SiteMapResolveEventHandler(this.ExpandIssuePaths);

            ctlIssueTabs.IssueId = IssueId;
            ctlIssueTabs.ProjectId = ProjectId;

        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Project p = Project.GetProjectById(ProjectId);

            if (p == null)
                ErrorRedirector.TransferToNotFoundPage(Page);

            //check if the user can access this project
            if (p.AccessType == Globals.ProjectAccessType.Private && !User.Identity.IsAuthenticated)
            {
                ErrorRedirector.TransferToLoginPage(Page);
            }
            else if (User.Identity.IsAuthenticated && p.AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(User.Identity.Name, ProjectId))
            {
                ErrorRedirector.TransferToLoginPage(Page);
            }
            else if (IssueId == 0)
            {
                //security check: add issue
                if (!ITUser.HasPermission(ProjectId, Globals.Permission.ADD_ISSUE.ToString()))
                    ErrorRedirector.TransferToLoginPage(Page);

                //check users role permission for adding an attachment
                if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.ADD_ATTACHMENT.ToString()))
                    pnlAddAttachment.Visible = false;
            }
            //security check: assign issue
            if (!ITUser.HasPermission(ProjectId, Globals.Permission.ASSIGN_ISSUE.ToString()))
                DropAssignedTo.Enabled = false;

            if (!p.AllowIssueVoting)
                VoteBox.Visible = false;

            if (IssueId != 0)
            {
                //private issue check
                Issue issue = Issue.GetIssueById(IssueId);
                if (issue.Visibility == (int)Globals.IssueVisibility.Private && issue.AssignedDisplayName != Security.GetUserName() && issue.CreatorDisplayName != Security.GetUserName() && (!ITUser.IsInRole(Globals.SuperUserRole) || !ITUser.IsInRole(Globals.ProjectAdminRole)))
                    ErrorRedirector.TransferToLoginPage(Page);

                Page.Title = string.Concat(issue.FullId, ": ", issue.Title);
                lblIssueNumber.Text = string.Format("{0}-{1}", p.Code, IssueId);
                ctlIssueTabs.Visible = true;
                TimeLogged.Visible = true;
                TimeLoggedLabel.Visible = true;
                chkNotifyAssignedTo.Visible = false;
                chkNotifyOwner.Visible = false;
                SetFieldSecurity();
            }

            ctlCustomFields.DataBind();
        }

        /// <summary>
        /// Handles the Unload event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Unload(object sender, System.EventArgs e)
        {
            //remove the event handler
            SiteMap.SiteMapResolve -=
             new SiteMapResolveEventHandler(this.ExpandIssuePaths);
        }

        /// <summary>
        /// Expands the issue paths.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.SiteMapResolveEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private SiteMapNode ExpandIssuePaths(Object sender, SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            // The current node, and its parents, can be modified to include
            // dynamic query string information relevant to the currently
            // executing request.
            if (IssueId != 0)
            {
                string title = (TitleTextBox.Text.Length >= 30) ? TitleTextBox.Text.Substring(0, 30) + " ..." : TitleTextBox.Text;
                tempNode.Title = string.Format("{0}: {1}", lblIssueNumber.Text, title);
                tempNode.Url = tempNode.Url + "?id=" + IssueId.ToString();
            }
            else
                tempNode.Title = "New Issue";

            if ((null != (tempNode = tempNode.ParentNode)))
            {
                tempNode.Url = string.Format("~/Issues/IssueList.aspx?pid={0}", ProjectId);
            }

            return currentNode;
        }

        /// <summary>
        /// Binds the values.
        /// </summary>
        private void BindValues()
        {
            Issue currentIssue = Issue.GetIssueById(IssueId);

            if (currentIssue == null)
            {
                // BGN-1379
                ErrorRedirector.TransferToNotFoundPage(Page);
            }
            else
            {

                ProjectId = currentIssue.ProjectId;

                BindOptions();

                lblIssueNumber.Text = string.Concat(currentIssue.FullId, ":");
                TitleLabel.Text = Server.HtmlDecode(currentIssue.Title);
                //Page.Title = string.Concat(currentIssue.FullId, ": ", currentIssue.Title);
                DropIssueType.SelectedValue = currentIssue.IssueTypeId;
                DropResolution.SelectedValue = currentIssue.ResolutionId;
                DropStatus.SelectedValue = currentIssue.StatusId;
                DropPriority.SelectedValue = currentIssue.PriorityId;
                DropOwned.SelectedValue = currentIssue.OwnerUserName;

                // SMOSS: XSS Bugfix
                Description.Text = (currentIssue.Description);

                // WARNING: Do Not html decode the text for the editor. 
                // The editor is expecting HtmlEncoded text as input.
                DescriptionHtmlEditor.Text = currentIssue.Description;
                lblLastUpdateUser.Text = currentIssue.LastUpdateDisplayName;
                lblReporter.Text = currentIssue.CreatorDisplayName;

                // XSS Bugfix
                // The text box is expecting raw html
                TitleTextBox.Text = Server.HtmlDecode(currentIssue.Title);
                DisplayTitleLabel.Text = currentIssue.Title;

                Label5.Text = currentIssue.IssueTypeName;
                lblDateCreated.Text = currentIssue.DateCreated.ToString("g");
                lblLastModified.Text = currentIssue.LastUpdate.ToString("g");
                lblIssueNumber.Text = currentIssue.FullId;
                DropCategory.SelectedValue = currentIssue.CategoryId;
                DropMilestone.SelectedValue = currentIssue.MilestoneId;
                DropAffectedMilestone.SelectedValue = currentIssue.AffectedMilestoneId;
                DropAssignedTo.SelectedValue = currentIssue.AssignedUserName;
                lblLoggedTime.Text = currentIssue.TimeLogged.ToString();
                txtEstimation.Text = currentIssue.Estimation == 0 ? string.Empty : currentIssue.Estimation.ToString();
                DueDate.Text = currentIssue.DueDate == DateTime.MinValue ? String.Empty : currentIssue.DueDate.ToShortDateString();
                chkPrivate.Checked = currentIssue.Visibility == 0 ? false : true;
                ProgressSlider.Text = currentIssue.Progress.ToString();
                IssueVoteCount.Text = currentIssue.Votes.ToString();

                if (currentIssue.Votes > 1)
                    Votes.Text = GetLocalResourceObject("Votes").ToString();

                if (User.Identity.IsAuthenticated && IssueVote.HasUserVoted(currentIssue.Id, Security.GetUserName()))
                {
                    VoteButton.Visible = false;
                    VotedLabel.Visible = true;
                    VotedLabel.Text = GetLocalResourceObject("Voted").ToString();
                }
                else
                {
                    VoteButton.Visible = true;
                    VotedLabel.Visible = false;
                }

                if (Status.GetStatusById(currentIssue.StatusId).IsClosedState)
                {
                    VoteButton.Visible = false;
                    VotedLabel.Visible = false;
                }

            }


        }

        /// <summary>
        /// Binds the options.
        /// </summary>
        private void BindOptions()
        {
            List<ITUser> users = ITUser.GetUsersByProjectId(ProjectId);
            //Get Type
            DropIssueType.DataSource = IssueType.GetIssueTypesByProjectId(ProjectId);
            DropIssueType.DataBind();

            //Get Priority
            DropPriority.DataSource = Priority.GetPrioritiesByProjectId(ProjectId);
            DropPriority.DataBind();

            //Get Resolutions
            DropResolution.DataSource = Resolution.GetResolutionsByProjectId(ProjectId);
            DropResolution.DataBind();

            //Get categories
            CategoryTree categories = new CategoryTree();
            DropCategory.DataSource = categories.GetCategoryTreeByProjectId(ProjectId);
            DropCategory.DataBind();

            //Get milestones
            if (IssueId == 0)
            {
                DropMilestone.DataSource = Milestone.GetMilestoneByProjectId(ProjectId, false);
            }
            else
            {
                DropMilestone.DataSource = Milestone.GetMilestoneByProjectId(ProjectId);
            }
            DropMilestone.DataBind();

            DropAffectedMilestone.DataSource = Milestone.GetMilestoneByProjectId(ProjectId);
            DropAffectedMilestone.DataBind();

            //Get Users
            DropAssignedTo.DataSource = users;
            DropAssignedTo.DataBind();

            DropOwned.DataSource = users;
            DropOwned.DataBind();
            DropOwned.SelectedValue = User.Identity.Name;

            DropStatus.DataSource = Status.GetStatusByProjectId(ProjectId);
            DropStatus.DataBind();

            lblDateCreated.Text = DateTime.Now.ToString("f");
            lblLastModified.Text = DateTime.Now.ToString("f");

            if (User.Identity.IsAuthenticated)
            {
                lblReporter.Text = Security.GetDisplayName();
                lblLastUpdateUser.Text = Security.GetDisplayName();
            }

        }

        /// <summary>
        /// Saves the bug.
        /// </summary>
        /// <returns></returns>
        private bool SaveIssue()
        {
            decimal estimation;
            decimal.TryParse(txtEstimation.Text.Trim(), out estimation);
            DateTime dueDate = DueDate.Text.Length > 0 ? DateTime.Parse(DueDate.Text) : DateTime.MinValue;

            bool NewIssue = (IssueId <= 0);

            // WARNING: DO NOT ENCODE THE HTMLEDITOR TEXT. 
            // It expects raw input. So pass through a raw string. 
            // This is a potential XSS vector as the Issue Class should
            // handle sanitizing the input and checking that its input is HtmlEncoded
            // (ie no < or > characters), not the IssueDetail.aspx.cs

            Issue newIssue = new Issue(IssueId, ProjectId, string.Empty, string.Empty, Server.HtmlEncode(TitleTextBox.Text), DescriptionHtmlEditor.Text.Trim(),
                DropCategory.SelectedValue, DropCategory.SelectedText, DropPriority.SelectedValue, DropPriority.SelectedText,
                string.Empty, DropStatus.SelectedValue, DropStatus.SelectedText, string.Empty, DropIssueType.SelectedValue,
                DropIssueType.SelectedText, string.Empty, DropResolution.SelectedValue, DropResolution.SelectedText, string.Empty,
                DropAssignedTo.SelectedText, DropAssignedTo.SelectedValue, Guid.Empty, Security.GetDisplayName(),
                Security.GetUserName(), Guid.Empty, DropOwned.SelectedText, DropOwned.SelectedValue, Guid.Empty, dueDate,
                DropMilestone.SelectedValue, DropMilestone.SelectedText, string.Empty, null, DropAffectedMilestone.SelectedValue, DropAffectedMilestone.SelectedText,
                string.Empty, chkPrivate.Checked == true ? 1 : 0,
                0, estimation, DateTime.MinValue, DateTime.MinValue, Security.GetUserName(), Security.GetDisplayName(),
                Convert.ToInt32(ProgressSlider.Text), false, 0);


            if (!newIssue.Save())
            {
                Message1.ShowErrorMessage("Could not save issue");
                return false;
            }

            IssueId = newIssue.Id;

            if (!CustomField.SaveCustomFieldValues(IssueId, ctlCustomFields.Values))
            {
                Message1.ShowErrorMessage("Could not save issue custom fields");
                return false;
            }


            //if new issue check if notify owner and assigned is checked.
            if (NewIssue)
            {
                //add attachment if present.
                // get the current file
                HttpPostedFile uploadFile = this.AspUploadFile.PostedFile;
                HttpContext context = HttpContext.Current;
                if (uploadFile.ContentLength > 0)
                {
                    IssueAttachment.UploadFile(IssueId, uploadFile, context, AttachmentDescription.Text.Trim());
                }

                //create a vote for the new issue
                IssueVote vote = new IssueVote(IssueId, Security.GetUserName());
                if (!vote.Save())
                    Message1.ShowErrorMessage("Could not save issue vote.");

                if (chkNotifyOwner.Checked)
                {
                    System.Web.Security.MembershipUser oUser = ITUser.GetUser(newIssue.OwnerUserName);
                    if (oUser != null)
                    {
                        IssueNotification notify = new IssueNotification(IssueId, oUser.UserName);
                        notify.Save();
                    }
                }
                if (chkNotifyAssignedTo.Checked && !string.IsNullOrEmpty(newIssue.AssignedUserName))
                {
                    System.Web.Security.MembershipUser oUser = ITUser.GetUser(newIssue.AssignedUserName);
                    if (oUser != null)
                    {
                        IssueNotification notify = new IssueNotification(IssueId, oUser.UserName);
                        notify.Save();
                    }
                }
                IssueNotification.SendIssueAddNotifications(IssueId);
            }


            return true;
        }
        /// <summary>
        /// Handles the Click event of the lnkUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveIssue();
                Response.Redirect(string.Format("~/Issues/IssueDetail.aspx?id={0}", this.IssueId));
            }
        }

        /// <summary>
        /// Handles the Click event of the VoteButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void VoteButton_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect(string.Format("~/Login.aspx?ReturnUrl={0}", Server.UrlEncode(Request.RawUrl)));

            IssueVote vote = new IssueVote(this.IssueId, Security.GetUserName());
            vote.Save();
            int count = Convert.ToInt32(IssueVoteCount.Text) + 1;
            Votes.Text = GetLocalResourceObject("Votes").ToString();
            IssueVoteCount.Text = count.ToString();
            VoteButton.Visible = false;
            VotedLabel.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkDone control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkDone_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && SaveIssue())
                ReturnToPreviousPage();
        }

        /// <summary>
        /// Handles the Click event of the lnkDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            Issue.DeleteIssue(IssueId);
            ReturnToPreviousPage();
        }

        /// <summary>
        /// Cancels the button click.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void CancelButtonClick(Object s, EventArgs e)
        {
            ReturnToPreviousPage();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Handles the Click event of the EditSummary control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.</param>
        protected void EditTitle_Click(object sender, ImageClickEventArgs e)
        {
            EditTitle.Visible = false;
            TitleTextBox.Visible = !TitleTextBox.Visible;
            DisplayTitleLabel.Visible = !DisplayTitleLabel.Visible;
        }

        /// <summary>
        /// Allows editing of the issue description
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EditDescription_Click(object sender, ImageClickEventArgs e)
        {
            EditDescription.Visible = false;
            DescriptionHtmlEditor.Visible = !DescriptionHtmlEditor.Visible;
            Description.Visible = !Description.Visible;
        }

        /// <summary>
        /// Sets security according to permissions
        /// </summary>
        private void SetFieldSecurity()
        {
            //check permission objects
            if (User.Identity.IsAuthenticated)
            {
                //enable editing of description if user has permission or in admin role
                if ((ITUser.IsInRole(ProjectId, Globals.ProjectAdminRole) || ITUser.HasPermission(ProjectId, Globals.Permission.EDIT_ISSUE_DESCRIPTION.ToString())) && !DescriptionHtmlEditor.Visible)
                    EditDescription.Visible = true;

                if ((ITUser.IsInRole(ProjectId, Globals.ProjectAdminRole) || ITUser.HasPermission(ProjectId, Globals.Permission.EDIT_ISSUE_TITLE.ToString())) && !TitleTextBox.Visible)
                    EditTitle.Visible = true;

                //edit issue permission check
                if (!ITUser.HasPermission(ProjectId, Globals.Permission.EDIT_ISSUE.ToString()))
                    LockFields();

                //assign issue permission check
                if (!ITUser.HasPermission(ProjectId, Globals.Permission.ASSIGN_ISSUE.ToString()))
                    DropAssignedTo.Enabled = false;

                //delete issue
                if (ITUser.HasPermission(ProjectId, Globals.Permission.DELETE_ISSUE.ToString()))
                    DeleteButton.Visible = true;

                if (!ITUser.HasPermission(ProjectId, Globals.Permission.CHANGE_ISSUE_STATUS.ToString()))
                    DropStatus.Enabled = false;

                //remove closed status' if user does not have access
                if (!ITUser.HasPermission(ProjectId, Globals.Permission.CLOSE_ISSUE.ToString()))
                {
                    List<Status> status = Status.GetStatusByProjectId(ProjectId).FindAll(st => st.IsClosedState == true);
                    DropDownList stat = (DropDownList)DropStatus.FindControl("dropStatus");
                    foreach (Status s in status)
                        stat.Items.Remove(stat.Items.FindByValue(s.Id.ToString()));
                }

                //if status is closed, check if user is allowed to reopen issue
                //if (editBug.StatusId.CompareTo((int)Globals.StatusType.Closed) == 0)
                //{
                //    LockFields();
                //    pnlClosedMessage.Visible = true;

                //    if (UserIT.HasPermission(ProjectId, Globals.Permissions.REOPEN_ISSUE.ToString()))
                //        lnkReopen.Visible = true;
                //}
            }
            else
            {
                LockFields();
            }
        }

        /// <summary>
        /// Makes all editable fields on the form disabled
        /// </summary>
        private void LockFields()
        {
            lnkDone.Visible = false;
            imgDone.Visible = false;
            lnkSave.Visible = false;
            imgSave.Visible = false;
            imgDelete.Visible = false;
            lnkDelete.Visible = false;
            DescriptionHtmlEditor.Visible = false;
            Description.Visible = true;
            TitleTextBox.Visible = false;
            DisplayTitleLabel.Visible = true;
            DropOwned.Enabled = false;
            DropCategory.Enabled = false;
            DropStatus.Enabled = false;
            DropMilestone.Enabled = false;
            DropIssueType.Enabled = false;
            DropPriority.Enabled = false;
            DropResolution.Enabled = false;
            DropAssignedTo.Enabled = false;
            DropAffectedMilestone.Enabled = false;
            DueDate.Enabled = false;
            chkPrivate.Enabled = false;
            DueDate.Enabled = false;
            DueDate.CssClass = string.Empty;
            ctlCustomFields.IsLocked = true;
            txtEstimation.Enabled = false;
            SliderExtender2.Enabled = false;
            ProgressSlider.Visible = false;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the issue id.
        /// </summary>
        /// <value>The issue id.</value>
        int IssueId
        {
            get
            {
                if (ViewState["IssueId"] == null)
                    return 0;
                else
                    return (int)ViewState["IssueId"];
            }
            set { ViewState["IssueId"] = value; }
        }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>The project id.</value>
        public override int ProjectId
        {
            get
            {
                if (ViewState["ProjectId"] == null)
                    return 0;
                else
                    return (int)ViewState["ProjectId"];
            }
            set { ViewState["ProjectId"] = value; }
        }

        #endregion
    }
}
