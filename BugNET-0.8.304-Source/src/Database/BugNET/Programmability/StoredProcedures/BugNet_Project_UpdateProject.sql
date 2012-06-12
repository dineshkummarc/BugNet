IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_UpdateProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_UpdateProject]
GO


CREATE PROCEDURE [dbo].[BugNet_Project_UpdateProject]
 @ProjectId 		int,
 @Name				nvarchar(50),
 @Code				nvarchar(3),
 @Description 		nvarchar(1000),
 @ManagerUserName	nvarchar(255),
 @UploadPath 		nvarchar(80),
 @AccessType		int,
 @Active 			int,
 @AllowAttachments	bit
AS
DECLARE @ManagerUserId UNIQUEIDENTIFIER
SELECT @ManagerUserId = UserId FROM aspnet_users WHERE Username = @ManagerUserName

UPDATE Project SET
	Name = @Name,
	Code = @Code,
	Description = @Description,
	ManagerUserId = @ManagerUserId,
	UploadPath = @UploadPath,
	AccessType = @AccessType,
	Active = @Active,
	AllowAttachments = @AllowAttachments
WHERE
	ProjectId = @ProjectId


GO

