﻿namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class Constants
    {
        // Environment Constants
        public const string EnvironmentNameConfigKey = "EnvironmentName";
        public const string ConfigurationStorageConnectionStringConfigKey = "ConfigurationStorageConnectionString";
        public const string VersionConfigKey = "Version";
        public const string ServiceVersionConfigKey = "ServiceVersion";
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
        public const string DefaultPerformedBy = "System";
        public const string CreatedBy = "createdBy";

        // Controller Names
        public const string HomeController = "Home";
        public const string AccountController = "Account";
        public const string DashboardController = "Dashboard";
        public const string HelpController = "Help";
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
        public const string ChangeAcademicYearConfirmationViewModel = "RejoinRegistrationConfirmationViewModel";
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
        public const string AddLearnerRecordConfirmation = "AddLearnerRecordConfirmation";
        public const string SearchLearnerRecordViewModel = "SearchLearnerRecordViewModel";
        public const string IndustryPlacementUpdatedConfirmation = "IndustryPlacementUpdatedConfirmation";
        public const string AddAddressConfirmation = "AddAddressConfirmation";
        public const string RequestSoaConfirmation = "RequestSoaConfirmation";
        public const string UserSessionActivityId = "UserSessionActivityId";
        public const string AcademicYearTo = "AcademicYearTo";
        public const string IndustryPlacementStatusTo = "IndustryPlacementStatusTo";

        // Registration Data Index Constants
        public const int RegistrationProfileStartIndex = 100000;
        public const int RegistrationPathwayStartIndex = 200000;
        public const int RegistrationSpecialismsStartIndex = 300000;

        // Assessment Data Index Constants
        public const int PathwayAssessmentsStartIndex = 100000;
        public const int SpecialismAssessmentsStartIndex = 300000;

        // Results Data Index Constants
        public const int PathwayResultsStartIndex = 100000;

        public const int SpecialismResultsStartIndex = 300000;

        // Industry Placement Data Index Constants
        public const int IndustryPlacementStartIndex = 100000;

        // Overall Result Index Constants
        public const int OverallResultStartIndex = 100000;

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
        public const string RommOutcomeTypeId = "outcomeTypeId";
        public const string RommOutcomeKnownTypeId = "outcomeKnownTypeId";
        public const string IsRommOutcomeJourney = "isRommOutcomeJourney";
        public const string ComponentType = "componentType";
        public const string AppealOutcomeKnownTypeId = "outcomeKnownTypeId";
        public const string IsAppealOutcomeJourney = "isAppealOutcomeJourney";
        public const string AcademicYear = "academicYear";
        public const string RegistrationPathwayId = "registrationPathwayId";
        public const string SpecialismsId = "specialismsId";
        public const string ChangeLogId = "changeLogId";
        public const string Type = "type";
        public const string NotificationId = "notificationId";

        // Assessments
        public const int AssessmentEndInYears = 4;
        public const int CoreAssessmentStartInYears = 0;
        public const int SpecialismAssessmentStartInYears = 1;
        public const string SpecialismAssessmentIds = "specialismAssessmentIds";
        public const int AdminAssessmentEntryLimit = 2;
        public const string AdminValidAssessmentSeries = "ValidAssessmentSeries";
        public const string AdminSpecialismAssessmentId = "AdminSpecialismAssessmentId";

        // ChangeLog
        public const string PathwayResultId = "PathwayResultId";
        public const string PathwayAssessmentId = "PathwayAssessmentId";
        public const string SpecialismResultId = "SpecialismResultId";


        public const int MaxFileSizeInMb = 5;

        // Industry placements
        public const string MultipleEmployer = "Multiple employer";
        public const string EmployerLedActivities = "Employer led activities/projects";
        public const string BlendedPlacements = "Blended placements";

        // Printing Constants
        public const string Completed = "Completed";
        public const string NotCompleted = "Not completed";
        public const string IndustryPlacementCompleted = "Completed";
        public const string IndustryPlacementNotCompleted = "Not completed";
        public const string EnglishAndMathsMet = "Met";
        public const string EnglishAndMathsNotMet = "Not met";
        public const string TLevelIn = "T Level in ";
        public const string MathsAndEnglishAchievedText = "The named recipient has also achieved a qualification at Level 2 in both maths and English.";
        public const string MathsAchievedText = "The named recipient has also achieved a qualification at Level 2 in maths.";
        public const string EnglishAchievedText = "The named recipient has also achieved a qualification at Level 2 in English.";
        public const string Met = "Met";
        public const string NotMet = "Not met";

        // Other constants
        public const string NoBorderBottomCssClassName = "tl-no-border-bottom";
        public const string BlueTagClassName = "govuk-tag--blue";
        public const string PurpleTagClassName = "govuk-tag--purple";
        public const string RedTagClassName = "govuk-tag--red";
        public const string GreenTagClassName = "govuk-tag govuk-tag--green";
        public const string TagFloatRightClassName = "tag-float-right";
        public const string PostcodeValidationRegex = "^(([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))(\\s?)?[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
        public const string UlnValidationRegex = @"^\d{10}$";
        public const string IpSpecialConsiderationHoursRegex = @"^([1-9][0-9]{0,2})$";
        public const string PipeSeperator = "|";
        public const string AndSeperator = " and ";
        public const string CommaSeparator = ",";
        public const string NotReceived = "NR";
        public const string NotSpecified = "NotSpecified";

        // Function Name Constants
        public const string FetchLearnerGender = "FetchLearnerGender";
        public const string VerifyLearnerAndFetchLearningEvents = "VerifyLearnerAndFetchLearningEvents";
        public const string SubmitCertificatePrintingRequest = "SubmitCertificatePrintingRequest";
        public const string GenerateCertificatePrintingBatches = "GenerateCertificatePrintingBatches";
        public const string FetchCertificatePrintingBatchSummary = "FetchCertificatePrintingBatchSummary";
        public const string FetchCertificatePrintingTrackBatch = "FetchCertificatePrintingTrackBatch";
        public const string UcasTransferEntries = "UcasTransferEntries";
        public const string UcasTransferResults = "UcasTransferResults";
        public const string UcasTransferAmendments = "UcasTransferAmendments";
        public const string OverallResultCalculation = "OverallResultCalculation";
        public const string IndustryPlacementExtract = "IndustryPlacementExtract";
        public const string IndustryPlacementExtractsFolder = "extracts";
        public const string AnalystOverallResultExtract = "AnalystOverallResultExtract";
        public const string AnalystCoreResultExtract = "AnalystCoreResultExtract";
        public const string SpecialismRommExtract = "SpecialismRommExtract";
        public const string CertificateTrackingExtract = "CertificateTrackingExtract";
        public const string HealthCheck = "HealthCheck";
        public const string AnalystOverallResultExtractsFolder = "extracts";
        public const string AnalystCoreResultExtractsFolder = "extracts";
        public const string SpecialismRommFolder = "extracts";
        public const string SpecialismRomm = "SpecialismRomm";
        public const string CoreRommExtract = "CoreRommExtract";
        public const string CoreRommFolder = "extracts";
        public const string ProviderAddressExtract = "ProviderAddressExtract";
        public const string CertificateTrackingExtractsFolder = "extracts";

        // File Extensions
        public const string FileExtensionTxt = "txt";

        public const int OverallResultDefaultNoOfAcademicYearsToProcess = 4;
        public const int OverallResultDefaultBatchSize = 5000;
        public const int CertificatePrintingDefaultProvidersBatchSize = 50;

        public const string PathwayComponentGradeUnclassifiedCode = "PCG7";
        public const string PathwayComponentGradeQpendingResultCode = "PCG8";
        public const string PathwayComponentGradeXNoResultCode = "PCG9";

        public const string SpecialismComponentGradeUnclassifiedCode = "SCG4";
        public const string SpecialismComponentGradeQpendingResultCode = "SCG5";
        public const string SpecialismComponentGradeXNoResultCode = "SCG6";

        public const string OverallResultPartialAchievementCode = "OR5";
        public const string OverallResultUnclassifiedCode = "OR6";
        public const string OverallResultXNoResultCode = "OR7";
        public const string OverallResultQpendingCode = "OR8";

        public const string WJECEduqasEnglishLanguageQualificationCode = "601/4505/5";

        // Content types
        public const string TextCsv = "text/csv";
        public const string TextXlsx = "text/xlsx";

        // Mapper keys
        public const string CertificateRerequestDays = "certificateRerequestDays";
    }
}