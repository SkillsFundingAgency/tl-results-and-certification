/*
Insert initial data for AcademicYear
*/

SET IDENTITY_INSERT [dbo].[AcademicYear] ON

MERGE INTO [dbo].[AcademicYear] AS Target 
USING (VALUES 
  (1, N'2020/21', 2020, N'2020-09-01', N'2021-08-31'),
  (2, N'2021/22', 2021, N'2021-09-01', N'2022-08-31'),
  (3, N'2022/23', 2022, N'2022-09-01', N'2023-08-31'),
  (4, N'2023/24', 2023, N'2023-09-01', N'2024-08-31'),
  (5, N'2024/25', 2024, N'2024-09-01', N'2025-08-31')
  )
  AS Source ([Id], [Name], [Year], [StartDate], [EndDate]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Year] <> Source.[Year])
	 OR (Target.[StartDate] <> Source.[StartDate] COLLATE Latin1_General_CS_AS)
	 OR (Target.[EndDate] <> Source.[EndDate] COLLATE Latin1_General_CS_AS)
	 )
THEN 
UPDATE SET 
	[Name] = Source.[Name],
	[Year] = Source.[Year],
	[StartDate] = Source.[StartDate],
	[EndDate] = Source.[EndDate],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name], [Year], [StartDate], [EndDate], [CreatedBy]) 
	VALUES ([Id], [Name], [Year], [StartDate], [EndDate], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[AcademicYear] OFF
