IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectMailBox]') AND type in (N'U'))
DROP TABLE [dbo].[ProjectMailBox]
GO

CREATE TABLE [dbo].[ProjectMailBox](
	[ProjectMailboxId] [int] IDENTITY(1,1) NOT NULL,
	[MailBox] [nvarchar](100) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[AssignToUserId] [uniqueidentifier] NULL,
	[IssueTypeId] [int] NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProjectMailBox]') AND name = N'PK_ProjectMailBox')
ALTER TABLE [dbo].[ProjectMailBox] DROP CONSTRAINT [PK_ProjectMailBox]
GO

ALTER TABLE [dbo].[ProjectMailBox] ADD  CONSTRAINT [PK_ProjectMailBox] PRIMARY KEY CLUSTERED 
(
	[ProjectMailboxId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectMailBox]  WITH NOCHECK ADD  CONSTRAINT [FK_ProjectMailBox_Project] FOREIGN KEY([ProjectId])
REFERENCES [Project] ([ProjectID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectMailBox] CHECK CONSTRAINT [FK_ProjectMailBox_Project]
GO

