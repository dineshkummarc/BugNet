﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using log4net;
using log4net.Config;
using BugNET.BusinessLogicLayer;
using System.Configuration.Provider;
using System.IO;
using System.Linq;


namespace BugNET.DataAccessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlDataProvider : DataProvider
    {
        /*** DELEGATE ***/
        private delegate void TGenerateListFromReader<T>(SqlDataReader returnData, ref List<T> tempList);
        private static readonly ILog Log = LogManager.GetLogger(typeof(SqlDataProvider));
        private string _connectionString = string.Empty;
        private string _providerPath = string.Empty;
        private string _mappedProviderPath = string.Empty;

        /// <summary>
        /// Initializes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="config">The configuration</param>
        public override void Initialize(string name, NameValueCollection config)
        {          
            base.Initialize(name, config);

            string str = config["connectionStringName"];

            if (config["providerPath"] != null)
                _providerPath = config["providerPath"];

            this._connectionString = ConfigurationManager.ConnectionStrings[str].ConnectionString;
           
            if (string.IsNullOrEmpty(str))
                throw new ProviderException("connectionStringName value not specified in web.config for SqlDataProvider");

            if (string.IsNullOrEmpty(this._connectionString))
                throw new ProviderException("connectionString value not specified in web.config");

            if (string.IsNullOrEmpty(this._providerPath))
                throw new ProviderException("providerPath folder value not specified in web.config for SqlDataProvider");

        }

        #region Private Constants
        private const string DATA_ACCESS_POLICY = "Data Access Policy";

        /// <summary>
        /// Stored Procedure Constants
        /// </summary>
        private const string SP_PROJECT_CREATE = "BugNet_Project_CreateNewProject";
        private const string SP_PROJECT_DELETE = "BugNet_Project_DeleteProject";
        private const string SP_PROJECT_GETALLPROJECTS = "BugNet_Project_GetAllProjects";
        private const string SP_PROJECT_GETPUBLICPROJECTS = "BugNet_Project_GetPublicProjects";
        private const string SP_PROJECT_GETPROJECTBYID = "BugNet_Project_GetProjectById";
        private const string SP_PROJECT_UPDATE = "BugNet_Project_UpdateProject";
        private const string SP_PROJECT_ADDUSERTOPROJECT = "BugNet_Project_AddUserToProject";
        private const string SP_PROJECT_REMOVEUSERFROMPROJECT = "BugNet_Project_RemoveUserFromProject";
        private const string SP_PROJECT_GETPROJECTSBYMEMBERUSERNAME = "BugNet_Project_GetProjectsByMemberUserName";
        private const string SP_PROJECT_GETPROJECTBYCODE = "BugNet_Project_GetProjectByCode";
        private const string SP_PROJECT_GETPROJECTBYMAILBOX = "BugNet_Project_GetProjectByMailbox";
        private const string SP_PROJECT_GETMAILBYPROJECTID = "BugNet_Project_GetMailboxByProjectId";
        private const string SP_PROJECT_CREATEPROJECTMAILBOX = "BugNet_Project_CreateProjectMailbox";
        private const string SP_PROJECT_DELETEPROJECTMAILBOX = "BugNet_Project_DeleteProjectMailbox";
        private const string SP_PROJECT_CLONEPROJECT = "BugNet_Project_CloneProject";
        private const string SP_PROJECT_GETROADMAP = "BugNet_Project_GetRoadMap";
        private const string SP_PROJECT_GETROADMAPPROGRESS = "BugNet_Project_GetRoadMapProgress";
        private const string SP_PROJECT_GETCHANGELOG = "BugNet_Project_GetChangeLog";

        private const string SP_PROJECT_ISUSERPROJECTMEMBER = "BugNet_Project_IsUserProjectMember";

        //User - Role Stored Procs
        private const string SP_USER_GETUSERSBYPROJECTID = "BugNet_User_GetUsersByProjectId";
        private const string SP_USER_AUTHENTICATE = "BugNet_User_Authenticate";
        private const string SP_USER_GETUSERBYUSERNAME = "BugNet_User_GetUserByUserName";
        private const string SP_USER_GETUSERBYID = "BugNet_User_GetUserById";
        private const string SP_USER_GETALLUSERS = "BugNet_User_GetAllUsers";
        private const string SP_USER_GETALLUSERSBYROLENAME = "BugNet_User_GetAllUsersByRoleName";
        private const string SP_USER_UPDATEUSER = "BugNet_User_UpdateUser";
        private const string SP_USER_CREATENEWUSER = "BugNet_User_CreateNewUser";
        private const string SP_PROJECT_GETMEMBERROLESBYPROJECTID = "BugNET_Project_GetMemberRolesByProjectId";


        private const string SP_PERMISSION_GETALLPERMISSIONS = "BugNet_Permission_GetAllPermissions";
        private const string SP_PERMISSION_GETROLEPERMISSIONS = "BugNet_Permission_GetRolePermission";
        private const string SP_PERMISSION_GETPERMISSIONSBYROLE = "BugNet_Permission_GetPermissionsByRole";
        private const string SP_PERMISSION_DELETEROLEPERMISSION = "BugNet_Permission_DeleteRolePermission";
        private const string SP_PERMISSION_ADDROLEPERMISSION = "BugNet_Permission_AddRolePermission";


        private const string SP_ROLE_GETPROJECTROLESBYUSER = "BugNet_Role_GetProjectRolesByUser";
        private const string SP_ROLE_GETROLESBYUSER = "BugNet_Role_GetRolesByUser";
        private const string SP_ROLE_GETROLEBYID = "BugNet_Role_GetRoleById";
        private const string SP_ROLE_GETROLESBYPROJECT = "BugNet_Role_GetRolesByProject";
        private const string SP_ROLE_ROLEEXISTS = "BugNet_Role_RoleExists";
        private const string SP_ROLE_GETALLROLES = "BugNet_Role_GetAllRoles";
        private const string SP_ROLE_DELETEROLE = "BugNet_Role_DeleteRole";
        private const string SP_ROLE_REMOVEUSERFROMROLE = "BugNet_Role_RemoveUserFromRole";
        private const string SP_ROLE_ADDUSERTOROLE = "BugNet_Role_AddUserToRole";
        private const string SP_ROLE_UPDATEROLE = "BugNet_Role_UpdateRole";
        private const string SP_ROLE_CREATE = "BugNet_Role_CreateNewRole";

        //Issue Stored Procs
        private const string SP_ISSUE_CREATE = "BugNet_Issue_CreateNewIssue";
        private const string SP_ISSUE_UPDATE = "BugNet_Issue_UpdateIssue";
        private const string SP_ISSUE_DELETE = "BugNet_Issue_Delete";
        private const string SP_ISSUE_GETISSUEBYID = "BugNet_Issue_GetIssueById";
        private const string SP_ISSUE_UPDATEISSUE = "BugNet_Issue_UpdateIssue";
        private const string SP_ISSUE_GETISSUESBYRELEVANCY = "BugNet_Issue_GetIssuesByRelevancy";
        private const string SP_ISSUE_GETISSUESBYASSIGNEDUSERNAME = "BugNet_Issue_GetIssuesByAssignedUserName";
        private const string SP_ISSUE_GETISSUESBYCREATORUSERNAME = "BugNet_Issue_GetIssuesByCreatorUserName";
        private const string SP_ISSUE_GETISSUESBYOWNERUSERNAME = "BugNet_Issue_GetIssuesByOwnerUserName";
        private const string SP_ISSUE_GETMONITOREDISSUESBYUSERNAME = "BugNet_Issue_GetMonitoredIssuesByUserName";
        private const string SP_ISSUE_GETISSUESBYPROJECTID = "BugNet_Issue_GetIssuesByProjectId";

        private const string SP_ISSUE_GETISSUEMILESTONECOUNTBYPROJECT = "BugNet_Issue_GetIssueMilestoneCountByProject";
        private const string SP_ISSUE_GETISSUEVERSIONCOUNTBYPROJECT = "BugNet_Issue_GetIssueVersionCountByProject";
        private const string SP_ISSUE_GETISSUESTATUSCOUNTBYPROJECT = "BugNet_Issue_GetIssueStatusCountByProject";
        private const string SP_ISSUE_GETISSUEPRIORITYCOUNTBYPROJECT = "BugNet_Issue_GetIssuePriorityCountByProject";
        private const string SP_ISSUE_GETISSUEUSERCOUNTBYPROJECT = "BugNet_Issue_GetIssueUserCountByProject";
        private const string SP_ISSUE_GETISSUEUNASSIGNEDCOUNTBYPROJECT = "BugNet_Issue_GetIssueUnassignedCountByProject";
        private const string SP_ISSUE_GETISSUEUNSCHEDULEDMILESTONECOUNTBYPROJECT = "BugNet_Issue_GetIssueUnscheduledMilestoneCountByProject";
        private const string SP_ISSUE_GETISSUETYPECOUNTBYPROJECT = "BugNet_Issue_GetIssueTypeCountByProject";
        private const string SP_ISSUE_GETISSUECATEGORYCOUNTBYPROJECT = "BugNet_Issue_GetIssueCategoryCountByProject";
        private const string SP_ISSUE_GETISSUESBYCOMPONENT = "BugNet_Issue_GetIssuesByComponent";
        private const string SP_ISSUE_GETISSUESBYVERSION = "BugNet_Issue_GetIssuesByVersion";
        private const string SP_ISSUE_GETISSUESBYTYPE = "BugNet_Issue_GetIssuesByType";
        private const string SP_ISSUE_GETISSUESBYPRIORITY = "BugNet_Issue_GetIssuesByPriority";
        private const string SP_ISSUE_GETISSUESBYSTATUS = "BugNet_Issue_GetIssuesByStatus";
        private const string SP_ISSUE_GETISSUESASSIGNEDTO = "BugNet_Issue_GetIssuesAssignedTo";
        private const string SP_ISSUE_GETOPENISSUES = "BugNet_Issue_GetOpenIssues";


        private const string SP_ISSUE_GETRECENTLYADDEDISSUESBYPROJECT = "BugNet_Issue_GetRecentlyAddedIssuesByProject";
        private const string SP_ISSUE_GETCHANGELOG = "BugNet_Issue_GetChangeLog";

        private const string SP_ISSUE_GETROADMAPPROGRESS = "BugNet_Issue_GetRoadMapProgress";
        private const string SP_ISSUE_GETISSUESBYCRITERIA = "BugNet_Issue_GetIssuesByCriteria";


        private const string SP_QUERY_GETQUERIESBYUSERNAME = "BugNet_Query_GetQueriesByUsername";
        private const string SP_QUERY_SAVEQUERY = "BugNet_Query_SaveQuery";
        private const string SP_QUERY_UPDATEQUERY = "BugNet_Query_UpdateQuery";
        private const string SP_QUERY_SAVEQUERYCLAUSE = "BugNet_Query_SaveQueryClause";
        private const string SP_QUERY_GETSAVEDQUERY = "BugNet_Query_GetSavedQuery";
        private const string SP_QUERY_DELETEQUERY = "BugNet_Query_DeleteQuery";
        private const string SP_QUERY_GETQUERYBYID = "BugNet_Query_GetQueryById";

        //Related Issue Stored Procs
        private const string SP_RELATEDISSUE_GETRELATEDISSUES = "BugNet_RelatedIssue_GetRelatedIssues";
        private const string SP_RELATEDISSUE_CREATENEWRELATEDISSUE = "BugNet_RelatedIssue_CreateNewRelatedIssue";
        private const string SP_RELATEDISSUE_DELETERELATEDISSUE = "BugNet_RelatedIssue_DeleteRelatedIssue";
        private const string SP_RELATEDISSUE_CREATENEWPARENTISSUE = "BugNet_RelatedIssue_CreateNewParentIssue";
        private const string SP_RELATEDISSUE_CREATENEWCHILDISSUE = "BugNet_RelatedIssue_CreateNewChildIssue";
        private const string SP_RELATEDISSUE_DELETECHILDISSUE = "BugNet_RelatedIssue_DeleteChildIssue";
        private const string SP_RELATEDISSUE_DELETEPARENTISSUE = "BugNet_RelatedIssue_DeleteParentIssue";
        private const string SP_RELATEDISSUE_GETPARENTISSUES = "BugNet_RelatedIssue_GetParentIssues";
        private const string SP_RELATEDISSUE_GETCHILDISSUES = "BugNet_RelatedIssue_GetChildIssues";

        //Attachment Stored Procs
        private const string SP_ISSUEATTACHMENT_CREATE = "BugNet_IssueAttachment_CreateNewIssueAttachment";
        private const string SP_ISSUEATTACHMENT_GETATTACHMENTBYID = "BugNet_IssueAttachment_GetIssueAttachmentById";
        private const string SP_ISSUEATTACHMENT_GETATTACHMENTSBYISSUEID = "BugNet_IssueAttachment_GetIssueAttachmentsByIssueId";
        private const string SP_ISSUEATTACHMENT_DELETEATTACHMENT = "BugNet_IssueAttachment_DeleteIssueAttachment";

        //Comment Stored Procs
        private const string SP_ISSUECOMMENT_CREATE = "BugNet_IssueComment_CreateNewIssueComment";
        private const string SP_ISSUECOMMENT_GETISSUECOMMENTBYID = "BugNet_IssueComment_GetIssueCommentById";
        private const string SP_ISSUECOMMENT_GETISSUECOMMENTSBYISSUEID = "BugNet_IssueComment_GetIssueCommentsByIssueId";
        private const string SP_ISSUECOMMENT_DELETE = "BugNet_IssueComment_DeleteIssueComment";
        private const string SP_ISSUECOMMENT_UPDATE = "BugNet_IssueComment_UpdateIssueComment";

        //Issue Revisions
        private const string SP_ISSUEREVISION_CREATE = "BugNet_IssueRevision_CreateNewIssueRevision";
        private const string SP_ISSUEREVISION_GETISSUEREVISIONSBYISSUEID = "BugNet_IssueRevision_GetIssueRevisionsByIssueId";
        private const string SP_ISSUEREVISION_DELETE = "BugNet_IssueRevision_DeleteIssueRevision";

        //Issue Votes
        private const string SP_ISSUEVOTE_CREATE = "BugNet_IssueVote_CreateNewIssueVote";
        private const string SP_ISSUEVOTE_HASUSERVOTED = "BugNet_IssueVote_HasUserVoted";


        //Milestone Stored Procs
        private const string SP_MILESTONE_CREATE = "BugNet_ProjectMilestones_CreateNewMilestone";
        private const string SP_MILESTONE_GETMILESTONEBYPROJECTID = "BugNet_ProjectMilestones_GetMilestonesByProjectId";
        private const string SP_MILESTONE_DELETE = "BugNet_ProjectMilestones_DeleteMilestone";
        private const string SP_MILESTONE_GETMILESTONEBYID = "BugNet_ProjectMilestones_GetMilestoneById";
        private const string SP_MILESTONE_UPDATE = "BugNet_ProjectMilestones_UpdateMilestone";

        //Category Stored Procs
        private const string SP_CATEGORY_CREATE = "BugNet_ProjectCategories_CreateNewCategory";
        private const string SP_CATEGORY_UPDATE = "BugNet_ProjectCategories_UpdateCategory";
        private const string SP_CATEGORY_DELETE = "BugNet_ProjectCategories_DeleteCategory";
        private const string SP_CATEGORY_GETCATEGORIESBYPROJECTID = "BugNet_ProjectCategories_GetCategoriesByProjectId";
        private const string SP_CATEGORY_GETCATEGORYBYID = "BugNet_ProjectCategories_GetCategoryById";
        private const string SP_CATEGORY_GETROOTCATEGORIESBYPROJECTID = "BugNet_ProjectCategories_GetRootCategoriesByProjectId";
        private const string SP_CATEGORY_GETCHILDCATEGORIESBYCATEGORYID = "BugNet_ProjectCategories_GetChildCategoriesByCategoryId";

        //Status
        private const string SP_STATUS_GETSTATUSBYPROJECTID = "BugNet_ProjectStatus_GetStatusByProjectId";
        private const string SP_STATUS_CREATE = "BugNet_ProjectStatus_CreateNewStatus";
        private const string SP_STATUS_UPDATE = "BugNet_ProjectStatus_UpdateStatus";
        private const string SP_STATUS_GETSTATUSBYID = "BugNet_ProjectStatus_GetStatusById";
        private const string SP_STATUS_DELETE = "BugNet_ProjectStatus_DeleteStatus";

        //History Stored Procs
        private const string SP_ISSUEHISTORY_CREATENEWISSUEHISTORY = "BugNet_IssueHistory_CreateNewIssueHistory";
        private const string SP_ISSUEHISTORY_GETISSUEHISTORYBYISSUEID = "BugNet_IssueHistory_GetIssueHistoryByIssueId";

        //Issue Type Stored Procs
        private const string SP_ISSUETYPE_GETISSUETYPEBYID = "BugNet_ProjectIssueTypes_GetIssueTypeById";
        private const string SP_ISSUETYPE_GETISSUETYPESBYPROJECTID = "BugNet_ProjectIssueTypes_GetIssueTypesByProjectId";
        private const string SP_ISSUETYPE_CREATE = "BugNet_ProjectIssueTypes_CreateNewIssueType";
        private const string SP_ISSUETYPE_DELETE = "BugNet_ProjectIssueTypes_DeleteIssueType";
        private const string SP_ISSUETYPE_UPDATE = "BugNet_ProjectIssueTypes_UpdateIssueType";

        //Resolution Stored Procs
        private const string SP_RESOLUTION_GETRESOLUTIONBYID = "BugNet_ProjectResolutions_GetResolutionById";
        private const string SP_RESOLUTION_GETRESOLUTIONSBYPROJECTID = "BugNet_ProjectResolutions_GetResolutionsByProjectId";
        private const string SP_RESOLUTION_CREATE = "BugNet_ProjectResolutions_CreateNewResolution";
        private const string SP_RESOLUTION_DELETE = "BugNet_ProjectResolutions_DeleteResolution";
        private const string SP_RESOLUTION_UPDATE = "BugNet_ProjectResolutions_UpdateResolution";

        //Priority Stored Procs
        private const string SP_PRIORITY_GETPRIORITYBYID = "BugNet_ProjectPriorities_GetPriorityById";
        private const string SP_PRIORITY_GETPRIORITIESBYPROJECTID = "BugNet_ProjectPriorities_GetPrioritiesByProjectId";
        private const string SP_PRIORITY_CREATE = "BugNet_ProjectPriorities_CreateNewPriority";
        private const string SP_PRIORITY_DELETE = "BugNet_ProjectPriorities_DeletePriority";
        private const string SP_PRIORITY_UPDATE = "BugNet_ProjectPriorities_UpdatePriority";

        //Notification Stored Procs
        private const string SP_ISSUENOTIFICATION_CREATE = "BugNet_IssueNotification_CreateNewIssueNotification";
        private const string SP_ISSUENOTIFICATION_DELETE = "BugNet_IssueNotification_DeleteIssueNotification";
        private const string SP_ISSUENOTIFICATION_GETISSUENOTIFICATIONSBYISSUEID = "BugNet_IssueNotification_GetIssueNotificationsByIssueId";

        //Project Notification
        private const string SP_PROJECTNOTIFICATION_CREATE = "BugNet_ProjectNotification_CreateNewProjectNotification";
        private const string SP_PROJECTNOTIFICATION_DELETE = "BugNet_ProjectNotification_DeleteProjectNotification";
        private const string SP_PROJECTNOTIFICATION_GETPROJECTNOTIFICATIONSBYPROJECTID = "BugNet_ProjectNotification_GetProjectNotificationsByProjectId";
        private const string SP_PROJECTNOTIFICATION_GETPROJECTNOTIFICATIONSBYUSERNAME = "BugNet_ProjectNotification_GetProjectNotificationsByUsername";

        private const string SP_HOSTSETTING_GETHOSTSETTINGS = "BugNet_HostSetting_GetHostSettings";
        private const string SP_HOSTSETTING_UPDATEHOSTSETTING = "BugNet_HostSetting_UpdateHostSetting";

        private const string SP_ISSUEWORKREPORT_CREATE = "BugNet_IssueWorkReport_CreateNewIssueWorkReport";
        private const string SP_ISSUEWORKREPORT_DELETE = "BugNet_IssueWorkReport_DeleteIssueWorkReport";
        private const string SP_ISSUEWORKREPORT_GETBYISSUEWORKREPORTSBYISSUEID = "BugNet_IssueWorkReport_GetIssueWorkReportsByIssueId";
        private const string SP_ISSUEWORKREPORT_GETISSUEWORKREPORTBYPROJECTID = "BugNet_IssueWorkReport_GetIssueWorkReportByProjectId";
        private const string SP_ISSUEWORKREPORT_GETISSUEWORKREPORTBYPROJECTMEMBER = "BugNet_TimeEntry_GetProjectWorkerWorkReport";

        private const string SP_ROLEPERMISSION_GETROLEPERMISSION = "BugNet_Permission_GetRolePermission";

        private const string SP_APPLICATIONLOG_GETLOG = "BugNet_ApplicationLog_GetLog";
        private const string SP_APPLICATIONLOG_GETLOGCOUNT = "BugNet_ApplicationLog_GetLogCount";
        private const string SP_APPLICATIONLOG_CLEARLOG = "BugNet_ApplicationLog_ClearLog";

        private const string SP_CUSTOMFIELD_GETCUSTOMFIELDBYID = "BugNet_ProjectCustomField_GetCustomFieldById";
        private const string SP_CUSTOMFIELD_GETCUSTOMFIELDSBYPROJECTID = "BugNet_ProjectCustomField_GetCustomFieldsByProjectId";
        private const string SP_CUSTOMFIELD_GETCUSTOMFIELDSBYISSUEID = "BugNet_ProjectCustomField_GetCustomFieldsByIssueId";
        private const string SP_CUSTOMFIELD_CREATE = "BugNet_ProjectCustomField_CreateNewCustomField";
        private const string SP_CUSTOMFIELD_UPDATE = "BugNet_ProjectCustomField_UpdateCustomField";
        private const string SP_CUSTOMFIELD_DELETE = "BugNet_ProjectCustomField_DeleteCustomField";
        private const string SP_CUSTOMFIELD_SAVECUSTOMFIELDVALUE = "BugNet_ProjectCustomField_SaveCustomFieldValue";

        private const string SP_CUSTOMFIELDSELECTION_CREATE = "BugNet_ProjectCustomFieldSelection_CreateNewCustomFieldSelection";
        private const string SP_CUSTOMFIELDSELECTION_DELETE = "BugNet_ProjectCustomFieldSelection_DeleteCustomFieldSelection";
        private const string SP_CUSTOMFIELDSELECTION_GETCUSTOMFIELDSELECTIONSBYCUSTOMFIELDID = "BugNet_ProjectCustomFieldSelection_GetCustomFieldSelectionsByCustomFieldId";
        private const string SP_CUSTOMFIELDSELECTION_GETCUSTOMFIELDSELECTIONBYID = "BugNet_ProjectCustomFieldSelection_GetCustomFieldSelectionById";
        private const string SP_CUSTOMFIELDSELECTION_UPDATE = "BugNet_ProjectCustomFieldSelection_Update";
        private const string SP_CUSTOMFIELDSELECTION_GETCUSTOMFIELDSELECTION = "BugNet_ProjectCustomFieldSelection_GetCustomFieldSelection";

        private const string SP_REQUIREDFIELDS_GETFIELDLIST = "BugNet_RequiredField_GetRequiredFieldListForIssues";
        private const string SP_ISSUE_BYPROJECTIDANDCUSTOMFIELDVIEW = "BugNet_GetIssuesByProjectIdAndCustomFieldView";

        //String Resources
        private const string SP_STRINGRESOURCES_GETINSTALLEDLANGUAGERESOURCES = "BugNet_StringResources_GetInstalledLanguageResources";
        #endregion

        /*** INSTANCE PROPERTIES ***/

        /// <summary>
        /// Gets a value indicating whether [supports project cloning].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [supports project cloning]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsProjectCloning
        {
            get { return true; }
        }

        /// <summary>
        /// Creates the new category.
        /// </summary>
        /// <param name="newCategory">The new category.</param>
        /// <returns></returns>
        public override int CreateNewCategory(BugNET.BusinessLogicLayer.Category newCategory)
        {
            if (newCategory == null)
                throw (new ArgumentNullException("newCategory"));

            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newCategory.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@CategoryName", SqlDbType.NText, 255, ParameterDirection.Input, newCategory.Name);
            AddParamToSQLCmd(sqlCmd, "@ParentCategoryId", SqlDbType.Int, 0, ParameterDirection.Input, newCategory.ParentCategoryId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_CREATE);
            ExecuteScalarCmd(sqlCmd);

            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public override bool DeleteCategory(int categoryId)
        {
            // Validate Parameters
            if (categoryId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("categoryId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 4, ParameterDirection.Input, categoryId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="categoryToUpdate">The category to update.</param>
        /// <returns></returns>
        public override bool UpdateCategory(BugNET.BusinessLogicLayer.Category categoryToUpdate)
        {
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, categoryToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, categoryToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@CategoryName", SqlDbType.NText, 255, ParameterDirection.Input, categoryToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@ParentCategoryId", SqlDbType.Int, 0, ParameterDirection.Input, categoryToUpdate.ParentCategoryId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_UPDATE);
            ExecuteScalarCmd(sqlCmd);

            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Gets the categories by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Category> GetCategoriesByProjectId(int projectId)
        {
            // Validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETCATEGORIESBYPROJECTID);
            List<Category> categoryList = new List<Category>();
            TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);

            return categoryList;
        }

        /// <summary>
        /// Gets the root categories by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Category> GetRootCategoriesByProjectId(int projectId)
        {
            // Validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETROOTCATEGORIESBYPROJECTID);
            List<Category> categoryList = new List<Category>();
            TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);

            return categoryList;
        }

        /// <summary>
        /// Gets the child categories by category id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Category> GetChildCategoriesByCategoryId(int categoryId)
        {
            // Validate Parameters
            if (categoryId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("categoryId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, categoryId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETCHILDCATEGORIESBYCATEGORYID);
            List<Category> categoryList = new List<Category>();
            TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);

            return categoryList;
        }

        /// <summary>
        /// Gets the category by id.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public override Category GetCategoryById(int categoryId)
        {
            // Validate Parameters
            if (categoryId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("categoryId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, categoryId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETCATEGORYBYID);
            List<Category> categoryList = new List<Category>();
            TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);

            if (categoryList.Count > 0)
                return categoryList[0];
            else
                return null;
        }

        /// <summary>
        /// Deletes the issue.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override bool DeleteIssue(int issueId)
        {
            if (issueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("ssueId"));
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_DELETE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Ported to 0.8.99 by Stewart Moss
        /// 29-Feb-2009
        /// Gets the issues by Criteria. After a post on the forum, I am using another method to search.
        /// </summary>        
        /// <param name="projectId">The project id.</param>
        /// <param name="componentId">The component id.</param>
        /// <param name="versionId">The version id.</param>
        /// <param name="typeId">The type id.</param>
        /// <param name="priorityId">The priority id.</param>
        /// <param name="statusId">The status id.</param>
        /// <param name="assignedToUserName">Name of the assigned to user.</param>
        /// <param name="resolutionId">The resolution id.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="excludeClosed">if set to <c>true</c> [exclude closed].</param>
        /// <param name="reporterUserName">Name of the reporter user.</param>
        /// <param name="fixedInVersionId">The fixed in version id.</param>
        /// <returns></returns>
        //public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> GetIssuesByCriteria(int projectId, int componentId, int versionId, int typeId,
        //            int priorityId, int statusId, string assignedToUserName,
        //            int resolutionId, string keywords, bool excludeClosed, string reporterUserName, int fixedInVersionId)
        //{
           // if (projectId <= 0)
           // throw (new NotImplementedException("GetIssuesByCriteria"));

            //SqlCommand sqlCmd = new SqlCommand();            

            //////always add project id            
            //AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            //////optional parameters
            //if (componentId != -1)
            //    AddParamToSQLCmd(sqlCmd, "@ComponentId", SqlDbType.Int, 0, ParameterDirection.Input, componentId);
            //if (versionId != -1)
            //    AddParamToSQLCmd(sqlCmd, "@VersionId", SqlDbType.Int, 0, ParameterDirection.Input, versionId);
            //if (fixedInVersionId != -1)
            //    AddParamToSQLCmd(sqlCmd, "@FixedInVersionId", SqlDbType.Int, 0, ParameterDirection.Input, fixedInVersionId);
            //if (typeId != 0)
            //    AddParamToSQLCmd(sqlCmd, "@TypeId", SqlDbType.Int,0, ParameterDirection.Input, typeId);
            //if (priorityId != 0)
            //    AddParamToSQLCmd(sqlCmd, "@PriorityId", SqlDbType.Int, 0, ParameterDirection.Input, priorityId);
            ////if status is 0 , search for all active issues
            //if (statusId != -1)
            //    AddParamToSQLCmd(sqlCmd, "@StatusId", SqlDbType.Int, 0, ParameterDirection.Input, statusId);
            //if (!String.IsNullOrEmpty(assignedToUserName))
            //    AddParamToSQLCmd(sqlCmd, "@AssignedToUserName", SqlDbType.VarChar , 0, ParameterDirection.Input, assignedToUserName);
            //if (!String.IsNullOrEmpty(reporterUserName))
            //    AddParamToSQLCmd(sqlCmd, "@ReporterUserName", SqlDbType.VarChar , 0, ParameterDirection.Input, reporterUserName);
            //if (resolutionId != 0)
            //    AddParamToSQLCmd(sqlCmd, "@ResolutionId", SqlDbType.Int, 0, ParameterDirection.Input, resolutionId);
            //if (keywords.Length != 0 && !keywords.Trim().Equals(string.Empty))
            //    AddParamToSQLCmd(sqlCmd, "@Keywords", SqlDbType.VarChar, 0, ParameterDirection.Input, keywords);


            //SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUESBYCRITERIA);

            //List<Issue> issueList = new List<Issue>();
            //TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            //return issueList;

        //}

        /// <summary>
        /// Gets the issues by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> GetIssuesByProjectId(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUESBYPROJECTID);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            return issueList;
        }

        /// <summary>
        /// Gets the issue by id.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.Issue GetIssueById(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUEBYID);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            if (issueList.Count > 0)
                return issueList[0];
            else
                return null;
        }

        /// <summary>
        /// Updates the issue.
        /// </summary>
        /// <param name="issueToUpdate">The issue to update.</param>
        /// <returns></returns>
        public override bool UpdateIssue(BugNET.BusinessLogicLayer.Issue issueToUpdate)
        {
            if (issueToUpdate == null)
                throw (new ArgumentNullException("issueToUpdate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, issueToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@IssueTitle", SqlDbType.NVarChar, 500, ParameterDirection.Input, issueToUpdate.Title);
            AddParamToSQLCmd(sqlCmd, "@IssueCategoryId", SqlDbType.Int, 0, ParameterDirection.Input, (issueToUpdate.CategoryId == 0) ? DBNull.Value : (object)issueToUpdate.CategoryId);
            AddParamToSQLCmd(sqlCmd, "@IssueStatusId", SqlDbType.Int, 0, ParameterDirection.Input, issueToUpdate.StatusId);
            AddParamToSQLCmd(sqlCmd, "@IssuePriorityId", SqlDbType.Int, 0, ParameterDirection.Input, issueToUpdate.PriorityId);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeId", SqlDbType.Int, 0, ParameterDirection.Input, issueToUpdate.IssueTypeId);
            AddParamToSQLCmd(sqlCmd, "@IssueResolutionId", SqlDbType.Int, 0, ParameterDirection.Input, (issueToUpdate.ResolutionId == 0) ? DBNull.Value : (object)issueToUpdate.ResolutionId);
            AddParamToSQLCmd(sqlCmd, "@IssueMilestoneId", SqlDbType.Int, 0, ParameterDirection.Input, (issueToUpdate.MilestoneId == 0) ? DBNull.Value : (object)issueToUpdate.MilestoneId);
            AddParamToSQLCmd(sqlCmd, "@IssueAffectedMilestoneId", SqlDbType.Int, 0, ParameterDirection.Input, (issueToUpdate.AffectedMilestoneId == 0) ? DBNull.Value : (object)issueToUpdate.AffectedMilestoneId);
            AddParamToSQLCmd(sqlCmd, "@IssueAssignedUserName", SqlDbType.NText, 255, ParameterDirection.Input, (issueToUpdate.AssignedUserName == string.Empty) ? DBNull.Value : (object)issueToUpdate.AssignedUserName);
            AddParamToSQLCmd(sqlCmd, "@IssueOwnerUserName", SqlDbType.NText, 255, ParameterDirection.Input, (issueToUpdate.OwnerUserName == string.Empty) ? DBNull.Value : (object)issueToUpdate.OwnerUserName);
            AddParamToSQLCmd(sqlCmd, "@IssueCreatorUserName", SqlDbType.NText, 255, ParameterDirection.Input, issueToUpdate.CreatorUserName);
            AddParamToSQLCmd(sqlCmd, "@IssueDueDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (issueToUpdate.DueDate == DateTime.MinValue) ? DBNull.Value : (object)issueToUpdate.DueDate);
            AddParamToSQLCmd(sqlCmd, "@IssueEstimation", SqlDbType.Decimal, 0, ParameterDirection.Input, issueToUpdate.Estimation);
            AddParamToSQLCmd(sqlCmd, "@IssueVisibility", SqlDbType.Bit, 0, ParameterDirection.Input, issueToUpdate.Visibility);
            AddParamToSQLCmd(sqlCmd, "@IssueDescription", SqlDbType.NText, 0, ParameterDirection.Input, issueToUpdate.Description);
            AddParamToSQLCmd(sqlCmd, "@IssueProgress", SqlDbType.Int, 0, ParameterDirection.Input, issueToUpdate.Progress);
            AddParamToSQLCmd(sqlCmd, "@LastUpdateUserName", SqlDbType.NText, 255, ParameterDirection.Input, issueToUpdate.LastUpdateUserName);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Creates the new issue.
        /// </summary>
        /// <param name="issueToCreate">The issue to create.</param>
        /// <returns></returns>
        public override int CreateNewIssue(BugNET.BusinessLogicLayer.Issue newIssue)
        {
            // Validate Parameters
            if (newIssue == null)
                throw (new ArgumentNullException("newIssue"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newIssue.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@IssueTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, newIssue.Title);
            AddParamToSQLCmd(sqlCmd, "@IssueDescription", SqlDbType.NVarChar, 0, ParameterDirection.Input, newIssue.Description);
            AddParamToSQLCmd(sqlCmd, "@IssueCategoryId", SqlDbType.Int, 0, ParameterDirection.Input, (newIssue.CategoryId == 0) ? DBNull.Value : (object)newIssue.CategoryId);
            AddParamToSQLCmd(sqlCmd, "@IssueStatusId", SqlDbType.Int, 0, ParameterDirection.Input, newIssue.StatusId);
            AddParamToSQLCmd(sqlCmd, "@IssuePriorityId", SqlDbType.Int, 0, ParameterDirection.Input, newIssue.PriorityId);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeId", SqlDbType.Int, 0, ParameterDirection.Input, newIssue.IssueTypeId);
            AddParamToSQLCmd(sqlCmd, "@IssueResolutionId", SqlDbType.Int, 0, ParameterDirection.Input, (newIssue.ResolutionId == 0) ? DBNull.Value : (object)newIssue.ResolutionId);
            AddParamToSQLCmd(sqlCmd, "@IssueMilestoneId", SqlDbType.Int, 0, ParameterDirection.Input, (newIssue.MilestoneId == 0) ? DBNull.Value : (object)newIssue.MilestoneId);
            AddParamToSQLCmd(sqlCmd, "@IssueAffectedMilestoneId", SqlDbType.Int, 0, ParameterDirection.Input, (newIssue.AffectedMilestoneId == 0) ? DBNull.Value : (object)newIssue.AffectedMilestoneId);
            AddParamToSQLCmd(sqlCmd, "@IssueAssignedUserName", SqlDbType.NText, 255, ParameterDirection.Input, (newIssue.AssignedUserName == string.Empty) ? DBNull.Value : (object)newIssue.AssignedUserName);
            AddParamToSQLCmd(sqlCmd, "@IssueOwnerUserName", SqlDbType.NText, 255, ParameterDirection.Input, (newIssue.OwnerUserName == string.Empty) ? DBNull.Value : (object)newIssue.OwnerUserName);
            AddParamToSQLCmd(sqlCmd, "@IssueCreatorUserName", SqlDbType.NText, 255, ParameterDirection.Input, newIssue.CreatorUserName);
            AddParamToSQLCmd(sqlCmd, "@IssueDueDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (newIssue.DueDate == DateTime.MinValue) ? DBNull.Value : (object)newIssue.DueDate);
            AddParamToSQLCmd(sqlCmd, "@IssueEstimation", SqlDbType.Decimal, 0, ParameterDirection.Input, newIssue.Estimation);
            AddParamToSQLCmd(sqlCmd, "@IssueVisibility", SqlDbType.Bit, 0, ParameterDirection.Input, newIssue.Visibility);
            AddParamToSQLCmd(sqlCmd, "@IssueProgress", SqlDbType.Int, 0, ParameterDirection.Input, newIssue.Progress);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Gets the issues by relevancy.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="userName">The userName.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> GetIssuesByRelevancy(int projectId, string userName)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUESBYRELEVANCY);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            return issueList;
        }

        /// <summary>
        /// Gets the name of the issues by assigned user.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="assignedUserName">Name of the assigned user.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> GetIssuesByAssignedUserName(int projectId, string userName)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUESBYASSIGNEDUSERNAME);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            return issueList;
        }

        /// <summary>
        /// Gets the open issues.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> GetOpenIssues(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETOPENISSUES);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            return issueList;
        }

        /// <summary>
        /// Gets the name of the issues by creator user.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> GetIssuesByCreatorUserName(int projectId, string userName)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUESBYCREATORUSERNAME);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            return issueList;
        }

        /// <summary>
        /// Gets the name of the issues by owner user.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> GetIssuesByOwnerUserName(int projectId, string userName)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUESBYOWNERUSERNAME);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            return issueList;
        }

        /// <summary>
        /// Gets the name of the monitored issues by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="excludeClosedStatus">if set to <c>true</c> [exclude closed status].</param>
        /// <returns></returns>
        public override List<Issue> GetMonitoredIssuesByUserName(string userName, bool excludeClosedStatus)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ExcludeClosedStatus", SqlDbType.Bit, 0, ParameterDirection.Input, excludeClosedStatus);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETMONITOREDISSUESBYUSERNAME);

            List<Issue> issueList = new List<Issue>();
            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);

            return issueList;
        }

        /// <summary>
        /// Performs the saved query.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryId">The query id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Issue> PerformSavedQuery(int projectId, int queryId)
        {
            // Validate Parameters
            if (queryId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("queryId"));

            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Get Query Clauses
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@QueryId", SqlDbType.Int, 0, ParameterDirection.Input, queryId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_QUERY_GETSAVEDQUERY);

            List<QueryClause> queryClauses = new List<QueryClause>();
            TExecuteReaderCmd<QueryClause>(sqlCmd, TGenerateQueryClauseListFromReader<QueryClause>, ref queryClauses);
            
            return PerformQuery(projectId, queryClauses);
        }

        /// <summary>
        /// Gets the child issues.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.RelatedIssue> GetChildIssues(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.ParentChild);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_GETCHILDISSUES);

            List<RelatedIssue> relatedIssueList = new List<RelatedIssue>();
            TExecuteReaderCmd<RelatedIssue>(sqlCmd, TGenerateRelatedIssueListFromReader<RelatedIssue>, ref relatedIssueList);

            return relatedIssueList;
        }

        /// <summary>
        /// Gets the parent issues.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.RelatedIssue> GetParentIssues(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.ParentChild);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_GETPARENTISSUES);

            List<RelatedIssue> relatedIssueList = new List<RelatedIssue>();
            TExecuteReaderCmd<RelatedIssue>(sqlCmd, TGenerateRelatedIssueListFromReader<RelatedIssue>, ref relatedIssueList);

            return relatedIssueList;
        }

        /// <summary>
        /// Gets the related issues.
        /// </summary>
        /// <param name="IssueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.RelatedIssue> GetRelatedIssues(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
                AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.Related);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_GETRELATEDISSUES);

                List<RelatedIssue> relatedIssueList = new List<RelatedIssue>();
                TExecuteReaderCmd<RelatedIssue>(sqlCmd, TGenerateRelatedIssueListFromReader<RelatedIssue>, ref relatedIssueList);

                return relatedIssueList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Creates the new child issue.
        /// </summary>
        /// <param name="primaryIssueId">The primary issue id.</param>
        /// <param name="secondaryIssueId">The secondary issue id.</param>
        /// <returns></returns>
        public override int CreateNewChildIssue(int primaryIssueId, int secondaryIssueId)
        {
            // Validate Parameters
            if (primaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("primaryIssueId"));
            if (secondaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("secondaryIssueId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@PrimaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, primaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@SecondaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, secondaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.ParentChild);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_CREATENEWCHILDISSUE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the child issue.
        /// </summary>
        /// <param name="primaryIssueId">The primary issue id.</param>
        /// <param name="secondaryIssueId">The secondary issue id.</param>
        /// <returns></returns>
        public override bool DeleteChildIssue(int primaryIssueId, int secondaryIssueId)
        {
            // Validate Parameters
            if (primaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("primaryIssueId"));
            if (secondaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("secondaryIssueId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@PrimaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, primaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@SecondaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, secondaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.ParentChild);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_DELETECHILDISSUE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Creates the new parent issue.
        /// </summary>
        /// <param name="primaryIssueId">The primary issue id.</param>
        /// <param name="secondaryIssueId">The secondary issue id.</param>
        /// <returns></returns>
        public override int CreateNewParentIssue(int primaryIssueId, int secondaryIssueId)
        {
            // Validate Parameters
            if (primaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("primaryIssueId"));
            if (secondaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("secondaryIssueId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@PrimaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, primaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@SecondaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, secondaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.ParentChild);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_CREATENEWPARENTISSUE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the parent issue.
        /// </summary>
        /// <param name="primaryIssueId">The primary issue id.</param>
        /// <param name="secondaryIssueId">The secondary issue id.</param>
        /// <returns></returns>
        public override bool DeleteParentIssue(int primaryIssueId, int secondaryIssueId)
        {
            // Validate Parameters
            if (primaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("primaryIssueId"));
            if (secondaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("secondaryIssueId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@PrimaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, primaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@SecondaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, secondaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.ParentChild);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_DELETEPARENTISSUE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Creates the new related issue.
        /// </summary>
        /// <param name="primaryIssueId">The primary issue id.</param>
        /// <param name="secondaryIssueId">The secondary issue id.</param>
        /// <returns></returns>
        public override int CreateNewRelatedIssue(int primaryIssueId, int secondaryIssueId)
        {
            // Validate Parameters
            if (primaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("primaryIssueId"));
            if (secondaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("secondaryIssueId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@PrimaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, primaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@SecondaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, secondaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.Related);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_CREATENEWRELATEDISSUE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the related issue.
        /// </summary>
        /// <param name="primaryIssueId">The primary issue id.</param>
        /// <param name="secondaryIssueId">The secondary issue id.</param>
        /// <returns></returns>
        public override bool DeleteRelatedIssue(int primaryIssueId, int secondaryIssueId)
        {
            // Validate Parameters
            if (primaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("primaryIssueId"));

            if (secondaryIssueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("secondaryIssueId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@PrimaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, primaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@SecondaryIssueId", SqlDbType.Int, 0, ParameterDirection.Input, secondaryIssueId);
                AddParamToSQLCmd(sqlCmd, "@RelationType", SqlDbType.Int, 0, ParameterDirection.Input, IssueRelationType.Related);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RELATEDISSUE_DELETERELATEDISSUE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the query.
        /// </summary>
        /// <param name="queryId">The query id.</param>
        /// <returns></returns>
        public override bool DeleteQuery(int queryId)
        {
            if (queryId <= 0)
                throw new ArgumentOutOfRangeException("queryId");
            try
            {
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@QueryId", SqlDbType.Int, 0, ParameterDirection.Input, queryId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_QUERY_DELETEQUERY);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the query clauses by query id.
        /// </summary>
        /// <returns></returns>
        public override List<QueryClause> GetQueryClausesByQueryId(int queryId)
        {
            // Get Query Clauses
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@QueryId", SqlDbType.Int, 0, ParameterDirection.Input, queryId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_QUERY_GETSAVEDQUERY);

            List<QueryClause> queryClauses = new List<QueryClause>();
            TExecuteReaderCmd<QueryClause>(sqlCmd, TGenerateQueryClauseListFromReader<QueryClause>, ref queryClauses);
            return queryClauses;
        }

        /// <summary>
        /// Gets the query by id.
        /// </summary>
        /// <param name="queryId">The query id.</param>
        /// <returns></returns>
        public override Query GetQueryById(int queryId)
        {
            // Validate Parameters
            if (queryId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("queryId"));

            // Get Query Clauses
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@QueryId", SqlDbType.Int, 0, ParameterDirection.Input, queryId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_QUERY_GETQUERYBYID);

            List<Query> queryList = new List<Query>();
            TExecuteReaderCmd<Query>(sqlCmd, TGenerateQueryListFromReader<Query>, ref queryList);

            Query q = queryList[0];
           
            // Get Query Clauses
            sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@QueryId", SqlDbType.Int, 0, ParameterDirection.Input, queryId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_QUERY_GETSAVEDQUERY);

            List<QueryClause> queryClauses = new List<QueryClause>();
            TExecuteReaderCmd<QueryClause>(sqlCmd, TGenerateQueryClauseListFromReader<QueryClause>, ref queryClauses);
            q.Clauses = queryClauses;

            return q;
        }

        /// <summary>
        /// Creates the new issue comment.
        /// </summary>
        /// <param name="newComment">The new comment.</param>
        /// <returns></returns>
        public override int CreateNewIssueComment(BugNET.BusinessLogicLayer.IssueComment newComment)
        {
            // Validate Parameters
            if (newComment == null)
                throw (new ArgumentNullException("newComment"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, newComment.IssueId);
            AddParamToSQLCmd(sqlCmd, "@CreatorUserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, newComment.CreatorUserName);
            AddParamToSQLCmd(sqlCmd, "@Comment", SqlDbType.NText, 0, ParameterDirection.Input, newComment.Comment);


            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUECOMMENT_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Gets the issue comments by issue id.
        /// </summary>
        /// <param name="IssueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.IssueComment> GetIssueCommentsByIssueId(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUECOMMENT_GETISSUECOMMENTSBYISSUEID);

                List<IssueComment> issueCommentList = new List<IssueComment>();
                TExecuteReaderCmd<IssueComment>(sqlCmd, TGenerateIssueCommentListFromReader<IssueComment>, ref issueCommentList);

                return issueCommentList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue comment by id.
        /// </summary>
        /// <param name="issueCommentId">The issue comment id.</param>
        /// <returns></returns>
        public override IssueComment GetIssueCommentById(int issueCommentId)
        {
            if (issueCommentId <= 0)
                throw (new ArgumentOutOfRangeException("issueCommentId"));

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@IssueCommentId", SqlDbType.Int, 0, ParameterDirection.Input, issueCommentId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUECOMMENT_GETISSUECOMMENTBYID);

                List<IssueComment> issueCommentList = new List<IssueComment>();
                TExecuteReaderCmd<IssueComment>(sqlCmd, TGenerateIssueCommentListFromReader<IssueComment>, ref issueCommentList);

                if (issueCommentList.Count > 0)
                    return issueCommentList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Updates the issue comment.
        /// </summary>
        /// <param name="issueCommentToUpdate">The issue comment to update.</param>
        /// <returns></returns>
        public override bool UpdateIssueComment(IssueComment issueCommentToUpdate)
        {
            // Validate Parameters
            if (issueCommentToUpdate == null)
                throw (new ArgumentNullException("issueCommentToUpdate"));
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueCommentId", SqlDbType.Int, 0, ParameterDirection.Input, issueCommentToUpdate.Id);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueCommentToUpdate.IssueId);
                AddParamToSQLCmd(sqlCmd, "@CreatorUserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, issueCommentToUpdate.CreatorUserName);
                AddParamToSQLCmd(sqlCmd, "@Comment", SqlDbType.NText, 0, ParameterDirection.Input, issueCommentToUpdate.Comment);


                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUECOMMENT_UPDATE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the issue comment by id.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <returns></returns>
        public override bool DeleteIssueCommentById(int commentId)
        {
            if (commentId <= 0)
                throw new ArgumentOutOfRangeException("commentId");
            try
            {
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueCommentId", SqlDbType.Int, 0, ParameterDirection.Input, commentId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUECOMMENT_DELETE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Creates the new issue attachment.
        /// </summary>
        /// <param name="newAttachment">The new attachment.</param>
        /// <returns></returns>
        public override int CreateNewIssueAttachment(BugNET.BusinessLogicLayer.IssueAttachment newAttachment)
        {
            // Validate Parameters
            if (newAttachment == null)
                throw (new ArgumentNullException("newAttachment"));
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, newAttachment.IssueId);
                AddParamToSQLCmd(sqlCmd, "@CreatorUserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, newAttachment.CreatorUserName);
                AddParamToSQLCmd(sqlCmd, "@FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input, newAttachment.FileName);
                AddParamToSQLCmd(sqlCmd, "@FileSize", SqlDbType.Int, 0, ParameterDirection.Input, newAttachment.Size);
                AddParamToSQLCmd(sqlCmd, "@ContentType", SqlDbType.NVarChar, 80, ParameterDirection.Input, newAttachment.ContentType);
                AddParamToSQLCmd(sqlCmd, "@Description", SqlDbType.NVarChar, 80, ParameterDirection.Input, newAttachment.Description);
                if(newAttachment.Attachment != null)
                    AddParamToSQLCmd(sqlCmd, "@Attachment", SqlDbType.Image, newAttachment.Attachment.Length, ParameterDirection.Input, newAttachment.Attachment);
                else
                    AddParamToSQLCmd(sqlCmd, "@Attachment", SqlDbType.Image, 0, ParameterDirection.Input, DBNull.Value);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEATTACHMENT_CREATE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue attachments by issue id.
        /// </summary>
        /// <param name="IssueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.IssueAttachment> GetIssueAttachmentsByIssueId(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEATTACHMENT_GETATTACHMENTSBYISSUEID);

            List<IssueAttachment> issueAttachmentList = new List<IssueAttachment>();
            TExecuteReaderCmd<IssueAttachment>(sqlCmd, TGenerateIssueAttachmentListFromReader<IssueAttachment>, ref issueAttachmentList);

            return issueAttachmentList;
        }

        /// <summary>
        /// Gets the issue attachment by id.
        /// </summary>
        /// <param name="attachmentId">The attachment id.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.IssueAttachment GetIssueAttachmentById(int attachmentId)
        {
            if (attachmentId <= 0)
                throw (new ArgumentOutOfRangeException("attachmentId"));

            IssueAttachment attachment = null;

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@IssueAttachmentId", SqlDbType.Int, 0, ParameterDirection.Input, attachmentId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEATTACHMENT_GETATTACHMENTBYID);

                // Execute Reader
                if (_connectionString == string.Empty)
                    throw (new ArgumentOutOfRangeException("_connectionString"));


                using (SqlConnection cn = new SqlConnection(this._connectionString))
                {
                    sqlCmd.Connection = cn;
                    cn.Open();
                    SqlDataReader dtr = sqlCmd.ExecuteReader();
                    if (dtr.Read())
                    {
                        byte[] attachmentData = null;
                        if (dtr["Attachment"] != DBNull.Value)
                            attachmentData = (byte[])dtr["Attachment"];

                        attachment = new IssueAttachment((int)dtr["IssueId"], (string)dtr["CreatorUserName"], (string)dtr["FileName"], (string)dtr["ContentType"], attachmentData, (int)dtr["FileSize"], (string)dtr["Description"]);
                    }

                }

                return attachment;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the issue attachment.
        /// </summary>
        /// <param name="attachmentId">The attachment id.</param>
        /// <returns></returns>
        public override bool DeleteIssueAttachment(int attachmentId)
        {
            if (attachmentId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("attachmentId"));
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueAttachmentId", SqlDbType.Int, 0, ParameterDirection.Input, attachmentId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEATTACHMENT_DELETEATTACHMENT);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue history by issue id.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.IssueHistory> GetIssueHistoryByIssueId(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEHISTORY_GETISSUEHISTORYBYISSUEID);

            List<IssueHistory> issueHistoryList = new List<IssueHistory>();
            TExecuteReaderCmd<IssueHistory>(sqlCmd, TGenerateIssueHistoryListFromReader<IssueHistory>, ref issueHistoryList);

            return issueHistoryList;
        }

        /// <summary>
        /// Creates the new issue history.
        /// </summary>
        /// <param name="newHistory">The new history.</param>
        /// <returns></returns>
        public override int CreateNewIssueHistory(IssueHistory newHistory)
        {
            // Validate Parameters
            if (newHistory == null)
                throw (new ArgumentNullException("newHistory"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, newHistory.IssueId);
            AddParamToSQLCmd(sqlCmd, "@CreatedUserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, newHistory.CreatedUserName);
            AddParamToSQLCmd(sqlCmd, "@FieldChanged", SqlDbType.NVarChar, 50, ParameterDirection.Input, newHistory.FieldChanged);
            AddParamToSQLCmd(sqlCmd, "@OldValue", SqlDbType.NVarChar, 50, ParameterDirection.Input, newHistory.OldValue);
            AddParamToSQLCmd(sqlCmd, "@NewValue", SqlDbType.NVarChar, 50, ParameterDirection.Input, newHistory.NewValue);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEHISTORY_CREATENEWISSUEHISTORY);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Creates the new issue notification.
        /// </summary>
        /// <param name="newNotification">The new notification.</param>
        /// <returns></returns>
        public override int CreateNewIssueNotification(BugNET.BusinessLogicLayer.IssueNotification newNotification)
        {
            // Validate Parameters
            if (newNotification == null)
                throw (new ArgumentNullException("newNotification"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, newNotification.IssueId);
                AddParamToSQLCmd(sqlCmd, "@NotificationUserName", SqlDbType.NText, 255, ParameterDirection.Input, newNotification.NotificationUsername);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUENOTIFICATION_CREATE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue notifications by issue id.
        /// </summary>
        /// <param name="IssueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.IssueNotification> GetIssueNotificationsByIssueId(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUENOTIFICATION_GETISSUENOTIFICATIONSBYISSUEID);

            List<IssueNotification> issueNotificationList = new List<IssueNotification>();
            TExecuteReaderCmd<IssueNotification>(sqlCmd, TGenerateIssueNotificationListFromReader<IssueNotification>, ref issueNotificationList);

            return issueNotificationList;
        }

        /// <summary>
        /// Deletes the issue notification.
        /// </summary>
        /// <param name="IssueId">The issue id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override bool DeleteIssueNotification(int issueId, string userName)
        {
            // Validate Parameters
            if (issueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("issueId"));
            if (userName == String.Empty)
                throw (new ArgumentOutOfRangeException("userName"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
                AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUENOTIFICATION_DELETE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Creates the new milestone.
        /// </summary>
        /// <param name="newMileStone">The new mile stone.</param>
        /// <returns></returns>
        public override int CreateNewMilestone(BugNET.BusinessLogicLayer.Milestone newMileStone)
        {
            // Validate Parameters
            if (newMileStone == null)
                throw (new ArgumentNullException("newMileStone"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newMileStone.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@MilestoneName", SqlDbType.NText, 50, ParameterDirection.Input, newMileStone.Name);
            AddParamToSQLCmd(sqlCmd, "@MilestoneImageUrl", SqlDbType.NText, 255, ParameterDirection.Input, newMileStone.ImageUrl);
            AddParamToSQLCmd(sqlCmd, "@MilestoneCompleted", SqlDbType.Bit, 0, ParameterDirection.Input, newMileStone.IsCompleted);
            AddParamToSQLCmd(sqlCmd, "@MilestoneNotes", SqlDbType.NText, 255, ParameterDirection.Input, newMileStone.Notes);

            //Bypass to AddParamToSQLCmd method which doesn't handle null values properly
            if (newMileStone.DueDate.HasValue)
                AddParamToSQLCmd(sqlCmd, "@MilestoneDueDate", SqlDbType.DateTime, 0, ParameterDirection.Input, newMileStone.DueDate);
            else
                sqlCmd.Parameters.AddWithValue("@MilestoneDueDate", DBNull.Value);

            if (newMileStone.ReleaseDate.HasValue)
                AddParamToSQLCmd(sqlCmd, "@MilestoneReleaseDate", SqlDbType.DateTime, 0, ParameterDirection.Input, newMileStone.ReleaseDate);
            else
                sqlCmd.Parameters.AddWithValue("@MilestoneReleaseDate", DBNull.Value);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_MILESTONE_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the milestone.
        /// </summary>
        /// <param name="milestoneId">The milestone id.</param>
        /// <returns></returns>
        public override bool DeleteMilestone(int milestoneId)
        {
            // Validate Parameters
            if (milestoneId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("milestoneId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@MilestoneIdToDelete", SqlDbType.Int, 4, ParameterDirection.Input, milestoneId);
            AddParamToSQLCmd(sqlCmd, "@ResultValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_MILESTONE_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int resultValue = (int)sqlCmd.Parameters["@ResultValue"].Value;
            return (resultValue == 0 ? true : false);
        }

        /// <summary>
        /// Gets the milestones by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="milestoneCompleted">if set to <c>true</c> [milestone completed].</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Milestone> GetMilestonesByProjectId(int projectId)
        {
            return GetMilestonesByProjectId(projectId, true);
        }


        /// <summary>
        /// Gets the milestones by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="milestoneCompleted">if set to <c>true</c> [milestone completed].</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Milestone> GetMilestonesByProjectId(int projectId, bool milestoneCompleted)
        {
            // Validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            AddParamToSQLCmd(sqlCmd, "@MilestoneCompleted", SqlDbType.Bit, 0, ParameterDirection.Input, milestoneCompleted);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_MILESTONE_GETMILESTONEBYPROJECTID);
            List<Milestone> milestoneList = new List<Milestone>();
            TExecuteReaderCmd<Milestone>(sqlCmd, TGenerateMilestoneListFromReader<Milestone>, ref milestoneList);

            return milestoneList;
        }

        /// <summary>
        /// Gets the milestone by id.
        /// </summary>
        /// <param name="milestoneId">The milestone id.</param>
        /// <returns></returns>
        public override Milestone GetMilestoneById(int milestoneId)
        {
            // Validate Parameters
            if (milestoneId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("milestoneId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@MilestoneId", SqlDbType.Int, 0, ParameterDirection.Input, milestoneId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_MILESTONE_GETMILESTONEBYID);
            List<Milestone> milestoneList = new List<Milestone>();
            TExecuteReaderCmd<Milestone>(sqlCmd, TGenerateMilestoneListFromReader<Milestone>, ref milestoneList);

            if (milestoneList.Count > 0)
                return milestoneList[0];
            else
                return null;
        }

        /// <summary>
        /// Updates the milestone.
        /// </summary>
        /// <param name="milestoneToUpdate">The milestone to update.</param>
        /// <returns></returns>
        public override bool UpdateMilestone(Milestone milestoneToUpdate)
        {
            // Validate Parameters
            if (milestoneToUpdate == null)
                throw (new ArgumentNullException("milestoneToUpdate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@MilestoneId", SqlDbType.Int, 0, ParameterDirection.Input, milestoneToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, milestoneToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@SortOrder", SqlDbType.Int, 0, ParameterDirection.Input, milestoneToUpdate.SortOrder);
            AddParamToSQLCmd(sqlCmd, "@MilestoneName", SqlDbType.NText, 50, ParameterDirection.Input, milestoneToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@MilestoneImageUrl", SqlDbType.NText, 50, ParameterDirection.Input, milestoneToUpdate.ImageUrl);
            AddParamToSQLCmd(sqlCmd, "@MilestoneCompleted", SqlDbType.Bit, 0, ParameterDirection.Input, milestoneToUpdate.IsCompleted);
            AddParamToSQLCmd(sqlCmd, "@MilestoneNotes", SqlDbType.NText, 255, ParameterDirection.Input, milestoneToUpdate.Notes);

            //Bypass to AddParamToSQLCmd method which doesn't handle null values properly
            if (milestoneToUpdate.DueDate.HasValue)
                AddParamToSQLCmd(sqlCmd, "@MilestoneDueDate", SqlDbType.DateTime, 0, ParameterDirection.Input, milestoneToUpdate.DueDate);
            else
                sqlCmd.Parameters.AddWithValue("@MilestoneDueDate", DBNull.Value);

            if (milestoneToUpdate.ReleaseDate.HasValue)
                AddParamToSQLCmd(sqlCmd, "@MilestoneReleaseDate", SqlDbType.DateTime, 0, ParameterDirection.Input, milestoneToUpdate.ReleaseDate);
            else
                sqlCmd.Parameters.AddWithValue("@MilestoneReleaseDate", DBNull.Value);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_MILESTONE_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Creates the new priority.
        /// </summary>
        /// <param name="newPriority">The new priority.</param>
        /// <returns></returns>
        public override int CreateNewPriority(BugNET.BusinessLogicLayer.Priority newPriority)
        {
            // Validate Parameters
            if (newPriority == null)
                throw (new ArgumentNullException("newPriority"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newPriority.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@PriorityName", SqlDbType.NText, 50, ParameterDirection.Input, newPriority.Name);
            AddParamToSQLCmd(sqlCmd, "@PriorityImageUrl", SqlDbType.NText, 50, ParameterDirection.Input, newPriority.ImageUrl);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PRIORITY_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the priority.
        /// </summary>
        /// <param name="PriorityId">The priority id.</param>
        /// <returns></returns>
        public override bool DeletePriority(int PriorityId)
        {
            // Validate Parameters
            if (PriorityId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("PriorityId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@PriorityIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, PriorityId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PRIORITY_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Gets the priorities by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Priority> GetPrioritiesByProjectId(int projectId)
        {
            // Validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PRIORITY_GETPRIORITIESBYPROJECTID);
            List<Priority> priorityList = new List<Priority>();
            TExecuteReaderCmd<Priority>(sqlCmd, TGeneratePriorityListFromReader<Priority>, ref priorityList);
            return priorityList;

        }

        /// <summary>
        /// Gets the priority by id.
        /// </summary>
        /// <param name="priorityId">The priority id.</param>
        /// <returns></returns>
        public override Priority GetPriorityById(int priorityId)
        {
            // Validate Parameters
            if (priorityId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("priorityId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@PriorityId", SqlDbType.Int, 0, ParameterDirection.Input, priorityId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PRIORITY_GETPRIORITYBYID);
            List<Priority> priorityList = new List<Priority>();
            TExecuteReaderCmd<Priority>(sqlCmd, TGeneratePriorityListFromReader<Priority>, ref priorityList);
            if (priorityList.Count > 0)
                return priorityList[0];
            else
                return null;
        }

        /// <summary>
        /// Updates the priority.
        /// </summary>
        /// <param name="priorityToUpdate">The priority to update.</param>
        /// <returns></returns>
        public override bool UpdatePriority(Priority priorityToUpdate)
        {
            // Validate Parameters
            if (priorityToUpdate == null)
                throw (new ArgumentNullException("priorityToUpdate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@PriorityId", SqlDbType.Int, 0, ParameterDirection.Input, priorityToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, priorityToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@SortOrder", SqlDbType.Int, 0, ParameterDirection.Input, priorityToUpdate.SortOrder);
            AddParamToSQLCmd(sqlCmd, "@PriorityName", SqlDbType.NText, 50, ParameterDirection.Input, priorityToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@PriorityImageUrl", SqlDbType.NText, 50, ParameterDirection.Input, priorityToUpdate.ImageUrl);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PRIORITY_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Creates the new project.
        /// </summary>
        /// <param name="newProject">The new project.</param>
        /// <returns></returns>
        public override int CreateNewProject(BugNET.BusinessLogicLayer.Project newProject)
        {
            // Validate Parameters
            if (newProject == null)
                throw (new ArgumentNullException("newProject"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@AllowAttachments", SqlDbType.Bit, 0, ParameterDirection.Input, newProject.AllowAttachments);
            //AddParamToSQLCmd(sqlCmd, "@Active", SqlDbType.Bit, 0, ParameterDirection.Input, newProject.Active);
            AddParamToSQLCmd(sqlCmd, "@ProjectName", SqlDbType.NText, 256, ParameterDirection.Input, newProject.Name);
            AddParamToSQLCmd(sqlCmd, "@ProjectDescription", SqlDbType.NText, 1000, ParameterDirection.Input, newProject.Description);
            AddParamToSQLCmd(sqlCmd, "@ProjectManagerUserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, newProject.ManagerUserName);
            AddParamToSQLCmd(sqlCmd, "@ProjectCreatorUserName", SqlDbType.NText, 255, ParameterDirection.Input, newProject.CreatorUserName);
            AddParamToSQLCmd(sqlCmd, "@ProjectAccessType", SqlDbType.Int, 0, ParameterDirection.Input, newProject.AccessType);
            AddParamToSQLCmd(sqlCmd, "@AttachmentUploadPath", SqlDbType.NVarChar, 80, ParameterDirection.Input, newProject.UploadPath);
            AddParamToSQLCmd(sqlCmd, "@ProjectCode", SqlDbType.NVarChar, 80, ParameterDirection.Input, newProject.Code);
            AddParamToSQLCmd(sqlCmd, "@AttachmentStorageType", SqlDbType.Int, 1, ParameterDirection.Input, newProject.AttachmentStorageType);
            AddParamToSQLCmd(sqlCmd, "@SvnRepositoryUrl", SqlDbType.NVarChar, 0, ParameterDirection.Input, newProject.SvnRepositoryUrl);
            AddParamToSQLCmd(sqlCmd, "@AllowIssueVoting", SqlDbType.Bit, 0, ParameterDirection.Input, newProject.AllowIssueVoting);
            if (newProject.Image != null)
            {
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileContent", SqlDbType.Binary, 0, ParameterDirection.Input, newProject.Image.ImageContent);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileName", SqlDbType.NVarChar, 150, ParameterDirection.Input, newProject.Image.ImageFileName);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageContentType", SqlDbType.NVarChar, 50, ParameterDirection.Input, newProject.Image.ImageContentType);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileSize", SqlDbType.BigInt, 0, ParameterDirection.Input, newProject.Image.ImageFileLength);
            }
            else
            {
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileContent", SqlDbType.Binary, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileName", SqlDbType.NVarChar, 150, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageContentType", SqlDbType.NVarChar, 50, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileSize", SqlDbType.BigInt, 0, ParameterDirection.Input, DBNull.Value);
            }

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override bool DeleteProject(int projectId)
        {
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectID"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Project> GetAllProjects()
        {
            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETALLPROJECTS);

            List<Project> projectList = new List<Project>();
            TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref projectList);

            return projectList;
        }

        /// <summary>
        /// Gets the project by id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.Project GetProjectById(int projectId)
        {
            if (projectId <= Globals.NewId)
                throw (new ArgumentNullException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETPROJECTBYID);

            List<Project> projectList = new List<Project>();
            TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref projectList);

            if (projectList.Count > 0)
                return projectList[0];
            else
                return null;
        }

        /// <summary>
        /// Gets the name of the projects by member user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Project> GetProjectsByMemberUserName(string userName)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));

            return GetProjectsByMemberUserName(userName, true);
        }

        /// <summary>
        /// Gets the projects by member userName.
        /// </summary>
        /// <param name="userName">The userName.</param>
        /// <param name="activeOnly">if set to <c>true</c> [active only].</param>
        /// <returns></returns>
        public override List<Project> GetProjectsByMemberUserName(string userName, bool activeOnly)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, userName);
                AddParamToSQLCmd(sqlCmd, "@ActiveOnly", SqlDbType.Bit, 0, ParameterDirection.Input, activeOnly);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETPROJECTSBYMEMBERUSERNAME);

                List<Project> projectList = new List<Project>();
                TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref projectList);

                return projectList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Updates the project.
        /// </summary>
        /// <param name="projectToUpdate">The project to update.</param>
        /// <returns></returns>
        public override bool UpdateProject(BugNET.BusinessLogicLayer.Project projectToUpdate)
        {
            if (projectToUpdate == null)
                throw (new ArgumentNullException("projectToUpdate"));

            if (projectToUpdate.Id <= 0)
                throw (new ArgumentOutOfRangeException("projectToUpdate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@AllowAttachments", SqlDbType.Bit, 0, ParameterDirection.Input, projectToUpdate.AllowAttachments);
            AddParamToSQLCmd(sqlCmd, "@ProjectDisabled", SqlDbType.Bit, 0, ParameterDirection.Input, projectToUpdate.Disabled);
            AddParamToSQLCmd(sqlCmd, "@ProjectName", SqlDbType.NText, 256, ParameterDirection.Input, projectToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@ProjectDescription", SqlDbType.NText, 1000, ParameterDirection.Input, projectToUpdate.Description);
            AddParamToSQLCmd(sqlCmd, "@ProjectManagerUserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, projectToUpdate.ManagerUserName);
            AddParamToSQLCmd(sqlCmd, "@ProjectAccessType", SqlDbType.Int, 0, ParameterDirection.Input, projectToUpdate.AccessType);
            AddParamToSQLCmd(sqlCmd, "@AttachmentUploadPath", SqlDbType.NVarChar, 80, ParameterDirection.Input, projectToUpdate.UploadPath);
            AddParamToSQLCmd(sqlCmd, "@ProjectCode", SqlDbType.NVarChar, 80, ParameterDirection.Input, projectToUpdate.Code);
            AddParamToSQLCmd(sqlCmd, "@AttachmentStorageType", SqlDbType.Int, 1, ParameterDirection.Input, projectToUpdate.AttachmentStorageType);
            AddParamToSQLCmd(sqlCmd, "@SvnRepositoryUrl", SqlDbType.NVarChar, 0, ParameterDirection.Input, projectToUpdate.SvnRepositoryUrl);
            AddParamToSQLCmd(sqlCmd, "@AllowIssueVoting", SqlDbType.Bit, 0, ParameterDirection.Input, projectToUpdate.AllowIssueVoting);
            if (projectToUpdate.Image == null)
            {
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileContent", SqlDbType.Binary, 0, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileName", SqlDbType.NVarChar, 150, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageContentType", SqlDbType.NVarChar, 50, ParameterDirection.Input, DBNull.Value);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileSize", SqlDbType.BigInt, 0, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileContent", SqlDbType.Binary, 0, ParameterDirection.Input, projectToUpdate.Image.ImageContent);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileName", SqlDbType.NVarChar, 150, ParameterDirection.Input, projectToUpdate.Image.ImageFileName);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageContentType", SqlDbType.NVarChar, 50, ParameterDirection.Input, projectToUpdate.Image.ImageContentType);
                AddParamToSQLCmd(sqlCmd, "@ProjectImageFileSize", SqlDbType.BigInt, 0, ParameterDirection.Input, projectToUpdate.Image.ImageFileLength);
            }
            
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Adds the user to project.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override bool AddUserToProject(string userName, int projectId)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NText, 256, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_ADDUSERTOPROJECT);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Removes the user from project.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override bool RemoveUserFromProject(string userName, int projectId)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NText, 256, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_REMOVEUSERFROMPROJECT);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Clones the project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <returns></returns>
        public override int CloneProject(int projectId, string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
                throw new ArgumentNullException("projectName");
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectName", SqlDbType.NText, 256, ParameterDirection.Input, projectName);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_CLONEPROJECT);
                ExecuteScalarCmd(sqlCmd);
                return (int)sqlCmd.Parameters["@ReturnValue"].Value;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the project by code.
        /// </summary>
        /// <param name="projectCode">The project code.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.Project GetProjectByCode(string projectCode)
        {
            if (projectCode == null)
                throw (new ArgumentNullException("projectCode"));
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@ProjectCode", SqlDbType.NVarChar, 0, ParameterDirection.Input, projectCode);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETPROJECTBYCODE);
                List<Project> projectList = new List<Project>();
                TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref projectList);
                if (projectList.Count > 0)
                    return projectList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the public projects.
        /// </summary>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Project> GetPublicProjects()
        {
            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETPUBLICPROJECTS);

            List<Project> projectList = new List<Project>();
            TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref projectList);

            return projectList;
        }

        /// <summary>
        /// Determines whether [is user project member] [the specified user name].
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>
        /// 	<c>true</c> if [is user project member] [the specified user name]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsUserProjectMember(string userName, int projectId)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));

            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, userName);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_ISUSERPROJECTMEMBER);
            ExecuteScalarCmd(sqlCmd);

            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }


        /// <summary>
        /// Gets the users by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<ITUser> GetUsersByProjectId(int projectId)
        {
            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_USER_GETUSERSBYPROJECTID);

            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            List<ITUser> userList = new List<ITUser>();
            TExecuteReaderCmd<ITUser>(sqlCmd, TGenerateUserListFromReader<ITUser>, ref userList);
            return userList;
        }


        /// <summary>
        /// Deletes the project image.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override bool DeleteProjectImage(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
                string commandText = "UPDATE BugNet_Projects SET ProjectImageFileContent = NULL, ProjectImageFileName = NULL, ProjectImageContentType = NULL, ProjectImageFileSize = NULL WHERE ProjectId = @ProjectId";

                SetCommandType(sqlCmd, CommandType.Text, commandText);
                int retVal;

                using (SqlConnection cn = new SqlConnection(this._connectionString))
                {
                    sqlCmd.Connection = cn;
                    cn.Open();
                    retVal = sqlCmd.ExecuteNonQuery(); 
                }

                return retVal == 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }


        /// <summary>
        /// Gets the project image by id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override ProjectImage GetProjectImageById(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
                sqlCmd.CommandText = "SELECT [ProjectId], [ProjectImageFileContent],[ProjectImageFileName],[ProjectImageContentType],[ProjectImageFileSize] FROM BugNet_Projects WHERE ProjectId = @ProjectId";

                ProjectImage projectImage = null;

                using (SqlConnection cn = new SqlConnection(this._connectionString))
                {
                    sqlCmd.Connection = cn;
                    cn.Open();
                    SqlDataReader dtr = sqlCmd.ExecuteReader();
                    if (dtr.Read())
                    {
                        byte[] attachmentData = null;
                        if (dtr["ProjectImageFileContent"] != DBNull.Value)
                            attachmentData = (byte[])dtr["ProjectImageFileContent"];
                        else
                            return null;

                        projectImage = new ProjectImage((int)dtr["ProjectId"], attachmentData, (string)dtr["ProjectImageFileName"], (long)dtr["ProjectImageFileSize"], (string)dtr["ProjectImageContentType"]);

                    }

                }

                return projectImage;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the project by mailbox.
        /// </summary>
        /// <param name="mailbox">The mailbox.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.ProjectMailbox GetProjectByMailbox(string mailbox)
        {
            // validate input
            if (String.IsNullOrEmpty(mailbox))
                throw new ArgumentOutOfRangeException("mailbox");

            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETPROJECTBYMAILBOX);

            AddParamToSQLCmd(sqlCmd, "@mailbox", SqlDbType.NVarChar, 0, ParameterDirection.Input, mailbox);

            List<ProjectMailbox> projectList = new List<ProjectMailbox>();
            TExecuteReaderCmd<ProjectMailbox>(sqlCmd, TGenerateProjectMailboxListFromReader<ProjectMailbox>, ref projectList);
            
            if (projectList.Count > 0)
                return projectList[0];
            else
                return null;
                     
        }

        /// <summary>
        /// Gets the mailboxs by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.ProjectMailbox> GetMailboxsByProjectId(int projectId)
        {
            // validate input
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));


            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETMAILBYPROJECTID);

            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            List<ProjectMailbox> projectList = new List<ProjectMailbox>();
            TExecuteReaderCmd<ProjectMailbox>(sqlCmd, TGenerateProjectMailboxListFromReader<ProjectMailbox>, ref projectList);
            return projectList;
        }

        /// <summary>
        /// Creates the project mailbox.
        /// </summary>
        /// <param name="mailboxToUpdate">The mailbox to update.</param>
        /// <returns></returns>
        public override bool CreateProjectMailbox(BugNET.BusinessLogicLayer.ProjectMailbox mailboxToUpdate)
        {
            if (mailboxToUpdate == null)
                throw new ArgumentNullException("mailboxToUpdate");

            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_CREATEPROJECTMAILBOX);

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, mailboxToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@MailBox", SqlDbType.NVarChar, 0, ParameterDirection.Input, mailboxToUpdate.Mailbox);
            AddParamToSQLCmd(sqlCmd, "@AssignToUserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, mailboxToUpdate.AssignToName);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeId", SqlDbType.Int, 0, ParameterDirection.Input, mailboxToUpdate.IssueTypeId);

            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);

        }

        /// <summary>
        /// Deletes the project mailbox.
        /// </summary>
        /// <param name="mailboxId">The mailbox id.</param>
        /// <returns></returns>
        public override bool DeleteProjectMailbox(int mailboxId)
        {
            if (mailboxId <= 0)
                throw new ArgumentOutOfRangeException("mailboxId");

            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_DELETEPROJECTMAILBOX);

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectMailboxId", SqlDbType.Int, 0, ParameterDirection.Input, mailboxId);

            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Creates the new status.
        /// </summary>
        /// <param name="newStatus">The new status.</param>
        /// <returns></returns>
        public override int CreateNewStatus(BugNET.BusinessLogicLayer.Status newStatus)
        {
            // Validate Parameters
            if (newStatus == null)
                throw (new ArgumentNullException("newStatus"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newStatus.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@StatusName", SqlDbType.NVarChar, 0, ParameterDirection.Input, newStatus.Name);
            AddParamToSQLCmd(sqlCmd, "@StatusImageUrl", SqlDbType.NText, 255, ParameterDirection.Input, newStatus.ImageUrl);
            AddParamToSQLCmd(sqlCmd, "@IsClosedState", SqlDbType.Bit, 0, ParameterDirection.Input, newStatus.IsClosedState);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_STATUS_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the status.
        /// </summary>
        /// <param name="statusId">The status id.</param>
        /// <returns></returns>
        public override bool DeleteStatus(int statusId)
        {
            if (statusId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("statusId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@StatusIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, statusId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_STATUS_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Gets the status by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Status> GetStatusByProjectId(int projectId)
        {
            // validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_STATUS_GETSTATUSBYPROJECTID);
            List<Status> statusList = new List<Status>();
            TExecuteReaderCmd<Status>(sqlCmd, TGenerateStatusListFromReader<Status>, ref statusList);
            return statusList;
        }


        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="statusToUpdate">The status to update.</param>
        /// <returns></returns>
        public override bool UpdateStatus(Status statusToUpdate)
        {
            // Validate Parameters
            if (statusToUpdate == null)
                throw (new ArgumentNullException("statusToUpdate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@StatusId", SqlDbType.Int, 0, ParameterDirection.Input, statusToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, statusToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@SortOrder", SqlDbType.Int, 0, ParameterDirection.Input, statusToUpdate.SortOrder);
            AddParamToSQLCmd(sqlCmd, "@StatusName", SqlDbType.NVarChar, 0, ParameterDirection.Input, statusToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@StatusImageUrl", SqlDbType.NText, 255, ParameterDirection.Input, statusToUpdate.ImageUrl);
            AddParamToSQLCmd(sqlCmd, "@IsClosedState", SqlDbType.Bit, 0, ParameterDirection.Input, statusToUpdate.IsClosedState);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_STATUS_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Gets the status by id.
        /// </summary>
        /// <param name="statusId">The status id.</param>
        /// <returns></returns>
        public override Status GetStatusById(int statusId)
        {
            // validate Parameters
            if (statusId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("statusId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@StatusId", SqlDbType.Int, 0, ParameterDirection.Input, statusId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_STATUS_GETSTATUSBYID);
            List<Status> statusList = new List<Status>();
            TExecuteReaderCmd<Status>(sqlCmd, TGenerateStatusListFromReader<Status>, ref statusList);

            if (statusList.Count > 0)
                return statusList[0];
            else
                return null;
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Role> GetAllRoles()
        {
            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_GETALLROLES);
            List<Role> roleList = new List<Role>();
            TExecuteReaderCmd<Role>(sqlCmd, TGenerateRoleListFromReader<Role>, ref roleList);
            return roleList;
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="roleToUpdate">The role to update.</param>
        /// <returns></returns>
        public override bool UpdateRole(BugNET.BusinessLogicLayer.Role roleToUpdate)
        {
            // Validate Parameters
            if (roleToUpdate == null)
                throw (new ArgumentNullException("roleToUpdate"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleToUpdate.Id);
                AddParamToSQLCmd(sqlCmd, "@RoleName", SqlDbType.NText, 256, ParameterDirection.Input, roleToUpdate.Name);
                AddParamToSQLCmd(sqlCmd, "@RoleDescription", SqlDbType.NText, 1000, ParameterDirection.Input, roleToUpdate.Description);
                AddParamToSQLCmd(sqlCmd, "@AutoAssign", SqlDbType.Bit, 0, ParameterDirection.Input, roleToUpdate.AutoAssign);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, roleToUpdate.ProjectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_UPDATEROLE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Creates the new role.
        /// </summary>
        /// <param name="newRole">The new role.</param>
        /// <returns></returns>
        public override int CreateNewRole(BugNET.BusinessLogicLayer.Role newRole)
        {
            // Validate Parameters
            if (newRole == null)
                throw (new ArgumentNullException("newRole"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@RoleName", SqlDbType.NText, 256, ParameterDirection.Input, newRole.Name);
                AddParamToSQLCmd(sqlCmd, "@RoleDescription", SqlDbType.NText, 1000, ParameterDirection.Input, newRole.Description);
                AddParamToSQLCmd(sqlCmd, "@AutoAssign", SqlDbType.Bit, 0, ParameterDirection.Input, newRole.AutoAssign);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newRole.ProjectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_CREATE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Roles the exists.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override bool RoleExists(string roleName, int projectId)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");
            if (projectId <= Globals.NewId)
                throw new ArgumentOutOfRangeException("projectId");

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@RoleName", SqlDbType.NText, 256, ParameterDirection.Input, roleName);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_ROLEEXISTS);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 1 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the name of the roles by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Role> GetRolesByUserName(string userName, int projectId)
        {
            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_GETPROJECTROLESBYUSER);

            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, userName);

            List<Role> roleList = new List<Role>();
            TExecuteReaderCmd<Role>(sqlCmd, TGenerateRoleListFromReader<Role>, ref roleList);
            return roleList;
        }

        /// <summary>
        /// Removes the user from role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        public override bool RemoveUserFromRole(string userName, int roleId)
        {
            if (roleId <= Globals.NewId)
                throw (new ArgumentNullException("roleId"));
            if (string.IsNullOrEmpty(userName))
                throw (new ArgumentNullException("userName"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleId);
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, userName);


            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_REMOVEUSERFROMROLE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Adds the user to role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        public override bool AddUserToRole(string userName, int roleId)
        {
            if (roleId <= Globals.NewId)
                throw (new ArgumentNullException("roleId"));
            if (string.IsNullOrEmpty(userName))
                throw (new ArgumentNullException("userName"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleId);
                AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, userName);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_ADDUSERTOROLE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        public override bool DeleteRole(int roleId)
        {
            if (roleId <= Globals.NewId)
                throw (new ArgumentNullException("roleId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_DELETEROLE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        public override BugNET.BusinessLogicLayer.Role GetRoleById(int roleId)
        {
            if (roleId <= Globals.NewId)
                throw (new ArgumentNullException("roleId"));

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_GETROLEBYID);

                List<Role> roleList = new List<Role>();
                TExecuteReaderCmd<Role>(sqlCmd, TGenerateRoleListFromReader<Role>, ref roleList);
                if (roleList.Count > 0)
                    return roleList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the roles by userName.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Role> GetRolesByUserName(string userName)
        {
            if (userName == null)
                throw (new ArgumentNullException("userName"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, userName);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_GETROLESBYUSER);

            List<Role> roleList = new List<Role>();
            TExecuteReaderCmd<Role>(sqlCmd, TGenerateRoleListFromReader<Role>, ref roleList);

            return roleList;
        }

        /// <summary>
        /// Gets the roles by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Role> GetRolesByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentNullException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ROLE_GETROLESBYPROJECT);

            List<Role> roleList = new List<Role>();
            TExecuteReaderCmd<Role>(sqlCmd, TGenerateRoleListFromReader<Role>, ref roleList);

            return roleList;
        }

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <returns></returns>
        public override List<Permission> GetAllPermissions()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PERMISSION_GETALLPERMISSIONS);
                List<Permission> permissionList = new List<Permission>();
                TExecuteReaderCmd<Permission>(sqlCmd, TGeneratePermissionListFromReader<Permission>, ref permissionList);
                return permissionList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets all role permissions.
        /// </summary>
        /// <returns></returns>
        public override List<RolePermission> GetRolePermissions()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PERMISSION_GETROLEPERMISSIONS);
                List<RolePermission> rolePermissionList = new List<RolePermission>();
                TExecuteReaderCmd<RolePermission>(sqlCmd, TGenerateRolePermissionListFromReader<RolePermission>, ref rolePermissionList);
                return rolePermissionList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the permissions by role id.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.Permission> GetPermissionsByRoleId(int roleId)
        {
            if (roleId <= Globals.NewId)
                throw (new ArgumentNullException("roleId"));

            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PERMISSION_GETPERMISSIONSBYROLE);
                List<Permission> permissionList = new List<Permission>();
                TExecuteReaderCmd<Permission>(sqlCmd, TGeneratePermissionListFromReader<Role>, ref permissionList);
                return permissionList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the permission.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="permissionId">The permission id.</param>
        /// <returns></returns>
        public override bool DeletePermission(int roleId, int permissionId)
        {
            if (roleId <= Globals.NewId)
                throw (new ArgumentNullException("roleId"));
            if (permissionId <= Globals.NewId)
                throw (new ArgumentNullException("permissionId"));
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleId);
                AddParamToSQLCmd(sqlCmd, "@PermissionId", SqlDbType.Int, 0, ParameterDirection.Input, permissionId);
                AddParamToSQLCmd(sqlCmd, "@ResultValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PERMISSION_DELETEROLEPERMISSION);
                ExecuteScalarCmd(sqlCmd);
                int resultValue = (int)sqlCmd.Parameters["@ResultValue"].Value;
                return (resultValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Adds the permission.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="permissionId">The permission id.</param>
        /// <returns></returns>
        public override bool AddPermission(int roleId, int permissionId)
        {
            // Validate Parameters
            if (roleId <= Globals.NewId)
                throw (new ArgumentNullException("roleId"));
            if (permissionId <= Globals.NewId)
                throw (new ArgumentNullException("permissionId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@RoleId", SqlDbType.Int, 0, ParameterDirection.Input, roleId);
            AddParamToSQLCmd(sqlCmd, "@PermissionId", SqlDbType.Int, 0, ParameterDirection.Input, permissionId);


            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PERMISSION_ADDROLEPERMISSION);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Gets the custom field by id.
        /// </summary>
        /// <param name="customFieldId">The custom field id.</param>
        /// <returns></returns>
        public override CustomField GetCustomFieldById(int customFieldId)
        {
            if (customFieldId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("customFieldId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@CustomFieldId", SqlDbType.Int, 0, ParameterDirection.Input, customFieldId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELD_GETCUSTOMFIELDBYID);

            List<CustomField> customFieldList = new List<CustomField>();
            TExecuteReaderCmd<CustomField>(sqlCmd, TGenerateCustomFieldListFromReader<CustomField>, ref customFieldList);

            if (customFieldList.Count > 0)
                return customFieldList[0];
            else
                return null;
        }

        /// <summary>
        /// Gets the custom fields by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.CustomField> GetCustomFieldsByProjectId(int projectId)
        {
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELD_GETCUSTOMFIELDSBYPROJECTID);

            List<CustomField> customFieldList = new List<CustomField>();
            TExecuteReaderCmd<CustomField>(sqlCmd, TGenerateCustomFieldListFromReader<CustomField>, ref customFieldList);

            return customFieldList;
        }

        /// <summary>
        /// Gets the custom fields by issue id.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.CustomField> GetCustomFieldsByIssueId(int issueId)
        {
            if (issueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELD_GETCUSTOMFIELDSBYISSUEID);

            List<CustomField> customFieldList = new List<CustomField>();
            TExecuteReaderCmd<CustomField>(sqlCmd, TGenerateCustomFieldListFromReader<CustomField>, ref customFieldList);

            return customFieldList;
        }

        /// <summary>
        /// Creates the new custom field.
        /// </summary>
        /// <param name="newCustomField">The new custom field.</param>
        /// <returns></returns>
        public override int CreateNewCustomField(BugNET.BusinessLogicLayer.CustomField newCustomField)
        {
            if (newCustomField == null)
                throw new ArgumentNullException("newCustomField");

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldName", SqlDbType.NText, 50, ParameterDirection.Input, newCustomField.Name);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldDataType", SqlDbType.Int, 0, ParameterDirection.Input, newCustomField.DataType);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newCustomField.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldRequired", SqlDbType.Bit, 0, ParameterDirection.Input, newCustomField.Required);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldTypeId", SqlDbType.Int, 0, ParameterDirection.Input, (int)newCustomField.FieldType);


            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELD_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Updates the custom field.
        /// </summary>
        /// <param name="customFieldToUpdate">The custom field to update.</param>
        /// <returns></returns>
        public override bool UpdateCustomField(BugNET.BusinessLogicLayer.CustomField customFieldToUpdate)
        {
            if (customFieldToUpdate == null)
                throw new ArgumentNullException("customFieldToUpdate");

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldId", SqlDbType.Int, 0, ParameterDirection.Input, customFieldToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldName", SqlDbType.NText, 50, ParameterDirection.Input, customFieldToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldDataType", SqlDbType.Int, 0, ParameterDirection.Input, customFieldToUpdate.DataType);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, customFieldToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldRequired", SqlDbType.Bit, 0, ParameterDirection.Input, customFieldToUpdate.Required);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldTypeId", SqlDbType.Int, 0, ParameterDirection.Input, (int)customFieldToUpdate.FieldType);


            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELD_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Deletes the custom field.
        /// </summary>
        /// <param name="customFieldId">The custom field id.</param>
        /// <returns></returns>
        public override bool DeleteCustomField(int customFieldId)
        {
            // Validate Parameters
            if (customFieldId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("customFieldId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@CustomFieldIdToDelete", SqlDbType.Int, 4, ParameterDirection.Input, customFieldId);
            AddParamToSQLCmd(sqlCmd, "@ResultValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELD_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int resultValue = (int)sqlCmd.Parameters["@ResultValue"].Value;
            return (resultValue == 0 ? true : false);
        }

        /// <summary>
        /// Saves the custom field values.
        /// </summary>
        /// <param name="IssueId">The issue id.</param>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        public override bool SaveCustomFieldValues(int issueId, System.Collections.Generic.List<BugNET.BusinessLogicLayer.CustomField> fields)
        {
            // Validate Parameters
            if (fields == null)
                throw (new ArgumentNullException("fields"));

            if (issueId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("issueId"));

            // Execute SQL Commands
            SqlCommand sqlCmd = new SqlCommand();

            sqlCmd.Parameters.Add("@ReturnValue", SqlDbType.Int, 0).Direction = ParameterDirection.ReturnValue;
            sqlCmd.Parameters.Add("@IssueId", SqlDbType.Int, 0).Direction = ParameterDirection.Input;
            sqlCmd.Parameters.Add("@CustomFieldId", SqlDbType.Int, 0).Direction = ParameterDirection.Input;
            sqlCmd.Parameters.Add("@CustomFieldValue", SqlDbType.NVarChar, 4000).Direction = ParameterDirection.Input;

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELD_SAVECUSTOMFIELDVALUE);

            bool errors = false;

            foreach (CustomField field in fields)
            {
                sqlCmd.Parameters["@IssueId"].Value = issueId;
                sqlCmd.Parameters["@CustomFieldId"].Value = field.Id;
                sqlCmd.Parameters["@CustomFieldValue"].Value = field.Value;
                ExecuteScalarCmd(sqlCmd);
                if ((int)sqlCmd.Parameters["@ReturnValue"].Value == 1)
                    errors = true;
            }
            return !errors;
        }

        /// <summary>
        /// Creates the new custom field selection.
        /// </summary>
        /// <param name="newCustomFieldSelection">The new custom field selection.</param>
        /// <returns></returns>
        public override int CreateNewCustomFieldSelection(BugNET.BusinessLogicLayer.CustomFieldSelection newCustomFieldSelection)
        {
            if (newCustomFieldSelection == null)
                throw new ArgumentNullException("newCustomFieldSelection");

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldId", SqlDbType.Int, 0, ParameterDirection.Input, newCustomFieldSelection.CustomFieldId);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldSelectionName", SqlDbType.NVarChar, 255, ParameterDirection.Input, newCustomFieldSelection.Name);
            AddParamToSQLCmd(sqlCmd, "@CUstomFieldSelectionValue", SqlDbType.NVarChar, 255, ParameterDirection.Input, newCustomFieldSelection.Value);


            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELDSELECTION_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the custom field selection.
        /// </summary>
        /// <param name="customFieldSelectionId">The custom field selection id.</param>
        /// <returns></returns>
        public override bool DeleteCustomFieldSelection(int customFieldSelectionId)
        {
            if (customFieldSelectionId <= 0)
                throw new ArgumentOutOfRangeException("customFieldSelectionId");

            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldSelectionIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, customFieldSelectionId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELDSELECTION_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Gets the custom field selections by custom field id.
        /// </summary>
        /// <param name="customFieldId">The custom field id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.CustomFieldSelection> GetCustomFieldSelectionsByCustomFieldId(int customFieldId)
        {
            if (customFieldId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("customFieldId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@CustomFieldId", SqlDbType.Int, 0, ParameterDirection.Input, customFieldId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELDSELECTION_GETCUSTOMFIELDSELECTIONSBYCUSTOMFIELDID);

            List<CustomFieldSelection> customFieldSelectionList = new List<CustomFieldSelection>();
            TExecuteReaderCmd<CustomFieldSelection>(sqlCmd, TGenerateCustomFieldSelectionListFromReader<CustomFieldSelection>, ref customFieldSelectionList);

            return customFieldSelectionList;
        }

        /// <summary>
        /// Gets the custom field selection by id.
        /// </summary>
        /// <param name="customFieldSelectionId">The custom field selection id.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.CustomFieldSelection GetCustomFieldSelectionById(int customFieldSelectionId)
        {
            if (customFieldSelectionId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("customFieldSelectionId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@CustomFieldSelectionId", SqlDbType.Int, 0, ParameterDirection.Input, customFieldSelectionId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELDSELECTION_GETCUSTOMFIELDSELECTIONBYID);

            List<CustomFieldSelection> customFieldSelectionList = new List<CustomFieldSelection>();
            TExecuteReaderCmd<CustomFieldSelection>(sqlCmd, TGenerateCustomFieldSelectionListFromReader<CustomFieldSelection>, ref customFieldSelectionList);

            if (customFieldSelectionList.Count > 0)
                return customFieldSelectionList[0];
            else
                return null;
        }

        /// <summary>
        /// Updates the custom field selection.
        /// </summary>
        /// <param name="customFieldSelectionToUpdate">The custom field selection to update.</param>
        /// <returns></returns>
        public override bool UpdateCustomFieldSelection(BugNET.BusinessLogicLayer.CustomFieldSelection customFieldSelectionToUpdate)
        {
            if (customFieldSelectionToUpdate == null)
                throw new ArgumentNullException("customFieldSelectionToUpdate");

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldId", SqlDbType.Int, 0, ParameterDirection.Input, customFieldSelectionToUpdate.CustomFieldId);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldSelectionId", SqlDbType.Int, 0, ParameterDirection.Input, customFieldSelectionToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldSelectionName", SqlDbType.NText, 50, ParameterDirection.Input, customFieldSelectionToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldSelectionValue", SqlDbType.NText, 50, ParameterDirection.Input, customFieldSelectionToUpdate.Value);
            AddParamToSQLCmd(sqlCmd, "@CustomFieldSelectionSortOrder", SqlDbType.Int, 0, ParameterDirection.Input, customFieldSelectionToUpdate.SortOrder);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CUSTOMFIELDSELECTION_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Gets the issue type by id.
        /// </summary>
        /// <param name="issueTypeId">The issue type id.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.IssueType GetIssueTypeById(int issueTypeId)
        {
            // validate Parameters
            if (issueTypeId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("issueTypeId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@IssueTypeId", SqlDbType.Int, 0, ParameterDirection.Input, issueTypeId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUETYPE_GETISSUETYPEBYID);
            List<IssueType> issueTypeList = new List<IssueType>();
            TExecuteReaderCmd<IssueType>(sqlCmd, TGenerateIssueTypeListFromReader<IssueType>, ref issueTypeList);

            if (issueTypeList.Count > 0)
                return issueTypeList[0];
            else
                return null;
        }

        /// <summary>
        /// Creates the new type of the issue.
        /// </summary>
        /// <param name="issueTypeToCreate">The issue type to create.</param>
        /// <returns></returns>
        public override int CreateNewIssueType(IssueType issueTypeToCreate)
        {
            // Validate Parameters
            if (issueTypeToCreate == null)
                throw (new ArgumentNullException("issueTypeToCreate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, issueTypeToCreate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeName", SqlDbType.NText, 50, ParameterDirection.Input, issueTypeToCreate.Name);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeImageUrl", SqlDbType.NText, 255, ParameterDirection.Input, issueTypeToCreate.ImageUrl);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUETYPE_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the type of the issue.
        /// </summary>
        /// <param name="issueTypeId">The issue type id.</param>
        /// <returns></returns>
        public override bool DeleteIssueType(int issueTypeId)
        {
            if (issueTypeId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("issueTypeId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, issueTypeId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUETYPE_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Updates the type of the issue.
        /// </summary>
        /// <param name="issueTypeToUpdate">The issue type to update.</param>
        /// <returns></returns>
        public override bool UpdateIssueType(IssueType issueTypeToUpdate)
        {
            // Validate Parameters
            if (issueTypeToUpdate == null)
                throw (new ArgumentNullException("issueTypeToUpdate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeId", SqlDbType.Int, 0, ParameterDirection.Input, issueTypeToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@SortOrder", SqlDbType.Int, 0, ParameterDirection.Input, issueTypeToUpdate.SortOrder);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, issueTypeToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeName", SqlDbType.NText, 50, ParameterDirection.Input, issueTypeToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@IssueTypeImageUrl", SqlDbType.NText, 50, ParameterDirection.Input, issueTypeToUpdate.ImageUrl);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUETYPE_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Gets the issue types by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<IssueType> GetIssueTypesByProjectId(int projectId)
        {
            // validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUETYPE_GETISSUETYPESBYPROJECTID);
            List<IssueType> issueTypeList = new List<IssueType>();
            TExecuteReaderCmd<IssueType>(sqlCmd, TGenerateIssueTypeListFromReader<IssueType>, ref issueTypeList);
            return issueTypeList;
        }

        /// <summary>
        /// Gets the resolution by id.
        /// </summary>
        /// <param name="resolutionId">The resolution id.</param>
        /// <returns></returns>
        public override BugNET.BusinessLogicLayer.Resolution GetResolutionById(int resolutionId)
        {
            // validate Parameters
            if (resolutionId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("resolutinoId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ResolutionId", SqlDbType.Int, 0, ParameterDirection.Input, resolutionId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RESOLUTION_GETRESOLUTIONBYID);
            List<Resolution> resolutionList = new List<Resolution>();
            TExecuteReaderCmd<Resolution>(sqlCmd, TGenerateResolutionListFromReader<Resolution>, ref resolutionList);

            if (resolutionList.Count > 0)
                return resolutionList[0];
            else
                return null;
        }


        /// <summary>
        /// Creates the new resolution.
        /// </summary>
        /// <param name="resolutionToCreate">The resolution to create.</param>
        /// <returns></returns>
        public override int CreateNewResolution(Resolution resolutionToCreate)
        {
            // Validate Parameters
            if (resolutionToCreate == null)
                throw (new ArgumentNullException("resolutionToCreate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, resolutionToCreate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@ResolutionName", SqlDbType.NText, 50, ParameterDirection.Input, resolutionToCreate.Name);
            AddParamToSQLCmd(sqlCmd, "@ResolutionImageUrl", SqlDbType.NText, 255, ParameterDirection.Input, resolutionToCreate.ImageUrl);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RESOLUTION_CREATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
        }

        /// <summary>
        /// Deletes the resolution.
        /// </summary>
        /// <param name="resolutionId">The resolution id.</param>
        /// <returns></returns>
        public override bool DeleteResolution(int resolutionId)
        {
            if (resolutionId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("resolutionId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ResolutionIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, resolutionId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RESOLUTION_DELETE);
            ExecuteScalarCmd(sqlCmd);
            int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
            return (returnValue == 0 ? true : false);
        }

        /// <summary>
        /// Updates the resolution.
        /// </summary>
        /// <param name="resolutionToUpdate">The resolution to update.</param>
        /// <returns></returns>
        public override bool UpdateResolution(Resolution resolutionToUpdate)
        {
            // Validate Parameters
            if (resolutionToUpdate == null)
                throw (new ArgumentNullException("resolutionToUpdate"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@ResolutionId", SqlDbType.Int, 0, ParameterDirection.Input, resolutionToUpdate.Id);
            AddParamToSQLCmd(sqlCmd, "@SortOrder", SqlDbType.Int, 0, ParameterDirection.Input, resolutionToUpdate.SortOrder);
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, resolutionToUpdate.ProjectId);
            AddParamToSQLCmd(sqlCmd, "@ResolutionName", SqlDbType.NText, 50, ParameterDirection.Input, resolutionToUpdate.Name);
            AddParamToSQLCmd(sqlCmd, "@ResolutionImageUrl", SqlDbType.NText, 50, ParameterDirection.Input, resolutionToUpdate.ImageUrl);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RESOLUTION_UPDATE);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Gets the resolutions by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<Resolution> GetResolutionsByProjectId(int projectId)
        {
            // validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_RESOLUTION_GETRESOLUTIONSBYPROJECTID);
            List<Resolution> resolutionList = new List<Resolution>();
            TExecuteReaderCmd<Resolution>(sqlCmd, TGenerateResolutionListFromReader<Resolution>, ref resolutionList);
            return resolutionList;
        }

        /// <summary>
        /// Gets the host settings.
        /// </summary>
        /// <returns>A list of host setting objects.</returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.HostSetting> GetHostSettings()
        {
            SqlCommand sqlCmd = new SqlCommand();
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_HOSTSETTING_GETHOSTSETTINGS);

            List<HostSetting> hostSettingList = new List<HostSetting>();
            TExecuteReaderCmd<HostSetting>(sqlCmd, TGenerateHostSettingListFromReader<HostSetting>, ref hostSettingList);

            return hostSettingList;
        }

        /// <summary>
        /// Updates the host setting.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="settingValue">The setting value.</param>
        /// <returns></returns>
        public override bool UpdateHostSetting(string settingName, string settingValue)
        {
            if (string.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("settingName");

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();

            AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(sqlCmd, "@SettingName", SqlDbType.NVarChar, 0, ParameterDirection.Input, settingName);
            AddParamToSQLCmd(sqlCmd, "@SettingValue", SqlDbType.NVarChar, 0, ParameterDirection.Input, settingValue);

            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_HOSTSETTING_UPDATEHOSTSETTING);
            ExecuteScalarCmd(sqlCmd);
            return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
        }

        /// <summary>
        /// Creates the new issue work report.
        /// </summary>
        /// <param name="issueWorkReportToCreate">The issue work report to create.</param>
        /// <returns></returns>
        public override int CreateNewIssueWorkReport(BugNET.BusinessLogicLayer.IssueWorkReport issueWorkReportToCreate)
        {
            if (issueWorkReportToCreate == null)
                throw (new ArgumentNullException("issueWorkReportToCreate"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.NVarChar, 0, ParameterDirection.Input, issueWorkReportToCreate.IssueId);
                AddParamToSQLCmd(sqlCmd, "@CreatorUserName", SqlDbType.NVarChar, 0, ParameterDirection.Input, issueWorkReportToCreate.CreatorUserName);
                AddParamToSQLCmd(sqlCmd, "@WorkDate", SqlDbType.DateTime, 0, ParameterDirection.Input, issueWorkReportToCreate.WorkDate);
                AddParamToSQLCmd(sqlCmd, "@Duration", SqlDbType.Decimal, 0, ParameterDirection.Input, issueWorkReportToCreate.Duration);
                AddParamToSQLCmd(sqlCmd, "@IssueCommentId", SqlDbType.Int, 0, ParameterDirection.Input, issueWorkReportToCreate.CommentId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEWORKREPORT_CREATE);
                ExecuteScalarCmd(sqlCmd);

                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the work report by issue id.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.IssueWorkReport> GetIssueWorkReportsByIssueId(int issueId)
        {
            if (issueId <= 0)
                throw (new ArgumentOutOfRangeException("issueId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEWORKREPORT_GETBYISSUEWORKREPORTSBYISSUEID);

            List<IssueWorkReport> issueTimeEntryList = new List<IssueWorkReport>();
            TExecuteReaderCmd<IssueWorkReport>(sqlCmd, TGenerateIssueTimeEntryListFromReader<IssueWorkReport>, ref issueTimeEntryList);

            return issueTimeEntryList;
        }

        /// <summary>
        /// Deletes the issue work report.
        /// </summary>
        /// <param name="issueWorkReportId">The issue work report id.</param>
        /// <returns></returns>
        public override bool DeleteIssueWorkReport(int issueWorkReportId)
        {
            if (issueWorkReportId <= 0)
                throw (new ArgumentOutOfRangeException("issueWorkReportId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueWorkReportId", SqlDbType.Int, 0, ParameterDirection.Input, issueWorkReportId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEWORKREPORT_DELETE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.IssueWorkReport> GetIssueWorkReportsByProjectId(int projectId)
        {
            throw new NotImplementedException();
        }

        public override System.Collections.Generic.List<BugNET.BusinessLogicLayer.IssueWorkReport> GetIssueWorkReportsByUserName(int projectId, string reporterUserName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the application log.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <returns></returns>
        public override List<ApplicationLog> GetApplicationLog(string filterType)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_APPLICATIONLOG_GETLOG);

                List<ApplicationLog> applicationLogList = new List<ApplicationLog>();
                AddParamToSQLCmd(sqlCmd, "@FilterType", SqlDbType.NVarChar, 50, ParameterDirection.Input, (filterType.Length == 0) ? DBNull.Value : (object)filterType);
                TExecuteReaderCmd<ApplicationLog>(sqlCmd, TGenerateApplicationLogListFromReader<ApplicationLog>, ref applicationLogList);

                return applicationLogList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Clears the application log.
        /// </summary>
        public override void ClearApplicationLog()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_APPLICATIONLOG_CLEARLOG);
                ExecuteScalarCmd(sqlCmd);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }


        /// <summary>
        /// Gets the required fields for issues.
        /// </summary>
        /// <returns></returns>
        public override List<RequiredField> GetRequiredFieldsForIssues()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_REQUIREDFIELDS_GETFIELDLIST);
                List<RequiredField> requiredFieldList = new List<RequiredField>();
                TExecuteReaderCmd<RequiredField>(sqlCmd, TGenerateRequiredFieldListFromReader<RequiredField>, ref requiredFieldList);
                return requiredFieldList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }

        }

        /// <summary>
        /// Gets the name of the queries by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<Query> GetQueriesByUserName(string userName, int projectId)
        {
            // Validate Parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            // Execute SQL Command
            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_QUERY_GETQUERIESBYUSERNAME);
            List<Query> queryList = new List<Query>();
            TExecuteReaderCmd<Query>(sqlCmd, TGenerateQueryListFromReader<Query>, ref queryList);
            return queryList;
        }

        /// <summary>
        /// Saves the query.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryName">Name of the query.</param>
        /// <param name="queryClauses">The query clauses.</param>
        /// <returns></returns>
        public override bool SaveQuery(string userName, int projectId, string queryName, bool isPublic, List<QueryClause> queryClauses)
        {
            if (queryName == null || queryName == String.Empty)
                throw (new ArgumentOutOfRangeException("queryName"));

            if (queryClauses.Count == 0)
                throw (new ArgumentOutOfRangeException("queryClauses"));

            // Create Save Query Command
            SqlCommand cmdSaveQuery = new SqlCommand();
            int intPublic;
            if (isPublic == true)
                intPublic = 1;
            else
                intPublic = 0;


            AddParamToSQLCmd(cmdSaveQuery, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(cmdSaveQuery, "@Username", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName == string.Empty ? DBNull.Value : (object)userName);
            AddParamToSQLCmd(cmdSaveQuery, "@projectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            AddParamToSQLCmd(cmdSaveQuery, "@QueryName", SqlDbType.NText, 50, ParameterDirection.Input, queryName);
            AddParamToSQLCmd(cmdSaveQuery, "@IsPublic", SqlDbType.Bit, 0, ParameterDirection.Input, intPublic);

            SetCommandType(cmdSaveQuery, CommandType.StoredProcedure, SP_QUERY_SAVEQUERY);

            // Create Save Query Clause Command
            SqlCommand cmdClause = new SqlCommand();

            cmdClause.Parameters.Add("@QueryId", SqlDbType.Int);
            cmdClause.Parameters.Add("@BooleanOperator", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@FieldName", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@ComparisonOperator", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@FieldValue", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@DataType", SqlDbType.Int);
            cmdClause.Parameters.Add("@IsCustomField", SqlDbType.Bit);

            SetCommandType(cmdClause, CommandType.StoredProcedure, SP_QUERY_SAVEQUERYCLAUSE);

            ExecuteScalarCmd(cmdSaveQuery);

            int queryId = (int)cmdSaveQuery.Parameters["@ReturnValue"].Value;
            if (queryId == 0)
                return false;

            // Save Query Clauses
            foreach (QueryClause clause in queryClauses)
            {
                cmdClause.Parameters["@QueryId"].Value = queryId;
                cmdClause.Parameters["@BooleanOperator"].Value = clause.BooleanOperator;
                cmdClause.Parameters["@FieldName"].Value = clause.FieldName;
                cmdClause.Parameters["@ComparisonOperator"].Value = clause.ComparisonOperator;
                cmdClause.Parameters["@FieldValue"].Value = clause.FieldValue;
                cmdClause.Parameters["@DataType"].Value = clause.DataType;
                cmdClause.Parameters["@IsCustomField"].Value = clause.CustomFieldQuery;
                ExecuteScalarCmd(cmdClause);
            }

            return true;
        }

        /// <summary>
        /// Updates the query.
        /// </summary>
        /// <param name="queryId">The query id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryName">Name of the query.</param>
        /// <param name="isPublic">if set to <c>true</c> [is public].</param>
        /// <param name="queryClauses">The query clauses.</param>
        /// <returns></returns>
        public override bool UpdateQuery(int queryId, string userName, int projectId, string queryName, bool isPublic, List<QueryClause> queryClauses)
        {
            if (queryId <= 0)
                throw new ArgumentOutOfRangeException("queryId");

            if (queryName == null || queryName == String.Empty)
                throw (new ArgumentOutOfRangeException("queryName"));

            if (queryClauses.Count == 0)
                throw (new ArgumentOutOfRangeException("queryClauses"));

            // Create Save Query Command
            SqlCommand cmdSaveQuery = new SqlCommand();
            int intPublic;
            if (isPublic == true)
                intPublic = 1;
            else
                intPublic = 0;


            AddParamToSQLCmd(cmdSaveQuery, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
            AddParamToSQLCmd(cmdSaveQuery, "@QueryId", SqlDbType.Int, 0, ParameterDirection.Input, queryId);
            AddParamToSQLCmd(cmdSaveQuery, "@Username", SqlDbType.NVarChar, 255, ParameterDirection.Input, userName == string.Empty ? DBNull.Value : (object)userName);
            AddParamToSQLCmd(cmdSaveQuery, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            AddParamToSQLCmd(cmdSaveQuery, "@QueryName", SqlDbType.NText, 50, ParameterDirection.Input, queryName);
            AddParamToSQLCmd(cmdSaveQuery, "@IsPublic", SqlDbType.Bit, 0, ParameterDirection.Input, intPublic);

            SetCommandType(cmdSaveQuery, CommandType.StoredProcedure, SP_QUERY_UPDATEQUERY);

            // Create Save Query Clause Command
            SqlCommand cmdClause = new SqlCommand();

            cmdClause.Parameters.Add("@QueryId", SqlDbType.Int);
            cmdClause.Parameters.Add("@BooleanOperator", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@FieldName", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@ComparisonOperator", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@FieldValue", SqlDbType.NVarChar, 50);
            cmdClause.Parameters.Add("@DataType", SqlDbType.Int);
            cmdClause.Parameters.Add("@IsCustomField", SqlDbType.Bit);

            SetCommandType(cmdClause, CommandType.StoredProcedure, SP_QUERY_SAVEQUERYCLAUSE);

            ExecuteScalarCmd(cmdSaveQuery);

            // Save Query Clauses
            foreach (QueryClause clause in queryClauses)
            {
                cmdClause.Parameters["@QueryId"].Value = queryId;
                cmdClause.Parameters["@BooleanOperator"].Value = clause.BooleanOperator;
                cmdClause.Parameters["@FieldName"].Value = clause.FieldName;
                cmdClause.Parameters["@ComparisonOperator"].Value = clause.ComparisonOperator;
                cmdClause.Parameters["@FieldValue"].Value = clause.FieldValue;
                cmdClause.Parameters["@DataType"].Value = clause.DataType;
                cmdClause.Parameters["@IsCustomField"].Value = clause.CustomFieldQuery;
                ExecuteScalarCmd(cmdClause);
            }

            return true;
        }

        //public override List<Issue> PerformQuery(int projectId, List<QueryClause> queryClauses)
        //{
        //    if (projectId <= Globals.NewId)
        //        throw (new ArgumentOutOfRangeException("projectId"));

        //    if (queryClauses.Count == 0)
        //        throw (new ArgumentOutOfRangeException("queryClauses"));

        //    try
        //    {

        //        // Build Command Text
        //        StringBuilder commandBuilder = new StringBuilder();
        //        commandBuilder.Append("SELECT * FROM BugNet_IssuesView WHERE ProjectId=@projectId AND IssueId IN (SELECT IssueId FROM BugNet_Issues WHERE 1=1 ");

        //        int i = 0;
        //        foreach (QueryClause qc in queryClauses)
        //        {
        //            commandBuilder.AppendFormat(" {0} {1} {2} @p{3}", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator, i);
        //            i++;
        //        }
        //        commandBuilder.Append(") ORDER BY IssueId DESC");

        //        // Create Command object
        //        SqlCommand sqlCmd = new SqlCommand();
        //        sqlCmd.CommandText = commandBuilder.ToString();


        //        // Build Parameter List
        //        sqlCmd.Parameters.Add("@projectId", SqlDbType.Int).Value = projectId;
        //        i = 0;
        //        foreach (QueryClause qc in queryClauses)
        //        {
        //            sqlCmd.Parameters.Add("@p" + i.ToString(), qc.DataType).Value = qc.FieldValue;
        //            i++;
        //        }

        //        List<Issue> issueList = new List<Issue>();
        //        TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);
        //        return issueList;
        //    }
        //    catch (Exception ex)
        //    {
        //       throw ProcessException(ex);
        //    }
        //}

        /// <summary>
        /// Performs the query.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryClauses">The query clauses.</param>
        /// <param name="outIssueCollection">The out issue collection.</param>
        private void PerformQueryOnCustomFields(int projectId, ref List<QueryClause> queryClauses, ref List<Issue> outIssueCollection)
        {
            //validate parameters
            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));

            if (queryClauses.Count == 0)
                throw (new ArgumentOutOfRangeException("queryClauses"));

            List<Issue> issueList2 = new List<Issue>();

            //Build the command text
            StringBuilder commandBuilder = new StringBuilder("SELECT TOP 100 PERCENT * FROM ");
            commandBuilder.Append(SP_ISSUE_BYPROJECTIDANDCUSTOMFIELDVIEW);
            commandBuilder.Append(" WHERE ");
            commandBuilder.Append(" ProjectId=@projectId AND Disabled=0 AND IssueId IN ");
            commandBuilder.Append(" (SELECT IssueId FROM BugNet_Issues WHERE 1=1 ");
            int i = 1000; //RW incremental number for the parameter name, started with 1000


            foreach (QueryClause qc in queryClauses)
            {
                if (qc.CustomFieldQuery)
                {
                    commandBuilder.AppendFormat(" {0} {1} {2} {3} {4} @p{5}", qc.BooleanOperator, "CustomFieldName=", "'" + qc.FieldName + "'", " AND CustomFieldValue ", qc.ComparisonOperator, i);
                    i++;
                } //note: those are single quote marks inside a double qoute on either side of qc.fieldname

            }

            commandBuilder.Append(" ) ORDER BY IssueId DESC");


            //create a new SqlCommand object
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = commandBuilder.ToString();

            //RW build parm list
            sqlCmd.Parameters.Add("@projectId", SqlDbType.Int).Value = projectId;

            //Why 1000? Remember, we passed the queryClauses collection by ref.
            //If there were non-custom queries, they would end up with duplicate parameters
            //if we use i=0; thus:
            i = 1000; //RW reset our counter

            foreach (QueryClause qc in queryClauses)
            {
                if (qc.CustomFieldQuery)  //we only want customfield queries.
                {
                    sqlCmd.Parameters.Add("@p" + i.ToString(), qc.DataType).Value = qc.FieldValue;
                    i++;
                }

            }

            TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList2);

            // RW Generate a collection
            //GenerateCollectionFromReader sqlData = new GenerateCollectionFromReader(GenerateIssueCollectionFromReader);
            //issCollection2 = (IssueCollection)ExecuteReaderCmd(sqlCmd, sqlData);
            if (issueList2.Count > 0)
            {
                foreach (Issue iss in issueList2)
                {
                    try
                    {
                        if (!outIssueCollection.Exists(delegate(Issue issue) { return issue.Id == iss.Id; }))
                            outIssueCollection.Add(iss);
                    }


                    catch { }
                    // RW do nothing. You can add a routine to check for and remove duplicate issues
                    //because we are populating an existing collection.	

                }
            }
            //return (issCollection) we really don't return anything, because the object was passed by ref.
            //so we just leave the routine.

        } // end PerformQueryOnCustomFields

        

        /// <summary>
        /// Performs the query.
        /// </summary>   
        /// <param name="projectId">The project id.</param>
        /// <param name="queryClauses">The query clauses.</param>
        /// <returns></returns>        
        public List<Issue> PerformQueryOLD(int projectId, List<QueryClause> queryClauses)
        {
            
            // Validate Parameters
            int queryClauseCount = 0;

            if (projectId <= Globals.NewId)
                throw (new ArgumentOutOfRangeException("projectId"));
            
            //assign the queryClauses Count to our variable and then check the result.
            if ((queryClauseCount = queryClauses.Count) == 0)
                throw (new ArgumentOutOfRangeException("queryClauses"));


            // Build Command Text
            StringBuilder commandBuilder = new StringBuilder();
            commandBuilder.Append("SELECT * FROM BugNet_IssuesView WHERE ProjectId=@projectId AND Disabled=0 AND IssueId IN (SELECT IssueId FROM BugNet_Issues WHERE 1=1 ");

            int i = 0;

            //RW check for Standard Query
            foreach (QueryClause qc in queryClauses)
            {                
                if (!qc.CustomFieldQuery)
                {
                    //if null or empty do a null comparision
                    if (string.IsNullOrEmpty(qc.FieldValue))
                    {
                        commandBuilder.AppendFormat(" {0} {1} {2} NULL", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator);
                    }
                    else
                    {
                        commandBuilder.AppendFormat(" {0} {1} {2} @p{3}", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator, i);
                    }
                    i++;
                }
            }

            commandBuilder.Append(") ORDER BY IssueId DESC");

            // Create Command object
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = commandBuilder.ToString();

            // Build Parameter List
            sqlCmd.Parameters.Add("@projectId", SqlDbType.Int).Value = projectId;
            i = 0; //RW counter for parameters			

            //RW loop thru and add non custom field queries parameters.
            foreach (QueryClause qc in queryClauses)
            {
                if (!qc.CustomFieldQuery) //RW not a custom field query
                {
                    //skip if value null
                    if (!string.IsNullOrEmpty(qc.FieldValue))
                    {
                        sqlCmd.Parameters.Add("@p" + i.ToString(), qc.DataType).Value = qc.FieldValue;
                    }
                    i++;
                }
            }


            //RW create a new issue collection here
            List<Issue> issueList = new List<Issue>();

            //more queries, but they are not custom.
            //So, populate the collection with what we have.
            if (i > 0 && i <= queryClauseCount)
            {
                TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);
            }

            if (queryClauseCount > i) //there were custom fields
            {
                // Query the custom fields in all the projects we have 
                PerformQueryOnCustomFields(projectId, ref queryClauses, ref issueList);
            }

            //note we are passing this methods objects by reference because we want
            //the collection to be added to. If we don't pass by reference, we lose the collection
            //when the method returns.
            return (issueList);

        } //end PerformQuery


        /// <summary>
        /// Combined Perform Custom Query Method
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryClauses">The query clauses.</param>
        /// <returns></returns>
        public override List<Issue> PerformQuery(int projectId, List<QueryClause> queryClauses)
        {
            // Validate Parameters
            int queryClauseCount = 0;
            //if (projectId <= Globals.NewId)
            //    throw (new ArgumentOutOfRangeException("projectId"));

            //assign the queryClauses Count to our variable and then check the result.
            if ((queryClauseCount = queryClauses.Count) == 0)
                throw (new ArgumentOutOfRangeException("queryClauses"));

            try
            {
                // Build Command Text
                StringBuilder commandBuilder = new StringBuilder();
                //'DSS customfields in the same query   
                if (projectId != 0)
                    commandBuilder.Append("SELECT DISTINCT * FROM BugNet_GetIssuesByProjectIdAndCustomFieldView WHERE ProjectId=@ProjectId AND (Disabled = 0 AND ProjectDisabled = 0) AND IssueId IN (SELECT IssueId FROM BugNet_Issues WHERE 1=1 ");
                else
                    commandBuilder.Append("SELECT DISTINCT * FROM BugNet_GetIssuesByProjectIdAndCustomFieldView WHERE (Disabled = 0 AND ProjectDisabled = 0) AND IssueId IN (SELECT IssueId FROM BugNet_Issues WHERE 1=1 ");

                int i = 0;

                //RW check for Standard Query
                foreach (QueryClause qc in queryClauses)
                {
                    

                    if (!qc.CustomFieldQuery)
                    {
                        if (qc.BooleanOperator.Trim().Equals(")"))
                            commandBuilder.AppendFormat(" {0}", qc.BooleanOperator);
                        else if (string.IsNullOrEmpty(qc.FieldValue))
                        {
                            commandBuilder.AppendFormat(" {0} {1} {2} NULL", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator);
                        }
                        else
                        {
                            commandBuilder.AppendFormat(" {0} {1} {2} @p{3}", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator, i);
                        }
                        i++;
                        //commandBuilder.AppendFormat(" {0} {1} {2} @p{3}", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator, i);
                        //if (qc.BooleanOperator.Trim().Equals(")"))
                        //    commandBuilder.AppendFormat(" {0}", qc.BooleanOperator);
                        //else
                        //    commandBuilder.AppendFormat(" {0} {1} {2} @p{3}", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator, i);
                        //i++;
                    }
                    else
                    {
                        //'DSS customfields in the same query
                        commandBuilder.AppendFormat(" {0} {1} {2} {3} {4} @p{5}", qc.BooleanOperator, "CustomFieldName=", "'" + qc.FieldName + "'", " AND CustomFieldValue ", qc.ComparisonOperator, i);
                        i += 1;
                    }
                }
                commandBuilder.Append(") ORDER BY IssueId DESC");
                // Create Command object
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = commandBuilder.ToString();

                // Build Parameter List
                if (projectId != 0)
                    sqlCmd.Parameters.Add("@projectId", SqlDbType.Int).Value = projectId;

                i = 0; //RW counter for parameters

                //RW loop thru and add non custom field queries parameters.
                foreach (QueryClause qc in queryClauses)
                {

                    if (!qc.CustomFieldQuery) //RW not a custom field query
                    {
                        //skip if value null
                        if (!string.IsNullOrEmpty(qc.FieldValue))
                        {
                            sqlCmd.Parameters.Add("@p" + i.ToString(), qc.DataType).Value = qc.FieldValue;
                        }
                        i++;
                        //sqlCmd.Parameters.Add("@p" + i.ToString(), qc.DataType).Value = qc.FieldValue;
                        //i++;
                    }
                    else
                    {
                        //DSS customfields in the same query
                        sqlCmd.Parameters.Add("@p" + i.ToString(), qc.DataType).Value = qc.FieldValue;
                        i += 1;
                    }
                }

                //RW create a new issue collection here
                List<Issue> issueList = new List<Issue>();

                //more queries, but they are not custom.
                //So, populate the collection with what we have.
                if (i > 0 && i <= queryClauseCount)
                {
                    TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);
                }

                //return distinct issues
                List<Issue> distinctIssueList = issueList.Distinct(new DistinctIssueComparer()).ToList();
                return distinctIssueList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }

        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public override ApplicationException ProcessException(Exception ex)
        {
            if (System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                MDC.Set("user", System.Web.HttpContext.Current.User.Identity.Name);

            if (Log.IsErrorEnabled)
                Log.Error(ex.Message, ex);

            return new ApplicationException(Logging.GetErrorMessageResource("DatabaseError"), ex);
        }

        /// <summary>
        /// Creates the new issue revision.
        /// </summary>
        /// <param name="newIssueRevision">The new issue revision.</param>
        /// <returns></returns>
        public override int CreateNewIssueRevision(IssueRevision newIssueRevision)
        {
            if (newIssueRevision == null)
                throw new ArgumentNullException("newIssueRevision");

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@Revision", SqlDbType.Int, 0, ParameterDirection.Input, newIssueRevision.Revision);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, newIssueRevision.IssueId);
                AddParamToSQLCmd(sqlCmd, "@Repository", SqlDbType.NVarChar, 400, ParameterDirection.Input, newIssueRevision.Repository);
                AddParamToSQLCmd(sqlCmd, "@RevisionAuthor", SqlDbType.NVarChar, 100, ParameterDirection.Input, newIssueRevision.Author);
                AddParamToSQLCmd(sqlCmd, "@RevisionDate", SqlDbType.NVarChar, 100, ParameterDirection.Input, newIssueRevision.RevisionDate);
                AddParamToSQLCmd(sqlCmd, "@RevisionMessage", SqlDbType.NText, 0, ParameterDirection.Input, newIssueRevision.Message);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEREVISION_CREATE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue revisions by issue id.
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <returns></returns>
        public override List<IssueRevision> GetIssueRevisionsByIssueId(int issueId)
        {
            if (issueId <= Globals.NewId)
                throw new ArgumentOutOfRangeException("issueId");
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEREVISION_GETISSUEREVISIONSBYISSUEID);
                List<IssueRevision> revisionList = new List<IssueRevision>();
                TExecuteReaderCmd<IssueRevision>(sqlCmd, TGenerateIssueRevisionListFromReader<IssueRevision>, ref revisionList);
                return revisionList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Deletes the issue revision.
        /// </summary>
        /// <param name="issueRevisionId">The issue revision id.</param>
        /// <returns></returns>
        public override bool DeleteIssueRevision(int issueRevisionId)
        {
            if (issueRevisionId <= 0)
                throw (new ArgumentOutOfRangeException("issueRevisionId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueRevisionId", SqlDbType.Int, 0, ParameterDirection.Input, issueRevisionId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEREVISION_DELETE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }


        /// <summary>
        /// Gets the project roadmap.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<RoadMapIssue> GetProjectRoadmap(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETROADMAP);
                List<RoadMapIssue> issueList = new List<RoadMapIssue>();
                TExecuteReaderCmd<RoadMapIssue>(sqlCmd, TGenerateRoadmapIssueListFromReader<RoadMapIssue>, ref issueList);
                return issueList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }


        /// <summary>
        /// Gets the project roadmap progress.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="milestoneId">The milestone id.</param>
        /// <returns></returns>
        public override int[] GetProjectRoadmapProgress(int projectId, int milestoneId)
        {
            if (projectId <= Globals.NewId)
                throw new ArgumentOutOfRangeException("projectId");
            if (milestoneId < -1) //allow unscheduled
                throw new ArgumentOutOfRangeException("milestoneId");
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
                AddParamToSQLCmd(sqlCmd, "@MilestoneId", SqlDbType.Int, 0, ParameterDirection.Input, milestoneId);
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETROADMAPPROGRESS);

                using (SqlConnection cn = new SqlConnection(this._connectionString))
                {
                    sqlCmd.Connection = cn;
                    cn.Open();
                    SqlDataReader returnData = sqlCmd.ExecuteReader();
                    int[] values = new int[2];
                    while (returnData.Read())
                    {
                        values[0] = (int)returnData["ClosedCount"];
                        values[1] = (int)returnData["TotalCount"];
                    }
                    return values;
                }


            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }


        /// <summary>
        /// Gets the project change log.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<Issue> GetProjectChangeLog(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETCHANGELOG);
                List<Issue> issueList = new List<Issue>();
                TExecuteReaderCmd<Issue>(sqlCmd, TGenerateIssueListFromReader<Issue>, ref issueList);
                return issueList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue status count by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<IssueCount> GetIssueStatusCountByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUESTATUSCOUNTBYPROJECT);
                List<IssueCount> issueCountList = new List<IssueCount>();
                TExecuteReaderCmd<IssueCount>(sqlCmd, TGenerateIssueCountListFromReader<IssueCount>, ref issueCountList);
                return issueCountList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue milestone count by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<IssueCount> GetIssueMilestoneCountByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUEMILESTONECOUNTBYPROJECT);
                List<IssueCount> issueCountList = new List<IssueCount>();
                TExecuteReaderCmd<IssueCount>(sqlCmd, TGenerateIssueCountListFromReader<IssueCount>, ref issueCountList);
                return issueCountList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue user count by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<IssueCount> GetIssueUserCountByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUEUSERCOUNTBYPROJECT);
                List<IssueCount> issueCountList = new List<IssueCount>();
                TExecuteReaderCmd<IssueCount>(sqlCmd, TGenerateIssueCountListFromReader<IssueCount>, ref issueCountList);
                return issueCountList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue unassigned count by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override int GetIssueUnassignedCountByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUEUNASSIGNEDCOUNTBYPROJECT);
                return (int)ExecuteScalarCmd(sqlCmd);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }

        }

        /// <summary>
        /// Gets the issue unscheduled milestone count by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override int GetIssueUnscheduledMilestoneCountByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUEUNSCHEDULEDMILESTONECOUNTBYPROJECT);
                return (int)ExecuteScalarCmd(sqlCmd);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue count by project and category.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public override int GetIssueCountByProjectAndCategory(int projectId, int categoryId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
                if(categoryId == 0)
                    AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, DBNull.Value);
                else
                    AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, categoryId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUECATEGORYCOUNTBYPROJECT);
                return (int)ExecuteScalarCmd(sqlCmd);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue type count by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<IssueCount> GetIssueTypeCountByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUETYPECOUNTBYPROJECT);
                List<IssueCount> issueCountList = new List<IssueCount>();
                TExecuteReaderCmd<IssueCount>(sqlCmd, TGenerateIssueCountListFromReader<IssueCount>, ref issueCountList);
                return issueCountList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the issue priority count by project.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<IssueCount> GetIssuePriorityCountByProject(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUE_GETISSUEPRIORITYCOUNTBYPROJECT);
                List<IssueCount> issueCountList = new List<IssueCount>();
                TExecuteReaderCmd<IssueCount>(sqlCmd, TGenerateIssueCountListFromReader<IssueCount>, ref issueCountList);
                return issueCountList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Creates the new project notification.
        /// </summary>
        /// <param name="newProjectNotification">The new project notification.</param>
        /// <returns></returns>
        public override int CreateNewProjectNotification(ProjectNotification newProjectNotification)
        {
            // Validate Parameters
            if (newProjectNotification == null)
                throw (new ArgumentNullException("newProjectNotification"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newProjectNotification.ProjectId);
                AddParamToSQLCmd(sqlCmd, "@NotificationUserName", SqlDbType.NText, 255, ParameterDirection.Input, newProjectNotification.NotificationUsername);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECTNOTIFICATION_CREATE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the project notifications by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
        public override List<ProjectNotification> GetProjectNotificationsByProjectId(int projectId)
        {
            if (projectId <= DefaultValues.GetProjectIdMinValue())
                throw (new ArgumentOutOfRangeException("projectId"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECTNOTIFICATION_GETPROJECTNOTIFICATIONSBYPROJECTID);

            List<ProjectNotification> projectNotificationList = new List<ProjectNotification>();
            TExecuteReaderCmd<ProjectNotification>(sqlCmd, TGenerateProjectNotificationListFromReader<ProjectNotification>, ref projectNotificationList);

            return projectNotificationList;
        }

        /// <summary>
        /// Deletes the project notification.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public override bool DeleteProjectNotification(int projectId, string username)
        {
            // Validate Parameters
            if (projectId <= DefaultValues.GetProjectIdMinValue())
                throw (new ArgumentOutOfRangeException("projectId"));
            if (username == String.Empty)
                throw (new ArgumentOutOfRangeException("username"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
                AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NVarChar, 255, ParameterDirection.Input, username);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECTNOTIFICATION_DELETE);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 0 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }

        /// <summary>
        /// Gets the project notifications by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public override List<ProjectNotification> GetProjectNotificationsByUsername(string username)
        {
            if (username == String.Empty)
                throw (new ArgumentOutOfRangeException("username"));

            SqlCommand sqlCmd = new SqlCommand();
            AddParamToSQLCmd(sqlCmd, "@Username", SqlDbType.NVarChar, 255, ParameterDirection.Input, username);
            SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECTNOTIFICATION_GETPROJECTNOTIFICATIONSBYUSERNAME);

            List<ProjectNotification> projectNotificationList = new List<ProjectNotification>();
            TExecuteReaderCmd<ProjectNotification>(sqlCmd, TGenerateProjectNotificationListFromReader<ProjectNotification>, ref projectNotificationList);

            return projectNotificationList;
        }

        /// <summary>
        /// Creates the new issue vote.
        /// </summary>
        /// <param name="newIssueVote">The new issue vote.</param>
        /// <returns></returns>
        public override int CreateNewIssueVote(IssueVote newIssueVote)
        {
            // Validate Parameters
            if (newIssueVote == null)
                throw (new ArgumentNullException("newIssueVote"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, newIssueVote.IssueId);
                AddParamToSQLCmd(sqlCmd, "@VoteUserName", SqlDbType.NText, 255, ParameterDirection.Input, newIssueVote.VoteUsername);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEVOTE_CREATE);
                ExecuteScalarCmd(sqlCmd);
                return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }



        /// <summary>
        /// Determines whether [has user voted] [the specified issue id].
        /// </summary>
        /// <param name="issueId">The issue id.</param>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if [has user voted] [the specified issue id]; otherwise, <c>false</c>.
        /// </returns>
        public override bool HasUserVoted(int issueId, string username)
        {
            if (issueId <= 0)
                throw new ArgumentOutOfRangeException("issueId");
            if (username == String.Empty)
                throw (new ArgumentOutOfRangeException("username"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@IssueId", SqlDbType.Int, 0, ParameterDirection.Input, issueId);
                AddParamToSQLCmd(sqlCmd, "@VoteUserName", SqlDbType.NText, 255, ParameterDirection.Input, username);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_ISSUEVOTE_HASUSERVOTED);
                ExecuteScalarCmd(sqlCmd);
                int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
                return (returnValue == 1 ? true : false);
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }

        }

        #region SQL Helper Methods

        //*********************************************************************
        //
        // SQL Helper Methods
        //
        // The following utility methods are used to interact with SQL Server.
        //
        //*********************************************************************

        
        /// <summary>
        /// Written by Stewart Moss
        /// 13-April-2010
        /// 
        /// Returns a TGenericListFromReader method pointer to enable us to have 
        /// a method which can execute full blown QueryClauses 
        /// and load it in a Generic List. 
        /// 
        /// eg 
        /// List<IssueComments> lst = IssueComments.PerformQuery(issueid, QueryClauses);                
        /// 
        /// Using this method we don't have to change the visibility of the TGenerateListFromReader
        /// methods. Nor do we have to burden the method caller with the details either.
        /// 
        /// Perhaps some kind of mapping class would be better to map theType to 
        /// a defined TGenerateListFromReader method.
        /// 
        /// Not Working. Moved to the type of code (using delegates) embedded direct 
        /// into PerformGenericQuery(). I think my declaration of this method and 
        /// PerformGernericQuery is wrong ??
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
     /*   private TGenerateListFromReader<ProjectMailbox, Project, Issue, T> GetListTypeFromReader(Type theType)
        {
            // ---------------------------------------------------------------
            //
            // The following Regular Expression yields the lines out of this provider file.
            // 
            // TGenerate\w*FromReader\<T\>\(SqlDataReader returnData, ref List\<\w*\> *\w* *\w*\) *
            //
            //TGenerateProjectMailboxListFromReader<T>(SqlDataReader returnData, ref List<ProjectMailbox> projectMailboxList)
            //TGenerateIssueCountListFromReader<T>(SqlDataReader returnData, ref List<IssueCount> issueCountList)
            //TGenerateProjectMemberRoleListFromReader<T>(SqlDataReader returnData, ref List<MemberRoles> memberRoleList)
            //TGenerateRolePermissionListFromReader<T>(SqlDataReader returnData, ref List<RolePermission> rolePermissionList)
            //TGeneratePermissionListFromReader<T>(SqlDataReader returnData, ref List<Permission> permissionList)
            //TGenerateIssueRevisionListFromReader<T>(SqlDataReader returnData, ref List<IssueRevision> revisionList)
            //TGenerateRequiredFieldListFromReader<T>(SqlDataReader returnData, ref List<RequiredField> requiredFieldList)
            //TGenerateCategoryListFromReader<T>(SqlDataReader returnData, ref List<Category> categoryList)
            //TGenerateHostSettingListFromReader<T>(SqlDataReader returnData, ref List<HostSetting> hostsettingList)
            //TGenerateProjectListFromReader<T>(SqlDataReader returnData, ref List<Project> projectList)
            //TGenerateIssueAttachmentListFromReader<T>(SqlDataReader returnData, ref List<IssueAttachment> issueAttachmentList)
            //TGenerateIssueCommentListFromReader<T>(SqlDataReader returnData, ref List<IssueComment> issueCommentList)
            //TGenerateApplicationLogListFromReader<T>(SqlDataReader returnData, ref List<ApplicationLog> applicationLogList)
            //TGenerateIssueHistoryListFromReader<T>(SqlDataReader returnData, ref List<IssueHistory> issueHistoryList)
            //TGenerateIssueNotificationListFromReader<T>(SqlDataReader returnData, ref List<IssueNotification> issueNotificationList)
            //TGenerateProjectNotificationListFromReader<T>(SqlDataReader returnData, ref List<ProjectNotification> projectNotificationList)
            //TGenerateRelatedIssueListFromReader<T>(SqlDataReader returnData, ref List<RelatedIssue> relatedIssueList)
            //TGenerateUserListFromReader<T>(SqlDataReader returnData, ref List<ITUser> userList)
            //TGenerateStatusListFromReader<T>(SqlDataReader returnData, ref List<Status> statusList)
            //TGenerateMilestoneListFromReader<T>(SqlDataReader returnData, ref List<Milestone> milestoneList)
            //TGeneratePriorityListFromReader<T>(SqlDataReader returnData, ref List<Priority> priorityList)
            //TGenerateIssueTypeListFromReader<T>(SqlDataReader returnData, ref List<IssueType> issueTypeList)
            //TGenerateQueryClauseListFromReader<T>(SqlDataReader returnData, ref List<QueryClause> queryClauseList)
            //TGenerateIssueTimeEntryListFromReader<T>(SqlDataReader returnData, ref List<IssueWorkReport> issueWorkReportList)
            //TGenerateResolutionListFromReader<T>(SqlDataReader returnData, ref List<Resolution> resolutionList)
            //TGenerateRoleListFromReader<T>(SqlDataReader returnData, ref List<Role> roleList)
            //TGenerateCustomFieldListFromReader<T>(SqlDataReader returnData, ref List<CustomField> customFieldList)
            //TGenerateCustomFieldSelectionListFromReader<T>(SqlDataReader returnData, ref List<CustomFieldSelection> customFieldSelectionList)
            //TGenerateQueryListFromReader<T>(SqlDataReader returnData, ref List<Query> queryList)
            //TGenerateIssueListFromReader<T>(SqlDataReader returnData, ref List<Issue> issueList)
            //TGenerateRoadmapIssueListFromReader<T>(SqlDataReader returnData, ref List<RoadMapIssue> issueList)
            //
            // ---------------------------------------------------------------

            if (theType == typeof(IssueComment))
            {
                return new TGenerateListFromReader<IssueComment>(TGenerateIssueCommentListFromReader<IssueComment>);
            }
            if (theType == typeof(ApplicationLog))
            {
                return new TGenerateListFromReader<ApplicationLog>(TGenerateApplicationLogListFromReader<ApplicationLog>);
            }
            if (theType == typeof(IssueHistory))
            {
                return new TGenerateListFromReader<IssueHistory>(TGenerateIssueHistoryListFromReader<IssueHistory>);
            }

            throw new ArgumentOutOfRangeException("I do not know Data Type '" + theType.ToString()+"'");
        } */

        /// <summary>
        /// Performs the query against any generic List<T>
        /// Added SMOSS 11-May-2009
        /// Modified 13-April-2010
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryClauses">The query clauses.</param>
        /// <returns></returns>
        public override void PerformGenericQuery<T>(ref List<T> List, List<QueryClause> queryClauses, string strSQL, string strOrderBy)
        {
             TGenerateListFromReader <T>  gcfr = null;// = GetListTypeFromReader(typeof(T));

            // ---------------------------------------------------------------
            //
            // The following Regular Expression yields the lines out of this provider file.
            // 
            // TGenerate\w*FromReader\<T\>\(SqlDataReader returnData, ref List\<\w*\> *\w* *\w*\) *
            //            
            //TGenerateIssueCountListFromReader<T>(SqlDataReader returnData, ref List<IssueCount> issueCountList)
            //TGenerateProjectMemberRoleListFromReader<T>(SqlDataReader returnData, ref List<MemberRoles> memberRoleList)
            //TGenerateRolePermissionListFromReader<T>(SqlDataReader returnData, ref List<RolePermission> rolePermissionList)
            //TGeneratePermissionListFromReader<T>(SqlDataReader returnData, ref List<Permission> permissionList)
            //TGenerateIssueRevisionListFromReader<T>(SqlDataReader returnData, ref List<IssueRevision> revisionList)
            //TGenerateRequiredFieldListFromReader<T>(SqlDataReader returnData, ref List<RequiredField> requiredFieldList)
            //TGenerateCategoryListFromReader<T>(SqlDataReader returnData, ref List<Category> categoryList)
            //TGenerateHostSettingListFromReader<T>(SqlDataReader returnData, ref List<HostSetting> hostsettingList)
            //TGenerateProjectListFromReader<T>(SqlDataReader returnData, ref List<Project> projectList)
            //TGenerateApplicationLogListFromReader<T>(SqlDataReader returnData, ref List<ApplicationLog> applicationLogList)
            //TGenerateIssueNotificationListFromReader<T>(SqlDataReader returnData, ref List<IssueNotification> issueNotificationList)
            //TGenerateProjectNotificationListFromReader<T>(SqlDataReader returnData, ref List<ProjectNotification> projectNotificationList)
            //TGenerateRelatedIssueListFromReader<T>(SqlDataReader returnData, ref List<RelatedIssue> relatedIssueList)
            //TGenerateUserListFromReader<T>(SqlDataReader returnData, ref List<ITUser> userList)
            //TGenerateStatusListFromReader<T>(SqlDataReader returnData, ref List<Status> statusList)
            //TGenerateMilestoneListFromReader<T>(SqlDataReader returnData, ref List<Milestone> milestoneList)
            //TGeneratePriorityListFromReader<T>(SqlDataReader returnData, ref List<Priority> priorityList)            
            //TGenerateQueryClauseListFromReader<T>(SqlDataReader returnData, ref List<QueryClause> queryClauseList)
            //TGenerateIssueTimeEntryListFromReader<T>(SqlDataReader returnData, ref List<IssueWorkReport> issueWorkReportList)
            //TGenerateResolutionListFromReader<T>(SqlDataReader returnData, ref List<Resolution> resolutionList)
            //TGenerateRoleListFromReader<T>(SqlDataReader returnData, ref List<Role> roleList)
            //TGenerateCustomFieldListFromReader<T>(SqlDataReader returnData, ref List<CustomField> customFieldList)
            //TGenerateCustomFieldSelectionListFromReader<T>(SqlDataReader returnData, ref List<CustomFieldSelection> customFieldSelectionList)
            //TGenerateQueryListFromReader<T>(SqlDataReader returnData, ref List<Query> queryList)
            //TGenerateRoadmapIssueListFromReader<T>(SqlDataReader returnData, ref List<RoadMapIssue> issueList)
            //
            // ---------------------------------------------------------------
            Type theType = typeof(T);

 
            // Wont work in a switch statement due to Type
            
            //if (theType == typeof(ProjectMailbox))
            //{
            //    System.Reflection.MethodInfo mi = this.GetType().GetMethod("TGenerateProjectMailboxListFromReader").MakeGenericMethod(new Type[] { typeof(ProjectMailbox) });
            //    gcfr = (TGenerateListFromReader<T>)Delegate.CreateDelegate(typeof(TGenerateListFromReader<ProjectMailbox>), this, mi);
            //}


            if (theType == typeof(IssueHistory))
            {
                System.Reflection.MethodInfo mi = this.GetType().GetMethod("TGenerateIssueHistoryListFromReader").MakeGenericMethod(new Type[] { typeof(IssueHistory) });
                gcfr = (TGenerateListFromReader<T>)Delegate.CreateDelegate(typeof(TGenerateListFromReader<IssueHistory>), this, mi);
            }

            //TGenerateIssueTypeListFromReader<T>(SqlDataReader returnData, ref List<IssueType> issueTypeList)
            if (theType == typeof(IssueHistory))
            {
                System.Reflection.MethodInfo mi = this.GetType().GetMethod("TGenerateIssueHistoryListFromReader").MakeGenericMethod(new Type[] { typeof(IssueHistory) });
                gcfr = (TGenerateListFromReader<T>)Delegate.CreateDelegate(typeof(TGenerateListFromReader<IssueHistory>), this, mi);
            }
            

            if (theType == typeof(IssueAttachment))
            {
                System.Reflection.MethodInfo mi = this.GetType().GetMethod("TGenerateIssueAttachmentListFromReader").MakeGenericMethod(new Type[] { typeof(IssueAttachment) });
                gcfr = (TGenerateListFromReader<T>)Delegate.CreateDelegate(typeof(TGenerateListFromReader<IssueAttachment>), this, mi);
            }

            if (theType == typeof(Issue))
            {
                System.Reflection.MethodInfo mi = this.GetType().GetMethod("TGenerateIssueListFromReader").MakeGenericMethod(new Type[] { typeof(Issue) });
                gcfr = (TGenerateListFromReader<T>)Delegate.CreateDelegate(typeof(TGenerateListFromReader<Issue>), this, mi);
            }

            if (theType == typeof(IssueComment))
            {
                System.Reflection.MethodInfo mi = this.GetType().GetMethod("TGenerateIssueCommentListFromReader").MakeGenericMethod(new Type[] { typeof(IssueComment) });
                gcfr = (TGenerateListFromReader<T>)Delegate.CreateDelegate(typeof(TGenerateListFromReader<IssueComment>), this, mi);             
            }


            // Validate Parameters
            int queryClauseCount = 0;

            //assign the queryClauses Count to our variable and then check the result.
            if ((queryClauseCount = queryClauses.Count) == 0)
                throw (new ArgumentOutOfRangeException("queryClauses == 0"));


            // Build Command Text
            StringBuilder commandBuilder = new StringBuilder();
            commandBuilder.Append(strSQL);

            int i = 0;

            //RW check for Standard Query
            foreach (QueryClause qc in queryClauses)
            {
                if (!qc.CustomFieldQuery)
                {
                    // if Field Value is null or empty do a null comparision
                    // But only if the operator is not blank
                    // "William Highfield" Method
                    if (string.IsNullOrEmpty(qc.FieldValue))
                    {
                        if (!string.IsNullOrEmpty(qc.ComparisonOperator))
                        {
                        commandBuilder.AppendFormat(" {0} {1} {2} NULL", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator);
                    }
                    else
                    {
                            // allows for William Highfield method
                            commandBuilder.AppendFormat(" {0} {1} {2}", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator);
                        }
                    }
                    else
                    {
                        commandBuilder.AppendFormat(" {0} {1} {2} @p{3}", qc.BooleanOperator, qc.FieldName, qc.ComparisonOperator, i);
                    }
                    i++;
                }
            }

            commandBuilder.Append(strOrderBy);

            // Create Command object
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = commandBuilder.ToString();

            // Build Parameter List
            //  sqlCmd.Parameters.Add("@projectId", SqlDbType.Int).Value = projectId;
            i = 0; //RW counter for parameters			

            //RW loop thru and add non custom field queries parameters.
            foreach (QueryClause qc in queryClauses)
            {
                if (!qc.CustomFieldQuery) //RW not a custom field query
                {
                    //skip if value null
                    if (!string.IsNullOrEmpty(qc.FieldValue))
                    {
                        sqlCmd.Parameters.Add("@p" + i.ToString(), qc.DataType).Value = qc.FieldValue;
                    }
                    i++;
                }
            }

            if (gcfr != null)
            {
                TExecuteReaderCmd<T>(sqlCmd, gcfr, ref List);
                //note we are passing this methods objects by reference because we want
                //the collection to be added to. If we don't pass by reference, we lose the collection
                //when the method returns.
            }


        } //end PerformGenericQuery



        /// <summary>
        /// Adds the param to SQL CMD.
        /// </summary>
        /// <param name="sqlCmd">The SQL CMD.</param>
        /// <param name="paramId">The param id.</param>
        /// <param name="sqlType">Type of the SQL.</param>
        /// <param name="paramSize">Size of the param.</param>
        /// <param name="paramDirection">The param direction.</param>
        /// <param name="paramvalue">The paramvalue.</param>
        private void AddParamToSQLCmd(SqlCommand sqlCmd, string paramId, SqlDbType sqlType, int paramSize, ParameterDirection paramDirection, object paramvalue)
        {
            // Validate Parameter Properties
            if (sqlCmd == null)
                throw (new ArgumentNullException("sqlCmd"));
            if (paramId == string.Empty)
                throw (new ArgumentOutOfRangeException("paramId"));

            // Add Parameter
            SqlParameter newSqlParam = new SqlParameter();
            newSqlParam.ParameterName = paramId;
            newSqlParam.SqlDbType = sqlType;
            newSqlParam.Direction = paramDirection;

            if (paramSize > 0)
                newSqlParam.Size = paramSize;

            if (paramvalue != null)
                newSqlParam.Value = paramvalue;

            sqlCmd.Parameters.Add(newSqlParam);
        }


        /// <summary>
        /// Executes the scalar CMD.
        /// </summary>
        /// <param name="sqlCmd">The SQL CMD.</param>
        /// <returns></returns>
        private Object ExecuteScalarCmd(SqlCommand sqlCmd)
        {
            // Validate Command Properties
            if (_connectionString == string.Empty)
                throw (new ArgumentOutOfRangeException("_connectionString"));

            if (sqlCmd == null)
                throw (new ArgumentNullException("sqlCmd"));

            Object result = null;

            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                sqlCmd.Connection = cn;
                cn.Open();
                result = sqlCmd.ExecuteScalar();
            }

            return result;
        }


        /// <summary>
        /// Ts the execute reader CMD.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlCmd">The SQL CMD.</param>
        /// <param name="gcfr">The GCFR.</param>
        /// <param name="List">The list.</param>
        private void TExecuteReaderCmd<T>(SqlCommand sqlCmd, TGenerateListFromReader<T> gcfr, ref List<T> List)
        {
            if (_connectionString == string.Empty)
                throw (new ArgumentOutOfRangeException("_connectionString"));
            if (sqlCmd == null)
                throw (new ArgumentNullException("sqlCmd"));

            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                sqlCmd.Connection = cn;
                cn.Open();
                gcfr(sqlCmd.ExecuteReader(), ref List);
            }
        }

        /// <summary>
        /// Sets the type of the command.
        /// </summary>
        /// <param name="sqlCmd">The SQL CMD.</param>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        private void SetCommandType(SqlCommand sqlCmd, CommandType cmdType, string cmdText)
        {
            sqlCmd.CommandType = cmdType;
            sqlCmd.CommandText = cmdText;
        }

        #endregion

        #region GENARATE LIST HELPER METHODS

        /*****************************  GENARATE LIST HELPER METHODS  *****************************/

        /// <summary>
        /// Ts the generate project mailbox list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="projectMailboxList">The project mailbox list.</param>
        public void TGenerateProjectMailboxListFromReader<T>(SqlDataReader returnData, ref List<ProjectMailbox> projectMailboxList)
        {
            while (returnData.Read())
            {
                ProjectMailbox mailbox = new ProjectMailbox((int)returnData["ProjectMailboxId"],(string)returnData["Mailbox"], (int)returnData["ProjectId"]
                    , returnData["AssignToUserId"] == DBNull.Value ? Guid.Empty : (Guid)returnData["AssignToUserId"]
                    , (int)returnData["IssueTypeId"], returnData["AssignToName"] == DBNull.Value ? string.Empty : (string)returnData["AssignToName"], (string)returnData["IssueTypeName"]);

                projectMailboxList.Add(mailbox);
            }
        }
        //Iman Mayes
        /// <summary>
        /// Ts the generate issue count list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueCountList">The issue count list.</param>
        private void TGenerateIssueCountListFromReader<T>(SqlDataReader returnData, ref List<IssueCount> issueCountList)
        {
            while (returnData.Read())
            {
                IssueCount issueCount = new IssueCount(returnData.GetValue(2), (string)returnData.GetValue(0), (int)returnData.GetValue(1), (string)returnData.GetValue(3));
                issueCountList.Add(issueCount);
            }
        }

        /// <summary>
        /// Ts the generate member roles list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="memberRoleList">The member roles list.</param>
        private void TGenerateProjectMemberRoleListFromReader<T>(SqlDataReader returnData, ref List<MemberRoles> memberRoleList)
        {
            while (returnData.Read())
            {
                int i = 0;
                bool found = false;
                for (i = 0; i < memberRoleList.Count; i++)
                {
                    if (memberRoleList[i].Username.Equals((string)returnData.GetValue(0)))
                    {
                        memberRoleList[i].AddRole((string)returnData.GetValue(1));
                        found = true;
                    }                    
                }
                if (!found) {
                    MemberRoles memberRoles = new MemberRoles((string)returnData.GetValue(0), (string)returnData.GetValue(1));
                    memberRoleList.Add(memberRoles);
                }
            }
        }

        /// <summary>
        /// Ts the generate role permission list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="rolePermissionList">The role permission list.</param>
        private void TGenerateRolePermissionListFromReader<T>(SqlDataReader returnData, ref List<RolePermission> rolePermissionList)
        {
            while (returnData.Read())
            {
                RolePermission permission = new RolePermission((int)returnData["PermissionId"], (int)returnData["ProjectId"], returnData["RoleName"].ToString(),
                    returnData["PermissionName"].ToString(), returnData["PermissionKey"].ToString());
                rolePermissionList.Add(permission);
            }
        }

        /// <summary>
        /// Ts the generate permission list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="permissionList">The permission list.</param>
        private void TGeneratePermissionListFromReader<T>(SqlDataReader returnData, ref List<Permission> permissionList)
        {
            while (returnData.Read())
            {
                Permission permission = new Permission((int)returnData["PermissionId"], returnData["PermissionName"].ToString(), returnData["PermissionKey"].ToString());
                permissionList.Add(permission);
            }
        }


        /// <summary>
        /// Ts the generate issue revision list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="revisionList">The revision list.</param>
        private void TGenerateIssueRevisionListFromReader<T>(SqlDataReader returnData, ref List<IssueRevision> revisionList)
        {
            while (returnData.Read())
            {
                IssueRevision issueRevision = new IssueRevision((int)returnData["IssueRevisionId"], (int)returnData["IssueId"], (int)returnData["Revision"],
                    returnData["RevisionAuthor"].ToString(), returnData["RevisionMessage"].ToString(), returnData["Repository"].ToString(), returnData["RevisionDate"].ToString(),
                    (DateTime)returnData["DateCreated"]);
                revisionList.Add(issueRevision);
            }
        }

        /// <summary>
        /// Ts the generate required field list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="requiredFieldList">The required field list.</param>
        private void TGenerateRequiredFieldListFromReader<T>(SqlDataReader returnData, ref List<RequiredField> requiredFieldList)
        {
            while (returnData.Read())
            {
                RequiredField newRequiredField = new RequiredField(returnData["FieldName"].ToString(), returnData["FieldValue"].ToString());
                requiredFieldList.Add(newRequiredField);
            }
        }

        /// <summary>
        /// Ts the generate category list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="categoryList">The category list.</param>
        private void TGenerateCategoryListFromReader<T>(SqlDataReader returnData, ref List<Category> categoryList)
        {
            while (returnData.Read())
            {
                Category category = new Category((int)returnData["CategoryId"], (int)returnData["ProjectId"], (int)returnData["ParentCategoryId"], (string)returnData["CategoryName"], (int)returnData["ChildCount"]);
                categoryList.Add(category);
            }
        }

        /// <summary>
        /// Ts the generate host setting list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="hostsettingList">The hostsetting list.</param>
        private void TGenerateHostSettingListFromReader<T>(SqlDataReader returnData, ref List<HostSetting> hostsettingList)
        {
            while (returnData.Read())
            {
                HostSetting hostsetting = new HostSetting((string)returnData["SettingName"], (string)returnData["SettingValue"]);
                hostsettingList.Add(hostsetting);
            }
        }

        /// <summary>
        /// Ts the generate project list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="projectList">The project list.</param>
        private void TGenerateProjectListFromReader<T>(SqlDataReader returnData, ref List<Project> projectList)
        {
            while (returnData.Read())
            {
                Project project = new Project((int)returnData["ProjectId"], (string)returnData["ProjectName"],
                    (string)returnData["ProjectCode"], (string)returnData["ProjectDescription"], (string)returnData["ManagerUserName"],
                    (string)returnData["ManagerDisplayName"],(string)returnData["ManagerUserName"], (string)returnData["CreatorDisplayName"],
                    (string)returnData["AttachmentUploadPath"], (DateTime)returnData["DateCreated"],
                    (Globals.ProjectAccessType)returnData["ProjectAccessType"], (bool)returnData["ProjectDisabled"],
                    (bool)returnData["AllowAttachments"], (Guid)returnData["ProjectManagerUserId"],
                    (IssueAttachmentStorageType)returnData["AttachmentStorageType"], returnData["SvnRepositoryUrl"].ToString(), (bool)returnData["AllowIssueVoting"], 
                    null);
                projectList.Add(project);
            }
        }


        /// <summary>
        /// Ts the generate issue attachment list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueAttachmentList">The issue attachment list.</param>
        public void TGenerateIssueAttachmentListFromReader<T>(SqlDataReader returnData, ref List<IssueAttachment> issueAttachmentList)
        {
            while (returnData.Read())
            {
                IssueAttachment newAttachment = new IssueAttachment((int)returnData["IssueAttachmentId"], (int)returnData["IssueId"], (string)returnData["CreatorUsername"], (string)returnData["CreatorDisplayName"], (DateTime)returnData["DateCreated"], (string)returnData["FileName"], (int)returnData["FileSize"], (string)returnData["Description"]);
                issueAttachmentList.Add(newAttachment);
            }
        }

        /// <summary>
        /// Ts the generate issue comment list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueCommentList">The issue comment list.</param>
        public void TGenerateIssueCommentListFromReader<T>(SqlDataReader returnData, ref List<IssueComment> issueCommentList)
        {
            while (returnData.Read())
            {
                IssueComment newAttachment = new IssueComment((int)returnData["IssueCommentId"], (int)returnData["IssueId"], (string)returnData["Comment"], (string)returnData["CreatorUsername"], (Guid)returnData["CreatorUserId"], (string)returnData["CreatorDisplayName"], (DateTime)returnData["DateCreated"]);
                issueCommentList.Add(newAttachment);
            }
        }

        /// <summary>
        /// Ts the generate application log list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="applicationLogList">The application log list.</param>
        private void TGenerateApplicationLogListFromReader<T>(SqlDataReader returnData, ref List<ApplicationLog> applicationLogList)
        {
            while (returnData.Read())
            {
                ApplicationLog newAppLog = new ApplicationLog((int)returnData["Id"], (DateTime)returnData["Date"], (string)returnData["Thread"], (string)returnData["Level"], (string)returnData["User"], (string)returnData["Logger"], (string)returnData["Message"], (string)returnData["Exception"]);
                applicationLogList.Add(newAppLog);
            }
        }

        /// <summary>
        /// Ts the generate issue history list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueHistoryList">The issue history list.</param>
        public void TGenerateIssueHistoryListFromReader<T>(SqlDataReader returnData, ref List<IssueHistory> issueHistoryList)
        {
            while (returnData.Read())
            {
                IssueHistory newHistory = new IssueHistory((int)returnData["IssueHistoryId"], (int)returnData["IssueId"], (string)returnData["CreatorUsername"], (string)returnData["CreatorDisplayName"], (string)returnData["FieldChanged"], (string)returnData["OldValue"], (string)returnData["NewValue"], (DateTime)returnData["DateCreated"]);
                issueHistoryList.Add(newHistory);
            }
        }

        /// <summary>
        /// Ts the generate issue notification list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueNotificationList">The issue notification list.</param>
        private void TGenerateIssueNotificationListFromReader<T>(SqlDataReader returnData, ref List<IssueNotification> issueNotificationList)
        {
            while (returnData.Read())
            {
                //IssueNotification newNotification = new IssueNotification((int)returnData["IssueNotificationId"], (int)returnData["IssueId"], (string)returnData["NotificationUsername"], (string)returnData["NotificationEmail"], (string)returnData["NotificationDisplayName"]);
                IssueNotification newNotification = new IssueNotification(1, (int)returnData["IssueId"], (string)returnData["NotificationUsername"], (string)returnData["NotificationEmail"], (string)returnData["NotificationDisplayName"]);
                issueNotificationList.Add(newNotification);
            }
        }

        /// <summary>
        /// Ts the generate project notification list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="projectNotificationList">The project notification list.</param>
        private void TGenerateProjectNotificationListFromReader<T>(SqlDataReader returnData, ref List<ProjectNotification> projectNotificationList)
        {
            while (returnData.Read())
            {
                ProjectNotification newNotification = new ProjectNotification((int)returnData["ProjectNotificationId"], (int)returnData["ProjectId"], (string)returnData["ProjectName"], (string)returnData["NotificationUsername"], (string)returnData["NotificationEmail"], (string)returnData["NotificationDisplayName"]);
                projectNotificationList.Add(newNotification);
            }
        }

        /// <summary>
        /// Ts the generate related issue list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="relatedIssueList">The related issue list.</param>
        private void TGenerateRelatedIssueListFromReader<T>(SqlDataReader returnData, ref List<RelatedIssue> relatedIssueList)
        {
            while (returnData.Read())
            {
                RelatedIssue newRelatedIssue = new RelatedIssue((int)returnData["IssueId"], (string)returnData["IssueTitle"],returnData["IssueStatus"] == DBNull.Value ? string.Empty : (string)returnData["IssueStatus"], returnData["IssueResolution"] == DBNull.Value ? string.Empty : (string)returnData["IssueResolution"], (DateTime)returnData["DateCreated"]);
                relatedIssueList.Add(newRelatedIssue);
            }
        }

        /// <summary>
        /// Ts the generate user list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="userList">The user list.</param>
        private void TGenerateUserListFromReader<T>(SqlDataReader returnData, ref List<ITUser> userList)
        {
            while (returnData.Read())
            {
                ITUser user = new ITUser((Guid)returnData["UserId"], (string)returnData["UserName"], (string)returnData["DisplayName"]);
                userList.Add(user);
            }
        }

        /// <summary>
        /// Ts the generate status list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="statusList">The status list.</param>
        private void TGenerateStatusListFromReader<T>(SqlDataReader returnData, ref List<Status> statusList)
        {
            while (returnData.Read())
            {
                Status status = new Status((int)returnData["StatusId"], (int)returnData["ProjectId"], (string)returnData["StatusName"],
                    (int)returnData["SortOrder"], (string)returnData["StatusImageUrl"], (bool)returnData["IsClosedState"]);
                statusList.Add(status);
            }
        }

        /// <summary>
        /// Ts the generate milestone list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="milestoneList">The milestone list.</param>
        private void TGenerateMilestoneListFromReader<T>(SqlDataReader returnData, ref List<Milestone> milestoneList)
        {
            while (returnData.Read())
            {
                Milestone milestone = new Milestone((int)returnData["MilestoneId"], (int)returnData["ProjectId"],
                    (string)returnData["MilestoneName"], (int)returnData["SortOrder"], (string)returnData["MilestoneImageUrl"],
                    returnData["MilestoneDueDate"] == DBNull.Value ? null : (DateTime?)returnData["MilestoneDueDate"], 
                    returnData["MilestoneReleaseDate"] == DBNull.Value ? null : (DateTime?)returnData["MilestoneReleaseDate"],(bool)returnData["MilestoneCompleted"],
                    returnData["MilestoneNotes"] == DBNull.Value ? string.Empty : (string)returnData["MilestoneNotes"]);
                milestoneList.Add(milestone);
            }
        }


        /// <summary>
        /// Ts the generate priority list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="priorityList">The priority list.</param>
        private void TGeneratePriorityListFromReader<T>(SqlDataReader returnData, ref List<Priority> priorityList)
        {
            while (returnData.Read())
            {
                Priority priority = new Priority((int)returnData["PriorityId"], (int)returnData["ProjectId"], (string)returnData["PriorityName"], (int)returnData["SortOrder"],
                    (string)returnData["PriorityImageUrl"]);
                priorityList.Add(priority);
            }
        }


        /// <summary>
        /// Ts the generate issue type list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueTypeList">The issue type list.</param>
        private void TGenerateIssueTypeListFromReader<T>(SqlDataReader returnData, ref List<IssueType> issueTypeList)
        {
            while (returnData.Read())
            {
                IssueType issueType = new IssueType((int)returnData["IssueTypeId"], (int)returnData["ProjectId"], (string)returnData["IssueTypeName"], (int)returnData["SortOrder"], (string)returnData["IssueTypeImageUrl"]);
                issueTypeList.Add(issueType);
            }
        }

        /// <summary>
        /// Ts the generate query clause list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="queryClauseList">The query clause list.</param>
        private void TGenerateQueryClauseListFromReader<T>(SqlDataReader returnData, ref List<QueryClause> queryClauseList)
        {
            while (returnData.Read())
            {
                QueryClause queryClause = new QueryClause((string)returnData["BooleanOperator"], (string)returnData["FieldName"], (string)returnData["ComparisonOperator"], (string)returnData["FieldValue"], (SqlDbType)returnData["DataType"], (bool)returnData["IsCustomField"]);
                queryClauseList.Add(queryClause);
            }
        }

        /// <summary>
        /// Ts the generate issue time entry list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueTimeEntryList">The issue time entry list.</param>
        private void TGenerateIssueTimeEntryListFromReader<T>(SqlDataReader returnData, ref List<IssueWorkReport> issueWorkReportList)
        {
            while (returnData.Read())
            {
                IssueWorkReport issueWorkReport = new IssueWorkReport((int)returnData["IssueWorkReportId"], (int)returnData["IssueId"], (Guid)returnData["CreatorUserId"], (DateTime)returnData["WorkDate"], (decimal)returnData["Duration"], (int)returnData["IssueCommentId"], (string)returnData["Comment"], (string)returnData["CreatorUserName"], (string)returnData["CreatorDisplayName"]);
                issueWorkReportList.Add(issueWorkReport);
            }
        }


        /// <summary>
        /// Ts the generate resolution list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="resolutionList">The resolution list.</param>
        private void TGenerateResolutionListFromReader<T>(SqlDataReader returnData, ref List<Resolution> resolutionList)
        {
            while (returnData.Read())
            {
                Resolution resolution = new Resolution((int)returnData["ResolutionId"], (int)returnData["ProjectId"], (string)returnData["ResolutionName"], (int)returnData["SortOrder"], (string)returnData["ResolutionImageUrl"]);
                resolutionList.Add(resolution);
            }
        }

        /// <summary>
        /// Ts the generate role list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="roleList">The role list.</param>
        private void TGenerateRoleListFromReader<T>(SqlDataReader returnData, ref List<Role> roleList)
        {
            while (returnData.Read())
            {
                int ProjectId = 0;
                if (returnData["ProjectId"] != DBNull.Value)
                    ProjectId = Convert.ToInt32(returnData["ProjectId"]);

                Role role = new Role((int)returnData["RoleId"], ProjectId, (string)returnData["RoleName"], (string)returnData["RoleDescription"], (bool)returnData["AutoAssign"]);
                roleList.Add(role);
            }
        }

        /// <summary>
        /// Ts the generate custom field list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="customFieldList">The custom field list.</param>
        private void TGenerateCustomFieldListFromReader<T>(SqlDataReader returnData, ref List<CustomField> customFieldList)
        {
            while (returnData.Read())
            {

                CustomField cf = new CustomField((int)returnData["CustomFieldId"],
                            (int)returnData["ProjectId"],
                            (string)returnData["CustomFieldName"],
                            (ValidationDataType)returnData["CustomFieldDataType"],
                            (bool)returnData["CustomFieldRequired"],
                            (string)returnData["CustomFieldValue"], (CustomField.CustomFieldType)returnData["CustomFieldTypeId"]);
                customFieldList.Add(cf);
            }
        }

        /// <summary>
        /// Ts the generate custom field selection list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="customFieldList">The custom field list.</param>
        private void TGenerateCustomFieldSelectionListFromReader<T>(SqlDataReader returnData, ref List<CustomFieldSelection> customFieldSelectionList)
        {
            while (returnData.Read())
            {

                CustomFieldSelection cfs = new CustomFieldSelection((int)returnData["CustomFieldSelectionId"],
                                (int)returnData["CustomFieldId"],
                                (string)returnData["CustomFieldSelectionName"],
                                (string)returnData["CustomFieldSelectionValue"],
                                (int)returnData["CustomFieldSelectionSortOrder"]);
                customFieldSelectionList.Add(cfs);
            }
        }

        /// <summary>
        /// Ts the generate query list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="queryList">The query list.</param>
        private void TGenerateQueryListFromReader<T>(SqlDataReader returnData, ref List<Query> queryList)
        {
            while (returnData.Read())
            {
                Query q = new Query((int)returnData["QueryId"], (string)returnData["QueryName"], (bool)returnData["IsPublic"]);
                queryList.Add(q);
            }
        }

        /// <summary>
        /// Ts the generate issue list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueList">The issue list.</param>
        public void TGenerateIssueListFromReader<T>(SqlDataReader returnData, ref List<Issue> issueList)
        {
            while (returnData.Read())
            {
                Issue issue = new Issue((int)returnData["IssueId"],
                    (int)returnData["ProjectId"],
                    (string)returnData["ProjectName"],
                    (string)returnData["ProjectCode"],
                    (string)returnData["IssueTitle"],
                    (string)returnData["IssueDescription"],
                    returnData["IssueCategoryId"] == DBNull.Value ? 0 : (int)returnData["IssueCategoryId"],
                    (string)returnData["CategoryName"],
                    (int)returnData["IssuePriorityId"],
                    (string)returnData["PriorityName"],
                    (string)returnData["PriorityImageUrl"],
                    (int)returnData["IssueStatusId"],
                    (string)returnData["StatusName"],
                    (string)returnData["StatusImageUrl"],
                    (int)returnData["IssueTypeId"],
                    (string)returnData["IssueTypeName"],
                    (string)returnData["IssueTypeImageUrl"],
                    returnData["IssueResolutionId"] == DBNull.Value ? 0 : (int)returnData["IssueResolutionId"],
                    (string)returnData["ResolutionName"],
                    (string)returnData["ResolutionImageUrl"],
                    (string)returnData["AssignedDisplayName"],
                    (string)returnData["AssignedUserName"],
                    returnData["IssueAssignedUserId"] == DBNull.Value ? Guid.Empty : (Guid)returnData["IssueAssignedUserId"],
                    (string)returnData["CreatorDisplayName"],
                    (string)returnData["CreatorUserName"],
                    (Guid)returnData["IssueCreatorUserId"],
                    (string)returnData["OwnerDisplayName"],
                    (string)returnData["OwnerUserName"],
                    returnData["IssueOwnerUserId"] == DBNull.Value ? Guid.Empty : (Guid)returnData["IssueOwnerUserId"],
                    returnData["IssueDueDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)returnData["IssueDueDate"],
                    returnData["IssueMilestoneId"] == DBNull.Value ? 0 : (int)returnData["IssueMilestoneId"],
                    (string)returnData["MilestoneName"],
                    (string)returnData["MilestoneImageUrl"],
                     returnData["MilestoneDueDate"] == DBNull.Value ? null : (DateTime?)returnData["MilestoneDueDate"],
                      returnData["IssueAffectedMilestoneId"] == DBNull.Value ? 0 : (int)returnData["IssueAffectedMilestoneId"],
                    (string)returnData["AffectedMilestoneName"],
                    (string)returnData["AffectedMilestoneImageUrl"],
                    (int)returnData["IssueVisibility"],
                    Convert.ToDouble(returnData["TimeLogged"].ToString()),
                    Convert.ToDecimal(returnData["IssueEstimation"].ToString()),
                    (DateTime)returnData["DateCreated"],
                    (DateTime)returnData["LastUpdate"],
                    (string)returnData["LastUpdateUserName"],
                    (string)returnData["LastUpdateDisplayName"],
                    (int)returnData["IssueProgress"],
                    (bool)returnData["Disabled"],
                    (int)returnData["IssueVotes"]);

                issueList.Add(issue);
            }
        }

        /// <summary>
        /// Ts the generate roadmap issue list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="issueList">The issue list.</param>
        private void TGenerateRoadmapIssueListFromReader<T>(SqlDataReader returnData, ref List<RoadMapIssue> issueList)
        {
            while (returnData.Read())
            {
                RoadMapIssue issue = new RoadMapIssue((int)returnData["MilestoneSortOrder"],(int)returnData["IssueId"],
                    (int)returnData["ProjectId"],
                    (string)returnData["ProjectName"],
                    (string)returnData["ProjectCode"],
                    (string)returnData["IssueTitle"],
                    (string)returnData["IssueDescription"],
                    returnData["IssueCategoryId"] == DBNull.Value ? 0 : (int)returnData["IssueCategoryId"],
                    (string)returnData["CategoryName"],
                    (int)returnData["IssuePriorityId"],
                    (string)returnData["PriorityName"],
                    (string)returnData["PriorityImageUrl"],
                    (int)returnData["IssueStatusId"],
                    (string)returnData["StatusName"],
                    (string)returnData["StatusImageUrl"],
                    (int)returnData["IssueTypeId"],
                    (string)returnData["IssueTypeName"],
                    (string)returnData["IssueTypeImageUrl"],
                    returnData["IssueResolutionId"] == DBNull.Value ? 0 : (int)returnData["IssueResolutionId"],
                    (string)returnData["ResolutionName"],
                    (string)returnData["ResolutionImageUrl"],
                    (string)returnData["AssignedDisplayName"],
                    (string)returnData["AssignedUserName"],
                    returnData["IssueAssignedUserId"] == DBNull.Value ? Guid.Empty : (Guid)returnData["IssueAssignedUserId"],
                    (string)returnData["CreatorDisplayName"],
                    (string)returnData["CreatorUserName"],
                    (Guid)returnData["IssueCreatorUserId"],
                    (string)returnData["OwnerDisplayName"],
                    (string)returnData["OwnerUserName"],
                    returnData["IssueOwnerUserId"] == DBNull.Value ? Guid.Empty : (Guid)returnData["IssueOwnerUserId"],
                    returnData["IssueDueDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)returnData["IssueDueDate"],
                    returnData["IssueMilestoneId"] == DBNull.Value ? 0 : (int)returnData["IssueMilestoneId"],
                    (string)returnData["MilestoneName"],
                    (string)returnData["MilestoneImageUrl"],
                     returnData["MilestoneDueDate"] == DBNull.Value ? null : (DateTime?)returnData["MilestoneDueDate"],
                      returnData["IssueAffectedMilestoneId"] == DBNull.Value ? 0 : (int)returnData["IssueAffectedMilestoneId"],
                    (string)returnData["AffectedMilestoneName"],
                    (string)returnData["AffectedMilestoneImageUrl"],
                    (int)returnData["IssueVisibility"],
                    Convert.ToDouble(returnData["TimeLogged"].ToString()),
                    Convert.ToDecimal(returnData["IssueEstimation"].ToString()),
                    (DateTime)returnData["DateCreated"],
                    (DateTime)returnData["LastUpdate"],
                    (string)returnData["LastUpdateUserName"],
                    (string)returnData["LastUpdateDisplayName"],
                    (int)returnData["IssueProgress"],
                    (bool)returnData["Disabled"],
                    (int)returnData["IssueVotes"]);

                issueList.Add(issue);
            }
        }

        /// <summary>
        /// Ts the generate installed resources list from reader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnData">The return data.</param>
        /// <param name="cultureCodes">The culture codes.</param>
        private void TGenerateInstalledResourcesListFromReader<T>(SqlDataReader returnData, ref List<string> cultureCodes)
        {
            while (returnData.Read())
            {
                cultureCodes.Add((string)returnData["cultureCode"]);
            }
        }

        /// Iman Mayes
        /// <summary>
        /// Gets all users and roles by project Id
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns>Generic list of username strings and role strings</returns>
        public override List<MemberRoles> GetProjectMembersRoles(int projectId)
        {
            if (projectId <= 0)
                throw (new ArgumentOutOfRangeException("projectId"));

            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();

                AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
                AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETMEMBERROLESBYPROJECTID);
                List<MemberRoles> projectMemberRoleList = new List<MemberRoles>();
                TExecuteReaderCmd<MemberRoles>(sqlCmd, TGenerateProjectMemberRoleListFromReader<MemberRoles>, ref projectMemberRoleList);
                return projectMemberRoleList;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }            
        }

        #endregion
    
        /// <summary>
        /// Gets the installed language resources.
        /// </summary>
        /// <returns></returns>
        public override List<string> GetInstalledLanguageResources()
        {
            try
            {
                // Execute SQL Command
                SqlCommand sqlCmd = new SqlCommand();
           
                SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_STRINGRESOURCES_GETINSTALLEDLANGUAGERESOURCES);
                List<string> cultureCodes = new List<string>();
                TExecuteReaderCmd<string>(sqlCmd, TGenerateInstalledResourcesListFromReader<string>, ref cultureCodes);
                return cultureCodes;
            }
            catch (Exception ex)
            {
                throw ProcessException(ex);
            }
        }


        /// <summary>
        /// Gets the provider path.
        /// </summary>
        /// <returns></returns>
        public override string GetProviderPath()
        {
            HttpContext context =  HttpContext.Current;

            if(_providerPath != string.Empty)
            {
                if (_mappedProviderPath == string.Empty)
                { 
                    _mappedProviderPath = context.Server.MapPath(_providerPath);
                    if (!Directory.Exists(_mappedProviderPath))
                         return string.Format("ERROR: providerPath folder {0} specified for SqlDataProvider does not exist on web server", _mappedProviderPath);                       
                }
            }
            else
            {
                return "ERROR: providerPath folder value not specified in web.config for SqlDataProvider";
            }
            return _mappedProviderPath;
        }

        /// <summary>
        /// Gets the database version.
        /// </summary>
        /// <returns></returns>
        public override string GetDatabaseVersion()
        {
            SqlCommand sqlCmd = new SqlCommand();              
            string CurrentVersion = string.Empty;

            //Check For 0.7 Version
            try
            {
                SetCommandType(sqlCmd, CommandType.Text, "SELECT SettingValue FROM HostSettings WHERE SettingName='Version'");    
                CurrentVersion = (string)ExecuteScalarCmd(sqlCmd);
            }
            catch (SqlException e)
            {
                CurrentVersion = "ERROR";
            }
           
            //check for version 0.8 if 0.7 has not already been found or threw an exception
            if (CurrentVersion == string.Empty || CurrentVersion == "ERROR")
            {
                try
                {
                    SetCommandType(sqlCmd, CommandType.Text,"SELECT SettingValue FROM BugNet_HostSettings WHERE SettingName='Version'");
                    CurrentVersion = (string)ExecuteScalarCmd(sqlCmd);
                }
                catch (SqlException e)
                {
                    switch (e.Number)
                    {
                        case 4060:
                            return "ERROR " + e.Message;
                    }
                    CurrentVersion = string.Empty;
                }
              
            }

            return CurrentVersion;
        }

        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        public override void ExecuteScript(List<string> sql)
        {
            if (sql == null)
                throw new ArgumentNullException("sql");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                foreach (string stmt in sql)
                {        
                    SqlCommand command = new SqlCommand(stmt, conn);
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

       
    }
}
