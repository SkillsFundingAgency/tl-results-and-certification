/*
Insert initial data for TlPathwaySpecialismMar
*/

SET IDENTITY_INSERT [dbo].[TlPathwaySpecialismMar] ON

MERGE INTO [dbo].[TlPathwaySpecialismMar] AS Target 
USING (VALUES 
  (1, 1, 1, NULL),
  (2, 1, 2, NULL),
  (3, 2, NULL, 1),
  (4, 2, NULL, 2),
  (5, 3, NULL, 3),
  (6, 3, NULL, 4),
  (7, 4, 3, NULL),
  (8, 5, 3, NULL)
  )
  AS Source ([Id], [MarId], [PathwayId], [SpecialismId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[MarId] <> Source.[MarId])	 
	 OR (Target.[PathwayId] <> Source.[PathwayId])
	 OR (Target.[SpecialismId] <> Source.[SpecialismId])) 
THEN 
UPDATE SET 
	[MarId] = Source.[MarId],
	[PathwayId] = source.[PathwayId],
	[SpecialismId] = Source.[SpecialismId],	
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [MarId], [PathwayId], [SpecialismId], [CreatedBy]) 
	VALUES ([Id], [MarId], [PathwayId], [SpecialismId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlPathwaySpecialismMar] OFF
