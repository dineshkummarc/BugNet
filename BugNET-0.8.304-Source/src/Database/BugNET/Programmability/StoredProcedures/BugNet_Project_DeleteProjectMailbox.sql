IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_DeleteProjectMailbox]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_DeleteProjectMailbox]
GO


CREATE PROCEDURE [dbo].[BugNet_Project_DeleteProjectMailbox]
	@ProjectMailboxId int
AS
DELETE  ProjectMailBox 
WHERE
	ProjectMailboxId = @ProjectMailboxId

IF @@ROWCOUNT > 0 
	RETURN 0
ELSE
	RETURN 1

GO

