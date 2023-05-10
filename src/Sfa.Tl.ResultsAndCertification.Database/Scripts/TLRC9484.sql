
BEGIN TRANSACTION [UpdateMathsEnglishStatus]

	BEGIN TRY
	-- Update script for 
		IF EXISTS (SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=8924153845)
		 BEGIN 
			UPDATE [dbo].[TqRegistrationProfile]
			SET MathsStatus=1,EnglishStatus=1,
			ModifiedBy='Support', ModifiedOn=GETDATE()
			WHERE UniqueLearnerNumber=8924153845
			PRINT 'Maths and English Status updated for ULN:' + CONVERT(VARCHAR,8924153845)
		END
		ELSE 
			BEGIN
				PRINT 'Registration Profile not exists for  ULN:' + CONVERT(VARCHAR,8924153845)
			END 

		IF EXISTS (SELECT ID FROM TqRegistrationProfile WHERE UniqueLearnerNumber=8939213393)
			BEGIN
				UPDATE [dbo].[TqRegistrationProfile]
				SET EnglishStatus=2,
				ModifiedBy='Support', ModifiedOn=GETDATE()
				WHERE UniqueLearnerNumber=8939213393
				PRINT 'English Status updated for ULN:' + CONVERT(VARCHAR,8939213393)
			END
		ELSE 
		BEGIN
			PRINT 'Registration Profile not exists for  ULN:' + CONVERT(VARCHAR,8939213393)
		END 

 
		COMMIT TRANSACTION  [UpdateMathsEnglishStatus]
  END TRY
	  BEGIN CATCH
		   SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_STATE() AS ErrorState,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		 ERROR_MESSAGE() AS ErrorMessage;
	ROLLBACK TRANSACTION  [UpdateMathsEnglishStatus]
	  END CATCH  




	  


