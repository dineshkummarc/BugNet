IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugTypeCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugTypeCountByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugTypeCountByProject]
	@ProjectId int
AS
	SELECT     t.Name, COUNT(nt.TypeID) AS Number, t.TypeID, t.ImageUrl
	FROM  Type t 
	LEFT OUTER JOIN (SELECT TypeID, ProjectID 
	FROM Bug b WHERE (b.StatusID <> 4) 
	AND (b.StatusID <> 5)) nt 
	ON t.TypeID = nt.TypeID 
	AND nt.ProjectID = @ProjectId
	GROUP BY t.Name, t.TypeID,t.ImageUrl



GO

