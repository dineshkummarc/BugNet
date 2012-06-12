IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_BugNotification_DeleteBugNotification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_BugNotification_DeleteBugNotification]
GO

CREATE PROCEDURE [dbo].[BugNet_BugNotification_DeleteBugNotification]
	@BugId Int,
	@Username NVarChar(255)
AS
DECLARE @UserId uniqueidentifier
SELECT @UserId = UserId FROM aspnet_Users WHERE Username = @Username
DELETE 
	BugNotification
WHERE
	BugId = @BugId
	AND CreatedUserId = @UserId 


GO

