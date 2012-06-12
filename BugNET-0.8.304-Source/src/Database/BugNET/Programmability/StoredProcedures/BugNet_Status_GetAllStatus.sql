IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Status_GetAllStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Status_GetAllStatus]
GO

CREATE PROCEDURE [dbo].[BugNet_Status_GetAllStatus]
AS
SELECT
	StatusId,
	Name
FROM 
	Status


GO

