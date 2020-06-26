CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_TqRegistrationPathway] ON TqRegistrationPathway 
(
    TqRegistrationProfileId, TqProviderId
)
WHERE ([Status] = 1)