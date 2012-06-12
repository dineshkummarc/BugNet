IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomFieldSelection_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomFieldSelection_Update]
GO

CREATE PROCEDURE [dbo].[BugNet_CustomFieldSelection_Update]
	@CustomFieldSelectionId int,
	@CustomFieldId int,
	@CustomFieldSelectionName nvarchar(255),
	@CustomFieldSelectionValue nvarchar(255),
	@CustomFieldSelectionSortOrder int
AS


UPDATE ProjectCustomFieldSelection SET
	CustomFieldId = @CustomFieldId,
	CustomFieldSelectionName = @CustomFieldSelectionName,
	CustomFieldSelectionValue = @CustomFieldSelectionValue,
	CustomFieldSelectionSortOrder = @CustomFieldSelectionSortOrder
WHERE CustomFieldSelectionId= @CustomFieldSelectionId

GO

