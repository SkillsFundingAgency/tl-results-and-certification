using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.PostResultsServiceControllerTests.UploadWithdrawlsFilePost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns("test.csv");
            ViewModel.File = FormFile;
            BlobUniqueReference = Guid.NewGuid();

            ResponseViewModel = new UploadRommsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            PostResultsServiceLoader.ProcessBulkRommsAsync(ViewModel).Returns(ResponseViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            PostResultsServiceLoader.Received(1).ProcessBulkRommsAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_RommUploadUnsuccessful()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RommsUploadUnsuccessful);
        }
    }
}
