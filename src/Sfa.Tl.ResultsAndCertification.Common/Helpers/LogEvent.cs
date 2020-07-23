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
    }
}
