IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_Project_CloneProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_Project_CloneProject]
GO

CREATE PROCEDURE [dbo].[BugNet_Project_CloneProject] 
(
  @ProjectId INT,
  @ProjectName NVarChar(256)
)
AS
-- Copy Project
INSERT Project
(
  Name,
  Code,
  Description,
  UploadPath,
  CreateDate,
  Active,
  AccessType,
  CreatorUserId,
  ManagerUserId,
  AllowAttachments
)
SELECT
  @ProjectName,
  Code,
  Description,
  UploadPath,
  GetDate(),
  Active,
  AccessType,
  CreatorUserId,
  ManagerUserId,
  AllowAttachments
FROM 
  Project
WHERE
  ProjectId = @ProjectId
  
DECLARE @NewProjectId INT
SET @NewProjectId = @@IDENTITY

-- Copy Versions / Milestones
INSERT Version
(
  ProjectId,
  Name,
  SortOrder
)
SELECT
  @NewProjectId,
  Name,
  SortOrder
FROM
  Version
WHERE
  ProjectId = @ProjectID  

-- Copy Project Members
INSERT UserProjects
(
  UserId,
  ProjectId,
  CreatedDate
)
SELECT
  UserId,
  @NewProjectId,
  GetDate()
FROM
  UserProjects
WHERE
  ProjectId = @ProjectId

-- Copy Project Roles
INSERT Roles
( 
	ProjectId,
	RoleName,
	Description,
	AutoAssign
)
SELECT 
	@NewProjectId,
	RoleName,
	Description,
	AutoAssign
FROM
	Roles
WHERE
	ProjectId = @ProjectId

CREATE TABLE #OldRoles
(
  OldRowNumber INT IDENTITY,
  OldRoleId INT,
)

INSERT #OldRoles
(
  OldRoleId
)
SELECT
	RoleId
FROM
  Roles
WHERE
  ProjectId = @ProjectId
ORDER BY RoleId

CREATE TABLE #NewRoles
(
  NewRowNumber INT IDENTITY,
  NewRoleId INT,
)

INSERT #NewRoles
(
  NewRoleId
)
SELECT
  RoleId
FROM
  Roles
WHERE
  ProjectId = @NewProjectId
ORDER BY RoleId

INSERT UserRoles
(
	UserId,
	RoleId
)
SELECT 
	UserId,
	RoleId = NewRoleId
FROM #OldRoles INNER JOIN #NewRoles ON  OldRowNumber = NewRowNumber
INNER JOIN UserRoles UR ON UR.RoleId = OldRoleId

-- Copy Custom Fields
INSERT ProjectCustomFields
(
  ProjectId,
  CustomFieldName,
  CustomFieldRequired,
  CustomFieldDataType,
  CustomFieldTypeId
)
SELECT
  @NewProjectId,
  CustomFieldName,
  CustomFieldRequired,
  CustomFieldDataType,
  CustomFieldTypeId
FROM
  ProjectCustomFields
WHERE
  ProjectId = @ProjectId
  
-- Copy Custom Field Selections
CREATE TABLE #OldCustomFields
(
  OldRowNumber INT IDENTITY,
  OldCustomFieldId INT,
)
INSERT #OldCustomFields
(
  OldCustomFieldId
)
SELECT
	CustomFieldId
FROM
  ProjectCustomFields
WHERE
  ProjectId = @ProjectId
ORDER BY CustomFieldId

CREATE TABLE #NewCustomFields
(
  NewRowNumber INT IDENTITY,
  NewCustomFieldId INT,
)

INSERT #NewCustomFields
(
  NewCustomFieldId
)
SELECT
  CustomFieldId
FROM
  ProjectCustomFields
WHERE
  ProjectId = @NewProjectId
ORDER BY CustomFieldId

INSERT ProjectCustomFieldSelection
(
	CustomFieldId,
	CustomFieldSelectionValue,
	CustomFieldSelectionName,
	CustomFieldSelectionSortOrder
)
SELECT 
	CustomFieldId = NewCustomFieldId,
	CustomFieldSelectionValue,
	CustomFieldSelectionName,
	CustomFieldSelectionSortOrder
FROM #OldCustomFields INNER JOIN #NewCustomFields ON  OldRowNumber = NewRowNumber
INNER JOIN ProjectCustomFieldSelection CFS ON CFS.CustomFieldId = OldCustomFieldId

-- Copy Project Mailboxes
INSERT ProjectMailbox
(
  MailBox,
  ProjectId,
  AssignToUserId,
  IssueTypeId
)
SELECT
  Mailbox,
  @NewProjectId,
  AssignToUserId,
  IssueTypeId
FROM
  ProjectMailBox
WHERE
  ProjectId = @ProjectId

-- Copy Categories
INSERT Component
(
  ProjectId,
  Name,
  ParentComponentId
)
SELECT
  @NewProjectId,
  Name,
  ParentComponentId
FROM
  Component
WHERE
  ProjectId = @ProjectId  


CREATE TABLE #OldCategories
(
  OldRowNumber INT IDENTITY,
  OldComponentId INT,
)

INSERT #OldCategories
(
  OldComponentId
)
SELECT
  ComponentId
FROM
  Component
WHERE
  ProjectId = @ProjectId
ORDER BY ComponentId

CREATE TABLE #NewCategories
(
  NewRowNumber INT IDENTITY,
  NewComponentId INT,
)

INSERT #NewCategories
(
  NewComponentId
)
SELECT
  ComponentId
FROM
  Component
WHERE
  ProjectId = @NewProjectId
ORDER BY ComponentId


UPDATE Component SET
  ParentComponentId = NewComponentId
FROM
  #OldCategories INNER JOIN #NewCategories ON OldRowNumber = NewRowNumber
WHERE
  ProjectId = @NewProjectId
  And ParentComponentID = OldComponentId 