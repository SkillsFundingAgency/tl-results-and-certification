UPDATE [dbo].[OverallResult]  
SET [CertificateStatus] = 1
WHERE IsOptedin = 1 AND EndDate IS NULL AND CertificateStatus IS NULL AND CalculationStatus IN (1,4) -- 1 - Completed, 4 - Partially Completed