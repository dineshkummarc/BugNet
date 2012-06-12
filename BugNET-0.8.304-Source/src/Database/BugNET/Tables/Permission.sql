IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permission]') AND type in (N'U'))
DROP TABLE [dbo].[Permission]
GO

CREATE TABLE [dbo].[Permission](
	[PermissionId] [int] NOT NULL,
	[PermissionKey] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Permission]') AND name = N'PK_Permission')
ALTER TABLE [dbo].[Permission] DROP CONSTRAINT [PK_Permission]
GO

ALTER TABLE [dbo].[Permission] ADD  CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

