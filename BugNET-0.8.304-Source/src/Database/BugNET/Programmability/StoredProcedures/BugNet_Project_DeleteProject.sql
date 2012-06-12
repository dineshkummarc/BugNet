IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_DeleteProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_DeleteProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_DeleteProject]
	@ProjectId int
AS

DELETE FROM Project where ProjectId = @ProjectId

IF @@ROWCOUNT > 0 
	RETURN 0
ELSE
	RETURN 1

GO

