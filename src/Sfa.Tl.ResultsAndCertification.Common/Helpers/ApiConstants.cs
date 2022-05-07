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
        public const string AddRegistrationUri = "/api/registration/AddRegistration";
        public const string FindUlnUri = "/api/registration/FindUln/{0}/{1}";
        public const string GetRegistrationDetailsUri = "/api/registration/GetRegistrationDetails/{0}/{1}/{2}";
        public const string DeleteRegistrationUri = "/api/registration/DeleteRegistration/{0}/{1}";
        public const string UpdateRegistrationUri = "/api/registration/UpdateRegistration";
        public const string WithdrawRegistrationUri = "/api/registration/WithdrawRegistration";
        public const string RejoinRegistrationUri = "/api/registration/RejoinRegistration";
        public const string ReregistrationUri = "/api/registration/Reregistration";

        // Assessments Related Uri's
        public const string ProcessBulkAssessmentsUri = "/api/assessment/ProcessBulkAssessments";
        public const string GetAvailableAssessmentSeriesUri = "/api/assessment/GetAvailableAssessmentSeries/{0}/{1}/{2}/{3}";
        public const string AddAssessmentEntryUri = "/api/assessment/AddAssessmentEntry";
        public const string GetActiveAssessmentEntryDetailsUri = "/api/assessment/GetActiveAssessmentEntryDetails/{0}/{1}/{2}";
        public const string GetActiveSpecialismAssessmentEntriesUri = "/api/assessment/GetActiveSpecialismAssessmentEntries/{0}/{1}";
        public const string RemoveAssessmentEntryUri = "/api/assessment/RemoveAssessmentEntry";
        public const string GetAssessmentSeriesDetailsUri = "/api/assessment/GetAssessmentSeries";

        // Results Related Uri's
        public const string ProcessBulkResultsUri = "/api/result/ProcessBulkResults";
        public const string GetResultDetailsUri = "/api/result/GetResultDetails/{0}/{1}/{2}";
        public const string AddResultUri = "/api/result/AddResult";
        public const string ChangeResultUri = "/api/result/ChangeResult";
        
        // DocumentUploadHistory Related Uri's
        public const string GetDocumentUploadHistoryDetailsAsyncUri = "/api/DocumentUploadHistory/GetDocumentUploadHistoryDetails/{0}/{1}";

        // TrainingProvider
        public const string FindLearnerRecordUri = "/api/trainingprovider/FindLearnerRecord/{0}/{1}/{2}";
        public const string GetLearnerRecordDetailsUri = "/api/trainingprovider/GetLearnerRecordDetails/{0}/{1}/{2}";
        public const string AddLearnerRecordUri = "/api/trainingprovider/AddLearnerRecord";
        public const string UpdateLearnerRecordUri = "/api/trainingprovider/UpdateLearnerRecord";
        public const string UpdateLearnerSubjectUri = "/api/trainingprovider/UpdateLearnerSubject";

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
        public const string GetTempFlexNavigationUri = "/api/industryplacement/GetTempFlexNavigation/{0}/{1}";
        public const string ProcessIndustryPlacementDetailsAsync = "/api/industryplacement/ProcessIndustryPlacementDetails";

        // Common Api Uri's
        public const string GetLookupDataUri = "/api/common/GetLookupData/{0}";
        public const string GetLoggedInUserTypeInfoUri = "/api/common/GetLoggedInUserTypeInfo/{0}";
        public const string GetCurrentAcademicYears = "/api/common/CurrentAcademicYears";
        public const string GetAcademicYears = "/api/common/AcademicYears";

        public const string GetLearnerRecordUri = "/api/learner/GetLearnerRecord/{0}/{1}/{2}";

        public const string GetDataExportUri = "/api/DataExport/GetDataExport/{0}/{1}/{2}";
    }
}