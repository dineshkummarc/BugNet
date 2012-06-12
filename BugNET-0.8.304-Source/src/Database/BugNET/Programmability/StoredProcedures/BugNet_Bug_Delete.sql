IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_Delete]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_Delete]
	@BugId Int
AS
DELETE 
	Bug
WHERE
	BugId = @BugId


GO

