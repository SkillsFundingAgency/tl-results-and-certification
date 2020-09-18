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
        public const string SubmitRegistrationUln = "SubmitRegistrationUln";
        public const string AddRegistrationLearnersName = "AddRegistrationLearnersName";
        public const string SubmitRegistrationLearnersName = "SubmitRegistrationLearnersName";
        public const string AddRegistrationDateofBirth = "AddRegistrationDateofBirth";
        public const string SubmitRegistrationDateofBirth = "SubmitRegistrationDateofBirth";
        public const string AddRegistrationProvider = "AddRegistrationProvider";
        public const string SubmitRegistrationProvider = "SubmitRegistrationProvider";
        public const string AddRegistrationCore = "AddRegistrationCore";
        public const string SubmitRegistrationCore = "SubmitRegistrationCore";
        public const string AddRegistrationSpecialismQuestion = "AddRegistrationSpecialismQuestion";
        public const string SubmitRegistrationSpecialismQuestion = "SubmitRegistrationSpecialismQuestion";
        public const string AddRegistrationSpecialisms = "AddRegistrationSpecialisms";
        public const string SubmitRegistrationSpecialisms = "SubmitRegistrationSpecialisms";
        public const string AddRegistrationAcademicYear = "AddRegistrationAcademicYear";
        public const string UlnCannotBeRegistered = "UlnCannotBeRegistered";
        public const string SubmitRegistrationAcademicYear = "SubmitRegistrationAcademicYear";
        public const string AddRegistrationCheckAndSubmit = "AddRegistrationCheckAndSubmit";
        public const string SubmitRegistrationCheckAndSubmit = "SubmitRegistrationCheckAndSubmit";
        public const string AddRegistrationConfirmation = "AddRegistrationConfirmation";

        // Change Registration
        public const string ChangeRegistrationLearnersName = "ChangeRegistrationLearnersName";
        public const string SubmitChangeRegistrationLearnersName = "SubmitChangeRegistrationLearnersName";
        public const string ChangeRegistrationDateofBirth = "ChangeRegistrationDateofBirth";
        public const string SubmitChangeRegistrationDateofBirth = "SubmitChangeRegistrationDateofBirth";
        public const string ChangeRegistrationProvider = "ChangeRegistrationProvider";
        public const string SubmitChangeRegistrationProvider = "SubmitChangeRegistrationProvider";
        public const string ChangeRegistrationCoreQuestion = "ChangeRegistrationCoreQuestion";
        public const string SubmitChangeCoreQuestion = "SubmitChangeCoreQuestion";
        public const string ChangeRegistrationConfirmation = "ChangeRegistrationConfirmation";
        public const string ChangeRegistrationCore = "ChangeRegistrationCore";
        public const string ChangeRegistrationSpecialismQuestion = "ChangeRegistrationSpecialismQuestion";
        public const string SubmitChangeRegistrationSpecialismQuestion = "SubmitChangeRegistrationSpecialismQuestion";
        public const string ChangeRegistrationSpecialisms = "ChangeRegistrationSpecialisms";
        public const string SubmitChangeRegistrationSpecialisms = "SubmitChangeRegistrationSpecialisms";
        public const string ChangeAcademicYear = "ChangeAcademicYear";
        public const string ChangeRegistrationProviderAndCoreNeedToWithdraw = "ChangeRegistrationProviderAndCoreNeedToWithdraw";
        public const string ChangeRegistrationProviderNotOfferingSameCore = "ChangeRegistrationProviderNotOfferingSameCore";
        public const string AmendActiveRegistration = "AmendActiveRegistration";
        public const string SubmitAmendActiveRegistration = "SubmitAmendActiveRegistration";
        public const string WithdrawRegistration = "WithdrawRegistration";
        public const string SubmitWithdrawRegistration = "SubmitWithdrawRegistration";

        // Search Registration
        public const string SearchRegistration = "SearchRegistration";
        public const string SubmitSearchRegistration = "SubmitSearchRegistration";
        public const string SearchRegistrationNotFound = "SearchRegistrationNotFound";
        public const string RegistrationDetails = "RegistrationDetails";
                
        // Cancel Registration
        public const string CancelRegistration = "CancelRegistration";
        public const string SubmitCancelRegistration = "SubmitCancelRegistration";
        public const string RegistrationCancelledConfirmation = "RegistrationCancelledConfirmation";

        // Error
        public const string PageNotFound = "PageNotFound";
        public const string ServiceAccessDenied = "ServiceAccessDenied";
        public const string AccessDeniedWrongRole = "AccessDeniedWrongRole";
        public const string ProblemWithService = "ProblemWithService";
        public const string Error = "Error";

        // Help
        public const string Cookies = "Cookies";
        public const string CookieDetails = "CookieDetails";
        public const string PrivacyPolicy = "PrivacyPolicy";
        public const string TermsAndConditions = "TermsAndConditions";
        public const string UserGuide = "UserGuide";

        // Document
        public const string RegistrationDataFormatAndRulesGuide = "RegistrationDataFormatAndRulesGuide";
        public const string DownloadRegistrationDataFormatAndRulesGuide = "DownloadRegistrationDataFormatAndRulesGuide";
    }
}
