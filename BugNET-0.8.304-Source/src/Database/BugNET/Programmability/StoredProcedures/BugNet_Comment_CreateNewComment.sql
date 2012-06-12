IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Comment_CreateNewComment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Comment_CreateNewComment]
GO

CREATE PROCEDURE [dbo].[BugNet_Comment_CreateNewComment]
	@BugId int,
	@CreatorUserName NVarChar(255),
	@Comment ntext
AS
-- Get Last Update UserID
DECLARE @CreatorUserId uniqueidentifier
SELECT @CreatorUserId = UserId FROM aspnet_users WHERE Username = @CreatorUserName
INSERT BugComment
(
	BugId,
	CreatedUserId,
	CreatedDate,
	Comment
) 
VALUES 
(
	@BugId,
	@CreatorUserId,
	GetDate(),
	@Comment
)

/* Update the LastUpdate fields of this bug*/
UPDATE Bug SET LastUpdate = GetDate(),LastUpdateUserId = @CreatorUserId WHERE BugId = @BugId

RETURN @@IDENTITY

GO

