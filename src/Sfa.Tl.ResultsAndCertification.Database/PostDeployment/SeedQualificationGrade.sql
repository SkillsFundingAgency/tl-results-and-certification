/*
Insert initial data for QualificationGrade
*/

SET IDENTITY_INSERT [dbo].[QualificationGrade] ON

MERGE INTO [dbo].[QualificationGrade] AS Target 
USING (VALUES 
   /* Basic Skills */
  (1, 1, N'Pass', 1, 1, 0, 1),
  (2, 1, N'Fail', 2, 0, 0, 1),

  /* Free Standing Mathematics Qualification */
  (3, 2, N'A', 1, 1, 0, 1),
  (4, 2, N'B', 2, 1, 0, 1),
  (5, 2, N'C', 3, 1, 0, 1),
  (6, 2, N'D', 4, 0, 0, 1),
  (7, 2, N'E', 5, 0, 0, 1),

  /* Functional Skills */
  (8, 3, N'Pass', 1, 1, 0, 1),
  (9, 3, N'Fail', 2, 0, 0, 1),

  /* Functional Skills (QCF) */
  (10, 4, N'Pass', 1, 1, 0, 1),
  (11, 4, N'Fail', 2, 0, 0, 1),

  /* GCE A Level */
  (12, 5, N'A*', 1, 1, 0, 1),
  (13, 5, N'A', 2, 1, 0, 1),
  (14, 5, N'B', 3, 1, 0, 1),
  (15, 5, N'C', 4, 1, 0, 1),
  (16, 5, N'D', 5, 1, 0, 1),
  (17, 5, N'E', 6, 1, 0, 1),
  (18, 5, N'G', 7, 0, 0, 1),

  /* GCE AS Level */
  (19, 6, N'A', 1, 1, 0, 1),
  (20, 6, N'B', 2, 1, 0, 1),
  (21, 6, N'C', 3, 1, 0, 1),
  (22, 6, N'D', 4, 1, 0, 1),
  (23, 6, N'E', 5, 1, 0, 1),

  /* GCSE (9 to 1) */
  (24, 7, N'9', 1, 1, 0, 1),
  (25, 7, N'8', 2, 1, 0, 1),
  (26, 7, N'7', 3, 1, 0, 1),
  (27, 7, N'6', 4, 1, 0, 1),
  (28, 7, N'5', 5, 1, 0, 1),
  (29, 7, N'4', 6, 1, 0, 1),
  (30, 7, N'3', 7, 1, 1, 0),
  (31, 7, N'2', 8, 1, 1, 0),
  (32, 7, N'1', 9, 1, 1, 0),

  /* GCSE (A* to G) */
  (33, 8, N'A*', 1, 1, 0, 1),
  (34, 8, N'A', 2, 1, 0, 1),
  (35, 8, N'B', 3, 1, 0, 1),
  (36, 8, N'C', 4, 1, 0, 1),
  (37, 8, N'D', 5, 0, 0, 1),
  (38, 8, N'E', 6, 0, 0, 1),
  (39, 8, N'F', 7, 0, 0, 1),
  (40, 8, N'G', 8, 0, 0, 1),

  /* GCSE (A* to G) - Double Award */
  (41, 9, N'A*A*', 1, 1, 0, 1),
  (42, 9, N'A*A', 2, 1, 0, 1),
  (43, 9, N'AA', 3, 1, 0, 1),
  (44, 9, N'AB', 4, 1, 0, 1),
  (45, 9, N'BB', 5, 1, 0, 1),
  (46, 9, N'BC', 6, 1, 0, 1),
  (47, 9, N'CC', 7, 1, 0, 1),
  (48, 9, N'CD', 8, 0, 0, 1),
  (49, 9, N'DD', 9, 0, 0, 1),
  (50, 9, N'DE', 10, 0, 0, 1),
  (51, 9, N'EE', 11, 0, 0, 1),
  (52, 9, N'EF', 12, 0, 0, 1),
  (53, 9, N'FF', 13, 0, 0, 1),
  (54, 9, N'FG', 14, 0, 0, 1),
  (55, 9, N'GG', 15, 0, 0, 1),
  
  /* Other General Qualification - Level 1/Level 2 Certificate - (A* to G) */
  (56, 10, N'A*', 1, 1, 0, 1),
  (57, 10, N'A', 2, 1, 0, 1),
  (58, 10, N'B', 3, 1, 0, 1),
  (59, 10, N'C', 4, 1, 0, 1),
  (60, 10, N'D', 5, 0, 0, 1),
  (61, 10, N'E', 6, 0, 0, 1),
  (62, 10, N'F', 7, 0, 0, 1),
  (63, 10, N'G', 8, 0, 0, 1),

  /* Other General Qualification - Level 1/Level 2 Certificate - (9 to 1) */
  (64, 11, N'9', 1, 1, 0, 1),
  (65, 11, N'8', 2, 1, 0, 1),
  (66, 11, N'7', 3, 1, 0, 1),
  (67, 11, N'6', 4, 1, 0, 1),
  (68, 11, N'5', 5, 1, 0, 1),
  (69, 11, N'4', 6, 1, 0, 1),
  (70, 11, N'3', 7, 0, 0, 1),
  (71, 11, N'2', 8, 0, 0, 1),
  (72, 11, N'1', 9, 0, 0, 1),

  /* Other General Qualification - Math */
  (73, 12, N'A', 1, 1, 0, 1),
  (74, 12, N'B', 2, 1, 0, 1),
  (75, 12, N'C', 3, 1, 0, 1),
  (76, 12, N'D', 4, 1, 0, 1),
  (77, 12, N'E', 5, 1, 0, 1),

  /* Other General Qualification - Pre U Certificate */
  (78, 13, N'D1', 1, 1, 0, 1),
  (79, 13, N'D2', 2, 1, 0, 1),
  (80, 13, N'D3', 3, 1, 0, 1),
  (81, 13, N'M1', 4, 1, 0, 1),
  (82, 13, N'M2', 5, 1, 0, 1),
  (83, 13, N'M3', 6, 1, 0, 1),
  (84, 13, N'P1', 7, 1, 0, 1),
  (85, 13, N'P2', 8, 1, 0, 1),
  (86, 13, N'P3', 9, 1, 0, 1),

  /* Other General Qualification - IBO Level1/Level 2 MYP */
  (87, 14, N'7', 1, 1, 0, 1),
  (88, 14, N'6', 2, 1, 0, 1),
  (89, 14, N'5', 3, 1, 0, 1),
  (90, 14, N'4', 4, 1, 0, 1),
  (91, 14, N'3', 5, 1, 0, 1),
  (92, 14, N'2', 6, 0, 0, 1),
  (93, 14, N'1', 7, 0, 0, 1),

  /* Other General Qualification - IBO Level 3 Certificate */
  (94, 15, N'7', 1, 1, 0, 1),
  (95, 15, N'6', 2, 1, 0, 1),
  (96, 15, N'5', 3, 1, 0, 1),
  (97, 15, N'4', 4, 1, 0, 1),
  (98, 15, N'3', 5, 1, 0, 1),
  (99, 15, N'2', 6, 0, 0, 1),
  (100, 15, N'1', 7, 0, 0, 1),

  /* Other General Qualification - British Sign Language */
  (101, 16, N'Pass', 1, 1, 0, 1),
  (102, 16, N'Fail', 2, 0, 0, 1),

  /* Other General Qualification - Level 2 Essential Skills Wales */
  (103, 17, N'Pass', 1, 1, 0, 1),
  (104, 17, N'Fail', 2, 0, 0, 1),

  /* Other Vocational Qualification - British Sign Language */
  (105, 18, N'Pass', 1, 1, 0, 1),
  (106, 18, N'Fail', 2, 0, 0, 1),

  /* Project - British Sign Language */
  (107, 19, N'Pass', 1, 1, 0, 1),
  (108, 19, N'Fail', 2, 0, 0, 1),

  /* QCF - British Sign Language */
  (109, 20, N'Pass', 1, 1, 0, 1),
  (110, 20, N'Fail', 2, 0, 0, 1),

  /* Vocationally-Related Qualification - Level 3 Math */
  (111, 21, N'A', 1, 1, 0, 1),
  (112, 21, N'B', 2, 1, 0, 1),
  (113, 21, N'C', 3, 1, 0, 1),
  (114, 21, N'D', 4, 1, 0, 1),
  (115, 21, N'E', 5, 1, 0, 1),

  /* Vocationally-Related Qualification - British Sign Language */
  (116, 22, N'Pass', 1, 1, 0, 1),
  (117, 22, N'Fail', 2, 0, 0, 1),

  /* Functional Skills - Entry 3 */
  (118, 23, N'Pass', 1, 1, 0, 1),
  (119, 23, N'Fail', 2, 0, 0, 1),

  /* Functional Skills (QCF) - Entry 3 */
  (120, 24, N'Pass', 1, 1, 0, 1),
  (121, 24, N'Fail', 2, 0, 0, 1),

  /* Functional Skills - Level 1 */
  (122, 25, N'Pass', 1, 1, 0, 1),
  (123, 25, N'Fail', 2, 0, 0, 1),

  /* Functional Skills (QCF) - Level 1 */
  (124, 26, N'Pass', 1, 1, 0, 1),
  (125, 26, N'Fail', 2, 0, 0, 1)
  )
  AS Source ([Id], [QualificationTypeId], [Grade], [GradeRank], [IsAllowable], [IsSendGrade], [IsActive]) 
ON Target.[Id] = Source.[Id] 
-- Update from Source when Id is Matched
WHEN MATCHED 
	 AND ((Target.[QualificationTypeId] <> Source.[QualificationTypeId])     
	 OR (Target.[Grade] <> Source.[Grade] COLLATE Latin1_General_CS_AS)
	 OR (Target.[GradeRank] <> Source.[GradeRank])
     OR (Target.[IsAllowable] <> Source.[IsAllowable])
	 OR (Target.[IsSendGrade] <> Source.[IsSendGrade])
	 OR (Target.[IsActive] <> Source.[IsActive])	
	 )
THEN 
UPDATE SET 	
    [QualificationTypeId] = Source.[QualificationTypeId],
	[Grade] = Source.[Grade],
	[GradeRank] = Source.[GradeRank],
    [IsAllowable] = Source.[IsAllowable],
	[IsSendGrade] = Source.[IsSendGrade],
	[IsActive] = Source.[IsActive],
	[ModifiedOn] = GETDATE(),
	[ModifiedBy] = 'System'
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [QualificationTypeId], [Grade], [GradeRank], [IsAllowable], [IsSendGrade], [IsActive], [CreatedBy]) 
	VALUES ([Id], [QualificationTypeId], [Grade], [GradeRank], [IsAllowable], [IsSendGrade], [IsActive],'System') 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE;

SET IDENTITY_INSERT [dbo].[QualificationGrade] OFF
