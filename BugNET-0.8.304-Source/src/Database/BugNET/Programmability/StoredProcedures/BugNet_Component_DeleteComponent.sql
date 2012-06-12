IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Component_DeleteComponent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Component_DeleteComponent]
GO

CREATE PROCEDURE [dbo].[BugNet_Component_DeleteComponent]
	@ComponentId Int 
AS
DELETE 
	Component
WHERE
	ComponentId = @ComponentId
IF @@ROWCOUNT > 0 
	RETURN 0
ELSE
	RETURN 1


GO

