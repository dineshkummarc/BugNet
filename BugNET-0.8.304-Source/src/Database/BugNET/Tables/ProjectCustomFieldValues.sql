IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFieldValues]') AND type in (N'U'))
DROP TABLE [dbo].[ProjectCustomFieldValues]
GO

CREATE TABLE [dbo].[ProjectCustomFieldValues](
	[CustomFieldValueId] [int] IDENTITY(1,1) NOT NULL,
	[BugId] [int] NOT NULL,
	[CustomFieldId] [int] NOT NULL,
	[CustomFieldValue] [ntext] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ProjectCustomFieldValues]') AND name = N'PK_ProjectCustomFieldValues')
ALTER TABLE [dbo].[ProjectCustomFieldValues] DROP CONSTRAINT [PK_ProjectCustomFieldValues]
GO

ALTER TABLE [dbo].[ProjectCustomFieldValues] ADD  CONSTRAINT [PK_ProjectCustomFieldValues] PRIMARY KEY CLUSTERED 
(
	[CustomFieldValueId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProjectCustomFieldValues]  WITH CHECK ADD  CONSTRAINT [FK_ProjectCustomFieldValues_Bug] FOREIGN KEY([BugId])
REFERENCES [Bug] ([BugID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ProjectCustomFieldValues] CHECK CONSTRAINT [FK_ProjectCustomFieldValues_Bug]
GO

