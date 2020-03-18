﻿/*
Insert initial data for Tl Providers
*/

SET IDENTITY_INSERT [dbo].[TlProvider] ON

MERGE INTO [dbo].[TlProvider] AS Target 
USING (VALUES 
(1, 10000080, N'Access Creative College (Access to Music Ltd.)', N'Access Creative College (Access to Music Ltd.)'),
(2, 10000536, N'Barnsley College', N'Barnsley College'),
(3, 10000612, N'Bedfordshire & Luton Education Business Partnership', N'Bedfordshire & Luton Education Business Partnership'),
(4, 10000721, N'Bishop Burton College', N'Bishop Burton College'),
(5, 10000754, N'Blackpool and The Fylde College', N'Blackpool and The Fylde College'),
(6, 10000878, N'Bridgwater & Taunton College', N'Bridgwater & Taunton College'),
(7, 10001165, N'Cardinal Newman College', N'Cardinal Newman College'),
(8, 10007817, N'Chichester College Group', N'Chichester College Group'),
(9, 10001446, N'Cirencester College', N'Cirencester College'),
(10, 10065146, N'City of Stoke-on-Trent sixth Form College', N'City of Stoke-on-Trent sixth Form College'),
(11, 10033241, N'Cranford Community College', N'Cranford Community College'),
(12, 10001919, N'Derby College', N'Derby College'),
(13, 10007924, N'Dudley College of Technology', N'Dudley College of Technology'),
(14, 10002065, N'Durham Sixth Form Centre', N'Durham Sixth Form Centre'),
(15, 10002923, N'East Sussex College Group', N'East Sussex College Group'),
(16, 10002370, N'Exeter College', N'Exeter College'),
(17, 10007928, N'Fareham College', N'Fareham College'),
(18, 10002412, N'Farnborough College of Technology', N'Farnborough College of Technology'),
(19, 10002638, N'Gateshead College', N'Gateshead College'),
(20, 10007938, N'Grimsby Institute of Further & Higher Education', N'Grimsby Institute of Further & Higher Education'),
(21, 10005979, N'Havant and South Downs College', N'Havant and South Downs College'),
(22, 10007193, N'HCUC (Harrow College and Uxbridge College)', N'HCUC (Harrow College and Uxbridge College'),
(23, 10003732, N'La Retraite RC Girls School', N'La Retraite RC Girls School'),
(24, 10033245, N'Lordswood Girls'' School & Sixth Form Centre', N'Lordswood Girls'' School & Sixth Form Centre'),
(25, 10004552, N'Nelson and Colne College', N'Nelson and Colne College'),
(26, 10004576, N'New College Durham', N'New College Durham'),
(27, 10004772, N'City College Norwich', N'City College Norwich'),
(28, 10004785, N'Notre Dame Catholic Sixth Form College', N'Notre Dame Catholic Sixth Form College'),
(29, 10065145, N'Oldham Sixth Form College', N'Oldham Sixth Form College'),
(30, 10038662, N'Painsley Catholic College', N'Painsley Catholic College'),
(31, 10005072, N'Peter Symonds College', N'Peter Symonds College'),
(32, 10063846, N'Priestley College', N'Priestley College'),
(33, 10005575, N'Runshaw College', N'Runshaw College'),
(34, 10057649, N'Salesian School', N'Salesian School'),
(35, 10017433, N'Sandwell Academy', N'Sandwell Academy'),
(36, 10005687, N'Scarborough Sixth Form College', N'Scarborough Sixth Form College'),
(37, 10005810, N'Shipley College of Further Education', N'Shipley College of Further Education'),
(38, 10036413, N'St Thomas More Catholic School', N'St Thomas More Catholic School'),
(39, 10006378, N'Strode College', N'Strode College'),
(40, 10006398, N'Suffolk New College', N'Suffolk New College'),
(41, 10001550, N'The College of Richard Collyer', N'The College of Richard Collyer'),
(42, 10047247, N'The Leigh UTC', N'The Leigh UTC'),
(43, 10057638, N'Thorpe St Andrew School and Sixth Form', N'Thorpe St Andrew School and Sixth Form'),
(44, 10007063, N'Truro and Penwith College', N'Truro and Penwith College'),
(45, 10000712, N'University College Birmingham', N'University College Birmingham'),
(46, 10007190, N'Ursuline High School', N'Ursuline High School'),
(47, 10007315, N'Walsall College', N'Walsall College'),
(48, 10042313, N'Walsall Studio School', N'Walsall Studio School'),
(49, 10007459, N'Weston College', N'Weston College'),
(50, 10007709, N'York College', N'York College'),
(51, 10000055, N'Abingdon and Witney College', N'Abingdon and Witney College'),
(52, 10004927, N'Activate Learning', N'Activate Learning'),
(53, 10000528, N'Barking & Dagenham College', N'Barking & Dagenham College'),
(54, 10001465, N'Bath College', N'Bath College'),
(55, 10000610, N'Bedford College Group', N'Bedford College Group'),
(56, 10000670, N'Bexhill College', N'Bexhill College'),
(57, 10000794, N'Bolton College', N'Bolton College'),
(58, 10000473, N'Buckinghamshire College Group', N'Buckinghamshire College Group'),
(59, 10001000, N'Burnley College', N'Burnley College'),
(60, 10001005, N'Bury College', N'Bury College'),
(61, 10001093, N'Calderdale College', N'Calderdale College'),
(62, 10001116, N'Cambridge Regional College', N'Cambridge Regional College'),
(63, 10005972, N'Cheshire College South and West', N'Cheshire College South and West'),
(64, 10001475, N'City of Sunderland College', N'City of Sunderland College'),
(65, 10004695, N'DN College Group', N'DN College Group'),
(66, 10067981, N'East Norfolk Sixth Form College', N'East Norfolk Sixth Form College'),
(67, 10006570, N'EKC Group', N'EKC Group'),
(68, 10002599, N'Furness College', N'Furness College'),
(69, 10002696, N'Gloucestershire College', N'Gloucestershire College'),
(70, 10002852, N'Halesowen College', N'Halesowen College'),
(71, 10002899, N'Harlow College', N'Harlow College'),
(72, 10007977, N'Heart of Worcestershire College', N'Heart of Worcestershire College'),
(73, 10003023, N'Herefordshire, Ludlow and North Shropshire College', N'Herefordshire, Ludlow and North Shropshire College'),
(74, 10003146, N'Hopwood Hall College', N'Hopwood Hall College'),
(75, 10003193, N'Hugh Baird College', N'Hugh Baird College'),
(76, 10003558, N'Kendal College', N'Kendal College'),
(77, 10003753, N'Lakes College', N'Lakes College'),
(78, 10024962, N'Leeds City College', N'Leeds City College'),
(79, 10003867, N'Leicester College', N'Leicester College'),
(80, 10004112, N'Loughborough College', N'Loughborough College'),
(81, 10023139, N'LTE Group trading as The Manchester College', N'LTE Group trading as The Manchester College'),
(82, 10004344, N'Middlesbrough College', N'Middlesbrough College'),
(83, 10004340, N'Midkent College', N'Midkent College'),
(84, 10004375, N'Milton Keynes College', N'Milton Keynes College'),
(85, 10064840, N'Mulberry University Technical College', N'Mulberry University Technical College'),
(86, 10006963, N'New City College', N'New City College'),
(87, 10004579, N'New College Swindon', N'New College Swindon'),
(88, 10004603, N'Newcastle and Stafford Colleges Group  (NSCG)', N'Newcastle and Stafford Colleges Group  (NSCG)'),
(89, 10004607, N'Newham College of Further Education', N'Newham College of Further Education'),
(90, 10004608, N'Newham Sixth Form College', N'Newham Sixth Form College'),
(91, 10004577, N'Nottingham College', N'Nottingham College'),
(92, 10006770, N'Oldham College', N'Oldham College'),
(93, 10004676, N'Petroc', N'Petroc'),
(94, 10005200, N'Preston College', N'Preston College'),
(95, 10065148, N'Reigate College', N'Reigate College'),
(96, 10005669, N'Sandwell College', N'Sandwell College'),
(97, 10005032, N'SCC Group', N'SCC Group'),
(98, 10005946, N'Solihull College &  University  Centre', N'Solihull College &  University  Centre'),
(99, 10005977, N'South Devon College', N'South Devon College'),
(100, 10005981, N'South Essex College', N'South Essex College'),
(101, 10009439, N'Stanmore College', N'Stanmore College'),
(102, 10007916, N'The College of West Anglia', N'The College of West Anglia'),
(103, 10007455, N'The WKCIC Group', N'The WKCIC Group'),
(104, 10005998, N'Trafford College Group', N'Trafford College Group'),
(105, 10001476, N'United Colleges Group', N'United Colleges Group'),
(106, 10007289, N'Wakefield College', N'Wakefield College'),
(107, 10007859, N'Warwickshire College Group', N'Warwickshire College Group'),
(108, 10053988, N'West Midlands UTC', N'West Midlands UTC'),
(109, 10007431, N'West Suffolk College', N'West Suffolk College'),
(110, 10007500, N'Wigan & Leigh College', N'Wigan & Leigh College'),
(111, 10007503, N'Wilberforce College', N'Wilberforce College'),
(112, 10007673, N'Wyke Sixth Form College', N'Wyke Sixth Form College'),
(113, 10007696, N'Yeovil College', N'Yeovil College')

)
  AS Source ([Id], [UkPrn], [Name], [DisplayName]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[UkPrn] <> Source.[UkPrn])
	   OR (Target.[Name] <> Source.[Name] COLLATE Latin1_General_CS_AS)
	   OR (Target.[DisplayName] <> Source.[DisplayName] COLLATE Latin1_General_CS_AS))
THEN 
UPDATE SET 
	[UkPrn] = Source.[UkPrn],
	[Name] = Source.[Name],
	[DisplayName] = Source.[DisplayName],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [UkPrn], [Name], [DisplayName], [CreatedBy]) 
	VALUES ([Id], [UkPrn], [Name], [DisplayName], 'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[TlProvider] OFF
