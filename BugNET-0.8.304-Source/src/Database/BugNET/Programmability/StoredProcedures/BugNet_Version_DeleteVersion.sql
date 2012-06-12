IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Version_DeleteVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Version_DeleteVersion]
GO

CREATE PROCEDURE [dbo].[BugNet_Version_DeleteVersion]
	@VersionId Int 
AS
DELETE 
	Version
WHERE
	VersionId = @VersionId
/*We need to delete all bugs with this version too */
DELETE
	Bug
WHERE 
	VersionId = @VersionId
	
IF @@ROWCOUNT > 0 
	RETURN 0
ELSE
	RETURN 1


GO

