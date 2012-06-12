IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetChangeLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetChangeLog]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetChangeLog] 
	@ProjectId int
AS

Select * from BugsView WHERE ProjectId = @ProjectId  AND StatusId = 5
Order By MilestoneId DESC,ComponentName ASC, TypeName ASC, AssignedToUserName ASC

GO

