IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Bug_UpdateBug]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Bug_UpdateBug]
GO

CREATE PROCEDURE [dbo].[BugNet_Bug_UpdateBug]
  @BugId Int,
  @Summary nvarchar(500),
  @Description ntext,
  @ProjectId Int,
  @ComponentId Int,
  @StatusId Int,
  @PriorityId Int,
  @VersionId Int,
  @TypeId Int,
  @ResolutionId Int,
  @AssignedToUserName nvarchar(255),
  @LastUpdateUserName NVarChar(255),
  @DueDate datetime,
  @FixedInVersionId int,
  @Visibility int,
  @Estimation decimal(4,2)
   
AS
DECLARE @newIssueId Int
-- Get Last Update UserID
DECLARE @LastUpdateUserId UniqueIdentifier
DECLARE @AssignedToUserId UniqueIdentifier

SELECT @LastUpdateUserId = UserId FROM aspnet_users WHERE UserName = @LastUpdateUserName
SELECT @AssignedToUserId = UserId FROM aspnet_users WHERE UserName = @AssignedToUserName

	Update Bug Set
		Summary = @Summary,
		Description = @Description,
		StatusID =@StatusId,
		PriorityID =@PriorityId,
		TypeId = @TypeId,
		ComponentID = @ComponentId,
		AssignedToUserId=@AssignedToUserId,
		ProjectId =@ProjectId,
		ResolutionId =@ResolutionId,
		VersionId =@VersionId,
		LastUpdateUserId = @LastUpdateUserId,
		LastUpdate = GetDate(),
		DueDate = @DueDate,
		FixedInVersionId = @FixedInVersionId,
		Visibility = @Visibility,
		Estimation	= @Estimation
	WHERE 
		BugId = @BugId

GO

