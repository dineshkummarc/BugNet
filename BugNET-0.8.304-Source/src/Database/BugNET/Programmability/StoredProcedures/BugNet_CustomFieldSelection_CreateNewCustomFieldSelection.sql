IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomFieldSelection_CreateNewCustomFieldSelection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomFieldSelection_CreateNewCustomFieldSelection]
GO


CREATE PROCEDURE [dbo].[BugNet_CustomFieldSelection_CreateNewCustomFieldSelection]
	@CustomFieldId Int,
	@CustomFieldSelectionValue NVarChar(255),
	@CustomFieldSelectionName NVarChar(255)
AS

DECLARE @CustomFieldSelectionSortOrder int
SELECT @CustomFieldSelectionSortOrder = ISNULL(MAX(CustomFieldSelectionSortOrder),0) + 1 FROM ProjectCustomFieldSelection

IF NOT EXISTS(SELECT CustomFieldSelectionId FROM ProjectCustomFieldSelection WHERE CustomFieldId = @CustomFieldId AND LOWER(CustomFieldSelectionName) = LOWER(@CustomFieldSelectionName) )
BEGIN
	INSERT ProjectCustomFieldSelection
	(
		CustomFieldId,
		CustomFieldSelectionValue,
		CustomFieldSelectionName,
		CustomFieldSelectionSortOrder
	)
	VALUES
	(
		@CustomFieldId,
		@CustomFieldSelectionValue,
		@CustomFieldSelectionName,
		@CustomFieldSelectionSortOrder
		
	)

	RETURN @@IDENTITY
END
RETURN 0



GO

