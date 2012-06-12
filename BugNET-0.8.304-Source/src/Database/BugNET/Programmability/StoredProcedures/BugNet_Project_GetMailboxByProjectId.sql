IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_GetMailboxByProjectId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_GetMailboxByProjectId]
GO


CREATE  PROCEDURE [dbo].[BugNet_Project_GetMailboxByProjectId]
	@ProjectId int
AS

SELECT ProjectMailbox.*,
	u.Username AssignToName,
	Type.Name IssueTypeName
FROM 
	ProjectMailbox
	INNER JOIN aspnet_Users u ON u.UserId = AssignToUserID
	INNER JOIN Type ON Type.TypeId = IssueTypeId	
WHERE
	ProjectId = @ProjectId



GO

