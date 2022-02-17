/*
Insert initial data for TlPathways
*/

SET IDENTITY_INSERT [dbo].[TlPathway] ON

MERGE INTO [dbo].[TlPathway] AS Target 
USING (VALUES 
  (1, N'60358300', N'T Level in Design, Surveying and Planning for Construction', N'Design, Surveying and Planning', 2020, 1),
  (2, N'60369176', N'T Level in Onsite Construction', N'Onsite Construction', 2021, 1),
  (3, N'60369115', N'T Level in Building Services Engineering for Construction', N'Building Services Engineering', 2021, 1),
  (4, N'60358294', N'T Level in Education and Childcare', N'Education and Childcare', 2020, 2),
  (5, N'60358324', N'T Level in Digital Production, Design and Development', N'Digital Production, Design and Development', 2020, 3),
  (6, N'60369012', N'T Level in Digital Support Services', N'Digital Support Services', 2021, 3),
  (7, N'60369024', N'T Level in Digital Business Services', N'Digital Business Services', 2021, 3),
  (8, N'6037066X', N'T Level in Health', N'Health', 2021, 4),
  (9, N'6037083X', N'T Level in Healthcare Science', N'Healthcare Science', 2021, 4),
  (10, N'60369899', N'T Level in Science', N'Science', 2021, 4)
  )
  AS Source ([Id], [LarId], [TlevelTitle], [Name], [StartYear], [TlRouteId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[LarId] <> Source.[LarId] COLLATE Latin1_General_CS_AS)
	 OR (Target.[TlevelTitle] <> Source.[TlevelTitle] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[StartYear] <> Source.[StartYear])
	 OR (Target.[TlRouteId] <> Source.[TlRouteId])) 
THEN 
UPDATE SET 
	[LarId] = Source.[LarId],
	[TlevelTitle] = Source.[TlevelTitle],
	[Name] = Source.[Name],
	[StartYear] = Source.[StartYear],
	[TlRouteId] = Source.[TlRouteId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [LarId], [TlevelTitle], [Name], [StartYear], [TlRouteId], [CreatedBy]) 
	VALUES ([Id], [LarId], [TlevelTitle], [Name], [StartYear], [TlRouteId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlPathway] OFF