IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Version_GetVersionById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Version_GetVersionById]
GO

CREATE PROCEDURE [dbo].[BugNet_Version_GetVersionById]
 @VersionId INT
AS
SELECT
	VersionId,
	ProjectId,
	Name,
	SortOrder
FROM 
	Version
WHERE
	VersionId = @VersionId


GO

