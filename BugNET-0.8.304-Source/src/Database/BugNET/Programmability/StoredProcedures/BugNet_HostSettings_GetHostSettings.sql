IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_HostSettings_GetHostSettings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_HostSettings_GetHostSettings]
GO

CREATE PROCEDURE [dbo].[BugNet_HostSettings_GetHostSettings] AS

SELECT SettingName, SettingValue FROM HostSettings

GO

