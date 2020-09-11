using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Document;
using Xunit;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.RegistrationDataFormatAndRulesGuide
{
    public class When_Action_Called : TestSetup
    {
        private RegistrationDataFormatAndRulesGuideViewModel _viewModel;
        public override void Given()
        {
            _viewModel = new RegistrationDataFormatAndRulesGuideViewModel
            {
                FileType = FileType.Xlsx.ToString().ToUpperInvariant(),
                FileSize = DocumentResource.RegistrationDataFormatAndRulesGuide.FileSize_Text,
                Version = DocumentResource.RegistrationDataFormatAndRulesGuide.Version_Text,
                PublishedDate = $"{DocumentResource.RegistrationDataFormatAndRulesGuide.Published_Text} {DocumentResource.RegistrationDataFormatAndRulesGuide.PublishedDate_Text}"
            };
        }

        [Fact]
        public void Then_Expected_ViewModel_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RegistrationDataFormatAndRulesGuideViewModel));

            var viewResultModel = viewResult.Model as RegistrationDataFormatAndRulesGuideViewModel;
            viewResultModel.FileType.Should().Be(_viewModel.FileType);
            viewResultModel.FileSize.Should().Be(_viewModel.FileSize);
            viewResultModel.Version.Should().Be(_viewModel.Version);
            viewResultModel.PublishedDate.Should().Be(_viewModel.PublishedDate);
        }
    }
}
