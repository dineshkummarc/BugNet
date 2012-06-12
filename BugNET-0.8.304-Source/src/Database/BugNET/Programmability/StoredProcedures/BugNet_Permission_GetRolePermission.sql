IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Permission_GetRolePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Permission_GetRolePermission]
GO

CREATE PROCEDURE  [dbo].[BugNet_Permission_GetRolePermission]  AS

SELECT R.RoleId, R.ProjectId,P.PermissionId,P.PermissionKey,R.RoleName
FROM RolePermission RP 
JOIN
Permission P ON RP.PermissionId = P.PermissionId
JOIN
Roles R ON RP.RoleId = R.RoleId

GO

