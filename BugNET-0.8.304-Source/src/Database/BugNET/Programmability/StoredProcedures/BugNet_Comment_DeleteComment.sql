IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Comment_DeleteComment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Comment_DeleteComment]
GO

CREATE PROCEDURE [dbo].[BugNet_Comment_DeleteComment]
	@BugCommentId Int
AS
DELETE 
	BugComment
WHERE
	BugCommentId = @BugCommentId

GO

