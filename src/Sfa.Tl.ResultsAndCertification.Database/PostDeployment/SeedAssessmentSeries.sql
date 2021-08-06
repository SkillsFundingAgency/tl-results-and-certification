/*
Insert initial data for AssessmentSeries
*/

SET IDENTITY_INSERT [dbo].[AssessmentSeries] ON

MERGE INTO [dbo].[AssessmentSeries] AS Target 
USING (VALUES 
  (1, N'Summer 2021', N'Summer 2021', 2021, N'2020-11-26', N'2021-08-10', N'2021-09-24'),
  (2, N'Autumn 2021', N'Autumn 2021', 2021, N'2021-08-11', N'2022-03-07', N'2022-04-21'),
  (3, N'Summer 2022', N'Summer 2022', 2022, N'2022-03-08', N'2022-08-08', N'2022-09-22'),
  (4, N'Autumn 2022', N'Autumn 2022', 2022, N'2022-08-09', N'2022-03-06', N'2022-04-20')
  )
  AS Source ([Id], [Name], [Description], [Year], [StartDate], [EndDate], [AppealEndDate]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Description] <> Source.[Description] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Year] <> Source.[Year])
	 OR (Target.[StartDate] <> Source.[StartDate] COLLATE Latin1_General_CS_AS)
	 OR (Target.[EndDate] <> Source.[EndDate] COLLATE Latin1_General_CS_AS)
	 OR (Target.[AppealEndDate] <> Source.AppealEndDate COLLATE Latin1_General_CS_AS)
	 )
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[Description] = Source.[Description],
	[Year] = Source.[Year],
	[StartDate] = Source.[StartDate],
	[EndDate] = Source.[EndDate],
	[AppealEndDate] = Source.[AppealEndDate],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [Description], [Year], [StartDate], [EndDate], [AppealEndDate], [CreatedBy]) 
	VALUES ([Id], [Name], [Description], [Year], [StartDate], [EndDate],[AppealEndDate], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[AssessmentSeries] OFF
