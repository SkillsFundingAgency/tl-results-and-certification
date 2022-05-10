/*
Insert initial data for IpTempFlexNavigation
*/

SET IDENTITY_INSERT [dbo].[IpTempFlexNavigation] ON

MERGE INTO [dbo].[IpTempFlexNavigation] AS Target 
USING (VALUES 
	
	(1, 1, 2020, 1, 1, 1),
	(2, 1, 2021, 0, 1, 1),

	(3, 2, 2021, 0, 1, 1),
	(4, 3, 2021, 0, 1, 1),

	(5, 4, 2020, 1, 0, 1),
	(6, 4, 2021, 1, 0, 1),
	(7, 5, 2020, 1, 1, 1),
	(8, 5, 2021, 0, 1, 1),

	(9, 6, 2021, 0, 1, 1),
	(10, 7, 2021, 0, 1, 1),
	(11, 8, 2021, 1, 0, 1),
	(12, 9, 2021, 0, 1, 1),
	(13, 10, 2021, 1, 0, 1)
  )
  AS Source ([Id], [TlPathwayId], [AcademicYear], [AskTempFlexibility], [AskBlendedPlacement], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[TlPathwayId] <> Source.[TlPathwayId])
	   OR (Target.[AcademicYear] <> Source.[AcademicYear])
	   OR (Target.[AskTempFlexibility] <> Source.[AskTempFlexibility])
	   OR (Target.[AskBlendedPlacement] <> Source.[AskBlendedPlacement])
	   OR (Target.[IsActive] <> Source.[IsActive]))
THEN 
UPDATE SET 
	[TlPathwayId] = Source.[TlPathwayId],
	[AcademicYear] = Source.[AcademicYear],
	[AskTempFlexibility] = Source.[AskTempFlexibility],
	[AskBlendedPlacement] = Source.[AskBlendedPlacement],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [TlPathwayId], [AcademicYear], [AskTempFlexibility], [AskBlendedPlacement], [IsActive], [CreatedBy]) 
	VALUES ([Id], [TlPathwayId], [AcademicYear], [AskTempFlexibility], [AskBlendedPlacement], [IsActive], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[IpTempFlexNavigation] OFF
