/*
Insert initial data for Notification Templates
*/

MERGE INTO [dbo].[NotificationTemplate] AS Target 
USING (VALUES 
(N'60581938-fcdd-4bcb-910a-04a136803092', N'TlevelDetailsQueried'),
(N'a1b21a18-8555-45b8-9739-f18a902282dc', N'EnglishAndMathsLrsDataQueried')
)
  AS Source ([TemplateId], [TemplateName]) 
ON Target.[TemplateName] = Source.[TemplateName] 
WHEN MATCHED 
	 AND (Target.[TemplateId] <> Source.[TemplateId]) 
THEN 
UPDATE SET 
	[TemplateId] = Source.[TemplateId],
	[TemplateName] = Source.[TemplateName],	
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([TemplateId], [TemplateName], [CreatedBy]) 
	VALUES ([TemplateId], [TemplateName], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;