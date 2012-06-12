IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_CreateNewBug]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_CreateNewBug]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_CreateNewBug]
  @Summary nvarchar(500),
  @Description ntext,
  @ProjectId Int,
  @ComponentId Int,
  @StatusId Int,
  @PriorityId Int,
  @VersionId Int,
  @TypeId Int,
  @ResolutionId Int,
  @AssignedToUserName NVarChar(255),
  @ReporterUserName NVarChar(255),
  @DueDate datetime,
  @FixedInVersionId int,
  @Visibility int,
  @Estimation decimal(4,2)
AS
DECLARE @newIssueId Int
-- Get Reporter UserID
DECLARE @AssignedToUserId	UNIQUEIDENTIFIER
DECLARE @ReporterUserId		UNIQUEIDENTIFIER

SELECT @AssignedToUserId = UserId FROM aspnet_users WHERE Username = @AssignedToUserName
SELECT @ReporterUserId = UserId FROM aspnet_users WHERE Username = @ReporterUserName

	INSERT Bug
	(
		Summary,
		Description,
		ReporterUserId,
		ReportedDate,
		StatusId,
		PriorityId,
		TypeId,
		ComponentId,
		AssignedToUserId,
		ProjectId,
		ResolutionId,
		VersionId,
		LastUpdateUserId,
		LastUpdate,
		DueDate,
		FixedInVersionId,
		Visibility,
		Estimation
	)
	VALUES
	(
		@Summary,
		@Description,
		@ReporterUserId,
		GetDate(),
		@StatusId,
		@PriorityId,
		@TypeId,
		@ComponentId,
		@AssignedToUserId,
		@ProjectId,
		@ResolutionId,
		@VersionId,
		@ReporterUserId,
		GetDate(),
		@DueDate,
		@FixedInVersionId,
		@Visibility,
		@Estimation
	)
RETURN scope_identity()

GO

