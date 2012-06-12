IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_RelatedBug_CreateNewRelatedBug]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_RelatedBug_CreateNewRelatedBug]
GO

CREATE PROCEDURE BugNet_RelatedBug_CreateNewRelatedBug 
	@PrimaryBugId Int,
	@SecondaryBugId Int,
	@RelationType Int
AS
IF NOT EXISTS(SELECT PrimaryBugId FROM RelatedBug WHERE (PrimaryBugId = @PrimaryBugId OR PrimaryBugId = @SecondaryBugId) AND (SecondaryBugId = @SecondaryBugId OR SecondaryBugId = @PrimaryBugId) AND RelationType = @RelationType)
BEGIN
	INSERT RelatedBug
	(
		primaryBugId,
		secondaryBugId,
		relationType
	)
	VALUES
	(
		@SecondaryBugId,
		@PrimaryBugId,
		@RelationType
	)
END

GO

