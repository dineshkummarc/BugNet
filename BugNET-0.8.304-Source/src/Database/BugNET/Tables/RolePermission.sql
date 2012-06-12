IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolePermission]') AND type in (N'U'))
DROP TABLE [dbo].[RolePermission]
GO

CREATE TABLE [dbo].[RolePermission](
	[RolePermissionId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[RolePermission]') AND name = N'PK_RolePermission')
ALTER TABLE [dbo].[RolePermission] DROP CONSTRAINT [PK_RolePermission]
GO

ALTER TABLE [dbo].[RolePermission] ADD  CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED 
(
	[RolePermissionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RolePermission]  WITH NOCHECK ADD  CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY([PermissionId])
REFERENCES [Permission] ([PermissionId])
GO

ALTER TABLE [dbo].[RolePermission] CHECK CONSTRAINT [FK_RolePermission_Permission]
GO

ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_Roles] FOREIGN KEY([RoleId])
REFERENCES [Roles] ([RoleId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[RolePermission] CHECK CONSTRAINT [FK_RolePermission_Roles]
GO

