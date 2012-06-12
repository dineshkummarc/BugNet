IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugUserCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugUserCountByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugUserCountByProject]
 @ProjectId int
AS
	SELECT u.UserID,u.Username, COUNT(b.BugID) AS Number FROM UserProjects pm 
	LEFT OUTER JOIN aspnet_Users u ON pm.UserId = u.UserId 
	LEFT OUTER JOIN Bug b ON b.AssignedToUserId = u.UserId
	WHERE (pm.ProjectID = @ProjectId) 
	 AND (b.ProjectID = @ProjectId ) 
	 AND (b.StatusID <> 4) 
	 AND (b.StatusID <> 5)  
	 GROUP BY u.Username, u.UserID

GO

