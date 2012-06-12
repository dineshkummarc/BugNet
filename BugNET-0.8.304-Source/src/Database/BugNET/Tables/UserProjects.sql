IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserProjects]') AND type in (N'U'))
DROP TABLE [dbo].[UserProjects]
GO

CREATE TABLE [dbo].[UserProjects](
	[UserId] [uniqueidentifier] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[UserProjectId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[UserProjects]') AND name = N'PK_UserProjects_1')
ALTER TABLE [dbo].[UserProjects] DROP CONSTRAINT [PK_UserProjects_1]
GO

ALTER TABLE [dbo].[UserProjects] ADD  CONSTRAINT [PK_UserProjects_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ProjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

