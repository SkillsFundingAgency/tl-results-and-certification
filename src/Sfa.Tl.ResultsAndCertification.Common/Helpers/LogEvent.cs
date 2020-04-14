namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public class LogEvent
    {
        // Generic events
        public const int UnhandledException = 1001;
        public const int NoDataFound = 1002;
        public const int ConfirmationPageFailed = 1003;

        // Tlevel events
        public const int TlevelNotFound = 2001;

        // provider events
        public const int ProviersNotFound = 3001;
        public const int ProviderTlevelNotAdded = 3002;
        public const int ProviderTlevelNotRemoved = 3003;
        public const int ProviderTlevelNotFound = 3004;
    }
}
