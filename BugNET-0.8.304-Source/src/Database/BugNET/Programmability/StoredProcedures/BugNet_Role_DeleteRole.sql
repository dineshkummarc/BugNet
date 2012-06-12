IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_DeleteRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_DeleteRole]
GO

CREATE PROCEDURE dbo.BugNet_Role_DeleteRole
	@RoleId Int 
AS
DELETE 
	Roles
WHERE
	RoleId = @RoleId
IF @@ROWCOUNT > 0 
	RETURN 0
ELSE
	RETURN 1

GO

