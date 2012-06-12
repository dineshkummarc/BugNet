IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Type_GetTypeById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Type_GetTypeById]
GO

CREATE PROCEDURE [dbo].[BugNet_Type_GetTypeById]
	@TypeId int
AS
SELECT
	TypeId,
	Name,
	ImageUrl
FROM 
	Type
WHERE
	TypeId = @TypeId



GO

