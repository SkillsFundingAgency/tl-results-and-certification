namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public static class ApiConstants
    {
        // Verify T Level Related Uri's
        public const string GetAllTLevelsUri = "/api/tlevel/GetAllTLevels/{0}";
        public const string GetTlevelsByStatus = "/api/tlevel/{0}/GetTlevelsByStatus/{1}";
        public const string TlevelDetailsUri = "/api/tlevel/{0}/TlevelDetails/{1}";
        public const string VerifyTlevelUri = "/api/tlevel/VerifyTlevel";
        public const string GetPathwaySpecialismsByPathwayLarIdAsyncUri = "/api/tlevel/{0}/GetPathwaySpecialisms/{1}";

        // Provider Related Uri's
        public const string IsAnyProviderSetupCompletedUri = "/api/provider/IsAnyProviderSetupCompleted/{0}";
        public const string FindProviderAsyncUri = "/api/provider/FindProvider/{0}/{1}";
        public const string GetSelectProviderTlevelsUri = "/api/provider/GetSelectProviderTlevels/{0}/{1}";
        public const string AddProviderTlevelsUri = "/api/provider/AddProviderTlevels";
        public const string GetAllProviderTlevelsAsyncUri = "/api/provider/GetAllProviderTlevels/{0}/{1}";
        public const string GetTqAoProviderDetailsAsyncUri = "/api/provider/GetTqAoProviderDetails/{0}";
        public const string GetTqProviderTlevelDetailsAsyncUri = "/api/provider/GetTqProviderTlevelDetails/{0}/{1}";
        public const string RemoveTqProviderTlevelAsyncUri = "/api/provider/RemoveProviderTlevel/{0}/{1}";
        public const string HasAnyTlevelSetupForProviderAsyncUri = "/api/provider/HasAnyTlevelSetupForProvider/{0}/{1}";
        public const string GetRegisteredProviderPathwayDetailsAsyncUri = "/api/provider/GetRegisteredProviderPathwayDetails/{0}/{1}";

        // Registrations Related Uri's
        public const string ProcessBulkRegistrationsUri = "/api/registration/ProcessBulkRegistrations";
        public const string ProcessBulkWithdrawalsUri = "/api/registration/ProcessBulkWithdrawals";
        public const string AddRegistrationUri = "/api/registration/AddRegistration";
        public const string FindUlnUri = "/api/registration/FindUln/{0}/{1}";
        public const string GetRegistrationDetailsUri = "/api/registration/GetRegistrationDetails/{0}/{1}/{2}";
        public const string DeleteRegistrationUri = "/api/registration/DeleteRegistration/{0}/{1}";
        public const string UpdateRegistrationUri = "/api/registration/UpdateRegistration";
        public const string WithdrawRegistrationUri = "/api/registration/WithdrawRegistration";
        public const string RejoinRegistrationUri = "/api/registration/RejoinRegistration";
        public const string ReregistrationUri = "/api/registration/Reregistration";
        public const string SetRegistrationAsPendingWithdrawalUri = "/api/registration/SetRegistrationAsPendingWithdrawal";
        public const string ReinstateRegistrationFromPendingWithdrawalUri = "/api/registration/ReinstateRegistrationFromPendingWithdrawal";
        public const string ProcessChangeAcademicYearUri = "/api/registration/ProcessChangeAcademicYear/{0}";

        // Assessments Related Uri's
        public const string ProcessBulkAssessmentsUri = "/api/assessment/ProcessBulkAssessments";
        public const string GetAvailableAssessmentSeriesUri = "/api/assessment/GetAvailableAssessmentSeries/{0}/{1}/{2}/{3}";
        public const string AddAssessmentEntryUri = "/api/assessment/AddAssessmentEntry";
        public const string GetActiveAssessmentEntryDetailsUri = "/api/assessment/GetActiveAssessmentEntryDetails/{0}/{1}/{2}";
        public const string GetActiveSpecialismAssessmentEntriesUri = "/api/assessment/GetActiveSpecialismAssessmentEntries/{0}/{1}";
        public const string RemoveAssessmentEntryUri = "/api/assessment/RemoveAssessmentEntry";
        public const string GetAssessmentSeriesDetailsUri = "/api/assessment/GetAssessmentSeries";

        // Assessment Series Related Uri's
        public const string GetResultCalculationAssessmentUri = "/api/assessmentseries/GetResultCalculationAssessment";

        // Results Related Uri's
        public const string ProcessBulkResultsUri = "/api/result/ProcessBulkResults";
        public const string GetResultDetailsUri = "/api/result/GetResultDetails/{0}/{1}/{2}";
        public const string AddResultUri = "/api/result/AddResult";
        public const string ChangeResultUri = "/api/result/ChangeResult";

        // DocumentUploadHistory Related Uri's
        public const string GetDocumentUploadHistoryDetailsAsyncUri = "/api/DocumentUploadHistory/GetDocumentUploadHistoryDetails/{0}/{1}";

        // TrainingProvider
        public const string SearchLearnerDetailsUri = "/api/trainingprovider/SearchLearnerDetails";
        public const string SearchLearnerFiltersUri = "/api/trainingprovider/GetSearchLearnerFilters/{0}";
        public const string FindLearnerRecordUri = "/api/trainingprovider/FindLearnerRecord/{0}/{1}";
        public const string GetLearnerRecordDetailsUri = "/api/trainingprovider/GetLearnerRecordDetails/{0}/{1}/{2}";
        public const string AddLearnerRecordUri = "/api/trainingprovider/AddLearnerRecord";
        public const string UpdateLearnerRecordUri = "/api/trainingprovider/UpdateLearnerRecord";
        public const string UpdateLearnerSubjectUri = "/api/trainingprovider/UpdateLearnerSubject";

        // Provider Replacement Document Uri's
        public const string CreateReplacementDocumentPrintingRequestUri = "/api/trainingprovider/CreateReplacementDocumentPrintingRequest";

        // Ordinance Survery Uri's
        public const string SearchAddressByPostcodeUri = "/postcode?postcode={0}&key={1}";
        public const string GetAddressByUprnUri = "/uprn?uprn={0}&key={1}";

        // ProviderAddress Uri's
        public const string AddAddressUri = "/api/provideraddress/AddAddress";
        public const string GetAddressUri = "/api/provideraddress/GetAddress/{0}";

        // Provider Statement Of Achievement Uri's
        public const string FindSoaLearnerRecordUri = "/api/statementofachievement/FindSoaLearnerRecord/{0}/{1}";
        public const string GetSoaLearnerRecordDetailsUri = "/api/statementofachievement/GetSoaLearnerRecordDetails/{0}/{1}";
        public const string CreateSoaPrintingRequestUri = "/api/statementofachievement/CreateSoaPrintingRequest";
        public const string GetPrintRequestSnapshotUri = "/api/statementofachievement/GetPrintRequestSnapshot/{0}/{1}/{2}";

        // Post Results Service Uri's
        public const string FindPrsLearnerRecordUri = "/api/postresultsservice/FindPrsLearnerRecord/{0}/{1}";
        public const string FindPrsLearnerRecordByProfileIdUri = "/api/postresultsservice/FindPrsLearnerRecordByProfileId/{0}/{1}";
        public const string PrsActivityUri = "/api/postresultsservice/PrsActivity";
        public const string PrsGradeChangeRequestUri = "/api/postresultsservice/PrsGradeChangeRequest";
        public const string ProcessBulkRommsUri = "/api/postresultsservice/ProcessBulkRomms";

        //LRS Api Uri's
        public const string LearnerServiceUri = "/LearnerService.svc";
        public const string PlrServiceUri = "/LearnerServiceR9.svc";

        // Printing Api Uri's
        public const string PrintingTokenUri = "/api/DFE/Token?username={0}&password={1}";
        public const string PrintRequestUri = "/api/DFE/PrintRequest?token={0}";
        public const string PrintBatchSummaryRequestUri = "/api/DFE/BatchSummary?batchNumber={0}&token={1}";
        public const string PrintTrackBatchRequestUri = "/api/DFE/TrackBatch?batchNumber={0}&token={1}";

        // Ucas Api Uri's        
        public const string FormDataHashType = "hashtype";
        public const string FormDataHash = "hash";
        public const string FormDataFile = "file";
        public const string SHA256 = "sha-256";
        public const string UcasTokenParameters = "grant_type={0}&username={1}&password={2}";
        public const string UcasBaseUri = "/api/v{0}";
        public const string UcasTokenUri = "/token";
        public const string UcasFileUri = "/folders/{0}/files";

        // Industry Placement 
        public const string GetIpLookupDataUri = "/api/industryplacement/GetIpLookupData/{0}/{1}";
        public const string ProcessIndustryPlacementDetailsUri = "/api/industryplacement/ProcessIndustryPlacementDetails";

        // Common Api Uri's
        public const string GetLookupDataUri = "/api/common/GetLookupData/{0}";
        public const string GetLoggedInUserTypeInfoUri = "/api/common/GetLoggedInUserTypeInfo/{0}";
        public const string GetCurrentAcademicYears = "/api/common/CurrentAcademicYears";
        public const string GetAcademicYears = "/api/common/AcademicYears";

        public const string GetLearnerRecordUri = "/api/learner/GetLearnerRecord/{0}/{1}/{2}";

        public const string GetDataExportUri = "/api/DataExport/GetDataExport/{0}/{1}/{2}";
        public const string DownloadOverallResultsDataUri = "/api/DataExport/DownloadOverallResultsData/{0}/{1}";
        public const string DownloadOverallResultSlipsDataUri = "/api/DataExport/DownloadOverallResultSlipsData/{0}/{1}";
        public const string DownloadLearnerOverallResultSlipsDataUri = "/api/DataExport/DownloadLearnerOverallResultSlipsData/{0}/{1}/{2}";
        public const string DownloadRommExportUri = "/api/DataExport/DownloadRommExport/{0}/{1}";

        // Industry Placement Upload
        public const string ProcessBulkIndustryPlacementsUri = "/api/industryplacement/ProcessBulkIndustryPlacements";

        // Admin dashboard
        public const string GetAdminSearchLearnerFiltersUri = "/api/admindashboard/GetAdminSearchLearnerFilters";
        public const string GetAdminSearchLearnerDetailsUri = "/api/admindashboard/GetAdminSearchLearnerDetails";
        public const string GetAdminLearnerRecordUri = "/api/admindashboard/GetAdminLearnerRecord/{0}";
        public const string GetAllowedChangeAcademicYearsUri = "/api/admindashboard/GetAllowedChangeAcademicYears/{0}/{1}";
        public const string ProcessChangeStartYearUri = "/api/admindashboard/ProcessChangeStartYear";
        public const string ProcessChangeIPUri = "/api/admindashboard/ProcessChangeIndustryPlacement";
        public const string ProcessAddCoreAssessmentUri = "/api/admindashboard/ProcessAddCoreAssessmentRequest";
        public const string ProcessAddSpecialismAssessmentUri = "/api/admindashboard/ProcessAddSpecialismAssessmentRequest";
        public const string ReviewRemoveCoreAssessmentEntryUri = "/api/admindashboard/ReviewRemoveCoreAssessmentEntry";
        public const string ReviewRemoveSpecialismAssessmentEntryUri = "/api/admindashboard/ReviewRemoveSpecialismAssessmentEntry";
        public const string ProcessAdminAddPathwayResultUri = "/api/admindashboard/ProcessAdminAddPathwayResult";
        public const string ProcessAdminAddSpecialismResultUri = "/api/admindashboard/ProcessAdminAddSpecialismResult";

        public const string ProcessAdminAddPathwayResult = "/api/admindashboard/ProcessAdminAddPathwayResult";
        public const string ProcessAdminAddSpecialismResult = "/api/admindashboard/ProcessAdminAddSpecialismResult";

        public const string ProcessAdminChangePathwayResult = "/api/admindashboard/ProcessAdminChangePathwayResult";
        public const string ProcessAdminChangeSpecialismResult = "/api/admindashboard/ProcessAdminChangeSpecialismResult";

        public const string ProcessAdminCreateReplacementDocumentPrintingRequestUri = "/api/admindashboard/ProcessAdminCreateReplacementDocumentPrintingRequest";

        // Admin change log
        public const string SearchChangeLogsUri = "/api/adminchangelog/SearchChangeLogs";
        public const string GetAdminChangeLogRecord = "/api/adminchangelog/GetAdminChangeLogRecord/{0}";

        // Admin post results
        public const string ProcessAdminOpenPathwayRomm = "/api/adminpostresults/ProcessAdminOpenPathwayRomm";
        public const string ProcessAdminOpenSpecialismRomm = "/api/adminpostresults/ProcessAdminOpenSpecialismRomm";

        public const string ProcessAdminReviewChangesRommOutcomeCore = "/api/adminpostresults/ProcessAdminReviewChangesRommOutcomeCore";
        public const string ProcessAdminReviewChangesRommOutcomeSpecialism = "/api/adminpostresults/ProcessAdminReviewChangesRommOutcomeSpecialism";

        public const string ProcessAdminOpenCoreAppeal = "/api/adminpostresults/ProcessAdminOpenCoreAppeal";
        public const string ProcessAdminOpenSpecialismAppeal = "/api/adminpostresults/ProcessAdminOpenSpecialismAppeal";

        public const string ProcessAdminReviewChangesAppealOutcomeCore = "/api/adminpostresults/ProcessAdminReviewChangesAppealOutcomeCore";
        public const string ProcessAdminReviewChangesAppealOutcomeSpecialism = "/api/adminpostresults/ProcessAdminReviewChangesAppealOutcomeSpecialism";

        // Search registration
        public const string GetSearchRegistrationFiltersUri = "/api/searchregistration/GetSearchRegistrationFilters";
        public const string SearchRegistrationDetailsUri = "/api/searchregistration/SearchRegistrationDetails";

        // Download registrations
        public const string GetAvailableStartYearsUri = "/api/providerregistrations/GetAvailableStartYears";
        public const string GetRegistrationsUri = "/api/providerregistrations/GetRegistrations";

        // Admin providers
        public const string GetProvider = "/api/adminprovider/GetProvider/{0}";
        public const string AddProvider = "/api/adminprovider/AddProvider";
        public const string UpdateProvider = "/api/adminprovider/UpdateProvider";

        //Dashboard
        public const string GetAwardingOrganisationBanners = "/api/dashboardbanner/GetAwardingOrganisationBanners";
        public const string GetProviderBanners = "/api/dashboardbanner/GetProviderBanners";

        //Admin notifications
        public const string SearchNotifications = "/api/adminnotification/SearchNotifications";
        public const string GetNotification = "/api/adminnotification/GetNotification/{0}";
        public const string AddNotification = "/api/adminnotification/AddNotification";
        public const string UpdateNotification = "/api/adminnotification/UpdateNotification";

        //Awarding organisations
        public const string GetAllAwardingOrganisations = "/api/awardingorganisation/GetAllAwardingOrganisations";
        public const string GetAwardingOrganisationByUkprn = "/api/awardingorganisation/GetAwardingOrganisationByUkprn/{0}";
    }
}