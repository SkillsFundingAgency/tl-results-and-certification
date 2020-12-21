using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Document;
using Xunit;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.TlevelDataFormatAndRulesGuide
{
    public class When_Action_Called : TestSetup
    {
        private TlevelDataFormatAndRulesGuideViewModel _viewModel;

        public override void Given()
        {
            _viewModel = new TlevelDataFormatAndRulesGuideViewModel
            {
                FileType = FileType.Xlsx.ToString().ToUpperInvariant(),
                RegistrationsFileSize = DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_FileSize_Text,
                RegistrationsVersion = DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_Version_Text,
                RegistrationsPublishedDate = $"{DocumentResource.TlevelDataFormatAndRulesGuide.Published_Text} {DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_PublishedDate_Text}",
                AssessmentEntriesFileSize = DocumentResource.TlevelDataFormatAndRulesGuide.Assessment_Entries_FileSize_Text,
                AssessmentEntriesVersion = DocumentResource.TlevelDataFormatAndRulesGuide.Assessment_Entries_Version_Text,
                AssessmentEntriesPublishedDate = $"{DocumentResource.TlevelDataFormatAndRulesGuide.Published_Text} {DocumentResource.TlevelDataFormatAndRulesGuide.Assessment_Entries_PublishedDate_Text}"
            };
        }

        [Fact]
        public void Then_Expected_ViewModel_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(TlevelDataFormatAndRulesGuideViewModel));

            var viewResultModel = viewResult.Model as TlevelDataFormatAndRulesGuideViewModel;
            viewResultModel.FileType.Should().Be(_viewModel.FileType);
            viewResultModel.RegistrationsFileSize.Should().Be(_viewModel.RegistrationsFileSize);
            viewResultModel.RegistrationsVersion.Should().Be(_viewModel.RegistrationsVersion);
            viewResultModel.RegistrationsPublishedDate.Should().Be(_viewModel.RegistrationsPublishedDate);
            viewResultModel.AssessmentEntriesFileSize.Should().Be(_viewModel.AssessmentEntriesFileSize);
            viewResultModel.AssessmentEntriesVersion.Should().Be(_viewModel.AssessmentEntriesVersion);
            viewResultModel.AssessmentEntriesPublishedDate.Should().Be(_viewModel.AssessmentEntriesPublishedDate);
        }
    }
}
