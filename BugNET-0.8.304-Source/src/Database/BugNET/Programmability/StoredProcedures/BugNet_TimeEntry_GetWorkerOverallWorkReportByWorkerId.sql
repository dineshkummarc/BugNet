IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_TimeEntry_GetWorkerOverallWorkReportByWorkerId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_TimeEntry_GetWorkerOverallWorkReportByWorkerId]
GO


CREATE PROCEDURE [dbo].[BugNet_TimeEntry_GetWorkerOverallWorkReportByWorkerId]
 @ReporterUserName nvarchar(255)
AS
DECLARE @ReporterUserId uniqueidentifier
SELECT @ReporterUserId = UserId FROM aspnet_users WHERE Username = @ReporterUserName

SELECT     Project.ProjectId, Project.Name, Bug.BugId, Bug.Summary, Creators.UserName AS CreatorDisplayName, BugTimeEntry.Duration, 
                      ISNULL(BugComment.Comment, '') AS Comment
FROM         BugTimeEntry INNER JOIN
                      aspnet_users Creators ON Creators.UserID =  BugTimeEntry.CreatedUserId INNER JOIN
                      Bug ON  BugTimeEntry.BugId = Bug.BugId INNER JOIN
                      Project ON Bug.ProjectId = Project.ProjectId LEFT OUTER JOIN
                      BugComment ON BugComment.BugCommentId =  BugTimeEntry.BugCommentId
WHERE
	 BugTimeEntry.CreatedUserId = @ReporterUserId
ORDER BY WorkDate

GO

