IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Comment_GetCommentsByBugId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Comment_GetCommentsByBugId]
GO

CREATE PROCEDURE [dbo].[BugNet_Comment_GetCommentsByBugId]
 @BugId INT
AS
SELECT
	BugCommentId,
	BugId,
	BugComment.CreatedUserId,
	CreatedDate,
	BugComment.Comment,
	Creators.UserName CreatorUserName,
	Membership.Email CreatorEmail
FROM 
	BugComment
	INNER JOIN aspnet_users Creators ON Creators.UserId = BugComment.CreatedUserId
	INNER JOIN aspnet_membership Membership on Creators.UserId = Membership.UserId	
WHERE
	BugId = @BugId
ORDER BY 
	CreatedDate DESC

GO
