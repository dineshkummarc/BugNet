IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomFieldSelection_GetCustomFieldSelection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomFieldSelection_GetCustomFieldSelection]
GO



CREATE PROCEDURE [dbo].[BugNet_CustomFieldSelection_GetCustomFieldSelection] 
	@CustomFieldSelectionId Int
AS


SELECT
	CustomFieldSelectionId,
	CustomFieldId,
	CustomFieldSelectionName,
	CustomFieldSelectionValue,
	CustomFieldSelectionSortOrder
FROM
	ProjectCustomFieldSelection 
WHERE
	CustomFieldSelectionId = @CustomFieldSelectionId



GO

