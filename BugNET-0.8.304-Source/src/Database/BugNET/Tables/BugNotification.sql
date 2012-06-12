IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNotification]') AND type in (N'U'))
DROP TABLE [dbo].[BugNotification]
GO

CREATE TABLE [dbo].[BugNotification](
	[BugNotificationID] [int] IDENTITY(1,1) NOT NULL,
	[BugID] [int] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BugNotification]') AND name = N'PK_BugNotification')
ALTER TABLE [dbo].[BugNotification] DROP CONSTRAINT [PK_BugNotification]
GO

ALTER TABLE [dbo].[BugNotification] ADD  CONSTRAINT [PK_BugNotification] PRIMARY KEY CLUSTERED 
(
	[BugNotificationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BugNotification]  WITH CHECK ADD  CONSTRAINT [FK_BugNotification_Bug] FOREIGN KEY([BugID])
REFERENCES [Bug] ([BugID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BugNotification] CHECK CONSTRAINT [FK_BugNotification_Bug]
GO

