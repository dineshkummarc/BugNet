using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BugNET.BusinessLogicLayer;
using BugNET.UserInterfaceLayer;
using System.IO;
using log4net;

namespace BugNET.Issues.UserControls
{
    public partial class TimeTracking : System.Web.UI.UserControl, IIssueTab
    {
        private double Total;
        private int _IssueId = 0;
        private int _ProjectId = 0;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region IIssueTab Members

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
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            TimeEntriesDataGrid.Columns[0].HeaderText = GetLocalResourceObject("TimeEntriesDataGrid.WorkDateHeader.Text").ToString();
            TimeEntriesDataGrid.Columns[1].HeaderText = GetLocalResourceObject("TimeEntriesDataGrid.DurationHeader.Text").ToString();
            TimeEntriesDataGrid.Columns[2].HeaderText = GetLocalResourceObject("TimeEntriesDataGrid.CreatorHeader.Text").ToString();
            TimeEntriesDataGrid.Columns[3].HeaderText = GetLocalResourceObject("TimeEntriesDataGrid.CommentHeader.Text").ToString();

            BindTimeEntries();

            if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.ADD_TIME_ENTRY.ToString()))
                AddTimeEntryPanel.Visible = false;

            if (!Page.User.Identity.IsAuthenticated || !ITUser.HasPermission(ProjectId, Globals.Permission.DELETE_TIME_ENTRY.ToString()))
                TimeEntriesDataGrid.Columns[4].Visible = false;
        }

        #endregion

        /// <summary>
        /// Binds the work reports.
        /// </summary>
        private void BindTimeEntries()
        {
            //System.Globalization.NumberFormatInfo nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            double minimum = 0;

            RangeValidator1.MinimumValue = minimum.ToString();
            RangeValidator1.CultureInvariantValues = true;

            TimeEntryDate.Text = DateTime.Today.ToShortDateString();
            cpTimeEntry.ValueToCompare = DateTime.Today.ToShortDateString();

            List<IssueWorkReport> WorkReports = IssueWorkReport.GetWorkReportsByIssueId(IssueId);
            if (WorkReports == null || WorkReports.Count == 0)
            {
                TimeEntryLabel.Text = GetLocalResourceObject("NoTimeEntries").ToString();
                TimeEntryLabel.Visible = true;
                TimeEntriesDataGrid.Visible = false;
            }
            else
            {
                TimeEntriesDataGrid.Visible = true;
                TimeEntryLabel.Visible = false;
                TimeEntriesDataGrid.DataSource = WorkReports;
                TimeEntriesDataGrid.DataBind();
            }
        }

        /// <summary>
        /// Handles the Click event of the cmdAddBugTimeEntry control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AddTimeEntry_Click(object sender, EventArgs e)
        {
            if (this.DurationTextBox.Text.Trim().Length != 0)
            {
                IssueWorkReport newWorkReport = new IssueWorkReport(IssueId, Convert.ToDateTime(TimeEntryDate.Text), Convert.ToDecimal(this.DurationTextBox.Text), CommentHtmlEditor.Text.Trim(), Context.User.Identity.Name);
                newWorkReport.Save();
                CommentHtmlEditor.Text = string.Empty;
                DurationTextBox.Text = string.Empty;
                BindTimeEntries();
            }
        }

        /// <summary>
        /// Handles the ItemCommand event of the TimeEntriesDataGrid control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void TimeEntriesDataGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (IssueWorkReport.DeleteIssueWorkReport(id))
                BindTimeEntries();
        }

        /// <summary>
        /// Handles the ItemDataBound event of the TimeEntriesDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void TimeEntriesDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            string delete = string.Format("return confirm('{0}');", GetLocalResourceObject("DeleteTimeEntry").ToString());
            switch (e.Item.ItemType)
            {

                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    Total += Convert.ToDouble(e.Item.Cells[1].Text);
                    ((ImageButton)e.Item.FindControl("RemoveEntry")).OnClientClick = delete;
                    ((LinkButton)e.Item.FindControl("lnkDeleteTimeEntry")).OnClientClick = delete;
                    break;
                case ListItemType.Footer:
                    //Use the footer to display the summary row.
                    e.Item.Cells[0].Text = "Total Hours:";
                    e.Item.Cells[0].Attributes.Add("align", "left");
                    e.Item.Cells[0].Style.Add("font-weight", "bold");
                    e.Item.Cells[0].Style.Add("padding-top", "10px");
                    e.Item.Cells[0].Style.Add("border-top", "1px solid #999");
                    e.Item.Cells[1].Attributes.Add("align", "right");
                    e.Item.Cells[1].Style.Add("border-top", "1px solid #999");
                    e.Item.Cells[1].Style.Add("padding-top", "10px");
                    e.Item.Cells[1].Text = Total.ToString();
                    e.Item.Cells[2].Style.Add("border-top", "1px solid #999");
                    e.Item.Cells[3].Style.Add("border-top", "1px solid #999");
                    e.Item.Cells[4].Style.Add("border-top", "1px solid #999");
                    break;
            }

        }
    }
}