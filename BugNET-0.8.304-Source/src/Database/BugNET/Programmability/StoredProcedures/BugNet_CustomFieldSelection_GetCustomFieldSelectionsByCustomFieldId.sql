IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomFieldSelection_GetCustomFieldSelectionsByCustomFieldId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomFieldSelection_GetCustomFieldSelectionsByCustomFieldId]
GO



CREATE PROCEDURE [dbo].[BugNet_CustomFieldSelection_GetCustomFieldSelectionsByCustomFieldId] 
	@CustomFieldId Int
AS


SELECT
	CustomFieldSelectionId,
	CustomFieldId,
	CustomFieldSelectionName,
	rtrim(CustomFieldSelectionValue) CustomFieldSelectionValue,
	CustomFieldSelectionSortOrder
FROM
	ProjectCustomFieldSelection 
WHERE
	CustomFieldId = @CustomFieldId
ORDER BY CustomFieldSelectionSortOrder



GO

