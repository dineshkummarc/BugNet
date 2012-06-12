IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_RoleHasPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_RoleHasPermission]
GO

CREATE PROCEDURE dbo.BugNet_Role_RoleHasPermission 
	@ProjectID 		int,
	@Role 			nvarchar(256),
	@PermissionKey	nvarchar(50)
AS

SELECT COUNT(*) FROM RolePermission INNER JOIN Roles ON Roles.RoleId = RolePermission.RoleId INNER JOIN
Permission ON RolePermission.PermissionId = Permission.PermissionId

WHERE ProjectId = @ProjectID 
AND 
PermissionKey = @PermissionKey
AND 
Roles.RoleName = @Role

GO

