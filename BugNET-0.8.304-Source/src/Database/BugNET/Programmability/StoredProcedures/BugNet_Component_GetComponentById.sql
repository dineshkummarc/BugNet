IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Component_GetComponentById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Component_GetComponentById]
GO

CREATE PROCEDURE [dbo].[BugNet_Component_GetComponentById]
	@ComponentId int
AS
SELECT
	ComponentId,
	ProjectId,
	Name,
	ParentComponentId,
	(SELECT COUNT(*) FROM Component WHERE ParentComponentId=c.ComponentId) ChildCount
FROM Component c
WHERE 
ComponentId = @ComponentId



GO

