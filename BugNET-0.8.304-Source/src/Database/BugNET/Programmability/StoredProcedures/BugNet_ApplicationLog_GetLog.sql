IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_ApplicationLog_GetLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_ApplicationLog_GetLog]
GO

CREATE PROCEDURE [dbo].[BugNet_ApplicationLog_GetLog] 
	@startRowIndex int ,
    @maximumRows int
AS

DECLARE @first_id int, @startRow int
	
-- A check can be added to make sure @startRowIndex isn't > count(1)
-- from employees before doing any actual work unless it is guaranteed
-- the caller won't do that

-- Get the first employeeID for our page of records
SET ROWCOUNT @startRowIndex
SELECT @first_id = Id FROM Log ORDER BY Id

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @maximumRows


SELECT L.* FROM Log L
   
WHERE Id >= @first_id
ORDER BY L.Id

SET ROWCOUNT 0




GO

