IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Resolution_GetAllResolutions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Resolution_GetAllResolutions]
GO

CREATE PROCEDURE [dbo].[BugNet_Resolution_GetAllResolutions]
AS
SELECT
	ResolutionId,
	Name
FROM 
	Resolution


GO

