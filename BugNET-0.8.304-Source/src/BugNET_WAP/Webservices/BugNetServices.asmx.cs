using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using BugNET.BusinessLogicLayer;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading;

namespace BugNET.Webservices
{
    /// <summary>
    /// Summary description for BugNetServices
    /// </summary>
    [WebService(Namespace = "http://bugnetproject.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class BugNetServices : LogInWebService
    {
        
        /// <summary>
        /// Creates the new issue revision.
        /// </summary>
        /// <param name="revision">The revision.</param>
        /// <param name="issueId">The issue id.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="revisionAuthor">The revision author.</param>
        /// <param name="revisionDate">The revision date.</param>
        /// <param name="revisionMessage">The revision message.</param>
        /// <returns>The new id of the revision</returns>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public bool CreateNewIssueRevision(int revision, int issueId,string repository,string revisionAuthor, string revisionDate, string revisionMessage)
        {
            int projectId = Issue.GetIssueById(issueId).ProjectId;
            //authentication checks against user access to project
            if (Project.GetProjectById(projectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, projectId))
                throw new UnauthorizedAccessException(string.Format(Logging.GetErrorMessageResource("ProjectAccessDenied"), UserName)); 

            IssueRevision issueRevision = new IssueRevision(revision, issueId, revisionAuthor, revisionMessage, repository, revisionDate);
            return issueRevision.Save();
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public bool CreateNewIssueAttachment(int issueId, string creatorUserName, string fileName, string contentType, byte[] attachment, int size, string description)
        {
            if (issueId <= 0)
                throw new ArgumentOutOfRangeException("issueId");

            int projectId = Issue.GetIssueById(issueId).ProjectId;

            //authentication checks against user access to project
            if (Project.GetProjectById(projectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, projectId))
                throw new UnauthorizedAccessException(string.Format(Logging.GetErrorMessageResource("ProjectAccessDenied"), UserName));

            IssueAttachment issueAttachment = new IssueAttachment(issueId, creatorUserName, fileName, contentType, attachment, size, description);
            return issueAttachment.Save();
        }


        /// <summary>
        /// Changes the tree node.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="newText">The new text.</param>
        /// <param name="oldText">The old text.</param>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod]
        public void ChangeTreeNode(string nodeId, string newText, string oldText)
        {          
            if (Convert.ToInt32(nodeId) == 0)
                return;
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentNullException("nodeId");
            if (string.IsNullOrEmpty(newText))
                throw new ArgumentNullException("newText");
            if (string.IsNullOrEmpty(oldText))
                throw new ArgumentNullException("oldText");

            Category c = Category.GetCategoryById(Convert.ToInt32(nodeId));
            if (c != null)
            {
                string UserName = Thread.CurrentPrincipal.Identity.Name;
                if (!ITUser.IsInRole(UserName, c.ProjectId, Globals.ProjectAdminRole) && !ITUser.IsInRole(UserName, 0, Globals.SuperUserRole))
                    throw new UnauthorizedAccessException(Logging.GetErrorMessageResource("AccessDenied"));

                c.Name = newText;
                c.Save();
            }

        }

        /// <summary>
        /// Moves the node.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="oldParentId">The old parent id.</param>
        /// <param name="newParentId">The new parent id.</param>
        /// <param name="index">The index.</param>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod]
        public void MoveNode(string nodeId, string oldParentId, string newParentId, string index)
        {          
            if (string.IsNullOrEmpty(nodeId))
                throw new ArgumentNullException("nodeId");
            if (string.IsNullOrEmpty(oldParentId))
                throw new ArgumentNullException("oldParentId");
            if (string.IsNullOrEmpty(newParentId))
                throw new ArgumentNullException("newParentId");
            if (string.IsNullOrEmpty(index))
                throw new ArgumentNullException("index");

            Category c = Category.GetCategoryById(Convert.ToInt32(nodeId));
            if (c != null)
            {
                string UserName = Thread.CurrentPrincipal.Identity.Name;

                if (!ITUser.IsInRole(UserName, c.ProjectId, Globals.ProjectAdminRole) && !ITUser.IsInRole(UserName, 0, Globals.SuperUserRole))
                    throw new UnauthorizedAccessException(Logging.GetErrorMessageResource("AccessDenied"));

                c.ParentCategoryId = Convert.ToInt32(newParentId);
                c.Save();
            }

        }

        /// <summary>
        /// Adds the Category.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="name">The name.</param>
        /// <param name="parentCategoryId">The parent Category id.</param>
        /// <returns>Id value of the created Category</returns>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public int AddCategory(string projectId, string name, string parentCategoryId)
        {  
            if (string.IsNullOrEmpty(projectId))
                throw new ArgumentNullException("projectId");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(parentCategoryId))
                throw new ArgumentNullException("parentCategoryId");

            string UserName = Thread.CurrentPrincipal.Identity.Name; 

            if (!ITUser.IsInRole(UserName, Convert.ToInt32(projectId), Globals.ProjectAdminRole) && !ITUser.IsInRole(UserName, 0, Globals.SuperUserRole))
                throw new UnauthorizedAccessException(Logging.GetErrorMessageResource("AccessDenied"));

            Category c = new Category(Convert.ToInt32(projectId),Convert.ToInt32(parentCategoryId), name, 0);
            c.Save();
            return c.Id;
        }

        /// <summary>
        /// Deletes the Category.
        /// </summary>
        /// <param name="CategoryId">The Category id.</param>
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
        [WebMethod(EnableSession = true)]
        public void DeleteCategory(string CategoryId)
        {
            if (string.IsNullOrEmpty(CategoryId))
                throw new ArgumentNullException("CategoryId");

            Category c = Category.GetCategoryById(Convert.ToInt32(CategoryId));
            if (c != null)
            {
                string UserName = Thread.CurrentPrincipal.Identity.Name;

                if (!ITUser.IsInRole(UserName, c.ProjectId, Globals.ProjectAdminRole) && !ITUser.IsInRole(UserName, 0, Globals.SuperUserRole))
                    throw new UnauthorizedAccessException(Logging.GetErrorMessageResource("AccessDenied"));

                Category.DeleteCategory(Convert.ToInt32(CategoryId));
            }
        }

        
       #region Methods for BugnetExplorer

       /// <summary>
       /// Returns all resolutions for a project
       /// </summary>
       /// <param name="ProjectId">id of project</param>
       /// <returns>Array of resolutionnames</returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public String[] GetResolutions(int ProjectId)
       {
           if (Project.GetProjectById(ProjectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, ProjectId))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           List<Resolution> resolutions = Resolution.GetResolutionsByProjectId(ProjectId);
           List<String> returnval = new List<String>();
           foreach (Resolution item in resolutions)
           {
               returnval.Add(item.Name.ToString());
           }
           return returnval.ToArray();
       }

       /// <summary>
       /// List of all project milestones
       /// </summary>
       /// <param name="ProjectId">project id</param>
       /// <returns>Array of all milestone names</returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public String[] GetMilestones(int ProjectId)
       {
           if (Project.GetProjectById(ProjectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, ProjectId))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           List<Milestone> milestones = Milestone.GetMilestoneByProjectId(ProjectId);
           List<String> returnval = new List<String>();
           foreach (Milestone item in milestones)
           {
               returnval.Add(item.Name.ToString());
           }
           return returnval.ToArray();

       }

       /// <summary>
       /// List all Issuetypes of a project
       /// </summary>
       /// <param name="ProjectId">project id</param>
       /// <returns>Array of all issue type names</returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public String[] GetIssueTypes(int ProjectId)
       {
           if (Project.GetProjectById(ProjectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, ProjectId))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           List<IssueType> issuetypes = IssueType.GetIssueTypesByProjectId(ProjectId);
           List<String> returnval = new List<String>();
           foreach (IssueType item in issuetypes)
           {
               returnval.Add(item.Name.ToString());
           }
           return returnval.ToArray();
       }

       /// <summary>
       /// List of all priorities in a project
       /// </summary>
       /// <param name="ProjectId">project id</param>
       /// <returns>Array of all priority names</returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public String[] GetPriorities(int ProjectId)
       {
           if (Project.GetProjectById(ProjectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, ProjectId))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           List<Priority> priorites = Priority.GetPrioritiesByProjectId(ProjectId);
           List<String> returnval = new List<String>();
           foreach (Priority item in priorites)
           {
               returnval.Add(item.Name.ToString());
           }
           return returnval.ToArray();
       }

       /// <summary>
       /// List of all categories in a project
       /// </summary>
       /// <param name="ProjectId">project id</param>
       /// <returns>Array of all category names</returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public String[] GetCategories(int ProjectId)
       {
           if (Project.GetProjectById(ProjectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, ProjectId))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           CategoryTree categoriyTree = new CategoryTree();
           List<Category> categories = categoriyTree.GetCategoryTreeByProjectId(ProjectId);
           List<String> returnval = new List<String>();
           foreach (Category item in categories)
           {
               returnval.Add(item.Name.ToString());
           }
           return returnval.ToArray();
       }

       /// <summary>
       /// List of all status in a project
       /// </summary>
       /// <param name="ProjectId">project id</param>
       /// <returns>Array of all status names</returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public String[] GetStatus(int ProjectId)
       {
           if (Project.GetProjectById(ProjectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, ProjectId))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           List<Status> statuslist = Status.GetStatusByProjectId(ProjectId);
           List<String> returnval = new List<String>();
           foreach (Status item in statuslist)
           {
               returnval.Add(item.Name.ToString());
           }
           return returnval.ToArray();
       }


       /// <summary>
       /// Returns the internal ID of a ProjectCode
       /// </summary>
       /// <param name="ProjectCode">Named ProjectCode</param>
       /// <returns>Project ID</returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public int GetProjectId(string ProjectCode)
       {
           Project project = Project.GetProjectByCode(ProjectCode);
           if (project.AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, project.Id))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           return project.Id;
       }

       /// <summary>
       /// Gets the project issues.
       /// </summary>
       /// <param name="ProjectId">The project id.</param>
       /// <param name="Filter">The filter.</param>
       /// <returns></returns>
       [PrincipalPermission(SecurityAction.Demand, Authenticated = true)]
       [WebMethod(EnableSession = true)]
       public object[] GetProjectIssues(int ProjectId, string Filter)
       {
           if (Project.GetProjectById(ProjectId).AccessType == Globals.ProjectAccessType.Private && !Project.IsUserProjectMember(UserName, ProjectId))
               throw new UnauthorizedAccessException(string.Format("The user {0} does not have permission to this project.", UserName)); //TODO: Get this from resource string

           List<Issue> issues;
           QueryClause q;
           List<QueryClause> queryClauses = new List<QueryClause>();
           string BooleanOperator = "AND";

           if (Filter.Trim() == "")
           {
               // Return all Issues
               issues = Issue.GetIssuesByProjectId(ProjectId);
           }
           else
           {
               foreach (string item in Filter.Split('&'))
               {
                   if (item.StartsWith("status=", StringComparison.CurrentCultureIgnoreCase))
                   {
                       if (item.EndsWith("=notclosed", StringComparison.CurrentCultureIgnoreCase))
                       {
                           List<Status> status = Status.GetStatusByProjectId(ProjectId).FindAll(delegate(Status s) { return s.IsClosedState == true; });
                           foreach (Status st in status)
                           {
                               q = new QueryClause(BooleanOperator, "IssueStatusId", "<>", st.Id.ToString(), SqlDbType.Int, false);
                               queryClauses.Add(q);
                           }
                       }
                       else if (item.EndsWith("=new", StringComparison.CurrentCultureIgnoreCase))
                       {
                           q = new QueryClause(BooleanOperator, "AssignedUsername", "=", "none", SqlDbType.NVarChar, false);
                           queryClauses.Add(q);
                           List<Status> status = Status.GetStatusByProjectId(ProjectId).FindAll(delegate(Status s) { return s.IsClosedState == true; });
                           foreach (Status st in status)
                           {
                               q = new QueryClause(BooleanOperator, "IssueStatusId", "<>", st.Id.ToString(), SqlDbType.Int, false);
                               queryClauses.Add(q);
                           }
                       }
                   }
                   else if (item.StartsWith("owner=", StringComparison.CurrentCultureIgnoreCase))
                   {
                       q = new QueryClause(BooleanOperator, "OwnerUsername", "=", item.Substring(item.IndexOf('=') + 1, item.Length - item.IndexOf('=') - 1).ToString(), SqlDbType.NVarChar, false);
                       queryClauses.Add(q);
                   }
                   else if (item.StartsWith("reporter=", StringComparison.CurrentCultureIgnoreCase))
                   {
                       q = new QueryClause(BooleanOperator, "CreatorUsername", "=", item.Substring(item.IndexOf('=') + 1, item.Length - item.IndexOf('=') - 1).ToString(), SqlDbType.NVarChar, false);
                       queryClauses.Add(q);
                   }
                   else if (item.StartsWith("assigned=", StringComparison.CurrentCultureIgnoreCase))
                   {
                       q = new QueryClause(BooleanOperator, "AssignedUsername", "=", item.Substring(item.IndexOf('=') + 1, item.Length - item.IndexOf('=') - 1).ToString(), SqlDbType.NVarChar, false);
                       queryClauses.Add(q);
                   }
               }
               issues = Issue.PerformQuery(ProjectId, queryClauses);
           }

           List<Object> issueList = new List<Object>();
           Object[] issueitem;
           foreach (Issue item in issues)
           {
               issueitem = new Object[13];
               issueitem[0] = item.Id;
               issueitem[1] = item.DateCreated;
               issueitem[2] = item.LastUpdate;
               issueitem[3] = item.StatusName;
               issueitem[4] = item.Description;
               issueitem[5] = item.CreatorUserName;
               issueitem[6] = item.ResolutionName;
               issueitem[7] = item.CategoryName;
               issueitem[8] = item.Title;
               issueitem[9] = item.PriorityName;
               issueitem[10] = item.MilestoneName;
               issueitem[11] = item.OwnerUserName;
               issueitem[12] = item.IssueTypeName;
               issueList.Add(issueitem);
           }
           return issueList.ToArray();
       }


       #endregion

    }
}
