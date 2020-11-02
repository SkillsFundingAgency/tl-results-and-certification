using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.UploadAssessmentsFilePost
{
    public class When_Upload_Unsuccessful : TestSetup
    {
        private UploadAssessmentsResponseViewModel _responseViewModel;

        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns("test.csv");
            ViewModel.File = FormFile;
            BlobUniqueReference = Guid.NewGuid();

            _responseViewModel = new UploadAssessmentsResponseViewModel
            {
                IsSuccess = false,
                BlobUniqueReference = BlobUniqueReference,
                ErrorFileSize = 1.5
            };

            AssessmentLoader.ProcessBulkAssessmentsAsync(ViewModel).Returns(_responseViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).ProcessBulkAssessmentsAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_AssessmentUploadUnsuccessfulAsync()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AssessmentsUploadUnsuccessful);
        }
    }
}
