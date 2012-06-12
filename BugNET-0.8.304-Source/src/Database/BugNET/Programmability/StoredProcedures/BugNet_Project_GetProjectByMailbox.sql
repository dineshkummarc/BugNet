IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_GetProjectByMailbox]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_GetProjectByMailbox]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_GetProjectByMailbox]
	(
	@mailbox nvarchar(256)
	)
	
AS
	SET NOCOUNT ON 
	
	SELECT  Mailbox,ProjectMailbox.ProjectId,IssueTypeId,Users.UserName as AssignToName, AssignToUserId FROM Project INNER JOIN ProjectMailbox 
	ON ProjectMailbox.ProjectId = Project.ProjectId
	INNER JOIN aspnet_users Users ON ProjectMailbox.AssignToUserId = Users.UserId	
	
	WHERE ProjectMailbox.Mailbox = @mailbox
	

GO

