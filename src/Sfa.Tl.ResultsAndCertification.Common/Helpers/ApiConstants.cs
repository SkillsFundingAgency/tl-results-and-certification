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
        public const string GetAssessmentDetailsUri = "/api/assessment/GetAssessmentDetails/{0}/{1}/{2}";
        public const string GetAvailableAssessmentSeriesUri = "/api/assessment/GetAvailableAssessmentSeries/{0}/{1}/{2}";
        public const string AddAssessmentEntryUri = "/api/assessment/AddAssessmentEntry";
        public const string GetActiveAssessmentEntryDetailsUri = "/api/assessment/GetActiveAssessmentEntryDetails/{0}/{1}/{2}";
        public const string RemoveAssessmentEntryUri = "/api/assessment/RemoveAssessmentEntry";

        // Results Related Uri's
        public const string ProcessBulkResultsUri = "/api/result/ProcessBulkResults";
        public const string GetResultDetailsUri = "/api/result/GetResultDetails/{0}/{1}/{2}";
        public const string AddResultUri = "/api/result/AddResult";
        public const string ChangeResultUri = "/api/result/ChangeResult";
        public const string GetLookupDataUri = "/api/common/GetLookupData/{0}";

        // LoggedIn UserType Related Uri's
        public const string GetLoggedInUserTypeInfoUri = "/api/common/GetLoggedInUserTypeInfo/{0}";
        
        // DocumentUploadHistory Related Uri's
        public const string GetDocumentUploadHistoryDetailsAsyncUri = "/api/DocumentUploadHistory/GetDocumentUploadHistoryDetails/{0}/{1}";

        // TrainingProvider
        public const string FindLearnerRecordUri = "/api/trainingprovider/FindLearnerRecord/{0}/{1}/{2}";
        public const string GetLearnerRecordDetailsUri = "/api/trainingprovider/GetLearnerRecordDetails/{0}/{1}/{2}";
        public const string AddLearnerRecordUri = "/api/trainingprovider/AddLearnerRecord";
        public const string UpdateLearnerRecordUri = "/api/trainingprovider/UpdateLearnerRecord";

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
        public const string FindPrsLearnerRecordUri = "/api/statementofachievement/FindPrsLearnerRecord/{0}/{1}";

        //LRS Api Uri's
        public const string LearnerServiceUri = "/LearnerService.svc";
        public const string PlrServiceUri = "/LearnerServiceR9.svc";
    }
}