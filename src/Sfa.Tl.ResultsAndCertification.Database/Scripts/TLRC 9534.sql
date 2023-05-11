BEGIN TRANSACTION [UpdateAcademicYear]

  BEGIN TRY
	-- Update script for 
	DECLARE @ULN1 BIGINT = 3545229229

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
