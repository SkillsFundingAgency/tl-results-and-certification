namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class ApiConstants
    {
        // Verify T Level Related Uri's
        public const string GetAllTLevelsUri = "/api/Tlevel/GetAllTLevels/{0}";
        public const string GetTlevelsByStatus = "/api/Tlevel/{0}/GetTlevelsByStatus/{1}";
        public const string TlevelDetailsUri = "/api/Tlevel/{0}/TlevelDetails/{1}";
        public const string VerifyTlevelUri = "/api/Tlevel/VerifyTlevel";

        // Provider Related Uri's
        public const string IsAnyProviderSetupCompletedUri = "/api/provider/IsAnyProviderSetupCompleted/{0}";
        public const string FindProviderAsyncUri = "/api/provider/FindProvider/{0}/{1}";
        public const string GetSelectProviderTlevelsUri = "/api/provider/GetSelectProviderTlevels/{0}/{1}";
        public const string AddProviderTlevelsUri = "/api/provider/AddProviderTlevels";
        public const string GetAllProviderTlevelsAsyncUri = "/api/provider/GetAllProviderTlevels/{0}/{1}";
        public const string GetTqAoProviderDetailsAsyncUri = "/api/provider/GetTqAoProviderDetails/{0}";
        public const string GetTqProviderTlevelDetailsAsyncUri = "/api/provider/GetTqProviderTlevelDetails/{0}/{1}";
        public const string RemoveTqProviderTlevelAsyncUri = "/api/provider/RemoveProviderTlevel/{0}/{1}";
    }
}
