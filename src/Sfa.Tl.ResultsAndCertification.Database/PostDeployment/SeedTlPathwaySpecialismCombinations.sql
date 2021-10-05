/*
Insert initial data for TlPathwaySpecialismCombination
*/

SET IDENTITY_INSERT [dbo].[TlPathwaySpecialismCombination] ON

MERGE INTO [dbo].[TlPathwaySpecialismCombination] AS Target 
USING (VALUES 
	(1, 3, 11, 1, 1),
	(2, 3, 16, 1, 1),
	(3, 3, 13, 2, 1),
	(4, 3, 11, 2, 1),
	(5, 3, 14, 3, 1),
	(6, 3, 15, 3, 1)
  )
  AS Source ([Id], [TlPathwayId], [TlSpecialismId], [GroupId], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TlPathwayId] <> Source.[TlPathwayId])
	   OR (Target.[TlSpecialismId] <> Source.[TlSpecialismId])
	   OR (Target.[GroupId] <> Source.[GroupId])
	   OR (Target.[IsActive] <> Source.[IsActive]))
THEN 
UPDATE SET 
	[TlPathwayId] = Source.[TlPathwayId],
	[TlSpecialismId] = Source.[TlSpecialismId],
	[GroupId] = Source.[GroupId],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlPathwayId], [TlSpecialismId], [GroupId], [IsActive], [CreatedBy]) 
	VALUES ([Id], [TlPathwayId], [TlSpecialismId], [GroupId], [IsActive], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlPathwaySpecialismCombination] OFF
