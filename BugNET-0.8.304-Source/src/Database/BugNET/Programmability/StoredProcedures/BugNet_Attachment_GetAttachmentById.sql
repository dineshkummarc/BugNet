IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Attachment_GetAttachmentById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Attachment_GetAttachmentById]
GO

CREATE PROCEDURE [dbo].[BugNet_Attachment_GetAttachmentById]
 @AttachmentId INT
AS
SELECT
	BugAttachmentId,
	BugId,
	FileName,
	Description,
	FileSize,
	Type,
	UploadedDate,
	UploadedUserId,
	Creators.UserName CreatorUserName
FROM 
	BugAttachment
	INNER JOIN aspnet_users Creators ON Creators.UserId = BugAttachment.UploadedUserId
WHERE
	BugAttachmentId = @AttachmentId

GO

