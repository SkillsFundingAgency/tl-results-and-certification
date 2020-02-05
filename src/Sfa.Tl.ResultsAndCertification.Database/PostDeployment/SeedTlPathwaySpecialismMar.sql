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
  AS Source ([Id], [TlMandatoryAdditionalRequirementId], [TlPathwayId], [TlSpecialismId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TlMandatoryAdditionalRequirementId] <> Source.[TlMandatoryAdditionalRequirementId])	 
	 OR (Target.[TlPathwayId] <> Source.[TlPathwayId])
	 OR (Target.[TlSpecialismId] <> Source.[TlSpecialismId])) 
THEN 
UPDATE SET 
	[TlMandatoryAdditionalRequirementId] = Source.[TlMandatoryAdditionalRequirementId],
	[TlPathwayId] = source.[TlPathwayId],
	[TlSpecialismId] = Source.[TlSpecialismId],	
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlMandatoryAdditionalRequirementId], [TlPathwayId], [TlSpecialismId], [CreatedBy]) 
	VALUES ([Id], [TlMandatoryAdditionalRequirementId], [TlPathwayId], [TlSpecialismId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlPathwaySpecialismMar] OFF
