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
        public const string WithdrawRegistrationConfirmation = "WithdrawRegistrationConfirmation";
        public const string AmendWithdrawRegistration = "AmendWithdrawRegistration";
        public const string SubmitAmendWithdrawRegistration = "SubmitAmendWithdrawRegistration";
        public const string RejoinRegistration = "RejoinRegistration";
        public const string SubmitRejoinRegistration = "SubmitRejoinRegistration";
        public const string RejoinRegistrationConfirmation = "RejoinRegistrationConfirmation";

        // Reregister
        public const string ReregisterProvider = "ReregisterProvider";
        public const string SubmitReregisterProvider = "SubmitReregisterProvider";
        public const string ReregisterCore = "ReregisterCore";
        public const string SubmitReregisterCore = "SubmitReregisterCore";
        public const string ReregisterCannotSelectSameCore = "ReregisterCannotSelectSameCore";
        public const string ReregisterSpecialismQuestion = "ReregisterSpecialismQuestion";
        public const string SubmitReregisterSpecialismQuestion = "SubmitReregisterSpecialismQuestion";
        public const string ReregisterSpecialisms = "ReregisterSpecialisms";
        public const string SubmitReregisterSpecialisms = "SubmitReregisterSpecialisms";
        public const string ReregisterAcademicYear = "ReregisterAcademicYear";
        public const string SubmitReregisterAcademicYear = "SubmitReregisterAcademicYear";
        public const string ReregisterCheckAndSubmit = "ReregisterCheckAndSubmit";
        public const string SubmitReregisterCheckAndSubmit = "SubmitReregisterCheckAndSubmit";
        public const string ReregistrationConfirmation = "ReregistrationConfirmation";

        // Search Registration
        public const string SearchRegistration = "SearchRegistration";
        public const string SubmitSearchRegistration = "SubmitSearchRegistration";
        public const string SearchRegistrationNotFound = "SearchRegistrationNotFound";
        public const string RegistrationDetails = "RegistrationDetails";

        // Cancel Registration
        public const string DeleteRegistration = "DeleteRegistration";
        public const string SubmitDeleteRegistration = "SubmitDeleteRegistration";
        public const string RegistrationCancelledConfirmation = "RegistrationCancelledConfirmation";
        public const string RegistrationCannotBeDeleted = "RegistrationCannotBeDeleted";

        // Assessment Entries
        public const string AssessmentDashboard = "AssessmentDashboard";
        public const string UploadAssessmentsFile = "UploadAssessmentsFile";
        public const string SubmitUploadAssessmentsFile = "SubmitUploadAssessmentsFile";
        public const string AssessmentsUploadSuccessful = "AssessmentsUploadSuccessful";
        public const string AssessmentsUploadUnsuccessful = "AssessmentsUploadUnsuccessful";
        public const string ProblemWithAssessmentsUpload = "ProblemWithAssessmentsUpload";
        public const string DownloadAssessmentErrors = "DownloadAssessmentErrors";
        public const string AddCoreAssessmentEntry = "AddCoreAssessmentEntry";
        public const string EntrySeries = "SubmitAddCoreAssessmentEntry";
        public const string AssessmentEntryAddedConfirmation = "AssessmentEntryAddedConfirmation";
        public const string RemoveCoreAssessmentEntry = "RemoveCoreAssessmentEntry";
        public const string SubmitRemoveCoreAssessmentEntry = "SubmitRemoveCoreAssessmentEntry";
        public const string AssessmentEntryRemovedConfirmation = "AssessmentEntryRemovedConfirmation";

        // Search Assessment Entries
        public const string SearchAssessments = "SearchAssessments";
        public const string SubmitSearchAssessments = "SubmitSearchAssessments";
        public const string SearchAssessmentsNotFound = "SearchAssessmentsNotFound";
        public const string AssessmentDetails = "AssessmentDetails";
        public const string AssessmentWithdrawnDetails = "AssessmentWithdrawnDetails";

        // Results
        public const string ResultsDashboard = "ResultsDashboard";
        public const string UploadResultsFile = "UploadResultsFile";
        public const string SubmitUploadResultsFile = "SubmitUploadResultsFile";
        public const string ProblemWithResultsUpload = "ProblemWithResultsUpload";
        public const string ResultsUploadSuccessful = "ResultsUploadSuccessful";
        public const string ResultsUploadUnsuccessful = "ResultsUploadUnsuccessful";
        public const string DownloadResultErrors = "DownloadResultErrors";
        public const string ChangeCoreResult = "ChangeCoreResult";
        public const string SubmitChangeCoreResult = "SubmitChangeCoreResult";
        public const string AddCoreResult = "AddCoreResult";
        public const string SubmitAddCoreResult = "SubmitAddCoreResult";
        public const string AddResultConfirmation = "AddResultConfirmation";
        public const string ChangeResultConfirmation = "ChangeResultConfirmation";

        // Search Results
        public const string SearchResults = "SearchResults";
        public const string SubmitSearchResults = "SubmitSearchResults";
        public const string SearchResultsNotFound = "SearchResultsNotFound";
        public const string ResultWithdrawnDetails = "ResultWithdrawnDetails";
        public const string ResultDetails = "ResultDetails";

        // Error
        public const string PageNotFound = "PageNotFound";
        public const string ServiceAccessDenied = "ServiceAccessDenied";
        public const string AccessDeniedWrongRole = "AccessDeniedWrongRole";
        public const string ProblemWithService = "ProblemWithService";
        public const string Error = "Error";
        public const string VirusUploaded = "VirusUploaded";

        // Help
        public const string Cookies = "Cookies";
        public const string Contact = "Contact";
        public const string CookieDetails = "CookieDetails";
        public const string PrivacyPolicy = "PrivacyPolicy";
        public const string TermsAndConditions = "TermsAndConditions";
        public const string UserGuide = "UserGuide";

        // Document
        public const string RegistrationDataFormatAndRulesGuide = "RegistrationDataFormatAndRulesGuide";
        public const string DownloadRegistrationDataFormatAndRulesGuide = "DownloadRegistrationDataFormatAndRulesGuide";
        public const string DownloadAssessmentEntriesDataFormatAndRulesGuide = "DownloadAssessmentEntriesDataFormatAndRulesGuide";
        public const string DownloadResultsDataFormatAndRulesGuide = "DownloadResultsDataFormatAndRulesGuide";

        public const string TlevelDataFormatAndRulesGuide = "TlevelDataFormatAndRulesGuide";

        // Timeout
        public const string ActiveDuration = "ActiveDuration";
        public const string RenewSessionActivity = "RenewSessionActivity";
        public const string ActivityTimeout = "ActivityTimeout";
        public const string Timeout = "Timeout";

        # region TrainingProvider specific constants

        public const string ManageLearnerRecordsDashboard = "ManageLearnerRecordsDashboard";
        public const string AddLearnerRecord = "AddLearnerRecord";
        public const string EnterUniqueLearnerNumber = "EnterUniqueLearnerNumber";
        public const string SubmitEnterUniqueLearnerNumber = "SubmitEnterUniqueLearnerNumber";
        public const string EnterUniqueLearnerNumberNotFound = "EnterUniqueLearnerNumberNotFound";
        public const string EnterUniqueLearnerNumberAddedAlready = "EnterUniqueLearnerNumberAddedAlready";
        public const string AddEnglishAndMathsQuestion = "AddEnglishAndMathsQuestion";
        public const string SubmitAddEnglishAndMathsQuestion = "SubmitAddEnglishAndMathsQuestion";
        public const string AddEnglishAndMathsLrsQuestion = "AddEnglishAndMathsLrsQuestion";
        public const string SubmitAddEnglishAndMathsLrsQuestion = "SubmitAddEnglishAndMathsLrsQuestion";
        public const string AddIndustryPlacementQuestion = "AddIndustryPlacementQuestion";
        public const string SubmitIndustryPlacementQuestion = "SubmitIndustryPlacementQuestion";
        public const string AddLearnerRecordCheckAndSubmit = "AddLearnerRecordCheckAndSubmit";
        public const string SubmitLearnerRecordCheckAndSubmit = "SubmitLearnerRecordCheckAndSubmit";
        public const string AddLearnerRecordCancel = "AddLearnerRecordCancel";
        public const string SubmitLearnerRecordCancel = "SubmitLearnerRecordCancel";
        public const string LearnerRecordAddedConfirmation = "LearnerRecordAddedConfirmation";
        public const string AddEnglishAndMathsSendDataConfirmation = "AddEnglishAndMathsSendDataConfirmation";

        public const string UpdateLearnerRecord = "UpdateLearnerRecord";
        public const string SearchLearnerRecord = "SearchLearnerRecord";
        public const string SubmitSearchLearnerRecord = "SubmitSearchLearnerRecord";
        public const string SearchLearnerRecordNotAdded = "SearchLearnerRecordNotAdded";
        public const string SearchLearnerRecordNotFound = "SearchLearnerRecordNotFound";
        public const string LearnerRecordDetails = "LearnerRecordDetails";
        public const string QueryEnglishAndMathsStatus = "QueryEnglishAndMathsStatus";
        public const string UpdateIndustryPlacementQuestion = "UpdateIndustryPlacementQuestion";
        public const string SubmitUpdateIndustryPlacementQuestion = "SubmitUpdateIndustryPlacementQuestion";
        public const string IndustryPlacementUpdatedConfirmation = "IndustryPlacementUpdatedConfirmation";
        public const string UpdateEnglisAndMathsAchievement = "UpdateEnglisAndMathsAchievement";
        public const string SubmitUpdateEnglisAndMathsAchievement = "SubmitUpdateEnglisAndMathsAchievement";
        public const string EnglishAndMathsAchievementUpdatedConfirmation = "EnglishAndMathsAchievementUpdatedConfirmation";
        public const string ManagePostalAddress = "ManagePostalAddress";
        public const string AddAddress = "AddAddress";
        public const string AddAddressPostcode = "AddAddressPostcode";
        public const string SubmitAddAddressPostcode = "SubmitAddAddressPostcode";
        public const string AddAddressManually = "AddAddressManually";
        public const string AddPostalAddressManual = "AddPostalAddressManual";
        public const string SubmitAddPostalAddressManual = "SubmitAddPostalAddressManual";
        public const string AddAddressSelect = "AddAddressSelect";
        public const string SubmitAddAddressSelect = "SubmitAddAddressSelect";
        public const string AddAddressCheckAndSubmit = "AddAddressCheckAndSubmit";
        public const string SubmitAddAddressCheckAndSubmit = "SubmitAddAddressCheckAndSubmit";
        public const string AddAddressConfirmation = "AddAddressConfirmation";
        public const string AddAddressCancel = "AddAddressCancel";
        public const string SubmitAddAddressCancel = "SubmitAddAddressCancel";
        public const string AddAddressNotFound = "AddAddressNotFound";
        public const string SubmitAddAddressNotFound = "SubmitAddAddressNotFound";
        #endregion

        #region Statement of achievement

        public const string RequestStatementOfAchievement = "RequestStatementOfAchievement";
        public const string StatementsOfAchievementNotAvailable = "StatementsOfAchievementNotAvailable";
        public const string PostalAddressMissing = "PostalAddressMissing";
        public const string RequestSoaUniqueLearnerNumber = "RequestSoaUniqueLearnerNumber";
        public const string SubmitRequestSoaUniqueLearnerNumber = "SubmitRequestSoaUniqueLearnerNumber";
        public const string RequestSoaUlnNotFound = "RequestSoaUlnNotFound";

        #endregion
    }
}
