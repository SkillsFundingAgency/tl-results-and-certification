using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessIndustryPlacementQuestionUpdate
{
    public class When_Called_With_IsLearnerRecordAdded_False : TestSetup
    {
        private LearnerRecordDetails _expectedApiResult;

        public override void Given()
        {
            ProviderUkprn = 87945612;
            ProfileId = 1;
            RegistrationPathwayId = 1;

            _expectedApiResult = new LearnerRecordDetails { ProfileId = ProfileId, IsLearnerRecordAdded = false };
            ViewModel = new UpdateIndustryPlacementQuestionViewModel { ProfileId = ProfileId, RegistrationPathwayId = RegistrationPathwayId, IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId, RegistrationPathwayId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId, RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
