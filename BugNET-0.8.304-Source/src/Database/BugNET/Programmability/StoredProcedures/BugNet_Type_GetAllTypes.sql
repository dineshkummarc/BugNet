IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Type_GetAllTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Type_GetAllTypes]
GO

CREATE PROCEDURE [dbo].[BugNet_Type_GetAllTypes]
AS
SELECT
	TypeId,
	Name,
	ImageUrl
FROM 
	Type



GO

