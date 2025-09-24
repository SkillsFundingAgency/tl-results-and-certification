/*
Insert initial data for AssessmentSeries
*/

SET IDENTITY_INSERT [dbo].[AssessmentSeries] ON

MERGE INTO [dbo].[AssessmentSeries] AS Target 
USING (VALUES 
  (1, 1, N'Summer 2021', N'Summer 2021', 2021, N'2020-11-26', N'2021-08-10', N'2021-09-24', N'2022-10-31', NULL, NULL, NULL),
  (2, 1, N'Autumn 2021', N'Autumn 2021', 2021, N'2021-08-11', N'2022-03-07', N'2022-04-28', N'2022-10-31', NULL, NULL, NULL),
  (3, 1, N'Summer 2022', N'Summer 2022', 2022, N'2022-03-08', N'2022-08-08', N'2022-09-29', N'2022-10-31', 2020, N'2022-08-17', N'2022-11-01'),
  (4, 1, N'Autumn 2022', N'Autumn 2022', 2022, N'2022-08-09', N'2023-03-06', N'2023-04-27', N'2023-05-31', 2020, N'2023-03-15', N'2023-06-01'),
  (5, 1, N'Summer 2023', N'Summer 2023', 2023, N'2023-03-07', N'2023-08-07', N'2023-09-28', N'2023-10-31', 2021, N'2023-08-16', N'2023-11-01'),
  (6, 1, N'Autumn 2023', N'Autumn 2023', 2023, N'2023-08-08', N'2024-03-11', N'2024-05-02', N'2024-05-31', 2021, N'2024-03-20', N'2024-06-01'),
  (7, 2, N'Summer 2022', N'Summer 2022', 2022, N'2021-10-01', N'2022-08-08', N'2022-09-29', N'2022-10-31', NULL, NULL, NULL),
  (8, 2, N'Summer 2023', N'Summer 2023', 2023, N'2022-08-09', N'2023-08-07', N'2023-09-28', N'2023-10-31', NULL, NULL, NULL),
  (9, 2, N'Summer 2024', N'Summer 2024', 2024, N'2023-08-08', N'2024-08-05', N'2024-09-26', N'2024-10-31', NULL, NULL, NULL),
  (10, 1, N'Summer 2024', N'Summer 2024', 2024, N'2024-03-12', N'2024-08-05', N'2024-09-26', N'2024-10-31', 2022, N'2024-08-14', N'2024-11-01'),
  (11, 1, N'Autumn 2024', N'Autumn 2024', 2024, N'2024-08-06', N'2025-03-10', N'2025-05-01', N'2025-05-31', 2022, N'2025-03-19', N'2025-06-01'),
  (12, 1, N'Summer 2025', N'Summer 2025', 2025, N'2025-03-11', N'2025-08-04 23:59', N'2025-09-25 23:59', N'2025-10-31 23:59', 2023, N'2025-08-12', N'2025-11-01 00:01'),
  (13, 2, N'Summer 2025', N'Summer 2025', 2025, N'2024-08-06', N'2025-08-04', N'2025-09-26', N'2025-10-31', NULL, NULL, NULL),
  (14, 1, N'Autumn 2025', N'Autumn 2025', 2025, N'2025-08-05 00:01', N'2026-03-09 23:59', N'2026-04-30 23:59', N'2026-05-31 23:59', 2023, N'2026-03-18 23:59', N'2026-06-01 00:01'),
  -- The following rows are provisional dates and will be confirmed in the future
  (15, 1, N'Summer 2026', N'Summer 2026', 2026, N'2026-03-10 00:01', N'2026-08-03 23:59', N'2026-09-24 23:59', N'2026-10-31 23:59', 2024, N'2026-08-12 23:59', N'2026-11-01 00:01'),
  (16, 1, N'Autumn 2026', N'Autumn 2026', 2026, N'2026-08-04 00:01', N'2027-03-08 23:59', N'2027-04-29 23:59', N'2027-05-31 23:59', 2024, N'2027-03-18 23:59', N'2027-06-01 00:01'),
  (17, 1, N'Summer 2027', N'Summer 2027', 2027, N'2027-03-09 00:01', N'2027-08-02 23:59', N'2027-09-23 23:59', N'2027-10-31 23:59', 2025, N'2027-08-11 23:59', N'2027-11-01 00:01')
  )
  AS Source ([Id], [ComponentType], [Name], [Description], [Year], [StartDate], [EndDate], [RommEndDate], [AppealEndDate], [ResultCalculationYear], [ResultPublishDate], [PrintAvailableDate]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[ComponentType] <> Source.[ComponentType])
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Description] <> Source.[Description] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Year] <> Source.[Year])	 
	 OR (Target.[StartDate] <> Source.[StartDate] COLLATE Latin1_General_CS_AS)
	 OR (Target.[EndDate] <> Source.[EndDate] COLLATE Latin1_General_CS_AS)
	 OR (Target.[RommEndDate] <> Source.[RommEndDate] COLLATE Latin1_General_CS_AS)
	 OR (Target.[AppealEndDate] <> Source.[AppealEndDate])
	 OR (ISNULL(Target.[ResultCalculationYear],0) <> ISNULL(Source.[ResultCalculationYear], 0))
	 OR (ISNULL(Target.[ResultPublishDate], '') <> ISNULL(Source.[ResultPublishDate], ''))
	 OR (ISNULL(Target.[PrintAvailableDate], '') <> ISNULL(Source.[PrintAvailableDate], ''))
	 )
THEN 
UPDATE SET 
	[ComponentType] = Source.[ComponentType],
	[Name] = Source.[Name],	
	[Description] = Source.[Description],
	[Year] = Source.[Year],	
	[StartDate] = Source.[StartDate],
	[EndDate] = Source.[EndDate],
	[RommEndDate] = Source.[RommEndDate],
	[AppealEndDate] = Source.[AppealEndDate],
	[ResultCalculationYear] = Source.[ResultCalculationYear],
	[ResultPublishDate] = Source.[ResultPublishDate],
	[PrintAvailableDate] = Source.[PrintAvailableDate],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [ComponentType], [Name], [Description], [Year], [StartDate], [EndDate], [RommEndDate], [AppealEndDate], [ResultCalculationYear], [ResultPublishDate], [PrintAvailableDate], [CreatedBy]) 
	VALUES ([Id], [ComponentType], [Name], [Description], [Year], [StartDate], [EndDate], [RommEndDate], [AppealEndDate], [ResultCalculationYear], [ResultPublishDate], [PrintAvailableDate], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[AssessmentSeries] OFF
