IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFields]') AND type in (N'U'))
DROP TABLE [dbo].[ProjectCustomFields]
GO

CREATE TABLE [dbo].[ProjectCustomFields](
	[CustomFieldId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[CustomFieldName] [nvarchar](50) NOT NULL,
	[CustomFieldRequired] [bit] NOT NULL,
	[CustomFieldDataType] [int] NOT NULL,
	[CustomFieldTypeId] [int] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFields]') AND name = N'PK_ProjectCustomFields')
ALTER TABLE [dbo].[ProjectCustomFields] DROP CONSTRAINT [PK_ProjectCustomFields]
GO

ALTER TABLE [dbo].[ProjectCustomFields] ADD  CONSTRAINT [PK_ProjectCustomFields] PRIMARY KEY CLUSTERED 
(
	[CustomFieldId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectCustomFields]  WITH CHECK ADD  CONSTRAINT [FK_ProjectCustomFields_Project] FOREIGN KEY([ProjectId])
REFERENCES [Project] ([ProjectID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectCustomFields] CHECK CONSTRAINT [FK_ProjectCustomFields_Project]
GO

ALTER TABLE [dbo].[ProjectCustomFields]  WITH CHECK ADD  CONSTRAINT [FK_ProjectCustomFields_ProjectCustomFieldType] FOREIGN KEY([CustomFieldTypeId])
REFERENCES [ProjectCustomFieldType] ([CustomFieldTypeId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectCustomFields] CHECK CONSTRAINT [FK_ProjectCustomFields_ProjectCustomFieldType]
GO

