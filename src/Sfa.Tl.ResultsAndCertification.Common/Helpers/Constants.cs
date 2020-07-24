namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class Constants
    {
        // Environment Constants
        public const string EnvironmentNameConfigKey = "EnvironmentName";
        public const string ConfigurationStorageConnectionStringConfigKey = "ConfigurationStorageConnectionString";
        public const string VersionConfigKey = "Version";
        public const string ServiceNameConfigKey = "ServiceName";

        // Controller Names
        public const string HomeController = "Home";
        public const string AccountController = "Account";
        public const string DashboardController = "Dashboard";
        public const string ErrorController = "Error";
        public const string TlevelController = "Tlevel";

        // TempData Key Constants
        public const string IsRedirect = "IsRedirect";
        public const string IsBackToVerifyPage = "IsBackToVerifyPage";
        public const string FindProviderSearchCriteria = "FindProviderSearchCriteria";
        public const string ProviderTlevelsViewModel = "ProviderTlevelsViewModel";
        public const string ProviderTlevelDetailsViewModel = "ProviderTlevelDetailsViewModel";

        public const string UploadUnsuccessfulViewModel = "UploadUnsuccessfulViewModel";
        public const string UploadSuccessfulViewModel = "UploadSuccessfulViewModel";
        public const string UlnNotFoundViewModel = "UlnNotFoundViewModel";

        public const string RegistrationConfirmationViewModel = "RegistrationConfirmationViewModel";
        public const string SearchRegistrationUlnNotFound = "SearchRegistrationUlnNotFound";
        public const string RegistrationSearchCriteria = "RegistrationSearchCriteria";

        // Registration Data Index Constants
        public const int RegistrationProfileStartIndex = 100000;
        public const int RegistrationPathwayStartIndex = 200000;
        public const int RegistrationSpecialismsStartIndex = 300000;
    }
}
