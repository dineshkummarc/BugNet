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
using System.Reflection;
using BugNET.UserInterfaceLayer;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BugNET
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public partial class _Default : System.Web.UI.Page
	{

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Page.Title = string.Format("{0} - {1}", "Home", HostSetting.GetHostSetting("ApplicationTitle"));
            if (!Page.IsPostBack)
            {
                lblApplicationTitle.Text = HostSetting.GetHostSetting("ApplicationTitle");
                WelcomeMessage.Text = HostSetting.GetHostSetting("WelcomeMessage");
                
            }

			if (!Context.User.Identity.IsAuthenticated)			
			{	
				//get all public available projects here
				if(!Boolean.Parse(HostSetting.GetHostSetting("DisableAnonymousAccess")))
				{
					rptProject.DataSource = Project.GetPublicProjects();
				}
				else
				{
					rptProject.Visible=false;
                    lblMessage.Text = GetLocalResourceObject("AnonymousAccessDisabled").ToString();
                    lblMessage.Visible=true;
				}     
			}
			else
			{
				rptProject.DataSource = Project.GetProjectsByMemberUserName(User.Identity.Name);	
			}

			rptProject.DataBind();

            if (!lblMessage.Visible)
            // remember that we could have set the message already!
            {
                if (rptProject.Items.Count == 0)
                {
                    if (!Context.User.Identity.IsAuthenticated)
                    {
                        lblMessage.Text = GetLocalResourceObject("RegisterAndLoginMessage").ToString();
                        lblMessage.Visible = true;
                    }
                    else
                    {
                        lblMessage.Text = GetLocalResourceObject("NoProjectsToViewMessage").ToString();
                        lblMessage.Visible = true;
                    }
                }
            }

		
		}

		#region Web Form Designer generated code
        /// <summary>
        /// Overrides the default OnInit to provide a security check for pages
        /// </summary>
        /// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.rptProject.ItemDataBound+=new RepeaterItemEventHandler(rptProject_ItemDataBound);
		}
		#endregion

        /// <summary>
        /// Handles the ItemDataBound event of the rptProject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Web.UI.WebControls.RepeaterItemEventArgs"/> instance containing the event data.</param>
		private void rptProject_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{

			//check permissions
			if(e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				Project p = (Project)e.Item.DataItem;

				if(!Context.User.Identity.IsAuthenticated || !ITUser.HasPermission(p.Id,Globals.Permission.ADD_ISSUE.ToString()))
					e.Item.FindControl("ReportIssue").Visible=false;

                if (!Context.User.Identity.IsAuthenticated || !ITUser.HasPermission(p.Id, Globals.Permission.ADMIN_EDIT_PROJECT.ToString()))
                    e.Item.FindControl("Settings").Visible = false;

                if (!Context.User.Identity.IsAuthenticated || !ITUser.HasPermission(p.Id, Globals.Permission.VIEW_PROJECT_CALENDAR.ToString()))
                    e.Item.FindControl("ProjectCalendar").Visible = false;

                // Security and ease of use, ProjectImage comes from the attachment mechanism
                //
                // Secure because the image is not dynamically processed server side (eg hostile php)                

                System.Web.UI.WebControls.Image ProjectImage = (System.Web.UI.WebControls.Image)e.Item.FindControl("ProjectImage");                 
                ProjectImage.ImageUrl = string.Format("~/DownloadAttachment.axd?id={0}&mode=project",p.Id);
                
                Label OpenIssuesLink = (Label)e.Item.FindControl("OpenIssues");
                Label NextMilestoneDue = (Label)e.Item.FindControl("NextMilestoneDue");
                Label MilestoneComplete = (Label)e.Item.FindControl("MilestoneComplete");

                string milestone = string.Empty;
                //TODO Localize this with the same stuff as roadmap
                List<Milestone> milestoneList = Milestone.GetMilestoneByProjectId(p.Id);
                milestoneList = milestoneList.FindAll(m => m.DueDate.HasValue && m.IsCompleted != true);

                if (milestoneList.Count > 0)
                {

                    List<Milestone> sortedMilestoneList = milestoneList.Sort<Milestone>("DueDate").ToList();
                    Milestone mileStone = sortedMilestoneList[0];
                    if (mileStone != null)
                    {
                        milestone = ((DateTime)mileStone.DueDate).ToShortDateString();
                        int[] progressValues = Project.GetRoadMapProgress(p.Id, mileStone.Id);
                        if (progressValues[0] != 0 || progressValues[1] != 0)
                        {
                            double percent = progressValues[0] * 100 / progressValues[1];
                            MilestoneComplete.Text = string.Format("{0}%", percent.ToString());
                        }
                        else
                        {
                            MilestoneComplete.Text = "0%";
                        }

                    } else
                        milestone = "none";

                    NextMilestoneDue.Text = string.Format("Next Milestone Due - {0}", milestone);
                }
                else
                {
                    NextMilestoneDue.Text = "Next Milestone Due - <font color='red'>No Due Dates Set</font>";
                }

                //get total open issues
                List<QueryClause> queryClauses = new List<QueryClause>();
                List<Status> status = Status.GetStatusByProjectId(p.Id);

                if (status.Count > 0)
                {
                    List<Status> closedStatus = status.FindAll(s => s.IsClosedState);                  
                    foreach (Status st in closedStatus)
                    {
                        queryClauses.Add(new QueryClause("AND", "IssueStatusId", "<>", st.Id.ToString(), SqlDbType.Int, false));
                    }
                    if (queryClauses.Count > 0)
                    {
                        List<Issue> issueList = Issue.PerformQuery(p.Id, queryClauses);
                        OpenIssuesLink.Text = string.Format("{0} open issues", issueList.Count);
                    }
                    else
                    {
                        // No open issue states means that nothing is open.
                        // Optimization by not running the query.
                        OpenIssuesLink.Text = string.Format("{0} open issues", 0);
                    }
                }
                else
                {
                    // Warn users of a problem
                    OpenIssuesLink.Text = "Error: No Status set on Project!";
                }
               

				HyperLink atu = (HyperLink)e.Item.FindControl("AssignedToUser");
				Control AssignedUserFilter = e.Item.FindControl("AssignedUserFilter");

				if(Context.User.Identity.IsAuthenticated && Project.IsUserProjectMember(User.Identity.Name,p.Id))
				{
                    System.Web.Security.MembershipUser user = ITUser.GetUser(User.Identity.Name);
                    atu.NavigateUrl = string.Format("~/Issues/IssueList.aspx?pid={0}&u={1}", p.Id, user.ProviderUserKey);
				}
				else
				{
					AssignedUserFilter.Visible=false;
				}
			}

		}

        
	}

    
}
