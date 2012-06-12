IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Resolution_GetResolutionById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Resolution_GetResolutionById]
GO

CREATE PROCEDURE [dbo].[BugNet_Resolution_GetResolutionById]
	@ResolutionId int
AS
SELECT
	ResolutionId,
	Name
FROM 
	Resolution
WHERE
	ResolutionId = @ResolutionId


GO

