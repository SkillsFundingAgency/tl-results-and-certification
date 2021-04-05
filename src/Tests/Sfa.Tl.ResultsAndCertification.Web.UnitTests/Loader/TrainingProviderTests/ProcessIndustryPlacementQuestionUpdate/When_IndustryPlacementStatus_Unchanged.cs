using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessIndustryPlacementQuestionUpdate
{
    public class When_IndustryPlacementStatus_Unchanged : TestSetup
    {
        private LearnerRecordDetails _expectedApiResult;

        public override void Given()
        {
            ProviderUkprn = 987654321;
            ProfileId = 1;
            RegistrationPathwayId = 1;

            _expectedApiResult = new LearnerRecordDetails { ProfileId = ProfileId, IsLearnerRecordAdded = true, IndustryPlacementStatus = IndustryPlacementStatus.Completed  };
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
            ActualResult.Should().NotBeNull();
            ActualResult.IsModified.Should().BeFalse();
        }
    }
}
