IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Version_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Version_Update]
GO

CREATE PROCEDURE [dbo].[BugNet_Version_Update]
	@VersionId int,
	@ProjectId int,
	@Name nvarchar(255),
	@SortOrder int
AS


UPDATE Version SET
	ProjectId = @ProjectId,
	[Name] = @Name,
	SortOrder = @SortOrder
WHERE VersionId= @VersionId

GO

