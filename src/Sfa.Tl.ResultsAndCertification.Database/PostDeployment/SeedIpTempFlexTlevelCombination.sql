/*
Insert initial data for IpTempFlexTlevelCombination
*/

SET IDENTITY_INSERT [dbo].[IpTempFlexTlevelCombination] ON

MERGE INTO [dbo].[IpTempFlexTlevelCombination] AS Target 
USING (VALUES 

	-- Design, Surveying and Planning for Construction
	(1, 1, 19, 1),
	(2, 1, 20, 1),
	(3, 1, 21, 1),
	(4, 1, 23, 1),

	-- Onsite Construction
	(5, 2, 23, 1),
	
	-- Building Services Engineering for Construction
	(6, 3, 23, 1),
	
	-- Education and Childcare
	(7, 4, 17, 1),
	(8, 4, 18, 1),

	-- Digital production, design and development
	(9, 5, 19, 1),
	(10, 5, 20, 1),
	(11, 5, 21, 1),
	(12, 5, 23, 1),

	-- Digital Support Services
	(13, 6, 23, 1),

	-- Digital Business Services
	(14, 7, 23, 1),

	-- Health
	(15, 8, 22, 1),
	(16, 8, 23, 1),

	-- Healthcare Science
	(17, 9, 23, 1),

	-- Science
	(18, 10, 22, 1),
	(19, 10, 23, 1)	
  )
  AS Source ([Id], [TlPathwayId], [IpLookupId], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TlPathwayId] <> Source.[TlPathwayId])
	   OR (Target.[IpLookupId] <> Source.[IpLookupId])
	   OR (Target.[IsActive] <> Source.[IsActive]))
THEN 
UPDATE SET 
	[TlPathwayId] = Source.[TlPathwayId],
	[IpLookupId] = Source.[IpLookupId],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlPathwayId], [IpLookupId], [IsActive], [CreatedBy]) 
	VALUES ([Id], [TlPathwayId], [IpLookupId], [IsActive], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[IpTempFlexTlevelCombination] OFF
