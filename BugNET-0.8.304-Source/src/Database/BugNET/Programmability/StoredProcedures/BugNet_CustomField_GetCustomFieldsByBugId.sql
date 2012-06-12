IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomField_GetCustomFieldsByBugId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomField_GetCustomFieldsByBugId]
GO



CREATE PROCEDURE [dbo].[BugNet_CustomField_GetCustomFieldsByBugId] 
	@BugId Int
AS
DECLARE @ProjectId Int
SELECT @ProjectId = ProjectId FROM Bug WHERE BugId = @BugId

SELECT
	Fields.ProjectId,
	Fields.CustomFieldId,
	Fields.CustomFieldName,
	Fields.CustomFieldDataType,
	Fields.CustomFieldRequired,
	ISNULL(CustomFieldValue,'') CustomFieldValue,
	Fields.CustomFieldTypeId
FROM
	ProjectCustomFields Fields
	LEFT OUTER JOIN ProjectCustomFieldValues FieldValues ON (Fields.CustomFieldId = FieldValues.CustomFieldId AND FieldValues.BugId = @BugId)
WHERE
	Fields.ProjectId = @ProjectId



GO

