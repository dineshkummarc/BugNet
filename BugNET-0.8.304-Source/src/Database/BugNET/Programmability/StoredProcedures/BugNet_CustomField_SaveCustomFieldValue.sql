IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomField_SaveCustomFieldValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomField_SaveCustomFieldValue]
GO



CREATE PROCEDURE [dbo].[BugNet_CustomField_SaveCustomFieldValue]
	@BugId Int,
	@CustomFieldId Int, 
	@CustomFieldValue NVarChar(255)
AS
UPDATE ProjectCustomFieldValues SET
	CustomFieldValue = @CustomFieldValue
WHERE
	BugId = @BugId
	AND CustomFieldId = @CustomFieldId

IF @@ROWCOUNT = 0
	INSERT ProjectCustomFieldValues
	(
		BugId,
		CustomFieldId,
		CustomFieldValue
	)
	VALUES
	(
		@BugId,
		@CustomFieldId,
		@CustomFieldValue
	)



GO

