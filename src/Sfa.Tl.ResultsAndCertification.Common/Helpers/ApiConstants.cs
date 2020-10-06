namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class ApiConstants
    {
        // Verify T Level Related Uri's
        public const string GetAllTLevelsUri = "/api/tlevel/GetAllTLevels/{0}";
        public const string GetTlevelsByStatus = "/api/tlevel/{0}/GetTlevelsByStatus/{1}";
        public const string TlevelDetailsUri = "/api/tlevel/{0}/TlevelDetails/{1}";
        public const string VerifyTlevelUri = "/api/tlevel/VerifyTlevel";
        public const string GetPathwaySpecialismsByPathwayLarIdAsyncUri = "/api/tlevel/{0}/GetPathwaySpecialisms/{1}";

        // Provider Related Uri's
        public const string IsAnyProviderSetupCompletedUri = "/api/provider/IsAnyProviderSetupCompleted/{0}";
        public const string FindProviderAsyncUri = "/api/provider/FindProvider/{0}/{1}";
        public const string GetSelectProviderTlevelsUri = "/api/provider/GetSelectProviderTlevels/{0}/{1}";
        public const string AddProviderTlevelsUri = "/api/provider/AddProviderTlevels";
        public const string GetAllProviderTlevelsAsyncUri = "/api/provider/GetAllProviderTlevels/{0}/{1}";
        public const string GetTqAoProviderDetailsAsyncUri = "/api/provider/GetTqAoProviderDetails/{0}";
        public const string GetTqProviderTlevelDetailsAsyncUri = "/api/provider/GetTqProviderTlevelDetails/{0}/{1}";
        public const string RemoveTqProviderTlevelAsyncUri = "/api/provider/RemoveProviderTlevel/{0}/{1}";
        public const string HasAnyTlevelSetupForProviderAsyncUri = "/api/provider/HasAnyTlevelSetupForProvider/{0}/{1}";
        public const string GetRegisteredProviderPathwayDetailsAsyncUri = "/api/provider/GetRegisteredProviderPathwayDetails/{0}/{1}";

        // Registrations Related Uri's
        public const string ProcessBulkRegistrationsUri = "/api/registration/ProcessBulkRegistrations";
        public const string AddRegistrationUri = "/api/registration/AddRegistration";
        public const string FindUlnUri = "/api/registration/FindUln/{0}/{1}";
        public const string GetRegistrationDetailsUri = "/api/registration/GetRegistrationDetails/{0}/{1}/{2}";
        public const string DeleteRegistrationUri = "/api/registration/DeleteRegistration/{0}/{1}";
        public const string UpdateRegistrationUri = "/api/registration/UpdateRegistration";
        public const string WithdrawRegistrationUri = "/api/registration/WithdrawRegistration";
        public const string RejoinRegistrationUri = "/api/registration/RejoinRegistration";
        public const string ReregistrationUri = "/api/registration/Reregistration";

        // DocumentUploadHistory Related Uri's
        public const string GetDocumentUploadHistoryDetailsAsyncUri = "/api/DocumentUploadHistory/GetDocumentUploadHistoryDetails/{0}/{1}";
    }
}
