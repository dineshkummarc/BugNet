IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Component_GetRootComponentsByProjectId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Component_GetRootComponentsByProjectId]
GO

CREATE PROCEDURE [dbo].[BugNet_Component_GetRootComponentsByProjectId]
	@ProjectId int
AS
SELECT
	ComponentId,
	ProjectId,
	Name,
	ParentComponentId,
	(SELECT COUNT(*) FROM Component WHERE ParentComponentId=c.ComponentId) ChildCount
FROM Component c
WHERE 
ProjectId = @ProjectId AND c.ParentComponentId = 0
ORDER BY Name

GO

