using System;
using System.Collections;
using BugNET.DataAccessLayer;

namespace BugNET.BusinessLogicLayer
{
	/// <summary>
	/// Summary description for ProjectMailbox.
	/// </summary>
	public class ProjectMailbox
	{
		#region Private Variables
		private string _mailbox;
		private int _projectId;
		private Guid _assignToId;
		private int _issueTypeId;
		private string _AssignToName;
		private string _IssueTypeName;
		private int _Id;
		#endregion

		#region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectMailbox"/> class.
        /// </summary>
        /// <param name="mailbox">The mailbox.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="assignToName">Name of the assign to.</param>
        /// <param name="assignToId">The assign to id.</param>
        /// <param name="issueTypeId">The issue type id.</param>
		public ProjectMailbox(string mailbox, int projectId, string assignToName,Guid assignToId, int issueTypeId)
		{
			_mailbox = mailbox;
            _AssignToName = assignToName;
			_projectId = projectId;
			_assignToId = assignToId;
			_issueTypeId = issueTypeId;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ProjectMailbox"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="mailbox">The mailbox.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="assignToId">The assign to id.</param>
        /// <param name="issueTypeId">The issue type id.</param>
        /// <param name="assignToName">Name of the assign to.</param>
        /// <param name="issueTypeName">Name of the issue type.</param>
		public ProjectMailbox(int id,string mailbox, int projectId,Guid assignToId, int issueTypeId, string assignToName, string issueTypeName)
		{
			_Id = id;
			_mailbox = mailbox;
			_projectId = projectId;
			_assignToId = assignToId;
			_issueTypeId = issueTypeId;
			_AssignToName = assignToName;
			_IssueTypeName = issueTypeName;
		}
		#endregion

		#region Properties
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
		public int Id
		{
			get{return _Id;}
		}
        /// <summary>
        /// Gets the mailbox.
        /// </summary>
        /// <value>The mailbox.</value>
		public string Mailbox
		{
			get{return _mailbox;}
		}
        /// <summary>
        /// Gets the project id.
        /// </summary>
        /// <value>The project id.</value>
		public int ProjectId
		{
			get{return _projectId;}
		}
        /// <summary>
        /// Gets the assign to ID.
        /// </summary>
        /// <value>The assign to ID.</value>
		public Guid AssignToId
		{
			get{return _assignToId;}
		}
        /// <summary>
        /// Gets the issue type id.
        /// </summary>
        /// <value>The issue type id.</value>
		public int IssueTypeId
		{
			get{return _issueTypeId;}
		}
        /// <summary>
        /// Gets the name of the assign to.
        /// </summary>
        /// <value>The name of the assign to.</value>
		public string AssignToName
		{
			get{return _AssignToName;}
		}
        /// <summary>
        /// Gets the name of the issue type.
        /// </summary>
        /// <value>The name of the issue type.</value>
		public string IssueTypeName
		{
			get{return _IssueTypeName;}
		}
		#endregion

		#region Static Methods
        /// <summary>
        /// Gets the project by mailbox.
        /// </summary>
        /// <param name="mailbox">The mailbox.</param>
        /// <returns></returns>
		public static ProjectMailbox GetProjectByMailbox(string mailbox) 
		{
			if (mailbox == null || mailbox.Length == 0)
				throw (new ArgumentOutOfRangeException("mailbox"));
					
            return DataProviderManager.Provider.GetProjectByMailbox(mailbox);
		}

        /// <summary>
        /// Gets the mailboxs by project id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <returns></returns>
		public static IList GetMailboxsByProjectId(int projectId) 
		{
			if (projectId <= 0)
				throw (new ArgumentOutOfRangeException("projectId"));
			
            return DataProviderManager.Provider.GetMailboxsByProjectId(projectId);
		}

		#endregion

		#region Instance Methods
        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <returns></returns>
		public bool Add() 
		{
            return DataProviderManager.Provider.CreateProjectMailbox(this);
		}
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
		public bool Delete() 
		{
            return DataProviderManager.Provider.DeleteProjectMailbox(this.Id);
		}
		#endregion

	}
}
