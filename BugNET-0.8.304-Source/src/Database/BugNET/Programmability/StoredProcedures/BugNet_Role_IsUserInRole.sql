IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_IsUserInRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_IsUserInRole]
GO

CREATE procedure dbo.BugNet_Role_IsUserInRole 
	@UserName		nvarchar(256),
	@RoleId			int,
	@ProjectId      int
AS

DECLARE @UserId UNIQUEIDENTIFIER
SELECT	@UserId = UserId FROM aspnet_users WHERE Username = @UserName

SELECT	UserRoles.UserId,
		UserRoles.RoleId
FROM	UserRoles
INNER JOIN Roles ON UserRoles.RoleId = Roles.RoleId
WHERE	UserRoles.UserId = @UserId
AND		UserRoles.RoleId = @RoleId
AND		Roles.ProjectId = @ProjectId

GO

