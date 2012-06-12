IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_GetPublicProjects]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_GetPublicProjects]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_GetPublicProjects]
AS
SELECT
	ProjectId,
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
WHERE 
	AccessType = 1 AND Project.Active = 1
ORDER BY Name ASC

GO

