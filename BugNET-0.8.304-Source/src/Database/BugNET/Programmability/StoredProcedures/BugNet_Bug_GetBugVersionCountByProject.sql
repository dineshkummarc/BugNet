IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugVersionCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugVersionCountByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugVersionCountByProject]
	@ProjectId int
AS
	SELECT v.Name, COUNT(nt.VersionID) AS Number, v.VersionID 
	FROM Version v 
	LEFT OUTER JOIN (SELECT VersionID  
	FROM Bug b  
	WHERE (b.StatusID <> 4) AND (b.StatusID <> 5)) nt ON v.VersionID = nt.VersionID 
	WHERE (v.ProjectID = @ProjectId) 
	GROUP BY v.Name, v.VersionID,v.SortOrder
	ORDER BY v.SortOrder ASC

GO

