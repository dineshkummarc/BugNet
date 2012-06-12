IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Priority]') AND type in (N'U'))
DROP TABLE [dbo].[Priority]
GO

CREATE TABLE [dbo].[Priority](
	[PriorityID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ImageUrl] [nvarchar](50) NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Priority]') AND name = N'PK__Priority__07F6335A')
ALTER TABLE [dbo].[Priority] DROP CONSTRAINT [PK__Priority__07F6335A]
GO

ALTER TABLE [dbo].[Priority] ADD  CONSTRAINT [PK__Priority__07F6335A] PRIMARY KEY CLUSTERED 
(
	[PriorityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

