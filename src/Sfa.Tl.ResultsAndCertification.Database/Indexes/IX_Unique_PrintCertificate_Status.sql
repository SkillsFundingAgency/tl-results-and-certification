CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_PrintCertificate_Status] ON PrintCertificate 
(
    TqRegistrationPathwayId, Uln, [Type]
)
WHERE ([Status] = 1)