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
        public const string TlevelsDashboard = "TlevelsDashboard";
        public const string ReviewTlevels = "ReviewTlevels";
        public const string YourTlevels = "YourTlevels";
        public const string ConfirmedTlevels = "ConfirmedTlevels";
        public const string NoConfirmedTlevels = "NoConfirmedTlevels";
        public const string QueriedTlevels = "QueriedTlevels";
        public const string NoQueriedTlevels = "NoQueriedTlevels";
        public const string TlevelDetails = "TlevelDetails";
        public const string TlevelConfirmedDetails = "TlevelConfirmedDetails";
        public const string TlevelQueriedDetails = "TlevelQueriedDetails";
        public const string SelectTlevel = "SelectTlevel";
        public const string SelectTlevelSubmit = "SelectTlevelSubmit";
        public const string ReviewTlevelDetails = "ReviewTlevelDetails";
        public const string ConfirmTlevel = "ConfirmTlevel";
        public const string TlevelDetailsConfirmed = "TlevelDetailsConfirmed";
        public const string AllTlevelsReviewedSuccess = "AllTlevelsReviewedSuccess";
        public const string TlevelDetailsQueriedConfirmation = "TlevelDetailsQueriedConfirmation";
        public const string QueryTlevelDetails = "QueryTlevelDetails";  // QueryTlevelDetails_Get
        public const string SubmitTlevelIssue = "SubmitTlevelIssue";  // QueryTlevelDetails_Post
        public const string QueryTlevelSent = "QueryTlevelSent";
        public const string QueryServiceProblem = "QueryServiceProblem";
        public const string AllTlevelsReviewed = "AllTlevelsReviewed";

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
        public const string ChangeSpecialismRestriction = "ChangeSpecialismRestriction";
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
        public const string RegistrationCannotBeWithdrawn = "RegistrationCannotBeWithdrawn";

        // Data Export
        public const string RegistrationsNoRecordsFound = "RegistrationsNoRecordsFound";

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
        public const string RemoveCoreAssessmentEntry = "RemoveCoreAssessmentEntry";
        public const string SubmitRemoveCoreAssessmentEntry = "SubmitRemoveCoreAssessmentEntry";
        public const string AddSpecialismAssessmentEntry = "AddSpecialismAssessmentEntry";
        public const string SubmitAddSpecialismAssessmentEntry = "SubmitAddSpecialismAssessmentEntry";
        public const string RemoveSpecialismAssessmentEntries = "RemoveSpecialismAssessmentEntries";
        public const string SubmitRemoveSpecialismAssessmentEntries = "SubmitRemoveSpecialismAssessmentEntries";

        // Data Export
        public const string RegistrationsGeneratingDownload = "RegistrationsGeneratingDownload";
        public const string SubmitRegistrationsGeneratingDownload = "SubmitRegistrationsGeneratingDownload ";
        public const string AssessmentsGeneratingDownload = "AssessmentsGeneratingDownload";
        public const string SubmitAssessmentsGeneratingDownload = "SubmitAssessmentsGeneratingDownload";
        public const string AssessmentsNoRecordsFound = "AssessmentsNoRecordsFound";
        public const string RegistrationsDownloadData = "RegistrationsDownloadData";
        public const string RegistrationsDownloadDataLink = "RegistrationsDownloadDataLink";
        public const string AssessmentsDownloadData = "AssessmentsDownloadData";
        public const string AssessmentsDownloadDataLink = "AssessmentsDownloadDataLink";
        public const string ResultsGeneratingDownload = "ResultsGeneratingDownload";
        public const string SubmitResultsGeneratingDownload = "SubmitResultsGeneratingDownload";
        public const string ResultsDownloadData = "ResultsDownloadData";
        public const string ResultsDownloadDataLink = "ResultsDownloadDataLink";
        public const string ResultsNoRecordsFound = "ResultsNoRecordsFound";

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
        public const string AddSpecialismResult = "AddSpecialismResult";
        public const string SubmitAddSpecialismResult = "SubmitAddSpecialismResult";
        public const string ChangeSpecialismResult = "ChangeSpecialismResult";
        public const string SubmitChangeSpecialismResult = "SubmitChangeSpecialismResult";

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
        public const string ServiceUnavailable = "ServiceUnavailable";

        // Document
        public const string RegistrationDataFormatAndRulesGuide = "RegistrationDataFormatAndRulesGuide";
        public const string DownloadRegistrationDataFormatAndRulesGuide = "DownloadRegistrationDataFormatAndRulesGuide";
        public const string DownloadAssessmentEntriesDataFormatAndRulesGuide = "DownloadAssessmentEntriesDataFormatAndRulesGuide";
        public const string DownloadResultsDataFormatAndRulesGuide = "DownloadResultsDataFormatAndRulesGuide";
        public const string DownloadIndustryPlacementDataFormatAndRulesGuide = "DownloadIndustryPlacementDataFormatAndRulesGuide";

        public const string TlevelDataFormatAndRulesGuide = "TlevelDataFormatAndRulesGuide";

        // Timeout
        public const string ActiveDuration = "ActiveDuration";
        public const string RenewSessionActivity = "RenewSessionActivity";
        public const string ActivityTimeout = "ActivityTimeout";
        public const string Timeout = "Timeout";

        # region TrainingProvider specific constants

        public const string ProviderGuidance = "ProviderGuidance";
        public const string ProviderTimeline = "ProviderTimeline";

        public const string AddMathsStatus = "AddMathsStatus";
        public const string SubmitAddMathsStatus = "SubmitAddMathsStatus";
        public const string AddEnglishStatus = "AddEnglishStatus";
        public const string SubmitAddEnglishStatus = "SubmitAddEnglishStatus";

        public const string AddWithdrawnStatus = "AddWithdrawnStatus";
        public const string SubmitWithdrawnStatus = "SubmitWithdrawnStatus";
        public const string ChangeWithdrawnStatusHaveYouToldAwardingOrganisation = "ChangeWithdrawnStatusHaveYouToldAwardingOrganisation";
        public const string SubmitChangeWithdrawnStatusHaveYouToldAwardingOrganisation = "SubmitChangeWithdrawnStatusHaveYouToldAwardingOrganisation";
        public const string WithdrawLearnerAOMessage = "WithdrawLearnerAOMessage";
        public const string SubmitWithdrawLearnerAOMessage = "SubmitWithdrawLearnerAOMessage";

        public const string ChangeBackToActiveStatus = "ChangeBackToActiveStatus";
        public const string SubmitChangeBackToActiveStatus = "SubmitChangeBackToActiveStatus";
        public const string ChangeBackToActiveStatusHaveYouToldAwardingOrganisation = "ChangeBackToActiveStatusHaveYouToldAwardingOrganisation";
        public const string SubmitChangeBackToActiveStatusHaveYouToldAwardingOrganisation = "SubmitChangeBackToActiveStatusHaveYouToldAwardingOrganisation";
        public const string ChangeBackToActiveAOMessage = "ChangeBackToActiveAOMessage";
        public const string SubmitChangeBackToActiveAOMessage = "SubmitChangeBackToActiveAOMessage";

        public const string SearchLearnerDetails = "SearchLearnerDetails";
        public const string SubmitSearchLearnerDetails = "SubmitSearchLearnerDetails";
        public const string SubmitSearchLearnerApplyFilters = "SubmitSearchLearnerApplyFilters";
        public const string SearchLearnerClearFilters = "SearchLearnerClearFilters";
        public const string SearchLearnerClearKey = "SearchLearnerClearKey";

        //public const string GetRegisteredLearners = "ProviderRegisteredLearners";
        //public const string SubmitGetRegisteredLearners = "SubmitGetRegisteredLearners";
        public const string SubmitProviderRegisteredLearners = "SubmitProviderRegisteredLearners";
        public const string SearchLearnerRecord = "SearchLearnerRecord";
        public const string SubmitSearchLearnerRecord = "SubmitSearchLearnerRecord";
        public const string SearchLearnerRecordNotFound = "SearchLearnerRecordNotFound";
        public const string LearnerRecordDetails = "LearnerRecordDetails";
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
        public const string RequestReplacementDocument = "RequestReplacementDocument";
        public const string SubmitRequestReplacementDocument = "SubmitRequestReplacementDocument";
        #endregion

        #region Statement of achievement

        public const string StatementsOfAchievementNotAvailable = "StatementsOfAchievementNotAvailable";
        public const string PostalAddressMissing = "PostalAddressMissing";
        public const string RequestSoaUniqueLearnerNumber = "RequestSoaUniqueLearnerNumber";
        public const string SubmitRequestSoaUniqueLearnerNumber = "SubmitRequestSoaUniqueLearnerNumber";
        public const string RequestSoaUlnNotFound = "RequestSoaUlnNotFound";
        public const string RequestSoaUlnNotWithdrawn = "RequestSoaUlnNotWithdrawn";
        public const string RequestSoaNotAvailableNoIpStatus = "RequestSoaNotAvailableNoIpStatus";
        public const string RequestSoaNotAvailableNoResults = "RequestSoaNotAvailableNoResults";
        public const string RequestSoaCheckAndSubmit = "RequestSoaCheckAndSubmit";
        public const string SubmitRequestSoaCheckAndSubmit = "SubmitRequestSoaCheckAndSubmit";
        public const string RequestSoaConfirmation = "RequestSoaConfirmation";
        public const string RequestSoaCancel = "RequestSoaCancel";
        public const string SubmitRequestSoaCancel = "SubmitRequestSoaCancel";
        public const string RequestSoaChangeComponentAchievements = "RequestSoaChangeComponentAchievements";
        public const string RequestSoaChangePostalAddress = "RequestSoaChangePostalAddress";
        public const string RequestSoaAlreadySubmitted = "RequestSoaAlreadySubmitted";

        #endregion

        #region Post Results Service

        public const string StartReviewsAndAppeals = "StartReviewsAndAppeals";
        public const string PrsSearchLearner = "PrsSearchLearner";
        public const string SubmitPrsSearchLearner = "SubmitPrsSearchLearner";
        public const string PrsUlnNotFound = "PrsUlnNotFound";
        public const string PrsUlnWithdrawn = "PrsUlnWithdrawn";
        public const string PrsLearnerDetails = "PrsLearnerDetails";
        public const string PrsAddRomm = "PrsAddRomm";
        public const string SubmitPrsAddRomm = "SubmitPrsAddRomm";
        public const string PrsAddRommOutcome = "PrsAddRommOutcome";
        public const string SubmitPrsAddRommOutcome = "SubmitPrsAddRommOutcome";
        public const string PrsAddRommOutcomeKnown = "PrsAddRommOutcomeKnown";
        public const string SubmitPrsAddRommOutcomeKnown = "SubmitPrsAddRommOutcomeKnown";
        public const string PrsRommGradeChange = "PrsRommGradeChange";
        public const string SubmitPrsRommGradeChange = "SubmitPrsRommGradeChange";
        public const string PrsRommCheckAndSubmit = "PrsRommCheckAndSubmit";
        public const string SubmitPrsRommCheckAndSubmit = "SubmitPrsRommCheckAndSubmit";
        public const string PrsCancelRommUpdate = "PrsCancelRommUpdate";
        public const string SubmitPrsCancelRommUpdate = "SubmitPrsCancelRommUpdate";
        public const string PrsAddAppeal = "PrsAddAppeal";
        public const string SubmitPrsAddAppeal = "SubmitPrsAddAppeal";
        public const string PrsAddAppealOutcome = "PrsAddAppealOutcome";
        public const string SubmitPrsAddAppealOutcome = "SubmitPrsAddAppealOutcome";
        public const string PrsAddAppealOutcomeKnown = "PrsAddAppealOutcomeKnown";
        public const string SubmitPrsAddAppealOutcomeKnown = "SubmitPrsAddAppealOutcomeKnown";
        public const string PrsAppealGradeChange = "PrsAppealGradeChange";
        public const string SubmitPrsAppealGradeChange = "SubmitPrsAppealGradeChange";
        public const string PrsAppealCheckAndSubmit = "PrsAppealCheckAndSubmit";
        public const string SubmitPrsAppealCheckAndSubmit = "SubmitPrsAppealCheckAndSubmit";
        public const string PrsCancelAppealUpdate = "PrsCancelAppealUpdate";
        public const string SubmitPrsCancelAppealUpdate = "SubmitPrsCancelAppealUpdate";

        public const string PrsNoResults = "PrsNoResults";
        public const string PrsGradeChangeRequest = "PrsGradeChangeRequest";
        public const string SubmitPrsGradeChangeRequest = "SubmitPrsGradeChangeRequest";
        public const string PrsCancelGradeChangeRequest = "PrsCancelGradeChangeRequest";
        public const string SubmitPrsCancelGradeChangeRequest = "SubmitPrsCancelGradeChangeRequest";
        public const string PrsGradeChangeRequestConfirmation = "PrsGradeChangeRequestConfirmation";
        public const string SubmitPrsGradeChangeRequestConfirmation = "SubmitPrsGradeChangeRequestConfirmation";

        #endregion

        #region Industry Placement

        public const string AddIndustryPlacement = "AddIndustryPlacement";
        public const string ChangeIndustryPlacement = "ChangeIndustryPlacement";

        public const string IpCompletion = "IpCompletion";
        public const string IpCompletionChange = "IpCompletionChange";
        public const string SubmitIpCompletion = "SubmitIpCompletion";

        // Special Consideration
        public const string IpSpecialConsiderationHours = "IpSpecialConsiderationHours";
        public const string SubmitIpSpecialConsiderationHours = "SubmitIpSpecialConsiderationHours";
        public const string IpSpecialConsiderationReasons = "IpSpecialConsiderationReasons";
        public const string SubmitIpSpecialConsiderationReasons = "SubmitIpSpecialConsiderationReasons";

        public const string IpCheckAndSubmit = "IpCheckAndSubmit";
        public const string SubmitIpCheckAndSubmit = "SubmitIpCheckAndSubmit";
        public const string IpCheckAndSubmitCancel = "IpCheckAndSubmitCancel";

        #endregion

        #region Download Tlevel Results
        public const string DownloadOverallResultsPage = "DownloadOverallResultsPage";
        public const string DownloadOverallResultsFile = "DownloadOverallResultsFile";
        #endregion

        #region Industry Placement Import

        public const string UploadIndustryPlacementsFile = "UploadIndustryPlacementsFile";
        public const string SubmitUploadIndustryPlacementsFile = "SubmitUploadIndustryPlacementsFile";
        public const string IndustryPlacementsUploadSuccessful = "IndustryPlacementsUploadSuccessful";
        public const string IndustryPlacementsUploadUnsuccessful = "IndustryPlacementsUploadUnsuccessful";
        public const string DownloadIndustryPlacementErrors = "DownloadIndustryPlacementErrors";
        #endregion

        #region Admin Dashboard

        public const string AdminHome = "AdminHome";
        public const string AdminSearchLearnersRecords = "AdminSearchLearnersRecords";
        public const string AdminSearchLearnersRecordsClear = "AdminSearchLearnersRecordsClear";
        public const string SubmitAdminSearchLearnerRecordsApplySearchKey = "SubmitAdminSearchLearnerRecordsApplySearchKey";
        public const string SubmitAdminSearchLearnerClearKey = "SubmitAdminSearchLearnerClearKey";
        public const string SubmitAdminSearchLearnerRecordsApplyFilters = "SubmitAdminSearchLearnerRecordsApplyFilters";
        public const string SubmitAdminSearchLearnerClearFilters = "SubmitAdminSearchLearnerClearFilters";
        public const string AdminLearnerRecord = "AdminLearnerRecord";
        public const string ChangeStartYear = "ChangeStartYear";
        public const string SubmitChangeStartYear = "SubmitChangeStartYear";
        public const string ReviewChangeStartYear = "ReviewChangeStartYear";
        public const string SubmitReviewChangeStartYear = "SubmitReviewChangeStartYear";
        public const string AdminChangeIndustryPlacementClear = "AdminChangeIndustryPlacementClear";
        public const string AdminChangeIndustryPlacement = "AdminChangeIndustryPlacement";
        public const string AdminSubmitChangeIndustryPlacement = "AdminSubmitChangeIndustryPlacement";
        public const string AdminIndustryPlacementSpecialConsiderationHours = "AdminIndustryPlacementSpecialConsiderationHours";
        public const string SubmitAdminIndustryPlacementSpecialConsiderationHours = "SubmitAdminIndustryPlacementSpecialConsiderationHours";
        public const string AdminIndustryPlacementSpecialConsiderationReasons = "AdminIndustryPlacementSpecialConsiderationReasons";
        public const string SubmitAdminIndustryPlacementSpecialConsiderationReasons = "SubmitAdminIndustryPlacementSpecialConsiderationReasons";
        public const string AdminReviewChangesIndustryPlacement = "AdminReviewChangesIndustryPlacement";
        public const string SubmitReviewChangesIndustryPlacement = "SubmitReviewChangesIndustryPlacement";
        public const string RemoveAssessmentEntryCoreClear = "RemoveAssessmentEntryCoreClear";
        public const string RemoveAssessmentEntryCore = "RemoveAssessmentEntryCore";
        public const string SubmitRemoveAssessmentEntryCore = "SubmitRemoveAssessmentEntryCore";
        public const string RemoveAssessmentSpecialismEntryClear = "RemoveAssessmentSpecialismEntryClear";
        public const string RemoveAssessmentSpecialismEntry = "RemoveAssessmentSpecialismEntry";
        public const string SubmitRemoveAssessmentSpecialismEntry = "SubmitRemoveAssessmentSpecialismEntry";
        public const string AdminCoreComponentAssessmentEntry = "AdminCoreComponentAssessmentEntry";
        public const string SubmitCoreComponentAssessmentEntry = "SubmitCoreComponentAssessmentEntry";
        public const string AdminOccupationalSpecialisAssessmentEntry = "AdminOccupationalSpecialisAssessmentEntry";
        public const string SubmitOccupationalSpecialisAssessmentEntry = "SubmitOccupationalSpecialisAssessmentEntry";
       
        public const string AdminReviewChangesCoreAssessmentEntry = "AdminReviewChangesCoreAssessmentEntry";
        public const string SubmitReviewChangesCoreAssessmentEntry = "SubmitReviewChangesCoreAssessmentEntry";
        public const string AdminReviewChangesSpecialismAssessmentEntry = "AdminReviewChangesSpecialismAssessmentEntry";
        public const string SubmitReviewChangesSpecialismAssessmentEntry = "SubmitReviewChangesSpecialismAssessmentEntry";
        public const string AdminReviewRemoveCoreAssessmentEntry = "AdminReviewRemoveCoreAssessmentEntry";
        public const string SubmitReviewRemoveCoreAssessmentEntry = "SubmitReviewRemoveCoreAssessmentEntry";
        public const string AdminReviewRemoveSpecialismAssessmentEntry = "AdminReviewRemoveSpecialismAssessmentEntry";
        public const string SubmitReviewRemoveSpecialismAssessmentEntry = "SubmitReviewRemoveSpecialismAssessmentEntry";
        public const string AdminAddPathwayResultClear = "AdminAddPathwayResultClear";
        public const string AdminAddPathwayResult = "AdminAddPathwayResult";
        public const string SubmitAdminAddPathwayResult = "SubmitAdminAddPathwayResult";
        public const string AdminAddPathwayResultReviewChanges = "AdminAddPathwayResultReviewChanges";
        public const string SubmitAdminAddPathwayResultReviewChanges = "SubmitAdminAddPathwayResultReviewChanges";
        public const string AdminAddSpecialismResultClear = "AdminAddSpecialismResultClear";
        public const string AdminAddSpecialismResult = "AdminAddSpecialismResult";
        public const string SubmitAdminAddSpecialismResult = "SubmitAdminAddSpecialismResult";
        
        #endregion
    }
}