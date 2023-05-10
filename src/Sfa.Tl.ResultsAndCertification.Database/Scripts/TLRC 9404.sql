
BEGIN TRANSACTION [UpdateAcademicYear]

  BEGIN TRY
	-- Update script for 
	DECLARE @ULN1 INT = 6132081576

	IF EXISTS (SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN1)
		 BEGIN 
			UPDATE [dbo].[TqRegistrationPathway]
			SET [AcademicYear]=2021,
			ModifiedBy='Support', ModifiedOn=GETDATE()
			WHERE TqRegistrationProfileId=
			(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN1)

			PRINT 'Academic Year Updated for ULN:' + CONVERT(VARCHAR,@ULN1)
		END
		ELSE 
		  BEGIN 
			 PRINT 'ULN:' + CONVERT(VARCHAR,@ULN1) +'Does not exists.'
		  END 

 
	COMMIT TRANSACTION  [UpdateAcademicYear]
  END TRY
	  BEGIN CATCH

	    SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_STATE() AS ErrorState,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		 ERROR_MESSAGE() AS ErrorMessage;
	ROLLBACK TRANSACTION  [UpdateAcademicYear]
	  END CATCH  


	  -- Insert scripts for the core assessment for autumn 2022 

BEGIN TRANSACTION [UpdateCoreAssessment]

  BEGIN TRY

  	 -- ULN 6471844505
	 DECLARE @ULN2 INT = 6471844505,
			 @ULN3 INT = 8378805653,
			 @ULN4 INT = 9994253009,
			 @ULN5 INT = 1322580471,
			 @ULN6 INT = 2757754393,
			 @ULN7 INT = 5693362638,
			 @ULN8 INT = 8618567042,
			 @ULN9 INT = 9794483906,
			 @ULN10 INT = 6132081576,
			 @PathwayID INT;
	 
				
	SELECT @PathwayID= ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId =
	(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN2)
		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
					INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
					VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())

					PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN2)
					SET @PathwayID=''
			END
		ELSE 
			BEGIN 
				PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN2)
			END 
	 

	    
		SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId =
				(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN3)

		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
				INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
				VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
				PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN3)
				SET @PathwayID=''
			END
		ELSE 
			BEGIN 
				PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN3)
			END 

			   		 	  
	
			
	 
	 -- ULN 9994253009
	 SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId = 
				(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN4)
		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
			INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
			VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
				PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN4)
				SET @PathwayID=''
			END
		ELSE 
		BEGIN
			PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN4)
		END 

		   -- ULN 1322580471 
	 
	 SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId = 
						(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN5)

		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
					INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
					VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
				PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN5)
				SET @PathwayID=''
			END
		ELSE 
		BEGIN
			PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN5)
		END 

		   -- ULN 2757754393
 	 
	 SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId = 
						(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN6)

		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
				INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
				VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
			PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN6)
				SET @PathwayID=''
			END
		ELSE 
		BEGIN
			PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN6)
		END


	   -- ULN 5693362638
  
	 SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId = 
						(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN7)

		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
				INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
				VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
				PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN7)
				SET @PathwayID=''
				END
		ELSE 
		BEGIN
			PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN7)
		END


		 -- ULN 8618567042
			 
	 SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId = 
						(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN8)

		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
				INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
				VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
				PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN8)
				SET @PathwayID=''
			END
		ELSE 
		BEGIN
			PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN8)
		END

		 -- ULN 9794483906
 	 
	 SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId = 
						(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN9)

		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
				INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
				VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
			PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN9)
				SET @PathwayID=''
			END
		ELSE 
		BEGIN
			PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN9)
		END

		 -- ULN 6132081576
 	 
	 SELECT @PathwayID = ID FROM [dbo].[TqRegistrationPathway] WHERE TqRegistrationProfileId = 
						(SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=@ULN10)

		IF @PathwayID is  not null and @PathwayID <> ''		
			BEGIN
				INSERT into [dbo].[TqPathwayAssessment](TqRegistrationPathwayId,AssessmentSeriesId,StartDate,CreatedBy,CreatedOn)
				VALUES(@PathwayID,4,'2023-03-06','Support',GETDATE())
				PRINT 'Core Assesment Entry Created for ULN:' + CONVERT(VARCHAR,@ULN10)
				SET @PathwayID=''
			END
		ELSE 
		BEGIN
			PRINT 'RegistrationPathwayId is null or empty for ULN:' + CONVERT(VARCHAR,@ULN10)
		END

COMMIT TRANSACTION [UpdateCoreAssessment]
  END TRY
	  BEGIN CATCH
		   SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_STATE() AS ErrorState,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		 ERROR_MESSAGE() AS ErrorMessage;
	ROLLBACK TRANSACTION  [UpdateCoreAssessment]
	  END CATCH  


