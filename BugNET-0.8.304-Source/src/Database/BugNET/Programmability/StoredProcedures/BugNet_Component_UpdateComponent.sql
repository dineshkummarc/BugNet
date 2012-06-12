IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Component_UpdateComponent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Component_UpdateComponent]
GO

CREATE PROCEDURE [dbo].[BugNet_Component_UpdateComponent]
	@ComponentID int,
	@ProjectID int,
	@Name nvarchar(50),
	@ParentComponentID int
AS


UPDATE Component SET
	ProjectID = @ProjectID,
	Name = @Name,
	ParentComponentID = @ParentComponentID
WHERE ComponentID = @ComponentID

GO

