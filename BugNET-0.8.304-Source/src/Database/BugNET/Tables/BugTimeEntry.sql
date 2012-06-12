IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugTimeEntry]') AND type in (N'U'))
DROP TABLE [dbo].[BugTimeEntry]
GO

CREATE TABLE [dbo].[BugTimeEntry](
	[BugTimeEntryId] [int] IDENTITY(1,1) NOT NULL,
	[BugId] [int] NOT NULL,
	[WorkDate] [datetime] NOT NULL,
	[Duration] [decimal](4, 2) NOT NULL,
	[BugCommentId] [int] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BugTimeEntry]') AND name = N'PK_BugTimeEntry')
ALTER TABLE [dbo].[BugTimeEntry] DROP CONSTRAINT [PK_BugTimeEntry]
GO

ALTER TABLE [dbo].[BugTimeEntry] ADD  CONSTRAINT [PK_BugTimeEntry] PRIMARY KEY CLUSTERED 
(
	[BugTimeEntryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BugTimeEntry]  WITH CHECK ADD  CONSTRAINT [FK_BugTimeEntry_Bug] FOREIGN KEY([BugId])
REFERENCES [Bug] ([BugID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BugTimeEntry] CHECK CONSTRAINT [FK_BugTimeEntry_Bug]
GO

