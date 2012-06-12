IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_UpdateRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_UpdateRole]
GO

CREATE PROCEDURE dbo.BugNet_Role_UpdateRole
	@RoleId 		int,
	@Name			nvarchar(256),
	@Description 	nvarchar(256)
AS
UPDATE Roles SET
	RoleName = @Name,
	Description = @Description
WHERE
	RoleId = @RoleId


GO

