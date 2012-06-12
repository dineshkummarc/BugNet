IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_GetProjectsByUserName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_GetProjectsByUserName]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_GetProjectsByUserName]
	@UserName nvarchar(255),
	@ActiveOnly bit
AS
DECLARE @UserId UNIQUEIDENTIFIER
SELECT @UserId = UserId FROM aspnet_users WHERE Username = @UserName

SELECT DISTINCT
	Project.ProjectId,
	Name,
	Code,
	Description,
	UploadPath,
	ManagerUserId,
	CreatorUserId,
	CreateDate,
	Project.Active,
	AccessType,
	Managers.UserName ManagerDisplayName,
	Creators.UserName CreatorDisplayName,
	AllowAttachments
FROM 
	Project
	INNER JOIN aspnet_users AS Managers ON Managers.UserId = Project.ManagerUserId	
	INNER JOIN aspnet_users AS Creators ON Creators.UserId = Project.CreatorUserId
	Left JOIN UserProjects ON UserProjects.ProjectId = Project.ProjectId
WHERE
	 (Project.AccessType = 1 AND Project.Active = @ActiveOnly) OR
              (Project.AccessType = 2 AND Project.Active = @ActiveOnly AND UserProjects.UserId = @UserId)
              
ORDER BY Name ASC

GO

