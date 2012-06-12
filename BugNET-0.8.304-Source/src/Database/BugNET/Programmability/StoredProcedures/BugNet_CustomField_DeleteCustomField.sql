IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomField_DeleteCustomField]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomField_DeleteCustomField]
GO



CREATE PROCEDURE [dbo].[BugNet_CustomField_DeleteCustomField]
 @CustomIdToDelete INT
AS
DELETE FROM ProjectCustomFields WHERE CustomFieldId = @CustomIdToDelete

DELETE FROM ProjectCustomFieldValues WHERE CustomFieldId = @CustomIdToDelete




GO

