IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Comment_UpdateComment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Comment_UpdateComment]
GO

CREATE PROCEDURE [dbo].[BugNet_Comment_UpdateComment]
	@BugCommentId int,
	@BugId int,
	@CreatorUserName nvarchar(255),
	@Comment ntext
AS

DECLARE @CreatorUserId uniqueidentifier
SELECT @CreatorUserId = UserId FROM aspnet_users WHERE Username = @CreatorUserName

UPDATE BugComment SET
	BugId = @BugId,
	CreatedUserId = @CreatorUserId,
	Comment = @Comment
WHERE BugCommentId= @BugCommentId

GO

