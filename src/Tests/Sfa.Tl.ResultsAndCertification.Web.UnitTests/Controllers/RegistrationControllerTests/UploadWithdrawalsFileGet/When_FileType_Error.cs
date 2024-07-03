using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Xunit;
using UploadContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.Upload;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadWithdrawlsFileGet
{
    public class When_FileType_Error : TestSetup
    {
        public override void Given()
        {
            RequestErrorTypeId = (int)RequestErrorType.FileType;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UploadWithdrawalsRequestViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(UploadWithdrawalsRequestViewModel.File)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(UploadWithdrawalsRequestViewModel.File)];
            modelState.Errors[0].ErrorMessage.Should().Be(UploadContent.Must_Be_Csv_Validation_Message);
        }
    }
}
