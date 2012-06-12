IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Priority_GetPriorityById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Priority_GetPriorityById]
GO

CREATE PROCEDURE [dbo].[BugNet_Priority_GetPriorityById]
	@PriorityId int
AS
SELECT
	PriorityId,
	Name,
	ImageUrl
FROM 
	Priority
WHERE
	PriorityId = @PriorityId



GO

