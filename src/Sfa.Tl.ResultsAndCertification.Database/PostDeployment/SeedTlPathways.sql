/*
Insert initial data for TlPathways
*/

SET IDENTITY_INSERT [dbo].[TlPathway] ON

MERGE INTO [dbo].[TlPathway] AS Target 
USING (VALUES 
  (1, N'10123456', N'T Level in Design, Surveying and Planning for Construction', N'Design, Surveying and Planning', 1),
  (2, N'10123456', N'Construction: Onsite Construction', N'Onsite Construction', 1),
  (3, N'10123456', N'Construction: Building Services Engineering', N'Building Services Engineering', 1),
  (4, N'10223456', N'T Level in Education and Childcare', N'Education and Childcare', 2),
  (5, N'10323456', N'T Level in Digital Production, Design and Development', N'Digital Production, Design and Development', 3),
  (6, N'10323456', N'Digital: Digital Support Services', N'Digital Support Services', 3),
  (7, N'10323456', N'Digital: Digital Business Services', N'Digital Business Services', 3),
  (8, N'10223456', N'Health and Science: Health', N'Health', 4),
  (9, N'10223456', N'Health and Science: Healthcare Science', N'Healthcare Science', 4),
  (10, N'10223456', N'Health and Science: Science', N'Science', 4)
  )
  AS Source ([Id], [LarId], [TlevelTitle], [Name], [TlRouteId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[LarId] <> Source.[LarId] COLLATE Latin1_General_CS_AS)
	 OR (Target.[TlevelTitle] <> Source.[TlevelTitle] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[TlRouteId] <> Source.[TlRouteId])) 
THEN 
UPDATE SET 
	[LarId] = Source.[LarId],
	[TlevelTitle] = Source.[TlevelTitle],
	[Name] = Source.[Name],
	[TlRouteId] = Source.[TlRouteId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [LarId], [TlevelTitle], [Name], [TlRouteId], [CreatedBy]) 
	VALUES ([Id], [LarId], [TlevelTitle], [Name], [TlRouteId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlPathway] OFF
