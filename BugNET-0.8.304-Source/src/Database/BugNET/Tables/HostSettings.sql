IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HostSettings]') AND type in (N'U'))
DROP TABLE [dbo].[HostSettings]
GO

CREATE TABLE [dbo].[HostSettings](
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](2000) NULL
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[HostSettings]') AND name = N'PK_HostSettings')
ALTER TABLE [dbo].[HostSettings] DROP CONSTRAINT [PK_HostSettings]
GO

ALTER TABLE [dbo].[HostSettings] ADD  CONSTRAINT [PK_HostSettings] PRIMARY KEY CLUSTERED 
(
	[SettingName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

