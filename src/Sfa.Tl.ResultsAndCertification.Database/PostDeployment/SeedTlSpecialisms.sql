/*
Insert initial data for TlSpecialisms
*/

SET IDENTITY_INSERT [dbo].[TlSpecialism] ON

MERGE INTO [dbo].[TlSpecialism] AS Target 
USING (VALUES 
  (1, N'11234567', N'Surveying and design for construction and the built environment', 1),
  (2, N'12234567', N'Civil Engineering', 1),
  (3, N'13234567', N'Building services design', 1),
  (4, N'14234567', N'Hazardous materials analysis and surveying', 1),
  (5, N'15234567', N'Early years education and childcare', 2),
  (6, N'16234567', N'Assisting teaching', 2),
  (7, N'17234567', N'Supporting and mentoring students in further and higher education', 2),
  (8, N'18234567', N'Digital Production, Design and Development', 3)
  )
  AS Source ([Id], [LarId], [Name], [PathwayId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[LarId] <> Source.[LarId] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[PathwayId] <> Source.[PathwayId])) 
THEN 
UPDATE SET 
	[LarId] = Source.[LarId],
	[Name] = Source.[Name],
	[PathwayId] = Source.[PathwayId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [LarId], [Name], [PathwayId], [CreatedBy]) 
	VALUES ([Id], [LarId], [Name], [PathwayId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlSpecialism] OFF
