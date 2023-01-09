using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadIndustryPlacementsFilePost
{
    public class When_Upload_Unsuccessful : TestSetup
    {
        private UploadIndustryPlacementsResponseViewModel _responseViewModel;

        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns("test.csv");
            ViewModel.File = FormFile;
            BlobUniqueReference = Guid.NewGuid();

            _responseViewModel = new UploadIndustryPlacementsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            IndustryPlacementLoader.ProcessBulkIndustryPlacementsAsync(ViewModel).Returns(_responseViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            IndustryPlacementLoader.Received(1).ProcessBulkIndustryPlacementsAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_IndustryPlacementUploadUnsuccessfulAsync()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.IndustryPlacementsUploadUnsuccessful);
        }
    }
}
