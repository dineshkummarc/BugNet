IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_GetBugsByCriteria]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_GetBugsByCriteria]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_GetBugsByCriteria]
(
    @ProjectId int = NULL,
    @ComponentId int = NULL,
    @VersionId int = NULL,
    @PriorityId int = NULL,
    @TypeId int = NULL,
    @ResolutionId int = NULL,
    @StatusId int = NULL,
    @AssignedToUserName nvarchar(256) = NULL,
    @Keywords nvarchar(256) = NULL,
    @IncludeComments bit = NULL,
    @ReporterUserName nvarchar(255) = NULL,
    @MilestoneId int = NULL
)
AS
  
DECLARE @AssignedToUserId UNIQUEIDENTIFIER
SELECT @AssignedToUserId = UserId FROM aspnet_users WHERE Username = @AssignedToUserName
DECLARE @ReporterUserId UNIQUEIDENTIFIER
SELECT @ReporterUserId = UserId FROM aspnet_users WHERE Username = @ReporterUserName
  
IF @StatusId = 0

SELECT
    *
FROM
    BugsView 
WHERE  
((@ProjectId IS NULL) OR (ProjectId = @ProjectId)) AND
    ((@ComponentId IS NULL) OR (ComponentId = @ComponentId)) AND
    ((@VersionId IS NULL) OR (VersionId = @VersionId)) AND
    ((@MilestoneId IS NULL) OR (MilestoneId = @MilestoneId)) AND
    ((@PriorityId IS NULL) OR (PriorityId = @PriorityId))AND
    ((@TypeId IS NULL) OR (TypeId = @TypeId)) AND
    ((@ResolutionId IS NULL) OR (ResolutionId = @ResolutionId)) AND
    ((@StatusId IS NULL) OR (StatusId In (1,2,3))) AND
    ((@ReporterUserId IS NULL) OR (ReporterUserId = @ReporterUserId)) AND
    ((@Keywords IS NULL) OR (Description LIKE '%' + @Keywords + '%' )  OR (Title LIKE '%' + @Keywords + '%' ) ) AND
    ((@AssignedToUserName IS NULL) OR (@AssignedToUserName = '-1' AND AssignedToUserId IS NULL) 
    OR (AssignedToUserId IS NOT NULL AND AssignedToUserId = @AssignedToUserId))
 ORDER BY PriorityId ASC
 
ELSE

SELECT
    *
FROM
    BugsView
WHERE
    ((@ProjectId IS NULL) OR (ProjectId = @ProjectId)) AND
    ((@ComponentId IS NULL) OR (ComponentId = @ComponentId)) AND
    ((@VersionId IS NULL) OR (VersionId = @VersionId)) AND
    ((@MilestoneId  IS NULL) OR (MilestoneId  = @MilestoneId )) AND
    ((@PriorityId IS NULL) OR (PriorityId = @PriorityId))AND
    ((@TypeId IS NULL) OR (TypeId = @TypeId)) AND
    ((@ResolutionId IS NULL) OR (ResolutionId = @ResolutionId)) AND
    ((@StatusId IS NULL) OR (StatusId = @StatusId)) AND
    ((@AssignedToUserName IS NULL) OR (@AssignedToUserName = '-1' AND AssignedToUserId IS NULL) 
    OR (AssignedToUserId IS NOT NULL AND AssignedToUserId = @AssignedToUserId)) AND
    ((@ReporterUserId IS NULL) OR (ReporterUserId = @ReporterUserId)) AND
    ((@Keywords IS NULL) OR (Description LIKE '%' + @Keywords + '%' )  OR (Title LIKE '%' + @Keywords + '%' ))
  ORDER BY PriorityId ASC

GO

