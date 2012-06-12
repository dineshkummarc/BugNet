IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_GetRolesByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_GetRolesByProject]
GO

CREATE PROCEDURE dbo.BugNet_Role_GetRolesByProject
	@ProjectId int
AS
SELECT RoleId,ProjectId, RoleName, Description, AutoAssign
FROM Roles
WHERE ProjectId = @ProjectId

GO

