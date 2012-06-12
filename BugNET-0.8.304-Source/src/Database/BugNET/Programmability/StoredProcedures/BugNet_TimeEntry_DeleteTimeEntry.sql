IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_TimeEntry_DeleteTimeEntry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_TimeEntry_DeleteTimeEntry]
GO


CREATE PROCEDURE [dbo].[BugNet_TimeEntry_DeleteTimeEntry]
	@BugTimeEntryId int
AS
DELETE 
	BugTimeEntry
WHERE
	BugTimeEntryId = @BugTimeEntryId

GO

