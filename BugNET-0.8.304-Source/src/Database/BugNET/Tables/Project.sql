IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Project]') AND type in (N'U'))
DROP TABLE [dbo].[Project]
GO

CREATE TABLE [dbo].[Project](
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](3) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[UploadPath] [nvarchar](80) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Active] [int] NOT NULL CONSTRAINT [DF_Project_Active]  DEFAULT ((1)),
	[AccessType] [int] NOT NULL,
	[ManagerUserID] [uniqueidentifier] NOT NULL,
	[CreatorUserID] [uniqueidentifier] NOT NULL,
	[AllowAttachments] [bit] NOT NULL CONSTRAINT [DF_Project_AllowAttachments]  DEFAULT ((1))
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Project]') AND name = N'PK__Product__1DE57479')
ALTER TABLE [dbo].[Project] DROP CONSTRAINT [PK__Product__1DE57479]
GO

ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [PK__Product__1DE57479] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

