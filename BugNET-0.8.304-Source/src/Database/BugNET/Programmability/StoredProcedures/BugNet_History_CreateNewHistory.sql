IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_History_CreateNewHistory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_History_CreateNewHistory]
GO

CREATE PROCEDURE [dbo].[BugNet_History_CreateNewHistory]
  @BugId int,
  @CreatedUserName nvarchar(255),
  @FieldChanged nvarchar(50),
  @OldValue nvarchar(50),
  @NewValue nvarchar(50)
AS
DECLARE @CreatedUserId UniqueIdentifier
SELECT @CreatedUserId = UserId FROM aspnet_users WHERE UserName = @CreatedUserName

	INSERT BugHistory
	(
		BugId,
		CreatedUserId,
		FieldChanged,
		OldValue,
		NewValue,
		CreatedDate
	)
	VALUES
	(
		@BugId,
		@CreatedUserId,
		@FieldChanged,
		@OldValue,
		@NewValue,
		GetDate()
	)
RETURN @@IDENTITY

GO

