IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugById]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugById] 
  @BugId Int
AS
SELECT 
	*
FROM 
	BugsView
WHERE
	BugId = @BugId


GO

