using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessIndustryPlacementQuestionUpdate
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = null;
            ViewModel = new UpdateIndustryPlacementQuestionViewModel { ProfileId = 0, RegistrationPathwayId = 1, IndustryPlacementStatus = IndustryPlacementStatus.Completed  };
            InternalApiClient.GetLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
