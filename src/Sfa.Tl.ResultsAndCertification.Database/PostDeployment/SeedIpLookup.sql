/*
Insert initial data for IpLookup
*/

SET IDENTITY_INSERT [dbo].[IpLookup] ON

MERGE INTO [dbo].[IpLookup] AS Target 
USING (VALUES 
  (1, 14, N'Learner''s medical reasons', N'2020-01-01', NULL, NULL, 1),
  (2, 14, N'Learner''s family medical reasons', N'2020-01-01', NULL, NULL, 2),
  (3, 14, N'Bereavement', N'2020-01-01', NULL, NULL, 3),
  (4, 14, N'Domestic crisis', N'2020-01-01', NULL, NULL, 4),
  (5, 14, N'Trauma or significant change of circumstances', N'2020-01-01', NULL, NULL, 5),
  (6, 14, N'Alternative priorities: Sport, training or other competitions', N'2020-01-01', NULL, NULL, 6),
  (7, 14, N'Unsafe placement', N'2020-01-01', NULL, NULL, 7),
  (8, 14, N'Placement withdrawn', N'2020-01-01', NULL, NULL, 8),
  (9, 14, N'COVID-19', N'2020-01-01', N'2020-12-31', NULL, 9),

  (10, 15, N'Relevant part-time work', N'2020-09-01', NULL, NULL, 1),
  (11, 15, N'On-site facilities for SEND students', N'2020-09-01', NULL, NULL, 2),
  (12, 15, N'On-site facilities for young people in young offender institutions', N'2020-09-01', NULL, NULL, 3),
  (13, 15, N'Route-level placements', N'2020-09-01', NULL, NULL, 4),
  (14, 15, N'Commercial, charitable or community projects', N'2020-09-01', NULL, NULL, 5),
  (15, 15, N'One lead employer facilitating the required hours', N'2020-09-01',NULL, 0, 6),
  (16, 15, N'Use of skills hubs or employer training centres', N'2020-09-01', NULL, 0, 7),

  (17, 16, N'JABQG risk-rated approach', N'2020-01-01', NULL, 1, 1),
  (18, 16, N'Reduction in hours', N'2020-01-01', NULL, 1, 2),
  (19, 16, N'Employer led activities/projects', N'2020-01-01', N'2020-12-31', NULL, 3),
  (20, 16, N'Up to 100% remote', N'2020-01-01', N'2020-12-31', 0, 4),
  (21, 16, N'No other flexibility used', N'2020-01-01', N'2020-12-31', 1, 5),  
  (22, 16, N'Pathway level placements', N'2021-01-01', NULL, 1, 6),
  (23, 16, N'Blended placements', N'2021-01-01', NULL, 1, 7)
  )
  AS Source ([Id], [TlLookupId], [Name], [StartDate], [EndDate], [ShowOption], [SortOrder]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TlLookupId] <> Source.[TlLookupId])
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[StartDate] <> Source.[StartDate] COLLATE Latin1_General_CS_AS)
	 OR (Target.[EndDate] <> Source.[EndDate])
	 OR (ISNULL(Target.[ShowOption],0) <> ISNULL(Source.[ShowOption], 0))
	 OR (ISNULL(Target.[SortOrder], 0) <> ISNULL(Source.[SortOrder],0)))
THEN 
UPDATE SET 
	[TlLookupId] = Source.[TlLookupId],
	[Name] = Source.[Name],
	[StartDate] = Source.[StartDate],
	[EndDate] = Source.[EndDate],
	[ShowOption] = Source.[ShowOption],
	[SortOrder] = Source.[SortOrder],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlLookupId], [Name], [StartDate], [EndDate], [ShowOption], [SortOrder], [CreatedBy]) 
	VALUES ([Id], [TlLookupId], [Name], [StartDate], [EndDate], [ShowOption], [SortOrder], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[IpLookup] OFF
