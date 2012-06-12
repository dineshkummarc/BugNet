IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugComponentCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugComponentCountByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugComponentCountByProject]
 @ProjectId int,
 @ComponentId int
AS
	SELECT     Count(BugId) From Bug Where ProjectId = @ProjectId 
	AND ComponentId = @ComponentId AND StatusId <> 4 AND StatusId <> 5


GO

