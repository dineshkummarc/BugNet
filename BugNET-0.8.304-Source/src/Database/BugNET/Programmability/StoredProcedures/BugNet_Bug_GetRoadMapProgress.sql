IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetRoadMapProgress]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetRoadMapProgress]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetRoadMapProgress]
	@ProjectId int,
	@MilestoneId int
AS
	/* SET NOCOUNT ON */ 
SELECT (SELECT Count(*) from BugsView 
WHERE ProjectId = @ProjectId AND MilestoneId = @MilestoneId AND StatusId In (4,5)) As ClosedCount , (SELECT Count(*) from BugsView 
WHERE ProjectId = @ProjectId AND MilestoneId = @MilestoneId) As TotalCount



GO

