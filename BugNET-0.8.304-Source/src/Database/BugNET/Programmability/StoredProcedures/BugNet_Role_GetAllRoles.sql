IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_GetAllRoles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_GetAllRoles]
GO

CREATE PROCEDURE dbo.BugNet_Role_GetAllRoles
AS
SELECT RoleId, RoleName,Description,ProjectId,AutoAssign FROM Roles


GO

