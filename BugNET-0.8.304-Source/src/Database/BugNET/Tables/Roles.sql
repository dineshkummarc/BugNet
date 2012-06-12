IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
DROP TABLE [dbo].[Roles]
GO

CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[AutoAssign] [bit] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND name = N'PK_Roles')
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [PK_Roles]
GO

ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Project] FOREIGN KEY([ProjectId])
REFERENCES [Project] ([ProjectID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Project]
GO

