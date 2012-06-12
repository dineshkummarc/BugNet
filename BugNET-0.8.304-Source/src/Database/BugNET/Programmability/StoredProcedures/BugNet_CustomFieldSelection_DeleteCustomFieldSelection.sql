IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BugNet_CustomFieldSelection_DeleteCustomFieldSelection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[BugNet_CustomFieldSelection_DeleteCustomFieldSelection]
GO


CREATE PROCEDURE [dbo].[BugNet_CustomFieldSelection_DeleteCustomFieldSelection]
 @CustomSelectionIdToDelete INT
AS
DELETE FROM ProjectCustomFieldSelection WHERE CustomFieldSelectionId = @CustomSelectionIdToDelete



GO

