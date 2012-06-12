IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_RemoveUserFromRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_RemoveUserFromRole]
GO

CREATE PROCEDURE dbo.BugNet_Role_RemoveUserFromRole
	@UserName	nvarchar(256),
	@RoleId		Int 
AS

DECLARE @UserId UNIQUEIDENTIFIER
SELECT	@UserId = UserId FROM aspnet_users WHERE Username = @UserName

DELETE 
	UserRoles
WHERE
	UserId = @UserId
	AND RoleId = @RoleId


GO

