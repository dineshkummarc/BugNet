IF  EXISTS (SELECT * FROM sys.views WHERE object_Id = OBJECT_Id(N'[dbo].[BugsView]'))
DROP VIEW [dbo].[BugsView]
GO

/* Handles 'unassigned' version 
*/
CREATE VIEW dbo.BugsView
AS
SELECT        dbo.Bug.BugId, dbo.Bug.Title, dbo.Bug.Description, dbo.Bug.DateCreated, dbo.Bug.StatusId, dbo.Bug.PriorityId, dbo.Bug.TypeId, 
                         dbo.Bug.ComponentId, dbo.Bug.ProjectId, dbo.Bug.ResolutionId, dbo.Bug.VersionId, dbo.Bug.LastUpdate, dbo.Bug.ReporterUserId, 
                         dbo.Bug.AssignedToUserId, dbo.Bug.LastUpdateUserId, dbo.Status.Name AS StatusName, dbo.Component.Name AS ComponentName, 
                         dbo.Priority.Name AS PriorityName, dbo.Project.Name AS ProjectName, dbo.Project.Code AS ProjectCode, dbo.Resolution.Name AS ResolutionName, 
                         dbo.Type.Name AS TypeName, ISNULL(dbo.Version.Name, 'Unassigned') AS VersionName, LastUpdateUsers.UserName AS LastUpdateUserName, 
                         ReportedUsers.UserName AS ReporterUserName, ISNULL(AssignedUsers.UserName, 'Unassigned') AS AssignedToUserName, dbo.Bug.DueDate, 
                         dbo.Bug.MilestoneId, ISNULL(Milestone.Name, 'Unassigned') AS MilestoneName, dbo.Bug.Visibility, ISNULL
                             ((SELECT        SUM(Duration) AS Expr1
                                 FROM            dbo.BugTimeEntry AS BTE
                                 WHERE        (BugId = dbo.Bug.BugId)), 0.00) AS TimeLogged, dbo.Bug.Estimation
FROM            dbo.Bug LEFT OUTER JOIN
                         dbo.Component ON dbo.Bug.ComponentId = dbo.Component.ComponentId LEFT OUTER JOIN
                         dbo.Priority ON dbo.Bug.PriorityId = dbo.Priority.PriorityId LEFT OUTER JOIN
                         dbo.Project ON dbo.Bug.ProjectId = dbo.Project.ProjectId LEFT OUTER JOIN
                         dbo.Resolution ON dbo.Bug.ResolutionId = dbo.Resolution.ResolutionId LEFT OUTER JOIN
                         dbo.Status ON dbo.Bug.StatusId = dbo.Status.StatusId LEFT OUTER JOIN
                         dbo.Type ON dbo.Bug.TypeId = dbo.Type.TypeId LEFT OUTER JOIN
                         dbo.Version ON dbo.Bug.VersionId = dbo.Version.VersionId LEFT OUTER JOIN
                         dbo.aspnet_Users AS AssignedUsers ON dbo.Bug.AssignedToUserId = AssignedUsers.UserId LEFT OUTER JOIN
                         dbo.aspnet_Users AS ReportedUsers ON dbo.Bug.ReporterUserId = ReportedUsers.UserId LEFT OUTER JOIN
                         dbo.aspnet_Users AS LastUpdateUsers ON dbo.Bug.LastUpdateUserId = LastUpdateUsers.UserId LEFT OUTER JOIN
                         dbo.Version AS Milestone ON dbo.Bug.MilestoneId = Milestone.VersionId


GO

