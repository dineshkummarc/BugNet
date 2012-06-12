IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_CreateNewProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_CreateNewProject]
GO


CREATE PROCEDURE [dbo].[BugNet_Project_CreateNewProject]
 @Name nvarchar(50),
 @Code nvarchar(3),
 @Description 	nvarchar(1000),
 @ManagerUserName nvarchar(255),
 @UploadPath nvarchar(80),
 @Active int,
 @AccessType int,
 @CreatorUserName nvarchar(255),
 @AllowAttachments int
AS
IF NOT EXISTS( SELECT ProjectId,Code  FROM Project WHERE LOWER(Name) = LOWER(@Name) OR LOWER(Code) = LOWER(@Code) )
BEGIN
	DECLARE @ManagerUserId UNIQUEIDENTIFIER
	DECLARE @CreatorUserId UNIQUEIDENTIFIER
	SELECT @ManagerUserId = UserId FROM aspnet_users WHERE Username = @ManagerUserName
	SELECT @CreatorUserId = UserId FROM aspnet_users WHERE Username = @CreatorUserName
	
	INSERT Project 
	(
		Name,
		Code,
		Description,
		UploadPath,
		ManagerUserId,
		CreateDate,
		CreatorUserId,
		AccessType,
		Active,
		AllowAttachments
	) 
	VALUES
	(
		@Name,
		@Code,
		@Description,
		@UploadPath,
		@ManagerUserId ,
		GetDate(),
		@CreatorUserId,
		@AccessType,
		@Active,
		@AllowAttachments
	)
 	RETURN @@IDENTITY
END
ELSE
  RETURN 0


GO

