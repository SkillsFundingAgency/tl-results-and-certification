using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using UploadContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.UploadRomms;

namespace Sfa.Tl.ResultsAndCertification.Web.PostResultsServiceControllerTests.UploadWithdrawlsFilePost
{
    public class When_ModelState_FileSize_Invalid : TestSetup
    {
        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            Controller.ModelState.AddModelError("File", string.Format(UploadContent.File_Size_Too_Large_Validation_Message, 5));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UploadRommsRequestViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(UploadRommsRequestViewModel.File)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(UploadRommsRequestViewModel.File)];
            modelState.Errors[0].ErrorMessage.Should().Be(string.Format(UploadContent.File_Size_Too_Large_Validation_Message, 5));
        }
    }
}
