using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.ProcessBulkAssessments
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Expected_UploadAssessmentsResponse_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().Be(UploadAssessmentsResponseViewModel.IsSuccess);
            ActualResult.ErrorFileSize.Should().Be(UploadAssessmentsResponseViewModel.ErrorFileSize);
            ActualResult.BlobUniqueReference.Should().Be(UploadAssessmentsResponseViewModel.BlobUniqueReference);
        }
    }
}