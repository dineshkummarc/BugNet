IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Attachment_GetAttachmentsByBugId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Attachment_GetAttachmentsByBugId]
GO

CREATE PROCEDURE [dbo].[BugNet_Attachment_GetAttachmentsByBugId]
 @BugId INT
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
	BugId = @BugId

GO

