IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_IsUserProjectMember]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_IsUserProjectMember]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_IsUserProjectMember]
	@UserName	nvarchar(255),
 	@ProjectId	int
AS
DECLARE @UserId UNIQUEIDENTIFIER
SELECT @UserId = UserId FROM aspnet_users WHERE Username = @UserName

IF EXISTS( SELECT UserId FROM UserProjects WHERE UserId = @UserId AND ProjectId = @ProjectId)
  RETURN 0
ELSE
  RETURN -1

GO

