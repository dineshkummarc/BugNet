IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Permission_GetAllPermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Permission_GetAllPermissions]
GO

CREATE PROCEDURE [dbo].[BugNet_Permission_GetAllPermissions] AS

SELECT PermissionId,PermissionKey, Name  FROM Permission

GO

