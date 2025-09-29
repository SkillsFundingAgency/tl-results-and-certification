/*
Insert initial data for TlSpecialisms
*/

SET IDENTITY_INSERT [dbo].[TlSpecialism] ON

MERGE INTO [dbo].[TlSpecialism] AS Target 
USING (VALUES 
	(1, N'ZTLOS001', N'Surveying and Design for Construction and the Built Environment', 1, 1),
	(2, N'ZTLOS002', N'Civil Engineering', 1, 1),
	(3, N'ZTLOS003', N'Building Services Design', 1, 1),
	(4, N'ZTLOS004', N'Hazardous Materials Analysis and Surveying', 1, 1),
	(5, N'ZTLOS023', N'Carpentry and Joinery', 2, 1),
	(6, N'ZTLOS025', N'Plastering', 2, 1),
	(7, N'ZTLOS022', N'Bricklaying', 2, 1),
	(8, N'ZTLOS024', N'Painting and Decorating', 2, 1),
	(9, N'ZTLOS026', N'Electrotechnical Engineering', 3, 1),
	(10, N'ZTLOS028', N'Protection Systems Engineering', 3, 1),
	(11, N'10202101', N'Heating Engineering', 3, 1),
	(12, N'ZTLOS029', N'Gas Engineering', 3, 1),
	(13, N'10202102', N'Plumbing Engineering', 3, 1),
	(14, N'10202103', N'Refrigeration Engineering', 3, 1),
	(15, N'10202104', N'Air Conditioning Engineering', 3, 1),
	(16, N'10202105', N'Ventilation', 3, 1),
	(17, N'ZTLOS006', N'Early Years Educator', 4, 1),
	(18, N'ZTLOS007', N'Assisting Teaching', 4, 1),
	(20, N'ZTLOS005', N'Digital Production, Design and Development', 5, 1),
	(21, N'ZTLOS027', N'Electrical and Electronic Equipment Engineering', 3, 1),
	(22, N'ZTLOS010', N'Digital Infrastructure', 6, 1),
	(23, N'ZTLOS011', N'Network Cabling', 6, 1),
	(24, N'ZTLOS012', N'Digital Support', 6, 1),
	(25, N'ZTLOS009', N'Data Technician', 7, 1),
	(26, N'ZTLOS013', N'Supporting Healthcare - Supporting the Adult Nursing Team', 8, 1),
	(27, N'ZTLOS014', N'Supporting Healthcare - Supporting the Midwifery Team', 8, 1),
	(28, N'ZTLOS015', N'Supporting Healthcare - Supporting the Mental Health Team', 8, 1),
	(29, N'ZTLOS016', N'Supporting Healthcare - Supporting the Care of Children and Young People', 8, 1),
	(30, N'ZTLOS017', N'Supporting Healthcare - Supporting the Therapy Teams', 8, 1),
	(31, N'ZTLOS018', N'Assisting with Healthcare Science', 9, 1),
	(32, N'ZTLOS019', N'Technical: Laboratory Sciences', 10, 1),
	(33, N'ZTLOS020', N'Technical: Food Sciences', 10, 1),
	(34, N'ZTLOS021', N'Technical: Metrology Sciences', 10, 1),
	(35, N'ZTLOS055', N'Dental Nursing', 8, 1),
	(36, N'ZTLOS054', N'Optical Care Services', 9, 1),
	(37, N'ZTLOS034', N'Investment Banking and Asset and Wealth Management Analyst', 11, 1),
	(38, N'ZTLOS033', N'Retail and Commercial Banking Analyst', 11, 1),
	(39, N'ZTLOS035', N'Insurance Practitioner', 11, 1),
	(40, N'ZTLOS036', N'Financial Compliance / Risk Analyst', 11, 1),
	(41, N'ZTLOS037', N'Assistant Accountant', 12, 1),
	(42, N'ZTLOS041', N'Design and Development: Mechanical Engineering', 13, 1),
	(43, N'ZTLOS042', N'Design and Development: Electrical and Electronic Engineering', 13, 1),
	(44, N'ZTLOS043', N'Design and Development: Control and Instrumentation Engineering', 13, 1),
	(45, N'ZTLOS044', N'Design and Development: Structural Engineering', 13, 1),
	(46, N'ZTLOS049', N'Light and Electric Vehicles', 14, 1),
	(47, N'ZTLOS045', N'Maintenance Engineering Technologies: Mechanical', 14, 1),
	(48, N'ZTLOS047', N'Maintenance Engineering Technologies: Electrical & Electronics', 14, 1),
	(49, N'ZTLOS048', N'Maintenance Engineering Technologies: Control & Instrumentation', 14, 1),
	(50, N'ZTLOS046', N'Maintenance Engineering Technologies: Mechatronics', 14, 1),
	(51, N'ZTLOS050', N'Fitting and Assembly Technologies', 15, 1),
	(52, N'ZTLOS051', N'Machining & Toolmaking Technologies', 15, 1),
	(53, N'ZTLOS052', N'Composites Manufacturing Technologies', 15, 1),
	(54, N'ZTLOS053', N'Fabrication & Welding Technologies', 15, 1),
	(55, N'ZTLOS038', N'Business Improvement', 16, 1),
	(56, N'ZTLOS039', N'Team Leadership / Management', 16, 1),
	(57, N'ZTLOS040', N'Business Support', 16, 1),
	(58, N'ZTLOS062', N'Legal Services: Business, Finance and Employment', 17, 1),
	(59, N'ZTLOS063', N'Legal Services: Crime, Criminal Justice and Social Welfare', 17, 1),
	(60, N'ZTLOS056', N'Crop Production', 18, 1),
	(61, N'ZTLOS057', N'Floristry', 18, 1),
	(62, N'ZTLOS058', N'Land-based Engineering', 18, 1),
	(63, N'ZTLOS059', N'Livestock Production', 18, 1),
	(64, N'ZTLOS060', N'Ornamental and Environmental Horticulture and Landscaping', 18, 1),
	(66, N'ZTLOS064', N'Cyber Security', 6, 1),
	(67, N'ZTLOS065', N'Tree and Woodland Management and Maintenance (Arboriculture)', 18, 1),
	(68, N'ZTLOS066', N'Tree and Woodland Management and Maintenance (Forestry)', 18, 1),
	(69, N'ZTLOS074', N'Animal Management and Behaviour', 19, 1),
	(70, N'ZTLOS075', N'Animal Management and Science', 19, 1),
	(71, N'ZTLOS068', N'Jewellery Maker', 20, 1),
	(72, N'ZTLOS069', N'Ceramics Maker', 20, 1),
	(73, N'ZTLOS076', N'Furniture Maker (Maker)', 20, 1),
	(74, N'ZTLOS067', N'Textiles and Fashion Maker', 20, 1),
	(75, N'ZTLOS072', N'Creative Media Technician', 21, 1),
	(76, N'ZTLOS073', N'Events and Venues Technician', 21, 1),
	(77, N'ZTLOS071', N'Content Creation and Production', 21, 1),
	(78, N'ZTLOS077', N'Furniture Maker (Upholsterer)', 20, 1),
	(79, N'ZTLOS084', N'Electrotechnical Engineering', 22, 1),
	(80, N'ZTLOS086', N'Protection Systems Engineering', 22, 1),
	(81, N'ZTLOS087', N'Plumbing and Heating Engineering', 22, 1),
	(82, N'ZTLOS085', N'Gas Engineering', 22, 1),
	(83, N'10202106', N'Refrigeration Engineering', 22, 1),
	(84, N'10202107', N'Air Conditioning Engineering', 22, 1),
	(85, N'ZTLOS083', N'Building Services Design', 23, 1),
	(86, N'ZTLOS082', N'Civil Engineering', 23, 1),
	(87, N'ZTLOS081', N'Surveying and Design for Construction and the Built Environment', 23, 1),
	(88, N'ZTLOS095', N'Data Analytics Technician', 24, 1),
	(89, N'ZTLOS090', N'Digital Software Development', 25, 1),
	(90, N'ZTLOS091', N'Digital Infrastructure', 26, 1),
	(91, N'ZTLOS092', N'Network Cabling', 26, 1),
	(92, N'ZTLOS093', N'Digital Support', 26, 1),
	(93, N'ZTLOS094', N'Cyber Security', 26, 1),
	(94, N'ZTLOS079', N'Early Years Educator', 27, 1),
	(95, N'ZTLOS080', N'Assisting Teaching', 27, 1),
	(96, N'ZTLOS088', N'Heating Engineering and Ventilation', 22, 1),
	(97, N'ZTLOS096', N'Project Delivery for Construction and the Built Environment', 23, 1),
	(98, N'ZTLOS078', N'Marketing Assistant (Multi-Channel)', 28, 1)
  )
  AS Source ([Id], [LarId], [Name], [TlPathwayId], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[LarId] <> Source.[LarId] COLLATE Latin1_General_CS_AS)
	 OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	 OR (Target.[TlPathwayId] <> Source.[TlPathwayId])
	 OR (Target.[IsActive] <> Source.[IsActive]))
THEN 
UPDATE SET 
	[LarId] = Source.[LarId],
	[Name] = Source.[Name],
	[TlPathwayId] = Source.[TlPathwayId],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [LarId], [Name], [TlPathwayId], [CreatedBy]) 
	VALUES ([Id], [LarId], [Name], [TlPathwayId], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;
SET IDENTITY_INSERT [dbo].[TlSpecialism] OFF