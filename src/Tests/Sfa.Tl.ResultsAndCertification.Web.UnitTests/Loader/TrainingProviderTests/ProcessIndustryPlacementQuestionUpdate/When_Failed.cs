using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessIndustryPlacementQuestionUpdate
{
    public class When_Failed : TestSetup
    {
        private LearnerRecordDetails _expectedApiResult;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            _expectedApiResult = new LearnerRecordDetails { ProfileId = 1, Uln = 1234567890, Name = "Test User", IsLearnerRecordAdded = true, IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            ViewModel = new UpdateIndustryPlacementQuestionViewModel { ProfileId = 1, RegistrationPathwayId = 1, IndustryPlacementId = 0, IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted };

            InternalApiClient.GetLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(_expectedApiResult);
            InternalApiClient.UpdateLearnerRecordAsync(Arg.Is<UpdateLearnerRecordRequest>
                (x => x.ProfileId == ViewModel.ProfileId &&
                x.Ukprn == ProviderUkprn &&
                x.RegistrationPathwayId == ViewModel.RegistrationPathwayId &&
                x.IndustryPlacementId == ViewModel.IndustryPlacementId &&
                x.IndustryPlacementStatus == ViewModel.IndustryPlacementStatus &&
                x.PerformedBy == $"{Givenname} {Surname}"))
                .Returns(false);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>());
            InternalApiClient.Received(1).UpdateLearnerRecordAsync(Arg.Is<UpdateLearnerRecordRequest>
                (x => x.ProfileId == ViewModel.ProfileId &&
                x.Ukprn == ProviderUkprn &&
                x.RegistrationPathwayId == ViewModel.RegistrationPathwayId &&
                x.IndustryPlacementId == ViewModel.IndustryPlacementId &&
                x.IndustryPlacementStatus == ViewModel.IndustryPlacementStatus &&
                x.PerformedBy == $"{Givenname} {Surname}"));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsModified.Should().BeTrue();
            ActualResult.IsSuccess.Should().BeFalse();

            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Name.Should().Be(_expectedApiResult.Name);
        }
    }
}
