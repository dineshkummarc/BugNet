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

namespace BugNET.Issues.UserControls
{
    public partial class IssueTabs : System.Web.UI.UserControl
    {

        private int _ProjectId = 0;
        private int _IssueId = 0;
        private Control contentControl;

        /// <summary>
        /// Gets or sets the bug id.
        /// </summary>
        /// <value>The bug id.</value>
        public int IssueId
        {
            get { return _IssueId; }
            set { _IssueId = value; }
        }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        /// <value>The project id.</value>
        public int ProjectId
        {
            get { return _ProjectId; }
            set { _ProjectId = value; }
        }


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ArrayList colTabs = new ArrayList();
                colTabs.Add("Comments");
                colTabs.Add("Attachments");
                colTabs.Add("History");
                colTabs.Add("Notifications");
                colTabs.Add("Sub Issues");
                colTabs.Add("Parent Issues");
                colTabs.Add("Related Issues");
                colTabs.Add("Revisions");
                colTabs.Add("Time Tracking");
                lstTabs.DataSource = colTabs;
                lstTabs.SelectedIndex = 0;
                lstTabs.DataBind();
            }
           
            LoadTab();
        }


        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Page_PreRender(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
                ((IIssueTab)contentControl).Initialize();
      
        }



        /// <summary>
        /// Handles the ItemDataBound event of the lstTabs control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataListItemEventArgs"/> instance containing the event data.</param>
        protected void lstTabs_ItemDataBound(Object s, DataListItemEventArgs e)
        {
            LinkButton lnkTab = (LinkButton)e.Item.FindControl("lnkTab");
            Image Icon = (Image)e.Item.FindControl("TabIcon");
            string imageUrl = GetTabIconUrl((string)e.Item.DataItem);
            Icon.ImageUrl = imageUrl;
            if (string.IsNullOrEmpty(imageUrl))
                Icon.Visible = false;
            lnkTab.Text = GetTabName((string)e.Item.DataItem);
        }

        /// <summary>
        /// Handles the ItemCommand event of the lstTabs control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataListCommandEventArgs"/> instance containing the event data.</param>
        protected void lstTabs_ItemCommand(Object s, DataListCommandEventArgs e)
        {
            lstTabs.SelectedIndex = e.Item.ItemIndex;
            LoadTab();
            ((IIssueTab)contentControl).Initialize();
        }


        /// <summary>
        /// Loads the tab.
        /// </summary>
        void LoadTab()
        {
            string controlName = "Comments.ascx";

            switch (lstTabs.SelectedIndex)
            {
                case 0:
                    controlName = "Comments.ascx";
                    break;
                case 1:
                    controlName = "Attachments.ascx";
                    break;
                case 2:
                    controlName = "History.ascx";
                    break;
                case 3:
                    controlName = "Notifications.ascx";
                    break;
                case 4:
                    controlName = "SubIssues.ascx";
                    break;
                case 5:
                    controlName = "ParentIssues.ascx";
                    break;
                case 6:
                    controlName = "RelatedIssues.ascx";
                    break;
                case 7:
                    controlName = "Revisions.ascx";
                    break;
                case 8:
                    controlName = "TimeTracking.ascx";
                    break;
            }

            contentControl = Page.LoadControl("~/Issues/UserControls/" + controlName);
            ((IIssueTab)contentControl).IssueId = _IssueId;
            ((IIssueTab)contentControl).ProjectId = _ProjectId;
            plhContent.Controls.Clear();
            plhContent.Controls.Add(contentControl);
            contentControl.ID = "ctlContent";
        }



        /// <summary>
        /// Gets the name of the tab.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <returns></returns>
        private string GetTabName(string tab)
        {
            //if (IssueId == 0)
            //    return string.Empty;

            switch (tab)
            {
                case "Comments":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : IssueComment.GetIssueCommentsByIssueId(IssueId).Count);
                case "History":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : IssueHistory.GetIssueHistoryByIssueId(IssueId).Count);
                case "Attachments":
                    return string.Format("{0} ({1})",tab,IssueId == 0 ? 0 : IssueAttachment.GetIssueAttachmentsByIssueId(IssueId).Count);
                case "Notifications":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : IssueNotification.GetIssueNotificationsByIssueId(IssueId).Count);
                case "Related Issues":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : RelatedIssue.GetRelatedIssues(IssueId).Count);
                case "Parent Issues":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : RelatedIssue.GetParentIssues(IssueId).Count);
                case "Sub Issues":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : RelatedIssue.GetChildIssues(IssueId).Count);
                case "Revisions":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : IssueRevision.GetIssueRevisionsByIssueId(IssueId).Count);
                case "Time Tracking":
                    return string.Format("{0} ({1})", tab, IssueId == 0 ? 0 : IssueWorkReport.GetWorkReportsByIssueId(IssueId).Count);
                default:
                    return tab;
            }
        }

        /// <summary>
        /// Gets the tab icon URL.
        /// </summary>
        /// <returns></returns>
        private string GetTabIconUrl(string tab)
        {
            string imageUrl = string.Empty;

            switch (tab)
            {
                case "Comments":
                    imageUrl = "~/images/comment.gif";
                    break;
                case "Attachments":
                    imageUrl = "~/images/attach.gif";
                    break;
                case "History":
                    imageUrl = "~/images/history.gif";
                    break;
                case "Notifications":
                    imageUrl = "~/images/email.gif";
                    break;
                //case 4:
                //    imageUrl = "SubIssues.ascx";
                //    break;
                //case 5:
                //    imageUrl = "ParentIssues.ascx";
                //    break;
                case "Related Issues":
                    imageUrl = "~/images/link.gif";
                    break;
                case "Time Tracking":
                    imageUrl = "~/images/time.gif";
                    break;
            }
            return imageUrl;
        }

    }
}