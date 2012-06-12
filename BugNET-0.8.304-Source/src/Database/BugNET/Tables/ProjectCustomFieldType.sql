IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFieldType]') AND type in (N'U'))
DROP TABLE [dbo].[ProjectCustomFieldType]
GO

CREATE TABLE [dbo].[ProjectCustomFieldType](
	[CustomFieldTypeId] [int] IDENTITY(1,1) NOT NULL,
	[CustomFieldTypeName] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFieldType]') AND name = N'PK_ProjectCustomFieldType')
ALTER TABLE [dbo].[ProjectCustomFieldType] DROP CONSTRAINT [PK_ProjectCustomFieldType]
GO

ALTER TABLE [dbo].[ProjectCustomFieldType] ADD  CONSTRAINT [PK_ProjectCustomFieldType] PRIMARY KEY CLUSTERED 
(
	[CustomFieldTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

