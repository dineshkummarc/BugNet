IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_ApplicationLog_GetLogCount]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_ApplicationLog_GetLogCount]
GO

CREATE PROCEDURE [dbo].[BugNet_ApplicationLog_GetLogCount] 
AS

SELECT COUNT(Id) FROM Log




GO

