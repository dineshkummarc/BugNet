IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_TimeEntry_CreateNewTimeEntry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_TimeEntry_CreateNewTimeEntry]
GO



CREATE PROCEDURE [dbo].[BugNet_TimeEntry_CreateNewTimeEntry]
	@BugId int,
	@CreatorUserName nvarchar(255),
	@WorkDate datetime ,
	@Duration decimal(4,2),
	@BugCommentId int
AS
-- Get Last Update UserID
DECLARE @CreatorUserId uniqueidentifier
SELECT @CreatorUserId = UserId FROM aspnet_users WHERE Username = @CreatorUserName
INSERT BugTimeEntry
(
	BugId,
	CreatedUserId,
	WorkDate,
	Duration,
	BugCommentId
) 
VALUES 
(
	@BugId,
	@CreatorUserId,
	@WorkDate,
	@Duration,
	@BugCommentID
)
RETURN @@IDENTITY

GO

