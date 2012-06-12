IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugsByProjectId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugsByProjectId]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugsByProjectId]
	@ProjectId int
As
Select * from BugsView WHERE ProjectId = @ProjectId
Order By PriorityId,StatusId ASC


GO

