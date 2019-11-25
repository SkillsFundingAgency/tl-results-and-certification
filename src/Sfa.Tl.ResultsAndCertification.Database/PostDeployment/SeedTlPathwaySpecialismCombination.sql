/*
Insert initial data for TlPathwaySpecialismCombination
*/

SET IDENTITY_INSERT [dbo].[TlPathwaySpecialismCombination] ON

MERGE INTO [dbo].[TlPathwaySpecialismCombination] AS Target 
USING (VALUES 
  (1, 1, 1, 'G1'),
  (2, 1, 2, 'G1'),
  (3, 1, 3, 'G2'),
  (4, 1, 4, 'G2'),
  (5, 2, 5, 'G1'),
  (6, 2, 6, 'G1'),
  (7, 2, 7, 'G1'),
  (8, 3, 8, 'G1')
  )
  AS Source ([Id], [PathwayId], [SpecialismId], [Group]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[PathwayId] <> Source.[PathwayId])	 
	 OR (Target.[SpecialismId] <> Source.[SpecialismId])
	 OR (Target.[Group] <> Source.[Group])) 
THEN 
UPDATE SET 
	[PathwayId] = source.[PathwayId],
	[SpecialismId] = Source.[SpecialismId],	
	[Group] = Source.[Group],	
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [PathwayId], [SpecialismId], [Group], [CreatedBy]) 
	VALUES ([Id], [PathwayId], [SpecialismId], [Group], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlPathwaySpecialismCombination] OFF
