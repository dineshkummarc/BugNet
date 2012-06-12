using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using BugNET.DataAccessLayer;

namespace BugNET.BusinessLogicLayer
{
	/// <summary>
	/// Summary description for Milestone.
	/// </summary>
	public class Milestone
	{
		#region Private Variables
			private int         _Id;
			private int         _ProjectId;
			private string      _Name;
            private int         _SortOrder;
            private string      _ImageUrl;
            private DateTime?   _DueDate;
            private DateTime?   _ReleaseDate;
            private bool        _IsCompleted;
            private string      _Notes;
		#endregion

		#region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="T:Milestone"/> class.
            /// </summary>
            /// <param name="projectId">The project id.</param>
            /// <param name="name">The name.</param>
            /// <param name="sortOrder">The sort order.</param>
			public Milestone(int projectId, string name, int sortOrder,string imageUrl, DateTime? dueDate, DateTime? releaseDate, bool isCompleted, string notes)
                : this(Globals.NewId, projectId, name, sortOrder, imageUrl, dueDate, releaseDate, isCompleted, notes)
				{}

            /// <summary>
            /// Initializes a new instance of the <see cref="Milestone"/> class.
            /// </summary>
            /// <param name="projectId">The project id.</param>
            /// <param name="name">The name.</param>
            /// <param name="imageUrl">The image URL.</param>
            public Milestone(int projectId, string name, string imageUrl)
                : this(Globals.NewId, projectId, name, -1, imageUrl, null, null,false,string.Empty)
            { }

            /// <summary>
            /// Initializes a new instance of the <see cref="Milestone"/> class.
            /// </summary>
            /// <param name="projectId">The project id.</param>
            /// <param name="name">The name.</param>
            /// <param name="imageUrl">The image URL.</param>
            /// <param name="dueDate">The due date.</param>
            /// <param name="releaseDate">The release date.</param>
            /// <param name="isCompleted">if set to <c>true</c> [is completed].</param>
            /// <param name="notes">The notes.</param>
            public Milestone(int projectId, string name, string imageUrl, DateTime? dueDate, DateTime? releaseDate, bool isCompleted, string notes)
                : this(Globals.NewId, projectId, name, -1, imageUrl, dueDate, releaseDate, isCompleted, notes)
            { }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:Milestone"/> class.
            /// </summary>
            /// <param name="id">The id.</param>
            /// <param name="projectId">The project id.</param>
            /// <param name="name">The name.</param>
            /// <param name="sortOrder">The sort order.</param>
            /// <param name="imageUrl">The image URL.</param>
            /// <param name="dueDate">The due date.</param>
            /// <param name="releaseDate">The release date.</param>
            /// <param name="isCompleted">if set to <c>true</c> [is completed].</param>
            /// <param name="notes">The notes.</param>
			public Milestone(int id, int projectId, string name, int sortOrder, string imageUrl, DateTime? dueDate, DateTime? releaseDate, bool isCompleted, string notes) 
			{
				if (projectId<=Globals.NewId && id > Globals.NewId)
					throw (new ArgumentOutOfRangeException("projectId"));

				if (name == null ||name.Length==0 )
					throw (new ArgumentOutOfRangeException("name"));

				_Id           = id;
				_ProjectId    = projectId;
				_Name         = name;
                _SortOrder    = sortOrder;
                _ImageUrl     = imageUrl;
                _DueDate      = dueDate;
                _ReleaseDate = releaseDate;
                _IsCompleted = isCompleted;
                _Notes = notes;
			}
		#endregion

		#region Properties
            /// <summary>
            /// Gets the id.
            /// </summary>
            /// <value>The id.</value>
			public int Id 
			{
				get {return _Id;}
			}


            /// <summary>
            /// Gets or sets the project id.
            /// </summary>
            /// <value>The project id.</value>
			public int ProjectId 
			{
				get {return _ProjectId;}
				set 
				{
					if (value<=Globals.NewId )
						throw (new ArgumentOutOfRangeException("value"));
					_ProjectId=value;
				}
			}


            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
			public string Name 
			{
				get 
				{
					if (_Name == null ||_Name.Length==0)
						return string.Empty;
					else
						return _Name;
				}
				set {_Name=value;}
			}

            /// <summary>
            /// Gets or sets the sort order.
            /// </summary>
            /// <value>The sort order.</value>
            public int SortOrder
            {
                get { return _SortOrder; }
                set { _SortOrder = value; }
            }

            /// <summary>
            /// Gets the image URL.
            /// </summary>
            /// <value>The image URL.</value>
            public string ImageUrl
            {
                get
                {
                    if (_ImageUrl == null || _ImageUrl.Length == 0)
                        return string.Empty;
                    else
                        return _ImageUrl;
                }
                set
                {
                    _ImageUrl = value;
                }
            }


            /// <summary>
            /// Gets or sets the notes.
            /// </summary>
            /// <value>The notes.</value>
            public string Notes
            {
                get
                {
                    if (_Notes == null || _Notes.Length == 0)
                        return string.Empty;
                    else
                        return _Notes;
                }
                set
                {
                    _Notes = value;
                }
            }

            /// <summary>
            /// Gets or sets the due date.
            /// </summary>
            /// <value>The Due Date.</value>
            public DateTime? DueDate
            {
                get
                {
                    return _DueDate;
                }
                set
                {
                    _DueDate = value;
                }
            }

            /// <summary>
            /// Gets or sets the release date.
            /// </summary>
            /// <value>The release date.</value>
            public DateTime? ReleaseDate
            {
                get
                {
                    return _ReleaseDate;
                }
                set
                {
                    _ReleaseDate = value;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is complete.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is complete; otherwise, <c>false</c>.
            /// </value>
            public bool IsCompleted
            {

                get { return _IsCompleted; }
                set { _IsCompleted = value; }
            }

		#endregion

		#region Instance Methods
            /// <summary>
            /// Saves this instance.
            /// </summary>
            /// <returns></returns>
			public bool Save () 
			{

                if (Id <= Globals.NewId)
                {

                    int TempId = DataProviderManager.Provider.CreateNewMilestone(this);
                    if (TempId > 0)
                    {
                        _Id = TempId;
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                   return DataProviderManager.Provider.UpdateMilestone(this);
                }
					
			}
		#endregion
		
		#region Static Methods
            /// <summary>
            /// Deletes the Milestone.
            /// </summary>
            /// <param name="original_id">The original_id.</param>
            /// <returns></returns>
            public static bool DeleteMilestone(int original_id) 
			{
                if (original_id <= Globals.NewId)
                    throw (new ArgumentOutOfRangeException("original_id"));

				

                return DataProviderManager.Provider.DeleteMilestone(original_id);	
			}


            /// <summary>
            /// Gets the Milestone by project id.
            /// </summary>
            /// <param name="projectId">The project id.</param>
            /// <param name="milestoneCompleted">if set to <c>true</c> [milestone completed].</param>
            /// <returns></returns>
			public static List<Milestone> GetMilestoneByProjectId(int projectId, bool milestoneCompleted) 
			{
				if (projectId <= Globals.NewId )
					return new List<BugNET.BusinessLogicLayer.Milestone>();

                return DataProviderManager.Provider.GetMilestonesByProjectId(projectId, milestoneCompleted);
			}

            /// <summary>
            /// Gets the milestone by project id.
            /// </summary>
            /// <param name="projectId">The project id.</param>
            /// <returns></returns>
            public static List<Milestone> GetMilestoneByProjectId(int projectId)
            {
               return Milestone.GetMilestoneByProjectId(projectId, true);
            }

            /// <summary>
            /// Gets the Milestone by id.
            /// </summary>
            /// <param name="MilestoneId">The Milestone id.</param>
            /// <returns></returns>
			public static Milestone GetMilestoneById(int MilestoneId)
			{
				if (MilestoneId < Globals.NewId )
					throw (new ArgumentOutOfRangeException("MilestoneId"));

				
				return DataProviderManager.Provider.GetMilestoneById(MilestoneId);
			}

          

            /// <summary>
            /// Updates the Milestone.
            /// </summary>
            /// <param name="projectId">The project id.</param>
            /// <param name="name">The name.</param>
            /// <param name="sortOrder">The sort order.</param>
            /// <param name="original_id">The original_id.</param>
            /// <returns></returns>
            public static bool UpdateMilestone(int projectId, string name, int sortOrder, int original_id)
            {
               
                Milestone v = Milestone.GetMilestoneById(original_id);

                v.Name = name;
                v.SortOrder = sortOrder;
              
                return (DataProviderManager.Provider.UpdateMilestone(v));
            }


            /// <summary>
            /// Inserts the Milestone.
            /// </summary>
            /// <param name="projectId">The project id.</param>
            /// <param name="name">The name.</param>
            /// <param name="sortOrder">The sort order.</param>
            /// <returns></returns>
            //public static bool InsertMilestone(int projectId, string name, int sortOrder, string imageUrl)
            //{
            //    Milestone v = new Milestone(projectId, name, sortOrder, imageUrl);
            //    return v.Save();
            //}

			
		#endregion
	}
}
