IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_BugNotification_CreateNewBugNotification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_BugNotification_CreateNewBugNotification]
GO

CREATE PROCEDURE [dbo].[BugNet_BugNotification_CreateNewBugNotification]
	@BugId Int,
	@NotificationUsername NVarChar(255) 
AS
DECLARE @UserId UniqueIdentifier
SELECT @UserId = UserId FROM aspnet_Users WHERE Username = @NotificationUsername
IF NOT EXISTS( SELECT BugNotificationId FROM BugNotification WHERE CreatedUserId = @UserId AND BugId = @BugId)
BEGIN
	INSERT BugNotification
	(
		BugId,
		CreatedUserId
	)
	VALUES
	(
		@BugId,
		@UserId
	)
	RETURN scope_identity()
END


GO

