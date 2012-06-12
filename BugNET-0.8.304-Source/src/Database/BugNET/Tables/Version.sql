IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Version]') AND type in (N'U'))
DROP TABLE [dbo].[Version]
GO

CREATE TABLE [dbo].[Version](
	[VersionID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Version]') AND name = N'PK__Version__0425A276')
ALTER TABLE [dbo].[Version] DROP CONSTRAINT [PK__Version__0425A276]
GO

ALTER TABLE [dbo].[Version] ADD  CONSTRAINT [PK__Version__0425A276] PRIMARY KEY CLUSTERED 
(
	[VersionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Version]  WITH NOCHECK ADD  CONSTRAINT [FK_Version_Project] FOREIGN KEY([ProjectID])
REFERENCES [Project] ([ProjectID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Version] CHECK CONSTRAINT [FK_Version_Project]
GO

