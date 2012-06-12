IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Permission_DeleteRolePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Permission_DeleteRolePermission]
GO

CREATE PROCEDURE [dbo].[BugNet_Permission_DeleteRolePermission]
	@PermissionId Int,
	@RoleId Int 
AS
DELETE 
	RolePermission
WHERE
	PermissionId = @PermissionId
	AND RoleId = @RoleId

GO

