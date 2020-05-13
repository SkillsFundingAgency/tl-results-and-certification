namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class RouteConstants
    {
        // Route Constants

        // Account
        public const string SignIn = "SignIn";
        public const string SignOut = "SignOut";
        public const string SignOutDsi = "SignOutDsi";
        public const string SignOutComplete = "SignOutComplete";
        public const string AccountProfile = "AccountProfile";

        // Dashboard
        public const string Home = "Home";

        // Tlevel
        public const string Tlevels = "Tlevels";        
        public const string ViewAllTlevels = "ViewAllTlevels";
        public const string TlevelDetails = "TlevelDetails";
        public const string SelectTlevel = "SelectTlevel";
        public const string SelectTlevelSubmit = "SelectTlevelSubmit";
        public const string VerifyTlevel = "VerifyTlevel";
        public const string ConfirmTlevel = "ConfirmTlevel";
        public const string TlevelConfirmation = "TlevelConfirmation";
        public const string ReportTlevelIssue = "ReportTlevelIssue";  // ReportTlevel_Get
        public const string SubmitTlevelIssue = "SubmitTlevelIssue";  // ReportTlevel_Post

        // Providers
        public const string YourProviders = "YourProviders";
        public const string FindProvider = "FindProvider";
        public const string SubmitFindProvider = "SubmitFindProvider";
        public const string ProviderNameLookup = "ProviderNameLookup";
        public const string SelectProviderTlevels = "SelectProviderTlevels";
        public const string AddProviderTlevels = "AddProviderTlevels";
        public const string SubmitSelectProviderTlevels = "SubmitSelectProviderTlevels";
        public const string SubmitAddProviderTlevels = "SubmitAddProviderTlevels";
        public const string ProviderTlevelConfirmation = "ProviderTlevelConfirmation";
        public const string ProviderTlevels = "ProviderTlevels";
        public const string RemoveProviderTlevel = "RemoveProviderTlevel";
        public const string SubmitRemoveProviderTlevel = "SubmitRemoveProviderTlevel";
        public const string RemoveProviderTlevelConfirmation = "RemoveProviderTlevelConfirmation";

        // Error
        public const string PageNotFound = "PageNotFound";
        public const string ServiceAccessDenied = "ServiceAccessDenied";
        public const string AccessDenied = "AccessDenied";
        public const string ProblemWithService = "ProblemWithService";
        public const string Error = "Error";

        // Help
        public const string CookiePolicy = "CookiePolicy";
        public const string Privacy = "Privacy";
        public const string TermsAndConditions = "TermsAndConditions";
        public const string UserGuide = "UserGuide";
    }
}
