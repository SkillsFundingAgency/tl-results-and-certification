using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadRegistrationsFilePost
{
    public class Then_On_Failed_Redirect_To_RegistrationsUploadUnSuccessful_Route : When_UploadRegistrationsFile_Post_Action_Is_Called
    {
        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns("test.csv");
            ViewModel.File = FormFile;

            ResponseViewModel = new UploadRegistrationsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            RegistrationLoader.ProcessBulkRegistrationsAsync(ViewModel).Returns(ResponseViewModel);
        }

        [Fact]
        public void Then_If_Failed_Redirected_To_RegistrationsUploadUnsuccessful()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RegistrationsUploadUnsuccessful);
        }
    }
}
