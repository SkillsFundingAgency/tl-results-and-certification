/*
Insert initial data for TlLookup
*/

SET IDENTITY_INSERT [dbo].[TlLookup] ON

MERGE INTO [dbo].[TlLookup] AS Target 
USING (VALUES 
  (1, N'PathwayComponentGrade', N'PCG1', N'A*', 1, 1),
  (2, N'PathwayComponentGrade', N'PCG2', N'A', 2, 1),
  (3, N'PathwayComponentGrade', N'PCG3', N'B', 3, 1),
  (4, N'PathwayComponentGrade', N'PCG4', N'C', 4, 1),
  (5, N'PathwayComponentGrade', N'PCG5', N'D', 5, 1),
  (6, N'PathwayComponentGrade', N'PCG6', N'E', 6, 1),
  (7, N'PathwayComponentGrade', N'PCG7', N'Unclassified', 7, 1)
  )
  AS Source ([Id], [Category], [Code], [Value], [SortOrder], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[Category] <> Source.[Category] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Code] <> Source.[Code] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Value] <> Source.[Value] COLLATE Latin1_General_CS_AS)
	 OR (Target.[SortOrder] <> Source.[SortOrder])
	 OR (Target.[IsActive] <> Source.[IsActive]))
THEN 
UPDATE SET 
	[Category] = Source.[Category],
	[Code] = Source.[Code],
	[Value] = Source.[Value],
	[SortOrder] = Source.[SortOrder],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Category], [Code], [Value], [SortOrder], [IsActive], [CreatedBy]) 
	VALUES ([Id], [Category], [Code], [Value], [SortOrder], [IsActive], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlLookup] OFF
