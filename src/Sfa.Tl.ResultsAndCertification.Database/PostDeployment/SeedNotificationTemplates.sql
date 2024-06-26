﻿/*
Insert initial data for Notification Templates
*/

MERGE INTO [dbo].[NotificationTemplate] AS Target 
USING (VALUES 
(N'99a03bd6-89e5-4fc7-9248-e8c695245d3f', N'TlevelDetailsQueriedUserNotification'),
(N'52baea90-8784-418a-8134-b1d9ac891b6f', N'TlevelDetailsQueriedTechnicalTeamNotification'),
(N'9a033adb-cc33-461c-8a77-e3fc15582dfb', N'FunctionJobFailedNotification'),
(N'bc76d0c9-c92f-4cdb-8b7e-00fab2e7e046', N'PrintingJobFailedNotification'),
(N'6b28c163-1730-4627-9120-2ae4510c4066', N'GradeChangeRequestUserNotification'),
(N'e4087caf-b68c-4c37-840e-524666c7652a', N'AppealGradeAfterDeadlineRequestUserNotification'),
(N'a889ff99-af5e-4c66-adff-fa07f7c228bf', N'AppealGradeAfterDeadlineRequestTechnicalTeamNotification'),
(N'daa342c6-8f92-4695-80ee-f251a7844449', N'GradeChangeRequestTechnicalTeamNotificationCoreComponent'),
(N'559a3508-432d-4dd2-ac87-e7e15c81bcf3', N'GradeChangeRequestTechnicalTeamNotificationSpecialism')
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