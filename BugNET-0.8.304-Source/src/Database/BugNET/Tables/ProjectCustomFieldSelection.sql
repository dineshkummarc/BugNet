IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFieldSelection]') AND type in (N'U'))
DROP TABLE [dbo].[ProjectCustomFieldSelection]
GO

CREATE TABLE [dbo].[ProjectCustomFieldSelection](
	[CustomFieldSelectionId] [int] IDENTITY(1,1) NOT NULL,
	[CustomFieldId] [int] NOT NULL,
	[CustomFieldSelectionValue] [nchar](255) NOT NULL,
	[CustomFieldSelectionName] [nchar](255) NOT NULL,
	[CustomFieldSelectionSortOrder] [int] NOT NULL CONSTRAINT [DF_ProjectCustomFieldSelection_CustomFieldSelectionSortOrder]  DEFAULT ((0))
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFieldSelection]') AND name = N'PK_ProjectCustomFieldSelection')
ALTER TABLE [dbo].[ProjectCustomFieldSelection] DROP CONSTRAINT [PK_ProjectCustomFieldSelection]
GO

ALTER TABLE [dbo].[ProjectCustomFieldSelection] ADD  CONSTRAINT [PK_ProjectCustomFieldSelection] PRIMARY KEY CLUSTERED 
(
	[CustomFieldSelectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectCustomFieldSelection]  WITH CHECK ADD  CONSTRAINT [FK_ProjectCustomFieldSelection_ProjectCustomFields] FOREIGN KEY([CustomFieldId])
REFERENCES [ProjectCustomFields] ([CustomFieldId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectCustomFieldSelection] CHECK CONSTRAINT [FK_ProjectCustomFieldSelection_ProjectCustomFields]
GO

