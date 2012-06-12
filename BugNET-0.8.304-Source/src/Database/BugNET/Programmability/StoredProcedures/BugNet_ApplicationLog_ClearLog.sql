IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_ApplicationLog_ClearLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_ApplicationLog_ClearLog]
GO

CREATE PROCEDURE [dbo].[BugNet_ApplicationLog_ClearLog] 
	
AS
	DELETE FROM Log


GO

