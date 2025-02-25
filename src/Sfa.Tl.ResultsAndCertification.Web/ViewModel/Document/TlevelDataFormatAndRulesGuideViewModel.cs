using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Document
{
    public class TlevelDataFormatAndRulesGuideViewModel
    {
        public FormatAndRulesGuideModel Registrations = new()
        {
            Heading = TlevelDataFormatAndRulesGuide.Registrations_Heading_Text,
            DownloadLink = RouteConstants.DownloadRegistrationDataFormatAndRulesGuide,
            DownloadLinkText = string.Format(TlevelDataFormatAndRulesGuide.Registration_DataFormat_And_Rules, TlevelDataFormatAndRulesGuide.XlsxFileType, TlevelDataFormatAndRulesGuide.Registrations_FileSize_Text),
            PublishedOn = $"{TlevelDataFormatAndRulesGuide.Published_Text} {TlevelDataFormatAndRulesGuide.Registrations_PublishedDate_Text}"
        };

        public FormatAndRulesGuideModel Withdrawals = new()
        {
            Heading = TlevelDataFormatAndRulesGuide.Withdrawals_Heading_Text,
            DownloadLink = RouteConstants.DownloadWithdrawalsDataFormatAndRulesGuide,
            DownloadLinkText = string.Format(TlevelDataFormatAndRulesGuide.Withdrawals_DataFormat_And_Rules, TlevelDataFormatAndRulesGuide.XlsxFileType, TlevelDataFormatAndRulesGuide.Widthdrawals_FileSize_Text),
            PublishedOn = $"{TlevelDataFormatAndRulesGuide.Published_Text} {TlevelDataFormatAndRulesGuide.Withdrawals_PublishedDate_Text}"
        };

        public FormatAndRulesGuideModel AssessmentEntries = new()
        {
            Heading = TlevelDataFormatAndRulesGuide.Assessment_Entries_Heading_Text,
            DownloadLink = RouteConstants.DownloadAssessmentEntriesDataFormatAndRulesGuide,
            DownloadLinkText = string.Format(TlevelDataFormatAndRulesGuide.Assessment_Entries_DataFormat_And_Rules, TlevelDataFormatAndRulesGuide.XlsxFileType, TlevelDataFormatAndRulesGuide.Assessment_Entries_FileSize_Text),
            PublishedOn = $"{TlevelDataFormatAndRulesGuide.Published_Text} {TlevelDataFormatAndRulesGuide.Assessment_Entries_PublishedDate_Text}"
        };

        public FormatAndRulesGuideModel Results = new()
        {
            Heading = TlevelDataFormatAndRulesGuide.Results_Heading_Text,
            DownloadLink = RouteConstants.DownloadResultsDataFormatAndRulesGuide,
            DownloadLinkText = string.Format(TlevelDataFormatAndRulesGuide.Results_DataFormat_And_Rules, TlevelDataFormatAndRulesGuide.XlsxFileType, TlevelDataFormatAndRulesGuide.Results_FileSize_Text),
            PublishedOn = $"{TlevelDataFormatAndRulesGuide.Published_Text} {TlevelDataFormatAndRulesGuide.Results_PublishedDate_Text}"
        };

        public FormatAndRulesGuideModel Romms = new()
        {
            Heading = TlevelDataFormatAndRulesGuide.Romms_Heading_Text,
            DownloadLink = RouteConstants.DownloadRommDataFormatAndRulesGuide,
            DownloadLinkText = string.Format(TlevelDataFormatAndRulesGuide.Romms_DataFormat_And_Rules, TlevelDataFormatAndRulesGuide.XlsxFileType, TlevelDataFormatAndRulesGuide.Romms_FileSize_Text),
            PublishedOn = $"{TlevelDataFormatAndRulesGuide.Published_Text} {TlevelDataFormatAndRulesGuide.Romms_PublishedDate_Text}"
        };
    }
}