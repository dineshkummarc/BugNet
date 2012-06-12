IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_AddUserToProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_AddUserToProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_AddUserToProject]
@UserName nvarchar(255),
@ProjectId int
AS
DECLARE @UserId UNIQUEIDENTIFIER
SELECT	@UserId = UserId FROM aspnet_users WHERE Username = @UserName

IF NOT EXISTS (SELECT UserId FROM UserProjects WHERE UserId = @UserId AND ProjectId = @ProjectId)
BEGIN
	INSERT  UserProjects
	(
		UserId,
		ProjectId,
		CreatedDate
	)
	VALUES
	(
		@UserId,
		@ProjectId,
		getdate()
	)
END

GO

