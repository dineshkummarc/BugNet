IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetMonitoredBugsByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetMonitoredBugsByUser]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetMonitoredBugsByUser]
  @UserName nvarchar(255)
AS
DECLARE @UserId UNIQUEIDENTIFIER
SELECT @UserId = UserId FROM aspnet_users WHERE Username = @UserName

SELECT 
	*
FROM 
	BugsView
	
	INNER JOIN BugNotification on BugsView.BugId = BugNotification.BugId
	AND BugNotification.CreatedUserId = @UserId
	WHERE StatusId In (1,2,3)



GO

