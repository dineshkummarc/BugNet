IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugUnassignedCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugUnassignedCountByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugUnassignedCountByProject]
 @ProjectId int
AS
	SELECT     COUNT(BugID) AS Number 
	FROM Bug 
	WHERE (AssignedToUserId IS NULL) 
		AND (ProjectID = @ProjectId) 
		AND (StatusID <> 4) 
		AND (StatusID <> 5)


GO

