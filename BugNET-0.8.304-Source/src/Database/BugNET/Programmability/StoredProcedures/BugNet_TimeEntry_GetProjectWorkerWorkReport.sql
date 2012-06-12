IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_TimeEntry_GetProjectWorkerWorkReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_TimeEntry_GetProjectWorkerWorkReport]
GO


CREATE PROCEDURE [dbo].[BugNet_TimeEntry_GetProjectWorkerWorkReport]
 @ProjectId INT,
 @ReporterUsername NVARCHAR(255)
AS
DECLARE @UserId UNIQUEIDENTIFIER
SELECT @UserId = UserId FROM aspnet_users WHERE Username = @ReporterUsername

SELECT     Project.ProjectId, Project.Name as ProjectName,Project.Code + '-' + str(Bug.BugId) as FullId, Bug.BugId, Bug.Summary, Creators.UserName AS Reporter,  BugTimeEntry.Duration,  BugTimeEntry.WorkDate,
                      ISNULL(BugComment.Comment, '') AS Comment
FROM         BugTimeEntry INNER JOIN
                      aspnet_users Creators ON Creators.UserId =  BugTimeEntry.CreatedUserId INNER JOIN
                      Bug ON  BugTimeEntry.BugId = Bug.BugId INNER JOIN
                      Project ON Bug.ProjectId = Project.ProjectId LEFT OUTER JOIN
                      BugComment ON BugComment.BugCommentId =  BugTimeEntry.BugCommentId
WHERE
	Project.ProjectID = @ProjectId 
AND
	BugTimeEntry.CreatedUserId = @UserId
ORDER BY Reporter, WorkDate

GO

