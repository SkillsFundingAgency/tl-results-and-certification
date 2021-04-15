using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferencePost
{
    public class When_LearnerRecord_IsAddedAlready : TestSetup
    {
        private readonly long _uln = 123456789;
        private FindLearnerRecord _mockResult;
        private int _profileId;

        public override void Given()
        {
            _profileId = 1;
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = _uln.ToString() };
            _mockResult = new FindLearnerRecord { ProfileId = _profileId, IsLearnerRegistered = true, IsLearnerRecordAdded = true };
            TrainingProviderLoader.FindLearnerRecordAsync(ProviderUkprn, _uln).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).FindLearnerRecordAsync(ProviderUkprn, _uln);

            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<AddLearnerRecordViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_EnterUniqueLearnerNumberAddedAlready()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumberAddedAlready);
            route.RouteValues[Constants.ProfileId].Should().Be(_profileId);
        }
    }
}
