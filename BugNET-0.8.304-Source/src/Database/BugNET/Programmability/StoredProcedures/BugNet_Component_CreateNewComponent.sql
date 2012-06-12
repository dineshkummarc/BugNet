IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Component_CreateNewComponent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Component_CreateNewComponent]
GO

CREATE PROCEDURE [dbo].[BugNet_Component_CreateNewComponent]
  @ProjectId int,
  @Name nvarchar(50),
  @ParentComponentId int
AS
	INSERT Component
	(
		ProjectID,
		Name,
		ParentComponentID
	)
	VALUES
	(
		@ProjectId,
		@Name,
		@ParentComponentId
	)
RETURN @@IDENTITY

GO

