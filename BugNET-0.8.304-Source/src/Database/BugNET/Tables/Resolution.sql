IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Resolution]') AND type in (N'U'))
DROP TABLE [dbo].[Resolution]
GO

CREATE TABLE [dbo].[Resolution](
	[ResolutionID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Resolution]') AND name = N'PK__Resolution__00551192')
ALTER TABLE [dbo].[Resolution] DROP CONSTRAINT [PK__Resolution__00551192]
GO

ALTER TABLE [dbo].[Resolution] ADD  CONSTRAINT [PK__Resolution__00551192] PRIMARY KEY CLUSTERED 
(
	[ResolutionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

