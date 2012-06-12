IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugPriorityCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugPriorityCountByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugPriorityCountByProject]
 @ProjectId int
AS
	SELECT p.Name, COUNT(nt.PriorityID) AS Number, p.PriorityID 
	FROM   Priority p 
	LEFT OUTER JOIN (SELECT  PriorityID, ProjectID FROM   
	Bug b WHERE  (b.StatusID <> 4) AND (b.StatusID <> 5)) nt 
	ON p.PriorityID = nt.PriorityID AND nt.ProjectID = @ProjectId
	GROUP BY p.Name, p.PriorityID


GO

