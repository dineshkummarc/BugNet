IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Component]') AND type in (N'U'))
DROP TABLE [dbo].[Component]
GO

CREATE TABLE [dbo].[Component](
	[ComponentID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentComponentID] [int] NULL CONSTRAINT [DF_Component_ParentComponentID]  DEFAULT ((0))
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Component]') AND name = N'PK__Component__09DE7BCC')
ALTER TABLE [dbo].[Component] DROP CONSTRAINT [PK__Component__09DE7BCC]
GO

ALTER TABLE [dbo].[Component] ADD  CONSTRAINT [PK__Component__09DE7BCC] PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Component]  WITH NOCHECK ADD  CONSTRAINT [FK_Component_Component] FOREIGN KEY([ComponentID])
REFERENCES [Component] ([ComponentID])
GO

ALTER TABLE [dbo].[Component] CHECK CONSTRAINT [FK_Component_Component]
GO

