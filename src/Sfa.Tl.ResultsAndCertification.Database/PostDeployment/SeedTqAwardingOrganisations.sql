/*
Insert initial data for Tq Awarding Organisations
*/

SET IDENTITY_INSERT [dbo].[TqAwardingOrganisation] ON

MERGE INTO [dbo].[TqAwardingOrganisation] AS Target 
USING (VALUES 
  (1, 1, 2, 2, 1),
  (2, 2, 1, 1, 1),
  (3, 2, 3, 3, 1)
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
