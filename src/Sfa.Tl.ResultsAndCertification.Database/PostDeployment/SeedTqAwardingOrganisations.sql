/*
Insert initial data for Tq Awarding Organisations
*/

SET IDENTITY_INSERT [dbo].[TqAwardingOrganisation] ON

MERGE INTO [dbo].[TqAwardingOrganisation] AS Target 
USING (VALUES 
	(1, 1, 2, 4, 1),
	(2, 1, 3, 6, 1),
	(3, 1, 3, 7, 1),
	(4, 1, 4, 8, 1),
	(5, 1, 4, 9, 1),
	(6, 1, 4, 10, 1),
	(7, 2, 1, 1, 1),
	(8, 2, 3, 5, 1),
	(9, 3, 1, 2, 1),
	(10, 3, 1, 3, 1)
  )
  AS Source ([Id], [TlAwardingOrganisatonId], [TlRouteId], [TlPathwayId], [ReviewStatus]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TlAwardingOrganisatonId] <> Source.[TlAwardingOrganisatonId])
	   OR (Target.[TlRouteId] <> Source.[TlRouteId])
	   OR (Target.[TlPathwayId] <> Source.[TlPathwayId]))
THEN 
UPDATE SET 
	[TlAwardingOrganisatonId] = Source.[TlAwardingOrganisatonId],
	[TlRouteId] = Source.[TlRouteId],
	[TlPathwayId] = Source.[TlPathwayId],
	[ReviewStatus] = Source.[ReviewStatus],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlAwardingOrganisatonId], [TlRouteId], [TlPathwayId], [ReviewStatus], [CreatedBy]) 
	VALUES ([Id], [TlAwardingOrganisatonId], [TlRouteId], [TlPathwayId], [ReviewStatus], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TqAwardingOrganisation] OFF
