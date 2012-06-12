IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Permission_AddRolePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Permission_AddRolePermission]
GO

CREATE PROCEDURE [dbo].[BugNet_Permission_AddRolePermission]
	@PermissionId int,
	@RoleId int
AS
IF NOT EXISTS (SELECT PermissionId FROM RolePermission WHERE PermissionId = @PermissionId AND RoleId = @RoleId)
BEGIN
	INSERT  RolePermission
	(
		PermissionId,
		RoleId
	)
	VALUES
	(
		@PermissionId,
		@RoleId
	)
END

GO

