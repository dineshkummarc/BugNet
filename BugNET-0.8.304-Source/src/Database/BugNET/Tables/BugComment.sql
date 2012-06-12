IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugComment]') AND type in (N'U'))
DROP TABLE [dbo].[BugComment]
GO

CREATE TABLE [dbo].[BugComment](
	[BugCommentID] [int] IDENTITY(1,1) NOT NULL,
	[BugID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Comment] [ntext] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BugComment]') AND name = N'PK__BugComment__164452B1')
ALTER TABLE [dbo].[BugComment] DROP CONSTRAINT [PK__BugComment__164452B1]
GO

ALTER TABLE [dbo].[BugComment] ADD  CONSTRAINT [PK__BugComment__164452B1] PRIMARY KEY CLUSTERED 
(
	[BugCommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BugComment]  WITH CHECK ADD  CONSTRAINT [FK_BugComment_Bug] FOREIGN KEY([BugID])
REFERENCES [Bug] ([BugID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BugComment] CHECK CONSTRAINT [FK_BugComment_Bug]
GO

