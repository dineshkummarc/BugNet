IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Version_CreateNewVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Version_CreateNewVersion]
GO

CREATE PROCEDURE [dbo].[BugNet_Version_CreateNewVersion]
  @ProjectId int,
  @Name nvarchar(50),
  @SortOrder int
AS
	INSERT Version
	(
		ProjectID,
		Name,
		SortOrder
	)
	VALUES
	(
		@ProjectId,
		@Name,
		@SortOrder
	)
RETURN @@IDENTITY

GO

