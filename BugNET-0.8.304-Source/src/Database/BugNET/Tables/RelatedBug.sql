IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RelatedBug]') AND type in (N'U'))
DROP TABLE [dbo].[RelatedBug]
GO

CREATE TABLE [dbo].[RelatedBug](
	[PrimaryBugId] [int] NOT NULL,
	[SecondaryBugId] [int] NOT NULL,
	[RelationType] [int] NOT NULL 
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RelatedBug]') AND name = N'PK_BugRelation')
ALTER TABLE [dbo].[RelatedBug] DROP CONSTRAINT [PK_BugRelation]
GO

ALTER TABLE [dbo].[RelatedBug]  WITH CHECK ADD  CONSTRAINT [FK_RelatedBug_Bug] FOREIGN KEY([PrimaryBugId])
REFERENCES [Bug] ([BugId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RelatedBug] CHECK CONSTRAINT [FK_RelatedBug_Bug]
GO

