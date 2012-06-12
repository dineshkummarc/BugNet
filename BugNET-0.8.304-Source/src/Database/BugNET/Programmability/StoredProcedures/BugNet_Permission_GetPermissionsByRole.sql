IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Permission_GetPermissionsByRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Permission_GetPermissionsByRole]
GO

CREATE PROCEDURE [dbo].[BugNet_Permission_GetPermissionsByRole]
	@RoleId int
 AS
SELECT Permission.PermissionId,PermissionKey, Name  FROM Permission
Inner join RolePermission on RolePermission.PermissionId = Permission.PermissionId
WHERE RoleId = @RoleId

GO

