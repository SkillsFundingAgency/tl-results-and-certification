using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class ApiConstants
    {
        public const string GetAllTLevelsUri = "/api/Tlevel/GetAllTLevels/{0}";
        public const string GetTlevelsByStatus = "/api/Tlevel/{0}/GetTlevelsByStatus/{1}";
        public const string TlevelDetailsUri = "/api/Tlevel/{0}/TlevelDetails/{1}";
        public const string VerifyTlevelUri = "/api/Tlevel/VerifyTlevel";

        public const string IsAnyProviderSetupCompletedUri = "/api/provider/IsAnyProviderSetupCompleted/{0}";
        public const string FindProviderNameAsyncUri = "/api/provider/FindProviderName/{0}/{1}";  // TODO: can this be a object post rather two params. 
        public const string GetSelectProviderTlevelsUri = "/api/provider/GetSelectProviderTlevels/{0}/{1}";
    }
}
