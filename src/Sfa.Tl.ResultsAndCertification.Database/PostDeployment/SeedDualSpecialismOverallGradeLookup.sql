/*
Insert initial data for DualSpecialismOverallGradeLookup
*/

SET IDENTITY_INSERT [dbo].[DualSpecialismOverallGradeLookup] ON

MERGE INTO [dbo].[DualSpecialismOverallGradeLookup] AS Target 
USING (VALUES 
  	 (1, 10, 10, 10, 1),
	 (2, 10, 11, 10, 1),
	 (3, 10, 12, 11, 1),
	 (4, 11, 10, 10, 1),
	 (5, 11, 11, 11, 1),
	 (6, 11, 12, 12, 1),
	 (7, 12, 10, 11, 1),
	 (8, 12, 11, 12, 1),
	 (9, 12, 12, 12, 1)
  )
  AS Source ([Id], [FirstTlLookupSpecialismGradeId], [SecondTlLookupSpecialismGradeId], [TlLookupOverallSpecialismGradeId], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[FirstTlLookupSpecialismGradeId] <> Source.[FirstTlLookupSpecialismGradeId])
	   OR (Target.[SecondTlLookupSpecialismGradeId] <> Source.[SecondTlLookupSpecialismGradeId])
	   OR (Target.[TlLookupOverallSpecialismGradeId] <> Source.[TlLookupOverallSpecialismGradeId])
	   OR (Target.[IsActive] <> Source.[IsActive]))
THEN 
UPDATE SET 
	[FirstTlLookupSpecialismGradeId] = Source.[FirstTlLookupSpecialismGradeId],
	[SecondTlLookupSpecialismGradeId] = Source.[SecondTlLookupSpecialismGradeId],
	[TlLookupOverallSpecialismGradeId] = Source.[TlLookupOverallSpecialismGradeId],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [FirstTlLookupSpecialismGradeId], [SecondTlLookupSpecialismGradeId], [TlLookupOverallSpecialismGradeId], [IsActive], [CreatedBy])
	VALUES ([Id], [FirstTlLookupSpecialismGradeId], [SecondTlLookupSpecialismGradeId], [TlLookupOverallSpecialismGradeId], [IsActive], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[DualSpecialismOverallGradeLookup] OFF
