IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_RelatedBug_GetRelatedBugsByBugId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_RelatedBug_GetRelatedBugsByBugId]
GO

CREATE PROCEDURE [dbo].[BugNet_RelatedBug_GetRelatedBugsByBugId]
	@BugId int
As
Select * from BugsView join RelatedBug on BugsView.BugId = RelatedBug.LinkedBugId
WHERE RelatedBug.BugId = @BugId

GO

