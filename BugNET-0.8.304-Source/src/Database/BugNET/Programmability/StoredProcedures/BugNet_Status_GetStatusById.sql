IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Status_GetStatusById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Status_GetStatusById]
GO

CREATE PROCEDURE [dbo].[BugNet_Status_GetStatusById]
	@StatusId int
AS
SELECT
	StatusId,
	Name
FROM 
	Status
WHERE
	StatusId = @StatusId


GO

