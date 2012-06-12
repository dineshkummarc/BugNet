IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomField_CreateNewCustomField]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomField_CreateNewCustomField]
GO


CREATE PROCEDURE [dbo].[BugNet_CustomField_CreateNewCustomField]
	@ProjectId Int,
	@CustomFieldName NVarChar(50),
	@CustomFieldDataType Int,
	@CustomFieldRequired Bit,
	@CustomFieldTypeId	int
AS
IF NOT EXISTS(SELECT CustomFieldId FROM ProjectCustomFields WHERE ProjectID = @ProjectID AND LOWER(CustomFieldName) = LOWER(@CustomFieldName) )
BEGIN
	INSERT ProjectCustomFields
	(
		ProjectId,
		CustomFieldName,
		CustomFieldDataType,
		CustomFieldRequired,
		CustomFieldTypeId
	)
	VALUES
	(
		@ProjectId,
		@CustomFieldName,
		@CustomFieldDataType,
		@CustomFieldRequired,
		@CustomFieldTypeId
	)

	RETURN @@IDENTITY
END
RETURN 0



GO

