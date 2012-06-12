IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugStatusCountByProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugStatusCountByProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugStatusCountByProject]
 @ProjectId int
AS
	SELECT s.Name,Count(b.StatusID) as 'Number',s.StatusID 
	From Status s 
	LEFT JOIN Bug b on s.StatusID = b.StatusID AND b.ProjectID = @ProjectId 
	Group BY s.Name,s.StatusID Order By s.StatusID ASC


GO

