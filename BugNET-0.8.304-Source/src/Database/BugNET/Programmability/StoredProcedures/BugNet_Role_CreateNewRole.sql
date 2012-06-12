IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_CreateNewRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_CreateNewRole]
GO

CREATE PROCEDURE dbo.BugNet_Role_CreateNewRole
  @ProjectId 	int,
  @Name 		nvarchar(256),
  @Description 	nvarchar(256),
  @AutoAssign	bit
AS
	INSERT Roles
	(
		ProjectID,
		RoleName,
		Description,
		AutoAssign
	)
	VALUES
	(
		@ProjectId,
		@Name,
		@Description,
		@AutoAssign
	)
RETURN scope_identity()

GO

