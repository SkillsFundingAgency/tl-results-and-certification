﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessIndustryPlacementQuestionUpdate
{
    public class When_Success : TestSetup
    {
        private LearnerRecordDetails _expectedApiResult;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;
            ProfileId = 1;
            RegistrationPathwayId = 1;

            _expectedApiResult = new LearnerRecordDetails { ProfileId = ProfileId, Uln = 1234567890, Name = "Test User", IsLearnerRecordAdded = true, IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            ViewModel = new UpdateIndustryPlacementQuestionViewModel { ProfileId = ProfileId, RegistrationPathwayId = RegistrationPathwayId, IndustryPlacementId = 10, IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted };
            
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId, RegistrationPathwayId).Returns(_expectedApiResult);
            InternalApiClient.UpdateLearnerRecordAsync(Arg.Is<UpdateLearnerRecordRequest>
                (x => x.ProfileId == ViewModel.ProfileId &&
                x.Ukprn == ProviderUkprn &&
                x.RegistrationPathwayId == ViewModel.RegistrationPathwayId &&
                x.IndustryPlacementId == ViewModel.IndustryPlacementId &&
                x.IndustryPlacementStatus == ViewModel.IndustryPlacementStatus &&
                x.PerformedBy == $"{Givenname} {Surname}"))
                .Returns(true);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId, RegistrationPathwayId);
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
            ActualResult.IsSuccess.Should().BeTrue();

            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Name.Should().Be(_expectedApiResult.Name);
        }
    }
}
