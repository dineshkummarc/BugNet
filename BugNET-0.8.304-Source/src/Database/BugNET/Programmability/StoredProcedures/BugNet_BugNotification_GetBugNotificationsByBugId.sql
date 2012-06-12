IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_BugNotification_GetBugNotificationsByBugId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_BugNotification_GetBugNotificationsByBugId]
GO

CREATE PROCEDURE [dbo].[BugNet_BugNotification_GetBugNotificationsByBugId] 
	@BugId Int
AS
SELECT 
	BugNotificationId,
	BugId,
	Username NotificationUsername,
	Membership.Email NotificationEmail
FROM
	BugNotification
	INNER JOIN aspnet_Users Users ON BugNotification.CreatedUserId = Users.UserId
	INNER JOIN aspnet_membership Membership on BugNotification.CreatedUserId = Membership.UserId
WHERE
	BugId = @BugId
ORDER BY
	Username


GO

