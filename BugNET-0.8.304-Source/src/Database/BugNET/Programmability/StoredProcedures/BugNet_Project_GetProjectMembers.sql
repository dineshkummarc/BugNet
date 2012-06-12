IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_GetProjectMembers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_GetProjectMembers]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_GetProjectMembers]
	@ProjectId Int
AS
SELECT Username
FROM 
	aspnet_users
LEFT OUTER JOIN
	UserProjects
ON
	aspnet_users.UserId = UserProjects.UserId
WHERE
	UserProjects.ProjectId = @ProjectId
ORDER BY Username ASC

GO

