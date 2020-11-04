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
        public const string ChangeRegistrationConfirmationViewModel = "ChangeRegistrationConfirmationViewModel";
        public const string SearchRegistrationUlnNotFound = "SearchRegistrationUlnNotFound";
        public const string RegistrationSearchCriteria = "RegistrationSearchCriteria";
        public const string RegistrationChangeProviderViewModel = "RegistrationChangeProviderViewModel";
        public const string ChangeRegistrationProviderCoreNotSupportedViewModel = "ChangeRegistrationProviderCoreNotSupportedViewModel";
        public const string ChangeRegistrationCoreNotSupportedProviderUkprn = "ChangeRegistrationCoreNotSupportedProviderUkprn";
        public const string WithdrawRegistrationConfirmationViewModel = "WithdrawRegistrationConfirmationViewModel";
        public const string RejoinRegistrationConfirmationViewModel = "RejoinRegistrationConfirmationViewModel";
        public const string ReregistrationConfirmationViewModel = "ReregistrationConfirmationViewModel";

        public const string AssessmentsUploadSuccessfulViewModel = "AssessmentsUploadSuccessfulViewModel";

        public const string UserSessionActivityId = "UserSessionActivityId";

        // Registration Data Index Constants
        public const int RegistrationProfileStartIndex = 100000;
        public const int RegistrationPathwayStartIndex = 200000;
        public const int RegistrationSpecialismsStartIndex = 300000;

        // Assessment Data Index Constants
        public const int PathwayAssessmentsStartIndex = 100000;
        public const int SpecialismAssessmentsStartIndex = 100000;

        // Route Attributes
        public const string IsChangeMode = "isChangeMode";
        public const string ProfileId = "profileId";
        public const string IsBack = "isBack";
        public const string ChangeStatusId = "changeStatusId";
        public const string WithdrawBackLinkOptionId = "withdrawBackLinkOptionId";
    }
}
