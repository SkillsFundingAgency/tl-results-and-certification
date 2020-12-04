using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.RemoveAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private bool ExpectedApiResult { get; set; }

        public override void Given()
        {
            ViewModel = new AssessmentEntryDetailsViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 1,
                AssessmentEntryType = Common.Enum.AssessmentEntryType.Core,
            };            

            ExpectedApiResult = true;
            InternalApiClient
                .RemoveAssessmentEntryAsync(Arg.Is<RemoveAssessmentEntryRequest>(
                    x => x.AssessmentId == ViewModel.AssessmentId &&
                    x.AoUkprn == AoUkprn &&
                    x.AssessmentEntryType == Common.Enum.AssessmentEntryType.Core))
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
