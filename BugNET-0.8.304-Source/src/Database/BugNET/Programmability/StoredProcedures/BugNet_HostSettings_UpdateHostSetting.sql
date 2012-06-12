IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_HostSettings_UpdateHostSetting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_HostSettings_UpdateHostSetting]
GO


CREATE PROCEDURE [dbo].[BugNet_HostSettings_UpdateHostSetting]
 @SettingName	nvarchar(50),
 @SettingValue 	nvarchar(2000)
AS
UPDATE HostSettings SET
	SettingName = @SettingName,
	SettingValue = @SettingValue
WHERE
	SettingName  = @SettingName


GO

