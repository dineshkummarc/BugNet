IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_AddUserToRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_AddUserToRole]
GO

CREATE PROCEDURE dbo.BugNet_Role_AddUserToRole
	@UserName nvarchar(256),
	@RoleId int
AS

DECLARE @ProjectId int
DECLARE @UserId UNIQUEIDENTIFIER
SELECT	@UserId = UserId FROM aspnet_users WHERE Username = @UserName
SELECT	@ProjectId = ProjectId FROM Roles WHERE RoleId = @RoleId

IF NOT EXISTS (SELECT UserId FROM UserProjects WHERE UserId = @UserId AND ProjectId = @ProjectId) AND @RoleId <> 1
BEGIN
 EXEC BugNet_Project_AddUserToProject @UserName, @ProjectId
END

IF NOT EXISTS (SELECT UserId FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
BEGIN
	INSERT  UserRoles
	(
		UserId,
		RoleId
	)
	VALUES
	(
		@UserId,
		@RoleId
	)
END



GO

