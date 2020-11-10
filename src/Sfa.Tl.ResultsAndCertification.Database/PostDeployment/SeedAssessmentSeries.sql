/*
Insert initial data for AssessmentSeries
*/

SET IDENTITY_INSERT [dbo].[AssessmentSeries] ON

MERGE INTO [dbo].[AssessmentSeries] AS Target 
USING (VALUES 
  (1, N'Summer 2021', N'Summer 2021', 2021, N'2021-08-10'),
  (2, N'Autumn 2021', N'Autumn 2021', 2021, N'2021-03-07'),
  (3, N'Summer 2022', N'Summer 2022', 2022, N'2022-08-08'),
  (4, N'Autumn 2022', N'Autumn 2022', 2022, N'2022-03-06')
  )
  AS Source ([Id], [Name], [Description], [Year], [EndDate]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Description] <> Source.[Description] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Year] <> Source.[Year])
	 OR (Target.[EndDate] <> Source.[EndDate] COLLATE Latin1_General_CS_AS)
	 )
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[Description] = Source.[Description],
	[Year] = Source.[Year],
	[EndDate] = Source.[EndDate],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [Description], [Year], [EndDate], [CreatedBy]) 
	VALUES ([Id], [Name], [Description], [Year], [EndDate], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[AssessmentSeries] OFF
