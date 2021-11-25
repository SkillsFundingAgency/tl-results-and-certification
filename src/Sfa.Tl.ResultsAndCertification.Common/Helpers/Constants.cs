namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class Constants
    {
        // Environment Constants
        public const string EnvironmentNameConfigKey = "EnvironmentName";
        public const string ConfigurationStorageConnectionStringConfigKey = "ConfigurationStorageConnectionString";
        public const string VersionConfigKey = "Version";
        public const string ServiceNameConfigKey = "ServiceName";

        // LearnerRecordService(LRS) Constants
        public const string LearnerLearningEventsUserType = "LNR";
        public const string LearnerLearningEventsGetType = "FULL";
        public const string LearnerLearningEventsNotVerifiedResponseCode = "WSEC0208";
        public const string LrsLanguage = "ENG";
        public const string LrsDateFormat = "yyyy-MM-dd";
        public const string LearnerByULNFindType = "FUL";
        public const string LearnerByUlnExactMatchResponseCode = "WSRC0004";
        public const string LrsProfileId = "profileId";
        public const string LrsResponseCode = "responseCode";
        public const string FunctionPerformedBy = "System";

        // Controller Names
        public const string HomeController = "Home";
        public const string AccountController = "Account";
        public const string DashboardController = "Dashboard";
        public const string ErrorController = "Error";
        public const string TlevelController = "Tlevel";

        // TempData Key Constants        
        public const string IsRedirect = "IsRedirect";
        public const string IsBackToVerifyPage = "IsBackToVerifyPage";
        public const string TlevelConfirmation = "TlevelConfirmation";
        public const string FindProviderSearchCriteria = "FindProviderSearchCriteria";
        public const string ProviderTlevelsViewModel = "ProviderTlevelsViewModel";
        public const string ProviderTlevelDetailsViewModel = "ProviderTlevelDetailsViewModel";

        public const string UploadUnsuccessfulViewModel = "UploadUnsuccessfulViewModel";
        public const string UploadSuccessfulViewModel = "UploadSuccessfulViewModel";
        public const string UlnRegistrationNotFoundViewModel = "UlnRegistrationNotFoundViewModel";

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
        public const string RegistrationCannotBeDeletedViewModel = "RegistrationCannotBeDeletedViewModel";
        public const string AssessmentsUploadSuccessfulViewModel = "AssessmentsUploadSuccessfulViewModel";
        public const string AssessmentsSearchCriteria = "AssessmentsSearchCriteria";
        public const string SearchAssessmentsUlnNotFound = "SearchAssessmentsUlnNotFound";        
        public const string SearchResultsUlnNotFound = "SearchResultsUlnNotFound";
        public const string ResultWithdrawn = "ResultWithdrawn";
        public const string ResultsSearchCriteria = "ResultsSearchCriteria";
        public const string ResultsUploadSuccessfulViewModel = "ResultsUploadSuccessfulViewModel";
        public const string ResultConfirmationViewModel = "ResultConfirmationViewModel";
        public const string ChangeResultConfirmationViewModel = "ChangeResultConfirmationViewModel";
        public const string AddLearnerRecordConfirmation = "AddLearnerRecordConfirmation";
        public const string SearchLearnerRecordViewModel = "SearchLearnerRecordViewModel";
        public const string IndustryPlacementUpdatedConfirmation = "IndustryPlacementUpdatedConfirmation";
        public const string EnglishAndMathsAchievementUpdatedConfirmation = "EnglishAndMathsAchievementUpdatedConfirmation";
        public const string AddEnglishAndMathsSendDataConfirmation = "AddEnglishAndMathsSendDataConfirmation";
        public const string AddAddressConfirmation = "AddAddressConfirmation";
        public const string RequestSoaConfirmation = "RequestSoaConfirmation";
        public const string UserSessionActivityId = "UserSessionActivityId";        

        // Registration Data Index Constants
        public const int RegistrationProfileStartIndex = 100000;
        public const int RegistrationPathwayStartIndex = 200000;
        public const int RegistrationSpecialismsStartIndex = 300000;

        // Assessment Data Index Constants
        public const int PathwayAssessmentsStartIndex = 100000;
        public const int SpecialismAssessmentsStartIndex = 100000;

        // Results Data Index Constants
        public const int PathwayResultsStartIndex = 100000;

        // Industry Placement Data Index Constants
        public const int IndustryPlacementStartIndex = 100000;

        // Route Attributes
        public const string IsChangeMode = "isChangeMode";
        public const string ProfileId = "profileId";
        public const string IsBack = "isBack";
        public const string ChangeStatusId = "changeStatusId";
        public const string WithdrawBackLinkOptionId = "withdrawBackLinkOptionId";
        public const string AssessmentId = "assessmentId";
        public const string ResultId = "resultId";
        public const string PathwayId = "pathwayId";
        public const string IsFromSelectAddress = "isFromSelectAddress";
        public const string ShowPostcode = "showPostcode";
        public const string IsAddressMissing = "isFromAddressMissing";
        public const string PopulateUln = "populateUln";
        public const string AppealOutcomeTypeId = "outcomeTypeId";
        public const string IsResultJourney = "isResultJourney";

        // Assessments
        public const int AssessmentEndInYears = 4;
        public const int CoreAssessmentStartInYears = 0;
        public const int SpecialismAssessmentStartInYears = 1;

        public const int MaxFileSizeInMb = 5;

        // Printing Constants
        public const string Completed = "Completed";
        public const string NotCompleted = "Not completed";
        public const string IndustryPlacementCompleted = "Completed";
        public const string IndustryPlacementNotCompleted = "Not completed";
        public const string EnglishAndMathsMet = "Met";
        public const string EnglishAndMathsNotMet = "Not met";
        public const string TLevelIn = "T Level in ";


        // Other constants
        public const string NoBorderBottomCssClassName = "tl-no-border-bottom";
        public const string PurpleTagClassName = "govuk-tag--purple";
        public const string RedTagClassName = "govuk-tag--red";
        public const string TagFloatRightClassName = "tag-float-right";
        public const string PostcodeValidationRegex = "^(([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))(\\s?)?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
        public const string UlnValidationRegex = @"^\d{10}$";
        public const string PipeSeperator = "|";
        public const string AndSeperator = " and ";


        // Function Name Constants
        public const string SubmitCertificatePrintingRequest = "SubmitCertificatePrintingRequest";
        public const string FetchCertificatePrintingBatchSummary = "FetchCertificatePrintingBatchSummary";
        public const string FetchCertificatePrintingTrackBatch = "FetchCertificatePrintingTrackBatch";
        public const string UcasTransferEntries = "UcasTransferEntries";
        public const string UcasTransferResultEntries = "UcasTransferResultEntries";

        // File Extensions
        public const string FileExtensionTxt = "txt";
    }
}
