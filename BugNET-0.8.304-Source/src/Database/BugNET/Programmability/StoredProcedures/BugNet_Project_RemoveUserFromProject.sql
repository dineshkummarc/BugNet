IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_RemoveUserFromProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_RemoveUserFromProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_RemoveUserFromProject]
	@UserName nvarchar(255),
	@ProjectId Int 
AS
DECLARE @UserId UNIQUEIDENTIFIER
SELECT	@UserId = UserId FROM aspnet_users WHERE Username = @UserName

DELETE 
	UserProjects
WHERE
	UserId = @UserId
	AND ProjectId = @ProjectId

GO

