/*
Insert initial data for TlSpecialisms
*/

SET IDENTITY_INSERT [dbo].[TlSpecialism] ON

MERGE INTO [dbo].[TlSpecialism] AS Target 
USING (VALUES 
	(1, N'11234567', N'Surveying and Design for Construction and the Built Environment', 1),
	(2, N'12234567', N'Civil Engineering', 1),
	(3, N'13234567', N'Building Services Design', 1),
	(4, N'14234567', N'Hazardous Materials Analysis and Surveying', 1),
	(5, N'15234567', N'Carpentry and Joinery', 2),
	(6, N'16234567', N'Plastering', 2),
	(7, N'17234567', N'Bricklaying', 2),
	(8, N'18234567', N'Painting and Decorating', 2),
	(9, N'19234567', N'Electrotechnical Engineering', 3),
	(10, N'20234567', N'Protection Systems Engineering', 3),
	(11, N'21234567', N'Ventilation', 3),
	(12, N'22234567', N'Gas Engineering', 3),
	(13, N'23234567', N'Plumbing', 3),
	(14, N'24234567', N'Heating Engineering', 3),
	(15, N'25234567', N'Air Conditioning Engineering', 3),
	(16, N'26234567', N'Refrigeration Engineering', 3),
	(17, N'27234567', N'Early Years Educator', 4),
	(18, N'28234567', N'Assisting Teaching', 4),
	(19, N'29234567', N'Electrical and Electronic Equipment Engineering', 3),
	(20, N'30234567', N'Digital Production, Design and Development', 5),
	(21, N'31234567', N'Digital Infrastructure', 6),
	(22, N'32234567', N'Network Cabling', 6),
	(23, N'33234567', N'Unified Communications', 6),
	(24, N'34234567', N'Digital Support', 6),
	(25, N'35234567', N'Data Technician', 7),
	(26, N'36234567', N'Dental Nursing', 8),
	(27, N'37234567', N'Supporting Healthcare', 8),
	(28, N'38234567', N'Optical Care Services', 9),
	(29, N'39234567', N'Pharmacy Services', 9),
	(30, N'40234567', N'Assisting with Healthcare Science', 9),
	(31, N'41234567', N'Dental Technical Services', 9),
	(32, N'42234567', N'Prosthetic and Orthotic Technical Services', 9),
	(33, N'43234567', N'Technical: Laboratory Sciences', 10),
	(34, N'44234567', N'Technical: Food Sciences', 10),
	(35, N'45234567', N'Technical: Animal Sciences', 10),
	(36, N'46234567', N'Technical: Metrology Sciences', 10)
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
