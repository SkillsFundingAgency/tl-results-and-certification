UPDATE [dbo].[FunctionLog]  
SET [FunctionType] = CASE [Name]  
	WHEN 'FetchLearnerGender' THEN 1  
	WHEN 'VerifyLearnerAndFetchLearningEvents' THEN 2  
	WHEN 'SubmitCertificatePrintingRequest' THEN 3  
	WHEN 'FetchCertificatePrintingBatchSummary' THEN 4  
	WHEN 'FetchCertificatePrintingTrackBatch' THEN 5  
	WHEN 'UcasTransferEntries' THEN 6  
	WHEN 'UcasTransferResults' THEN 7  
	WHEN 'UcasTransferAmendments' THEN 8  
	WHEN 'OverallResultCalculation' THEN 9  
ELSE 0  
END 