IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_RelatedBug_DeleteRelatedBug]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_RelatedBug_DeleteRelatedBug]
GO

CREATE PROCEDURE [dbo].[BugNet_RelatedBug_DeleteRelatedBug]
	@PrimaryBugId Int,
	@SecondaryBugId Int,
	@RelationType Int
AS
DELETE
	RelatedBug
WHERE
	( (PrimaryBugId = @PrimaryBugId AND SecondaryBugId = @SecondaryBugId) OR (PrimaryBugId = @SecondaryBugId AND SecondaryBugId = @PrimaryBugId) )
	AND RelationType = @RelationType
GO

