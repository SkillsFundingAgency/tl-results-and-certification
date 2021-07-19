namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public class LogEvent
    {
        // Generic events
        public const int UnhandledException = 1001;
        public const int NoDataFound = 1002;
        public const int ConfigurationMissing = 1003;
        public const int ServiceAccessDenied = 1004;
        public const int FileStreamNotFound = 1005;
        public const int RecordExists = 1006;
        public const int StateChanged = 1007;

        public const int ConfirmationPageFailed = 1020;
        public const int UploadSuccessfulPageFailed = 1021;
        public const int UploadUnsuccessfulPageFailed = 1022;
        public const int DownloadRegistrationErrorsFailed = 1023;
        public const int EmailTemplateNotFound = 1030;
        public const int EmailSendFailed = 1031;

        // Tlevel events
        public const int TlevelsNotFound = 2001;
        public const int TlevelsNotConfirmed = 2002;
        public const int TlevelReportIssueFailed = 2003;

        // provider events
        public const int ProviersNotFound = 3001;
        public const int ProviderTlevelNotAdded = 3002;
        public const int ProviderTlevelNotRemoved = 3003;
        public const int ProviderTlevelNotFound = 3004;

        // Registration events
        public const int BulkRegistrationProcessFailed = 4001;
        public const int ManualRegistrationProcessFailed = 4002;
        public const int RegistrationNotDeleted = 4003;
        public const int ManualReregistrationProcessFailed = 4004;

        // Assessment events
        public const int BulkAssessmentProcessFailed = 5001;
        public const int DownloadAssesssmentErrorsFailed = 5002;
        public const int AddCoreAssessmentEntryFailed = 5501;

        // Results events
        public const int BulkResultProcessFailed = 6001;
        public const int DownloadResultErrorsFailed = 6002;

        // Training Provider events
        public const int AddLearnerRecordFailed = 7001;
        public const int AddEnglishAndMathsSendDataEmailFailed = 7002;
        public const int UnSupportedMethod = 7003;

        // Ordnance Survey events
        public const int UnableToGetAddressFromOrdnanceSurvey = 8001;

        // Provider Address events
        public const int AddAddressFailed = 9001;
    }
}
