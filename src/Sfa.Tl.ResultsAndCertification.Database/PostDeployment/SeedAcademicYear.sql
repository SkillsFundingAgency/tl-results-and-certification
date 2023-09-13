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
  (5, N'2024/25', 2024, N'2024-09-01', N'2025-08-31'),
  (6, N'2025/26', 2025, N'2025-09-01', N'2026-08-31'),
  (7, N'2026/27', 2026, N'2026-09-01', N'2027-08-31'),
  (8, N'2027/28', 2027, N'2027-09-01', N'2028-08-31'),
  (9, N'2028/29', 2028, N'2028-09-01', N'2029-08-31'),
  (10, N'2029/30', 2029, N'2029-09-01', N'2030-08-31'),
  (11, N'2030/31', 2030, N'2030-09-01', N'2031-08-31'),
  (12, N'2031/32', 2031, N'2031-09-01', N'2032-08-31'),
  (13, N'2032/33', 2032, N'2032-09-01', N'2033-08-31'),
  (14, N'2033/34', 2033, N'2033-09-01', N'2034-08-31'),
  (15, N'2034/35', 2034, N'2034-09-01', N'2035-08-31'),
  (16, N'2035/36', 2035, N'2035-09-01', N'2036-08-31'),
  (17, N'2036/37', 2036, N'2036-09-01', N'2037-08-31'),
  (18, N'2037/38', 2037, N'2037-09-01', N'2038-08-31'),
  (19, N'2038/39', 2038, N'2038-09-01', N'2039-08-31'),
  (20, N'2039/40', 2039, N'2039-09-01', N'2040-08-31'),
  (21, N'2040/41', 2040, N'2040-09-01', N'2041-08-31'),
  (22, N'2041/42', 2041, N'2041-09-01', N'2042-08-31'),
  (23, N'2042/43', 2042, N'2042-09-01', N'2043-08-31'),
  (24, N'2043/44', 2043, N'2043-09-01', N'2044-08-31'),
  (25, N'2044/45', 2044, N'2044-09-01', N'2045-08-31')
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
