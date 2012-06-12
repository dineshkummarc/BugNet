IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetRecentlyAddedBugsByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetRecentlyAddedBugsByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetRecentlyAddedBugsByProject]
	@ProjectId int
AS
SELECT TOP 5
	*
FROM 
	BugsView
WHERE
	ProjectId = @ProjectId
ORDER BY BugID DESC


GO

