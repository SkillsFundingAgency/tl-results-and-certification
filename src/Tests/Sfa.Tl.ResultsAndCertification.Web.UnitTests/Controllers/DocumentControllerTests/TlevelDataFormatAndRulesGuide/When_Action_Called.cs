using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Document;
using Xunit;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document.TlevelDataFormatAndRulesGuide;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.TlevelDataFormatAndRulesGuide
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Expected_ViewModel_Results_Are_Returned()
        {
            var viewResultModel = Result.ShouldBeViewResult<TlevelDataFormatAndRulesGuideViewModel>();

            viewResultModel.Registrations.Should().NotBeNull();
            viewResultModel.Registrations.Heading.Should().Be(DocumentResource.Registrations_Heading_Text);
            viewResultModel.Registrations.DownloadLink.Should().Be(RouteConstants.DownloadRegistrationDataFormatAndRulesGuide);
            viewResultModel.Registrations.DownloadLinkText.Should().Be(string.Format(DocumentResource.Registration_DataFormat_And_Rules, DocumentResource.XlsxFileType, DocumentResource.Registrations_FileSize_Text));
            viewResultModel.Registrations.PublishedOn.Should().Be($"{DocumentResource.Published_Text} {DocumentResource.Registrations_PublishedDate_Text}");

            viewResultModel.AssessmentEntries.Should().NotBeNull();
            viewResultModel.AssessmentEntries.Heading.Should().Be(DocumentResource.Assessment_Entries_Heading_Text);
            viewResultModel.AssessmentEntries.DownloadLink.Should().Be(RouteConstants.DownloadAssessmentEntriesDataFormatAndRulesGuide);
            viewResultModel.AssessmentEntries.DownloadLinkText.Should().Be(string.Format(DocumentResource.Assessment_Entries_DataFormat_And_Rules, DocumentResource.XlsxFileType, DocumentResource.Assessment_Entries_FileSize_Text));
            viewResultModel.AssessmentEntries.PublishedOn.Should().Be($"{DocumentResource.Published_Text} {DocumentResource.Assessment_Entries_PublishedDate_Text}"); 
            
            viewResultModel.Withdrawals.Should().NotBeNull();
            viewResultModel.Withdrawals.Heading.Should().Be(DocumentResource.Withdrawals_Heading_Text);
            viewResultModel.Withdrawals.DownloadLink.Should().Be(RouteConstants.DownloadWithdrawalsDataFormatAndRulesGuide);
            viewResultModel.Withdrawals.DownloadLinkText.Should().Be(string.Format(DocumentResource.Withdrawals_DataFormat_And_Rules, DocumentResource.XlsxFileType, DocumentResource.Widthdrawals_FileSize_Text));
            viewResultModel.Withdrawals.PublishedOn.Should().Be($"{DocumentResource.Published_Text} {DocumentResource.Withdrawals_PublishedDate_Text}");

            viewResultModel.Results.Should().NotBeNull();
            viewResultModel.Results.Heading.Should().Be(DocumentResource.Results_Heading_Text);
            viewResultModel.Results.DownloadLink.Should().Be(RouteConstants.DownloadResultsDataFormatAndRulesGuide);
            viewResultModel.Results.DownloadLinkText.Should().Be(string.Format(DocumentResource.Results_DataFormat_And_Rules, DocumentResource.XlsxFileType, DocumentResource.Results_FileSize_Text));
            viewResultModel.Results.PublishedOn.Should().Be($"{DocumentResource.Published_Text} {DocumentResource.Results_PublishedDate_Text}");

            viewResultModel.Romms.Should().NotBeNull();
            viewResultModel.Romms.Heading.Should().Be(DocumentResource.Romms_Heading_Text);
            viewResultModel.Romms.DownloadLink.Should().Be(RouteConstants.DownloadRommDataFormatAndRulesGuide);
            viewResultModel.Romms.DownloadLinkText.Should().Be(string.Format(DocumentResource.Romms_DataFormat_And_Rules, DocumentResource.XlsxFileType, DocumentResource.Romms_FileSize_Text));
            viewResultModel.Romms.PublishedOn.Should().Be($"{DocumentResource.Published_Text} {DocumentResource.Romms_PublishedDate_Text}");
        }
    }
}
