IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Attachment_CreateNewAttachment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Attachment_CreateNewAttachment]
GO

CREATE PROCEDURE [dbo].[BugNet_Attachment_CreateNewAttachment]
  @BugId int,
  @FileName nvarchar(100),
  @Description nvarchar(80),
  @FileSize Int,
  @ContentType nvarchar(50),
  @UploadedUserName nvarchar(255)
AS
-- Get Uploaded UserID
DECLARE @UploadedUserId UniqueIdentifier
SELECT @UploadedUserId = UserId FROM aspnet_users WHERE Username = @UploadedUserName
	INSERT BugAttachment
	(
		BugID,
		FileName,
		Description,
		FileSize,
		Type,
		UploadedDate,
		UploadedUserId
	)
	VALUES
	(
		@BugId,
		@FileName,
		@Description,
		@FileSize,
		@ContentType,
		GetDate(),
		@UploadedUserId
	)
	RETURN scope_identity()

GO

