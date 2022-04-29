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
	(7, 1, 17, 1),

	-- Onsite Construction
	(8, 2, 10, 1),
	(9, 2, 11, 1),
	(10, 2, 12, 1),
	(11, 2, 14, 1),
	(12, 2, 15, 1),
	(13, 2, 16, 1),
	(14, 2, 17, 1),

	-- Building Services Engineering for Construction
	(15, 3, 10, 1),
	(16, 3, 11, 1),
	(17, 3, 12, 1),
	(18, 3, 14, 1),
	(19, 3, 15, 1),
	(20, 3, 16, 1),
	(21, 3, 17, 1),

	-- Education and Childcare
	(22, 4, 10, 1),
	(23, 4, 11, 1),
	(24, 4, 12, 1),
	(25, 4, 17, 1),

	-- Digital production, design and development
	(26, 5, 10, 1),
	(27, 5, 11, 1),
	(28, 5, 12, 1),
	(29, 5, 13, 1),
	(30, 5, 17, 1),

	-- Digital Support Services
	(31, 6, 10, 1),
	(32, 6, 11, 1),
	(33, 6, 12, 1),
	(34, 6, 13, 1),
	(35, 6, 17, 1),

	-- Digital Business Services
	(36, 7, 10, 1),
	(37, 7, 11, 1),
	(38, 7, 12, 1),
	(39, 7, 13, 1),
	(40, 7, 17, 1),

	-- Health
	(41, 8, 10, 1),
	(42, 8, 11, 1),
	(43, 8, 12, 1),
	(44, 8, 17, 1),

	-- Healthcare Science
	(45, 9, 10, 1),
	(46, 9, 11, 1),
	(47, 9, 12, 1),
	(48, 9, 17, 1),

	-- Science
	(49, 10, 10, 1),
	(50, 10, 11, 1),
	(51, 10, 12, 1),
	(52, 10, 17, 1)	
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
