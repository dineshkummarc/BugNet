IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_TimeEntry_GetWorkReportByProjectId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_TimeEntry_GetWorkReportByProjectId]
GO


CREATE PROCEDURE [dbo].[BugNet_TimeEntry_GetWorkReportByProjectId]
 @ProjectId INT
AS
SELECT     Project.ProjectID, Project.Name as ProjectName,Project.Code + '-' + str(Bug.BugID) as FullId,Bug.BugId, Bug.Summary, Creators.UserName AS Reporter,  BugTimeEntry.Duration, BugTimeEntry.WorkDate, 
                      ISNULL(BugComment.Comment, '') AS Comment
FROM        BugTimeEntry INNER JOIN
                      aspnet_users Creators ON Creators.UserId =  BugTimeEntry.CreatedUserId INNER JOIN
                      Bug ON  BugTimeEntry.BugId = Bug.BugId INNER JOIN
                      Project ON Bug.ProjectId = Project.ProjectId LEFT OUTER JOIN
                      BugComment ON BugComment.BugCommentId =  BugTimeEntry.BugCommentId
WHERE
	Project.ProjectId = @ProjectId
ORDER BY WorkDate

GO

