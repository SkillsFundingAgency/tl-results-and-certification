/*
Insert initial data for TlLookup
*/

SET IDENTITY_INSERT [dbo].[TlLookup] ON

MERGE INTO [dbo].[TlLookup] AS Target 
USING (VALUES 
  (1, N'PathwayComponentGrade', N'PCG1', N'A*', 1, 1),
  (2, N'PathwayComponentGrade', N'PCG2', N'A', 2, 1),
  (3, N'PathwayComponentGrade', N'PCG3', N'B', 3, 1),
  (4, N'PathwayComponentGrade', N'PCG4', N'C', 4, 1),
  (5, N'PathwayComponentGrade', N'PCG5', N'D', 5, 1),
  (6, N'PathwayComponentGrade', N'PCG6', N'E', 6, 1),
  (7, N'PathwayComponentGrade', N'PCG7', N'Unclassified', 7, 1),
  (8, N'QualificationSubject', N'Eng', N'English', 1, 1),
  (9, N'QualificationSubject', N'Math', N'Maths', 2, 1),
  (10, N'SpecialismComponentGrade', N'SCG1', N'Distinction', 1, 1),
  (11, N'SpecialismComponentGrade', N'SCG2', N'Merit', 2, 1),
  (12, N'SpecialismComponentGrade', N'SCG3', N'Pass', 3, 1),
  (13, N'SpecialismComponentGrade', N'SCG4', N'Unclassified', 4, 1),
  (14, N'SpecialConsideration', N'SC', N'SpecialConsideration', 1, 1),
  (15, N'IndustryPlacementModel', N'IPM', N'IndustryPlacementModel', 2, 1),
  (16, N'TemporaryFlexibility', N'TF', N'TemporaryFlexibility', 3, 1),
  (17, N'OverallResult', N'OR1', N'Distinction *', 1, 1),
  (18, N'OverallResult', N'OR2', N'Distinction', 2, 1),
  (19, N'OverallResult', N'OR3', N'Merit', 3, 1),
  (20, N'OverallResult', N'OR4', N'Pass', 4, 1),
  (21, N'OverallResult', N'OR5', N'Partial achievement', 5, 1),
  (22, N'OverallResult', N'OR6', N'Unclassified', 6, 1),
  (23, N'OverallResult', N'OR7', N'X - no result', 7, 1)
  )
  AS Source ([Id], [Category], [Code], [Value], [SortOrder], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Category] <> Source.[Category] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Code] <> Source.[Code] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Value] <> Source.[Value] COLLATE Latin1_General_CS_AS)
	 OR (ISNULL(Target.[SortOrder], 0) <> ISNULL(Source.[SortOrder],0))
	 OR (Target.[IsActive] <> Source.[IsActive]))
THEN 
UPDATE SET 
	[Category] = Source.[Category],
	[Code] = Source.[Code],
	[Value] = Source.[Value],
	[SortOrder] = Source.[SortOrder],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Category], [Code], [Value], [SortOrder], [IsActive], [CreatedBy]) 
	VALUES ([Id], [Category], [Code], [Value], [SortOrder], [IsActive], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlLookup] OFF
