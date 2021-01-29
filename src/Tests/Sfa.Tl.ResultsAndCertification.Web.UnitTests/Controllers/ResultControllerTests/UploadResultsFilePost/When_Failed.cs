using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.UploadResultsFilePost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns("results.csv");
            ViewModel.File = FormFile;
            BlobUniqueReference = Guid.Empty;

            ResponseViewModel = new UploadResultsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            ResultLoader.ProcessBulkResultsAsync(ViewModel).Returns(ResponseViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ResultLoader.Received(1).ProcessBulkResultsAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithResultsUpload()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithResultsUpload);
        }
    }
}
