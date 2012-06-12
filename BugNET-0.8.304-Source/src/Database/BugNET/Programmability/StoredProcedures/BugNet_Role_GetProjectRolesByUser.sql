IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_GetProjectRolesByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_GetProjectRolesByUser]
GO

CREATE procedure [dbo].[BugNet_Role_GetProjectRolesByUser] 
	@UserName       nvarchar(256),
	@ProjectId      int
AS

DECLARE @UserId UNIQUEIDENTIFIER
SELECT	@UserId = UserId FROM aspnet_users WHERE Username = @UserName

SELECT	Roles.RoleName,
		Roles.ProjectId,
		Roles.Description,
		Roles.RoleId,
		Roles.AutoAssign
FROM	UserRoles
INNER JOIN aspnet_users ON UserRoles.UserId = aspnet_users.UserId
INNER JOIN Roles ON UserRoles.RoleId = Roles.RoleId
WHERE  aspnet_users.UserId = @UserId
AND    (Roles.ProjectId IS NULL OR Roles.ProjectId = @ProjectId)



GO

