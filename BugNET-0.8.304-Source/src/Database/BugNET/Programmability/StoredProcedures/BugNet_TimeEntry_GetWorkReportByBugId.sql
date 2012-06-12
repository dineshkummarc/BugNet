IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_TimeEntry_GetWorkReportByBugId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_TimeEntry_GetWorkReportByBugId]
GO


CREATE PROCEDURE [dbo].[BugNet_TimeEntry_GetWorkReportByBugId]
 @BugId INT
AS
SELECT      BugTimeEntry.*, Creators.UserName AS CreatorUserName, Membership.Email AS CreatorEmail, 
                      ISNULL(BugComment.Comment, '') Comment
FROM         BugTimeEntry
	 INNER JOIN aspnet_users Creators ON Creators.UserID =  BugTimeEntry.CreatedUserId
	 INNER JOIN aspnet_membership Membership ON Creators.UserId = Membership.UserId
	 LEFT OUTER JOIN BugComment ON BugComment.BugCommentId =  BugTimeEntry.BugCommentId
WHERE
	 BugTimeEntry.BugId = @BugId
ORDER BY WorkDate DESC

GO

