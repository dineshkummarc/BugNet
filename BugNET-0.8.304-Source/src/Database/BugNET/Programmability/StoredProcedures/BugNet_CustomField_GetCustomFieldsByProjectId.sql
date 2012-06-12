IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomField_GetCustomFieldsByProjectId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomField_GetCustomFieldsByProjectId]
GO



CREATE PROCEDURE [dbo].[BugNet_CustomField_GetCustomFieldsByProjectId] 
	@ProjectId Int
AS
SELECT
	ProjectId,
	CustomFieldId,
	CustomFieldName,
	CustomFieldDataType,
	CustomFieldRequired,
	'' CustomFieldValue,
	CustomFieldTypeId
FROM
	ProjectCustomFields
WHERE
	ProjectId = @ProjectId



GO

