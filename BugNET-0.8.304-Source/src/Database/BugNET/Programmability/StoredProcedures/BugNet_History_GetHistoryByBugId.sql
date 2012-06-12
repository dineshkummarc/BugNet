IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_History_GetHistoryByBugId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_History_GetHistoryByBugId]
GO

CREATE PROCEDURE [dbo].[BugNet_History_GetHistoryByBugId]
	@BugId int
AS
 SELECT
	BugHistoryID,
	BugId,
	CreateUser.UserName,
	FieldChanged,
	OldValue,
	NewValue,
	CreatedDate
FROM 
	BugHistory
JOIN 
	aspnet_users CreateUser 
ON
	BugHistory.CreatedUserId = CreateUser.UserId
WHERE 
	BugId = @BugId

GO

