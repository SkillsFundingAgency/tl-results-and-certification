/*
Insert initial data for Tq Awarding Organisations
*/

SET IDENTITY_INSERT [dbo].[TqAwardingOrganisation] ON

MERGE INTO [dbo].[TqAwardingOrganisation] AS Target 
USING (VALUES 
	(1, 1, 4, 1),
	(2, 1, 6, 1),
	(3, 1, 7, 1),
	(4, 1, 8, 1),
	(5, 1, 9, 1),
	(6, 1, 10, 1),
	(7, 2, 1, 1),
	(8, 2, 5, 1),
	(9, 3, 2, 1),
	(10, 3, 3, 1),
	(11, 2, 11, 1),
	(12, 2, 12, 1),
	(13, 3, 13, 1),
	(14, 3, 14, 1),
	(15, 3, 15, 1),
	(16, 3, 16, 1),
	(17, 2, 17, 1),
	(18, 3, 18, 1),
	(19, 3, 19, 1),
	(20, 2, 20, 1),
	(21, 2, 21, 1),
	(22, 4, 22, 2),
	(23, 2, 23, 2),
	(24, 2, 24, 2),
	(25, 2, 25, 2),
	(26, 2, 26, 2),
	(27, 1, 27, 2)
  )
AS Source ([Id], [TlAwardingOrganisatonId], [TlPathwayId], [ReviewStatus]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TlAwardingOrganisatonId] <> Source.[TlAwardingOrganisatonId])
	   OR (Target.[TlPathwayId] <> Source.[TlPathwayId]))
THEN 
UPDATE SET 
	[TlAwardingOrganisatonId] = Source.[TlAwardingOrganisatonId],
	[TlPathwayId] = Source.[TlPathwayId],
	[ReviewStatus] = Source.[ReviewStatus],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlAwardingOrganisatonId], [TlPathwayId], [ReviewStatus], [CreatedBy]) 
	VALUES ([Id], [TlAwardingOrganisatonId], [TlPathwayId], [ReviewStatus], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TqAwardingOrganisation] OFF
