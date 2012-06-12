IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_GetRoleById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_GetRoleById]
GO

CREATE PROCEDURE dbo.BugNet_Role_GetRoleById
	@RoleId int
AS
SELECT RoleId, ProjectId,RoleName,Description,AutoAssign 
FROM Roles
WHERE RoleId = @RoleId


GO

