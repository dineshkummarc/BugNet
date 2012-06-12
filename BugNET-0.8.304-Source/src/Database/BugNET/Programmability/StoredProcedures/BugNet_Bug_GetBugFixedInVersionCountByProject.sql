IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugFixedInVersionCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugFixedInVersionCountByProject]
GO

CREATE PROCEDURE BugNet_Bug_GetBugFixedInVersionCountByProject 
	@ProjectId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT v.Name, COUNT(nt.FixedInVersionId) AS Number, v.VersionID
	FROM Version v 
	LEFT OUTER JOIN (SELECT FixedInVersionId
	FROM Bug b  
	WHERE (b.StatusID <> 4) AND (b.StatusID <> 5)) nt ON v.VersionID = nt.FixedInVersionId 
	WHERE (v.ProjectID = @ProjectId) 
	GROUP BY v.Name, v.VersionID,v.SortOrder
	ORDER BY v.SortOrder ASC
END


GO

