IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Role_RoleExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Role_RoleExists]
GO

CREATE PROCEDURE dbo.BugNet_Role_RoleExists
    @RoleName   nvarchar(256),
    @ProjectId	int
AS
BEGIN
    IF (EXISTS (SELECT RoleName FROM dbo.Roles WHERE @RoleName = RoleName AND ProjectId = @ProjectId))
        RETURN(1)
    ELSE
        RETURN(0)
END


GO

