IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Version_GetVersionByProjectId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Version_GetVersionByProjectId]
GO

CREATE PROCEDURE [dbo].[BugNet_Version_GetVersionByProjectId]
 @ProjectId INT
AS
SELECT
	VersionId,
	ProjectId,
	Name,
	SortOrder
FROM 
	Version
WHERE
	ProjectId = @ProjectId
ORDER BY SortOrder ASC

GO

