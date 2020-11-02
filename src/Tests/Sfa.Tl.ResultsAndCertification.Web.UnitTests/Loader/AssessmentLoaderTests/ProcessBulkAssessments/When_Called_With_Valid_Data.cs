using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.ProcessBulkAssessments
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            BulkAssessmentResponse = new BulkAssessmentResponse
            {
                IsSuccess = true,
                Stats = new BulkUploadStats
                {
                    TotalRecordsCount = 10
                }
            };

            UploadAssessmentsRequestViewModel = new UploadAssessmentsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadAssessmentsResponseViewModel = new UploadAssessmentsResponseViewModel
            {
                IsSuccess = true,
                Stats = new BulkUploadStatsViewModel
                {
                    TotalRecordsCount = 10
                }
            };

            Mapper.Map<BulkAssessmentRequest>(UploadAssessmentsRequestViewModel).Returns(BulkAssessmentRequest);
            Mapper.Map<UploadAssessmentsResponseViewModel>(BulkAssessmentResponse).Returns(UploadAssessmentsResponseViewModel);
            InternalApiClient.ProcessBulkAssessmentsAsync(BulkAssessmentRequest).Returns(BulkAssessmentResponse);
            Loader = new AssessmentLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).ProcessBulkAssessmentsAsync(BulkAssessmentRequest);
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
            Mapper.Received().Map<BulkAssessmentRequest>(UploadAssessmentsRequestViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(UploadAssessmentsResponseViewModel.IsSuccess);
            ActualResult.Stats.Should().NotBeNull();

            ActualResult.Stats.TotalRecordsCount.Should().Be(UploadAssessmentsResponseViewModel.Stats.TotalRecordsCount);            
        }
    }
}
