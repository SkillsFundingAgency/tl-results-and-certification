/*
Insert initial data for TlSpecialisms
*/

SET IDENTITY_INSERT [dbo].[TlSpecialism] ON

MERGE INTO [dbo].[TlSpecialism] AS Target 
USING (VALUES 
	(1, N'ZTLOS001', N'Surveying and Design for Construction and the Built Environment', 1),
	(2, N'ZTLOS002', N'Civil Engineering', 1),
	(3, N'ZTLOS003', N'Building Services Design', 1),
	(4, N'ZTLOS004', N'Hazardous Materials Analysis and Surveying', 1),
	(5, N'ZTLOS023', N'Carpentry and Joinery', 2),
	(6, N'ZTLOS025', N'Plastering', 2),
	(7, N'ZTLOS022', N'Bricklaying', 2),
	(8, N'ZTLOS024', N'Painting and Decorating', 2),
	(9, N'ZTLOS026', N'Electrotechnical Engineering', 3),
	(10, N'ZTLOS028', N'Protection Systems Engineering', 3),
	(11, N'10202101', N'Heating Engineering', 3),
	(12, N'ZTLOS029', N'Gas Engineering', 3),
	(13, N'10202102', N'Plumbing', 3),
	(14, N'10202103', N'Refrigeration Engineering', 3),
	(15, N'10202104', N'Air Conditioning Engineering', 3),
	(16, N'10202105', N'Ventilation', 3),
	(17, N'ZTLOS006', N'Early Years Educator', 4),
	(18, N'ZTLOS007', N'Assisting Teaching', 4),
	(19, N'ZTLOS008', N'Supporting and Mentoring Students in Educational Settings', 4),
	(20, N'ZTLOS005', N'Digital Production, Design and Development', 5),
	(21, N'ZTLOS027', N'Electrical and Electronic Equipment Engineering', 3),
	(22, N'ZTLOS010', N'Digital Infrastructure', 6),
	(23, N'ZTLOS011', N'Network Cabling', 6),
	(24, N'ZTLOS012', N'Digital Support', 6),
	(25, N'ZTLOS009', N'Data Technician', 7),
	(26, N'ZTLOS013', N'Supporting Healthcare - Supporting the Adult Nursing Team', 8),
	(27, N'ZTLOS014', N'Supporting Healthcare - Supporting the Midwifery Team', 8),
	(28, N'ZTLOS015', N'Supporting Healthcare - Supporting the Mental Health Team', 8),
	(29, N'ZTLOS016', N'Supporting Healthcare - Supporting the Care of Children and Young People', 8),
	(30, N'ZTLOS017', N'Supporting Healthcare - Supporting the Therapy Teams', 8),
	(31, N'ZTLOS018', N'Assisting with Healthcare Science', 9),
	(32, N'ZTLOS019', N'Technical: Laboratory Sciences', 10),
	(33, N'ZTLOS020', N'Technical: Food Sciences', 10),
	(34, N'ZTLOS021', N'Technical: Metrology Sciences', 10)
  )
  AS Source ([Id], [LarId], [Name], [TlPathwayId]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[LarId] <> Source.[LarId] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[TlPathwayId] <> Source.[TlPathwayId])) 
THEN 
UPDATE SET 
	[LarId] = Source.[LarId],
	[Name] = Source.[Name],
	[TlPathwayId] = Source.[TlPathwayId],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [LarId], [Name], [TlPathwayId], [CreatedBy]) 
	VALUES ([Id], [LarId], [Name], [TlPathwayId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlSpecialism] OFF
