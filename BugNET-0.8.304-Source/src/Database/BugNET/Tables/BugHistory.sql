IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugHistory]') AND type in (N'U'))
DROP TABLE [dbo].[BugHistory]
GO

CREATE TABLE [dbo].[BugHistory](
	[BugHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[BugID] [int] NOT NULL,
	[FieldChanged] [nvarchar](50) NOT NULL,
	[OldValue] [nvarchar](50) NOT NULL,
	[NewValue] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BugHistory]') AND name = N'PK__BugHistory__182C9B23')
ALTER TABLE [dbo].[BugHistory] DROP CONSTRAINT [PK__BugHistory__182C9B23]
GO

ALTER TABLE [dbo].[BugHistory] ADD  CONSTRAINT [PK__BugHistory__182C9B23] PRIMARY KEY CLUSTERED 
(
	[BugHistoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BugHistory]  WITH CHECK ADD  CONSTRAINT [FK_BugHistory_Bug] FOREIGN KEY([BugID])
REFERENCES [Bug] ([BugID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BugHistory] CHECK CONSTRAINT [FK_BugHistory_Bug]
GO

