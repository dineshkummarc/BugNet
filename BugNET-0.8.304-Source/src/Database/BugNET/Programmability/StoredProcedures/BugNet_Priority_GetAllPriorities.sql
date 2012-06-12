IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Priority_GetAllPriorities]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Priority_GetAllPriorities]
GO

CREATE PROCEDURE [dbo].[BugNet_Priority_GetAllPriorities]
AS
SELECT
	PriorityId,
	Name,
	ImageUrl
FROM 
	Priority



GO

