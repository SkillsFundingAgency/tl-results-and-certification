using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadIndustryPlacementsFilePost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns("IndustryPlacement.csv");
            ViewModel.File = FormFile;

            ResponseViewModel = new UploadIndustryPlacementsResponseViewModel
            {
                IsSuccess = true
            };

            IndustryPlacementLoader.ProcessBulkIndustryPlacementsAsync(ViewModel).Returns(ResponseViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            IndustryPlacementLoader.Received(1).ProcessBulkIndustryPlacementsAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_IndustryPlacementsUploadSuccessful()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.IndustryPlacementsUploadSuccessful);
        }
    }
}
