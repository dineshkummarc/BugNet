IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugAttachment]') AND type in (N'U'))
DROP TABLE [dbo].[BugAttachment]
GO

CREATE TABLE [dbo].[BugAttachment](
	[BugAttachmentID] [int] IDENTITY(1,1) NOT NULL,
	[BugID] [int] NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](80) NOT NULL,
	[FileSize] [int] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[UploadedDate] [datetime] NOT NULL,
	[UploadedUserId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BugAttachment]') AND name = N'PK__BugAttachment__1273C1CD')
ALTER TABLE [dbo].[BugAttachment] DROP CONSTRAINT [PK__BugAttachment__1273C1CD]
GO

ALTER TABLE [dbo].[BugAttachment] ADD  CONSTRAINT [PK__BugAttachment__1273C1CD] PRIMARY KEY CLUSTERED 
(
	[BugAttachmentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BugAttachment]  WITH CHECK ADD  CONSTRAINT [FK_BugAttachment_Bug] FOREIGN KEY([BugID])
REFERENCES [Bug] ([BugID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BugAttachment] CHECK CONSTRAINT [FK_BugAttachment_Bug]
GO

