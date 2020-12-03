using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.AddAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddAssessmentEntryResponse ExpectedResult { get; set; }
        private AddAssessmentEntryResponse ExpectedApiResult { get; set; }

        public override void Given()
        {
            ViewModel = new AddAssessmentEntryViewModel
            {
                ProfileId = ProfileId,
                AssessmentSeriesId = 1,
                AssessmentEntryType = Common.Enum.AssessmentEntryType.Core,
            };

            ExpectedApiResult = new AddAssessmentEntryResponse { IsSuccess = true, UniqueLearnerNumber = 1234567890 }; 
            InternalApiClient
                .AddAssessmentEntryAsync(Arg.Is<AddAssessmentEntryRequest>(
                    x => x.ProfileId == ViewModel.ProfileId && 
                    x.AoUkprn == AoUkprn && 
                    x.AssessmentEntryType == Common.Enum.AssessmentEntryType.Core &&
                    x.AssessmentSeriesId == ViewModel.AssessmentSeriesId))
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().Be(ExpectedApiResult.IsSuccess);
            ActualResult.UniqueLearnerNumber.Should().Be(ExpectedApiResult.UniqueLearnerNumber);
        }
    }
}
