IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Attachment_DeleteAttachment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Attachment_DeleteAttachment]
GO

CREATE PROCEDURE [dbo].[BugNet_Attachment_DeleteAttachment]
	@AttachmentId Int
AS
DELETE 
	BugAttachment
WHERE
	BugAttachmentId = @AttachmentId

GO

