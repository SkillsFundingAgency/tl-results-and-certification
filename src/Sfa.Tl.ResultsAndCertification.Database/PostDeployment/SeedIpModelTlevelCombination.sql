/*
Insert initial data for IpModelTlevelCombination
*/

SET IDENTITY_INSERT [dbo].[IpModelTlevelCombination] ON

MERGE INTO [dbo].[IpModelTlevelCombination] AS Target 
USING (VALUES 

	-- Design, Surveying and Planning for Construction
	(1, 1, 10, 1),
	(2, 1, 11, 1),
	(3, 1, 12, 1),
	(4, 1, 14, 1),
	(5, 1, 15, 1),
	(6, 1, 16, 1),

	-- Onsite Construction
	(7, 2, 10, 1),
	(8, 2, 11, 1),
	(9, 2, 12, 1),
	(10, 2, 14, 1),
	(11, 2, 15, 1),
	(12, 2, 16, 1),

	-- Building Services Engineering for Construction
	(13, 3, 10, 1),
	(14, 3, 11, 1),
	(15, 3, 12, 1),
	(16, 3, 14, 1),
	(17, 3, 15, 1),
	(18, 3, 16, 1),

	-- Education and Childcare
	(19, 4, 10, 1),
	(20, 4, 11, 1),
	(21, 4, 12, 1),

	-- Digital production, design and development
	(22, 5, 10, 1),
	(23, 5, 11, 1),
	(24, 5, 12, 1),
	(25, 5, 13, 1),

	-- Digital Support Services
	(26, 6, 10, 1),
	(27, 6, 11, 1),
	(28, 6, 12, 1),
	(29, 6, 13, 1),

	-- Digital Business Services
	(30, 7, 10, 1),
	(31, 7, 11, 1),
	(32, 7, 12, 1),
	(33, 7, 13, 1),

	-- Health
	(34, 8, 10, 1),
	(35, 8, 11, 1),
	(36, 8, 12, 1),

	-- Healthcare Science
	(37, 9, 10, 1),
	(38, 9, 11, 1),
	(39, 9, 12, 1),

	-- Science
	(40, 10, 10, 1),
	(41, 10, 11, 1),
	(42, 10, 12, 1)	
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

SET IDENTITY_INSERT [dbo].[IpModelTlevelCombination] OFF
