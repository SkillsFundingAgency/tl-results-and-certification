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
        public const string YourTlevels = "YourTlevels";
        public const string TlevelDetails = "TlevelDetails";
        public const string SelectTlevel = "SelectTlevel";
        public const string SelectTlevelSubmit = "SelectTlevelSubmit";
        public const string AreDetailsCorrect = "AreDetailsCorrect";
        public const string ConfirmTlevel = "ConfirmTlevel";
        public const string TlevelDetailsConfirmed = "TlevelDetailsConfirmed";
        public const string TlevelDetailsQueriedConfirmation = "TlevelDetailsQueriedConfirmation";
        public const string QueryTlevelDetails = "QueryTlevelDetails";  // QueryTlevelDetails_Get
        public const string SubmitTlevelIssue = "SubmitTlevelIssue";  // QueryTlevelDetails_Post
        public const string QueryServiceProblem = "QueryServiceProblem";

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

        // Registrations
        public const string RegistrationDashboard = "RegistrationDashboard";
        public const string UploadRegistrationsFile = "UploadRegistrationsFile";
        public const string SubmitUploadRegistrationsFile = "SubmitUploadRegistrationsFile";
        public const string RegistrationsUploadSuccessful = "RegistrationsUploadSuccessful";
        public const string RegistrationsUploadUnsuccessful = "RegistrationsUploadUnsuccessful";
        public const string ProblemWithRegistrationsUpload = "ProblemWithRegistrationsUpload";
        public const string DownloadRegistrationErrors = "DownloadRegistrationErrors";

        // Add Registration
        public const string AddRegistration = "AddRegistration";
        public const string AddRegistrationUln = "AddRegistrationUln";
        public const string AddRegistrationLearnersName = "AddRegistrationLearnersName";
        public const string SubmitRegistrationLearnersName = "SubmitRegistrationLearnersName";

        // Error
        public const string PageNotFound = "PageNotFound";
        public const string ServiceAccessDenied = "ServiceAccessDenied";
        public const string AccessDeniedWrongRole = "AccessDeniedWrongRole";
        public const string ProblemWithService = "ProblemWithService";
        public const string Error = "Error";

        // Help
        public const string CookiePolicy = "CookiePolicy";
        public const string Privacy = "Privacy";
        public const string TermsAndConditions = "TermsAndConditions";
        public const string UserGuide = "UserGuide";
    }
}
